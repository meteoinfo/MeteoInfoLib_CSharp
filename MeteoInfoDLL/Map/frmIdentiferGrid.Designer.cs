namespace MeteoInfoC.Map
{
    partial class frmIdentiferGrid
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
            this.Lab_I = new System.Windows.Forms.Label();
            this.Lab_J = new System.Windows.Forms.Label();
            this.Lab_CellValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Lab_I
            // 
            this.Lab_I.AutoSize = true;
            this.Lab_I.Location = new System.Drawing.Point(19, 18);
            this.Lab_I.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lab_I.Name = "Lab_I";
            this.Lab_I.Size = new System.Drawing.Size(17, 12);
            this.Lab_I.TabIndex = 0;
            this.Lab_I.Text = "I=";
            // 
            // Lab_J
            // 
            this.Lab_J.AutoSize = true;
            this.Lab_J.Location = new System.Drawing.Point(90, 18);
            this.Lab_J.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lab_J.Name = "Lab_J";
            this.Lab_J.Size = new System.Drawing.Size(17, 12);
            this.Lab_J.TabIndex = 1;
            this.Lab_J.Text = "J=";
            // 
            // Lab_CellValue
            // 
            this.Lab_CellValue.AutoSize = true;
            this.Lab_CellValue.Location = new System.Drawing.Point(19, 54);
            this.Lab_CellValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lab_CellValue.Name = "Lab_CellValue";
            this.Lab_CellValue.Size = new System.Drawing.Size(71, 12);
            this.Lab_CellValue.TabIndex = 2;
            this.Lab_CellValue.Text = "Cell Value:";
            // 
            // frmIdentiferGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(186, 89);
            this.Controls.Add(this.Lab_CellValue);
            this.Controls.Add(this.Lab_J);
            this.Controls.Add(this.Lab_I);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmIdentiferGrid";
            this.Text = "Identifer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmIdentiferGrid_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Lab_I;
        internal System.Windows.Forms.Label Lab_J;
        internal System.Windows.Forms.Label Lab_CellValue;
    }
}