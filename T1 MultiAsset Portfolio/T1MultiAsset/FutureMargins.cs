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
    public partial class FutureMargins : Form
    {
        // Global Variables
        public Form ParentForm1;
        Boolean CheckForMissingAtStartup = false;
        DataTable dt_FutureMargins;

        public FutureMargins()
        {
            InitializeComponent();
        }

        private void FutureMargins_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            //LoadData();

        } //FutureMargins_Load()

        private void FutureMargins_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);
            LoadData();
        } //FutureMargins_Shown()

        public void FromParent(Form inForm, Boolean inCheckForMissingAtStartup)
        {
            ParentForm1 = inForm;
            CheckForMissingAtStartup = inCheckForMissingAtStartup;
        } // FromParent()

        private void LoadData()
        {
            dt_FutureMargins = SystemLibrary.SQLSelectToDataTable("Select * from FutureMargins Order By FromDate, BBG_Ticker, ToDate");

            dg_FutureMargins.DataSource = dt_FutureMargins;
            SystemLibrary.SetDataGridView(dg_FutureMargins);

            if (CheckForMissingAtStartup)
            {
                CheckForMissingAtStartup = false;
                bt_Check_Click(null, null);
            }

        } //LoadData()

        private void bt_ProcessOutstandingTrades_Click(object sender, EventArgs e)
        {
            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            // See if any records need saving first
            if (SystemLibrary.AreRowsAltered(dt_FutureMargins))
            {
                if (MessageBox.Show("You have not yet Saved your changes.\r\n\r\nDo you wish to continue?", bt_ProcessOutstandingTrades.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }

            // Process any outstanding records
            SystemLibrary.SQLExecute("Exec sp_CreateMarginTransactionForTradeOutstanding");

            // See if any undefined futures
            bt_Check_Click(null, null);
           
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Completed", bt_ProcessOutstandingTrades.Text);

        } //bt_ProcessOutstandingTrades_Click()

        private void bt_Check_Click(object sender, EventArgs e)
        {
            // Local Variables
            String myMessage = "";
            DataTable dt_Missing = SystemLibrary.SQLSelectToDataTable("Exec sp_MissingFutureMargins");

            if (dt_Missing.Rows.Count == 0)
            {
                // See if User pressed the button or internally run.
                if (sender != null)
                    MessageBox.Show("No Trades found without a corresponding defined Future Margin", bt_Check.Text);
                return;
            }
            else
            {
                myMessage = "Found Trades for the following\r\n\r\n";
                for (int i = 0; i < dt_Missing.Rows.Count; i++)
                {
                    myMessage = myMessage + "Ticker='" + dt_Missing.Rows[i]["BBG_Ticker"].ToString() +
                                            "',ID_BB_UNIQUE='" + dt_Missing.Rows[i]["ID_BB_UNIQUE"].ToString() +
                                            "',From='" + Convert.ToDateTime(dt_Missing.Rows[i]["FromTradeDate"]).ToString("dd-MMM-yyyy") +
                                            "'\r\n";
                }

                myMessage = myMessage + "\r\nDo you want these added to the Margins list?";
                if (MessageBox.Show(myMessage, bt_Check.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < dt_Missing.Rows.Count; i++)
                    {
                        DataRow myRow = dt_FutureMargins.Rows.Add();
                        myRow["BBG_Ticker"] = dt_Missing.Rows[i]["BBG_Ticker"];
                        myRow["ID_BB_UNIQUE"] = dt_Missing.Rows[i]["ID_BB_UNIQUE"];
                        myRow["FromDate"] = dt_Missing.Rows[i]["FromTradeDate"];
                        myRow["crncy"] = dt_Missing.Rows[i]["crncy"];
                        myRow["Amount"] = 0;
                    }
                    MessageBox.Show("Make sure you set the correct Margin Amount.");
                }
            }

        } //FutureMargins_Load()

        private void bt_Save_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;
            Boolean FoundAltered = SystemLibrary.AreRowsAltered(dt_FutureMargins);
            String ToDate;

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            if (!ValidateRecords())
                return;

            for (int i = 0; i < dt_FutureMargins.Rows.Count; i++)
            {
                switch (dt_FutureMargins.Rows[i].RowState)
                {
                    case DataRowState.Added:
                        if (dt_FutureMargins.Rows[i]["ToDate"] == DBNull.Value)
                            ToDate = "null";
                        else
                            ToDate = "'" + Convert.ToDateTime(dt_FutureMargins.Rows[i]["ToDate"]).ToString("dd-MMM-yyyy") + "'";
                        mySql = "Insert into FutureMargins (BBG_Ticker, ID_BB_UNIQUE, FromDate, ToDate, crncy, Amount) " +
                                "Values ('" + dt_FutureMargins.Rows[i]["BBG_Ticker"].ToString() + "', " +
                                "        '" + dt_FutureMargins.Rows[i]["ID_BB_UNIQUE"].ToString() + "', " +
                                "        '" + Convert.ToDateTime(dt_FutureMargins.Rows[i]["FromDate"]).ToString("dd-MMM-yyyy") + "', " + ToDate + ", " +
                                "        '" + dt_FutureMargins.Rows[i]["crncy"].ToString() + "', " +
                                "         " + dt_FutureMargins.Rows[i]["Amount"].ToString() + ")";
                        SystemLibrary.SQLExecute(mySql);
                        break;
                    case DataRowState.Deleted:
                        // Delete the Old record
                        if (dt_FutureMargins.Rows[i]["ToDate", DataRowVersion.Original] == DBNull.Value)
                            ToDate = "is null";
                        else
                            ToDate = "='" + Convert.ToDateTime(dt_FutureMargins.Rows[i]["ToDate", DataRowVersion.Original]).ToString("dd-MMM-yyyy") + "'";

                        mySql = "Delete From FutureMargins " +
                                "Where BBG_Ticker='" + dt_FutureMargins.Rows[i]["BBG_Ticker", DataRowVersion.Original].ToString() + "' " +
                                "And   ID_BB_UNIQUE='" + dt_FutureMargins.Rows[i]["ID_BB_UNIQUE", DataRowVersion.Original].ToString() + "' " +
                                "And   FromDate='" + Convert.ToDateTime(dt_FutureMargins.Rows[i]["FromDate", DataRowVersion.Original]).ToString("dd-MMM-yyyy") + "' " +
                                "And   ToDate " + ToDate + " " +
                                "And   crncy='" + dt_FutureMargins.Rows[i]["crncy", DataRowVersion.Original].ToString() + "' " +
                                "And   Amount=" + dt_FutureMargins.Rows[i]["Amount", DataRowVersion.Original].ToString();
                        SystemLibrary.SQLExecute(mySql);

                        break;
                    case DataRowState.Modified:
                        // Delete the Old record
                        if (dt_FutureMargins.Rows[i]["ToDate", DataRowVersion.Original] == DBNull.Value)
                            ToDate = "is null";
                        else
                            ToDate = "='" + Convert.ToDateTime(dt_FutureMargins.Rows[i]["ToDate", DataRowVersion.Original]).ToString("dd-MMM-yyyy") + "'";

                        mySql = "Delete From FutureMargins " +
                                "Where BBG_Ticker='" + dt_FutureMargins.Rows[i]["BBG_Ticker", DataRowVersion.Original].ToString() + "' " +
                                "And   ID_BB_UNIQUE='" + dt_FutureMargins.Rows[i]["ID_BB_UNIQUE", DataRowVersion.Original].ToString() + "' " +
                                "And   FromDate='" + Convert.ToDateTime(dt_FutureMargins.Rows[i]["FromDate", DataRowVersion.Original]).ToString("dd-MMM-yyyy") + "' " +
                                "And   ToDate " + ToDate + " " +
                                "And   crncy='" + dt_FutureMargins.Rows[i]["crncy", DataRowVersion.Original].ToString() + "' " +
                                "And   Amount=" + dt_FutureMargins.Rows[i]["Amount", DataRowVersion.Original].ToString();
                        SystemLibrary.SQLExecute(mySql);

                        // Insert the new record
                        if (dt_FutureMargins.Rows[i]["ToDate"] == DBNull.Value)
                            ToDate = "null";
                        else
                            ToDate = "'" + Convert.ToDateTime(dt_FutureMargins.Rows[i]["ToDate"]).ToString("dd-MMM-yyyy") + "'";

                        mySql = "Insert into FutureMargins (BBG_Ticker, ID_BB_UNIQUE, FromDate, ToDate, crncy, Amount) " +
                                "Values ('" + dt_FutureMargins.Rows[i]["BBG_Ticker"].ToString() + "', " +
                                "        '" + dt_FutureMargins.Rows[i]["ID_BB_UNIQUE"].ToString() + "', " +
                                "        '" + Convert.ToDateTime(dt_FutureMargins.Rows[i]["FromDate"]).ToString("dd-MMM-yyyy") + "', " + ToDate + ", " +
                                "        '" + dt_FutureMargins.Rows[i]["crncy"].ToString() + "', " +
                                "         " + dt_FutureMargins.Rows[i]["Amount"].ToString() + ")";
                        SystemLibrary.SQLExecute(mySql);
                        break;
                    default:
                        // No change
                        break;
                }
            }

            // Reload the data
            LoadData();

            if (FoundAltered)
            {
                if (MessageBox.Show("Do you wish to also [Process Oustanding Trades]?", bt_ProcessOutstandingTrades.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bt_ProcessOutstandingTrades_Click(null, null);
                }
            }

            Cursor.Current = Cursors.Default;
            MessageBox.Show("Completed.", bt_Save.Text);

        } //bt_Save_Click()

        private Boolean ValidateRecords()
        {
            // Local Variables
            String ID_BB_UNIQUE = "";

            for (int i = 0; i < dt_FutureMargins.Rows.Count; i++)
            {
                switch (dt_FutureMargins.Rows[i].RowState)
                {
                    case DataRowState.Added:
                    case DataRowState.Modified:
                       if (SystemLibrary.ToString(dt_FutureMargins.Rows[i]["BBG_Ticker"]).Length==0)
                       {
                           MessageBox.Show("Must supply a Ticker","Validation");
                           return (false);
                       }
                       else if (SystemLibrary.ToString(dt_FutureMargins.Rows[i]["ID_BB_UNIQUE"]).Length == 0)
                       {
                           MessageBox.Show("Must supply a valid ID_BB_UNIQUE.","Validation");
                           return (false);
                       }
                       else if (dt_FutureMargins.Rows[i]["FromDate"] == DBNull.Value)
                       {
                           MessageBox.Show("Must supply a From Date.","Validation");
                           return (false);
                       }
                       else if (dt_FutureMargins.Rows[i]["crncy"] == DBNull.Value)
                       {
                           MessageBox.Show("Must supply a valid Currency.","Validation");
                           return (false);
                       }
                       else if (SystemLibrary.ToDecimal(dt_FutureMargins.Rows[i]["Amount"]) <= 0)
                       {
                           MessageBox.Show("Margin Amount must be > 0.","Validation");
                           return (false);
                       }
                       ID_BB_UNIQUE = SystemLibrary.SQLSelectString("Select ID_BB_UNIQUE from Securities where BBG_Ticker = '" +dt_FutureMargins.Rows[i]["BBG_Ticker"].ToString() + "'");
                       if (ID_BB_UNIQUE != SystemLibrary.ToString(dt_FutureMargins.Rows[i]["ID_BB_UNIQUE"]))
                       {
                           MessageBox.Show("Must supply a valid ID_BB_UNIQUE for '" + SystemLibrary.ToString(dt_FutureMargins.Rows[i]["BBG_Ticker"]) +
                                            "'.\r\nThe Securties record has a value of '" + ID_BB_UNIQUE + "'", "Validation");
                           return (false);
                       }
                       break;
                }
            }
            return (true);
        }

    }
}
