namespace T1MultiAsset
{
    partial class SMTPSetup
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
            this.bt_Test = new System.Windows.Forms.Button();
            this.bt_Save = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_SmtpClient = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_email = new System.Windows.Forms.TextBox();
            this.cb_SSL = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // bt_Test
            // 
            this.bt_Test.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Test.ForeColor = System.Drawing.Color.Green;
            this.bt_Test.Location = new System.Drawing.Point(333, 104);
            this.bt_Test.Name = "bt_Test";
            this.bt_Test.Size = new System.Drawing.Size(72, 32);
            this.bt_Test.TabIndex = 0;
            this.bt_Test.Text = "Test";
            this.bt_Test.UseVisualStyleBackColor = true;
            this.bt_Test.Click += new System.EventHandler(this.bt_Test_Click);
            // 
            // bt_Save
            // 
            this.bt_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Save.ForeColor = System.Drawing.Color.Green;
            this.bt_Save.Location = new System.Drawing.Point(333, 290);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(72, 32);
            this.bt_Save.TabIndex = 1;
            this.bt_Save.Text = "Save";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(393, 65);
            this.label1.TabIndex = 2;
            this.label1.Text = "Set up SMTP for outgoing email\r\n\r\nEnter a valid smtp address and press the [Test]" +
                " button\r\nIf the Test works, then press the [Save] button.";
            // 
            // tb_SmtpClient
            // 
            this.tb_SmtpClient.Location = new System.Drawing.Point(21, 111);
            this.tb_SmtpClient.Name = "tb_SmtpClient";
            this.tb_SmtpClient.Size = new System.Drawing.Size(252, 20);
            this.tb_SmtpClient.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(28, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(201, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "smtp server [eg. mail.google.com ]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Test Email Address";
            // 
            // tb_email
            // 
            this.tb_email.Location = new System.Drawing.Point(21, 177);
            this.tb_email.Name = "tb_email";
            this.tb_email.Size = new System.Drawing.Size(215, 20);
            this.tb_email.TabIndex = 5;
            // 
            // cb_SSL
            // 
            this.cb_SSL.AutoSize = true;
            this.cb_SSL.Location = new System.Drawing.Point(21, 203);
            this.cb_SSL.Name = "cb_SSL";
            this.cb_SSL.Size = new System.Drawing.Size(82, 17);
            this.cb_SSL.TabIndex = 7;
            this.cb_SSL.Text = "Enable SSL";
            this.cb_SSL.UseVisualStyleBackColor = true;
            // 
            // SMTPSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 334);
            this.Controls.Add(this.cb_SSL);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_email);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_SmtpClient);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.bt_Test);
            this.Name = "SMTPSetup";
            this.Text = "SMTP Setup";
            this.Load += new System.EventHandler(this.SMTPSetup_Load);
            this.Shown += new System.EventHandler(this.SMTPSetup_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_Test;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_SmtpClient;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_email;
        private System.Windows.Forms.CheckBox cb_SSL;
    }
}