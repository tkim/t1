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
    public partial class CommissionSummary: Form
    {
        // Public Variables
        public Form1 ParentForm1;
        DataTable dt_CommissionSummary;
        DataTable dt_Fund;
        DataTable dt_Broker;

        int     FundID = -1;
        String  Fund_Name = "";
        int     BrokerID = -1;
        String Broker_Name = "";
        DateTime FromDate = DateTime.MinValue;
        DateTime ToDate = DateTime.MaxValue;


        public CommissionSummary()
        {
            // Local Variables
            String mySql;

            InitializeComponent();

            // RULE: Need to have this data before FromParent() is called.
            dtp_FromDate.Value = SystemLibrary.f_Today().AddMonths(-3);
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

            mySql = "Select BrokerID, BrokerName " +
                    "From   Broker " +
                    "Order by BrokerName";
            dt_Broker = SystemLibrary.SQLSelectToDataTable(mySql);
            cb_Broker.DataSource = dt_Broker;
            cb_Broker.DisplayMember = "BrokerName";
            cb_Broker.ValueMember = "BrokerId";
            cb_Broker.SelectedIndex = 0;


        } //CommissionSummary()

        private void CommissionSummary_Load(object sender, EventArgs e)
        {
            FormatCommissionSummary();
        }

        public void FromParent(Form inForm, int inFundID, object inFromDate, object inToDate, int inBrokerID)
        {
            ParentForm1 = (Form1)inForm;

            // Set the Global variables
            FundID = inFundID;
            BrokerID = inBrokerID;

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

            if (BrokerID == -1)
                cb_hasBrokerID.Checked = false;
            else
            {
                cb_Broker.SelectedValue = BrokerID;
                cb_hasBrokerID.Checked = true;
            }

            LoadCommissions();

            //ParentForm1.CopyDataTable(ref dt_Fund, ref dt_Portfolio);
        } //FromParent()


        private void ReportTrade_Load(object sender, EventArgs e)
        {
            FormatCommissionSummary();
        } //ReportTrade_Load

        private void cb_Fund_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FundID = Convert.ToInt16(((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[0].ToString());
            Fund_Name = ((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[1].ToString();
            //LoadCommissions();

        } // cb_Fund_SelectionChangeCommitted()

        private void cb_Broker_SelectionChangeCommitted(object sender, EventArgs e)
        {
            BrokerID = Convert.ToInt16(((DataRowView)(cb_Broker.SelectedItem)).Row.ItemArray[0].ToString());
            Broker_Name = ((DataRowView)(cb_Broker.SelectedItem)).Row.ItemArray[1].ToString();
            //LoadCommissions();

        } // cb_Broker_SelectionChangeCommitted()

        public void LoadCommissions()
        {
            /*
             * Commission Report
             */
            String myReportType = "'" + cb_ReportType.Text + "'";
            String myFundID = "null";
            String myBrokerID = "null";
            String myStartDate = "null";
            String myEndDate = "null";

            // Local Variables
            String mySql;

            if (cb_hasFundID.Checked)
                myFundID = FundID.ToString();
            if (cb_hasBrokerID.Checked)
                myBrokerID = BrokerID.ToString();
            if (cb_hasFromDate.Checked)
                myStartDate = "'" + dtp_FromDate.Value.ToString("dd-MMM-yyyy") + "'";
            if (cb_hasToDate.Checked)
                myEndDate = "'" + dtp_ToDate.Value.ToString("dd-MMM-yyyy") + "'";

            // Exec sp_Report_Commissions 	@ReportType, @FundID, @BrokerID, @StartDate, @EndDate
            mySql = "Exec sp_Report_Commissions " + myReportType + ", " + myFundID + ", " + myBrokerID + ", " + myStartDate + ", " + myEndDate + " ";

            dt_CommissionSummary= SystemLibrary.SQLSelectToDataTable(mySql);
            dg_CommissionSummary.DataSource = dt_CommissionSummary;

            FormatCommissionSummary();

        } //LoadCommissions()

        private void FormatCommissionSummary()
        {
            dg_CommissionSummary.Columns["TotalComm"].HeaderText = "Total Commission";
            ParentForm1.SetFormatColumn(dg_CommissionSummary, "Description", Color.RoyalBlue, Color.Gainsboro, "", "");
            ParentForm1.SetFormatColumn(dg_CommissionSummary, "TotalComm", Color.Empty, Color.Empty, "$#,###.00", "0");
            ParentForm1.SetFormatColumn(dg_CommissionSummary, "Commission", Color.Empty, Color.LightBlue, "$#,###.00", "0");
            ParentForm1.SetFormatColumn(dg_CommissionSummary, "GST", Color.Empty, Color.Empty, "$#,###.00", "0");

            // Loop on columns
            for (Int32 i = 0; i < dg_CommissionSummary.Columns.Count; i++)
            {
                dg_CommissionSummary.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            // Loop down rows
            for (Int32 i = 0; i < dg_CommissionSummary.Rows.Count; i++) // Last row in dg_CommissionSummaryis a blank row
            {
                if (dg_CommissionSummary["Description", i].Value.ToString().ToUpper() == "TOTAL")
                {
                    for (Int32 j = 0; j < dg_CommissionSummary.Columns.Count; j++)
                        dg_CommissionSummary[j, i].Style.ForeColor = Color.Green;
                    //dg_CommissionSummary["Description", i].Style.
                }
            }
        } //FormatCommissionSummary()

        private void bt_Request_Click(object sender, EventArgs e)
        {
            LoadCommissions();

        } //bt_Request_Click()

        private void cb_hasFundID_CheckedChanged(object sender, EventArgs e)
        {
            cb_Fund.Enabled = cb_hasFundID.Checked;
        }

        private void cb_hasBrokerID_CheckedChanged(object sender, EventArgs e)
        {
            cb_Broker.Enabled = cb_hasBrokerID.Checked;
        }

        private void cb_hasFromDate_CheckedChanged(object sender, EventArgs e)
        {
            dtp_FromDate.Enabled = cb_hasFromDate.Checked;
        }

        private void cb_hasToDate_CheckedChanged(object sender, EventArgs e)
        {
            dtp_ToDate.Enabled = cb_hasToDate.Checked;
        }

        private void dg_CommissionSummary_Sorted(object sender, EventArgs e)
        {
            FormatCommissionSummary();
        }

        private void CommissionSummary_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control && e.KeyValue == (int)Keys.P)
                SystemLibrary.PrintScreen(this);

        }

        private void dg_CommissionSummary_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dg_CommissionSummary_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control && e.KeyValue == (int)Keys.P)
                SystemLibrary.PrintScreen(this);
        }

        private void CommissionSummary_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);
        }


    }
}
