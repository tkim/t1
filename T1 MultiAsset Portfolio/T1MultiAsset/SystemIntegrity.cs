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
    public partial class SystemIntegrity : Form
    {
        // Public Variables
        public Form1 ParentForm1;

        DataTable dt_System_Intergity;

        public SystemIntegrity()
        {
            InitializeComponent();
        }

        private void SystemIntegrity_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            bt_Refresh_Click(null, null);
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            //if (dgv_SystemIntegrity.DataSource != null)
            //    dgv_SystemIntegrity.Rows.Clear();

            dt_System_Intergity = SystemLibrary.SQLSelectToDataTable("Exec sp_SystemIntegrity 2, 0");
            dgv_SystemIntegrity.DataSource = dt_System_Intergity;
            SystemLibrary.SetDataGridView(dgv_SystemIntegrity);
        }

        private void bt_Close_Click(object sender, EventArgs e)
        {
            this.Close();

        } //bt_Close_Click()

        private void bt_Repair_Click(object sender, EventArgs e)
        {
            SystemLibrary.SQLExecute("Exec sp_SystemIntegrity 0, 0");
            bt_Refresh_Click(null, null);

            MessageBox.Show("Repair Completed", "Repair");

        } // bt_Repair_Click()

        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;

        } //FromParent()

        private void SystemIntegrity_FormClosed(object sender, FormClosedEventArgs e)
        {
            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            // Let the Parent know this has close
            SystemLibrary.SQLExecute("Exec sp_Update_Positions 'Y' ");
            if (ParentForm1 != null)
            {
                ParentForm1.SetUpSecurities_DataTable();
                ParentForm1.LoadPortfolioIncr();
                ParentForm1.LoadActionTab(true);
            }
            SystemLibrary.DebugLine("ProcessOrders_FormClosed: Post LoadActionTab() ");

            Cursor.Current = Cursors.Default;
        } //SystemIntegrity_FormClosed()


    }
}
