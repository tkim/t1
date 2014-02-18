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
    public partial class ProcessJournals : Form
    {
        // Global Variables
        public Form ParentForm1;
        public String TranID;
        public String FundID;
        public String ParentFundID;
        public String AccountID;
        public String PortfolioID;
        DataTable dt_Accounts;
        DataTable dt_Fund;
        DataTable dt_TranType;
        //DataTable dt_Portfolio;
        //private int CXLocation = 0;
        //private int CYLocation = 0;
        public object LastValue;
        public Boolean HavePassedInData = false;


        public ProcessJournals()
        {
            InitializeComponent();
            lb_TranType.Text = "";
        }

        private void ProcessJournals_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            LoadData();
        }

        private void LoadData()
        {
            Console.WriteLine("LoadData()");
        } //LoadData()

        // Use this when not passing in a know Journal Action
        public void FromParent(Form inForm)
        {
            ParentForm1 = inForm;
            HavePassedInData = false;
        }

        public void FromParent(Form inForm, int inTranID, String inTranType, DateTime inEffectiveDate, int inAccountID, String inAccountType, int inParentFundID, int inFundID, String inPortfolioID, String inDescription, Double inAmount, String incrncy)
        {
            // Local Variables
            String mySql;

            ParentForm1 = inForm;
            PortfolioID = inPortfolioID;
            HavePassedInData = true; // Needed so that form knows if it needs to load other data

            // What action is this defaulting to:
            if (inTranType.ToUpper() == "Franking Credit".ToUpper() && inParentFundID != inFundID)
            {
                // Move Franking Credit to another Sub-Fund(s)
                lb_TranType.Text = inTranType;
                rb_BetweenSubFunds.Checked = true;
            }
            else if (inTranType.ToUpper() == "Dividend".ToUpper() && inParentFundID != inFundID)
            {
                // Move Dividend to another Sub-Fund(s)
                lb_TranType.Text = inTranType;
                rb_BetweenSubFunds.Checked = true;
            }
            else if (inTranType.ToUpper() == "Interest".ToUpper() && inParentFundID != inFundID)
            {
                // Apportion Interest to another Sub-Fund(s)
                lb_TranType.Text = inTranType;
                rb_BetweenSubFunds.Checked = true;
            }

            TranID = inTranID.ToString();
            FundID = inFundID.ToString();
            ParentFundID = inParentFundID.ToString();
            AccountID = inAccountID.ToString();
            dtp_EffectiveDate.Value = inEffectiveDate;
            dtp_EffectiveDate.Enabled = false;
            cb_crncy.Text = incrncy;
            cb_crncy.Enabled = false;

            dt_Fund = SystemLibrary.SQLSelectToDataTable("Select * From Fund Where ParentFundID = " + ParentFundID + " And AllowTrade = 'Y'");
            DataGridViewComboBoxColumn dcb_FundID = (DataGridViewComboBoxColumn)dg_Journal.Columns["j_FundID"];
            dcb_FundID.DataSource = dt_Fund;
            dcb_FundID.DisplayMember = "FundName";
            dcb_FundID.ValueMember = "FundID";

            // Cannot Change the Account
            if (inAccountType.ToUpper() == "CASH")
                mySql = "Select * From Accounts Where AccountID = " + AccountID;
            else
                mySql = "Select * From Accounts Where AccountType = '" + inAccountType + "' and FundID in (Select FundID from Fund Where ParentFundID = " + inParentFundID + ")";
            dt_Accounts = SystemLibrary.SQLSelectToDataTable(mySql);
            DataGridViewComboBoxColumn dcb_AccountID = (DataGridViewComboBoxColumn)dg_Journal.Columns["j_AccountID"];
            dcb_AccountID.DataSource = dt_Accounts;
            dcb_AccountID.DisplayMember = "AccountName";
            dcb_AccountID.ValueMember = "AccountID";

            dt_TranType = SystemLibrary.SQLSelectToDataTable("Select * From TranType Where TranType = '" + inTranType + "' ");
            DataGridViewComboBoxColumn dcb_TranType = (DataGridViewComboBoxColumn)dg_Journal.Columns["j_TranType"];
            dcb_TranType.DataSource = dt_TranType;
            dcb_TranType.DisplayMember = "TranType";
            dcb_TranType.ValueMember = "TranType";

            if (inAccountType.ToUpper() != "ACCRUAL")
                rb_ReverseAccrual.Enabled = false;

            dg_Journal.Columns["j_TranType"].ReadOnly = true;

            // Add the journal Entries that are needed
            lb_Imbalance.Text = "";
            int myRow = dg_Journal.Rows.Add();
            dg_Journal.Rows[myRow].Cells["j_AccountID"].Value = inAccountID;
            dg_Journal.Rows[myRow].Cells["j_FundID"].Value = inFundID;
            dg_Journal.Rows[myRow].Cells["j_Description"].Value = inDescription;
            dg_Journal.Rows[myRow].Cells["j_TranType"].Value = inTranType;
            dg_Journal.Rows[myRow].Cells["j_Amount"].Value = -inAmount;
            dg_Journal.Rows[myRow].Cells["j_PortfolioID"].Value = inPortfolioID;
            dg_Journal.Rows[myRow].Cells["j_AllowEdit"].Value = "N";

            myRow = dg_Journal.Rows.Add();
            dg_Journal.Rows[myRow].Cells["j_AccountID"].Value = inAccountID;
            dg_Journal.Rows[myRow].Cells["j_FundID"].Value = inFundID;
            dg_Journal.Rows[myRow].Cells["j_Description"].Value = inDescription;
            dg_Journal.Rows[myRow].Cells["j_TranType"].Value = inTranType;
            dg_Journal.Rows[myRow].Cells["j_Amount"].Value = inAmount;
            dg_Journal.Rows[myRow].Cells["j_PortfolioID"].Value = inPortfolioID;
            dg_Journal.Rows[myRow].Cells["j_AllowEdit"].Value = "Y";

            SystemLibrary.SetColumn(dg_Journal, "j_Amount");


        } //FromParent()

        public void FromParent(Form inForm, String inAction, int inTranID, String inTranType, DateTime inEffectiveDate, int inAccountID, String inAccountType, String inDescription, Double inAmount, String incrncy, int inParentFundID, int inFundID, String inPortfolioID)
        {
            // Local Variables
            String mySql;

            ParentForm1 = inForm;
            PortfolioID = inPortfolioID;
            HavePassedInData = true; // Needed so that form knows if it needs to load other data

            if (inAction == "Reverse Accrual")
            {
                lb_TranType.Text = inTranType;
                rb_ReverseAccrual.Checked = true;
            }
            else if (inAction.StartsWith("Between Accounts"))
            {
                lb_TranType.Text = inTranType;
                rb_BetweenAccounts.Checked = true;
            }
            else
            {
                // "General Journal" o
                lb_TranType.Text = inTranType;
                rb_GeneralJournal.Checked = true;
            }

            TranID = inTranID.ToString();
            FundID = inFundID.ToString();
            ParentFundID = inParentFundID.ToString();
            AccountID = inAccountID.ToString();
            dtp_EffectiveDate.Value = inEffectiveDate;
            dtp_EffectiveDate.Enabled = false;
            cb_crncy.Text = incrncy;
            cb_crncy.Enabled = false;

            dt_Fund = SystemLibrary.SQLSelectToDataTable("Select * From Fund Where (ParentFundID = " + ParentFundID + " And AllowTrade = 'Y') Or FundID = " + FundID);
            DataGridViewComboBoxColumn dcb_FundID = (DataGridViewComboBoxColumn)dg_Journal.Columns["j_FundID"];
            dcb_FundID.DataSource = dt_Fund;
            dcb_FundID.DisplayMember = "FundName";
            dcb_FundID.ValueMember = "FundID";

            // Cannot Change the Account
            if (inAccountType.ToUpper() == "CASH" && !rb_BetweenAccounts.Checked)
                mySql = "Select * From Accounts Where AccountID = " + AccountID;
            else
                mySql = "Select * From Accounts Where AccountType = '" + inAccountType + "' and FundID in (Select FundID from Fund Where ParentFundID = " + inParentFundID + ")";
            dt_Accounts = SystemLibrary.SQLSelectToDataTable(mySql);
            DataGridViewComboBoxColumn dcb_AccountID = (DataGridViewComboBoxColumn)dg_Journal.Columns["j_AccountID"];
            dcb_AccountID.DataSource = dt_Accounts;
            dcb_AccountID.DisplayMember = "AccountName";
            dcb_AccountID.ValueMember = "AccountID";

            dt_TranType = SystemLibrary.SQLSelectToDataTable("Select * From TranType Where TranType = '" + inTranType + "' ");
            DataGridViewComboBoxColumn dcb_TranType = (DataGridViewComboBoxColumn)dg_Journal.Columns["j_TranType"];
            dcb_TranType.DataSource = dt_TranType;
            dcb_TranType.DisplayMember = "TranType";
            dcb_TranType.ValueMember = "TranType";

            if (inAccountType.ToUpper() != "ACCRUAL")
                rb_ReverseAccrual.Enabled = false;

            dg_Journal.Columns["j_TranType"].ReadOnly = true;

            // Add the journal Entries that are needed
            lb_Imbalance.Text = "";
            int myRow = dg_Journal.Rows.Add();
            dg_Journal.Rows[myRow].Cells["j_AccountID"].Value = inAccountID;
            dg_Journal.Rows[myRow].Cells["j_FundID"].Value = inFundID;
            dg_Journal.Rows[myRow].Cells["j_Description"].Value = inDescription;
            dg_Journal.Rows[myRow].Cells["j_TranType"].Value = inTranType;
            dg_Journal.Rows[myRow].Cells["j_Amount"].Value = -inAmount;
            dg_Journal.Rows[myRow].Cells["j_PortfolioID"].Value = inPortfolioID;
            dg_Journal.Rows[myRow].Cells["j_AllowEdit"].Value = "N";
            if (inAction == "Between Accounts - Other Half")
            {
                // This side is an existing transaction
                dg_Journal.Rows[myRow].Cells["j_TranID"].Value = inTranID;
                dg_Journal.Rows[myRow].Cells["j_Amount"].Value = inAmount;
            }
            if (rb_ReverseAccrual.Checked)
            {
                dg_Journal.AllowUserToAddRows = false;
                dg_Journal.AllowUserToDeleteRows = false;
            }
            else
            {
                myRow = dg_Journal.Rows.Add();
                dg_Journal.Rows[myRow].Cells["j_AccountID"].Value = inAccountID;
                dg_Journal.Rows[myRow].Cells["j_FundID"].Value = inFundID;
                dg_Journal.Rows[myRow].Cells["j_Description"].Value = inDescription;
                dg_Journal.Rows[myRow].Cells["j_TranType"].Value = inTranType;
                dg_Journal.Rows[myRow].Cells["j_Amount"].Value = inAmount;
                dg_Journal.Rows[myRow].Cells["j_PortfolioID"].Value = inPortfolioID;
                dg_Journal.Rows[myRow].Cells["j_AllowEdit"].Value = "Y";
                if (inAction == "Between Accounts - Other Half")
                {
                    // Oposite side to the existing transaction
                    dg_Journal.Rows[myRow].Cells["j_Amount"].Value = -inAmount;
                }

            }
            SystemLibrary.SetColumn(dg_Journal, "j_Amount");


        } // FromParent()

        private void ProcessJournals_Shown(object sender, EventArgs e)
        {
            Console.WriteLine("ProcessJournals_Shown");

            // See if this was passed in without existing data.
            if (!HavePassedInData)
            {
                // Load data here
                NewRecordLoadData();
            }
            
            SystemLibrary.PositionFormOverParent(this, ParentForm1);
        } //ProcessJournals_Shown()

        private void dg_Journal_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            LastValue = dg_Journal[e.ColumnIndex, e.RowIndex].Value;
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                e.Cancel = true;
            else if (SystemLibrary.ToString(dg_Journal["j_AllowEdit", e.RowIndex].Value) == "N")
                e.Cancel = true;
            if (rb_ReverseAccrual.Checked)
                e.Cancel = true;

        } //dg_Journal_CellBeginEdit()

        private void dg_Journal_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Local Variables

            switch (dg_Journal.Columns[e.ColumnIndex].Name)
            {
                case "j_Amount":
                    Double myValue = SystemLibrary.ToDouble(dg_Journal[e.ColumnIndex, e.RowIndex].Value);
                    dg_Journal[e.ColumnIndex, e.RowIndex].Value = myValue;
                    break;
                case "j_AccountID":
                    // Make sure the FundID matches this account if it is an Accrual Account
                    // - Cash Accounts belong to all funds within a parent fund
                    String myAccountID = SystemLibrary.ToString(dg_Journal[e.ColumnIndex, e.RowIndex].Value);
                    DataRow[] dr = dt_Accounts.Select("AccountType = 'ACCRUAL' and AccountID=" + myAccountID);
                    if (dr.Length > 0)
                    {
                        Int32 myFundID = SystemLibrary.ToInt32(dr[0]["FundID"]);
                        dg_Journal["j_FundID", e.RowIndex].Value = myFundID;
                    }
                    break;
                case "j_FundID":
                    // Make sure the FundID matches this account if it is an Accrual Account
                    // - Cash Accounts belong to all funds within a parent fund
                    String myFundID1 = SystemLibrary.ToString(dg_Journal[e.ColumnIndex, e.RowIndex].Value);
                    DataRow[] dr1 = dt_Accounts.Select("AccountType = 'ACCRUAL' and FundID=" + myFundID1);
                    if (dr1.Length > 0)
                    {
                        Int32 myAccountID1 = SystemLibrary.ToInt32(dr1[0]["AccountID"]);
                        dg_Journal["j_AccountID", e.RowIndex].Value = myAccountID1;
                    }
                    break;
                default:
                    break;
            }

            CalcBalance();
  
        } //dg_Journal_CellEndEdit()

        private void dg_Journal_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            // Local Variables
            DataGridViewRow dgr = e.Row;
            int FocusRow = -1; 

            // Find a locked row or the first row.
            for (int i = 0; i < dg_Journal.Rows.Count-1; i++)
            {
                if (SystemLibrary.ToString(dg_Journal["j_AllowEdit", i].Value.ToString()) == "N")
                {
                    FocusRow = i;
                    break;
                }
                else if (FocusRow == -1)
                {
                    FocusRow = i;
                }
            }

            // Set the base columns
            // unfortunately microsoft give me the wrong row, so loop around rows to fix
            for (int i = 0; i < dg_Journal.Rows.Count - 1; i++)
            {
                if (SystemLibrary.ToString(dg_Journal["j_AllowEdit", i].Value) == "")
                {
                    dg_Journal["j_AllowEdit", i].Value = "Y";
                    if (FocusRow > -1)
                    {

                        dg_Journal["j_AccountID", i].Value = dg_Journal["j_AccountID", FocusRow].Value;
                        dg_Journal["j_FundID", i].Value = dg_Journal["j_FundID", FocusRow].Value;
                        dg_Journal["j_Description", i].Value = dg_Journal["j_Description", FocusRow].Value;
                        dg_Journal["j_TranType", i].Value = dg_Journal["j_TranType", FocusRow].Value;
                        dg_Journal["j_PortfolioID", i].Value = dg_Journal["j_PortfolioID", FocusRow].Value;
                        dg_Journal["j_Amount", i].Value = 0;
                    }
                }
            }
            CalcBalance();

        } //dg_Journal_UserAddedRow()

        private void dg_Journal_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DataGridViewRow dgr = e.Row;
            if (SystemLibrary.ToString(dgr.Cells["j_AllowEdit"].Value) == "N")
            {
                MessageBox.Show("Cannot Delete this Row.\r\n\r\nIf you no longer want to do this Journal, then press [New] or close the window");
                e.Cancel = true;
            }
            CalcBalance();

        } //dg_Journal_CellEndEdit()

        private void CalcBalance()
        {
            Double myBalance = 0;

            SystemLibrary.SetColumn(dg_Journal, "j_Amount");

            for (int i = 0; i < dg_Journal.Rows.Count-1; i++)
            {
                myBalance = myBalance + SystemLibrary.ToDouble(dg_Journal["j_Amount", i].Value);
            }

            if (myBalance == 0)
            {
                lb_Imbalance.Text = "";
                bt_Save.Enabled = true;
            }
            else
            {
                lb_Imbalance.Text = "Imbalance = " + myBalance.ToString("$#,###.00");
                bt_Save.Enabled = false;
            }
        } //CalcBalance()

        private void bt_Calculator_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc");
        } //bt_Calculator_Click()

        private void bt_Save_Click(object sender, EventArgs e)
        {
            // Local Variables
            String LastAccountID = "";
            String LastFundID = "";
            String LastPortfolioID = "";
            Boolean isDifferent = false;
            String mySql;

            if (lb_Imbalance.Text.Length > 0)
            {
                MessageBox.Show("Cannot Save due to '" + lb_Imbalance.Text + "'", bt_Save.Text);
                return;
            }

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            Int32 NextJournalID = SystemLibrary.SQLSelectInt32("exec sp_GetNextId 'JournalID'");

            // See if user doing a journal into the same account
            for (int i = 0; i < dg_Journal.Rows.Count - 1; i++)
            {
                if (LastAccountID == "")
                {
                    LastAccountID = SystemLibrary.ToString(dg_Journal.Rows[i].Cells["j_AccountID"].Value);
                    LastFundID = SystemLibrary.ToString(dg_Journal.Rows[i].Cells["j_FundID"].Value);
                    LastPortfolioID = SystemLibrary.ToString(dg_Journal.Rows[i].Cells["j_PortfolioID"].Value);
                }
                else if (LastAccountID != SystemLibrary.ToString(dg_Journal.Rows[i].Cells["j_AccountID"].Value) ||
                         LastFundID != SystemLibrary.ToString(dg_Journal.Rows[i].Cells["j_FundID"].Value) ||
                         LastPortfolioID != SystemLibrary.ToString(dg_Journal.Rows[i].Cells["j_PortfolioID"].Value)
                        )
                {
                    isDifferent = true;
                    break;
                }
            }

            if (!isDifferent)
            {
                if (MessageBox.Show("You do not appear to be changing the Account/Fund/Portfolio combination between these entries.\r\n\r\n" +
                                   "Are you sure you wish to contine?", "Save Journal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }

            for (int i = 0; i < dg_Journal.Rows.Count - 1; i++)
            {
                // Only insert if no exitsing TranID
                if (SystemLibrary.ToString(dg_Journal.Rows[i].Cells["j_TranID"].Value).Length == 0)
                {
                    Int32 NextTranID = SystemLibrary.SQLSelectInt32("exec sp_GetNextId 'TranID'");
                    String strPortfolioID = dg_Journal.Rows[i].Cells["j_PortfolioID"].Value.ToString();
                    if (strPortfolioID.Length == 0)
                        strPortfolioID = "null";
                    mySql = "Insert into Transactions (TranID, AccountID, FundID, PortfolioID, crncy, EffectiveDate, RecordDate, Amount, Reconcilled, " +
                            "Description, TranType, JournalID) " +
                            "Values (" + NextTranID.ToString() + "," + dg_Journal.Rows[i].Cells["j_AccountID"].Value.ToString() + "," + dg_Journal.Rows[i].Cells["j_FundID"].Value.ToString() + "," + strPortfolioID + ",'" +
                            cb_crncy.Text + "','" + dtp_EffectiveDate.Value.ToString("dd-MMM-yyy") + "','" + dtp_EffectiveDate.Value.ToString("dd-MMM-yyyy") + "'," +
                            dg_Journal.Rows[i].Cells["j_Amount"].Value.ToString() + ",'Y','" + dg_Journal.Rows[i].Cells["j_Description"].Value.ToString() + "','" + dg_Journal.Rows[i].Cells["j_TranType"].Value.ToString() + "'," + NextJournalID.ToString() + ")";
                    SystemLibrary.SQLExecute(mySql);
                }
                else
                {
                    // Make the inbound Transaction as Reconcilled and add JournalID
                    mySql = "Update Transactions Set Reconcilled = 'Y', JournalID = " + NextJournalID.ToString() + " Where TranID = " + SystemLibrary.ToString(dg_Journal.Rows[i].Cells["j_TranID"].Value);
                    SystemLibrary.SQLExecute(mySql);
                }
            }

            if (rb_BetweenAccounts.Checked && dg_Journal.Rows.Count > 1)
            {
                // Also mark the inbound Transaction as Reconcilled
                mySql = "Update Transactions Set Reconcilled = 'Y' Where TranID = " + TranID;
                SystemLibrary.SQLExecute(mySql);
            }


            SystemLibrary.SQLExecute("Exec sp_Cal_FundNav_RebuildFrom '"+dtp_EffectiveDate.Value.ToString("dd-MMM-yyy")+"'");
            SystemLibrary.SQLExecute("Exec sp_Profit_Portfolio");

            dg_Journal.ReadOnly = true;
            dg_Journal.Enabled = false;
            bt_Save.Enabled = false;
            Cursor.Current = Cursors.Default;

            if (ParentForm1 != null)
            {
                if (ParentForm1.GetType().Name == "ProcessUnallocatedTransactions")
                {
                    ((ProcessUnallocatedTransactions)ParentForm1).RefreshData();
                }
            }

            MessageBox.Show("Saved");
        } //bt_Save_Click() 

        private void NewRecordLoadData()
        {


        } //NewRecordLoadData()
    }
}
