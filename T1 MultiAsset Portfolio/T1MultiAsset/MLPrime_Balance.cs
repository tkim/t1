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
    public partial class MLPrime_Balance : Form
    {
        // Global Variables
        public DataTable dt_MLPrime_Accounts;
        public DataTable dt_Balances;
        public DataTable dt_Transactions;
        public Form1 ParentForm1;
        public int FundID;
        public int AccountID;
        public Boolean inStartup = false;
        Boolean NeedFullUpdate = false;
        Boolean myReconcilled = true;
        Decimal myDiff;
        private int CXLocation = 0;
        private int CYLocation = 0;


        public MLPrime_Balance()
        {
            InitializeComponent();
            dtp_FromDate.Value = SystemLibrary.f_Now().AddDays(-7); //.AddMonths(-3);
        }

        private void MLPrime_Balance_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            LoadAccounts();
            LoadRec();
        } // MLPrime_Balance_Load()

        public void FromParent(Form inForm, int inFund)
        {
            ParentForm1 = (Form1)inForm;
            FundID = inFund;
            //ParentForm1.CopyDataTable(ref dt_Fund, ref dt_Portfolio);

        } //FromParent()

        public void LoadAccounts()
        {
            // Local Variables
            String mySql;

            mySql = "select	'Merrill Lynch' as CustodianName, MLPrime_Accounts.FundID, MLPrime_Accounts.AccountID, Fund.FundName, Fund.ShortName, " +
                    "		MLPrime_Accounts.ExtID, MLPrime_Accounts.AccountType, MLPrime_Accounts.AccountName " +
                    "From	MLPrime_Accounts, " +
                    "		Fund " +
                    "Where	Fund.FundID = MLPrime_Accounts.FundID " +
                    "And	Fund.Active = 'Y' " +
                    "Union " +
                    "select	'SCOTIA' as CustodianName, SCOTIAPrime_Accounts.FundID, SCOTIAPrime_Accounts.AccountID, Fund.FundName, Fund.ShortName, " +
                    "		SCOTIAPrime_Accounts.ExtID, SCOTIAPrime_Accounts.AccountType, SCOTIAPrime_Accounts.AccountName " +
                    "From	SCOTIAPrime_Accounts, " +
                    "		Fund " +
                    "Where	Fund.FundID = SCOTIAPrime_Accounts.FundID " +
                    "And		Fund.Active = 'Y' " +
                    "Order by Fund.FundName ";

            dt_MLPrime_Accounts = SystemLibrary.SQLSelectToDataTable(mySql);
            if (dt_MLPrime_Accounts.Rows.Count > 0)
            {
                cb_Accounts.DataSource = dt_MLPrime_Accounts;
                cb_Accounts.DisplayMember = "FundName";
                cb_Accounts.ValueMember = "FundID";
                cb_Accounts.SelectedIndex = 0;

                // Select the Account that matches the passed in Fund
                DataRow[] dr_Find = dt_MLPrime_Accounts.Select("FundId=" + FundID.ToString());
                if (dr_Find.Length > 0)
                {
                    cb_Accounts.SelectedValue = FundID;
                }
                else
                {
                    FundID = SystemLibrary.ToInt32(cb_Accounts.SelectedValue);
                }

            }
        } //LoadAccounts()


        public void LoadRec()
        {
            // Local Variables
            String mySql;
            String UnReconcilled;
            String Currency = "null";
            String myFundID;
            String myDate = dtp_FromDate.Value.ToString("dd-MMM-yyyy");

            inStartup = true;

            if (FundID == -1)
                return;
            else
                myFundID = FundID.ToString();

            UnReconcilled = SystemLibrary.Bool_To_YN(cb_UnReconcilled.Checked);

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            // Exec sp_Reconcilliation_MLPrime @UnReconcilled, @FundID, @crncy, @RequireOutput
            mySql = "Exec dbo.sp_Reconcilliation_Prime '" + UnReconcilled + "', " + myFundID + ", " + Currency + ", 'Y', 'N', null, '" + myDate + "' ";
            dt_Balances = SystemLibrary.SQLSelectToDataTable(mySql);
            dgv_Balances.DataSource = dt_Balances;

            dgv_Balances.Columns["CustodianName"].Visible = false;
            dgv_Balances.Columns["FundID"].Visible = false;
            //dgv_Balances.Columns["AccountID"].Visible = false;
            dgv_Balances.Columns["FundName"].Visible = false;
            dgv_Balances.Columns["ShortName"].Visible = false;

            // Make columns read-only
            for (int i = 0; i < dgv_Balances.Columns.Count; i++)
                dgv_Balances.Columns[i].ReadOnly = true;

            dgv_Balances.Columns["Balance"].HeaderText = "Prime Balance";

            // Change Reconcilled to a check box
            dgv_Balances.Columns.Remove("Reconcilled");
            DataGridViewCheckBoxColumn Reconcilled = new DataGridViewCheckBoxColumn();
            Reconcilled.HeaderText = "Reconcilled";
            Reconcilled.FalseValue = "N";
            Reconcilled.TrueValue = "Y";
            Reconcilled.Name = "Reconcilled";
            Reconcilled.DataPropertyName = "Reconcilled";
            dgv_Balances.Columns.Add(Reconcilled);
            dgv_Balances.Columns["Reconcilled"].ReadOnly = false;

            SetLayout();
            inStartup = false;
            dg_Transactions.DataSource = null;
            //dg_Transactions.Rows.Clear();

            if (dt_Balances.Rows.Count > 0)
            {
                // Cause dg_Transactions to load if a row is unrecociled
                for (int i = 0; i < dt_Balances.Rows.Count; i++)
                {
                    if (!SystemLibrary.YN_To_Bool(SystemLibrary.ToString(dt_Balances.Rows[i]["Reconcilled"])))
                    {
                        dgv_Balances_RowEnter(null, new DataGridViewCellEventArgs(0, i));
                        break;
                    }
                }
            }

            Cursor.Current = Cursors.Default;

        } //LoadRec()

        private void SetLayout()
        {
            ParentForm1.SetFormatColumn(dgv_Balances, "Balance", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dgv_Balances, "Amount", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dgv_Balances, "Diff", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dgv_Balances, "EffectiveDate", Color.DarkBlue, Color.Empty, "dd-MMM-yy", "");
            ParentForm1.SetFormatColumn(dgv_Balances, "Reconcilled", Color.Empty, Color.LightGreen, "N0", "");

            // Loop on all columns and set the Autosize mode & Header Text
            for (int i = 0; i < dgv_Balances.Columns.Count; i++)
            {
                dgv_Balances.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgv_Balances.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            dgv_Balances.Columns["AccountName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv_Balances.Columns["EffectiveDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_Balances.Columns["Reconcilled"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_Balances.Columns["crncy"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_Balances.Columns["crncy"].Width = 40;


            // Loop over the Tickers 
            for (Int32 i = 0; i < dgv_Balances.Rows.Count; i++) // Last row in dg_Port is a blank row
            {
                if (dgv_Balances["Reconcilled", i].Value != null)
                {
                    ParentForm1.SetColumn(dgv_Balances, "Balance", i);
                    ParentForm1.SetColumn(dgv_Balances, "Amount", i);
                    ParentForm1.SetColumn(dgv_Balances, "Diff", i);
                    if (dgv_Balances["Reconcilled", i].Value.ToString() == "Y")
                        dgv_Balances["Reconcilled", i].Style.ForeColor = Color.Green;
                    else
                        dgv_Balances["Reconcilled", i].Style.ForeColor = Color.Red;

                }
            }

        } //SetLayout()

        private void cb_Accounts_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FundID = Convert.ToInt16(((DataRowView)(cb_Accounts.SelectedItem)).Row.ItemArray[1].ToString());
            //LoadRec();
        } //cb_Accounts_SelectionChangeCommitted()

        private void bt_Save_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql = "";


            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;


            // Saves any Edit changes down to dt_ForCustodians
            dgv_Balances.Refresh();

            // Loop around rows and see which ones have altered.
            for (int i = 0; i < dt_Balances.Rows.Count; i++)
            {
                // Save changed records back to the database.
                if (dt_Balances.Rows[i].RowState == DataRowState.Modified)
                {
                    String CustodianName = SystemLibrary.ToString(dt_Balances.Rows[i]["CustodianName"]);
                    String myFundID = SystemLibrary.ToString(dt_Balances.Rows[i]["FundID"]);
                    String myAccountID = SystemLibrary.ToString(dt_Balances.Rows[i]["AccountID"]);
                    String myCurrency = SystemLibrary.ToString(dt_Balances.Rows[i]["crncy"]);
                    String myEffectiveDate = Convert.ToDateTime(dt_Balances.Rows[i]["EffectiveDate"]).ToString(dgv_Balances.Columns["EffectiveDate"].DefaultCellStyle.Format);
                    String Reconcilled = SystemLibrary.ToString(dt_Balances.Rows[i]["Reconcilled"]);

                    // Dont process if any of these is null
                    if (myFundID.Length == 0 ||
                        myAccountID.Length == 0 ||
                        myCurrency.Length == 0 ||
                        myEffectiveDate.Length == 0)
                    {
                        continue;
                    }

                    if (Reconcilled == "Y")
                    {
                        switch (CustodianName)
                        {
                            case "Merrill Lynch":
                                mySql = "Update ML_E236 " +
                                        "Set Reconcilled = 'Y' " +
                                        "Where  FundID = " + myFundID + " " +
                                        "And    AccountID = " + myAccountID + " " +
                                        "And    Currency_code = '" + myCurrency + "' " +
                                        "And    balance_Date = Report_Date " +
                                        "And    Report_Date = '" + myEffectiveDate + "' ";
                                break;
                            case "SCOTIA":
                                mySql = "Update SCOTIA_CASHBALANCE " +
                                        "Set Reconcilled = 'Y' " +
                                        "Where  FundID = " + myFundID + " " +
                                        "And    AccountID = " + myAccountID + " " +
                                        "And    Ccy = '" + myCurrency + "' " +
                                        "And    balance_Date = '" + myEffectiveDate + "' ";
                                break;
                        }
                        if (mySql.Length>0)
                            SystemLibrary.SQLExecute(mySql);
                    }
                }
            }

            // Then re-run full run, so it picks up ALL data
            LoadRec();

            Cursor.Current = Cursors.Default;
            NeedFullUpdate = true;
            MessageBox.Show("Changes Saved", "Save Reconcilled");

        } //bt_Save_Click()

        private void dgv_Balances_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            // Local Variables
            String mySql;

            if (e.RowIndex < 0 || inStartup)
                return;

            String myFundID = SystemLibrary.ToString(dt_Balances.Rows[e.RowIndex]["FundID"]);
            String myAccountID = SystemLibrary.ToString(dt_Balances.Rows[e.RowIndex]["AccountID"]);
            String myCurrency = SystemLibrary.ToString(dt_Balances.Rows[e.RowIndex]["crncy"]);
            String myEffectiveDate = Convert.ToDateTime(dt_Balances.Rows[e.RowIndex]["EffectiveDate"]).ToString(dgv_Balances.Columns["EffectiveDate"].DefaultCellStyle.Format);
            myReconcilled = SystemLibrary.YN_To_Bool(SystemLibrary.ToString(dt_Balances.Rows[e.RowIndex]["Reconcilled"]));
            myDiff = SystemLibrary.ToDecimal(dt_Balances.Rows[e.RowIndex]["Diff"]);

            // Dont process if any of these is null
            if (myFundID.Length == 0 ||
                myAccountID.Length == 0 ||
                myCurrency.Length == 0 ||
                myEffectiveDate.Length == 0)
            {
                return;
            }

            mySql = "SELECT     tn.TranID, tn.TradeID, tn.FundID, tn.PortfolioID, f.ShortName, tn.crncy, tn.EffectiveDate, tn.RecordDate, tn.Amount,  " + 
                    "					CASE  " + 
                    "					WHEN tn.TradeID IS Not NULL THEN t .Side + '   ' + CAST(CAST(Abs(t .Quantity) AS int) AS Varchar) + '   ' + t .BBG_Ticker   " +
                    "					WHEN tn.CapitalID IS Not NULL THEN 'Capital - ' + fc.Description   " +
                    "					ELSE tn.Description END AS Description, isNull(TranType,'') as TranType, isNull(Reconcilled,'N') as Reconcilled " + 
                    "FROM         Transactions AS tn LEFT OUTER JOIN " + 
                    "                      Fund_Capital fc ON tn.CapitalID = fc.CapitalID LEFT OUTER JOIN " +
                    "                      Trade AS t ON tn.TradeID = t.TradeID LEFT OUTER JOIN " +
                    "                      Fund AS f ON tn.FundID = f.FundID " +
                    "Where  tn.FundID in (Select FundID from Fund Where ParentFundID =  " + myFundID + ") " +
                    "And    tn.AccountID = " + myAccountID + " " +
                    "And    tn.crncy = '" + myCurrency + "' " +
                    "And    tn.EffectiveDate >= '" + myEffectiveDate + "' " +
                    "ORDER BY tn.EffectiveDate, isNull(Reconcilled,'N'), tn.FundID, tn.PortfolioID, tn.crncy, tn.TranType, tn.TradeID ";

            dt_Transactions = SystemLibrary.SQLSelectToDataTable(mySql);

            dg_Transactions.DataSource = dt_Transactions;
            dg_Transactions.Columns["FundID"].Visible = false;
            dg_Transactions.Columns["TranID"].Visible = false;
            dg_Transactions.Columns["TradeID"].Visible = false;
            dg_Transactions.Columns["PortfolioID"].Visible = false;
            dg_Transactions.Columns["crncy"].Visible = false;
            dg_Transactions.Columns["TranType"].Visible = false;

            SetLayoutTrans();
        }

        private void SetLayoutTrans()
        {
            ParentForm1.SetFormatColumn(dg_Transactions, "Amount", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dg_Transactions, "EffectiveDate", Color.DarkBlue, Color.Empty, "dd-MMM-yy", "");
            ParentForm1.SetFormatColumn(dg_Transactions, "RecordDate", Color.Empty, Color.Empty, "dd-MMM-yy", "");

            // Loop on all columns and set the Autosize mode & Header Text
            for (int i = 0; i < dg_Transactions.Columns.Count; i++)
                dg_Transactions.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Loop on all columns and set the Autosize mode & Header Text
            dg_Transactions.Columns["EffectiveDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dg_Transactions.Columns["RecordDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dg_Transactions.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dg_Transactions.Columns["Description"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg_Transactions.Columns["Reconcilled"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            // Loop over the Transactions 
            for (Int32 i = 0; i < dg_Transactions.Rows.Count; i++) // Last row in dg_Port is a blank row
            {
                if (dg_Transactions["EffectiveDate", i].Value != null)
                {
                    ParentForm1.SetColumn(dg_Transactions, "Amount", i);
                }
                if (dg_Transactions["Reconcilled", i].Value.ToString() == "Y")
                    dg_Transactions["Reconcilled", i].Style.ForeColor = Color.Green;
                else
                {
                    dg_Transactions["Reconcilled", i].Style.ForeColor = Color.Red;
                    // See if the current record matches the differences
                    if (!myReconcilled)
                    {
                        if (SystemLibrary.ToDecimal(dg_Transactions["Amount", i].Value) == -myDiff)
                            dg_Transactions.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
                    }
                }

            }

        } //SetLayoutTrans()


        private void dgv_Balances_Sorted(object sender, EventArgs e)
        {
            SetLayout();
        } //dgv_Balances_Sorted()

        private void dg_Transactions_Sorted(object sender, EventArgs e)
        {
            SetLayoutTrans();
        } //dg_Transactions_Sorted()

        private void bt_Request_Click(object sender, EventArgs e)
        {
            LoadRec();
        } //bt_Request_Click()

        private void MLPrime_Balance_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (NeedFullUpdate)
                SystemLibrary.SQLExecute("Exec sp_actionsNeeded 2, 'N'");

            Cursor.Current = Cursors.Default;

        } //MLPrime_Balance_FormClosing()

        private void dg_Transactions_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Explain the transactions
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    Point myLocation = new Point(this.Location.X + CXLocation, this.Location.Y + CYLocation);
                    Boolean Reconcilled = SystemLibrary.YN_To_Bool(SystemLibrary.ToString(dg_Transactions.Rows[e.RowIndex].Cells["Reconcilled"].Value));
                    String TranType = SystemLibrary.ToString(dg_Transactions.Rows[e.RowIndex].Cells["TranType"].Value);

                    if (Reconcilled == false && TranType == "Dividend")
                    {
                        SystemLibrary.iMessageBox("This is an Unreconcilled Dividend.\r\n\r\n" +
                                                  "That means either the Custodian/Prime Broker has a different value or no value.\r\n" +
                                                  "First clear any Dividend messages in the Action tab.\r\n" +
                                                  "Next see if you have an incorrect Settlement Date.\r\n" +
                                                  "Lastly you can either check with the Custodian/Prime Broker or wait to see if this clears overnight.\r\n",
                                                  "Dividend", MessageBoxIcon.None, false);
                    }
                }
            }
            catch{}
        }

        private void dg_Transactions_MouseClick(object sender, MouseEventArgs e)
        {
            CXLocation = dg_Transactions.Location.X + e.Location.X + 5;
            CYLocation = dg_Transactions.Location.Y + e.Location.Y;
        }

    }

}
