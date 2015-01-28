namespace MeteoInfoC.Legend
{
    partial class frmPolylineSymbolSet
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
            this.B_OK = new System.Windows.Forms.Button();
            this.ChB_DrawShape = new System.Windows.Forms.CheckBox();
            this.B_Apply = new System.Windows.Forms.Button();
            this.NUD_Size = new System.Windows.Forms.NumericUpDown();
            this.Lab_Size = new System.Windows.Forms.Label();
            this.B_FillColor = new System.Windows.Forms.Button();
            this.Lab_Color = new System.Windows.Forms.Label();
            this.GB_PointSymbol = new System.Windows.Forms.GroupBox();
            this.CB_SymbolStyle = new System.Windows.Forms.ComboBox();
            this.NUD_SymbolInterval = new System.Windows.Forms.NumericUpDown();
            this.Lab_SymbolInterval = new System.Windows.Forms.Label();
            this.NUD_SymbolSize = new System.Windows.Forms.NumericUpDown();
            this.Lab_SymbolSize = new System.Windows.Forms.Label();
            this.B_SymbolColor = new System.Windows.Forms.Button();
            this.Lab_SymbolColor = new System.Windows.Forms.Label();
            this.ChB_DrawPointSymbol = new System.Windows.Forms.CheckBox();
            this.symbolControl1 = new MeteoInfoC.Legend.SymbolControl();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Size)).BeginInit();
            this.GB_PointSymbol.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_SymbolInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_SymbolSize)).BeginInit();
            this.SuspendLayout();
            // 
            // B_OK
            // 
            this.B_OK.Location = new System.Drawing.Point(55, 295);
            this.B_OK.Name = "B_OK";
            this.B_OK.Size = new System.Drawing.Size(59, 29);
            this.B_OK.TabIndex = 42;
            this.B_OK.Text = "OK";
            this.B_OK.UseVisualStyleBackColor = true;
            this.B_OK.Click += new System.EventHandler(this.B_OK_Click);
            // 
            // ChB_DrawShape
            // 
            this.ChB_DrawShape.AutoSize = true;
            this.ChB_DrawShape.Location = new System.Drawing.Point(17, 161);
            this.ChB_DrawShape.Name = "ChB_DrawShape";
            this.ChB_DrawShape.Size = new System.Drawing.Size(84, 16);
            this.ChB_DrawShape.TabIndex = 40;
            this.ChB_DrawShape.Text = "Draw Shape";
            this.ChB_DrawShape.UseVisualStyleBackColor = true;
            this.ChB_DrawShape.CheckedChanged += new System.EventHandler(this.ChB_DrawShape_CheckedChanged);
            // 
            // B_Apply
            // 
            this.B_Apply.Location = new System.Drawing.Point(165, 295);
            this.B_Apply.Name = "B_Apply";
            this.B_Apply.Size = new System.Drawing.Size(59, 29);
            this.B_Apply.TabIndex = 41;
            this.B_Apply.Text = "Apply";
            this.B_Apply.UseVisualStyleBackColor = true;
            this.B_Apply.Click += new System.EventHandler(this.B_Apply_Click);
            // 
            // NUD_Size
            // 
            this.NUD_Size.DecimalPlaces = 1;
            this.NUD_Size.Location = new System.Drawing.Point(56, 125);
            this.NUD_Size.Name = "NUD_Size";
            this.NUD_Size.Size = new System.Drawing.Size(59, 21);
            this.NUD_Size.TabIndex = 38;
            this.NUD_Size.ValueChanged += new System.EventHandler(this.NUD_Size_ValueChanged);
            // 
            // Lab_Size
            // 
            this.Lab_Size.AutoSize = true;
            this.Lab_Size.Location = new System.Drawing.Point(15, 127);
            this.Lab_Size.Name = "Lab_Size";
            this.Lab_Size.Size = new System.Drawing.Size(35, 12);
            this.Lab_Size.TabIndex = 37;
            this.Lab_Size.Text = "Size:";
            // 
            // B_FillColor
            // 
            this.B_FillColor.Location = new System.Drawing.Point(222, 123);
            this.B_FillColor.Name = "B_FillColor";
            this.B_FillColor.Size = new System.Drawing.Size(57, 23);
            this.B_FillColor.TabIndex = 36;
            this.B_FillColor.UseVisualStyleBackColor = true;
            this.B_FillColor.Click += new System.EventHandler(this.B_FillColor_Click);
            // 
            // Lab_Color
            // 
            this.Lab_Color.AutoSize = true;
            this.Lab_Color.Location = new System.Drawing.Point(170, 128);
            this.Lab_Color.Name = "Lab_Color";
            this.Lab_Color.Size = new System.Drawing.Size(41, 12);
            this.Lab_Color.TabIndex = 35;
            this.Lab_Color.Text = "Color:";
            // 
            // GB_PointSymbol
            // 
            this.GB_PointSymbol.Controls.Add(this.CB_SymbolStyle);
            this.GB_PointSymbol.Controls.Add(this.NUD_SymbolInterval);
            this.GB_PointSymbol.Controls.Add(this.Lab_SymbolInterval);
            this.GB_PointSymbol.Controls.Add(this.NUD_SymbolSize);
            this.GB_PointSymbol.Controls.Add(this.Lab_SymbolSize);
            this.GB_PointSymbol.Controls.Add(this.B_SymbolColor);
            this.GB_PointSymbol.Controls.Add(this.Lab_SymbolColor);
            this.GB_PointSymbol.Location = new System.Drawing.Point(13, 190);
            this.GB_PointSymbol.Name = "GB_PointSymbol";
            this.GB_PointSymbol.Size = new System.Drawing.Size(265, 88);
            this.GB_PointSymbol.TabIndex = 43;
            this.GB_PointSymbol.TabStop = false;
            this.GB_PointSymbol.Text = "Point Symbol";
            // 
            // CB_SymbolStyle
            // 
            this.CB_SymbolStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_SymbolStyle.FormattingEnabled = true;
            this.CB_SymbolStyle.Location = new System.Drawing.Point(18, 53);
            this.CB_SymbolStyle.Name = "CB_SymbolStyle";
            this.CB_SymbolStyle.Size = new System.Drawing.Size(98, 20);
            this.CB_SymbolStyle.TabIndex = 45;
            this.CB_SymbolStyle.SelectedIndexChanged += new System.EventHandler(this.CB_SymbolStyle_SelectedIndexChanged);
            // 
            // NUD_SymbolInterval
            // 
            this.NUD_SymbolInterval.Location = new System.Drawing.Point(200, 51);
            this.NUD_SymbolInterval.Name = "NUD_SymbolInterval";
            this.NUD_SymbolInterval.Size = new System.Drawing.Size(59, 21);
            this.NUD_SymbolInterval.TabIndex = 42;
            this.NUD_SymbolInterval.ValueChanged += new System.EventHandler(this.NUD_SymbolInterval_ValueChanged);
            // 
            // Lab_SymbolInterval
            // 
            this.Lab_SymbolInterval.AutoSize = true;
            this.Lab_SymbolInterval.Location = new System.Drawing.Point(138, 53);
            this.Lab_SymbolInterval.Name = "Lab_SymbolInterval";
            this.Lab_SymbolInterval.Size = new System.Drawing.Size(59, 12);
            this.Lab_SymbolInterval.TabIndex = 41;
            this.Lab_SymbolInterval.Text = "Interval:";
            // 
            // NUD_SymbolSize
            // 
            this.NUD_SymbolSize.DecimalPlaces = 1;
            this.NUD_SymbolSize.Location = new System.Drawing.Point(57, 22);
            this.NUD_SymbolSize.Name = "NUD_SymbolSize";
            this.NUD_SymbolSize.Size = new System.Drawing.Size(59, 21);
            this.NUD_SymbolSize.TabIndex = 40;
            this.NUD_SymbolSize.ValueChanged += new System.EventHandler(this.NUD_SymbolSize_ValueChanged);
            // 
            // Lab_SymbolSize
            // 
            this.Lab_SymbolSize.AutoSize = true;
            this.Lab_SymbolSize.Location = new System.Drawing.Point(16, 24);
            this.Lab_SymbolSize.Name = "Lab_SymbolSize";
            this.Lab_SymbolSize.Size = new System.Drawing.Size(35, 12);
            this.Lab_SymbolSize.TabIndex = 39;
            this.Lab_SymbolSize.Text = "Size:";
            // 
            // B_SymbolColor
            // 
            this.B_SymbolColor.Location = new System.Drawing.Point(202, 18);
            this.B_SymbolColor.Name = "B_SymbolColor";
            this.B_SymbolColor.Size = new System.Drawing.Size(57, 23);
            this.B_SymbolColor.TabIndex = 26;
            this.B_SymbolColor.UseVisualStyleBackColor = true;
            this.B_SymbolColor.Click += new System.EventHandler(this.B_SymbolColor_Click);
            // 
            // Lab_SymbolColor
            // 
            this.Lab_SymbolColor.AutoSize = true;
            this.Lab_SymbolColor.Location = new System.Drawing.Point(157, 24);
            this.Lab_SymbolColor.Name = "Lab_SymbolColor";
            this.Lab_SymbolColor.Size = new System.Drawing.Size(41, 12);
            this.Lab_SymbolColor.TabIndex = 1;
            this.Lab_SymbolColor.Text = "Color:";
            // 
            // ChB_DrawPointSymbol
            // 
            this.ChB_DrawPointSymbol.AutoSize = true;
            this.ChB_DrawPointSymbol.Location = new System.Drawing.Point(153, 161);
            this.ChB_DrawPointSymbol.Name = "ChB_DrawPointSymbol";
            this.ChB_DrawPointSymbol.Size = new System.Drawing.Size(126, 16);
            this.ChB_DrawPointSymbol.TabIndex = 44;
            this.ChB_DrawPointSymbol.Text = "Draw Point Symbol";
            this.ChB_DrawPointSymbol.UseVisualStyleBackColor = true;
            this.ChB_DrawPointSymbol.CheckedChanged += new System.EventHandler(this.ChB_DrawPointSymbol_CheckedChanged);
            // 
            // symbolControl1
            // 
            this.symbolControl1.AllowDrop = true;
            this.symbolControl1.BackColor = System.Drawing.Color.White;
            this.symbolControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.symbolControl1.CellSize = new System.Drawing.Size(50, 40);
            this.symbolControl1.ColumnNumber = 5;
            this.symbolControl1.Location = new System.Drawing.Point(10, 12);
            this.symbolControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.symbolControl1.MarkerType = MeteoInfoC.Drawing.MarkerType.Simple;
            this.symbolControl1.Name = "symbolControl1";
            this.symbolControl1.SelectedCell = -1;
            this.symbolControl1.ShapeType = MeteoInfoC.Shape.ShapeTypes.Polyline;
            this.symbolControl1.Size = new System.Drawing.Size(272, 85);
            this.symbolControl1.SymbolNumber = 5;
            this.symbolControl1.TabIndex = 39;
            this.symbolControl1.SelectedCellChanged += new System.EventHandler(this.symbolControl1_SelectedCellChanged);
            // 
            // frmPolylineSymbolSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 337);
            this.Controls.Add(this.ChB_DrawPointSymbol);
            this.Controls.Add(this.GB_PointSymbol);
            this.Controls.Add(this.B_OK);
            this.Controls.Add(this.ChB_DrawShape);
            this.Controls.Add(this.symbolControl1);
            this.Controls.Add(this.B_Apply);
            this.Controls.Add(this.NUD_Size);
            this.Controls.Add(this.Lab_Size);
            this.Controls.Add(this.B_FillColor);
            this.Controls.Add(this.Lab_Color);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmPolylineSymbolSet";
            this.Text = "Polyline Symbol Set";
            this.Load += new System.EventHandler(this.frmPolylineSymbolSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Size)).EndInit();
            this.GB_PointSymbol.ResumeLayout(false);
            this.GB_PointSymbol.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_SymbolInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_SymbolSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button B_OK;
        private System.Windows.Forms.CheckBox ChB_DrawShape;
        private SymbolControl symbolControl1;
        private System.Windows.Forms.Button B_Apply;
        private System.Windows.Forms.NumericUpDown NUD_Size;
        private System.Windows.Forms.Label Lab_Size;
        private System.Windows.Forms.Button B_FillColor;
        private System.Windows.Forms.Label Lab_Color;
        private System.Windows.Forms.GroupBox GB_PointSymbol;
        private System.Windows.Forms.Button B_SymbolColor;
        private System.Windows.Forms.Label Lab_SymbolColor;
        private System.Windows.Forms.CheckBox ChB_DrawPointSymbol;
        private System.Windows.Forms.NumericUpDown NUD_SymbolSize;
        private System.Windows.Forms.Label Lab_SymbolSize;
        private System.Windows.Forms.NumericUpDown NUD_SymbolInterval;
        private System.Windows.Forms.Label Lab_SymbolInterval;
        private System.Windows.Forms.ComboBox CB_SymbolStyle;
    }
}