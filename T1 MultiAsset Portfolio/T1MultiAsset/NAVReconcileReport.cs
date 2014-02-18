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
    public partial class NAVReconcileReport : Form
    {
        // Public Variables
        public Form1 ParentForm1;
        public DataTable dt_Fund;
        public DataTable dt_NAV_Rec;

        public NAVReconcileReport()
        {
            InitializeComponent();
        }

        private void NAVReconcileReport_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            // Load up the Fund & Portfolio Datatables
            String mySql = "Select FundId, FundName, FundAmount, crncy, ShortName " +
                           "From   Fund " +
                           "Where  Active = 'Y' " +
                           "And   ParentFundID = FundID " +
                           "Order By 2 ";
            dt_Fund = SystemLibrary.SQLSelectToDataTable(mySql);
            cb_Fund.DataSource = dt_Fund;
            cb_Fund.DisplayMember = "FundName";
            cb_Fund.ValueMember = "FundId";

            cb_Fund.SelectedIndex = 0;

        } //NAVReconcileReport_Load()

        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;
        } //FromParent()

        private void NAVReconcileReport_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);
            bt_Request_Click(null, null);

        }

        private void bt_Request_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;
            String AsAtDate = dtp_AsAtDate.Value.ToString("dd-MMM-yyyy");
            String FundID = ((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[0].ToString(); ;
            String FundName = ((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[1].ToString();


            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            mySql = "Exec sp_NAV_Rec @FundId = " + FundID + ", @ActionsNeeded = null, @DisplayOrder = null, @AsAtDate = '" + AsAtDate + "' ";
            dt_NAV_Rec = SystemLibrary.SQLSelectToDataTable(mySql);

            // Crystal Report
            cr_NAV_Rec objRpt = new cr_NAV_Rec();
            objRpt.SetDataSource(dt_NAV_Rec);

            CrystalDecisions.CrystalReports.Engine.TextObject root;
            root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["TextTitle_FundName"];
            root.Text = FundName;
            root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["TextTitle_AsAtDate"];
            root.Text = AsAtDate;

            // Binding the crystalReportViewer with our report object. 
            objRpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
            crv_NAV_Rec.ReportSource = objRpt;
            HideTheTabControl(crv_NAV_Rec);

            Cursor.Current = Cursors.Default;


        } //bt_Request_Click()

        public void HideTheTabControl(CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1)
        {
            System.Diagnostics.Debug.Assert(
                crystalReportViewer1.ReportSource != null,
                "you have to set the ReportSource first");

            foreach (Control c1 in crystalReportViewer1.Controls)
            {
                if (c1 is CrystalDecisions.Windows.Forms.PageView)
                {
                    CrystalDecisions.Windows.Forms.PageView pv = (CrystalDecisions.Windows.Forms.PageView)c1;
                    foreach (Control c2 in pv.Controls)
                    {
                        if (c2 is TabControl)
                        {
                            TabControl tc = (TabControl)c2;
                            tc.ItemSize = new Size(0, 1);
                            tc.SizeMode = TabSizeMode.Fixed;
                        }
                    }
                }
            }
        } //HideTheTabControl()


    }
}
