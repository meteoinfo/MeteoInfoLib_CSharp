namespace MeteoInfoC.Legend
{
    partial class frmPointSymbolSet
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
            this.CB_MarkerType = new System.Windows.Forms.ComboBox();
            this.Lab_MarkerType = new System.Windows.Forms.Label();
            this.Lab_FillColor = new System.Windows.Forms.Label();
            this.B_FillColor = new System.Windows.Forms.Button();
            this.Lab_Size = new System.Windows.Forms.Label();
            this.NUD_Size = new System.Windows.Forms.NumericUpDown();
            this.GB_Outline = new System.Windows.Forms.GroupBox();
            this.B_OutlineColor = new System.Windows.Forms.Button();
            this.Lab_OutlineColor = new System.Windows.Forms.Label();
            this.ChB_DrawOutline = new System.Windows.Forms.CheckBox();
            this.Lab_FontFamily = new System.Windows.Forms.Label();
            this.CB_FontFamily = new System.Windows.Forms.ComboBox();
            this.ChB_DrawShape = new System.Windows.Forms.CheckBox();
            this.B_OK = new System.Windows.Forms.Button();
            this.B_Apply = new System.Windows.Forms.Button();
            this.Lab_Angle = new System.Windows.Forms.Label();
            this.NUD_Angle = new System.Windows.Forms.NumericUpDown();
            this.ChB_DrawFill = new System.Windows.Forms.CheckBox();
            this.markerControl1 = new MeteoInfoC.Legend.SymbolControl();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Size)).BeginInit();
            this.GB_Outline.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Angle)).BeginInit();
            this.SuspendLayout();
            // 
            // CB_MarkerType
            // 
            this.CB_MarkerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_MarkerType.FormattingEnabled = true;
            this.CB_MarkerType.Location = new System.Drawing.Point(112, 12);
            this.CB_MarkerType.Name = "CB_MarkerType";
            this.CB_MarkerType.Size = new System.Drawing.Size(130, 20);
            this.CB_MarkerType.TabIndex = 0;
            this.CB_MarkerType.SelectedIndexChanged += new System.EventHandler(this.CB_MarkerType_SelectedIndexChanged);
            // 
            // Lab_MarkerType
            // 
            this.Lab_MarkerType.AutoSize = true;
            this.Lab_MarkerType.Location = new System.Drawing.Point(29, 15);
            this.Lab_MarkerType.Name = "Lab_MarkerType";
            this.Lab_MarkerType.Size = new System.Drawing.Size(77, 12);
            this.Lab_MarkerType.TabIndex = 1;
            this.Lab_MarkerType.Text = "Marker Type:";
            // 
            // Lab_FillColor
            // 
            this.Lab_FillColor.AutoSize = true;
            this.Lab_FillColor.Location = new System.Drawing.Point(147, 345);
            this.Lab_FillColor.Name = "Lab_FillColor";
            this.Lab_FillColor.Size = new System.Drawing.Size(71, 12);
            this.Lab_FillColor.TabIndex = 24;
            this.Lab_FillColor.Text = "Fill Color:";
            // 
            // B_FillColor
            // 
            this.B_FillColor.Location = new System.Drawing.Point(224, 340);
            this.B_FillColor.Name = "B_FillColor";
            this.B_FillColor.Size = new System.Drawing.Size(57, 23);
            this.B_FillColor.TabIndex = 25;
            this.B_FillColor.UseVisualStyleBackColor = true;
            this.B_FillColor.Click += new System.EventHandler(this.B_FillColor_Click);
            // 
            // Lab_Size
            // 
            this.Lab_Size.AutoSize = true;
            this.Lab_Size.Location = new System.Drawing.Point(17, 332);
            this.Lab_Size.Name = "Lab_Size";
            this.Lab_Size.Size = new System.Drawing.Size(35, 12);
            this.Lab_Size.TabIndex = 26;
            this.Lab_Size.Text = "Size:";
            // 
            // NUD_Size
            // 
            this.NUD_Size.DecimalPlaces = 1;
            this.NUD_Size.Location = new System.Drawing.Point(58, 330);
            this.NUD_Size.Name = "NUD_Size";
            this.NUD_Size.Size = new System.Drawing.Size(59, 21);
            this.NUD_Size.TabIndex = 27;
            this.NUD_Size.ValueChanged += new System.EventHandler(this.NUD_Size_ValueChanged);
            // 
            // GB_Outline
            // 
            this.GB_Outline.Controls.Add(this.B_OutlineColor);
            this.GB_Outline.Controls.Add(this.Lab_OutlineColor);
            this.GB_Outline.Controls.Add(this.ChB_DrawOutline);
            this.GB_Outline.Location = new System.Drawing.Point(16, 363);
            this.GB_Outline.Name = "GB_Outline";
            this.GB_Outline.Size = new System.Drawing.Size(265, 51);
            this.GB_Outline.TabIndex = 28;
            this.GB_Outline.TabStop = false;
            this.GB_Outline.Text = "Outline";
            // 
            // B_OutlineColor
            // 
            this.B_OutlineColor.Location = new System.Drawing.Point(202, 18);
            this.B_OutlineColor.Name = "B_OutlineColor";
            this.B_OutlineColor.Size = new System.Drawing.Size(57, 23);
            this.B_OutlineColor.TabIndex = 26;
            this.B_OutlineColor.UseVisualStyleBackColor = true;
            this.B_OutlineColor.Click += new System.EventHandler(this.B_OutlineColor_Click);
            // 
            // Lab_OutlineColor
            // 
            this.Lab_OutlineColor.AutoSize = true;
            this.Lab_OutlineColor.Location = new System.Drawing.Point(157, 24);
            this.Lab_OutlineColor.Name = "Lab_OutlineColor";
            this.Lab_OutlineColor.Size = new System.Drawing.Size(41, 12);
            this.Lab_OutlineColor.TabIndex = 1;
            this.Lab_OutlineColor.Text = "Color:";
            // 
            // ChB_DrawOutline
            // 
            this.ChB_DrawOutline.AutoSize = true;
            this.ChB_DrawOutline.Location = new System.Drawing.Point(20, 22);
            this.ChB_DrawOutline.Name = "ChB_DrawOutline";
            this.ChB_DrawOutline.Size = new System.Drawing.Size(96, 16);
            this.ChB_DrawOutline.TabIndex = 0;
            this.ChB_DrawOutline.Text = "Draw Outline";
            this.ChB_DrawOutline.UseVisualStyleBackColor = true;
            this.ChB_DrawOutline.CheckedChanged += new System.EventHandler(this.ChB_DrawOutline_CheckedChanged);
            // 
            // Lab_FontFamily
            // 
            this.Lab_FontFamily.AutoSize = true;
            this.Lab_FontFamily.Location = new System.Drawing.Point(14, 45);
            this.Lab_FontFamily.Name = "Lab_FontFamily";
            this.Lab_FontFamily.Size = new System.Drawing.Size(77, 12);
            this.Lab_FontFamily.TabIndex = 30;
            this.Lab_FontFamily.Text = "Font Family:";
            // 
            // CB_FontFamily
            // 
            this.CB_FontFamily.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_FontFamily.FormattingEnabled = true;
            this.CB_FontFamily.Location = new System.Drawing.Point(97, 42);
            this.CB_FontFamily.Name = "CB_FontFamily";
            this.CB_FontFamily.Size = new System.Drawing.Size(145, 20);
            this.CB_FontFamily.TabIndex = 29;
            this.CB_FontFamily.SelectedIndexChanged += new System.EventHandler(this.CB_FontFamily_SelectedIndexChanged);
            // 
            // ChB_DrawShape
            // 
            this.ChB_DrawShape.AutoSize = true;
            this.ChB_DrawShape.Location = new System.Drawing.Point(175, 288);
            this.ChB_DrawShape.Name = "ChB_DrawShape";
            this.ChB_DrawShape.Size = new System.Drawing.Size(84, 16);
            this.ChB_DrawShape.TabIndex = 32;
            this.ChB_DrawShape.Text = "Draw Shape";
            this.ChB_DrawShape.UseVisualStyleBackColor = true;
            this.ChB_DrawShape.CheckedChanged += new System.EventHandler(this.ChB_DrawShape_CheckedChanged);
            // 
            // B_OK
            // 
            this.B_OK.Location = new System.Drawing.Point(57, 425);
            this.B_OK.Name = "B_OK";
            this.B_OK.Size = new System.Drawing.Size(59, 29);
            this.B_OK.TabIndex = 34;
            this.B_OK.Text = "OK";
            this.B_OK.UseVisualStyleBackColor = true;
            this.B_OK.Click += new System.EventHandler(this.B_OK_Click);
            // 
            // B_Apply
            // 
            this.B_Apply.Location = new System.Drawing.Point(167, 425);
            this.B_Apply.Name = "B_Apply";
            this.B_Apply.Size = new System.Drawing.Size(59, 29);
            this.B_Apply.TabIndex = 33;
            this.B_Apply.Text = "Apply";
            this.B_Apply.UseVisualStyleBackColor = true;
            this.B_Apply.Click += new System.EventHandler(this.B_Apply_Click);
            // 
            // Lab_Angle
            // 
            this.Lab_Angle.AutoSize = true;
            this.Lab_Angle.Location = new System.Drawing.Point(15, 295);
            this.Lab_Angle.Name = "Lab_Angle";
            this.Lab_Angle.Size = new System.Drawing.Size(41, 12);
            this.Lab_Angle.TabIndex = 36;
            this.Lab_Angle.Text = "Angle:";
            // 
            // NUD_Angle
            // 
            this.NUD_Angle.DecimalPlaces = 1;
            this.NUD_Angle.Location = new System.Drawing.Point(58, 291);
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
            this.NUD_Angle.Size = new System.Drawing.Size(59, 21);
            this.NUD_Angle.TabIndex = 37;
            this.NUD_Angle.ValueChanged += new System.EventHandler(this.NUD_Angle_ValueChanged);
            // 
            // ChB_DrawFill
            // 
            this.ChB_DrawFill.AutoSize = true;
            this.ChB_DrawFill.Location = new System.Drawing.Point(175, 314);
            this.ChB_DrawFill.Name = "ChB_DrawFill";
            this.ChB_DrawFill.Size = new System.Drawing.Size(78, 16);
            this.ChB_DrawFill.TabIndex = 38;
            this.ChB_DrawFill.Text = "Draw Fill";
            this.ChB_DrawFill.UseVisualStyleBackColor = true;
            this.ChB_DrawFill.CheckedChanged += new System.EventHandler(this.ChB_DrawFill_CheckedChanged);
            // 
            // markerControl1
            // 
            this.markerControl1.AllowDrop = true;
            this.markerControl1.BackColor = System.Drawing.Color.White;
            this.markerControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.markerControl1.CellSize = new System.Drawing.Size(25, 25);
            this.markerControl1.ColumnNumber = 10;
            this.markerControl1.Location = new System.Drawing.Point(12, 76);
            this.markerControl1.MarkerType = MeteoInfoC.Drawing.MarkerType.Simple;
            this.markerControl1.Name = "markerControl1";
            this.markerControl1.SelectedCell = -1;
            this.markerControl1.ShapeType = MeteoInfoC.Shape.ShapeTypes.Point;
            this.markerControl1.Size = new System.Drawing.Size(272, 201);
            this.markerControl1.SymbolNumber = 256;
            this.markerControl1.TabIndex = 31;
            this.markerControl1.SelectedCellChanged += new System.EventHandler(this.markerControl1_SelectedCellChanged);
            // 
            // frmPointSymbolSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 466);
            this.Controls.Add(this.ChB_DrawFill);
            this.Controls.Add(this.NUD_Angle);
            this.Controls.Add(this.Lab_Angle);
            this.Controls.Add(this.B_OK);
            this.Controls.Add(this.ChB_DrawShape);
            this.Controls.Add(this.markerControl1);
            this.Controls.Add(this.B_Apply);
            this.Controls.Add(this.Lab_FontFamily);
            this.Controls.Add(this.CB_FontFamily);
            this.Controls.Add(this.GB_Outline);
            this.Controls.Add(this.NUD_Size);
            this.Controls.Add(this.Lab_Size);
            this.Controls.Add(this.B_FillColor);
            this.Controls.Add(this.Lab_FillColor);
            this.Controls.Add(this.Lab_MarkerType);
            this.Controls.Add(this.CB_MarkerType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmPointSymbolSet";
            this.Text = "Point Symbol Set";
            this.Load += new System.EventHandler(this.frmPointSymbolSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Size)).EndInit();
            this.GB_Outline.ResumeLayout(false);
            this.GB_Outline.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Angle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CB_MarkerType;
        private System.Windows.Forms.Label Lab_MarkerType;
        private System.Windows.Forms.Label Lab_FillColor;
        private System.Windows.Forms.Button B_FillColor;
        private System.Windows.Forms.Label Lab_Size;
        private System.Windows.Forms.NumericUpDown NUD_Size;
        private System.Windows.Forms.GroupBox GB_Outline;
        private System.Windows.Forms.CheckBox ChB_DrawOutline;
        private System.Windows.Forms.Button B_OutlineColor;
        private System.Windows.Forms.Label Lab_OutlineColor;
        private SymbolControl markerControl1;
        private System.Windows.Forms.Label Lab_FontFamily;
        private System.Windows.Forms.ComboBox CB_FontFamily;
        private System.Windows.Forms.CheckBox ChB_DrawShape;
        private System.Windows.Forms.Button B_OK;
        private System.Windows.Forms.Button B_Apply;
        private System.Windows.Forms.Label Lab_Angle;
        private System.Windows.Forms.NumericUpDown NUD_Angle;
        private System.Windows.Forms.CheckBox ChB_DrawFill;
    }
}