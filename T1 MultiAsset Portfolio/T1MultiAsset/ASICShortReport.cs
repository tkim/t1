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
    public partial class ASICShortReport : Form
    {
        // Public Variables
        public Form1 ParentForm1;
        DataTable dt_ASICShorts;

        public ASICShortReport()
        {
            InitializeComponent();
        }

        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;
        } //FromParent()

        private void button1_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySQL;
            String myDate;

            myDate = SystemLibrary.f_Today().ToString("dd-MMM-yyyy");

            if (MessageBox.Show(this, "This Button is to CONFIRM you HAVE submitted the Short Data to ASIC via ASIX.\r\n\r\n" +
                                      "It does NOT do this automatically, so make sure you have followed the Instructions.\r\n\n" +
                                      "Have you submitted the Short Report Data via ASIX<go>?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            else
            {
                // In here Flag the shorts positions having been sent to ASIC.
                for (int i = 0; i < dg_ASICShorts.Rows.Count;i++ )
                {
                    // [Position Date] is the last trading date prior to what is in the positions table.
                    //      That is End-of-Day. But it takes its information from the Start-of-Day on the positions table
                    //      So, Need to get the Positions record date
                    mySQL = "Select Min(EffectiveDate) from Positions " +
                            "Where  BBG_Ticker = '" + dg_ASICShorts["Bloomberg Security", i].Value.ToString() + "' " +
                            "And    EffectiveDate > '" + Convert.ToDateTime(dg_ASICShorts["Position Date", i].Value).ToString("dd-MMM-yyyy") + "' ";
                    String myEffectiveDate = SystemLibrary.SQLSelectDateTime(mySQL,Convert.ToDateTime(dg_ASICShorts["Position Date", i].Value)).ToString("dd-MMM-yyyy");

                    mySQL = "Update Positions " +
                            "Set    ASICShortReport = 'Y' " +
                            "Where  EffectiveDate = '" + myEffectiveDate + "' " +
                            "And    BBG_Ticker = '" + dg_ASICShorts["Bloomberg Security", i].Value.ToString() + "' ";
                    SystemLibrary.SQLExecute(mySQL);

                    mySQL = "Insert into ASIC_Shorts_Sent (Account, BBG_Ticker, Volume, EffectiveDate, SentDate) " +
                            "Values ('" + dg_ASICShorts["Account", i].Value.ToString() + "', " +
                            "        '" + dg_ASICShorts["Bloomberg Security", i].Value.ToString() + "', " +
                            "         " + dg_ASICShorts["Volume", i].Value.ToString() + ", " +
                            "        '" + myEffectiveDate + "', " +
                            "        '" + myDate + "' " +
                            "       ) ";
                    SystemLibrary.SQLExecute(mySQL);

                }
                // Refresh the Data
                cb_AllMissingDates.Checked = false;
                cb_AllMissingDates_CheckedChanged(null, null);
            }
        } //button1_Click()

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;
            String myDate;

            myDate = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
            if (cb_AllMissingDates.Checked)
            {
                mySql = "Select * " +
                        "From   ASIC_Shorts " +
                        "Where	[Already Sent] = 'N' " +
                        "Order by [Position Date], [Bloomberg Security] ";
            }
            else
            {
                mySql = "Select * " +
                        "From   ASIC_Shorts " +
                        "Where	[Position Date] = '" + myDate + "' " +
                        "Order by [Position Date], [Bloomberg Security] ";
            }

            dt_ASICShorts = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_ASICShorts.DataSource = dt_ASICShorts;
            ParentForm1.SetFormatColumn(dg_ASICShorts, "Volume", Color.Red, Color.LightCyan, "N0", "0");
            dg_ASICShorts.Columns["Position Date"].DefaultCellStyle.Format = "dd-MMM-yy"; // M = dd-Mon, D = Short Date/Time, F = Long Date/Time
            dg_ASICShorts.Columns["Position Date"].DefaultCellStyle.ForeColor = Color.Blue;
            dg_ASICShorts.Columns["Send Date"].DefaultCellStyle.Format = "dd-MMM-yy"; // M = dd-Mon, D = Short Date/Time, F = Long Date/Time

            dg_ASICShorts.Columns["Volume"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dg_ASICShorts.Columns["Already Sent"].DefaultCellStyle.BackColor = Color.LightCyan;

            // Distinguish the records already sent
            for (Int32 i = 0; i < dg_ASICShorts.Rows.Count; i++)
            {
                if (dg_ASICShorts["Already Sent", i].Value.ToString() == "Y")
                    dg_ASICShorts["Already Sent", i].Style.ForeColor = Color.Green;
            }
        } //dateTimePicker1_ValueChanged()

        private void ASICShortReport_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            dateTimePicker1.Value = SystemLibrary.f_Now();
            dateTimePicker1_ValueChanged(null, null);
        } //ASICShortReport_Load()

        private void ASICShortReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Let the Parent know this has close
            if (ParentForm1 != null)
                ParentForm1.LoadActionTab(true);
        }


        private void dg_ASICShorts_MouseDown(object sender, MouseEventArgs e)
        {
            //dg_ASICShorts
            String myDragData="";

            // Get the Headers
            /*
            foreach (DataGridViewColumn dgc in dg_ASICShorts.Columns)
            {
                if (dgc.Name != "Already Sent")
                    myDragData = myDragData + dgc.HeaderText + "\t";
            }
            // Strip off the trailing Tab
            myDragData = myDragData.Substring(0,myDragData.Length-1);
            myDragData = myDragData + "\r\n";
            */
            // Get the Rows
            for (int i = 0; i < dg_ASICShorts.Rows.Count; i++)
            {
                for (int j = 0; j < dg_ASICShorts.Columns.Count; j++)
                {
                    if (dg_ASICShorts.Columns[j].Name != "Already Sent")
                    {
                        if (dg_ASICShorts[j, i].ValueType.Name == "DateTime")
                            myDragData = myDragData + Convert.ToDateTime(dg_ASICShorts[j, i].Value).ToString("yyyy/MM/dd") + "\t";
                        else
                            myDragData = myDragData + dg_ASICShorts[j, i].Value.ToString() + "\t";
                    }
                }
                // Strip off the trailing Tab
                myDragData = myDragData.Substring(0, myDragData.Length-1);
                myDragData = myDragData + "\r\n";
            }

            DragDropEffects dde1 = DoDragDrop(myDragData, DragDropEffects.Copy);
        }

        private void cb_AllMissingDates_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = !cb_AllMissingDates.Checked;
            dateTimePicker1_ValueChanged(null, null);
        }

        private void bt_ASIX_Click(object sender, EventArgs e)
        {
            SystemLibrary.BBGBloombergCommand(1, "ASIX<go>");
        }

        private void ASICShortReport_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);
        } 
    }
}
