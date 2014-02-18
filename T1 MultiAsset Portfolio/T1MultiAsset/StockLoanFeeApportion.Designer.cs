namespace T1MultiAsset
{
    partial class StockLoanFeeApportion
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockLoanFeeApportion));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bt_Print = new System.Windows.Forms.Button();
            this.bt_Calculator = new System.Windows.Forms.Button();
            this.dg_Summary = new System.Windows.Forms.DataGridView();
            this.bt_Refresh = new System.Windows.Forms.Button();
            this.dg_inTransaction = new System.Windows.Forms.DataGridView();
            this.dg_Transactions = new System.Windows.Forms.DataGridView();
            this.bt_Save = new System.Windows.Forms.Button();
            this.lb_AccrualNotes = new System.Windows.Forms.Label();
            this.dtp_ToDate = new System.Windows.Forms.DateTimePicker();
            this.dtp_FromDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.FundID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FundName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockLoanFee = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_Imbalance = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Summary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_inTransaction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Transactions)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_Print
            // 
            this.bt_Print.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bt_Print.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Print.ForeColor = System.Drawing.Color.DarkGreen;
            this.bt_Print.Location = new System.Drawing.Point(93, 231);
            this.bt_Print.Margin = new System.Windows.Forms.Padding(0);
            this.bt_Print.Name = "bt_Print";
            this.bt_Print.Size = new System.Drawing.Size(72, 25);
            this.bt_Print.TabIndex = 37;
            this.bt_Print.Text = "Print";
            this.bt_Print.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bt_Print.UseVisualStyleBackColor = true;
            this.bt_Print.Click += new System.EventHandler(this.bt_Print_Click);
            // 
            // bt_Calculator
            // 
            this.bt_Calculator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Calculator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bt_Calculator.ForeColor = System.Drawing.Color.MediumBlue;
            this.bt_Calculator.Location = new System.Drawing.Point(835, 22);
            this.bt_Calculator.Margin = new System.Windows.Forms.Padding(0);
            this.bt_Calculator.Name = "bt_Calculator";
            this.bt_Calculator.Size = new System.Drawing.Size(72, 25);
            this.bt_Calculator.TabIndex = 36;
            this.bt_Calculator.Text = "Calculator";
            this.bt_Calculator.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bt_Calculator.UseVisualStyleBackColor = true;
            this.bt_Calculator.Click += new System.EventHandler(this.bt_Calculator_Click);
            // 
            // dg_Summary
            // 
            this.dg_Summary.AllowUserToAddRows = false;
            this.dg_Summary.AllowUserToDeleteRows = false;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.Green;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_Summary.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dg_Summary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_Summary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FundID,
            this.FundName,
            this.StockLoanFee});
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_Summary.DefaultCellStyle = dataGridViewCellStyle17;
            this.dg_Summary.Location = new System.Drawing.Point(317, 184);
            this.dg_Summary.Name = "dg_Summary";
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.ForestGreen;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_Summary.RowHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.dg_Summary.RowHeadersVisible = false;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dg_Summary.RowsDefaultCellStyle = dataGridViewCellStyle19;
            this.dg_Summary.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dg_Summary.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dg_Summary.RowTemplate.Height = 17;
            this.dg_Summary.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dg_Summary.Size = new System.Drawing.Size(602, 93);
            this.dg_Summary.TabIndex = 35;
            this.dg_Summary.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dg_Summary_CellBeginEdit);
            this.dg_Summary.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_Summary_CellEndEdit);
            // 
            // bt_Refresh
            // 
            this.bt_Refresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Refresh.ForeColor = System.Drawing.Color.MediumBlue;
            this.bt_Refresh.Location = new System.Drawing.Point(15, 231);
            this.bt_Refresh.Name = "bt_Refresh";
            this.bt_Refresh.Size = new System.Drawing.Size(75, 25);
            this.bt_Refresh.TabIndex = 34;
            this.bt_Refresh.Text = "Refresh";
            this.bt_Refresh.UseVisualStyleBackColor = true;
            this.bt_Refresh.Click += new System.EventHandler(this.bt_Refresh_Click);
            // 
            // dg_inTransaction
            // 
            this.dg_inTransaction.AllowUserToAddRows = false;
            this.dg_inTransaction.AllowUserToDeleteRows = false;
            this.dg_inTransaction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.MidnightBlue;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_inTransaction.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle20;
            this.dg_inTransaction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_inTransaction.DefaultCellStyle = dataGridViewCellStyle21;
            this.dg_inTransaction.Location = new System.Drawing.Point(12, 101);
            this.dg_inTransaction.Name = "dg_inTransaction";
            this.dg_inTransaction.ReadOnly = true;
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle22.ForeColor = System.Drawing.Color.MediumSeaGreen;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_inTransaction.RowHeadersDefaultCellStyle = dataGridViewCellStyle22;
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle23.ForeColor = System.Drawing.Color.Blue;
            this.dg_inTransaction.RowsDefaultCellStyle = dataGridViewCellStyle23;
            this.dg_inTransaction.Size = new System.Drawing.Size(908, 68);
            this.dg_inTransaction.TabIndex = 33;
            // 
            // dg_Transactions
            // 
            this.dg_Transactions.AllowUserToAddRows = false;
            this.dg_Transactions.AllowUserToDeleteRows = false;
            this.dg_Transactions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle24.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle24.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle24.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle24.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_Transactions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle24;
            this.dg_Transactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle25.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle25.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_Transactions.DefaultCellStyle = dataGridViewCellStyle25;
            this.dg_Transactions.Location = new System.Drawing.Point(12, 311);
            this.dg_Transactions.Name = "dg_Transactions";
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle26.ForeColor = System.Drawing.Color.MediumSeaGreen;
            dataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_Transactions.RowHeadersDefaultCellStyle = dataGridViewCellStyle26;
            this.dg_Transactions.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dg_Transactions.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dg_Transactions.RowTemplate.Height = 17;
            this.dg_Transactions.Size = new System.Drawing.Size(908, 554);
            this.dg_Transactions.TabIndex = 32;
            // 
            // bt_Save
            // 
            this.bt_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Save.ForeColor = System.Drawing.Color.DarkGreen;
            this.bt_Save.Location = new System.Drawing.Point(224, 233);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(75, 25);
            this.bt_Save.TabIndex = 31;
            this.bt_Save.Text = "Save";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // lb_AccrualNotes
            // 
            this.lb_AccrualNotes.ForeColor = System.Drawing.Color.MediumBlue;
            this.lb_AccrualNotes.Location = new System.Drawing.Point(12, 287);
            this.lb_AccrualNotes.Name = "lb_AccrualNotes";
            this.lb_AccrualNotes.Size = new System.Drawing.Size(287, 20);
            this.lb_AccrualNotes.TabIndex = 30;
            this.lb_AccrualNotes.Text = "Start of Day:    Short Positions";
            // 
            // dtp_ToDate
            // 
            this.dtp_ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_ToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_ToDate.Location = new System.Drawing.Point(91, 207);
            this.dtp_ToDate.Name = "dtp_ToDate";
            this.dtp_ToDate.Size = new System.Drawing.Size(208, 20);
            this.dtp_ToDate.TabIndex = 29;
            // 
            // dtp_FromDate
            // 
            this.dtp_FromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_FromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_FromDate.Location = new System.Drawing.Point(91, 184);
            this.dtp_FromDate.Name = "dtp_FromDate";
            this.dtp_FromDate.Size = new System.Drawing.Size(208, 20);
            this.dtp_FromDate.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 207);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "To Date:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "From Date:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.Green;
            this.richTextBox1.Location = new System.Drawing.Point(12, 8);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(908, 87);
            this.richTextBox1.TabIndex = 25;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // FundID
            // 
            this.FundID.Frozen = true;
            this.FundID.HeaderText = "FundID";
            this.FundID.Name = "FundID";
            this.FundID.Visible = false;
            // 
            // FundName
            // 
            this.FundName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FundName.DefaultCellStyle = dataGridViewCellStyle15;
            this.FundName.Frozen = true;
            this.FundName.HeaderText = "Fund Name";
            this.FundName.MinimumWidth = 200;
            this.FundName.Name = "FundName";
            this.FundName.ReadOnly = true;
            this.FundName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FundName.Width = 200;
            // 
            // StockLoanFee
            // 
            this.StockLoanFee.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.LightCyan;
            dataGridViewCellStyle16.Format = "$#,###.00";
            this.StockLoanFee.DefaultCellStyle = dataGridViewCellStyle16;
            this.StockLoanFee.Frozen = true;
            this.StockLoanFee.HeaderText = "Stock Loan Fee";
            this.StockLoanFee.MinimumWidth = 110;
            this.StockLoanFee.Name = "StockLoanFee";
            this.StockLoanFee.Width = 110;
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.MediumBlue;
            this.label3.Location = new System.Drawing.Point(452, 284);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 20);
            this.label3.TabIndex = 39;
            this.label3.Text = "Imbalance:";
            // 
            // tb_Imbalance
            // 
            this.tb_Imbalance.Location = new System.Drawing.Point(523, 283);
            this.tb_Imbalance.Name = "tb_Imbalance";
            this.tb_Imbalance.Size = new System.Drawing.Size(105, 20);
            this.tb_Imbalance.TabIndex = 40;
            this.tb_Imbalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // StockLoanFeeApportion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 873);
            this.Controls.Add(this.tb_Imbalance);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bt_Print);
            this.Controls.Add(this.bt_Calculator);
            this.Controls.Add(this.dg_Summary);
            this.Controls.Add(this.bt_Refresh);
            this.Controls.Add(this.dg_inTransaction);
            this.Controls.Add(this.dg_Transactions);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.lb_AccrualNotes);
            this.Controls.Add(this.dtp_ToDate);
            this.Controls.Add(this.dtp_FromDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "StockLoanFeeApportion";
            this.Text = "Stock Loan Fee Apportion";
            this.Load += new System.EventHandler(this.StockLoanFeeApportion_Load);
            this.Shown += new System.EventHandler(this.StockLoanFeeApportion_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dg_Summary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_inTransaction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Transactions)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_Print;
        private System.Windows.Forms.Button bt_Calculator;
        private System.Windows.Forms.DataGridView dg_Summary;
        private System.Windows.Forms.Button bt_Refresh;
        private System.Windows.Forms.DataGridView dg_inTransaction;
        private System.Windows.Forms.DataGridView dg_Transactions;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.Label lb_AccrualNotes;
        private System.Windows.Forms.DateTimePicker dtp_ToDate;
        private System.Windows.Forms.DateTimePicker dtp_FromDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn FundID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FundName;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockLoanFee;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_Imbalance;
    }
}