namespace T1MultiAsset
{
    partial class MLPrime_Reconcile
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dg_ML_E238T = new System.Windows.Forms.DataGridView();
            this.GPB_Transaction_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustodianName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TradeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TradeDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Client_Transaction_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FundID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FundName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BBG_Ticker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SettlementDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NetValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Commission = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Side = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Reason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Source = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Product_Short_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParentFundID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_Trade = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bt_Save = new System.Windows.Forms.Button();
            this.bt_Mark_As_Confirmed = new System.Windows.Forms.Button();
            this.bt_Calculator = new System.Windows.Forms.Button();
            this.bt_BackEnd_Reprocess = new System.Windows.Forms.Button();
            this.t_GPB_Transaction_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_CustodianName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_TradeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_TradeDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_FundID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_FundName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_BBG_Ticker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_SettlementDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_NetValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_Side = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_Commission = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_ParentFundID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dg_ML_E238T)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Trade)).BeginInit();
            this.SuspendLayout();
            // 
            // dg_ML_E238T
            // 
            this.dg_ML_E238T.AllowDrop = true;
            this.dg_ML_E238T.AllowUserToAddRows = false;
            this.dg_ML_E238T.AllowUserToDeleteRows = false;
            this.dg_ML_E238T.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_ML_E238T.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dg_ML_E238T.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_ML_E238T.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GPB_Transaction_ID,
            this.CustodianName,
            this.TradeID,
            this.TradeDate,
            this.Client_Transaction_ID,
            this.FundID,
            this.FundName,
            this.BBG_Ticker,
            this.SettlementDate,
            this.Quantity,
            this.Price,
            this.NetValue,
            this.Commission,
            this.Side,
            this.Reason,
            this.Source,
            this.Product_Short_Name,
            this.ParentFundID});
            this.dg_ML_E238T.Location = new System.Drawing.Point(2, 26);
            this.dg_ML_E238T.MultiSelect = false;
            this.dg_ML_E238T.Name = "dg_ML_E238T";
            this.dg_ML_E238T.RowHeadersVisible = false;
            this.dg_ML_E238T.Size = new System.Drawing.Size(1307, 216);
            this.dg_ML_E238T.TabIndex = 0;
            this.dg_ML_E238T.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dg_ML_E238T_MouseDown);
            this.dg_ML_E238T.DragEnter += new System.Windows.Forms.DragEventHandler(this.dg_ML_E238T_DragEnter);
            this.dg_ML_E238T.DragDrop += new System.Windows.Forms.DragEventHandler(this.dg_ML_E238T_DragDrop);
            // 
            // GPB_Transaction_ID
            // 
            this.GPB_Transaction_ID.HeaderText = "Custodian ID";
            this.GPB_Transaction_ID.Name = "GPB_Transaction_ID";
            // 
            // CustodianName
            // 
            this.CustodianName.HeaderText = "Custodian";
            this.CustodianName.Name = "CustodianName";
            this.CustodianName.ReadOnly = true;
            // 
            // TradeID
            // 
            this.TradeID.HeaderText = "Match to TradeID";
            this.TradeID.Name = "TradeID";
            this.TradeID.ReadOnly = true;
            // 
            // TradeDate
            // 
            dataGridViewCellStyle1.Format = "dd-MMM-yyyy";
            this.TradeDate.DefaultCellStyle = dataGridViewCellStyle1;
            this.TradeDate.HeaderText = "Trade Date";
            this.TradeDate.Name = "TradeDate";
            this.TradeDate.ReadOnly = true;
            // 
            // Client_Transaction_ID
            // 
            this.Client_Transaction_ID.HeaderText = "Client TranID";
            this.Client_Transaction_ID.Name = "Client_Transaction_ID";
            this.Client_Transaction_ID.ReadOnly = true;
            // 
            // FundID
            // 
            this.FundID.HeaderText = "FundID";
            this.FundID.Name = "FundID";
            this.FundID.Visible = false;
            // 
            // FundName
            // 
            this.FundName.HeaderText = "Fund Name";
            this.FundName.Name = "FundName";
            // 
            // BBG_Ticker
            // 
            this.BBG_Ticker.HeaderText = "Ticker";
            this.BBG_Ticker.Name = "BBG_Ticker";
            // 
            // SettlementDate
            // 
            dataGridViewCellStyle2.Format = "dd-MMM-yyyy";
            this.SettlementDate.DefaultCellStyle = dataGridViewCellStyle2;
            this.SettlementDate.HeaderText = "Settlement Date";
            this.SettlementDate.Name = "SettlementDate";
            // 
            // Quantity
            // 
            dataGridViewCellStyle3.Format = "N0";
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle3;
            this.Quantity.HeaderText = "Quantity";
            this.Quantity.Name = "Quantity";
            // 
            // Price
            // 
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            // 
            // NetValue
            // 
            dataGridViewCellStyle4.Format = "N2";
            this.NetValue.DefaultCellStyle = dataGridViewCellStyle4;
            this.NetValue.HeaderText = "Net Value";
            this.NetValue.Name = "NetValue";
            // 
            // Commission
            // 
            dataGridViewCellStyle5.Format = "N2";
            this.Commission.DefaultCellStyle = dataGridViewCellStyle5;
            this.Commission.HeaderText = "Commission";
            this.Commission.Name = "Commission";
            // 
            // Side
            // 
            this.Side.HeaderText = "Side";
            this.Side.Name = "Side";
            // 
            // Reason
            // 
            this.Reason.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Reason.HeaderText = "Reason";
            this.Reason.Name = "Reason";
            this.Reason.Width = 69;
            // 
            // Source
            // 
            this.Source.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Source.HeaderText = "Source";
            this.Source.Name = "Source";
            this.Source.Width = 66;
            // 
            // Product_Short_Name
            // 
            this.Product_Short_Name.HeaderText = "Product";
            this.Product_Short_Name.Name = "Product_Short_Name";
            this.Product_Short_Name.ReadOnly = true;
            // 
            // ParentFundID
            // 
            this.ParentFundID.HeaderText = "ParentFundID";
            this.ParentFundID.Name = "ParentFundID";
            this.ParentFundID.Visible = false;
            // 
            // dg_Trade
            // 
            this.dg_Trade.AllowDrop = true;
            this.dg_Trade.AllowUserToAddRows = false;
            this.dg_Trade.AllowUserToDeleteRows = false;
            this.dg_Trade.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_Trade.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dg_Trade.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_Trade.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.t_GPB_Transaction_ID,
            this.t_CustodianName,
            this.t_TradeID,
            this.t_TradeDate,
            this.t_FundID,
            this.t_FundName,
            this.t_BBG_Ticker,
            this.t_SettlementDate,
            this.t_Quantity,
            this.t_Price,
            this.t_NetValue,
            this.t_Side,
            this.t_Commission,
            this.t_ParentFundID});
            this.dg_Trade.Location = new System.Drawing.Point(2, 402);
            this.dg_Trade.MultiSelect = false;
            this.dg_Trade.Name = "dg_Trade";
            this.dg_Trade.RowHeadersVisible = false;
            this.dg_Trade.Size = new System.Drawing.Size(1307, 240);
            this.dg_Trade.TabIndex = 1;
            this.dg_Trade.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dg_Trade_MouseDown);
            this.dg_Trade.DragEnter += new System.Windows.Forms.DragEventHandler(this.dg_Trade_DragEnter);
            this.dg_Trade.DragDrop += new System.Windows.Forms.DragEventHandler(this.dg_Trade_DragDrop);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkBlue;
            this.label1.Location = new System.Drawing.Point(-1, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Unreconcilled Prime Trade Records";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DarkBlue;
            this.label2.Location = new System.Drawing.Point(-1, 382);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Unreconcilled Trades";
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Green;
            this.label3.Location = new System.Drawing.Point(202, 257);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(534, 110);
            this.label3.TabIndex = 4;
            this.label3.Text = "To allow Prime Broker Reconcilliation.\r\n\r\nDrag && Drop the record onto the matchi" +
                "ng Records.\r\n\r\nIf a Prime record is wrong,then mark it as Reconcilled and Add a " +
                "description why in the Reason.";
            // 
            // bt_Save
            // 
            this.bt_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Save.ForeColor = System.Drawing.Color.DarkGreen;
            this.bt_Save.Location = new System.Drawing.Point(12, 257);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(100, 23);
            this.bt_Save.TabIndex = 5;
            this.bt_Save.Text = "Save Changes";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // bt_Mark_As_Confirmed
            // 
            this.bt_Mark_As_Confirmed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Mark_As_Confirmed.ForeColor = System.Drawing.Color.Brown;
            this.bt_Mark_As_Confirmed.Location = new System.Drawing.Point(12, 300);
            this.bt_Mark_As_Confirmed.Name = "bt_Mark_As_Confirmed";
            this.bt_Mark_As_Confirmed.Size = new System.Drawing.Size(127, 23);
            this.bt_Mark_As_Confirmed.TabIndex = 6;
            this.bt_Mark_As_Confirmed.Text = "Mark as Confirmed";
            this.bt_Mark_As_Confirmed.UseVisualStyleBackColor = true;
            this.bt_Mark_As_Confirmed.Click += new System.EventHandler(this.bt_Mark_As_Confirmed_Click);
            // 
            // bt_Calculator
            // 
            this.bt_Calculator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bt_Calculator.ForeColor = System.Drawing.Color.MediumBlue;
            this.bt_Calculator.Location = new System.Drawing.Point(847, 257);
            this.bt_Calculator.Margin = new System.Windows.Forms.Padding(0);
            this.bt_Calculator.Name = "bt_Calculator";
            this.bt_Calculator.Size = new System.Drawing.Size(72, 25);
            this.bt_Calculator.TabIndex = 23;
            this.bt_Calculator.Text = "Calculator";
            this.bt_Calculator.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bt_Calculator.UseVisualStyleBackColor = true;
            this.bt_Calculator.Click += new System.EventHandler(this.bt_Calculator_Click);
            // 
            // bt_BackEnd_Reprocess
            // 
            this.bt_BackEnd_Reprocess.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_BackEnd_Reprocess.ForeColor = System.Drawing.Color.DarkGreen;
            this.bt_BackEnd_Reprocess.Location = new System.Drawing.Point(1026, 257);
            this.bt_BackEnd_Reprocess.Name = "bt_BackEnd_Reprocess";
            this.bt_BackEnd_Reprocess.Size = new System.Drawing.Size(92, 41);
            this.bt_BackEnd_Reprocess.TabIndex = 27;
            this.bt_BackEnd_Reprocess.Text = "BackEnd Reprocess";
            this.bt_BackEnd_Reprocess.UseVisualStyleBackColor = true;
            this.bt_BackEnd_Reprocess.Click += new System.EventHandler(this.bt_BackEnd_Reprocess_Click);
            // 
            // t_GPB_Transaction_ID
            // 
            this.t_GPB_Transaction_ID.HeaderText = "Match to Custodian ID";
            this.t_GPB_Transaction_ID.Name = "t_GPB_Transaction_ID";
            this.t_GPB_Transaction_ID.ReadOnly = true;
            // 
            // t_CustodianName
            // 
            this.t_CustodianName.HeaderText = "Custodian";
            this.t_CustodianName.Name = "t_CustodianName";
            this.t_CustodianName.ReadOnly = true;
            // 
            // t_TradeID
            // 
            this.t_TradeID.HeaderText = "TradeID";
            this.t_TradeID.Name = "t_TradeID";
            // 
            // t_TradeDate
            // 
            dataGridViewCellStyle6.Format = "dd-MMM-yyyy";
            this.t_TradeDate.DefaultCellStyle = dataGridViewCellStyle6;
            this.t_TradeDate.HeaderText = "Trade Date";
            this.t_TradeDate.Name = "t_TradeDate";
            this.t_TradeDate.ReadOnly = true;
            // 
            // t_FundID
            // 
            this.t_FundID.HeaderText = "FundID";
            this.t_FundID.Name = "t_FundID";
            this.t_FundID.Visible = false;
            // 
            // t_FundName
            // 
            this.t_FundName.HeaderText = "Fund Name";
            this.t_FundName.Name = "t_FundName";
            // 
            // t_BBG_Ticker
            // 
            this.t_BBG_Ticker.HeaderText = "Ticker";
            this.t_BBG_Ticker.Name = "t_BBG_Ticker";
            // 
            // t_SettlementDate
            // 
            dataGridViewCellStyle7.Format = "dd-MMM-yyyy";
            this.t_SettlementDate.DefaultCellStyle = dataGridViewCellStyle7;
            this.t_SettlementDate.HeaderText = "Settlement Date";
            this.t_SettlementDate.Name = "t_SettlementDate";
            // 
            // t_Quantity
            // 
            dataGridViewCellStyle8.Format = "N0";
            this.t_Quantity.DefaultCellStyle = dataGridViewCellStyle8;
            this.t_Quantity.HeaderText = "Quantity";
            this.t_Quantity.Name = "t_Quantity";
            // 
            // t_Price
            // 
            this.t_Price.HeaderText = "Price";
            this.t_Price.Name = "t_Price";
            // 
            // t_NetValue
            // 
            dataGridViewCellStyle9.Format = "N2";
            this.t_NetValue.DefaultCellStyle = dataGridViewCellStyle9;
            this.t_NetValue.HeaderText = "Net Value";
            this.t_NetValue.Name = "t_NetValue";
            // 
            // t_Side
            // 
            this.t_Side.HeaderText = "Side";
            this.t_Side.Name = "t_Side";
            // 
            // t_Commission
            // 
            dataGridViewCellStyle10.Format = "N2";
            this.t_Commission.DefaultCellStyle = dataGridViewCellStyle10;
            this.t_Commission.HeaderText = "Commission";
            this.t_Commission.Name = "t_Commission";
            // 
            // t_ParentFundID
            // 
            this.t_ParentFundID.HeaderText = "ParentFundID";
            this.t_ParentFundID.Name = "t_ParentFundID";
            this.t_ParentFundID.Visible = false;
            // 
            // MLPrime_Reconcile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1321, 654);
            this.Controls.Add(this.bt_BackEnd_Reprocess);
            this.Controls.Add(this.bt_Calculator);
            this.Controls.Add(this.bt_Mark_As_Confirmed);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dg_Trade);
            this.Controls.Add(this.dg_ML_E238T);
            this.Name = "MLPrime_Reconcile";
            this.Text = "Reconcile Prime Data";
            this.Load += new System.EventHandler(this.MLPrime_Reconcile_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MLPrime_Reconcile_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dg_ML_E238T)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Trade)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_ML_E238T;
        private System.Windows.Forms.DataGridView dg_Trade;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.Button bt_Mark_As_Confirmed;
        private System.Windows.Forms.Button bt_Calculator;
        private System.Windows.Forms.Button bt_BackEnd_Reprocess;
        private System.Windows.Forms.DataGridViewTextBoxColumn GPB_Transaction_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustodianName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TradeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn TradeDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Client_Transaction_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FundID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FundName;
        private System.Windows.Forms.DataGridViewTextBoxColumn BBG_Ticker;
        private System.Windows.Forms.DataGridViewTextBoxColumn SettlementDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn NetValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Commission;
        private System.Windows.Forms.DataGridViewTextBoxColumn Side;
        private System.Windows.Forms.DataGridViewTextBoxColumn Reason;
        private System.Windows.Forms.DataGridViewTextBoxColumn Source;
        private System.Windows.Forms.DataGridViewTextBoxColumn Product_Short_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParentFundID;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_GPB_Transaction_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_CustodianName;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_TradeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_TradeDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_FundID;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_FundName;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_BBG_Ticker;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_SettlementDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_NetValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_Side;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_Commission;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_ParentFundID;
    }
}