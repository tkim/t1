namespace T1MultiAsset
{
    partial class FTP_ScotiaPrime
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTP_ScotiaPrime));
            this.label1 = new System.Windows.Forms.Label();
            this.tb_ServerIP = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_ServerIP2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_Password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_UserID = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_LastUpdate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_Interval_seconds = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_EMSXFileNameStartsWith = new System.Windows.Forms.TextBox();
            this.bt_Save = new System.Windows.Forms.Button();
            this.bt_Test = new System.Windows.Forms.Button();
            this.bt_Cancel = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tp_FTPDetails = new System.Windows.Forms.TabPage();
            this.tp_Parameters = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.bt_Param_Cancel = new System.Windows.Forms.Button();
            this.bt_Param_Save = new System.Windows.Forms.Button();
            this.dg_System_Parameters = new System.Windows.Forms.DataGridView();
            this.bt_Param_Test = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tp_FTPDetails.SuspendLayout();
            this.tp_Parameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_System_Parameters)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DarkBlue;
            this.label1.Location = new System.Drawing.Point(9, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server URL:";
            // 
            // tb_ServerIP
            // 
            this.tb_ServerIP.Location = new System.Drawing.Point(12, 37);
            this.tb_ServerIP.Name = "tb_ServerIP";
            this.tb_ServerIP.Size = new System.Drawing.Size(317, 20);
            this.tb_ServerIP.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tb_ServerIP2);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tb_Password);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tb_UserID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tb_ServerIP);
            this.groupBox1.Location = new System.Drawing.Point(5, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 196);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FTP Details";
            // 
            // tb_ServerIP2
            // 
            this.tb_ServerIP2.Location = new System.Drawing.Point(12, 78);
            this.tb_ServerIP2.Name = "tb_ServerIP2";
            this.tb_ServerIP2.Size = new System.Drawing.Size(317, 20);
            this.tb_ServerIP2.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.DarkBlue;
            this.label7.Location = new System.Drawing.Point(10, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Backup URL:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.DarkBlue;
            this.label3.Location = new System.Drawing.Point(10, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password:";
            // 
            // tb_Password
            // 
            this.tb_Password.Location = new System.Drawing.Point(13, 164);
            this.tb_Password.Name = "tb_Password";
            this.tb_Password.Size = new System.Drawing.Size(317, 20);
            this.tb_Password.TabIndex = 5;
            this.tb_Password.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.DarkBlue;
            this.label2.Location = new System.Drawing.Point(10, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "UserID:";
            // 
            // tb_UserID
            // 
            this.tb_UserID.Location = new System.Drawing.Point(13, 121);
            this.tb_UserID.Name = "tb_UserID";
            this.tb_UserID.Size = new System.Drawing.Size(317, 20);
            this.tb_UserID.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tb_LastUpdate);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tb_Interval_seconds);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tb_EMSXFileNameStartsWith);
            this.groupBox2.Location = new System.Drawing.Point(5, 210);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(328, 155);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "FTP Details";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.DarkBlue;
            this.label4.Location = new System.Drawing.Point(10, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Last File Updated:";
            // 
            // tb_LastUpdate
            // 
            this.tb_LastUpdate.Location = new System.Drawing.Point(13, 76);
            this.tb_LastUpdate.Name = "tb_LastUpdate";
            this.tb_LastUpdate.ReadOnly = true;
            this.tb_LastUpdate.Size = new System.Drawing.Size(317, 20);
            this.tb_LastUpdate.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.DarkBlue;
            this.label5.Location = new System.Drawing.Point(10, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Interval (Secs):";
            // 
            // tb_Interval_seconds
            // 
            this.tb_Interval_seconds.Location = new System.Drawing.Point(13, 33);
            this.tb_Interval_seconds.Name = "tb_Interval_seconds";
            this.tb_Interval_seconds.Size = new System.Drawing.Size(317, 20);
            this.tb_Interval_seconds.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.DarkBlue;
            this.label6.Location = new System.Drawing.Point(10, 108);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "EMSX File Starts With:";
            this.label6.Visible = false;
            // 
            // tb_EMSXFileNameStartsWith
            // 
            this.tb_EMSXFileNameStartsWith.Location = new System.Drawing.Point(13, 124);
            this.tb_EMSXFileNameStartsWith.Name = "tb_EMSXFileNameStartsWith";
            this.tb_EMSXFileNameStartsWith.Size = new System.Drawing.Size(317, 20);
            this.tb_EMSXFileNameStartsWith.TabIndex = 1;
            this.tb_EMSXFileNameStartsWith.Visible = false;
            // 
            // bt_Save
            // 
            this.bt_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_Save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bt_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Save.ForeColor = System.Drawing.Color.DarkBlue;
            this.bt_Save.Location = new System.Drawing.Point(5, 405);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(65, 30);
            this.bt_Save.TabIndex = 4;
            this.bt_Save.Text = "&Save";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // bt_Test
            // 
            this.bt_Test.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_Test.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Test.ForeColor = System.Drawing.Color.Green;
            this.bt_Test.Location = new System.Drawing.Point(76, 405);
            this.bt_Test.Name = "bt_Test";
            this.bt_Test.Size = new System.Drawing.Size(65, 30);
            this.bt_Test.TabIndex = 5;
            this.bt_Test.Text = "&Test";
            this.bt_Test.UseVisualStyleBackColor = true;
            this.bt_Test.Click += new System.EventHandler(this.bt_Test_Click);
            // 
            // bt_Cancel
            // 
            this.bt_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Cancel.ForeColor = System.Drawing.Color.DarkBlue;
            this.bt_Cancel.Location = new System.Drawing.Point(256, 405);
            this.bt_Cancel.Name = "bt_Cancel";
            this.bt_Cancel.Size = new System.Drawing.Size(65, 30);
            this.bt_Cancel.TabIndex = 6;
            this.bt_Cancel.Text = "&Cancel";
            this.bt_Cancel.UseVisualStyleBackColor = true;
            this.bt_Cancel.Click += new System.EventHandler(this.bt_Cancel_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tp_FTPDetails);
            this.tabControl1.Controls.Add(this.tp_Parameters);
            this.tabControl1.Location = new System.Drawing.Point(7, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(356, 467);
            this.tabControl1.TabIndex = 7;
            // 
            // tp_FTPDetails
            // 
            this.tp_FTPDetails.BackColor = System.Drawing.SystemColors.Control;
            this.tp_FTPDetails.Controls.Add(this.bt_Cancel);
            this.tp_FTPDetails.Controls.Add(this.groupBox1);
            this.tp_FTPDetails.Controls.Add(this.bt_Test);
            this.tp_FTPDetails.Controls.Add(this.groupBox2);
            this.tp_FTPDetails.Controls.Add(this.bt_Save);
            this.tp_FTPDetails.Location = new System.Drawing.Point(4, 22);
            this.tp_FTPDetails.Name = "tp_FTPDetails";
            this.tp_FTPDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tp_FTPDetails.Size = new System.Drawing.Size(348, 441);
            this.tp_FTPDetails.TabIndex = 0;
            this.tp_FTPDetails.Text = "FTP Details";
            // 
            // tp_Parameters
            // 
            this.tp_Parameters.BackColor = System.Drawing.SystemColors.Control;
            this.tp_Parameters.Controls.Add(this.bt_Param_Test);
            this.tp_Parameters.Controls.Add(this.label8);
            this.tp_Parameters.Controls.Add(this.bt_Param_Cancel);
            this.tp_Parameters.Controls.Add(this.bt_Param_Save);
            this.tp_Parameters.Controls.Add(this.dg_System_Parameters);
            this.tp_Parameters.Location = new System.Drawing.Point(4, 22);
            this.tp_Parameters.Name = "tp_Parameters";
            this.tp_Parameters.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Parameters.Size = new System.Drawing.Size(348, 441);
            this.tp_Parameters.TabIndex = 1;
            this.tp_Parameters.Text = "Scotia Parameters";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.ForeColor = System.Drawing.Color.DarkRed;
            this.label8.Location = new System.Drawing.Point(10, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(433, 94);
            this.label8.TabIndex = 9;
            this.label8.Text = resources.GetString("label8.Text");
            // 
            // bt_Param_Cancel
            // 
            this.bt_Param_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Param_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_Param_Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Param_Cancel.ForeColor = System.Drawing.Color.DarkBlue;
            this.bt_Param_Cancel.Location = new System.Drawing.Point(360, 423);
            this.bt_Param_Cancel.Name = "bt_Param_Cancel";
            this.bt_Param_Cancel.Size = new System.Drawing.Size(65, 30);
            this.bt_Param_Cancel.TabIndex = 8;
            this.bt_Param_Cancel.Text = "&Cancel";
            this.bt_Param_Cancel.UseVisualStyleBackColor = true;
            this.bt_Param_Cancel.Click += new System.EventHandler(this.bt_Param_Cancel_Click);
            // 
            // bt_Param_Save
            // 
            this.bt_Param_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_Param_Save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bt_Param_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Param_Save.ForeColor = System.Drawing.Color.DarkBlue;
            this.bt_Param_Save.Location = new System.Drawing.Point(8, 409);
            this.bt_Param_Save.Name = "bt_Param_Save";
            this.bt_Param_Save.Size = new System.Drawing.Size(65, 30);
            this.bt_Param_Save.TabIndex = 7;
            this.bt_Param_Save.Text = "&Save";
            this.bt_Param_Save.UseVisualStyleBackColor = true;
            this.bt_Param_Save.Click += new System.EventHandler(this.bt_Param_Save_Click);
            // 
            // dg_System_Parameters
            // 
            this.dg_System_Parameters.AllowUserToAddRows = false;
            this.dg_System_Parameters.AllowUserToDeleteRows = false;
            this.dg_System_Parameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_System_Parameters.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dg_System_Parameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_System_Parameters.Location = new System.Drawing.Point(8, 107);
            this.dg_System_Parameters.Name = "dg_System_Parameters";
            this.dg_System_Parameters.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.RoyalBlue;
            this.dg_System_Parameters.Size = new System.Drawing.Size(334, 299);
            this.dg_System_Parameters.TabIndex = 0;
            // 
            // bt_Param_Test
            // 
            this.bt_Param_Test.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_Param_Test.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Param_Test.ForeColor = System.Drawing.Color.Green;
            this.bt_Param_Test.Location = new System.Drawing.Point(94, 409);
            this.bt_Param_Test.Name = "bt_Param_Test";
            this.bt_Param_Test.Size = new System.Drawing.Size(65, 30);
            this.bt_Param_Test.TabIndex = 10;
            this.bt_Param_Test.Text = "&Test";
            this.bt_Param_Test.UseVisualStyleBackColor = true;
            this.bt_Param_Test.Click += new System.EventHandler(this.bt_Param_Test_Click);
            // 
            // FTP_ScotiaPrime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bt_Cancel;
            this.ClientSize = new System.Drawing.Size(366, 487);
            this.Controls.Add(this.tabControl1);
            this.Name = "FTP_ScotiaPrime";
            this.Text = "Scotia Prime FTP Parameters";
            this.Load += new System.EventHandler(this.FTP_ScotiaPrime_Load);
            this.Shown += new System.EventHandler(this.FTP_ScotiaPrime_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tp_FTPDetails.ResumeLayout(false);
            this.tp_Parameters.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg_System_Parameters)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_ServerIP;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_Password;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_UserID;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_LastUpdate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_Interval_seconds;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_EMSXFileNameStartsWith;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.Button bt_Test;
        private System.Windows.Forms.Button bt_Cancel;
        private System.Windows.Forms.TextBox tb_ServerIP2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tp_FTPDetails;
        private System.Windows.Forms.TabPage tp_Parameters;
        private System.Windows.Forms.Button bt_Param_Cancel;
        private System.Windows.Forms.Button bt_Param_Save;
        private System.Windows.Forms.DataGridView dg_System_Parameters;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button bt_Param_Test;
    }
}