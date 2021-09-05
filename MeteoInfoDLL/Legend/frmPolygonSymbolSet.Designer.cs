﻿namespace MeteoInfoC.Legend
{
    partial class frmPolygonSymbolSet
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
            this.GB_Outline = new System.Windows.Forms.GroupBox();
            this.B_OutlineColor = new System.Windows.Forms.Button();
            this.Lab_OutlineColor = new System.Windows.Forms.Label();
            this.NUD_OutlineSize = new System.Windows.Forms.NumericUpDown();
            this.Lab_OutlineSize = new System.Windows.Forms.Label();
            this.ChB_DrawOutline = new System.Windows.Forms.CheckBox();
            this.B_FillColor = new System.Windows.Forms.Button();
            this.Lab_FillColor = new System.Windows.Forms.Label();
            this.NUD_TransparencyPerc = new System.Windows.Forms.NumericUpDown();
            this.Lab_Trans = new System.Windows.Forms.Label();
            this.ChB_DrawFill = new System.Windows.Forms.CheckBox();
            this.B_BackColor = new System.Windows.Forms.Button();
            this.Lab_BackColor = new System.Windows.Forms.Label();
            this.symbolControl1 = new MeteoInfoC.Legend.SymbolControl();
            this.GB_Outline.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_OutlineSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_TransparencyPerc)).BeginInit();
            this.SuspendLayout();
            // 
            // B_OK
            // 
            this.B_OK.Location = new System.Drawing.Point(85, 452);
            this.B_OK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.B_OK.Name = "B_OK";
            this.B_OK.Size = new System.Drawing.Size(79, 36);
            this.B_OK.TabIndex = 46;
            this.B_OK.Text = "OK";
            this.B_OK.UseVisualStyleBackColor = true;
            this.B_OK.Click += new System.EventHandler(this.B_OK_Click);
            // 
            // ChB_DrawShape
            // 
            this.ChB_DrawShape.AutoSize = true;
            this.ChB_DrawShape.Location = new System.Drawing.Point(19, 256);
            this.ChB_DrawShape.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ChB_DrawShape.Name = "ChB_DrawShape";
            this.ChB_DrawShape.Size = new System.Drawing.Size(109, 19);
            this.ChB_DrawShape.TabIndex = 44;
            this.ChB_DrawShape.Text = "Draw Shape";
            this.ChB_DrawShape.UseVisualStyleBackColor = true;
            this.ChB_DrawShape.CheckedChanged += new System.EventHandler(this.ChB_DrawShape_CheckedChanged);
            // 
            // B_Apply
            // 
            this.B_Apply.Location = new System.Drawing.Point(232, 452);
            this.B_Apply.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.B_Apply.Name = "B_Apply";
            this.B_Apply.Size = new System.Drawing.Size(79, 36);
            this.B_Apply.TabIndex = 45;
            this.B_Apply.Text = "Apply";
            this.B_Apply.UseVisualStyleBackColor = true;
            this.B_Apply.Click += new System.EventHandler(this.B_Apply_Click);
            // 
            // GB_Outline
            // 
            this.GB_Outline.Controls.Add(this.B_OutlineColor);
            this.GB_Outline.Controls.Add(this.Lab_OutlineColor);
            this.GB_Outline.Controls.Add(this.NUD_OutlineSize);
            this.GB_Outline.Controls.Add(this.Lab_OutlineSize);
            this.GB_Outline.Location = new System.Drawing.Point(20, 368);
            this.GB_Outline.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GB_Outline.Name = "GB_Outline";
            this.GB_Outline.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GB_Outline.Size = new System.Drawing.Size(353, 64);
            this.GB_Outline.TabIndex = 42;
            this.GB_Outline.TabStop = false;
            this.GB_Outline.Text = "Outline";
            // 
            // B_OutlineColor
            // 
            this.B_OutlineColor.Location = new System.Drawing.Point(269, 22);
            this.B_OutlineColor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.B_OutlineColor.Name = "B_OutlineColor";
            this.B_OutlineColor.Size = new System.Drawing.Size(76, 29);
            this.B_OutlineColor.TabIndex = 26;
            this.B_OutlineColor.UseVisualStyleBackColor = true;
            this.B_OutlineColor.Click += new System.EventHandler(this.B_OutlineColor_Click);
            // 
            // Lab_OutlineColor
            // 
            this.Lab_OutlineColor.AutoSize = true;
            this.Lab_OutlineColor.Location = new System.Drawing.Point(209, 30);
            this.Lab_OutlineColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lab_OutlineColor.Name = "Lab_OutlineColor";
            this.Lab_OutlineColor.Size = new System.Drawing.Size(55, 15);
            this.Lab_OutlineColor.TabIndex = 1;
            this.Lab_OutlineColor.Text = "Color:";
            // 
            // NUD_OutlineSize
            // 
            this.NUD_OutlineSize.DecimalPlaces = 1;
            this.NUD_OutlineSize.Location = new System.Drawing.Point(69, 25);
            this.NUD_OutlineSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.NUD_OutlineSize.Name = "NUD_OutlineSize";
            this.NUD_OutlineSize.Size = new System.Drawing.Size(79, 25);
            this.NUD_OutlineSize.TabIndex = 41;
            this.NUD_OutlineSize.ValueChanged += new System.EventHandler(this.NUD_OutlineSize_ValueChanged);
            // 
            // Lab_OutlineSize
            // 
            this.Lab_OutlineSize.AutoSize = true;
            this.Lab_OutlineSize.Location = new System.Drawing.Point(15, 28);
            this.Lab_OutlineSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lab_OutlineSize.Name = "Lab_OutlineSize";
            this.Lab_OutlineSize.Size = new System.Drawing.Size(47, 15);
            this.Lab_OutlineSize.TabIndex = 40;
            this.Lab_OutlineSize.Text = "Size:";
            // 
            // ChB_DrawOutline
            // 
            this.ChB_DrawOutline.AutoSize = true;
            this.ChB_DrawOutline.Location = new System.Drawing.Point(19, 332);
            this.ChB_DrawOutline.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ChB_DrawOutline.Name = "ChB_DrawOutline";
            this.ChB_DrawOutline.Size = new System.Drawing.Size(125, 19);
            this.ChB_DrawOutline.TabIndex = 0;
            this.ChB_DrawOutline.Text = "Draw Outline";
            this.ChB_DrawOutline.UseVisualStyleBackColor = true;
            this.ChB_DrawOutline.CheckedChanged += new System.EventHandler(this.ChB_DrawOutline_CheckedChanged);
            // 
            // B_FillColor
            // 
            this.B_FillColor.Location = new System.Drawing.Point(288, 289);
            this.B_FillColor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.B_FillColor.Name = "B_FillColor";
            this.B_FillColor.Size = new System.Drawing.Size(76, 29);
            this.B_FillColor.TabIndex = 39;
            this.B_FillColor.UseVisualStyleBackColor = true;
            this.B_FillColor.Click += new System.EventHandler(this.B_FillColor_Click);
            // 
            // Lab_FillColor
            // 
            this.Lab_FillColor.AutoSize = true;
            this.Lab_FillColor.Location = new System.Drawing.Point(185, 295);
            this.Lab_FillColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lab_FillColor.Name = "Lab_FillColor";
            this.Lab_FillColor.Size = new System.Drawing.Size(95, 15);
            this.Lab_FillColor.TabIndex = 38;
            this.Lab_FillColor.Text = "Fill Color:";
            // 
            // NUD_TransparencyPerc
            // 
            this.NUD_TransparencyPerc.Location = new System.Drawing.Point(285, 255);
            this.NUD_TransparencyPerc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.NUD_TransparencyPerc.Name = "NUD_TransparencyPerc";
            this.NUD_TransparencyPerc.Size = new System.Drawing.Size(79, 25);
            this.NUD_TransparencyPerc.TabIndex = 48;
            this.NUD_TransparencyPerc.ValueChanged += new System.EventHandler(this.NUD_TransparencyPerc_ValueChanged);
            // 
            // Lab_Trans
            // 
            this.Lab_Trans.AutoSize = true;
            this.Lab_Trans.Location = new System.Drawing.Point(169, 258);
            this.Lab_Trans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lab_Trans.Name = "Lab_Trans";
            this.Lab_Trans.Size = new System.Drawing.Size(111, 15);
            this.Lab_Trans.TabIndex = 47;
            this.Lab_Trans.Text = "TransPerency:";
            // 
            // ChB_DrawFill
            // 
            this.ChB_DrawFill.AutoSize = true;
            this.ChB_DrawFill.Location = new System.Drawing.Point(19, 294);
            this.ChB_DrawFill.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ChB_DrawFill.Name = "ChB_DrawFill";
            this.ChB_DrawFill.Size = new System.Drawing.Size(101, 19);
            this.ChB_DrawFill.TabIndex = 51;
            this.ChB_DrawFill.Text = "Draw Fill";
            this.ChB_DrawFill.UseVisualStyleBackColor = true;
            this.ChB_DrawFill.CheckedChanged += new System.EventHandler(this.ChB_DrawFill_CheckedChanged);
            // 
            // B_BackColor
            // 
            this.B_BackColor.Location = new System.Drawing.Point(288, 328);
            this.B_BackColor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.B_BackColor.Name = "B_BackColor";
            this.B_BackColor.Size = new System.Drawing.Size(76, 29);
            this.B_BackColor.TabIndex = 53;
            this.B_BackColor.UseVisualStyleBackColor = true;
            this.B_BackColor.Click += new System.EventHandler(this.B_BackColor_Click);
            // 
            // Lab_BackColor
            // 
            this.Lab_BackColor.AutoSize = true;
            this.Lab_BackColor.Location = new System.Drawing.Point(185, 334);
            this.Lab_BackColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lab_BackColor.Name = "Lab_BackColor";
            this.Lab_BackColor.Size = new System.Drawing.Size(95, 15);
            this.Lab_BackColor.TabIndex = 52;
            this.Lab_BackColor.Text = "Back Color:";
            // 
            // symbolControl1
            // 
            this.symbolControl1.AllowDrop = true;
            this.symbolControl1.BackColor = System.Drawing.Color.White;
            this.symbolControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.symbolControl1.CellSize = new System.Drawing.Size(25, 25);
            this.symbolControl1.ColumnNumber = 10;
            this.symbolControl1.Location = new System.Drawing.Point(13, 8);
            this.symbolControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.symbolControl1.MarkerType = MeteoInfoC.Drawing.MarkerType.Simple;
            this.symbolControl1.Name = "symbolControl1";
            this.symbolControl1.SelectedCell = -1;
            this.symbolControl1.ShapeType = MeteoInfoC.Shape.ShapeTypes.Polygon;
            this.symbolControl1.Size = new System.Drawing.Size(362, 230);
            this.symbolControl1.SymbolNumber = 10;
            this.symbolControl1.TabIndex = 43;
            this.symbolControl1.SelectedCellChanged += new System.EventHandler(this.symbolControl1_SelectedCellChanged);
            // 
            // frmPolygonSymbolSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 504);
            this.Controls.Add(this.B_BackColor);
            this.Controls.Add(this.Lab_BackColor);
            this.Controls.Add(this.ChB_DrawFill);
            this.Controls.Add(this.NUD_TransparencyPerc);
            this.Controls.Add(this.Lab_Trans);
            this.Controls.Add(this.ChB_DrawOutline);
            this.Controls.Add(this.B_OK);
            this.Controls.Add(this.ChB_DrawShape);
            this.Controls.Add(this.symbolControl1);
            this.Controls.Add(this.B_Apply);
            this.Controls.Add(this.GB_Outline);
            this.Controls.Add(this.B_FillColor);
            this.Controls.Add(this.Lab_FillColor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmPolygonSymbolSet";
            this.Text = "Polygon Symbol Set";
            this.Load += new System.EventHandler(this.frmPolygonSymbolSet_Load);
            this.GB_Outline.ResumeLayout(false);
            this.GB_Outline.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_OutlineSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_TransparencyPerc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button B_OK;
        private System.Windows.Forms.CheckBox ChB_DrawShape;
        private SymbolControl symbolControl1;
        private System.Windows.Forms.Button B_Apply;
        private System.Windows.Forms.GroupBox GB_Outline;
        private System.Windows.Forms.Button B_OutlineColor;
        private System.Windows.Forms.Label Lab_OutlineColor;
        private System.Windows.Forms.CheckBox ChB_DrawOutline;
        private System.Windows.Forms.NumericUpDown NUD_OutlineSize;
        private System.Windows.Forms.Label Lab_OutlineSize;
        private System.Windows.Forms.Button B_FillColor;
        private System.Windows.Forms.Label Lab_FillColor;
        private System.Windows.Forms.NumericUpDown NUD_TransparencyPerc;
        private System.Windows.Forms.Label Lab_Trans;
        private System.Windows.Forms.CheckBox ChB_DrawFill;
        private System.Windows.Forms.Button B_BackColor;
        private System.Windows.Forms.Label Lab_BackColor;
    }
}