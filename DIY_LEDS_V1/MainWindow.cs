using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;             //Enables the use of sleep(), which even though I know should be avoided is simple and works
using System.Xml;                   //Enables the app to store and retrieve user settings 
using System.Timers;               //To have a semi-constant refresh-rate on the graphical side of things
using System.IO.Ports;              //For communicaation with the arduino through an emulated serial port


namespace DIY_LEDS_V1
{    
    public partial class Controller_Window : Form
    {
        System.Timers.Timer graphicRefresh;
        SerialPort arduino = new SerialPort("COM1", 19200);
        BackgroundWorker graphics_Updater = new BackgroundWorker();
        BackgroundWorker LEDWritter = new BackgroundWorker();
        XmlDocument xmlDoc = new XmlDocument();
        Color foreColor = Color.Black, backColor = Color.Black, calcColor;
        String command = "Off", portName;
        bool useNext = false, bounce = false, LEDSfound = false, addressable = false, pixelChange = false, SetDefaultChange = false;
        bool color1Change = false, color2Change = false, patternChange = false, rateChange = false;
        int rate = 5, hue = 0, framerate = 30, connectAtempt = 0, leds = 1, pixel = 1;
        
        public Controller_Window() //initializes all of the necesary elements for the app
        {
            InitializeComponent();
            pattern_Select.SelectedItem = "Off";            //set the default pattern to Off

            arduino.ErrorReceived += new SerialErrorReceivedEventHandler(arduino_ErrorReceived);    //subscribe to the serial error event handler in order to react accordingly to serial errors

            graphicRefresh = new System.Timers.Timer(1000 / framerate);
            graphicRefresh.AutoReset = true;
            graphicRefresh.Enabled = true;
            graphicRefresh.Elapsed += new ElapsedEventHandler(RefreshPixels);       //start and subscirbe to a timer in order to have the graphical elements refresh periodically

            graphics_Updater.WorkerSupportsCancellation = true;
            graphics_Updater.WorkerReportsProgress = true;
            graphics_Updater.DoWork += new DoWorkEventHandler(graphics_Updater_DoWork);
            graphics_Updater.ProgressChanged += new ProgressChangedEventHandler(graphics_Updater_ProgressChanged);
            graphics_Updater.RunWorkerCompleted += new RunWorkerCompletedEventHandler(graphics_Updater_Completed); //Start and subscribe to a background worker that handles all necesary graphical calculations 

            LEDWritter.WorkerSupportsCancellation = true;
            LEDWritter.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LEDWritter_Completed);
            LEDWritter.DoWork += new DoWorkEventHandler(LEDWritter_DoWork);       //create and susbscribe to a background worker to handle asynncronous serial comunication to provide a seamless experience

            try{                                                //try to retrieve the use defined colors at the start of the app
                xmlDoc.Load("userInfo.xml");               //load the XML file containing the user colors
                int[] custom_colors = new int[16];                  //array to temporarily store and handle the user colors
                foreach(XmlNode node in xmlDoc.ChildNodes[1])       //loop that reads the XML color value and stores it in the array
                {
                    if (node.Name == "Color")
                    {
                        int index, value;
                        index = Int32.Parse(node.Attributes["index"].Value);
                        value = Int32.Parse(node.Attributes["value"].Value);
                        //Console.WriteLine("Loaded user color " + index + " has a value of: " + value);    //Used for debugging
                        custom_colors[index] = value;
                    }
                    else
                    {
                        if(node.Name == "Config" && node.Attributes["Name"].Value == "Default")
                        {
                            rate = Int32.Parse(node.Attributes["Rate"].Value);
                            rate_TrackBar.Value = rate;
                            numericRateBox.Value = (decimal)rate;
                            foreColor = Color.FromArgb(Int32.Parse(node.Attributes["ForeColor"].Value));
                            backColor = Color.FromArgb(Int32.Parse(node.Attributes["BackColor"].Value));
                            Color1_Button.BackColor = foreColor;
                            Color2_Button.BackColor = backColor;
                            pattern_Select.SelectedItem = node.Attributes["Pattern"].Value;
                            if (node.Attributes["Bounce"].Value == "True")
                                bouceEnabled.Checked = true;
                            
                        }
                    }
                    
                }
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

        void arduino_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)   //A function triggered with the serial error event
        {
            //safePortOpenClose(arduino, false);        //if the serial communication goes wrong, close the correspoding port
            //LEDSfound = false;                        //Tell the rest of the app that the LEDS are no longer there
            //MessageBox.Show(this, "A Serial error " + e.ToString() + " has ocurred");   //And finally tell the user something went wrong
        }

        private void Color1_Button_Click(object sender, EventArgs e)   //enables the user to select any 24 bit color
        {
            Color_Select.Color = foreColor;                           
            if (Color_Select.ShowDialog() == DialogResult.OK)        //Only sets the new color value if the user selects ok
            {                
                foreColor = Color_Select.Color;                     //Save the new color to a global variable to be used in other sections of the code
                Color1_Button.BackColor = foreColor;                //Set the button to the selected color
                color1Change = true;                                //Tell the serial writter that there is a Color1 change to be updated to the LEDS
                if (LEDSfound)
                {
                    safeAsync(LEDWritter, true);                    //if the LEDS are there, write the new changes asynchronously
                }                
            }
            
        }

        private void Color2_Button_Click(object sender, EventArgs e)    //enables the user to select any 24 bit color (same as the above one)
        {
            Color_Select.Color = backColor;
            if (Color_Select.ShowDialog() == DialogResult.OK)
            {                
                backColor = Color_Select.Color;
                Color2_Button.BackColor = backColor;
                color2Change = true;
                if (LEDSfound)
                {
                    safeAsync(LEDWritter, true);
                }
            }
        }

        private void Controller_Window_Load(object sender, EventArgs e)
        {

        }

        private void pattern_Select_SelectedIndexChanged(object sender, EventArgs e)    //Tanke appropiate action when a new pattern is selected by the user
        {
            command = pattern_Select.SelectedItem.ToString();
            patternChange = true;
            Thread.Sleep(10);
            if (LEDSfound)
            {
                safeAsync(LEDWritter, true);
            }            
            switch(command){
                case "Off":
                    safeAsync(graphics_Updater, false);
                    break;
                case "Fixed Color":
                    safeAsync(graphics_Updater, false);
                    break;
                case "Color Fade":
                    safeAsync(graphics_Updater, true);
                    break;
                case "Color Chase":
                    if (LEDSfound && !addressable)
                    {
                        MessageBox.Show(this, "Please note that this effect can only be displayed by addressable LEDS", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    safeAsync(graphics_Updater, true);
                    break;
                case "Color Switching":
                    if (LEDSfound && !addressable)
                    {
                        MessageBox.Show(this, "Please note that this effect can only be displayed by addressable LEDS", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    safeAsync(graphics_Updater, true);
                    break;
                case "Color Breathe":
                    safeAsync(graphics_Updater, true);
                    break;
                case "Color Cycle":
                    safeAsync(graphics_Updater, true);
                    break;
                case "Rainbow":
                    if (LEDSfound && !addressable)
                    {
                        MessageBox.Show(this, "Please note that this effect can only be displayed by addressable LEDS", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    safeAsync(graphics_Updater, true);
                    break;
                case "Rainbow Chase":
                    if (LEDSfound && !addressable)
                    {
                        MessageBox.Show(this, "Please note that this effect can only be displayed by addressable LEDS", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    safeAsync(graphics_Updater, true);
                    break;
                case "Rainbow Breathe":
                    if (LEDSfound && !addressable)
                    {
                        MessageBox.Show(this, "Please note that this effect can only be displayed by addressable LEDS", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    safeAsync(graphics_Updater, true);
                    break;
                case "Custom Pattern":
                    if (LEDSfound && !addressable)
                    {
                        MessageBox.Show(this, "Please note that this effect can only be displayed by addressable LEDS", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    safeAsync(graphics_Updater, false);
                    break;
                default:
                    safeAsync(graphics_Updater, false);
                    command = "Off";
                    break;
            }
            setControls();
        } 
        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void rate_TrackBar_Scroll(object sender, EventArgs e)
        {
            rateChange = true;
            rate = rate_TrackBar.Value;
            numericRateBox.Value = (decimal) rate;
            if (LEDSfound)
            {
                safeAsync(LEDWritter, true);
            }
        }

        private void bouceEnabled_CheckedChanged(object sender, EventArgs e)
        {
            bounce = bouceEnabled.Checked;
            patternChange = true;
            if (LEDSfound)
            {
                safeAsync(LEDWritter, true);
            }
        }

        private void pixel1_Click(object sender, EventArgs e)
        {
            if (command == "Custom Pattern")
            {
                Color_Select.Color =  pixel1.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel1.BackColor = Color_Select.Color;
                    pixel = 1;
                    pixelChange = true;
                    if (LEDSfound)
                    {
                        safeAsync(LEDWritter, true);
                    }
                }
            }
        }

        private void pixel2_Click(object sender, EventArgs e)
        {
            if (command == "Custom Pattern")
            {
                Color_Select.Color = pixel2.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel2.BackColor = Color_Select.Color;
                    pixel = 2;
                    pixelChange = true;
                    if (LEDSfound)
                    {
                        safeAsync(LEDWritter, true);
                    }
                }
            }
        }

        private void pixel3_Click(object sender, EventArgs e)
        {
            if (command == "Custom Pattern")
            {
                Color_Select.Color = pixel3.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel3.BackColor = Color_Select.Color;
                    pixel = 3;
                    pixelChange = true;
                    if (LEDSfound)
                    {
                        safeAsync(LEDWritter, true);
                    }
                }
            }
        }

        private void pixel4_Click(object sender, EventArgs e)
        {            
            if (command == "Custom Pattern")
            {
                Color_Select.Color = pixel4.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel4.BackColor = Color_Select.Color;
                    pixel = 4;
                    pixelChange = true;
                    if (LEDSfound)
                    {
                        safeAsync(LEDWritter, true);
                    }
                }
            }
        }

        private void pixel5_Click_1(object sender, EventArgs e)
        {
            if (command == "Custom Pattern")
            {
                Color_Select.Color = pixel5.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel5.BackColor = Color_Select.Color;
                    pixel = 5;
                    pixelChange = true;
                    if (LEDSfound)
                    {
                        safeAsync(LEDWritter, true);
                    }
                }
            }
        }

        private void pixel6_Click(object sender, EventArgs e)
        {
            if (command == "Custom Pattern")
            {
                Color_Select.Color = pixel6.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel6.BackColor = Color_Select.Color;
                    pixel = 6;
                    pixelChange = true;
                    if (LEDSfound)
                    {
                        safeAsync(LEDWritter, true);
                    }
                }
            }
        }

        private void pixel7_Click(object sender, EventArgs e)
        {
            if (command == "Custom Pattern")
            {
                Color_Select.Color = pixel7.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel7.BackColor = Color_Select.Color;
                    pixel = 7;
                    pixelChange = true;
                    if (LEDSfound)
                    {
                        safeAsync(LEDWritter, true);
                    }
                }
            }
        }

        private void pixel8_Click(object sender, EventArgs e)
        {
            if (command == "Custom Pattern")
            {
                Color_Select.Color = pixel8.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel8.BackColor = Color_Select.Color;
                    pixel = 8;
                    pixelChange = true;
                    if (LEDSfound)
                    {
                        safeAsync(LEDWritter, true);
                    }
                }
            }
        }

        private void pixel9_Click(object sender, EventArgs e)
        {
            if (command == "Custom Pattern")
            {
                Color_Select.Color = pixel9.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel9.BackColor = Color_Select.Color;
                    pixel = 9;
                    pixelChange = true;
                    if (LEDSfound)
                    {
                        safeAsync(LEDWritter, true);
                    }
                }
            }
        }

        private void pixel10_Click(object sender, EventArgs e)
        {
            if (command == "Custom Pattern")
            {
                Color_Select.Color = pixel10.BackColor;
                if (Color_Select.ShowDialog() == DialogResult.OK)
                {
                    pixel10.BackColor = Color_Select.Color;
                    pixel = 10;
                    pixelChange = true;
                    if (LEDSfound)
                    {
                        safeAsync(LEDWritter, true);
                    }
                }
            }
        }

        private void graphics_Updater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int p = e.ProgressPercentage;
            if (command.Contains("Breathe"))
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
                if (command.Contains("Rainbow"))
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
            if (command.Contains("Chase"))
            {
                int pixel = constrain(e.ProgressPercentage, 1, 10);
                if (command.Contains("Rainbow"))
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
            if (command.Contains("Fade"))
            {
                int fadeAmount = (int) (p * 2.55);
                Color temp = ColorMix(backColor, foreColor, fadeAmount);
                SetAllPixels(temp);
            }
            ///////////////////////////////////////////////////////////////
            if (command.Contains("Switching"))
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
        }

        private void graphics_Updater_Completed(object sender, RunWorkerCompletedEventArgs e)
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

        private void graphics_Updater_DoWork(object sender, DoWorkEventArgs e)
        {
            ///////////////////////////////////////////////////////////////
            if (command.Contains("Chase") || command.Contains("Switching"))
            {
                for (int i = 1; i <= 10; i++)
                {
                    if (graphics_Updater.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    graphics_Updater.ReportProgress(i);
                    Thread.Sleep((rate + 2) * 9);
                }
                useNext = !useNext;
                if (bounce)
                {
                    for (int i = 10; i > 0; i--)
                    {
                        if (graphics_Updater.CancellationPending)
                        {
                            e.Cancel = true;
                            break;
                        }
                        graphics_Updater.ReportProgress(i);
                        Thread.Sleep((rate + 2) * 9);
                    }
                    useNext = !useNext;
                }
                
            }
            ////////////////////////////////////////////////////////////////////
            if (command.Contains("Breathe") || command.Contains("Fade"))
            {
                for (int i = 1; i < 100; i++)
                {
                    if (graphics_Updater.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    graphics_Updater.ReportProgress(i);
                    Thread.Sleep(rate*2 + 2);
                }
                for (int i = 100; i > 1; i--)
                {
                    if (graphics_Updater.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    graphics_Updater.ReportProgress(i);
                    Thread.Sleep(rate*2 + 2);
                }
                useNext = !useNext;
            }
            ///////////////////////////////////////////////////////
            if (command == "Rainbow" || command == "Color Cycle")
            {
                if (graphics_Updater.CancellationPending)
                {
                    e.Cancel = true;
                }
                hue++;
                Thread.Sleep(rate * 3);
            }
        }

        private void LEDWritter_DoWork(object sender, DoWorkEventArgs e)
        {
            if(LEDWritter.CancellationPending){
                e.Cancel = true;
            }
            else
            {
                if (pixelChange)
                {
                    WriteSerialCommand(arduino, "Pixel," + (pixel - 1) + "," + GetPixelColor(pixel).R + "," + GetPixelColor(pixel).G + "," + GetPixelColor(pixel).B);
                    Console.WriteLine("Pixel," + (pixel - 1) + "," + GetPixelColor(pixel).R + "," + GetPixelColor(pixel).G + "," + GetPixelColor(pixel).B);
                    pixelChange = false;
                    Thread.Sleep(50);
                }
                if (patternChange)
                {
                    
                    Console.WriteLine(WriteSerialCommand(arduino, command));
                    if(bounce){
                        WriteSerialCommand(arduino, " Bounce");
                    }
                    patternChange = false;
                    Thread.Sleep(100);
                }
                if (color1Change)
                {
                    String color = "Color1," + foreColor.R + "," + foreColor.G + "," + foreColor.B;
                    Console.WriteLine(WriteSerialCommand(arduino, color));
                    color1Change = false;
                    Thread.Sleep(100);
                }
                if (color2Change)
                {
                    String color = "Color2," + backColor.R + "," + backColor.G + "," + backColor.B;
                    Console.WriteLine(WriteSerialCommand(arduino, color));
                    color2Change = false;
                    Thread.Sleep(100);
                }
                if (rateChange)
                {
                    String r = "Rate," + rate;
                    Console.WriteLine(WriteSerialCommand(arduino, r));
                    rateChange = false;
                    Thread.Sleep(100);
                }
                if (SetDefaultChange)
                {
                    Console.WriteLine(WriteSerialCommand(arduino, "Save Default"));
                    SetDefaultChange = false;
                    Thread.Sleep(100);
                }
            }
        }

        private void LEDWritter_Completed(object sender, AsyncCompletedEventArgs e)
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
                //Console.WriteLine("*****Graphics updater worker has completed its task!*****");
            }
        }

        private void RefreshPixels(object sender, ElapsedEventArgs e)
        {
            if (hue > 360)
            {
                hue = 0;
            }
            switch (command)
            {
                case "Off":
                    fadeAllPixels(Color.Black,16);
                    break;
                case "Fixed Color":
                    SetAllPixels(foreColor);
                    break;
                case "Color Breathe":
                    SetAllPixels(calcColor);
                    break;
                case "Color Chase":
                    fadeAllPixels(backColor, (20 - rate) * 2);
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
            }
        }

        private void setControls()
        {
            Color1_Button.Hide();
            Color2_Button.Hide();
            bouceEnabled.Hide();
            numericRateBox.Hide();
            rate_TrackBar.Hide();
            numericLEDNumber.Hide();
            label1.Hide();
            label2.Hide();
            pixelSetColor1_Button.Hide();
            pixelSetColor2_Button.Hide();

            if ((command.Contains("Color") || command.Contains("Pattern")) && command != "Color Cycle")
            {
                Color1_Button.Show();
            }
            if ((command.Contains("Breathe") || command.Contains("Chase") || command.Contains("Switching")) || command.Contains("Fade") && command != "Rainbow Breathe")
            {
                Color2_Button.Show();
            }
            if (command.Contains("Rainbow") || command.Contains("Breathe") || command.Contains("Chase") || command.Contains("Cycle") || command.Contains("Fade") || command.Contains("Switching"))
            {
                numericRateBox.Show();
                rate_TrackBar.Show();
                label1.Show();
            }
            if(command.Contains("Chase") || command.Contains("Switch"))
            {
                bouceEnabled.Show();
            }
            if (command == "Custom Pattern")
            {
                numericLEDNumber.Show();
                label2.Show();
                pixelSetColor1_Button.Show();
                pixelSetColor2_Button.Show();
                if (LEDSfound)
                {
                    numericLEDNumber.Enabled = true;
                    pixelSetColor1_Button.Enabled = true;
                    pixelSetColor2_Button.Enabled = false;
                }
                else
                {
                    numericLEDNumber.Enabled = false;
                    pixelSetColor1_Button.Enabled = false;
                    pixelSetColor2_Button.Enabled = false;
                }
            }
        }        

        private Color ColorFromHSV(double hue, double saturation, double value)
        {
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

        private void SetAllPixels(Color x)
        {
            pixel1.BackColor = x;
            pixel2.BackColor = x;
            pixel3.BackColor = x;
            pixel4.BackColor = x;
            pixel5.BackColor = x;
            pixel6.BackColor = x;
            pixel7.BackColor = x;
            pixel8.BackColor = x;
            pixel9.BackColor = x;
            pixel10.BackColor = x;
        }

        private void RainbowPixels(double initialhue, int dispersion)
        {
            pixel1.BackColor = ColorFromHSV(initialhue, 1 , 1);
            pixel2.BackColor = ColorFromHSV(initialhue + dispersion, 1, 1);
            pixel3.BackColor = ColorFromHSV(initialhue + dispersion * 2, 1, 1);
            pixel4.BackColor = ColorFromHSV(initialhue + dispersion * 3, 1, 1);
            pixel5.BackColor = ColorFromHSV(initialhue + dispersion * 4, 1, 1);
            pixel6.BackColor = ColorFromHSV(initialhue + dispersion * 5, 1, 1);
            pixel7.BackColor = ColorFromHSV(initialhue + dispersion * 6, 1, 1);
            pixel8.BackColor = ColorFromHSV(initialhue + dispersion * 7, 1, 1);
            pixel9.BackColor = ColorFromHSV(initialhue + dispersion * 8, 1, 1);
            pixel10.BackColor = ColorFromHSV(initialhue + dispersion * 9, 1, 1);
        }

        private void setPixel(int pixel_Number,Color x)
        {
            if(pixel_Number >=1 && pixel_Number <=10)
            {
                switch (pixel_Number)
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
            else
            {
                throw new ArgumentOutOfRangeException("Pixel number must be between 1 and 10", "original");
            }
        }

        private Color GetPixelColor(int pixelNumber)
        {
            switch (pixelNumber){
                case 1:
                    return pixel1.BackColor;
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
                    return Color.Black;
            }
        }

        private Color ColorAverage(Color c1, Color c2)
        {
            int r1, g1, b1, r2, g2, b2, r3, g3, b3;
            r1 = c1.R;
            g1 = c1.G;
            b1 = c1.B;
            r2 = c2.R;
            g2 = c2.G;
            b2 = c2.B;
            r3 = (r1 + r2) / 2;
            g3 = (g1 + g2) / 2;
            b3 = (b1 + b2) / 2;
            return Color.FromArgb(r3, g3, b3);
        }

        private Color ColorMix(Color secondColor, Color mainColor, int mixAmount)
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

        private void fadeAllPixels(Color x, int fadeby)
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

        private int constrain(int input, int min, int max)
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

        private void xml_Corrupted(String prompt)
        {
            MessageBox.Show(this,"The file containing user settings \"userInfo.xml\" " + prompt + ".\n" +
                                "The file has been overwritten with the default configuration", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            xmlDoc.LoadXml("<?xml version=\"1.0\"?> \n" +
                              "<Info> \n" +
                              "  <Color index=\"0\" value=\"16777215\"></Color>\n" +
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
                              "  <Config Name=\"Default\" Pattern=\"Off\" ForeColor=\"0\" BackColor=\"0\" Rate=\"5\" Bounce=\"False\"></Config>>" +
                              "</Info>");
            xmlDoc.Save("userInfo.xml");
        }

        private void safeAsync(BackgroundWorker worker, bool status)
        {
            if (worker.IsBusy && status)
            {

            }
            if (worker.IsBusy && !status)
            {
                worker.CancelAsync();
            }
            if (!worker.IsBusy && status)
            {
                worker.RunWorkerAsync();
            }
            if (!worker.IsBusy && !status)
            {

            }
        }

        private void checkForLEDS_Button_Click(object sender, EventArgs e)
        {
            if(!LEDSfound){
                String message = "";
                foreach (String port in SerialPort.GetPortNames())
                {
                    safePortOpenClose(arduino, false);
                    arduino.PortName = port;
                    try
                    {
                        safePortOpenClose(arduino, true);
                        arduino.Write("id");
                        arduino.ReadTimeout = 300;
                        message = arduino.ReadLine();
                    }
                    catch (TimeoutException)
                    {
                        safePortOpenClose(arduino, false);
                    }
                    catch (System.IO.IOException)
                    {
                        MessageBox.Show(this,"IO error when trying to access port " + port, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        safePortOpenClose(arduino, false);
                    }
                    catch(UnauthorizedAccessException)
                    {
                        MessageBox.Show(this, "Could not access port " + port + ", access denied", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        safePortOpenClose(arduino, false);
                    }
                    if (message.Contains("WS281"))
                    {
                        int index  = message.IndexOf(":") + 1;
                        leds = Int32.Parse(message.Remove(0, index));
                        LEDSfound = true;
                        portName = port;
                        addressable = true;
                        break;
                    }
                    else
                    {
                        if (message.Contains("Simple RGB"))
                        {
                            LEDSfound = true;
                            portName = port;
                            addressable = false;
                            break;
                        }
                    }
                }
                if (LEDSfound && addressable)
                {
                    MessageBox.Show(this,"Success! " + leds + " Addressable LEDS found in port " + portName, "LED scan result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    checkForLEDS_Button.Text = "Close Connection";
                    LEDWritter.RunWorkerAsync();
                }
                else
                {
                    if (LEDSfound && !addressable)
                    {
                        MessageBox.Show(this, "Success! Simple RGB LEDS found in port" + portName, "LED scan result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        checkForLEDS_Button.Text = "Close Connection";
                        LEDWritter.RunWorkerAsync();
                    }
                    else
                    {
                        if(connectAtempt < 2){
                            MessageBox.Show(this,"LEDS not found. Try again","LED scan result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            connectAtempt++;
                        }else{
                            if(DialogResult.Yes == MessageBox.Show(this,"LEDS not found. Are you sure everything is configured propperly?\n"
                                + "Check for latest arudino code, correct drivers and propper electrical connections\n"
                                + "Do you want to open the instructables page for reference?", "LED scan result:", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                            {
                                System.Diagnostics.Process.Start("http://www.google.com");
                            }
                        }                        
                    }
                }
            }
            else
            {
                connectAtempt = 0;
                safePortOpenClose(arduino, false);
                LEDSfound = false;
                MessageBox.Show(this, "Connection terminated", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Information);
                checkForLEDS_Button.Text = "Scan for LEDS";
            }
            setControls();
        }

        private void safePortOpenClose(SerialPort port, bool status)
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

        private String WriteSerialCommand(SerialPort port,String text){
            try{
                safePortOpenClose(port,true);
                port.Write(text);
                //Console.WriteLine(text);
                String received = null;
                port.ReadTimeout = 100;

                received = port.ReadLine();
                if (received.Contains("OK"))
                {
                    return received;
                }
                else
                {
                    return "No confirmation";
                }
            }catch(TimeoutException)
            {
                return "Timeout";
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show(this, "The LED controller could not be reached. Make sure the harware is properly set up", "Hardware Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                LEDSfound = false;
                safePortOpenClose(arduino, false);
                return "IO exception";
            }catch(UnauthorizedAccessException){
                return "No Access";
            }
        }
        
        private void pixelSet_Button_Click(object sender, EventArgs e)
        {
            if (arduino.IsOpen && addressable)
            {
                WriteSerialCommand(arduino, "Pixel," + (pixel - 1) + "," + foreColor.R + "," + foreColor.G + "," + foreColor.B);
            }
        }

        private void pixelSetColor2_Button_Click(object sender, EventArgs e)
        {
            if (arduino.IsOpen && addressable)
            {
                WriteSerialCommand(arduino, "Pixel," + (pixel - 1) + "," + backColor.R + "," + backColor.G + "," + backColor.B);
            }
        }

        private void Controller_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            safeAsync(graphics_Updater, false);
            safePortOpenClose(arduino, false);
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
            if (rate >= 20)
            {
                rate = 20;
            }
            if (rate <= 1)
            {
                rate = 1;
            }
            rate_TrackBar.Value = rate;
            rateChange = true;
            if (LEDSfound)
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
            SetDefaultChange = true;
            foreach (XmlNode node in xmlDoc.ChildNodes[1])
            {
                if (node.Name == "Config")
                {
                    node.Attributes["Pattern"].Value = String.Concat(pattern_Select.SelectedItem);
                    node.Attributes["Rate"].Value = String.Concat(rate);
                    node.Attributes["ForeColor"].Value = String.Concat(foreColor.ToArgb());
                    node.Attributes["BackColor"].Value = String.Concat(backColor.ToArgb());
                    node.Attributes["Bounce"].Value = bouceEnabled.Checked.ToString();
                    xmlDoc.Save("userInfo.xml");
                }
                if (LEDSfound)
                {
                    safeAsync(LEDWritter, true);
                }
            }
        }
    }
}