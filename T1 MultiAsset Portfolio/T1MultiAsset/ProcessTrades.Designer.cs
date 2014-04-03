namespace T1MultiAsset
{
    partial class ProcessTrades
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessTrades));
            this.label1 = new System.Windows.Forms.Label();
            this.cb_AggregateTrades = new System.Windows.Forms.CheckBox();
            this.dg_Trades = new System.Windows.Forms.DataGridView();
            this.cb_SelectAll = new System.Windows.Forms.CheckBox();
            this.bt_CreateTrades = new System.Windows.Forms.Button();
            this.bt_SendToBrokers = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tp_OrdersToTrades = new System.Windows.Forms.TabPage();
            this.tp_TradesForBrokers = new System.Windows.Forms.TabPage();
            this.bt_MarkAsSent = new System.Windows.Forms.Button();
            this.lb_TradeForBrokers = new System.Windows.Forms.Label();
            this.dg_ForBrokers = new System.Windows.Forms.DataGridView();
            this.tp_TradesForCustodian = new System.Windows.Forms.TabPage();
            this.cb_CloseWhenDone = new System.Windows.Forms.CheckBox();
            this.bt_OpenBookingsDirectory = new System.Windows.Forms.Button();
            this.bt_CheckContractNotes = new System.Windows.Forms.Button();
            this.cb_ShowUnconfirmedTrades = new System.Windows.Forms.CheckBox();
            this.lb_SendToCustodian = new System.Windows.Forms.Label();
            this.dg_ForCustodians = new System.Windows.Forms.DataGridView();
            this.bt_SendToCustodian = new System.Windows.Forms.Button();
            this.tp_PreviouslyProcessedTrades = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dtp_ToDate = new System.Windows.Forms.DateTimePicker();
            this.dtp_FromDate = new System.Windows.Forms.DateTimePicker();
            this.bt_RequestHistoricTrades = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dg_HistoricTrades = new System.Windows.Forms.DataGridView();
            this.tp_ResendCustodianFile = new System.Windows.Forms.TabPage();
            this.bt_RequestHistoricFiles = new System.Windows.Forms.Button();
            this.dg_CustodianFiles = new System.Windows.Forms.DataGridView();
            this.Custodian = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileMethod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileSent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TradesSent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileConfirmed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TimeStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MLClientBatchID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bt_Refresh = new System.Windows.Forms.Button();
            this.timer_CheckContractNotes = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dg_Trades)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tp_OrdersToTrades.SuspendLayout();
            this.tp_TradesForBrokers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_ForBrokers)).BeginInit();
            this.tp_TradesForCustodian.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_ForCustodians)).BeginInit();
            this.tp_PreviouslyProcessedTrades.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_HistoricTrades)).BeginInit();
            this.tp_ResendCustodianFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_CustodianFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkGreen;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(674, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Convert Today\'s completed Orders into Trades, ready to be Sent to the Broker at t" +
                "he End of the Day.";
            // 
            // cb_AggregateTrades
            // 
            this.cb_AggregateTrades.AutoSize = true;
            this.cb_AggregateTrades.Checked = true;
            this.cb_AggregateTrades.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_AggregateTrades.ForeColor = System.Drawing.Color.MediumBlue;
            this.cb_AggregateTrades.Location = new System.Drawing.Point(6, 33);
            this.cb_AggregateTrades.Name = "cb_AggregateTrades";
            this.cb_AggregateTrades.Size = new System.Drawing.Size(298, 17);
            this.cb_AggregateTrades.TabIndex = 1;
            this.cb_AggregateTrades.Tag = "This is the prefered method as it saves on Prime Broker fees";
            this.cb_AggregateTrades.Text = "Aggregate Trades (where same Ticker && Commision Rate)";
            this.cb_AggregateTrades.UseVisualStyleBackColor = true;
            this.cb_AggregateTrades.CheckedChanged += new System.EventHandler(this.cb_AggregateTrades_CheckedChanged);
            // 
            // dg_Trades
            // 
            this.dg_Trades.AllowUserToAddRows = false;
            this.dg_Trades.AllowUserToDeleteRows = false;
            this.dg_Trades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_Trades.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.dg_Trades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_Trades.DefaultCellStyle = dataGridViewCellStyle14;
            this.dg_Trades.EnableHeadersVisualStyles = false;
            this.dg_Trades.Location = new System.Drawing.Point(6, 56);
            this.dg_Trades.Name = "dg_Trades";
            this.dg_Trades.Size = new System.Drawing.Size(1023, 428);
            this.dg_Trades.TabIndex = 2;
            this.dg_Trades.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_Trades_CellMouseClick);
            this.dg_Trades.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dg_Trades_CellBeginEdit);
            this.dg_Trades.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dg_Trades_MouseClick);
            this.dg_Trades.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_Trades_CellEndEdit);
            this.dg_Trades.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dg_Trades_DataError);
            // 
            // cb_SelectAll
            // 
            this.cb_SelectAll.AutoSize = true;
            this.cb_SelectAll.ForeColor = System.Drawing.Color.DarkGreen;
            this.cb_SelectAll.Location = new System.Drawing.Point(22, 81);
            this.cb_SelectAll.Name = "cb_SelectAll";
            this.cb_SelectAll.Size = new System.Drawing.Size(70, 17);
            this.cb_SelectAll.TabIndex = 24;
            this.cb_SelectAll.Text = "Select All";
            this.cb_SelectAll.UseVisualStyleBackColor = true;
            this.cb_SelectAll.CheckedChanged += new System.EventHandler(this.cb_SelectAll_CheckedChanged);
            // 
            // bt_CreateTrades
            // 
            this.bt_CreateTrades.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_CreateTrades.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_CreateTrades.ForeColor = System.Drawing.Color.DarkGreen;
            this.bt_CreateTrades.Location = new System.Drawing.Point(848, 6);
            this.bt_CreateTrades.Name = "bt_CreateTrades";
            this.bt_CreateTrades.Size = new System.Drawing.Size(168, 20);
            this.bt_CreateTrades.TabIndex = 25;
            this.bt_CreateTrades.Text = "Create Trades From Order";
            this.bt_CreateTrades.UseVisualStyleBackColor = true;
            this.bt_CreateTrades.Click += new System.EventHandler(this.bt_CreateTrades_Click);
            // 
            // bt_SendToBrokers
            // 
            this.bt_SendToBrokers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_SendToBrokers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_SendToBrokers.ForeColor = System.Drawing.Color.DarkBlue;
            this.bt_SendToBrokers.Location = new System.Drawing.Point(909, 6);
            this.bt_SendToBrokers.Name = "bt_SendToBrokers";
            this.bt_SendToBrokers.Size = new System.Drawing.Size(117, 20);
            this.bt_SendToBrokers.TabIndex = 26;
            this.bt_SendToBrokers.Text = "Send To Brokers";
            this.bt_SendToBrokers.UseVisualStyleBackColor = true;
            this.bt_SendToBrokers.Click += new System.EventHandler(this.bt_SendToBrokers_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tp_OrdersToTrades);
            this.tabControl1.Controls.Add(this.tp_TradesForBrokers);
            this.tabControl1.Controls.Add(this.tp_TradesForCustodian);
            this.tabControl1.Controls.Add(this.tp_PreviouslyProcessedTrades);
            this.tabControl1.Controls.Add(this.tp_ResendCustodianFile);
            this.tabControl1.Location = new System.Drawing.Point(12, 104);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1040, 513);
            this.tabControl1.TabIndex = 27;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tp_OrdersToTrades
            // 
            this.tp_OrdersToTrades.Controls.Add(this.dg_Trades);
            this.tp_OrdersToTrades.Controls.Add(this.cb_AggregateTrades);
            this.tp_OrdersToTrades.Controls.Add(this.bt_CreateTrades);
            this.tp_OrdersToTrades.Controls.Add(this.label1);
            this.tp_OrdersToTrades.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tp_OrdersToTrades.Location = new System.Drawing.Point(4, 22);
            this.tp_OrdersToTrades.Name = "tp_OrdersToTrades";
            this.tp_OrdersToTrades.Padding = new System.Windows.Forms.Padding(3);
            this.tp_OrdersToTrades.Size = new System.Drawing.Size(1032, 487);
            this.tp_OrdersToTrades.TabIndex = 0;
            this.tp_OrdersToTrades.Text = "Step 1 - Orders To Trades";
            this.tp_OrdersToTrades.UseVisualStyleBackColor = true;
            // 
            // tp_TradesForBrokers
            // 
            this.tp_TradesForBrokers.Controls.Add(this.bt_MarkAsSent);
            this.tp_TradesForBrokers.Controls.Add(this.lb_TradeForBrokers);
            this.tp_TradesForBrokers.Controls.Add(this.bt_SendToBrokers);
            this.tp_TradesForBrokers.Controls.Add(this.dg_ForBrokers);
            this.tp_TradesForBrokers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tp_TradesForBrokers.Location = new System.Drawing.Point(4, 22);
            this.tp_TradesForBrokers.Name = "tp_TradesForBrokers";
            this.tp_TradesForBrokers.Padding = new System.Windows.Forms.Padding(3);
            this.tp_TradesForBrokers.Size = new System.Drawing.Size(1032, 487);
            this.tp_TradesForBrokers.TabIndex = 1;
            this.tp_TradesForBrokers.Text = "Step 2 - Trades For Brokers";
            this.tp_TradesForBrokers.UseVisualStyleBackColor = true;
            // 
            // bt_MarkAsSent
            // 
            this.bt_MarkAsSent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_MarkAsSent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_MarkAsSent.ForeColor = System.Drawing.Color.Maroon;
            this.bt_MarkAsSent.Location = new System.Drawing.Point(909, 32);
            this.bt_MarkAsSent.Name = "bt_MarkAsSent";
            this.bt_MarkAsSent.Size = new System.Drawing.Size(117, 20);
            this.bt_MarkAsSent.TabIndex = 27;
            this.bt_MarkAsSent.Tag = "Process the Trade, but do not send the email directly to the Broker.";
            this.bt_MarkAsSent.Text = "Mark as Sent";
            this.bt_MarkAsSent.UseVisualStyleBackColor = true;
            this.bt_MarkAsSent.Click += new System.EventHandler(this.bt_MarkAsSent_Click);
            // 
            // lb_TradeForBrokers
            // 
            this.lb_TradeForBrokers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_TradeForBrokers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_TradeForBrokers.ForeColor = System.Drawing.Color.DarkBlue;
            this.lb_TradeForBrokers.Location = new System.Drawing.Point(6, 3);
            this.lb_TradeForBrokers.Name = "lb_TradeForBrokers";
            this.lb_TradeForBrokers.Size = new System.Drawing.Size(382, 51);
            this.lb_TradeForBrokers.TabIndex = 4;
            this.lb_TradeForBrokers.Text = "This step will email Trades to each Broker.\r\nThe Brokers will send back confirms." +
                " Reconcile && Update any diferences.\r\nThen move to Step 3 to Forward these Confi" +
                "rmed Trades to the Custodian.";
            // 
            // dg_ForBrokers
            // 
            this.dg_ForBrokers.AllowUserToAddRows = false;
            this.dg_ForBrokers.AllowUserToDeleteRows = false;
            this.dg_ForBrokers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_ForBrokers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.dg_ForBrokers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_ForBrokers.DefaultCellStyle = dataGridViewCellStyle16;
            this.dg_ForBrokers.EnableHeadersVisualStyles = false;
            this.dg_ForBrokers.Location = new System.Drawing.Point(5, 57);
            this.dg_ForBrokers.Name = "dg_ForBrokers";
            this.dg_ForBrokers.Size = new System.Drawing.Size(1023, 414);
            this.dg_ForBrokers.TabIndex = 3;
            this.dg_ForBrokers.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_ForBrokers_CellMouseClick);
            this.dg_ForBrokers.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dg_ForBrokers_MouseClick);
            // 
            // tp_TradesForCustodian
            // 
            this.tp_TradesForCustodian.Controls.Add(this.cb_CloseWhenDone);
            this.tp_TradesForCustodian.Controls.Add(this.bt_OpenBookingsDirectory);
            this.tp_TradesForCustodian.Controls.Add(this.bt_CheckContractNotes);
            this.tp_TradesForCustodian.Controls.Add(this.cb_ShowUnconfirmedTrades);
            this.tp_TradesForCustodian.Controls.Add(this.lb_SendToCustodian);
            this.tp_TradesForCustodian.Controls.Add(this.dg_ForCustodians);
            this.tp_TradesForCustodian.Controls.Add(this.bt_SendToCustodian);
            this.tp_TradesForCustodian.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tp_TradesForCustodian.Location = new System.Drawing.Point(4, 22);
            this.tp_TradesForCustodian.Name = "tp_TradesForCustodian";
            this.tp_TradesForCustodian.Size = new System.Drawing.Size(1032, 487);
            this.tp_TradesForCustodian.TabIndex = 2;
            this.tp_TradesForCustodian.Text = "Step 3 - Broker Confirm / Trades For Custodian";
            this.tp_TradesForCustodian.UseVisualStyleBackColor = true;
            // 
            // cb_CloseWhenDone
            // 
            this.cb_CloseWhenDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_CloseWhenDone.AutoSize = true;
            this.cb_CloseWhenDone.Checked = true;
            this.cb_CloseWhenDone.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_CloseWhenDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_CloseWhenDone.ForeColor = System.Drawing.Color.DarkGreen;
            this.cb_CloseWhenDone.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cb_CloseWhenDone.Location = new System.Drawing.Point(422, 15);
            this.cb_CloseWhenDone.Name = "cb_CloseWhenDone";
            this.cb_CloseWhenDone.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cb_CloseWhenDone.Size = new System.Drawing.Size(218, 17);
            this.cb_CloseWhenDone.TabIndex = 33;
            this.cb_CloseWhenDone.Tag = "";
            this.cb_CloseWhenDone.Text = "Auto-send to Custodian and Close";
            this.cb_CloseWhenDone.UseVisualStyleBackColor = true;
            // 
            // bt_OpenBookingsDirectory
            // 
            this.bt_OpenBookingsDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_OpenBookingsDirectory.Enabled = false;
            this.bt_OpenBookingsDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_OpenBookingsDirectory.ForeColor = System.Drawing.Color.RoyalBlue;
            this.bt_OpenBookingsDirectory.Location = new System.Drawing.Point(690, 37);
            this.bt_OpenBookingsDirectory.Name = "bt_OpenBookingsDirectory";
            this.bt_OpenBookingsDirectory.Size = new System.Drawing.Size(161, 20);
            this.bt_OpenBookingsDirectory.TabIndex = 32;
            this.bt_OpenBookingsDirectory.Text = "Open Bookings Directory";
            this.bt_OpenBookingsDirectory.UseVisualStyleBackColor = true;
            this.bt_OpenBookingsDirectory.Click += new System.EventHandler(this.bt_OpenBookingsDirectory_Click);
            // 
            // bt_CheckContractNotes
            // 
            this.bt_CheckContractNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_CheckContractNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_CheckContractNotes.ForeColor = System.Drawing.Color.Green;
            this.bt_CheckContractNotes.Location = new System.Drawing.Point(715, 12);
            this.bt_CheckContractNotes.Name = "bt_CheckContractNotes";
            this.bt_CheckContractNotes.Size = new System.Drawing.Size(136, 20);
            this.bt_CheckContractNotes.TabIndex = 31;
            this.bt_CheckContractNotes.Text = "Check Contract Notes";
            this.bt_CheckContractNotes.UseVisualStyleBackColor = true;
            this.bt_CheckContractNotes.Click += new System.EventHandler(this.bt_CheckContractNotes_Click);
            // 
            // cb_ShowUnconfirmedTrades
            // 
            this.cb_ShowUnconfirmedTrades.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_ShowUnconfirmedTrades.AutoSize = true;
            this.cb_ShowUnconfirmedTrades.Enabled = false;
            this.cb_ShowUnconfirmedTrades.ForeColor = System.Drawing.Color.MediumBlue;
            this.cb_ShowUnconfirmedTrades.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cb_ShowUnconfirmedTrades.Location = new System.Drawing.Point(859, 38);
            this.cb_ShowUnconfirmedTrades.Name = "cb_ShowUnconfirmedTrades";
            this.cb_ShowUnconfirmedTrades.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cb_ShowUnconfirmedTrades.Size = new System.Drawing.Size(160, 17);
            this.cb_ShowUnconfirmedTrades.TabIndex = 30;
            this.cb_ShowUnconfirmedTrades.Tag = "This is the prefered method as it saves on Prime Broker fees";
            this.cb_ShowUnconfirmedTrades.Text = "Include Unconfirmed Trades";
            this.cb_ShowUnconfirmedTrades.UseVisualStyleBackColor = true;
            // 
            // lb_SendToCustodian
            // 
            this.lb_SendToCustodian.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_SendToCustodian.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_SendToCustodian.ForeColor = System.Drawing.Color.Indigo;
            this.lb_SendToCustodian.Location = new System.Drawing.Point(6, 9);
            this.lb_SendToCustodian.Name = "lb_SendToCustodian";
            this.lb_SendToCustodian.Size = new System.Drawing.Size(382, 51);
            this.lb_SendToCustodian.TabIndex = 29;
            this.lb_SendToCustodian.Text = "When the Broker Confirms have occured, then send the Trades to the Custodian.Note" +
                ": You are able to Send WITHOUT the broker confirms, but will then need to reconc" +
                "ile again.";
            // 
            // dg_ForCustodians
            // 
            this.dg_ForCustodians.AllowUserToAddRows = false;
            this.dg_ForCustodians.AllowUserToDeleteRows = false;
            this.dg_ForCustodians.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_ForCustodians.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.dg_ForCustodians.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_ForCustodians.DefaultCellStyle = dataGridViewCellStyle15;
            this.dg_ForCustodians.EnableHeadersVisualStyles = false;
            this.dg_ForCustodians.Location = new System.Drawing.Point(5, 63);
            this.dg_ForCustodians.Name = "dg_ForCustodians";
            this.dg_ForCustodians.Size = new System.Drawing.Size(1023, 414);
            this.dg_ForCustodians.TabIndex = 28;
            this.dg_ForCustodians.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_ForCustodians_CellMouseClick);
            this.dg_ForCustodians.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dg_ForCustodians_CellBeginEdit);
            this.dg_ForCustodians.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dg_ForCustodians_MouseClick);
            this.dg_ForCustodians.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_ForCustodians_CellEndEdit);
            this.dg_ForCustodians.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dg_ForCustodians_DataError);
            // 
            // bt_SendToCustodian
            // 
            this.bt_SendToCustodian.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_SendToCustodian.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_SendToCustodian.ForeColor = System.Drawing.Color.Indigo;
            this.bt_SendToCustodian.Location = new System.Drawing.Point(883, 12);
            this.bt_SendToCustodian.Name = "bt_SendToCustodian";
            this.bt_SendToCustodian.Size = new System.Drawing.Size(136, 20);
            this.bt_SendToCustodian.TabIndex = 27;
            this.bt_SendToCustodian.Text = "Send To Custodian";
            this.bt_SendToCustodian.UseVisualStyleBackColor = true;
            this.bt_SendToCustodian.Click += new System.EventHandler(this.bt_SendToCustodian_Click);
            // 
            // tp_PreviouslyProcessedTrades
            // 
            this.tp_PreviouslyProcessedTrades.Controls.Add(this.label6);
            this.tp_PreviouslyProcessedTrades.Controls.Add(this.label5);
            this.tp_PreviouslyProcessedTrades.Controls.Add(this.dtp_ToDate);
            this.tp_PreviouslyProcessedTrades.Controls.Add(this.dtp_FromDate);
            this.tp_PreviouslyProcessedTrades.Controls.Add(this.bt_RequestHistoricTrades);
            this.tp_PreviouslyProcessedTrades.Controls.Add(this.label4);
            this.tp_PreviouslyProcessedTrades.Controls.Add(this.dg_HistoricTrades);
            this.tp_PreviouslyProcessedTrades.Location = new System.Drawing.Point(4, 22);
            this.tp_PreviouslyProcessedTrades.Name = "tp_PreviouslyProcessedTrades";
            this.tp_PreviouslyProcessedTrades.Padding = new System.Windows.Forms.Padding(3);
            this.tp_PreviouslyProcessedTrades.Size = new System.Drawing.Size(1032, 487);
            this.tp_PreviouslyProcessedTrades.TabIndex = 3;
            this.tp_PreviouslyProcessedTrades.Text = "Previously Processed Trades";
            this.tp_PreviouslyProcessedTrades.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.SteelBlue;
            this.label6.Location = new System.Drawing.Point(627, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 13);
            this.label6.TabIndex = 37;
            this.label6.Text = "To";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.SteelBlue;
            this.label5.Location = new System.Drawing.Point(414, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "From";
            // 
            // dtp_ToDate
            // 
            this.dtp_ToDate.Location = new System.Drawing.Point(627, 25);
            this.dtp_ToDate.Name = "dtp_ToDate";
            this.dtp_ToDate.Size = new System.Drawing.Size(200, 20);
            this.dtp_ToDate.TabIndex = 35;
            // 
            // dtp_FromDate
            // 
            this.dtp_FromDate.Location = new System.Drawing.Point(411, 25);
            this.dtp_FromDate.Name = "dtp_FromDate";
            this.dtp_FromDate.Size = new System.Drawing.Size(200, 20);
            this.dtp_FromDate.TabIndex = 34;
            // 
            // bt_RequestHistoricTrades
            // 
            this.bt_RequestHistoricTrades.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_RequestHistoricTrades.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_RequestHistoricTrades.ForeColor = System.Drawing.Color.DarkBlue;
            this.bt_RequestHistoricTrades.Location = new System.Drawing.Point(855, 25);
            this.bt_RequestHistoricTrades.Name = "bt_RequestHistoricTrades";
            this.bt_RequestHistoricTrades.Size = new System.Drawing.Size(157, 20);
            this.bt_RequestHistoricTrades.TabIndex = 33;
            this.bt_RequestHistoricTrades.Text = "Request Historic Trades";
            this.bt_RequestHistoricTrades.UseVisualStyleBackColor = true;
            this.bt_RequestHistoricTrades.Click += new System.EventHandler(this.bt_RequestHistoricTrades_Click);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(6, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(382, 51);
            this.label4.TabIndex = 32;
            this.label4.Text = "BEWARE: This screen allows you to Reverse Trades after they have been sent to the" +
                " Broker or Custodian.  This will generate trade reversals.";
            // 
            // dg_HistoricTrades
            // 
            this.dg_HistoricTrades.AllowUserToAddRows = false;
            this.dg_HistoricTrades.AllowUserToDeleteRows = false;
            this.dg_HistoricTrades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_HistoricTrades.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_HistoricTrades.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dg_HistoricTrades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_HistoricTrades.DefaultCellStyle = dataGridViewCellStyle18;
            this.dg_HistoricTrades.EnableHeadersVisualStyles = false;
            this.dg_HistoricTrades.Location = new System.Drawing.Point(5, 63);
            this.dg_HistoricTrades.Name = "dg_HistoricTrades";
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_HistoricTrades.RowHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.dg_HistoricTrades.Size = new System.Drawing.Size(1023, 414);
            this.dg_HistoricTrades.TabIndex = 31;
            this.dg_HistoricTrades.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_HistoricTrades_CellMouseClick);
            this.dg_HistoricTrades.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dg_HistoricTrades_MouseClick);
            // 
            // tp_ResendCustodianFile
            // 
            this.tp_ResendCustodianFile.Controls.Add(this.bt_RequestHistoricFiles);
            this.tp_ResendCustodianFile.Controls.Add(this.dg_CustodianFiles);
            this.tp_ResendCustodianFile.Controls.Add(this.label3);
            this.tp_ResendCustodianFile.Location = new System.Drawing.Point(4, 22);
            this.tp_ResendCustodianFile.Name = "tp_ResendCustodianFile";
            this.tp_ResendCustodianFile.Padding = new System.Windows.Forms.Padding(3);
            this.tp_ResendCustodianFile.Size = new System.Drawing.Size(1032, 487);
            this.tp_ResendCustodianFile.TabIndex = 4;
            this.tp_ResendCustodianFile.Text = "Resend Custodian File";
            this.tp_ResendCustodianFile.UseVisualStyleBackColor = true;
            // 
            // bt_RequestHistoricFiles
            // 
            this.bt_RequestHistoricFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_RequestHistoricFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_RequestHistoricFiles.ForeColor = System.Drawing.Color.DarkBlue;
            this.bt_RequestHistoricFiles.Location = new System.Drawing.Point(869, 6);
            this.bt_RequestHistoricFiles.Name = "bt_RequestHistoricFiles";
            this.bt_RequestHistoricFiles.Size = new System.Drawing.Size(157, 20);
            this.bt_RequestHistoricFiles.TabIndex = 32;
            this.bt_RequestHistoricFiles.Text = "Request Historic Files";
            this.bt_RequestHistoricFiles.UseVisualStyleBackColor = true;
            this.bt_RequestHistoricFiles.Click += new System.EventHandler(this.bt_RequestHistoricFiles_Click);
            // 
            // dg_CustodianFiles
            // 
            this.dg_CustodianFiles.AllowUserToAddRows = false;
            this.dg_CustodianFiles.AllowUserToDeleteRows = false;
            this.dg_CustodianFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_CustodianFiles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_CustodianFiles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle20;
            this.dg_CustodianFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_CustodianFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Custodian,
            this.FileName,
            this.FileMethod,
            this.FileSent,
            this.TradesSent,
            this.FileConfirmed,
            this.TimeStamp,
            this.MLClientBatchID});
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle25.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle25.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_CustodianFiles.DefaultCellStyle = dataGridViewCellStyle25;
            this.dg_CustodianFiles.EnableHeadersVisualStyles = false;
            this.dg_CustodianFiles.Location = new System.Drawing.Point(5, 57);
            this.dg_CustodianFiles.Name = "dg_CustodianFiles";
            this.dg_CustodianFiles.ReadOnly = true;
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle26.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_CustodianFiles.RowHeadersDefaultCellStyle = dataGridViewCellStyle26;
            this.dg_CustodianFiles.RowHeadersVisible = false;
            this.dg_CustodianFiles.Size = new System.Drawing.Size(1023, 427);
            this.dg_CustodianFiles.TabIndex = 31;
            this.dg_CustodianFiles.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_CustodianFiles_CellMouseClick);
            this.dg_CustodianFiles.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dg_CustodianFiles_MouseClick);
            // 
            // Custodian
            // 
            this.Custodian.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Custodian.HeaderText = "Custodian";
            this.Custodian.MinimumWidth = 150;
            this.Custodian.Name = "Custodian";
            this.Custodian.ReadOnly = true;
            this.Custodian.Width = 150;
            // 
            // FileName
            // 
            this.FileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.FileName.DefaultCellStyle = dataGridViewCellStyle21;
            this.FileName.HeaderText = "File Name";
            this.FileName.MinimumWidth = 150;
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            this.FileName.Width = 150;
            // 
            // FileMethod
            // 
            this.FileMethod.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.FileMethod.DefaultCellStyle = dataGridViewCellStyle22;
            this.FileMethod.HeaderText = "Method Sent";
            this.FileMethod.MinimumWidth = 30;
            this.FileMethod.Name = "FileMethod";
            this.FileMethod.ReadOnly = true;
            this.FileMethod.Width = 86;
            // 
            // FileSent
            // 
            this.FileSent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle23.Format = "F";
            dataGridViewCellStyle23.NullValue = null;
            this.FileSent.DefaultCellStyle = dataGridViewCellStyle23;
            this.FileSent.HeaderText = "Sent";
            this.FileSent.MinimumWidth = 100;
            this.FileSent.Name = "FileSent";
            this.FileSent.ReadOnly = true;
            // 
            // TradesSent
            // 
            this.TradesSent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TradesSent.HeaderText = "# of Trades";
            this.TradesSent.MinimumWidth = 30;
            this.TradesSent.Name = "TradesSent";
            this.TradesSent.ReadOnly = true;
            this.TradesSent.Width = 80;
            // 
            // FileConfirmed
            // 
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle24.ForeColor = System.Drawing.Color.Green;
            dataGridViewCellStyle24.NullValue = false;
            this.FileConfirmed.DefaultCellStyle = dataGridViewCellStyle24;
            this.FileConfirmed.FalseValue = "N";
            this.FileConfirmed.HeaderText = "Broker Confirmed Received";
            this.FileConfirmed.MinimumWidth = 70;
            this.FileConfirmed.Name = "FileConfirmed";
            this.FileConfirmed.ReadOnly = true;
            this.FileConfirmed.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FileConfirmed.TrueValue = "Y";
            this.FileConfirmed.Width = 70;
            // 
            // TimeStamp
            // 
            this.TimeStamp.HeaderText = "MLTimeStamp";
            this.TimeStamp.Name = "TimeStamp";
            this.TimeStamp.ReadOnly = true;
            this.TimeStamp.Visible = false;
            this.TimeStamp.Width = 5;
            // 
            // MLClientBatchID
            // 
            this.MLClientBatchID.HeaderText = "MLClientBatchID";
            this.MLClientBatchID.Name = "MLClientBatchID";
            this.MLClientBatchID.ReadOnly = true;
            this.MLClientBatchID.Visible = false;
            this.MLClientBatchID.Width = 5;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Indigo;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(428, 51);
            this.label3.TabIndex = 30;
            this.label3.Text = "If for some reason a file does not get sent to the Prime Broker, then this area a" +
                "llows you to resend the exact file. Right-Click over the file and choose [Re-Sen" +
                "d].";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DarkGreen;
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(674, 69);
            this.label2.TabIndex = 28;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // bt_Refresh
            // 
            this.bt_Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Refresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Refresh.ForeColor = System.Drawing.Color.DarkGreen;
            this.bt_Refresh.Location = new System.Drawing.Point(974, 12);
            this.bt_Refresh.Name = "bt_Refresh";
            this.bt_Refresh.Size = new System.Drawing.Size(78, 20);
            this.bt_Refresh.TabIndex = 29;
            this.bt_Refresh.Text = "Refresh";
            this.bt_Refresh.UseVisualStyleBackColor = true;
            this.bt_Refresh.Click += new System.EventHandler(this.bt_Refresh_Click);
            // 
            // timer_CheckContractNotes
            // 
            this.timer_CheckContractNotes.Interval = 60000;
            this.timer_CheckContractNotes.Tag = "This does the same thing as pressing the [Check Contract] button.";
            this.timer_CheckContractNotes.Tick += new System.EventHandler(this.timer_CheckContractNotes_Tick);
            // 
            // ProcessTrades
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 629);
            this.Controls.Add(this.bt_Refresh);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cb_SelectAll);
            this.Name = "ProcessTrades";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Process Trades";
            this.Load += new System.EventHandler(this.ProcessTrades_Load);
            this.Shown += new System.EventHandler(this.ProcessTrades_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProcessTrades_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dg_Trades)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tp_OrdersToTrades.ResumeLayout(false);
            this.tp_OrdersToTrades.PerformLayout();
            this.tp_TradesForBrokers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg_ForBrokers)).EndInit();
            this.tp_TradesForCustodian.ResumeLayout(false);
            this.tp_TradesForCustodian.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_ForCustodians)).EndInit();
            this.tp_PreviouslyProcessedTrades.ResumeLayout(false);
            this.tp_PreviouslyProcessedTrades.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_HistoricTrades)).EndInit();
            this.tp_ResendCustodianFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg_CustodianFiles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_AggregateTrades;
        private System.Windows.Forms.DataGridView dg_Trades;
        private System.Windows.Forms.CheckBox cb_SelectAll;
        private System.Windows.Forms.Button bt_CreateTrades;
        private System.Windows.Forms.Button bt_SendToBrokers;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tp_OrdersToTrades;
        private System.Windows.Forms.TabPage tp_TradesForBrokers;
        private System.Windows.Forms.DataGridView dg_ForBrokers;
        private System.Windows.Forms.TabPage tp_TradesForCustodian;
        private System.Windows.Forms.Label lb_TradeForBrokers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb_SendToCustodian;
        private System.Windows.Forms.DataGridView dg_ForCustodians;
        private System.Windows.Forms.Button bt_SendToCustodian;
        private System.Windows.Forms.CheckBox cb_ShowUnconfirmedTrades;
        private System.Windows.Forms.Button bt_Refresh;
        private System.Windows.Forms.Button bt_MarkAsSent;
        private System.Windows.Forms.Button bt_CheckContractNotes;
        private System.Windows.Forms.Timer timer_CheckContractNotes;
        private System.Windows.Forms.Button bt_OpenBookingsDirectory;
        private System.Windows.Forms.TabPage tp_PreviouslyProcessedTrades;
        private System.Windows.Forms.TabPage tp_ResendCustodianFile;
        private System.Windows.Forms.DataGridView dg_CustodianFiles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bt_RequestHistoricFiles;
        private System.Windows.Forms.Button bt_RequestHistoricTrades;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dg_HistoricTrades;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtp_ToDate;
        private System.Windows.Forms.DateTimePicker dtp_FromDate;
        private System.Windows.Forms.CheckBox cb_CloseWhenDone;
        private System.Windows.Forms.DataGridViewTextBoxColumn Custodian;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileMethod;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileSent;
        private System.Windows.Forms.DataGridViewTextBoxColumn TradesSent;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FileConfirmed;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeStamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn MLClientBatchID;
    }
}