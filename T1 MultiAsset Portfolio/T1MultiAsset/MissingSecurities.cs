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
    public partial class MissingSecurities : Form
    {
        // Public Variables
        public Form1 ParentForm1;
        DataTable dt_MissingSecurities;
        //DataTable dt_Fund;
        //DataTable dt_Portfolio;


        public MissingSecurities()
        {
            InitializeComponent();
        }

        private void MissingSecurities_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            LoadMissingSecurities();
        }

        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;
            //ParentForm1.CopyDataTable(ref dt_Fund, ref dt_Portfolio);
        } //FromParent()

        public void  LoadMissingSecurities()
        {
            // Local Variables
            String mySql;

            // Get the Data
            mySql = "Exec sp_MissingSecurities ";
            dt_MissingSecurities = SystemLibrary.SQLSelectToDataTable(mySql);
            //dg_MissingSecurities.Rows.Clear();
            dg_MissingSecurities.DataSource = dt_MissingSecurities;
            //dg_MissingSecurities.Refresh();


            dg_MissingSecurities.Columns["PARSEKYABLE_DES"].HeaderText = "Bloomberg Equiv Ticker";
            ParentForm1.SetFormatColumn(dg_MissingSecurities, "PARSEKYABLE_DES", Color.DarkBlue, Color.Empty, "", "");

            // TODO (5) Need to do a =BDP(dg_MissingSecurities["PARSEKYABLE_DES", i].Value.ToString(),"PARSEKYABLE_DES")
            //          This will yield the Bloomberg Ticker. Then need to get Form1 to extract the data of these tickers back into the
            //          Securities table.


            // Fire up DDE Request to back fill the securities table
            //for (int i = 0;i<dg_MissingSecurities.Rows.Count;i++)
            //    ParentForm.star BloombergDDE.StartRTD(dg_MissingSecurities["PARSEKYABLE_DES", i].Value.ToString());

            /*
            // Hide Reference columns
            dg_MissingSecurities.Columns["BBG_Ticker"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg_MissingSecurities.Columns["FundName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg_MissingSecurities.Columns["PortfolioName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg_MissingSecurities.Columns["BrokerName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg_MissingSecurities.Columns["CustodianName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dg_MissingSecurities.Columns["GrossValue"].HeaderText = "Gross Value";
            dg_MissingSecurities.Columns["CommissionRate"].HeaderText = "Comm Rate";
            dg_MissingSecurities.Columns["NetValue"].HeaderText = "Net Value";
            dg_MissingSecurities.Columns["TradeDate"].HeaderText = "Trade Date";
            dg_MissingSecurities.Columns["SettlementDate"].HeaderText = "Settle Date";
            dg_MissingSecurities.Columns["Exchange"].HeaderText = "Exch";
            dg_MissingSecurities.Columns["FundName"].HeaderText = "Fund";
            dg_MissingSecurities.Columns["PortfolioName"].HeaderText = "Portfolio";
            dg_MissingSecurities.Columns["BrokerName"].HeaderText = "Broker";
            dg_MissingSecurities.Columns["CustodianName"].HeaderText = "Custodian";

            if(dg_MissingSecurities.Columns.Contains("Send"))
                dg_MissingSecurities.Columns.Remove("Send");
            DataGridViewCheckBoxColumn Send = new DataGridViewCheckBoxColumn();
            Send.HeaderText = "Send";
            Send.FalseValue = "N";
            Send.TrueValue = "Y";
            Send.Name = "Send";
            Send.DataPropertyName = "Send";
            dg_MissingSecurities.Columns.Insert(0, Send);

            ParentForm1.SetFormatColumn(dg_MissingSecurities, "Quantity", Color.Empty, Color.Empty, "N0", "0");
            ParentForm1.SetFormatColumn(dg_MissingSecurities, "Price", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dg_MissingSecurities, "GrossValue", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dg_MissingSecurities, "CommissionRate", Color.Empty, Color.Empty, "0.00%", "0");
            ParentForm1.SetFormatColumn(dg_MissingSecurities, "Commission", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dg_MissingSecurities, "Stamp", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dg_MissingSecurities, "GST", Color.Empty, Color.Empty, "N2", "0");
            ParentForm1.SetFormatColumn(dg_MissingSecurities, "NetValue", Color.Empty, Color.Empty, "N2", "0");
            dg_MissingSecurities.Columns["TradeDate"].DefaultCellStyle.Format = "dd-MMM-yy"; // M = dd-Mon, D = Short Date/Time, F = Long Date/Time
            dg_MissingSecurities.Columns["TradeDate"].DefaultCellStyle.ForeColor = Color.Blue;
            dg_MissingSecurities.Columns["SettlementDate"].DefaultCellStyle.Format = "dd-MMM-yy"; // M = dd-Mon, D = Short Date/Time, F = Long Date/Time

            for (Int32 i = 0; i < dg_MissingSecurities.Rows.Count; i++)
            {
                dg_MissingSecurities["Send", i].Value = "N";
                ParentForm1.SetColumn(dg_MissingSecurities, "Quantity", i);
                ParentForm1.SetColumn(dg_MissingSecurities, "GrossValue", i);
                ParentForm1.SetColumn(dg_MissingSecurities, "Commission", i);
                ParentForm1.SetColumn(dg_MissingSecurities, "Stamp", i);
                ParentForm1.SetColumn(dg_MissingSecurities, "GST", i);
                ParentForm1.SetColumn(dg_MissingSecurities, "NetValue", i);
            }

            */
            // Loop on all columns and set the Autosize mode & Header Text
            for (int i = 0; i < dg_MissingSecurities.Columns.Count; i++)
            {
                //dg_Orders.Columns[i].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
                dg_MissingSecurities.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dg_MissingSecurities.Columns[i].HeaderText = dg_MissingSecurities.Columns[i].HeaderText.Replace('_', ' ');
                dg_MissingSecurities.Columns[i].ReadOnly = true;
            }
            /*
            dg_MissingSecurities.Columns["Send"].ReadOnly = false;
            dg_MissingSecurities.Columns["Side"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_MissingSecurities.Columns["Side"].Width = 30;
            dg_MissingSecurities.Columns["Exchange"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_MissingSecurities.Columns["Exchange"].Width = 35;
            dg_MissingSecurities.Columns["TradeDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_MissingSecurities.Columns["TradeDate"].Width = 60;
            dg_MissingSecurities.Columns["SettlementDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg_MissingSecurities.Columns["SettlementDate"].Width = 60;        
            */

        } //LoadMissingSecurities()

        private void ProcessTrades_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Let the Parent know this has close
            if (ParentForm1 != null)
                ParentForm1.LoadActionTab(true);

        } //ProcessTrades_FormClosed()

        private void bt_SendToBrokers_Click(object sender, EventArgs e)
        {
            // Local Variables

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;
 
            // Refresh
             LoadMissingSecurities();
            Cursor.Current = Cursors.Default;
            //MessageBox.Show("Need to prepare emails here, so Bloomberg Terminal will need a email address, or gets done on non-Bloomberg terminal?");

        } //bt_SendToBrokers_Click()

        private void bt_Manual_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("If the security is a valid Bloomberg Ticker, then this is the WRONG path to take.\r\n\r\n" +
                                "In that case, please model the Ticker in the [Trade] area and the system will look after you.\r\n\r\n" +
                                "Do you wish to continue?", "Manual Ticker Creation", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                String BBG_Ticker = "";
                MaintainSecurities f = new MaintainSecurities();
                f = (MaintainSecurities)SystemLibrary.FormExists(f, false);
                if (dg_MissingSecurities.Rows.Count > 0)
                {
                    BBG_Ticker = SystemLibrary.ToString(dg_MissingSecurities.Rows[0].Cells["PARSEKYABLE_DES"].Value);
                }
                f.FromParent(ParentForm1, BBG_Ticker);
                f.Show(); //(this);
            }

        } //bt_SendToBrokers_Click()

    }
}
