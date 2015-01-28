namespace MeteoInfoC.Legend
{
    partial class SymbolControl
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
            this._vScrollBar = new System.Windows.Forms.VScrollBar();
            this.SuspendLayout();
            // 
            // _vScrollBar
            // 
            this._vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this._vScrollBar.Location = new System.Drawing.Point(240, 0);
            this._vScrollBar.Name = "_vScrollBar";
            this._vScrollBar.Size = new System.Drawing.Size(17, 246);
            this._vScrollBar.TabIndex = 0;
            this._vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this._vScrollBar_Scroll);
            // 
            // MakerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._vScrollBar);
            this.Name = "MakerControl";
            this.Size = new System.Drawing.Size(257, 246);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.VScrollBar _vScrollBar;
    }
}
