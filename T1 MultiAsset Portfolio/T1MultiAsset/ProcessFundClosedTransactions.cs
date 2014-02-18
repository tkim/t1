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
    public partial class ProcessFundClosedTransactions : Form
    {
        // Global Variables
        public Form1 ParentForm1;
        DataTable dt_Summary;
        private int CXLocation = 0;
        private int CYLocation = 0;

        public struct DetailMenuStruct
        {
            public int TranID;
            public int FundID;
            public int ParentFundID;
            public int JournalID;
            public int AccountID;
            public Form myParentForm;
            public String TranType;
            public DateTime EffectiveDate;
            public DateTime RecordDate;
            public String Description;
            public String AccountType;
            public Double Amount;
            public String crncy;
            public String PortfolioID;
        }

        public ProcessFundClosedTransactions()
        {
            InitializeComponent();
        }

        private void ProcessFundClosedTransactions_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            LoadSummary();
        }

        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;

        } //FromParent()

        public void LoadSummary()
        {
            // Local Variables
            String mySql;

            mySql = "Exec sp_GetPostFundClosedTransactions ";
            dt_Summary = SystemLibrary.SQLSelectToDataTable(mySql);

            dg_Summary.DataSource = dt_Summary;
            SystemLibrary.SetDataGridView(dg_Summary);
            //dg_Summary.Columns["EffectiveDate"].HeaderText = "Effective Date";


        } //LoadSummary()

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            LoadSummary();
        } //bt_Refresh_Click()

        private void dg_Summary_MouseClick(object sender, MouseEventArgs e)
        {
            CXLocation = dg_Summary.Location.X + e.Location.X + 5;
            CYLocation = dg_Summary.Location.Y + e.Location.Y;

        } //dg_Summary_MouseClick()

        private void dg_Summary_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Show the popup menu
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    Point myLocation = new Point(this.Location.X + CXLocation, this.Location.Y + CYLocation);
                    String JournalID = SystemLibrary.ToString(dg_Summary.Rows[e.RowIndex].Cells["JournalID"].Value);

                    ContextMenuStrip myMenu = new ContextMenuStrip();
                    ToolStripMenuItem mySubMenu = new ToolStripMenuItem();

                    DetailMenuStruct myDetailStr = new DetailMenuStruct();
                    myDetailStr.TranID = SystemLibrary.ToInt32(dg_Summary.Rows[e.RowIndex].Cells["TranID"].Value);
                    myDetailStr.FundID = SystemLibrary.ToInt32(dg_Summary.Rows[e.RowIndex].Cells["FundID"].Value);
                    myDetailStr.ParentFundID = SystemLibrary.ToInt32(dg_Summary.Rows[e.RowIndex].Cells["ParentFundID"].Value);
                    myDetailStr.JournalID = SystemLibrary.ToInt32(dg_Summary.Rows[e.RowIndex].Cells["JournalID"].Value);
                    myDetailStr.AccountID = SystemLibrary.ToInt32(dg_Summary.Rows[e.RowIndex].Cells["AccountID"].Value);
                    myDetailStr.TranType = SystemLibrary.ToString(dg_Summary.Rows[e.RowIndex].Cells["TranType"].Value);
                    myDetailStr.EffectiveDate = Convert.ToDateTime(dg_Summary.Rows[e.RowIndex].Cells["EffectiveDate"].Value);
                    myDetailStr.RecordDate = Convert.ToDateTime(dg_Summary.Rows[e.RowIndex].Cells["RecordDate"].Value);
                    myDetailStr.Description = SystemLibrary.ToString(dg_Summary.Rows[e.RowIndex].Cells["Description"].Value);
                    myDetailStr.AccountType = SystemLibrary.ToString(dg_Summary.Rows[e.RowIndex].Cells["AccountType"].Value);
                    myDetailStr.Amount = SystemLibrary.ToDouble(dg_Summary.Rows[e.RowIndex].Cells["Amount"].Value);
                    myDetailStr.crncy = SystemLibrary.ToString(dg_Summary.Rows[e.RowIndex].Cells["crncy"].Value);
                    myDetailStr.PortfolioID = SystemLibrary.ToString(dg_Summary.Rows[e.RowIndex].Cells["PortfolioID"].Value);
                    myDetailStr.myParentForm = this;

                    // Create Menu
                    if (myDetailStr.TranType.ToUpper() == "FRANKING CREDIT")
                    {
                        mySubMenu = new ToolStripMenuItem("Check 45 Day Rule");
                        mySubMenu.Tag = myDetailStr;
                        mySubMenu.MouseUp += new MouseEventHandler(dg_SummarySystemMenuItem_Click);
                        myMenu.Items.Add(mySubMenu);
                        if (myDetailStr.FundID != myDetailStr.ParentFundID)
                        {
                            mySubMenu = new ToolStripMenuItem("Move Franking Credit to another Sub-Fund(s)");
                            mySubMenu.Tag = myDetailStr;
                            mySubMenu.MouseUp += new MouseEventHandler(dg_SummarySystemMenuItem_Click);
                            myMenu.Items.Add(mySubMenu);
                        }
                        else
                        {
                            mySubMenu = new ToolStripMenuItem("General Journal");
                            mySubMenu.Tag = myDetailStr;
                            mySubMenu.MouseUp += new MouseEventHandler(dg_SummarySystemMenuItem_Click);
                            myMenu.Items.Add(mySubMenu);
                        }
                    }
                    else if (myDetailStr.TranType.ToUpper() == "DIVIDEND")
                    {
                        if (myDetailStr.FundID != myDetailStr.ParentFundID)
                        {
                            mySubMenu = new ToolStripMenuItem("Move Dividend to another Sub-Fund(s)");
                            mySubMenu.Tag = myDetailStr;
                            mySubMenu.MouseUp += new MouseEventHandler(dg_SummarySystemMenuItem_Click);
                            myMenu.Items.Add(mySubMenu);
                        }
                        else
                        {
                            mySubMenu = new ToolStripMenuItem("General Journal");
                            mySubMenu.Tag = myDetailStr;
                            mySubMenu.MouseUp += new MouseEventHandler(dg_SummarySystemMenuItem_Click);
                            myMenu.Items.Add(mySubMenu);
                        }
                    }
                    else if (myDetailStr.TranType.ToUpper() == "INTEREST")
                    {
                        if (myDetailStr.AccountType.ToUpper() == "ACCRUAL")
                        {
                            mySubMenu = new ToolStripMenuItem("Reverse Interest Accrual Entry");
                            mySubMenu.Tag = myDetailStr;
                            mySubMenu.MouseUp += new MouseEventHandler(dg_SummarySystemMenuItem_Click);
                            myMenu.Items.Add(mySubMenu);
                        }
                        if (myDetailStr.FundID != myDetailStr.ParentFundID)
                            mySubMenu = new ToolStripMenuItem("Apportion Interest to another Sub-Fund(s)");
                        else
                            mySubMenu = new ToolStripMenuItem("General Journal");
                        mySubMenu.Tag = myDetailStr;
                        mySubMenu.MouseUp += new MouseEventHandler(dg_SummarySystemMenuItem_Click);
                        myMenu.Items.Add(mySubMenu);
                    }
                    else
                    {
                        mySubMenu = new ToolStripMenuItem("General Journal");
                        mySubMenu.Tag = myDetailStr;
                        mySubMenu.MouseUp += new MouseEventHandler(dg_SummarySystemMenuItem_Click);
                        myMenu.Items.Add(mySubMenu);
                    }

                    // Show the Menu
                    myMenu.Show(myLocation);
                }
            }
            catch { }

        } // dg_Summary_CellClick()

        public static void dg_SummarySystemMenuItem_Click(object sender, MouseEventArgs e)
        {
            //
            // Purpose: A generic menu click event for the Detail Right-Click
            //

            // Local Variables
            ToolStripMenuItem ts_From = (ToolStripMenuItem)sender;
            DetailMenuStruct myDetailStr = (DetailMenuStruct)ts_From.Tag;

            switch (ts_From.Text)
            {
                case "Check 45 Day Rule":
                    String mySql = "Exec sp_Check_45_Day_Rule " + myDetailStr.TranID.ToString();
                    String myMessage = SystemLibrary.SQLSelectString(mySql);
                    if (myMessage.Length == 0)
                        myMessage = "45 Day Rule checked and applied";
                    ((ProcessFundClosedTransactions)myDetailStr.myParentForm).LoadSummary();
                    MessageBox.Show(myMessage, ts_From.Text);
                    break;
                case "Move Franking Credit to another Sub-Fund(s)":
                case "Move Dividend to another Sub-Fund(s)":
                case "Apportion Interest to another Sub-Fund(s)":
                    ProcessJournals frm = new ProcessJournals();
                    frm.FromParent(myDetailStr.myParentForm, myDetailStr.TranID, myDetailStr.TranType, myDetailStr.EffectiveDate, myDetailStr.AccountID, myDetailStr.AccountType, myDetailStr.ParentFundID, myDetailStr.FundID, myDetailStr.PortfolioID, myDetailStr.Description, myDetailStr.Amount, myDetailStr.crncy);
                    frm.Show();
                    break;
                case "Reverse Interest Accrual Entry":
                    ProcessJournals frm1 = new ProcessJournals();
                    frm1.FromParent(myDetailStr.myParentForm, "Reverse Accrual", myDetailStr.TranID, myDetailStr.TranType, myDetailStr.EffectiveDate, myDetailStr.AccountID, myDetailStr.AccountType, myDetailStr.Description, myDetailStr.Amount, myDetailStr.crncy, myDetailStr.ParentFundID, myDetailStr.FundID, myDetailStr.PortfolioID);
                    frm1.Show();
                    break;
                case "General Journal":
                default:
                    ProcessJournals frm2 = new ProcessJournals();
                    frm2.FromParent(myDetailStr.myParentForm, "General Journal", myDetailStr.TranID, myDetailStr.TranType, myDetailStr.EffectiveDate, myDetailStr.AccountID, myDetailStr.AccountType, myDetailStr.Description, myDetailStr.Amount, myDetailStr.crncy, myDetailStr.ParentFundID, myDetailStr.FundID, myDetailStr.PortfolioID);
                    frm2.Show();
                    break;
            }

        } //dg_SummarySystemMenuItem_Click()



        private void ProcessFundClosedTransactions_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);
        } //ProcessFundClosedTransactions_Shown()

        private void ProcessFundClosedTransactions_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Refresh the ActionsTab
            if (ParentForm1 != null)
                ParentForm1.LoadActionTab(true);

        } //ProcessFundClosedTransactions_FormClosed()

        

    }
}
