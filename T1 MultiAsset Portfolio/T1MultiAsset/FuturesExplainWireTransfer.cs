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
    public partial class FuturesExplainWireTransfer : Form
    {
        // Local Variables
        DataTable dt_Trades;
        DataTable dt_MarginMovement;
        DataTable dt_Transfer;
        DataTable dt_Fund;
        Form ParentForm1;
        DateTime TransferDate = DateTime.MinValue;
        Decimal diffAmount = 0;
        String TranID;
        String AccountID = "";
        int FundID;

        public FuturesExplainWireTransfer()
        {
            InitializeComponent();
        }

        private void FuturesExplainWireTransfer_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            LoadParentFunds();
            // First time, get the date of the last known transfer, or use today's date.
            // - Relies on ParentFundID from LoadParentFunds()
            if (TransferDate == DateTime.MinValue)
                dtp_TransferDate.Value = FindLastTransferDate(SystemLibrary.f_Today());
            else
                dtp_TransferDate.Value = TransferDate;
            LoadData();

        } //FuturesExplainWireTransfer_Load()

        public void FromParent(Form inForm, int inFund, DateTime inDate)
        {
            ParentForm1 = (Form)inForm;
            FundID = SystemLibrary.SQLSelectInt32("Select ParentFundID From Fund Where FundID = "+ inFund.ToString());
            if (inDate != DateTime.MinValue)
                TransferDate = inDate;

        } //FromParent()

        private void FuturesExplainWireTransfer_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);

            // See if any Trades with Missing Margins
            String myMissingTicker = SystemLibrary.SQLSelectString("Exec sp_MissingFutureMargins");
            if (myMissingTicker.Length > 0)
            {
                FutureMargins frm_FutMargin = new FutureMargins();
                frm_FutMargin.FromParent(this, true);
                frm_FutMargin.Show();
            }
        } // FuturesExplainWireTransfer_Shown()

        private void LoadParentFunds()
        {
            // Local Variables
            String mySql;

            // Only applies where there are sub-funds, otherwise no need.
            mySql = "Select FundID, FundName " +
                    "From Fund " +
                    "Where Active = 'Y' " +
                    "And FundID = ParentFundID " +
                    "And Exists (Select 'x'  " +
                    "			From	Fund f2 " + 
                    "			Where	f2.ParentFundID = Fund.FundID " +
                    "			And		f2.FundID <> f2.ParentFundID " +
                    "		   ) " +
                    "Order by FundName";

            dt_Fund = SystemLibrary.SQLSelectToDataTable(mySql);
            cb_ParentFund.DataSource = dt_Fund;
            cb_ParentFund.DisplayMember = "FundName";
            cb_ParentFund.ValueMember = "FundId";

            DataRow[] dr_Find = dt_Fund.Select("FundId=" + FundID.ToString());
            if (dr_Find.Length > 0)
                cb_ParentFund.SelectedValue = FundID;
            else
            {
                if (dt_Fund.Rows.Count>0)
                    cb_ParentFund.SelectedIndex = 0;
            }


        } //LoadParentFunds()

        private DateTime FindLastTransferDate(DateTime DefaultTransferDate)
        {
            // Local Variables
            String mySql;
            DateTime myDate;
            String ParentFundID;


            if (cb_ParentFund.SelectedItem == null)
                return (SystemLibrary.f_Today());

            ParentFundID = ((DataRowView)(cb_ParentFund.SelectedItem)).Row.ItemArray[0].ToString();


            // Find the Date of the last Transfer
            mySql = "Select Transactions.EffectiveDate " +
                    "From   Transactions, " +
                    "       Accounts " +
                    "Where  Transactions.TranType = 'Transfer' " +
                    "And    Transactions.Source is not null " +
                    "And    Accounts.FundID = " + ParentFundID + " " +
                    "And	Accounts.Future_BrokerID is not null " +
                    "And	Transactions.AccountID = Accounts.AccountID " +
                    "And    EffectiveDate = (Select Max(Transactions.EffectiveDate) as EffectiveDate " +
                    "                        From   Transactions, " +
                    "                               Accounts " +
                    "                        Where  Transactions.TranType = 'Transfer' " +
                    "                        And    Transactions.Source is not null " +
                    "                        And    Accounts.FundID = " + ParentFundID + " " +
                    "                        And	Accounts.Future_BrokerID is not null " +
                    "                        And	Transactions.AccountID = Accounts.AccountID " +
                    "                       )";

            myDate = SystemLibrary.SQLSelectDateTime(mySql, DefaultTransferDate);

            return (myDate);


        } //FindLastTransferDate()

        private void LoadData()
        {
            // Local Varaibles
            String mySql;
            String myDate;
            String ParentFundID;



            if (cb_ParentFund.SelectedItem == null)
                return;
            
            ParentFundID = ((DataRowView)(cb_ParentFund.SelectedItem)).Row.ItemArray[0].ToString();

            lb_TotalMargin.Text = "";
            TranID = "";

            // Find the Details of the Transfer
            mySql = "Select Transactions.EffectiveDate, Transactions.Amount, Transactions.Description, Transactions.TranID, Transactions.AccountID " +
                    "From   Transactions, " +
                    "       Accounts " +
                    "Where  Transactions.TranType = 'Transfer' " +
                    "And    Transactions.Source is not null " +
                    "And    Accounts.FundID = " + ParentFundID + " " +
                    "And	Accounts.Future_BrokerID is not null " +
                    "And	Transactions.AccountID = Accounts.AccountID " +
                    "And    EffectiveDate = '" + dtp_TransferDate.Value.ToString("dd-MMM-yyyy") + "' ";

            dt_Transfer = SystemLibrary.SQLSelectToDataTable(mySql);

            Decimal TransferAmount = 0;
            String TransferDescription = "";
            if (dt_Transfer.Rows.Count > 0)
            {
                dtp_TransferDate.Value = Convert.ToDateTime(dt_Transfer.Rows[0]["EffectiveDate"]);
                TransferAmount = SystemLibrary.ToDecimal(dt_Transfer.Rows[0]["Amount"]);
                TransferDescription = SystemLibrary.ToString(dt_Transfer.Rows[0]["Description"]);
                TranID = SystemLibrary.ToString(SystemLibrary.ToInt32(dt_Transfer.Rows[0]["TranID"]));
                AccountID = SystemLibrary.ToString(SystemLibrary.ToInt32(dt_Transfer.Rows[0]["AccountID"]));
            }
            myDate = dtp_TransferDate.Value.ToString("dd-MMM-yyyy");
            TransferDate = dtp_TransferDate.Value;
            tb_TransferAmount.Text = TransferAmount.ToString("$#,###.00");
            if (TransferAmount < 0)
                tb_TransferAmount.ForeColor = Color.Red;
            else
                tb_TransferAmount.ForeColor = Color.Green;
            tb_TransferAmount.Tag = TransferDescription; // Was thinking mouseover toolbartip

            // dt_Trades
            mySql = "SELECT     Trade.TradeID, Trade.TradeDate, Fund.ShortName AS [Sub-Fund], CASE WHEN Quantity > 0 THEN CONVERT(Varchar, CAST(Quantity AS int), 2) ELSE '' END AS Buy, " +
                    "                      CASE WHEN Quantity < 0 THEN CONVERT(Varchar, CAST(- Quantity AS int), 2) ELSE '' END AS Sell, Securities.Security_Name AS [Contract Description],  " +
                    "                      Trade.Price AS [Trade Price], EOD_Prices.Close_Price as [Close Price], Trade.crncy AS CC, Trade.Commission AS [Comm & Clearing Fee], Transactions.Amount AS [Expected Margin Movement],  " +
                    "					  (EOD_Prices.Close_Price-Trade.Price)*Trade.Pos_Mult_Factor*Trade.Quantity as [Trade Profit], " +
                    "					  (Transactions.Amount + Trade.Commission + -((EOD_Prices.Close_Price-Trade.Price)*Trade.Pos_Mult_Factor*Trade.Quantity)) as [Expected Extra Cash Required] " +
                    "FROM         Trade INNER JOIN " +
                    "                      EOD_Prices ON Trade.BBG_Ticker = EOD_Prices.BBG_Ticker AND ISNULL(Trade.ID_BB_UNIQUE, EOD_Prices.ID_BB_UNIQUE) = EOD_Prices.ID_BB_UNIQUE AND Trade.TradeDate = EOD_Prices.EffectiveDate INNER JOIN " +
                    "                      Fund ON Trade.FundID = Fund.FundID INNER JOIN " +
                    "                      Securities ON Trade.BBG_Ticker = Securities.BBG_Ticker AND ISNULL(Trade.ID_BB_UNIQUE, Securities.ID_BB_UNIQUE) = Securities.ID_BB_UNIQUE INNER JOIN " +
                    "                      Accounts ON Fund.ParentFundID = Accounts.FundID LEFT OUTER JOIN " +
                    "                      Transactions ON Accounts.AccountID = Transactions.AccountID AND Trade.TradeID = Transactions.TradeID AND Trade.FundID = Transactions.FundID " +
                    "WHERE     Trade.TradeID IN (SELECT     Transactions_1.TradeID " +
                    "                            FROM       Transactions AS Transactions_1 INNER JOIN " +
                    "                                       Fund AS Fund_1 ON Transactions_1.FundID = Fund_1.FundID INNER JOIN " +
                    "                                       Accounts AS Accounts_1 ON Transactions_1.AccountID = Accounts_1.AccountID AND Fund_1.ParentFundID = Accounts_1.FundID " +
                    "                            WHERE      Transactions_1.EffectiveDate = '" + myDate + "' " +
                    "							AND		   Fund_1.ParentFundID = " + ParentFundID + " " +
                    "							AND		   Transactions_1.TranType = 'Transfer' " +
                    "							AND        Transactions_1.Description = 'Futures Margin' " +
                    "							AND        Accounts_1.Future_BrokerID IS NOT NULL " +
                    "						   )  " +
                    "AND		isNull(Securities.Security_Typ2,'') = 'Future' " +
                    "AND		Transactions.TranType = 'Transfer' " +
                    "AND		Transactions.JournalID IS NOT NULL " +
                    "AND		Transactions.Description = 'Futures Margin' " +
                    "AND		Accounts.Future_BrokerID IS NOT NULL " +
                    "ORDER BY [Sub-Fund], Trade.TradeID ";

            dt_Trades = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_Trades.DataSource = dt_Trades;

            //dg_Trades.Columns["TradeID"].Visible = false;
            //dg_Trades.Columns["TradeDate"].DefaultCellStyle.Format = "dd-MMM-yyyy";
            SystemLibrary.SetDataGridView(dg_Trades);


    //        "And		Transactions.Description in ('Futures Margin','Day Profit + Future comm and charges','Unbalanced Future Transfer pro-rata allocation') " +
            mySql = "Select  Transactions.EffectiveDate, Fund.ShortName as [Sub-Fund], Transactions.crncy, Transactions.Description, Sum(Amount) as Amount, Transactions.AccountID " +
                    "From	Transactions, " +
                    "		Fund, " +
                    "		Accounts " +
                    "Where	Transactions.EffectiveDate = '" + myDate + "' " +
                    "And		Fund.ParentFundID = " + ParentFundID + " " +
                    "And		Transactions.FundID = Fund.FundID " +
                    "And		Fund.ParentFundID  <> Fund.FundID " +
                    "And		Transactions.TranType = 'Transfer' " +
                    "And		Transactions.AccountID = Accounts.AccountID " +
                    "And		Accounts.FundID = Fund.ParentFundID " +
                    "And		Accounts.Future_BrokerID is not null " +
                    "Group By Transactions.EffectiveDate, Fund.ShortName, Transactions.crncy, Transactions.Description, Transactions.AccountID " +
                    "Order By Transactions.EffectiveDate, Fund.ShortName, Transactions.crncy, Transactions.Description, Transactions.AccountID ";

            dt_MarginMovement = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_MarginMovement.DataSource = dt_MarginMovement;

            //dg_MarginMovement.Columns["EffectiveDate"].DefaultCellStyle.Format = "dd-MMM-yyyy";
            //SystemLibrary.SetColumn(dg_MarginMovement, "Amount");
            SystemLibrary.SetDataGridView(dg_MarginMovement);

            if (AccountID.Length==0)
                 AccountID = SystemLibrary.ToString(dt_MarginMovement.Compute("Min(AccountID)", ""));

            Decimal TotalTransfer = SystemLibrary.ToDecimal(dt_MarginMovement.Compute("Sum(Amount)", ""));
            String TotalMargin = SystemLibrary.ToDecimal(dt_MarginMovement.Compute("Sum(Amount)", "Description='Futures Margin'")).ToString("$#,###.00");
            String TotalPL = SystemLibrary.ToDecimal(dt_MarginMovement.Compute("Sum(Amount)", "Description='Day Profit + Future comm and charges'")).ToString("$#,###.00");
            Decimal TotalAdj = SystemLibrary.ToDecimal(dt_MarginMovement.Compute("Sum(Amount)", "Description='Unbalanced Future Transfer pro-rata allocation'"));
            lb_TotalMargin.Text = "Margin=" + TotalMargin + ", Profit=" + TotalPL + ".";
            if (TotalAdj != 0)
                lb_TotalMargin.Text = lb_TotalMargin.Text + ", Adjustments="+TotalAdj.ToString("$#,###.00");

            tb_Expected.Text = TotalTransfer.ToString("$#,###.00");
            if (TotalTransfer < 0)
                tb_Expected.ForeColor = Color.Red;
            else
                tb_Expected.ForeColor = Color.Green;

            diffAmount = TransferAmount-TotalTransfer;
            tb_Diff.Text = diffAmount.ToString("$#,###.00");
            if (diffAmount < 0)
                tb_Diff.ForeColor = Color.Red;
            else
                tb_Diff.ForeColor = Color.Green;
            if (diffAmount != 0 ) //&& dt_Trades.Rows.Count > 0)
            {
                bt_recalc_margins.Enabled = true;
                bt_JournalDiff.Enabled = true;
            }
            else
            {
                bt_recalc_margins.Enabled = false;
                bt_JournalDiff.Enabled = false;
            }
        } //LoadData()

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            LoadData();
        } // bt_Refresh_Click()

        private void bt_recalc_margins_Click(object sender, EventArgs e)
        {
            // Local Variables
            String myDate;


            if (dt_Trades != null && dt_Trades.Rows.Count > 0)
            {
                myDate = Convert.ToDateTime(dt_Trades.Rows[0]["TradeDate"]).ToString("dd-MMM-yyyy");
                SystemLibrary.SQLExecute("Exec sp_ReapplyMargin " + FundID.ToString() + ", '" + myDate + "' ");
                LoadData();
                MessageBox.Show("Completed", bt_recalc_margins.Text);
            }

        } //bt_recalc_margins_Click()

        private void bt_JournalDiff_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;
            DateTime NextTradeDate;
            String Explanation;
            Boolean FoundMatch = false;

            if (TransferDate == DateTime.MinValue)
                return;

            NextTradeDate = SystemLibrary.SQLSelectDateTime("Select dbo.f_NextTradingDate ('" + TransferDate.ToString("dd-MMM-yyyy") + "')", SystemLibrary.f_Today().AddDays(1));
            Explanation = "Next Trading Date of '" + NextTradeDate.ToString("dd-MMM-yyyy") + "'";

            // See if there is a match for this amount on another day with issues
            mySql = "Exec sp_GetUnallocatedAmounts ";
            DataRow[] dr = SystemLibrary.SQLSelectToDataTable(mySql).Select("FundID=" + FundID.ToString() + " and AccountID=" + AccountID + " and EffectiveDate <> '" + TransferDate.ToString("dd-MMM-yyyy") + "'"); //+ " and [Unallocated Amount] = " + (-1.0m*diffAmount).ToString());
            if (dr.Length == 1)
            {
                // Allocate this to a forward date that has the same difference.
                DateTime myDate = Convert.ToDateTime(dr[0]["EffectiveDate"]);
                Decimal UnallocatedAmount = SystemLibrary.ToDecimal(dr[0]["Unallocated Amount"]);
                if (DateTime.Compare(myDate, TransferDate) < 0)
                {
                    if (MessageBox.Show(this, "The System found a Journal required on an earlier date of '" + myDate.ToString("dd-MMM-yyyy") + "'.\r\n\r\n" +
                                              "It is suggested you CANCEL this 'Journal' and process the earlier date first.\r\n" +
                                              "This can then move the earlier difference to help clear both days.\r\n\r\n" +
                                              "Do you want to CANCEL this Journal and fix the earlier date?", bt_JournalDiff.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        MessageBox.Show("Journal not created.\r\n\r\nChange the Date to '" + myDate.ToString("dd-MMM-yyyy") + "' and process that day first.", bt_JournalDiff.Text);
                        return;
                    }
                }
                else if (UnallocatedAmount == -diffAmount)
                {
                    // Dont ask the user, just move the date.
                    NextTradeDate = myDate;
                    Explanation = "Next [Difference Date] of '" + NextTradeDate.ToString("dd-MMM-yyyy") + "'";
                    FoundMatch = true;
                }
                else
                {
                    if (MessageBox.Show(this, "The System found a Journal required on the Later Date of '" + myDate.ToString("dd-MMM-yyyy") + "'.\r\n\r\n" +
                                              "It is NOT for an equal and opposite amount, but is for " + UnallocatedAmount.ToString("$#,###.00") + ".\r\n\r\n" +
                                              "It may pay to roll this difference forward NOT to the " + Explanation + ", but to the later date of '" + myDate.ToString("dd-MMM-yyyy") + "' to help clear both days.\r\n\r\n" +
                                              "Do you want to use the Later Date of '" + myDate.ToString("dd-MMM-yyyy") + "'?", bt_JournalDiff.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        NextTradeDate = myDate;
                        Explanation = "Next difference Date of '" + NextTradeDate.ToString("dd-MMM-yyyy") + "'";
                        FoundMatch = true;
                    }

                }
            }

            String myMessage = "You are about to Journal the Difference of " + tb_Diff.Text + ".\r\n\r\n" +
                                      "This will create a new Journal Entry that will move these Funds back to the corresponding 'Cash' account for '" + TransferDate.ToString("dd-MMM-yyyy") + "'\r\n" +
                                      "and then reallocate on the " + Explanation + ".\r\n" +
                                      "This will be done on a pro-rata basic between sub-Fund.\r\n\r\n";
            if (!FoundMatch)
            {
                myMessage = myMessage +
                            "It sometimes pays to wait for a later exception before processing so that they match off.\r\n\r\n" +
                            "Do you really want to do this?";
            }
            else
            {
                myMessage = myMessage + "Continue?";
            }

            if (MessageBox.Show(this, myMessage, bt_JournalDiff.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                MessageBox.Show("Journal not created.", bt_JournalDiff.Text);
            }
            else
            {
                mySql = "Exec sp_ProRataFuturesTransfer " + FundID.ToString() + ", '" + TransferDate.ToString("dd-MMM-yyyy") + "', '" + NextTradeDate.ToString("dd-MMM-yyyy") + "', " + diffAmount.ToString() + ", ";
                if (TranID == "")
                    mySql = mySql + "null";
                else
                    mySql = mySql + TranID;

                SystemLibrary.SQLExecute(mySql);
                LoadData();
                MessageBox.Show("Journal Created", bt_JournalDiff.Text);
            }
        } // bt_JournalDiff_Click()

        private void bt_Print_Click(object sender, EventArgs e)
        {
            SystemLibrary.PrintScreen(this);

        } //bt_Print_Click()

        private void bt_AlterMarginAmounts_Click(object sender, EventArgs e)
        {
            FutureMargins frm_FutMargin = new FutureMargins();
            frm_FutMargin.FromParent(this, true);
            frm_FutMargin.Show();

        } //bt_AlterMarginAmounts_Click()

    }
}
