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
    public partial class StockLoanFeeApportion : Form
    {
        // Public Variables
        public Form ParentForm1;
        DataTable dt_Transactions;
        DataTable dt_inTransaction;
        //DataTable dt_Accruals;
        DataTable dt_Fund;
        String AccountID;
        String crncy;
        DateTime EffectiveDate;
        Object LastValue;
        Boolean inStartup = true;
        //Boolean UsingDailyBalance = false;

        int TranID;

        public StockLoanFeeApportion()
        {
            InitializeComponent();
        }

        public void FromParent(Form inForm, int inTranID)
        {
            ParentForm1 = inForm;
            TranID = inTranID;

        } //FromParent()

        private void StockLoanFeeApportion_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);
            StockLoanFeeReporting_Load();
            inStartup = false;

        } //StockLoanFeeApportion_Shown()

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            StockLoanFeeReporting_Load();

        } //bt_Refresh_Click()


        private void StockLoanFeeApportion_Load(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;

            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            // Get the passed in transactions
            mySql = "Select t.TranID, t.AccountID, t.FundID, t.EffectiveDate, t.RecordDate, t.crncy, t.Description, t.Amount, f.FundName " +
                    "From   Transactions t, " +
                    "       Fund f, " +
                    "       Transactions t2 " +
                    "Where  t2.TranID = " + TranID.ToString() + " " +
                    "And		t.EffectiveDate = t2.EffectiveDate " +
                    "And		t.crncy = t2.crncy " +
                    "And		t.FundID = t2.FundID " +
                    "And		t.TranType = t2.TranType " +
                    "And		t.JournalID is null " +
                    "And    f.FundID = t.FundID " +
                    "And    f.ParentFundID = f.FundID ";
            dt_inTransaction = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_inTransaction.DataSource = dt_inTransaction;

            dg_inTransaction.Columns["TranID"].Visible = false;
            dg_inTransaction.Columns["AccountID"].Visible = false;
            dg_inTransaction.Columns["FundID"].Visible = false;
            dg_inTransaction.Columns["EffectiveDate"].DefaultCellStyle.Format = "dd MMM yy";
            dg_inTransaction.Columns["RecordDate"].DefaultCellStyle.Format = "dd MMM yy";
            dg_inTransaction.Columns["Amount"].DefaultCellStyle.Format = "$#,###.00";
            dg_inTransaction.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            if (dt_inTransaction.Rows.Count > 0)
            {
                bt_Save.Enabled = true;
                /*
                lb_FundName.Text = SystemLibrary.ToString(dt_inTransaction.Rows[0]["FundName"]);
                lb_Description.Text = SystemLibrary.ToString(dt_inTransaction.Rows[0]["Description"]);
                lb_Amount.Text = "For Amount: " + SystemLibrary.ToDecimal(dt_inTransaction.Rows[0]["Amount"]).ToString("$#,###.00");
                */
                AccountID = SystemLibrary.ToString(dt_inTransaction.Rows[0]["AccountID"]);
                crncy = SystemLibrary.ToString(dt_inTransaction.Rows[0]["crncy"]);
                EffectiveDate = Convert.ToDateTime(dt_inTransaction.Rows[0]["EffectiveDate"]);
                for (int j = 0; j < dg_inTransaction.Rows.Count; j++)
                    SystemLibrary.SetColumn(dg_inTransaction, "Amount", j);

            }
            else
            {
                bt_Save.Enabled = false;
                MessageBox.Show("Sorry could not find the Transaction.\r\nPlease close this window and try again");
                return;
            }
            for (int i = 0; i < dg_inTransaction.Columns.Count; i++)
                dg_inTransaction.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;


        } //StockLoanFeeApportion_Load()

        private void StockLoanFeeReporting_Load()
        {
            // Local Variables
            String mySql;
            String myFromDate = "null";
            String myToDate = "null";


            // Get the Fund ShortName involved in this
            mySql = "Select FundID, ShortName from Fund Where Active = 'Y' And AllowTrade = 'Y' And ParentFundID = (Select FundID From Transactions Where TranID = " + TranID + ") Order by ShortName";
            dt_Fund = SystemLibrary.SQLSelectToDataTable(mySql);

            // Setup the dg_Summary
            dg_Summary.Rows.Clear();
            for (int i = 0; i < dt_Fund.Rows.Count; i++)
            {
                int myRow = dg_Summary.Rows.Add();
                dg_Summary.Rows[myRow].Cells["FundID"].Value = dt_Fund.Rows[i]["FundID"];
                dg_Summary.Rows[myRow].Cells["FundName"].Value = dt_Fund.Rows[i]["ShortName"];
                dg_Summary.Rows[myRow].Cells["StockLoanFee"].Value = 0;
            }
            CheckImbalance();

            if (inStartup)
            {
                if (dt_inTransaction.Rows.Count > 0)
                {
                    // Set the ToDate to be the day the Stock Loan fee was paid.
                    dtp_ToDate.Value = Convert.ToDateTime(dt_inTransaction.Compute("Max(EffectiveDate)", ""));
                    myToDate = "'" + dtp_ToDate.Value.ToString("dd-MMM-yyyy") + "'";

                    // See when the last StockLoanFee occured and if withing 45 days (<2 months), then start with this+1
                    // Otherwise First Day of the month
                    mySql = "Select Max(t.EffectiveDate)+1 " +
                            "From	Transactions t, " +
                            "		Transactions t2 " +
                            "Where	t.TranType = 'Stock Loan Fee'  " +
                            "And		t2.TranID = " + TranID + " " +
                            "And		t.FundID = t2.FundID " +
                            "And		t.EffectiveDate between t2.EffectiveDate - 45 and t2.EffectiveDate - 1 ";
                    dtp_FromDate.Value = SystemLibrary.SQLSelectDateTime(mySql, SystemLibrary.FirstDayOfMonth(dtp_ToDate.Value));
                    myFromDate = "'" + dtp_FromDate.Value.ToString("dd-MMM-yyyy") + "'";
                }
            }
            else
            {
                myFromDate = "'" + dtp_FromDate.Value.ToString("dd-MMM-yyyy") + "'";
                myToDate = "'" + dtp_ToDate.Value.ToString("dd-MMM-yyyy") + "'";
            }

            // See if there are any existing Accrual Records for TranType = 'Interest' for this account.
            mySql = "Exec sp_ShortReport  " + TranID + ", " + myFromDate + ", " + myToDate + "";
            dt_Transactions = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_Transactions.DataSource = dt_Transactions;

            SystemLibrary.SetDataGridView(dg_Transactions);


        } //StockLoanFeeReporting_Load

        private void bt_Calculator_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc");

        } //bt_Calculator_Click()

        private void bt_Print_Click(object sender, EventArgs e)
        {
            SystemLibrary.PrintScreen(this);

        } //bt_Print_Click

        private Boolean CheckImbalance()
        {
            // Local Variables
            Decimal StockLoanFee = 0;
            Decimal InTransactionNet = SystemLibrary.ToDecimal(dt_inTransaction.Compute("Sum(Amount)", ""));

            for (int i = 0; i < dg_Summary.Rows.Count; i++)
                StockLoanFee = StockLoanFee + SystemLibrary.ToDecimal(dg_Summary["StockLoanFee", i].Value);


            tb_Imbalance.Text = (InTransactionNet - StockLoanFee).ToString("$#,##0.00");
            if (InTransactionNet != StockLoanFee)
            {
                bt_Save.Enabled = false;
                tb_Imbalance.ForeColor = Color.Red;
            }
            else
            {
                bt_Save.Enabled = true;
                tb_Imbalance.ForeColor = Color.Green;
            }

            return (bt_Save.Enabled);

        } //CheckImbalance()

        private void dg_Summary_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex > -1 && dg_Summary.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "StockLoanFee")
            {
                LastValue = dg_Summary[e.ColumnIndex, e.RowIndex].Value;
            }
            else
                e.Cancel = true;
        } //dg_Summary_CellBeginEdit()

        private void dg_Summary_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Need to make sure the value type is OK.
            Decimal myValue = SystemLibrary.ToDecimal(dg_Summary.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

            dg_Summary.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myValue;
            SystemLibrary.SetColumn(dg_Summary, dg_Summary.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name, e.RowIndex);
            LastValue = null;

            CheckImbalance();

        } //dg_Summary_CellEndEdit()

        private void bt_Save_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;
            Int32 myRows;
            Int32 NewTranID;

            if (!CheckImbalance())
            {
                MessageBox.Show("There is an imbalance between the Stock Loan Fee and the sub-funds.\r\n\r\n" +
                                "Please correct and try again.", "Save - Stock Loan Fee Apportion");
                return;
            }

            // Get the JournalID
            Int32 JournalID = SystemLibrary.SQLSelectInt32("exec sp_GetNextId 'JournalID'");

            // Update the Original Transaction 
            mySql = "Update Transactions " +
                    "Set JournalID = " + JournalID.ToString() + ", " +
                    "    Reconcilled = 'Y' " +
                    "Where  TranID = " + TranID.ToString() + " " +
                    "And    JournalID is Null ";
            myRows = SystemLibrary.SQLExecute(mySql);

            if (myRows<1)
            {
                MessageBox.Show("The original Stock Loan Fee appears to have been changed.\r\n\r\n" + 
                                "Please exit this screen and Refresh to see if it has been removed.\r\n" +
                                "Then try again if needed.","Save - Stock Loan Fee Apportion");
                return;
            }

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            // Create a reversing line item
            NewTranID = SystemLibrary.SQLSelectInt32("exec sp_GetNextId 'TranID'");
            mySql = "Insert into Transactions(TranID,AccountID,FundID,PortfolioID,crncy,EffectiveDate,RecordDate,Amount,Reconcilled,Description,TranType,JournalID) " +
                    "Select "+ NewTranID.ToString() + ",AccountID,FundID,PortfolioID,crncy,EffectiveDate,RecordDate,-Amount,Reconcilled,Description,TranType,JournalID " +
	                "From	Transactions " +
	                "Where	TranID = " + TranID.ToString();
            SystemLibrary.SQLExecute(mySql);

            // Add the new sub-fund splits
            for (int i = 0; i < dg_Summary.Rows.Count; i++)
            {
                Int32 FundID = SystemLibrary.ToInt32(dg_Summary["FundID", i].Value);
                Decimal StockLoanFee = SystemLibrary.ToDecimal(dg_Summary["StockLoanFee", i].Value);

                if (StockLoanFee != 0)
                {
                    NewTranID = SystemLibrary.SQLSelectInt32("exec sp_GetNextId 'TranID'");
                    mySql = "Insert into Transactions(TranID,AccountID,FundID,PortfolioID,crncy,EffectiveDate,RecordDate,Amount,Reconcilled,Description,TranType,JournalID) " +
                            "Select "+ NewTranID.ToString() + ",AccountID," + FundID.ToString() + ",PortfolioID,crncy,EffectiveDate,RecordDate," + StockLoanFee.ToString() + ",Reconcilled,Description,TranType,JournalID " +
	                        "From	Transactions " +
	                        "Where	TranID = " + TranID.ToString();
                    SystemLibrary.SQLExecute(mySql);
                }
            }

            mySql = "Exec sp_Apply_Trans_to_Profit";
            SystemLibrary.SQLExecute(mySql);

            Cursor.Current = Cursors.Default;

            // Finshed
            MessageBox.Show("Journal Stock Loan Fee Created\r\n\r\nWill now close the window.", "Journal Stock Loan Fee");
            if (ParentForm1 is ProcessUnallocatedTransactions)
            {
                ((ProcessUnallocatedTransactions)ParentForm1).RefreshData();
            }

            this.Close();


        } //bt_Save_Click()

    }
}
