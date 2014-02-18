namespace T1MultiAsset
{
    partial class ProcessJournals
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gb_Action = new System.Windows.Forms.GroupBox();
            this.rb_BetweenAccounts = new System.Windows.Forms.RadioButton();
            this.lb_TranType = new System.Windows.Forms.Label();
            this.rb_BetweenSubFunds = new System.Windows.Forms.RadioButton();
            this.rb_ReverseAccrual = new System.Windows.Forms.RadioButton();
            this.rb_GeneralJournal = new System.Windows.Forms.RadioButton();
            this.dg_Journal = new System.Windows.Forms.DataGridView();
            this.j_AccountID = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.j_FundID = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.j_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.j_TranType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.j_Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.j_PortfolioID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.j_AllowEdit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.j_TranID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bt_Save = new System.Windows.Forms.Button();
            this.dtp_EffectiveDate = new System.Windows.Forms.DateTimePicker();
            this.lb_Imbalance = new System.Windows.Forms.Label();
            this.bt_Calculator = new System.Windows.Forms.Button();
            this.cb_crncy = new System.Windows.Forms.ComboBox();
            this.bt_Print = new System.Windows.Forms.Button();
            this.gb_Action.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Journal)).BeginInit();
            this.SuspendLayout();
            // 
            // gb_Action
            // 
            this.gb_Action.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Action.Controls.Add(this.rb_BetweenAccounts);
            this.gb_Action.Controls.Add(this.lb_TranType);
            this.gb_Action.Controls.Add(this.rb_BetweenSubFunds);
            this.gb_Action.Controls.Add(this.rb_ReverseAccrual);
            this.gb_Action.Controls.Add(this.rb_GeneralJournal);
            this.gb_Action.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Action.ForeColor = System.Drawing.Color.DarkGreen;
            this.gb_Action.Location = new System.Drawing.Point(880, 12);
            this.gb_Action.Name = "gb_Action";
            this.gb_Action.Size = new System.Drawing.Size(175, 145);
            this.gb_Action.TabIndex = 0;
            this.gb_Action.TabStop = false;
            this.gb_Action.Text = "Journal Action";
            // 
            // rb_BetweenAccounts
            // 
            this.rb_BetweenAccounts.AutoSize = true;
            this.rb_BetweenAccounts.ForeColor = System.Drawing.Color.SteelBlue;
            this.rb_BetweenAccounts.Location = new System.Drawing.Point(23, 89);
            this.rb_BetweenAccounts.Name = "rb_BetweenAccounts";
            this.rb_BetweenAccounts.Size = new System.Drawing.Size(131, 17);
            this.rb_BetweenAccounts.TabIndex = 5;
            this.rb_BetweenAccounts.Text = "Between Accounts";
            this.rb_BetweenAccounts.UseVisualStyleBackColor = true;
            // 
            // lb_TranType
            // 
            this.lb_TranType.Location = new System.Drawing.Point(20, 113);
            this.lb_TranType.Name = "lb_TranType";
            this.lb_TranType.Size = new System.Drawing.Size(149, 20);
            this.lb_TranType.TabIndex = 4;
            this.lb_TranType.Text = "label TranType";
            // 
            // rb_BetweenSubFunds
            // 
            this.rb_BetweenSubFunds.AutoSize = true;
            this.rb_BetweenSubFunds.ForeColor = System.Drawing.Color.SteelBlue;
            this.rb_BetweenSubFunds.Location = new System.Drawing.Point(21, 43);
            this.rb_BetweenSubFunds.Name = "rb_BetweenSubFunds";
            this.rb_BetweenSubFunds.Size = new System.Drawing.Size(138, 17);
            this.rb_BetweenSubFunds.TabIndex = 3;
            this.rb_BetweenSubFunds.Text = "Between Sub-Funds";
            this.rb_BetweenSubFunds.UseVisualStyleBackColor = true;
            // 
            // rb_ReverseAccrual
            // 
            this.rb_ReverseAccrual.AutoSize = true;
            this.rb_ReverseAccrual.ForeColor = System.Drawing.Color.SteelBlue;
            this.rb_ReverseAccrual.Location = new System.Drawing.Point(21, 66);
            this.rb_ReverseAccrual.Name = "rb_ReverseAccrual";
            this.rb_ReverseAccrual.Size = new System.Drawing.Size(119, 17);
            this.rb_ReverseAccrual.TabIndex = 2;
            this.rb_ReverseAccrual.Text = "Reverse Accrual";
            this.rb_ReverseAccrual.UseVisualStyleBackColor = true;
            // 
            // rb_GeneralJournal
            // 
            this.rb_GeneralJournal.AutoSize = true;
            this.rb_GeneralJournal.Checked = true;
            this.rb_GeneralJournal.Location = new System.Drawing.Point(21, 20);
            this.rb_GeneralJournal.Name = "rb_GeneralJournal";
            this.rb_GeneralJournal.Size = new System.Drawing.Size(114, 17);
            this.rb_GeneralJournal.TabIndex = 0;
            this.rb_GeneralJournal.TabStop = true;
            this.rb_GeneralJournal.Text = "General Journal";
            this.rb_GeneralJournal.UseVisualStyleBackColor = true;
            // 
            // dg_Journal
            // 
            this.dg_Journal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Green;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_Journal.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dg_Journal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_Journal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.j_AccountID,
            this.j_FundID,
            this.j_Description,
            this.j_TranType,
            this.j_Amount,
            this.j_PortfolioID,
            this.j_AllowEdit,
            this.j_TranID});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_Journal.DefaultCellStyle = dataGridViewCellStyle2;
            this.dg_Journal.Location = new System.Drawing.Point(12, 176);
            this.dg_Journal.Name = "dg_Journal";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_Journal.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dg_Journal.Size = new System.Drawing.Size(1043, 319);
            this.dg_Journal.TabIndex = 1;
            this.dg_Journal.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dg_Journal_UserDeletingRow);
            this.dg_Journal.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dg_Journal_CellBeginEdit);
            this.dg_Journal.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dg_Journal_UserAddedRow);
            this.dg_Journal.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_Journal_CellEndEdit);
            // 
            // j_AccountID
            // 
            this.j_AccountID.HeaderText = "Account Name";
            this.j_AccountID.MinimumWidth = 200;
            this.j_AccountID.Name = "j_AccountID";
            this.j_AccountID.Width = 200;
            // 
            // j_FundID
            // 
            this.j_FundID.HeaderText = "Fund Name";
            this.j_FundID.Name = "j_FundID";
            this.j_FundID.Width = 200;
            // 
            // j_Description
            // 
            this.j_Description.HeaderText = "Description";
            this.j_Description.MinimumWidth = 200;
            this.j_Description.Name = "j_Description";
            this.j_Description.Width = 200;
            // 
            // j_TranType
            // 
            this.j_TranType.HeaderText = "Type";
            this.j_TranType.Name = "j_TranType";
            // 
            // j_Amount
            // 
            this.j_Amount.HeaderText = "Amount";
            this.j_Amount.Name = "j_Amount";
            // 
            // j_PortfolioID
            // 
            this.j_PortfolioID.HeaderText = "Portfolio Name";
            this.j_PortfolioID.MinimumWidth = 200;
            this.j_PortfolioID.Name = "j_PortfolioID";
            this.j_PortfolioID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.j_PortfolioID.Width = 200;
            // 
            // j_AllowEdit
            // 
            this.j_AllowEdit.HeaderText = "AllowEdit";
            this.j_AllowEdit.Name = "j_AllowEdit";
            this.j_AllowEdit.Visible = false;
            // 
            // j_TranID
            // 
            this.j_TranID.HeaderText = "j_TranID";
            this.j_TranID.Name = "j_TranID";
            this.j_TranID.Visible = false;
            // 
            // bt_Save
            // 
            this.bt_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Save.ForeColor = System.Drawing.Color.DarkGreen;
            this.bt_Save.Location = new System.Drawing.Point(988, 510);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(67, 31);
            this.bt_Save.TabIndex = 2;
            this.bt_Save.Text = "Save";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // dtp_EffectiveDate
            // 
            this.dtp_EffectiveDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_EffectiveDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_EffectiveDate.Location = new System.Drawing.Point(12, 38);
            this.dtp_EffectiveDate.Name = "dtp_EffectiveDate";
            this.dtp_EffectiveDate.Size = new System.Drawing.Size(208, 20);
            this.dtp_EffectiveDate.TabIndex = 3;
            // 
            // lb_Imbalance
            // 
            this.lb_Imbalance.ForeColor = System.Drawing.Color.Maroon;
            this.lb_Imbalance.Location = new System.Drawing.Point(624, 141);
            this.lb_Imbalance.Name = "lb_Imbalance";
            this.lb_Imbalance.Size = new System.Drawing.Size(229, 23);
            this.lb_Imbalance.TabIndex = 4;
            this.lb_Imbalance.Text = "Imbalance = 0";
            this.lb_Imbalance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bt_Calculator
            // 
            this.bt_Calculator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Calculator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bt_Calculator.ForeColor = System.Drawing.Color.MediumBlue;
            this.bt_Calculator.Location = new System.Drawing.Point(736, 12);
            this.bt_Calculator.Margin = new System.Windows.Forms.Padding(0);
            this.bt_Calculator.Name = "bt_Calculator";
            this.bt_Calculator.Size = new System.Drawing.Size(72, 25);
            this.bt_Calculator.TabIndex = 23;
            this.bt_Calculator.Text = "Calculator";
            this.bt_Calculator.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bt_Calculator.UseVisualStyleBackColor = true;
            this.bt_Calculator.Click += new System.EventHandler(this.bt_Calculator_Click);
            // 
            // cb_crncy
            // 
            this.cb_crncy.FormattingEnabled = true;
            this.cb_crncy.Items.AddRange(new object[] {
            "AUD",
            "USD",
            "GBP",
            "EUR"});
            this.cb_crncy.Location = new System.Drawing.Point(12, 74);
            this.cb_crncy.Name = "cb_crncy";
            this.cb_crncy.Size = new System.Drawing.Size(65, 21);
            this.cb_crncy.TabIndex = 24;
            // 
            // bt_Print
            // 
            this.bt_Print.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Print.BackgroundImage = global::T1MultiAsset.Properties.Resources.Printer1;
            this.bt_Print.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bt_Print.Location = new System.Drawing.Point(937, 514);
            this.bt_Print.Margin = new System.Windows.Forms.Padding(0);
            this.bt_Print.Name = "bt_Print";
            this.bt_Print.Size = new System.Drawing.Size(35, 23);
            this.bt_Print.TabIndex = 27;
            this.bt_Print.UseVisualStyleBackColor = true;
            // 
            // ProcessJournals
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 544);
            this.Controls.Add(this.bt_Print);
            this.Controls.Add(this.cb_crncy);
            this.Controls.Add(this.bt_Calculator);
            this.Controls.Add(this.lb_Imbalance);
            this.Controls.Add(this.dtp_EffectiveDate);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.dg_Journal);
            this.Controls.Add(this.gb_Action);
            this.Name = "ProcessJournals";
            this.Text = "Process Journals";
            this.Load += new System.EventHandler(this.ProcessJournals_Load);
            this.Shown += new System.EventHandler(this.ProcessJournals_Shown);
            this.gb_Action.ResumeLayout(false);
            this.gb_Action.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Journal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_Action;
        private System.Windows.Forms.RadioButton rb_BetweenSubFunds;
        private System.Windows.Forms.RadioButton rb_ReverseAccrual;
        private System.Windows.Forms.RadioButton rb_GeneralJournal;
        private System.Windows.Forms.Label lb_TranType;
        private System.Windows.Forms.DataGridView dg_Journal;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.DateTimePicker dtp_EffectiveDate;
        private System.Windows.Forms.Label lb_Imbalance;
        private System.Windows.Forms.Button bt_Calculator;
        private System.Windows.Forms.ComboBox cb_crncy;
        private System.Windows.Forms.RadioButton rb_BetweenAccounts;
        private System.Windows.Forms.DataGridViewComboBoxColumn j_AccountID;
        private System.Windows.Forms.DataGridViewComboBoxColumn j_FundID;
        private System.Windows.Forms.DataGridViewTextBoxColumn j_Description;
        private System.Windows.Forms.DataGridViewComboBoxColumn j_TranType;
        private System.Windows.Forms.DataGridViewTextBoxColumn j_Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn j_PortfolioID;
        private System.Windows.Forms.DataGridViewTextBoxColumn j_AllowEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn j_TranID;
        private System.Windows.Forms.Button bt_Print;
    }
}