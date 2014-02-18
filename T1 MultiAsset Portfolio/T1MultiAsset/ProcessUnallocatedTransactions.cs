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
    public partial class ProcessUnallocatedTransactions : Form
    {
        // Global Variables
        public Form1 ParentForm1;
        DataTable dt_Summary;
        DataTable dt_Detail;
        DataTable dt_Journal;
        private int CXLocation = 0;
        private int CYLocation = 0;
        
        public struct DetailMenuStruct
        {
            public int TranID;
            public int FundID;
            public Form myParentForm;
            public String TranType;
            public DateTime EffectiveDate;
            public DateTime RecordDate;
            public String Description;
            public String Source;
            public String Journal;
        }



        public ProcessUnallocatedTransactions()
        {
            InitializeComponent();
        }

        private void ProcessUnallocatedTransactions_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            LoadSummary();
        }

        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;

        } //FromParent()

        private void LoadSummary()
        {
            // Local Variables
            String mySql;

            mySql = "Exec sp_GetUnallocatedAmounts ";
            dt_Summary = SystemLibrary.SQLSelectToDataTable(mySql);

            dg_Summary.DataSource = dt_Summary;
            SystemLibrary.SetDataGridView(dg_Summary);
            dg_Summary.Columns["EffectiveDate"].HeaderText = "Effective Date";


            if (dt_Summary.Rows.Count > 0)
                LoadDetail(0);

            for (int i=0;i<dg_Summary.Columns.Count;i++)
                dg_Summary.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

        } // LoadSummary()

        private void LoadDetail(int myRow)
        {
            // Local Variables exec sp_GetJournalDetails 864

            String mySql;
            String myFundID;
            String myAccountID;
            String myEffectiveDate;
            Decimal UnallocatedAmount;

            myFundID = SystemLibrary.ToString(dt_Summary.Rows[myRow]["FundID"]);
            myAccountID = SystemLibrary.ToString(dt_Summary.Rows[myRow]["AccountID"]);
            myEffectiveDate = Convert.ToDateTime(dt_Summary.Rows[myRow]["EffectiveDate"]).ToString("dd-MMM-yyyy");
            UnallocatedAmount = SystemLibrary.ToDecimal(dt_Summary.Rows[myRow]["Unallocated Amount"]);

            mySql = "Exec sp_GetUnallocatedAmounts null, "+myFundID+", "+myAccountID+", '"+myEffectiveDate+"' ";
            dt_Detail = SystemLibrary.SQLSelectToDataTable(mySql);

            dg_Detail.DataSource = dt_Detail;
            SystemLibrary.SetDataGridView(dg_Detail);
            dg_Detail.Columns["ExtID"].Visible = false;
            dg_Detail.Columns["Source"].Visible = false;
            dg_Detail.Columns["EffectiveDate"].HeaderText = "Effective Date";
            dg_Detail.Columns["RecordDate"].HeaderText = "Record Date";

            if (dt_Detail.Rows.Count > 0)
                LoadJournal(0);
            else
            {
                // Clean up the Journals data window
                if (dt_Journal != null && dt_Journal.Rows.Count > 0)
                    dt_Journal.Rows.Clear();
            }

            for (int i = 0; i < dg_Detail.Columns.Count; i++)
                dg_Detail.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // See if any of the rows matc the difference
            for (int i = 0; i < dg_Detail.Rows.Count; i++)
            {
                String myJournal = SystemLibrary.ToString(dg_Detail.Rows[i].Cells["Journal"].Value);
                Decimal myAmount = SystemLibrary.ToDecimal(dg_Detail.Rows[i].Cells["Amount"].Value);
                if (myAmount == UnallocatedAmount && myJournal.Length==0)
                    dg_Detail.Rows[i].Cells["Amount"].Style.BackColor = Color.LightCyan;
            }


        } // LoadDetail()

        private void dg_Summary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        } // dg_Summary_CellContentClick()

        private void dg_Summary_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            LoadDetail(e.RowIndex);

        } // dg_Summary_CellClick()

         private void bt_Refresh_Click(object sender, EventArgs e)
        {
            // Clean up the details data window
            if (dt_Detail != null && dt_Detail.Rows.Count > 0)
                dt_Detail.Rows.Clear();

            // Clean up the Journals data window
            if (dt_Journal != null && dt_Journal.Rows.Count > 0)
                dt_Journal.Rows.Clear();

            LoadSummary();


        } // bt_Refresh_Click()

        public delegate void RefreshDataCallback();
        public void RefreshData()
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                RefreshDataCallback cb = new RefreshDataCallback(RefreshData);
                this.Invoke(cb, new object[] {  });
            }
            else
            {
                bt_Refresh_Click(null, null);
            }
        } //RefreshData()

        private void LoadJournal(int myRow)
        {
            // Local Variables
            String mySql;
            String myJournalID;

            myJournalID = SystemLibrary.ToString(dt_Detail.Rows[myRow]["JournalID"]);

            if (myJournalID.Length == 0)
            {
                if (dt_Journal != null && dt_Journal.Rows.Count > 0)
                    dt_Journal.Rows.Clear();
                return;
            }

            mySql = "Exec sp_GetJournalDetails " + myJournalID;
            dt_Journal = SystemLibrary.SQLSelectToDataTable(mySql);

            dg_Journal.DataSource = dt_Journal;
            SystemLibrary.SetDataGridView(dg_Journal);

            //dg_Journal.Columns["TranID"].Visible = false;
            //dg_Journal.Columns["AccountID"].Visible = false;
            //dg_Journal.Columns["FundID"].Visible = false;
            //dg_Journal.Columns["PortfolioID"].Visible = false;
            //dg_Journal.Columns["TradeID"].Visible = false;
            //dg_Journal.Columns["CapitalID"].Visible = false;
            dg_Journal.Columns["ExtID"].Visible = false;
            dg_Journal.Columns["Source"].Visible = false;
            dg_Journal.Columns["Source_AccountID"].Visible = false;
            dg_Journal.Columns["JournalID"].Visible = false;

            //dg_Journal.Columns["EffectiveDate"].DefaultCellStyle.Format = "dd-MMM-yyyy";
            dg_Journal.Columns["EffectiveDate"].HeaderText = "Effective Date";
            //dg_Journal.Columns["RecordDate"].DefaultCellStyle.Format = "dd-MMM-yyyy";
            dg_Journal.Columns["RecordDate"].HeaderText = "Record Date";
            dg_Journal.Columns["FundName"].HeaderText = "Fund Name";
            dg_Journal.Columns["AccountName"].HeaderText = "Account Name";

            //for (int i = 0; i < dg_Journal.Columns.Count; i++)
            //    dg_Journal.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;


        } // LoadJournal()

        private void dg_Detail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            LoadJournal(e.RowIndex);

        } //dg_Detail_CellClick()

        private void dg_Detail_MouseClick(object sender, MouseEventArgs e)
        {
            CXLocation = dg_Detail.Location.X + e.Location.X + 5;
            CYLocation = splitContainer1.Location.Y + splitContainer2.Location.Y + splitContainer1.Panel2.Top  + dg_Detail.Location.Y + e.Location.Y;

        } //dg_Detail_MouseClick()

        /*
         * Special menu case where the transaction has been created by another Source
         */
        private Boolean OfferDeleteMenu(ref DetailMenuStruct myDetailStr, ref ContextMenuStrip myMenu, ref ToolStripMenuItem mySubMenu)
        {
            // Local Variables
            Boolean RetVal = false;

            if (myDetailStr.Source.Length > 0 && myDetailStr.Journal.Length == 0)
            {
                mySubMenu = new ToolStripMenuItem("Delete the Transactions that came from '" + myDetailStr.Source + "'");
                mySubMenu.Tag = myDetailStr;
                mySubMenu.MouseUp += new MouseEventHandler(dg_DetailSystemMenuItem_Click);
                myMenu.Items.Add(mySubMenu);
                RetVal = true;
            }
            return (RetVal);
        } //OfferDeleteMenu()

        private void dg_Detail_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Show the popup menu
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    Point myLocation = new Point(this.Location.X + CXLocation, this.Location.Y + CYLocation);
                    String JournalID = SystemLibrary.ToString(dg_Detail.Rows[e.RowIndex].Cells["JournalID"].Value);

                    ContextMenuStrip myMenu = new ContextMenuStrip();
                    ToolStripMenuItem mySubMenu = new ToolStripMenuItem();

                    DetailMenuStruct myDetailStr = new DetailMenuStruct();
                    myDetailStr.TranID = SystemLibrary.ToInt32(dg_Detail.Rows[e.RowIndex].Cells["TranID"].Value); 
                    myDetailStr.FundID = SystemLibrary.ToInt32(dg_Detail.Rows[e.RowIndex].Cells["FundID"].Value); 
                    myDetailStr.TranType = SystemLibrary.ToString(dg_Detail.Rows[e.RowIndex].Cells["TranType"].Value);
                    myDetailStr.EffectiveDate = Convert.ToDateTime(dg_Detail.Rows[e.RowIndex].Cells["EffectiveDate"].Value);
                    myDetailStr.RecordDate = Convert.ToDateTime(dg_Detail.Rows[e.RowIndex].Cells["RecordDate"].Value);
                    myDetailStr.Description = SystemLibrary.ToString(dg_Detail.Rows[e.RowIndex].Cells["Description"].Value);
                    myDetailStr.Source = SystemLibrary.ToString(dg_Detail.Rows[e.RowIndex].Cells["Source"].Value);
                    myDetailStr.Journal = SystemLibrary.ToString(dg_Detail.Rows[e.RowIndex].Cells["Journal"].Value);
                    myDetailStr.myParentForm = this;

                    // Create Menu
                    if (myDetailStr.TranType.ToUpper() == "INTEREST" && JournalID.Length == 0)
                    {
                        mySubMenu = new ToolStripMenuItem("Apportion Interest Paid to sub accounts");
                        mySubMenu.Tag = myDetailStr;
                        mySubMenu.MouseUp += new MouseEventHandler(dg_DetailSystemMenuItem_Click);
                        myMenu.Items.Add(mySubMenu);

                        OfferDeleteMenu(ref myDetailStr, ref myMenu, ref mySubMenu);
                    }
                    else if (myDetailStr.TranType.ToUpper() == "STOCK LOAN FEE" && JournalID.Length == 0)
                    {
                        mySubMenu = new ToolStripMenuItem("Apportion Stock Loan Fee to sub accounts");
                        mySubMenu.Tag = myDetailStr;
                        mySubMenu.MouseUp += new MouseEventHandler(dg_DetailSystemMenuItem_Click);
                        myMenu.Items.Add(mySubMenu);

                        OfferDeleteMenu(ref myDetailStr, ref myMenu, ref mySubMenu);
                    }
                    else if (myDetailStr.TranType.ToUpper() == "TICKET CHARGES" && JournalID.Length == 0)
                    {
                        mySubMenu = new ToolStripMenuItem("Apportion Ticket Charges to sub accounts based on number of trades");
                        mySubMenu.Tag = myDetailStr;
                        mySubMenu.MouseUp += new MouseEventHandler(dg_DetailSystemMenuItem_Click);
                        myMenu.Items.Add(mySubMenu);

                        myMenu.Items.Add("-");
                        mySubMenu = new ToolStripMenuItem("Manually Apportion Ticket Charges to sub accounts");
                        mySubMenu.Tag = myDetailStr;
                        mySubMenu.MouseUp += new MouseEventHandler(dg_DetailSystemMenuItem_Click);
                        myMenu.Items.Add(mySubMenu);

                        OfferDeleteMenu(ref myDetailStr, ref myMenu, ref mySubMenu);
                    }
                    else if (myDetailStr.TranType.ToUpper() == "TRANSFER" && JournalID.Length > 0 && 
                                (myDetailStr.Description.ToUpper() == "FUTURES MARGIN" || myDetailStr.Description.ToUpper() == "Day Profit + Future comm and charges".ToUpper() )
                            )
                    {
                        /*
                         * See if there are any missing entries from the FuturesMargin Table
                         * sp_CreateMarginTransactionForTradeOutstanding
                         */
                        String myMissingTicker = SystemLibrary.SQLSelectString("Exec sp_MissingFutureMargins");
                        if (myMissingTicker.Length > 0)
                        {
                            mySubMenu = new ToolStripMenuItem("DO THIS FIRST - Update MISSING Future Margin Definition for " + myMissingTicker);
                            mySubMenu.Tag = myDetailStr;
                            mySubMenu.MouseUp += new MouseEventHandler(dg_DetailSystemMenuItem_Click);
                            myMenu.Items.Add(mySubMenu);
                        }

                        mySubMenu = new ToolStripMenuItem("Rebuild Future Margins for " + myDetailStr.RecordDate.ToString("dd-MMM-yyyy"));
                        mySubMenu.Tag = myDetailStr;
                        mySubMenu.MouseUp += new MouseEventHandler(dg_DetailSystemMenuItem_Click);
                        myMenu.Items.Add(mySubMenu);
                        mySubMenu = new ToolStripMenuItem("Explain Future Margins movements for " + myDetailStr.EffectiveDate.ToString("dd-MMM-yyyy"));
                        mySubMenu.Tag = myDetailStr;
                        mySubMenu.MouseUp += new MouseEventHandler(dg_DetailSystemMenuItem_Click);
                        myMenu.Items.Add(mySubMenu);
                    }
                    else if (myDetailStr.TranType.ToUpper() == "TRANSFER" && JournalID.Length == 0 &&
                             myDetailStr.Description.Length > 0
                            )
                    {
                        // This is a Transfer sent by the source. See if it matches a Capital movement.
                        String oldSql = "Select	count(*) " +
                                       "From	Transactions t1, " +
                                       "		Transactions t2, " +
                                       "		Fund f1, " +
                                       "		Fund f2 " +
                                       "Where	t1.TranID = " + myDetailStr.TranID.ToString() + " " +
                                       "And		f1.FundID = t1.FundID " +
                                       "And		f2.ParentFundID = f1.ParentFundID " +
                                       "And		f2.FundID = t2.FundID " +
                                       "And		t2.AccountID = t1.AccountID " +
                                       "And		t2.EffectiveDate = t1.EffectiveDate " +
                                       "And		t2.Amount = t1.Amount " +
                                       "And		t2.TranType = 'Capital' " +
                                       "And		t1.TranType = 'Transfer' " +
                                       "And     isNull(t1.Reconcilled,'N') = 'N' " +
                                       "And     isNull(t2.Reconcilled,'N') = 'N' ";

                        //               "						And		t2.Amount = t1.Amount  " +
                        String mySql = "Select Count(*) " +
                                       "From	Transactions t1 " +
                                       "Where	t1.TranID = " + myDetailStr.TranID.ToString() + " " +
                                       "And     isNull(t1.Reconcilled,'N') = 'N'  " +
                                       "And		t1.TranType = 'Transfer'  " +
                                       "And		t1.Amount = (	Select	sum(t2.Amount) " +
                                       "						From	Transactions t2,  " +
                                       "								Fund f1,  " +
                                       "								Fund f2  " +
                                       "						Where	f1.FundID = t1.FundID  " +
                                       "						And		f2.ParentFundID = f1.ParentFundID  " +
                                       "						And		f2.FundID = t2.FundID  " +
                                       "						And		t2.AccountID = t1.AccountID  " +
                                       "						And		t2.EffectiveDate = t1.EffectiveDate  " +
                                       "						And		t2.TranType = 'Capital'  " +
                                       "						And     isNull(t2.Reconcilled,'N') = 'N'  " +
                                       "					)";
                        if (SystemLibrary.SQLSelectInt32(mySql) > 0)
                        {
                            // Found a Fund Capital inflow/outflow that matches this record, so offer a new window to process this
                            JournalCapitalMovement JCP = new JournalCapitalMovement();
                            JCP.FromParent(this, myDetailStr.TranID, SystemLibrary.ToDouble(dg_Detail.Rows[e.RowIndex].Cells["Amount"].Value));
                            JCP.ShowDialog();
                            RefreshData();
                            /*
                            if (MessageBox.Show(this, "Found a Fund Capital inflow/outflow that matches this record.\r\n\r\n" +
                                                      "Do you want to match these against each other?", myDetailStr.Description, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                mySql = "exec sp_Map_Capital_Movement " + myDetailStr.TranID.ToString() + " ";
                                SystemLibrary.SQLExecute(mySql);
                                RefreshData();
                                MessageBox.Show("Fund Capital inflow/outflow Reapplied");
                            }
                            */
                        }
                        else
                        {
                            // See if there is another 'Cash' account that the funds could be transferred to.
                            // I have chosen not to offer Linked Accounts like the Futures Account as I assume that comes electronically?
                            mySql = "Select  Count(*) " +
                                    "From	Accounts, " +
                                    "		Transactions " +
                                    "Where	Transactions.TranID = " + myDetailStr.TranID.ToString() + " " +
                                    "And		Accounts.FundID = Transactions.FundID " +
                                    "And		Accounts.AccountID <> Transactions.AccountID " +
                                    "And		Accounts.AccountType = (Select	a.AccountType " +
                                    "								From	Accounts a " +
                                    "								Where	a.FundID = Transactions.FundID " +
                                    "								And		a.AccountID = Transactions.AccountID " +
                                    "							   ) " +
                                    "And		isNull(Accounts.Linked_AccountID,-123456) <> Transactions.AccountID "; // No linked account
                            if (SystemLibrary.SQLSelectInt32(mySql) > 0)
                            {
                                mySubMenu = new ToolStripMenuItem("Transfer Funds to another account of the same type");
                                mySubMenu.Tag = myDetailStr;
                                mySubMenu.MouseUp += new MouseEventHandler(dg_DetailSystemMenuItem_Click);
                                myMenu.Items.Add(mySubMenu);
                                mySubMenu = new ToolStripMenuItem("Create Other Side of Transfer to another account of the same type");
                                mySubMenu.Tag = myDetailStr;
                                mySubMenu.MouseUp += new MouseEventHandler(dg_DetailSystemMenuItem_Click);
                                myMenu.Items.Add(mySubMenu);
                            }

                            OfferDeleteMenu(ref myDetailStr, ref myMenu, ref mySubMenu);
                        }
                    }
                    else
                    {
                        if(OfferDeleteMenu(ref myDetailStr, ref myMenu, ref mySubMenu))
                            myMenu.Show(myLocation);
                        return;
                    }

                    // Show the Menu
                    myMenu.Show(myLocation);
                }
            }
            catch { }
            
        } // dg_Detail_CellClick()

        public static void dg_DetailSystemMenuItem_Click(object sender, MouseEventArgs e)
        {
            //
            // Purpose: A generic menu click event for the Detail Right-Click
            //

            // Local Variables
            ToolStripMenuItem ts_From = (ToolStripMenuItem)sender;
            DetailMenuStruct myDetailStr = (DetailMenuStruct)ts_From.Tag;
            String Reason = "Deleted because {your text here}";

            if (ts_From.Text.StartsWith("Delete the Transactions that came from"))
            {
                if (SystemLibrary.InputBox(ts_From.Text, "You are about to remove a transactions sourced from '" + myDetailStr.Source + "'.\r\n" +
                                           "You will not be able to reverse this.\r\n\r\n" +
                                           "Press [Ok] to continue the deletion, or [Cancel] to abort.",
                                           ref Reason, ((ProcessUnallocatedTransactions)myDetailStr.myParentForm).validate_SaveAs, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    SystemLibrary.SQLExecute("Exec sp_RemoveTransaction " + myDetailStr.TranID.ToString() + ",'" + Reason + "'");
                    ((ProcessUnallocatedTransactions)myDetailStr.myParentForm).RefreshData();
                    MessageBox.Show("Transaction removed.");
                }
            }
            else if (ts_From.Text.Equals("Transfer Funds to another account of the same type"))
            {
                // Local Variables
                String mySql = "Select t.AccountID, a.AccountType, t.Amount, t.crncy, f.ParentFundID, t.PortfolioID " +
                               "From    Transactions t, " + 
                               "        Accounts a, " +
                               "        Fund f " +
                               "Where   t.TranID = "+myDetailStr.TranID.ToString()+" " +
                               "And     a.AccountID = t.AccountID " + 
                               "And     f.FundID = t.FundID";

                DataTable dt_Journal = SystemLibrary.SQLSelectToDataTable(mySql);

                if (dt_Journal.Rows.Count>0)
                {
                    Int32 AccountID = SystemLibrary.ToInt32(dt_Journal.Rows[0]["AccountID"]);
                    String AccountType = SystemLibrary.ToString(dt_Journal.Rows[0]["AccountType"]);
                    Double Amount = SystemLibrary.ToDouble(dt_Journal.Rows[0]["Amount"]);
                    String crncy = SystemLibrary.ToString(dt_Journal.Rows[0]["crncy"]);
                    Int32 ParentFundID = SystemLibrary.ToInt32(dt_Journal.Rows[0]["ParentFundID"]);
                    Int32 PortfolioID = SystemLibrary.ToInt32(dt_Journal.Rows[0]["PortfolioID"]);

                    ProcessJournals frm2 = new ProcessJournals();
                    frm2.FromParent(myDetailStr.myParentForm, "Between Accounts", myDetailStr.TranID, myDetailStr.TranType, 
                        myDetailStr.EffectiveDate, AccountID, AccountType, myDetailStr.Description, 
                        Amount, crncy, ParentFundID, myDetailStr.FundID, PortfolioID.ToString());
                    frm2.Show();
                }        
            }
            else if (ts_From.Text.Equals("Create Other Side of Transfer to another account of the same type"))
            {
                // Local Variables
                String mySql = "Select t.AccountID, a.AccountType, t.Amount, t.crncy, f.ParentFundID, t.PortfolioID " +
                               "From    Transactions t, " +
                               "        Accounts a, " +
                               "        Fund f " +
                               "Where   t.TranID = " + myDetailStr.TranID.ToString() + " " +
                               "And     a.AccountID = t.AccountID " +
                               "And     f.FundID = t.FundID";

                DataTable dt_Journal = SystemLibrary.SQLSelectToDataTable(mySql);

                if (dt_Journal.Rows.Count > 0)
                {
                    Int32 AccountID = SystemLibrary.ToInt32(dt_Journal.Rows[0]["AccountID"]);
                    String AccountType = SystemLibrary.ToString(dt_Journal.Rows[0]["AccountType"]);
                    Double Amount = SystemLibrary.ToDouble(dt_Journal.Rows[0]["Amount"]);
                    String crncy = SystemLibrary.ToString(dt_Journal.Rows[0]["crncy"]);
                    Int32 ParentFundID = SystemLibrary.ToInt32(dt_Journal.Rows[0]["ParentFundID"]);
                    Int32 PortfolioID = SystemLibrary.ToInt32(dt_Journal.Rows[0]["PortfolioID"]);

                    ProcessJournals frm2 = new ProcessJournals();
                    frm2.FromParent(myDetailStr.myParentForm, "Between Accounts - Other Half", myDetailStr.TranID, myDetailStr.TranType,
                        myDetailStr.EffectiveDate, AccountID, AccountType, myDetailStr.Description,
                        Amount, crncy, ParentFundID, myDetailStr.FundID, PortfolioID.ToString());
                    frm2.Show();
                }
            }
            else if (ts_From.Text.Equals("Manually Apportion Ticket Charges to sub accounts"))
            {
                // Local Variables
                String mySql = "Select t.AccountID, a.AccountType, t.Amount, t.crncy, f.ParentFundID, t.PortfolioID " +
                               "From    Transactions t, " +
                               "        Accounts a, " +
                               "        Fund f " +
                               "Where   t.TranID = " + myDetailStr.TranID.ToString() + " " +
                               "And     a.AccountID = t.AccountID " +
                               "And     f.FundID = t.FundID";

                DataTable dt_Journal = SystemLibrary.SQLSelectToDataTable(mySql);

                if (dt_Journal.Rows.Count > 0)
                {
                    Int32 AccountID = SystemLibrary.ToInt32(dt_Journal.Rows[0]["AccountID"]);
                    String AccountType = SystemLibrary.ToString(dt_Journal.Rows[0]["AccountType"]);
                    Double Amount = SystemLibrary.ToDouble(dt_Journal.Rows[0]["Amount"]);
                    String crncy = SystemLibrary.ToString(dt_Journal.Rows[0]["crncy"]);
                    Int32 ParentFundID = SystemLibrary.ToInt32(dt_Journal.Rows[0]["ParentFundID"]);
                    Int32 PortfolioID = SystemLibrary.ToInt32(dt_Journal.Rows[0]["PortfolioID"]);

                    ProcessJournals frm2 = new ProcessJournals();
                    frm2.FromParent(myDetailStr.myParentForm, "Between Accounts - Manually Apportion Ticket Charges to sub accounts", myDetailStr.TranID, myDetailStr.TranType,
                        myDetailStr.EffectiveDate, AccountID, AccountType, myDetailStr.Description,
                        Amount, crncy, ParentFundID, myDetailStr.FundID, PortfolioID.ToString());
                    frm2.Show();
                }
            }
            else
            {
                switch (myDetailStr.TranType.ToUpper())
                {
                    case "INTEREST":
                        InterestApportion frm = new InterestApportion();
                        frm.FromParent(myDetailStr.myParentForm, myDetailStr.TranID);
                        frm.Show();
                        break;
                    case "STOCK LOAN FEE":
                        StockLoanFeeApportion frm1 = new StockLoanFeeApportion();
                        frm1.FromParent(myDetailStr.myParentForm, myDetailStr.TranID);
                        frm1.Show();
                        break;
                    case "TICKET CHARGES":
                        String myMessage = SystemLibrary.SQLSelectString("Exec sp_ApportionTicketCharges " + myDetailStr.TranID.ToString() + " ");
                        ((ProcessUnallocatedTransactions)myDetailStr.myParentForm).RefreshData();
                        MessageBox.Show(myMessage, "Apportion Ticket Charges");
                        break;
                    case "TRANSFER":
                        if (myDetailStr.Description.ToUpper() == "FUTURES MARGIN" || myDetailStr.Description.ToUpper() == "Day Profit + Future comm and charges".ToUpper())
                        {
                            if (ts_From.Text.StartsWith("Explain Future Margins movements for "))
                            {
                                FuturesExplainWireTransfer frm_Explain = new FuturesExplainWireTransfer();
                                frm_Explain.FromParent((ProcessUnallocatedTransactions)myDetailStr.myParentForm, myDetailStr.FundID, myDetailStr.EffectiveDate);
                                frm_Explain.Show();
                            }
                            else if (ts_From.Text.StartsWith("DO THIS FIRST - Update MISSING Future Margin Definition for "))
                            {
                                FutureMargins frm_FutMargin = new FutureMargins();
                                frm_FutMargin.FromParent(myDetailStr.myParentForm, true);
                                frm_FutMargin.Show();
                            }
                            else
                            {
                                SystemLibrary.SQLExecute("Exec sp_ReapplyMargin " + myDetailStr.FundID.ToString() + ", '" + myDetailStr.RecordDate.ToString("dd-MMM-yyyy") + "' ");
                                ((ProcessUnallocatedTransactions)myDetailStr.myParentForm).RefreshData();
                                MessageBox.Show("Futures Margins Reapplied");
                            }
                        }
                        break;
                }
            }

        } //dg_DetailSystemMenuItem_Click()

        SystemLibrary.InputBoxValidation validate_SaveAs = delegate(String Reason)
        {
            if (Reason == "Deleted because {your text here}")
                return "Please supply a reason for the deletion.";
            else if (Reason.Length > 1024)
                return "Please shorted the Reason from " + Reason.Length.ToString() + "characters to 1024.";
            else
                return "";
        }; //validate_SaveAs()

        private void ProcessUnallocatedTransactions_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Refresh the ActionsTab
            if (ParentForm1 != null)
                ParentForm1.LoadActionTab(true);

        } //ProcessUnallocatedTransactions_FormClosed()

    }
}
