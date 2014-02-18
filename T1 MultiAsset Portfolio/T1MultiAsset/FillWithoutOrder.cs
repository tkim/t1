using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace T1MultiAsset
{
    public partial class FillWithoutOrder : Form
    {
        // Public Variables
        public DataTable dt_FillWithoutOrder;
        public DataTable dt_Match;
        public Form1 ParentForm1;
        public DataTable dt_Fund;
        public DataTable dt_Portfolio;
        public object LastValue;

        public struct OrderStruct
        {
            public String OrderRefID;
            public String BBG_Ticker;
            public String Side;
            public String crncy;
            public String Country;
            public String TradeDate;
            public Int32 Qty_Fill;
            public Int32 Round_Lot_Size;
        }
        public static OrderStruct Order = new OrderStruct();


        public FillWithoutOrder()
        {
            InitializeComponent();
        }

        private void FillWithoutOrder_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            LoadFillWithoutOrder();

        } //FillWithoutOrder_Load()


        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;
            // Load up the Fund & Portfolio Datatables
            ParentForm1.CopyDataTable(ref dt_Fund, ref dt_Portfolio);

            String mySql = "Select FundId, FundName, FundAmount, crncy, ShortName " +
                           "From   Fund " +
                           "Where  Active = 'Y' " +
                           "And   AllowTrade = 'Y' " +
                           "Order By 2 ";
            dt_Fund = SystemLibrary.SQLSelectToDataTable(mySql);

            // 14-Nov-2011 Found the ParentForm could have a different Portfolio set than needed for settlement.
            mySql = "Select PortfolioId, PortfolioName, PortfolioAmount, crncy " +
                           "From   Portfolio " +
                           "Where  Active = 'Y' " +
                           "Order By 2 ";
            dt_Portfolio = SystemLibrary.SQLSelectToDataTable(mySql);

        } //FromParent()


        private void LoadFillWithoutOrder()
        {
            // Local Variables
            String mySql;

            mySql = "SELECT Fills.OrderRefID, Fills.TradeDate, Fills.BBG_Ticker, Fills.Side, Fills.Amount, Fills.FillAmount, Fills.FillPrice, Securities.Round_Lot_size, Securities.crncy, Securities.Country_Full_Name " +
                    "FROM   Fills LEFT OUTER JOIN " +
                    "           Securities ON Fills.BBG_Ticker = Securities.BBG_Ticker " +
                    "Where Not Exists (	Select	'x' " +
                    "					From	Orders " +
                    "					Where	Orders.OrderRefId = Fills.OrderRefId " +
                    "				 ) " +
                    "Order by Fills.OrderRefId";
            dt_FillWithoutOrder = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_FillWithoutOrder.DataSource = dt_FillWithoutOrder;
            dg_FillWithoutOrder.Columns["OrderRefId"].Visible = false;
            dg_FillWithoutOrder.Columns["Round_Lot_size"].Visible = false;
            dg_FillWithoutOrder.Columns["crncy"].Visible = false;
            dg_FillWithoutOrder.Columns["Country_Full_Name"].Visible = false;
            dg_FillWithoutOrder.Columns["TradeDate"].DefaultCellStyle.Format = "F"; // Long Date/Time
            dg_FillWithoutOrder.Columns["TradeDate"].DefaultCellStyle.ForeColor = Color.Blue;
            dg_FillWithoutOrder.Columns["BBG_Ticker"].DefaultCellStyle.ForeColor = Color.Blue;
            ParentForm1.SetFormatColumn(dg_FillWithoutOrder, "Amount", Color.Empty, Color.Empty, "N0", "0");
            ParentForm1.SetFormatColumn(dg_FillWithoutOrder, "FillAmount", Color.Empty, Color.Empty, "N0", "0");
            ParentForm1.SetFormatColumn(dg_FillWithoutOrder, "FillPrice", Color.Empty, Color.Empty, "N2", "0");

            // Loop on all columns and set the Autosize mode & Header Text
            for (int i = 0; i < dg_FillWithoutOrder.Columns.Count; i++)
            {
                dg_FillWithoutOrder.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dg_FillWithoutOrder.Columns[i].HeaderText = dg_FillWithoutOrder.Columns[i].HeaderText.Replace('_', ' ');
            }

            for (Int32 i = 0; i < dg_FillWithoutOrder.Rows.Count; i++) // Last row in dg_Port is a blank row
            {
                ParentForm1.SetColumn(dg_FillWithoutOrder, "Amount", i);
                ParentForm1.SetColumn(dg_FillWithoutOrder, "FillAmount", i);
            }

            tb_TotalQuantity.Text = "0";
            dg_Match.Enabled = false;
            bt_CreateOrder.Enabled = false;
            dg_FillWithoutOrder.Focus();

        } //LoadFillWithoutOrder()

        private void FillWithoutOrder_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Let the Parent know this has close
            if (ParentForm1 != null)
                ParentForm1.LoadActionTab(true);

        } //FillWithoutOrder_FormClosed()

        private void dg_FillWithoutOrder_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Local Variables
            String mySql;
            Int32 Qty_Remain;

            if (e.RowIndex < 0)
                return;
            Order.OrderRefID = dg_FillWithoutOrder["OrderRefID", e.RowIndex].Value.ToString();
            Order.BBG_Ticker = dg_FillWithoutOrder["BBG_Ticker", e.RowIndex].Value.ToString();
            Order.crncy = dg_FillWithoutOrder["crncy", e.RowIndex].Value.ToString();
            Order.Country = dg_FillWithoutOrder["Country_Full_Name", e.RowIndex].Value.ToString();
            Order.Side = dg_FillWithoutOrder["Side", e.RowIndex].Value.ToString();
            Order.TradeDate = Convert.ToDateTime(dg_FillWithoutOrder["TradeDate", e.RowIndex].Value).ToString("dd-MMMM-yyyy");
            Order.Qty_Fill = Convert.ToInt32(dg_FillWithoutOrder["Amount", e.RowIndex].Value);
            Order.Round_Lot_Size = Convert.ToInt32(dg_FillWithoutOrder["Round_Lot_Size", e.RowIndex].Value);
            Qty_Remain = Order.Qty_Fill;

            mySql = "Select	FundId,PortfolioID,BBG_Ticker,IsNull(Quantity,0) + IsNull(Qty_Order,0) as Quantity, 0 as Qty_Fill, Round_Lot_Size " +
                    "From	Positions " +
                    "Where	EffectiveDate = '" + Order.TradeDate + "' " +
                    "And	BBG_Ticker = '" + Order.BBG_Ticker + "' " +
                    "And	Status <> 'Fill without an Order' " +
                    "And	Sign(IsNull(Quantity,0) + IsNull(Qty_Order,0)) <> Sign(" + Order.Qty_Fill.ToString() + ") ";

            // Load the Match Box
            dt_Match = SystemLibrary.SQLSelectToDataTable(mySql);
            DataGridViewComboBoxColumn dcb = (DataGridViewComboBoxColumn)dg_Match.Columns["FundID"];
            dcb.DataSource = dt_Fund;
            dcb.DisplayMember = "FundName";
            dcb.ValueMember = "FundID";
            dcb = (DataGridViewComboBoxColumn)dg_Match.Columns["PortfolioID"];
            dcb.DataSource = dt_Portfolio;
            dcb.DisplayMember = "PortfolioName";
            dcb.ValueMember = "PortfolioID";

            dg_Match.Rows.Clear();
            foreach(DataRow dr in dt_Match.Rows)
            {
                int myRow = dg_Match.Rows.Add();
                dg_Match["FundID", myRow].Value = dr["FundID"];
                dg_Match["PortfolioID", myRow].Value = dr["PortfolioID"];
                dg_Match["BBG_Ticker", myRow].Value = dr["BBG_Ticker"];
                dg_Match["Quantity", myRow].Value = dr["Quantity"];
                dg_Match["Qty_Fill", myRow].Value = dr["Qty_Fill"];
                dg_Match["Round_Lot_Size", myRow].Value = dr["Round_Lot_Size"];
                ParentForm1.SetColumn(dg_Match, "Quantity", myRow);
                ParentForm1.SetColumn(dg_Match, "Qty_Fill", myRow);
            }
            bt_CreateOrder.Enabled = false;
            dg_Match.Enabled = true;

            // If only 1 row, then fully fill
            if (dt_Match.Rows.Count == 1)
            {
                dg_Match["Qty_Fill", 0].Value = Order.Qty_Fill;
                Qty_Remain = 0;
                bt_CreateOrder.Enabled = true;
                ParentForm1.SetColumn(dg_Match, "Quantity", 0);
                ParentForm1.SetColumn(dg_Match, "Qty_Fill", 0);
            }

            // If no rows, then fully fill a single line
            if (dt_Match.Rows.Count == 0)
            {
                int myRow = dg_Match.Rows.Add();
                dg_Match["FundID", myRow].Value = -1;
                dg_Match["PortfolioID", myRow].Value = -1;
                dg_Match["BBG_Ticker", myRow].Value = Order.BBG_Ticker;
                dg_Match["Quantity", myRow].Value = 0;
                dg_Match["Qty_Fill", myRow].Value = Order.Qty_Fill;
                Qty_Remain = 0;
                dg_Match["Round_Lot_Size", myRow].Value = Order.Round_Lot_Size;
                ParentForm1.SetColumn(dg_Match, "Quantity", myRow);
                ParentForm1.SetColumn(dg_Match, "Qty_Fill", myRow);
                bt_CreateOrder.Enabled = true;
            }
            tb_TotalQuantity.Text = Order.Qty_Fill.ToString("N0");
            if (Order.Qty_Fill < 0)
                tb_TotalQuantity.ForeColor = Color.Red;
            else if (Order.Qty_Fill > 0)
                tb_TotalQuantity.ForeColor = Color.Green;
            else
                tb_TotalQuantity.ForeColor = Color.Black;

            tb_MissingQuantity.Text = Qty_Remain.ToString("N0");
            if (Qty_Remain < 0)
                tb_MissingQuantity.ForeColor = Color.Red;
            else if (Qty_Remain > 0)
                tb_MissingQuantity.ForeColor = Color.Green;
            else
                tb_MissingQuantity.ForeColor = Color.Black;

        } //dg_FillWithoutOrder_CellClick()

        private void dg_Match_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            LastValue = dg_Match[e.ColumnIndex, e.RowIndex].Value;
        } //dg_Match_CellBeginEdit

        private void dg_Match_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Local Variables
            Int32 Qty_Remain = Order.Qty_Fill;
            Int32 Qty;

            if (dg_Match.Columns[e.ColumnIndex].Name == "Qty_Fill")
            {
                // Add all the Qty_Fill and calculate the MissingQuantity
                foreach (DataGridViewRow dgr in dg_Match.Rows)
                {
                    Qty = Convert.ToInt32(dgr.Cells["Qty_Fill"].Value);
                    if (Math.Sign(Qty) != Math.Sign(Order.Qty_Fill) && Qty!=0)
                    {
                        MessageBox.Show("Wrong direction on Fill Quantity");
                        dg_Match[e.ColumnIndex, e.RowIndex].Value = LastValue;
                        return;
                    }
                    Qty_Remain = Qty_Remain - Qty;
                }
            }
            if (Qty_Remain==0)
                bt_CreateOrder.Enabled = true;
            else
                bt_CreateOrder.Enabled = false;
            tb_MissingQuantity.Text = Qty_Remain.ToString("N0");
            if (Qty_Remain < 0)
                tb_MissingQuantity.ForeColor = Color.Red;
            else if (Qty_Remain > 0)
                tb_MissingQuantity.ForeColor = Color.Green;
            else
                tb_MissingQuantity.ForeColor = Color.Black;
            if (Convert.ToInt32(dg_Match[e.ColumnIndex, e.RowIndex].Value) < 0)
                dg_Match[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
            else
                dg_Match[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Green;

        } //dg_Match_CellEndEdit() 

        private void dg_Match_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            dg_Match.CurrentRow.Cells["FundID"].Value = -1;
            dg_Match.CurrentRow.Cells["PortfolioID"].Value = -1;
            dg_Match.CurrentRow.Cells["BBG_Ticker"].Value = Order.BBG_Ticker;
            dg_Match.CurrentRow.Cells["Quantity"].Value = 0;
            dg_Match.CurrentRow.Cells["Qty_Fill"].Value = 0;
            dg_Match.CurrentRow.Cells["Round_Lot_Size"].Value = Order.Round_Lot_Size;
            dg_Match.CurrentRow.Cells["Quantity"].Style.ForeColor = Color.Green;
            //dg_Match.CurrentRow.Cells["Quantity"].Style.ForeColor = Color.Green;
        } //dg_Match_UserAddedRow() 

        private void CreateOrder(int inRow)
        {
            // TODO (1) CreateOrder() - Quick Cheat for now - assumes 1 row in dg_Match
            // Local Variables
            String myTable = "Orders";
            String myColumns = "";
            String Ticker = "";
            String Exch = "";
            String YellowKey = "";
            int myRows;
            int FundID;
            int PortfolioID;

            FundID = SystemLibrary.ToInt32(dg_Match["FundID", inRow].Value);
            PortfolioID = SystemLibrary.ToInt32(dg_Match["PortfolioID", inRow].Value);

            SendToBloomberg.EMSTickerSplit(Order.BBG_Ticker, ref Ticker, ref Exch, ref YellowKey);

            // -- Place the Order
            DataTable dt_load = SystemLibrary.SQLBulk_GetDefinition(myColumns, myTable);
            DataRow drSQL = dt_load.NewRow();
            drSQL["OrderRefID"] = Order.OrderRefID;
            drSQL["EMSX_Sequence"] = Order.OrderRefID;
            drSQL["EffectiveDate"] = Order.TradeDate;
            drSQL["BBG_Ticker"] = Order.BBG_Ticker;
            drSQL["Exchange"] = Exch.ToUpper();
            drSQL["Crncy"] = Order.crncy;
            drSQL["Quantity"] = Order.Qty_Fill;
            drSQL["Side"] = SendToBloomberg.GetSide(Ticker, YellowKey, Order.Country, 0, Order.Qty_Fill).ToUpper();
            drSQL["OrderType"] = "";
            // drSQL["Limit"] = dr["Limit"];
            drSQL["TimeinForce"] = "";
            drSQL["UserName"] = SystemInformation.UserName;
            drSQL["UpdateDate"] = SystemLibrary.f_Now();
            drSQL["ProcessedEOD"] = "N";
            drSQL["ManualOrder"] = "N";
            drSQL["Order_Completed"] = "N";
            drSQL["CreatedDate"] = SystemLibrary.f_Now();
            dt_load.Rows.Add(drSQL);
            myRows = SystemLibrary.SQLBulkUpdate(dt_load, myColumns, myTable);
            // Process the Splits
            SystemLibrary.SQLExecute("Exec sp_OrderSplits '" + drSQL["OrderRefID"].ToString() + "', " + FundID.ToString() + ", " + PortfolioID.ToString() + ", " + Order.Round_Lot_Size.ToString());
            // Reprocess the Fill, so get the Fill_Allocations
            SystemLibrary.SQLExecute("Exec sp_ReprocessFillbyRef '" + drSQL["OrderRefID"].ToString() + "' ");

            SystemLibrary.SQLExecute("Exec sp_Update_Positions 'Y' ");
            if (ParentForm1 != null)
                ParentForm1.LoadPortfolio(true);

        } //CreateOrder()

        private void bt_CreateOrder_Click(object sender, EventArgs e)
        {
            // Local Variables
            Boolean FoundSingleMatchRow = false;
            int MatchRow = -1;

            // TODO (2) - Need to check using RoundLot
            // If only one row, then create the Order
            if (dg_Match.Rows.Count == 2) // last row is a blank one.
            {
                // Create the order
                CreateOrder(0);
                // Clean up
                dg_Match.Rows.Clear();
                LoadFillWithoutOrder();
                return;
            }
            // Test the the FundID & PortfolioID for each row are unique
            Hashtable FundIDs = new Hashtable();
            Hashtable PortfolioIDs = new Hashtable();
            for (int i=0;i<dg_Match.Rows.Count;i++)
            {
                if (SystemLibrary.ToInt32(dg_Match["Qty_Fill",i].Value)!=0)
                {
                    if (!FoundSingleMatchRow)
                    {
                        MatchRow = i;
                        FoundSingleMatchRow = true;
                    }
                    else
                        MatchRow = -1;
                }
                if (FundIDs.Contains(Convert.ToInt32(dg_Match["FundID",i].Value)))
                {
                    MessageBox.Show("Cannot have Duplicate Fund Names", "Create Order");
                    return;
                }
                else
                    FundIDs.Add(Convert.ToInt32(dg_Match["FundID",i].Value), Convert.ToInt32(dg_Match["FundID",i].Value));

                if (PortfolioIDs.Contains(Convert.ToInt32(dg_Match["PortfolioID",i].Value)))
                {
                    MessageBox.Show("Cannot have Duplicate Portfolio Names", "Create Order");
                    return;
                }
                else
                    PortfolioIDs.Add(Convert.ToInt32(dg_Match["PortfolioID",i].Value), Convert.ToInt32(dg_Match["PortfolioID",i].Value));
            }

            // Now create a Multi Split Order
            if (MatchRow!=-1)
            {
                // Create the order
                CreateOrder(MatchRow);
                // Clean up
                dg_Match.Rows.Clear();
                LoadFillWithoutOrder();
                return;
            }

            MessageBox.Show("At this stage we can only match to a single Portfolio.\r\n\r\n" + 
                            "Set the Fill Quantity for just one portfolio and process.\r\n" + 
                            "You can then Modify the Order to split across Multiple Portfolios/Funds.",
                            "Create Order");

        } //bt_CreateOrder_Click()

        
        private void bt_Calculator_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc");
        }

       

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

      
        private void dg_Match_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            return;
        }  


    }
}
