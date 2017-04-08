using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;               //Enables the use of sleep(), used to give proper timing to background workers
using System.Xml;                     //Enables the app to store and retrieve user settings in an XML file
using System.Timers;                  //Used for recurring functions that requiere a certain timing
using System.IO.Ports;                //For communicaation with the arduino through an serial port
using OpenHardwareMonitor.Hardware;

namespace DIY_LEDS_V1
{    
    public partial class Controller_Window : Form
    {
        Computer thisPC;
        System.Timers.Timer graphicRefresh;
        SerialPort arduino = new SerialPort("COM1", 19200);
        BackgroundWorker graphics_Updater = new BackgroundWorker();
        BackgroundWorker LEDWritter = new BackgroundWorker();
        XmlDocument xmlDoc = new XmlDocument();
        SerialDevice[] controllers;
        Color foreColor, backColor, calcColor;
        String effect = "Off", controller;
        bool useNext = false, bounce = false, controllerAvailable = false, addressable = false, pixelChange = false, setDefaultChange = false, getData = false;
        bool color1Change = false, color2Change = false, patternChange = false, rateChange = false, pixelColor1Change = false, pixelColor2Change = false, HWupdate = false;
        int rate = 5, hue = 0, controllerIndex = 0, leds = 1, pixel = 1, colorOrder = 0, cpuTemp, gpuTemp, cpuUtil, gpuUtil;
        
        public Controller_Window() //initialize all of the necesary elements for the app
        {
            thisPC = new Computer();            //create a new instance of the computer object from open hardware monitor in order to get hardware info
            thisPC.CPUEnabled = true;           //indicate we want to get CPU and GPU information
            thisPC.GPUEnabled = true;
            thisPC.Open();
            
            InitializeComponent();
            pattern_Select.SelectedIndex = 0;            //set the default pattern to Off
            ColorOrderBox.SelectedIndex = 0;             //set the default color order to RGB

            graphicRefresh = new System.Timers.Timer(33);                           //Set the timer to every 33 ms, or roughly 30FPS
            graphicRefresh.AutoReset = true;                                        //Set autoReset to true so that the timer repeats periodically
            graphicRefresh.Enabled = true;                                          //Activate the timer
            graphicRefresh.Elapsed += new ElapsedEventHandler(RefreshPixels);       //start and subscirbe to a timer in order to handle all necesary color alterations that need to happen in a regular basis independantly from the GraphicsUpdateer

            graphics_Updater.WorkerSupportsCancellation = true;
            graphics_Updater.WorkerReportsProgress = true;
            graphics_Updater.DoWork += new DoWorkEventHandler(graphics_Updater_DoWork);
            graphics_Updater.ProgressChanged += new ProgressChangedEventHandler(graphics_Updater_ProgressChanged);
            graphics_Updater.RunWorkerCompleted += new RunWorkerCompletedEventHandler(graphics_Updater_Completed); //Start and subscribe to a background worker that handles all necesary graphical calculations 

            LEDWritter.WorkerSupportsCancellation = true;
            LEDWritter.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LEDWritter_Completed);
            LEDWritter.DoWork += new DoWorkEventHandler(LEDWritter_DoWork);       //create and susbscribe to a background worker to handle asynncronous serial comunication to provide a seamless experience

            ScanSerialDevices();
            
            try
            {                                                //try to retrieve the use defined colors at the start of the app
                xmlDoc.Load("userInfo.xml");               //load the XML file containing the user data
                int[] custom_colors = new int[16];                  //array to temporarily store and handle the user colors
                foreach(XmlNode node in xmlDoc.ChildNodes[1])       //loop that reads the XML color value and stores it in the array
                {
                    if (node.Name == "Color")                       //Load all of the user's Colors
                    {
                        int index, value;
                        index = Int32.Parse(node.Attributes["index"].Value);
                        value = Int32.Parse(node.Attributes["value"].Value);
                        //Console.WriteLine("Loaded user color " + index + " has a value of: " + value);    //Used for debugging
                        custom_colors[index] = value;
                    }
                    else
                    {
                        if(node.Name == "Default")                       //Load a user profile, including all the necessary information for effect, colors, rate, and bounce propperty
                        {
                            effect = node.Attributes["Pattern"].Value;
                            rate = Int32.Parse(node.Attributes["Rate"].Value);
                            foreColor = Color.FromArgb(Int32.Parse(node.Attributes["ForeColor"].Value));
                            backColor = Color.FromArgb(Int32.Parse(node.Attributes["BackColor"].Value));
                            colorOrder = Int32.Parse(node.Attributes["Order"].Value);
                            if (node.Attributes["Bounce"].Value == "True")
                                bouceEnabled.Checked = true;                            
                        }
                    }                    
                }
                pattern_Select.SelectedItem = effect;
                rate_TrackBar.Value = rate;
                numericRateBox.Value = (decimal)rate;
                Color1_Button.BackColor = foreColor;
                Color2_Button.BackColor = backColor;
                ColorOrderBox.SelectedIndex = colorOrder;
                rate_TrackBar.Value = rate;
                Color_Select.CustomColors = custom_colors;          //Store the custom colors inside the color selector element
            }catch(System.IO.FileNotFoundException){
                xml_Corrupted("was not found");              //If the XML file is not there, run a custom function to create it
            }
            catch (XmlException)
            {
                xml_Corrupted("could not be read");          //If the XML file is not readable, run a custom function to replace it with a blank one
            }catch(FormatException)
            {
                xml_Corrupted("is corrupted: excpected numbers, found something else");         //If the stored values are not numerical, run a custom function to replace them with blanks
            }
        }
        
        private void Color1_Button_Click(object sender, EventArgs e)   //enable the user to select any 24 bit color
        {
            Color_Select.Color = foreColor;                        //This sets the selected color to the backgorund color, making sure that there is no crosstalk between the foreground and background colors               
            if (Color_Select.ShowDialog() == DialogResult.OK)        //Only sets the new color value if the user selects ok
            {                
                foreColor = Color_Select.Color;                     //Save the new color to a global variable "foreColor" which stands for foreground color or main color
                Color1_Button.BackColor = foreColor;                //Set the button to the selected color
                if (controllerAvailable)
                {
                    color1Change = true;                                //Tell the worker that interfaces with the phycial LEDS that the foreground color needs to be updated
                    safeAsync(LEDWritter, true);                        //Proceed to activate the aformentioned worker
                }                
            }
            
        }

        private void Color2_Button_Click(object sender, EventArgs e)    //enable the user to select any 24 bit color (same as the above one)
        {
            Color_Select.Color = backColor;                        //This sets the selected color to the backgorund color, making sure that there is no crosstalk between the foreground and background colors
            if (Color_Select.ShowDialog() == DialogResult.OK)                //Only sets the new color value if the user selects ok
            {                
                backColor = Color_Select.Color;                 //Save the new color to a global variable "backColor", representing the backgorund color to be used by different effects
                Color2_Button.BackColor = backColor;            //Set the button to the selected color
                if (controllerAvailable)
                {
                    color2Change = true;                                //Tell the worker that interfaces with the phycial LEDS that the foreground color needs to be updated
                    safeAsync(LEDWritter, true);                        //Proceed to activate the aformentioned worker
                }
            }
        }
        
        private void pattern_Select_SelectedIndexChanged(object sender, EventArgs e)    //Take appropiate action when the dropdown menu box gets a user input
        {
            effect = pattern_Select.SelectedItem.ToString();                           //Load the Name of the selected item into the "Pattern" variable, since that is what it is
            if (controllerAvailable)
            {
                patternChange = true;                                                       //Tell the code that the LED pattern needs to be updated to the physical controller
                safeAsync(LEDWritter, true);                                                //Activate the backgorund thread that handles the interfacing with the actual controller
            }
            switch (effect){                               //This switch takes appropriate action depending on the pattern selected
                case "Off":
                    safeAsync(graphics_Updater, false);     //All of the effects that dont require color calculations stop the "graphis_Updater" worker
                    break;
                case "Fixed Color":
                    safeAsync(graphics_Updater, false);
                    break;
                case "Color Fade":
                    safeAsync(graphics_Updater, true);      //All of the effects that do require color calculations start the "graphis_Updater" worker
                    break;
                case "Color Chase":
                    if (controllerAvailable && !addressable)          //Some patterns can only be displayed with addressable LED strips, so we inform the user if that is not the case
                        MessageBox.Show(this, "Please note that this effect can only be displayed by addressable LEDS", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    safeAsync(graphics_Updater, true);
                    break;
                case "Color Switching":
                    if (controllerAvailable && !addressable)
                        MessageBox.Show(this, "Please note that this effect can only be displayed by addressable LEDS", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    safeAsync(graphics_Updater, true);
                    break;
                case "Color Rain":
                    if(controllerAvailable && !addressable)
                        MessageBox.Show(this, "Please note that this effect can only be displayed by addressable LEDS", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    safeAsync(graphics_Updater, true);
                    break;
                case "Color Breathe":
                    safeAsync(graphics_Updater, true);
                    break;
                case "Color Cycle":
                    safeAsync(graphics_Updater, true);
                    break;
                case "Color Gradient":
                    if (controllerAvailable && !addressable)
                        MessageBox.Show(this, "Please note that this effect can only be displayed by addressable LEDS", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    safeAsync(graphics_Updater, false);
                    break;
                case "Rainbow":
                    if (controllerAvailable && !addressable)
                    {
                        MessageBox.Show(this, "Please note that this effect can only be displayed by addressable LEDS", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    safeAsync(graphics_Updater, true);
                    break;
                case "Rainbow Chase":
                    if (controllerAvailable && !addressable)
                    {
                        MessageBox.Show(this, "Please note that this effect can only be displayed by addressable LEDS", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    safeAsync(graphics_Updater, true);
                    break;
                case "Rainbow Breathe":
                    if (controllerAvailable && !addressable)
                    {
                        MessageBox.Show(this, "Please note that this effect can only be displayed by addressable LEDS", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    safeAsync(graphics_Updater, true);
                    break;
                case "Custom Pattern":
                    if (controllerAvailable && !addressable)
                    {
                        MessageBox.Show(this, "Please note that this effect can only be displayed by addressable LEDS", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    SetAllPixels(Color.Black);
                    safeAsync(graphics_Updater, false);
                    break;
                case "CPU temperature":
                    safeAsync(graphics_Updater, false);
                    break;
                case "CPU utilization":
                    safeAsync(graphics_Updater, false);
                    break;
                case "GPU temperature":
                    safeAsync(graphics_Updater, false);
                    break;
                case "GPU utilization":
                    safeAsync(graphics_Updater, false);
                    break;
                default:
                    safeAsync(graphics_Updater, false);
                    break;
            }
            setControls();              //This function is called in order to either show or hide controls from the user interface as necessary
        } 
        
        private void rate_TrackBar_Scroll(object sender, EventArgs e)       //React when the rate trackbar is moved
        {
            rate = rate_TrackBar.Value;                 //Get the new rate value from the trackbar
            numericRateBox.Value = (decimal)rate;       //Update the rate value to the numeric box
            if(controllerAvailable)
            rateChange = true;                          //Tell the LEDWritter worker that the rate needs to be updated to the physical LEDS
            safeAsync(LEDWritter, true);                //Activate the LEDWritter worker
        }

        private void bouceEnabled_CheckedChanged(object sender, EventArgs e)   //React to the Bounce checkBox changin state
        {
            bounce = bouceEnabled.Checked;      //Update the new bounce value
            if(controllerAvailable)
            patternChange = true;               //Tell the LEDWritter worker that the bounce needs to be updated in the physical LEDS
            safeAsync(LEDWritter, true);        //Activate the LEDWritter
        }

        private void pixel1_Click(object sender, EventArgs e)       //React when the button that represents pixel 1 is clicked, the exact same procedure is repeated with all the other "pixels"
        {
            if (effect == "Custom Pattern")                    //The code only needs to be executed when Custom pattern is selected
            {
                Color_Select.Color =  pixel1.BackColor;             //Show the windows pantone form and set its color to the current color of the button
                if (Color_Select.ShowDialog() == DialogResult.OK)       //if the user exits the form with the OK button then proceed
                {
                    pixel1.BackColor = Color_Select.Color;          //Set the button's color to the one selected by the user
                    pixelChange = true;                             //A change needs to be updated regarding the pixels for a custom pattern
                    pixel = 1;                                      //Tell the code that there has been a change in the first pixel
                    if(controllerAvailable) safeAsync(LEDWritter, true);    //Activate the LEDWritter worker to interface with the physical LEDS
                }
            }
        }

        private void pixel2_Click(object sender, EventArgs e)
        {
            if (effect == "Custom Pattern")
            {
                Color_Select.Color = pixel2.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel2.BackColor = Color_Select.Color;
                    pixel = 2;
                    pixelChange = true;
                    if (controllerAvailable) safeAsync(LEDWritter, true);
                }
            }
        }

        private void pixel3_Click(object sender, EventArgs e)
        {
            if (effect == "Custom Pattern")
            {
                Color_Select.Color = pixel3.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel3.BackColor = Color_Select.Color;
                    pixel = 3;
                    pixelChange = true;
                    if (controllerAvailable) safeAsync(LEDWritter, true);
                }
            }
        }

        private void pixel4_Click(object sender, EventArgs e)
        {            
            if (effect == "Custom Pattern")
            {
                Color_Select.Color = pixel4.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel4.BackColor = Color_Select.Color;
                    pixel = 4;
                    pixelChange = true;
                    if (controllerAvailable) safeAsync(LEDWritter, true);
                }
            }
        }

        private void pixel5_Click(object sender, EventArgs e)
        {
            if (effect == "Custom Pattern")
            {
                Color_Select.Color = pixel5.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel5.BackColor = Color_Select.Color;
                    pixel = 5;
                    pixelChange = true;
                    if(controllerAvailable) safeAsync(LEDWritter, true);
                }
            }
        }

        private void pixel6_Click(object sender, EventArgs e)
        {
            if (effect == "Custom Pattern")
            {
                Color_Select.Color = pixel6.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel6.BackColor = Color_Select.Color;
                    pixel = 6;
                    pixelChange = true;
                    if(controllerAvailable) safeAsync(LEDWritter, true);
                }
            }
        }

        private void pixel7_Click(object sender, EventArgs e)
        {
            if (effect == "Custom Pattern")
            {
                Color_Select.Color = pixel7.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel7.BackColor = Color_Select.Color;
                    pixel = 7;
                    pixelChange = true;
                    if(controllerAvailable) safeAsync(LEDWritter, true);
                }
            }
        }

        private void pixel8_Click(object sender, EventArgs e)
        {
            if (effect == "Custom Pattern")
            {
                Color_Select.Color = pixel8.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel8.BackColor = Color_Select.Color;
                    pixel = 8;
                    pixelChange = true;
                    if(controllerAvailable) safeAsync(LEDWritter, true);
                }
            }
        }

        private void pixel9_Click(object sender, EventArgs e)
        {
            if (effect == "Custom Pattern")
            {
                Color_Select.Color = pixel9.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel9.BackColor = Color_Select.Color;
                    pixel = 9;
                    pixelChange = true;
                    if(controllerAvailable) safeAsync(LEDWritter, true);
                }
            }
        }

        private void pixel10_Click(object sender, EventArgs e)
        {
            if (effect == "Custom Pattern")
            {
                Color_Select.Color = pixel10.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel10.BackColor = Color_Select.Color;
                    pixel = 10;
                    pixelChange = true;
                    if(controllerAvailable) safeAsync(LEDWritter, true);
                }
            }
        }

        private void graphics_Updater_DoWork(object sender, DoWorkEventArgs e)      //This background worker takes care of all the necesary counters to do the adequate color calculations to be displayed
        {
            if (effect.Contains("Chase") || effect.Contains("Switching"))         //If the effect is either "chase" or "switching" (of any kind) from 1 to 10 because there are 10 buttons that represent pixels
            {
                for (int i = 1; i <= 10; i++)
                {
                    if (graphics_Updater.CancellationPending)               //check on every iteration if the worker should be cancelled
                    {
                        e.Cancel = true;
                        break;                                              //break the loop to ensure proper behaviour in a timely manner (this is done for all time consuming loops)
                    }
                    graphics_Updater.ReportProgress(i);
                    Thread.Sleep((rate * 2) + 20);                           //The rate variable is used to determine the delayg between iterations, thereby controlling the speed of the counting (used for all loops)
                }
                useNext = !useNext;                                             //This variable is used for the swiching effect, as the color needs to be changed for every pass
                if (bounce)                              //If bounce is enabled, count back from 10 to 1
                {
                    for (int i = 10; i > 0; i--)
                    {
                        if (graphics_Updater.CancellationPending)
                        {
                            e.Cancel = true;
                            break;
                        }
                        graphics_Updater.ReportProgress(i);
                        Thread.Sleep((rate * 2) + 20);
                    }
                    useNext = !useNext;
                }

            }
            ////////////////////////////////////////////////////////////////////
            if (effect.Contains("Breathe") || effect.Contains("Fade") || effect == "Color Rain")             //For the any "Breathe", "Fade", or "Color Rain effect"
            {
                for (int i = 1; i < 100; i++)                           //Count from 1 to 100
                {
                    if (graphics_Updater.CancellationPending)       
                    {
                        e.Cancel = true;
                        break;
                    }
                    graphics_Updater.ReportProgress(i);
                    Thread.Sleep(rate);
                }
                for (int i = 100; i > 1; i--)                           //Count back from 100 to 1
                {
                    if (graphics_Updater.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    graphics_Updater.ReportProgress(i);
                    Thread.Sleep(rate);
                }
                useNext = !useNext;                                    //After the count has gone from 1-100 and back, change the color
            }
            ///////////////////////////////////////////////////////
            if (effect == "Rainbow" || effect == "Color Cycle")           //For the "Rainbow" and "Colo Cycle" effects
            {
                if (graphics_Updater.CancellationPending)
                {
                    e.Cancel = true;
                }
                hue++;                     //Simply increase the hue variable by 1
                if (hue > 360)             //Make sure that hue staying wihtin its acceptable bounds 0-360
                {
                    hue = 0;
                }
                Thread.Sleep(rate);
            }
        }

        private void graphics_Updater_ProgressChanged(object sender, ProgressChangedEventArgs e)        //this is the part of the background worker that actually deals with the colors and pixels
        {
            int p = e.ProgressPercentage;       //Create a local variable to better keep track of the counting reported by the DoWork part of the worker
            if (effect.Contains("Breathe"))
            {
                Color temp;
                int fadeAmount = (int)(p * 2.55);
                if (!useNext)
                {
                    temp = ColorMix(foreColor, Color.Black, fadeAmount);
                }
                else
                {
                    temp = ColorMix(backColor, Color.Black, fadeAmount);
                }
                if (effect.Contains("Rainbow"))
                {
                    float value = (float)p / 100;
                    hue++;
                    calcColor = ColorFromHSV(hue, 1, value);
                    //Console.WriteLine("{0}  {1}  {2}", p, hue, value);
                }
                else
                {
                    calcColor = temp;
                }
                //Console.WriteLine(p.ToString() + "    " + r.ToString() + "  " + g.ToString() + "  " + b.ToString());
            }
            /////////////////////////////////////////////////////////////////
            if (effect.Contains("Chase"))
            {
                int pixel = constrain(e.ProgressPercentage, 1, 10);
                if (effect.Contains("Rainbow"))
                {
                    hue += 2;
                    setPixel(pixel, ColorFromHSV(hue, 1, 1));
                    //Console.WriteLine(pixel + "  " + hue);
                }
                else
                {
                    setPixel(pixel, foreColor);
                    //Console.WriteLine(pixel + " " + foreColor.R  + "  " + foreColor.G + "  " + foreColor.B);
                }
            }
            ///////////////////////////////////////////////////////////////
            if (effect.Contains("Fade"))
            {
                int fadeAmount = (int) (p * 2.55);
                Color temp = ColorMix(backColor, foreColor, fadeAmount);
                SetAllPixels(temp);
            }
            ///////////////////////////////////////////////////////////////
            if (effect.Contains("Switching"))
            {
                int pixel = constrain(e.ProgressPercentage, 1, 10);
                if (!useNext)
                {
                    setPixel(pixel, foreColor);
                }
                else
                {
                    setPixel(pixel, backColor);
                }
            }
           ////////////////////////////////////////////////////////////////////////
           if(effect == "Color Rain"){
                Random generator = new Random();
                if(generator.Next(0,10) == 5)
                    setPixel(generator.Next(1, 11), foreColor);
            }
        }

        private void graphics_Updater_Completed(object sender, RunWorkerCompletedEventArgs e)       //This function re-starts the background worker unless an error or cancelation arises
        {
            if (e.Error != null)
            {
                MessageBox.Show(this,"The color updater has encountered an error.\n OK to ternimate", "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
            }
            else if (e.Cancelled)
            {
                //Console.WriteLine("*****The graphics updater worker has been ternimated!*****");
            }
            else
            {
                //Console.WriteLine("*****Graphics updater worker has completed its task!*****");
                graphics_Updater.RunWorkerAsync();
            }
        }

        private void LEDWritter_DoWork(object sender, DoWorkEventArgs e)    //This background Worker is used to communicate with the led controller asynchronously in order to mantain a responsive UI
        {
            Thread.Sleep(10);
            if(LEDWritter.CancellationPending && !controllerAvailable){               //Only go throught the serial sequences is there is a controller connected and the worker should be running
                e.Cancel = true;
            }
            else
            {
                if (pixelChange)                //Go into this statement if a single pixel should be written to, this happens with custom pattern
                {
                    //Console.WriteLine("pix");     //Used for debugging
                    String color = "";           //Initialize a String with the basic structure for the controller to recognize
                    switch (colorOrder)                 //Send the correct color order to the controller, some pixel chipsets are not RGB
                    {
                        case 0:
                            color += GetPixelColor(pixel).R + "," + GetPixelColor(pixel).G + "," + GetPixelColor(pixel).B;  //RGB
                            break;
                        case 1:
                            color += GetPixelColor(pixel).R + "," + GetPixelColor(pixel).B + "," + GetPixelColor(pixel).G;   //RBG
                            break;
                        case 2:
                            color += GetPixelColor(pixel).G + "," + GetPixelColor(pixel).R + "," + GetPixelColor(pixel).B;    //GRB
                            break;
                        case 3:
                            color += GetPixelColor(pixel).G + "," + GetPixelColor(pixel).B + "," + GetPixelColor(pixel).R;   //GBR
                            break;
                        case 4:
                            color += GetPixelColor(pixel).B + "," + GetPixelColor(pixel).R + "," + GetPixelColor(pixel).G;   //BRG
                            break;
                        case 5:
                            color += GetPixelColor(pixel).B + "," + GetPixelColor(pixel).G + "," + GetPixelColor(pixel).R;   //BGR
                            break;
                        default:
                            color += "0,0,0";   //0 to all just in case something weird happens
                            break;
                    }
                    if ("OK" == controllers[controllerIndex].WriteSerial("Pixel," + (pixel - 1) + "," + color));       //Write the actual data, which consists of the pixel number, plus the RGB color values, the write function returns whatever the controller sends back
                        pixelChange = false;        //if the controller responds with an "OK", then we know that the attribute has been updated. Otherwise, the variable remains true and another attempt is made in the next cycle
                    Thread.Sleep(100);               //Give enough time for the controller to aknowledge the new instructions
                }
                if (getData)                //Read the settings from the current controller (still in progress)
                {
                    getData = false;
                    String[] resp = controllers[controllerIndex].WriteSerial("Read").Split(',');
                    foreach (String item in resp)
                    {
                        Console.Write(item + "\t");
                    }
                }
                if (HWupdate)
                {
                    String returnMessage = "";
                    switch (effect)
                    {
                        case "CPU temperature":
                            returnMessage = controllers[controllerIndex].WriteSerial("Fill Gradient," + (int)controllers[controllerIndex].leds * cpuTemp / 100);
                            break;
                        case "CPU utilization":
                            returnMessage = controllers[controllerIndex].WriteSerial("Fill Gradient," + (int)controllers[controllerIndex].leds * cpuUtil / 100);
                            break;
                        case "GPU temperature":
                            returnMessage = controllers[controllerIndex].WriteSerial("Fill Gradient," + (int)controllers[controllerIndex].leds * gpuTemp / 100);
                            break;
                        case "GPU utilization":
                            returnMessage = controllers[controllerIndex].WriteSerial("Fill Gradient," + (int)controllers[controllerIndex].leds * gpuUtil / 100);
                            break;
                    }
                    if (returnMessage == "OK")
                        HWupdate = false;
                    Thread.Sleep(100);
                }
                    
                if (patternChange)          //Update the LED effect 
                {
                    //Console.WriteLine("pattern");     //Used for debugging
                    String returnMessage = ""; 
                    switch(effect)
                    {
                        case "CPU temperature":
                            returnMessage = controllers[controllerIndex].WriteSerial("Fill Gradient," + (int)controllers[controllerIndex].leds * cpuTemp / 100);
                            break;
                        case "CPU utilization":
                            returnMessage = controllers[controllerIndex].WriteSerial("Fill Gradient," + (int)controllers[controllerIndex].leds * cpuUtil / 100);
                            break;
                        case "GPU temperature":
                            returnMessage = controllers[controllerIndex].WriteSerial("Fill Gradient," + (int)controllers[controllerIndex].leds * gpuTemp / 100);
                            break;
                        case "GPU utilization":
                            returnMessage = controllers[controllerIndex].WriteSerial("Fill Gradient," + (int)controllers[controllerIndex].leds * gpuUtil / 100);
                            break;
                        default:
                            returnMessage = controllers[controllerIndex].WriteSerial(effect);
                            if (bounce)
                                controllers[controllerIndex].WriteSerial(" Bounce");
                            break;
                    }
                        
                    
                    if(returnMessage == "OK")
                        patternChange = false;
                    Thread.Sleep(100);
                }
                if (color1Change)       //Update the foreground color in the controller
                {
                    //Console.WriteLine("color1");      //for debugging
                    String color = "Color1,";           //Initialize a String with the basic structure for the controller to recognize
                    switch (colorOrder)                 //Send the correct color order to the controller, some pixel chipsets are not RGB
                    {
                        case 0:
                            color += foreColor.R + "," + foreColor.G + "," + foreColor.B;  //RGB
                            break;
                        case 1:
                            color += foreColor.R + "," + foreColor.B + "," + foreColor.G;   //RBG
                            break;
                        case 2:
                            color += foreColor.G + "," + foreColor.R + "," + foreColor.B;    //GRB
                            break;
                        case 3:
                            color += foreColor.G + "," + foreColor.B + "," + foreColor.R;   //GBR
                            break;
                        case 4:
                            color += foreColor.B + "," + foreColor.R + "," + foreColor.G;   //BRG
                            break;
                        case 5:
                            color += foreColor.B + "," + foreColor.G + "," + foreColor.R;   //BGR
                            break;
                        default:
                            color += "0,0,0";   //0 to all just in case something weird happens
                            break;
                    }
                    if("OK" == controllers[controllerIndex].WriteSerial(color))     //If the controller replies with "OK" we know all is good
                        color1Change = false;   //Set this variable false becasue we know the update was succesful
                    Thread.Sleep(100);          //Give the controller some time to react and be ready for the next command
                }
                if (color2Change)
                {
                    //Console.WriteLine("color2");      //for debugging
                    String color = "Color2,";           //Initialize a String with the basic structure for the controller to recognize
                    switch (colorOrder)                 //Send the correct color order to the controller, some pixel chipsets are not RGB
                    {
                        case 0:
                            color += backColor.R + "," + backColor.G + "," + backColor.B;  //RGB
                            break;
                        case 1:
                            color += backColor.R + "," + backColor.B + "," + backColor.G;   //RBG
                            break;
                        case 2:
                            color += backColor.G + "," + backColor.R + "," + backColor.B;    //GRB
                            break;
                        case 3:
                            color += backColor.G + "," + backColor.B + "," + backColor.R;   //GBR
                            break;
                        case 4:
                            color += backColor.B + "," + backColor.R + "," + backColor.G;   //BRG
                            break;
                        case 5:
                            color += backColor.B + "," + backColor.G + "," + backColor.R;   //BGR
                            break;
                        default:
                            color += "0,0,0";   //0 to all just in case something weird happens
                            break;
                    }
                    if ("OK" == controllers[controllerIndex].WriteSerial(color))
                        color2Change = false;
                    Thread.Sleep(100);
                }
                if (rateChange)
                {
                    //Console.WriteLine("rate");        //for debugging
                    String r = "Rate," + rate;
                    if("OK" == controllers[controllerIndex].WriteSerial(r))
                        rateChange = false;
                    Thread.Sleep(100);
                }
                if (setDefaultChange)
                {
                    //Console.WriteLine("default");     //for debugging
                    if ("OK" == controllers[controllerIndex].WriteSerial("Save Default"))
                        setDefaultChange = false;
                    Thread.Sleep(100);
                }
                if (pixelColor1Change)
                {
                    String color = "";           //Initialize a String with the basic structure for the controller to recognize
                    switch (colorOrder)                 //Send the correct color order to the controller, some pixel chipsets are not RGB
                    {
                        case 0:
                            color += foreColor.R + "," + foreColor.G + "," + foreColor.B;  //RGB
                            break;
                        case 1:
                            color += foreColor.R + "," + foreColor.B + "," + foreColor.G;   //RBG
                            break;
                        case 2:
                            color += foreColor.G + "," + foreColor.R + "," + foreColor.B;    //GRB
                            break;
                        case 3:
                            color += foreColor.G + "," + foreColor.B + "," + foreColor.R;   //GBR
                            break;
                        case 4:
                            color += foreColor.B + "," + foreColor.R + "," + foreColor.G;   //BRG
                            break;
                        case 5:
                            color += foreColor.B + "," + foreColor.G + "," + foreColor.R;   //BGR
                            break;
                        default:
                            color += "0,0,0";   //0 to all just in case something weird happens
                            break;
                    }
                    controllers[controllerIndex].WriteSerial("Pixel," + (pixel - 1) + "," +color);
                    pixelColor1Change = false;
                    Thread.Sleep(100);
                }
                if (pixelColor2Change)
                {
                    String color = "";           //Initialize a String with the basic structure for the controller to recognize
                    switch (colorOrder)                 //Send the correct color order to the controller, some pixel chipsets are not RGB
                    {
                        case 0:
                            color += backColor.R + "," + backColor.G + "," + backColor.B;  //RGB
                            break;
                        case 1:
                            color += backColor.R + "," + backColor.B + "," + backColor.G;   //RBG
                            break;
                        case 2:
                            color += backColor.G + "," + backColor.R + "," + backColor.B;    //GRB
                            break;
                        case 3:
                            color += backColor.G + "," + backColor.B + "," + backColor.R;   //GBR
                            break;
                        case 4:
                            color += backColor.B + "," + backColor.R + "," + backColor.G;   //BRG
                            break;
                        case 5:
                            color += backColor.B + "," + backColor.G + "," + backColor.R;   //BGR
                            break;
                        default:
                            color += "0,0,0";   //0 to all just in case something weird happens
                            break;
                    }
                    controllers[controllerIndex].WriteSerial("Pixel," + (pixel - 1) + "," + color);
                    pixelColor2Change = false;
                    Thread.Sleep(100);
                }
            }
        }

        private void LEDWritter_Completed(object sender, AsyncCompletedEventArgs e)     //This function re-starts the background worker unless an error or cancelation arises
        {
            if (e.Error != null)
            {
                MessageBox.Show(this, "The Led Writter has encountered an error.\n OK to ternimate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            else if (e.Cancelled)
            {
                //Console.WriteLine("*****The graphics updater worker has been ternimated!*****");
            }
            else
            {
                LEDWritter.RunWorkerAsync();
                //Console.WriteLine("*****Graphics updater worker has completed its task!*****");
            }
        }

        private void RefreshPixels(object sender, ElapsedEventArgs e)       //this is a timer driven event, repeated every 33 milliseconds, to compliment the graphicsUpdater worker
        {
            switch (effect)
            {
                case "Off":
                    fadeAllPixels(Color.Black,16);                          //Smoothly turn off the buttons that represent pixels
                    safeAsync(graphics_Updater, false);                     //Be absolutely sure that graphics_Updater is not operating (since there is no delay for case: "Off" the processor would begin to choke)
                    break;
                case "Fixed Color":
                    fadeAllPixels(foreColor,20);                            //Smoothly turn the pixels to the selected color
                    safeAsync(graphics_Updater, false);                     //Ensure that graphics_Updater is not operating
                    break;
                case "Color Rain":
                    fadeAllPixels(backColor, 15);                           //Slowly return the pixels to the background color, this compliments the graphics_Updater task
                    break;
                case "Color Breathe":
                    SetAllPixels(calcColor);                                //Turn the pixels to the calculated color from the graphics_Updater at a rate of 30FPS
                    break;
                case "Color Chase":
                    fadeAllPixels(backColor, (55 - rate) * 2);
                    break;
                case "Color Gradient":
                    backColor = ColorFromHSV(backColor.GetHue(), 1, 1);
                    foreColor = ColorFromHSV(foreColor.GetHue(), 1, 1);
                    if( color1Change || color2Change)
                    {
                        Color1_Button.BackColor = foreColor;
                        Color2_Button.BackColor = backColor;
                    }
                    PixelGradient(foreColor, backColor);
                    safeAsync(graphics_Updater, false);
                    break;
                case "Rainbow Breathe":
                    SetAllPixels(calcColor);
                    break;
                case "Color Cycle":
                    SetAllPixels(ColorFromHSV(hue, 1, 1));
                    break;
                case "Rainbow Chase":
                    fadeAllPixels(backColor, (20 - rate) * 2);
                    break;
                case ("Rainbow"):
                    RainbowPixels(hue, 7);
                    break;
                case "CPU temperature":
                    Fill_Partial_Gradient(foreColor, backColor, cpuTemp / 10);
                    break;
                case "CPU utilization":
                    Fill_Partial_Gradient(foreColor, backColor, cpuUtil / 10);
                    break;
                case "GPU temperature":
                    Fill_Partial_Gradient(foreColor, backColor, gpuTemp / 10);
                    break;
                case "GPU utilization":
                    Fill_Partial_Gradient(foreColor, backColor, gpuUtil / 10);
                    break;
                case "Custom Pattern":
                    safeAsync(graphics_Updater, false);
                    break;
            }
        }

        private void PixelGradient(Color startColor, Color endColor)
        {
            float starthue = startColor.GetHue();
            float endhue = endColor.GetHue();
            float huestep = (Math.Abs(endhue - starthue))/9;
            if(starthue < endhue)
            {
                pixel1.BackColor = startColor;
                pixel2.BackColor = ColorFromHSV(starthue + huestep, 1, 1);
                pixel3.BackColor = ColorFromHSV(starthue + 2 * huestep, 1, 1);
                pixel4.BackColor = ColorFromHSV(starthue + 3 * huestep, 1, 1);
                pixel5.BackColor = ColorFromHSV(starthue + 4 * huestep, 1, 1);
                pixel6.BackColor = ColorFromHSV(starthue + 5 * huestep, 1, 1);
                pixel7.BackColor = ColorFromHSV(starthue + 6 * huestep, 1, 1);
                pixel8.BackColor = ColorFromHSV(starthue + 7 * huestep, 1, 1);
                pixel9.BackColor = ColorFromHSV(starthue + 8 * huestep, 1, 1);
                pixel10.BackColor = ColorFromHSV(starthue + 9 * huestep, 1, 1);
            }
            else
            {
                pixel1.BackColor = startColor;
                pixel2.BackColor = ColorFromHSV(starthue - huestep, 1, 1);
                pixel3.BackColor = ColorFromHSV(starthue - 2 * huestep, 1, 1);
                pixel4.BackColor = ColorFromHSV(starthue - 3 * huestep, 1, 1);
                pixel5.BackColor = ColorFromHSV(starthue - 4 * huestep, 1, 1);
                pixel6.BackColor = ColorFromHSV(starthue - 5 * huestep, 1, 1);
                pixel7.BackColor = ColorFromHSV(starthue - 6 * huestep, 1, 1);
                pixel8.BackColor = ColorFromHSV(starthue - 7 * huestep, 1, 1);
                pixel9.BackColor = ColorFromHSV(starthue - 8 * huestep, 1, 1);
                pixel10.BackColor = ColorFromHSV(starthue - 9 * huestep, 1, 1);
            }            
        }

        private void PixelFill(Color target, int pixNum)
        {
            SetAllPixels(Color.Black);
            if(pixNum >= 1)
                pixel1.BackColor = target;
            if (pixNum >= 2)
                pixel2.BackColor = target;
            if (pixNum >= 3)
                pixel3.BackColor = target;
            if (pixNum >= 4)
                pixel4.BackColor = target;
            if (pixNum >= 5)
                pixel5.BackColor = target;
            if (pixNum >= 6)
                pixel6.BackColor = target;
            if (pixNum >= 7)
                pixel7.BackColor = target;
            if (pixNum >= 8)
                pixel8.BackColor = target;
            if (pixNum >= 9)
                pixel9.BackColor = target;
            if (pixNum >= 10)
                pixel10.BackColor = target;
        }

        private void setControls()      //This function handles the controls displauyed by the user interface
        {
            bouceEnabled.Enabled = false;
            numericLEDNumber.Enabled = false;
            ColorOrderBox.Enabled = false;
            pixelSetColor1_Button.Enabled = false;
            pixelSetColor2_Button.Enabled = false;
            
            if (effect.Contains("Rainbow") || effect == "Off") Color1_Button.Enabled = false;
                else Color1_Button.Enabled = true;

            if (effect == "Rainbow" || effect == "Rainbow Breathe" || effect.Contains("Cycle") || effect == "Off" || effect.Contains("Fixed")) Color2_Button.Enabled = false;
                else Color2_Button.Enabled = true;

            if (effect.Contains("Gradient") || effect.Contains("Fixed") || effect.Contains("Off") || effect.Contains("Custom") || effect.Contains("PU"))
            {
                numericRateBox.Enabled = false;
                rate_TrackBar.Enabled = false;
            }
            else
            {
                numericRateBox.Enabled = true;
                rate_TrackBar.Enabled = true;
            }
            if(effect.Contains("Chase") || effect.Contains("Switch")) bouceEnabled.Enabled = true;
            
            if (effect.Contains("Custom"))
            {
                pixelSetColor1_Button.Enabled = true;
                pixelSetColor2_Button.Enabled = true;
                if (controllerAvailable)
                {
                    numericLEDNumber.Enabled = true;
                    pixelSetColor1_Button.Enabled = true;
                    pixelSetColor2_Button.Enabled = true;
                }
                else
                {
                    numericLEDNumber.Enabled = false;
                    pixelSetColor1_Button.Enabled = false;
                    pixelSetColor2_Button.Enabled = false;
                }
            }
            if (controllerAvailable)
            {
                ColorOrderBox.Enabled = true;
            }
        }        

        private void ScanSerialDevices()        //Scan all serial ports and try to find a compatible controller
        {
            try                         //The try/catch structure is used in case the controllers array has not yet been created
            {
                foreach (SerialDevice item in controllers)
                {      
                    item.SafePortOpenClose(false);              //Get rid of all the existing serial ports in the controllers array 
                    item.port.Dispose();                        //this is done so that we dont get an execption later when trying to access a port if it is already assigned to a controller
                }
            }
            catch { }
            int numPorts = SerialPort.GetPortNames().Length;        //Get the number of serial ports in a variable to be used for various checks later
            controllers = new SerialDevice[numPorts];           //Initialize the controllers array with as many elements as serial ports
            for (int i = 0; i < numPorts; i++)                  //Cycle trough every serial port
            {
                SerialDevice temp = new SerialDevice();         //Create a temporary SerialDevice object to which all the data will be loaded
                temp.Initilize(SerialPort.GetPortNames()[i]);   //Set the serial port to the current one iterating in the loop
                controllers[i] = temp;                          //load all of the properties of the temporary object into an element of the public array
                //Console.WriteLine("COM port found in : " + controllers[i].port.PortName);       //For debugging
            }
        }

        private Color ColorFromHSV(double hue, double saturation, double value)     //Return a color object from the given HSV values, algorithm written by Greg from StackOverFlow
        {                                                                      //http://stackoverflow.com/users/12971/greg huge thanks to him, information taken from the HSL and HSV wiki https://en.wikipedia.org/wiki/HSL_and_HSV
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        private void runInBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void SetAllPixels(Color color)      //Set all of the buttons representing pixels to the same color
        {
            pixel1.BackColor = color;
            pixel2.BackColor = color;
            pixel3.BackColor = color;
            pixel4.BackColor = color;
            pixel5.BackColor = color;
            pixel6.BackColor = color;
            pixel7.BackColor = color;
            pixel8.BackColor = color;
            pixel9.BackColor = color;
            pixel10.BackColor = color;
        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
            alwaysOnTopToolStripMenuItem.Checked = this.TopMost;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
        }

        private void reactiveEffectToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            effect = e.ClickedItem.Text;
            pattern_Select.SelectedItem = effect;
        }

        private void RainbowPixels(double initialhue, int dispersion)       //Set a spectrum pattern on the buttons representing pixels
        {
            pixel1.BackColor = ColorFromHSV(initialhue, 1 , 1);                 //the first button sets its color according to the specified initial hue
            pixel2.BackColor = ColorFromHSV(initialhue + dispersion, 1, 1);     //The rest of the buttons add a certain amount of extra hue in order to create the spectrum
            pixel3.BackColor = ColorFromHSV(initialhue + dispersion * 2, 1, 1); 
            pixel4.BackColor = ColorFromHSV(initialhue + dispersion * 3, 1, 1);
            pixel5.BackColor = ColorFromHSV(initialhue + dispersion * 4, 1, 1);
            pixel6.BackColor = ColorFromHSV(initialhue + dispersion * 5, 1, 1);
            pixel7.BackColor = ColorFromHSV(initialhue + dispersion * 6, 1, 1);
            pixel8.BackColor = ColorFromHSV(initialhue + dispersion * 7, 1, 1);
            pixel9.BackColor = ColorFromHSV(initialhue + dispersion * 8, 1, 1);
            pixel10.BackColor = ColorFromHSV(initialhue + dispersion * 9, 1, 1);
        }

        private void Controller_Window_Load(object sender, EventArgs e)
        {

        }

        private void Fill_Partial_Gradient(Color startColor, Color endColor, int upToPixel)
        {

            float starthue = startColor.GetHue();
            float endhue = endColor.GetHue();
            float huestep = (Math.Abs(endhue - starthue)) / 9;
            if (starthue < endhue)
            {
                SetAllPixels(Color.Black);
                if (upToPixel >= 1)
                    pixel1.BackColor = startColor; ;
                if (upToPixel >= 2)
                    pixel2.BackColor = ColorFromHSV(starthue + huestep, 1, 1); ;
                if (upToPixel >= 3)
                    pixel3.BackColor = ColorFromHSV(starthue + 2 * huestep, 1, 1);
                if (upToPixel >= 4)
                    pixel4.BackColor = ColorFromHSV(starthue + 3 * huestep, 1, 1);
                if (upToPixel >= 5)
                    pixel5.BackColor = ColorFromHSV(starthue + 4 * huestep, 1, 1);
                if (upToPixel >= 6)
                    pixel6.BackColor = ColorFromHSV(starthue + 5 * huestep, 1, 1);
                if (upToPixel >= 7)
                    pixel7.BackColor = ColorFromHSV(starthue + 6 * huestep, 1, 1);
                if (upToPixel >= 8)
                    pixel8.BackColor = ColorFromHSV(starthue + 7 * huestep, 1, 1);
                if (upToPixel >= 9)
                    pixel9.BackColor = ColorFromHSV(starthue + 8 * huestep, 1, 1);
                if (upToPixel >= 10)
                    pixel10.BackColor = ColorFromHSV(starthue + 9 * huestep, 1, 1);
            }
            else
            {
                SetAllPixels(Color.Black);
                if (upToPixel >= 1)
                    pixel1.BackColor = startColor; ;
                if (upToPixel >= 2)
                    pixel2.BackColor = ColorFromHSV(starthue - huestep, 1, 1); ;
                if (upToPixel >= 3)
                    pixel3.BackColor = ColorFromHSV(starthue - 2 * huestep, 1, 1);
                if (upToPixel >= 4)
                    pixel4.BackColor = ColorFromHSV(starthue - 3 * huestep, 1, 1);
                if (upToPixel >= 5)
                    pixel5.BackColor = ColorFromHSV(starthue - 4 * huestep, 1, 1);
                if (upToPixel >= 6)
                    pixel6.BackColor = ColorFromHSV(starthue - 5 * huestep, 1, 1);
                if (upToPixel >= 7)
                    pixel7.BackColor = ColorFromHSV(starthue - 6 * huestep, 1, 1);
                if (upToPixel >= 8)
                    pixel8.BackColor = ColorFromHSV(starthue - 7 * huestep, 1, 1);
                if (upToPixel >= 9)
                    pixel9.BackColor = ColorFromHSV(starthue - 8 * huestep, 1, 1);
                if (upToPixel >= 10)
                    pixel10.BackColor = ColorFromHSV(starthue - 9 * huestep, 1, 1);
            }
        }

        private void HWtimer_Tick(object sender, EventArgs e)
        {
            if (controllerAvailable)
                HWupdate = true;
            foreach(IHardware component in thisPC.Hardware)
            {
                component.Update();
                if(component.HardwareType == HardwareType.CPU)
                {
                    label5.Text = component.Name;
                    foreach (ISensor sens in component.Sensors)
                    {
                        if (sens.SensorType == SensorType.Temperature)
                        {
                            cpuTemp = (int) sens.Value;
                            label6.Text = "CPU temp: " + String.Concat(cpuTemp) + "°C";
                        }     
                        if(sens.SensorType == SensorType.Load)
                        {
                            cpuUtil = (int) sens.Value;
                            label9.Text = "CPU util: " + String.Concat(cpuUtil) + "%";
                        }                       
                    }
                }
                if (component.HardwareType == HardwareType.GpuNvidia)
                {
                    label7.Text = component.Name;
                    foreach (ISensor sens in component.Sensors)
                    {
                        if (sens.SensorType == SensorType.Temperature)
                        {
                            gpuTemp = (int) sens.Value;
                            label8.Text = "GPU temp: " + String.Concat(gpuTemp);
                        }
                        if (sens.SensorType == SensorType.Load)
                        {
                            gpuUtil = (int)sens.Value;
                            label10.Text = "GPU util: " + String.Concat(gpuUtil);
                        }
                    }
                }
                if(component.HardwareType == HardwareType.GpuAti)
                {
                    label7.Text = component.Name;
                    foreach(ISensor sens in component.Sensors)
                    {
                        if(sens.SensorType == SensorType.Temperature)
                        {
                            gpuTemp = (int)sens.Value;
                            label8.Text = "GPU temp" + String.Concat(gpuTemp);
                        }
                        if (sens.SensorType == SensorType.Load)
                        {
                            gpuUtil = (int)sens.Value;
                        }
                    }
                }
            }
        }

        private void ColorOrderBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            colorOrder = ColorOrderBox.SelectedIndex;
            //Console.WriteLine("Color Order Index {0}", colorOrder);
        }

        private void setPixel(int pixel_Number,Color x)         //function to set the color of any of the buttons representing pixels
        {
            if(pixel_Number >=1 && pixel_Number <=10)           //Make sure the index is within proper range
            {
                switch (pixel_Number)           //Sinply use a switch to select the correct button
                {
                    case 1:
                        pixel1.BackColor = x;
                        break;
                    case 2:
                        pixel2.BackColor = x;
                        break;
                    case 3:
                        pixel3.BackColor = x;
                        break;
                    case 4:
                        pixel4.BackColor = x;
                        break;
                    case 5:
                        pixel5.BackColor = x;
                        break;
                    case 6:
                        pixel6.BackColor = x;
                        break;
                    case 7:
                        pixel7.BackColor = x;
                        break;
                    case 8:
                        pixel8.BackColor = x;
                        break;
                    case 9:
                        pixel9.BackColor = x;
                        break;
                    case 10:
                        pixel10.BackColor = x;
                        break;
                }
            }
        }

        private void GetData_Button_Click(object sender, EventArgs e)       //Get the current status of the physical LED controller
        {
            getData = true;                 
            safeAsync(LEDWritter, true);
        }

        private void Connect_Button_Click(object sender, EventArgs e)       //This button toggles the internal parameters that tell other pieces of code if there's a controller available
        {
            if(Connect_Button.Text == "Connect")                    //If the text is "connect", then the code knows there are no stablished parameters at that time
            {
                for (int i = 0; i < controllers.Length; i++)        //go through all the devices found in the controllers array, one for each serial port
                {
                    if (controllers[i].selected)                    //when the controller selected by the user on the combo-box is found start the process
                    {
                        controllerAvailable = true;                           //The code now knows there are leds available
                        addressable = controllers[i].addressable;   //Fetch the vriable from the selected controller
                        leds = controllers[i].leds;                 //Fetch the number of LEDs from the selected controller
                        controllerIndex = i;                        //This variable is used to access the selected controller in an easier manner
                        //Console.WriteLine("add: {0} numleds: {1} name: {2}", addressable, leds, controllers[i].controllerName);   //Used for debugging
                        Devices_ComboBox.Enabled = false;           //Disable the combo box so that the selected controller can't be changed inproperly
                        Connect_Button.Text = "Disconnect";         //Change the text on the button to give the user the option to de-select the current controller
                        label13.Text = leds + " found ";
                        break;                                                           
                    }
                }                
            }
            else
            {
                controllers[controllerIndex].SafePortOpenClose(false);
                controllerAvailable = false;                         //If the button's text is not "connect", then tell the rest of the code there is no available controller
                Connect_Button.Text = "Connect";           //Change the text to connect so the "connect" process can be re-started
                Devices_ComboBox.Enabled = true;           //Enable the controller selection box to give the user the option to target any controller
            }
            
        }

        private void Devices_ComboBox_SelectedIndexChanged(object sender, EventArgs e)      //whenever the controller selection box changes its selected object execute this function
        {
            for(int i = 0; i < controllers.Length; i++)     //cycle through all the controllers array to search for the selected one
            {
                if (Devices_ComboBox.SelectedItem.ToString() == controllers[i].controllerName)      //Once the selected controller has been found in the array
                {
                    controllers[i].selected = true;         //Mark it as the selected one
                    controller = controllers[i].controllerName;
                    Console.WriteLine(controller);
                }                                                                    
                else
                    controllers[i].selected = false;                                                //And make sure all the others are not selected to prevent conflicts
            }
            
        }

        private Color GetPixelColor(int pixelNumber)            //Gets the current color of the buttons representing pixels
        {
            switch (pixelNumber){                               //Use a switch to select the corresponding pixel
                case 1:
                    return pixel1.BackColor;                    //Finally get the corresponging button to the target color
                case 2:
                    return pixel2.BackColor;
                case 3:
                    return pixel3.BackColor;
                case 4:
                    return pixel4.BackColor;
                case 5:
                    return pixel5.BackColor;
                case 6:
                    return pixel6.BackColor;
                case 7:
                    return pixel7.BackColor;
                case 8:
                    return pixel8.BackColor;
                case 9:
                    return pixel9.BackColor;
                case 10:
                    return pixel10.BackColor;
                default:
                    return Color.Black;                     //Return black if the index is out of bounds
            }
        }

        private Color ColorAverage(Color c1, Color c2)          //Mixes two colors together to get the "average"
        {
            int r1, g1, b1, r2, g2, b2, r3, g3, b3;             //Create temporary variables to store the RGB components
            r1 = c1.R;          //Decompose both colors into their RGB components
            g1 = c1.G;
            b1 = c1.B;
            r2 = c2.R;
            g2 = c2.G;
            b2 = c2.B;
            r3 = (r1 + r2) / 2;                                 //Average the RGB components
            g3 = (g1 + g2) / 2;
            b3 = (b1 + b2) / 2;
            return Color.FromArgb(r3, g3, b3);                  //Return the resulting color
        }

        private Color ColorMix(Color secondColor, Color mainColor, int mixAmount) //A custom function that takes a color, and fades it by a proportional amount of another color
        {
            int r1, g1, b1, r2, g2, b2; //Separate the RGB components of the colors to work with them 
            r1 = mainColor.R;
            g1 = mainColor.G;
            b1 = mainColor.B;
            r2 = secondColor.R;
            g2 = secondColor.G;
            b2 = secondColor.B;

            if (r1 != r2)       //Start with the red, if its the same for both colors, no need to do anyhitng
            {
                if (r1 > r2)        //If the red from the secondary color is greater than the red from the main color, reduce the red value by the specifed amount
                {
                    for (int i = 0; i < mixAmount; i++)
                    {
                        r1--;
                        if (r1 == r2) break;        //If the two colors end up matching before the cycle ends, get out of the cycle to prevent color distortions
                    }
                }
                else
                {
                    if (r1 < r2)        //If the red from the secondary color is lower than the red from the primary color, increase the red value by the specifed amount
                    {
                        for (int i = mixAmount; i > 0; i--)
                        {
                            r1++;
                            if (r1 == r2) break;
                        }
                    }
                }
            }
            ////////////////////////////////////////////////////////////////////////////
            if (g1 != g2)       //The exact same process aplies for the green component
            {
                if (g1 > g2)
                {
                    for (int i = 0; i < mixAmount; i++)
                    {
                        g1--;
                        if (g1 == g2) break;
                    }
                }
                else
                {
                    if (g1 < g2)
                    {
                        for (int i = mixAmount; i > 0; i--)
                        {
                            g1++;
                            if (g1 == g2) break;
                        }
                    }
                }
            }
            ////////////////////////////////////////////////////////////////////////////
            if (b1 != b2)           //The exact same process aplies for the blue component
            {
                if (b1 > b2)
                {
                    for (int i = 0; i < mixAmount; i++)
                    {
                        b1--;
                        if (b1 == b2) break;
                    }
                }
                else
                {
                    if (b1 < b2)
                    {
                        for (int i = mixAmount; i > 0; i--)
                        {
                            b1++;
                            if (b1 == b2) break;
                        }
                    }
                }
            }
            
            r1 = constrain(r1, 0, 255); //Make sure the RGB components are within the valid range --> 0-255
            g1 = constrain(g1, 0, 255);
            b1 = constrain(b1, 0, 255);
            return Color.FromArgb(r1, g1, b1); //return the resulting color
        }

        private void fadeAllPixels(Color x, int fadeby) //Uses the ColorMix function to fade all "pixel" buttons at once
        {
            pixel1.BackColor = ColorMix(x, pixel1.BackColor, fadeby);
            pixel2.BackColor = ColorMix(x, pixel2.BackColor, fadeby);
            pixel3.BackColor = ColorMix(x, pixel3.BackColor, fadeby);
            pixel4.BackColor = ColorMix(x, pixel4.BackColor, fadeby);
            pixel5.BackColor = ColorMix(x, pixel5.BackColor, fadeby);
            pixel6.BackColor = ColorMix(x, pixel6.BackColor, fadeby);
            pixel7.BackColor = ColorMix(x, pixel7.BackColor, fadeby);
            pixel8.BackColor = ColorMix(x, pixel8.BackColor, fadeby);
            pixel9.BackColor = ColorMix(x, pixel9.BackColor, fadeby);
            pixel10.BackColor = ColorMix(x, pixel10.BackColor, fadeby);
        }

        private int constrain(int input, int min, int max)  //Constrain the value of integer variables within the specified range 
        {
            if (input > max)
            {
                input = max;
            }
            if (input < min)
            {
                input = min;
            }
            return input;
        }

        private void xml_Corrupted(String prompt)   //Custom function called to address errors when attempting to read an XML file
        {
            MessageBox.Show(this,"The file containing user settings \"userInfo.xml\" " + prompt + ".\n" +
                                "The file has been overwritten with the default configuration", "Atention",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);      //Informs the user that there was something wrong with their stored data
            xmlDoc.LoadXml("<?xml version=\"1.0\"?> \n" +                               //Create a clean XML file with all default values
                              "<Info> \n" +
                              "  <Color index=\"0\" value=\"16777215\"></Color>\n" +    //Color nodes represent the user defined colors from the ColorSelect
                              "  <Color index=\"1\" value=\"16777215\"></Color>\n" +
                              "  <Color index=\"2\" value=\"16777215\"></Color>\n" +
                              "  <Color index=\"3\" value=\"16777215\"></Color>\n" +
                              "  <Color index=\"4\" value=\"16777215\"></Color>\n" +
                              "  <Color index=\"5\" value=\"16777215\"></Color>\n" +
                              "  <Color index=\"6\" value=\"16777215\"></Color>\n" +
                              "  <Color index=\"7\" value=\"16777215\"></Color>\n" +
                              "  <Color index=\"8\" value=\"16777215\"></Color>\n" +
                              "  <Color index=\"9\" value=\"16777215\"></Color>\n" +
                              "  <Color index=\"10\" value=\"16777215\"></Color>\n" +
                              "  <Color index=\"11\" value=\"16777215\"></Color>\n" +
                              "  <Color index=\"12\" value=\"16777215\"></Color>\n" +
                              "  <Color index=\"13\" value=\"16777215\"></Color>\n" +
                              "  <Color index=\"14\" value=\"16777215\"></Color>\n" +
                              "  <Color index=\"15\" value=\"16777215\"></Color>\n" +
                              "  <Default Pattern=\"Off\" ForeColor=\"0\" BackColor=\"0\" Order=\"0\" Rate=\"5\" Bounce=\"False\" />" + //The config node holds the default values loaded on startup
                              "</Info>");
            xmlDoc.Save("userInfo.xml");        //Save the XML file so that it can be accessed the next time the app is launched
        }

        private void safeAsync(BackgroundWorker worker, bool status)        //"Safely" start or stop a BG worker (prevent system.InvalidOperationException)
        {
            if (worker.IsBusy && status)        //If the worker is running and the code needs it running then do nothing
            {

            }
            if (worker.IsBusy && !status)       //If the worker is running and the code needs it stopped then stop it
            {
                worker.CancelAsync();
            }
            if (!worker.IsBusy && status)       //If the worker is not running and the code needs it running then do start it
            {
                worker.RunWorkerAsync();
            }
            if (!worker.IsBusy && !status)      //If the worker is not running and the code needs it stopped then do nothing
            {

            }
        }

        private void checkForLEDS_Button_Click(object sender, EventArgs e)      //This button executes the "Handshake" protocol to identify any controllers availble
        {
            ScanSerialDevices();                            //Make sure we can check all of the serial ports by filling the controllers array with serial ports
            Devices_ComboBox.Items.Clear();                 //Clear the box that enables the selection of controllers to prepare it for the incomming ones (if any)
            Devices_ComboBox.Enabled = false;
            Connect_Button.Text = "Scanning";
            Connect_Button.Enabled = false;
            foreach (SerialDevice item in controllers)      //Iterate through all of the elements of the controllers array to check if they contain a valid device
            {
                switch (item.Scan())         //The scan function actually performs all of the serial handshaking
                {
                    case "OK":              //If we get an "OK", then we know that serial port contains a valid device
                        Devices_ComboBox.Items.Add(item.controllerName);        //Add the device to the list of available controllers
                        break;
                }
            }
            Devices_ComboBox.Enabled = true;
            Connect_Button.Enabled = true;
            Connect_Button.Text = "Connect";
            setControls();              //Update all the UI elements to match the current controllers
            if (Devices_ComboBox.Items.Count > 0)           //If any controllers were found in the process
                Devices_ComboBox.SelectedIndex = 0;         //Make the selection box "highlight" the first one on the list

        }

        private void pixelSet_Button_Click(object sender, EventArgs e)
        {
            pixelColor1Change = true;
            safeAsync(LEDWritter, true);
        }

        private void pixelSetColor2_Button_Click(object sender, EventArgs e)
        {
            pixelColor2Change = true;
            safeAsync(LEDWritter, true);
        }

        private void Controller_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            safeAsync(graphics_Updater, false);
            controllers[controllerIndex].SafePortOpenClose(false);
            Thread.Sleep(20);
            int[] custom_colors = new int[16];
            Color_Select.CustomColors.CopyTo(custom_colors, 0);
            try
            {
                foreach (XmlNode node in xmlDoc.ChildNodes[1])
                {
                    if (node.Name == "Color")
                    {
                        int index;
                        index = Int32.Parse(node.Attributes["index"].Value);
                        node.Attributes["value"].Value = String.Concat(custom_colors[index]);
                        //Console.WriteLine("Saved user color " + index + " has a value of: " + custom_colors[index]);                        
                    }
                    if (node.Name == "Default")
                    {
                        node.Attributes["Pattern"].Value = effect;
                        node.Attributes["Rate"].Value = String.Concat(rate);
                        node.Attributes["ForeColor"].Value = String.Concat(foreColor.ToArgb());
                        node.Attributes["BackColor"].Value = String.Concat(backColor.ToArgb());
                        node.Attributes["Bounce"].Value = bouceEnabled.Checked.ToString();
                        node.Attributes["Order"].Value = String.Concat(colorOrder);
                    }
                }
                xmlDoc.Save("userInfo.xml");
            }
            catch (XmlException)
            {
                MessageBox.Show(this, "Error while atempting to save the custom color", "XML Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numericRateBox_ValueChanged(object sender, EventArgs e)
        {
            rate = (int) numericRateBox.Value;
            if (rate >= 50)
            {
                rate = 50;
            }
            if (rate <= 1)
            {
                rate = 1;
            }
            rate_TrackBar.Value = rate;
            rateChange = true;
            if (controllerAvailable)
            {
                safeAsync(LEDWritter, true);
            }
        }

        private void numericLEDNumber_ValueChanged(object sender, EventArgs e)
        {
            int number = (int) numericLEDNumber.Value;
            if (number > leds)
            {
                MessageBox.Show(this, "Your LED strip only has " + leds + " LEDs!", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                throw new ArgumentOutOfRangeException();
            }
            if (number <= 0)
            {
                MessageBox.Show(this, "The first pixel is 1, no zero or negative numbers allowed", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                throw new ArgumentOutOfRangeException();
            }
            pixel = number;
        }

        private void Default_Button_Click(object sender, EventArgs e)
        {
            if (controllerAvailable)
            {
                setDefaultChange = true;
                safeAsync(LEDWritter, true);
                
            }
            /*
            XmlElement newConfig = xmlDoc.CreateElement(controller);        //Create a new element on the root hierarchy of the root
            XmlAttribute PAT = newConfig.SetAttributeNode("Pattern", "");   //Create a new attribute of the element created in root to store one of the relevant variables.
            PAT.Value = effect;                                             //Set the value of the attribute accordinlgy 
            XmlAttribute RAT = newConfig.SetAttributeNode("Rate", "");
            RAT.Value = String.Concat(rate);
            xmlDoc.DocumentElement.AppendChild(newConfig);       //When all of the attributes are finally set, save they new element as a child node to the root of the tree of the XML file in memory
            */
            foreach (XmlNode node in xmlDoc.ChildNodes[1])
            {
                if (node.Name == "Default" )
                {
                    node.Attributes["Pattern"].Value = effect;
                    node.Attributes["Rate"].Value = String.Concat(rate);
                    node.Attributes["ForeColor"].Value = String.Concat(foreColor.ToArgb());
                    node.Attributes["BackColor"].Value = String.Concat(backColor.ToArgb());
                    node.Attributes["Bounce"].Value = bouceEnabled.Checked.ToString();
                    node.Attributes["Order"].Value = String.Concat(colorOrder);
                    xmlDoc.Save("userInfo.xml");
                }
                
            }

        }
    }

    public class SerialDevice
    {
        public SerialPort port;
        public string ledType, controllerName, version;
        public int leds;
        public bool selected, addressable;

        public void terminate()
        {
            SafePortOpenClose(false);
        }

        public String WriteSerial(string command)
        {
            try
            {
                SafePortOpenClose(true);
                port.Write(command);
                return port.ReadLine();
            }
            catch
            {
                return "Error";
            }
        }

        public void SafePortOpenClose(bool status)
        {
            try
            {
                if (port.IsOpen && status)
                {

                }
                if (port.IsOpen && !status)
                {
                    port.Close();
                }
                if (!port.IsOpen && !status)
                {

                }
                if (!port.IsOpen && status)
                {
                    port.Open();
                }
            }
            catch
            {

            }
        }

        public void Initilize(String portName)
        {
            port = new SerialPort(portName, 19200);
            port.ReadTimeout = 70;
            port.WriteTimeout = 70;
        }

        public String GetData()
        {
            return "0";
        }

        public string Scan()
        {
            try
            {
                SafePortOpenClose(true);
                String[] data = { "","","",""};
                for (int i = 0; i < 5; i++)                    
                {
                    port.Write("id");
                    data = port.ReadLine().Split(',');
                    if (data[1].Contains("addressable") || data[1].Contains("simple"))
                        break;
                    Thread.Sleep(50);
                }
                SafePortOpenClose(false);
                version = data[0];
                ledType = data[1];
                if (ledType == "addressable") addressable = true;
                controllerName = data[2];
                leds = Int32.Parse(data[3]);
                return "OK";
            }
            catch(TimeoutException)
            {
                return "Timeout";
            }
            catch (InvalidOperationException)
            {
                return "InvalidOperation";
            }catch (System.IO.IOException)
            {
                return "IOException";
            }
            catch (FormatException)
            {
                return "FormatException";
            }
        }
    }

    public class LEDPixel
    {
        int xPos, yPos;
        Color backColor;


    }
}