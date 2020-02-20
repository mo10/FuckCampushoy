namespace FuckCampushoy
{
    partial class Login
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
            this.qrcode = new System.Windows.Forms.PictureBox();
            this.notice = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.qrcode)).BeginInit();
            this.SuspendLayout();
            // 
            // qrcode
            // 
            this.qrcode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.qrcode.Location = new System.Drawing.Point(12, 12);
            this.qrcode.Name = "qrcode";
            this.qrcode.Size = new System.Drawing.Size(300, 300);
            this.qrcode.TabIndex = 0;
            this.qrcode.TabStop = false;
            // 
            // notice
            // 
            this.notice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.notice.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.notice.Location = new System.Drawing.Point(12, 315);
            this.notice.Name = "notice";
            this.notice.Size = new System.Drawing.Size(300, 23);
            this.notice.TabIndex = 1;
            this.notice.Text = "请使用今日校园APP扫码登陆";
            this.notice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(12, 341);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(296, 30);
            this.button1.TabIndex = 2;
            this.button1.Text = "刷新二维码";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.notice);
            this.Controls.Add(this.qrcode);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(336, 489);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.qrcode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox qrcode;
        private System.Windows.Forms.Label notice;
        private System.Windows.Forms.Button button1;
    }
}