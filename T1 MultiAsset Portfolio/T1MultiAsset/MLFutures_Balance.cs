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
    public partial class MLFutures_Balance : Form
    {
        // Global Variables
        public DataTable dt_MLFut_Accounts;
        public DataTable dt_Balances;
        public DataTable dt_Transactions;
        public Form1 ParentForm1;
        public int FundID;
        public int AccountID;
        public Boolean inStartup = false;
        Boolean NeedFullUpdate = false;

        public MLFutures_Balance()
        {
            InitializeComponent();
            dtp_FromDate.Value = SystemLibrary.f_Now().AddDays(-7); // AddMonths(-3);
        }

        private void MLFutures_Balance_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            LoadAccounts();
            LoadRec();
        } // MLFutures_Balance_Load()

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

            mySql = "select	MLFut_Accounts.FundID, MLFut_Accounts.AccountID, Fund.FundName, Fund.ShortName, " +
                    "		MLFut_Accounts.ExtID, MLFut_Accounts.AccountType, MLFut_Accounts.AccountName " +
                    "From	MLFut_Accounts, " +
                    "		Fund " +
                    "Where	Fund.FundID = MLFut_Accounts.FundID " +
                    "And		Fund.Active = 'Y' " +
                    "Order by Fund.FundName ";

            dt_MLFut_Accounts = SystemLibrary.SQLSelectToDataTable(mySql);
            if (dt_MLFut_Accounts.Rows.Count > 0)
            {
                cb_Accounts.DataSource = dt_MLFut_Accounts;
                cb_Accounts.DisplayMember = "FundName";
                cb_Accounts.ValueMember = "FundID";
                cb_Accounts.SelectedIndex = 0;

                // Select the Account that matches the passed in Fund
                DataRow[] dr_Find = dt_MLFut_Accounts.Select("FundId=" + FundID.ToString());
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


            // get Currency
            if (tb_Currency.Text.Length > 0)
            {
                tb_Currency.Text = tb_Currency.Text.ToUpper();
                Currency = "'" + tb_Currency.Text.Trim() + "'";
            }


            UnReconcilled = SystemLibrary.Bool_To_YN(cb_UnReconcilled.Checked);

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            // Exec sp_Reconcilliation_MLFutures @UnReconcilled, @FundID, @crncy, @RequireOutput
            mySql = "Exec sp_Reconcilliation_MLFutures '" + UnReconcilled + "', " + myFundID + ", " + Currency + ", 'Y', 'N', null, '" + myDate + "' ";
            dt_Balances = SystemLibrary.SQLSelectToDataTable(mySql);
            dgv_Balances.DataSource = dt_Balances;

            if (dt_Balances.Rows.Count > 0)
            {
                dgv_Balances.Columns["FundID"].Visible = false;
                //dgv_Balances.Columns["AccountID"].Visible = false;
                dgv_Balances.Columns["FundName"].Visible = false;
                dgv_Balances.Columns["ShortName"].Visible = false;

                // Make columns read-only
                for (int i = 0; i < dgv_Balances.Columns.Count; i++)
                    dgv_Balances.Columns[i].ReadOnly = true;

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
            }
            inStartup = false;
            dg_Transactions.DataSource = null;
            //dg_Transactions.Rows.Clear();

            Cursor.Current = Cursors.Default;

        } //LoadRec()

        private void SetLayout()
        {
            ParentForm1.SetFormatColumn(dgv_Balances, "TotalEquity", Color.Empty, Color.Empty, "N2", "0");
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
                    ParentForm1.SetColumn(dgv_Balances, "TotalEquity", i);
                    ParentForm1.SetColumn(dgv_Balances, "Amount", i);
                    ParentForm1.SetColumn(dgv_Balances, "Diff", i);
                    if (dgv_Balances["Reconcilled", i].Value.ToString() == "Y")
                        dgv_Balances["Reconcilled", i].Style.ForeColor = Color.Green;
                    else
                        dgv_Balances["Reconcilled", i].Style.ForeColor = Color.Red;

                }
            }

        } //SetLayout()

        private void cb_UnReconcilled_CheckedChanged(object sender, EventArgs e)
        {
            //LoadRec();
        } //cb_UnReconcilled_CheckedChanged()

        private void cb_Accounts_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FundID = Convert.ToInt16(((DataRowView)(cb_Accounts.SelectedItem)).Row.ItemArray[0].ToString());
            //LoadRec();
        } //cb_Accounts_SelectionChangeCommitted()

        private void bt_Save_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;
            Boolean FoundChange = false;

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
                    FoundChange = true;

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

                    mySql = "Update MLFut_Money " +
                            "Set Reconcilled = '" + Reconcilled + "' " +
                            "Where  FundID = " + myFundID + " " +
                            "And    AccountID = " + myAccountID + " " +
                            "And    Currency = '" + myCurrency + "' " +
                            "And    RunDate = '" + myEffectiveDate + "' ";
                    //if (Reconcilled=="Y")
                    SystemLibrary.SQLExecute(mySql);
                }
            }

            // Then re-run full run, so it picks up ALL data
            if (FoundChange)
            {
                mySql = "exec sp_MLFut_Process_File '', 'MLFut_Money' ";
                //mySql = "Exec sp_Reconcilliation_MLFutures null, null, null, 'N' ";
                SystemLibrary.SQLExecute(mySql);
            }

            LoadRec();

            Cursor.Current = Cursors.Default;
            NeedFullUpdate = true;
            MessageBox.Show("Changes Saved", "Save Reconcilled");

        } //bt_Save_Click()

        private void dgv_Balances_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

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

            // Dont process if any of these is null
            if (myFundID.Length == 0 ||
                myAccountID.Length == 0 ||
                myCurrency.Length == 0 ||
                myEffectiveDate.Length == 0)
            {
                return;
            }

            mySql = "Select t.TranID, t.TradeID, t.PortfolioID, t.crncy, t.EffectiveDate, t.RecordDate, t.Amount, t.Description, f.FundName " + 
                    "From	Transactions t, " +
                    "       Fund f " +
                    "Where  t.FundID In (Select FundID from Fund Where ParentFundID = " + myFundID + ") " +
                    "And    t.AccountID = " + myAccountID + " " +
                    "And    t.crncy = '" + myCurrency + "' " +
                    "And    t.EffectiveDate >= '" + myEffectiveDate + "' " +
                    "And    f.FundID = t.FundID " +
                    "Order by t.EffectiveDate, f.FundName, t.PortfolioID, t.crncy, t.TranType, t.TradeID, t.TranID ";
            dt_Transactions = SystemLibrary.SQLSelectToDataTable(mySql);

            dg_Transactions.DataSource = dt_Transactions;
            dg_Transactions.Columns["TranID"].Visible = false;
            dg_Transactions.Columns["TradeID"].Visible = false;
            dg_Transactions.Columns["PortfolioID"].Visible = false;
            dg_Transactions.Columns["crncy"].Visible = false;
            dg_Transactions.Columns["FundName"].HeaderText = "Fund Name";

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


            // Loop over the Transactions 
            for (Int32 i = 0; i < dg_Transactions.Rows.Count; i++) // Last row in dg_Port is a blank row
            {
                if (dg_Transactions["EffectiveDate", i].Value != null)
                {
                    ParentForm1.SetColumn(dg_Transactions, "Amount", i);
                }
            }

        } //SetLayoutTrans()


        private void dgv_Balances_Sorted(object sender, EventArgs e)
        {
            SetLayout();
        }

        private void dg_Transactions_Sorted(object sender, EventArgs e)
        {
            SetLayoutTrans();
        }

        private void bt_Request_Click(object sender, EventArgs e)
        {
            LoadRec();
        }

        private void MLFutures_Balance_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (NeedFullUpdate)
                SystemLibrary.SQLExecute("Exec sp_actionsNeeded 2, 'N'");

            Cursor.Current = Cursors.Default;
        }

        private void bt_BackEnd_Reprocess_Click(object sender, EventArgs e)
        {
            // Force through the processing scripts
            Cursor.Current = Cursors.WaitCursor;
            SystemLibrary.SQLExecute("Exec sp_MLFut_Process_File '', 'MLFut_Cash' ");
            SystemLibrary.SQLExecute("Exec sp_MLFut_Process_File '', 'MLFut_Money' ");
            SystemLibrary.SQLExecute("Exec sp_MLFut_Process_File '', 'MLFut_Trades' ");
            SystemLibrary.SQLExecute("Exec sp_MLFut_Process_File '', 'MLFut_Open_Positions' ");
            LoadRec();

            Cursor.Current = Cursors.Default;
            NeedFullUpdate = true;
            MessageBox.Show("Completed.\r\n\r\nThis has forced a reprocess of uploaded files which will have Reconcile records if able.\r\n" +
                            "This is not normally needed but is here for completeness.", ((Button)sender).Text);


        } // bt_BackEnd_Reprocess_Click()

    }
}
