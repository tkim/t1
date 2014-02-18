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
    public partial class MissingSettlementDate : Form
    {
        // Global Variables
        DataTable dt_ApplSettlementDate;

        public MissingSettlementDate()
        {
            InitializeComponent();

            LoadData();
        }

        public void LoadData()
        {
            String mySql;

            mySql = "Select	Fills.OrderRefID, Fills.BBG_Ticker, Securities.Security_Name, Fills.Side, Fills.FillNo, Fills.FillAmount, " +
                    "       Fills.FillPrice, Fills.TradeDate, isNull(Fills.SettlementDate,dbo.f_TruncDate(dbo.f_GetDate()) ) As SettlementDate, Orders.CreatedDate " +
                    "From	Orders, " +
                    "		Fills, " +
                    "		Securities " +
                    "Where	Orders.EffectiveDate < dbo.f_TruncDate(dbo.f_GetDate())  " +
                    "And		isNull(Orders.ProcessedEOD,'N') = 'N' " +
                    "And		Fills.OrderRefID = Orders.OrderRefID " +
                    "And		Fills.SettlementDate is null " +
                    "And		Securities.BBG_Ticker = Orders.BBG_Ticker " +
                    "And		Securities.Security_Typ2 = 'Future' ";

            dt_ApplSettlementDate = SystemLibrary.SQLSelectToDataTable(mySql);

            dg_ApplSettlementDate.DataSource = dt_ApplSettlementDate;

            for (int i = 0; i < dg_ApplSettlementDate.Columns.Count; i++)
            {
                dg_ApplSettlementDate.Columns[i].ReadOnly = true;
            }

            SystemLibrary.SetDataGridView(dg_ApplSettlementDate);
            dg_ApplSettlementDate.Columns["SettlementDate"].ReadOnly = false;
            dg_ApplSettlementDate.Columns["SettlementDate"].DefaultCellStyle.BackColor = Color.LightGreen;

            dg_ApplSettlementDate.Columns["CreatedDate"].DefaultCellStyle.Format = "dd-MMM-yyyy h:mm tt";


        } //LoadData()

        private void bt_Apply_Click(object sender, EventArgs e)
        {
            // Save the Settlement Dates
            for (int i = 0; i < dg_ApplSettlementDate.Rows.Count; i++)
            {
                String OrderRefID = dg_ApplSettlementDate["OrderRefID", i].Value.ToString();
                String FillNo = dg_ApplSettlementDate["FillNo", i].Value.ToString();
                String SettlementDate = Convert.ToDateTime(dg_ApplSettlementDate["SettlementDate", i].Value).ToString("dd-MMM-yyyy");

                String myUpdateSql = "Update Fills " +
                                     "Set   SettlementDate = '" + SettlementDate + "', " +
                                     "      TradeDate = '" + SettlementDate + "' " +
                                     "Where OrderRefID = '" + OrderRefID + "' " +
                                     "And   FillNo = " + FillNo;

                SystemLibrary.SQLExecute(myUpdateSql);
            }

            LoadData();

        } //bt_Apply_Click()

        private void MissingSettlementDate_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        } //MissingSettlementDate_Load()

    }
}
