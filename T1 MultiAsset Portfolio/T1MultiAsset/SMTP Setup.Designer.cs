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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lb_Port = new System.Windows.Forms.Label();
            this.tb_Port = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_Password = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_UserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tb_Port_Secondary = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_Password_Secondary = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tb_UserName_Secondary = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.bt_Test_Secondary = new System.Windows.Forms.Button();
            this.cb_SSL_Secondary = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_SmtpClient_Secondary = new System.Windows.Forms.TextBox();
            this.cb_DomainAutheticate = new System.Windows.Forms.CheckBox();
            this.cb_DomainAutheticate_Secondary = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bt_Test
            // 
            this.bt_Test.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Test.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Test.ForeColor = System.Drawing.Color.Green;
            this.bt_Test.Location = new System.Drawing.Point(265, 15);
            this.bt_Test.Name = "bt_Test";
            this.bt_Test.Size = new System.Drawing.Size(72, 32);
            this.bt_Test.TabIndex = 0;
            this.bt_Test.Text = "Test";
            this.bt_Test.UseVisualStyleBackColor = true;
            this.bt_Test.Click += new System.EventHandler(this.bt_Test_Click);
            // 
            // bt_Save
            // 
            this.bt_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Save.ForeColor = System.Drawing.Color.Green;
            this.bt_Save.Location = new System.Drawing.Point(277, 581);
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
            this.tb_SmtpClient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_SmtpClient.Location = new System.Drawing.Point(9, 31);
            this.tb_SmtpClient.Name = "tb_SmtpClient";
            this.tb_SmtpClient.Size = new System.Drawing.Size(233, 20);
            this.tb_SmtpClient.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(201, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "smtp server [eg. mail.google.com ]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(19, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Test Email Address";
            // 
            // tb_email
            // 
            this.tb_email.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_email.Location = new System.Drawing.Point(22, 105);
            this.tb_email.Name = "tb_email";
            this.tb_email.Size = new System.Drawing.Size(201, 20);
            this.tb_email.TabIndex = 5;
            // 
            // cb_SSL
            // 
            this.cb_SSL.AutoSize = true;
            this.cb_SSL.Location = new System.Drawing.Point(9, 57);
            this.cb_SSL.Name = "cb_SSL";
            this.cb_SSL.Size = new System.Drawing.Size(82, 17);
            this.cb_SSL.TabIndex = 7;
            this.cb_SSL.Text = "Enable SSL";
            this.cb_SSL.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cb_DomainAutheticate);
            this.groupBox1.Controls.Add(this.lb_Port);
            this.groupBox1.Controls.Add(this.tb_Port);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tb_Password);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.tb_UserName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cb_SSL);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tb_SmtpClient);
            this.groupBox1.Controls.Add(this.bt_Test);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.DarkGreen;
            this.groupBox1.Location = new System.Drawing.Point(13, 143);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(343, 207);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "STMP Primary Server";
            // 
            // lb_Port
            // 
            this.lb_Port.AutoSize = true;
            this.lb_Port.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Port.Location = new System.Drawing.Point(129, 59);
            this.lb_Port.Name = "lb_Port";
            this.lb_Port.Size = new System.Drawing.Size(29, 13);
            this.lb_Port.TabIndex = 14;
            this.lb_Port.Text = "Port:";
            this.lb_Port.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tb_Port
            // 
            this.tb_Port.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_Port.Location = new System.Drawing.Point(164, 56);
            this.tb_Port.Name = "tb_Port";
            this.tb_Port.Size = new System.Drawing.Size(75, 20);
            this.tb_Port.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 162);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Password";
            // 
            // tb_Password
            // 
            this.tb_Password.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_Password.Location = new System.Drawing.Point(8, 178);
            this.tb_Password.Name = "tb_Password";
            this.tb_Password.PasswordChar = '*';
            this.tb_Password.Size = new System.Drawing.Size(233, 20);
            this.tb_Password.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(4, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "UserName";
            // 
            // tb_UserName
            // 
            this.tb_UserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_UserName.Location = new System.Drawing.Point(6, 139);
            this.tb_UserName.Name = "tb_UserName";
            this.tb_UserName.Size = new System.Drawing.Size(233, 20);
            this.tb_UserName.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(6, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(281, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "It is advised NOT to supply Username/Password";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cb_DomainAutheticate_Secondary);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.tb_Port_Secondary);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.tb_Password_Secondary);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.tb_UserName_Secondary);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.bt_Test_Secondary);
            this.groupBox2.Controls.Add(this.cb_SSL_Secondary);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tb_SmtpClient_Secondary);
            this.groupBox2.ForeColor = System.Drawing.Color.RoyalBlue;
            this.groupBox2.Location = new System.Drawing.Point(13, 366);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(343, 207);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "STMP Secondary Server";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(128, 60);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "Port:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tb_Port_Secondary
            // 
            this.tb_Port_Secondary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_Port_Secondary.Location = new System.Drawing.Point(163, 57);
            this.tb_Port_Secondary.Name = "tb_Port_Secondary";
            this.tb_Port_Secondary.Size = new System.Drawing.Size(75, 20);
            this.tb_Port_Secondary.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(8, 163);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Password";
            // 
            // tb_Password_Secondary
            // 
            this.tb_Password_Secondary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_Password_Secondary.Location = new System.Drawing.Point(10, 179);
            this.tb_Password_Secondary.Name = "tb_Password_Secondary";
            this.tb_Password_Secondary.PasswordChar = '*';
            this.tb_Password_Secondary.Size = new System.Drawing.Size(233, 20);
            this.tb_Password_Secondary.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 124);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "UserName";
            // 
            // tb_UserName_Secondary
            // 
            this.tb_UserName_Secondary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_UserName_Secondary.Location = new System.Drawing.Point(8, 140);
            this.tb_UserName_Secondary.Name = "tb_UserName_Secondary";
            this.tb_UserName_Secondary.Size = new System.Drawing.Size(233, 20);
            this.tb_UserName_Secondary.TabIndex = 14;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(8, 106);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(281, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "It is advised NOT to supply Username/Password";
            // 
            // bt_Test_Secondary
            // 
            this.bt_Test_Secondary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Test_Secondary.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Test_Secondary.ForeColor = System.Drawing.Color.RoyalBlue;
            this.bt_Test_Secondary.Location = new System.Drawing.Point(264, 15);
            this.bt_Test_Secondary.Name = "bt_Test_Secondary";
            this.bt_Test_Secondary.Size = new System.Drawing.Size(72, 32);
            this.bt_Test_Secondary.TabIndex = 10;
            this.bt_Test_Secondary.Text = "Test";
            this.bt_Test_Secondary.UseVisualStyleBackColor = true;
            this.bt_Test_Secondary.Click += new System.EventHandler(this.bt_Test_Secondary_Click);
            // 
            // cb_SSL_Secondary
            // 
            this.cb_SSL_Secondary.AutoSize = true;
            this.cb_SSL_Secondary.Location = new System.Drawing.Point(9, 57);
            this.cb_SSL_Secondary.Name = "cb_SSL_Secondary";
            this.cb_SSL_Secondary.Size = new System.Drawing.Size(82, 17);
            this.cb_SSL_Secondary.TabIndex = 7;
            this.cb_SSL_Secondary.Text = "Enable SSL";
            this.cb_SSL_Secondary.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(16, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(201, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "smtp server [eg. mail.google.com ]";
            // 
            // tb_SmtpClient_Secondary
            // 
            this.tb_SmtpClient_Secondary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_SmtpClient_Secondary.Location = new System.Drawing.Point(9, 31);
            this.tb_SmtpClient_Secondary.Name = "tb_SmtpClient_Secondary";
            this.tb_SmtpClient_Secondary.Size = new System.Drawing.Size(233, 20);
            this.tb_SmtpClient_Secondary.TabIndex = 3;
            // 
            // cb_DomainAutheticate
            // 
            this.cb_DomainAutheticate.AutoSize = true;
            this.cb_DomainAutheticate.Location = new System.Drawing.Point(9, 80);
            this.cb_DomainAutheticate.Name = "cb_DomainAutheticate";
            this.cb_DomainAutheticate.Size = new System.Drawing.Size(125, 17);
            this.cb_DomainAutheticate.TabIndex = 15;
            this.cb_DomainAutheticate.Text = "Domain Authenticate";
            this.cb_DomainAutheticate.UseVisualStyleBackColor = true;
            // 
            // cb_DomainAutheticate_Secondary
            // 
            this.cb_DomainAutheticate_Secondary.AutoSize = true;
            this.cb_DomainAutheticate_Secondary.Location = new System.Drawing.Point(9, 80);
            this.cb_DomainAutheticate_Secondary.Name = "cb_DomainAutheticate_Secondary";
            this.cb_DomainAutheticate_Secondary.Size = new System.Drawing.Size(125, 17);
            this.cb_DomainAutheticate_Secondary.TabIndex = 20;
            this.cb_DomainAutheticate_Secondary.Text = "Domain Authenticate";
            this.cb_DomainAutheticate_Secondary.UseVisualStyleBackColor = true;
            // 
            // SMTPSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 617);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_email);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bt_Save);
            this.Name = "SMTPSetup";
            this.Text = "SMTP Setup";
            this.Load += new System.EventHandler(this.SMTPSetup_Load);
            this.Shown += new System.EventHandler(this.SMTPSetup_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button bt_Test_Secondary;
        private System.Windows.Forms.CheckBox cb_SSL_Secondary;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_SmtpClient_Secondary;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_Password;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_UserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_Password_Secondary;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb_UserName_Secondary;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lb_Port;
        private System.Windows.Forms.TextBox tb_Port;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tb_Port_Secondary;
        private System.Windows.Forms.CheckBox cb_DomainAutheticate;
        private System.Windows.Forms.CheckBox cb_DomainAutheticate_Secondary;
    }
}