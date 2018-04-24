namespace FinaleLib
{
    partial class Finale
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
            this.browser = new System.Windows.Forms.WebBrowser();
            this.LuckyNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // browser
            // 
            this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.browser.Name = "browser";
            this.browser.Size = new System.Drawing.Size(800, 450);
            this.browser.TabIndex = 0;
            this.browser.Visible = false;
            // 
            // LuckyNumber
            // 
            this.LuckyNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 70F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LuckyNumber.Location = new System.Drawing.Point(0, 0);
            this.LuckyNumber.Name = "LuckyNumber";
            this.LuckyNumber.Size = new System.Drawing.Size(800, 450);
            this.LuckyNumber.TabIndex = 1;
            this.LuckyNumber.Text = "kjh";
            this.LuckyNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Finale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LuckyNumber);
            this.Controls.Add(this.browser);
            this.Name = "Finale";
            this.Text = "Finale";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser browser;
        private System.Windows.Forms.Label LuckyNumber;
    }
}