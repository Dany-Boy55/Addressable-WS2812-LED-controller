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
            this.GetData_Button = new System.Windows.Forms.Button();
            this.ColorOrderBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.rate_TrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRateBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLEDNumber)).BeginInit();
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
            this.Color1_Button.Location = new System.Drawing.Point(13, 89);
            this.Color1_Button.Name = "Color1_Button";
            this.Color1_Button.Size = new System.Drawing.Size(198, 28);
            this.Color1_Button.TabIndex = 2;
            this.Color1_Button.Text = "Foreground Color";
            this.Color1_Button.UseVisualStyleBackColor = true;
            this.Color1_Button.Click += new System.EventHandler(this.Color1_Button_Click);
            // 
            // Color2_Button
            // 
            this.Color2_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Color2_Button.Location = new System.Drawing.Point(13, 123);
            this.Color2_Button.Name = "Color2_Button";
            this.Color2_Button.Size = new System.Drawing.Size(198, 28);
            this.Color2_Button.TabIndex = 3;
            this.Color2_Button.Text = "Background Color";
            this.Color2_Button.UseVisualStyleBackColor = true;
            this.Color2_Button.Click += new System.EventHandler(this.Color2_Button_Click);
            // 
            // pixel1
            // 
            this.pixel1.BackColor = System.Drawing.Color.Black;
            this.pixel1.Location = new System.Drawing.Point(13, 12);
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
            this.pixel2.Location = new System.Drawing.Point(49, 12);
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
            this.pixel3.Location = new System.Drawing.Point(85, 12);
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
            this.pixel4.Location = new System.Drawing.Point(121, 12);
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
            this.pixel6.Location = new System.Drawing.Point(193, 12);
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
            this.pixel7.Location = new System.Drawing.Point(229, 12);
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
            this.pixel8.Location = new System.Drawing.Point(265, 12);
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
            this.pixel9.Location = new System.Drawing.Point(301, 12);
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
            this.pixel10.Location = new System.Drawing.Point(337, 12);
            this.pixel10.Name = "pixel10";
            this.pixel10.Size = new System.Drawing.Size(30, 30);
            this.pixel10.TabIndex = 13;
            this.pixel10.TabStop = false;
            this.pixel10.UseVisualStyleBackColor = false;
            this.pixel10.Click += new System.EventHandler(this.pixel10_Click);
            // 
            // pattern_Select
            // 
            this.pattern_Select.BackColor = System.Drawing.SystemColors.ScrollBar;
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
            "Custom Pattern"});
            this.pattern_Select.Location = new System.Drawing.Point(13, 57);
            this.pattern_Select.Name = "pattern_Select";
            this.pattern_Select.Size = new System.Drawing.Size(198, 26);
            this.pattern_Select.TabIndex = 1;
            this.pattern_Select.Tag = "Control to select the LED lighting pattern";
            this.pattern_Select.SelectedIndexChanged += new System.EventHandler(this.pattern_Select_SelectedIndexChanged);
            // 
            // pixel5
            // 
            this.pixel5.BackColor = System.Drawing.Color.Black;
            this.pixel5.Location = new System.Drawing.Point(157, 12);
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
            this.bouceEnabled.Location = new System.Drawing.Point(387, 16);
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
            this.rate_TrackBar.Location = new System.Drawing.Point(12, 175);
            this.rate_TrackBar.Maximum = 50;
            this.rate_TrackBar.Minimum = 1;
            this.rate_TrackBar.Name = "rate_TrackBar";
            this.rate_TrackBar.Size = new System.Drawing.Size(355, 45);
            this.rate_TrackBar.TabIndex = 4;
            this.rate_TrackBar.Value = 25;
            this.rate_TrackBar.Scroll += new System.EventHandler(this.rate_TrackBar_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 18);
            this.label1.TabIndex = 18;
            this.label1.Text = "Rate:";
            // 
            // checkForLEDS_Button
            // 
            this.checkForLEDS_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkForLEDS_Button.Location = new System.Drawing.Point(15, 226);
            this.checkForLEDS_Button.Name = "checkForLEDS_Button";
            this.checkForLEDS_Button.Size = new System.Drawing.Size(136, 32);
            this.checkForLEDS_Button.TabIndex = 6;
            this.checkForLEDS_Button.Text = "Scan for LEDs";
            this.checkForLEDS_Button.UseVisualStyleBackColor = true;
            this.checkForLEDS_Button.Click += new System.EventHandler(this.checkForLEDS_Button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(226, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 18);
            this.label2.TabIndex = 25;
            this.label2.Text = "LED number:";
            // 
            // pixelSetColor1_Button
            // 
            this.pixelSetColor1_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pixelSetColor1_Button.Location = new System.Drawing.Point(229, 89);
            this.pixelSetColor1_Button.Name = "pixelSetColor1_Button";
            this.pixelSetColor1_Button.Size = new System.Drawing.Size(105, 29);
            this.pixelSetColor1_Button.TabIndex = 26;
            this.pixelSetColor1_Button.TabStop = false;
            this.pixelSetColor1_Button.Text = "Set Color 1";
            this.pixelSetColor1_Button.UseVisualStyleBackColor = true;
            this.pixelSetColor1_Button.Click += new System.EventHandler(this.pixelSet_Button_Click);
            // 
            // numericRateBox
            // 
            this.numericRateBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericRateBox.Location = new System.Drawing.Point(374, 175);
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
            this.numericLEDNumber.Location = new System.Drawing.Point(337, 55);
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
            this.pixelSetColor2_Button.Location = new System.Drawing.Point(352, 89);
            this.pixelSetColor2_Button.Name = "pixelSetColor2_Button";
            this.pixelSetColor2_Button.Size = new System.Drawing.Size(105, 29);
            this.pixelSetColor2_Button.TabIndex = 29;
            this.pixelSetColor2_Button.TabStop = false;
            this.pixelSetColor2_Button.Text = "Set Color 2";
            this.pixelSetColor2_Button.UseVisualStyleBackColor = true;
            this.pixelSetColor2_Button.Click += new System.EventHandler(this.pixelSetColor2_Button_Click);
            // 
            // Default_Button
            // 
            this.Default_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Default_Button.Location = new System.Drawing.Point(15, 264);
            this.Default_Button.Name = "Default_Button";
            this.Default_Button.Size = new System.Drawing.Size(136, 32);
            this.Default_Button.TabIndex = 7;
            this.Default_Button.Text = "Save as Default ";
            this.Default_Button.UseVisualStyleBackColor = true;
            this.Default_Button.Click += new System.EventHandler(this.Default_Button_Click);
            // 
            // Devices_ComboBox
            // 
            this.Devices_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Devices_ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.Devices_ComboBox.FormattingEnabled = true;
            this.Devices_ComboBox.Location = new System.Drawing.Point(167, 232);
            this.Devices_ComboBox.Name = "Devices_ComboBox";
            this.Devices_ComboBox.Size = new System.Drawing.Size(154, 26);
            this.Devices_ComboBox.TabIndex = 31;
            this.Devices_ComboBox.SelectedIndexChanged += new System.EventHandler(this.Devices_ComboBox_SelectedIndexChanged);
            // 
            // Connect_Button
            // 
            this.Connect_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Connect_Button.Location = new System.Drawing.Point(167, 264);
            this.Connect_Button.Name = "Connect_Button";
            this.Connect_Button.Size = new System.Drawing.Size(74, 32);
            this.Connect_Button.TabIndex = 32;
            this.Connect_Button.Text = "Connect";
            this.Connect_Button.UseVisualStyleBackColor = true;
            this.Connect_Button.Click += new System.EventHandler(this.Connect_Button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(164, 211);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 18);
            this.label3.TabIndex = 33;
            this.label3.Text = "Controller:";
            // 
            // GetData_Button
            // 
            this.GetData_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GetData_Button.Location = new System.Drawing.Point(247, 264);
            this.GetData_Button.Name = "GetData_Button";
            this.GetData_Button.Size = new System.Drawing.Size(74, 32);
            this.GetData_Button.TabIndex = 34;
            this.GetData_Button.Text = "Read";
            this.GetData_Button.UseVisualStyleBackColor = true;
            this.GetData_Button.Visible = false;
            this.GetData_Button.Click += new System.EventHandler(this.GetData_Button_Click);
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
            this.ColorOrderBox.Location = new System.Drawing.Point(323, 134);
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
            this.label4.Location = new System.Drawing.Point(226, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 18);
            this.label4.TabIndex = 36;
            this.label4.Text = "Color Order:";
            // 
            // Controller_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(477, 312);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ColorOrderBox);
            this.Controls.Add(this.GetData_Button);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
        private System.Windows.Forms.Button GetData_Button;
        private System.Windows.Forms.ComboBox ColorOrderBox;
        private System.Windows.Forms.Label label4;
    }
}

