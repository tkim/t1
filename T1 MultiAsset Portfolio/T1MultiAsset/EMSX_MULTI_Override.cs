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
    public partial class EMSX_MULTI_Override : Form
    {
        // Global Variables
        DataTable dt_MULTI;

        public EMSX_MULTI_Override()
        {
            InitializeComponent();
        }

        private void EMSX_MULTI_Override_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            LoadMulti();
        } //EMSX_MULTI_Override_Load()

        private void cb_Mapped_CheckedChanged(object sender, EventArgs e)
        {
            LoadMulti();
        } //cb_Mapped_CheckedChanged()

        private void  LoadMulti()
        {
            // Local Variables
            String mySql;

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;


            if (cb_Mapped.Checked)
            {
                mySql = "Select OrderNumber, TransactionNumber, FillsFileName, Broker, StrategyType, dbo.f_BBGTicker(Ticker,Exchange,YellowKey) as Ticker, Side, Abs(RoutedAmount) as [Routed Amount], ExecFilledAmount as [Filled Amount], ExecAvgPrice as [Avg Price] " +
                        "From	EMS_Fills_Detail " +
                        "Where dbo.f_truncDate(ExecDate) = dbo.f_Today() " +
                        "and ExecSeqNumber = (Select Max(ExecSeqNumber) " +
                        "					 From		EMS_Fills_Detail b " +
                        "					 Where	b.OrderNumber = EMS_Fills_Detail.OrderNumber " +
                        "					 And	b.TransactionNumber = EMS_Fills_Detail.TransactionNumber " +
                        "					) " +
                        "And Exists (	Select	'x' " +
                        "				From	EMS_Fills_Detail_MULTI_Override a " +
                        "				Where	a.OrderNumber = EMS_Fills_Detail.OrderNumber " +
                        "				And		a.TransactionNumber = EMS_Fills_Detail.TransactionNumber " +
                        "		  ) " +
                        "Order by 1,2 ";
            }
            else
            {
                mySql = "Select OrderNumber, TransactionNumber, FillsFileName, Broker, StrategyType, dbo.f_BBGTicker(Ticker,Exchange,YellowKey) as Ticker, Side, Abs(RoutedAmount) as [Routed Amount], ExecFilledAmount as [Filled Amount], ExecAvgPrice as [Avg Price] " +
                        "From	EMS_Fills_Detail " +
                        "Where dbo.f_truncDate(ExecDate) = dbo.f_Today() " +
                        "and ExecSeqNumber = (Select Max(ExecSeqNumber) " +
                        "					 From		EMS_Fills_Detail b " +
                        "					 Where	b.OrderNumber = EMS_Fills_Detail.OrderNumber " +
                        "					 And	b.TransactionNumber = EMS_Fills_Detail.TransactionNumber " +
                        "					) " +
                        "And StrategyType = 'MULTI' " +
                        "Order by 1,2 ";
            }

            dt_MULTI = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_MULTI.DataSource = dt_MULTI;

            // Hide Columns
            dg_MULTI.Columns["OrderNumber"].Visible = false;
            dg_MULTI.Columns["TransactionNumber"].Visible = false;
            dg_MULTI.Columns["FillsFileName"].Visible = false;

            for (int i = 0; i < dg_MULTI.Columns.Count; i++)
            {
                dg_MULTI.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dg_MULTI.Columns[i].ReadOnly = true;
            }

            dg_MULTI.Columns["StrategyType"].ReadOnly = false;
            dg_MULTI.Columns["StrategyType"].DefaultCellStyle.BackColor = Color.LightBlue;

            if (dt_MULTI.Rows.Count > 0)
                bt_Save.Enabled = true;

            
            Cursor.Current = Cursors.Default;

        } //LoadMulti()

        private void dg_MULTI_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Change to upper case
            if (e.ColumnIndex <0 || e.RowIndex<0)
                return;

            dg_MULTI[e.ColumnIndex, e.RowIndex].Value = dg_MULTI[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();

        } //dg_MULTI_CellEndEdit()

        private void bt_Save_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            dg_MULTI.Refresh();

            for (int i = 0;i<dg_MULTI.Rows.Count;i++)
            {
                String OrderNumber = SystemLibrary.ToString(dg_MULTI["OrderNumber",i].Value);
                String TransactionNumber = SystemLibrary.ToString(dg_MULTI["TransactionNumber",i].Value);
                String Broker = SystemLibrary.ToString(dg_MULTI["Broker",i].Value);
                String StrategyType = SystemLibrary.ToString(dg_MULTI["StrategyType",i].Value);
                String FillsFileName = SystemLibrary.ToString(dg_MULTI["FillsFileName",i].Value);

                if (StrategyType != "MULTI")
                {
                    // Check that this combination lives in BrokerMapping
                    int myRows = SystemLibrary.SQLSelectRowsCount("Select * From BrokerMapping Where Broker = '" + Broker + "' and StrategyType = '" + StrategyType + "' ");
                    if (myRows < 1)
                    {
                        if (MessageBox.Show(this, "There is no existing Broker Mapping for:\r\n\r\n\tBroker = '" + Broker + "' and StrategyType = '" + StrategyType + "'\r\n\r\n" +
                                            "Do you really want to set this value?", "[Save]", MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            continue;
                        }
                    }

                    // Update the Database
                    mySql = "Delete From EMS_Fills_Detail_MULTI_Override " +
                            "Where  OrderNumber = " + OrderNumber + " " +
                            "And    TransactionNumber = " + TransactionNumber + " ";
                    SystemLibrary.SQLExecute(mySql);
                    mySql = "Insert Into EMS_Fills_Detail_MULTI_Override (OrderNumber,TransactionNumber,StrategyType) " +
                            "Values (" + OrderNumber + "," + TransactionNumber + ",'" + StrategyType + "') ";
                    SystemLibrary.SQLExecute(mySql);
                    mySql = "exec sp_EMS_Process_Export_File '" + FillsFileName + "' ";
                    SystemLibrary.SQLExecute(mySql);
                }
            }

            // Reset
            bt_Save.Enabled = false;
            LoadMulti();
            Cursor.Current = Cursors.Default;

            MessageBox.Show("Save completed", "EMSX 'MULTI' Override");

        } //bt_Save_Click()

        private void bt_ViewBrokerMapping_Click(object sender, EventArgs e)
        {
            BrokerMaintenance f = new BrokerMaintenance();
            //f.FromParent(this); NEVER CALL THIS FROM HERE.
            f.Show(); //(this);

        } //bt_ViewBrokerMapping_Click()

        private void bt_EMSX_Click(object sender, EventArgs e)
        {
            // Fire up EMSX
            SystemLibrary.BBGBloombergCommand(1, "EMSX<go>");

        } //bt_EMSX_Click()

    }
}
