using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace T1MultiAsset
{
    public partial class ReportTrade : Form
    {
        // Public Variables
        public Form1 ParentForm1;
        DataTable dt_ReportTrade;
        DataTable dt_Fund;
        DataTable dt_Portfolio;
        DataTable dt_Broker;

        int     FundID = -1;
        String  Fund_Name = "";
        int     PortfolioID = -1;
        String  Portfolio_Name = "";
        String  BBG_Ticker = "";
        int     BrokerID = -1;
        String Broker_Name = "";
        DateTime FromDate = DateTime.MinValue;
        DateTime ToDate = DateTime.MaxValue;

        public struct TradeMenuStruct
        {
            public ReportTrade myParentForm;
            public String Instruction;
            public String TradeID;
            public String NewSide;
        }

        private int CXLocation = 0;
        private int CYLocation = 0;


        public ReportTrade()
        {
            // Local Variables
            String mySql;

            InitializeComponent();

            // RULE: Need to have this data before FromParent() is called.
            mySql = "Select Max(TradeDate) from Trade Where TradeDate >dbo.f_GetDate()-30";
            dtp_FromDate.Value = SystemLibrary.SQLSelectDateTime(mySql, SystemLibrary.f_Today().AddDays(-1));
            dtp_ToDate.Value = SystemLibrary.f_Today();
            // Load the Drop down boxes
            mySql = "Select FundID, FundName, ShortName, CreatedDate " +
                    "From   Fund " +
                    "Where  Active = 'Y' " +
                    "And    AllowTrade = 'Y' " +
                    "Order by FundName";
            dt_Fund = SystemLibrary.SQLSelectToDataTable(mySql);
            cb_Fund.DataSource = dt_Fund;
            cb_Fund.DisplayMember = "FundName";
            cb_Fund.ValueMember = "FundId";
            if (dt_Fund.Rows.Count>0)
                cb_Fund.SelectedIndex = 0;

            mySql = "Select PortfolioID, PortfolioName, CreatedDate, ClosedDate, Active " +
                    "From   Portfolio " +
                    "Order by PortfolioName";
            dt_Portfolio = SystemLibrary.SQLSelectToDataTable(mySql);
            cb_Portfolio.DataSource = dt_Portfolio;
            cb_Portfolio.DisplayMember = "PortfolioName";
            cb_Portfolio.ValueMember = "PortfolioId";
            if (dt_Portfolio.Rows.Count > 0)
                cb_Portfolio.SelectedIndex = 0;

            mySql = "Select BrokerID, BrokerName " +
                    "From   Broker " +
                    "Order by BrokerName";
            dt_Broker = SystemLibrary.SQLSelectToDataTable(mySql);
            cb_Broker.DataSource = dt_Broker;
            cb_Broker.DisplayMember = "BrokerName";
            cb_Broker.ValueMember = "BrokerId";
            if (dt_Broker.Rows.Count > 0)
                cb_Broker.SelectedIndex = 0;


        } //ReportTrade()

        public void FromParent(Form inForm, int inFundID, int inPortfolioID, String inBBG_Ticker, object inFromDate, object inToDate, int inBrokerID)
        {
            ParentForm1 = (Form1)inForm;

            // Set the Global variables
            FundID = inFundID;
            PortfolioID = inPortfolioID;
            BBG_Ticker = inBBG_Ticker;
            BrokerID = inBrokerID;

            if (inFromDate != null)
            {
                FromDate = (DateTime)inFromDate;
                cb_hasFromDate.Checked = true;
                dtp_FromDate.Value = FromDate;
            }
            if (inToDate != null)
            {
                ToDate = (DateTime)inToDate;
                cb_hasToDate.Checked = true;
                dtp_ToDate.Value = FromDate;
            }

            // Set up the boxes
            if (FundID == -1)
                cb_hasFundID.Checked = false;
            else
            {
                cb_Fund.SelectedValue = FundID;
                cb_hasFundID.Checked = true;
            }

            if (PortfolioID == -1)
                cb_hasPortfolioID.Checked = false;
            else
            {
                cb_Portfolio.SelectedValue = PortfolioID;
                cb_hasPortfolioID.Checked = true;
            }

            if (BBG_Ticker == "")
                cb_hasBBG_Ticker.Checked = false;
            else
            {
                tb_BBG_Ticker.Text = BBG_Ticker;
                cb_hasBBG_Ticker.Checked = true;
            }

            if (BrokerID == -1)
                cb_hasBrokerID.Checked = false;
            else
            {
                cb_Broker.SelectedValue = BrokerID;
                cb_hasBrokerID.Checked = true;
            }

            LoadTrades();

            //ParentForm1.CopyDataTable(ref dt_Fund, ref dt_Portfolio);
        } //FromParent()


        private void ReportTrade_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            FormatTrades();
        } //ReportTrade_Load

        private void cb_Fund_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FundID = Convert.ToInt16(((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[0].ToString());
            Fund_Name = ((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[1].ToString();
            //Fund_Amount = Convert.ToDecimal(((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[2].ToString());
            //Fund_Crncy = ((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[3].ToString();
            //LoadTrades();

        } // cb_Fund_SelectionChangeCommitted()

        private void cb_Portfolio_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PortfolioID = Convert.ToInt16(((DataRowView)(cb_Portfolio.SelectedItem)).Row.ItemArray[0].ToString());
            Portfolio_Name = ((DataRowView)(cb_Portfolio.SelectedItem)).Row.ItemArray[1].ToString();
            //Portfolio_Amount = Convert.ToDecimal(((DataRowView)(cb_Portfolio.SelectedItem)).Row.ItemArray[2].ToString());
            //Portfolio_Crncy = ((DataRowView)(cb_Portfolio.SelectedItem)).Row.ItemArray[3].ToString();
            //LoadTrades();

        } // cb_Portfolio_SelectionChangeCommitted()

        private void cb_Broker_SelectionChangeCommitted(object sender, EventArgs e)
        {
            BrokerID = Convert.ToInt16(((DataRowView)(cb_Broker.SelectedItem)).Row.ItemArray[0].ToString());
            Broker_Name = ((DataRowView)(cb_Broker.SelectedItem)).Row.ItemArray[1].ToString();
            //LoadTrades();

        } // cb_Broker_SelectionChangeCommitted()

        private void LoadTrades()
        {
            /*
             * Trade report
             */

            // Local Variables
            String mySql;

            mySql = "Select	FundName, PortfolioName, TradeDate, BBG_Ticker, Side, Quantity, Price, NetValue, " +
                    "       Case When isNull(NAV_SOD + Capital_SOD,0) = 0 Then null Else NetValue / (NAV_SOD + Capital_SOD) End as [% FUM], " +
                    "       BrokerName, SettlementDate, UpdateDate, TradeID " +
                    "from	Trade, " +
                    "		Fund, " +
                    "		Portfolio, " +
                    "		Broker, " +
                    "		FundNAV " +
                    "Where	Fund.FundID = Trade.FundID " +
                    "And	Fund.Active = 'Y' " +
                    "And	Portfolio.PortfolioID = Trade.PortfolioID " +
                    "And	FundNAV.FundID = Trade.FundID " +
                    "And      FundNAV.EffectiveDate = (	Select max(b.EffectiveDate) " +
                    "									From	FundNAV b " +
                    "									Where	b.EffectiveDate Between Trade.TradeDate -90 and Trade.TradeDate " +
                    "								  ) " +
                    "And	Broker.BrokerID = Trade.BrokerID ";
            if (cb_hasFundID.Checked)
            {
                mySql = mySql +
                        "And		Trade.FundID = " + FundID.ToString() + " ";
            }
            if (cb_hasPortfolioID.Checked)
            {
                mySql = mySql +
                        "And		Trade.PortfolioID = " + PortfolioID.ToString() + " ";
            }
            if (cb_hasBrokerID.Checked)
            {
                mySql = mySql +
                        "And		Trade.BrokerID = " + BrokerID.ToString() + " ";
            }
            if (cb_hasBBG_Ticker.Checked && tb_BBG_Ticker.Text.Trim().Length > 0)
            {
                mySql = mySql +
                        "And		Trade.BBG_Ticker = '" + tb_BBG_Ticker.Text + "' ";
            }
            if (cb_hasFromDate.Checked)
            {
                mySql = mySql +
                        "And		Trade.TradeDate >= '" + dtp_FromDate.Value.ToString("dd-MMM-yyyy") + "' ";
            }
            if (cb_hasToDate.Checked)
            {
                mySql = mySql +
                        "And		Trade.TradeDate <= '" + dtp_ToDate.Value.ToString("dd-MMM-yyyy") + "' ";
            }

            mySql = mySql +
                    "Order by TradeDate, BBG_Ticker, Side, FundName, PortfolioName, UpdateDate ";

            dt_ReportTrade = SystemLibrary.SQLSelectToDataTable(mySql);

            dg_ReportTrade.DataSource = dt_ReportTrade;

            // Scroll to the last row
            if (dg_ReportTrade.Rows.Count>0)
                dg_ReportTrade.CurrentCell = dg_ReportTrade[0, dg_ReportTrade.Rows.Count - 1];

            FormatTrades();

        } //LoadTrades()

        private void FormatTrades()
        {
            ParentForm1.SetFormatColumn(dg_ReportTrade, "FundName", Color.RoyalBlue, Color.Gainsboro, "", "");
            ParentForm1.SetFormatColumn(dg_ReportTrade, "BBG_Ticker", Color.RoyalBlue, Color.Gainsboro, "", "");
            ParentForm1.SetFormatColumn(dg_ReportTrade, "Quantity", Color.Empty, Color.Empty, "N0", "0");
            ParentForm1.SetFormatColumn(dg_ReportTrade, "Price", Color.Blue, Color.Empty, "N4", "0");
            ParentForm1.SetFormatColumn(dg_ReportTrade, "NetValue", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dg_ReportTrade, @"% FUM", Color.Empty, Color.Empty, "0.0%", "0");
            ParentForm1.SetFormatColumn(dg_ReportTrade, "TradeDate", Color.DarkBlue, Color.Empty, "dd-MMM-yy", "");
            ParentForm1.SetFormatColumn(dg_ReportTrade, "SettlementDate", Color.DarkBlue, Color.Gainsboro, "dd-MMM-yy", "");
            ParentForm1.SetFormatColumn(dg_ReportTrade, "UpdateDate", Color.Empty, Color.Empty, "dd-MMM-yy HH:mm", "");

            dg_ReportTrade.Columns["TradeID"].Visible = false;

            // Loop over the Tickers 
            for (Int32 i = 0; i < dg_ReportTrade.Rows.Count; i++) // Last row in dg_ReportTrade is a blank row
            {
                if (dg_ReportTrade["BBG_Ticker", i].Value != null)
                {
                    if (dg_ReportTrade["Side", i].Value.ToString().StartsWith("B"))
                        dg_ReportTrade["Side", i].Style.ForeColor = Color.Green;
                    else
                        dg_ReportTrade["Side", i].Style.ForeColor = Color.Red;
                    if (dg_ReportTrade["Side", i].Value.ToString().Length == 2)
                        dg_ReportTrade["Side", i].Style.BackColor = Color.LightPink;
                    ParentForm1.SetColumn(dg_ReportTrade, "Quantity", i);
                    ParentForm1.SetColumn(dg_ReportTrade, "NetValue", i);
                    ParentForm1.SetColumn(dg_ReportTrade, @"% FUM", i);
                }
            }

            // Loop on all columns and set the Autosize mode & Header Text
            for (int i = 0; i < dg_ReportTrade.Columns.Count; i++)
            {
                //dg_Orders.Columns[i].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
                dg_ReportTrade.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                Console.WriteLine(dg_ReportTrade.Columns[i].ValueType.Name + "," + dg_ReportTrade.Columns[i].Name);
                // dg_ReportTrade.Columns[i].CellType.IsValueType
                switch (dg_ReportTrade.Columns[i].ValueType.Name.ToUpper())
                {
                    case "STRING":
                    case "DATETIME":
                        break;
                    default:
                        // Right align non-String & non-date columns.
                        dg_ReportTrade.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        break;
                }
            }

        } //FormatTrades()

        private void bt_Request_Click(object sender, EventArgs e)
        {
            LoadTrades();

        } //bt_Request_Click()

        private void cb_hasFundID_CheckedChanged(object sender, EventArgs e)
        {
            cb_Fund.Enabled = cb_hasFundID.Checked;
        }

        private void cb_hasPortfolioID_CheckedChanged(object sender, EventArgs e)
        {
            cb_Portfolio.Enabled = cb_hasPortfolioID.Checked;
        }

        private void cb_hasBBG_Ticker_CheckedChanged(object sender, EventArgs e)
        {
            tb_BBG_Ticker.Enabled = cb_hasBBG_Ticker.Checked;
        }

        private void cb_hasBrokerID_CheckedChanged(object sender, EventArgs e)
        {
            cb_Broker.Enabled = cb_hasBrokerID.Checked;
        }

        private void cb_hasFromDate_CheckedChanged(object sender, EventArgs e)
        {
            dtp_FromDate.Enabled = cb_hasFromDate.Checked;
        }

        private void cb_hasToDate_CheckedChanged(object sender, EventArgs e)
        {
            dtp_ToDate.Enabled = cb_hasToDate.Checked;
        }

        private void dg_ReportTrade_Sorted(object sender, EventArgs e)
        {
            FormatTrades();
        }

        private void ReportTrade_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);
        }

        private void dg_ReportTrade_MouseClick(object sender, MouseEventArgs e)
        {
            CXLocation = dg_ReportTrade.Location.X + e.Location.X + 5;
            CYLocation = dg_ReportTrade.Location.Y + e.Location.Y;
        } //dg_ReportTrade_MouseClick()

        private void dg_ReportTrade_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
           // Show the popup menu
            try
            {
                /*
                 * WARNING: This block does not deal with Stock Location rebuild, so have restricted access to the developer.
                 *          It was temporarily added 26-Feb-2014 to help a new client who was working in EMSX only for Orders,
                 *          and thus missing whether an order direction was Long or Short.
                 */
                if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1 && SystemLibrary.GetUserName() == "Colin Ritchie")
                {
                    Point myLocation = new Point(this.Location.X + CXLocation, this.Location.Y + CYLocation);
                    ContextMenuStrip myMenu = new ContextMenuStrip();
                    ToolStripMenuItem mySubMenu = new ToolStripMenuItem();

                    String TradeID = SystemLibrary.ToString(dg_ReportTrade.Rows[e.RowIndex].Cells["TradeID"].Value);
                    String Side = SystemLibrary.ToString(dg_ReportTrade.Rows[e.RowIndex].Cells["Side"].Value);
                    String NewSide = Side;

                    // Swap between Short & Long Order Menu
                    String MenuText = "";
                    switch (Side)
                    {
                        case "B":
                            MenuText = "Swap from 'Buy' to 'Buy to Cover'";
                            NewSide = "BS";
                            break;
                        case "BS":
                            MenuText = "Swap from 'Buy to Cover' to 'Buy'";
                            NewSide = "B";
                            break;
                        case "S":
                            MenuText = "Swap from 'Sell' to 'Sell Short'";
                            NewSide = "SS";
                            break;
                        case "SS":
                            MenuText = "Swap from 'Sell Short' to 'Sell'";
                            NewSide = "S";
                            break;
                        default:
                            Console.WriteLine("Programmer issue: dg_Orders_CellMouseClick(Unknown Side='" + Side + "'");
                            MenuText = "";
                            break;
                    }
                    if (MenuText.Length > 0)
                    {
                        TradeMenuStruct myTradeStr = new TradeMenuStruct();
                        myTradeStr.TradeID = TradeID;
                        myTradeStr.NewSide = NewSide;
                        myTradeStr.myParentForm = this;

                        mySubMenu = new ToolStripMenuItem(MenuText);
                        myTradeStr.Instruction = "Swap Trade";
                        mySubMenu.Tag = myTradeStr;
                        mySubMenu.MouseUp += new MouseEventHandler(dg_ReportTradeMenuItem_Click);
                        myMenu.Items.Add(mySubMenu);
                    }

                    // Show the Menu
                    myMenu.Show(myLocation);


                
                }
            }
            catch { }

        } //dg_ReportTrade_CellMouseClick()

        public static void dg_ReportTradeMenuItem_Click(object sender, MouseEventArgs e)
        {
            //
            // Purpose: A generic menu click event for the Order Right-Click
            //

            // Local Variables
            ToolStripMenuItem ts_From = (ToolStripMenuItem)sender;
            TradeMenuStruct myTradeStr = (TradeMenuStruct)ts_From.Tag;

            String Tradeid = myTradeStr.TradeID;
            String NewSide = myTradeStr.NewSide;
            ReportTrade myForm = myTradeStr.myParentForm;

            String mySql;

            // Alter the Trade Record
            mySql = "Update Trade Set Side = '" + NewSide + "' Where TradeID = " + Tradeid;
            int myRows = SystemLibrary.SQLExecute(mySql);
            mySql = "Update Fills Set Side = '" + NewSide + "' Where OrderRefID = (Select OrderRefID from Fills_Allocation Where TradeID = " + Tradeid + ") ";
            myRows = SystemLibrary.SQLExecute(mySql);
            mySql = "Update Orders Set Side = '" + NewSide + "' Where OrderRefID = (Select OrderRefID from Fills_Allocation Where TradeID = " + Tradeid + ") ";
            myRows = SystemLibrary.SQLExecute(mySql);

            myForm.LoadTrades();

        } //dg_ReportTradeMenuItem_Click()

    }
}
