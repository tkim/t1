using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace T1MultiAsset
{
    public partial class ChartOfAccounts : Form
    {
        // Public Variables
        public Form1 ParentForm1;
        int myFundID = -1;
        //DataTable dt_Transactions;
        //DataTable dt_TranType;
        public DataTable dt_EMSX_API;
        public DataTable dt_Open_Orders;
        //DataTable dt_Fund;
        public static Boolean TestProcess = false;
 
        public ChartOfAccounts()
        {
            InitializeComponent();
        }

        private void ChartOfAccounts_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); 
            dgv_EMSX_API.DataSource = dt_EMSX_API;
            dgv_Open_Orders.DataSource = dt_Open_Orders;
        } //ChartOfAccounts_Load()

        public void FromParent(Form inForm, int inFundID)
        {
            ParentForm1 = (Form1)inForm;
            //ParentForm1.CopyDataTable(ref dt_Fund, ref dt_Portfolio);
            myFundID = inFundID;

        } //FromParent()

        public delegate void Refresh_dgv_EMSX_APICallback();
        public void Refresh_dgv_EMSX_API()
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                Refresh_dgv_EMSX_APICallback cb = new Refresh_dgv_EMSX_APICallback(Refresh_dgv_EMSX_API);
                this.Invoke(cb, new object[] {  });
            }
            else
            {
                try
                {
                    dgv_EMSX_API.DataSource = dt_EMSX_API;
                    for (int i = 0; i < dgv_EMSX_API.Columns.Count; i++)
                    {
                        if (dgv_EMSX_API.Columns[i].HeaderText.StartsWith("EMSX_"))
                            dgv_EMSX_API.Columns[i].HeaderText = dgv_EMSX_API.Columns[i].HeaderText.Substring(5);
                        dgv_EMSX_API.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                    }

                    dgv_EMSX_API.Refresh();
                    for (int i = 0; i < dt_EMSX_API.Rows.Count; i++)
                    {
                        if (dt_EMSX_API.Rows[i]["MSG_SUB_TYPE"].ToString() == "O")
                            dgv_EMSX_API.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;
                        if (dt_EMSX_API.Rows[i]["EMSX_STATUS"].ToString().ToUpper() == "FILLED")
                            dgv_EMSX_API.Rows[i].Cells["EMSX_STATUS"].Style.ForeColor = Color.Green;
                    }
                    dgv_EMSX_API.Refresh();
                }
                catch { }

            }
        } //Refresh_dgv_EMSX_API()

        public delegate void Refresh_dgv_Open_OrdersCallback();
        public void Refresh_dgv_Open_Orders()
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                Refresh_dgv_Open_OrdersCallback cb = new Refresh_dgv_Open_OrdersCallback(Refresh_dgv_Open_Orders);
                this.Invoke(cb, new object[] { });
            }
            else
            {
                try
                {
                    dgv_Open_Orders.DataSource = dt_Open_Orders;
                    dgv_Open_Orders.Refresh();
                    SystemLibrary.SetDataGridView(dgv_Open_Orders);
                    dgv_Open_Orders.Columns["PM"].Visible = false;
                    dgv_Open_Orders.Columns["IdeaOwner"].Visible = false;
                    dgv_Open_Orders.Columns["Strategy1"].Visible = false;
                    dgv_Open_Orders.Columns["Strategy2"].Visible = false;
                    dgv_Open_Orders.Columns["Strategy3"].Visible = false;
                    dgv_Open_Orders.Columns["Strategy4"].Visible = false;
                    dgv_Open_Orders.Columns["Strategy5"].Visible = false;
                }
                catch { }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            TestProcess = checkBox1.Checked;
        }

        public delegate Boolean GetTestProcessCallback();
        public Boolean GetTestProcess()
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                GetTestProcessCallback cb = new GetTestProcessCallback(GetTestProcess);
                return((Boolean)this.Invoke(cb, new object[] { }));
            }
            else
            {
                return (TestProcess);
            }
        }


      }
}
