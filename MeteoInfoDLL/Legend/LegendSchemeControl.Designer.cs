namespace MeteoInfoC.Legend
{
    partial class LegendSchemeControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LegendSchemeControl));
            this.TSB_Down = new System.Windows.Forms.ToolStripButton();
            this.TSB_Up = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.TSB_DelAll = new System.Windows.Forms.ToolStripButton();
            this.TSB_Reverse = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CB_LegendType = new System.Windows.Forms.ComboBox();
            this.TSB_MakeBreaks = new System.Windows.Forms.ToolStripButton();
            this.TSB_Del = new System.Windows.Forms.ToolStripButton();
            this.TSB_Add = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TSB_Save = new System.Windows.Forms.ToolStripButton();
            this.TSB_Open = new System.Windows.Forms.ToolStripButton();
            this.CB_Field = new System.Windows.Forms.ComboBox();
            this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
            this.legendView1 = new MeteoInfoC.Legend.LegendView();
            this.ToolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TSB_Down
            // 
            this.TSB_Down.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_Down.Image = ((System.Drawing.Image)(resources.GetObject("TSB_Down.Image")));
            this.TSB_Down.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_Down.Name = "TSB_Down";
            this.TSB_Down.Size = new System.Drawing.Size(23, 22);
            this.TSB_Down.Text = "ToolStripButton1";
            this.TSB_Down.ToolTipText = "Move Break Down";
            this.TSB_Down.Click += new System.EventHandler(this.TSB_Down_Click);
            // 
            // TSB_Up
            // 
            this.TSB_Up.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_Up.Image = ((System.Drawing.Image)(resources.GetObject("TSB_Up.Image")));
            this.TSB_Up.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_Up.Name = "TSB_Up";
            this.TSB_Up.Size = new System.Drawing.Size(23, 22);
            this.TSB_Up.Text = "ToolStripButton2";
            this.TSB_Up.ToolTipText = "Move Break Up";
            this.TSB_Up.Click += new System.EventHandler(this.TSB_Up_Click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // TSB_DelAll
            // 
            this.TSB_DelAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_DelAll.Image = ((System.Drawing.Image)(resources.GetObject("TSB_DelAll.Image")));
            this.TSB_DelAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_DelAll.Name = "TSB_DelAll";
            this.TSB_DelAll.Size = new System.Drawing.Size(23, 22);
            this.TSB_DelAll.Text = "ToolStripButton5";
            this.TSB_DelAll.ToolTipText = "Remove All Breaks";
            this.TSB_DelAll.Click += new System.EventHandler(this.TSB_DelAll_Click);
            // 
            // TSB_Reverse
            // 
            this.TSB_Reverse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_Reverse.Image = ((System.Drawing.Image)(resources.GetObject("TSB_Reverse.Image")));
            this.TSB_Reverse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_Reverse.Name = "TSB_Reverse";
            this.TSB_Reverse.Size = new System.Drawing.Size(23, 22);
            this.TSB_Reverse.Text = "Reverse Breaks";
            this.TSB_Reverse.Click += new System.EventHandler(this.TSB_Reverse_Click);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(100, 76);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 33;
            this.label2.Text = "Field:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 43);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 15);
            this.label1.TabIndex = 31;
            this.label1.Text = "Legend Type:";
            // 
            // CB_LegendType
            // 
            this.CB_LegendType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_LegendType.FormattingEnabled = true;
            this.CB_LegendType.Location = new System.Drawing.Point(161, 40);
            this.CB_LegendType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CB_LegendType.Name = "CB_LegendType";
            this.CB_LegendType.Size = new System.Drawing.Size(193, 23);
            this.CB_LegendType.TabIndex = 30;
            this.CB_LegendType.SelectedIndexChanged += new System.EventHandler(this.CB_LegendType_SelectedIndexChanged);
            // 
            // TSB_MakeBreaks
            // 
            this.TSB_MakeBreaks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_MakeBreaks.Image = ((System.Drawing.Image)(resources.GetObject("TSB_MakeBreaks.Image")));
            this.TSB_MakeBreaks.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_MakeBreaks.Name = "TSB_MakeBreaks";
            this.TSB_MakeBreaks.Size = new System.Drawing.Size(23, 22);
            this.TSB_MakeBreaks.Text = "New Legend or Colors";
            this.TSB_MakeBreaks.ToolTipText = "New Legend or Colors";
            this.TSB_MakeBreaks.Click += new System.EventHandler(this.TSB_MakeBreaks_Click);
            // 
            // TSB_Del
            // 
            this.TSB_Del.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_Del.Image = ((System.Drawing.Image)(resources.GetObject("TSB_Del.Image")));
            this.TSB_Del.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_Del.Name = "TSB_Del";
            this.TSB_Del.Size = new System.Drawing.Size(23, 22);
            this.TSB_Del.Text = "ToolStripButton4";
            this.TSB_Del.ToolTipText = "Remove Break";
            this.TSB_Del.Click += new System.EventHandler(this.TSB_Del_Click);
            // 
            // TSB_Add
            // 
            this.TSB_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_Add.Image = ((System.Drawing.Image)(resources.GetObject("TSB_Add.Image")));
            this.TSB_Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_Add.Name = "TSB_Add";
            this.TSB_Add.Size = new System.Drawing.Size(23, 22);
            this.TSB_Add.Text = "ToolStripButton3";
            this.TSB_Add.ToolTipText = "Add Break";
            this.TSB_Add.Click += new System.EventHandler(this.TSB_Add_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // TSB_Save
            // 
            this.TSB_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_Save.Image = ((System.Drawing.Image)(resources.GetObject("TSB_Save.Image")));
            this.TSB_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_Save.Name = "TSB_Save";
            this.TSB_Save.Size = new System.Drawing.Size(23, 22);
            this.TSB_Save.Text = "ToolStripButton2";
            this.TSB_Save.ToolTipText = "Export Legend";
            this.TSB_Save.Click += new System.EventHandler(this.TSB_Save_Click);
            // 
            // TSB_Open
            // 
            this.TSB_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_Open.Image = ((System.Drawing.Image)(resources.GetObject("TSB_Open.Image")));
            this.TSB_Open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_Open.Name = "TSB_Open";
            this.TSB_Open.Size = new System.Drawing.Size(23, 22);
            this.TSB_Open.Text = "ToolStripButton1";
            this.TSB_Open.ToolTipText = "Import Legend";
            this.TSB_Open.Click += new System.EventHandler(this.TSB_Open_Click);
            // 
            // CB_Field
            // 
            this.CB_Field.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Field.FormattingEnabled = true;
            this.CB_Field.Location = new System.Drawing.Point(161, 72);
            this.CB_Field.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CB_Field.Name = "CB_Field";
            this.CB_Field.Size = new System.Drawing.Size(193, 23);
            this.CB_Field.TabIndex = 32;
            this.CB_Field.SelectedIndexChanged += new System.EventHandler(this.CB_Field_SelectedIndexChanged);
            // 
            // ToolStrip1
            // 
            this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSB_Open,
            this.TSB_Save,
            this.ToolStripSeparator1,
            this.TSB_Add,
            this.TSB_Del,
            this.TSB_DelAll,
            this.ToolStripSeparator2,
            this.TSB_Up,
            this.TSB_Down,
            this.TSB_Reverse,
            this.ToolStripSeparator3,
            this.TSB_MakeBreaks});
            this.ToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip1.Name = "ToolStrip1";
            this.ToolStrip1.Size = new System.Drawing.Size(420, 25);
            this.ToolStrip1.TabIndex = 25;
            this.ToolStrip1.Text = "ToolStrip1";
            // 
            // legendView1
            // 
            this.legendView1.BackColor = System.Drawing.Color.White;
            this.legendView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.legendView1.LegendScheme = null;
            this.legendView1.Location = new System.Drawing.Point(4, 104);
            this.legendView1.Margin = new System.Windows.Forms.Padding(5);
            this.legendView1.Name = "legendView1";
            this.legendView1.Size = new System.Drawing.Size(411, 346);
            this.legendView1.TabIndex = 34;
            // 
            // LegendSchemeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.legendView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CB_LegendType);
            this.Controls.Add(this.CB_Field);
            this.Controls.Add(this.ToolStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "LegendSchemeControl";
            this.Size = new System.Drawing.Size(420, 455);
            this.Load += new System.EventHandler(this.LegendSchemeControl_Load);
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ToolStripButton TSB_Down;
        internal System.Windows.Forms.ToolStripButton TSB_Up;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        internal System.Windows.Forms.ToolStripButton TSB_DelAll;
        private System.Windows.Forms.ToolStripButton TSB_Reverse;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CB_LegendType;
        internal System.Windows.Forms.ToolStripButton TSB_MakeBreaks;
        internal System.Windows.Forms.ToolStripButton TSB_Del;
        internal System.Windows.Forms.ToolStripButton TSB_Add;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripButton TSB_Save;
        internal System.Windows.Forms.ToolStripButton TSB_Open;
        private System.Windows.Forms.ComboBox CB_Field;
        internal System.Windows.Forms.ToolStrip ToolStrip1;
        internal LegendView legendView1;
    }
}
