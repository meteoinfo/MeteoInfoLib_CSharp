namespace MeteoInfoC.Layer
{
    partial class frmLayerProperty
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
            this.TabControl_Prop = new System.Windows.Forms.TabControl();
            this.TabPage_General = new System.Windows.Forms.TabPage();
            this.PG_General = new System.Windows.Forms.PropertyGrid();
            this.TabPage_Legend = new System.Windows.Forms.TabPage();
            this.TabPage_Chart = new System.Windows.Forms.TabPage();
            this.CB_Align = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CHB_Collision = new System.Windows.Forms.CheckBox();
            this.TB_YShift = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TB_XShift = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.GB_Size = new System.Windows.Forms.GroupBox();
            this.TB_MinSize = new System.Windows.Forms.TextBox();
            this.Lab_MinSize = new System.Windows.Forms.Label();
            this.TB_MaxSize = new System.Windows.Forms.TextBox();
            this.Lab_Height = new System.Windows.Forms.Label();
            this.TB_BarWidth = new System.Windows.Forms.TextBox();
            this.Lab_Width = new System.Windows.Forms.Label();
            this.CLB_Fields = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CB_ChartType = new System.Windows.Forms.ComboBox();
            this.B_Apply = new System.Windows.Forms.Button();
            this.B_OK = new System.Windows.Forms.Button();
            this.B_Cancel = new System.Windows.Forms.Button();
            this.CHB_View3D = new System.Windows.Forms.CheckBox();
            this.legendSchemeControl1 = new MeteoInfoC.Legend.LegendSchemeControl();
            this.legendView_Chart = new MeteoInfoC.Legend.LegendView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TB_Thickness = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TabControl_Prop.SuspendLayout();
            this.TabPage_General.SuspendLayout();
            this.TabPage_Legend.SuspendLayout();
            this.TabPage_Chart.SuspendLayout();
            this.GB_Size.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControl_Prop
            // 
            this.TabControl_Prop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl_Prop.Controls.Add(this.TabPage_General);
            this.TabControl_Prop.Controls.Add(this.TabPage_Legend);
            this.TabControl_Prop.Controls.Add(this.TabPage_Chart);
            this.TabControl_Prop.Location = new System.Drawing.Point(0, 0);
            this.TabControl_Prop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TabControl_Prop.Name = "TabControl_Prop";
            this.TabControl_Prop.SelectedIndex = 0;
            this.TabControl_Prop.Size = new System.Drawing.Size(439, 490);
            this.TabControl_Prop.TabIndex = 0;
            this.TabControl_Prop.SelectedIndexChanged += new System.EventHandler(this.TabControl_Prop_SelectedIndexChanged);
            // 
            // TabPage_General
            // 
            this.TabPage_General.Controls.Add(this.PG_General);
            this.TabPage_General.Location = new System.Drawing.Point(4, 25);
            this.TabPage_General.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TabPage_General.Name = "TabPage_General";
            this.TabPage_General.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TabPage_General.Size = new System.Drawing.Size(431, 461);
            this.TabPage_General.TabIndex = 0;
            this.TabPage_General.Text = "General";
            this.TabPage_General.UseVisualStyleBackColor = true;
            // 
            // PG_General
            // 
            this.PG_General.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PG_General.Location = new System.Drawing.Point(4, 4);
            this.PG_General.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PG_General.Name = "PG_General";
            this.PG_General.Size = new System.Drawing.Size(423, 453);
            this.PG_General.TabIndex = 1;
            this.PG_General.ToolbarVisible = false;
            // 
            // TabPage_Legend
            // 
            this.TabPage_Legend.Controls.Add(this.legendSchemeControl1);
            this.TabPage_Legend.Location = new System.Drawing.Point(4, 25);
            this.TabPage_Legend.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TabPage_Legend.Name = "TabPage_Legend";
            this.TabPage_Legend.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TabPage_Legend.Size = new System.Drawing.Size(431, 461);
            this.TabPage_Legend.TabIndex = 1;
            this.TabPage_Legend.Text = "Legend";
            this.TabPage_Legend.UseVisualStyleBackColor = true;
            // 
            // TabPage_Chart
            // 
            this.TabPage_Chart.Controls.Add(this.groupBox1);
            this.TabPage_Chart.Controls.Add(this.CB_Align);
            this.TabPage_Chart.Controls.Add(this.label4);
            this.TabPage_Chart.Controls.Add(this.CHB_Collision);
            this.TabPage_Chart.Controls.Add(this.TB_YShift);
            this.TabPage_Chart.Controls.Add(this.label3);
            this.TabPage_Chart.Controls.Add(this.TB_XShift);
            this.TabPage_Chart.Controls.Add(this.label2);
            this.TabPage_Chart.Controls.Add(this.GB_Size);
            this.TabPage_Chart.Controls.Add(this.legendView_Chart);
            this.TabPage_Chart.Controls.Add(this.CLB_Fields);
            this.TabPage_Chart.Controls.Add(this.label1);
            this.TabPage_Chart.Controls.Add(this.CB_ChartType);
            this.TabPage_Chart.Location = new System.Drawing.Point(4, 25);
            this.TabPage_Chart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TabPage_Chart.Name = "TabPage_Chart";
            this.TabPage_Chart.Size = new System.Drawing.Size(431, 461);
            this.TabPage_Chart.TabIndex = 2;
            this.TabPage_Chart.Text = "Chart";
            this.TabPage_Chart.UseVisualStyleBackColor = true;
            // 
            // CB_Align
            // 
            this.CB_Align.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Align.FormattingEnabled = true;
            this.CB_Align.Location = new System.Drawing.Point(315, 321);
            this.CB_Align.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CB_Align.Name = "CB_Align";
            this.CB_Align.Size = new System.Drawing.Size(94, 23);
            this.CB_Align.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(252, 321);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 15);
            this.label4.TabIndex = 34;
            this.label4.Text = "Align:";
            // 
            // CHB_Collision
            // 
            this.CHB_Collision.AutoSize = true;
            this.CHB_Collision.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CHB_Collision.Location = new System.Drawing.Point(248, 280);
            this.CHB_Collision.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CHB_Collision.Name = "CHB_Collision";
            this.CHB_Collision.Size = new System.Drawing.Size(101, 34);
            this.CHB_Collision.TabIndex = 30;
            this.CHB_Collision.Text = "Collision\r\nAvoidance";
            this.CHB_Collision.UseVisualStyleBackColor = true;
            this.CHB_Collision.CheckedChanged += new System.EventHandler(this.CHB_Collision_CheckedChanged);
            // 
            // TB_YShift
            // 
            this.TB_YShift.Location = new System.Drawing.Point(324, 238);
            this.TB_YShift.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TB_YShift.Name = "TB_YShift";
            this.TB_YShift.Size = new System.Drawing.Size(85, 25);
            this.TB_YShift.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(245, 240);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Y Shift:";
            // 
            // TB_XShift
            // 
            this.TB_XShift.Location = new System.Drawing.Point(324, 204);
            this.TB_XShift.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TB_XShift.Name = "TB_XShift";
            this.TB_XShift.Size = new System.Drawing.Size(85, 25);
            this.TB_XShift.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(245, 208);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "X Shift:";
            // 
            // GB_Size
            // 
            this.GB_Size.Controls.Add(this.TB_MinSize);
            this.GB_Size.Controls.Add(this.Lab_MinSize);
            this.GB_Size.Controls.Add(this.TB_MaxSize);
            this.GB_Size.Controls.Add(this.Lab_Height);
            this.GB_Size.Controls.Add(this.TB_BarWidth);
            this.GB_Size.Controls.Add(this.Lab_Width);
            this.GB_Size.Location = new System.Drawing.Point(237, 66);
            this.GB_Size.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GB_Size.Name = "GB_Size";
            this.GB_Size.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GB_Size.Size = new System.Drawing.Size(181, 130);
            this.GB_Size.TabIndex = 3;
            this.GB_Size.TabStop = false;
            this.GB_Size.Text = "Size";
            // 
            // TB_MinSize
            // 
            this.TB_MinSize.Location = new System.Drawing.Point(87, 26);
            this.TB_MinSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TB_MinSize.Name = "TB_MinSize";
            this.TB_MinSize.Size = new System.Drawing.Size(85, 25);
            this.TB_MinSize.TabIndex = 5;
            // 
            // Lab_MinSize
            // 
            this.Lab_MinSize.AutoSize = true;
            this.Lab_MinSize.Location = new System.Drawing.Point(8, 29);
            this.Lab_MinSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lab_MinSize.Name = "Lab_MinSize";
            this.Lab_MinSize.Size = new System.Drawing.Size(71, 15);
            this.Lab_MinSize.TabIndex = 4;
            this.Lab_MinSize.Text = "Minimum:";
            // 
            // TB_MaxSize
            // 
            this.TB_MaxSize.Location = new System.Drawing.Point(87, 59);
            this.TB_MaxSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TB_MaxSize.Name = "TB_MaxSize";
            this.TB_MaxSize.Size = new System.Drawing.Size(85, 25);
            this.TB_MaxSize.TabIndex = 3;
            // 
            // Lab_Height
            // 
            this.Lab_Height.AutoSize = true;
            this.Lab_Height.Location = new System.Drawing.Point(8, 62);
            this.Lab_Height.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lab_Height.Name = "Lab_Height";
            this.Lab_Height.Size = new System.Drawing.Size(71, 15);
            this.Lab_Height.TabIndex = 2;
            this.Lab_Height.Text = "Maximum:";
            // 
            // TB_BarWidth
            // 
            this.TB_BarWidth.Location = new System.Drawing.Point(87, 92);
            this.TB_BarWidth.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TB_BarWidth.Name = "TB_BarWidth";
            this.TB_BarWidth.Size = new System.Drawing.Size(85, 25);
            this.TB_BarWidth.TabIndex = 1;
            // 
            // Lab_Width
            // 
            this.Lab_Width.AutoSize = true;
            this.Lab_Width.Location = new System.Drawing.Point(8, 96);
            this.Lab_Width.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lab_Width.Name = "Lab_Width";
            this.Lab_Width.Size = new System.Drawing.Size(79, 15);
            this.Lab_Width.TabIndex = 0;
            this.Lab_Width.Text = "BarWidth:";
            // 
            // CLB_Fields
            // 
            this.CLB_Fields.CheckOnClick = true;
            this.CLB_Fields.FormattingEnabled = true;
            this.CLB_Fields.Location = new System.Drawing.Point(11, 66);
            this.CLB_Fields.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CLB_Fields.Name = "CLB_Fields";
            this.CLB_Fields.Size = new System.Drawing.Size(217, 184);
            this.CLB_Fields.TabIndex = 2;
            this.CLB_Fields.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CLB_Fields_ItemCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Chart Type:";
            // 
            // CB_ChartType
            // 
            this.CB_ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_ChartType.FormattingEnabled = true;
            this.CB_ChartType.Location = new System.Drawing.Point(125, 21);
            this.CB_ChartType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CB_ChartType.Name = "CB_ChartType";
            this.CB_ChartType.Size = new System.Drawing.Size(199, 23);
            this.CB_ChartType.TabIndex = 0;
            this.CB_ChartType.SelectedIndexChanged += new System.EventHandler(this.CB_ChartType_SelectedIndexChanged);
            // 
            // B_Apply
            // 
            this.B_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_Apply.Location = new System.Drawing.Point(41, 502);
            this.B_Apply.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.B_Apply.Name = "B_Apply";
            this.B_Apply.Size = new System.Drawing.Size(80, 30);
            this.B_Apply.TabIndex = 31;
            this.B_Apply.Text = "Apply";
            this.B_Apply.UseVisualStyleBackColor = true;
            this.B_Apply.Click += new System.EventHandler(this.B_Apply_Click);
            // 
            // B_OK
            // 
            this.B_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_OK.Location = new System.Drawing.Point(169, 502);
            this.B_OK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.B_OK.Name = "B_OK";
            this.B_OK.Size = new System.Drawing.Size(80, 30);
            this.B_OK.TabIndex = 30;
            this.B_OK.Text = "OK";
            this.B_OK.UseVisualStyleBackColor = true;
            this.B_OK.Click += new System.EventHandler(this.B_OK_Click);
            // 
            // B_Cancel
            // 
            this.B_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_Cancel.Location = new System.Drawing.Point(301, 502);
            this.B_Cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.B_Cancel.Name = "B_Cancel";
            this.B_Cancel.Size = new System.Drawing.Size(80, 30);
            this.B_Cancel.TabIndex = 29;
            this.B_Cancel.Text = "Cancel";
            this.B_Cancel.UseVisualStyleBackColor = true;
            this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
            // 
            // CHB_View3D
            // 
            this.CHB_View3D.AutoSize = true;
            this.CHB_View3D.Location = new System.Drawing.Point(18, 24);
            this.CHB_View3D.Name = "CHB_View3D";
            this.CHB_View3D.Size = new System.Drawing.Size(141, 19);
            this.CHB_View3D.TabIndex = 36;
            this.CHB_View3D.Text = "Display in 3-D";
            this.CHB_View3D.UseVisualStyleBackColor = true;
            // 
            // legendSchemeControl1
            // 
            this.legendSchemeControl1.Location = new System.Drawing.Point(4, 1);
            this.legendSchemeControl1.Margin = new System.Windows.Forms.Padding(5);
            this.legendSchemeControl1.Name = "legendSchemeControl1";
            this.legendSchemeControl1.Size = new System.Drawing.Size(420, 455);
            this.legendSchemeControl1.TabIndex = 1;
            // 
            // legendView_Chart
            // 
            this.legendView_Chart.BackColor = System.Drawing.Color.White;
            this.legendView_Chart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.legendView_Chart.LegendScheme = null;
            this.legendView_Chart.Location = new System.Drawing.Point(11, 271);
            this.legendView_Chart.Margin = new System.Windows.Forms.Padding(5);
            this.legendView_Chart.Name = "legendView_Chart";
            this.legendView_Chart.Size = new System.Drawing.Size(217, 173);
            this.legendView_Chart.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TB_Thickness);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.CHB_View3D);
            this.groupBox1.Location = new System.Drawing.Point(237, 349);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 94);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "3D";
            // 
            // TB_Thickness
            // 
            this.TB_Thickness.Location = new System.Drawing.Point(103, 58);
            this.TB_Thickness.Margin = new System.Windows.Forms.Padding(4);
            this.TB_Thickness.Name = "TB_Thickness";
            this.TB_Thickness.Size = new System.Drawing.Size(69, 25);
            this.TB_Thickness.TabIndex = 38;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 62);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 15);
            this.label5.TabIndex = 37;
            this.label5.Text = "Thickness:";
            // 
            // frmLayerProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 544);
            this.Controls.Add(this.B_Apply);
            this.Controls.Add(this.B_OK);
            this.Controls.Add(this.B_Cancel);
            this.Controls.Add(this.TabControl_Prop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLayerProperty";
            this.ShowIcon = false;
            this.Text = "Layer Property";
            this.Load += new System.EventHandler(this.frmLayerProperty_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLayerProperty_FormClosed);
            this.TabControl_Prop.ResumeLayout(false);
            this.TabPage_General.ResumeLayout(false);
            this.TabPage_Legend.ResumeLayout(false);
            this.TabPage_Chart.ResumeLayout(false);
            this.TabPage_Chart.PerformLayout();
            this.GB_Size.ResumeLayout(false);
            this.GB_Size.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabControl_Prop;
        private System.Windows.Forms.TabPage TabPage_General;
        private System.Windows.Forms.TabPage TabPage_Legend;
        private System.Windows.Forms.PropertyGrid PG_General;
        internal System.Windows.Forms.Button B_Apply;
        internal System.Windows.Forms.Button B_OK;
        internal System.Windows.Forms.Button B_Cancel;
        private MeteoInfoC.Legend.LegendSchemeControl legendSchemeControl1;
        private System.Windows.Forms.TabPage TabPage_Chart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CB_ChartType;
        private System.Windows.Forms.CheckedListBox CLB_Fields;
        private System.Windows.Forms.GroupBox GB_Size;
        private System.Windows.Forms.TextBox TB_MaxSize;
        private System.Windows.Forms.Label Lab_Height;
        private System.Windows.Forms.TextBox TB_BarWidth;
        private System.Windows.Forms.Label Lab_Width;
        private MeteoInfoC.Legend.LegendView legendView_Chart;
        private System.Windows.Forms.TextBox TB_MinSize;
        private System.Windows.Forms.Label Lab_MinSize;
        private System.Windows.Forms.TextBox TB_XShift;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TB_YShift;
        private System.Windows.Forms.Label label3;
        internal System.Windows.Forms.CheckBox CHB_Collision;
        internal System.Windows.Forms.ComboBox CB_Align;
        internal System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox CHB_View3D;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TB_Thickness;
        private System.Windows.Forms.Label label5;
    }
}