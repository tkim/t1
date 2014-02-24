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
    public partial class DefineInterestAccrual : Form
    {
        // Public Variables
        public Form1 ParentForm1;
        //DataTable dt_Transactions;
        //DataTable dt_TranType;
        DataTable dt_Fund;
        DataTable dt_Portfolio;
        DataTable dt_Accounts;
        DataTable dt_Crncy;
        DataTable dt_AccountInterest;
        public Object LastValue;
        int myFundID;
        int myAccountID = int.MinValue;
        String myCrncy;
        Boolean isStartUp = false;

        // Store the last sucessful Request
        int ReqAccountID = int.MinValue;
        String ReqCrncy;


        public DefineInterestAccrual()
        {
            InitializeComponent();
        }

        private void DefineInterestAccrual_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            dateTimePicker1.Value = SystemLibrary.f_Now();
            isStartUp = true;
            LoadFunds();
            LoadAccounts();
            LoadCrncy();
            bt_Request_Click(null, null);
            isStartUp = false;

        } //DefineInterestAccrual_Load()

        public void FromParent(Form inForm, int inFundID, String inCrncy)
        {
            ParentForm1 = (Form1)inForm;
            ParentForm1.CopyDataTable(ref dt_Fund, ref dt_Portfolio);
            myFundID = inFundID;
            myCrncy = inCrncy;
        } //FromParent()

        public void LoadFunds()
        {
            cb_Fund.DataSource = dt_Fund;
            cb_Fund.DisplayMember = "FundName";
            cb_Fund.ValueMember = "FundId";

            // Select the Fund that matches the passed in Fund
            DataRow[] dr_Find = dt_Fund.Select("FundID=" + myFundID.ToString());
            if (dr_Find.Length > 0)
            {
                cb_Fund.SelectedValue = myFundID;
            }
            else
            {
                cb_Fund.SelectedIndex = 0;
                myFundID = Convert.ToInt16(((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[0].ToString());
            }



        } //LoadFunds()

        public void LoadAccounts()
        {
            // Local Variables
            String mySql;

            mySql = "Select a.AccountID, a.AccountName, a.ExtID " +
                    "From   Accounts a, " +
                    "       Fund f " +
                    "Where  a.AccountType = 'CASH' " +
                    "And    f.FundID = " + myFundID + " " +
                    "And    a.FundID = f.ParentFundID " +
                    "Order by a.AccountName ";

            dt_Accounts = SystemLibrary.SQLSelectToDataTable(mySql);
            if (dt_Accounts.Rows.Count > 0)
            {
                cb_Accounts.DataSource = dt_Accounts;
                cb_Accounts.DisplayMember = "AccountName";
                cb_Accounts.ValueMember = "AccountID";
                cb_Accounts.SelectedIndex = 0;

                // Select the Account that matches the passed in Fund
                DataRow[] dr_Find = dt_Accounts.Select("AccountID=" + myAccountID.ToString());
                if (dr_Find.Length > 0)
                {
                    cb_Accounts.SelectedValue = myAccountID;
                }
            }
        } //LoadAccounts()

        private void LoadCrncy()
        {
            // Local Variables
            String mySql;

            mySql = "select	crncy " +
                    "From	Transactions " +
                    "Where	AccountID = " + myAccountID.ToString() + " " +
                    "Group by crncy " +
                    "Order by crncy ";

            dt_Crncy = SystemLibrary.SQLSelectToDataTable(mySql);
            if (dt_Crncy.Rows.Count > 0)
            {
                cb_crncy.DataSource = dt_Crncy;
                cb_crncy.DisplayMember = "crncy";
                cb_crncy.ValueMember = "crncy";
                cb_crncy.SelectedIndex = 0;

                // Select the Account that matches the passed in Fund
                DataRow[] dr_Find = dt_Crncy.Select("crncy='" + myCrncy + "'");
                if (dr_Find.Length > 0)
                {
                    cb_crncy.SelectedValue = myCrncy;
                }
            }
            else
                cb_crncy.Text = myCrncy;

        } //LoadCrncy()
        
        private void bt_Save_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;
            String myDate;
            String myRemoveReconcilled;

            if (dt_AccountInterest == null)
                return;

            if (ReqAccountID == int.MinValue)
                return;

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            // Saves any Edit changes down to dt_ForCustodians
            dg_AccountInterest.Refresh();

            myDate = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
            myRemoveReconcilled = SystemLibrary.Bool_To_YN(cb_RemoveReconcilled.Checked);

            if (cb_RemoveReconcilled.Checked == true)
            {
                if (MessageBox.Show(this, "WARNING:    This will remove ALL reconcilled 'Interest' AccountInterest for this account from '" + myDate + "'.\r\n\r\n" +
                                          "Do you really want to do this?", "Change Tab", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            // Check for any user changes and update the database
            for (int i = 0; i < dt_AccountInterest.Rows.Count; i++)
            {
                // Save changed records back to the database.
                switch (dt_AccountInterest.Rows[i].RowState.ToString())
                {
                    case "Deleted":
                        // Delete the AccountInterest Row for this TranID
                        int DelAccountID = SystemLibrary.ToInt32(dt_AccountInterest.Rows[i]["AccountID", DataRowVersion.Original]);
                        String DelCrncy = SystemLibrary.ToString(dt_AccountInterest.Rows[i]["crncy", DataRowVersion.Original]);
                        String DelFromDate = Convert.ToDateTime(dt_AccountInterest.Rows[i]["FromDate", DataRowVersion.Original]).ToString("dd-MMM-yyyy");
                        mySql = "Delete From AccountInterest " +
                                "Where  AccountID=" + DelAccountID.ToString() + " " +
                                "And    crncy = '" + DelCrncy + "' " +
                                "and    FromDate = '" + DelFromDate + "' ";
                        SystemLibrary.SQLExecute(mySql);
                        break;

                    case "Unchanged":
                        break;

                    case "Added":
                        // On NEW records Need to Set myAccountID, myCrncy
                        dt_AccountInterest.Rows[i]["AccountID"] = ReqAccountID;
                        dt_AccountInterest.Rows[i]["crncy"] = ReqCrncy;
                        String FromDate = "'" + Convert.ToDateTime(dt_AccountInterest.Rows[i]["FromDate"]).ToString("dd-MMM-yyyy") +"'";
                        String ToDate;
                        if (dt_AccountInterest.Rows[i]["ToDate"] == DBNull.Value)
                            ToDate = "null";
                        else
                            ToDate = "'" + Convert.ToDateTime(dt_AccountInterest.Rows[i]["ToDate"]).ToString("dd-MMM-yyyy") +"'";

                        mySql = "Insert into AccountInterest (AccountID, crncy, FromDate, ToDate, BorrowRate, EarnRate, BaseRate, BorrowSpread, EarnSpread) " +
                                "Values (" + ReqAccountID.ToString() + ", '" + ReqCrncy + "', " + FromDate + ", " + ToDate + ", " +
                                         SystemLibrary.ToString(dt_AccountInterest.Rows[i]["BorrowRate"]) + ", " +
                                         SystemLibrary.ToString(dt_AccountInterest.Rows[i]["EarnRate"]) + ", " +
                                         SystemLibrary.ToString(dt_AccountInterest.Rows[i]["BaseRate"]) + ", " +
                                         SystemLibrary.ToString(dt_AccountInterest.Rows[i]["BorrowSpread"]) + ", " +
                                         SystemLibrary.ToString(dt_AccountInterest.Rows[i]["EarnSpread"]) + " " +
                                         ") ";
                        SystemLibrary.SQLExecute(mySql);
                        break;

                    case "Modified":
            			// Original Record data			
                        String origFromDate = "'" + Convert.ToDateTime(dt_AccountInterest.Rows[i]["FromDate", DataRowVersion.Original]).ToString("dd-MMM-yyyy") +"'";
                        String origToDate;
                        if (dt_AccountInterest.Rows[i]["ToDate", DataRowVersion.Original] == DBNull.Value)
                            origToDate = " is null";
                        else
                            origToDate = "= '" + Convert.ToDateTime(dt_AccountInterest.Rows[i]["ToDate", DataRowVersion.Original]).ToString("dd-MMM-yyyy") +"'";
            			String origBorrowRate = SystemLibrary.ToString(dt_AccountInterest.Rows[i]["BorrowRate", DataRowVersion.Original]);
                        String origEarnRate = SystemLibrary.ToString(dt_AccountInterest.Rows[i]["EarnRate", DataRowVersion.Original]);
                        String origBaseRate = SystemLibrary.ToString(dt_AccountInterest.Rows[i]["BaseRate", DataRowVersion.Original]);
                        String origBorrowSpread = SystemLibrary.ToString(dt_AccountInterest.Rows[i]["BorrowSpread", DataRowVersion.Original]);
                        String origEarnSpread = SystemLibrary.ToString(dt_AccountInterest.Rows[i]["EarnSpread", DataRowVersion.Original]);

			            // New Record Data
                        String newFromDate = "'" + Convert.ToDateTime(dt_AccountInterest.Rows[i]["FromDate"]).ToString("dd-MMM-yyyy") +"'";
                        String newToDate;
                        if (dt_AccountInterest.Rows[i]["ToDate"] == DBNull.Value)
                            newToDate = "null";
                        else
                            newToDate = "'" + Convert.ToDateTime(dt_AccountInterest.Rows[i]["ToDate"]).ToString("dd-MMM-yyyy") + "'";
            			String newBorrowRate = SystemLibrary.ToString(dt_AccountInterest.Rows[i]["BorrowRate"]);
                        String newEarnRate = SystemLibrary.ToString(dt_AccountInterest.Rows[i]["EarnRate"]);
                        String newBaseRate = SystemLibrary.ToString(dt_AccountInterest.Rows[i]["BaseRate"]);
                        String newBorrowSpread = SystemLibrary.ToString(dt_AccountInterest.Rows[i]["BorrowSpread"]);
                        String newEarnSpread = SystemLibrary.ToString(dt_AccountInterest.Rows[i]["EarnSpread"]);

                        
                        // Update the Database
                        mySql = "Update AccountInterest " +
                                "Set FromDate = " + newFromDate + ", " +
                                "    ToDate = " + newToDate + ", " +
                                "    BorrowRate = " + newBorrowRate + ", " +
                                "    EarnRate = " + newEarnRate + ", " +
                                "    BaseRate = " + newBaseRate + ", " +
                                "    BorrowSpread = " + newBorrowSpread + ", " +
                                "    EarnSpread = " + newEarnSpread + " " +
                                "Where  AccountID = " + ReqAccountID.ToString() + " " +
                                "And    crncy = '" + ReqCrncy + "' " +
                                "And    FromDate = " + origFromDate + " " +
                                "And    ToDate " + origToDate + " " +
                                "And    BorrowRate = " + origBorrowRate + " " +
                                "And    EarnRate = " + origEarnRate + " " +
                                "And    BaseRate = " + origBaseRate + " " +
                                "And    BorrowSpread = " + origBorrowSpread + " " +
                                "And    EarnSpread = " + origEarnSpread + " ";
                        SystemLibrary.SQLExecute(mySql);
                        break;
                    default:
                        //Console.WriteLine("TranID={0} - RowState={1}", TranID, dt_AccountInterest.Rows[i].RowState);
                        break;
                }
            }


            // Apply the Records
            if (ReqAccountID != int.MinValue)
            {
                mySql = "exec sp_ApplyInterestAccrual " + ReqAccountID.ToString() + ", '" + myDate + "', '" + myRemoveReconcilled + "' ";
                SystemLibrary.SQLExecute(mySql);
            }
            // Refresh
            bt_Request_Click(null, null);
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Interest Accrual Records Saved + Transactions Created.", bt_Save.Text);


        } //bt_Save_Click()

        private void cb_Fund_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isStartUp)
                return;
            myFundID = Convert.ToInt16(((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[0].ToString());
            LoadAccounts();

        }

        private void cb_Account_SelectedIndexChanged(object sender, EventArgs e)
        {
            myAccountID = Convert.ToInt16(((DataRowView)(cb_Accounts.SelectedItem)).Row.ItemArray[0].ToString());
            LoadCrncy();
        }

        private void cb_crncy_SelectedIndexChanged(object sender, EventArgs e)
        {
            myCrncy = SystemLibrary.ToString(((DataRowView)(cb_crncy.SelectedItem)).Row.ItemArray[0].ToString());

        } //cb_crncy_SelectedIndexChanged()

        private void bt_Request_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;

            ReqAccountID = myAccountID;
            ReqCrncy = myCrncy;

            mySql = "select	AccountID, crncy, FromDate, ToDate, BorrowRate, EarnRate, " +
                    "       BaseRate, BorrowSpread, EarnSpread " +
                    "From	AccountInterest " +
                    "Where	AccountID = " + myAccountID.ToString() + " " +
                    "And    crncy = '" + myCrncy + "' " +
                    "Order by FromDate ";

            dt_AccountInterest = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_AccountInterest.DataSource = dt_AccountInterest;

            dg_AccountInterest.Columns["AccountID"].Visible = false;
            dg_AccountInterest.Columns["crncy"].Visible = false;

            ParentForm1.SetFormatColumn(dg_AccountInterest, "FromDate", Color.Empty, Color.Empty, "dd-MMM-yy", "");
            ParentForm1.SetFormatColumn(dg_AccountInterest, "ToDate", Color.Empty, Color.Empty, "dd-MMM-yy", "");
            ParentForm1.SetFormatColumn(dg_AccountInterest, "BorrowRate", Color.Empty, Color.AliceBlue, "N6", "");
            ParentForm1.SetFormatColumn(dg_AccountInterest, "EarnRate", Color.Empty, Color.AliceBlue, "N6", "");
            ParentForm1.SetFormatColumn(dg_AccountInterest, "BaseRate", Color.Empty, Color.Gold, "N6", "");
            ParentForm1.SetFormatColumn(dg_AccountInterest, "BorrowSpread", Color.Empty, Color.LightGreen, "N6", "");
            ParentForm1.SetFormatColumn(dg_AccountInterest, "EarnSpread", Color.Empty, Color.LightGreen, "N6", "");

            for (int i = 0; i < dg_AccountInterest.Columns.Count; i++)
            {
                // Prevent Sort
                dg_AccountInterest.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                // Put a Gap between words in the header
                dg_AccountInterest.Columns[i].HeaderText = dg_AccountInterest.Columns[i].HeaderText.Replace("Date", " Date");
                dg_AccountInterest.Columns[i].HeaderText = dg_AccountInterest.Columns[i].HeaderText.Replace("Rate", " Rate");
                dg_AccountInterest.Columns[i].HeaderText = dg_AccountInterest.Columns[i].HeaderText.Replace("Spread", " Spread");

                dg_AccountInterest.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            for (int i = 0; i < dg_AccountInterest.Rows.Count; i++)
            {
                ParentForm1.SetColumn(dg_AccountInterest, "BorrowSpread", i);
                ParentForm1.SetColumn(dg_AccountInterest, "EarnSpread", i);
            }

            lb_Selected.Text = "Selected: " + cb_Accounts.Text;


            // Scroll to the last Row by selecting a column (using -2, as the last row is a NEW row)
            if (dg_AccountInterest.Rows.Count > 1)
                dg_AccountInterest.CurrentCell = dg_AccountInterest.Rows[dg_AccountInterest.Rows.Count - 2].Cells["BaseRate"];

        } //bt_Request_Click()


        private void dg_AccountInterest_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //if (dg_PortfolioTranspose.Columns[e.ColumnIndex].Name.StartsWith("Quantity_"))
                //e.Cancel = true;

            // Seems the only way I can get the previous value before CellEndEdit().
            LastValue = dg_AccountInterest[e.ColumnIndex, e.RowIndex].Value;

        } //dg_AccountInterest_CellBeginEdit()

        private void dg_AccountInterest_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Local Variables
            DateTime myResult = new DateTime();
            Decimal myBorrowRate;
            Decimal myEarnRate;
            Decimal myBaseRate;
            Decimal myBorrowSpread;
            Decimal myEarnSpread;


            switch (dg_AccountInterest.Columns[e.ColumnIndex].Name)
            {
                case "BorrowRate":
                    myBorrowRate = SystemLibrary.ToDecimal(dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    // To deal with non-numeric, reset BorrowRate
                    dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myBorrowRate;

                    myBaseRate = SystemLibrary.ToDecimal(dg_AccountInterest.Rows[e.RowIndex].Cells["BaseRate"].Value);

                    if (myBaseRate != 0)
                    {
                        myBorrowSpread = myBorrowRate - myBaseRate;
                        dg_AccountInterest.Rows[e.RowIndex].Cells["BorrowSpread"].Value = myBorrowSpread;
                    }
                    break;
                case "EarnRate":
                    myEarnRate = SystemLibrary.ToDecimal(dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    // To deal with non-numeric, reset EarnRate
                    dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myEarnRate;

                    myBaseRate = SystemLibrary.ToDecimal(dg_AccountInterest.Rows[e.RowIndex].Cells["BaseRate"].Value);

                    if (myBaseRate != 0)
                    {
                        myEarnSpread = myEarnRate - myBaseRate;
                        dg_AccountInterest.Rows[e.RowIndex].Cells["EarnSpread"].Value = myEarnSpread;
                    }
                    break;

                case "BorrowSpread":
                    myBorrowSpread = SystemLibrary.ToDecimal(dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    // To deal with non-numeric, reset BorrowSpread
                    dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myBorrowSpread;

                    myBaseRate = SystemLibrary.ToDecimal(dg_AccountInterest.Rows[e.RowIndex].Cells["BaseRate"].Value);

                    if (myBaseRate != 0)
                    {
                        myBorrowRate = myBaseRate + myBorrowSpread;
                        dg_AccountInterest.Rows[e.RowIndex].Cells["BorrowRate"].Value = myBorrowRate;
                    }
                    break;
                case "EarnSpread":
                    myEarnSpread = SystemLibrary.ToDecimal(dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    // To deal with non-numeric, reset EarnSpread
                    dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myEarnSpread;

                    myBaseRate = SystemLibrary.ToDecimal(dg_AccountInterest.Rows[e.RowIndex].Cells["BaseRate"].Value);

                    if (myBaseRate != 0)
                    {
                        myEarnRate = myBaseRate + myEarnSpread;
                        dg_AccountInterest.Rows[e.RowIndex].Cells["EarnRate"].Value = myEarnRate;
                    }
                    break;

                case "BaseRate":
                    myBaseRate = SystemLibrary.ToDecimal(dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    // To deal with non-numeric, reset myBaseRate
                    dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myBaseRate;

                    myBorrowSpread = SystemLibrary.ToDecimal(dg_AccountInterest.Rows[e.RowIndex].Cells["BorrowSpread"].Value);
                    myEarnSpread = SystemLibrary.ToDecimal(dg_AccountInterest.Rows[e.RowIndex].Cells["EarnSpread"].Value);

                    myBorrowRate = myBaseRate + myBorrowSpread;
                    dg_AccountInterest.Rows[e.RowIndex].Cells["BorrowRate"].Value = myBorrowRate;

                    myEarnRate = myBaseRate + myEarnSpread;
                    dg_AccountInterest.Rows[e.RowIndex].Cells["EarnRate"].Value = myEarnRate;
                    break;
                
                case "FromDate":
                    // Validate this is a Date
                    if (!DateTime.TryParse(dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim(), out myResult))
                    {
                        dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else
                        dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult.ToString(dg_AccountInterest.Columns[e.ColumnIndex].DefaultCellStyle.Format);
                    break;
                case "ToDate":
                    // Validate this is a Date or Blank
                    if (!DateTime.TryParse(dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim(), out myResult))
                    {
                        if (dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != DBNull.Value)
                        {
                            if (dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim() == "")
                                dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
                            else
                                dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                        }
                        //else
                         //   dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else
                        dg_AccountInterest.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult.ToString(dg_AccountInterest.Columns[e.ColumnIndex].DefaultCellStyle.Format);
                    break;
            }

            ParentForm1.SetColumn(dg_AccountInterest, "BorrowSpread", e.RowIndex);
            ParentForm1.SetColumn(dg_AccountInterest, "EarnSpread", e.RowIndex);

        } //dg_AccountInterest_CellEndEdit()

    }
}
