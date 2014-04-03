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
    public partial class ShowFillStatus : Form
    {
        // Public Variables
        public Form ParentForm1;
        public int FundID;
        public int PortfolioID;
        public String FundDescription;
        public String BBG_Ticker = "";
        public Decimal Last_Sale;
        public String EMSX_Sequence = "";
        public DataTable dt_ShowFills;
        public Boolean NeedUpdate = false;
        public Boolean NeedFullUpdate = false;
        private int CXLocation = 0;
        private int CYLocation = 0;
        public static Decimal Qty_Order = 0;

        public ShowFillStatus()
        {
            InitializeComponent();
        }

        private void ShowFillStatus_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            LoadFills();
        }

        public void FromParent(Form inForm, int inFundID, int inPortfolioID, String inFundDescription, String inBBG_Ticker, Decimal inLast_Sale)
        {
            ParentForm1 = inForm;
            FundID = inFundID;
            PortfolioID = inPortfolioID;
            FundDescription = inFundDescription;
            BBG_Ticker = inBBG_Ticker;
            Last_Sale = inLast_Sale;

        } // FromParent()

        public void FromParent(Form inForm, String inBBG_Ticker, String inEMSX_Sequence)
        {
            ParentForm1 = inForm;
            FundID = -1;
            PortfolioID = -1;
            FundDescription = "Details for EMSX Order# " + inEMSX_Sequence;
            BBG_Ticker = inBBG_Ticker;
            Last_Sale = 0;
            EMSX_Sequence = inEMSX_Sequence;

        } // FromParent()

        private void bt_EMSX_Click(object sender, EventArgs e)
        {
            // Fire up EMSX
            SystemLibrary.BBGBloombergCommand(1, "EMSX<go>");

        } //bt_EMSX_Click()

        private void bt_Print_Click(object sender, EventArgs e)
        {
            SystemLibrary.PrintScreen(this);

        } //bt_Print_Click() 

        private void LoadFills()
        {
            // Local Variables
            String mySql;
            String QtyFormat = "N0";
            String PriceFormat = "N2";

            if (EMSX_Sequence.Length > 0)
                mySql = "Exec sp_ShowFills -1, -1, '" + BBG_Ticker + "', " + EMSX_Sequence;
            else
                mySql = "Exec sp_ShowFills " + FundID.ToString() + ", " + PortfolioID.ToString() + ", '" + BBG_Ticker + "' ";
            
            dt_ShowFills = SystemLibrary.SQLSelectToDataTable(mySql);
            dgv_ShowFills.DataSource = dt_ShowFills;

            if (dgv_ShowFills.Rows.Count > 0)
            {
                if (SystemLibrary.ToString(dgv_ShowFills.Rows[0].Cells["Sector"].Value) == "Currency")
                {
                    QtyFormat = "N2";
                    PriceFormat = "N8";
                    dgv_Summary.Columns["Order_Quantity"].DefaultCellStyle.Format = QtyFormat;
                    dgv_Summary.Columns["Qty_Routed"].DefaultCellStyle.Format = QtyFormat;
                    dgv_Summary.Columns["Qty_Working"].DefaultCellStyle.Format = QtyFormat;
                    dgv_Summary.Columns["Fill_Quantity"].DefaultCellStyle.Format = QtyFormat;
                }
            }

            try
            {
                SystemLibrary.SetDataGridView(dgv_ShowFills);

                // Prevent Sort
                for(int i=0;i<dgv_ShowFills.Columns.Count;i++)
                    dgv_ShowFills.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                // Set up some column descriptions
                dgv_ShowFills.Columns["Order#"].ToolTipText = "This is the Bloomberg internal reference for the Order.";
                dgv_ShowFills.Columns["Route"].ToolTipText = "This is Order of the Child Route on EMSX.";
                dgv_ShowFills.Columns["Total Order"].ToolTipText = "If 'Total Order' is not the same as 'Qty Order', then the Order was placed across Multiple Portfolio/Funds.";
                dgv_ShowFills.Columns["Qty_Order"].ToolTipText = "What has been Modelled and sent to EMSX for this Order and this Fund/Portfolio set.";
                dgv_ShowFills.Columns["Qty_Routed"].ToolTipText = "The Quantity that has been sent to the Broker to be filled.";
                dgv_ShowFills.Columns["Qty_Working"].ToolTipText = "The Quantity outstanding with the Broker.";
                dgv_ShowFills.Columns["Fill_Quantity"].ToolTipText = "The Quantity that has been filled at the Fill Price";
                dgv_ShowFills.Columns["Profit"].ToolTipText = "Profit set by the difference between the Fill Price or Previous Close Price and the Current Price of @" + Last_Sale.ToString("N2");

                // Now cleanup extra columns
                this.Text = "Show Fill Status '" + FundDescription + "'";
                if (dt_ShowFills.Rows.Count > 0)
                    this.Text = this.Text + " - " + SystemLibrary.ToString(dgv_ShowFills.Rows[0].Cells["Security_Name"].Value);
                if (Last_Sale != 0)
                    this.Text = this.Text + " - Price @" + Last_Sale.ToString(PriceFormat);
                dgv_ShowFills.Columns["Security_Name"].Visible = false;
                dgv_ShowFills.Columns["Order#"].DefaultCellStyle.Format = "#";
                dgv_ShowFills.Columns["CreatedDate"].DefaultCellStyle.Format = "dd-MMM-yyyy HH:mm:ss";
                dgv_ShowFills.Columns["FillDate"].DefaultCellStyle.Format = "dd-MMM-yyyy HH:mm:ss";
                dgv_ShowFills.Columns["Pos_Mult_Factor"].Visible = false;
                dgv_ShowFills.Columns["EffectiveDate"].Visible = false;
                dgv_ShowFills.Columns["Fill_Price"].DefaultCellStyle.Format = "N4";
                dgv_ShowFills.Columns["Limit_Price"].DefaultCellStyle.Format = "N2";
                dgv_ShowFills.Columns["Total Order"].DefaultCellStyle.Format = QtyFormat;
                dgv_ShowFills.Columns["Qty_Order"].DefaultCellStyle.Format = QtyFormat;
                dgv_ShowFills.Columns["Qty_Routed"].DefaultCellStyle.Format = QtyFormat;
                dgv_ShowFills.Columns["Qty_Working"].DefaultCellStyle.Format = QtyFormat;
                dgv_ShowFills.Columns["Fill_Quantity"].DefaultCellStyle.Format = QtyFormat;
                dgv_ShowFills.Columns["FillValue"].Visible = false;
                dgv_ShowFills.Columns["Route"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgv_ShowFills.Columns["Route"].Width = 45;
                dgv_ShowFills.Columns["Sector"].Visible = false;

                // Hide the Profit Columns in Last_Sale = 0
                if (Last_Sale == 0)
                {
                    dgv_ShowFills.Columns["Profit"].Visible = false;
                    dgv_Summary.Columns["Profit"].Visible = false;
                }

                // Loop on Rows 
                for (int i = 0; i < dgv_ShowFills.Rows.Count; i++)
                {
                    String mySide = SystemLibrary.ToString(dgv_ShowFills.Rows[i].Cells["Side"].Value);
                    Boolean FoundRepeat = false;

                    // Add the Price and Calculate the Profit
                    dgv_ShowFills.Rows[i].Cells["Profit"].Value = SystemLibrary.ToDecimal(dgv_ShowFills.Rows[i].Cells["Fill_Quantity"].Value) *
                                                                  SystemLibrary.ToDecimal(dgv_ShowFills.Rows[i].Cells["Pos_Mult_Factor"].Value) *
                                                                  (Last_Sale - SystemLibrary.ToDecimal(dgv_ShowFills.Rows[i].Cells["Fill_Price"].Value));

                    if (i > 0)
                    {
                        // Remove Repeating Total Order Quantity and Qty_Order
                        if (SystemLibrary.ToString(dgv_ShowFills.Rows[i].Cells["Order#"].Value) == SystemLibrary.ToString(dgv_ShowFills.Rows[i - 1].Cells["Order#"].Value))
                        {
                            dgv_ShowFills.Rows[i].Cells["Total Order"].Value = DBNull.Value;
                            dgv_ShowFills.Rows[i].Cells["Qty_Order"].Value = DBNull.Value;
                            FoundRepeat = true;
                        }
                    }

                    if (!FoundRepeat)
                    {
                        // Colour Code First Order/Fill combination
                        switch (mySide)
                        {
                            case "SOD - Short":
                            case "SOD - Long":
                                dgv_ShowFills.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                                dgv_ShowFills.Rows[i].Cells["Side"].Style.ForeColor = Color.Black;
                                break;
                            case "Sell":
                            case "Short":
                                dgv_ShowFills.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;
                                dgv_ShowFills.Rows[i].Cells["Side"].Style.ForeColor = Color.Red;
                                break;
                            default: // Buy
                                dgv_ShowFills.Rows[i].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                                dgv_ShowFills.Rows[i].Cells["Side"].Style.ForeColor = Color.Green;
                                break;
                        }
                    }
                }

                SystemLibrary.SetColumn(dgv_ShowFills, "Total Order");
                SystemLibrary.SetColumn(dgv_ShowFills, "Qty_Order");
                SystemLibrary.SetColumn(dgv_ShowFills, "Qty_Routed");
                SystemLibrary.SetColumn(dgv_ShowFills, "Qty_Working");
                SystemLibrary.SetColumn(dgv_ShowFills, "Fill_Quantity");
                SystemLibrary.SetColumn(dgv_ShowFills, "Profit");

                // Now Setup & Fill the Summary Block
                SetUpSummary();
                SetSummary();
            }
            catch
            {
            }

        } // LoadFills()

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            LoadFills();

        } // bt_Refresh_Click()

        private void bt_Close_Click(object sender, EventArgs e)
        {
            this.Close();

        } // bt_Close_Click()

        private void ShowFillStatus_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);
        } // ShowFillStatus_Shown()

        private void SetUpSummary()
        {
            // Local Variables


            dgv_Summary.Columns["Profit"].ToolTipText = "Profit set by the difference between the Fill Price or Previous Close Price and the Current Price of @" + Last_Sale.ToString("N2");
            dgv_Summary.Rows.Clear();
            dgv_Summary.Rows.Add(4);
            dgv_Summary.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;
            dgv_Summary.Rows[0].Cells["Side"].Value = "SOD";
            dgv_Summary.Rows[1].Cells["Side"].Value = "Buy";
            dgv_Summary.Rows[2].Cells["Side"].Value = "Sell";
            dgv_Summary.Rows[3].DefaultCellStyle.BackColor = Color.LightGray;
            dgv_Summary.Rows[3].Cells["Side"].Value = "EOD";

        } //SetUpSummary()

        private void SetSummary()
        {
            // Local Variables
            Decimal TotalProfit = 0;
            Decimal Pos_Mult_Factor = SystemLibrary.ToDecimal(dt_ShowFills.Compute("Min(Pos_Mult_Factor)", ""));
            Decimal Fill_Quantity; // Moved to this as IFF is unreliable.
            Decimal FillValue;

            // SOD
            dgv_Summary.Rows[0].Cells["Order_Quantity"].Value = dt_ShowFills.Compute("Sum(Qty_Order)", "Side Like 'SOD%'");
            dgv_Summary.Rows[0].Cells["Fill_Quantity"].Value = dgv_Summary.Rows[0].Cells["Order_Quantity"].Value;
            //dgv_Summary.Rows[0].Cells["Fill_Price"].Value = dt_ShowFills.Compute("IIF(Sum(Fill_Quantity)=0,0,Sum(FillValue)/Sum(Fill_Quantity))", "Side Like 'SOD%'");
            Fill_Quantity = SystemLibrary.ToDecimal(dt_ShowFills.Compute("Sum(Fill_Quantity)", "Side Like 'SOD%'"));
            FillValue = SystemLibrary.ToDecimal(dt_ShowFills.Compute("Sum(FillValue)", "Side Like 'SOD%'"));
            if (Fill_Quantity == 0)
                dgv_Summary.Rows[0].Cells["Fill_Price"].Value = DBNull.Value;
            else
                dgv_Summary.Rows[0].Cells["Fill_Price"].Value = FillValue / Fill_Quantity;

            // Buy
            dgv_Summary.Rows[1].Cells["Order_Quantity"].Value = dt_ShowFills.Compute("Sum(Qty_Order)", "Side Like '%Buy%'");
            dgv_Summary.Rows[1].Cells["Qty_Routed"].Value = dt_ShowFills.Compute("Sum(Qty_Routed)", "Side Like '%Buy%'");
            dgv_Summary.Rows[1].Cells["Qty_Working"].Value = dt_ShowFills.Compute("Sum(Qty_Working)", "Side Like '%Buy%'");
            dgv_Summary.Rows[1].Cells["Fill_Quantity"].Value = dt_ShowFills.Compute("Sum(Fill_Quantity)", "Side Like '%Buy%'");
            //dgv_Summary.Rows[1].Cells["Fill_Price"].Value = dt_ShowFills.Compute("IIF(Sum(Fill_Quantity)=0,0,Sum(FillValue)/Sum(Fill_Quantity))", "Side Like '%Buy%'");
            Fill_Quantity = SystemLibrary.ToDecimal(dt_ShowFills.Compute("Sum(Fill_Quantity)", "Side Like '%Buy%'"));
            FillValue = SystemLibrary.ToDecimal(dt_ShowFills.Compute("Sum(FillValue)", "Side Like '%Buy%'"));
            if (Fill_Quantity == 0)
                dgv_Summary.Rows[1].Cells["Fill_Price"].Value = DBNull.Value;
            else
                dgv_Summary.Rows[1].Cells["Fill_Price"].Value = FillValue / Fill_Quantity;

            // Sell
            dgv_Summary.Rows[2].Cells["Order_Quantity"].Value = dt_ShowFills.Compute("Sum(Qty_Order)", "Side='Sell' Or Side='Short'");
            dgv_Summary.Rows[2].Cells["Qty_Routed"].Value = dt_ShowFills.Compute("Sum(Qty_Routed)", "Side='Sell' Or Side='Short'");
            dgv_Summary.Rows[2].Cells["Qty_Working"].Value = dt_ShowFills.Compute("Sum(Qty_Working)", "Side='Sell' Or Side='Short'");
            dgv_Summary.Rows[2].Cells["Fill_Quantity"].Value = dt_ShowFills.Compute("Sum(Fill_Quantity)", "Side='Sell' Or Side='Short'");
            //dgv_Summary.Rows[2].Cells["Fill_Price"].Value = dt_ShowFills.Compute("IIF(Sum(Fill_Quantity)=0,0,Sum(FillValue)/Sum(Fill_Quantity))", "Side='Sell' Or Side='Short'");
            Fill_Quantity = SystemLibrary.ToDecimal(dt_ShowFills.Compute("Sum(Fill_Quantity)", "Side='Sell' Or Side='Short'"));
            FillValue = SystemLibrary.ToDecimal(dt_ShowFills.Compute("Sum(FillValue)", "Side='Sell' Or Side='Short'"));
            if (Fill_Quantity == 0)
                dgv_Summary.Rows[2].Cells["Fill_Price"].Value = DBNull.Value;
            else
                dgv_Summary.Rows[2].Cells["Fill_Price"].Value = FillValue / Fill_Quantity;

            // EOD
            dgv_Summary.Rows[3].Cells["Order_Quantity"].Value = dt_ShowFills.Compute("Sum(Qty_Order)", "");
            dgv_Summary.Rows[3].Cells["Qty_Routed"].Value = dt_ShowFills.Compute("Sum(Qty_Routed)", "");
            dgv_Summary.Rows[3].Cells["Qty_Working"].Value = dt_ShowFills.Compute("Sum(Qty_Working)", "");
            dgv_Summary.Rows[3].Cells["Fill_Quantity"].Value = dt_ShowFills.Compute("Sum(Fill_Quantity)", "");
            Fill_Quantity = SystemLibrary.ToDecimal(dt_ShowFills.Compute("Sum(Fill_Quantity)", ""));
            FillValue = SystemLibrary.ToDecimal(dt_ShowFills.Compute("Sum(FillValue)", ""));
            if (Fill_Quantity == 0)
                dgv_Summary.Rows[3].Cells["Fill_Price"].Value = DBNull.Value;
            else
                dgv_Summary.Rows[3].Cells["Fill_Price"].Value = FillValue / Fill_Quantity;

            // Calulate Profit
            for (int i = 0; i < dgv_Summary.Rows.Count; i++)
            {
                dgv_Summary.Rows[i].Cells["Profit"].Value = SystemLibrary.ToDecimal(dgv_Summary.Rows[i].Cells["Fill_Quantity"].Value) *
                                                          Pos_Mult_Factor *
                                                          (Last_Sale - SystemLibrary.ToDecimal(dgv_Summary.Rows[i].Cells["Fill_Price"].Value));
                // Total Line is Sum of parts as Buy Qty may equal Sell Qty
                if (i < 3)
                    TotalProfit = TotalProfit + SystemLibrary.ToDecimal(dgv_Summary.Rows[i].Cells["Profit"].Value);
                else
                    dgv_Summary.Rows[i].Cells["Profit"].Value = TotalProfit;
            }

            // Formats
            SystemLibrary.SetColumn(dgv_Summary, "Order_Quantity");
            SystemLibrary.SetColumn(dgv_Summary, "Qty_Routed");
            SystemLibrary.SetColumn(dgv_Summary, "Qty_Working");
            SystemLibrary.SetColumn(dgv_Summary, "Fill_Quantity");
            SystemLibrary.SetColumn(dgv_Summary, "Profit");

            // See if just for one EMSX_Sequence.Length
            if (EMSX_Sequence.Length > 0)
            {
                dgv_Summary.Rows[0].Visible = false; // Hide SOD
                dgv_Summary.Rows[3].Visible = false; // Hide EOD

                // Hide one of the Sides
                if (SystemLibrary.ToDecimal(dgv_Summary.Rows[1].Cells["Order_Quantity"].Value)!=0)
                    dgv_Summary.Rows[2].Visible = false;
                else
                    dgv_Summary.Rows[1].Visible = false;
            }

        } //SetSummary() 

        private void dgv_ShowFills_MouseClick(object sender, MouseEventArgs e)
        {
            //CXLocation = splitContainer1.Location.X + splitContainer1.Panel2.Location.X + dgv_ShowFills.Location.X + e.Location.X + 5;
            //CYLocation = splitContainer1.Location.Y + splitContainer1.Panel2.Location.Y + dgv_ShowFills.Location.Y + e.Location.Y;
            CXLocation = dgv_ShowFills.Location.X + e.Location.X + 5;
            CYLocation = dgv_ShowFills.Location.Y + e.Location.Y;
        } //dgv_ShowFills_MouseClick()

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
            public ShowFillStatus myParentForm;
        }

        private void dgv_ShowFills_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
                       // Show the popup menu
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    Point myLocation = new Point(this.Location.X + CXLocation, this.Location.Y + CYLocation);
                    // Select the Order as needed in code later on.
                    String EMSX_Sequence = SystemLibrary.ToString(dgv_ShowFills.Rows[e.RowIndex].Cells["Order#"].Value);
                    String mySql = "select OrderRefID, BBG_Ticker, Amount as Quantity, Sum(RoutedAmount) as RoutedAmount, Sum(FillAmount) as FillAmount, Side " +
                                   "From Fills " +
                                   "Where OrderRefID = (Select OrderRefID From Orders where Emsx_Sequence = " + EMSX_Sequence + ") " +
                                   "Group By OrderRefID, BBG_Ticker, Amount, Side " +
                                   "Union " +
                                   "Select OrderRefID, BBG_Ticker, Quantity, 0 as RoutedAmount, 0 as FillAmount, Side " +
                                   "From Orders " +
                                   "Where Emsx_Sequence = " + EMSX_Sequence + " " +
                                   "And Not Exists (Select 'x' from Fills Where Fills.OrderRefID = Orders.OrderRefID) ";
                    DataTable dt_Order = SystemLibrary.SQLSelectToDataTable(mySql);
                    if (dt_Order.Rows.Count > 0)
                    {
                        String OrderRefID = SystemLibrary.ToString(dt_Order.Rows[0]["OrderRefID"]);
                        String BBG_Ticker = SystemLibrary.ToString(dt_Order.Rows[0]["BBG_Ticker"]);
                        String Side = SystemLibrary.ToString(dt_Order.Rows[0]["Side"]);
                        Decimal Quantity = SystemLibrary.ToDecimal(dt_Order.Rows[0]["Quantity"]);
                        Decimal RoutedAmount = SystemLibrary.ToDecimal(dt_Order.Rows[0]["RoutedAmount"]);
                        Decimal FillAmount = SystemLibrary.ToDecimal(dt_Order.Rows[0]["FillAmount"]);
                        String FormattedQuantity = Quantity.ToString("#,##0");
                        String FormattedFillAmount = FillAmount.ToString("#,##0");
                        String FormattedRoutedAmount = RoutedAmount.ToString("#,##0");
                        Qty_Order = Quantity;

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
                        //myOrderStr.FillPrice = FillPrice;
                        myOrderStr.RoutedAmount = RoutedAmount;
                        myOrderStr.Side = Side;
                        myOrderStr.myParentForm = this;

                        // Resize to Fill Menu
                        if (Quantity != FillAmount)
                        {
                            mySubMenu = new ToolStripMenuItem("Resize Order (" + FormattedQuantity + ") to the Fill Amount (" + FormattedFillAmount + ")");
                            myOrderStr.Instruction = "ResizeToFill";
                            mySubMenu.Tag = myOrderStr;
                            mySubMenu.MouseUp += new MouseEventHandler(dgv_ShowFillsSystemMenuItem_Click);
                            myMenu.Items.Add(mySubMenu);
                        }

                        // Resize to Fill Menu
                        if (Quantity != RoutedAmount)
                        {
                            mySubMenu = new ToolStripMenuItem("Resize Order (" + FormattedQuantity + ") to the Routed Amount (" + FormattedRoutedAmount + ")");
                            myOrderStr.Instruction = "ResizeToRouted";
                            mySubMenu.Tag = myOrderStr;
                            mySubMenu.MouseUp += new MouseEventHandler(dgv_ShowFillsSystemMenuItem_Click);
                            myMenu.Items.Add(mySubMenu);
                        }
                        // Double Order Menu
                        mySubMenu = new ToolStripMenuItem("Double Order Size");
                        myOrderStr.Instruction = "ResizeToDouble";
                        mySubMenu.Tag = myOrderStr;
                        mySubMenu.MouseUp += new MouseEventHandler(dgv_ShowFillsSystemMenuItem_Click);
                        myMenu.Items.Add(mySubMenu);

                        // Resize Order Menu
                        myMenu.Items.Add("-");
                        mySubMenu = new ToolStripMenuItem("Resize Order");
                        myOrderStr.Instruction = "ResizeOrder";
                        mySubMenu.Tag = myOrderStr;
                        mySubMenu.MouseUp += new MouseEventHandler(dgv_ShowFillsSystemMenuItem_Click);
                        myMenu.Items.Add(mySubMenu);

                        // Delete Order Menu
                        myMenu.Items.Add("-");
                        mySubMenu = new ToolStripMenuItem("Delete Order");
                        myOrderStr.Instruction = "Delete";
                        mySubMenu.Tag = myOrderStr;
                        mySubMenu.MouseUp += new MouseEventHandler(dgv_ShowFillsSystemMenuItem_Click);
                        myMenu.Items.Add(mySubMenu);

                        // Show the Menu
                        myMenu.Show(myLocation);
                    }
                }
            }
            catch { }

        } //dgv_ShowFills_CellMouseClick()

        public static void dgv_ShowFillsSystemMenuItem_Click(object sender, MouseEventArgs e)
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
            ShowFillStatus myForm = myOrderStr.myParentForm;


            if (OrderRefID.Length > 0)
            {
                switch (myOrderStr.Instruction)
                {
                    case "Delete":
                        if (myOrderStr.FillAmount != 0)
                            myQuestion = "WARNING: This order has already been filled " + myOrderStr.FillAmount.ToString("#,###") + " @" + myOrderStr.FillPrice.ToString("#,###.####") + ", so you should 'Resize to Fill' rather than Delete\r\n\r\n";
                        else if (myOrderStr.RoutedAmount != 0)
                            myQuestion = "WARNING: Part of this order has already been routed " + myOrderStr.RoutedAmount.ToString("#,###") + " and should be cancelled first.\r\n\r\n";
                        myQuestion = myQuestion +
                                     "You are about to Delete the Order for\r\n\r\n" +
                                     myOrderStr.Side + " " + myOrderStr.Quantity + "  " + myOrderStr.BBG_Ticker + "\r\n";
                        if (myOrderStr.EMSX_Sequence.Length > 0)
                            myQuestion = myQuestion + "\r\nBloomberg Order# " + myOrderStr.EMSX_Sequence;
                        myQuestion = myQuestion + "\r\n\r\n" + "Is this Ok?";

                        if (MessageBox.Show(myQuestion, "Delete Order", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if (myOrderStr.EMSX_Sequence.Length > 0)
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
                            myForm.bt_Refresh_Click(null, null);

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
                            myQuestion = "WARNING: The Routed part of this order (" + myOrderStr.RoutedAmount.ToString("#,###") + ") is greater than the Filled Amount (" + myOrderStr.FillAmount.ToString("#,###") + ") and should be cancelled first.\r\n\r\n" +
                                         myQuestion;
                        }

                        if (FillAmount == 0)
                        {
                            MessageBox.Show("The Order has not been Filled (Fill Amount = 0).\r\n\r\nPlease use the [Delete Order] menu if that is your intention", "Resize order to Fill Amount");
                        }
                        else if (MessageBox.Show(myQuestion, "Resize Order", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            myMessage = SendToBloomberg.EMSXAPI_Modify(OrderRefID, myOrderStr.BBG_Ticker, Convert.ToInt32(myOrderStr.FillAmount));

                            // Resize the Order
                            SystemLibrary.SQLExecute("Exec sp_ResizeOrder '" + OrderRefID + "', " + myOrderStr.FillAmount.ToString());
                            myForm.bt_Refresh_Click(null, null);

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
                            myMessage = SendToBloomberg.EMSXAPI_Modify(OrderRefID, myOrderStr.BBG_Ticker, Convert.ToInt32(myOrderStr.RoutedAmount));

                            // Resize the Order
                            SystemLibrary.SQLExecute("Exec sp_ResizeOrder '" + OrderRefID + "', " + myOrderStr.RoutedAmount.ToString());
                            myForm.bt_Refresh_Click(null, null);

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
                            myMessage = SendToBloomberg.EMSXAPI_Modify(OrderRefID, myOrderStr.BBG_Ticker, Convert.ToInt32(DoubleQuantity));

                            // Resize the Order
                            SystemLibrary.SQLExecute("Exec sp_ResizeOrder '" + OrderRefID + "', " + DoubleQuantity.ToString());
                            myForm.bt_Refresh_Click(null, null);

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

                            myMessage = SendToBloomberg.EMSXAPI_Modify(OrderRefID, myOrderStr.BBG_Ticker, (Int32)myQty);

                            mySql = "Exec sp_ResizeOrder '" + OrderRefID + "', " + myQty.ToString();
                            SystemLibrary.SQLExecute(mySql);

                            myForm.bt_Refresh_Click(null, null);

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
            }
        } //dgv_ShowFillsSystemMenuItem_Click()

        private void ShowFillStatus_FormClosed(object sender, FormClosedEventArgs e)
        {
            // See if need to Update Positions to reflect the changes.
            SystemLibrary.DebugLine("ShowFillStatus_FormClosed: Start");
            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;
            if (NeedFullUpdate)
            {
                /* 
                 * Needed in cases where Delete's occur otherwise the Positions Table will not force sp_Positions to refresh properly for other users.
                 */
                SystemLibrary.SQLExecute("Exec sp_SOD_Positions ");
                SystemLibrary.DebugLine("ShowFillStatus_FormClosed: Post sp_SOD_Positions ");
                if (ParentForm1 != null && ParentForm1.GetType().Name == "Form1")
                    ((Form1)ParentForm1).LoadPortfolioIncr();
                SystemLibrary.DebugLine("ShowFillStatus_FormClosed: Post LoadPortfolioIncr() ");
            }
            else if (NeedUpdate)
            {
                SystemLibrary.SQLExecute("Exec sp_Update_Positions 'Y' ");
                SystemLibrary.DebugLine("ShowFillStatus_FormClosed: Post sp_Update_Positions ");
                if (ParentForm1 != null && ParentForm1.GetType().Name == "Form1")
                    ((Form1)ParentForm1).LoadPortfolioIncr();
                SystemLibrary.DebugLine("ShowFillStatus_FormClosed: Post LoadPortfolioIncr() ");
            }

            // Let the Parent know this has close
            if (ParentForm1 != null && ParentForm1.GetType().Name == "Form1")
                ((Form1)ParentForm1).LoadActionTab(true);
            SystemLibrary.DebugLine("ShowFillStatus_FormClosed: Post LoadActionTab() ");

            Cursor.Current = Cursors.Default;
            SystemLibrary.DebugLine("ShowFillStatus_FormClosed: End");


        } //ShowFillStatus_FormClosed()

        SystemLibrary.InputBoxValidation validate_ResizOrder = delegate(String myValue)
        {
            // Rules: Ok to be Zero - this means cancelling the order
            //          Cannot be opposite sign
            //          Look at Order.Side
            int myIntValue = SystemLibrary.ToInt32(myValue);
            if (Math.Sign((decimal)Qty_Order) != Math.Sign((decimal)myIntValue))
                return "'" + myValue + "' cannot be opposite sign to original order. Please set to 0 (Zero) if you want to delete this order.";
            else
                return "";

        }; //validate_ResizOrder()


    }
}
