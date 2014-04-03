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
    public partial class Commission : Form
    {
        // Public variables
        public Form1 ParentForm1;
        DataTable dt_CommModel;
        DataTable dt_Commission;
        String CommModel = "";


        public Commission()
        {
            InitializeComponent();
        }

        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;

        } // FromParent()

        private void Commission_Load(object sender, EventArgs e)
        {
            // Local Variables
            String mySql = "Select CommModel from Commission Group By CommModel";
            dt_CommModel = SystemLibrary.SQLSelectToDataTable(mySql);
            if (dt_CommModel.Rows.Count > 0)
            {
                cb_CommModel.DataSource = dt_CommModel;
                cb_CommModel.DisplayMember = "CommModel";
                cb_CommModel.ValueMember = "CommModel";
                cb_CommModel.SelectedIndex = 0;

                /*
                // Select the Account that matches the passed in Fund
                DataRow[] dr_Find = dt_MLPrime_Accounts.Select("FundId=" + FundID.ToString());
                if (dr_Find.Length > 0)
                {
                    cb_Accounts.SelectedValue = FundID;
                }
                else
                {
                 */
                    CommModel = SystemLibrary.ToString(cb_CommModel.SelectedValue);
                /*
                }
                */
            }

            LoadCommission();

        } //Commission_Load()

        private void LoadCommission()
        {
            /*
             * Rules
             * - Dont allow the user to touch the CommModel column
             * - Once it is saved to the database, the "Style" shouls also be locked?
             * - Cant delete a model that is assigned to a broker.
             * 
             * Maybe have Style as a Drop-down on the form?
             */
            String mySql = "Select CommModel, Exchange, Style, Price_From, Price_To, Rate_Per_Share, bps " +
                           "From Commission " +
                           "Where CommModel = '" + CommModel + "' " +
                           "Order by Exchange, Price_From ";
            dt_Commission = SystemLibrary.SQLSelectToDataTable(mySql);

            dgv_Commission.DataSource = dt_Commission;
            dgv_Commission.Columns["CommModel"].ReadOnly = true;
            dgv_Commission.Columns["Style"].ReadOnly = true;

            if (dt_Commission.Rows.Count == 0)
            {
                // New CommModel
                DataRow dr = dt_Commission.NewRow();
                dr["CommModel"] = CommModel;
                dt_Commission.Rows.Add(dr);
                // HOW DO/DO I NEED To FORCE THIS TO SHOW IN dgv_Commission
            }
            else
            {
                /*
                 * Set the viewable columns based on the Style
                 */
                String myStyle = SystemLibrary.ToString(dt_Commission.Rows[0]["Style"]);
                switch (myStyle)
                {
                    case "Rate":
                        dgv_Commission.Columns["Rate_Per_Share"].Visible = true;
                        dgv_Commission.Columns["bps"].Visible = false;
                        break;
                    case "bps":
                        dgv_Commission.Columns["bps"].Visible = true;
                        dgv_Commission.Columns["Rate_Per_Share"].Visible = false;
                        break;
                }
            }


            /*
            //dgv_Balances.Columns["AccountID"].Visible = false;
            dgv_Balances.Columns["FundName"].Visible = false;
            dgv_Balances.Columns["ShortName"].Visible = false;

            // Make columns read-only
            for (int i = 0; i < dgv_Balances.Columns.Count; i++)
                dgv_Balances.Columns[i].ReadOnly = true;
            */

        } //LoadCommission()

        private void Commission_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);

        } //Commission_Shown()

        private void cb_CommModel_SelectionChangeCommitted(object sender, EventArgs e)
        {
            /*
             * Really should ask if they want to save changes.
             */
            CommModel = SystemLibrary.ToString(((DataRowView)(cb_CommModel.SelectedItem)).Row.ItemArray[0]);
            LoadCommission();

        } //cb_CommModel_SelectionChangeCommitted()

        private void dgv_Commission_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["CommModel"].Value = CommModel;
            e.Row.Cells["Style"].Value = dgv_Commission.Rows[0].Cells["Style"].Value;
        } //cb_CommModel_SelectionChangeCommitted()

        private void bt_New_Click(object sender, EventArgs e)
        {

        } //bt_New_Click()


    }
}
