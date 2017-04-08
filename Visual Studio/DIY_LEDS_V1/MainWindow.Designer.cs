namespace DIY_LEDS_V1
{
    partial class Controller_Window
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Controller_Window));
            this.Color_Select = new System.Windows.Forms.ColorDialog();
            this.Color1_Button = new System.Windows.Forms.Button();
            this.Color2_Button = new System.Windows.Forms.Button();
            this.pixel1 = new System.Windows.Forms.Button();
            this.pixel2 = new System.Windows.Forms.Button();
            this.pixel3 = new System.Windows.Forms.Button();
            this.pixel4 = new System.Windows.Forms.Button();
            this.pixel6 = new System.Windows.Forms.Button();
            this.pixel7 = new System.Windows.Forms.Button();
            this.pixel8 = new System.Windows.Forms.Button();
            this.pixel9 = new System.Windows.Forms.Button();
            this.pixel10 = new System.Windows.Forms.Button();
            this.pattern_Select = new System.Windows.Forms.ComboBox();
            this.pixel5 = new System.Windows.Forms.Button();
            this.bouceEnabled = new System.Windows.Forms.CheckBox();
            this.rate_TrackBar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.checkForLEDS_Button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pixelSetColor1_Button = new System.Windows.Forms.Button();
            this.numericRateBox = new System.Windows.Forms.NumericUpDown();
            this.numericLEDNumber = new System.Windows.Forms.NumericUpDown();
            this.pixelSetColor2_Button = new System.Windows.Forms.Button();
            this.Default_Button = new System.Windows.Forms.Button();
            this.Devices_ComboBox = new System.Windows.Forms.ComboBox();
            this.Connect_Button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ColorOrderBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.HWtimer = new System.Windows.Forms.Timer(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runInBackgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label11 = new System.Windows.Forms.Label();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reactiveEffectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cPUTemperatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cPUUtilizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gPUTemperatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gPUUtilizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label13 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.rate_TrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRateBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLEDNumber)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Color_Select
            // 
            this.Color_Select.AnyColor = true;
            this.Color_Select.FullOpen = true;
            // 
            // Color1_Button
            // 
            this.Color1_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Color1_Button.Location = new System.Drawing.Point(12, 104);
            this.Color1_Button.Name = "Color1_Button";
            this.Color1_Button.Size = new System.Drawing.Size(198, 28);
            this.Color1_Button.TabIndex = 2;
            this.Color1_Button.Text = "Foreground Color";
            this.Color1_Button.UseVisualStyleBackColor = false;
            this.Color1_Button.Click += new System.EventHandler(this.Color1_Button_Click);
            // 
            // Color2_Button
            // 
            this.Color2_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Color2_Button.Location = new System.Drawing.Point(12, 138);
            this.Color2_Button.Name = "Color2_Button";
            this.Color2_Button.Size = new System.Drawing.Size(198, 28);
            this.Color2_Button.TabIndex = 3;
            this.Color2_Button.Text = "Background Color";
            this.Color2_Button.UseVisualStyleBackColor = false;
            this.Color2_Button.Click += new System.EventHandler(this.Color2_Button_Click);
            // 
            // pixel1
            // 
            this.pixel1.BackColor = System.Drawing.Color.Black;
            this.pixel1.Location = new System.Drawing.Point(12, 27);
            this.pixel1.Name = "pixel1";
            this.pixel1.Size = new System.Drawing.Size(30, 30);
            this.pixel1.TabIndex = 50;
            this.pixel1.TabStop = false;
            this.pixel1.UseVisualStyleBackColor = false;
            this.pixel1.Click += new System.EventHandler(this.pixel1_Click);
            // 
            // pixel2
            // 
            this.pixel2.BackColor = System.Drawing.Color.Black;
            this.pixel2.Location = new System.Drawing.Point(48, 27);
            this.pixel2.Name = "pixel2";
            this.pixel2.Size = new System.Drawing.Size(30, 30);
            this.pixel2.TabIndex = 4;
            this.pixel2.TabStop = false;
            this.pixel2.UseVisualStyleBackColor = false;
            this.pixel2.Click += new System.EventHandler(this.pixel2_Click);
            // 
            // pixel3
            // 
            this.pixel3.BackColor = System.Drawing.Color.Black;
            this.pixel3.Location = new System.Drawing.Point(84, 27);
            this.pixel3.Name = "pixel3";
            this.pixel3.Size = new System.Drawing.Size(30, 30);
            this.pixel3.TabIndex = 5;
            this.pixel3.TabStop = false;
            this.pixel3.UseVisualStyleBackColor = false;
            this.pixel3.Click += new System.EventHandler(this.pixel3_Click);
            // 
            // pixel4
            // 
            this.pixel4.BackColor = System.Drawing.Color.Black;
            this.pixel4.Location = new System.Drawing.Point(120, 27);
            this.pixel4.Name = "pixel4";
            this.pixel4.Size = new System.Drawing.Size(30, 30);
            this.pixel4.TabIndex = 6;
            this.pixel4.TabStop = false;
            this.pixel4.UseVisualStyleBackColor = false;
            this.pixel4.Click += new System.EventHandler(this.pixel4_Click);
            // 
            // pixel6
            // 
            this.pixel6.BackColor = System.Drawing.Color.Black;
            this.pixel6.Location = new System.Drawing.Point(192, 27);
            this.pixel6.Name = "pixel6";
            this.pixel6.Size = new System.Drawing.Size(30, 30);
            this.pixel6.TabIndex = 8;
            this.pixel6.TabStop = false;
            this.pixel6.UseVisualStyleBackColor = false;
            this.pixel6.Click += new System.EventHandler(this.pixel6_Click);
            // 
            // pixel7
            // 
            this.pixel7.BackColor = System.Drawing.Color.Black;
            this.pixel7.Location = new System.Drawing.Point(228, 27);
            this.pixel7.Name = "pixel7";
            this.pixel7.Size = new System.Drawing.Size(30, 30);
            this.pixel7.TabIndex = 9;
            this.pixel7.TabStop = false;
            this.pixel7.UseVisualStyleBackColor = false;
            this.pixel7.Click += new System.EventHandler(this.pixel7_Click);
            // 
            // pixel8
            // 
            this.pixel8.BackColor = System.Drawing.Color.Black;
            this.pixel8.Location = new System.Drawing.Point(264, 27);
            this.pixel8.Name = "pixel8";
            this.pixel8.Size = new System.Drawing.Size(30, 30);
            this.pixel8.TabIndex = 11;
            this.pixel8.TabStop = false;
            this.pixel8.UseVisualStyleBackColor = false;
            this.pixel8.Click += new System.EventHandler(this.pixel8_Click);
            // 
            // pixel9
            // 
            this.pixel9.BackColor = System.Drawing.Color.Black;
            this.pixel9.Location = new System.Drawing.Point(300, 27);
            this.pixel9.Name = "pixel9";
            this.pixel9.Size = new System.Drawing.Size(30, 30);
            this.pixel9.TabIndex = 12;
            this.pixel9.TabStop = false;
            this.pixel9.UseVisualStyleBackColor = false;
            this.pixel9.Click += new System.EventHandler(this.pixel9_Click);
            // 
            // pixel10
            // 
            this.pixel10.BackColor = System.Drawing.Color.Black;
            this.pixel10.Location = new System.Drawing.Point(336, 27);
            this.pixel10.Name = "pixel10";
            this.pixel10.Size = new System.Drawing.Size(30, 30);
            this.pixel10.TabIndex = 13;
            this.pixel10.TabStop = false;
            this.pixel10.UseVisualStyleBackColor = false;
            this.pixel10.Click += new System.EventHandler(this.pixel10_Click);
            // 
            // pattern_Select
            // 
            this.pattern_Select.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pattern_Select.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pattern_Select.FormattingEnabled = true;
            this.pattern_Select.Items.AddRange(new object[] {
            "Off",
            "Fixed Color",
            "Color Fade",
            "Color Breathe",
            "Color Chase",
            "Color Cycle",
            "Color Switching",
            "Color Rain",
            "Color Gradient",
            "Rainbow",
            "Rainbow Breathe",
            "Rainbow Chase",
            "CPU temperature",
            "CPU utilization",
            "GPU temperature",
            "GPU utilization",
            "Custom Pattern"});
            this.pattern_Select.Location = new System.Drawing.Point(12, 72);
            this.pattern_Select.Name = "pattern_Select";
            this.pattern_Select.Size = new System.Drawing.Size(198, 26);
            this.pattern_Select.TabIndex = 1;
            this.pattern_Select.Tag = "Control to select the LED lighting pattern";
            this.pattern_Select.SelectedIndexChanged += new System.EventHandler(this.pattern_Select_SelectedIndexChanged);
            // 
            // pixel5
            // 
            this.pixel5.BackColor = System.Drawing.Color.Black;
            this.pixel5.Location = new System.Drawing.Point(156, 27);
            this.pixel5.Name = "pixel5";
            this.pixel5.Size = new System.Drawing.Size(30, 30);
            this.pixel5.TabIndex = 15;
            this.pixel5.TabStop = false;
            this.pixel5.UseVisualStyleBackColor = false;
            this.pixel5.Click += new System.EventHandler(this.pixel5_Click);
            // 
            // bouceEnabled
            // 
            this.bouceEnabled.AutoSize = true;
            this.bouceEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bouceEnabled.Location = new System.Drawing.Point(397, 31);
            this.bouceEnabled.Name = "bouceEnabled";
            this.bouceEnabled.Size = new System.Drawing.Size(78, 22);
            this.bouceEnabled.TabIndex = 16;
            this.bouceEnabled.TabStop = false;
            this.bouceEnabled.Text = "Bounce";
            this.bouceEnabled.UseVisualStyleBackColor = true;
            this.bouceEnabled.CheckedChanged += new System.EventHandler(this.bouceEnabled_CheckedChanged);
            // 
            // rate_TrackBar
            // 
            this.rate_TrackBar.Location = new System.Drawing.Point(11, 190);
            this.rate_TrackBar.Maximum = 50;
            this.rate_TrackBar.Minimum = 1;
            this.rate_TrackBar.Name = "rate_TrackBar";
            this.rate_TrackBar.Size = new System.Drawing.Size(380, 45);
            this.rate_TrackBar.TabIndex = 4;
            this.rate_TrackBar.Value = 25;
            this.rate_TrackBar.Scroll += new System.EventHandler(this.rate_TrackBar_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 169);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 18);
            this.label1.TabIndex = 18;
            this.label1.Text = "Rate:";
            // 
            // checkForLEDS_Button
            // 
            this.checkForLEDS_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkForLEDS_Button.Location = new System.Drawing.Point(14, 241);
            this.checkForLEDS_Button.Name = "checkForLEDS_Button";
            this.checkForLEDS_Button.Size = new System.Drawing.Size(136, 32);
            this.checkForLEDS_Button.TabIndex = 6;
            this.checkForLEDS_Button.Text = "Scan for LEDs";
            this.checkForLEDS_Button.UseVisualStyleBackColor = false;
            this.checkForLEDS_Button.Click += new System.EventHandler(this.checkForLEDS_Button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(239, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 18);
            this.label2.TabIndex = 25;
            this.label2.Text = "LED number:";
            // 
            // pixelSetColor1_Button
            // 
            this.pixelSetColor1_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pixelSetColor1_Button.Location = new System.Drawing.Point(242, 104);
            this.pixelSetColor1_Button.Name = "pixelSetColor1_Button";
            this.pixelSetColor1_Button.Size = new System.Drawing.Size(105, 29);
            this.pixelSetColor1_Button.TabIndex = 26;
            this.pixelSetColor1_Button.TabStop = false;
            this.pixelSetColor1_Button.Text = "Set Color 1";
            this.pixelSetColor1_Button.UseVisualStyleBackColor = false;
            this.pixelSetColor1_Button.Click += new System.EventHandler(this.pixelSet_Button_Click);
            // 
            // numericRateBox
            // 
            this.numericRateBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericRateBox.Location = new System.Drawing.Point(397, 190);
            this.numericRateBox.Name = "numericRateBox";
            this.numericRateBox.Size = new System.Drawing.Size(83, 24);
            this.numericRateBox.TabIndex = 5;
            this.numericRateBox.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericRateBox.ValueChanged += new System.EventHandler(this.numericRateBox_ValueChanged);
            // 
            // numericLEDNumber
            // 
            this.numericLEDNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericLEDNumber.Location = new System.Drawing.Point(360, 70);
            this.numericLEDNumber.Name = "numericLEDNumber";
            this.numericLEDNumber.Size = new System.Drawing.Size(120, 24);
            this.numericLEDNumber.TabIndex = 28;
            this.numericLEDNumber.TabStop = false;
            this.numericLEDNumber.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericLEDNumber.ValueChanged += new System.EventHandler(this.numericLEDNumber_ValueChanged);
            // 
            // pixelSetColor2_Button
            // 
            this.pixelSetColor2_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pixelSetColor2_Button.Location = new System.Drawing.Point(375, 104);
            this.pixelSetColor2_Button.Name = "pixelSetColor2_Button";
            this.pixelSetColor2_Button.Size = new System.Drawing.Size(105, 29);
            this.pixelSetColor2_Button.TabIndex = 29;
            this.pixelSetColor2_Button.TabStop = false;
            this.pixelSetColor2_Button.Text = "Set Color 2";
            this.pixelSetColor2_Button.UseVisualStyleBackColor = false;
            this.pixelSetColor2_Button.Click += new System.EventHandler(this.pixelSetColor2_Button_Click);
            // 
            // Default_Button
            // 
            this.Default_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Default_Button.Location = new System.Drawing.Point(14, 279);
            this.Default_Button.Name = "Default_Button";
            this.Default_Button.Size = new System.Drawing.Size(136, 32);
            this.Default_Button.TabIndex = 7;
            this.Default_Button.Text = "Save as Default ";
            this.Default_Button.UseVisualStyleBackColor = false;
            this.Default_Button.Click += new System.EventHandler(this.Default_Button_Click);
            // 
            // Devices_ComboBox
            // 
            this.Devices_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Devices_ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Devices_ComboBox.FormattingEnabled = true;
            this.Devices_ComboBox.Location = new System.Drawing.Point(239, 244);
            this.Devices_ComboBox.Name = "Devices_ComboBox";
            this.Devices_ComboBox.Size = new System.Drawing.Size(127, 28);
            this.Devices_ComboBox.TabIndex = 31;
            this.Devices_ComboBox.SelectedIndexChanged += new System.EventHandler(this.Devices_ComboBox_SelectedIndexChanged);
            // 
            // Connect_Button
            // 
            this.Connect_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Connect_Button.Location = new System.Drawing.Point(377, 244);
            this.Connect_Button.Name = "Connect_Button";
            this.Connect_Button.Size = new System.Drawing.Size(103, 32);
            this.Connect_Button.TabIndex = 32;
            this.Connect_Button.Text = "Connect";
            this.Connect_Button.UseVisualStyleBackColor = false;
            this.Connect_Button.Click += new System.EventHandler(this.Connect_Button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(156, 248);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 20);
            this.label3.TabIndex = 33;
            this.label3.Text = "Controller:";
            // 
            // ColorOrderBox
            // 
            this.ColorOrderBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColorOrderBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.ColorOrderBox.FormattingEnabled = true;
            this.ColorOrderBox.Items.AddRange(new object[] {
            "Red, Green, Blue",
            "Red, Blue, Green",
            "Green, Red, Blue",
            "Green, Blue, Red",
            "Blue, Red, Green",
            "Blue, Green, Red"});
            this.ColorOrderBox.Location = new System.Drawing.Point(342, 149);
            this.ColorOrderBox.Name = "ColorOrderBox";
            this.ColorOrderBox.Size = new System.Drawing.Size(138, 26);
            this.ColorOrderBox.TabIndex = 35;
            this.ColorOrderBox.TabStop = false;
            this.ColorOrderBox.SelectedIndexChanged += new System.EventHandler(this.ColorOrderBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(239, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 18);
            this.label4.TabIndex = 36;
            this.label4.Text = "Color Order:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.label5.Location = new System.Drawing.Point(14, 314);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(142, 18);
            this.label5.TabIndex = 51;
            this.label5.Text = "CPU not recognized";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.label6.Location = new System.Drawing.Point(14, 332);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 18);
            this.label6.TabIndex = 52;
            this.label6.Text = "CPU temperature NA";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.label7.Location = new System.Drawing.Point(14, 350);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(143, 18);
            this.label7.TabIndex = 53;
            this.label7.Text = "GPU not recognized";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.label8.Location = new System.Drawing.Point(14, 368);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(148, 18);
            this.label8.TabIndex = 54;
            this.label8.Text = "GPU temperature NA";
            // 
            // HWtimer
            // 
            this.HWtimer.Enabled = true;
            this.HWtimer.Interval = 200;
            this.HWtimer.Tick += new System.EventHandler(this.HWtimer_Tick);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.label9.Location = new System.Drawing.Point(214, 332);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(132, 18);
            this.label9.TabIndex = 55;
            this.label9.Text = "CPU Utilization NA";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.label10.Location = new System.Drawing.Point(214, 368);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(133, 18);
            this.label10.TabIndex = 56;
            this.label10.Text = "GPU Utilization NA";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(489, 24);
            this.menuStrip1.TabIndex = 57;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runInBackgroundToolStripMenuItem,
            this.alwaysOnTopToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.viewToolStripMenuItem.Text = "Options";
            // 
            // runInBackgroundToolStripMenuItem
            // 
            this.runInBackgroundToolStripMenuItem.Name = "runInBackgroundToolStripMenuItem";
            this.runInBackgroundToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.runInBackgroundToolStripMenuItem.Text = "Run in background";
            this.runInBackgroundToolStripMenuItem.Click += new System.EventHandler(this.runInBackgroundToolStripMenuItem_Click);
            // 
            // alwaysOnTopToolStripMenuItem
            // 
            this.alwaysOnTopToolStripMenuItem.Name = "alwaysOnTopToolStripMenuItem";
            this.alwaysOnTopToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.alwaysOnTopToolStripMenuItem.Text = "Always on top";
            this.alwaysOnTopToolStripMenuItem.Click += new System.EventHandler(this.alwaysOnTopToolStripMenuItem_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(157, 279);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(0, 18);
            this.label11.TabIndex = 58;
            // 
            // TrayIcon
            // 
            this.TrayIcon.ContextMenuStrip = this.contextMenuStrip1;
            this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
            this.TrayIcon.Text = "LED controller";
            this.TrayIcon.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideToolStripMenuItem,
            this.showToolStripMenuItem,
            this.reactiveEffectToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(152, 92);
            // 
            // hideToolStripMenuItem
            // 
            this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            this.hideToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.hideToolStripMenuItem.Text = "Hide";
            this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // reactiveEffectToolStripMenuItem
            // 
            this.reactiveEffectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cPUTemperatureToolStripMenuItem,
            this.cPUUtilizationToolStripMenuItem,
            this.gPUTemperatureToolStripMenuItem,
            this.gPUUtilizationToolStripMenuItem});
            this.reactiveEffectToolStripMenuItem.Name = "reactiveEffectToolStripMenuItem";
            this.reactiveEffectToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.reactiveEffectToolStripMenuItem.Text = "Reactive effect";
            this.reactiveEffectToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.reactiveEffectToolStripMenuItem_DropDownItemClicked);
            // 
            // cPUTemperatureToolStripMenuItem
            // 
            this.cPUTemperatureToolStripMenuItem.Name = "cPUTemperatureToolStripMenuItem";
            this.cPUTemperatureToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.cPUTemperatureToolStripMenuItem.Text = "CPU temperature";
            // 
            // cPUUtilizationToolStripMenuItem
            // 
            this.cPUUtilizationToolStripMenuItem.Name = "cPUUtilizationToolStripMenuItem";
            this.cPUUtilizationToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.cPUUtilizationToolStripMenuItem.Text = "CPU utilization";
            // 
            // gPUTemperatureToolStripMenuItem
            // 
            this.gPUTemperatureToolStripMenuItem.Name = "gPUTemperatureToolStripMenuItem";
            this.gPUTemperatureToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.gPUTemperatureToolStripMenuItem.Text = "GPU temperature";
            // 
            // gPUUtilizationToolStripMenuItem
            // 
            this.gPUUtilizationToolStripMenuItem.Name = "gPUUtilizationToolStripMenuItem";
            this.gPUUtilizationToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.gPUUtilizationToolStripMenuItem.Text = "GPU utilization";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label13.Location = new System.Drawing.Point(156, 285);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 20);
            this.label13.TabIndex = 60;
            this.label13.Text = "Details";
            // 
            // Controller_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 392);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ColorOrderBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Connect_Button);
            this.Controls.Add(this.Devices_ComboBox);
            this.Controls.Add(this.Default_Button);
            this.Controls.Add(this.pixelSetColor2_Button);
            this.Controls.Add(this.numericLEDNumber);
            this.Controls.Add(this.numericRateBox);
            this.Controls.Add(this.pixelSetColor1_Button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkForLEDS_Button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rate_TrackBar);
            this.Controls.Add(this.bouceEnabled);
            this.Controls.Add(this.pixel5);
            this.Controls.Add(this.pattern_Select);
            this.Controls.Add(this.pixel10);
            this.Controls.Add(this.pixel9);
            this.Controls.Add(this.pixel8);
            this.Controls.Add(this.pixel7);
            this.Controls.Add(this.pixel6);
            this.Controls.Add(this.pixel4);
            this.Controls.Add(this.pixel3);
            this.Controls.Add(this.pixel2);
            this.Controls.Add(this.pixel1);
            this.Controls.Add(this.Color2_Button);
            this.Controls.Add(this.Color1_Button);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Controller_Window";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LED Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Controller_Window_FormClosing);
            this.Load += new System.EventHandler(this.Controller_Window_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rate_TrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRateBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLEDNumber)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog Color_Select;
        private System.Windows.Forms.Button Color1_Button;
        private System.Windows.Forms.Button Color2_Button;
        private System.Windows.Forms.Button pixel1;
        private System.Windows.Forms.Button pixel2;
        private System.Windows.Forms.Button pixel3;
        private System.Windows.Forms.Button pixel4;
        private System.Windows.Forms.Button pixel6;
        private System.Windows.Forms.Button pixel7;
        private System.Windows.Forms.Button pixel8;
        private System.Windows.Forms.Button pixel9;
        private System.Windows.Forms.Button pixel10;
        private System.Windows.Forms.ComboBox pattern_Select;
        private System.Windows.Forms.Button pixel5;
        private System.Windows.Forms.CheckBox bouceEnabled;
        private System.Windows.Forms.TrackBar rate_TrackBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button checkForLEDS_Button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button pixelSetColor1_Button;
        private System.Windows.Forms.NumericUpDown numericRateBox;
        private System.Windows.Forms.NumericUpDown numericLEDNumber;
        private System.Windows.Forms.Button pixelSetColor2_Button;
        private System.Windows.Forms.Button Default_Button;
        private System.Windows.Forms.ComboBox Devices_ComboBox;
        private System.Windows.Forms.Button Connect_Button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ColorOrderBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer HWtimer;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runInBackgroundToolStripMenuItem;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ToolStripMenuItem alwaysOnTopToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reactiveEffectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cPUTemperatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cPUUtilizationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gPUTemperatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gPUUtilizationToolStripMenuItem;
        private System.Windows.Forms.Label label13;
    }
}

