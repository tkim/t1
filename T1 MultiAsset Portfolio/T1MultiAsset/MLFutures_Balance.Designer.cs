namespace T1MultiAsset
{
    partial class MLFutures_Balance
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cb_Accounts = new System.Windows.Forms.ComboBox();
            this.cb_UnReconcilled = new System.Windows.Forms.CheckBox();
            this.dgv_Balances = new System.Windows.Forms.DataGridView();
            this.bt_Save = new System.Windows.Forms.Button();
            this.dg_Transactions = new System.Windows.Forms.DataGridView();
            this.bt_Request = new System.Windows.Forms.Button();
            this.dtp_FromDate = new System.Windows.Forms.DateTimePicker();
            this.tb_Currency = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bt_BackEnd_Reprocess = new System.Windows.Forms.Button();
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
            this.cb_UnReconcilled.CheckedChanged += new System.EventHandler(this.cb_UnReconcilled_CheckedChanged);
            // 
            // dgv_Balances
            // 
            this.dgv_Balances.AllowUserToAddRows = false;
            this.dgv_Balances.AllowUserToDeleteRows = false;
            this.dgv_Balances.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_Balances.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Balances.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_Balances.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Balances.Location = new System.Drawing.Point(3, 3);
            this.dgv_Balances.Name = "dgv_Balances";
            this.dgv_Balances.Size = new System.Drawing.Size(976, 244);
            this.dgv_Balances.TabIndex = 2;
            this.dgv_Balances.Sorted += new System.EventHandler(this.dgv_Balances_Sorted);
            this.dgv_Balances.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Balances_RowEnter);
            this.dgv_Balances.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Balances_CellContentClick);
            // 
            // bt_Save
            // 
            this.bt_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Save.ForeColor = System.Drawing.Color.Green;
            this.bt_Save.Location = new System.Drawing.Point(910, 12);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(81, 28);
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
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_Transactions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dg_Transactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_Transactions.Location = new System.Drawing.Point(3, 3);
            this.dg_Transactions.Name = "dg_Transactions";
            this.dg_Transactions.ReadOnly = true;
            this.dg_Transactions.Size = new System.Drawing.Size(976, 261);
            this.dg_Transactions.TabIndex = 4;
            this.dg_Transactions.Sorted += new System.EventHandler(this.dg_Transactions_Sorted);
            // 
            // bt_Request
            // 
            this.bt_Request.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Request.ForeColor = System.Drawing.Color.Blue;
            this.bt_Request.Location = new System.Drawing.Point(669, 16);
            this.bt_Request.Name = "bt_Request";
            this.bt_Request.Size = new System.Drawing.Size(71, 21);
            this.bt_Request.TabIndex = 9;
            this.bt_Request.Text = "Request";
            this.bt_Request.UseVisualStyleBackColor = true;
            this.bt_Request.Click += new System.EventHandler(this.bt_Request_Click);
            // 
            // dtp_FromDate
            // 
            this.dtp_FromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_FromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_FromDate.Location = new System.Drawing.Point(368, 15);
            this.dtp_FromDate.Name = "dtp_FromDate";
            this.dtp_FromDate.Size = new System.Drawing.Size(208, 20);
            this.dtp_FromDate.TabIndex = 8;
            // 
            // tb_Currency
            // 
            this.tb_Currency.Location = new System.Drawing.Point(592, 15);
            this.tb_Currency.Name = "tb_Currency";
            this.tb_Currency.Size = new System.Drawing.Size(61, 20);
            this.tb_Currency.TabIndex = 10;
            this.tb_Currency.Tag = "Enter a Currency or leave blank for all currencies";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 49);
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
            this.splitContainer1.Size = new System.Drawing.Size(982, 521);
            this.splitContainer1.SplitterDistance = 250;
            this.splitContainer1.TabIndex = 11;
            // 
            // bt_BackEnd_Reprocess
            // 
            this.bt_BackEnd_Reprocess.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_BackEnd_Reprocess.ForeColor = System.Drawing.Color.DarkGreen;
            this.bt_BackEnd_Reprocess.Location = new System.Drawing.Point(798, 3);
            this.bt_BackEnd_Reprocess.Name = "bt_BackEnd_Reprocess";
            this.bt_BackEnd_Reprocess.Size = new System.Drawing.Size(92, 42);
            this.bt_BackEnd_Reprocess.TabIndex = 27;
            this.bt_BackEnd_Reprocess.Text = "BackEnd Reprocess";
            this.bt_BackEnd_Reprocess.UseVisualStyleBackColor = true;
            this.bt_BackEnd_Reprocess.Click += new System.EventHandler(this.bt_BackEnd_Reprocess_Click);
            // 
            // MLFutures_Balance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 572);
            this.Controls.Add(this.bt_BackEnd_Reprocess);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tb_Currency);
            this.Controls.Add(this.bt_Request);
            this.Controls.Add(this.dtp_FromDate);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.cb_UnReconcilled);
            this.Controls.Add(this.cb_Accounts);
            this.Name = "MLFutures_Balance";
            this.Text = "ML Futures Balance Reconcilliation";
            this.Load += new System.EventHandler(this.MLFutures_Balance_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MLFutures_Balance_FormClosing);
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
        private System.Windows.Forms.Button bt_Request;
        private System.Windows.Forms.DateTimePicker dtp_FromDate;
        private System.Windows.Forms.TextBox tb_Currency;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button bt_BackEnd_Reprocess;
    }
}