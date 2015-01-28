namespace MeteoInfoC.Legend
{
    partial class frmLabelSymbolSet
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
            this.TB_Text = new System.Windows.Forms.TextBox();
            this.B_Color = new System.Windows.Forms.Button();
            this.B_Font = new System.Windows.Forms.Button();
            this.NUD_Angle = new System.Windows.Forms.NumericUpDown();
            this.Lab_Angle = new System.Windows.Forms.Label();
            this.B_OK = new System.Windows.Forms.Button();
            this.B_Apply = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Angle)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TB_Text
            // 
            this.TB_Text.Location = new System.Drawing.Point(12, 12);
            this.TB_Text.Multiline = true;
            this.TB_Text.Name = "TB_Text";
            this.TB_Text.Size = new System.Drawing.Size(260, 114);
            this.TB_Text.TabIndex = 0;
            this.TB_Text.Text = "Text";
            this.TB_Text.TextChanged += new System.EventHandler(this.TB_Text_TextChanged);
            // 
            // B_Color
            // 
            this.B_Color.Location = new System.Drawing.Point(91, 148);
            this.B_Color.Margin = new System.Windows.Forms.Padding(2);
            this.B_Color.Name = "B_Color";
            this.B_Color.Size = new System.Drawing.Size(59, 29);
            this.B_Color.TabIndex = 29;
            this.B_Color.Text = "Color";
            this.B_Color.UseVisualStyleBackColor = true;
            this.B_Color.Click += new System.EventHandler(this.B_Color_Click);
            // 
            // B_Font
            // 
            this.B_Font.Location = new System.Drawing.Point(12, 148);
            this.B_Font.Margin = new System.Windows.Forms.Padding(2);
            this.B_Font.Name = "B_Font";
            this.B_Font.Size = new System.Drawing.Size(59, 29);
            this.B_Font.TabIndex = 28;
            this.B_Font.Text = "Font";
            this.B_Font.UseVisualStyleBackColor = true;
            this.B_Font.Click += new System.EventHandler(this.B_Font_Click);
            // 
            // NUD_Angle
            // 
            this.NUD_Angle.DecimalPlaces = 1;
            this.NUD_Angle.Location = new System.Drawing.Point(213, 152);
            this.NUD_Angle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.NUD_Angle.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.NUD_Angle.Name = "NUD_Angle";
            this.NUD_Angle.Size = new System.Drawing.Size(53, 21);
            this.NUD_Angle.TabIndex = 39;
            this.NUD_Angle.ValueChanged += new System.EventHandler(this.NUD_Angle_ValueChanged);
            // 
            // Lab_Angle
            // 
            this.Lab_Angle.AutoSize = true;
            this.Lab_Angle.Location = new System.Drawing.Point(161, 24);
            this.Lab_Angle.Name = "Lab_Angle";
            this.Lab_Angle.Size = new System.Drawing.Size(41, 12);
            this.Lab_Angle.TabIndex = 38;
            this.Lab_Angle.Text = "Angle:";
            // 
            // B_OK
            // 
            this.B_OK.Location = new System.Drawing.Point(52, 204);
            this.B_OK.Name = "B_OK";
            this.B_OK.Size = new System.Drawing.Size(59, 29);
            this.B_OK.TabIndex = 41;
            this.B_OK.Text = "OK";
            this.B_OK.UseVisualStyleBackColor = true;
            this.B_OK.Click += new System.EventHandler(this.B_OK_Click);
            // 
            // B_Apply
            // 
            this.B_Apply.Location = new System.Drawing.Point(162, 204);
            this.B_Apply.Name = "B_Apply";
            this.B_Apply.Size = new System.Drawing.Size(59, 29);
            this.B_Apply.TabIndex = 40;
            this.B_Apply.Text = "Apply";
            this.B_Apply.UseVisualStyleBackColor = true;
            this.B_Apply.Click += new System.EventHandler(this.B_Apply_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Lab_Angle);
            this.groupBox1.Location = new System.Drawing.Point(7, 132);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(265, 57);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            // 
            // frmLabelSymbolSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 247);
            this.Controls.Add(this.B_OK);
            this.Controls.Add(this.B_Apply);
            this.Controls.Add(this.NUD_Angle);
            this.Controls.Add(this.B_Color);
            this.Controls.Add(this.B_Font);
            this.Controls.Add(this.TB_Text);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLabelSymbolSet";
            this.Text = "Label Symbol Set";
            this.Load += new System.EventHandler(this.frmLabelSymbolSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Angle)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TB_Text;
        internal System.Windows.Forms.Button B_Color;
        internal System.Windows.Forms.Button B_Font;
        private System.Windows.Forms.NumericUpDown NUD_Angle;
        private System.Windows.Forms.Label Lab_Angle;
        private System.Windows.Forms.Button B_OK;
        private System.Windows.Forms.Button B_Apply;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}