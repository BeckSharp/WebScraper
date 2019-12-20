namespace Web_Scraper
{
    partial class WebScrapeDisplay
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
            this.StartCodeButton = new System.Windows.Forms.Button();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // StartCodeButton
            // 
            this.StartCodeButton.Location = new System.Drawing.Point(23, 657);
            this.StartCodeButton.Name = "StartCodeButton";
            this.StartCodeButton.Size = new System.Drawing.Size(896, 121);
            this.StartCodeButton.TabIndex = 0;
            this.StartCodeButton.Text = "Start";
            this.StartCodeButton.UseVisualStyleBackColor = true;
            // 
            // richTextBox
            // 
            this.richTextBox.Location = new System.Drawing.Point(23, 39);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(896, 612);
            this.richTextBox.TabIndex = 1;
            this.richTextBox.Text = "";
            // 
            // WebScrapeDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 801);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.StartCodeButton);
            this.Name = "WebScrapeDisplay";
            this.Style = MetroFramework.MetroColorStyle.Blue;
            this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Center;
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartCodeButton;
        private System.Windows.Forms.RichTextBox richTextBox;
    }
}

