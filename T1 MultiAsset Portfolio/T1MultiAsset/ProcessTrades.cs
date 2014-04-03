using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.IO;
using System.Diagnostics;
using org.pdfbox.pdmodel;
using org.pdfbox.util;


namespace T1MultiAsset
{
    public partial class ProcessTrades : Form
    {
        // Public Variables
        public Form1 ParentForm1;
        DataTable dt_Trades;
        DataTable dt_ForBrokers;
        DataTable dt_ForCustodians;
        DataTable dt_Fund;
        DataTable dt_Portfolio;
        DataTable dt_CustodianFiles;
        DataTable dt_HistoricTrades;
        public Object LastValue;
        public Boolean inCheckContractNotes = false;
        private int CXLocation = 0;
        private int CYLocation = 0;

        public struct TradeMenuStruct
        {
             public String OrderRefID;
             public String TradeID;
             public String FileName;
             public String Custodian;
             public Boolean FileConfirmed;
             public String TimeStamp;
             public String MLNumberofRecords;
             public String MLClientBatchID;
             public ProcessTrades myParentForm;
        }

        public ProcessTrades()
        {
            InitializeComponent();
        }

        private void ProcessTrades_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            LoadAll();

            if (SystemLibrary.BookingsFilePath == null)
                SystemLibrary.BookingsFilePath = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('Bookings_Path')");

            if (SystemLibrary.BookingsFilePath.Length > 0)
                bt_OpenBookingsDirectory.Enabled = true;

            dtp_FromDate.Value = SystemLibrary.f_Today().AddDays(-2);
            dtp_ToDate.Value = SystemLibrary.f_Today();
        }

        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;
            ParentForm1.CopyDataTable(ref dt_Fund, ref dt_Portfolio);
        } //FromParent()

        public void LoadAll()
        {
            LoadTrades();
            LoadForBrokers();
            LoadForCustodians();

            // Set the opening Tab
            //tabControl1.TabPages["tp_OrdersToTrades"].Select;
            if (dt_Trades.Rows.Count > 0)
                tabControl1.SelectTab(tp_OrdersToTrades);
            else if (dt_ForBrokers.Rows.Count > 0)
                tabControl1.SelectTab(tp_TradesForBrokers);
            else if (dt_ForCustodians.Rows.Count > 0)
                tabControl1.SelectTab(tp_TradesForCustodian);

        } //LoadAll()

        public void LoadTrades()
        {
            //TODO (3) - dg_Trades - CommRate needs to have an identifier if $ or %. Currently showing Futures rate @ 350%.

            // Local Variables
            String mySql;

            // Get the Data
            mySql = "Exec sp_ProcessTrade_Load '" + SystemLibrary.Bool_To_YN(cb_AggregateTrades.Checked) + "' ";
            dt_Trades = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_Trades.DataSource = dt_Trades;
            dg_Trades.Refresh();

            // Sometine the SQL fails
            if (!dg_Trades.Columns.Contains("TradeID"))
            {
                MessageBox.Show("sp_ProcessTrade_Load failed to even bring back headers");
                return;
            }
            // Hide Reference columns
            dg_Trades.Columns["TradeID"].Visible = false;
            dg_Trades.Columns["FundID"].Visible = false;
            dg_Trades.Columns["PortfolioID"].Visible = false;
            dg_Trades.Columns["Pos_Mult_Factor"].Visible = false;
            dg_Trades.Columns["SentToBroker"].Visible = false;
            dg_Trades.Columns["BrokerConfirmed"].Visible = false;
            dg_Trades.Columns["SentToCustodian"].Visible = false;
            dg_Trades.Columns["CustodianConfirmed"].Visible = false;
            dg_Trades.Columns["crncy"].Visible = false;
            dg_Trades.Columns["PM"].Visible = false;
            dg_Trades.Columns["IdeaOwner"].Visible = false;
            dg_Trades.Columns["Strategy1"].Visible = false;
            dg_Trades.Columns["Strategy2"].Visible = false;
            dg_Trades.Columns["Strategy3"].Visible = false;
            dg_Trades.Columns["Strategy4"].Visible = false;
            dg_Trades.Columns["Strategy5"].Visible = false;
            dg_Trades.Columns["UpdateDate"].Visible = false;
            dg_Trades.Columns["BrokerID"].Visible = false;
            dg_Trades.Columns["CustodianID"].Visible = false;
            dg_Trades.Columns["FillNo"].Visible = false;

            dg_Trades.Columns["BBG_Ticker"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg_Trades.Columns["FundName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg_Trades.Columns["PortfolioName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg_Trades.Columns["BrokerName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg_Trades.Columns["CustodianName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dg_Trades.Columns["GrossValue"].HeaderText = "Gross Value";
            dg_Trades.Columns["CommissionRate"].HeaderText = "Comm Rate";
            dg_Trades.Columns["NetValue"].HeaderText = "Net Value";
            dg_Trades.Columns["TradeDate"].HeaderText = "Trade Date";
            dg_Trades.Columns["SettlementDate"].HeaderText = "Settle Date";
            dg_Trades.Columns["Exchange"].HeaderText = "Exch";
            dg_Trades.Columns["FundName"].HeaderText = "Fund";
            dg_Trades.Columns["PortfolioName"].HeaderText = "Portfolio";
            dg_Trades.Columns["BrokerName"].HeaderText = "Broker";
            dg_Trades.Columns["CustodianName"].HeaderText = "Custodian";

            if (dg_Trades.Columns.Contains("Send"))
                dg_Trades.Columns.Remove("Send");
            DataGridViewCheckBoxColumn Send = new DataGridViewCheckBoxColumn();
            Send.HeaderText = "Send";
            Send.FalseValue = "N";
            Send.TrueValue = "Y";
            Send.Name = "Send";
            Send.DataPropertyName = "Send";
            dg_Trades.Columns.Insert(0, Send);

            ParentForm1.SetFormatColumn(dg_Trades, "Quantity", Color.Empty, Color.Empty, "N0", "0");
            //ParentForm1.SetFormatColumn(dg_Trades, "Price", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dg_Trades, "GrossValue", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dg_Trades, "CommissionRate", Color.Empty, Color.Empty, "0.00%", "0");
            ParentForm1.SetFormatColumn(dg_Trades, "Commission", Color.Empty, Color.FromArgb(192, 255, 192), "N2", "0");
            ParentForm1.SetFormatColumn(dg_Trades, "Stamp", Color.Empty, Color.FromArgb(192, 255, 192), "N2", "0");
            ParentForm1.SetFormatColumn(dg_Trades, "GST", Color.Empty, Color.FromArgb(192, 255, 192), "N2", "0");
            ParentForm1.SetFormatColumn(dg_Trades, "NetValue", Color.Empty, Color.FromArgb(192, 255, 192), "N2", "0");
            dg_Trades.Columns["TradeDate"].DefaultCellStyle.Format = "dd-MMM-yy"; // M = dd-Mon, D = Short Date/Time, F = Long Date/Time
            dg_Trades.Columns["TradeDate"].DefaultCellStyle.ForeColor = Color.Blue;
            dg_Trades.Columns["SettlementDate"].DefaultCellStyle.Format = "dd-MMM-yy"; // M = dd-Mon, D = Short Date/Time, F = Long Date/Time
            dg_Trades.Columns["SettlementDate"].DefaultCellStyle.BackColor = Color.FromArgb(192, 255, 192);

            for (Int32 i = 0; i < dg_Trades.Rows.Count; i++)
            {
                dg_Trades["Send", i].Value = "N";
                ParentForm1.SetColumn(dg_Trades, "Quantity", i);
                ParentForm1.SetColumn(dg_Trades, "GrossValue", i);
                ParentForm1.SetColumn(dg_Trades, "Commission", i);
                ParentForm1.SetColumn(dg_Trades, "Stamp", i);
                ParentForm1.SetColumn(dg_Trades, "GST", i);
                if (Math.Round(SystemLibrary.ToDecimal(dg_Trades.Rows[0].Cells["Quantity"].Value), 0) != SystemLibrary.ToDecimal(dg_Trades.Rows[0].Cells["Quantity"].Value))
                    dg_Trades.Rows[0].Cells["Quantity"].Style.Format = "N2";
            }


            // Loop on all columns and set the Autosize mode & Header Text
            for (int i = 0; i < dg_Trades.Columns.Count; i++)
            {
                //dg_Orders.Columns[i].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
                dg_Trades.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dg_Trades.Columns[i].HeaderText = dg_Trades.Columns[i].HeaderText.Replace('_', ' ');
                dg_Trades.Columns[i].ReadOnly = true;
            }

            // Now allow some columns to be altered
            dg_Trades.Columns["Send"].ReadOnly = false;
            dg_Trades.Columns["Commission"].ReadOnly = false;
            dg_Trades.Columns["Stamp"].ReadOnly = false;
            dg_Trades.Columns["GST"].ReadOnly = false;
            dg_Trades.Columns["NetValue"].ReadOnly = false;
            dg_Trades.Columns["SettlementDate"].ReadOnly = false;



            dg_Trades.Columns["Side"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_Trades.Columns["Side"].Width = 30;
            dg_Trades.Columns["Exchange"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_Trades.Columns["Exchange"].Width = 35;
            dg_Trades.Columns["TradeDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_Trades.Columns["TradeDate"].Width = 60;
            dg_Trades.Columns["SettlementDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_Trades.Columns["SettlementDate"].Width = 60;
            // Allow the User to modify the Settlement Date
            dg_Trades.Columns["SettlementDate"].ReadOnly = false;

            tabControl1.TabPages["tp_OrdersToTrades"].Text = "Step 1 - Orders To Trades (" + dg_Trades.Rows.Count.ToString() + ")";
        } //LoadTrades()

        public void LoadForBrokers()
        {
            // Local Variables
            String mySql;

            // Get the Data
            mySql = "Exec sp_ProcessTrade_ForBroker ";
            dt_ForBrokers = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_ForBrokers.DataSource = dt_ForBrokers;
            dg_ForBrokers.Refresh();

            // Hide Reference columns
            //dg_ForBrokers.Columns["TradeID"].Visible = false;
            //dg_ForBrokers.Columns["FundID"].Visible = false;
            //dg_ForBrokers.Columns["PortfolioID"].Visible = false;
            //dg_ForBrokers.Columns["Pos_Mult_Factor"].Visible = false;
            //dg_ForBrokers.Columns["SentToBroker"].Visible = false;
            //dg_ForBrokers.Columns["BrokerConfirmed"].Visible = false;
            //dg_ForBrokers.Columns["SentToCustodian"].Visible = false;
            //dg_ForBrokers.Columns["CustodianConfirmed"].Visible = false;
            dg_ForBrokers.Columns["Currency"].Visible = false;
            dg_ForBrokers.Columns["IsInternal"].Visible = false;
            dg_ForBrokers.Columns["EmailSalutation"].Visible = false;
            dg_ForBrokers.Columns["Email"].Visible = false;

            dg_ForBrokers.Columns["BBG_Ticker"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg_ForBrokers.Columns["FundName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //dg_ForBrokers.Columns["PortfolioName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg_ForBrokers.Columns["BrokerName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg_ForBrokers.Columns["CustodianID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dg_ForBrokers.Columns["GrossValue"].HeaderText = "Gross Value";
            //dg_ForBrokers.Columns["CommissionRate"].HeaderText = "Comm Rate";
            dg_ForBrokers.Columns["NetValue"].HeaderText = "Net Value";
            dg_ForBrokers.Columns["TradeDate"].HeaderText = "Trade Date";
            dg_ForBrokers.Columns["SettlementDate"].HeaderText = "Settle Date";
            //dg_ForBrokers.Columns["Exchange"].HeaderText = "Exch";
            dg_ForBrokers.Columns["FundName"].HeaderText = "Fund";
            //dg_ForBrokers.Columns["PortfolioName"].HeaderText = "Portfolio";
            dg_ForBrokers.Columns["BrokerName"].HeaderText = "Broker";
            //dg_ForBrokers.Columns["CustodianID"].HeaderText = "Custodian";

            if (dg_ForBrokers.Columns.Contains("Send"))
                dg_ForBrokers.Columns.Remove("Send");
            DataGridViewCheckBoxColumn Send = new DataGridViewCheckBoxColumn();
            Send.HeaderText = "Send";
            Send.FalseValue = "N";
            Send.TrueValue = "Y";
            Send.Name = "Send";
            Send.DataPropertyName = "Send";
            dg_ForBrokers.Columns.Insert(0, Send);

            ParentForm1.SetFormatColumn(dg_ForBrokers, "Quantity", Color.Empty, Color.Empty, "N0", "0");
            //ParentForm1.SetFormatColumn(dg_ForBrokers, "Price", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dg_ForBrokers, "GrossValue", Color.Empty, Color.Empty, "N2", "0");
            //ParentForm1.SetFormatColumn(dg_ForBrokers, "CommissionRate", Color.Empty, Color.Empty, "0.00%", "0");
            ParentForm1.SetFormatColumn(dg_ForBrokers, "Commission", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dg_ForBrokers, "Stamp", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dg_ForBrokers, "GST", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dg_ForBrokers, "NetValue", Color.Empty, Color.Empty, "N2", "0");
            dg_ForBrokers.Columns["TradeDate"].DefaultCellStyle.Format = "dd-MMM-yy"; // M = dd-Mon, D = Short Date/Time, F = Long Date/Time
            dg_ForBrokers.Columns["TradeDate"].DefaultCellStyle.ForeColor = Color.Blue;
            dg_ForBrokers.Columns["SettlementDate"].DefaultCellStyle.Format = "dd-MMM-yy"; // M = dd-Mon, D = Short Date/Time, F = Long Date/Time

            // Loop on all columns and set the Autosize mode & Header Text
            for (int i = 0; i < dg_ForBrokers.Columns.Count; i++)
            {
                //dg_Orders.Columns[i].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
                dg_ForBrokers.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dg_ForBrokers.Columns[i].HeaderText = dg_ForBrokers.Columns[i].HeaderText.Replace('_', ' ');
                dg_ForBrokers.Columns[i].ReadOnly = true;
                // Turn off Sort as Code relies on the sort order.
                dg_ForBrokers.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dg_ForBrokers.Columns["Send"].ReadOnly = false;
            dg_ForBrokers.Columns["LS"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_ForBrokers.Columns["BS"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_ForBrokers.Columns["LS"].Width = 40;
            dg_ForBrokers.Columns["BS"].Width = 30;
            //dg_ForBrokers.Columns["Exchange"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //dg_ForBrokers.Columns["Exchange"].Width = 35;
            dg_ForBrokers.Columns["TradeDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_ForBrokers.Columns["TradeDate"].Width = 60;
            dg_ForBrokers.Columns["SettlementDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_ForBrokers.Columns["SettlementDate"].Width = 60;

            tabControl1.TabPages["tp_TradesForBrokers"].Text = "Step 2 - Trades For Brokers (" + dg_ForBrokers.Rows.Count.ToString() + ")";

            FormatForBrokers();

        } //LoadForBrokers()


        public void FormatForBrokers()
        {
            for (Int32 i = 0; i < dg_ForBrokers.Rows.Count; i++)
            {
                dg_ForBrokers["Send", i].Value = "N";
                ParentForm1.SetColumn(dg_ForBrokers, "Quantity", i);
                ParentForm1.SetColumn(dg_ForBrokers, "GrossValue", i);
                ParentForm1.SetColumn(dg_ForBrokers, "Commission", i);
                ParentForm1.SetColumn(dg_ForBrokers, "Stamp", i);
                ParentForm1.SetColumn(dg_ForBrokers, "GST", i);
                ParentForm1.SetColumn(dg_ForBrokers, "NetValue", i);
                if (dg_ForBrokers["BS", i].Value.ToString() == "B")
                    dg_ForBrokers["BS", i].Style.ForeColor = Color.Green;
                else
                    dg_ForBrokers["BS", i].Style.ForeColor = Color.Red;
                if (dg_ForBrokers["LS", i].Value.ToString() == "Long")
                    dg_ForBrokers["LS", i].Style.ForeColor = Color.Green;
                else
                    dg_ForBrokers["LS", i].Style.ForeColor = Color.Red;
                if (dg_ForBrokers["RecordType", i].Value.ToString().ToLower() == "cancel")
                    dg_ForBrokers["RecordType", i].Style.ForeColor = Color.Red;
                else
                    dg_ForBrokers["RecordType", i].Style.ForeColor = Color.Green;

                if (Math.Round(SystemLibrary.ToDecimal(dg_ForBrokers.Rows[0].Cells["Quantity"].Value), 0) != SystemLibrary.ToDecimal(dg_ForBrokers.Rows[0].Cells["Quantity"].Value))
                    dg_ForBrokers.Rows[0].Cells["Quantity"].Style.Format = "N2";

            }

        } //FormatForBrokers()

        public void LoadForCustodians()
        {
            // Local Variables
            String mySql;

            // Get the Data
            mySql = "Exec sp_ProcessTrade_ForCustodian ";
            dt_ForCustodians = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_ForCustodians.DataSource = dt_ForCustodians;
            dg_ForCustodians.Refresh();

            if (dg_ForCustodians.Columns.Count==0)
                return;

            // Hide Reference columns
            dg_ForCustodians.Columns["Account Number"].Visible = false;
            dg_ForCustodians.Columns["TransactionType"].Visible = false;
            dg_ForCustodians.Columns["Client Transaction ID"].Visible = false;
            dg_ForCustodians.Columns["Client Product Id Type"].Visible = false;
            dg_ForCustodians.Columns["Client Product Id"].Visible = false;
            dg_ForCustodians.Columns["Client Executing Broker"].Visible = false;
            dg_ForCustodians.Columns["CommissionRate"].Visible = false;
            dg_ForCustodians.Columns["Pos_Mult_Factor"].Visible = false;
            dg_ForCustodians.Columns["Currency"].Visible = false;
            dg_ForCustodians.Columns["IsInternal"].Visible = false;
            dg_ForCustodians.Columns["EmailSalutation"].Visible = false;
            dg_ForCustodians.Columns["Email"].Visible = false;

            dg_ForCustodians.Columns["BBG_Ticker"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg_ForCustodians.Columns["FundName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //dg_ForCustodians.Columns["PortfolioName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg_ForCustodians.Columns["BrokerName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg_ForCustodians.Columns["CustodianID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dg_ForCustodians.Columns["GrossValue"].HeaderText = "Gross Value";
            //dg_ForCustodians.Columns["CommissionRate"].HeaderText = "Comm Rate";
            dg_ForCustodians.Columns["NetValue"].HeaderText = "Net Value";
            dg_ForCustodians.Columns["TradeDate"].HeaderText = "Trade Date";
            dg_ForCustodians.Columns["SettlementDate"].HeaderText = "Settle Date";
            //dg_ForCustodians.Columns["Exchange"].HeaderText = "Exch";
            dg_ForCustodians.Columns["FundName"].HeaderText = "Fund";
            //dg_ForCustodians.Columns["PortfolioName"].HeaderText = "Portfolio";
            dg_ForCustodians.Columns["BrokerName"].HeaderText = "Broker";
            //dg_ForCustodians.Columns["CustodianID"].HeaderText = "Custodian";

            if (dg_ForCustodians.Columns.Contains("Send"))
                dg_ForCustodians.Columns.Remove("Send");
            DataGridViewCheckBoxColumn Send = new DataGridViewCheckBoxColumn();
            Send.HeaderText = "Send";
            Send.FalseValue = "N";
            Send.TrueValue = "Y";
            Send.Name = "Send";
            Send.DataPropertyName = "Send";
            dg_ForCustodians.Columns.Insert(0, Send);

            ParentForm1.SetFormatColumn(dg_ForCustodians, "Quantity", Color.Empty, Color.FromArgb(192, 255, 192), "N0", "0");
            //ParentForm1.SetFormatColumn(dg_ForCustodians, "Price", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dg_ForCustodians, "GrossValue", Color.Empty, Color.FromArgb(192, 255, 192), "N2", "0");
            //ParentForm1.SetFormatColumn(dg_ForCustodians, "CommissionRate", Color.Empty, Color.Empty, "0.00%", "0");
            ParentForm1.SetFormatColumn(dg_ForCustodians, "Commission", Color.Empty, Color.FromArgb(192, 255, 192), "N2", "0");
            ParentForm1.SetFormatColumn(dg_ForCustodians, "Stamp", Color.Empty, Color.FromArgb(192, 255, 192), "N2", "0");
            ParentForm1.SetFormatColumn(dg_ForCustodians, "GST", Color.Empty, Color.FromArgb(192, 255, 192), "N2", "0");
            ParentForm1.SetFormatColumn(dg_ForCustodians, "NetValue", Color.Empty, Color.LightYellow, "N2", "0");
            dg_ForCustodians.Columns["TradeDate"].DefaultCellStyle.Format = "dd-MMM-yy"; // M = dd-Mon, D = Short Date/Time, F = Long Date/Time
            dg_ForCustodians.Columns["TradeDate"].DefaultCellStyle.ForeColor = Color.Blue;
            dg_ForCustodians.Columns["SettlementDate"].DefaultCellStyle.Format = "dd-MMM-yy"; // M = dd-Mon, D = Short Date/Time, F = Long Date/Time
            dg_ForCustodians.Columns["SettlementDate"].DefaultCellStyle.BackColor = Color.FromArgb(192, 255, 192);
            dg_ForCustodians.Columns["Price"].DefaultCellStyle.BackColor = Color.FromArgb(192, 255, 192);

            // Loop on all columns and set the Autosize mode & Header Text
            for (int i = 0; i < dg_ForCustodians.Columns.Count; i++)
            {
                //dg_Orders.Columns[i].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
                dg_ForCustodians.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dg_ForCustodians.Columns[i].HeaderText = dg_ForCustodians.Columns[i].HeaderText.Replace('_', ' ');
                dg_ForCustodians.Columns[i].ReadOnly = true;
                // Turn off Sort as Code relies on the sort order.
                dg_ForCustodians.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            // Now allow some columns to be altered
            dg_ForCustodians.Columns["Send"].ReadOnly = false;
            dg_ForCustodians.Columns["Quantity"].ReadOnly = false;
            dg_ForCustodians.Columns["Price"].ReadOnly = false;
            dg_ForCustodians.Columns["GrossValue"].ReadOnly = false;
            dg_ForCustodians.Columns["Commission"].ReadOnly = false;
            dg_ForCustodians.Columns["Stamp"].ReadOnly = false;
            dg_ForCustodians.Columns["GST"].ReadOnly = false;
            dg_ForCustodians.Columns["NetValue"].ReadOnly = false;
            dg_ForCustodians.Columns["SettlementDate"].ReadOnly = false;


            dg_ForCustodians.Columns["LS"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_ForCustodians.Columns["BS"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_ForCustodians.Columns["LS"].Width = 40;
            dg_ForCustodians.Columns["BS"].Width = 30;
            //dg_ForCustodians.Columns["Exchange"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //dg_ForCustodians.Columns["Exchange"].Width = 35;
            dg_ForCustodians.Columns["TradeDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_ForCustodians.Columns["TradeDate"].Width = 60;
            dg_ForCustodians.Columns["SettlementDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_ForCustodians.Columns["SettlementDate"].Width = 60;

            // Now set the Send if the ConfirmationNo is known
            foreach (DataGridViewRow dgr in dg_ForCustodians.Rows)
            {
                if (dgr.Cells["ConfirmationNo"].Value.ToString().Length > 0)
                    dgr.Cells["Send"].Value = "Y";
                if (Math.Round(SystemLibrary.ToDecimal(dgr.Cells["Quantity"].Value), 0) != SystemLibrary.ToDecimal(dgr.Cells["Quantity"].Value))
                    dgr.Cells["Quantity"].Style.Format = "N2";
            }

            tabControl1.TabPages["tp_TradesForCustodian"].Text = "Step 3 - Broker Confirm / Trades For Custodian (" + dg_ForCustodians.Rows.Count.ToString() + ")";

            FormatForCustodians();

            // Start the timer to read in the Contract notes if available.
            if (dg_ForCustodians.Rows.Count > 0 && timer_CheckContractNotes.Enabled==false)
            {
                timer_CheckContractNotes.Enabled = true;
                timer_CheckContractNotes.Start();
            }
        } //LoadForCustodians()

        public void FormatForCustodians()
        {
            for (Int32 i = 0; i < dg_ForCustodians.Rows.Count; i++)
            {
                if (dg_ForCustodians["ConfirmationNo", i].Value.ToString().Length > 0)
                    dg_ForCustodians["Send", i].Value = "Y";
                else
                    dg_ForCustodians["Send", i].Value = "N";

                ParentForm1.SetColumn(dg_ForCustodians, "Quantity", i);
                ParentForm1.SetColumn(dg_ForCustodians, "GrossValue", i);
                ParentForm1.SetColumn(dg_ForCustodians, "Commission", i);
                ParentForm1.SetColumn(dg_ForCustodians, "Stamp", i);
                ParentForm1.SetColumn(dg_ForCustodians, "GST", i);
                ParentForm1.SetColumn(dg_ForCustodians, "NetValue", i);
                if (dg_ForCustodians["BS", i].Value.ToString() == "B")
                    dg_ForCustodians["BS", i].Style.ForeColor = Color.Green;
                else
                    dg_ForCustodians["BS", i].Style.ForeColor = Color.Red;
                if (dg_ForCustodians["LS", i].Value.ToString() == "Long")
                    dg_ForCustodians["LS", i].Style.ForeColor = Color.Green;
                else
                    dg_ForCustodians["LS", i].Style.ForeColor = Color.Red;
                if (dg_ForCustodians["RecordType", i].Value.ToString().ToLower() == "cancel")
                    dg_ForCustodians["RecordType", i].Style.ForeColor = Color.Red;
                else
                    dg_ForCustodians["RecordType", i].Style.ForeColor = Color.Green;
            }

        } //FormatForCustodians()


        private void ProcessTrades_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Let the Parent know this has close
            if (ParentForm1 != null)
                ParentForm1.LoadActionTab(true);

        } //ProcessTrades_FormClosed() //bt_SendToBrokers_Click()

        private void bt_CreateTrades_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;
            String RunSODPositionsRebuild = "N";
            int myLastSendRow = -1;
            DataTable dt_Check;

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            //Get the state from the database again to make sure hasn't already been processed.
            mySql = "Exec sp_ProcessTrade_Load '" + SystemLibrary.Bool_To_YN(cb_AggregateTrades.Checked) + "' ";
            dt_Check = SystemLibrary.SQLSelectToDataTable(mySql);

            // Establish the last Row with Send = 'Y'
            for (int i = 0; i < dg_Trades.Rows.Count; i++)
                if (dg_Trades["Send", i].Value.ToString() == "Y")
                    myLastSendRow = i;


            for (int i = 0; i < dg_Trades.Rows.Count; i++)
            {
                if (dg_Trades["Send", i].Value.ToString() == "Y")
                {
                    // Make sure Settlement >= Trade
                    if (Convert.ToDateTime(dg_Trades["TradeDate", i].Value) > Convert.ToDateTime(dg_Trades["SettlementDate", i].Value))
                    {
                        MessageBox.Show("Found a Settlement Date < Trade Date. Please adjust and try again.", bt_CreateTrades.Text);
                        return;
                    }
                    // Make sure the columns add up
                    if (SystemLibrary.ToDecimal(dg_Trades["NetValue", i].Value) != SystemLibrary.ToDecimal(dg_Trades["GrossValue", i].Value)
                                                                                 + SystemLibrary.ToDecimal(dg_Trades["Commission", i].Value)
                                                                                 + SystemLibrary.ToDecimal(dg_Trades["Stamp", i].Value)
                                                                                 + SystemLibrary.ToDecimal(dg_Trades["GST", i].Value)
                        )
                    {
                        MessageBox.Show("Found NetValue does not equal sum of parts. Please adjust and try again.", bt_CreateTrades.Text);
                        return;
                    }
                    // Make sure it hasn't already been processed
                    DataRow[] dr = dt_Check.Select("OrderRefID='" + SystemLibrary.ToString(dg_Trades["OrderRefID", i].Value) + "'");
                    if (dr.Length < 1)
                    {
                        MessageBox.Show("Found Some of the Orders had already been processed.\r\n\r\n" +
                                        "Will run a Refresh on your behalf so that only unprocessed Orders remain on screen.", bt_CreateTrades.Text);
                        LoadAll();
                        return;
                    }
                }
            }

            for (int i = 0; i < dg_Trades.Rows.Count; i++)
            {
                if (dg_Trades["Send", i].Value.ToString() == "Y")
                {
                    // Flags the last row to process. Added for speed.
                    if (i == myLastSendRow)
                        RunSODPositionsRebuild = "Y";
                    mySql = "Exec sp_ProcessTrade " +
                             "'" + dg_Trades["OrderRefID", i].Value.ToString() + "', " +
                             dg_Trades["BrokerID", i].Value.ToString() + ", " +
                             SystemLibrary.ToDecimal(dg_Trades["GrossValue", i].Value).ToString() + ", " +
                             SystemLibrary.ToDecimal(dg_Trades["CommissionRate", i].Value).ToString() + ", " +
                             SystemLibrary.ToDecimal(dg_Trades["Commission", i].Value).ToString() + ", " +
                             SystemLibrary.ToDecimal(dg_Trades["Stamp", i].Value).ToString() + ", " +
                             SystemLibrary.ToDecimal(dg_Trades["GST", i].Value).ToString() + ", " +
                             SystemLibrary.ToDecimal(dg_Trades["NetValue", i].Value).ToString() + ", " +
                             "'" + Convert.ToDateTime(dg_Trades["SettlementDate", i].Value).ToString("dd-MMM-yyyy") + "', " +
                             dg_Trades["CustodianID", i].Value.ToString() + ", " +
                             dg_Trades["FundID", i].Value.ToString() + ", " +
                             dg_Trades["FillNo", i].Value.ToString() + ", " +
                             "'" + RunSODPositionsRebuild + "' ";
                    SystemLibrary.SQLExecute(mySql);
                }
            }
            // Refresh
            LoadAll();
            cb_SelectAll.Checked = false;
            Cursor.Current = Cursors.Default;
            MessageBox.Show("When Completed creating Trades from Orders, then move to Step 2", bt_CreateTrades.Text);
        } //bt_CreateTrades_Click()

        private void cb_SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedTab.Name)
            {
                case "tp_OrdersToTrades":
                    foreach (DataGridViewRow dgr in dg_Trades.Rows)
                    {
                        dgr.Cells["Send"].Value = SystemLibrary.Bool_To_YN(cb_SelectAll.Checked);
                    }
                    break;
                case "tp_TradesForBrokers":
                    foreach (DataGridViewRow dgr in dg_ForBrokers.Rows)
                    {
                        dgr.Cells["Send"].Value = SystemLibrary.Bool_To_YN(cb_SelectAll.Checked);
                    }
                    break;
                case "tp_TradesForCustodian":
                    foreach (DataGridViewRow dgr in dg_ForCustodians.Rows)
                    {
                        dgr.Cells["Send"].Value = SystemLibrary.Bool_To_YN(cb_SelectAll.Checked);
                    }
                    break;
            }

        } //cb_SelectAll_CheckedChanged()

        private void cb_AggregateTrades_CheckedChanged(object sender, EventArgs e)
        {
            LoadTrades();
        } //cb_AggregateTrades_CheckedChanged()

        private void bt_SendToBrokers_Click(object sender, EventArgs e)
        {
            // Local Variables
            Boolean HasCancellation = false;
            String htmlBody = "";
            String myBSFontColour = "";
            String myRTFontColour = "";
            String mySql;
            String PrevEmail = "";
            MailMessage mail = null;
            String Broker_Path = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('Broker_Path')");
            String ContractNote_FromEmail = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('ContractNote:FromEmail')");
            String ContractNote_CCEmail = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('ContractNote:CCEmail')");
            String ContractNote_BCCEmail = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('ContractNote:BCCEmail')");
            String ContractNote_Title = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('ContractNote:Title')");
            String ContractNote_Signature = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('ContractNote:Signature')");
            String ContractNote_SmtpClient = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('SmtpClient')");
            // Attach XLS file
            StringBuilder sb_Out = new StringBuilder();
            int sb_Out_BaseLength = 0;
            String myFileName = "Allocation_" + SystemLibrary.f_Today().ToString("yyyy.MM.dd") + ".xls";
            String myFilePath;


            myFilePath = Broker_Path + @"\OutBound";
            if (!System.IO.Directory.Exists(myFilePath))
                myFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //Get the state from the database again to make sure hasn't already been processed.
            mySql = "Exec sp_ProcessTrade_ForBroker ";
            DataTable dt_Check = SystemLibrary.SQLSelectToDataTable(mySql);

            for (int i = 0; i < dg_ForBrokers.Rows.Count; i++)
            {
                // Make sure it hasn't already been processed
                DataRow[] dr = dt_Check.Select("TradeID='" + SystemLibrary.ToString(dg_ForBrokers["TradeID", i].Value) + "'");
                if (dr.Length < 1)
                {
                    MessageBox.Show("Found Some of the Trades have already been processed.\r\n\r\n" +
                                    "Will run a Refresh on your behalf so that only unprocessed Trades remain on screen.", bt_SendToBrokers.Text);
                    LoadAll();
                    return;
                }
            }
            
            // TODO (5) When upgrade to .NET 4.0, then can use this
            // using (SmtpClient SmtpServer = new SmtpClient(ContractNote_SmtpClient))
            SmtpClient SmtpServer = new SmtpClient(ContractNote_SmtpClient);
            {
                SmtpServer.Port = 25;
                //SmtpServer.Credentials = new System.Net.NetworkCredential("username", "pwd");
                SmtpServer.EnableSsl = false;


                // hourglass cursor
                Cursor.Current = Cursors.WaitCursor;


                // Sort by Email address
                //dg_ForBrokers.CommitEdit(DataGridViewDataErrorContexts.Commit);
                //dg_ForBrokers.Sort(dg_ForBrokers.Columns["Email"], ListSortDirection.Ascending); ;
                

                // Mark the Trades as Done
                for (int i = 0; i < dg_ForBrokers.Rows.Count; i++)
                {
                    String TradeID = dg_ForBrokers["TradeID", i].Value.ToString();
                    String BrokerName = dg_ForBrokers["BrokerName", i].Value.ToString();
                    String EmailSalutation = SystemLibrary.ToString(dg_ForBrokers["EmailSalutation", i].Value);
                    String Email = SystemLibrary.ToString(dg_ForBrokers["Email", i].Value);
                    String BBG_Ticker = dg_ForBrokers["BBG_Ticker", i].Value.ToString();
                    String IsInternal = SystemLibrary.ToString(dg_ForBrokers["IsInternal", i].Value);
                    String IsFuture = SystemLibrary.ToString(dg_ForBrokers["IsFuture", i].Value);
                    String RecordType = SystemLibrary.ToString(dg_ForBrokers["RecordType", i].Value);
                    String Ticker = "";
                    String Exch = "";
                    String YellowKey = "";
                    SendToBloomberg.EMSTickerSplit(BBG_Ticker, ref Ticker, ref Exch, ref YellowKey);

                    if (Email.Length == 0)
                        Email = "NOT SUPPLIED";

                    if (SystemLibrary.ToString(dg_ForBrokers["Send", i].Value) == "Y")
                    {
                        if (RecordType.ToLower() == "cancel")
                        {
                            HasCancellation = true;

                            // Mark the Database as Sent.
                            mySql = "Update Trade_Cancel Set Cancel_SentToBroker = dbo.f_GetDate() where TradeID =  " + TradeID;
                            SystemLibrary.SQLExecute(mySql);

                            // No email needed when Internal Broker
                            if (IsInternal == "Y" || IsFuture == "Y")
                            {
                                // Mark other Processing Fields as though this has been pushed through to the Custodian.
                                mySql = "Update Trade_Cancel Set Cancel_BrokerConfirmed = Cancel_SentToBroker, Cancel_SentToCustodian = Cancel_SentToBroker ";
                                //if (IsInternal == "Y")
                                //    mySql = mySql + ", Cancel_CustodianConfirmed = Cancel_SentToBroker ";
                                mySql = mySql + "Where TradeID =  " + TradeID;
                                SystemLibrary.SQLExecute(mySql);
                                if (IsInternal == "Y")
                                    continue;
                            }
                        }
                        else
                        {
                            // Mark the Database as Sent.
                            mySql = "Update Trade Set SentToBroker = dbo.f_GetDate() where TradeID =  " + TradeID;
                            SystemLibrary.SQLExecute(mySql);

                            // No email needed when Internal Broker
                            if (IsInternal == "Y" || IsFuture == "Y")
                            {
                                // Mark other Processing Fields as though this has been pushed through to the Custodian.
                                mySql = "Update Trade Set BrokerConfirmed = SentToBroker, SentToCustodian = SentToBroker ";
                                //if (IsInternal == "Y")
                                //    mySql = mySql + ", CustodianConfirmed = SentToBroker ";
                                mySql = mySql + "Where TradeID =  " + TradeID;
                                SystemLibrary.SQLExecute(mySql);
                                if (IsInternal == "Y")
                                    continue;
                            }
                        }

                        // Group by Email address
                        if (Email != PrevEmail)
                        {
                            PrevEmail = Email;
                            if (mail != null)
                            {
                                // On change of group, send the Mail
                                htmlBody = htmlBody + SystemLibrary.HTMLTableEnd();
                                if (HasCancellation)
                                {
                                    String myNoteCancellation = "<Font Color=Darkred><br /><br />Please Note the Trade Table above includes Cancellations.<br /><br /></Font>";
                                    htmlBody = htmlBody + SystemLibrary.HTMLLine(myNoteCancellation);
                                    HasCancellation = false;
                                }
                                htmlBody = htmlBody +
                                           SystemLibrary.HTMLLine(ContractNote_Signature) +
                                           SystemLibrary.HTMLEnd();
                                mail.Body = htmlBody;

                                if (sb_Out.Length > sb_Out_BaseLength)
                                {
                                    // Attach File
                                    // Write the Data to file
                                    File.Delete(myFilePath + @"\" + myFileName); // Should never need this.
                                    File.WriteAllText(myFilePath + @"\" + myFileName, sb_Out.ToString());

                                    mail.Attachments.Add(new Attachment(myFilePath + @"\" + myFileName));
                                }

                                //mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.OnSuccess;
                                try
                                {
                                    SmtpServer.Send(mail);
                                }
                                catch (Exception em)
                                {
                                    Exception CheckMessage = em;
                                    String myMessage = CheckMessage.Message;

                                    while (CheckMessage.InnerException != null)
                                    {
                                        CheckMessage = CheckMessage.InnerException;
                                        myMessage = myMessage + "\r\n" + CheckMessage.Message;
                                    }
                                    MessageBox.Show(myMessage, "Failed to Send email");
                                }

                                // Clean up
                                mail.Dispose();
                                mail = null;
                                sb_Out = new StringBuilder();
                            }

                            // Create File Header Row
                            sb_Out.AppendLine("SECURITY\tUNITS\tDIRECTION\tALLOCATION ACCOUNT\tAccount\tUNITS\tPRICE\tBROKERAGE");
                            sb_Out_BaseLength = sb_Out.Length;

                            // Create a new mail header record
                            mail = new MailMessage();
                            mail.From = new MailAddress(ContractNote_FromEmail);
                            if (Email == "NOT SUPPLIED")
                                mail.To.Add(ContractNote_FromEmail);
                            else
                            {
                                //String 
                                foreach (String myStr in Email.Split(",;".ToCharArray()))
                                    mail.To.Add(myStr);
                            }
                            if (ContractNote_CCEmail != "")
                            {
                                //String 
                                foreach (String myStr in ContractNote_CCEmail.Split(",;".ToCharArray()))
                                    mail.CC.Add(myStr);
                            }
                            if (ContractNote_BCCEmail != "")
                            {
                                //String 
                                foreach (String myStr in ContractNote_BCCEmail.Split(",;".ToCharArray()))
                                    mail.Bcc.Add(myStr);
                            }
                            mail.Subject = ContractNote_Title + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                            mail.IsBodyHtml = true;
                            htmlBody = SystemLibrary.HTMLStart();
                            if (EmailSalutation.Length > 0)
                            {
                                // Add some lines after the Salutation
                                EmailSalutation = EmailSalutation + "\r\n\r\n";
                            }

                            htmlBody = htmlBody +
                                       SystemLibrary.HTMLLine(EmailSalutation + "\r\nPlease allocate the Trades today as follows:\r\n\r\n") +
                                       SystemLibrary.HTMLTableStart();

                            // Now Build the Row Header
                            // SystemLibrary.HTMLHeaderField("FundName") +
                            htmlBody = htmlBody +
                                       SystemLibrary.HTMLHeaderStart() +
                                       SystemLibrary.HTMLHeaderField("TradeID") +
                                       SystemLibrary.HTMLHeaderField("Record Type") +
                                       SystemLibrary.HTMLHeaderField("BookCode") +
                                       SystemLibrary.HTMLHeaderField("TradeDate") +
                                       SystemLibrary.HTMLHeaderField("Ticker") +
                                       SystemLibrary.HTMLHeaderField("LS") +
                                       SystemLibrary.HTMLHeaderField("BS") +
                                       SystemLibrary.HTMLHeaderField("Quantity") +
                                       SystemLibrary.HTMLHeaderField("Price");
                            if (IsFuture == "N")
                            {
                                htmlBody = htmlBody +
                                       SystemLibrary.HTMLHeaderField("Gross Value") +
                                       SystemLibrary.HTMLHeaderField("Commission") +
                                       SystemLibrary.HTMLHeaderField("Stamp") +
                                       SystemLibrary.HTMLHeaderField("GST") +
                                       SystemLibrary.HTMLHeaderField("Net Value") +
                                       SystemLibrary.HTMLHeaderField("Currency") +
                                       SystemLibrary.HTMLHeaderField("CustodianID") +
                                       SystemLibrary.HTMLHeaderField("Sedol") +
                                       SystemLibrary.HTMLHeaderField("ISIN") +
                                       SystemLibrary.HTMLHeaderField("CUSIP");
                            }
                            htmlBody = htmlBody +
                                       SystemLibrary.HTMLHeaderField("Security Name");
                            if (Email == "NOT SUPPLIED")
                                htmlBody = htmlBody + SystemLibrary.HTMLHeaderField("Broker");
                            htmlBody = htmlBody + SystemLibrary.HTMLHeaderEnd();
                        }

                        // Add Trade to the message
                        String BS = dg_ForBrokers["BS", i].FormattedValue.ToString();
                        String BookCode = dg_ForBrokers["BookCode", i].FormattedValue.ToString();
                        //String RecordType = dg_ForBrokers["RecordType", i].FormattedValue.ToString();
                        String TradeDate = dg_ForBrokers["TradeDate", i].FormattedValue.ToString();
                        String LS = dg_ForBrokers["LS", i].FormattedValue.ToString();
                        String Quantity = dg_ForBrokers["Quantity", i].FormattedValue.ToString();
                        String Price = dg_ForBrokers["Price", i].FormattedValue.ToString();
                        String GrossValue = dg_ForBrokers["GrossValue", i].FormattedValue.ToString();
                        String Commission = dg_ForBrokers["Commission", i].FormattedValue.ToString();
                        String Stamp = dg_ForBrokers["Stamp", i].FormattedValue.ToString();
                        String GST = dg_ForBrokers["GST", i].FormattedValue.ToString();
                        String NetValue = dg_ForBrokers["NetValue", i].FormattedValue.ToString();
                        String Crncy = dg_ForBrokers["Currency", i].FormattedValue.ToString();
                        String CustodianID = dg_ForBrokers["CustodianID", i].FormattedValue.ToString();
                        String FundName = dg_ForBrokers["FundName", i].FormattedValue.ToString();
                        String Sedol = dg_ForBrokers["Sedol", i].FormattedValue.ToString();
                        String ISIN = dg_ForBrokers["ISIN", i].FormattedValue.ToString();
                        String CUSIP = dg_ForBrokers["CUSIP", i].FormattedValue.ToString();
                        String Security_Name = dg_ForBrokers["Security_Name", i].FormattedValue.ToString();
                        String ConfirmationNo = dg_ForBrokers["ConfirmationNo", i].FormattedValue.ToString();

                        String QuantityABS = Math.Abs(SystemLibrary.ToDecimal(dg_ForBrokers["Quantity", i].Value)).ToString();
                        String ExtID = SystemLibrary.ToString(dg_ForBrokers["ExtID", i].Value);
                        String Direction = "";


                        if (BS == "B")
                        {
                            myBSFontColour = "green";
                            Direction = "BUY";
                        }
                        else
                        {
                            myBSFontColour = "red";
                            Direction = "SELL";
                        }
                        switch (RecordType.ToLower())
                        {
                            case "new":
                                myRTFontColour = "green";
                                break;
                            case "cancel":
                                myRTFontColour = "Darkred";
                                break;
                            case "rebook":
                                myRTFontColour = "Darkblue";
                                break;
                        }

                        if (RecordType.ToLower() != "cancel")
                        {
                            sb_Out.AppendLine(Ticker + "\t" +
                                              QuantityABS + "\t" +
                                              Direction + "\t" +
                                              BookCode + "\t" +
                                              ExtID + "\t" +
                                              QuantityABS + "\t" +
                                              Price + "\t" +
                                              Commission
                                              );
                        }

                        //  SystemLibrary.HTMLRowField(FundName,"") + 
                        htmlBody = htmlBody +
                                   SystemLibrary.HTMLRowStart() +
                                   SystemLibrary.HTMLRowField(TradeID, "");
                        if (RecordType.ToLower() == "cancel" && ConfirmationNo.Length>0)
                        {
                            htmlBody = htmlBody +
                                       SystemLibrary.HTMLRowField(RecordType + " [Your Ref: " + ConfirmationNo +"]", myRTFontColour);
                        }
                        else
                        {
                            htmlBody = htmlBody +
                                       SystemLibrary.HTMLRowField(RecordType, myRTFontColour);
                        }
                        if (ExtID.Length > 0)
                        {
                            htmlBody = htmlBody +
                                   SystemLibrary.HTMLRowField(BookCode + @" / " + ExtID, ""); 
                        }
                        else
                        {
                            htmlBody = htmlBody +
                                   SystemLibrary.HTMLRowField(BookCode, "");
                        }
                        htmlBody = htmlBody +
                                   SystemLibrary.HTMLRowField(TradeDate, "") +
                                   SystemLibrary.HTMLRowField(BBG_Ticker, "") +
                                   SystemLibrary.HTMLRowField(LS, "") +
                                   SystemLibrary.HTMLRowField(BS, myBSFontColour) +
                                   SystemLibrary.HTMLRowField(Quantity, "") +
                                   SystemLibrary.HTMLRowField(Price, "");
                        if (IsFuture == "N")
                        {
                            htmlBody = htmlBody +
                                       SystemLibrary.HTMLRowField(GrossValue, "") +
                                       SystemLibrary.HTMLRowField(Commission, "") +
                                       SystemLibrary.HTMLRowField(Stamp, "") +
                                       SystemLibrary.HTMLRowField(GST, "") +
                                       SystemLibrary.HTMLRowField(NetValue, "") +
                                       SystemLibrary.HTMLRowField(Crncy, "") +
                                       SystemLibrary.HTMLRowField(CustodianID, "") +
                                       SystemLibrary.HTMLRowField(Sedol, "") +
                                       SystemLibrary.HTMLRowField(ISIN, "") +
                                       SystemLibrary.HTMLRowField(CUSIP, "");
                        }
                        htmlBody = htmlBody +
                                   SystemLibrary.HTMLRowField(Security_Name, "");
                        if (Email == "NOT SUPPLIED")
                            htmlBody = htmlBody + SystemLibrary.HTMLRowField(BrokerName, "");
                        htmlBody = htmlBody + SystemLibrary.HTMLRowEnd();
                    }
                }

                // Final message to be sent
                if (mail != null)
                {
                    // On change of group, send the Mail
                    htmlBody = htmlBody + SystemLibrary.HTMLTableEnd();
                    if (HasCancellation)
                    {
                        String myNoteCancellation = "<Font Color=Darkred><br /><br />Please Note the Trade Table above includes Cancellations.<br /><br /></Font>";
                        htmlBody = htmlBody + SystemLibrary.HTMLLine(myNoteCancellation);
                        HasCancellation = false;
                    }
                    htmlBody = htmlBody +
                               SystemLibrary.HTMLLine(ContractNote_Signature) +
                               SystemLibrary.HTMLEnd();
                    mail.Body = htmlBody;

                    if (sb_Out.Length > sb_Out_BaseLength)
                    {
                        // Attach File
                        // Write the Data to file
                        if (File.Exists(myFilePath + @"\" + myFileName))
                            File.Delete(myFilePath + @"\" + myFileName); // Should never need this.
                        File.WriteAllText(myFilePath + @"\" + myFileName, sb_Out.ToString());

                        mail.Attachments.Add(new Attachment(myFilePath + @"\" + myFileName));
                    }

                    //mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.OnSuccess;
                    try
                    {
                        SmtpServer.Send(mail);
                    }
                    catch (Exception em)
                    {
                        Exception CheckMessage = em;
                        String myMessage = CheckMessage.Message;

                        while (CheckMessage.InnerException != null)
                        {
                            CheckMessage = CheckMessage.InnerException;
                            myMessage = myMessage + "\r\n" + CheckMessage.Message;
                        }
                        MessageBox.Show(myMessage, "Failed to Send email");
                    }

                    // Clean up
                    mail.Dispose();
                    mail = null;
                }
            }

            // Refresh
            LoadAll();
            cb_SelectAll.Checked = false;
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Wait for Broker Confirms on Trades, then move to Step 3", bt_CreateTrades.Text);


        } //bt_SendToBrokers_Click()


        /*
         * Jan 2014
         * I need to make sure a user can change field data, which I think I have coded out with the Refresh()
         */
        private void bt_SendToCustodian_Click(object sender, EventArgs e)
        {
            // Local Variables
            DataTable dt_Out;
            StringBuilder sb_Out = new StringBuilder();
            String myMessage = "";
            String mySql;
            String myFileName;
            String myFilePath;
            String myTimeStamp = SystemLibrary.f_Now().ToString("yyyyMMddHHmmss");
            String mySentToCustodian = SystemLibrary.f_Now().ToString("dd-MMM-yyyy HH:mm:ss");
            String CustodianName = "";
            int NumberofRecords = 0;
            int MLClientBatchID;
            Boolean FoundChange = false;

            /*
             * Temporary: 22-Feb-2014
             */
            int origDebugLevel = SystemLibrary.GetDebugLevel();
            SystemLibrary.DebugLine(4);

            // Saves any Edit changes down to dt_ForCustodians
            dg_ForCustodians.Refresh();

            if (dg_ForCustodians.Rows.Count == 0)
            {
                if (sender != null)
                    MessageBox.Show("No Trades to Send", bt_SendToCustodian.Text);
                return;
            }


            /*
             * See if someone else hasnt already run these
             */
            // - Store away ticked items that do Not have a ConfirmationNo 
            DataTable dt_ItemsChecked = new DataTable();
            dt_ItemsChecked.Columns.Add("TradeID", typeof(int));
            for (int i=0;i<dg_ForCustodians.Rows.Count;i++)
            {
                if (SystemLibrary.ToString(dg_ForCustodians.Rows[i].Cells["Send"].Value) == "Y" &&
                    SystemLibrary.ToString(dg_ForCustodians.Rows[i].Cells["ConfirmationNo"].Value).Length == 0
                    )
                {
                    DataRow dr = dt_ItemsChecked.NewRow();
                    dr["TradeID"] = SystemLibrary.ToInt32(dg_ForCustodians.Rows[i].Cells["TradeID"].Value);
                    dt_ItemsChecked.Rows.Add(dr);
                }

            }

            // - Reload the data to see if already processed 
            LoadForCustodians();
            if (dg_ForCustodians.Rows.Count == 0)
            {
                if (sender != null)
                    MessageBox.Show("File was already sent, so did not resend.",bt_SendToCustodian.Text);
                return;
            }

            // - Reapply the Ticked items 
            for (int i = 0; i < dg_ForCustodians.Rows.Count; i++)
            {
                DataRow[] FoundTradeIDRows = dt_ItemsChecked.Select("TradeID="+SystemLibrary.ToInt32(dg_ForCustodians.Rows[i].Cells["TradeID"].Value));
                if (FoundTradeIDRows.Length>0)
                    dg_ForCustodians.Rows[i].Cells["Send"].Value = "Y";
            }

            dg_ForCustodians.Refresh();

            /*
             * Allow for Multiple Custodians
             * 2011 - Merrill Lynch
             * 2014 - SCOTIA
             */

            /*
             * Merrill Lynch Prime Broker
             */
            DataRow[] dr_Who = dt_ForCustodians.Select("CustodianName='Merrill Lynch'");
            if (dr_Who.Length > 0)
            {
                // Create the FileName
                String MLPrime_Path = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('MLPrime_Path')");
                myFileName = "XAAM_UPLOAD_" + SystemLibrary.f_Today().ToString("yyyy.MM.dd");
                mySql = "Select IsNull(Max(MLClientBatchID),0) + 1 From ML_Out where MLFileName like '" + myFileName + "%' ";
                MLClientBatchID = SystemLibrary.SQLSelectInt32(mySql);
                if (MLClientBatchID == 1)
                    myFileName = myFileName + ".csv";
                else
                    myFileName = myFileName + "_" + MLClientBatchID.ToString().Trim() + ".csv";

                // Ask the User for a file location
                //FolderBrowserDialog myFolder = new FolderBrowserDialog();
                //myFolder.Description = "Choose the Directory to Place the File:\r\n\r\n\t" + myFileName;
                //myFolder.RootFolder = Environment.SpecialFolder.MyDocuments;
                //myFolder.ShowDialog();
                //myFilePath = myFolder.SelectedPath + @"\" + myFileName;

                CustodianName = "Merrill Lynch";
                myFilePath = MLPrime_Path + @"\OutBound";
                if (!System.IO.Directory.Exists(myFilePath))
                    myFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // hourglass cursor
                Cursor.Current = Cursors.WaitCursor;

                // Set up the Database Upload data
                dt_Out = SystemLibrary.SQLSelectToDataTable("Select * from ML_Out where 1=2");


                // NumberofRecords is where "Send" is ticked
                for (int i = 0; i < dg_ForCustodians.Rows.Count; i++)
                    if (SystemLibrary.ToString(dg_ForCustodians["Send", i].Value) == "Y" && CustodianName == "Merrill Lynch")
                        NumberofRecords++;

                // Header Rows
                sb_Out.AppendLine("H1,Mandatory,,,,,,,,,,,Conditionally Mandatory,,,,,,,,,,,,,,Optional,,,,,,,,,,,,,,,,,");
                sb_Out.AppendLine("H2,TimeStamp:," + myTimeStamp + ",Client Batch Id:," + MLClientBatchID + ",Number of Records," + NumberofRecords.ToString() + ",File Type,Trade, , ,,,,,,,, ,,,,,,,,,,,,,,,,,,,,,,,,,");
                sb_Out.AppendLine("H3,Account Number,Record Type,Transaction Type ,Client Transaction ID,Trade Date,Client Product Id Type,Client Product Id,Price,Quantity,Settlement Date,Settlement Currency,Net Amount,Client Executing Broker,SEC Fee,Accrued Interest,Client Asset Type,Trading Unit,Trading Sub-Unit,Client Underlying Product Id,Client Underlying Product Id Type,Country of Trading,Expiration Date,Fee Type 1,Fee Amount 1,Fee Type 2,Fee Amount 2,Fee Type 3,Fee Amount 3,Fee Type 4,Fee Amount 4,Fee Type 5,Fee Amount 5,Option Type,Spot Date,Strike Price,Commission Amount ,Commission Rate,Commission Type,Prime Broker Type,Client Original Transaction Id,Client Block Id,Cost Basis FX Rate,Issue Currency");

                // Check for any user changes and update the database
                for (int i = 0; i < dt_ForCustodians.Rows.Count; i++)
                {
                    String RecordType = SystemLibrary.ToString(dg_ForCustodians["RecordType", i].Value);
                    String ClientTransactionID = SystemLibrary.ToString(dg_ForCustodians["Client Transaction ID", i].Value);
                    String Quantity = SystemLibrary.ToString(dg_ForCustodians["Quantity", i].Value);
                    String Price = SystemLibrary.ToString(dg_ForCustodians["Price", i].Value);
                    Decimal d_GrossValue = SystemLibrary.ToDecimal(dg_ForCustodians["GrossValue", i].Value);
                    Decimal d_Commission = SystemLibrary.ToDecimal(dg_ForCustodians["Commission", i].Value);
                    String GrossValue = SystemLibrary.ToString(dg_ForCustodians["GrossValue", i].Value);
                    String Commission = SystemLibrary.ToString(dg_ForCustodians["Commission", i].Value);
                    String Stamp = SystemLibrary.ToDecimal(dg_ForCustodians["Stamp", i].Value).ToString();
                    String GST = SystemLibrary.ToString(dg_ForCustodians["GST", i].Value);
                    String NetValue = SystemLibrary.ToString(dg_ForCustodians["NetValue", i].Value);
                    Decimal CommissionRate = Math.Abs(Math.Round(d_Commission / d_GrossValue, 6));
                    DateTime Settlement_Date = Convert.ToDateTime(dg_ForCustodians["SettlementDate", i].Value);
                    String ConfirmationNo = SystemLibrary.ToString(dg_ForCustodians["ConfirmationNo", i].Value);

                    // Irrespective of Send, save changed records back to the database.
                    if (dt_ForCustodians.Rows[i].RowState == DataRowState.Modified)
                    {
                        FoundChange = true;
                        dt_ForCustodians.Rows[i]["CommissionRate"] = CommissionRate;

                        // Not allowed to change a Cancel, so ignore
                        if (RecordType.ToLower() != "cancel")
                        {
                            mySql = "Update Trade " +
                                    "Set    BrokerConfirmed = '" + mySentToCustodian + "', " +
                                    "       Quantity = " + Quantity.ToString() + ", " +
                                    "       Price = " + Price.ToString() + ", " +
                                    "       GrossValue = " + GrossValue.ToString() + ", " +
                                    "       Commission = " + Commission.ToString() + ", " +
                                    "       Stamp = " + Stamp.ToString() + ", " +
                                    "       GST = " + GST.ToString() + ", " +
                                    "       NetValue = " + NetValue.ToString() + ", " +
                                    "       CommissionRate = " + CommissionRate.ToString() + ", " +
                                    "       SettlementDate = '" + Settlement_Date.ToString("dd-MMM-yyyy") + "', " +
                                    "       ConfirmationNo = '" + ConfirmationNo + "' " +
                                    "Where TradeID =  " + ClientTransactionID;

                            SystemLibrary.SQLExecute(mySql);

                            // Set the Transaction record to reflect the Trade record
                            mySql = "exec sp_ApplyTradeChangeToTransaction " + ClientTransactionID;
                            SystemLibrary.SQLExecute(mySql);
                        }
                    }
                }

                if (NumberofRecords < 1)
                {
                    Cursor.Current = Cursors.Default;
                    if (FoundChange)
                        MessageBox.Show("Saved Changes, now Please select Rows to Send", bt_SendToCustodian.Text);
                    else
                        MessageBox.Show("Please select Rows to Send", bt_SendToCustodian.Text);
                    return;
                }


                // NumberofRecords is where "Send" is ticked
                for (int i = 0; i < dg_ForCustodians.Rows.Count; i++)
                {
                    if (SystemLibrary.ToString(dg_ForCustodians["Send", i].Value) == "Y" && CustodianName == "Merrill Lynch")
                    {
                        // Load Variables
                        String AccountNumber = SystemLibrary.ToString(dg_ForCustodians["Account Number", i].Value);
                        String RecordType = SystemLibrary.ToString(dg_ForCustodians["RecordType", i].Value).Substring(0, 1);
                        String TransactionType = SystemLibrary.ToString(dg_ForCustodians["TransactionType", i].Value);
                        String TradeDate = Convert.ToDateTime(dg_ForCustodians["TradeDate", i].Value).ToString("yyyyMMdd");
                        DateTime Trade_Date = Convert.ToDateTime(dg_ForCustodians["TradeDate", i].Value);
                        String ClientProductIdType = SystemLibrary.ToString(dg_ForCustodians["Client Product Id Type", i].Value);
                        String ClientProductId = SystemLibrary.ToString(dg_ForCustodians["Client Product Id", i].Value);
                        String SettlementDate = Convert.ToDateTime(dg_ForCustodians["SettlementDate", i].Value).ToString("yyyyMMdd");
                        String Currency = SystemLibrary.ToString(dg_ForCustodians["Currency", i].Value);
                        String ClientExecutingBroker = SystemLibrary.ToString(dg_ForCustodians["Client Executing Broker", i].Value);
                        String ClientTransactionID = SystemLibrary.ToString(dg_ForCustodians["Client Transaction ID", i].Value);
                        String Quantity = SystemLibrary.ToString(dg_ForCustodians["Quantity", i].Value);
                        String Price = SystemLibrary.ToString(dg_ForCustodians["Price", i].Value);
                        String GrossValue = SystemLibrary.ToString(dg_ForCustodians["GrossValue", i].Value);
                        String Commission = SystemLibrary.ToString(dg_ForCustodians["Commission", i].Value);
                        String Stamp = SystemLibrary.ToString(dg_ForCustodians["Stamp", i].Value);
                        String GST = SystemLibrary.ToString(dg_ForCustodians["GST", i].Value);
                        String NetValue = SystemLibrary.ToString(dg_ForCustodians["NetValue", i].Value);
                        String CommissionRate = SystemLibrary.ToString(dg_ForCustodians["CommissionRate", i].Value);
                        DateTime Settlement_Date = Convert.ToDateTime(dg_ForCustodians["SettlementDate", i].Value);
                        String ClientOriginalTransactionId = "";

                        if (RecordType.ToLower() == "c")
                            ClientOriginalTransactionId = ClientTransactionID;

                        // Build up the file details.
                        sb_Out.AppendLine("," +
                                          AccountNumber + "," +
                                          RecordType + "," +
                                          TransactionType + "," +
                                          ClientTransactionID + "," +
                                          TradeDate + "," +
                                          ClientProductIdType + "," +
                                          ClientProductId + "," +
                                          Price + "," +
                                          Quantity + "," +
                                          SettlementDate + "," +
                                          Currency + "," +
                                          NetValue + "," +
                                          ClientExecutingBroker + "," +
                                          ",,,,,,,,,,,,,,,,,,,,,," +
                                          Commission + "," +
                                          CommissionRate + "," +
                                          ",," +
                                          ClientOriginalTransactionId + "," +
                                          ",,"
                                          );

                        // Mark the Database as Sent.
                        if (RecordType.ToLower() == "c") // Cancel
                        {
                            mySql = "Update Trade_Cancel " +
                                    "Set    Cancel_SentToCustodian = '" + mySentToCustodian + "', " +
                                    "       Cancel_BrokerConfirmed = '" + mySentToCustodian + "' " +
                                    "Where TradeID =  " + ClientTransactionID;
                        }
                        else
                        {
                            mySql = "Update Trade " +
                                    "Set    SentToCustodian = '" + mySentToCustodian + "', " +
                                    "       BrokerConfirmed = '" + mySentToCustodian + "' " +
                                    "Where TradeID =  " + ClientTransactionID;
                        }
                        SystemLibrary.SQLExecute(mySql);

                        // Write to the Database dt_Out
                        DataRow dr = dt_Out.NewRow();
                        dr["MLFileName"] = myFileName;
                        dr["MLTimeStamp"] = myTimeStamp;
                        dr["MLClientBatchID"] = MLClientBatchID;
                        dr["MLNumberofRecords"] = NumberofRecords;
                        dr["Account_Number"] = AccountNumber;
                        dr["Record_Type"] = RecordType;
                        dr["Transaction_Type"] = TransactionType;
                        dr["Client_Transaction_ID"] = ClientTransactionID;
                        dr["Trade_Date"] = Trade_Date;
                        dr["Client_Product_Id_Type"] = ClientProductIdType;
                        dr["Client_Product_Id"] = ClientProductId;
                        dr["Price"] = Price;
                        dr["Quantity"] = Quantity;
                        dr["Settlement_Date"] = Settlement_Date;
                        dr["Settlement_Currency"] = Currency;
                        dr["Net_Amount"] = NetValue;
                        dr["Client_Executing_Broker"] = ClientExecutingBroker;
                        dr["Commission_Amount"] = Commission;
                        dr["Commission_Rate"] = CommissionRate;
                        //dr["Commission_Type"] = Commission_Type; //  'S' Rate per Share, 'A' $$ - as per futures
                        dt_Out.Rows.Add(dr);
                    }
                }

                // Save to Database
                int myRows = SystemLibrary.SQLBulkUpdate(dt_Out, "", "ML_Out");

                // Write the Data to file
                File.Delete(myFilePath + @"\" + myFileName); // Should never need this.
                File.WriteAllText(myFilePath + @"\" + myFileName, sb_Out.ToString());

                // Drop the file into the MLPrime Web site.
                if (FTPToPrime(CustodianName, myFilePath, myFileName))
                {
                    mySql = "Update ML_Out " +
                            "Set    FileSent = dbo.f_GetDate(), " +
                            "       FileMethod = 'FTP' " +
                            "Where  MLFileName = '" + myFileName + "' ";
                    SystemLibrary.SQLExecute(mySql);

                    // Alter the LastUpdate for this user, so it searches for the confirm in 15 minutes
                    SystemLibrary.FTPMLPrimeVars.LastUpdate = SystemLibrary.f_Now().AddSeconds(-SystemLibrary.FTPMLPrimeVars.Interval_seconds + 900);

                    myMessage = "ML Prime file sent.\r\n\r\n";
                }
                else
                {
                    // FTP Failed, so trying email
                    if (EmailToPrime(CustodianName, myFilePath + @"\" + myFileName))
                    {

                        mySql = "Update ML_Out " +
                                "Set    FileSent = dbo.f_GetDate(), " +
                                "       FileMethod = 'Email' " +
                                "Where  MLFileName = '" + myFileName + "' ";
                        SystemLibrary.SQLExecute(mySql);
                        myMessage = "ML Prime File Failed to go directly to server, so have emailed instead.\r\n\r\n";
                    }
                    else
                    {
                        mySql = "Update ML_Out " +
                                "Set    FileSent = dbo.f_GetDate(), " +
                                "       FileMethod = 'Manual' " +
                                "Where  MLFileName = '" + myFileName + "' ";
                        SystemLibrary.SQLExecute(mySql);
                        myMessage = "ML Prime File Failed to go directly to server. Email also failed. Extract file from saved location and process appropriately.\r\n\r\n";
                    }
                }

                if (sender != null)
                    MessageBox.Show(myMessage + myFilePath + @"\" + myFileName, bt_SendToCustodian.Text);

            } // .Select("CustodianName='Merrill Lynch'")

            /*
             * SCOTIA Prime Broker
             */
            dr_Who = dt_ForCustodians.Select("CustodianName='SCOTIA'");
            if (dr_Who.Length > 0)
            {
                // Create the FileName
                // "<Fund>_TRADES_20140128_164021.csv"
                String Scotia_Path = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('SCOTIAPrime_Path')");
                String Scotia_Prefix = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('SCOTIA_TRADES_Prefix')");
                String SCOTIA_TradeId_Suffix = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('SCOTIA_TradeId_Suffix')");
                myFileName = Scotia_Prefix + "_TRADES_" + SystemLibrary.f_Now().ToString("yyyyMMdd_HHmmss") + ".csv";

                CustodianName = "SCOTIA";
                myFilePath = Scotia_Path + @"\OutBound";
                if (!System.IO.Directory.Exists(myFilePath))
                    myFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // hourglass cursor
                Cursor.Current = Cursors.WaitCursor;

                // Set up the Database Upload data
                dt_Out = SystemLibrary.SQLSelectToDataTable("Select * from SCOTIA_Out where 1=2");

                // NumberofRecords is where "Send" is ticked
                for (int i = 0; i < dg_ForCustodians.Rows.Count; i++)
                    if (SystemLibrary.ToString(dg_ForCustodians["Send", i].Value) == "Y" && CustodianName == "SCOTIA")
                        NumberofRecords++;

                // Header Rows
                sb_Out.AppendLine(@"!SET_USER,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                sb_Out.AppendLine(@"#! DEF INS_TRADE = OPF = TDP_LOADER.insert_trade_wrapper,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                sb_Out.AppendLine(@"#! DEF INIT = OPP = TDP_LOADER.initialise_err_qty,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                sb_Out.AppendLine(@"#! DEF END  = OPP = FIL.set_trade_upload_error_qty,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                sb_Out.AppendLine(@",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                sb_Out.AppendLine(@",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                sb_Out.AppendLine(@"#initialization,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                sb_Out.AppendLine(@"!INIT,0,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                sb_Out.AppendLine(@",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                sb_Out.AppendLine(@"# prefix,trade type,trade reference,fund,book,strategy,cpty,buy/sell,quantity,sec ident type,sec ident value,sec name,price,price divisor,traded net ind,trade ccy,trade dt,settle dt,commission,exchange fee,other fees,gross consid,net consid,sett ccy,trad/sett ccy xrt,trad/sett ccy xrt m/d ind,trad/inst ccy xrt,trad/inst ccy xrt m/d ind,prime brkr,pb acct,reason code,inst class,trd notes,pl/book ccy xrt,sett cond,logon,amendment,cont id,old ref,,,,,,,,,");
                sb_Out.AppendLine(@"# !INS_TRADE,SEC,,<8 digit acct>,<acct>_BOOK,NONE,,""B, S, BC, SS"",,CUSIP or RIC,<cusip> <RIC>,,,,,,,,,,,,,,1 or xrate,M (or D),1 or xrate,M (or D),SCOTIA,<8 digit acct>,NOR,,,1 or xrate,*,,,,,,,,,,,,, ");

                // Check for any user changes and update the database
                for (int i = 0; i < dt_ForCustodians.Rows.Count; i++)
                {
                    String RecordType = SystemLibrary.ToString(dg_ForCustodians["RecordType", i].Value);
                    String ClientTransactionID = SystemLibrary.ToString(dg_ForCustodians["Client Transaction ID", i].Value);
                    String Quantity = SystemLibrary.ToString(dg_ForCustodians["Quantity", i].Value);
                    String Price = SystemLibrary.ToString(dg_ForCustodians["Price", i].Value);
                    Decimal d_GrossValue = SystemLibrary.ToDecimal(dg_ForCustodians["GrossValue", i].Value);
                    Decimal d_Commission = SystemLibrary.ToDecimal(dg_ForCustodians["Commission", i].Value);
                    String GrossValue = SystemLibrary.ToString(dg_ForCustodians["GrossValue", i].Value);
                    String Commission = SystemLibrary.ToString(dg_ForCustodians["Commission", i].Value);
                    String Stamp = SystemLibrary.ToDecimal(dg_ForCustodians["Stamp", i].Value).ToString();
                    String GST = SystemLibrary.ToString(dg_ForCustodians["GST", i].Value);
                    String NetValue = SystemLibrary.ToString(dg_ForCustodians["NetValue", i].Value);
                    Decimal CommissionRate = Math.Abs(Math.Round(d_Commission / d_GrossValue, 6));
                    DateTime Settlement_Date = Convert.ToDateTime(dg_ForCustodians["SettlementDate", i].Value);
                    String ConfirmationNo = SystemLibrary.ToString(dg_ForCustodians["ConfirmationNo", i].Value);

                    // Irrespective of Send, save changed records back to the database.
                    if (dt_ForCustodians.Rows[i].RowState == DataRowState.Modified)
                    {
                        FoundChange = true;
                        dt_ForCustodians.Rows[i]["CommissionRate"] = CommissionRate;

                        // Not allowed to change a Cancel, so ignore
                        if (RecordType.ToLower() != "cancel")
                        {
                            mySql = "Update Trade " +
                                    "Set    BrokerConfirmed = '" + mySentToCustodian + "', " +
                                    "       Quantity = " + Quantity.ToString() + ", " +
                                    "       Price = " + Price.ToString() + ", " +
                                    "       GrossValue = " + GrossValue.ToString() + ", " +
                                    "       Commission = " + Commission.ToString() + ", " +
                                    "       Stamp = " + Stamp.ToString() + ", " +
                                    "       GST = " + GST.ToString() + ", " +
                                    "       NetValue = " + NetValue.ToString() + ", " +
                                    "       CommissionRate = " + CommissionRate.ToString() + ", " +
                                    "       SettlementDate = '" + Settlement_Date.ToString("dd-MMM-yyyy") + "', " +
                                    "       ConfirmationNo = '" + ConfirmationNo + "' " +
                                    "Where TradeID =  " + ClientTransactionID;

                            SystemLibrary.SQLExecute(mySql);

                            // Set the Transaction record to reflect the Trade record
                            mySql = "exec sp_ApplyTradeChangeToTransaction " + ClientTransactionID;
                            SystemLibrary.SQLExecute(mySql);
                        }
                    }
                }

                if (NumberofRecords < 1)
                {
                    Cursor.Current = Cursors.Default;
                    if (FoundChange)
                        MessageBox.Show("Saved Changes, now Please select Rows to Send", bt_SendToCustodian.Text);
                    else
                        MessageBox.Show("Please select Rows to Send", bt_SendToCustodian.Text);
                    return;
                }


                // NumberofRecords is where "Send" is ticked
                for (int i = 0; i < dg_ForCustodians.Rows.Count; i++)
                {
                    if (SystemLibrary.ToString(dg_ForCustodians["Send", i].Value) == "Y" && CustodianName == "SCOTIA")
                    {
                        // Load Variables
                        String ReportLine;
                        String ClientTransactionID = SystemLibrary.ToString(dg_ForCustodians["Client Transaction ID", i].Value);
                        String CustodianID = SystemLibrary.ToString(dg_ForCustodians["CustodianID", i].Value);
                        String ClientExecutingBroker = SystemLibrary.ToString(dg_ForCustodians["Client Executing Broker", i].Value);
                        String LS = SystemLibrary.ToString(dg_ForCustodians["LS", i].Value);
                        String BS = SystemLibrary.ToString(dg_ForCustodians["BS", i].Value);
                        String Security_Type = SystemLibrary.ToString(dg_ForCustodians["Security_Type", i].Value);
                        
                        String ScotiaBuySell = "";
                        /*
                         * B, S, SS, BC for equity/bond or BTO, BTC, STO, STC for futures/options
                         * - The problem with Futures being Buy-to-open Buy-to-close Sell-to-open Sell-to-close is that is Not the way they trade.
                         */
                        if (LS=="Long" && BS=="B" && Security_Type=="Option")
                            ScotiaBuySell = "BTO";
                        else if (LS=="Long" && BS=="S" && Security_Type=="Option")
                            ScotiaBuySell = "STC";
                        else if (LS=="Short" && BS=="S" && Security_Type=="Option")
                            ScotiaBuySell = "STO";
                        else if (LS=="Short" && BS=="B" && Security_Type=="Option")
                            ScotiaBuySell = "BTC";
                        else if (LS=="Long" && BS=="B")
                            ScotiaBuySell = "B";
                        else if (LS=="Long" && BS=="S")
                            ScotiaBuySell = "S";
                        else if (LS=="Short" && BS=="S")
                            ScotiaBuySell = "SS";
                        else if (LS=="Short" && BS=="B")
                            ScotiaBuySell = "BC";
                        

                        String Quantity = Math.Abs(SystemLibrary.ToDecimal(dg_ForCustodians["Quantity", i].Value)).ToString();             
                        String ClientProductIdType = SystemLibrary.ToString(dg_ForCustodians["Client Product Id Type", i].Value);
                        String ClientProductId = SystemLibrary.ToString(dg_ForCustodians["Client Product Id", i].Value);
                        String Security_Name = SystemLibrary.ToString(dg_ForCustodians["Security_Name", i].Value);
                        String Price = SystemLibrary.ToString(dg_ForCustodians["Price", i].Value);
                        String Pos_Mult_Factor = SystemLibrary.ToString(dg_ForCustodians["Pos_Mult_Factor", i].Value);
                        String Currency = SystemLibrary.ToString(dg_ForCustodians["Currency", i].Value);
                        String TradeDate = Convert.ToDateTime(dg_ForCustodians["TradeDate", i].Value).ToString("dd-MMM-yyyy");
                        String SettlementDate = Convert.ToDateTime(dg_ForCustodians["SettlementDate", i].Value).ToString("dd-MMM-yyyy");
                        String Commission = (SystemLibrary.ToDecimal(dg_ForCustodians["Commission", i].Value) / Math.Abs(SystemLibrary.ToDecimal(dg_ForCustodians["Quantity", i].Value))).ToString();
                        String Stamp = SystemLibrary.ToString(dg_ForCustodians["Stamp", i].Value);
                        String GST = SystemLibrary.ToString(dg_ForCustodians["GST", i].Value);
                        String GrossValue = Math.Abs(SystemLibrary.ToDecimal(dg_ForCustodians["GrossValue", i].Value)).ToString();
                        String NetValue = Math.Abs(SystemLibrary.ToDecimal(dg_ForCustodians["NetValue", i].Value)).ToString();
                        String AccountNumber = SystemLibrary.ToString(dg_ForCustodians["Account Number", i].Value);
                        String Scotia_Logon = SystemLibrary.ToString(dg_ForCustodians["SCOTIA_Logon", i].Value);
                        String RecordType = SystemLibrary.ToString(dg_ForCustodians["RecordType", i].Value).Substring(0, 1);
                        String ClientOriginalTransactionId = "";
                        String ReportClientTransactionID = "";

                        if (RecordType.ToLower() == "c")
                        {
                            /*
                             * If you are doing a straight cancel you would populate column AM with the original trade reference and populate the Column AK with a C.  You would leave column C (trade reference blank).
                             *
                             * For an cancel/correct you would populate column AM with the original trade reference and populate the Column AK with a A.  You would them put amend your ID to include an -A.  For example if you original ID was 123456_OBEL your new ID for column C would be 123456_OBEL-A.
                             */
                            ClientOriginalTransactionId = ClientTransactionID;
                            ReportClientTransactionID = "";
                        }
                        else
                        {
                            // Blank out RecordType
                            RecordType = "";
                            ReportClientTransactionID = ClientTransactionID;
                        }
                        // Build up the file details.
                        /* Scotia Emailed Rules 3-Feb-2014 (Caroline Sidle (Scotiabank GBM))
                         * 1.       “Price divisor” should be blank
                         * 2.       “Trade net ind” should be blank
                         * 3.       “exchange” should be blank
                         * 4.       “other fees” should be blank
                         * 5.       trade reference does not have the suffix of _OBEL 
                         */
                        Pos_Mult_Factor = ""; // Scotia: “Price divisor” should be blank
                        if (SystemLibrary.ToDecimal(Stamp) == 0)
                            Stamp = "";
                        if (SystemLibrary.ToDecimal(GST) == 0)
                            GST = "";
                        ReportLine = "!INS_TRADE,SEC," +
                                      ReportClientTransactionID + SCOTIA_TradeId_Suffix + "," + // Column C
                                      CustodianID + "," +
                                      CustodianID + "_BOOK," +
                                      "NONE," +
                                      ClientExecutingBroker + "," +
                                      ScotiaBuySell + "," +
                                      Quantity + "," +
                                      ClientProductIdType + "," +
                                      ClientProductId + "," +
                                      Security_Name + "," +
                                      Price + "," +
                                      Pos_Mult_Factor + ",," +
                                      Currency + "," + //trade ccy
                                      TradeDate + "," +
                                      SettlementDate + "," +
                                      Commission + "," +
                                      Stamp + "," +
                                      GST + "," + // Other Fees
                                      GrossValue + "," +
                                      NetValue + "," +
                                      Currency + ",1,M,1,M," + //sett ccy
                                      CustodianName + "," +
                                      AccountNumber + ",NOR,,,,*," +
                                      Scotia_Logon + "," +
                                      RecordType + ",," + // Column AK
                                      ClientOriginalTransactionId   // Column AK
                                      ;
                        sb_Out.AppendLine(ReportLine);

                        // Mark the Database as Sent.
                        if (RecordType.ToLower() == "c") // Cancel
                        {
                            mySql = "Update Trade_Cancel " +
                                    "Set    Cancel_SentToCustodian = '" + mySentToCustodian + "', " +
                                    "       Cancel_BrokerConfirmed = '" + mySentToCustodian + "' " +
                                    "Where TradeID =  " + ClientTransactionID;
                        }
                        else
                        {
                            mySql = "Update Trade " +
                                    "Set    SentToCustodian = '" + mySentToCustodian + "', " +
                                    "       BrokerConfirmed = '" + mySentToCustodian + "' " +
                                    "Where TradeID =  " + ClientTransactionID;
                        }
                        SystemLibrary.SQLExecute(mySql);

                        if (dt_Out.Columns.Count > 0)
                        {
                            // Write to the Database dt_Out
                            DataRow dr = dt_Out.NewRow();
                            dr["SCOTIAFileName"] = myFileName;
                            dr["SCOTIATimeStamp"] = myTimeStamp;
                            dr["Client_Transaction_ID"] = ClientTransactionID;
                            dr["Record_Type"] = RecordType;
                            dr["ReportLine"] = ReportLine;
                            dt_Out.Rows.Add(dr);
                        }
                    }
                }

                // Save to Database
                if (dt_Out.Columns.Count > 0)
                {
                    int myRows = SystemLibrary.SQLBulkUpdate(dt_Out, "", "SCOTIA_Out");
                }

                // Footer Rows
                sb_Out.AppendLine(",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                sb_Out.AppendLine("#Check for errors,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                sb_Out.AppendLine("!END,TRADE_UPLOAD_" + Scotia_Prefix + ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");


                // Write the Data to file
                File.Delete(myFilePath + @"\" + myFileName); // Should never need this.
                File.WriteAllText(myFilePath + @"\" + myFileName, sb_Out.ToString());

                // Drop the file into the Scotia Prime Web site.
                Boolean FtpResult = FTPToPrime(CustodianName, myFilePath, myFileName);
                if (FtpResult)
                {
                    mySql = "Update SCOTIA_Out " +
                            "Set    FileSent = dbo.f_GetDate(), " +
                            "       FileMethod = 'FTP' " +
                            "Where  SCOTIAFileName = '" + myFileName + "' ";
                    SystemLibrary.SQLExecute(mySql);

                    // Alter the LastUpdate for this user, so it searches for the confirm in 15 minutes
                    SystemLibrary.FTPSCOTIAPrimeVars.LastUpdate = SystemLibrary.f_Now().AddSeconds(-SystemLibrary.FTPMLPrimeVars.Interval_seconds + 900);

                    myMessage = "Scotia file sent.\r\n\r\n";
                }
                if(!FtpResult || CustodianName == "SCOTIA")
                {
                    // FTP Failed, so trying email
                    if (EmailToPrime(CustodianName, myFilePath + @"\" + myFileName))
                    {
                        mySql = "Update SCOTIA_Out " +
                                "Set    FileSent = dbo.f_GetDate(), " +
                                "       FileMethod = 'Email' " +
                                "Where  SCOTIAFileName = '" + myFileName + "' ";
                        SystemLibrary.SQLExecute(mySql);
                        myMessage = "Scotia File Failed to go directly to server, so have emailed instead.\r\n\r\n";
                    }
                    else
                    {
                        mySql = "Update SCOTIA_Out " +
                                "Set    FileSent = dbo.f_GetDate(), " +
                                "       FileMethod = 'Manual' " +
                                "Where  SCOTIAFileName = '" + myFileName + "' ";
                        SystemLibrary.SQLExecute(mySql);
                        myMessage = "Scotia File Failed to go directly to server. Email also failed. Extract file from saved location and process appropriately.\r\n\r\n";
                    }
                }

                if (sender != null)
                    MessageBox.Show(myMessage + myFilePath + @"\" + myFileName, bt_SendToCustodian.Text);

            } //.Select("CustodianName='SCOTIA'")

            SystemLibrary.DebugLine(origDebugLevel);

            // Refresh
            LoadAll();
            cb_SelectAll.Checked = false;
            Cursor.Current = Cursors.Default;

        } //tp_TradesForCustodian_Paint()

        private Boolean EmailToPrime(String WhichPrime, String myFilePath)
        {
            // Local Variables
            String htmlBody = "";
            String myPrefix = "";
            String myMessage = "";
            MailMessage mail = null;
            Boolean RetVal = true;


            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            int origDebugLevel = SystemLibrary.GetDebugLevel();
            SystemLibrary.SetDebugLevel(4);


            switch (WhichPrime)
            {
                case "Merrill Lynch":
                    myPrefix = "ML";
                    break;
                case "SCOTIA":
                    myPrefix = "Scotia";
                    break;
            }

            String Prime_ToEmail = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('" + myPrefix + "Prime:ToEmail')");
            String Prime_FromEmail = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('" + myPrefix + "Prime:FromEmail')");
            String Prime_CCEmail = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('" + myPrefix + "Prime:CCEmail')");
            String Prime_BCCEmail = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('" + myPrefix + "Prime:BCCEmail')");
            String Prime_Title = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('" + myPrefix + "Prime:Title')");
            String Prime_Signature = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('" + myPrefix + "Prime:Signature')");
           
            // Put together the email
            mail = new MailMessage();

            mail.From = new MailAddress(Prime_FromEmail);

            if (Prime_ToEmail == "")
                mail.To.Add(Prime_FromEmail);
            else
            {
                //String 
                foreach (String myStr in Prime_ToEmail.Split(",;".ToCharArray()))
                    mail.To.Add(myStr);
            }
            if (Prime_CCEmail != "")
            {
                foreach (String myStr in Prime_CCEmail.Split(",;".ToCharArray()))
                    mail.CC.Add(myStr);
            }
            if (Prime_BCCEmail != "")
            {
                foreach (String myStr in Prime_BCCEmail.Split(",;".ToCharArray()))
                    mail.Bcc.Add(myStr);
            }

            mail.Attachments.Add(new Attachment(myFilePath));

            mail.Subject = Prime_Title + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
            mail.IsBodyHtml = true;
            htmlBody = SystemLibrary.HTMLStart();

            // On change of group, send the Mail
            htmlBody = SystemLibrary.HTMLLine(Prime_Signature) +
                       SystemLibrary.HTMLEnd();
            mail.Body = htmlBody;
            //mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.OnSuccess;


            // Send the email
            RetVal = SystemLibrary.SendEmail(mail, ref myMessage);
            if (!RetVal)
                MessageBox.Show(myMessage, "Failed to Send email");

            // Clean up
            mail.Dispose();
            mail = null;

            Cursor.Current = Cursors.Default;
            SystemLibrary.SetDebugLevel(origDebugLevel);

            return (RetVal);
            
        } //EmailToPrime()


        private Boolean FTPToPrime(String WhichPrime, String myFilePath, String myFileName)
        {
            // Local Variables
            String myEncryptName = "";
            Boolean RetVal = false;
            Boolean isMLPrimeConfigured = true;
            Boolean isScotiaPrimeConfigured = true;
            Boolean[] RetFTP;

            int origDebugLevel = SystemLibrary.GetDebugLevel();
            SystemLibrary.SetDebugLevel(4);

            if (ParentForm1 != null)
            {
                isMLPrimeConfigured = ParentForm1.MLPrimeConfigured;
                isScotiaPrimeConfigured = ParentForm1.ScotiaPrimeConfigured;
            }

            SystemLibrary.DebugLine("Sent " + WhichPrime + " Prime FTP File: Start");

            // get the FTP parameters from the form
            switch (WhichPrime)
            {
                case "Merrill Lynch":
                    RetFTP = ParentForm1.FTPMLPrimeStructure("LOAD");
                    isMLPrimeConfigured = RetFTP[1];
                    if (RetFTP[0])
                    {
                        SystemLibrary.DebugLine("FTPMLPrimeStructure: OK");
                        if (SystemLibrary.FileExists(myFilePath + "\\" + myFileName))
                        {
                            // Encrypt the file
                            if (SystemLibrary.MLPrime_Encrypt(myFilePath, myFileName, ref myEncryptName))
                            {
                                // Deposit to the FTP server
                                RetVal = SystemLibrary.FTPUploadFile(SystemLibrary.FTPMLPrimeVars, myFilePath, "/incoming/gpb", myEncryptName);
                            }
                        }
                    }
                    break;
                case "SCOTIA":
                    RetFTP = ParentForm1.FTPSCOTIAPrimeStructure("LOAD");
                    isScotiaPrimeConfigured = RetFTP[1];
                    if (RetFTP[0])
                    {
                        SystemLibrary.DebugLine("FTPSCOTIAPrimeStructure: OK");
                        if (SystemLibrary.FileExists(myFilePath + "\\" + myFileName))
                        {
                            // Deposit to the FTP server
                            RetVal = SystemLibrary.FTPUploadFile(SystemLibrary.FTPSCOTIAPrimeVars, myFilePath, "", myFileName);
                        }
                    }
                    break;
            }

            ParentForm1.SetFlags(ParentForm1.BloombergEMSXFileConfigured, isScotiaPrimeConfigured, isMLPrimeConfigured, ParentForm1.MLFuturesConfigured);

            SystemLibrary.DebugLine("Sent " + WhichPrime + " Prime FTP File: End");
            SystemLibrary.SetDebugLevel(origDebugLevel);
            return (RetVal);

        } //FTPToMLPrime()


        private void dg_ForCustodians_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Local Variables


            // What column is changing
            // - dg_Broker.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.ToString().ToUpper()
            if (dg_ForCustodians.Columns[e.ColumnIndex].Name == "Quantity" ||
                dg_ForCustodians.Columns[e.ColumnIndex].Name == "GrossValue" ||
                dg_ForCustodians.Columns[e.ColumnIndex].Name == "Commission" ||
                dg_ForCustodians.Columns[e.ColumnIndex].Name == "Stamp" ||
                dg_ForCustodians.Columns[e.ColumnIndex].Name == "GST" ||
                dg_ForCustodians.Columns[e.ColumnIndex].Name == "NetValue" 
                )
            {
                // Validate this is a number
                Decimal myResult = new Decimal();
                if (!Decimal.TryParse(dg_ForCustodians.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim(), out myResult))
                    myResult = 0;
                else
                    myResult = SystemLibrary.ToDecimal(dg_ForCustodians.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                if (Math.Sign(myResult) != Math.Sign(SystemLibrary.ToDecimal(LastValue)) && myResult != 0 && SystemLibrary.ToDecimal(LastValue)!=0)
                    dg_ForCustodians.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                else
                    dg_ForCustodians.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult;
                ParentForm1.SetColumn(dg_ForCustodians, dg_ForCustodians.Columns[e.ColumnIndex].Name, e.RowIndex);
            }
            else if (dg_ForCustodians.Columns[e.ColumnIndex].Name == "Price")
            {
                // Validate this is a number
                Decimal myResult = new Decimal();
                if (!Decimal.TryParse(dg_ForCustodians.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim(), out myResult))
                    myResult = 0;
                else
                    myResult = SystemLibrary.ToDecimal(dg_ForCustodians.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                if (Math.Sign(myResult) < 0)
                    dg_ForCustodians.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                else
                    dg_ForCustodians.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult; //.ToString(dg_ForCustodians.Columns[e.ColumnIndex].DefaultCellStyle.Format);
                //dg_ForCustodians.Rows[e.RowIndex].Cells[e.ColumnIndex].for
            }
            else if (dg_ForCustodians.Columns[e.ColumnIndex].Name == "SettlementDate")
            {
                // Validate this is a Date
                DateTime myResult = new DateTime();
                if (!DateTime.TryParse(dg_ForCustodians.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim(), out myResult))
                {
                    dg_ForCustodians.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                }
                else
                    dg_ForCustodians.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult.ToString(dg_ForCustodians.Columns[e.ColumnIndex].DefaultCellStyle.Format);

            }
        } //dg_ForCustodians_CellEndEdit()

        private void dg_ForCustodians_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Seems the only way I can get the previous value before CellEndEdit().
            LastValue = dg_ForCustodians[e.ColumnIndex, e.RowIndex].Value;
            // Cannot alter cancelled records
            if (SystemLibrary.ToString(dg_ForCustodians["RecordType", e.RowIndex].Value).ToLower()=="cancel")
                e.Cancel = true;

        } //dg_ForCustodians_CellBeginEdit()

        private void dg_ForCustodians_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Dont need to report what is wrong, just reject.
            e.Cancel = false;
            dg_ForCustodians.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
        } //dg_ForCustodians_DataError()

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            LoadAll();
        } //bt_Refresh_Click()

        private void bt_MarkAsSent_Click(object sender, EventArgs e)
        {
            //Local Variables
            String mySql;

            //Get the state from the database again to make sure hasn't already been processed.
            mySql = "Exec sp_ProcessTrade_ForBroker ";
            DataTable dt_Check = SystemLibrary.SQLSelectToDataTable(mySql);

            for (int i = 0; i < dg_ForBrokers.Rows.Count; i++)
            {
                // Make sure it hasn't already been processed
                DataRow[] dr = dt_Check.Select("TradeID='" + SystemLibrary.ToString(dg_ForBrokers["TradeID", i].Value) + "'");
                if (dr.Length < 1)
                {
                    MessageBox.Show("Found Some of the Trades have already been Marked as Sent.\r\n\r\n" +
                                    "Will run a Refresh on your behalf so that only unsent Trades remain on screen.", bt_MarkAsSent.Text);
                    LoadAll();
                    return;
                }
            }


            if (MessageBox.Show(this, "WARNING: This will process the Trades\r\r\r\n" +
                          "\tHowever:  It WILL NOT generate any email telling the Broker the allocation.\r\n\r\n\r\n" +
                          "Do you wish to Continue?", bt_MarkAsSent.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // hourglass cursor
                Cursor.Current = Cursors.WaitCursor;

                // Mark the Trades as Done
                for (int i = 0; i < dg_ForBrokers.Rows.Count; i++)
                {
                    String TradeID = dg_ForBrokers["TradeID", i].Value.ToString();
                    String IsInternal = SystemLibrary.ToString(dg_ForBrokers["IsInternal", i].Value);
                    String IsFuture = SystemLibrary.ToString(dg_ForBrokers["IsFuture", i].Value);
                    String RecordType = SystemLibrary.ToString(dg_ForBrokers["RecordType", i].Value);

                    if (SystemLibrary.ToString(dg_ForBrokers["Send", i].Value) == "Y")
                    {
                        if (RecordType=="Cancel")
                        {
                            // Mark the Database as Sent.
                            mySql = "Update Trade_Cancel Set Cancel_SentToBroker = dbo.f_GetDate() where TradeID =  " + TradeID;
                            SystemLibrary.SQLExecute(mySql);

                            // No custodian needed
                            if (IsInternal == "Y" || IsFuture == "Y")
                            {
                                // Marke other Processing Fields as though this has been pushed through to the Custodian.
                                mySql = "Update Trade_Cancel Set Cancel_BrokerConfirmed = Cancel_SentToBroker, Cancel_SentToCustodian = Cancel_SentToBroker where TradeID =  " + TradeID;
                                SystemLibrary.SQLExecute(mySql);
                            }
                        }
                        else
                        {
                            // Mark the Database as Sent.
                            mySql = "Update Trade Set SentToBroker = dbo.f_GetDate() where TradeID =  " + TradeID;
                            SystemLibrary.SQLExecute(mySql);

                            // No custodian needed
                            if (IsInternal == "Y" || IsFuture == "Y")
                            {
                                // Marke other Processing Fields as though this has been pushed through to the Custodian.
                                mySql = "Update Trade Set BrokerConfirmed = SentToBroker, SentToCustodian = SentToBroker where TradeID =  " + TradeID;
                                SystemLibrary.SQLExecute(mySql);
                            }
                        }
                    }
                }

                // Refresh
                LoadAll();
                cb_SelectAll.Checked = false;
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Wait for Broker Confirms on Trades, then move to Step 3", bt_CreateTrades.Text);
            }

        } //bt_MarkAsSent_Click()

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            switch(e.TabPage.Name)
            {
                case "tp_TradesForBrokers":
                    FormatForBrokers();
                    break;
                case "tp_TradesForCustodian":
                    FormatForCustodians();
                    break;
                case "tp_ResendCustodianFile":
                    if (dg_CustodianFiles.Rows.Count<1)
                        bt_RequestHistoricFiles_Click(null, null);
                    break;
                case "tp_PreviouslyProcessedTrades":
                    if (dg_HistoricTrades.Rows.Count < 1)
                        bt_RequestHistoricTrades_Click(null, null);
                    break;
            }
        }

        private void bt_CheckContractNotes_Click(object sender, EventArgs e)
        {
            // Local Variables
            DataTable dt_Confirms = new DataTable();
            String mySql;
            String myFileName;
            Int32 myDirection;
            Int32 Quantity;
            int TotalFiles = 0;

            // Stop multiple running from background task and user
            if (inCheckContractNotes)
                return;

            inCheckContractNotes = true;

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            // Set up a DataTable to store the confirms
            dt_Confirms.Columns.Add("Processed"); // Y,N Flag
            dt_Confirms.Columns.Add("FileName");
            dt_Confirms.Columns.Add("ConfirmationNo");
            dt_Confirms.Columns.Add("BS");
            dt_Confirms.Columns.Add("Quantity", System.Type.GetType("System.Decimal"));
            dt_Confirms.Columns.Add("Ticker");
            dt_Confirms.Columns.Add("TradeID", System.Type.GetType("System.Int32"));
            //dt_Confirms.Columns.Add("Price", System.Type.GetType("System.Decimal"));

            // Loop over the files in the ML Bookings directory
            try
            {
                if (SystemLibrary.BookingsFilePath == null)
                    SystemLibrary.BookingsFilePath = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('Bookings_Path')");

                if (SystemLibrary.BookingsFilePath.Length > 0)
                    bt_OpenBookingsDirectory.Enabled = true;
                else
                    return;

                DirectoryInfo di = new DirectoryInfo(SystemLibrary.BookingsFilePath);
                FileInfo[] rgFiles = di.GetFiles("*.pdf");

                TotalFiles = rgFiles.Length;
                for (int i = 0; i < TotalFiles; i++)
                {
                    // Filename is space seperated "<ConfirmationNo> <Buy/Sell> <Qty> <Ticker. eg ANZ> <rest>.pdf"
                    // Rules: If <Qty> is Zero, then this is a cancellation for that ConfirmationNo.
                    myFileName = rgFiles[i].Name;
                    String[] myHeader = myFileName.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (myHeader.Length > 4)
                    {
                        // Set quantity sign based on BS

                        DataRow dr = dt_Confirms.NewRow();
                        dr["Processed"] = "N";
                        dr["FileName"] = myFileName;
                        dr["ConfirmationNo"] = myHeader[0];
                        dr["BS"] = myHeader[1].Substring(0, 1).ToUpper();
                        if (dr["BS"].ToString() == "S")
                            myDirection = -1;
                        else
                            myDirection = 1;
                        Quantity = SystemLibrary.ToInt32(myHeader[2]) * myDirection;
                        dr["Quantity"] = Quantity;
                        dr["Ticker"] = myHeader[3];

                        if (Quantity == 0)
                        {
                            // This is a cancellation, so remove from database if applied
                            // TODO (2) -  I need to work through this and add to Trade_Cancel table if already at custodian.
                            mySql = "Update Trade " +
                                    "Set ";
                            dr["Processed"] = "Y";

                            // Delete any file that starts with this confirmation number
                            FileInfo[] removeFiles = di.GetFiles(dr["ConfirmationNo"].ToString() + "*.pdf");
                            for (int d = 0; d < removeFiles.Length; d++)
                            {
                                // Delete the File
                                if (System.IO.File.Exists(removeFiles[d].FullName))
                                {
                                    try
                                    {
                                        System.IO.File.Delete(removeFiles[d].FullName);
                                    }
                                    catch { }
                                }
                            }
                        }
                        else
                        {
                            // The delete of cancelled files may mean the file no longer exists
                            if (System.IO.File.Exists(SystemLibrary.BookingsFilePath + @"\" + myFileName))
                            {
                                try
                                {
                                    // Strip the document
                                    PDDocument doc = PDDocument.load(SystemLibrary.BookingsFilePath + @"\" + myFileName);
                                    PDFTextStripper stripper = new PDFTextStripper();
                                    String myText = stripper.getText(doc);

                                    // Now filter the dt_ForCustodians
                                    DataRow[] FoundRows = dt_ForCustodians.Select("BBG_Ticker like '" + dr["Ticker"].ToString() + @"%' and " +
                                                                                  "BS = '" + dr["BS"].ToString() + "' and " +
                                                                                  "Quantity = " + dr["Quantity"].ToString() + " and " +
                                                                                  "RecordType<>'cancel'");
                                    foreach (DataRow drt in FoundRows)
                                    {
                                        // Search myText for key items
                                        Int32 FoundNet = myText.IndexOf(Math.Abs(SystemLibrary.ToDecimal(drt["NetValue"])).ToString("#,###.##"));
                                        Int32 FoundBrokerAccount = myText.IndexOf(drt["Broker Account"].ToString());
                                        Int32 FoundTradeDate = myText.IndexOf(Convert.ToDateTime(drt["TradeDate"]).ToString("d/MM/yyyy"));
                                        Int32 FoundSettlementDate = myText.IndexOf(Convert.ToDateTime(drt["SettlementDate"]).ToString("d/MM/yyyy"));
                                        if (FoundNet > -1 && FoundBrokerAccount > -1 && FoundTradeDate > -1 && FoundSettlementDate > -1)
                                        {
                                            drt["ConfirmationNo"] = dr["ConfirmationNo"].ToString();
                                            dr["TradeID"] = drt["TradeID"];
                                            dr["Processed"] = "Y";

                                            // Now update the database with all the data on this record, as the user may have changed to get it to match
                                            String ClientTransactionID = SystemLibrary.ToString(drt["Client Transaction ID"]);
                                            String myQuantity = SystemLibrary.ToString(drt["Quantity"]);
                                            String Price = SystemLibrary.ToString(drt["Price"]);
                                            Decimal d_GrossValue = SystemLibrary.ToDecimal(drt["GrossValue"]);
                                            Decimal d_Commission = SystemLibrary.ToDecimal(drt["Commission"]);
                                            String GrossValue = SystemLibrary.ToString(drt["GrossValue"]);
                                            String Commission = SystemLibrary.ToString(drt["Commission"]);
                                            String Stamp = SystemLibrary.ToDecimal(drt["Stamp"]).ToString();
                                            String GST = SystemLibrary.ToString(drt["GST"]);
                                            String NetValue = SystemLibrary.ToString(drt["NetValue"]);
                                            Decimal CommissionRate = Math.Abs(Math.Round(d_Commission / d_GrossValue, 6));
                                            DateTime Settlement_Date = Convert.ToDateTime(drt["SettlementDate"]);
                                            String ConfirmationNo = SystemLibrary.ToString(drt["ConfirmationNo"]);

                                            mySql = "Update Trade " +
                                                    "Set    Quantity = " + myQuantity.ToString() + ", " +
                                                    "       Price = " + Price.ToString() + ", " +
                                                    "       GrossValue = " + GrossValue.ToString() + ", " +
                                                    "       Commission = " + Commission.ToString() + ", " +
                                                    "       Stamp = " + Stamp.ToString() + ", " +
                                                    "       GST = " + GST.ToString() + ", " +
                                                    "       NetValue = " + NetValue.ToString() + ", " +
                                                    "       CommissionRate = " + CommissionRate.ToString() + ", " +
                                                    "       SettlementDate = '" + Settlement_Date.ToString("dd-MMM-yyyy") + "', " +
                                                    "       ConfirmationNo = '" + ConfirmationNo + "' " +
                                                    "Where TradeID =  " + ClientTransactionID;

                                            SystemLibrary.SQLExecute(mySql);

                                            // Set the Transaction record to reflect the Trade record
                                            mySql = "exec sp_ApplyTradeChangeToTransaction " + ClientTransactionID;
                                            SystemLibrary.SQLExecute(mySql);

                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                        dt_Confirms.Rows.Add(dr);

                    }
                }

                // Loop over dg_Trades and tick the Send box
                for (Int32 i = 0; i < dg_ForCustodians.Rows.Count; i++)
                {
                    if (dg_ForCustodians["ConfirmationNo", i].Value.ToString().Length > 0)
                        dg_ForCustodians["Send", i].Value = "Y";
                }

                // Loop over dt_Confirms and update both the database and the dg_ForCustodians
                for (int i = 0; i < dt_Confirms.Rows.Count; i++)
                {
                    myFileName = dt_Confirms.Rows[i]["FileName"].ToString();
                    String TradeID = SystemLibrary.ToString(dt_Confirms.Rows[i]["TradeID"]);
                    String ConfirmationNo = SystemLibrary.ToString(dt_Confirms.Rows[i]["ConfirmationNo"]);
                    // If this has a TradeID, then Upodate teh Trades table with the ConfirmationNo
                    if (TradeID.Length > 0 && ConfirmationNo.Length > 0)
                    {
                        mySql = "Update Trade " +
                                "Set ConfirmationNo = '" + ConfirmationNo + "' " +
                                "Where  TradeID = " + TradeID + " " +
                                "And    SentToBroker is Not Null " +
                                "And	SentToCustodian is Null " +
                                "And	CustodianConfirmed is Null ";
                        SystemLibrary.SQLExecute(mySql);

                        // Delete the File
                        if (System.IO.File.Exists(SystemLibrary.BookingsFilePath + @"\" + myFileName))
                        {
                            try
                            {
                                System.IO.File.Delete(SystemLibrary.BookingsFilePath + @"\" + myFileName);
                            }
                            catch { }
                        }
                    }
                }
            }
            catch { }

            // See if theses have not already been processed.
            LoadForCustodians();

            Cursor.Current = Cursors.Default;
            inCheckContractNotes = false;

            if (sender == null)
            {
                int myProcessedCount = 0;
                // This has come from the Timer event, so turn it off if no more work to be done
                for (Int32 i = 0; i < dg_ForCustodians.Rows.Count; i++)
                {
                    if (dg_ForCustodians["ConfirmationNo", i].Value.ToString().Length > 0)
                        myProcessedCount++;
                }
                /*
                 * 24-Dec-2013 Only stop check if no more rows to process.
                 */
                //if (myProcessedCount == dg_ForCustodians.Rows.Count)
                if (dg_ForCustodians.Rows.Count == 0)
                {
                    timer_CheckContractNotes.Stop();
                    timer_CheckContractNotes.Enabled = false;
                }
                
                // See if the user wants this to auto close and send to custodian
                if (cb_CloseWhenDone.Checked && myProcessedCount == dg_ForCustodians.Rows.Count)
                {
                    bt_SendToCustodian_Click(null, null);
                    if(tabControl1.SelectedTab.Name == "tp_TradesForCustodian")
                        this.Close();
                }

                if (myProcessedCount > 0)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.TopMost = true;
                    this.TopMost = false;
                }
            }
            else
                MessageBox.Show("Processed " + TotalFiles.ToString() + " Bookings files in directory;\r\n\r\n" + SystemLibrary.BookingsFilePath, "Processing Complete");

        } //bt_CheckContractNotes_Click()

        private void timer_CheckContractNotes_Tick(object sender, EventArgs e)
        {
            if (dg_ForCustodians.Rows.Count > 0 && inCheckContractNotes==false)
                bt_CheckContractNotes_Click(null, null);

        } //timer_CheckContractNotes_Tick()

        private void bt_OpenBookingsDirectory_Click(object sender, EventArgs e)
        {
            Process.Start(SystemLibrary.BookingsFilePath);

        } //bt_OpenBookingsDirectory_Click()

        private void ProcessTrades_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);

        } //ProcessTrades_Shown()

        private void dg_Trades_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Show the popup menu
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    Point myLocation = new Point(this.Location.X + CXLocation, this.Location.Y + CYLocation);
                    // Select the Order as needed in code later on.
                    String OrderRefID = SystemLibrary.ToString(dg_Trades.Rows[e.RowIndex].Cells["OrderRefId"].Value);
                    
                    ContextMenuStrip myMenu = new ContextMenuStrip();
                    ToolStripMenuItem mySubMenu = new ToolStripMenuItem();

                    TradeMenuStruct myTradeStr = new TradeMenuStruct();
                    myTradeStr.OrderRefID = OrderRefID;
                    myTradeStr.myParentForm = this;

                    // Release back to Order Screen 
                    mySubMenu = new ToolStripMenuItem("Release back to Order Screen");
                    mySubMenu.Tag = myTradeStr;
                    mySubMenu.MouseUp += new MouseEventHandler(dg_TradesSystemMenuItem_Click);
                    myMenu.Items.Add(mySubMenu);
                    // Show the Menu
                    myMenu.Show(myLocation);
                }
            }
            catch { }

        } //dg_Trades_CellMouseClick()

        public static void dg_TradesSystemMenuItem_Click(object sender, MouseEventArgs e)
        {
            //
            // Purpose: A generic menu click event for the Order Right-Click
            //

            // Local Variables
            ToolStripMenuItem ts_From = (ToolStripMenuItem)sender;
            TradeMenuStruct myTradeStr = (TradeMenuStruct)ts_From.Tag;
            String OrderRefID = myTradeStr.OrderRefID;
            ProcessTrades myForm = myTradeStr.myParentForm;
            
            if (OrderRefID.Length > 0)
            {
                String MySql = "Update Orders Set Order_Completed = 'N' where OrderRefID = '" + OrderRefID + "';" +
                               "Update Fills Set Confirmed = 'N' where OrderRefID = '" + OrderRefID + "'";
                SystemLibrary.SQLExecute(MySql);

                // Refresh this screen
                myForm.LoadTrades();
            }
        } //dg_TradesSystemMenuItem_Click()
        
        private void dg_Trades_MouseClick(object sender, MouseEventArgs e)
        {
            CXLocation = dg_Trades.Location.X + e.Location.X + 5;
            CYLocation = dg_Trades.Location.Y + tabControl1.Location.Y + tp_OrdersToTrades.Top + e.Location.Y;
        } //dg_Trades_MouseClick()

        private void dg_Trades_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Dont need to report what is wrong, just reject.
            e.Cancel = false;
            dg_Trades.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
        } //dg_Trades_DataError()

        private void dg_Trades_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Local Variables


            // What column is changing
            // - dg_Broker.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.ToString().ToUpper()
            if (dg_Trades.Columns[e.ColumnIndex].Name == "Commission" ||
                dg_Trades.Columns[e.ColumnIndex].Name == "Stamp" ||
                dg_Trades.Columns[e.ColumnIndex].Name == "GST" ||
                dg_Trades.Columns[e.ColumnIndex].Name == "NetValue"
                )
            {
                // Validate this is a number
                Decimal myResult = new Decimal();
                if (!Decimal.TryParse(dg_Trades.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim(), out myResult))
                    myResult = 0;
                else
                    myResult = SystemLibrary.ToDecimal(dg_Trades.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                if (Math.Sign(myResult) != Math.Sign(SystemLibrary.ToDecimal(LastValue)) && myResult != 0 && SystemLibrary.ToDecimal(LastValue) != 0)
                    dg_Trades.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                else
                    dg_Trades.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult;
                ParentForm1.SetColumn(dg_Trades, dg_Trades.Columns[e.ColumnIndex].Name, e.RowIndex);

                // Now adjust the other cells
                switch (dg_Trades.Columns[e.ColumnIndex].Name)
                {
                    case "Commission":
                    case "Stamp":
                    case "GST":
                        dg_Trades.Rows[e.RowIndex].Cells["NetValue"].Value = SystemLibrary.ToDecimal(dg_Trades.Rows[e.RowIndex].Cells["GrossValue"].Value)
                                                                           + SystemLibrary.ToDecimal(dg_Trades.Rows[e.RowIndex].Cells["Commission"].Value)
                                                                           + SystemLibrary.ToDecimal(dg_Trades.Rows[e.RowIndex].Cells["Stamp"].Value)
                                                                           + SystemLibrary.ToDecimal(dg_Trades.Rows[e.RowIndex].Cells["GST"].Value);
                        ParentForm1.SetColumn(dg_Trades, dg_Trades.Columns["NetValue"].Name, e.RowIndex);

                        if (dg_Trades.Columns[e.ColumnIndex].Name == "Commission")
                        {
                                dg_Trades.Rows[e.RowIndex].Cells["CommissionRate"].Value = SystemLibrary.ToDecimal(dg_Trades.Rows[e.RowIndex].Cells["Commission"].Value) / 
                                                               Math.Abs(SystemLibrary.ToDecimal(dg_Trades.Rows[e.RowIndex].Cells["GrossValue"].Value));
                                ParentForm1.SetColumn(dg_Trades, dg_Trades.Columns["CommissionRate"].Name, e.RowIndex);
                        }
                        break;
                }
            }
            else if (dg_Trades.Columns[e.ColumnIndex].Name == "SettlementDate")
            {
                // Validate this is a Date
                DateTime myResult = new DateTime();
                if (!DateTime.TryParse(dg_Trades.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim(), out myResult))
                {
                    dg_Trades.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                }
                else
                    dg_Trades.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult.ToString(dg_Trades.Columns[e.ColumnIndex].DefaultCellStyle.Format);

            }
        } //dg_Trades_CellEndEdit()

        private void dg_Trades_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Seems the only way I can get the previous value before CellEndEdit().
            LastValue = dg_Trades[e.ColumnIndex, e.RowIndex].Value;

        } //dg_Trades_CellBeginEdit()


        private void dg_ForBrokers_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Show the popup menu
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    Point myLocation = new Point(this.Location.X + CXLocation, this.Location.Y + CYLocation);
                    // Select the Order as needed in code later on.
                    String TradeID = SystemLibrary.ToString(dg_ForBrokers.Rows[e.RowIndex].Cells["TradeID"].Value);

                    ContextMenuStrip myMenu = new ContextMenuStrip();
                    ToolStripMenuItem mySubMenu = new ToolStripMenuItem();

                    TradeMenuStruct myTradeStr = new TradeMenuStruct();
                    myTradeStr.TradeID = TradeID;
                    myTradeStr.myParentForm = this;

                    // Release back to Order Screen 
                    mySubMenu = new ToolStripMenuItem("Reverse Trade Back to Order");
                    mySubMenu.Tag = myTradeStr;
                    mySubMenu.MouseUp += new MouseEventHandler(dg_ForBrokersSystemMenuItem_Click);
                    myMenu.Items.Add(mySubMenu);
                    // Show the Menu
                    myMenu.Show(myLocation);
                }
            }
            catch { }


        } //dg_ForBrokers_CellMouseClick()

        public static void dg_ForBrokersSystemMenuItem_Click(object sender, MouseEventArgs e)
        {
            //
            // Purpose: A generic menu click event for the Order Right-Click
            //

            // Local Variables
            ToolStripMenuItem ts_From = (ToolStripMenuItem)sender;
            TradeMenuStruct myTradeStr = (TradeMenuStruct)ts_From.Tag;
            String TradeID = myTradeStr.TradeID;
            ProcessTrades myForm = myTradeStr.myParentForm;

            if (TradeID.Length > 0)
            {
                String MySql = "Exec sp_ReverseTrade '" + TradeID + "'";
                SystemLibrary.SQLExecute(MySql);

                // Refresh this screen
                myForm.bt_RequestHistoricTrades_Click(null, null);
                myForm.LoadAll();
            }
        } //dg_ForBrokersSystemMenuItem_Click()


        private void dg_ForBrokers_MouseClick(object sender, MouseEventArgs e)
        {
            CXLocation = dg_ForBrokers.Location.X + e.Location.X + 5;
            CYLocation = dg_ForBrokers.Location.Y + tabControl1.Location.Y + tp_OrdersToTrades.Top + e.Location.Y;
        }//dg_ForBrokers_MouseClick()

        private void dg_ForCustodians_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Show the popup menu
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    Point myLocation = new Point(this.Location.X + CXLocation, this.Location.Y + CYLocation);
                    // Select the Order as needed in code later on.
                    String TradeID = SystemLibrary.ToString(dg_ForCustodians.Rows[e.RowIndex].Cells["TradeID"].Value);

                    ContextMenuStrip myMenu = new ContextMenuStrip();
                    ToolStripMenuItem mySubMenu = new ToolStripMenuItem();

                    TradeMenuStruct myTradeStr = new TradeMenuStruct();
                    myTradeStr.TradeID = TradeID;
                    myTradeStr.myParentForm = this;

                    // Release back to Order Screen 
                    mySubMenu = new ToolStripMenuItem("Reverse Trade Back to Order AND Prepare a Reversal Note for the Broker.");
                    mySubMenu.Tag = myTradeStr;
                    mySubMenu.MouseUp += new MouseEventHandler(dg_ForBrokersSystemMenuItem_Click);
                    myMenu.Items.Add(mySubMenu);
                    // Show the Menu
                    myMenu.Show(myLocation);
                }
            }
            catch { }


        } //dg_ForCustodians_CellMouseClick()

        public static void dg_ForCustodiansSystemMenuItem_Click(object sender, MouseEventArgs e)
        {
            //
            // Purpose: A generic menu click event for the Order Right-Click
            //

            // Local Variables
            ToolStripMenuItem ts_From = (ToolStripMenuItem)sender;
            TradeMenuStruct myTradeStr = (TradeMenuStruct)ts_From.Tag;
            String TradeID = myTradeStr.TradeID;
            ProcessTrades myForm = myTradeStr.myParentForm;

            if (TradeID.Length > 0)
            {
                String MySql = "Exec sp_ReverseTrade '" + TradeID + "'";
                SystemLibrary.SQLExecute(MySql);

                // TODO (1) Replace this message with code.
                // TODO (1) Replace this message with code.
                // TODO (1) Replace this message with code.
                MessageBox.Show("I need to Offer to inform the Broker, especially if this Trade has a ConfirmationNo from then");

                // Refresh this screen
                myForm.LoadForCustodians();
            }
        } //dg_ForBrokersSystemMenuItem_Click()

        private void dg_ForCustodians_MouseClick(object sender, MouseEventArgs e)
        {
            CXLocation = dg_ForCustodians.Location.X + e.Location.X + 5;
            CYLocation = dg_ForCustodians.Location.Y + tabControl1.Location.Y + tp_OrdersToTrades.Top + e.Location.Y;
        } //dg_ForCustodians_MouseClick()

        public void bt_RequestHistoricFiles_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;

            mySql = "Select 'Merrill Lynch' as Custodian, MLFileName as FileName, FileMethod, FileSent, Count(*) as Trades, isNull(FileConfirmed,'N') as FileConfirmed, MLTimeStamp as TimeStamp, MLClientBatchID " +
                    "From   ML_OUT " +
                    "Where  MLTimeStamp >= " + SystemLibrary.f_Now().AddMonths(-1).ToString("yyyyMMddHHmmss") + " " +
                    "Group By MLFileName, FileMethod, FileSent, isNull(FileConfirmed,'N'), MLTimeStamp, MLClientBatchID " +
                    "UNION " +
                    "Select 'SCOTIA' as Custodian, SCOTIAFileName as FileName, FileMethod, FileSent, Count(*) as Trades, isNull(FileConfirmed,'N') as FileConfirmed, SCOTIATimeStamp as TimeStamp, null " +
                    "From SCOTIA_Out " +
                    "Where  SCOTIATimeStamp >= " + SystemLibrary.f_Now().AddMonths(-1).ToString("yyyyMMddHHmmss") + " " +
                    "Group By SCOTIAFileName, FileMethod, FileSent, isNull(FileConfirmed,'N'), SCOTIATimeStamp " +
                    "Order By 2 Desc ";

            dt_CustodianFiles = SystemLibrary.SQLSelectToDataTable(mySql);

            if (dg_CustodianFiles.Rows.Count > 0)
                dg_CustodianFiles.Rows.Clear();
            for (int i = 0; i < dt_CustodianFiles.Rows.Count; i++)
            {
                int myRow = dg_CustodianFiles.Rows.Add(dt_CustodianFiles.Rows[i]["Custodian"], dt_CustodianFiles.Rows[i]["FileName"], dt_CustodianFiles.Rows[i]["FileMethod"],
                                                        dt_CustodianFiles.Rows[i]["FileSent"], dt_CustodianFiles.Rows[i]["Trades"],
                                                        dt_CustodianFiles.Rows[i]["FileConfirmed"], dt_CustodianFiles.Rows[i]["TimeStamp"],
                                                        dt_CustodianFiles.Rows[i]["MLClientBatchID"]
                                                      );
                if (SystemLibrary.YN_To_Bool(dt_CustodianFiles.Rows[i]["FileConfirmed"].ToString()))
                    dg_CustodianFiles.Rows[myRow].Cells["FileConfirmed"].Style.BackColor = Color.Green;
                else
                    dg_CustodianFiles.Rows[myRow].Cells["FileConfirmed"].Style.BackColor = Color.Salmon;
            }

        } //bt_RequestHistoricFiles_Click()

        private void dg_CustodianFiles_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Show the popup menu
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    Point myLocation = new Point(this.Location.X + CXLocation, this.Location.Y + CYLocation);
                    // Select the Order as needed in code later on.
                    String Custodian = SystemLibrary.ToString(dg_CustodianFiles.Rows[e.RowIndex].Cells["Custodian"].Value);
                    String myFileName = SystemLibrary.ToString(dg_CustodianFiles.Rows[e.RowIndex].Cells["FileName"].Value);
                    String myTimeStamp = SystemLibrary.ToString(dg_CustodianFiles.Rows[e.RowIndex].Cells["TimeStamp"].Value);
                    String MLNumberofRecords = SystemLibrary.ToString(dg_CustodianFiles.Rows[e.RowIndex].Cells["TradesSent"].Value);
                    String MLClientBatchID = SystemLibrary.ToString(dg_CustodianFiles.Rows[e.RowIndex].Cells["MLClientBatchID"].Value);
                    Boolean FileConfirmed = SystemLibrary.YN_To_Bool(SystemLibrary.ToString(dg_CustodianFiles.Rows[e.RowIndex].Cells["FileConfirmed"].Value));

                    ContextMenuStrip myMenu = new ContextMenuStrip();
                    ToolStripMenuItem mySubMenu = new ToolStripMenuItem();
                    String ToolStripText = "";

                    TradeMenuStruct myTradeStr = new TradeMenuStruct();
                    myTradeStr.Custodian = Custodian;
                    myTradeStr.FileName = myFileName;
                    myTradeStr.FileConfirmed = FileConfirmed;
                    myTradeStr.TimeStamp = myTimeStamp;
                    myTradeStr.MLNumberofRecords = MLNumberofRecords;
                    myTradeStr.MLClientBatchID = MLClientBatchID;
                    myTradeStr.myParentForm = this;

                    // Release back to Order Screen 
                    if (FileConfirmed)
                        ToolStripText = "Resend Confirmed File '" + myFileName + "'?";
                    else
                        ToolStripText = "Resend Unconfirmed File '" + myFileName + "'";
                    mySubMenu = new ToolStripMenuItem(ToolStripText);
                    mySubMenu.Tag = myTradeStr;
                    mySubMenu.MouseUp += new MouseEventHandler(dg_CustodianFilesSystemMenuItem_Click);
                    myMenu.Items.Add(mySubMenu);
                    // Show the Menu
                    myMenu.Show(myLocation);
                }
            }
            catch { }

        } //dg_CustodianFiles_CellMouseClick()

        public static void dg_CustodianFilesSystemMenuItem_Click(object sender, MouseEventArgs e)
        {
            //
            // Purpose: A generic menu click event for the Order Right-Click
            //

            // Local Variables
            ToolStripMenuItem ts_From = (ToolStripMenuItem)sender;
            TradeMenuStruct myTradeStr = (TradeMenuStruct)ts_From.Tag;
            String Custodian = myTradeStr.Custodian;
            String myFileName = myTradeStr.FileName;
            String myTimeStamp = myTradeStr.TimeStamp;
            String MLNumberofRecords = myTradeStr.MLNumberofRecords;
            String MLClientBatchID = myTradeStr.MLClientBatchID;
            ProcessTrades myForm = myTradeStr.myParentForm;
            StringBuilder sb_Out = new StringBuilder();
            String myMessage = "";
            String myFilePath = "";

            if (myFileName.Length > 0)
            {
                switch (Custodian.ToLower())
                {
                    case "merrill lynch":
                        {
                            // Resend the file (Code copied from bt_SendToCustodian_Click() 
                            String MLPrime_Path = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('MLPrime_Path')");
                            myFilePath = MLPrime_Path + @"\OutBound";
                            if (!System.IO.Directory.Exists(myFilePath))
                                myFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                            // hourglass cursor
                            Cursor.Current = Cursors.WaitCursor;

                            // Header Rows
                            sb_Out.AppendLine("H1,Mandatory,,,,,,,,,,,Conditionally Mandatory,,,,,,,,,,,,,,Optional,,,,,,,,,,,,,,,,,");
                            sb_Out.AppendLine("H2,TimeStamp:," + myTimeStamp + ",Client Batch Id:," + MLClientBatchID + ",Number of Records," + MLNumberofRecords + ",File Type,Trade, , ,,,,,,,, ,,,,,,,,,,,,,,,,,,,,,,,,,");
                            sb_Out.AppendLine("H3,Account Number,Record Type,Transaction Type ,Client Transaction ID,Trade Date,Client Product Id Type,Client Product Id,Price,Quantity,Settlement Date,Settlement Currency,Net Amount,Client Executing Broker,SEC Fee,Accrued Interest,Client Asset Type,Trading Unit,Trading Sub-Unit,Client Underlying Product Id,Client Underlying Product Id Type,Country of Trading,Expiration Date,Fee Type 1,Fee Amount 1,Fee Type 2,Fee Amount 2,Fee Type 3,Fee Amount 3,Fee Type 4,Fee Amount 4,Fee Type 5,Fee Amount 5,Option Type,Spot Date,Strike Price,Commission Amount ,Commission Rate,Commission Type,Prime Broker Type,Client Original Transaction Id,Client Block Id,Cost Basis FX Rate,Issue Currency");

                            // Build the data records
                            String mySql = "Select ','+Account_Number+','+Record_Type+','+Transaction_Type+','+Client_Transaction_ID+','+Convert(Varchar,Trade_Date,112)+','+ " +
                                           "		Client_Product_Id_Type+','+Client_Product_Id+','+Cast(Price as varchar)+','+Cast(Quantity as varchar)+','+Convert(Varchar,Settlement_Date,112)+','+ " +
                                           "		Settlement_Currency+','+Cast(Net_Amount as varchar)+','+Client_Executing_Broker+','+',,,,,,,,,,,,,,,,,,,,,,'+Cast(Commission_Amount as varchar) +','+ " +
                                           "		Cast(Commission_Rate as varchar)+','+',,'+Case When Record_Type = 'C' Then Client_Transaction_ID else '' end +',,,' as OutRow " +
                                           "From ML_OUT  " +
                                           "Where MLFileName = '" + myFileName + "' ";

                            DataTable dt_FetchRecords = SystemLibrary.SQLSelectToDataTable(mySql);

                            for (int i = 0; i < dt_FetchRecords.Rows.Count; i++)
                                sb_Out.AppendLine(dt_FetchRecords.Rows[i][0].ToString());

                            // Write the Data to file
                            File.Delete(myFilePath + @"\" + myFileName); // Should never need this.
                            File.WriteAllText(myFilePath + @"\" + myFileName, sb_Out.ToString());

                            // Drop the file into the MLPrime Web site.
                            if (myForm.FTPToPrime("Merrill Lynch", myFilePath, myFileName))
                            {
                                mySql = "Update ML_Out " +
                                        "Set    FileSent = dbo.f_GetDate(), " +
                                        "       FileMethod = 'FTP' " +
                                        "Where  MLFileName = '" + myFileName + "' ";
                                SystemLibrary.SQLExecute(mySql);

                                // Alter the LastUpdate for this user, so it searches for the confirm in 15 minutes
                                SystemLibrary.FTPMLPrimeVars.LastUpdate = SystemLibrary.f_Now().AddSeconds(-SystemLibrary.FTPMLPrimeVars.Interval_seconds + 900);

                                myMessage = "ML Prime file sent.\r\n\r\n";
                            }
                            else
                            {
                                // FTP Failed, so trying email
                                if (MessageBox.Show("FTP Failed to send the file to the Custodian.\r\n\r\nDo you want to send it via Email instead?", "Send File Failed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    if (myForm.EmailToPrime("Merrill Lynch", myFilePath + @"\" + myFileName))
                                    {
                                        mySql = "Update ML_Out " +
                                                "Set    FileSent = dbo.f_GetDate(), " +
                                                "       FileMethod = 'Email' " +
                                                "Where  MLFileName = '" + myFileName + "' ";
                                        SystemLibrary.SQLExecute(mySql);
                                        myMessage = "ML Prime File Failed to go directly to server, so have emailed instead.\r\n\r\n";
                                    }
                                    else
                                    {
                                        mySql = "Update ML_Out " +
                                                "Set    FileSent = dbo.f_GetDate(), " +
                                                "       FileMethod = 'Manual' " +
                                                "Where  MLFileName = '" + myFileName + "' ";
                                        SystemLibrary.SQLExecute(mySql);
                                        myMessage = "ML Prime File Failed to go directly to server. Email also failed. Extract file from saved location and process appropriately.\r\n\r\n";
                                    }
                                }
                            }
                        }
                        break;
                    case "scotia":
                        {
                            String Scotia_Path = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('SCOTIAPrime_Path')");
                            String Scotia_Prefix = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('SCOTIA_TRADES_Prefix')");
                            myFilePath = Scotia_Path + @"\OutBound";
                            if (!System.IO.Directory.Exists(myFilePath))
                                myFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                            // hourglass cursor
                            Cursor.Current = Cursors.WaitCursor;

                            // Header Rows
                            sb_Out.AppendLine(@"!SET_USER,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                            sb_Out.AppendLine(@"#! DEF INS_TRADE = OPF = TDP_LOADER.insert_trade_wrapper,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                            sb_Out.AppendLine(@"#! DEF INIT = OPP = TDP_LOADER.initialise_err_qty,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                            sb_Out.AppendLine(@"#! DEF END  = OPP = FIL.set_trade_upload_error_qty,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                            sb_Out.AppendLine(@",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                            sb_Out.AppendLine(@",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                            sb_Out.AppendLine(@"#initialization,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                            sb_Out.AppendLine(@"!INIT,0,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                            sb_Out.AppendLine(@",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                            sb_Out.AppendLine(@"# prefix,trade type,trade reference,fund,book,strategy,cpty,buy/sell,quantity,sec ident type,sec ident value,sec name,price,price divisor,traded net ind,trade ccy,trade dt,settle dt,commission,exchange fee,other fees,gross consid,net consid,sett ccy,trad/sett ccy xrt,trad/sett ccy xrt m/d ind,trad/inst ccy xrt,trad/inst ccy xrt m/d ind,prime brkr,pb acct,reason code,inst class,trd notes,pl/book ccy xrt,sett cond,logon,amendment,cont id,old ref,,,,,,,,,");
                            sb_Out.AppendLine(@"# !INS_TRADE,SEC,,<8 digit acct>,<acct>_BOOK,NONE,,""B, S, BC, SS"",,CUSIP or RIC,<cusip> <RIC>,,,,,,,,,,,,,,1 or xrate,M (or D),1 or xrate,M (or D),SCOTIA,<8 digit acct>,NOR,,,1 or xrate,*,,,,,,,,,,,,, ");

                            // Build the data records
                            String mySql = "Select ReportLine " +
                                           "From SCOTIA_OUT  " +
                                           "Where SCOTIAFileName = '" + myFileName + "' " +
                                           "Order by Client_Transaction_ID";

                            DataTable dt_FetchRecords = SystemLibrary.SQLSelectToDataTable(mySql);

                            for (int i = 0; i < dt_FetchRecords.Rows.Count; i++)
                                sb_Out.AppendLine(dt_FetchRecords.Rows[i][0].ToString());


                            // Footer Rows
                            sb_Out.AppendLine(",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                            sb_Out.AppendLine("#Check for errors,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                            sb_Out.AppendLine("!END,TRADE_UPLOAD_" + Scotia_Prefix + ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");


                            // Write the Data to file
                            File.Delete(myFilePath + @"\" + myFileName); // Should never need this.
                            File.WriteAllText(myFilePath + @"\" + myFileName, sb_Out.ToString());

                            // Drop the file into the Scotia Prime Web site.
                            Boolean FtpResult = myForm.FTPToPrime("SCOTIA", myFilePath, myFileName);
                            if (FtpResult)
                            {
                                mySql = "Update SCOTIA_Out " +
                                        "Set    FileSent = dbo.f_GetDate(), " +
                                        "       FileMethod = 'FTP' " +
                                        "Where  SCOTIAFileName = '" + myFileName + "' ";
                                SystemLibrary.SQLExecute(mySql);

                                // Alter the LastUpdate for this user, so it searches for the confirm in 15 minutes
                                SystemLibrary.FTPSCOTIAPrimeVars.LastUpdate = SystemLibrary.f_Now().AddSeconds(-SystemLibrary.FTPMLPrimeVars.Interval_seconds + 900);

                                myMessage = "Scotia file sent.\r\n\r\n";
                            }

                            if (!FtpResult || Custodian.ToLower() == "scotia")
                            {
                                // FTP Failed, so trying email
                                if (myForm.EmailToPrime("SCOTIA", myFilePath + @"\" + myFileName))
                                {
                                    mySql = "Update SCOTIA_Out " +
                                            "Set    FileSent = dbo.f_GetDate(), " +
                                            "       FileMethod = 'Email' " +
                                            "Where  SCOTIAFileName = '" + myFileName + "' ";
                                    SystemLibrary.SQLExecute(mySql);
                                    myMessage = "Scotia File Failed to go directly to server, so have emailed instead.\r\n\r\n";
                                }
                                else
                                {
                                    mySql = "Update SCOTIA_Out " +
                                            "Set    FileSent = dbo.f_GetDate(), " +
                                            "       FileMethod = 'Manual' " +
                                            "Where  SCOTIAFileName = '" + myFileName + "' ";
                                    SystemLibrary.SQLExecute(mySql);
                                    myMessage = "Scotia File Failed to go directly to server. Email also failed. Extract file from saved location and process appropriately.\r\n\r\n";
                                }
                            }
                        }
                        break;
                }

                // Refresh this screen
                myForm.bt_RequestHistoricFiles_Click(null, null);

                Cursor.Current = Cursors.Default;
                MessageBox.Show(myMessage + myFilePath + @"\" + myFileName,"Re-Send File to Custodian");
            }
        } //dg_CustodianFilesSystemMenuItem_Click()

        private void dg_CustodianFiles_MouseClick(object sender, MouseEventArgs e)
        {
            CXLocation = dg_ForCustodians.Location.X + e.Location.X + 5;
            CYLocation = dg_ForCustodians.Location.Y + tabControl1.Location.Y + tp_OrdersToTrades.Top + e.Location.Y;
        } //dg_CustodianFiles_MouseClick()

        private void bt_RequestHistoricTrades_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql = "Exec sp_TradeHistoryForReversing '" + dtp_FromDate.Value.ToString("dd-MMM-yyyy") + "', '" + dtp_ToDate.Value.ToString("dd-MMM-yyyy") + "'";

            dt_HistoricTrades = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_HistoricTrades.DataSource = dt_HistoricTrades;
            SystemLibrary.SetDataGridView(dg_HistoricTrades);

        } //bt_RequestHistoricTrades_Click()

        private void dg_HistoricTrades_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Show the popup menu
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    Point myLocation = new Point(this.Location.X + CXLocation, this.Location.Y + CYLocation);
                    // Select the Order as needed in code later on.
                    String TradeID = SystemLibrary.ToString(dg_HistoricTrades.Rows[e.RowIndex].Cells["TradeID"].Value);

                    ContextMenuStrip myMenu = new ContextMenuStrip();
                    ToolStripMenuItem mySubMenu = new ToolStripMenuItem();

                    TradeMenuStruct myTradeStr = new TradeMenuStruct();
                    myTradeStr.TradeID = TradeID;
                    myTradeStr.myParentForm = this;

                    // Release back to Order Screen 
                    mySubMenu = new ToolStripMenuItem("Reverse All Trades for this Order Back to Open Orders AND Prepare a Reversal Note for the Broker and/or Custodian.");
                    mySubMenu.Tag = myTradeStr;
                    mySubMenu.MouseUp += new MouseEventHandler(dg_ForBrokersSystemMenuItem_Click);
                    myMenu.Items.Add(mySubMenu);
                    // Show the Menu
                    myMenu.Show(myLocation);
                }
            }
            catch { }


        } //dg_dg_HistoricTrades_CellMouseClick()

        public static void dg_HistoricTradesSystemMenuItem_Click(object sender, MouseEventArgs e)
        {
            //
            // Purpose: A generic menu click event for the Order Right-Click
            //

            // Local Variables
            ToolStripMenuItem ts_From = (ToolStripMenuItem)sender;
            TradeMenuStruct myTradeStr = (TradeMenuStruct)ts_From.Tag;
            String TradeID = myTradeStr.TradeID;
            ProcessTrades myForm = myTradeStr.myParentForm;

            if (TradeID.Length > 0)
            {
                String MySql = "Exec sp_ReverseTrade '" + TradeID + "'";
                SystemLibrary.SQLExecute(MySql);

                // TODO (1) Replace this message with code.
                // TODO (1) Replace this message with code. -- This will also have informed the custodian via sp_ReverseTrade.
                // TODO (1) Replace this message with code.
                MessageBox.Show("I need to Offer to inform the Broker, especially if this Trade has a ConfirmationNo from then");

                // Refresh this screen
                myForm.LoadForCustodians();
            }
        } //dg_HistoricTradesSystemMenuItem_Click()

        private void dg_HistoricTrades_MouseClick(object sender, MouseEventArgs e)
        {
            CXLocation = dg_HistoricTrades.Location.X + e.Location.X + 5;
            CYLocation = dg_HistoricTrades.Location.Y + tabControl1.Location.Y + tp_OrdersToTrades.Top + e.Location.Y;

        } //dg_HistoricTrades_MouseClick()

    }
}
