namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Legend set form
    /// </summary>
    partial class frmLegendSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLegendSet));
            this.B_Apply = new System.Windows.Forms.Button();
            this.B_OK = new System.Windows.Forms.Button();
            this.B_Cancel = new System.Windows.Forms.Button();
            this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TSB_Open = new System.Windows.Forms.ToolStripButton();
            this.TSB_Save = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TSB_Add = new System.Windows.Forms.ToolStripButton();
            this.TSB_Del = new System.Windows.Forms.ToolStripButton();
            this.TSB_DelAll = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.TSB_Up = new System.Windows.Forms.ToolStripButton();
            this.TSB_Down = new System.Windows.Forms.ToolStripButton();
            this.TSB_Reverse = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.TSB_MakeBreaks = new System.Windows.Forms.ToolStripButton();
            this.CB_LegendType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CB_Field = new System.Windows.Forms.ComboBox();
            this.legendView1 = new MeteoInfoC.Legend.LegendView();
            this.ToolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // B_Apply
            // 
            this.B_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_Apply.Location = new System.Drawing.Point(31, 430);
            this.B_Apply.Name = "B_Apply";
            this.B_Apply.Size = new System.Drawing.Size(75, 30);
            this.B_Apply.TabIndex = 20;
            this.B_Apply.Text = "Apply";
            this.B_Apply.UseVisualStyleBackColor = true;
            this.B_Apply.Click += new System.EventHandler(this.B_Apply_Click);
            // 
            // B_OK
            // 
            this.B_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_OK.Location = new System.Drawing.Point(127, 430);
            this.B_OK.Name = "B_OK";
            this.B_OK.Size = new System.Drawing.Size(75, 30);
            this.B_OK.TabIndex = 19;
            this.B_OK.Text = "OK";
            this.B_OK.UseVisualStyleBackColor = true;
            this.B_OK.Click += new System.EventHandler(this.B_OK_Click);
            // 
            // B_Cancel
            // 
            this.B_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_Cancel.Location = new System.Drawing.Point(227, 430);
            this.B_Cancel.Name = "B_Cancel";
            this.B_Cancel.Size = new System.Drawing.Size(75, 30);
            this.B_Cancel.TabIndex = 18;
            this.B_Cancel.Text = "Cancel";
            this.B_Cancel.UseVisualStyleBackColor = true;
            this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
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
            this.ToolStrip1.Size = new System.Drawing.Size(343, 25);
            this.ToolStrip1.TabIndex = 16;
            this.ToolStrip1.Text = "ToolStrip1";
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
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            // CB_LegendType
            // 
            this.CB_LegendType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_LegendType.FormattingEnabled = true;
            this.CB_LegendType.Location = new System.Drawing.Point(127, 36);
            this.CB_LegendType.Name = "CB_LegendType";
            this.CB_LegendType.Size = new System.Drawing.Size(146, 21);
            this.CB_LegendType.TabIndex = 21;
            this.CB_LegendType.SelectedIndexChanged += new System.EventHandler(this.CB_LegendType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Legend Type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(81, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Field:";
            // 
            // CB_Field
            // 
            this.CB_Field.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Field.FormattingEnabled = true;
            this.CB_Field.Location = new System.Drawing.Point(127, 65);
            this.CB_Field.Name = "CB_Field";
            this.CB_Field.Size = new System.Drawing.Size(146, 21);
            this.CB_Field.TabIndex = 23;
            this.CB_Field.SelectedIndexChanged += new System.EventHandler(this.CB_Field_SelectedIndexChanged);
            // 
            // legendView1
            // 
            this.legendView1.BackColor = System.Drawing.Color.White;
            this.legendView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.legendView1.LegendScheme = null;
            this.legendView1.Location = new System.Drawing.Point(10, 96);
            this.legendView1.Name = "legendView1";
            this.legendView1.Size = new System.Drawing.Size(322, 317);
            this.legendView1.TabIndex = 25;
            // 
            // frmLegendSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 470);
            this.Controls.Add(this.legendView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CB_Field);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CB_LegendType);
            this.Controls.Add(this.B_Apply);
            this.Controls.Add(this.B_OK);
            this.Controls.Add(this.B_Cancel);
            this.Controls.Add(this.ToolStrip1);
            this.Font = new System.Drawing.Font("Arial", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLegendSet";
            this.ShowInTaskbar = false;
            this.Text = "Legend Set";
            this.Load += new System.EventHandler(this.frmLegendSet_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLegendSet_FormClosed);
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button B_Apply;
        internal System.Windows.Forms.Button B_OK;
        internal System.Windows.Forms.Button B_Cancel;
        internal System.Windows.Forms.ToolStrip ToolStrip1;
        internal System.Windows.Forms.ToolStripButton TSB_Open;
        internal System.Windows.Forms.ToolStripButton TSB_Save;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripButton TSB_Add;
        internal System.Windows.Forms.ToolStripButton TSB_Del;
        internal System.Windows.Forms.ToolStripButton TSB_DelAll;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        internal System.Windows.Forms.ToolStripButton TSB_Up;
        internal System.Windows.Forms.ToolStripButton TSB_Down;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
        internal System.Windows.Forms.ToolStripButton TSB_MakeBreaks;
        private System.Windows.Forms.ComboBox CB_LegendType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton TSB_Reverse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CB_Field;
        internal LegendView legendView1;
    }
}