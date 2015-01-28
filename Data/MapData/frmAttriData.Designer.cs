namespace MeteoInfoC.Data.MapData
{
    partial class frmAttriData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAttriData));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.TSMI_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_StartEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_StopEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMI_AddField = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_RemoveField = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMI_RenameField = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.QueryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_FieldAttribute = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AccessibleDescription = null;
            this.dataGridView1.AccessibleName = null;
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.BackgroundImage = null;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Font = null;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // menuStrip2
            // 
            this.menuStrip2.AccessibleDescription = null;
            this.menuStrip2.AccessibleName = null;
            resources.ApplyResources(this.menuStrip2, "menuStrip2");
            this.menuStrip2.BackgroundImage = null;
            this.menuStrip2.Font = null;
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Edit,
            this.SelectionToolStripMenuItem});
            this.menuStrip2.Name = "menuStrip2";
            // 
            // TSMI_Edit
            // 
            this.TSMI_Edit.AccessibleDescription = null;
            this.TSMI_Edit.AccessibleName = null;
            resources.ApplyResources(this.TSMI_Edit, "TSMI_Edit");
            this.TSMI_Edit.BackgroundImage = null;
            this.TSMI_Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_StartEdit,
            this.TSMI_StopEdit,
            this.ToolStripSeparator2,
            this.TSMI_AddField,
            this.TSMI_RemoveField,
            this.ToolStripSeparator1,
            this.TSMI_RenameField});
            this.TSMI_Edit.Name = "TSMI_Edit";
            this.TSMI_Edit.ShortcutKeyDisplayString = null;
            // 
            // TSMI_StartEdit
            // 
            this.TSMI_StartEdit.AccessibleDescription = null;
            this.TSMI_StartEdit.AccessibleName = null;
            resources.ApplyResources(this.TSMI_StartEdit, "TSMI_StartEdit");
            this.TSMI_StartEdit.BackgroundImage = null;
            this.TSMI_StartEdit.Name = "TSMI_StartEdit";
            this.TSMI_StartEdit.ShortcutKeyDisplayString = null;
            this.TSMI_StartEdit.Click += new System.EventHandler(this.TSMI_StartEdit_Click);
            // 
            // TSMI_StopEdit
            // 
            this.TSMI_StopEdit.AccessibleDescription = null;
            this.TSMI_StopEdit.AccessibleName = null;
            resources.ApplyResources(this.TSMI_StopEdit, "TSMI_StopEdit");
            this.TSMI_StopEdit.BackgroundImage = null;
            this.TSMI_StopEdit.Name = "TSMI_StopEdit";
            this.TSMI_StopEdit.ShortcutKeyDisplayString = null;
            this.TSMI_StopEdit.Click += new System.EventHandler(this.TSMI_StopEdit_Click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.AccessibleDescription = null;
            this.ToolStripSeparator2.AccessibleName = null;
            resources.ApplyResources(this.ToolStripSeparator2, "ToolStripSeparator2");
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            // 
            // TSMI_AddField
            // 
            this.TSMI_AddField.AccessibleDescription = null;
            this.TSMI_AddField.AccessibleName = null;
            resources.ApplyResources(this.TSMI_AddField, "TSMI_AddField");
            this.TSMI_AddField.BackgroundImage = null;
            this.TSMI_AddField.Name = "TSMI_AddField";
            this.TSMI_AddField.ShortcutKeyDisplayString = null;
            this.TSMI_AddField.Click += new System.EventHandler(this.TSMI_AddField_Click);
            // 
            // TSMI_RemoveField
            // 
            this.TSMI_RemoveField.AccessibleDescription = null;
            this.TSMI_RemoveField.AccessibleName = null;
            resources.ApplyResources(this.TSMI_RemoveField, "TSMI_RemoveField");
            this.TSMI_RemoveField.BackgroundImage = null;
            this.TSMI_RemoveField.Name = "TSMI_RemoveField";
            this.TSMI_RemoveField.ShortcutKeyDisplayString = null;
            this.TSMI_RemoveField.Click += new System.EventHandler(this.TSMI_RemoveField_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.AccessibleDescription = null;
            this.ToolStripSeparator1.AccessibleName = null;
            resources.ApplyResources(this.ToolStripSeparator1, "ToolStripSeparator1");
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            // 
            // TSMI_RenameField
            // 
            this.TSMI_RenameField.AccessibleDescription = null;
            this.TSMI_RenameField.AccessibleName = null;
            resources.ApplyResources(this.TSMI_RenameField, "TSMI_RenameField");
            this.TSMI_RenameField.BackgroundImage = null;
            this.TSMI_RenameField.Name = "TSMI_RenameField";
            this.TSMI_RenameField.ShortcutKeyDisplayString = null;
            this.TSMI_RenameField.Click += new System.EventHandler(this.TSMI_RenameField_Click);
            // 
            // SelectionToolStripMenuItem
            // 
            this.SelectionToolStripMenuItem.AccessibleDescription = null;
            this.SelectionToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.SelectionToolStripMenuItem, "SelectionToolStripMenuItem");
            this.SelectionToolStripMenuItem.BackgroundImage = null;
            this.SelectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.QueryToolStripMenuItem,
            this.TSMI_FieldAttribute});
            this.SelectionToolStripMenuItem.Name = "SelectionToolStripMenuItem";
            this.SelectionToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // QueryToolStripMenuItem
            // 
            this.QueryToolStripMenuItem.AccessibleDescription = null;
            this.QueryToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.QueryToolStripMenuItem, "QueryToolStripMenuItem");
            this.QueryToolStripMenuItem.BackgroundImage = null;
            this.QueryToolStripMenuItem.Name = "QueryToolStripMenuItem";
            this.QueryToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // TSMI_FieldAttribute
            // 
            this.TSMI_FieldAttribute.AccessibleDescription = null;
            this.TSMI_FieldAttribute.AccessibleName = null;
            resources.ApplyResources(this.TSMI_FieldAttribute, "TSMI_FieldAttribute");
            this.TSMI_FieldAttribute.BackgroundImage = null;
            this.TSMI_FieldAttribute.Name = "TSMI_FieldAttribute";
            this.TSMI_FieldAttribute.ShortcutKeyDisplayString = null;
            // 
            // frmAttriData
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip2);
            this.Font = null;
            this.Icon = null;
            this.Name = "frmAttriData";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frmAttriData_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAttriData_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        internal System.Windows.Forms.MenuStrip menuStrip2;
        internal System.Windows.Forms.ToolStripMenuItem TSMI_Edit;
        internal System.Windows.Forms.ToolStripMenuItem TSMI_StartEdit;
        internal System.Windows.Forms.ToolStripMenuItem TSMI_StopEdit;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        internal System.Windows.Forms.ToolStripMenuItem TSMI_AddField;
        internal System.Windows.Forms.ToolStripMenuItem TSMI_RemoveField;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripMenuItem TSMI_RenameField;
        internal System.Windows.Forms.ToolStripMenuItem SelectionToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem QueryToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TSMI_FieldAttribute;
    }
}