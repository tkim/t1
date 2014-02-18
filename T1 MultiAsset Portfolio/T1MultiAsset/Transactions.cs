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
    public partial class Transactions : Form
    {
        // Public Variables
        public Form1 ParentForm1;
        DataTable dt_Transactions;
        DataTable dt_TranType;
        DataTable dt_Fund;
        DataTable dt_Portfolio;
        DataTable dt_Accounts;
        public Object LastValue;
        int myFundID;
        int myAccountID;
        Boolean isStartUp = false;


        public Transactions()
        {
            InitializeComponent();
        }

        public void FromParent(Form inForm, int inFundID)
        {
            ParentForm1 = (Form1)inForm;
            ParentForm1.CopyDataTable(ref dt_Fund, ref dt_Portfolio);
            myFundID = inFundID;
        } //FromParent()

        private void cb_TranType_CheckedChanged(object sender, EventArgs e)
        {
            // Loop down all the entries in clb_TranType and set to the state of this object
            for (int i = 0; i < clb_TranType.Items.Count; i++)
                clb_TranType.SetItemChecked(i, cb_TranType.Checked);
        }

        private void Transactions_Load(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;

            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            isStartUp = true;

            // Load the TranType list box
            dt_TranType = SystemLibrary.SQLSelectToDataTable("Select * from TranType order by TranType");
            clb_TranType.Items.Clear();
            clb_TranType.BeginUpdate();
            for (int i = 0; i < dt_TranType.Rows.Count; i++)
            {
                clb_TranType.Items.Add(dt_TranType.Rows[i]["TranType"].ToString(), CheckState.Checked);
            }
            clb_TranType.Items.Add("(Blanks)", CheckState.Checked);
            clb_TranType.EndUpdate();

            // Can't have <All> as a Fund option, so create own records in dt_Fund
            // Load the Fund ddlb
            try
            {
                // Need FundAmount as Currency in Top half of SQL
                mySql = "Select FundId, FundName, FundAmount, crncy, ShortName " +
                        "From   Fund " +
                        "Where  Active = 'Y' " +
                        "Order By 2 ";
                dt_Fund = SystemLibrary.SQLSelectToDataTable(mySql);
            }
            catch { }


            // Load the cb_Fund
            cb_Fund.DataSource = dt_Fund;
            cb_Fund.DisplayMember = "FundName";
            cb_Fund.ValueMember = "FundId";

            // Select the Account that matches the passed in Fund
            DataRow[] dr_Find = dt_Fund.Select("FundId=" + myFundID.ToString());
            if (dr_Find.Length > 0)
            {
                cb_Fund.SelectedValue = myFundID;
            }
            else
            {
                if (cb_Fund.Items.Count < 1)
                {
                    //MessageBox.Show("Must have at least 1 fund", this.Text);
                    return;
                }
                else
                {
                    cb_Fund.SelectedIndex = 0;
                    myFundID = Convert.ToInt16(((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[0].ToString());
                }
            }

            LoadAccounts();

            isStartUp = false;

        }

        private void LoadAccounts()
        {
            // Load the Accounts based on the Fund
            dt_Accounts = SystemLibrary.SQLSelectToDataTable("Select * from Accounts where fundID = " + myFundID);
            cb_Accounts.DataSource = dt_Accounts;
            cb_Accounts.DisplayMember = "AccountName";
            cb_Accounts.ValueMember = "AccountId";
            cb_Accounts.SelectedIndex = 0;
            myAccountID = Convert.ToInt16(((DataRowView)(cb_Accounts.SelectedItem)).Row.ItemArray[0].ToString());

        }

        private void bt_Request_Click(object sender, EventArgs e)
        {
            // And isNull(TranType,'(Blanks)') in (..myTranType..)
            // Local Variables
            String myTranType = "";
            String mySql;


            for (int i = 0; i < clb_TranType.CheckedItems.Count; i++)
                myTranType = myTranType + "'" + clb_TranType.CheckedItems[i].ToString() + "',";
            // Strip off trailing ,
            myTranType = myTranType.Substring(0,myTranType.Length-1);

            // isNull(Reconcilled,'N')
            // DO ONE fund at a time.
            mySql = "SELECT     t.Source, t.TranID, t.TranType, " + 
                    "           t.AccountID, a.AccountName, a.AccountType, t.FundID, f.ShortName, t.PortfolioID, p.PortfolioName, t.crncy, " +
                    "		    t.EffectiveDate, t.RecordDate, t.Amount, t.Reconcilled, t.Description, t.TradeID, t.CapitalID, " +
                    "           t.ExtID, c.Description AS [Capital Desc], t.JournalID " +
                    "FROM         Transactions AS t INNER JOIN " +
                    "                      Fund AS f ON t.FundID = f.FundID INNER JOIN " +
                    "                      Accounts AS a ON t.AccountID = a.AccountID LEFT OUTER JOIN " +
                    "                      Portfolio AS p ON t.PortfolioID = p.PortfolioID LEFT OUTER JOIN " +
                    "                      Fund_Capital AS c ON t.CapitalID = c.CapitalID " +
                    "Where  isNull(TranType,'(Blanks)') in (" + myTranType + ") " +
                    "And    t.FundID in (Select FundID from Fund Where ParentFundID = " + myFundID + " or FundID = " + myFundID + ")" +
                    "And    t.AccountID = " + myAccountID + " ";

                    if (cb_Unreconcilled.Checked)
                        mySql = mySql + "And    t.Reconcilled = 'N' ";
                    
                    mySql = mySql + "Order by EffectiveDate ";

            dt_Transactions = SystemLibrary.SQLSelectToDataTable(mySql);
            dgv_Transactions.DataSource = dt_Transactions;

            /*
            if (!dgv_Transactions.Columns.Contains("FundName"))
            {
                DataGridViewComboBoxColumn FundName = new DataGridViewComboBoxColumn();
                FundName.DataPropertyName = "FundID";
                FundName.Name = "FundName";
                FundName.HeaderText = "FundName";
                FundName.DataSource = dt_Fund;
                FundName.DisplayMember = "ShortName"; // "FundName";
                FundName.ValueMember = "FundID";
                FundName.ReadOnly = true;
                //FundName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                FundName.Width = 300;
                dgv_Transactions.Columns.Insert(dgv_Transactions.Columns["FundID"].Index, FundName);
            }
             */
            //dgv_Transactions.Columns["TranID"].Visible = false;
            dgv_Transactions.Columns["AccountID"].Visible = false;
            dgv_Transactions.Columns["PortfolioID"].Visible = false;
            dgv_Transactions.Columns["FundID"].Visible = false;
            dgv_Transactions.Columns["CapitalID"].Visible = false;
            dgv_Transactions.Columns["ShortName"].HeaderText = "Account";

            SetFormats();

            /*
            dcb = (DataGridViewComboBoxColumn)dgv_Transactions.Columns["PortfolioID"];
            dcb.DataSource = dt_Portfolio;
            dcb.DisplayMember = "PortfolioName";
            dcb.ValueMember = "PortfolioID";
            */

            dgv_Transactions.Refresh();

            //MessageBox.Show(myTranType);
        }

        private void SetFormats()
        {
            dgv_Transactions.Columns["EffectiveDate"].DefaultCellStyle.Format = "dd-MMM-yy"; // M = dd-Mon, D = Short Date/Time, F = Long Date/Time
            dgv_Transactions.Columns["EffectiveDate"].DefaultCellStyle.ForeColor = Color.Blue;
            dgv_Transactions.Columns["RecordDate"].DefaultCellStyle.Format = "dd-MMM-yy"; // M = dd-Mon, D = Short Date/Time, F = Long Date/Time
            dgv_Transactions.Columns["RecordDate"].DefaultCellStyle.ForeColor = Color.Blue;

            // Set Column Widths
            for(int i=0;i<dgv_Transactions.Columns.Count;i++)
                dgv_Transactions.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dgv_Transactions.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_Transactions.Columns["Description"].Width = 150;
            dgv_Transactions.Columns["Capital Desc"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_Transactions.Columns["Capital Desc"].Width = 150;
            dgv_Transactions.Columns["ExtID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv_Transactions.Columns["ExtID"].Width = 80;

            ParentForm1.SetFormatColumn(dgv_Transactions, "Amount", Color.Empty, Color.Empty, "N2", "0");

            for (Int32 i = 0; i < dgv_Transactions.Rows.Count; i++) // Last row in dg_Port is a blank row
            {
                ParentForm1.SetColumn(dgv_Transactions, "Amount", i);
            }


        } //SetFormats()

        private void cb_Fund_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isStartUp)
                return;
            myFundID = Convert.ToInt16(((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[0].ToString());
            LoadAccounts();
        }

        private void dgv_Transactions_Sorted(object sender, EventArgs e)
        {
            SetFormats();
        }

        private void cb_Accounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            myAccountID = Convert.ToInt16(((DataRowView)(cb_Accounts.SelectedItem)).Row.ItemArray[0].ToString());
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySQL;
            Boolean FoundChange = false;
            int TranID = 0;

            if (dt_Transactions == null)
                return;

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;


            // Saves any Edit changes down to dt_ForCustodians
            dgv_Transactions.Refresh();

            // Check for any user changes and update the database
            for (int i = 0; i < dt_Transactions.Rows.Count; i++)
            {
                // Save changed records back to the database.
                switch(dt_Transactions.Rows[i].RowState.ToString())
                {
                    case "Deleted":
                        FoundChange = true;

                        // Delete the Transactions Row for this TranID
                        TranID = SystemLibrary.ToInt32(dt_Transactions.Rows[i]["TranID", DataRowVersion.Original]);
                        mySQL = "Delete From Transactions Where TranID=" + TranID.ToString();
                        SystemLibrary.SQLExecute(mySQL);

                        // Delete the Profit Row for this TranID
                        mySQL = "Delete From Profit Where TranID=" + TranID.ToString();
                        SystemLibrary.SQLExecute(mySQL);
                        break;

                    case "Unchanged":
                        break;

                    case "Added":
                        FoundChange = true;

                        // New Row - so obtain a new TranID and Insert
                        TranID = SystemLibrary.SQLSelectInt32("exec sp_GetNextID 'TranID' ");
                        if (TranID <= 0)
                        {
                            MessageBox.Show("Failed to get a new Transaction ID\r\n\r\nSome changes may be lost.", bt_Save.Text);
                            // Refresh
                            bt_Request_Click(null, null);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        mySQL = "Insert into Transactions???"; // OR USE BULK UPDATE with Single row?
                        break;

                    case "Modified":
                        FoundChange = true;

                        TranID = SystemLibrary.ToInt32(dt_Transactions.Rows[i]["TranID"]);

                        // Delete the Profit Row for this TranID
                        mySQL = "Delete From Profit Where TranID=" + TranID.ToString();
                        SystemLibrary.SQLExecute(mySQL);

                        // Update the Transaction Record

                        /*
                        dt_ForCustodians.Rows[i]["CommissionRate"] = CommissionRate;
                        mySql = "Update Trade " +
                                "Set    BrokerConfirmed = '" + mySentToCustodian + "', " +
                                "       Quantity = " + Quantity.ToString() + ", " +
                                "       Price = " + Price.ToString() + ", " +
                                "       GrossValue = " + GrossValue.ToString() + ", " +
                                "       Commission = " + Commission.ToString() + ", " +
                                "       Stamp = " + Stamp.ToString() + ", " +
                                "       GST = " + GST.ToString() + ", " +
                                "       NetValue = " + NetValue.ToString() + ", " +
                                "       CommissionRate = " + CommissionRate.ToString() + ", " +
                                "       SettlementDate = '" + Settlement_Date.ToString("dd-MMM-yyyy") + "' " +
                                "Where TradeID =  " + ClientTransactionID;
                        SystemLibrary.SQLExecute(mySql);
                         */
                        break;
                    default:
                        Console.WriteLine("TranID={0} - RowState={1}", TranID, dt_Transactions.Rows[i].RowState);
                        break;
                }
            }

            if (FoundChange)
            {
                // Update Profit Table
                SystemLibrary.SQLExecute("Exec sp_Apply_Trans_to_Profit ");
            }

            // Refresh
            bt_Request_Click(null,null);
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Transaction Records Saved.", bt_Save.Text);

        }
    }
}
