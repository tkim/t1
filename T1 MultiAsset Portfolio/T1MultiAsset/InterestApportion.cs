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
    public partial class InterestApportion : Form
    {
        // Public Variables
        public Form ParentForm1;
        DataTable dt_Transactions;
        DataTable dt_inTransaction;
        //DataTable dt_Accruals;
        DataTable dt_Fund;
        String AccountID;
        Int32 FundID = Int32.MinValue;
        String crncy;
        DateTime EffectiveDate;
        Object  LastValue;
        Boolean inStartup = true;
        Boolean UsingDailyBalance = false;
        Boolean isParentFund = false;
        int CountSubFunds = 0;


        int TranID;

        public InterestApportion()
        {
            InitializeComponent();
        }

        private void InterestApportion_Load(object sender, EventArgs e)
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
                    "And		t.AccountID = t2.AccountID " +
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
                FundID = SystemLibrary.ToInt32(dt_inTransaction.Rows[0]["FundID"]);
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

            //InterestAccrualReporting_Load();

        } //InterestApportion_Load()

        private void InterestAccrualReporting_Load()
        {
            // Local Variables
            String mySql;
            String myFromDate = "null";
            String myToDate = "null";


            // Get the Fund ShortName involved in this
            mySql = "Select FundID, ShortName from Fund";
            dt_Fund = SystemLibrary.SQLSelectToDataTable(mySql);

            if (inStartup)
            {
                if (dt_inTransaction.Rows.Count > 0)
                {
                    // Set the ToDate to be the day before the interest was paid.
                    dtp_ToDate.Value = Convert.ToDateTime(dt_inTransaction.Compute("Max(EffectiveDate)", "")).AddDays(-1.0);
                    // Check the Record Date is not earlier
                    DateTime RecordDate = Convert.ToDateTime(dt_inTransaction.Compute("Max(RecordDate)", ""));
                    if (DateTime.Compare(RecordDate, dtp_ToDate.Value) < 0)
                        dtp_ToDate.Value = RecordDate;
                    myToDate = "'" + dtp_ToDate.Value.ToString("dd-MMM-yyyy") + "'";
                }
            }
            else
            {
                myFromDate = "'" + dtp_FromDate.Value.ToString("dd-MMM-yyyy") + "'";
                myToDate = "'" + dtp_ToDate.Value.ToString("dd-MMM-yyyy") + "'";
            }
            // See if there are any existing Accrual Records for TranType = 'Interest' for this account.
            mySql = "Exec sp_InterestAccrualReporting  " + AccountID + ", '" + crncy + "', " + myFromDate + ", " + myToDate + "";
            dt_Transactions = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_Transactions.DataSource = dt_Transactions;

            if (dt_Transactions.Rows.Count == 0)
            {
                // Try to see if this is just an apportioned interest account
                UsingDailyBalance = true;
                mySql = "Exec sp_InterestAllocationByBalance  " + TranID.ToString() + ", " + myFromDate + ", " + myToDate + "";
                dt_Transactions = SystemLibrary.SQLSelectToDataTable(mySql);
                dg_Transactions.DataSource = dt_Transactions;
                if (dt_Transactions.Rows.Count > 0)
                {
                    lb_AccrualNotes.Text = "Estimate Interest Based on Daily Balances";
                    Font AN_Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                    lb_AccrualNotes.Font = AN_Font;
                }

            }

            // Add a column that lets the users signoff on data
            /*
            if (dg_Transactions.Columns.Contains("Rec"))
                dg_Transactions.Columns.Remove("Rec");
            DataGridViewCheckBoxColumn Rec = new DataGridViewCheckBoxColumn();
            Rec.HeaderText = "Rec";
            Rec.FalseValue = "N";
            Rec.TrueValue = "Y";
            Rec.Name = "Rec";
            dg_Transactions.Columns.Insert(3,Rec);
            dg_Transactions.Columns["Rec"].ReadOnly = false;
            */

            if (inStartup && dt_Transactions.Rows.Count>0)
            {
                // Set the From & To date to match the data
                dtp_FromDate.Value = Convert.ToDateTime(dt_Transactions.Compute("Min(EffectiveDate)", "")); 
                dtp_ToDate.Value = Convert.ToDateTime(dt_Transactions.Compute("Max(EffectiveDate)", ""));
                inStartup = false;
            }

            for (int i = 0; i < dg_Transactions.Columns.Count; i++)
            {
                // Local Variables
                String myColumn = dg_Transactions.Columns[i].Name;

                if (myColumn.StartsWith("TranID"))
                    dg_Transactions.Columns[i].Visible = false;
                else if (myColumn == "Rec")
                {
                    for (int j = 0; j < dg_Transactions.Rows.Count; j++)
                        dg_Transactions.Rows[j].Cells[i].Value = "N";
                }
                else if (myColumn == "IntRate")
                {
                    dg_Transactions.Columns[i].HeaderText = "Interest Rate";
                    dg_Transactions.Columns[i].DefaultCellStyle.BackColor = Color.LightGray;
                    dg_Transactions.Columns[i].DefaultCellStyle.Format = "0.000000%";
                    dg_Transactions.Columns[i].ReadOnly = true;
                }
                else if (myColumn == "EffectiveDate")
                {
                    dg_Transactions.Columns[i].HeaderText = "Effective Date";
                    dg_Transactions.Columns[i].DefaultCellStyle.Format = "dd MMM yy";
                    dg_Transactions.Columns[i].ReadOnly = true;
                }
                else if (myColumn == "Balance" || myColumn == "Interest")
                {
                    dg_Transactions.Columns[i].DefaultCellStyle.Format = "$#,###.00";
                    dg_Transactions.Columns[i].DefaultCellStyle.BackColor = Color.LightGray;
                    dg_Transactions.Columns[i].ReadOnly = true;
                    for (int j = 0; j < dg_Transactions.Rows.Count; j++)
                        SystemLibrary.SetColumn(dg_Transactions, myColumn, j);
                }
                else if (myColumn.StartsWith("Balance_"))
                {
                    CountSubFunds++;
                    dg_Transactions.Columns[i].DefaultCellStyle.Format = "$#,###.00";
                    dg_Transactions.Columns[i].ReadOnly = true;
                    for (int j = 0; j < dg_Transactions.Rows.Count; j++)
                        SystemLibrary.SetColumn(dg_Transactions, myColumn, j);

                }
                else
                {
                    dg_Transactions.Columns[i].DefaultCellStyle.Format = "$#,###.00";
                    dg_Transactions.Columns[i].DefaultCellStyle.BackColor = Color.LightCyan;
                    dg_Transactions.Columns[i].ReadOnly = false;
                    for (int j = 0; j < dg_Transactions.Rows.Count; j++)
                        SystemLibrary.SetColumn(dg_Transactions, myColumn, j);
                }
                dg_Transactions.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dg_Transactions.Columns[i].HeaderText = ColumnHeaderFromFundID(myColumn);
            }

            SetUpHeader();
            SetHeader();
            ApportionHeaderIntPaid(true);

            /*
            if (CountSubFunds == 1)
            {
                dg_Transactions.ReadOnly = true;
                dg_Summary.ReadOnly = true;

                MessageBox.Show("There are no sub-funds for this to Interest record:\r\n" +
                                "Hence, there is no need for any Journal to be created.\r\n\r\n" +
                                "Please press [Save] to mark the Interest as reconcilled.");
            }
            else 
            */
            if (UsingDailyBalance && dt_Transactions.Rows.Count > 0)
            {
                MessageBox.Show("The Daily Interest table has been created using the following Rules:\r\n" +
                                "   1) Interest Rate is constant across the period;\r\n" +
                                "   2) Interest is apportioned on a pro-rata basis across Daily Balances per sub-fund;\r\n" +
                                "   3) These transactions are presented for calculating the 'Interest Paid' line in the summary block, and will not be kept\r\n\r\n" +
                                "It is suggested that you download the Interest Report associated with the payment and make adjustments if/where needed.\r\n\r\n" +
                                "You do not need to adjust daily figures, but can update the 'Interest Paid' line in the summary block to define the split between sub-funds\r\n\r\n" +
                                "It is suggested you [Print] this page before pressing the [Save] button for future records.");
            }

        } //InterestAccrualReporting_Load()

        private String ColumnHeaderFromFundID(String myColName)
        {
            String myShortName = myColName;

            // Find the Fund details
            int FoundPos = myColName.IndexOf('_');
            if (FoundPos > 0)
            {
                String myStart = myColName.Substring(0, myColName.IndexOf('_'));
                String myFund = myColName.Substring(myColName.IndexOf('_') + 1);
                String myCol = myColName.Substring(0, myColName.IndexOf('_'));
                DataRow[] dr = dt_Fund.Select("FundId=" + myFund);
                if (dr.Length > 0)
                {
                    myShortName = myStart + " " + dr[0]["ShortName"].ToString();
                }
            }
            return (myShortName);
        } //ColumnHeaderFomFundID()

        private String ColumnHeaderFundID(String myColName)
        {
            String myFund = "";

            // Find the Fund details
            int FoundPos = myColName.IndexOf('_');
            if (FoundPos > 0)
            {
                myFund = myColName.Substring(myColName.IndexOf('_') + 1);
            }

            return (myFund);

        } //ColumnHeaderFundID()

        public void FromParent(Form inForm, int inTranID)
        {
            ParentForm1 = inForm;
            TranID = inTranID;

        } //FromParent()

        private void InterestApportion_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);
            InterestAccrualReporting_Load();

        } //InterestApportion_Shown()

        private void dg_Transactions_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dg_Transactions.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.StartsWith("Interest_"))
            {
                // If No TranID, then also cannot change
                String FundID = ColumnHeaderFundID(dg_Transactions.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name);
                if (!UsingDailyBalance)
                {
                    if (SystemLibrary.ToInt32(dg_Transactions.Rows[e.RowIndex].Cells["TranID_" + FundID].Value) == 0)
                    {
                        if (MessageBox.Show(this, "WARNING:\r\n\r\nThere is no original Interest Transaction associated with this fund on the day being altered.\r\n\r\n" +
                                                  "You are about to Create a Transaction.\r\n Make sure the Fund was 'Active' at the time you are adding Interest.\r\n\r\n\r\n" +
                                                  "Do you wish to Continue?", "Modify Interest", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                LastValue = dg_Transactions[e.ColumnIndex, e.RowIndex].Value;
            }
            else
                e.Cancel = true;

        } //dg_Transactions_CellBeginEdit()

        private void dg_Transactions_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Need to make sure the value type is OK.
            Decimal myValue = SystemLibrary.ToDecimal(dg_Transactions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

            dg_Transactions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myValue;
            SystemLibrary.SetColumn(dg_Transactions, dg_Transactions.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name, e.RowIndex);
            LastValue = null;


            //Need to update totals per-row, and any sums.
            Decimal TotalInterest = 0;
            for (int i = 0; i < dg_Transactions.Columns.Count; i++)
            {
                if (dg_Transactions.Rows[e.RowIndex].Cells[i].OwningColumn.Name.StartsWith("Interest_"))
                    TotalInterest = TotalInterest + SystemLibrary.ToDecimal(dg_Transactions.Rows[e.RowIndex].Cells[i].Value);
            }
            dg_Transactions.Rows[e.RowIndex].Cells["Interest"].Value = TotalInterest;
            SystemLibrary.SetColumn(dg_Transactions, "Interest", e.RowIndex);

            SetHeader();

        } //dg_Transactions_CellEndEdit()

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            InterestAccrualReporting_Load();

        } //bt_Refresh_Click()

        private void SetUpHeader()
        {
            // Local Variables
            Font FontBold = new Font(dg_Summary.RowsDefaultCellStyle.Font, FontStyle.Bold);


            
            DataGridViewCellStyle dGVCS = new DataGridViewCellStyle();
            dGVCS.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dGVCS.BackColor = Color.DarkSlateBlue;
            //dGVCS.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            dGVCS.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dGVCS.ForeColor = Color.White;
            dGVCS.SelectionBackColor = SystemColors.Highlight;
            dGVCS.SelectionForeColor = SystemColors.HighlightText;
            dGVCS.WrapMode = DataGridViewTriState.True;

            // Get the columns to match the Funds in dg_Transactions
            for (int i = dg_Summary.Columns.Count-1; i > 1; i--)
                dg_Summary.Columns.Remove(dg_Summary.Columns[i].Name);
            dg_Summary.Columns["Label"].ReadOnly = true;
            dg_Summary.Columns["Interest"].ReadOnly = true;

            for (int i = 0; i < dg_Transactions.Columns.Count; i++)
            {
                // Local Variables
                String myColumn = dg_Transactions.Columns[i].Name;

                if (myColumn.StartsWith("Interest_"))
                {
                    int myCol = dg_Summary.Columns.Add(myColumn, dg_Transactions.Columns[i].HeaderText.Substring("Interest ".Length));
                    dg_Summary.Columns[myCol].DefaultCellStyle.Format = "$#,###.00";
                    //dg_Summary.Columns[myCol].DefaultCellStyle.BackColor = Color.LightCyan;
                    dg_Summary.Columns[myCol].HeaderCell.Style = dGVCS;
                    dg_Summary.Columns[myCol].ReadOnly = false;
                }
            }


            dg_Summary.Rows.Add(4);
            dg_Summary.Rows[0].Cells["Label"].Value = "Debit Interest";
            dg_Summary.Rows[1].Cells["Label"].Value = "Credit Interest";
            dg_Summary.Rows[2].DefaultCellStyle.Font = FontBold;
            dg_Summary.Rows[2].DefaultCellStyle.BackColor = Color.LightGray;
            dg_Summary.Rows[2].Cells["Label"].Value = "Net";
            dg_Summary.Rows[3].DefaultCellStyle.Font = FontBold;
            dg_Summary.Rows[3].DefaultCellStyle.ForeColor = Color.DarkBlue;
            dg_Summary.Rows[3].Cells["Label"].Value = "Interest Paid";
            dg_Summary.Rows[3].DefaultCellStyle.BackColor = Color.LightCyan;

        } //SetUpHeader()

        private void SetHeader()
        {
            // Put this Try{} here as could be closing down
            try
            {
                // Fill in the Interest column
                dg_Summary["Interest", 0].Value = SystemLibrary.ToDecimal(dt_Transactions.Compute("Sum(Interest)", "Interest<0"));
                dg_Summary["Interest", 1].Value = SystemLibrary.ToDecimal(dt_Transactions.Compute("Sum(Interest)", "Interest>0"));
                dg_Summary["Interest", 2].Value = SystemLibrary.ToDecimal(dt_Transactions.Compute("Sum(Interest)", ""));

                // Loop across headers
                for (int i = 2; i < dg_Summary.Columns.Count; i++)
                {
                    String myColName = dg_Summary.Columns[i].Name;
                    dg_Summary[i, 0].Value = SystemLibrary.ToDecimal(dt_Transactions.Compute("Sum(" + myColName + ")", "" + myColName + "<0"));
                    dg_Summary[i, 1].Value = SystemLibrary.ToDecimal(dt_Transactions.Compute("Sum(" + myColName + ")", "" + myColName + ">0"));
                    dg_Summary[i, 2].Value = SystemLibrary.ToDecimal(dt_Transactions.Compute("Sum(" + myColName + ")", ""));
                }

                // Fill in the row to be journaled
                dg_Summary["Interest", 3].Value = SystemLibrary.ToDecimal(dt_inTransaction.Compute("Sum(Amount)", ""));

                // Add formats
                for (int i = 1; i < dg_Summary.Columns.Count; i++)
                    for (int j = 0; j < dg_Summary.Rows.Count; j++)
                        SystemLibrary.SetColumn(dg_Summary, dg_Summary.Columns[i].Name, j);

            }
            catch { }

            ApportionHeaderIntPaid();
            CheckedHeaderImbalance();

        } //SetHeader()

        public void ApportionHeaderIntPaid()
        {
            ApportionHeaderIntPaid(false);
        }

        public void ApportionHeaderIntPaid(Boolean WeightIfNeeded)
        {
            // See if Interest paid is the same as accrual.
            Decimal IntPaid = SystemLibrary.ToDecimal(dg_Summary["Interest", 3].Value);
            Decimal IntAccrual = SystemLibrary.ToDecimal(dg_Summary["Interest", 2].Value);
            Decimal Sum_Values = 0;

            if (IntPaid == IntAccrual)
            {
                for (int i = 2; i < dg_Summary.Columns.Count; i++)
                {
                    dg_Summary[i, 3].Value = dg_Summary[i, 2].Value;
                    SystemLibrary.SetColumn(dg_Summary, dg_Summary.Columns[i].Name, 3);
                }
                
                lb_Imbalance.Text = "";
                bt_Save.Enabled = true;
            }
            else if (WeightIfNeeded)
            {
                // IntAccrual is not the same as IntPaid, so use the IntAccrual weights to get an answer
                for (int i = 2; i < dg_Summary.Columns.Count; i++)
                {
                    dg_Summary[i, 3].Value = Math.Round(SystemLibrary.ToDecimal(dg_Summary[i, 2].Value) / IntAccrual * IntPaid, 2);
                    Sum_Values = Sum_Values + SystemLibrary.ToDecimal(dg_Summary[i, 3].Value);
                    if (i == dg_Summary.Columns.Count - 1)
                    {
                        // Adjust for rounding on last record
                        dg_Summary[i, 3].Value = SystemLibrary.ToDecimal(dg_Summary[i, 3].Value) + (IntPaid - Sum_Values);
                    }

                    SystemLibrary.SetColumn(dg_Summary, dg_Summary.Columns[i].Name, 3);
                }

                lb_Imbalance.Text = "";
                bt_Save.Enabled = true;
            }

            



        } //ApportionHeaderIntPaid()

        private void CheckedHeaderImbalance()
        {
            Decimal InTransactionNet = SystemLibrary.ToDecimal(dt_inTransaction.Compute("Sum(Amount)", ""));
            Decimal IntPaid = SystemLibrary.ToDecimal(dg_Summary["Interest", 3].Value);
            Decimal IntAccrual = SystemLibrary.ToDecimal(dg_Summary["Interest", 2].Value);

            bt_Save.Enabled = true;
            dg_Summary["Interest", 3].Style.BackColor = Color.LightCyan;

            // Cant allow user to Save when InTransactionNet <> IntPaid in summary table.
            if (IntPaid != InTransactionNet)
            {
                lb_Imbalance.Text = "'Interest Paid' Must add to the Sum(Amount) of the transactions =  " + InTransactionNet.ToString("[$#,###.00") + " - Out by " + (IntPaid - InTransactionNet).ToString("$#,###.00 ]");
                dg_Summary["Interest", 3].Style.BackColor = Color.LightPink;
                bt_Save.Enabled = false;
            }
            else if (IntPaid != IntAccrual)
            {
                // Let the user know if the "Interest Paid" row is out of balance
                lb_Imbalance.Text = "Interest Paid is Not equal to Interest Accrued " + IntPaid.ToString("[ Paid=  $#,###.00") + IntAccrual.ToString(", Accrued=$#,###.00 ") + 
                                    (IntPaid - IntAccrual).ToString(", Diff=  $#,###.00 ]");
            }
            else
            {
                lb_Imbalance.Text = "";
            }

        } //CheckedHeaderImbalance()

        private void dg_Summary_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex==3 && dg_Summary.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.StartsWith("Interest_"))
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


            //Need to update totals per-row, and any sums.
            Decimal TotalInterest = 0;
            for (int i = 0; i < dg_Summary.Columns.Count; i++)
            {
                if (dg_Summary.Rows[e.RowIndex].Cells[i].OwningColumn.Name.StartsWith("Interest_"))
                    TotalInterest = TotalInterest + SystemLibrary.ToDecimal(dg_Summary.Rows[e.RowIndex].Cells[i].Value);
            }
            dg_Summary.Rows[e.RowIndex].Cells["Interest"].Value = TotalInterest;
            SystemLibrary.SetColumn(dg_Summary, "Interest", e.RowIndex);

            CheckedHeaderImbalance();

        } //dg_Summary_CellEndEdit()

        private void bt_Calculator_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc");

        } //bt_Calculator_Click()

        private void bt_Save_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;
            int JournalID;

            // Warn user if the Int Paid != Net Accruals, but allow them to go ahead.
            Decimal IntPaid = SystemLibrary.ToDecimal(dg_Summary["Interest", 3].Value);
            Decimal IntAccrual = SystemLibrary.ToDecimal(dg_Summary["Interest", 2].Value);

            if (IntPaid != IntAccrual)
            {
                // This is allowable, but just warn user
                if (MessageBox.Show(this, "Creating Journals to Net Interest Accruals and Apply Interest Paid\r\n\r\n" +
                                          "You are about to Reverse the Accruals that add up to " + IntAccrual.ToString("$#,###.00") + "\r\n" +
                                          "When the Net Interest Received = " + IntPaid.ToString("$#,###.00") + "\r\n\r\n" +
                                          "This is allowable, but is this what you meant to do!\r\n\r\n" +
                                          "Do you wish to Continue?", "Create Journal for Interest", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    MessageBox.Show("Journal Records have not been created.", "Create Journal for Interest - Aborted");
                    return;
                }
            }

            if (UsingDailyBalance)
            {
                if (MessageBox.Show(this, "NOTE:\r\nIt is suggested you [Print] this page before pressing the [Save] button for future records.\r\n\r\n" +
                                          "ALSO:\r\nThis screen will NOT create the daily Accruals from the Estimates presented, but will apportion Interest to each sub-fund as per the summary.\r\n\r\n" +
                                          "If you wish to create Accruals, then answer [No] and use the 'Define Interest Accrual' module to set the rates.\r\n\r\n" +
                                          "Do you wish to Continue?", "Create Journal for Interest", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    MessageBox.Show("Journal Records have not been created.", "Create Journal for Interest - Aborted");
                    return;
                }
            }

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            // Get the JournalID
            JournalID = SystemLibrary.SQLSelectInt32("Exec sp_GetNextId 'JournalID'");

            if (!UsingDailyBalance)
            {
                // Update All TranID's in dg_Transaction
                for (int j = 0; j < dg_Transactions.Columns.Count; j++)
                {
                    if (dg_Transactions.Columns[j].Name.StartsWith("TranID_"))
                    {
                        for (int i = 0; i < dg_Transactions.Rows.Count; i++)
                        {
                            int TranID = SystemLibrary.ToInt32(dg_Transactions.Rows[i].Cells[j].Value);
                            if (TranID > 0)
                            {
                                mySql = "Update Transactions Set Reconcilled = 'Y', " +
                                        "JournalID = " + JournalID.ToString() + " " +
                                        "Where TranID = " + TranID.ToString();
                                SystemLibrary.SQLExecute(mySql);
                            }
                        }
                    }
                }
            }

            // Insert the Reversals
            if (!UsingDailyBalance)
            {
                for (int j = 2; j < dg_Summary.Columns.Count; j++)
                {
                    String ColumnName = dg_Summary.Columns[j].Name;
                    String FundID = ColumnHeaderFundID(ColumnName);
                    Decimal Accrual = SystemLibrary.ToDecimal(dg_Summary[ColumnName, 2].Value);

                    if (FundID.Length > 0) // Should never fail
                    {
                        int TranID = SystemLibrary.SQLSelectInt32("Exec sp_GetNextId 'TranID'");
                        mySql = "Insert into Transactions (TranID,AccountID,FundID,crncy,EffectiveDate,RecordDate,Amount,Reconcilled,Description,Source_AccountID,TranType,JournalID) " +
                                "Select " + TranID.ToString() + ",AccountID,FundID,'" + crncy + "','" + EffectiveDate.ToString("dd-MMM-yyyy") + "','" + EffectiveDate.ToString("dd-MMM-yyyy") + "'," + (-Accrual).ToString() + ",'Y','Net Interest Accrual'," + AccountID + ",'Interest'," + JournalID.ToString() + " " +
                                "From   Accounts " +
                                "Where Fundid = " + FundID + " " +
                                "And   AccountType = 'ACCRUAL' ";
                        SystemLibrary.SQLExecute(mySql);
                    }
                }
            }

            // Add Journal ID to Interest Paid records
            for (int i = 0; i < dg_inTransaction.Rows.Count; i++)
            {
                String TranID = SystemLibrary.ToString(dg_inTransaction["TranID", i].Value);
                mySql = "Update Transactions " +
                        "Set Reconcilled = 'Y', " +
                        "    JournalID = " + JournalID.ToString() + " " +
                        "Where  TranID = " + TranID;

                // Test to see if jut 1 fund and that it is the parentfund
                if (dg_Summary.Columns.Count == 3)
                {
                    String ColumnName = dg_Summary.Columns[2].Name;
                    Int32 myFundID = SystemLibrary.ToInt32(ColumnHeaderFundID(ColumnName));
                    if (myFundID == FundID)
                    {
                        // No need to create reversals
                        isParentFund = true;
                    }
                }

                if (TranID.Length > 0)
                {
                    SystemLibrary.SQLExecute(mySql);

                    if (isParentFund == false)
                    {
                        // Reverse out the Interest Paid records
                        int NewTranID = SystemLibrary.SQLSelectInt32("Exec sp_GetNextId 'TranID'");
                        mySql = "Insert into Transactions (TranID,AccountID,FundID,crncy,EffectiveDate,RecordDate,Amount,Reconcilled,Description,Source_AccountID,TranType,JournalID) " +
                                "Select " + NewTranID.ToString() + ",AccountID,FundID,crncy,EffectiveDate,RecordDate,-Amount,'Y',Description,Source_AccountID,'Interest'," + JournalID.ToString() + " " +
                                "From   Transactions " +
                                "Where  TranID = " + TranID;
                        SystemLibrary.SQLExecute(mySql);
                    }
                }
            }

            // Add new records to the sub-funds
            if (isParentFund == false)
            {
                for (int j = 2; j < dg_Summary.Columns.Count; j++)
                {
                    String ColumnName = dg_Summary.Columns[j].Name;
                    String FundID = ColumnHeaderFundID(ColumnName);
                    Decimal Accrual = SystemLibrary.ToDecimal(dg_Summary[ColumnName, 3].Value);

                    if (FundID.Length > 0) // Should never fail
                    {
                        int TranID = SystemLibrary.SQLSelectInt32("Exec sp_GetNextId 'TranID'");
                        mySql = "Insert into Transactions (TranID,AccountID,FundID,crncy,EffectiveDate,RecordDate,Amount,Reconcilled,Description,Source_AccountID,TranType,JournalID) " +
                                "Select " + TranID.ToString() + "," + AccountID + ",FundID,'" + crncy + "','" + EffectiveDate.ToString("dd-MMM-yyyy") + "','" + EffectiveDate.ToString("dd-MMM-yyyy") + "'," + Accrual.ToString() + ",'Y','Net Interest',null,'Interest'," + JournalID.ToString() + " " +
                                "From   Accounts " +
                                "Where Fundid = " + FundID + " " +
                                "And   AccountType = 'ACCRUAL' ";
                        SystemLibrary.SQLExecute(mySql);
                    }
                }
            }

            // Apply any updates the user made to Daily Interest Accruals
            if (!UsingDailyBalance)
                ApplyInterestAccrualChanges();

            // Clear up any other transaction dates that may have been dependant on this
            SystemLibrary.SQLExecute("exec sp_BalanceTransfersBetweenAccounts");

            // Now push the Transactions into profit
            SystemLibrary.SQLExecute("exec sp_Apply_Trans_to_Profit");

            Cursor.Current = Cursors.Default;

            // Finshed
            MessageBox.Show("Journal Interest Created\r\n\r\nWill now close the window.", "Journal Interest");
            if (ParentForm1 is ProcessUnallocatedTransactions)
            {
                ((ProcessUnallocatedTransactions)ParentForm1).RefreshData();
            }
               
            this.Close();


        } //bt_Save_Click()

        private void ApplyInterestAccrualChanges()
        {
            // Loop around Transactions to see if any have changed, then update
            dg_Transactions.Refresh();
            
            DataTable dt_Modified = dt_Transactions.GetChanges(DataRowState.Modified);

            if (dt_Modified == null)
                return;


            for (int i = 0; i < dt_Modified.Rows.Count; i++)
            {
                // Loop across columns
                for (int j = 0; j < dt_Modified.Columns.Count; j++)
                {
                    // Use the Interest_<FundID> column as the trigger.
                    String mySql;
                    String ColumnName = dt_Modified.Columns[j].ColumnName;
                    if (ColumnName.StartsWith("Interest_"))
                    {
                        // The only COlumn that can change is the Interest
                        String FundID = ColumnHeaderFundID(ColumnName);
                        // Get the tranID 
                        String TranID = SystemLibrary.ToString(dt_Modified.Rows[i]["TranID_" + FundID]);
                        Decimal Interest = SystemLibrary.ToDecimal(dt_Modified.Rows[i]["Interest_" + FundID]);
                        String myBalance = SystemLibrary.ToDecimal(dt_Modified.Rows[i]["Balance_" + FundID]).ToString("#,##0.00");
                        String myDescription;
                        String myDate = Convert.ToDateTime(dt_Modified.Rows[i]["EffectiveDate"]).ToString("dd-MMM-yyyy");
                        if (TranID == "0" && Interest == 0)
                            continue;
                        else if (TranID == "0")
                        {
                            TranID = SystemLibrary.ToString(SystemLibrary.SQLSelectInt32("Exec sp_GetNextId 'TranID'"));
                            if (Interest < 0)
                                myDescription = "Borrow Interest [Balance = " + myBalance + "]";
                            else
                                myDescription = "Earn Interest [Balance = " + myBalance + "]";

                            mySql = "Insert into Transactions (TranID,AccountID,FundID,crncy,EffectiveDate,RecordDate,Amount,Reconcilled,Description,Source_AccountID,TranType) " +
                            "Select " + TranID + ",AccountID,FundID,'" + crncy + "','" + myDate + "','" + myDate + "'," + Interest.ToString("#.##") + ",'N','" + myDescription + "'," + AccountID + ",'Interest' " +
                            "From   Accounts " +
                            "Where Fundid = " + FundID + " " +
                            "And   AccountType = 'ACCRUAL' ";
                        }
                        else
                        {
                            mySql = "Update Transactions " +
                                    "Set Amount = " + Interest.ToString("#.##") + " " +
                                    "Where TranID = " + TranID + " " +
                                    "And   Amount <> " + Interest + " ";
                        }
                        SystemLibrary.DebugLine(mySql);
                        
                        int myRows = SystemLibrary.SQLExecute(mySql);
                        if (myRows > 0)
                        {
                            mySql = "Delete from Profit Where TranID = " + TranID;
                            SystemLibrary.SQLExecute(mySql);
                        }
                    }
                }
            }


        } //ApplyInterestAccrualChanges

        private void bt_Print_Click(object sender, EventArgs e)
        {
            SystemLibrary.PrintScreen(this);

        } //bt_Print_Click

    }
}
