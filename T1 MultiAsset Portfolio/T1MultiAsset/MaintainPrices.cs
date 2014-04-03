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
    public partial class MaintainPrices : Form
    {
        // Public variables
        public Form1 ParentForm1;
        public DataTable dt_Prices;
        DateTime StartDate = SystemLibrary.f_Today();
        DateTime MinTradeDate = SystemLibrary.f_Today();
        Boolean FoundChanges = false;
        public object LastValue;
        
        public MaintainPrices()
        {
            InitializeComponent();
            lb_Message.Text = "";
        }
    
        public void FromParent(Form inForm, String inBBG_Ticker)
        {
            ParentForm1 = (Form1)inForm;
            tb_BBG_Ticker.Text = inBBG_Ticker.Trim().ToUpper();

        } // FromParent()

        private void MaintainPrices_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);
            if (tb_BBG_Ticker.Text.Length > 0)
            {
                LoadPrices();
            }

        } //MaintainPrices_Shown()

        public void LoadPrices()
        {
            // Local Variables
            String mySql;
            String BBG_Ticker = tb_BBG_Ticker.Text.Trim().ToUpper();

            if (BBG_Ticker.Length == 0)
                return;

            try
            {
                tb_BBG_Ticker.Text = BBG_Ticker;
                mySql = "Select Locked, EffectiveDate, Close_Price, VWAP, BBG_Ticker, ID_BB_UNIQUE, ID_BB_GLOBAL, Pos_Mult_Factor From EOD_Prices Where BBG_Ticker = '" + BBG_Ticker + "' Order By EffectiveDate";
                dt_Prices = SystemLibrary.SQLSelectToDataTable(mySql);
                dg_Prices.DataSource = dt_Prices;
                SystemLibrary.SetDataGridView(dg_Prices);

                // Now set Locked as a Y/N dropdownlist box.
                dg_Prices.Columns.Remove("Locked");
                DataGridViewCheckBoxColumn Locked = new DataGridViewCheckBoxColumn();
                Locked.HeaderText = "Locked";
                Locked.FalseValue = "N";
                Locked.TrueValue = "Y";
                Locked.IndeterminateValue = "N";
                Locked.Name = "Locked";
                Locked.DataPropertyName = "Locked";
                dg_Prices.Columns.Insert(0, Locked);

                //Scroll to the last row.
                dg_Prices.FirstDisplayedScrollingRowIndex = dg_Prices.RowCount - 1;
                dg_Prices.Rows[dg_Prices.RowCount - 1].Selected = true;

                mySql = "Select Min(TradeDate) From Trade Where BBG_Ticker = '" + BBG_Ticker + "' ";
                MinTradeDate = SystemLibrary.SQLSelectDateTime(mySql, MinTradeDate);
            }
            catch
            {
                dt_Prices.Rows.Clear();
            }

        } //LoadPrices()

        private void bt_Request_Click(object sender, EventArgs e)
        {
            // Check to make sure rows that have been chamnged have been saved.
            if (SystemLibrary.AreRowsAltered(dt_Prices))
            {
                if(MessageBox.Show("You have altered some of this data.\r\nThis request will loose your changes.\r\n\r\n" +
                                   "Do you wish to continue?","Request EOD Prices",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2)==DialogResult.No)
                    return;
            }

            // Now load the ticker.
            LoadPrices();

        } //bt_Request_Click()

        private void bt_Save_Click(object sender, EventArgs e)
        {
            // Local Variables
            String BBG_Ticker = "";
            String mySql;
            int myRows;

            // Eliminate the last row being new row issues
            dg_Prices.AllowUserToAddRows = false;

            // Get the Earliest Date
            for (int i = 0; i < dt_Prices.Rows.Count; i++)
            {
                if (dt_Prices.Rows[i].RowState == DataRowState.Deleted)
                {
                    BBG_Ticker = SystemLibrary.ToString(dt_Prices.Rows[0]["BBG_Ticker", DataRowVersion.Original]);
                    FoundChanges = true;
                    DateTime EffectiveDate1 = Convert.ToDateTime(dt_Prices.Rows[i]["EffectiveDate", DataRowVersion.Original]);
                    if (EffectiveDate1 < StartDate)
                        StartDate = EffectiveDate1;
                }
                else if (dt_Prices.Rows[i].RowState != DataRowState.Unchanged)
                {
                    BBG_Ticker = SystemLibrary.ToString(dt_Prices.Rows[0]["BBG_Ticker"]);
                    FoundChanges = true;
                    if (dt_Prices.Rows[i]["EffectiveDate"] == DBNull.Value || dt_Prices.Rows[i]["BBG_Ticker"] == DBNull.Value || dt_Prices.Rows[i]["POS_Mult_Factor"] == DBNull.Value)
                    {
                        MessageBox.Show("You must supply a valid date, Ticker, and Position Multiplication factor", "Save");
                        dg_Prices.AllowUserToAddRows = true;
                        return;
                    }
                    DateTime EffectiveDate = Convert.ToDateTime(dt_Prices.Rows[i]["EffectiveDate"]);
                    if (EffectiveDate<StartDate)
                        StartDate = EffectiveDate;
                }
            }

            // Now Save the Changed rows back to the database
            if (FoundChanges)
            {
                for (int i = 0; i < dt_Prices.Rows.Count; i++)
                {
                    switch (dt_Prices.Rows[i].RowState)
                    {
                        case DataRowState.Added:
                            BBG_Ticker = SystemLibrary.ToString(dt_Prices.Rows[0]["BBG_Ticker"]);
                            mySql = "Insert into EOD_Prices (Locked, EffectiveDate, Close_Price, VWAP, BBG_Ticker, ID_BB_UNIQUE, ID_BB_GLOBAL, Pos_Mult_Factor) values (" +
                                    "      '" + SystemLibrary.Bool_To_YN(SystemLibrary.YN_To_Bool(SystemLibrary.ToString(dt_Prices.Rows[i]["Locked"]))) + "', " +
                                    "      '" + Convert.ToDateTime(dt_Prices.Rows[i]["EffectiveDate"]).ToString("dd-MMM-yyyy") + "', " +
                                    "      " + SystemLibrary.ToString(dt_Prices.Rows[i]["Close_Price"]) + ", " +
                                    "      " + SystemLibrary.ToString(dt_Prices.Rows[i]["VWAP"]) + ", " +
                                    "      '" + SystemLibrary.ToString(dt_Prices.Rows[i]["BBG_Ticker"]) + "', " +
                                    "      '" + SystemLibrary.ToString(dt_Prices.Rows[i]["ID_BB_UNIQUE"]) + "', " +
                                    "      '" + SystemLibrary.ToString(dt_Prices.Rows[i]["ID_BB_GLOBAL"]) + "', " +
                                    "      " + SystemLibrary.ToString(dt_Prices.Rows[i]["Pos_Mult_Factor"]) + ") ";
                            myRows = SystemLibrary.SQLExecute(mySql);
                            break;
                        case DataRowState.Deleted:
                            BBG_Ticker = SystemLibrary.ToString(dt_Prices.Rows[0]["BBG_Ticker", DataRowVersion.Original]);
                            mySql = "Delete From EOD_Prices " +
                                    "Where BBG_Ticker='" + SystemLibrary.ToString(dt_Prices.Rows[i]["BBG_Ticker", DataRowVersion.Original]) + "' " +
                                    "And   ID_BB_UNIQUE='" + SystemLibrary.ToString(dt_Prices.Rows[i]["ID_BB_UNIQUE", DataRowVersion.Original]) + "' " +
                                    "And   ID_BB_GLOBAL='" + SystemLibrary.ToString(dt_Prices.Rows[i]["ID_BB_GLOBAL", DataRowVersion.Original]) + "' " +
                                    "And   EffectiveDate='" + Convert.ToDateTime(dt_Prices.Rows[i]["EffectiveDate", DataRowVersion.Original]).ToString("dd-MMM-yyyy") + "' ";
                            myRows = SystemLibrary.SQLExecute(mySql);
                            break;
                        case DataRowState.Modified:
                            BBG_Ticker = SystemLibrary.ToString(dt_Prices.Rows[0]["BBG_Ticker"]);
                            // Remove old row
                            mySql = "Delete From EOD_Prices " +
                                    "Where BBG_Ticker='" + SystemLibrary.ToString(dt_Prices.Rows[i]["BBG_Ticker", DataRowVersion.Original]) + "' " +
                                    "And   ID_BB_UNIQUE='" + SystemLibrary.ToString(dt_Prices.Rows[i]["ID_BB_UNIQUE", DataRowVersion.Original]) + "' " +
                                    "And   ID_BB_GLOBAL='" + SystemLibrary.ToString(dt_Prices.Rows[i]["ID_BB_GLOBAL", DataRowVersion.Original]) + "' " +
                                    "And   EffectiveDate='" + Convert.ToDateTime(dt_Prices.Rows[i]["EffectiveDate", DataRowVersion.Original]).ToString("dd-MMM-yyyy") + "' ";
                            myRows = SystemLibrary.SQLExecute(mySql);

                            // Add New row
                            mySql = "Insert into EOD_Prices (Locked, EffectiveDate, Close_Price, VWAP, BBG_Ticker, ID_BB_UNIQUE, ID_BB_GLOBAL, Pos_Mult_Factor) values (" +
                                    "      '" + SystemLibrary.Bool_To_YN(SystemLibrary.YN_To_Bool(SystemLibrary.ToString(dt_Prices.Rows[i]["Locked"]))) + "', " +
                                    "      '" + Convert.ToDateTime(dt_Prices.Rows[i]["EffectiveDate"]).ToString("dd-MMM-yyyy") + "', " +
                                    "      " + SystemLibrary.ToString(dt_Prices.Rows[i]["Close_Price"]) + ", " +
                                    "      " + SystemLibrary.ToString(dt_Prices.Rows[i]["VWAP"]) + ", " +
                                    "      '" + SystemLibrary.ToString(dt_Prices.Rows[i]["BBG_Ticker"]) + "', " +
                                    "      '" + SystemLibrary.ToString(dt_Prices.Rows[i]["ID_BB_UNIQUE"]) + "', " +
                                    "      '" + SystemLibrary.ToString(dt_Prices.Rows[i]["ID_BB_GLOBAL"]) + "', " +
                                    "      " + SystemLibrary.ToString(dt_Prices.Rows[i]["Pos_Mult_Factor"]) + ") ";
                            myRows = SystemLibrary.SQLExecute(mySql);
                            break;
                    }
                }
                dt_Prices.AcceptChanges();

                // Only need to recalc P&L when it has impact.
                if (StartDate < MinTradeDate)
                    StartDate = MinTradeDate;

                lb_Message.Text = "On Exit: Profit will be recalculated from " + StartDate.ToString("dd-MMM-yyyy");

                tb_RebuildProfit.Enabled = true;
            }

            dg_Prices.AllowUserToAddRows = true;
            MessageBox.Show("Prices saved for " + BBG_Ticker, "Save");

        } //bt_Save_Click()

        private void MaintainPrices_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FoundChanges)
            {
                RebuildProfit();
            }

        } //MaintainPrices_FormClosing()

        private void RebuildProfit()
        {
            if (StartDate.CompareTo(SystemLibrary.f_Today()) > -1)
                SystemLibrary.SQLExecute("Exec sp_Calc_Profit_RebuildFrom null");
            else
                SystemLibrary.SQLExecute("Exec sp_Calc_Profit_RebuildFrom '" + StartDate.ToString("dd-MMM-yyyy") + "' ");
            SystemLibrary.SQLExecute("Exec sp_SOD_Positions ");

        } //RebuildProfit()

        private void tb_RebuildProfit_Click(object sender, EventArgs e)
        {
            RebuildProfit();
            MessageBox.Show("Profit Rebuilt from " + StartDate.ToString("dd-MMM-yyyy"), "Rebuild profit");
            StartDate = SystemLibrary.f_Today();
            FoundChanges = false;
            lb_Message.Text = "";
            tb_RebuildProfit.Enabled = false;

        } //tb_RebuildProfit_Click()

        private void dg_Prices_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (dg_Prices.Rows.Count < 1)
            {
                dg_Prices.CurrentRow.Cells["ID_BB_UNIQUE"].Value = "";
                dg_Prices.CurrentRow.Cells["ID_BB_GLOBAL"].Value = "";
                dg_Prices.CurrentRow.Cells["Pos_Mult_Factor"].Value = 1;
            }
            else
            {
                // Get details from first row
                dg_Prices.CurrentRow.Cells["BBG_Ticker"].Value = dg_Prices.Rows[0].Cells["BBG_Ticker"].Value;
                dg_Prices.CurrentRow.Cells["ID_BB_UNIQUE"].Value = dg_Prices.Rows[0].Cells["ID_BB_UNIQUE"].Value;
                dg_Prices.CurrentRow.Cells["ID_BB_GLOBAL"].Value = dg_Prices.Rows[0].Cells["ID_BB_GLOBAL"].Value;
                dg_Prices.CurrentRow.Cells["Pos_Mult_Factor"].Value = dg_Prices.Rows[0].Cells["Pos_Mult_Factor"].Value;
            }

            //Locked, EffectiveDate, Close_Price, VWAP, BBG_Ticker, ID_BB_UNIQUE, ID_BB_GLOBAL, Pos_Mult_Factor

        }

        private void tb_BBG_Ticker_KeyDown(object sender, KeyEventArgs e)
        {
            SystemLibrary.BBGOnPreviewKeyDown(sender, e.KeyCode.ToString());

        } //tb_BBG_Ticker_KeyDown()

        private void dg_Prices_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            LastValue = dg_Prices[e.ColumnIndex, e.RowIndex].Value;

        } //dg_Prices_CellBeginEdit()

        private void dg_Prices_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dg_Prices.Columns[e.ColumnIndex].Name == "EffectiveDate")
            {
                // Validate this is a Date
                DateTime myResult = new DateTime();
                if (!DateTime.TryParse(dg_Prices.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim(), out myResult))
                {
                    dg_Prices.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                }
                else
                {
                    // make sure date is not already in list
                    DataRow[] dr = dt_Prices.Select("EffectiveDate='" + myResult.ToString("dd-MMM-yyyy") + "'");
                    if (dr.Length>0)
                    {
                        MessageBox.Show("Cannot have duplicate Dates '" + myResult.ToString("dd-MMM-yyyy") + "'", "Effective Date");
                        dg_Prices.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else
                        dg_Prices.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult.ToString(dg_Prices.Columns[e.ColumnIndex].DefaultCellStyle.Format);
                }
            }
            else if (e.RowIndex != 0 && (dg_Prices.Columns[e.ColumnIndex].Name == "BBG_Ticker" || dg_Prices.Columns[e.ColumnIndex].Name == "ID_BB_UNIQUE" || dg_Prices.Columns[e.ColumnIndex].Name == "ID_BB_GLOBAL" || dg_Prices.Columns[e.ColumnIndex].Name == "Pos_Mult_Factor" ))
            {
                // User may have changed a common variable away from that supplied in the firt Row?
                if (SystemLibrary.ToString(dg_Prices.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) != SystemLibrary.ToString(dg_Prices.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                {
                    MessageBox.Show("You have changed the value of '" + dg_Prices.Columns[e.ColumnIndex].HeaderText + "' to be different to the previous\r\n\r\n" +
                                    "Please double-check before saving this new data", "Price Data");
                }
            }
            if (dg_Prices.Columns[e.ColumnIndex].Name == "Pos_Mult_Factor" || dg_Prices.Columns[e.ColumnIndex].Name == "Close_Price" || dg_Prices.Columns[e.ColumnIndex].Name == "VWAP")
            {
                // Validate this is a number
                Decimal myResult = new Decimal();
                if (!Decimal.TryParse(dg_Prices.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim(), out myResult))
                    dg_Prices.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                else
                {
                    if (myResult < 0)
                    {
                        MessageBox.Show("Cannot have a value less than Zero", dg_Prices.Columns[e.ColumnIndex].Name);
                        dg_Prices.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else
                        dg_Prices.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult.ToString(dg_Prices.Columns[e.ColumnIndex].DefaultCellStyle.Format);
                }
            }

        } //dg_Prices_CellEndEdit()

        private void dg_Prices_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        } //dg_Prices_DataError()


    }
}
