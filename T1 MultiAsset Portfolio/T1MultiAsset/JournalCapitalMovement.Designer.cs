namespace T1MultiAsset
{
    partial class JournalCapitalMovement
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
            this.rb_Bank = new System.Windows.Forms.RadioButton();
            this.rb_Investor = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_Account = new System.Windows.Forms.ComboBox();
            this.bt_CreateTransaction = new System.Windows.Forms.Button();
            this.bt_Close = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rb_Bank
            // 
            this.rb_Bank.AutoSize = true;
            this.rb_Bank.Checked = true;
            this.rb_Bank.ForeColor = System.Drawing.Color.DarkBlue;
            this.rb_Bank.Location = new System.Drawing.Point(18, 29);
            this.rb_Bank.Name = "rb_Bank";
            this.rb_Bank.Size = new System.Drawing.Size(476, 17);
            this.rb_Bank.TabIndex = 0;
            this.rb_Bank.TabStop = true;
            this.rb_Bank.Text = "Money is moved to another Bank Account, then Money moves from the Investor";
            this.rb_Bank.UseVisualStyleBackColor = true;
            this.rb_Bank.MouseClick += new System.Windows.Forms.MouseEventHandler(this.rb_Bank_MouseClick);
            // 
            // rb_Investor
            // 
            this.rb_Investor.AutoSize = true;
            this.rb_Investor.ForeColor = System.Drawing.Color.DarkBlue;
            this.rb_Investor.Location = new System.Drawing.Point(18, 157);
            this.rb_Investor.Name = "rb_Investor";
            this.rb_Investor.Size = new System.Drawing.Size(202, 17);
            this.rb_Investor.TabIndex = 1;
            this.rb_Investor.Text = "Money moves from the Investor";
            this.rb_Investor.UseVisualStyleBackColor = true;
            this.rb_Investor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.rb_Investor_MouseClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_Account);
            this.groupBox1.Controls.Add(this.rb_Investor);
            this.groupBox1.Controls.Add(this.rb_Bank);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Green;
            this.groupBox1.Location = new System.Drawing.Point(35, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 201);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Assign Transfer to Capital Movement";
            // 
            // cb_Account
            // 
            this.cb_Account.FormattingEnabled = true;
            this.cb_Account.Location = new System.Drawing.Point(81, 65);
            this.cb_Account.Name = "cb_Account";
            this.cb_Account.Size = new System.Drawing.Size(269, 21);
            this.cb_Account.TabIndex = 2;
            // 
            // bt_CreateTransaction
            // 
            this.bt_CreateTransaction.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_CreateTransaction.ForeColor = System.Drawing.Color.DarkGreen;
            this.bt_CreateTransaction.Location = new System.Drawing.Point(447, 231);
            this.bt_CreateTransaction.Name = "bt_CreateTransaction";
            this.bt_CreateTransaction.Size = new System.Drawing.Size(123, 31);
            this.bt_CreateTransaction.TabIndex = 3;
            this.bt_CreateTransaction.Text = "Create Transaction";
            this.bt_CreateTransaction.UseVisualStyleBackColor = true;
            this.bt_CreateTransaction.Click += new System.EventHandler(this.bt_CreateTransaction_Click);
            // 
            // bt_Close
            // 
            this.bt_Close.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Close.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bt_Close.Location = new System.Drawing.Point(35, 231);
            this.bt_Close.Name = "bt_Close";
            this.bt_Close.Size = new System.Drawing.Size(56, 31);
            this.bt_Close.TabIndex = 4;
            this.bt_Close.Text = "Close";
            this.bt_Close.UseVisualStyleBackColor = true;
            this.bt_Close.Click += new System.EventHandler(this.bt_Close_Click);
            // 
            // JournalCapitalMovement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 265);
            this.Controls.Add(this.bt_Close);
            this.Controls.Add(this.bt_CreateTransaction);
            this.Controls.Add(this.groupBox1);
            this.Name = "JournalCapitalMovement";
            this.Text = "JournalCapitalMovement";
            this.Load += new System.EventHandler(this.JournalCapitalMovement_Load);
            this.Shown += new System.EventHandler(this.JournalCapitalMovement_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rb_Bank;
        private System.Windows.Forms.RadioButton rb_Investor;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cb_Account;
        private System.Windows.Forms.Button bt_CreateTransaction;
        private System.Windows.Forms.Button bt_Close;
    }
}