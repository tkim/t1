using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;


namespace T1MultiAsset
{
    public partial class ProcessOrders : Form
    {
        // Public Variables
        public Form1 ParentForm1;
        public DataView dv_Orders;
        public DataTable dt_Orders;
        public DataTable dt_OrdersSplit;
        public DataTable dt_Fills;
        public DataTable dt_Fills_allocation;
        public DataTable dt_Fund;
        public DataTable dt_Portfolio;
        public DataTable dt_Broker;
        public object LastValue;
        public Boolean NeedUpdate = false;
        public Boolean NeedFullUpdate = false;
        private int CXLocation = 0;
        private int CYLocation = 0;
        private String Last_cb_Portfolio_Text = "";

        public struct OrderMenuStruct
        {
            public String Instruction;
            public String OrderRefID;
            public String EMSX_Sequence;
            public String BBG_Ticker;
            public String Side;
            public String FormattedQuantity;
            public String FormattedFillAmount;
            public String FormattedRoutedAmount;
            public Decimal Quantity;
            public Decimal FillAmount;
            public Decimal FillPrice;
            public Decimal RoutedAmount;
            public ProcessOrders myParentForm;
        }


        public struct OrderStruct
        {
            public String OrderRefID;
            public String BBG_Ticker;
            public String Side;
            public String crncy;
            public String Country;
            public DateTime TradeDate;
            public Int32 Quantity;
            public Int32 Round_Lot_Size;
            public Boolean ProcessedEOD;
            public Boolean ChangeMade;
            public Boolean ChangeMadeFill;
        }

        public static OrderStruct Order = new OrderStruct();

        public struct FillStruct
        {
            public String OrderRefID;
            public String FillNo;
            public String Side;
            public DateTime TradeDate;
            public Int32 Quantity;
            public Int32 Round_Lot_Size;
            public Double FillPrice;
            public Boolean ChangeMade;
            public String BBG_Broker;
            public String BBG_StrategyType;
            public Int32 RoutedAmount;
        }

        public static FillStruct Fills_parent = new FillStruct();



        public ProcessOrders()
        {
            InitializeComponent();
            Order.ChangeMade = false;
            Order.ChangeMadeFill = false;
        }

        private void ProcessOrders_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            // Swap back & forth from Hourglass so user knows
            Cursor.Current = Cursors.WaitCursor;
            LoadBroker();
            LoadProcessOrders();
            Cursor.Current = Cursors.Default; 

        } //ProcessOrders_Load()

        private void pb_Refresh_Click(object sender, EventArgs e)
        {
            ProcessOrders_Load(null, null);
        } //pb_Refresh_Click()



        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;
            ParentForm1.CopyDataTable(ref dt_Fund, ref dt_Portfolio);

            // 14-Nov-2011 Found the ParentForm could have a different Portfolio set than needed for settlement.
            String mySql = "Select PortfolioId, PortfolioName, PortfolioAmount, crncy " +
                           "From   Portfolio " +
                           "Where  Active = 'Y' " +
                           "Order By 2 ";
            dt_Portfolio = SystemLibrary.SQLSelectToDataTable(mySql);

            // 1-Nov-2012 After Parent changes, now get the set needed for settlement.
            mySql = "Select FundId, FundName, FundAmount, crncy, ShortName " +
                    "From   Fund " +
                    "Where  Active = 'Y' " +
                    "And   AllowTrade = 'Y' " +
                    "Order By 2 ";
            dt_Fund = SystemLibrary.SQLSelectToDataTable(mySql);

        } //FromParent()

        private void ProcessOrders_FormClosed(object sender, FormClosedEventArgs e)
        {
            // See if need to Update Positions to reflect the changes.
             SystemLibrary.DebugLine("ProcessOrders_FormClosed: Start");
            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;
            if (NeedFullUpdate)
            {
                /* 
                 * Needed in cases where Delete's occur otherwise the Positions Table will not force sp_Positions to refresh properly for other users.
                 */
                SystemLibrary.SQLExecute("Exec sp_SOD_Positions ");
                SystemLibrary.DebugLine("ProcessOrders_FormClosed: Post sp_SOD_Positions ");
                if (ParentForm1 != null)
                    ParentForm1.LoadPortfolioIncr();
                SystemLibrary.DebugLine("ProcessOrders_FormClosed: Post LoadPortfolioIncr() ");
            }
            else if (NeedUpdate)
            {
                SystemLibrary.SQLExecute("Exec sp_Update_Positions 'Y' ");
                SystemLibrary.DebugLine("ProcessOrders_FormClosed: Post sp_Update_Positions ");
                if (ParentForm1 != null)
                    ParentForm1.LoadPortfolioIncr();
                SystemLibrary.DebugLine("ProcessOrders_FormClosed: Post LoadPortfolioIncr() ");
            }

            // Let the Parent know this has close
            if (ParentForm1 != null)
                ParentForm1.LoadActionTab(true);
            SystemLibrary.DebugLine("ProcessOrders_FormClosed: Post LoadActionTab() ");
            
            Cursor.Current = Cursors.Default;
            SystemLibrary.DebugLine("ProcessOrders_FormClosed: End");

        } //ProcessOrders_FormClosed()

        private void LoadProcessOrders()
        {
            // Local Variables
            String mySql;

            // Reset all the other DataGridViews
            // TODO (1) - UP TO HERE:       Reset all the other DataGridViews

            // Get the Data
            mySql = "exec sp_LoadProcessOrders '" + SystemLibrary.Bool_To_YN(cb_ShowProcessed.Checked) + "'";
            dt_Orders = SystemLibrary.SQLSelectToDataTable(mySql);
            if (dt_Orders.Columns.Count < 1)
                return;

            //dg_Orders.AutoGenerateColumns = false;
            dv_Orders = dt_Orders.DefaultView;
            dg_Orders.DataSource = dv_Orders;

            // Hide Reference columns
            dg_Orders.Columns["OrderRefId"].Visible = false;
            dg_Orders.Columns["Round_Lot_Size"].Visible = false;
            dg_Orders.Columns["Exchange"].Visible = false;
            dg_Orders.Columns["Sector"].Visible = false;
            // -- Hide some columns until I get back to them
            dg_Orders.Columns["PM"].Visible = false;
            dg_Orders.Columns["IdeaOwner"].Visible = false;
            dg_Orders.Columns["Strategy1"].Visible = false;
            dg_Orders.Columns["Strategy2"].Visible = false;
            dg_Orders.Columns["Strategy3"].Visible = false;
            dg_Orders.Columns["Strategy4"].Visible = false;
            dg_Orders.Columns["Strategy5"].Visible = false;
            dg_Orders.Columns["EffectiveDate"].HeaderText = "Date";
            dg_Orders.Columns["EffectiveDate"].DefaultCellStyle.Format = "D"; // M = dd-Mon, D = Short Date/Time, F = Long Date/Time
            dg_Orders.Columns["EffectiveDate"].DefaultCellStyle.ForeColor = Color.Blue;
            dg_Orders.Columns["EffectiveDate"].Width = 150;
            dg_Orders.Columns["EffectiveDate"].MinimumWidth = 150;
            dg_Orders.Columns["CreatedDate"].HeaderText = "Created";
            dg_Orders.Columns["CreatedDate"].DefaultCellStyle.Format = "t"; // t = Short Time, T = Long Time
            dg_Orders.Columns["CreatedDate"].DefaultCellStyle.ForeColor = Color.Blue;
            //dg_Orders.Columns["CreatedDate"].Width = 150;
            //dg_Orders.Columns["CreatedDate"].MinimumWidth = 150;
            ParentForm1.SetFormatColumn(dg_Orders, "Quantity", Color.Empty, Color.Empty, "N0", "0");
            ParentForm1.SetFormatColumn(dg_Orders, "Amount", Color.Empty, Color.Empty, "N0", "0");
            ParentForm1.SetFormatColumn(dg_Orders, "RoutedAmount", Color.Empty, Color.Empty, "N0", "0");
            ParentForm1.SetFormatColumn(dg_Orders, "FillAmount", Color.Empty, Color.Empty, "N0", "0");
            ParentForm1.SetFormatColumn(dg_Orders, "FillPrice", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dg_Orders, "Portfolio", Color.Blue, Color.LightCyan, "", "Multi");
            //dg_Orders.Columns["Portfolio"].DefaultCellStyle.ForeColor = Color.Blue;
            // TODO (1) Make ProcessedEOD checkBox / ManualOrder?
            dg_Orders.Columns.Remove("Order_Completed");
            DataGridViewCheckBoxColumn Order_Completed = new DataGridViewCheckBoxColumn();
            Order_Completed.HeaderText = "Complete Order";
            Order_Completed.FalseValue = "N";
            Order_Completed.TrueValue = "Y";
            Order_Completed.Name = "Order_Completed";
            Order_Completed.DataPropertyName = "Order_Completed";
            dg_Orders.Columns.Insert(0, Order_Completed);
            dg_Orders.Columns["ManualOrder"].HeaderText = "Manual";
            dg_Orders.Columns["ProcessedEOD"].HeaderText = "Processed EOD";

            // Colin
            if (dg_Orders.Columns.Contains("Order_Filled"))
                dg_Orders.Columns.Remove("Order_Filled");
            DataGridViewCheckBoxColumn Order_Filled = new DataGridViewCheckBoxColumn();
            Order_Filled.HeaderText = "Filled";
            Order_Filled.FalseValue = "N";
            Order_Filled.TrueValue = "Y";
            Order_Filled.Name = "Order_Filled";
            //Order_Completed.DataPropertyName = "Order_Filled";
            dg_Orders.Columns.Add(Order_Filled);
            // Colin
            if (dg_Orders.Columns.Contains("Order_Filled_PCT"))
                dg_Orders.Columns.Remove("Order_Filled_PCT");
            DataGridViewTextBoxColumn Order_Filled_PCT = new DataGridViewTextBoxColumn();
            Order_Filled_PCT.HeaderText = @"%";
            Order_Filled_PCT.DefaultCellStyle.Format = @"0.0%";
            Order_Filled_PCT.Name = "Order_Filled_PCT";
            dg_Orders.Columns.Add(Order_Filled_PCT);
            // Routed Filled
            if (dg_Orders.Columns.Contains("Routed_Filled_PCT"))
                dg_Orders.Columns.Remove("Routed_Filled_PCT");
            DataGridViewTextBoxColumn Routed_Filled_PCT = new DataGridViewTextBoxColumn();
            Routed_Filled_PCT.HeaderText = @"% Rtd";
            Routed_Filled_PCT.DefaultCellStyle.Format = @"0.0%";
            Routed_Filled_PCT.Name = "Routed_Filled_PCT";
            dg_Orders.Columns.Add(Routed_Filled_PCT);

            SetFormat(true);

            // Loop on all columns and set the Autosize mode & Header Text
            for (int i = 0; i < dg_Orders.Columns.Count; i++)
            {
                //dg_Orders.Columns[i].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
                dg_Orders.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dg_Orders.Columns[i].HeaderText = dg_Orders.Columns[i].HeaderText.Replace('_', ' ');
                dg_Orders.Columns[i].ReadOnly = true;
            }
            dg_Orders.Columns["Amount"].HeaderText = "Bloomberg Amount";
            dg_Orders.Columns["RoutedAmount"].HeaderText = "Routed Amount";
            dg_Orders.Columns["FillAmount"].HeaderText = "Fill Amount";
            dg_Orders.Columns["FillPrice"].HeaderText = "Fill Price";
            dg_Orders.Columns["Amount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_Orders.Columns["Amount"].Width = dg_Orders.Columns["Quantity"].Width;
            dg_Orders.Columns["RoutedAmount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_Orders.Columns["RoutedAmount"].Width = dg_Orders.Columns["Quantity"].Width;
            dg_Orders.Columns["FillAmount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_Orders.Columns["FillAmount"].Width = dg_Orders.Columns["Quantity"].Width;

            dg_Orders.Columns["Order_Completed"].ReadOnly = false;
            dg_Orders.Columns["Order_Completed"].DefaultCellStyle.BackColor = Color.LightCyan;
            dg_Orders.Columns["Order_Completed"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_Orders.Columns["Order_Completed"].Width = 60;
            //colin
            //dg_Orders.Columns["Order_Filled"].ReadOnly = false;
            //dg_Orders.Columns["Order_Filled"].DefaultCellStyle.BackColor = Color.LightCyan;
            //dg_Orders.Columns["Order_Filled"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //dg_Orders.Columns["Order_Filled"].Width = 60;
            //colin
            dg_Orders.Columns["ManualOrder"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_Orders.Columns["ManualOrder"].Width = 60;
            dg_Orders.Columns["ProcessedEOD"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_Orders.Columns["ProcessedEOD"].Width = 60;
            dg_Orders.Columns["Side"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_Orders.Columns["Side"].Width = 30;
            dg_Orders.Columns["crncy"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_Orders.Columns["crncy"].Width = 45;

            tb_TotalQuantity.Text = "0";
            dg_OrdersSplit.Enabled = false;
            bt_SaveSplit.Enabled = false;
            dg_Orders.Focus();

            // Clean up child forms
            dg_OrdersSplit.Rows.Clear();
            dg_Fills.DataSource = null;
            dg_Fills.Rows.Clear();
            dg_Fills_Allocation.Rows.Clear();

            bt_Split_Report.Visible = false;

            // Deal with Currency display
            for (int i = 0; i < dg_Orders.Rows.Count; i++)
            {
                if (SystemLibrary.ToString(dg_Orders.Rows[i].Cells["Sector"].Value) == "Currency")
                {
                    dg_Orders.Rows[i].Cells["Quantity"].Style.Format = "N2";
                    dg_Orders.Rows[i].Cells["Amount"].Style.Format = "N2";
                    dg_Orders.Rows[i].Cells["RoutedAmount"].Style.Format = "N2";
                    dg_Orders.Rows[i].Cells["FillAmount"].Style.Format = "N2";
                }
            }

            if (Last_cb_Portfolio_Text != "" && Last_cb_Portfolio_Text != cb_Portfolio.Text)
            {
                cb_Portfolio.Text = Last_cb_Portfolio_Text;
                cb_Portfolio_SelectedIndexChanged(null, null);
            }
        } //LoadProcessOrders()

        private void SetFormat(Boolean FromLoad)
        {
            this.SuspendLayout();
            if (FromLoad)
            {
                cb_Portfolio.Items.Clear();
                cb_Portfolio.Items.Add("<All>");
                cb_Portfolio.Text = "<All>";
            }
            for (Int32 i = 0; i < dg_Orders.Rows.Count; i++)
            {
                // Local Variables
                String Portfolio = SystemLibrary.ToString(dg_Orders.Rows[i].Cells["Portfolio"].Value);
                int FillAmount = SystemLibrary.ToInt32(dg_Orders.Rows[i].Cells["FillAmount"].Value);
                int RoutedAmount = SystemLibrary.ToInt32(dg_Orders.Rows[i].Cells["RoutedAmount"].Value);
                int Quantity = SystemLibrary.ToInt32(dg_Orders.Rows[i].Cells["Quantity"].Value);

                ParentForm1.SetColumn(dg_Orders, "Quantity", i);
                ParentForm1.SetColumn(dg_Orders, "Amount", i);
                ParentForm1.SetColumn(dg_Orders, "RoutedAmount", i);
                ParentForm1.SetColumn(dg_Orders, "FillAmount", i);

                if (Quantity == 0)
                    dg_Orders.Rows[i].Cells["Order_Filled_PCT"].Value = 0;
                else
                    dg_Orders.Rows[i].Cells["Order_Filled_PCT"].Value = Convert.ToDecimal(FillAmount) / Convert.ToDecimal(Quantity);

                if (FillAmount == Quantity)
                {
                    dg_Orders.Rows[i].Cells["Order_Filled"].Value = "Y";
                    dg_Orders.Rows[i].Cells["Order_Filled_PCT"].Style.BackColor = Color.Green;
                    dg_Orders.Rows[i].Cells["Order_Filled_PCT"].Style.ForeColor = Color.White;
                }
                else
                    dg_Orders.Rows[i].Cells["Order_Filled"].Value = "N";


                if (RoutedAmount == 0 && FillAmount == 0)
                    dg_Orders.Rows[i].Cells["Routed_Filled_PCT"].Value = 1;
                else if (RoutedAmount == 0)
                    dg_Orders.Rows[i].Cells["Routed_Filled_PCT"].Value = 0;
                else
                    dg_Orders.Rows[i].Cells["Routed_Filled_PCT"].Value = Convert.ToDecimal(FillAmount) / Convert.ToDecimal(RoutedAmount);

                if (FillAmount != RoutedAmount)
                {
                    dg_Orders.Rows[i].Cells["Routed_Filled_PCT"].Style.BackColor = Color.LightPink;
                    //dg_Orders.Rows[i].Cells["Order_Filled_PCT"].Style.ForeColor = Color.White;
                }
                else
                    dg_Orders.Rows[i].Cells["Order_Filled"].Value = "N";

                if (SystemLibrary.ToString(dg_Orders.Rows[i].Cells["Order_Filled"].Value) == "Y")
                    dg_Orders.Rows[i].Cells["Order_Filled"].Style.BackColor = Color.Green;
                else if (FillAmount == SystemLibrary.ToInt32(dg_Orders.Rows[i].Cells["RoutedAmount"].Value))
                    dg_Orders.Rows[i].Cells["Order_Filled"].Style.BackColor = Color.GreenYellow;
                else
                    dg_Orders.Rows[i].Cells["Order_Filled"].Style.BackColor = Color.LightCyan;

                if (FromLoad)
                {
                    if (!cb_Portfolio.Items.Contains(Portfolio))
                        cb_Portfolio.Items.Add(Portfolio);
                }
            }
            this.ResumeLayout(true);
        } //SetFormat()


        private void LoadBroker()
        {
            // Local Variables
            String mySql;

            // Load the Broker list
            mySql = "Select	Broker.BrokerID, Broker.BrokerName, BrokerMapping.Broker as BBG_Broker, " +
                    "       BrokerMapping.StrategyType as BBG_StrategyType, isNull(Broker.IsFuture,'N') as IsFuture " +
                    "From	Broker, " +
                    "		BrokerMapping " +
                    "Where	BrokerMapping.BrokerId = Broker.BrokerID " +
                    "And		Broker.isActive = 'Y' " +
                    "Union " +
                    "Select	Broker.BrokerID, Broker.BrokerName, null as BBG_Broker, null as BBG_StrategyType, " +
                    "		isNull(Broker.IsFuture,'N') as IsFuture " +
                    "From	Broker " +
                    "Where	BrokerID not in (Select BrokerID from BrokerMapping) " +
                    "And		Broker.isActive = 'Y' " +
                    "Order by 2 ";
            dt_Broker = SystemLibrary.SQLSelectToDataTable(mySql);

        } //LoadBroker()

        private void cb_ShowProcessed_CheckedChanged(object sender, EventArgs e)
        {
            LoadProcessOrders();

        } //cb_ShowProcessed_CheckedChanged()

        private void dg_Orders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadOrders_Split(e.RowIndex);

        } //dg_Orders_CellClick()
        
        private void LoadOrders_Split(int inRow)
        {
            // Local Variables
            String mySql;
            Int32 FilledQuantity = 0;


            // Make sure its a valid row
            if (inRow < 0)
                return;

            Order.ChangeMade = false;
            Order.ChangeMadeFill = false;
            Order.OrderRefID = dg_Orders["OrderRefID", inRow].Value.ToString();
            Order.BBG_Ticker = dg_Orders["BBG_Ticker", inRow].Value.ToString();
            Order.Quantity = Convert.ToInt32(dg_Orders["Quantity", inRow].Value);
            Order.Side = dg_Orders["Side", inRow].Value.ToString();
            Order.Round_Lot_Size = SystemLibrary.ToInt32(dg_Orders["Round_Lot_Size", inRow].Value);
            if (Order.Round_Lot_Size < 1)
                Order.Round_Lot_Size = 1;
            Order.ProcessedEOD = SystemLibrary.YN_To_Bool(dg_Orders["ProcessedEOD", inRow].Value.ToString());
            Order.TradeDate = Convert.ToDateTime(dg_Orders["EffectiveDate", inRow].Value);
            bt_Split_Report.Visible = true;

            // See which Tab
            switch (tctrl_Process.SelectedTab.Name)
            {
                case "tp_Fills":
                    // Fills
                    mySql = "SELECT     Fills.OrderRefID, Fills.FillNo, Fills.BBG_Ticker, isNull(Fills.BrokerID,Broker.BrokerID) as BrokerID, Fills.FillAmount,  " +
                            "           Fills.FillPrice, Fills.ManualFill, Fills.Confirmed, Fills.TradeDate, Fills.SettlementDate, Fills.Amount, Fills.RoutedAmount,  " +
                            "       	Fills.Side, Fills.BBG_Broker, Fills.BBG_StrategyType " +
                            "FROM       Broker AS Broker INNER JOIN " +
                            "               BrokerMapping_Group AS BrokerMapping ON Broker.BrokerID = BrokerMapping.BrokerID RIGHT OUTER JOIN " +
                            "               Fills AS Fills ON BrokerMapping.Broker = Fills.BBG_Broker AND BrokerMapping.StrategyType = Fills.BBG_StrategyType " +
                            "Where  Fills.OrderRefID = '" + Order.OrderRefID + "'";
                    dt_Fills = SystemLibrary.SQLSelectToDataTable(mySql);
                    DataGridViewComboBoxColumn dcb_Broker = (DataGridViewComboBoxColumn)dg_Fills.Columns["f_BrokerID"];
                    String myFilter = "";
                    if (Order.BBG_Ticker.EndsWith("Index") || Order.BBG_Ticker.EndsWith("Comdty"))
                        myFilter = "IsFuture='Y'";
                    DataTable dt_in = dt_Broker.Clone();
                    foreach (DataRow dr in dt_Broker.Select(myFilter))
                        dt_in.ImportRow(dr);
                    dcb_Broker.DataSource = dt_in;
                    dcb_Broker.DisplayMember = "BrokerName";
                    dcb_Broker.ValueMember = "BrokerID";

                    dg_Fills.Rows.Clear();
                    foreach (DataRow dr in dt_Fills.Rows)
                    {
                        int myRow = dg_Fills.Rows.Add();
                        dg_Fills["f_OrderRefID", myRow].Value = dr["OrderRefID"];
                        dg_Fills["f_FillNo", myRow].Value = dr["FillNo"];
                        dg_Fills["f_BBG_Ticker", myRow].Value = dr["BBG_Ticker"];
                        dg_Fills["f_BrokerID", myRow].Value = dr["BrokerID"];
                        dg_Fills["f_FillAmount", myRow].Value = dr["FillAmount"];
                        FilledQuantity = FilledQuantity + SystemLibrary.ToInt32(dr["FillAmount"]);
                        dg_Fills["f_FillPrice", myRow].Value = dr["FillPrice"];
                        dg_Fills["f_ManualFill", myRow].Value = dr["ManualFill"];
                        dg_Fills["f_Confirmed", myRow].Value = dr["Confirmed"];
                        dg_Fills["f_TradeDate", myRow].Value = dr["TradeDate"];
                        dg_Fills["f_SettlementDate", myRow].Value = dr["SettlementDate"];
                        dg_Fills["f_Amount", myRow].Value = dr["Amount"];
                        dg_Fills["f_RoutedAmount", myRow].Value = dr["RoutedAmount"];
                        dg_Fills["f_Side", myRow].Value = dr["Side"];
                        dg_Fills["f_BBG_Broker", myRow].Value = dr["BBG_Broker"];
                        dg_Fills["f_BBG_StrategyType", myRow].Value = dr["BBG_StrategyType"];
                        ParentForm1.SetColumn(dg_Fills, "f_FillAmount", myRow);
                    }

                    lb_RoundLotSize.Text = "Round Lot Size = " + Order.Round_Lot_Size.ToString("N0");
                    tb_TotalFillQuality.Text = Order.Quantity.ToString("N0");
                    SystemLibrary.SetTextBoxColour(tb_TotalFillQuality, Order.Quantity);
                    tb_UnfilledQuantity.Text = (Order.Quantity - FilledQuantity).ToString("N0");
                    SystemLibrary.SetTextBoxColour(tb_UnfilledQuantity, (Order.Quantity - FilledQuantity));

                    // Start with No manual override
                    cb_ManualOverride.Enabled = true;   
                    cb_ManualOverride.Checked = false;
                    cb_ManualOverride_CheckedChanged(null, null);
                    tb_TotalQuantity.Text = Order.Quantity.ToString("N0");

                    // Set the Fill_Allocation block
                    dg_Fills_Allocation.Rows.Clear();
                    if (dt_Fills.Rows.Count > 0)
                        LoadFills_Allocation(0);
                    break;
                default:
                    // Order Splits


                    mySql = "Select os.OrderRefID, os.FundID, f.FundName, os.PortfolioID, p.PortfolioName, os.Quantity, s.Round_Lot_Size " +
                            "From	Orders_Split os, " +
                            "       Fund f, " +
                            "       Portfolio p, " +
                            "       Securities s " +
                            "Where	os.OrderRefID = '" + Order.OrderRefID + "' " +
                            "And	s.BBG_Ticker = '" + Order.BBG_Ticker + "' " +
                            "And    f.FundID = os.FundID " +
                            "And    p.PortfolioID = os.PortfolioID ";
                    dt_OrdersSplit = SystemLibrary.SQLSelectToDataTable(mySql);
                    DataGridViewComboBoxColumn dcb = (DataGridViewComboBoxColumn)dg_OrdersSplit.Columns["FundID"];
                    dcb.DataSource = dt_Fund;
                    dcb.DisplayMember = "FundName";
                    dcb.ValueMember = "FundID";
                    dcb = (DataGridViewComboBoxColumn)dg_OrdersSplit.Columns["PortfolioID"];
                    dcb.DataSource = dt_Portfolio;
                    dcb.DisplayMember = "PortfolioName";
                    dcb.ValueMember = "PortfolioID";

                    dg_OrdersSplit.Rows.Clear();
                    foreach (DataRow dr in dt_OrdersSplit.Rows)
                    {
                        int myRow = dg_OrdersSplit.Rows.Add();
                        dg_OrdersSplit["FundID", myRow].Value = dr["FundID"];
                        dg_OrdersSplit["PortfolioID", myRow].Value = dr["PortfolioID"];
                        dg_OrdersSplit["BBG_Ticker", myRow].Value = Order.BBG_Ticker;
                        dg_OrdersSplit["Order_Quantity", myRow].Value = dr["Quantity"];
                        dg_OrdersSplit["Round_Lot_Size", myRow].Value = dr["Round_Lot_Size"];
                        ParentForm1.SetColumn(dg_OrdersSplit, "Order_Quantity", myRow);
                    }

                    if (Order.ProcessedEOD)
                    {
                        dg_OrdersSplit.ReadOnly = true;
                        dg_OrdersSplit.Enabled = false;
                        dg_OrdersSplit.AllowUserToAddRows = false;
                        bt_SaveSplit.Enabled = false;
                    }
                    else
                    {
                        dg_OrdersSplit.ReadOnly = false;
                        dg_OrdersSplit.Enabled = true;
                        dg_OrdersSplit.AllowUserToAddRows = true;
                        bt_SaveSplit.Enabled = true;
                    }

                    tb_TotalQuantity.Text = Order.Quantity.ToString("N0");
                    if (Order.Quantity < 0)
                        tb_TotalQuantity.ForeColor = Color.Red;
                    else if (Order.Quantity > 0)
                        tb_TotalQuantity.ForeColor = Color.Green;
                    else
                        tb_TotalQuantity.ForeColor = Color.Black;

                    tb_MissingQuantity.Text = "0";
                    tb_MissingQuantity.ForeColor = Color.Black;
                    break;
            }

        } //LoadOrders_Split()

        private void bt_Calculator_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc");

        } //dg_Orders_CellClick()

        private void tb_TotalQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Tell the Key handler to ignore this key stroke
            e.Handled = true;
        }

        private void tb_MissingQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Tell the Key handler to ignore this key stroke
            e.Handled = true;
        }

        private void bt_SaveSplit_Click(object sender, EventArgs e)
        {
            // TODO (2) - Need to check using RoundLot

            // Eliminate the last row being new row issues
            dg_OrdersSplit.AllowUserToAddRows = false;

            // Test the the FundID & PortfolioID for each row are unique
            if (dg_OrdersSplit.Rows.Count > 1)
            {
                Hashtable FundIDPortfolioID = new Hashtable();
                foreach (DataGridViewRow dgr in dg_OrdersSplit.Rows)
                {
                    String myKey = Convert.ToString(dgr.Cells["FundID"].Value) + "-" + Convert.ToString(dgr.Cells["PortfolioID"].Value);
                    if (FundIDPortfolioID.Contains(myKey))
                    {
                        dg_OrdersSplit.AllowUserToAddRows = true;
                        MessageBox.Show("Cannot have Duplicate Fund Name / Portfolio Name combinations", "Create Order");
                        return;
                    }
                    else
                        FundIDPortfolioID.Add(myKey, myKey);
                }
            }

            // Save back to the database
            SystemLibrary.SQLExecute("Delete from Orders_Split where OrderRefId='"+Order.OrderRefID+"' ");
            foreach (DataGridViewRow dgr in dg_OrdersSplit.Rows)
            {
                if (SystemLibrary.ToInt32(dgr.Cells["Order_Quantity"].Value) != 0)
                {
                    String mySql = "Insert into Orders_Split (OrderRefId, FundID, PortfolioID, Quantity, Round_Lot_Size) " +
                                   "Values ('" + Order.OrderRefID + "'," + dgr.Cells["FundID"].Value.ToString() + "," + 
                                            dgr.Cells["PortfolioID"].Value.ToString() + "," +
                                            SystemLibrary.ToInt32(dgr.Cells["Order_Quantity"].Value.ToString()).ToString() + "," + SystemLibrary.ToInt32(dgr.Cells["Round_Lot_Size"].Value.ToString()).ToString() + ")";
                    SystemLibrary.SQLExecute(mySql);
                }
            }

            // Will need to cause a Fill reprocess
            SystemLibrary.SQLExecute("Exec sp_ReprocessFillbyRef '"+Order.OrderRefID+"' ");
            Order.ChangeMade = false;
            // Update The Positions Table
            NeedUpdate = true;

            MessageBox.Show("Saved", "Order Splits");
        }

        private void dg_OrdersSplit_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            LastValue = dg_OrdersSplit[e.ColumnIndex, e.RowIndex].Value;
        } //dg_OrdersSplit_CellBeginEdit

        private void dg_OrdersSplit_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Local Variables
            Int32 Qty_Remain = Order.Quantity;
            Int32 Qty;

            //if (dg_OrdersSplit.Columns[e.ColumnIndex].Name == "Order_Quantity")
            {
                // Add all the Qty_Fill and calculate the MissingQuantity
                foreach (DataGridViewRow dgr in dg_OrdersSplit.Rows)
                {
                    Qty = SystemLibrary.ToInt32(dgr.Cells["Order_Quantity"].Value);
                    if (Math.Sign(Qty) != Math.Sign(Order.Quantity) && Qty != 0)
                    {
                        MessageBox.Show("Wrong Sign on Order Quantity");
                        dg_OrdersSplit[e.ColumnIndex, e.RowIndex].Value = LastValue;
                        return;
                    }
                    Qty_Remain = Qty_Remain - Qty;
                }
            }
            if (Qty_Remain == 0)
                bt_SaveSplit.Enabled = true;
            else
                bt_SaveSplit.Enabled = false;
            tb_MissingQuantity.Text = Qty_Remain.ToString("N0");
            if (Qty_Remain < 0)
                tb_MissingQuantity.ForeColor = Color.Red;
            else if (Qty_Remain > 0)
                tb_MissingQuantity.ForeColor = Color.Green;
            else
                tb_MissingQuantity.ForeColor = Color.Black;
            if (SystemLibrary.ToInt32(dg_OrdersSplit[e.ColumnIndex, e.RowIndex].Value) < 0)
                dg_OrdersSplit[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
            else
                dg_OrdersSplit[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Green;
            Order.ChangeMade = true;

        } //dg_OrdersSplit_CellEndEdit() 


        private void dg_OrdersSplit_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            dg_OrdersSplit.CurrentRow.Cells["FundID"].Value = -1;
            dg_OrdersSplit.CurrentRow.Cells["PortfolioID"].Value = -1;
            dg_OrdersSplit.CurrentRow.Cells["BBG_Ticker"].Value = Order.BBG_Ticker;
            dg_OrdersSplit.CurrentRow.Cells["Order_Quantity"].Value = 0;
            dg_OrdersSplit.CurrentRow.Cells["Round_Lot_Size"].Value = Order.Round_Lot_Size;
            Order.ChangeMade = true;
        } //dg_OrdersSplit_UserAddedRow() 

        private void cb_SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach(DataGridViewRow dgr in dg_Orders.Rows)
            {
                if (!SystemLibrary.YN_To_Bool(dgr.Cells["ProcessedEOD"].Value.ToString()))
                    dgr.Cells["Order_Completed"].Value = SystemLibrary.Bool_To_YN(cb_SelectAll.Checked);
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;

            foreach (DataGridViewRow dgr in dg_Orders.Rows)
            {
                if (!SystemLibrary.YN_To_Bool(dgr.Cells["ProcessedEOD"].Value.ToString()))
                {
                    DateTime EffectiveDate = Convert.ToDateTime(dgr.Cells["EffectiveDate"].Value);
                    if (EffectiveDate.CompareTo(SystemLibrary.f_Today()) > 0)
                    {
                        String BBG_Ticker = dgr.Cells["BBG_Ticker"].FormattedValue.ToString() + " - " + dgr.Cells["Side"].FormattedValue.ToString() + " " + dgr.Cells["Quantity"].FormattedValue.ToString() + @" @ " + dgr.Cells["FillPrice"].FormattedValue.ToString();
                        
                        if (MessageBox.Show(BBG_Ticker+"  is future dated.\r\n\r\nDo you want to Skip this Order and process it on '" + EffectiveDate.ToString("dd-MMM-yyyy") + "'?", "Future Dated Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            // Skip this Order
                            continue;
                        }
                    }
                    String OrderRefId = dgr.Cells["OrderRefID"].Value.ToString();
                    String Order_Completed = dgr.Cells["Order_Completed"].Value.ToString();
                    mySql = "Update Orders " +
                            "Set Order_Completed = '" + Order_Completed + "' " +
                            "Where OrderRefID = '" + OrderRefId + "' ";
                    SystemLibrary.SQLExecute(mySql);
                    mySql = "Update Fills " +
                            "Set Confirmed = '" + Order_Completed + "' " +
                            "Where OrderRefID = '" + OrderRefId + "' ";
                    SystemLibrary.SQLExecute(mySql);
                }
            }
            cb_SelectAll.Checked = false;
            LoadProcessOrders();
            MessageBox.Show("Orders Saved. Now go to Process Trades to Sent to Brokers.", this.Text);
        } //bt_Save_Click()

        private void dg_Orders_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (Order.OrderRefID!=null && Order.OrderRefID!=dg_Orders["OrderRefID", e.RowIndex].Value.ToString())
                LoadOrders_Split(e.RowIndex);
        }

        private void tctrl_Process_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (Order.ChangeMade || Order.ChangeMadeFill)
            {
                TabControl tc = (TabControl)sender;
                if (MessageBox.Show(this, "You have Not Saved your Changes.\r\n\r\n" +
                                          "Changing to the '" + tc.SelectedTab.Text + "' tab will mean you loose those changes.\r\n\n" +
                                          "Do you really want to change Tab?", "Change Tab", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    int myRow = dg_Orders.CurrentCell.RowIndex;
                    LoadOrders_Split(myRow);
                }
            }
            else
            {
                int myRow = dg_Orders.CurrentCell.RowIndex;
                LoadOrders_Split(myRow);
            }
        } //tctrl_Process_Selecting()

        private void dg_OrdersSplit_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            // Local Variables
            int Qty_Remain = Order.Quantity;

            // Add all the Qty_Fill and calculate the MissingQuantity
            foreach (DataGridViewRow dgr in dg_OrdersSplit.Rows)
            {
                int Qty = Convert.ToInt32(dgr.Cells["Order_Quantity"].Value);
                Qty_Remain = Qty_Remain - Qty;
            }

            if (Qty_Remain == 0)
                bt_SaveSplit.Enabled = true;
            else
                bt_SaveSplit.Enabled = false;
            tb_MissingQuantity.Text = Qty_Remain.ToString("N0");
            if (Qty_Remain < 0)
                tb_MissingQuantity.ForeColor = Color.Red;
            else if (Qty_Remain > 0)
                tb_MissingQuantity.ForeColor = Color.Green;
            else
                tb_MissingQuantity.ForeColor = Color.Black;
            Order.ChangeMade = true;
        } //dg_OrdersSplit_UserDeletedRow()

        private void bt_ProcessTrades_Click(object sender, EventArgs e)
        {
            // Open the Process Trades Form and if Available Pass on the Parent Form.
            ProcessTrades frm_po = new ProcessTrades();
            if(ParentForm1!=null)
                frm_po.FromParent(ParentForm1);
            frm_po.Show();
        }

        private void cb_ManualOverride_CheckedChanged(object sender, EventArgs e)
        {
            Boolean isOverride = cb_ManualOverride.Checked;

            dg_Fills.ReadOnly = !isOverride;

            bt_SaveFill.Visible = isOverride;
            lb_FillMessage.Visible = isOverride;
            lb_TotalFillQuality.Visible = isOverride;
            tb_TotalFillQuality.Visible = isOverride;
            lb_UnfilledQuantity.Visible = isOverride;
            tb_UnfilledQuantity.Visible = isOverride;
            lb_RoundLotSize.Visible = isOverride;
        }

        private void dg_Fills_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Seems the only way I can get the previous value before CellEndEdit().
            LastValue = dg_Fills[e.ColumnIndex, e.RowIndex].Value;
            //if (dg_Fills.Columns[e.ColumnIndex].Name == "fc_crncy")
                //e.Cancel = true;
        } //dg_Fills_CellBeginEdit()

        private void dg_Fills_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Local Variables
            Int32 FilledQuantity = 0;


            // What column is changing
            // - dg_Broker.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.ToString().ToUpper()
            if (dg_Fills.Columns[e.ColumnIndex].Name == "f_BrokerID")
            {
                // Set the BBG_Broker, & BBG_StrategyType columns
                String FindBroker = SystemLibrary.ToString(dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                if (FindBroker.Length > 0)
                {
                    DataRow[] FoundRows = dt_Broker.Select("BrokerID=" + FindBroker);
                    if (FoundRows.Length > 0)
                    {
                        dg_Fills.Rows[e.RowIndex].Cells["f_BBG_Broker"].Value = FoundRows[0]["BBG_Broker"].ToString();
                        dg_Fills.Rows[e.RowIndex].Cells["f_BBG_StrategyType"].Value = FoundRows[0]["BBG_StrategyType"].ToString();
                    }
                }
            }
            else if (dg_Fills.Columns[e.ColumnIndex].Name == "f_TradeDate" || dg_Fills.Columns[e.ColumnIndex].Name == "f_SettlementDate")
            {
                // Validate this is a Date
                DateTime myResult = new DateTime();
                if (!DateTime.TryParse(dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim(), out myResult))
                {
                    dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                }
                else
                    dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult.ToString(dg_Fills.Columns[e.ColumnIndex].DefaultCellStyle.Format);

            }
            else if (dg_Fills.Columns[e.ColumnIndex].Name == "f_FillAmount")
            {
                // Validate this is a number
                Decimal myResult = new Decimal();
                if (dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                    myResult = 0;
                else if (!Decimal.TryParse(dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim(), out myResult))
                    myResult = 0;
                else
                    myResult = SystemLibrary.ToDecimal(dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                if (Math.Sign(myResult) != Math.Sign(SystemLibrary.ToDecimal(dg_Fills.Rows[e.RowIndex].Cells["f_Amount"].Value)) && myResult != 0)
                    dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                else
                    dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult;
                ParentForm1.SetColumn(dg_Fills, "f_FillAmount", e.RowIndex);
                // Make sure Not Greater tham Amount
                if (myResult > SystemLibrary.ToDecimal(dg_Fills.Rows[e.RowIndex].Cells["f_Amount"].Value))
                {
                    dg_Fills.Rows[e.RowIndex].Cells["f_RoutedAmount"].Value = myResult;
                }
                //dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].for
            }
            else if (dg_Fills.Columns[e.ColumnIndex].Name == "f_FillPrice")
            {
                // Validate this is a number
                Decimal myResult = new Decimal();
                if (!Decimal.TryParse(dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim(), out myResult))
                    myResult = 0;
                else
                    myResult = SystemLibrary.ToDecimal(dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                if (Math.Sign(myResult)<0)
                    dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                else
                    dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult; //.ToString(dg_Fills.Columns[e.ColumnIndex].DefaultCellStyle.Format);
                //dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].for
            }

            // Mark it as a Manual Fill
            dg_Fills.Rows[e.RowIndex].Cells["f_ManualFill"].Value = "Y";
            dg_Fills.Rows[e.RowIndex].Cells["f_Confirmed"].Value = "Y";
            dg_Fills.Refresh();

            // Set Unfilled Quantity
            foreach (DataGridViewRow dgr in dg_Fills.Rows)
            {
                FilledQuantity = FilledQuantity + SystemLibrary.ToInt32(dgr.Cells["f_FillAmount"].Value);
            }
            //FilledQuantity = FilledQuantity + SystemLibrary.ToInt32(dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            tb_UnfilledQuantity.Text = (Order.Quantity - FilledQuantity).ToString("N0");
            SystemLibrary.SetTextBoxColour(tb_UnfilledQuantity, (Order.Quantity - FilledQuantity));

            // Check if can Save
            if (Math.Abs(Order.Quantity) >= Math.Abs(FilledQuantity) && Math.Sign(Order.Quantity) == Math.Sign(FilledQuantity))
                bt_SaveFill.Enabled = true;
            else
                bt_SaveFill.Enabled = false;

        } //dg_Fills_CellEndEdit()

        private void dg_Fills_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            Decimal MaxFillNo = 0;
            for (int i=0;i<dg_Fills.Rows.Count;i++)
                MaxFillNo = Math.Max(MaxFillNo,SystemLibrary.ToDecimal(dg_Fills.Rows[i].Cells["f_FillNo"].Value));

            dg_Fills.CurrentRow.Cells["f_OrderRefID"].Value = Order.OrderRefID;
            dg_Fills.CurrentRow.Cells["f_FillNo"].Value = (int)(MaxFillNo + 1);
            dg_Fills.CurrentRow.Cells["f_BBG_Ticker"].Value = Order.BBG_Ticker;
            dg_Fills.CurrentRow.Cells["f_ManualFill"].Value = "Y";
            dg_Fills.CurrentRow.Cells["f_Confirmed"].Value = "Y";
            dg_Fills.CurrentRow.Cells["f_Amount"].Value = Order.Quantity;
            dg_Fills.CurrentRow.Cells["f_RoutedAmount"].Value = 0;
            dg_Fills.CurrentRow.Cells["f_Side"].Value = Order.Side;
            dg_Fills.CurrentRow.Cells["f_TradeDate"].Value = Order.TradeDate.ToString(dg_Fills.Columns["f_TradeDate"].DefaultCellStyle.Format);
            dg_Fills.CurrentRow.Cells["f_SettlementDate"].Value = Order.TradeDate.ToString(dg_Fills.Columns["f_TradeDate"].DefaultCellStyle.Format);
            dg_Fills.CurrentRow.Cells["f_FillAmount"].Value = tb_UnfilledQuantity.Text; // Order.Quantity;
            dg_Fills.CurrentRow.Cells["f_FillPrice"].Value = 0;

            // If there is only 1 broker, then go ahead an set this
            DataGridViewComboBoxColumn dcb_Broker = (DataGridViewComboBoxColumn)dg_Fills.Columns["f_BrokerID"];
            DataTable dt_Brokers = (DataTable)dcb_Broker.DataSource;
            if (dt_Brokers.Rows.Count == 1)
            {
                dg_Fills.CurrentRow.Cells["f_BrokerID"].Value = Convert.ToInt32(dt_Brokers.Rows[0]["BrokerID"]);
                dg_Fills.CurrentRow.Cells["f_BBG_Broker"].Value = dt_Brokers.Rows[0]["BBG_Broker"].ToString();
                dg_Fills.CurrentRow.Cells["f_BBG_StrategyType"].Value = dt_Brokers.Rows[0]["BBG_StrategyType"].ToString();
            }

            dg_Fills.Refresh();

            bt_SaveFill.Enabled = true;
        } //dg_Fills_UserAddedRow()

        private void dg_Fills_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            // Local Variables
            Int32 FilledQuantity = 0;

            // Seems to be a bug that allows users to Delete Rows even when Readonly
            if (dg_Fills.ReadOnly)
                return;

            // Set Unfilled Quantity
            foreach (DataGridViewRow dgr in dg_Fills.Rows)
            {
                FilledQuantity = FilledQuantity + SystemLibrary.ToInt32(dgr.Cells["f_FillAmount"].Value);
            }
            //FilledQuantity = FilledQuantity + SystemLibrary.ToInt32(dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            tb_UnfilledQuantity.Text = (Order.Quantity - FilledQuantity).ToString("N0");
            SystemLibrary.SetTextBoxColour(tb_UnfilledQuantity, (Order.Quantity - FilledQuantity));

            // Check if can Save
            if (Math.Abs(Order.Quantity) >= Math.Abs(FilledQuantity) && Math.Sign(Order.Quantity) == Math.Sign(FilledQuantity))
                bt_SaveFill.Enabled = true;
            else
                bt_SaveFill.Enabled = false;

        } //dg_Fills_UserDeletedRow()

        private void bt_SaveFill_Click(object sender, EventArgs e)
        {
            // TODO (2) - Need to check using RoundLot
            // Local Variables
            String OrderRefID = Order.OrderRefID;
            Boolean okRoundLot = true;
            String mySql;
            DateTime TradeDate = DateTime.MinValue;
            DateTime SettlementDate = DateTime.MinValue;
            int myScrollRow = dg_Orders.FirstDisplayedScrollingRowIndex;
                //DataGridView.FirstDisplayedScrollingRowIndex

            if (tb_UnfilledQuantity.Text.Trim() != "0")
            {
                if (MessageBox.Show(this, "I notice there is still and Unfilled Quantity of " + tb_UnfilledQuantity.Text + ".\r\n\r\n" +
                              "Do you want to Abort [Save Fill] so you can correct this?", "Save Fill", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    return;
                }
            }

            // Eliminate the last row being new row issues
            dg_Fills.AllowUserToAddRows = false;

            foreach (DataGridViewRow dgr in dg_Fills.Rows)
            {
                if (TradeDate == DateTime.MinValue)
                    TradeDate = Convert.ToDateTime(dgr.Cells["f_TradeDate"].Value);

                // All TradeDates need to be the same
                if (TradeDate != Convert.ToDateTime(dgr.Cells["f_TradeDate"].Value))
                {
                    MessageBox.Show("Failed: All the Trade Dates must be the same.", "Save Fill");
                    dg_Fills.AllowUserToAddRows = true;
                    return;
                }

                if (SettlementDate == DateTime.MinValue)
                    SettlementDate = Convert.ToDateTime(dgr.Cells["f_SettlementDate"].Value);

                // All SettlementDate need to be the same
                if (SettlementDate != Convert.ToDateTime(dgr.Cells["f_SettlementDate"].Value))
                {
                    MessageBox.Show("Failed: All the Settlement Dates must be the same.", "Save Fill");
                    dg_Fills.AllowUserToAddRows = true;
                    return;
                }

                // SettlementDate >= TradeDate
                if (SettlementDate < TradeDate)
                {
                    MessageBox.Show("Failed: Settlement Date must be greate or equal to the Trade Date.", "Save Fill");
                    dg_Fills.AllowUserToAddRows = true;
                    return;
                }

                if (Math.IEEERemainder(SystemLibrary.ToDouble(dgr.Cells["f_FillAmount"].Value), Convert.ToDouble(Order.Round_Lot_Size)) != 0)
                    okRoundLot = false;
                //SystemLibrary.DebugLine(dgr.Cells["f_FillAmount"].Value + ", " + Order.Round_Lot_Size+", "+Math.IEEERemainder(SystemLibrary.ToDouble(dgr.Cells["f_FillAmount"].Value), Convert.ToDouble(Order.Round_Lot_Size)));
                if (dgr.Cells["f_BrokerID"].Value == null)
                {
                    MessageBox.Show("Failed: You must supply a broker", "Save Fill");
                    dg_Fills.AllowUserToAddRows = true;
                    return;
                }
                if (SystemLibrary.ToInt32(dgr.Cells["f_FillAmount"].Value) == 0)
                {
                    MessageBox.Show("Failed: You must supply a valid Fill Amount.\r\n\r\nOtherwise Delete the unwanted record.", "Save Fill");
                    dg_Fills.AllowUserToAddRows = true;
                    return;
                }
                if (SystemLibrary.ToDecimal(dgr.Cells["f_FillPrice"].Value) == 0)
                {
                    if (MessageBox.Show(this, "I notice not all Fill price supplied is Zero.\r\n\r\n" +
                                              "Do you really want to save this Fill with a Zero Price?", "Save Fill", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        dg_Fills.AllowUserToAddRows = true;
                        return;
                    }
                }
            }

            if (!okRoundLot)
            {
                if (MessageBox.Show(this, "I notice not all Fill Amounts divide evenly by the Round Lot Size of "+Order.Round_Lot_Size.ToString("N0")+".\r\n\r\n" +
                                          "Do you want to Abort [Save Fill] so you can correct this?", "Save Fill", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dg_Fills.AllowUserToAddRows = true;
                    return;
                }
            }

            // Remove old records
            mySql = "Delete From Fills where OrderRefID = '" + dg_Fills["f_OrderRefID",0].Value.ToString() + "' ";
            SystemLibrary.SQLExecute(mySql);

            // Save back to the database
            //SystemLibrary.SQLExecute("Delete from Fills where OrderRefId='" + Order.OrderRefID + "' ");
            foreach (DataGridViewRow dgr in dg_Fills.Rows)
            {
                /*
                dg_Fills["f_OrderRefID", myRow].Value = dr["OrderRefID"];
                dg_Fills["f_FillNo", myRow].Value = dr["FillNo"];
                dg_Fills["f_BBG_Ticker", myRow].Value = dr["BBG_Ticker"];
                dg_Fills["f_BrokerID", myRow].Value = dr["BrokerID"];
                dg_Fills["f_FillAmount", myRow].Value = dr["FillAmount"];
                FilledQuantity = FilledQuantity + SystemLibrary.ToInt32(dr["FillAmount"]);
                dg_Fills["f_FillPrice", myRow].Value = dr["FillPrice"];
                dg_Fills["f_ManualFill", myRow].Value = dr["ManualFill"];
                dg_Fills["f_Confirmed", myRow].Value = dr["Confirmed"];
                dg_Fills["f_TradeDate", myRow].Value = dr["TradeDate"];
                dg_Fills["f_Amount", myRow].Value = dr["Amount"];
                dg_Fills["f_RoutedAmount", myRow].Value = dr["RoutedAmount"];
                dg_Fills["f_Side", myRow].Value = dr["Side"];
                dg_Fills["f_BBG_Broker", myRow].Value = dr["BBG_Broker"];
                dg_Fills["f_BBG_StrategyType", myRow].Value = dr["BBG_StrategyType"];
                */

                //TODO (5) Why? dgr.Cells["f_Confirmed"].Value.ToString() + "', '" + Convert.ToDateTime(dgr.Cells["f_TradeDate"].Value).ToString("dd-MMM-yyyy") + "', " +
                String BBG_Broker = "null";
                String BBG_StrategyType = "null";
                String BrokerID = "null";
                if (dgr.Cells["f_BBG_Broker"].Value != null)
                    BBG_Broker = "'" + dgr.Cells["f_BBG_Broker"].Value.ToString() + "'";
                if (dgr.Cells["f_BBG_StrategyType"].Value != null)
                    BBG_StrategyType = "'" + dgr.Cells["f_BBG_StrategyType"].Value.ToString() + "'";
                if (dgr.Cells["f_BrokerID"].Value != null)
                    BrokerID = dgr.Cells["f_BrokerID"].Value.ToString();
                
                // 20120207 - Set RoutedAmount to FillAmount
                mySql = "Insert into Fills (OrderRefID, BBG_Ticker, BBG_Broker, BBG_StrategyType, BrokerID, Amount, RoutedAmount, FillAmount, FillPrice, Side, ManualFill, Confirmed, TradeDate, SettlementDate, FillNo) " +
                        "Values ('" + dgr.Cells["f_OrderRefID"].Value.ToString() + "', '" + dgr.Cells["f_BBG_Ticker"].Value.ToString() + "', " +
                        BBG_Broker + ", " + BBG_StrategyType + ", " + BrokerID + ", " +
                        SystemLibrary.ToDecimal(dgr.Cells["f_Amount"].Value).ToString() + ", " + SystemLibrary.ToDecimal(dgr.Cells["f_FillAmount"].Value).ToString() + ", " +
                        SystemLibrary.ToDecimal(dgr.Cells["f_FillAmount"].Value).ToString() + ", " + SystemLibrary.ToDecimal(dgr.Cells["f_FillPrice"].Value).ToString() + ", '" +
                        dgr.Cells["f_Side"].Value.ToString() + "', '" + dgr.Cells["f_ManualFill"].Value.ToString() + "', '" +
                        "N', '" + Convert.ToDateTime(dgr.Cells["f_TradeDate"].Value).ToString("dd-MMM-yyyy") + "', '" + Convert.ToDateTime(dgr.Cells["f_SettlementDate"].Value).ToString("dd-MMM-yyyy") + "', " +
                        dgr.Cells["f_FillNo"].Value.ToString() + ") ";

                int myRows = SystemLibrary.SQLExecute(mySql);
            }

            // Apply any change in TradeDate to the Order record
            if (TradeDate != DateTime.MinValue)
            {
                mySql = "Update Orders Set EffectiveDate = '" + TradeDate.ToString("dd-MMM-yyyy") + "' Where OrderRefId = '" + dg_Fills["f_OrderRefID", 0].Value.ToString() + "' ";
                SystemLibrary.SQLExecute(mySql);
            }
            // Mark Order as Manula
            mySql = "Update Orders Set ManualOrder = 'Y' Where OrderRefId = '" + dg_Fills["f_OrderRefID", 0].Value.ToString() + "' ";
            SystemLibrary.SQLExecute(mySql);

            // Will need to cause a Fill reprocess
            SystemLibrary.SQLExecute("Exec sp_ReprocessFillbyRef '" + Order.OrderRefID + "' ");
            Order.ChangeMadeFill = false;
            cb_ManualOverride.Checked = false;
            cb_ManualOverride_CheckedChanged(null, null);
            dg_Fills.AllowUserToAddRows = true;
            LoadProcessOrders();
            if (myScrollRow > 0 && dg_Orders.Rows.Count >= myScrollRow)
                dg_Orders.FirstDisplayedScrollingRowIndex = myScrollRow;

            // Go back the that row in dg_Orders
            dg_OrdersSelectRowbyOrderRefID(OrderRefID);


            // Update The Positions Table
            NeedUpdate = true;
            // Refresh the Portfolio Tab
            //ParentForm1.LoadPortfolioIncr();


            MessageBox.Show("Saved", "Fills");

        } //bt_SaveFill_Click()

        private void dg_OrdersSelectRowbyOrderRefID(String OrderRefID)
        {
            // Go back the that row in dg_Orders
            for (int i = 0; i < dg_Orders.Rows.Count; i++)
            {
                if (dg_Orders["OrderRefID", i].Value.ToString() == OrderRefID)
                {
                    dg_Orders.Rows[i].Selected = true;
                    dg_Orders.CurrentCell = dg_Orders["BBG_Ticker", i];
                    //LoadOrders_Split(i);
                }
                else
                    dg_Orders.Rows[i].Selected = false;
            }
        } //dg_OrdersSelectRowbyOrderRefID()

        private void tb_TotalFillQuality_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Tell the Key handler to ignore this key stroke
            e.Handled = true;
        }

        private void tb_UnfilledQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Tell the Key handler to ignore this key stroke
            e.Handled = true;
        }

        private void dg_Fills_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (cb_ManualOverride.Checked == false && e.RowIndex != 0)
                if (Order.OrderRefID != null && Order.OrderRefID != SystemLibrary.ToString(dg_Fills["f_OrderRefID", e.RowIndex].Value))
                    LoadFills_Allocation(e.RowIndex);
        }

        private void dg_Fills_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (cb_ManualOverride.Checked == false)
                LoadFills_Allocation(e.RowIndex);
        }

        private void LoadFills_Allocation(int inRow)
        {
            // Local Variables
            String mySql;
            String OrderRefID = "";
            //Int32 FilledQuantity = 0;


            // Make sure its a valid row
            if (inRow < 0)
                return;

            OrderRefID = SystemLibrary.ToString(dg_Fills["f_OrderRefID", inRow].Value);

            if (OrderRefID.Length == 0)
                return;

            Fills_parent.ChangeMade = false;
            Fills_parent.OrderRefID = OrderRefID;
            //Fills_parent.BBG_Ticker = dg_Fills["f_BBG_Ticker", inRow].Value.ToString();
            Fills_parent.Quantity = Convert.ToInt32(dg_Fills["f_Amount", inRow].Value);
            Fills_parent.FillPrice = Convert.ToDouble(dg_Fills["f_FillPrice", inRow].Value);
            Fills_parent.Side = dg_Fills["f_Side", inRow].Value.ToString();
            // Where Does Round_Lot_Size come from?
            Fills_parent.Round_Lot_Size = Order.Round_Lot_Size;
            //Fills_parent.TradeDate = Convert.ToDateTime(dg_Fills["f_TradeDate", inRow].Value);
            Fills_parent.BBG_Broker = dg_Fills["f_BBG_Broker", inRow].Value.ToString();
            Fills_parent.BBG_StrategyType = dg_Fills["f_BBG_StrategyType", inRow].Value.ToString();
            Fills_parent.RoutedAmount = Convert.ToInt32(dg_Fills["f_RoutedAmount", inRow].Value);
            Fills_parent.FillNo = dg_Fills["f_FillNo", inRow].Value.ToString();

            // BrokerID needs to come when I know BBG_Broker/BBG_StrategyType

            mySql = "SELECT	OrderRefID, FundID, PortfolioID, Order_Quantity, Fill_Quantity, Fill_Price, BBG_Broker, " +
                    "		BBG_StrategyType, BrokerID, Qty_Routed, TradeID, FillNo " +
                    "FROM	Fills_Allocation " +
                    "WHERE  OrderRefID = '" + Fills_parent.OrderRefID + "' " +
                    "AND    FillNo = " + Fills_parent.FillNo;

            dt_Fills_allocation = SystemLibrary.SQLSelectToDataTable(mySql);
            DataGridViewComboBoxColumn dcb = (DataGridViewComboBoxColumn)dg_Fills_Allocation.Columns["fa_FundID"];
            dcb.DataSource = dt_Fund;
            dcb.DisplayMember = "FundName";
            dcb.ValueMember = "FundID";
            dcb = (DataGridViewComboBoxColumn)dg_Fills_Allocation.Columns["fa_PortfolioID"];
            dcb.DataSource = dt_Portfolio;
            dcb.DisplayMember = "PortfolioName";
            dcb.ValueMember = "PortfolioID";

            dg_Fills_Allocation.Rows.Clear();
            foreach (DataRow dr in dt_Fills_allocation.Rows)
            {
                int myRow = dg_Fills_Allocation.Rows.Add();
                dg_Fills_Allocation["fa_OrderRefID", myRow].Value = dr["OrderRefID"];
                dg_Fills_Allocation["fa_FundID", myRow].Value = dr["FundID"];
                dg_Fills_Allocation["fa_PortfolioID", myRow].Value = dr["PortfolioID"];
                dg_Fills_Allocation["fa_Order_Quantity", myRow].Value = dr["Order_Quantity"];
                dg_Fills_Allocation["fa_Fill_Quantity", myRow].Value = dr["Fill_Quantity"];
                dg_Fills_Allocation["fa_Fill_Price", myRow].Value = dr["Fill_Price"];
                dg_Fills_Allocation["fa_BBG_Broker", myRow].Value = dr["BBG_Broker"]; ;
                dg_Fills_Allocation["fa_BBG_StrategyType", myRow].Value = dr["BBG_StrategyType"]; ;
                dg_Fills_Allocation["fa_BrokerID", myRow].Value = dr["BrokerID"]; ;
                dg_Fills_Allocation["fa_Qty_Routed", myRow].Value = dr["Qty_Routed"]; ;
                dg_Fills_Allocation["fa_TradeID", myRow].Value = dr["TradeID"]; ;
                dg_Fills_Allocation["fa_FillNo", myRow].Value = dr["FillNo"]; ;
                ParentForm1.SetColumn(dg_Fills_Allocation, "fa_Fill_Quantity", myRow);
                ParentForm1.SetColumn(dg_Fills_Allocation, "fa_Qty_Routed", myRow);
            }

            /* 
             * NEED TO CODE FOR ORDERS ALREADY PROCESSED
             * NEED TO CODE FOR ORDERS ALREADY PROCESSED
             * NEED TO CODE FOR ORDERS ALREADY PROCESSED
            if (Order.ProcessedEOD)
            {
                dg_OrdersSplit.ReadOnly = true;
                dg_OrdersSplit.Enabled = false;
                dg_OrdersSplit.AllowUserToAddRows = false;
                bt_SaveSplit.Enabled = false;
            }
            else
            {
                dg_OrdersSplit.ReadOnly = false;
                dg_OrdersSplit.Enabled = true;
                dg_OrdersSplit.AllowUserToAddRows = true;
                bt_SaveSplit.Enabled = true;
            }
             * NEED TO CODE FOR ORDERS ALREADY PROCESSED
             * NEED TO CODE FOR ORDERS ALREADY PROCESSED
             * NEED TO CODE FOR ORDERS ALREADY PROCESSED
            */

        }

        private void dg_Fills_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void dg_OrdersSplit_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            SystemLibrary.DebugLine(e.RowIndex + "," + e.ColumnIndex);
            //dg_OrdersSplit[e.ColumnIndex,e.RowIndex].Value
        }

        private void bt_ResizeOrder_Click(object sender, EventArgs e)
        {
            // Local Variable
            String myValue = "";
            int myQty = Order.Quantity;
            String  mySql;
            String OrderRefID = Order.OrderRefID;

            if (Order.OrderRefID == null)
            {
                MessageBox.Show("You must select an Order first", ((Button)sender).Text);
                return;
            }

            if (SystemLibrary.InputBox("Resize Order from " + tb_TotalQuantity.Text, "Change the Size of the Order OR Cancel", ref myValue, validate_ResizOrder, MessageBoxIcon.Question) == DialogResult.OK)
            {
                myQty = SystemLibrary.ToInt32(myValue);
                mySql = "Exec sp_ResizeOrder '" + OrderRefID + "', " + myQty.ToString();
                SystemLibrary.SQLExecute(mySql);

                LoadProcessOrders();
                for (int i = 0;i<dg_Orders.Rows.Count; i++)
                {
                    if(dg_Orders["OrderRefID",i].Value.ToString()==OrderRefID)
                    {
                        LoadOrders_Split(i);
                        break;
                    }
                }

                Order.ChangeMade = false;
                // Update The Positions Table
                NeedUpdate = true;

                MessageBox.Show("Saved", "Order");
            }

        } //bt_ResizeOrder_Click()

        SystemLibrary.InputBoxValidation validate_ResizOrder = delegate(String myValue)
        {
            // Rules: Ok to be Zero - this means cancelling the order
            //          Cannot be opposite sign
            //          Look at Order.Side
            int myIntValue = SystemLibrary.ToInt32(myValue);
            if (Math.Sign((decimal)Order.Quantity) != Math.Sign((decimal)myIntValue))
                return "'" + myValue + "' cannot be opposite sign to original order. Please set to 0 (Zero) if you want to delete this order.";
            else
                return "";

        }; //validate_ResizOrder()

        private void dg_Orders_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Show the popup menu
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    Point myLocation = new Point(this.Location.X + CXLocation, this.Location.Y + CYLocation);
                    // Select the Order as needed in code later on.
                    LoadOrders_Split(e.RowIndex);
                    String OrderRefID = SystemLibrary.ToString(dg_Orders.Rows[e.RowIndex].Cells["OrderRefId"].Value);
                    String EMSX_Sequence = SystemLibrary.ToString(dg_Orders.Rows[e.RowIndex].Cells["Order#"].Value);
                    String ProcessedEOD = SystemLibrary.ToString(dg_Orders.Rows[e.RowIndex].Cells["ProcessedEOD"].Value);
                    String BBG_Ticker = SystemLibrary.ToString(dg_Orders.Rows[e.RowIndex].Cells["BBG_Ticker"].Value);
                    String FormattedQuantity = dg_Orders.Rows[e.RowIndex].Cells["Quantity"].FormattedValue.ToString();
                    String FormattedFillAmount = dg_Orders.Rows[e.RowIndex].Cells["FillAmount"].FormattedValue.ToString();
                    String FormattedRoutedAmount = dg_Orders.Rows[e.RowIndex].Cells["RoutedAmount"].FormattedValue.ToString();
                    Decimal Quantity = SystemLibrary.ToDecimal(dg_Orders.Rows[e.RowIndex].Cells["Quantity"].Value);
                    Decimal FillAmount = SystemLibrary.ToDecimal(dg_Orders.Rows[e.RowIndex].Cells["FillAmount"].Value);
                    Decimal FillPrice = SystemLibrary.ToDecimal(dg_Orders.Rows[e.RowIndex].Cells["FillPrice"].Value);
                    Decimal RoutedAmount = SystemLibrary.ToDecimal(dg_Orders.Rows[e.RowIndex].Cells["RoutedAmount"].Value);
                    String Side = SystemLibrary.ToString(dg_Orders.Rows[e.RowIndex].Cells["Side"].Value);
                    if (OrderRefID.Length == 0)
                        return;
                    if (ProcessedEOD != "N")
                    {
                        MessageBox.Show("Must Delete the Trades associated with this Order first");
                        return;
                    }
                    
                    ContextMenuStrip myMenu = new ContextMenuStrip();
                    ToolStripMenuItem mySubMenu = new ToolStripMenuItem();

                    OrderMenuStruct myOrderStr = new OrderMenuStruct();
                    myOrderStr.OrderRefID = OrderRefID;
                    myOrderStr.EMSX_Sequence = EMSX_Sequence;
                    myOrderStr.BBG_Ticker = BBG_Ticker;
                    myOrderStr.FormattedQuantity = FormattedQuantity;
                    myOrderStr.FormattedFillAmount = FormattedFillAmount;
                    myOrderStr.FormattedRoutedAmount = FormattedRoutedAmount;
                    myOrderStr.Quantity = Quantity;
                    myOrderStr.FillAmount = FillAmount;
                    myOrderStr.FillPrice = FillPrice;
                    myOrderStr.RoutedAmount = RoutedAmount;
                    myOrderStr.Side = Side;
                    myOrderStr.myParentForm = this;

                    // Fill Status Order Menu
                    mySubMenu = new ToolStripMenuItem("EMSX Details");
                    myOrderStr.Instruction = "ShowFillStatus";
                    mySubMenu.Tag = myOrderStr;
                    mySubMenu.MouseUp += new MouseEventHandler(dg_OrdersSystemMenuItem_Click);
                    myMenu.Items.Add(mySubMenu);

                    // Resize to Fill Menu
                    myMenu.Items.Add("-");
                    mySubMenu = new ToolStripMenuItem("Resize Order to the Fill Amount");
                    myOrderStr.Instruction = "ResizeToFill";
                    mySubMenu.Tag = myOrderStr;
                    mySubMenu.MouseUp += new MouseEventHandler(dg_OrdersSystemMenuItem_Click);
                    myMenu.Items.Add(mySubMenu);

                    // Resize to Fill Menu
                    mySubMenu = new ToolStripMenuItem("Resize Order to the Routed Amount");
                    myOrderStr.Instruction = "ResizeToRouted";
                    mySubMenu.Tag = myOrderStr;
                    mySubMenu.MouseUp += new MouseEventHandler(dg_OrdersSystemMenuItem_Click);
                    myMenu.Items.Add(mySubMenu);

                    // Double Order Menu
                    mySubMenu = new ToolStripMenuItem("Double Order Size");
                    myOrderStr.Instruction = "ResizeToDouble";
                    mySubMenu.Tag = myOrderStr;
                    mySubMenu.MouseUp += new MouseEventHandler(dg_OrdersSystemMenuItem_Click);
                    myMenu.Items.Add(mySubMenu);

                    // Resize Order Menu
                    myMenu.Items.Add("-");
                    mySubMenu = new ToolStripMenuItem("Resize Order");
                    myOrderStr.Instruction = "ResizeOrder";
                    mySubMenu.Tag = myOrderStr;
                    mySubMenu.MouseUp += new MouseEventHandler(dg_OrdersSystemMenuItem_Click);
                    myMenu.Items.Add(mySubMenu);

                    // Delete Order Menu
                    myMenu.Items.Add("-");
                    mySubMenu = new ToolStripMenuItem("Delete Order");
                    myOrderStr.Instruction = "Delete";
                    mySubMenu.Tag = myOrderStr;
                    mySubMenu.MouseUp += new MouseEventHandler(dg_OrdersSystemMenuItem_Click);
                    myMenu.Items.Add(mySubMenu);

                    // Show the Menu
                    myMenu.Show(myLocation);

                }
            }
            catch { }

        } //dg_Orders_CellMouseClick()

        public static void dg_OrdersSystemMenuItem_Click(object sender, MouseEventArgs e)
        {
            //
            // Purpose: A generic menu click event for the Order Right-Click
            //

            // Local Variables
            ToolStripMenuItem ts_From = (ToolStripMenuItem)sender;
            OrderMenuStruct myOrderStr = (OrderMenuStruct)ts_From.Tag;
            String myQuestion = "";
            String myMessage = "";
            String OrderRefID = myOrderStr.OrderRefID;
            Decimal Quantity = myOrderStr.Quantity;
            Decimal FillAmount = myOrderStr.FillAmount;
            Decimal RoutedAmount = myOrderStr.RoutedAmount;
            ProcessOrders myForm = myOrderStr.myParentForm;
            int myScrollRow = myForm.dg_Orders.FirstDisplayedScrollingRowIndex;

            
            if (OrderRefID.Length > 0)
            {
                switch (myOrderStr.Instruction)
                {
                    case "ShowFillStatus":
                        ShowFillStatus fs = new ShowFillStatus();
                        SystemLibrary.FormExists(fs, true);

                        fs.FromParent(myForm, myOrderStr.BBG_Ticker, myOrderStr.EMSX_Sequence);
                        fs.Show();
                        break;

                    case "Delete":
                        if (myOrderStr.FillAmount != 0)
                            myQuestion = "WARNING: This order has already been filled " + myOrderStr.FillAmount.ToString("#,###") + " @" + myOrderStr.FillPrice.ToString("#,###.####") + ", so you should 'Resize to Fill' rather than Delete\r\n\r\n";
                        else if (myOrderStr.RoutedAmount != 0)
                            myQuestion = "WARNING: Part of this order has already been routed " + myOrderStr.RoutedAmount.ToString("#,###") + " and should be cancelled first.\r\n\r\n";
                        myQuestion = myQuestion +
                                     "You are about to Delete the Order for\r\n\r\n" +
                                     myOrderStr.Side + " " + myOrderStr.Quantity + "  " + myOrderStr.BBG_Ticker + "\r\n";
                        if (myOrderStr.EMSX_Sequence.Length>0)
                            myQuestion = myQuestion + "\r\nBloomberg Order# " + myOrderStr.EMSX_Sequence;
                        myQuestion = myQuestion + "\r\n\r\n" + "Is this Ok?";

                        if (MessageBox.Show(myQuestion, "Delete Order", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if (myOrderStr.EMSX_Sequence.Length > 0 && myForm.ParentForm1.isBloombergUser)
                                myMessage = SendToBloomberg.EMSXAPI_Delete(OrderRefID);

                            if (myMessage.Length > 0)
                            {
                                if (MessageBox.Show("We received the UNEXPECTED message below from Bloomberg while trying to delete the order.\r\n\r\n" +
                                                   "Do you wish to continue with the Delete?\r\n\r\n" +
                                                   "=============================================\r\n\r\n\r\n" +
                                                    myMessage, "Delete Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                                {
                                    return;
                                }
                            }

                            // Delete the Order
                            SystemLibrary.SQLExecute("Exec sp_ReverseOrder '" + OrderRefID + "'");
                            myForm.LoadProcessOrders();
                            if (myScrollRow > 0 && myForm.dg_Orders.Rows.Count >= myScrollRow)
                                myForm.dg_Orders.FirstDisplayedScrollingRowIndex = myScrollRow;

                            // Update The Positions Table
                            myForm.NeedUpdate = true;
                            myForm.NeedFullUpdate = true;
                            // Refresh the Portfolio Tab
                            //myForm.ParentForm1.LoadPortfolioIncr();

                            // Report what Bloomberg says
                            if (myMessage.Length > 0)
                            {
                                SystemLibrary.SQLExecute("Exec sp_Fills_From_EMSX_API");
                                MessageBox.Show("Changed in the Portfolio Management System, but received this messagae from Bloomberg.\r\n\r\n" + myMessage, myOrderStr.Instruction);
                            }
                        }
                        break;
                    case "ResizeToFill":
                        myQuestion = "You are about to Resize the Order for\r\n\r\n" +
                                           myOrderStr.Side + " " + myOrderStr.FormattedQuantity + "  " + myOrderStr.BBG_Ticker + "\r\n\n" +
                                           "To the FillAmount of " + myOrderStr.FormattedFillAmount + "\r\n\r\n" +
                                           "Is this Ok?";

                        if (Math.Abs(RoutedAmount) > Math.Abs(FillAmount))
                        {
                            myQuestion = "WARNING: The Routed part of this order (" + myOrderStr.RoutedAmount.ToString("#,###")  +") is greater than the Filled Amount (" + myOrderStr.FillAmount.ToString("#,###") + ") and should be cancelled first.\r\n\r\n" + 
                                         myQuestion;
                        }

                        if (FillAmount == 0)
                        {
                            MessageBox.Show("The Order has not been Filled (Fill Amount = 0).\r\n\r\nPlease use the [Delete Order] menu if that is your intention", "Resize order to Fill Amount");
                        }
                        else if (MessageBox.Show(myQuestion, "Resize Order", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if (myForm.ParentForm1.isBloombergUser)
                                myMessage = SendToBloomberg.EMSXAPI_Modify(OrderRefID, myOrderStr.BBG_Ticker, Convert.ToInt32(myOrderStr.FillAmount));
                            else
                                myMessage = "";

                            // Resize the Order
                            SystemLibrary.SQLExecute("Exec sp_ResizeOrder '" + OrderRefID + "', " + myOrderStr.FillAmount.ToString());
                            myForm.LoadProcessOrders();
                            if (myScrollRow > 0 && myForm.dg_Orders.Rows.Count >= myScrollRow)
                                myForm.dg_Orders.FirstDisplayedScrollingRowIndex = myScrollRow;

                            // Update The Positions Table
                            myForm.NeedUpdate = true;
                            // Refresh the Portfolio Tab
                            //myForm.ParentForm1.LoadPortfolioIncr();

                            // Report what Bloomberg says
                            if (myMessage.Length > 0)
                                MessageBox.Show("Changed in the Portfolio Management System, but received this messagae from Bloomberg.\r\n\r\n" + myMessage, myOrderStr.Instruction);
                        }
                        break;
                    case "ResizeToRouted":
                        if (RoutedAmount == 0)
                        {
                            MessageBox.Show("The Order has not been Routed (Routed Amount = 0).\r\n\r\nPlease use the [Delete Order] menu if that is your intention", "Resize order to Routed Amount");
                        }
                        else if (MessageBox.Show("You are about to Resize the Order for\r\n\r\n" +
                                           myOrderStr.Side + " " + myOrderStr.FormattedQuantity + "  " + myOrderStr.BBG_Ticker + "\r\n\n" +
                                           "To the RoutedAmount of " + myOrderStr.FormattedRoutedAmount + "\r\n\r\n" +
                                           "Is this Ok?", "Resize Order", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if (myForm.ParentForm1.isBloombergUser)
                                myMessage = SendToBloomberg.EMSXAPI_Modify(OrderRefID, myOrderStr.BBG_Ticker, Convert.ToInt32(myOrderStr.RoutedAmount));
                            else
                                myMessage = "";
                            

                            // Resize the Order
                            SystemLibrary.SQLExecute("Exec sp_ResizeOrder '" + OrderRefID + "', " + myOrderStr.RoutedAmount.ToString());
                            myForm.LoadProcessOrders();
                            if (myScrollRow > 0 && myForm.dg_Orders.Rows.Count >= myScrollRow)
                                myForm.dg_Orders.FirstDisplayedScrollingRowIndex = myScrollRow;

                            // Update The Positions Table
                            myForm.NeedUpdate = true;
                            // Refresh the Portfolio Tab
                            //myForm.ParentForm1.LoadPortfolioIncr();

                            // Report what Bloomberg says
                            if (myMessage.Length > 0)
                                MessageBox.Show("Changed in the Portfolio Management System, but received this messagae from Bloomberg.\r\n\r\n" + myMessage, myOrderStr.Instruction);
                        }
                        break;
                    case "ResizeToDouble":
                        Decimal DoubleQuantity = myOrderStr.Quantity * 2;
                        if (MessageBox.Show("You are about to Double the Order for\r\n\r\n" +
                                           myOrderStr.Side + " " + myOrderStr.FormattedQuantity + "  " + myOrderStr.BBG_Ticker + "\r\n\n" +
                                           "Is this Ok?", "Resize Order", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if (myForm.ParentForm1.isBloombergUser)
                                myMessage = SendToBloomberg.EMSXAPI_Modify(OrderRefID, myOrderStr.BBG_Ticker, Convert.ToInt32(DoubleQuantity));
                            else
                                myMessage = "";

                            // Resize the Order
                            SystemLibrary.SQLExecute("Exec sp_ResizeOrder '" + OrderRefID + "', " + DoubleQuantity.ToString());
                            myForm.LoadProcessOrders();
                            if (myScrollRow > 0 && myForm.dg_Orders.Rows.Count >= myScrollRow)
                                myForm.dg_Orders.FirstDisplayedScrollingRowIndex = myScrollRow;

                            // Update The Positions Table
                            myForm.NeedUpdate = true;
                            // Refresh the Portfolio Tab
                            //myForm.ParentForm1.LoadPortfolioIncr();

                            // Report what Bloomberg says
                            if (myMessage.Length > 0)
                                MessageBox.Show("Changed in the Portfolio Management System, but received this messagae from Bloomberg.\r\n\r\n" + myMessage, myOrderStr.Instruction);
                        }
                        break;
                    case "ResizeOrder":
                        String myValue = myOrderStr.FormattedQuantity;
                        long myQty;
                        String mySql;

                        if (SystemLibrary.InputBox("Resize Order for " + myOrderStr.BBG_Ticker + " from " + myOrderStr.FormattedQuantity, "Change the Size of the Order OR Cancel", ref myValue, myForm.validate_ResizOrder, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            myQty = SystemLibrary.ToInt32(myValue);
                            if (Math.Sign(myQty) != Math.Sign(myOrderStr.Quantity))
                            {
                                MessageBox.Show(@"Cannot swap between Buy & Sell.", "Resize aborted");
                                return;
                            }
                            if (Math.Abs(myQty) < Math.Abs(myOrderStr.FillAmount))
                            {
                                MessageBox.Show(@"Cannot resize below the Fill Amount of " + myOrderStr.FillAmount.ToString("#,###") + ".", "Resize aborted");
                                return;
                            }
                            if (Math.Abs(myQty) < Math.Abs(myOrderStr.RoutedAmount))
                            {
                                MessageBox.Show(@"Cannot resize below the Routed Amount of " + myOrderStr.RoutedAmount.ToString("#,###") + ".", "Resize aborted");
                                return;
                            }

                            if (myForm.ParentForm1.isBloombergUser)
                                myMessage = SendToBloomberg.EMSXAPI_Modify(OrderRefID, myOrderStr.BBG_Ticker, (Int32)myQty);
                            else
                                myMessage = "";

                            mySql = "Exec sp_ResizeOrder '" + OrderRefID + "', " + myQty.ToString();
                            SystemLibrary.SQLExecute(mySql);

                            myForm.LoadProcessOrders();
                            if (myScrollRow > 0 && myForm.dg_Orders.Rows.Count >= myScrollRow)
                                myForm.dg_Orders.FirstDisplayedScrollingRowIndex = myScrollRow;

                            // Update The Positions Table
                            myForm.NeedUpdate = true;
                            // Refresh the Portfolio Tab
                            //myForm.ParentForm1.LoadPortfolioIncr();

                            // Report what Bloomberg says
                            if (myMessage.Length > 0)
                                MessageBox.Show("Changed in the Portfolio Management System, but received this messagae from Bloomberg.\r\n\r\n" + myMessage, myOrderStr.Instruction);
                        }
                        break;
                }

                // Go back the that row in dg_Orders
                myForm.dg_OrdersSelectRowbyOrderRefID(OrderRefID);
            }
        } //dg_OrdersSystemMenuItem_Click()

        private void dg_Orders_MouseClick(object sender, MouseEventArgs e)
        {
            CXLocation = dg_Orders.Location.X + e.Location.X + 5;
            CYLocation = dg_Orders.Location.Y + e.Location.Y;

        } //dg_OrdersSystemMenuItem_Click()

        private void bt_EMSX_Click(object sender, EventArgs e)
        {
            // Fire up EMSX
            SystemLibrary.BBGBloombergCommand(1, "EMSX<go>");

        } //bt_EMSX_Click()

        private void bt_Split_Report_Click(object sender, EventArgs e)
        {
            // Local Variables
            String TempFile;
            String myMessage;
            String[] mySplitMessage;
            String myUserName;
            String mySql;


            // Report the Split details to Notepad depending on the Order Type.
            TempFile = Path.GetTempPath() + @"\OrderSplitDetails.txt";
            if (File.Exists(TempFile))
                File.Delete(TempFile);
            myUserName = SystemLibrary.GetUserName();
            if (Order.Side == "SS")
            {
                mySql = "Select min(CustodianMap.CustodianID) " +
                        "from	CustodianMap, " +
                        "		Accounts, " +
                        "		Fund, " +
                        "       Orders_Split " +
                        "Where	Fund.FundID = Orders_Split.FundID " +
                        "And		Orders_Split.OrderRefID = '" + Order.OrderRefID + "' " +
                        "And	Accounts.FundID = Fund.ParentFundID " +
                        "And	CustodianMap.AccountID = Accounts.AccountID";
                String myCustodian = SystemLibrary.ToString(SystemLibrary.SQLSelectInt32(mySql));

                myMessage = SystemLibrary.SQLSelectString("Exec sp_OrderSplit_Details '" + Order.OrderRefID + "', '" + myUserName + "', " + myCustodian);
            }
            else
                myMessage = SystemLibrary.SQLSelectString("Exec sp_OrderSplit_Details '" + Order.OrderRefID + "', '" + myUserName + "' ");
            if (myMessage.Length==0)
                myMessage = "Sorry could not get Split details for this Order at the moment";
            mySplitMessage = myMessage.Split("\r\n".ToCharArray());
            File.WriteAllLines(TempFile, mySplitMessage);
            if (File.Exists(TempFile))
                System.Diagnostics.Process.Start("notepad", TempFile);
            else
                MessageBox.Show("Problem producing File with Message Details for Order.");

        } //bt_Split_Report_Click()

        private void ProcessOrders_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);

        } //ProcessOrders_Shown()

        private void dg_Fills_KeyUp(object sender, KeyEventArgs e)
        {
            // Local Variables

            // Deal with Ctrl-V            
            if (e.Control == true && e.KeyValue == (int)Keys.V)
            {
                // Takes a Tab seperated or CR/LF seperated set of data
                
                String myData;
                DataTable dt_InData = new DataTable();
                Boolean isAllStrings = true;
                Boolean ExcludeLastRow = false;
                Decimal myValue;
                Decimal ExpectedQuantity = SystemLibrary.ToDecimal(tb_TotalFillQuality.Text);
                int QuantityColumn = -1;
                int PriceColumn = -1;
                RichTextBox rtb_InData = new RichTextBox();

                // Mark as Manual Override
                if (!cb_ManualOverride.Checked)
                    cb_ManualOverride_CheckedChanged(null,null);

                /*
                 * Excel seems to be able to deal with formats better than I can
                 * - A more effecient mechanism is to not copy back to the clipboard, but to use Excel to solve below,
                 *  but that would require recoding the logic.
                 */
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook wb = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
                ws.Paste(ws.Cells[1, 1], Missing.Value);
                ws.UsedRange.Copy(Missing.Value);
                wb.Close(false,Missing.Value,Missing.Value);
                ws = null;
                wb = null;
                xlApp.Quit();
                xlApp = null;

                // Get the new clipboard created by excel
                IDataObject inClipboard = Clipboard.GetDataObject();
                myData = inClipboard.GetData(DataFormats.Text).ToString();
                String[] myRows = myData.Split("\r\n".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);

                // Examine columns for Quantity & Price
                for (int i = 0; i < myRows.Length; i++)
                {
                    isAllStrings = true;
                    String[] myColumns = myRows[i].Split('\t');
                    for (int j = 0; j < myColumns.Length; j++)
                    {
                        if (i == 0)
                        {
                            dt_InData.Columns.Add(new DataColumn("Column" + j, typeof(Decimal)));
                        }

                        // First row might be just a header. (see if all strings)
                        if (decimal.TryParse(myColumns[j], out myValue))
                        {
                            isAllStrings = false;
                            if (i == myRows.Length-1)
                            {
                                // Last Row might just be totals, so want to avoid this
                                Decimal SumValue = SystemLibrary.ToDecimal(dt_InData.Compute("Sum(Column" + j+")", ""));
                                if (SumValue != 0 && SumValue == myValue)
                                    ExcludeLastRow = true;
                            }
                        }
                        else
                            myColumns[j] = "0"; // Convert Strings to ""
                    }

                    if (!(isAllStrings || ExcludeLastRow))
                        dt_InData.Rows.Add(myColumns);
                }

                // Now find the Fill and price columns
                for (int i = 0; i < dt_InData.Columns.Count; i++)
                {
                    Decimal SumValue = SystemLibrary.ToDecimal(dt_InData.Compute("Sum(Column" + i + ")", ""));
                    if (SumValue == Math.Abs(ExpectedQuantity))
                        QuantityColumn = i;
                    else if (SumValue != 0)
                        PriceColumn = i;
                }
                if (QuantityColumn != -1 && PriceColumn != -1)
                {
                    // Now fill the dg_Fills
                    for (int i = 0; i < dt_InData.Rows.Count; i++)
                    {
                        int myRow = dg_Fills.Rows.Add();

                        Decimal myQty = SystemLibrary.ToDecimal(dt_InData.Rows[i]["Column" + QuantityColumn]);
                        if (Math.Sign(myQty) != Math.Sign(ExpectedQuantity))
                            myQty = myQty * -1;
                        dg_Fills["f_FillAmount", myRow].Value = myQty;
                        ParentForm1.SetColumn(dg_Fills, "f_FillAmount", myRow);
                        dg_Fills["f_FillPrice", myRow].Value = SystemLibrary.ToDecimal(dt_InData.Rows[i]["Column" + PriceColumn]);
                        dg_Fills["f_OrderRefID", myRow].Value = Order.OrderRefID;
                        dg_Fills["f_FillNo", myRow].Value = (int)(i + 1);
                        dg_Fills["f_BBG_Ticker", myRow].Value = Order.BBG_Ticker;
                        dg_Fills["f_ManualFill", myRow].Value = "Y";
                        dg_Fills["f_Confirmed", myRow].Value = "Y";
                        dg_Fills["f_Amount", myRow].Value = Order.Quantity;
                        dg_Fills["f_RoutedAmount", myRow].Value = 0;
                        dg_Fills["f_Side", myRow].Value = Order.Side;
                        dg_Fills["f_TradeDate", myRow].Value = Order.TradeDate.ToString(dg_Fills.Columns["f_TradeDate"].DefaultCellStyle.Format);
                        DataGridViewComboBoxColumn dcb_Broker = (DataGridViewComboBoxColumn)dg_Fills.Columns["f_BrokerID"];
                        DataTable dt_Brokers = (DataTable)dcb_Broker.DataSource;
                        if (dt_Brokers.Rows.Count == 1)
                        {
                            dg_Fills["f_BrokerID", myRow].Value = Convert.ToInt32(dt_Brokers.Rows[0]["BrokerID"]);
                            dg_Fills["f_BBG_Broker", myRow].Value = dt_Brokers.Rows[0]["BBG_Broker"].ToString();
                            dg_Fills["f_BBG_StrategyType", myRow].Value = dt_Brokers.Rows[0]["BBG_StrategyType"].ToString();
                        }
                    }
                }
                dg_Fills.Refresh();

                // Set Unfilled Quantity
                Int32 FilledQuantity = 0;
                foreach (DataGridViewRow dgr in dg_Fills.Rows)
                {
                    FilledQuantity = FilledQuantity + SystemLibrary.ToInt32(dgr.Cells["f_FillAmount"].Value);
                }
                //FilledQuantity = FilledQuantity + SystemLibrary.ToInt32(dg_Fills.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                tb_UnfilledQuantity.Text = (Order.Quantity - FilledQuantity).ToString("N0");
                SystemLibrary.SetTextBoxColour(tb_UnfilledQuantity, (Order.Quantity - FilledQuantity));

                // Check if can Save
                if (Math.Abs(Order.Quantity) >= Math.Abs(FilledQuantity) && Math.Sign(Order.Quantity) == Math.Sign(FilledQuantity))
                    bt_SaveFill.Enabled = true;
                else
                    bt_SaveFill.Enabled = false;

            }

        } //dg_Fills_KeyUp()

        private void cb_Portfolio_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Local Variables
            String myFilter = "";

            if (cb_Portfolio.Text != "<All>")
                myFilter = "Portfolio = '" + cb_Portfolio.Text + "'";
            if (cb_PartFilled.Checked)
            {
                if (myFilter.Length > 0)
                    myFilter = myFilter + " And ";
                myFilter = myFilter + "FillAmount<>Quantity";
            }

            dv_Orders.RowFilter = myFilter;
            SetFormat(false);

            // Remember the change if made by the user. 
            // - On initialisation, this has a single record of "<All>"
            if (cb_Portfolio.Items.Count>1)
                Last_cb_Portfolio_Text = cb_Portfolio.Text;

        } //cb_Portfolio_SelectedIndexChanged()

        private void dg_Orders_Sorted(object sender, EventArgs e)
        {
            SetFormat(false);
        } //dg_Orders_Sorted()


        private void cb_PartFilled_CheckedChanged(object sender, EventArgs e)
        {
            cb_Portfolio_SelectedIndexChanged(null, null);
        }

        private void dg_Orders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        } //cb_PartFilled_CheckedChanged()



    }
}
