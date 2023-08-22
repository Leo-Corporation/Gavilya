namespace Gavilya.Fps{
    partial class FpsCounter
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FpsCounter));
			this.WindowHandle = new System.Windows.Forms.Timer(this.components);
			this.FpsLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// WindowHandle
			// 
			this.WindowHandle.Enabled = true;
			this.WindowHandle.Interval = 50;
			this.WindowHandle.Tick += new System.EventHandler(this.WindowHandle_Tick);
			// 
			// FpsLabel
			// 
			this.FpsLabel.AutoSize = true;
			this.FpsLabel.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FpsLabel.ForeColor = System.Drawing.Color.White;
			this.FpsLabel.Location = new System.Drawing.Point(0, 0);
			this.FpsLabel.Margin = new System.Windows.Forms.Padding(0);
			this.FpsLabel.Name = "FpsLabel";
			this.FpsLabel.Size = new System.Drawing.Size(0, 40);
			this.FpsLabel.TabIndex = 10;
			this.FpsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FpsCounter
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(30)))));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(120, 14);
			this.Controls.Add(this.FpsLabel);
			this.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "FpsCounter";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer WindowHandle;
        public System.Windows.Forms.Label FpsLabel;
    }
}

