namespace T1MultiAsset
{
    partial class Transactions
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
            this.clb_TranType = new System.Windows.Forms.CheckedListBox();
            this.lb_Fund = new System.Windows.Forms.Label();
            this.cb_TranType = new System.Windows.Forms.CheckBox();
            this.bt_Request = new System.Windows.Forms.Button();
            this.dgv_Transactions = new System.Windows.Forms.DataGridView();
            this.cb_Fund = new System.Windows.Forms.ComboBox();
            this.cb_Accounts = new System.Windows.Forms.ComboBox();
            this.lb_Account = new System.Windows.Forms.Label();
            this.cb_Unreconcilled = new System.Windows.Forms.CheckBox();
            this.bt_Save = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Transactions)).BeginInit();
            this.SuspendLayout();
            // 
            // clb_TranType
            // 
            this.clb_TranType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clb_TranType.CheckOnClick = true;
            this.clb_TranType.FormattingEnabled = true;
            this.clb_TranType.Location = new System.Drawing.Point(1039, 24);
            this.clb_TranType.Name = "clb_TranType";
            this.clb_TranType.Size = new System.Drawing.Size(150, 139);
            this.clb_TranType.TabIndex = 0;
            // 
            // lb_Fund
            // 
            this.lb_Fund.AutoSize = true;
            this.lb_Fund.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Fund.ForeColor = System.Drawing.Color.White;
            this.lb_Fund.Location = new System.Drawing.Point(12, 9);
            this.lb_Fund.Name = "lb_Fund";
            this.lb_Fund.Size = new System.Drawing.Size(35, 13);
            this.lb_Fund.TabIndex = 1;
            this.lb_Fund.Text = "Fund";
            // 
            // cb_TranType
            // 
            this.cb_TranType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_TranType.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_TranType.BackColor = System.Drawing.Color.MediumBlue;
            this.cb_TranType.Checked = true;
            this.cb_TranType.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_TranType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_TranType.ForeColor = System.Drawing.Color.White;
            this.cb_TranType.Location = new System.Drawing.Point(1039, 1);
            this.cb_TranType.Name = "cb_TranType";
            this.cb_TranType.Size = new System.Drawing.Size(150, 21);
            this.cb_TranType.TabIndex = 2;
            this.cb_TranType.Text = "Transaction Type";
            this.cb_TranType.UseVisualStyleBackColor = false;
            this.cb_TranType.CheckedChanged += new System.EventHandler(this.cb_TranType_CheckedChanged);
            // 
            // bt_Request
            // 
            this.bt_Request.BackColor = System.Drawing.Color.DarkGray;
            this.bt_Request.ForeColor = System.Drawing.Color.Navy;
            this.bt_Request.Location = new System.Drawing.Point(467, 24);
            this.bt_Request.Name = "bt_Request";
            this.bt_Request.Size = new System.Drawing.Size(61, 24);
            this.bt_Request.TabIndex = 3;
            this.bt_Request.Text = "Request";
            this.bt_Request.UseVisualStyleBackColor = false;
            this.bt_Request.Click += new System.EventHandler(this.bt_Request_Click);
            // 
            // dgv_Transactions
            // 
            this.dgv_Transactions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_Transactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Transactions.Location = new System.Drawing.Point(12, 181);
            this.dgv_Transactions.Name = "dgv_Transactions";
            this.dgv_Transactions.Size = new System.Drawing.Size(1177, 570);
            this.dgv_Transactions.TabIndex = 4;
            this.dgv_Transactions.Sorted += new System.EventHandler(this.dgv_Transactions_Sorted);
            // 
            // cb_Fund
            // 
            this.cb_Fund.FormattingEnabled = true;
            this.cb_Fund.Location = new System.Drawing.Point(15, 25);
            this.cb_Fund.Name = "cb_Fund";
            this.cb_Fund.Size = new System.Drawing.Size(131, 21);
            this.cb_Fund.TabIndex = 5;
            this.cb_Fund.SelectedIndexChanged += new System.EventHandler(this.cb_Fund_SelectedIndexChanged);
            // 
            // cb_Accounts
            // 
            this.cb_Accounts.FormattingEnabled = true;
            this.cb_Accounts.Location = new System.Drawing.Point(164, 25);
            this.cb_Accounts.Name = "cb_Accounts";
            this.cb_Accounts.Size = new System.Drawing.Size(131, 21);
            this.cb_Accounts.TabIndex = 7;
            this.cb_Accounts.SelectedIndexChanged += new System.EventHandler(this.cb_Accounts_SelectedIndexChanged);
            // 
            // lb_Account
            // 
            this.lb_Account.AutoSize = true;
            this.lb_Account.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Account.ForeColor = System.Drawing.Color.White;
            this.lb_Account.Location = new System.Drawing.Point(161, 9);
            this.lb_Account.Name = "lb_Account";
            this.lb_Account.Size = new System.Drawing.Size(54, 13);
            this.lb_Account.TabIndex = 6;
            this.lb_Account.Text = "Account";
            // 
            // cb_Unreconcilled
            // 
            this.cb_Unreconcilled.AutoSize = true;
            this.cb_Unreconcilled.Checked = true;
            this.cb_Unreconcilled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Unreconcilled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Unreconcilled.ForeColor = System.Drawing.Color.White;
            this.cb_Unreconcilled.Location = new System.Drawing.Point(15, 62);
            this.cb_Unreconcilled.Name = "cb_Unreconcilled";
            this.cb_Unreconcilled.Size = new System.Drawing.Size(104, 17);
            this.cb_Unreconcilled.TabIndex = 8;
            this.cb_Unreconcilled.Text = "Unreconcilled";
            this.cb_Unreconcilled.UseVisualStyleBackColor = true;
            // 
            // bt_Save
            // 
            this.bt_Save.BackColor = System.Drawing.Color.DarkGray;
            this.bt_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Save.ForeColor = System.Drawing.Color.Green;
            this.bt_Save.Location = new System.Drawing.Point(15, 123);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(116, 24);
            this.bt_Save.TabIndex = 9;
            this.bt_Save.Text = "Save Changes";
            this.bt_Save.UseVisualStyleBackColor = false;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // Transactions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Navy;
            this.ClientSize = new System.Drawing.Size(1201, 763);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.cb_Unreconcilled);
            this.Controls.Add(this.cb_Accounts);
            this.Controls.Add(this.lb_Account);
            this.Controls.Add(this.cb_Fund);
            this.Controls.Add(this.dgv_Transactions);
            this.Controls.Add(this.bt_Request);
            this.Controls.Add(this.cb_TranType);
            this.Controls.Add(this.lb_Fund);
            this.Controls.Add(this.clb_TranType);
            this.Name = "Transactions";
            this.Text = "Transactions";
            this.Load += new System.EventHandler(this.Transactions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Transactions)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clb_TranType;
        private System.Windows.Forms.Label lb_Fund;
        private System.Windows.Forms.CheckBox cb_TranType;
        private System.Windows.Forms.Button bt_Request;
        private System.Windows.Forms.DataGridView dgv_Transactions;
        private System.Windows.Forms.ComboBox cb_Fund;
        private System.Windows.Forms.ComboBox cb_Accounts;
        private System.Windows.Forms.Label lb_Account;
        private System.Windows.Forms.CheckBox cb_Unreconcilled;
        private System.Windows.Forms.Button bt_Save;
    }
}