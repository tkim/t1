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
    public partial class MaintainFunds : Form
    {
        public DataTable dt_MissingFunds;
        public DataTable dt_Fund;
        public DataTable dt_FundCapital;
        public DataTable dt_Portfolio;
        public DataTable dt_Portfolio_Group;
        public DataTable dt_Accounts;
        private Object LastValue;
        public Form1 ParentForm1;
        public Boolean HasChanged = false;

        public MaintainFunds()
        {
            InitializeComponent();
        }

        private void MaintainFunds_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            LoadFund();
            LoadFundCapital();
            LoadMissingFunds();
            LoadPortfolio();
            LoadPortfolio_Group();
        } //MaintainFunds_Load()

        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;

        } //FromParent()

        private void MaintainFunds_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Let the Parent know this has close
            if (ParentForm1 != null && HasChanged)
            {
                ParentForm1.LoadPostMaintainFunds();
            }

        } //MaintainFunds_FormClosed()

        public void LoadMissingFunds()
        {
            // Local Variables
            String mySql;

            mySql = "exec sp_MissingFunds";
            dt_MissingFunds = SystemLibrary.SQLSelectToDataTable(mySql);

            dg_MissingFunds.Rows.Clear();
            foreach (DataRow dr in dt_MissingFunds.Rows)
            {
                int myRow = dg_MissingFunds.Rows.Add();
                dg_MissingFunds["ExtID", myRow].Value = dr["ExtID"];
                dg_MissingFunds["Prime_Broker_Account_Name", myRow].Value = dr["Prime_Broker_Account_Name"].ToString();
                dg_MissingFunds["Client_Name", myRow].Value = dr["Client_Name"].ToString();
            }

        } //LoadMissingFunds()

        public void LoadFund()
        {
            // Local Variables
            String mySql;

            // Reseting dt_fund based in Active = 'Y' can cause issues.
            dg_Fund.Rows.Clear();

            mySql = "Select FundID, ExtID, ExtID2, ExtID3, FundName, ShortName, FundAmount, crncy, CreatedDate, ClosedDate, Active, ParentFundID, AllowTrade " +
                    "From   Fund ";
            if (cb_Active.Checked == true)
                mySql = mySql + "Where    Active = 'Y ' ";
            mySql = mySql +
                    "Order By FundName";
            dt_Fund = SystemLibrary.SQLSelectToDataTable(mySql);

            //Adds null values into the DataTable
            dt_Fund.Columns["FundID"].AllowDBNull = true;
            // create a new row
            DataRow row = dt_Fund.NewRow();
            // set the values
            row["ShortName"] = "  ";
            row["FundID"] = DBNull.Value;
            // add to the DataTable
            dt_Fund.Rows.InsertAt(row, 0);

            DataGridViewComboBoxColumn dcb = (DataGridViewComboBoxColumn)dg_Fund.Columns["ParentFundID"];
            dcb.DataSource = dt_Fund;
            dcb.DisplayMember = "ShortName";
            dcb.ValueMember = "FundID";
            dcb.DataPropertyName = "FundID";
            

            foreach (DataRow dr in dt_Fund.Rows)
            {
                if (dr["FundID"] != DBNull.Value)
                {
                    int myRow = dg_Fund.Rows.Add();
                    dg_Fund["FundID", myRow].Value = dr["FundID"];
                    dg_Fund["_ExtID", myRow].Value = dr["ExtID"];
                    dg_Fund["_ExtID2", myRow].Value = dr["ExtID2"];
                    dg_Fund["_ExtID3", myRow].Value = dr["ExtID3"];
                    dg_Fund["FundName", myRow].Value = dr["FundName"].ToString();
                    dg_Fund["ShortName", myRow].Value = dr["ShortName"].ToString();
                    dg_Fund["FundAmount", myRow].Value = dr["FundAmount"].ToString();
                    dg_Fund["crncy", myRow].Value = dr["crncy"].ToString();
                    dg_Fund["CreatedDate", myRow].Value = dr["CreatedDate"];
                    dg_Fund["ClosedDate", myRow].Value = dr["ClosedDate"];
                    dg_Fund["Active", myRow].Value = dr["Active"].ToString();
                    dg_Fund["AllowTrade", myRow].Value = dr["AllowTrade"].ToString();
                    dg_Fund["ParentFundID", myRow].Value = dr["ParentFundID"];
                }
            }

        } //LoadFund()

        public void LoadPortfolio()
        {
            // Local Variables
            String mySql;

            mySql = "Select PortfolioID, ExtID, PortfolioName, PortfolioAmount, crncy, CreatedDate, ClosedDate, Active " +
                    "From Portfolio ";
            if (cb_Active.Checked == true)
                mySql = mySql + "Where    Active = 'Y ' ";
            mySql = mySql +
                    "Order By PortfolioName";
            dt_Portfolio = SystemLibrary.SQLSelectToDataTable(mySql);

            dg_Portfolio.Rows.Clear();
            foreach (DataRow dr in dt_Portfolio.Rows)
            {
                int myRow = dg_Portfolio.Rows.Add();
                dg_Portfolio["p_PortfolioID", myRow].Value = dr["PortfolioID"];
                dg_Portfolio["p_ExtID", myRow].Value = dr["ExtID"];
                dg_Portfolio["p_PortfolioName", myRow].Value = dr["PortfolioName"].ToString();
                dg_Portfolio["p_PortfolioAmount", myRow].Value = dr["PortfolioAmount"].ToString();
                dg_Portfolio["p_crncy", myRow].Value = dr["crncy"].ToString();
                dg_Portfolio["p_CreatedDate", myRow].Value = dr["CreatedDate"];
                dg_Portfolio["p_ClosedDate", myRow].Value = dr["ClosedDate"];
                dg_Portfolio["p_Active", myRow].Value = dr["Active"].ToString();
            }

        } //LoadPortfolio()

        public void LoadPortfolio_Group()
        {
            // Local Variables
            String mySql;

            // Load the Portfolio Group
            mySql = "Select PortfolioGroupID, FundID, PortfolioID, StartDate, EndDate " +
                    "From	Portfolio_Group ";
            if (cb_Active.Checked == true)
            {
                mySql = mySql +
                        "Where Exists (Select 'x' From Fund Where Active = 'Y' And Portfolio_Group.FundID = Fund.FundID) " +
                        "And Exists (Select 'x' From Portfolio Where Active = 'Y' And Portfolio_Group.PortfolioID = Portfolio.PortfolioID) ";
            }
            mySql = mySql +
                    "Order by FundID, PortfolioID ";
            dt_Portfolio_Group = SystemLibrary.SQLSelectToDataTable(mySql);

            DataGridViewComboBoxColumn dcb = (DataGridViewComboBoxColumn)dg_Portfolio_Group.Columns["pg_FundID"];
            dcb.DataSource = dt_Fund;
            dcb.DisplayMember = "FundName";
            dcb.ValueMember = "FundId";
            dcb.DataPropertyName = "FundId";

            DataGridViewComboBoxColumn dcb1 = (DataGridViewComboBoxColumn)dg_Portfolio_Group.Columns["pg_PortfolioID"];
            dcb1.DataSource = dt_Portfolio;
            dcb1.DisplayMember = "PortfolioName";
            dcb1.ValueMember = "PortfolioID";
            dcb1.DataPropertyName = "PortfolioID";

            dg_Portfolio_Group.Rows.Clear();
            foreach (DataRow dr in dt_Portfolio_Group.Rows)
            {
                int myRow = dg_Portfolio_Group.Rows.Add();
                dg_Portfolio_Group["pg_PortfolioGroupID", myRow].Value = dr["PortfolioGroupID"];
                dg_Portfolio_Group["pg_FundID", myRow].Value = dr["FundID"];
                dg_Portfolio_Group["pg_PortfolioID", myRow].Value = dr["PortfolioID"];
                dg_Portfolio_Group["pg_StartDate", myRow].Value = dr["StartDate"];
                dg_Portfolio_Group["pg_EndDate", myRow].Value = dr["EndDate"];
            }
            
        } //LoadPortfolio_Group()


        private void bt_SaveFund_Click(object sender, EventArgs e)
        {
            // Rules: Only columns that can change are: ExtID, FundName, ShortName, crncy, CreatedDate, ClosedDate, Active
            //          FundID is a system column. FundAmount updated from Capital & P&L updates.

            // Local Variables
            int myFundID;

            // Pre Save.
            dg_Fund.Refresh();
            
            // Test each Row
            foreach (DataGridViewRow dgr in dg_Fund.Rows)
            {
                if (!dgr.IsNewRow) // This is the Blank new row
                {
                    // Check if New Row with data
                    if (dgr.Cells["FundID"].Value == null)
                    {
                        // Update with new FundID
                        myFundID = SystemLibrary.SQLSelectInt32("exec sp_GetNextID 'FundID'");
                        dgr.Cells["FundID"].Value = myFundID;
                    }
                    if (dgr.Cells["_ExtID"].Value == null)
                    {
                        dgr.Cells["_ExtID"].Value = "";
                    }
                    if (dgr.Cells["_ExtID2"].Value == null)
                    {
                        dgr.Cells["_ExtID2"].Value = "";
                    }
                    if (dgr.Cells["_ExtID3"].Value == null)
                    {
                        dgr.Cells["_ExtID3"].Value = "";
                    }
                    if (SystemLibrary.ToString(dgr.Cells["CreatedDate"].Value).Trim().Length == 0)
                    {
                        dgr.Cells["CreatedDate"].Value = DateTime.Now.Date;
                    }
                    if (SystemLibrary.ToString(dgr.Cells["Active"].Value).Trim().Length == 0)
                    {
                        dgr.Cells["Active"].Value = "Y";
                    }
                    if (dgr.Cells["Active"].Value.ToString() == "N" && SystemLibrary.ToString(dgr.Cells["ClosedDate"].Value).Trim().Length == 0)
                    {
                        dgr.Cells["ClosedDate"].Value = DateTime.Now.Date;
                    }
                    // Have the filled in all the fields
                    if (SystemLibrary.ToString(dgr.Cells["FundName"].Value).Trim().Length == 0)
                    {
                        MessageBox.Show("Cannot save Funds without a FundName", "Save Fund Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else if (SystemLibrary.ToString(dgr.Cells["ShortName"].Value).Trim().Length == 0)
                    {
                        MessageBox.Show("Cannot save Funds without a ShortName", "Save Fund Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else if (SystemLibrary.ToString(dgr.Cells["crncy"].Value).Trim().Length == 0)
                    {
                        MessageBox.Show("Cannot save Funds without a Fund Currency", "Save Fund Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
            }
            dg_Fund.Refresh();

            // Save the Data
            foreach (DataGridViewRow dgr in dg_Fund.Rows)
            {   
                String Str_ParentFundID = "null";
                
                if (!dgr.IsNewRow) // This is the Blank new row
                {
                    // need to update, then if rows ==0, insert.
                    SystemLibrary.DebugLine(dgr.Cells["FundID"].Value.ToString());
                    String mySql = "Update  Fund " +
                                   "Set     ExtID = '" + dgr.Cells["_ExtID"].Value.ToString().Replace("'", "''") + "', " +
                                   "        ExtID2 = '" + dgr.Cells["_ExtID2"].Value.ToString().Replace("'", "''") + "', " +
                                   "        ExtID3 = '" + dgr.Cells["_ExtID3"].Value.ToString().Replace("'", "''") + "', " +
                                   "        FundName = '" + dgr.Cells["FundName"].Value.ToString().Replace("'", "''") + "', " +
                                   "        ShortName = '" + dgr.Cells["ShortName"].Value.ToString().Replace("'", "''") + "', " +
                                   "        crncy = '" + dgr.Cells["crncy"].Value.ToString() + "', " +
                                   "        CreatedDate = '" + Convert.ToDateTime(dgr.Cells["CreatedDate"].Value).ToString("dd-MMM-yyyy") + "', ";

                    if (SystemLibrary.ToString(dgr.Cells["ClosedDate"].Value).Trim().Length > 0)
                        mySql = mySql + "        ClosedDate = '" + Convert.ToDateTime(dgr.Cells["ClosedDate"].Value).ToString("dd-MMM-yyyy") + "', ";
                    else
                        mySql = mySql + "        ClosedDate = null, ";
                    
                    if (dgr.Cells["ParentFundID"].Value == null)
                        Str_ParentFundID = dgr.Cells["FundID"].Value.ToString();
                    else
                        Str_ParentFundID = dgr.Cells["ParentFundID"].Value.ToString();
                    
                    if (dgr.Cells["AllowTrade"].Value == null)
                        dgr.Cells["AllowTrade"].Value = 'N';
                    mySql = mySql +
                                   "        Active = '" + dgr.Cells["Active"].Value.ToString() + "', " +
                                   "        ParentFundID = " + Str_ParentFundID + ", " +
                                   "        AllowTrade = '" + dgr.Cells["AllowTrade"].Value.ToString() + "' " +
                                   "Where   FundID = " + dgr.Cells["FundID"].Value.ToString();

                    Int32 myRows = SystemLibrary.SQLExecute(mySql);
                    if (myRows < 1)
                    {
                        String ClosedDate;
                        if (SystemLibrary.ToString(dgr.Cells["ClosedDate"].Value).Trim().Length > 0)
                            ClosedDate = " '" + Convert.ToDateTime(dgr.Cells["ClosedDate"].Value).ToString("dd-MMM-yyyy") + "' ";
                        else
                            ClosedDate = " null ";

                        mySql = "Insert into Fund (FundID, ExtID, ExtID2, ExtID3, FundName, ShortName, FundAmount, crncy, CreatedDate, ClosedDate, Active, ParentFundID, AllowTrade) " +
                                "Values (" + dgr.Cells["FundID"].Value.ToString() + ", '" + dgr.Cells["_ExtID"].Value.ToString().Replace("'", "''") + "', " +
                                "        '" + dgr.Cells["_ExtID2"].Value.ToString().Replace("'", "''") + "', " +
                                "        '" + dgr.Cells["_ExtID3"].Value.ToString().Replace("'", "''") + "', " +
                                "        '" + dgr.Cells["FundName"].Value.ToString().Replace("'", "''") + "', " +
                                "        '" + dgr.Cells["ShortName"].Value.ToString().Replace("'", "''") + "', 0, '" + dgr.Cells["crncy"].Value.ToString() + "', " +
                                "        '" + Convert.ToDateTime(dgr.Cells["CreatedDate"].Value).ToString("dd-MMM-yyyy") + "', " +
                                "        " + ClosedDate + ", '" + dgr.Cells["Active"].Value.ToString() + "', " +
                                "        " + Str_ParentFundID + ", '" + dgr.Cells["AllowTrade"].Value.ToString() + "' " +
                                "        )";
                        myRows = SystemLibrary.SQLExecute(mySql);
                    }

                }
            }

            MessageBox.Show("Fund Data Saved", "Save Fund", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadMissingFunds();
            HasChanged = true;

        } //bt_SaveFund_Click()

        private void dg_MissingFunds_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DataGridView.HitTestInfo info = dg_MissingFunds.HitTest(e.X, e.Y);
                if (info.RowIndex >= 0)
                {
                    //DataGridViewRow view = (DataGridViewRow)dg_MissingFunds.Rows[info.RowIndex].DataBoundItem;
                    DataGridViewRow view = (DataGridViewRow)dg_MissingFunds.Rows[info.RowIndex];
                    if (view != null)
                        dg_MissingFunds.DoDragDrop(view, DragDropEffects.Copy);
                }
            }

        }

        private void dg_Fund_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        } //dg_Fund_DragEnter()

        private void dg_Fund_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = dg_Fund.PointToClient(new Point(e.X, e.Y));
            int myTargetRow = dg_Fund.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            if (myTargetRow >= 0)
            {
                // Get the ExtId from the source.
                DataGridViewRow Source = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));
                String ExtID = SystemLibrary.ToString(Source.Cells["ExtID"].Value);

                // See What the Target table has
                DataGridViewRow Target = (DataGridViewRow)dg_Fund.Rows[myTargetRow];
                String _ExtID = SystemLibrary.ToString(Target.Cells["_ExtID"].Value);
                String FundName = SystemLibrary.ToString(Target.Cells["FundName"].Value);
                Console.Write("A");

                // See if there is already a value in the Target
                if (_ExtID != "")
                {
                    if (MessageBox.Show(this, "There is already a Value of '" + _ExtID + "' on this fund (" + FundName + ").\r\n\r\n" + 
                                        "Do you really wish to replace this with '" + ExtID + "'?",
                                        "Change External ID for Fund", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                Target.Cells["_ExtID"].Value = ExtID;

            }
        } //dg_Fund_DragDrop()

        private void cb_FundCapital_CheckedChanged(object sender, EventArgs e)
        {
            dg_FundCapital.ReadOnly = !cb_FundCapital.Checked;
            bt_SaveFundCapital.Enabled = cb_FundCapital.Checked;

        } //cb_FundCapital_CheckedChanged()

        public void LoadFundCapital()
        {
            // Local Variables
            String mySql;

            // Load the Accounts
            mySql = "Select AccountID, AccountName " +
                    "From   Accounts " +
                    "Where  AccountType = 'CASH' ";
            if (cb_Active.Checked == true)
            {
                mySql = mySql +
                        "And Exists (Select 'x' From Fund Where Active = 'Y' and Accounts.FundID = Fund.FundID) ";
            }
            mySql = mySql +
                    "Order by AccountName ";
            dt_Accounts = SystemLibrary.SQLSelectToDataTable(mySql);

            // RULE: Must Always get ALL rows - the Save button and sp_Fund_Capital_Upload relies on this.
            mySql = "Select Fund_Capital.FundID, Fund_Capital.EffectiveDate, Fund_Capital.Amount_SOD, Fund_Capital.Amount_EOD, Fund_Capital.crncy, Fund_Capital.Description, Fund_Capital.CapitalID, Fund_Capital.AccountID, Fund.Active " +
                    "From	Fund_Capital, " +
                    "       Fund " +
                    "Where  Fund.FundID = Fund_Capital.FundID " +
                    "Order by Fund_Capital.EffectiveDate, Fund_Capital.FundID ";
            dt_FundCapital = SystemLibrary.SQLSelectToDataTable(mySql);

            DataGridViewComboBoxColumn dcb = (DataGridViewComboBoxColumn)dg_FundCapital.Columns["fc_FundID"];
            dcb.DataSource = dt_Fund;
            dcb.DisplayMember = "FundName";
            dcb.ValueMember = "FundId";
            dcb.DataPropertyName = "FundId";

            DataGridViewComboBoxColumn dcb1 = (DataGridViewComboBoxColumn)dg_FundCapital.Columns["fc_AccountID"];
            dcb1.DataSource = dt_Accounts;
            dcb1.DisplayMember = "AccountName";
            dcb1.ValueMember = "AccountID";
            dcb1.DataPropertyName = "AccountID";

            dg_FundCapital.Rows.Clear();
            foreach (DataRow dr in dt_FundCapital.Rows)
            {
                int myRow = dg_FundCapital.Rows.Add();
                dg_FundCapital["fc_FundId", myRow].Value = dr["FundId"];
                dg_FundCapital["fc_EffectiveDate", myRow].Value = dr["EffectiveDate"];
                dg_FundCapital["fc_Amount_SOD", myRow].Value = dr["Amount_SOD"];
                dg_FundCapital["fc_Amount_EOD", myRow].Value = dr["Amount_EOD"];
                dg_FundCapital["fc_crncy", myRow].Value = dr["crncy"];
                dg_FundCapital["fc_Description", myRow].Value = dr["Description"];
                dg_FundCapital["fc_CapitalID", myRow].Value = dr["CapitalID"];
                dg_FundCapital["fc_AccountID", myRow].Value = dr["AccountID"];
                dg_FundCapital["fc_Active", myRow].Value = dr["Active"];
            }
            FundCapitalFilterActive(cb_Active.Checked);

        } //LoadFundCapital()

        private void FundCapitalFilterActive(Boolean onlyActive)
        {
            for (int myRow = 0; myRow < dg_FundCapital.Rows.Count; myRow++)
            {
                String isActive = SystemLibrary.ToString(dg_FundCapital["fc_Active", myRow].Value);

                // See if FundID is in active funds
                if (isActive == "N" && onlyActive)
                    dg_FundCapital.Rows[myRow].Visible = false;
                else
                    dg_FundCapital.Rows[myRow].Visible = true;
            }

        } //FundCapitalFilterActive()

        private void bt_SaveFundCapital_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;

            MessageBox.Show("Note, this update may take several minutes\r\n\r\nPress Ok to Continue.", "Save Fund Capital", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            // Pre Save.
            dg_Fund.Refresh();
            
            // Load ALL the records into Fund_Capital_Upload
            // - Test each Row
            foreach (DataGridViewRow dgr in dg_FundCapital.Rows)
            {
                if (!dgr.IsNewRow) // This is the Blank new row
                {
                    // Have the filled in all the fields
                    if (SystemLibrary.ToString(dgr.Cells["fc_FundId"].Value).Trim().Length == 0)
                    {
                        MessageBox.Show("Cannot save Fund Capital without a FundName", "Save Fund Capital Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else if (SystemLibrary.ToString(dgr.Cells["fc_AccountId"].Value).Trim().Length == 0)
                    {
                        MessageBox.Show("Cannot save Fund Capital without an AccountName (Bank Account)", "Save Fund Capital Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else if (SystemLibrary.ToString(dgr.Cells["fc_EffectiveDate"].Value).Trim().Length == 0)
                    {
                        MessageBox.Show("Cannot save Fund Capital without an EffectiveDate", "Save Fund Capital Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else if ( !(SystemLibrary.ToDecimal(dgr.Cells["fc_Amount_SOD"].Value) != 0 || SystemLibrary.ToDecimal(dgr.Cells["fc_Amount_EOD"].Value) != 0) )
                    {
                        MessageBox.Show("Cannot save Fund Capital without an valid Amount", "Save Fund Capital Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else if (SystemLibrary.ToString(dgr.Cells["fc_crncy"].Value).Trim().Length == 0)
                    {
                        MessageBox.Show("Cannot save Fund Capital without a Fund Currency", "Save Fund Capital Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else if (SystemLibrary.ToString(dgr.Cells["fc_Description"].Value).Trim().Length == 0)
                    {
                        MessageBox.Show("Cannot save Fund Capital without a transaction Description", "Save Fund Capital Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    // Set CapitalID to a database 'null' for the insert statement
                    if (dgr.Cells["fc_CapitalID"].Value==null)
                        dgr.Cells["fc_CapitalID"].Value = "null";
                }
            }
            // Clean up Upload Table
            SystemLibrary.SQLExecute("Delete from Fund_Capital_Upload");

            // - Test each Row
            foreach (DataGridViewRow dgr in dg_FundCapital.Rows)
            {
                String EffectiveDate = "";
                int myRows;
                if (!dgr.IsNewRow) // This is the Blank new row
                {
                    EffectiveDate = " '" + Convert.ToDateTime(dgr.Cells["fc_EffectiveDate"].Value).ToString("dd-MMM-yyyy") + "' ";
                    mySql = "Insert into Fund_Capital_Upload (FundId, EffectiveDate, Amount_SOD, Amount_EOD, crncy, Description, CapitalID, AccountID) " +
                            "Values (" + dgr.Cells["fc_FundId"].Value.ToString() + ", " + EffectiveDate + ", " +
                                     SystemLibrary.ToDecimal(dgr.Cells["fc_Amount_SOD"].Value).ToString() + ", " + SystemLibrary.ToDecimal(dgr.Cells["fc_Amount_EOD"].Value).ToString() + 
                                     ", '" + dgr.Cells["fc_crncy"].Value.ToString() +
                                     "','" + dgr.Cells["fc_Description"].Value.ToString().Replace("'", "''") + "', " +
                                     dgr.Cells["fc_CapitalID"].Value.ToString() + ", " + dgr.Cells["fc_AccountID"].Value.ToString() + ") ";
                    myRows = SystemLibrary.SQLExecute(mySql);
                }
            }
            SystemLibrary.SQLExecute("Exec sp_Fund_Capital_Upload");
            Cursor.Current = Cursors.Default;

            MessageBox.Show("Fund Capital Data Saved & History recalculated", "Save Fund Capital", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadFundCapital();
            HasChanged = true;

        } //bt_SaveFundCapital_Click()

        private void dg_FundCapital_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Seems the only way I can get the previous value before CellEndEdit().
            LastValue = dg_FundCapital[e.ColumnIndex, e.RowIndex].Value;
            if (dg_FundCapital.Columns[e.ColumnIndex].Name == "fc_crncy")
                e.Cancel = true;

        } //dg_FundCapital_CellBeginEdit()

        private void dg_FundCapital_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Local Variables
            String mySql;
            String myFundId;
            String myCrncy;

            // What column is changing
            // - dg_Broker.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.ToString().ToUpper()
            if (dg_FundCapital.Columns[e.ColumnIndex].Name == "fc_FundID")
            {
                // Set the curncy column
                if (dg_FundCapital.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    myFundId = dg_FundCapital.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim();
                    mySql = "Select crncy " +
                            "From   Fund " +
                            "Where  FundID = " + myFundId + " ";
                    myCrncy = SystemLibrary.SQLSelectString(mySql);
                    dg_FundCapital.Rows[e.RowIndex].Cells["fc_crncy"].Value = myCrncy;
                }
            }
            else if (dg_FundCapital.Columns[e.ColumnIndex].Name == "fc_EffectiveDate")
            {
                // Validate this is a Date
                DateTime myResult = new DateTime();
                if (!DateTime.TryParse(dg_FundCapital.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim(), out myResult))
                {
                    dg_FundCapital.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                }
                else
                    dg_FundCapital.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult.ToString(dg_FundCapital.Columns[e.ColumnIndex].DefaultCellStyle.Format);

            }
            else if (dg_FundCapital.Columns[e.ColumnIndex].Name == "fc_Amount_SOD" || dg_FundCapital.Columns[e.ColumnIndex].Name == "fc_Amount_EOD")
            {
                // Validate this is a number
                Decimal myResult = new Decimal();
                myResult = SystemLibrary.ToDecimal(dg_FundCapital.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                dg_FundCapital.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult; //.ToString(dg_FundCapital.Columns[e.ColumnIndex].DefaultCellStyle.Format);
                //dg_FundCapital.Rows[e.RowIndex].Cells[e.ColumnIndex].for
            }
            if (dg_FundCapital.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.DefaultCellStyle.Format.ToString().IndexOf('%') > -1)
            {
                dg_FundCapital.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = SystemLibrary.ToDouble(dg_FundCapital.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) / 100.0;
            }
            if (dg_FundCapital.Columns[e.ColumnIndex].Name == "Exchange")
                dg_FundCapital.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dg_FundCapital.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper();
            dg_FundCapital.Refresh();


        } //dg_FundCapital_CellEndEdit()

        private void dg_Portfolio_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            LastValue = dg_Portfolio[e.ColumnIndex, e.RowIndex].Value;

        } //dg_Portfolio_CellBeginEdit()

        private void dg_Portfolio_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dg_Portfolio.Columns[e.ColumnIndex].Name == "p_CreatedDate" ||
                dg_Portfolio.Columns[e.ColumnIndex].Name == "p_ClosedDate"
                )
            {
                // Validate this is a Date
                DateTime myResult = new DateTime();
                if (!DateTime.TryParse(dg_Portfolio.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim(), out myResult))
                {
                    dg_Portfolio.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                }
                else
                    dg_Portfolio.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult.ToString(dg_Portfolio.Columns[e.ColumnIndex].DefaultCellStyle.Format);

            }
            dg_Portfolio.Refresh();

        } //dg_Portfolio_CellEndEdit()

        private void bt_SavePortfolio_Click(object sender, EventArgs e)
        {
            // Rules: Only columns that can change are: ExtID, PortfolioName, CreatedDate, ClosedDate, Active
            //          PortfolioID is a system column. Other Columns may not be used.

            // Local Variables
            String mySql;
            int myPortfolioID;

            // Pre Save.
            dg_Portfolio.Refresh();

            // Test each Row
            foreach (DataGridViewRow dgr in dg_Portfolio.Rows)
            {
                if (!dgr.IsNewRow) // This is the Blank new row
                {
                    // Check if New Row with data
                    if (dgr.Cells["p_PortfolioID"].Value == null)
                    {
                        // Update with new PortfolioID
                        myPortfolioID = SystemLibrary.SQLSelectInt32("exec sp_GetNextID 'PortfolioID'");
                        dgr.Cells["p_PortfolioID"].Value = myPortfolioID;
                    }
                    if (dgr.Cells["p_ExtID"].Value == null)
                    {
                        dgr.Cells["p_ExtID"].Value = "";
                    }
                    if (SystemLibrary.ToString(dgr.Cells["p_CreatedDate"].Value).Trim().Length == 0)
                    {
                        dgr.Cells["p_CreatedDate"].Value = SystemLibrary.f_Now().Date;
                    }
                    if (SystemLibrary.ToString(dgr.Cells["p_Active"].Value).Trim().Length == 0)
                    {
                        dgr.Cells["p_Active"].Value = "Y";
                    }
                    if (dgr.Cells["p_Active"].Value.ToString() == "N" && SystemLibrary.ToString(dgr.Cells["p_ClosedDate"].Value).Trim().Length == 0)
                    {
                        dgr.Cells["p_ClosedDate"].Value = SystemLibrary.f_Now().Date;
                    }
                    // Have the filled in all the fields
                    if (SystemLibrary.ToString(dgr.Cells["p_PortfolioName"].Value).Trim().Length == 0)
                    {
                        MessageBox.Show("Cannot save Portfolios without a PortfolioName", "Save Portfolio Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    // See if PortfolioID CreatedDate is ok.
                    mySql = "Select min(TradeDate) From Trade Where PortfolioID = " + dgr.Cells["p_PortfolioID"].Value.ToString();
                    DateTime myCheck = SystemLibrary.SQLSelectDateTime(mySql, DateTime.MaxValue);
                    if (myCheck.CompareTo(Convert.ToDateTime(dgr.Cells["p_CreatedDate"].Value)) < 0)
                    {
                        MessageBox.Show("Cannot save '" + dgr.Cells["p_PortfolioName"].Value + "' as \r\n" +
                                        "Created Date needs to be less than the first trade date of "+myCheck.ToString("dd-MMM-yyyy"), "Save Portfolio Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    if (SystemLibrary.ToString(dgr.Cells["p_ClosedDate"].Value).Trim().Length > 0)
                    {
                        mySql = "Select max(TradeDate) From Trade Where PortfolioID = " + dgr.Cells["p_PortfolioID"].Value.ToString();
                        myCheck = SystemLibrary.SQLSelectDateTime(mySql, DateTime.MinValue);
                        if (myCheck.CompareTo(Convert.ToDateTime(dgr.Cells["p_ClosedDate"].Value)) > -1)
                        {
                            MessageBox.Show("Cannot save '" + dgr.Cells["p_PortfolioName"].Value + "' as \r\n" +
                                            "Closed Date needs to be greater than the last trade date of " + myCheck.ToString("dd-MMM-yyyy"), "Save Portfolio Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                }
            }
            dg_Portfolio.Refresh();

            // Save the Data
            foreach (DataGridViewRow dgr in dg_Portfolio.Rows)
            {
                if (!dgr.IsNewRow) // This is the Blank new row
                {
                    // need to update, then if rows ==0, insert.
                    SystemLibrary.DebugLine(dgr.Cells["p_PortfolioID"].Value.ToString());
                    //                            "        crncy = '" + dgr.Cells["p_crncy"].Value.ToString() + "', " +
                    mySql = "Update  Portfolio " +
                            "Set     ExtID = '" + SystemLibrary.ToString(dgr.Cells["p_ExtID"].Value).Replace("'", "''") + "', " +
                            "        PortfolioName = '" + dgr.Cells["p_PortfolioName"].Value.ToString().Replace("'", "''") + "', " +
                            "        CreatedDate = '" + Convert.ToDateTime(dgr.Cells["p_CreatedDate"].Value).ToString("dd-MMM-yyyy") + "', ";

                    if (SystemLibrary.ToString(dgr.Cells["p_ClosedDate"].Value).Trim().Length > 0)
                    {
                        mySql = mySql +
                                   "        ClosedDate = '" + Convert.ToDateTime(dgr.Cells["p_ClosedDate"].Value).ToString("dd-MMM-yyyy") + "', ";
                    }
                    mySql = mySql +
                                   "        Active = '" + dgr.Cells["p_Active"].Value.ToString() + "' " +
                                   "Where   PortfolioID = " + dgr.Cells["p_PortfolioID"].Value.ToString();

                    Int32 myRows = SystemLibrary.SQLExecute(mySql);
                    if (myRows < 1)
                    {
                        String ClosedDate;
                        if (SystemLibrary.ToString(dgr.Cells["p_ClosedDate"].Value).Trim().Length > 0)
                            ClosedDate = " '" + Convert.ToDateTime(dgr.Cells["p_ClosedDate"].Value).ToString("dd-MMM-yyyy") + "' ";
                        else
                            ClosedDate = " null ";

                        mySql = "Insert into Portfolio (PortfolioID, ExtID, PortfolioName, PortfolioAmount, crncy, CreatedDate, ClosedDate, Active) " +
                                "Values (" + dgr.Cells["p_PortfolioID"].Value.ToString() + ", '" + dgr.Cells["p_ExtID"].Value.ToString().Replace("'", "''") + "', " +
                                "        '" + dgr.Cells["p_PortfolioName"].Value.ToString().Replace("'", "''") + "', 0, null, " +
                                "        '" + Convert.ToDateTime(dgr.Cells["p_CreatedDate"].Value).ToString("dd-MMM-yyyy") + "', " +
                                "        " + ClosedDate + ", '" + dgr.Cells["p_Active"].Value.ToString() + "' " +
                                "        )";
                        myRows = SystemLibrary.SQLExecute(mySql);
                    }
                    else
                    {
                        // Need to update the History on the Positions Table
                        mySql = "Update Positions " +
                                "Set PortfolioName = '" + dgr.Cells["p_PortfolioName"].Value.ToString().Replace("'", "''") + "' " +
                                "Where  PortfolioID = " + dgr.Cells["p_PortfolioID"].Value.ToString() + " " +
                                "and    PortfolioName <> '" + dgr.Cells["p_PortfolioName"].Value.ToString().Replace("'", "''") + "' ";
                        myRows = SystemLibrary.SQLExecute(mySql);
                    }

                }
            }

            MessageBox.Show("Portfolio Data Saved", "Save Portfolio", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadPortfolio();
            HasChanged = true;

        } // bt_SavePortfolio_Click()

        private void dg_Portfolio_Group_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Local Variables
            String PortfolioGroupID = SystemLibrary.ToString(dg_Portfolio_Group["pg_PortfolioGroupID", e.RowIndex].Value);
            LastValue = dg_Portfolio_Group[e.ColumnIndex, e.RowIndex].Value;

            if (PortfolioGroupID != "")
            {
                switch (dg_Portfolio_Group.Columns[e.ColumnIndex].Name)
                {
                    case "pg_FundID":
                    case "pg_PortfolioID":
                        e.Cancel = true;
                        break;
                }
            }

        } //dg_Portfolio_Group_CellBeginEdit()

        private void dg_Portfolio_Group_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dg_Portfolio_Group.Columns[e.ColumnIndex].Name == "pg_StartDate" ||
                dg_Portfolio_Group.Columns[e.ColumnIndex].Name == "pg_EndDate"
                )
            {
                // Validate this is a Date
                DateTime myResult = new DateTime();
                if (!DateTime.TryParse(dg_Portfolio_Group.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim(), out myResult))
                {
                    dg_Portfolio_Group.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                }
                else
                    dg_Portfolio_Group.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult.ToString(dg_Portfolio_Group.Columns[e.ColumnIndex].DefaultCellStyle.Format);

            }
            dg_Portfolio_Group.Refresh();

        } //dg_Portfolio_Group_CellEndEdit()

        private void bt_SavePortfolioGroup_Click(object sender, EventArgs e)
        {
            
            // Rules: Only columns that can change are: StartDate, EndDate

            // Local Variables
            String mySql;
            int myPortfolioGroupID;

            // Pre Save.
            dg_Portfolio_Group.Refresh();

            // Test each Row
            foreach (DataGridViewRow dgr in dg_Portfolio_Group.Rows)
            {
                if (!dgr.IsNewRow) // This is the Blank new row
                {
                    // Check if New Row with data
                    if (dgr.Cells["pg_FundID"].Value == null)
                    {
                        MessageBox.Show("Cannot save Portfolio Group without a Fund Name", "Save Portfolio Group Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    if (dgr.Cells["pg_PortfolioID"].Value == null)
                    {
                        MessageBox.Show("Cannot save Portfolio Group without a PortfolioName", "Save Portfolio Group Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    if (SystemLibrary.ToString(dgr.Cells["pg_StartDate"].Value).Trim().Length == 0)
                    {
                        dgr.Cells["pg_StartDate"].Value = SystemLibrary.f_Now().Date;
                    }

                    // See if PortfolioID StartDate is ok.
                    mySql = "Select min(TradeDate) " +
                            "From   Trade " +
                            "Where  FundID = " + dgr.Cells["pg_FundID"].Value.ToString() + " " +
                            "And    PortfolioID = " + dgr.Cells["pg_PortfolioID"].Value.ToString();
                    DateTime myCheck = SystemLibrary.SQLSelectDateTime(mySql, DateTime.MaxValue);
                    if (myCheck.CompareTo(Convert.ToDateTime(dgr.Cells["pg_StartDate"].Value)) < 0)
                    {
                        MessageBox.Show("Cannot save '" + dgr.Cells["pg_FundID"].FormattedValue + "' / '" + dgr.Cells["pg_PortfolioID"].FormattedValue + "' as \r\n" +
                                        "Start Date needs to be less than the first trade date of " + myCheck.ToString("dd-MMM-yyyy"), "Save Portfolio Group Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    if (SystemLibrary.ToString(dgr.Cells["pg_EndDate"].Value).Trim().Length > 0)
                    {
                        mySql = "Select max(TradeDate) " +
                                "From   Trade " +
                                "Where  FundID = " + dgr.Cells["pg_FundID"].Value.ToString() + " " +
                                "And    PortfolioID = " + dgr.Cells["pg_PortfolioID"].Value.ToString();
                        myCheck = SystemLibrary.SQLSelectDateTime(mySql, DateTime.MinValue);
                        if (myCheck.CompareTo(Convert.ToDateTime(dgr.Cells["pg_EndDate"].Value)) > -1)
                        {
                            MessageBox.Show("Cannot save '" + dgr.Cells["pg_FundID"].FormattedValue + "' / '" + dgr.Cells["pg_PortfolioID"].FormattedValue + "' as \r\n" +
                                            "End Date needs to be greater than the last trade date of " + myCheck.ToString("dd-MMM-yyyy"), "Save Portfolio Group Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                }
            }
            dg_Portfolio_Group.Refresh();

            // Save the Data
            foreach (DataGridViewRow dgr in dg_Portfolio_Group.Rows)
            {
                if (!dgr.IsNewRow) // This is the Blank new row
                {
                    // Now ready to save, assign a PortfolioGroupID
                    // - Otherwise user cant necessarily fix previous errors as having a PortfolioGroupID block certain changes.
                    if (dgr.Cells["pg_PortfolioGroupID"].Value == null)
                    {
                        // Update with new PortfolioID
                        myPortfolioGroupID = SystemLibrary.SQLSelectInt32("exec sp_GetNextID 'PortfolioGroupID'");
                        dgr.Cells["pg_PortfolioGroupID"].Value = myPortfolioGroupID;
                    }
                    // need to update, then if rows ==0, insert.
                    SystemLibrary.DebugLine(dgr.Cells["pg_PortfolioGroupID"].Value.ToString());
                    //                            "        crncy = '" + dgr.Cells["p_crncy"].Value.ToString() + "', " +
                    mySql = "Update  Portfolio_Group " +
                            "Set     StartDate = '" + Convert.ToDateTime(dgr.Cells["pg_StartDate"].Value).ToString("dd-MMM-yyyy") + "' ";

                    if (SystemLibrary.ToString(dgr.Cells["pg_EndDate"].Value).Trim().Length > 0)
                    {
                        mySql = mySql +
                                   ",        EndDate = '" + Convert.ToDateTime(dgr.Cells["pg_EndDate"].Value).ToString("dd-MMM-yyyy") + "' ";
                    }
                    mySql = mySql +
                                   "Where   PortfolioGroupID = " + dgr.Cells["pg_PortfolioGroupID"].Value.ToString();

                    Int32 myRows = SystemLibrary.SQLExecute(mySql);
                    if (myRows < 1)
                    {
                        String EndDate;
                        if (SystemLibrary.ToString(dgr.Cells["pg_EndDate"].Value).Trim().Length > 0)
                            EndDate = " '" + Convert.ToDateTime(dgr.Cells["pg_EndDate"].Value).ToString("dd-MMM-yyyy") + "' ";
                        else
                            EndDate = " null ";

                        mySql = "Insert into Portfolio_Group (PortfolioGroupID, PortfolioID, FundID, StartDate, EndDate) " +
                                "Values (" + dgr.Cells["pg_PortfolioGroupID"].Value.ToString() + ", " + dgr.Cells["pg_PortfolioID"].Value.ToString().Replace("'", "''") + ", " +
                                "        " + dgr.Cells["pg_FundID"].Value.ToString() + ", " +
                                "        '" + Convert.ToDateTime(dgr.Cells["pg_StartDate"].Value).ToString("dd-MMM-yyyy") + "', " +
                                "        " + EndDate +
                                "        )";
                        myRows = SystemLibrary.SQLExecute(mySql);
                    }

                }
            }

            MessageBox.Show("Portfolio Group Data Saved", "Save Portfolio Group", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadPortfolio_Group();
            HasChanged = true;

        } //bt_SavePortfolioGroup_Click()

        private void cb_Active_CheckedChanged(object sender, EventArgs e)
        {
            MaintainFunds_Load(null,null);
            //FundCapitalFilterActive(cb_Active.Checked);

        }  //cb_Active_CheckedChanged()

        private void dg_FundCapital_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Hide this row from the user
            e.Cancel = false;

        } //dg_FundCapital_DataError()

    }
}
