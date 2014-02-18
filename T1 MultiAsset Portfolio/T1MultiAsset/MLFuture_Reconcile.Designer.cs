namespace T1MultiAsset
{
    partial class MLFuture_Reconcile
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
            this.dg_ML_Futures = new System.Windows.Forms.DataGridView();
            this.ContractID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TradeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TradeDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FundID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fund = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BBG_Ticker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GrossValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Commission = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Side = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.crncy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransactionType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Reason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MLFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pos_Mult_Factor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FundName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_Trade = new System.Windows.Forms.DataGridView();
            this.lb_Future = new System.Windows.Forms.Label();
            this.lb_Trade = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bt_Save = new System.Windows.Forms.Button();
            this.bt_Mark_As_Confirmed = new System.Windows.Forms.Button();
            this.bt_Calculator = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.bt_Request_ALL = new System.Windows.Forms.Button();
            this.dtp_AllFutures = new System.Windows.Forms.DateTimePicker();
            this.dg_AllFutures = new System.Windows.Forms.DataGridView();
            this.bt_BackEnd_Reprocess = new System.Windows.Forms.Button();
            this.t_CustodianConfirmed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_TradeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_TradeDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_SettlementDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_FundID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_Fund = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_BBG_Ticker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_GrossValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_Commission = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_Side = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_crncy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_BrokerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_SentToBroker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_Pos_Mult_Factor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_FundName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_ParentFundID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t_ParentFundName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dg_ML_Futures)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Trade)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_AllFutures)).BeginInit();
            this.SuspendLayout();
            // 
            // dg_ML_Futures
            // 
            this.dg_ML_Futures.AllowDrop = true;
            this.dg_ML_Futures.AllowUserToAddRows = false;
            this.dg_ML_Futures.AllowUserToDeleteRows = false;
            this.dg_ML_Futures.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_ML_Futures.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dg_ML_Futures.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_ML_Futures.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ContractID,
            this.TradeID,
            this.TradeDate,
            this.FundID,
            this.Fund,
            this.BBG_Ticker,
            this.Quantity,
            this.Price,
            this.GrossValue,
            this.Commission,
            this.Side,
            this.crncy,
            this.TransactionType,
            this.Reason,
            this.MLFileName,
            this.Pos_Mult_Factor,
            this.FundName});
            this.dg_ML_Futures.Location = new System.Drawing.Point(4, 6);
            this.dg_ML_Futures.MultiSelect = false;
            this.dg_ML_Futures.Name = "dg_ML_Futures";
            this.dg_ML_Futures.RowHeadersVisible = false;
            this.dg_ML_Futures.Size = new System.Drawing.Size(1180, 167);
            this.dg_ML_Futures.TabIndex = 0;
            this.dg_ML_Futures.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dg_ML_Futures_MouseDown);
            this.dg_ML_Futures.DragEnter += new System.Windows.Forms.DragEventHandler(this.dg_ML_Futures_DragEnter);
            this.dg_ML_Futures.DragDrop += new System.Windows.Forms.DragEventHandler(this.dg_ML_Futures_DragDrop);
            // 
            // ContractID
            // 
            this.ContractID.HeaderText = "ContractID";
            this.ContractID.Name = "ContractID";
            // 
            // TradeID
            // 
            this.TradeID.HeaderText = "Match to TradeID";
            this.TradeID.Name = "TradeID";
            this.TradeID.ReadOnly = true;
            this.TradeID.Width = 90;
            // 
            // TradeDate
            // 
            dataGridViewCellStyle1.Format = "dd-MMM-yyyy";
            this.TradeDate.DefaultCellStyle = dataGridViewCellStyle1;
            this.TradeDate.HeaderText = "Trade Date";
            this.TradeDate.Name = "TradeDate";
            this.TradeDate.ReadOnly = true;
            // 
            // FundID
            // 
            this.FundID.HeaderText = "FundID";
            this.FundID.Name = "FundID";
            this.FundID.Visible = false;
            // 
            // Fund
            // 
            this.Fund.HeaderText = "Fund";
            this.Fund.Name = "Fund";
            this.Fund.Width = 70;
            // 
            // BBG_Ticker
            // 
            this.BBG_Ticker.HeaderText = "Ticker";
            this.BBG_Ticker.Name = "BBG_Ticker";
            // 
            // Quantity
            // 
            dataGridViewCellStyle2.Format = "N0";
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle2;
            this.Quantity.HeaderText = "Quantity";
            this.Quantity.Name = "Quantity";
            // 
            // Price
            // 
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            // 
            // GrossValue
            // 
            dataGridViewCellStyle3.Format = "N2";
            this.GrossValue.DefaultCellStyle = dataGridViewCellStyle3;
            this.GrossValue.HeaderText = "Gross Value";
            this.GrossValue.Name = "GrossValue";
            // 
            // Commission
            // 
            dataGridViewCellStyle4.Format = "N2";
            this.Commission.DefaultCellStyle = dataGridViewCellStyle4;
            this.Commission.HeaderText = "Commission";
            this.Commission.Name = "Commission";
            // 
            // Side
            // 
            this.Side.HeaderText = "Side";
            this.Side.Name = "Side";
            this.Side.Width = 30;
            // 
            // crncy
            // 
            this.crncy.HeaderText = "Currency";
            this.crncy.Name = "crncy";
            this.crncy.Width = 55;
            // 
            // TransactionType
            // 
            this.TransactionType.HeaderText = "Transaction Type";
            this.TransactionType.Name = "TransactionType";
            this.TransactionType.Width = 70;
            // 
            // Reason
            // 
            this.Reason.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Reason.HeaderText = "Reason";
            this.Reason.Name = "Reason";
            this.Reason.Width = 69;
            // 
            // MLFileName
            // 
            this.MLFileName.HeaderText = "ML File Name";
            this.MLFileName.Name = "MLFileName";
            // 
            // Pos_Mult_Factor
            // 
            this.Pos_Mult_Factor.HeaderText = "Mult Factor";
            this.Pos_Mult_Factor.Name = "Pos_Mult_Factor";
            // 
            // FundName
            // 
            this.FundName.HeaderText = "Parent Fund Name";
            this.FundName.Name = "FundName";
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
            this.t_CustodianConfirmed,
            this.t_TradeID,
            this.t_TradeDate,
            this.t_SettlementDate,
            this.t_FundID,
            this.t_Fund,
            this.t_BBG_Ticker,
            this.t_Quantity,
            this.t_Price,
            this.t_GrossValue,
            this.t_Commission,
            this.t_Side,
            this.t_crncy,
            this.t_BrokerName,
            this.t_SentToBroker,
            this.t_Pos_Mult_Factor,
            this.t_FundName,
            this.t_ParentFundID,
            this.t_ParentFundName});
            this.dg_Trade.Location = new System.Drawing.Point(2, 402);
            this.dg_Trade.MultiSelect = false;
            this.dg_Trade.Name = "dg_Trade";
            this.dg_Trade.RowHeadersVisible = false;
            this.dg_Trade.Size = new System.Drawing.Size(1181, 240);
            this.dg_Trade.TabIndex = 1;
            this.dg_Trade.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dg_Trade_MouseDown);
            this.dg_Trade.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_Trade_CellMouseClick);
            this.dg_Trade.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dg_Trade_MouseClick);
            this.dg_Trade.DragEnter += new System.Windows.Forms.DragEventHandler(this.dg_Trade_DragEnter);
            this.dg_Trade.DragDrop += new System.Windows.Forms.DragEventHandler(this.dg_Trade_DragDrop);
            // 
            // lb_Future
            // 
            this.lb_Future.AutoSize = true;
            this.lb_Future.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Future.ForeColor = System.Drawing.Color.DarkBlue;
            this.lb_Future.Location = new System.Drawing.Point(-1, 9);
            this.lb_Future.Name = "lb_Future";
            this.lb_Future.Size = new System.Drawing.Size(234, 13);
            this.lb_Future.TabIndex = 2;
            this.lb_Future.Text = "Unreconcilled ML Future Trade Records";
            // 
            // lb_Trade
            // 
            this.lb_Trade.AutoSize = true;
            this.lb_Trade.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Trade.ForeColor = System.Drawing.Color.DarkBlue;
            this.lb_Trade.Location = new System.Drawing.Point(-1, 382);
            this.lb_Trade.Name = "lb_Trade";
            this.lb_Trade.Size = new System.Drawing.Size(128, 13);
            this.lb_Trade.TabIndex = 3;
            this.lb_Trade.Text = "Unreconcilled Trades";
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
            this.label3.Text = "To allow ML Futures Reconcilliation.\r\n\r\nDrag && Drop the \'Trade\' record onto the " +
                "matching Futures Records.\r\n(Bottom window to Top window).";
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
            this.bt_Calculator.Location = new System.Drawing.Point(796, 257);
            this.bt_Calculator.Margin = new System.Windows.Forms.Padding(0);
            this.bt_Calculator.Name = "bt_Calculator";
            this.bt_Calculator.Size = new System.Drawing.Size(72, 25);
            this.bt_Calculator.TabIndex = 24;
            this.bt_Calculator.Text = "Calculator";
            this.bt_Calculator.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bt_Calculator.UseVisualStyleBackColor = true;
            this.bt_Calculator.Click += new System.EventHandler(this.bt_Calculator_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(2, 41);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1192, 213);
            this.tabControl1.TabIndex = 25;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dg_ML_Futures);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1184, 187);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Unreconcille ML Futures";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.bt_Request_ALL);
            this.tabPage2.Controls.Add(this.dtp_AllFutures);
            this.tabPage2.Controls.Add(this.dg_AllFutures);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1184, 187);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "All ML Futures Trades";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // bt_Request_ALL
            // 
            this.bt_Request_ALL.ForeColor = System.Drawing.Color.MediumBlue;
            this.bt_Request_ALL.Location = new System.Drawing.Point(217, 6);
            this.bt_Request_ALL.Name = "bt_Request_ALL";
            this.bt_Request_ALL.Size = new System.Drawing.Size(92, 21);
            this.bt_Request_ALL.TabIndex = 3;
            this.bt_Request_ALL.Text = "Request";
            this.bt_Request_ALL.UseVisualStyleBackColor = true;
            this.bt_Request_ALL.Click += new System.EventHandler(this.bt_Request_ALL_Click);
            // 
            // dtp_AllFutures
            // 
            this.dtp_AllFutures.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_AllFutures.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_AllFutures.Location = new System.Drawing.Point(3, 6);
            this.dtp_AllFutures.Name = "dtp_AllFutures";
            this.dtp_AllFutures.Size = new System.Drawing.Size(208, 20);
            this.dtp_AllFutures.TabIndex = 2;
            // 
            // dg_AllFutures
            // 
            this.dg_AllFutures.AllowUserToAddRows = false;
            this.dg_AllFutures.AllowUserToDeleteRows = false;
            this.dg_AllFutures.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dg_AllFutures.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dg_AllFutures.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_AllFutures.Location = new System.Drawing.Point(3, 33);
            this.dg_AllFutures.Name = "dg_AllFutures";
            this.dg_AllFutures.ReadOnly = true;
            this.dg_AllFutures.RowHeadersVisible = false;
            this.dg_AllFutures.Size = new System.Drawing.Size(1178, 154);
            this.dg_AllFutures.TabIndex = 0;
            // 
            // bt_BackEnd_Reprocess
            // 
            this.bt_BackEnd_Reprocess.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_BackEnd_Reprocess.ForeColor = System.Drawing.Color.DarkGreen;
            this.bt_BackEnd_Reprocess.Location = new System.Drawing.Point(1091, 9);
            this.bt_BackEnd_Reprocess.Name = "bt_BackEnd_Reprocess";
            this.bt_BackEnd_Reprocess.Size = new System.Drawing.Size(92, 41);
            this.bt_BackEnd_Reprocess.TabIndex = 26;
            this.bt_BackEnd_Reprocess.Text = "BackEnd Reprocess";
            this.bt_BackEnd_Reprocess.UseVisualStyleBackColor = true;
            this.bt_BackEnd_Reprocess.Click += new System.EventHandler(this.bt_BackEnd_Reprocess_Click);
            // 
            // t_CustodianConfirmed
            // 
            this.t_CustodianConfirmed.HeaderText = "Custodian Confirmed";
            this.t_CustodianConfirmed.Name = "t_CustodianConfirmed";
            this.t_CustodianConfirmed.ReadOnly = true;
            // 
            // t_TradeID
            // 
            this.t_TradeID.HeaderText = "TradeID";
            this.t_TradeID.Name = "t_TradeID";
            this.t_TradeID.Width = 90;
            // 
            // t_TradeDate
            // 
            dataGridViewCellStyle5.Format = "dd-MMM-yyyy";
            this.t_TradeDate.DefaultCellStyle = dataGridViewCellStyle5;
            this.t_TradeDate.HeaderText = "Trade Date";
            this.t_TradeDate.Name = "t_TradeDate";
            this.t_TradeDate.ReadOnly = true;
            // 
            // t_SettlementDate
            // 
            dataGridViewCellStyle6.Format = "dd-MMM-yyyy";
            this.t_SettlementDate.DefaultCellStyle = dataGridViewCellStyle6;
            this.t_SettlementDate.HeaderText = "Settlement Date";
            this.t_SettlementDate.Name = "t_SettlementDate";
            this.t_SettlementDate.ReadOnly = true;
            // 
            // t_FundID
            // 
            this.t_FundID.HeaderText = "FundID";
            this.t_FundID.Name = "t_FundID";
            this.t_FundID.Visible = false;
            // 
            // t_Fund
            // 
            this.t_Fund.HeaderText = "Fund";
            this.t_Fund.Name = "t_Fund";
            this.t_Fund.Width = 70;
            // 
            // t_BBG_Ticker
            // 
            this.t_BBG_Ticker.HeaderText = "Ticker";
            this.t_BBG_Ticker.Name = "t_BBG_Ticker";
            // 
            // t_Quantity
            // 
            dataGridViewCellStyle7.Format = "N0";
            this.t_Quantity.DefaultCellStyle = dataGridViewCellStyle7;
            this.t_Quantity.HeaderText = "Quantity";
            this.t_Quantity.Name = "t_Quantity";
            // 
            // t_Price
            // 
            this.t_Price.HeaderText = "Price";
            this.t_Price.Name = "t_Price";
            // 
            // t_GrossValue
            // 
            dataGridViewCellStyle8.Format = "N2";
            this.t_GrossValue.DefaultCellStyle = dataGridViewCellStyle8;
            this.t_GrossValue.HeaderText = "Gross Value";
            this.t_GrossValue.Name = "t_GrossValue";
            // 
            // t_Commission
            // 
            dataGridViewCellStyle9.Format = "N2";
            this.t_Commission.DefaultCellStyle = dataGridViewCellStyle9;
            this.t_Commission.HeaderText = "Commission";
            this.t_Commission.Name = "t_Commission";
            // 
            // t_Side
            // 
            this.t_Side.HeaderText = "Side";
            this.t_Side.Name = "t_Side";
            this.t_Side.Width = 30;
            // 
            // t_crncy
            // 
            this.t_crncy.HeaderText = "Currency";
            this.t_crncy.Name = "t_crncy";
            this.t_crncy.Width = 55;
            // 
            // t_BrokerName
            // 
            this.t_BrokerName.HeaderText = "BrokerName";
            this.t_BrokerName.Name = "t_BrokerName";
            // 
            // t_SentToBroker
            // 
            this.t_SentToBroker.HeaderText = "SentToBroker";
            this.t_SentToBroker.Name = "t_SentToBroker";
            // 
            // t_Pos_Mult_Factor
            // 
            this.t_Pos_Mult_Factor.HeaderText = "Mult Factor";
            this.t_Pos_Mult_Factor.Name = "t_Pos_Mult_Factor";
            // 
            // t_FundName
            // 
            this.t_FundName.HeaderText = "Fund Name";
            this.t_FundName.Name = "t_FundName";
            // 
            // t_ParentFundID
            // 
            this.t_ParentFundID.HeaderText = "ParentFundID";
            this.t_ParentFundID.Name = "t_ParentFundID";
            this.t_ParentFundID.Visible = false;
            // 
            // t_ParentFundName
            // 
            this.t_ParentFundName.HeaderText = "Parent Fund Name";
            this.t_ParentFundName.Name = "t_ParentFundName";
            // 
            // MLFuture_Reconcile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 654);
            this.Controls.Add(this.bt_BackEnd_Reprocess);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.bt_Calculator);
            this.Controls.Add(this.bt_Mark_As_Confirmed);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lb_Trade);
            this.Controls.Add(this.lb_Future);
            this.Controls.Add(this.dg_Trade);
            this.Name = "MLFuture_Reconcile";
            this.Text = "Reconcile ML Futures Data";
            this.Load += new System.EventHandler(this.MLFuture_Reconcile_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MLFuture_Reconcile_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dg_ML_Futures)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Trade)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg_AllFutures)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_ML_Futures;
        private System.Windows.Forms.DataGridView dg_Trade;
        private System.Windows.Forms.Label lb_Future;
        private System.Windows.Forms.Label lb_Trade;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.Button bt_Mark_As_Confirmed;
        private System.Windows.Forms.Button bt_Calculator;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dg_AllFutures;
        private System.Windows.Forms.DateTimePicker dtp_AllFutures;
        private System.Windows.Forms.Button bt_Request_ALL;
        private System.Windows.Forms.Button bt_BackEnd_Reprocess;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContractID;
        private System.Windows.Forms.DataGridViewTextBoxColumn TradeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn TradeDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn FundID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fund;
        private System.Windows.Forms.DataGridViewTextBoxColumn BBG_Ticker;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn GrossValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Commission;
        private System.Windows.Forms.DataGridViewTextBoxColumn Side;
        private System.Windows.Forms.DataGridViewTextBoxColumn crncy;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransactionType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Reason;
        private System.Windows.Forms.DataGridViewTextBoxColumn MLFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pos_Mult_Factor;
        private System.Windows.Forms.DataGridViewTextBoxColumn FundName;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_CustodianConfirmed;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_TradeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_TradeDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_SettlementDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_FundID;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_Fund;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_BBG_Ticker;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_GrossValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_Commission;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_Side;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_crncy;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_BrokerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_SentToBroker;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_Pos_Mult_Factor;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_FundName;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_ParentFundID;
        private System.Windows.Forms.DataGridViewTextBoxColumn t_ParentFundName;
    }
}