namespace MeteoInfoC.Legend
{
    partial class frmColorSymbolSet
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
            this.B_FillColor = new System.Windows.Forms.Button();
            this.Lab_FillColor = new System.Windows.Forms.Label();
            this.NUD_TransparencyPerc = new System.Windows.Forms.NumericUpDown();
            this.Lab_Trans = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_TransparencyPerc)).BeginInit();
            this.SuspendLayout();
            // 
            // B_FillColor
            // 
            this.B_FillColor.Location = new System.Drawing.Point(99, 10);
            this.B_FillColor.Name = "B_FillColor";
            this.B_FillColor.Size = new System.Drawing.Size(57, 23);
            this.B_FillColor.TabIndex = 41;
            this.B_FillColor.UseVisualStyleBackColor = true;
            this.B_FillColor.Click += new System.EventHandler(this.B_FillColor_Click);
            // 
            // Lab_FillColor
            // 
            this.Lab_FillColor.AutoSize = true;
            this.Lab_FillColor.Location = new System.Drawing.Point(52, 16);
            this.Lab_FillColor.Name = "Lab_FillColor";
            this.Lab_FillColor.Size = new System.Drawing.Size(41, 12);
            this.Lab_FillColor.TabIndex = 40;
            this.Lab_FillColor.Text = "Color:";
            // 
            // NUD_TransparencyPerc
            // 
            this.NUD_TransparencyPerc.Location = new System.Drawing.Point(99, 50);
            this.NUD_TransparencyPerc.Name = "NUD_TransparencyPerc";
            this.NUD_TransparencyPerc.Size = new System.Drawing.Size(60, 21);
            this.NUD_TransparencyPerc.TabIndex = 50;
            this.NUD_TransparencyPerc.ValueChanged += new System.EventHandler(this.NUD_TransparencyPerc_ValueChanged);
            // 
            // Lab_Trans
            // 
            this.Lab_Trans.AutoSize = true;
            this.Lab_Trans.Location = new System.Drawing.Point(12, 52);
            this.Lab_Trans.Name = "Lab_Trans";
            this.Lab_Trans.Size = new System.Drawing.Size(83, 12);
            this.Lab_Trans.TabIndex = 49;
            this.Lab_Trans.Text = "TransPerency:";
            // 
            // frmColorSymbolSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(181, 91);
            this.Controls.Add(this.NUD_TransparencyPerc);
            this.Controls.Add(this.Lab_Trans);
            this.Controls.Add(this.B_FillColor);
            this.Controls.Add(this.Lab_FillColor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmColorSymbolSet";
            this.Text = "Color Symbol Set";
            this.Load += new System.EventHandler(this.frmColorSymbolSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NUD_TransparencyPerc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button B_FillColor;
        private System.Windows.Forms.Label Lab_FillColor;
        private System.Windows.Forms.NumericUpDown NUD_TransparencyPerc;
        private System.Windows.Forms.Label Lab_Trans;
    }
}