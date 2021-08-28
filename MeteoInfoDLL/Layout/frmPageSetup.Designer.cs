namespace MeteoInfoC.Layout
{
    partial class frmPageSetup
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
            this.RB_Landscape = new System.Windows.Forms.RadioButton();
            this.RB_Portrait = new System.Windows.Forms.RadioButton();
            this.Label1 = new System.Windows.Forms.Label();
            this.CB_PageSize = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TB_Width = new System.Windows.Forms.TextBox();
            this.CB_WidthUnit = new System.Windows.Forms.ComboBox();
            this.CB_HeightUnit = new System.Windows.Forms.ComboBox();
            this.TB_Height = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.B_Cancel = new System.Windows.Forms.Button();
            this.B_OK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RB_Landscape
            // 
            this.RB_Landscape.AutoSize = true;
            this.RB_Landscape.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RB_Landscape.Location = new System.Drawing.Point(213, 154);
            this.RB_Landscape.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RB_Landscape.Name = "RB_Landscape";
            this.RB_Landscape.Size = new System.Drawing.Size(100, 19);
            this.RB_Landscape.TabIndex = 2;
            this.RB_Landscape.TabStop = true;
            this.RB_Landscape.Text = "Landscape";
            this.RB_Landscape.UseVisualStyleBackColor = true;
            this.RB_Landscape.CheckedChanged += new System.EventHandler(this.RB_Landscape_CheckedChanged);
            // 
            // RB_Portrait
            // 
            this.RB_Portrait.AutoSize = true;
            this.RB_Portrait.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RB_Portrait.Location = new System.Drawing.Point(115, 154);
            this.RB_Portrait.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RB_Portrait.Name = "RB_Portrait";
            this.RB_Portrait.Size = new System.Drawing.Size(92, 19);
            this.RB_Portrait.TabIndex = 1;
            this.RB_Portrait.TabStop = true;
            this.RB_Portrait.Text = "Portrait";
            this.RB_Portrait.UseVisualStyleBackColor = true;
            this.RB_Portrait.CheckedChanged += new System.EventHandler(this.RB_Portrait_CheckedChanged);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Label1.Location = new System.Drawing.Point(23, 36);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(87, 15);
            this.Label1.TabIndex = 22;
            this.Label1.Text = "Page Size:";
            // 
            // CB_PageSize
            // 
            this.CB_PageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_PageSize.FormattingEnabled = true;
            this.CB_PageSize.Location = new System.Drawing.Point(118, 33);
            this.CB_PageSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CB_PageSize.Name = "CB_PageSize";
            this.CB_PageSize.Size = new System.Drawing.Size(195, 23);
            this.CB_PageSize.TabIndex = 21;
            this.CB_PageSize.SelectedIndexChanged += new System.EventHandler(this.CB_PageSize_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CB_HeightUnit);
            this.groupBox1.Controls.Add(this.TB_Height);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.CB_WidthUnit);
            this.groupBox1.Controls.Add(this.TB_Width);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.RB_Landscape);
            this.groupBox1.Controls.Add(this.RB_Portrait);
            this.groupBox1.Controls.Add(this.CB_PageSize);
            this.groupBox1.Controls.Add(this.Label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(336, 195);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Page";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(7, 156);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 15);
            this.label2.TabIndex = 23;
            this.label2.Text = "Orientation:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(55, 72);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 15);
            this.label3.TabIndex = 24;
            this.label3.Text = "Width:";
            // 
            // TB_Width
            // 
            this.TB_Width.Location = new System.Drawing.Point(118, 69);
            this.TB_Width.Name = "TB_Width";
            this.TB_Width.Size = new System.Drawing.Size(87, 25);
            this.TB_Width.TabIndex = 25;
            this.TB_Width.TextChanged += new System.EventHandler(this.TB_Width_TextChanged);
            // 
            // CB_WidthUnit
            // 
            this.CB_WidthUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_WidthUnit.FormattingEnabled = true;
            this.CB_WidthUnit.Location = new System.Drawing.Point(232, 71);
            this.CB_WidthUnit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CB_WidthUnit.Name = "CB_WidthUnit";
            this.CB_WidthUnit.Size = new System.Drawing.Size(81, 23);
            this.CB_WidthUnit.TabIndex = 26;
            // 
            // CB_HeightUnit
            // 
            this.CB_HeightUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_HeightUnit.FormattingEnabled = true;
            this.CB_HeightUnit.Location = new System.Drawing.Point(232, 108);
            this.CB_HeightUnit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CB_HeightUnit.Name = "CB_HeightUnit";
            this.CB_HeightUnit.Size = new System.Drawing.Size(81, 23);
            this.CB_HeightUnit.TabIndex = 29;
            // 
            // TB_Height
            // 
            this.TB_Height.Location = new System.Drawing.Point(118, 106);
            this.TB_Height.Name = "TB_Height";
            this.TB_Height.Size = new System.Drawing.Size(87, 25);
            this.TB_Height.TabIndex = 28;
            this.TB_Height.TextChanged += new System.EventHandler(this.TB_Height_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(47, 109);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 15);
            this.label4.TabIndex = 27;
            this.label4.Text = "Height:";
            // 
            // B_Cancel
            // 
            this.B_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.B_Cancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.B_Cancel.Location = new System.Drawing.Point(236, 229);
            this.B_Cancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Cancel.Name = "B_Cancel";
            this.B_Cancel.Size = new System.Drawing.Size(89, 27);
            this.B_Cancel.TabIndex = 28;
            this.B_Cancel.Text = "Cancel";
            this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
            // 
            // B_OK
            // 
            this.B_OK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.B_OK.Location = new System.Drawing.Point(139, 229);
            this.B_OK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_OK.Name = "B_OK";
            this.B_OK.Size = new System.Drawing.Size(89, 27);
            this.B_OK.TabIndex = 27;
            this.B_OK.Text = "OK";
            this.B_OK.Click += new System.EventHandler(this.B_OK_Click);
            // 
            // frmPageSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 272);
            this.Controls.Add(this.B_Cancel);
            this.Controls.Add(this.B_OK);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPageSetup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Page Setup";
            this.Load += new System.EventHandler(this.frmPageSetup_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton RB_Landscape;
        private System.Windows.Forms.RadioButton RB_Portrait;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.ComboBox CB_PageSize;
        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.ComboBox CB_HeightUnit;
        private System.Windows.Forms.TextBox TB_Height;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.ComboBox CB_WidthUnit;
        private System.Windows.Forms.TextBox TB_Width;
        internal System.Windows.Forms.Button B_Cancel;
        internal System.Windows.Forms.Button B_OK;
    }
}