namespace T1MultiAsset
{
    partial class MLPrime_Balance
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cb_Accounts = new System.Windows.Forms.ComboBox();
            this.cb_UnReconcilled = new System.Windows.Forms.CheckBox();
            this.dgv_Balances = new System.Windows.Forms.DataGridView();
            this.bt_Save = new System.Windows.Forms.Button();
            this.dg_Transactions = new System.Windows.Forms.DataGridView();
            this.dtp_FromDate = new System.Windows.Forms.DateTimePicker();
            this.bt_Request = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Balances)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Transactions)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_Accounts
            // 
            this.cb_Accounts.FormattingEnabled = true;
            this.cb_Accounts.Location = new System.Drawing.Point(12, 12);
            this.cb_Accounts.Name = "cb_Accounts";
            this.cb_Accounts.Size = new System.Drawing.Size(203, 21);
            this.cb_Accounts.TabIndex = 0;
            this.cb_Accounts.SelectionChangeCommitted += new System.EventHandler(this.cb_Accounts_SelectionChangeCommitted);
            // 
            // cb_UnReconcilled
            // 
            this.cb_UnReconcilled.AutoSize = true;
            this.cb_UnReconcilled.Checked = true;
            this.cb_UnReconcilled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_UnReconcilled.ForeColor = System.Drawing.Color.Green;
            this.cb_UnReconcilled.Location = new System.Drawing.Point(247, 16);
            this.cb_UnReconcilled.Name = "cb_UnReconcilled";
            this.cb_UnReconcilled.Size = new System.Drawing.Size(115, 17);
            this.cb_UnReconcilled.TabIndex = 1;
            this.cb_UnReconcilled.Text = "Unreconcilled Only";
            this.cb_UnReconcilled.UseVisualStyleBackColor = true;
            // 
            // dgv_Balances
            // 
            this.dgv_Balances.AllowUserToAddRows = false;
            this.dgv_Balances.AllowUserToDeleteRows = false;
            this.dgv_Balances.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_Balances.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Balances.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_Balances.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Balances.Location = new System.Drawing.Point(3, 3);
            this.dgv_Balances.Name = "dgv_Balances";
            this.dgv_Balances.Size = new System.Drawing.Size(1006, 294);
            this.dgv_Balances.TabIndex = 2;
            this.dgv_Balances.Sorted += new System.EventHandler(this.dgv_Balances_Sorted);
            this.dgv_Balances.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Balances_RowEnter);
            // 
            // bt_Save
            // 
            this.bt_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Save.ForeColor = System.Drawing.Color.Green;
            this.bt_Save.Location = new System.Drawing.Point(905, 13);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(81, 21);
            this.bt_Save.TabIndex = 3;
            this.bt_Save.Text = "Save";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // dg_Transactions
            // 
            this.dg_Transactions.AllowUserToAddRows = false;
            this.dg_Transactions.AllowUserToDeleteRows = false;
            this.dg_Transactions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_Transactions.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_Transactions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dg_Transactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_Transactions.Location = new System.Drawing.Point(3, 5);
            this.dg_Transactions.Name = "dg_Transactions";
            this.dg_Transactions.ReadOnly = true;
            this.dg_Transactions.Size = new System.Drawing.Size(1006, 450);
            this.dg_Transactions.TabIndex = 4;
            this.dg_Transactions.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_Transactions_CellMouseClick);
            this.dg_Transactions.Sorted += new System.EventHandler(this.dg_Transactions_Sorted);
            this.dg_Transactions.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dg_Transactions_MouseClick);
            // 
            // dtp_FromDate
            // 
            this.dtp_FromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_FromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_FromDate.Location = new System.Drawing.Point(388, 11);
            this.dtp_FromDate.Name = "dtp_FromDate";
            this.dtp_FromDate.Size = new System.Drawing.Size(208, 20);
            this.dtp_FromDate.TabIndex = 5;
            // 
            // bt_Request
            // 
            this.bt_Request.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Request.ForeColor = System.Drawing.Color.Blue;
            this.bt_Request.Location = new System.Drawing.Point(611, 12);
            this.bt_Request.Name = "bt_Request";
            this.bt_Request.Size = new System.Drawing.Size(71, 21);
            this.bt_Request.TabIndex = 7;
            this.bt_Request.Text = "Request";
            this.bt_Request.UseVisualStyleBackColor = true;
            this.bt_Request.Click += new System.EventHandler(this.bt_Request_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 39);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgv_Balances);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dg_Transactions);
            this.splitContainer1.Size = new System.Drawing.Size(1012, 762);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 8;
            // 
            // MLPrime_Balance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 805);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.bt_Request);
            this.Controls.Add(this.dtp_FromDate);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.cb_UnReconcilled);
            this.Controls.Add(this.cb_Accounts);
            this.Name = "MLPrime_Balance";
            this.Text = "Prime Balance Reconcilliation";
            this.Load += new System.EventHandler(this.MLPrime_Balance_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MLPrime_Balance_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Balances)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Transactions)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_Accounts;
        private System.Windows.Forms.CheckBox cb_UnReconcilled;
        private System.Windows.Forms.DataGridView dgv_Balances;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.DataGridView dg_Transactions;
        private System.Windows.Forms.DateTimePicker dtp_FromDate;
        private System.Windows.Forms.Button bt_Request;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}