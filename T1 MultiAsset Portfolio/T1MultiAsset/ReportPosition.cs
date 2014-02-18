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
    public partial class ReportPosition : Form
    {
        // Public Variables
        public Form1 ParentForm1;
        DataTable dt_ReportPosition;
        DataTable dt_Fund;
        DataTable dt_Portfolio;

        int     FundID = -1;
        String  Fund_Name = "";
        int     PortfolioID = -1;
        String  Portfolio_Name = "";
        String  BBG_Ticker = "";
        DateTime FromDate = DateTime.MinValue;
        DateTime ToDate = DateTime.MaxValue;


        public ReportPosition()
        {
            // Local Variables
            String mySql;

            InitializeComponent();

            // RULE: Need to have this data before FromParent() is called.
            cb_hasFromDate.Checked = true;
            dtp_FromDate.Value = SystemLibrary.f_Now().AddMonths(-3);
            dtp_ToDate.Value = SystemLibrary.f_Today();
            // Load the Drop down boxes
            mySql = "Select FundID, FundName, ShortName, CreatedDate " +
                    "From   Fund " +
                    "Where  Active = 'Y' " +
                    "And    AllowTrade = 'Y' " +
                    "Order by FundName";
            dt_Fund = SystemLibrary.SQLSelectToDataTable(mySql);
            cb_Fund.DataSource = dt_Fund;
            cb_Fund.DisplayMember = "FundName";
            cb_Fund.ValueMember = "FundId";
            cb_Fund.SelectedIndex = 0;

            mySql = "Select PortfolioID, PortfolioName, CreatedDate, ClosedDate, Active " +
                    "From   Portfolio " +
                    "Order by PortfolioName";
            dt_Portfolio = SystemLibrary.SQLSelectToDataTable(mySql);
            cb_Portfolio.DataSource = dt_Portfolio;
            cb_Portfolio.DisplayMember = "PortfolioName";
            cb_Portfolio.ValueMember = "PortfolioId";
            cb_Portfolio.SelectedIndex = 0;

        } //ReportPosition()

        public void FromParent(Form inForm, int inFundID, int inPortfolioID, String inBBG_Ticker, object inFromDate, object inToDate, int inBrokerID)
        {
            ParentForm1 = (Form1)inForm;

            // Set the Global variables
            FundID = inFundID;
            PortfolioID = inPortfolioID;
            BBG_Ticker = inBBG_Ticker;

            if (inFromDate != null)
            {
                FromDate = (DateTime)inFromDate;
                cb_hasFromDate.Checked = true;
                dtp_FromDate.Value = FromDate;
            }
            if (inToDate != null)
            {
                ToDate = (DateTime)inToDate;
                cb_hasToDate.Checked = true;
                dtp_ToDate.Value = FromDate;
            }

            // Set up the boxes
            if (FundID == -1)
                cb_hasFundID.Checked = false;
            else
            {
                cb_Fund.SelectedValue = FundID;
                cb_hasFundID.Checked = true;
            }

            if (PortfolioID == -1)
                cb_hasPortfolioID.Checked = false;
            else
            {
                cb_Portfolio.SelectedValue = PortfolioID;
                cb_hasPortfolioID.Checked = true;
            }

            if (BBG_Ticker == "")
                cb_hasBBG_Ticker.Checked = false;
            else
            {
                tb_BBG_Ticker.Text = BBG_Ticker;
                cb_hasBBG_Ticker.Checked = true;
            }

            LoadPositions();

            //ParentForm1.CopyDataTable(ref dt_Fund, ref dt_Portfolio);
        } //FromParent()


        private void ReportPosition_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            FormatPositions();
        } //ReportPosition_Load

        private void cb_Fund_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FundID = Convert.ToInt16(((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[0].ToString());
            Fund_Name = ((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[1].ToString();
            //Fund_Amount = Convert.ToDecimal(((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[2].ToString());
            //Fund_Crncy = ((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[3].ToString();
            //LoadPositions();

        } // cb_Fund_SelectionChangeCommitted()

        private void cb_Portfolio_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PortfolioID = Convert.ToInt16(((DataRowView)(cb_Portfolio.SelectedItem)).Row.ItemArray[0].ToString());
            Portfolio_Name = ((DataRowView)(cb_Portfolio.SelectedItem)).Row.ItemArray[1].ToString();
            //Portfolio_Amount = Convert.ToDecimal(((DataRowView)(cb_Portfolio.SelectedItem)).Row.ItemArray[2].ToString());
            //Portfolio_Crncy = ((DataRowView)(cb_Portfolio.SelectedItem)).Row.ItemArray[3].ToString();
            //LoadPositions();

        } // cb_Portfolio_SelectionChangeCommitted()

        private void LoadPositions()
        {
            /*
             * Positions report
             */

            // Local Variables
            String mySql;

            mySql = "Select	FundName, PortfolioName, EffectiveDate, BBG_Ticker, Quantity, prev_Close as Price, " +
                    "		Quantity * prev_Close * Pos_Mult_Factor * FX_Rate * Fx_Pos_Mult_Factor as  NetValue " +
                    "from	Positions " +
                    "Where	Positions.Quantity <> 0  ";
            if (cb_hasFundID.Checked)
            {
                mySql = mySql +
                        "And		Positions.FundID = " + FundID.ToString() + " ";
            }
            if (cb_hasPortfolioID.Checked)
            {
                mySql = mySql +
                        "And		Positions.PortfolioID = " + PortfolioID.ToString() + " ";
            }
            if (cb_hasBBG_Ticker.Checked && tb_BBG_Ticker.Text.Trim().Length > 0)
            {
                mySql = mySql +
                        "And		Positions.BBG_Ticker = '" + tb_BBG_Ticker.Text + "' ";
            }
            if (cb_hasFromDate.Checked)
            {
                mySql = mySql +
                        "And		Positions.EffectiveDate >= '" + dtp_FromDate.Value.ToString("dd-MMM-yyyy") + "' ";
            }
            if (cb_hasToDate.Checked)
            {
                mySql = mySql +
                        "And		Positions.EffectiveDate <= '" + dtp_ToDate.Value.ToString("dd-MMM-yyyy") + "' ";
            }

            mySql = mySql +
                    "Order by EffectiveDate, BBG_Ticker, FundName, PortfolioName ";

            dt_ReportPosition = SystemLibrary.SQLSelectToDataTable(mySql);

            dg_ReportPosition.DataSource = dt_ReportPosition;

            // Scroll to the last row
            if (dg_ReportPosition.Rows.Count>0)
                dg_ReportPosition.CurrentCell = dg_ReportPosition[0, dg_ReportPosition.Rows.Count - 1];

            FormatPositions();

        } //LoadPositions()

        private void FormatPositions()
        {
            ParentForm1.SetFormatColumn(dg_ReportPosition, "FundName", Color.RoyalBlue, Color.Gainsboro, "", "");
            ParentForm1.SetFormatColumn(dg_ReportPosition, "BBG_Ticker", Color.RoyalBlue, Color.Gainsboro, "", "");
            ParentForm1.SetFormatColumn(dg_ReportPosition, "Quantity", Color.Empty, Color.Empty, "N0", "0");
            ParentForm1.SetFormatColumn(dg_ReportPosition, "Price", Color.Blue, Color.Empty, "N4", "0");
            ParentForm1.SetFormatColumn(dg_ReportPosition, "NetValue", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dg_ReportPosition, "EffectiveDate", Color.DarkBlue, Color.Empty, "ddd dd-MMM-yy", "");
            dg_ReportPosition.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dg_ReportPosition.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dg_ReportPosition.Columns["NetValue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dg_ReportPosition.Columns["EffectiveDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            for (int i = 0; i < dg_ReportPosition.Columns.Count; i++)
            {
                dg_ReportPosition.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            // Loop over the Tickers 
            for (Int32 i = 0; i < dg_ReportPosition.Rows.Count; i++) // Last row in dg_ReportPosition is a blank row
            {
                if (dg_ReportPosition["BBG_Ticker", i].Value != null)
                {
                    ParentForm1.SetColumn(dg_ReportPosition, "Quantity", i);
                    ParentForm1.SetColumn(dg_ReportPosition, "NetValue", i);
                }
            }
        } //FormatPositions()

        private void bt_Request_Click(object sender, EventArgs e)
        {
            LoadPositions();

        } //bt_Request_Click()

        private void cb_hasFundID_CheckedChanged(object sender, EventArgs e)
        {
            cb_Fund_SelectionChangeCommitted(null, null);
            cb_Fund.Enabled = cb_hasFundID.Checked;
        }

        private void cb_hasPortfolioID_CheckedChanged(object sender, EventArgs e)
        {
            cb_Portfolio_SelectionChangeCommitted(null, null);
            cb_Portfolio.Enabled = cb_hasPortfolioID.Checked;
        }

        private void cb_hasBBG_Ticker_CheckedChanged(object sender, EventArgs e)
        {
            tb_BBG_Ticker.Enabled = cb_hasBBG_Ticker.Checked;
        }

        private void cb_hasFromDate_CheckedChanged(object sender, EventArgs e)
        {
            dtp_FromDate.Enabled = cb_hasFromDate.Checked;
        }

        private void cb_hasToDate_CheckedChanged(object sender, EventArgs e)
        {
            dtp_ToDate.Enabled = cb_hasToDate.Checked;
        }

        private void dg_ReportPosition_Sorted(object sender, EventArgs e)
        {
            FormatPositions();
        }

        private void ReportPosition_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);            
        }

    }
}
