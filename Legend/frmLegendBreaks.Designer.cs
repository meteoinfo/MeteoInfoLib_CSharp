namespace MeteoInfoC.Legend
{
    partial class frmLegendBreaks
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
            this.lab_Max = new System.Windows.Forms.Label();
            this.lab_Min = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TB_SValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TB_EValue = new System.Windows.Forms.TextBox();
            this.TB_Interval = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.RB_RainbowColors = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Lab_EndColor = new System.Windows.Forms.Label();
            this.Lab_StartColor = new System.Windows.Forms.Label();
            this.RB_ShadeColors = new System.Windows.Forms.RadioButton();
            this.B_NewColor = new System.Windows.Forms.Button();
            this.B_NewLegend = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lab_Max
            // 
            this.lab_Max.AutoSize = true;
            this.lab_Max.Location = new System.Drawing.Point(175, 18);
            this.lab_Max.Name = "lab_Max";
            this.lab_Max.Size = new System.Drawing.Size(39, 15);
            this.lab_Max.TabIndex = 17;
            this.lab_Max.Text = "Max:";
            // 
            // lab_Min
            // 
            this.lab_Min.AutoSize = true;
            this.lab_Min.Location = new System.Drawing.Point(21, 18);
            this.lab_Min.Name = "lab_Min";
            this.lab_Min.Size = new System.Drawing.Size(39, 15);
            this.lab_Min.TabIndex = 16;
            this.lab_Min.Text = "Min:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 15);
            this.label1.TabIndex = 18;
            this.label1.Text = "Contouring:";
            // 
            // TB_SValue
            // 
            this.TB_SValue.Location = new System.Drawing.Point(122, 50);
            this.TB_SValue.Name = "TB_SValue";
            this.TB_SValue.Size = new System.Drawing.Size(77, 25);
            this.TB_SValue.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(205, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 15);
            this.label2.TabIndex = 20;
            this.label2.Text = "to";
            // 
            // TB_EValue
            // 
            this.TB_EValue.Location = new System.Drawing.Point(234, 50);
            this.TB_EValue.Name = "TB_EValue";
            this.TB_EValue.Size = new System.Drawing.Size(77, 25);
            this.TB_EValue.TabIndex = 21;
            // 
            // TB_Interval
            // 
            this.TB_Interval.Location = new System.Drawing.Point(122, 86);
            this.TB_Interval.Name = "TB_Interval";
            this.TB_Interval.Size = new System.Drawing.Size(77, 25);
            this.TB_Interval.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 15);
            this.label3.TabIndex = 22;
            this.label3.Text = "Interval:";
            // 
            // RB_RainbowColors
            // 
            this.RB_RainbowColors.AutoSize = true;
            this.RB_RainbowColors.Location = new System.Drawing.Point(12, 28);
            this.RB_RainbowColors.Name = "RB_RainbowColors";
            this.RB_RainbowColors.Size = new System.Drawing.Size(140, 19);
            this.RB_RainbowColors.TabIndex = 24;
            this.RB_RainbowColors.TabStop = true;
            this.RB_RainbowColors.Text = "Rainbow Colors";
            this.RB_RainbowColors.UseVisualStyleBackColor = true;
            this.RB_RainbowColors.CheckedChanged += new System.EventHandler(this.RB_RainbowColors_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Lab_EndColor);
            this.groupBox1.Controls.Add(this.Lab_StartColor);
            this.groupBox1.Controls.Add(this.RB_ShadeColors);
            this.groupBox1.Controls.Add(this.RB_RainbowColors);
            this.groupBox1.Location = new System.Drawing.Point(12, 125);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(322, 98);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Color Set";
            // 
            // Lab_EndColor
            // 
            this.Lab_EndColor.AutoSize = true;
            this.Lab_EndColor.BackColor = System.Drawing.Color.Red;
            this.Lab_EndColor.Location = new System.Drawing.Point(219, 64);
            this.Lab_EndColor.Name = "Lab_EndColor";
            this.Lab_EndColor.Size = new System.Drawing.Size(79, 15);
            this.Lab_EndColor.TabIndex = 27;
            this.Lab_EndColor.Text = "End Color";
            this.Lab_EndColor.Click += new System.EventHandler(this.Lab_EndColor_Click);
            // 
            // Lab_StartColor
            // 
            this.Lab_StartColor.AutoSize = true;
            this.Lab_StartColor.BackColor = System.Drawing.Color.LightYellow;
            this.Lab_StartColor.Location = new System.Drawing.Point(204, 32);
            this.Lab_StartColor.Name = "Lab_StartColor";
            this.Lab_StartColor.Size = new System.Drawing.Size(95, 15);
            this.Lab_StartColor.TabIndex = 26;
            this.Lab_StartColor.Text = "Start Color";
            this.Lab_StartColor.Click += new System.EventHandler(this.Lab_StartColor_Click);
            // 
            // RB_ShadeColors
            // 
            this.RB_ShadeColors.AutoSize = true;
            this.RB_ShadeColors.Location = new System.Drawing.Point(12, 62);
            this.RB_ShadeColors.Name = "RB_ShadeColors";
            this.RB_ShadeColors.Size = new System.Drawing.Size(124, 19);
            this.RB_ShadeColors.TabIndex = 25;
            this.RB_ShadeColors.TabStop = true;
            this.RB_ShadeColors.Text = "Shade Colors";
            this.RB_ShadeColors.UseVisualStyleBackColor = true;
            this.RB_ShadeColors.CheckedChanged += new System.EventHandler(this.RB_ShadeColors_CheckedChanged);
            // 
            // B_NewColor
            // 
            this.B_NewColor.Location = new System.Drawing.Point(193, 238);
            this.B_NewColor.Name = "B_NewColor";
            this.B_NewColor.Size = new System.Drawing.Size(102, 26);
            this.B_NewColor.TabIndex = 26;
            this.B_NewColor.Text = "New Colors";
            this.B_NewColor.UseVisualStyleBackColor = true;
            this.B_NewColor.Click += new System.EventHandler(this.B_NewColor_Click);
            // 
            // B_NewLegend
            // 
            this.B_NewLegend.Location = new System.Drawing.Point(60, 238);
            this.B_NewLegend.Name = "B_NewLegend";
            this.B_NewLegend.Size = new System.Drawing.Size(104, 26);
            this.B_NewLegend.TabIndex = 28;
            this.B_NewLegend.Text = "New Legend";
            this.B_NewLegend.UseVisualStyleBackColor = true;
            this.B_NewLegend.Click += new System.EventHandler(this.B_NewLegend_Click);
            // 
            // frmLegendBreaks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 276);
            this.Controls.Add(this.B_NewLegend);
            this.Controls.Add(this.B_NewColor);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.TB_Interval);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TB_EValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TB_SValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lab_Max);
            this.Controls.Add(this.lab_Min);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLegendBreaks";
            this.ShowInTaskbar = false;
            this.Text = "Legend Breaks";
            this.Load += new System.EventHandler(this.frmLegendBreaks_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lab_Max;
        private System.Windows.Forms.Label lab_Min;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TB_SValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TB_EValue;
        private System.Windows.Forms.TextBox TB_Interval;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton RB_RainbowColors;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RB_ShadeColors;
        internal System.Windows.Forms.Label Lab_EndColor;
        internal System.Windows.Forms.Label Lab_StartColor;
        private System.Windows.Forms.Button B_NewColor;
        private System.Windows.Forms.Button B_NewLegend;
    }
}