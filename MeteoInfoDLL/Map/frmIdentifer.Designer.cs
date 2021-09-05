namespace MeteoInfoC.Map
{
    partial class frmIdentifer
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
            this.ListView1 = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // ListView1
            // 
            this.ListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListView1.GridLines = true;
            this.ListView1.Location = new System.Drawing.Point(0, 0);
            this.ListView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ListView1.Name = "ListView1";
            this.ListView1.Size = new System.Drawing.Size(192, 200);
            this.ListView1.TabIndex = 3;
            this.ListView1.UseCompatibleStateImageBehavior = false;
            // 
            // frmIdentifer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(192, 200);
            this.Controls.Add(this.ListView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "frmIdentifer";
            this.Text = "Identifer";
            this.Load += new System.EventHandler(this.frmIdentifer_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmIdentifer_FormClosed);
            this.Resize += new System.EventHandler(this.frmIdentifer_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.ListView ListView1;
    }
}