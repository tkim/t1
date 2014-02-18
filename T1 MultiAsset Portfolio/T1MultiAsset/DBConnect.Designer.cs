namespace T1MultiAsset
{
    partial class frm_DBConnect
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
            this.tb_ServerName = new System.Windows.Forms.TextBox();
            this.cb_Type = new System.Windows.Forms.ComboBox();
            this.lb_Type = new System.Windows.Forms.Label();
            this.tb_ProviderName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_Database = new System.Windows.Forms.Label();
            this.tb_DatabaseName = new System.Windows.Forms.TextBox();
            this.lb_ServerName = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bt_Browse = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_Microsoft_Authentification = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_DBPwd = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_DBUser = new System.Windows.Forms.TextBox();
            this.bt_Test = new System.Windows.Forms.Button();
            this.bt_Ok = new System.Windows.Forms.Button();
            this.bt_Cancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_ServerName
            // 
            this.tb_ServerName.Location = new System.Drawing.Point(9, 139);
            this.tb_ServerName.Name = "tb_ServerName";
            this.tb_ServerName.Size = new System.Drawing.Size(217, 20);
            this.tb_ServerName.TabIndex = 3;
            // 
            // cb_Type
            // 
            this.cb_Type.ForeColor = System.Drawing.Color.MediumBlue;
            this.cb_Type.FormattingEnabled = true;
            this.cb_Type.Items.AddRange(new object[] {
            "SQL Server",
            "Azure",
            "Oracle",
            "Access",
            "MySql"});
            this.cb_Type.Location = new System.Drawing.Point(57, 6);
            this.cb_Type.Name = "cb_Type";
            this.cb_Type.Size = new System.Drawing.Size(212, 21);
            this.cb_Type.TabIndex = 0;
            this.cb_Type.SelectedIndexChanged += new System.EventHandler(this.cb_Type_SelectedIndexChanged);
            // 
            // lb_Type
            // 
            this.lb_Type.AutoSize = true;
            this.lb_Type.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Type.ForeColor = System.Drawing.Color.DarkBlue;
            this.lb_Type.Location = new System.Drawing.Point(12, 9);
            this.lb_Type.Name = "lb_Type";
            this.lb_Type.Size = new System.Drawing.Size(39, 13);
            this.lb_Type.TabIndex = 1;
            this.lb_Type.Text = "Type:";
            // 
            // tb_ProviderName
            // 
            this.tb_ProviderName.Location = new System.Drawing.Point(9, 41);
            this.tb_ProviderName.Name = "tb_ProviderName";
            this.tb_ProviderName.Size = new System.Drawing.Size(217, 20);
            this.tb_ProviderName.TabIndex = 1;
            this.tb_ProviderName.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkBlue;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Provider Name:";
            // 
            // lb_Database
            // 
            this.lb_Database.AutoSize = true;
            this.lb_Database.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Database.ForeColor = System.Drawing.Color.DarkBlue;
            this.lb_Database.Location = new System.Drawing.Point(6, 64);
            this.lb_Database.Name = "lb_Database";
            this.lb_Database.Size = new System.Drawing.Size(101, 13);
            this.lb_Database.TabIndex = 5;
            this.lb_Database.Text = "Database Name:";
            // 
            // tb_DatabaseName
            // 
            this.tb_DatabaseName.Location = new System.Drawing.Point(9, 80);
            this.tb_DatabaseName.Name = "tb_DatabaseName";
            this.tb_DatabaseName.Size = new System.Drawing.Size(217, 20);
            this.tb_DatabaseName.TabIndex = 2;
            // 
            // lb_ServerName
            // 
            this.lb_ServerName.AutoSize = true;
            this.lb_ServerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_ServerName.ForeColor = System.Drawing.Color.DarkBlue;
            this.lb_ServerName.Location = new System.Drawing.Point(6, 123);
            this.lb_ServerName.Name = "lb_ServerName";
            this.lb_ServerName.Size = new System.Drawing.Size(84, 13);
            this.lb_ServerName.TabIndex = 7;
            this.lb_ServerName.Text = "Server Name:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bt_Browse);
            this.groupBox1.Controls.Add(this.lb_ServerName);
            this.groupBox1.Controls.Add(this.tb_ServerName);
            this.groupBox1.Controls.Add(this.lb_Database);
            this.groupBox1.Controls.Add(this.tb_DatabaseName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tb_ProviderName);
            this.groupBox1.Location = new System.Drawing.Point(23, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(246, 186);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Database Setup";
            // 
            // bt_Browse
            // 
            this.bt_Browse.ForeColor = System.Drawing.Color.DarkBlue;
            this.bt_Browse.Location = new System.Drawing.Point(152, 104);
            this.bt_Browse.Name = "bt_Browse";
            this.bt_Browse.Size = new System.Drawing.Size(64, 29);
            this.bt_Browse.TabIndex = 0;
            this.bt_Browse.TabStop = false;
            this.bt_Browse.Text = "&Browse";
            this.bt_Browse.UseVisualStyleBackColor = true;
            this.bt_Browse.Click += new System.EventHandler(this.bt_Browse_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_Microsoft_Authentification);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tb_DBPwd);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tb_DBUser);
            this.groupBox2.Location = new System.Drawing.Point(23, 247);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(246, 137);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "User Details";
            // 
            // cb_Microsoft_Authentification
            // 
            this.cb_Microsoft_Authentification.AutoSize = true;
            this.cb_Microsoft_Authentification.Location = new System.Drawing.Point(10, 108);
            this.cb_Microsoft_Authentification.Name = "cb_Microsoft_Authentification";
            this.cb_Microsoft_Authentification.Size = new System.Drawing.Size(167, 17);
            this.cb_Microsoft_Authentification.TabIndex = 6;
            this.cb_Microsoft_Authentification.Text = "Use Microsoft Authentification";
            this.cb_Microsoft_Authentification.UseVisualStyleBackColor = true;
            this.cb_Microsoft_Authentification.CheckedChanged += new System.EventHandler(this.cb_Microsoft_Authentification_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.DarkBlue;
            this.label5.Location = new System.Drawing.Point(6, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Password:";
            // 
            // tb_DBPwd
            // 
            this.tb_DBPwd.Location = new System.Drawing.Point(9, 82);
            this.tb_DBPwd.Name = "tb_DBPwd";
            this.tb_DBPwd.Size = new System.Drawing.Size(217, 20);
            this.tb_DBPwd.TabIndex = 5;
            this.tb_DBPwd.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.DarkBlue;
            this.label6.Location = new System.Drawing.Point(6, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "User Name:";
            // 
            // tb_DBUser
            // 
            this.tb_DBUser.Location = new System.Drawing.Point(9, 41);
            this.tb_DBUser.Name = "tb_DBUser";
            this.tb_DBUser.Size = new System.Drawing.Size(217, 20);
            this.tb_DBUser.TabIndex = 4;
            // 
            // bt_Test
            // 
            this.bt_Test.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Test.ForeColor = System.Drawing.Color.DarkGreen;
            this.bt_Test.Location = new System.Drawing.Point(287, 339);
            this.bt_Test.Name = "bt_Test";
            this.bt_Test.Size = new System.Drawing.Size(54, 23);
            this.bt_Test.TabIndex = 7;
            this.bt_Test.Text = "&Test";
            this.bt_Test.UseVisualStyleBackColor = true;
            this.bt_Test.Click += new System.EventHandler(this.bt_Test_Click);
            // 
            // bt_Ok
            // 
            this.bt_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bt_Ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Ok.Location = new System.Drawing.Point(287, 49);
            this.bt_Ok.Name = "bt_Ok";
            this.bt_Ok.Size = new System.Drawing.Size(54, 21);
            this.bt_Ok.TabIndex = 8;
            this.bt_Ok.Text = "&Ok";
            this.bt_Ok.UseVisualStyleBackColor = true;
            this.bt_Ok.Click += new System.EventHandler(this.bt_Ok_Click);
            // 
            // bt_Cancel
            // 
            this.bt_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Cancel.Location = new System.Drawing.Point(287, 76);
            this.bt_Cancel.Name = "bt_Cancel";
            this.bt_Cancel.Size = new System.Drawing.Size(54, 21);
            this.bt_Cancel.TabIndex = 9;
            this.bt_Cancel.Text = "&Cancel";
            this.bt_Cancel.UseVisualStyleBackColor = true;
            this.bt_Cancel.Click += new System.EventHandler(this.bt_Close_Click);
            // 
            // frm_DBConnect
            // 
            this.AcceptButton = this.bt_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bt_Cancel;
            this.ClientSize = new System.Drawing.Size(354, 394);
            this.Controls.Add(this.bt_Cancel);
            this.Controls.Add(this.bt_Ok);
            this.Controls.Add(this.bt_Test);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lb_Type);
            this.Controls.Add(this.cb_Type);
            this.Name = "frm_DBConnect";
            this.Text = "Database Connect";
            this.Load += new System.EventHandler(this.frm_DBConnect_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_Type;
        private System.Windows.Forms.Label lb_Type;
        private System.Windows.Forms.TextBox tb_ProviderName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_Database;
        private System.Windows.Forms.TextBox tb_DatabaseName;
        private System.Windows.Forms.Label lb_ServerName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cb_Microsoft_Authentification;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_DBPwd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_DBUser;
        private System.Windows.Forms.Button bt_Browse;
        private System.Windows.Forms.Button bt_Test;
        private System.Windows.Forms.Button bt_Ok;
        private System.Windows.Forms.Button bt_Cancel;
        private System.Windows.Forms.TextBox tb_ServerName;
    }
}