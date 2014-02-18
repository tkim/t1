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
    public partial class JournalCapitalMovement : Form
    {
        // Global Variables
        public Form ParentForm1;
        public int TranID;
        public Double Amount;
        public DataTable dt_Account;

        public JournalCapitalMovement()
        {
            InitializeComponent();
        }

        private void JournalCapitalMovement_Load(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;

            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            mySql = "Select * from accounts " +
                    "Where AccountType = 'CASH'  " +
                    "And	FundID = (Select ParentFundID from Fund Where FundID = (Select FundID from Transactions Where TranID = " + TranID.ToString() + ")) " +
                    "And AccountID <> (Select AccountID from Transactions Where TranID = " + TranID.ToString() + " ) " +
                    "And	Future_BrokerID is null ";

            dt_Account = SystemLibrary.SQLSelectToDataTable(mySql);

            cb_Account.DataSource = dt_Account;
            cb_Account.DisplayMember = "AccountName";
            cb_Account.ValueMember = "AccountID";

            if (dt_Account.Rows.Count == 0)
            {
                rb_Bank.Enabled = false;
                rb_Investor.Checked = true;
                cb_Account.Enabled = false;
            }

        } //JournalCapitalMovement_Load()

        // Use this when not passing in a know Journal Action
        public void FromParent(Form inForm, int InTranID, Double inAmount)
        {
            ParentForm1 = inForm;
            TranID = InTranID;
            Amount = inAmount;

            if (Amount < 0)
            {
                rb_Bank.Text = rb_Bank.Text.Replace("from the investor", "to the investor");
                rb_Investor.Text = rb_Investor.Text.Replace("from the investor", "to the investor");

            }
        } //FromParent()

        private void JournalCapitalMovement_Shown(object sender, EventArgs e)
        {
            if (ParentForm1 == null)
            {
                MessageBox.Show("There appears to be an software error as this window should not be called without incoming Transaction details. Please contact the developer.","Journal Capital Movement");
                this.Close();
            }   
            SystemLibrary.PositionFormOverParent(this, ParentForm1);
        } //JournalCapitalMovement_Shown()

        private void rb_Bank_MouseClick(object sender, MouseEventArgs e)
        {
            cb_Account.Enabled = true;

        } //rb_Bank_MouseClick()

        private void rb_Investor_MouseClick(object sender, MouseEventArgs e)
        {
            cb_Account.Enabled = false;

        } //rb_Investor_MouseClick()

        private void bt_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        } //bt_Close_Click()

        private void bt_CreateTransaction_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql = "";

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            if (rb_Investor.Checked)
            {
                mySql = "Exec sp_Transfer_Capital " + TranID.ToString() + ", null ";
            }
            else if (rb_Bank.Checked)
            {
                mySql = "Exec sp_Transfer_Capital " + TranID.ToString() + ", " + cb_Account.SelectedValue.ToString() + " ";
            }

            SystemLibrary.SQLExecute(mySql);
            //SystemLibrary.SQLExecute("Exec sp_Cal_FundNav_RebuildFrom '" + dtp_EffectiveDate.Value.ToString("dd-MMM-yyy") + "'");
            //SystemLibrary.SQLExecute("Exec sp_Profit_Portfolio");

            Cursor.Current = Cursors.Default;

            if (ParentForm1 != null)
            {
                ((ProcessUnallocatedTransactions)ParentForm1).RefreshData();
            }

            MessageBox.Show("Transactions created", bt_CreateTransaction.Text);
            this.Close();

        } //bt_CreateTransaction_Click()


    }
}
