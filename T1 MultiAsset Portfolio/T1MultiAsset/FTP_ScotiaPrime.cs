using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace T1MultiAsset
{
    public partial class FTP_ScotiaPrime : Form
    {
        // The parent code can attach a routine to here, that will be triggered on a sucessful close
        //public delegate void Message();
        //public event Message OnMessage;

        public DataTable dt_System_Parameters;

        private Form1 frm1;

        public FTP_ScotiaPrime()
        {
            InitializeComponent();
        }

        private void FTP_ScotiaPrime_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            // Ask the parent form to get data from database.
            frm1.FTPSCOTIAPrimeStructure("LOAD");
            // Display the data
            tb_ServerIP.Text = SystemLibrary.FTPSCOTIAPrimeVars.ServerIP;
            tb_ServerIP2.Text = SystemLibrary.FTPSCOTIAPrimeVars.ServerIP2;
            tb_UserID.Text = SystemLibrary.FTPSCOTIAPrimeVars.UserID;
            tb_Password.Text = SystemLibrary.FTPSCOTIAPrimeVars.Password;
            tb_EMSXFileNameStartsWith.Text = SystemLibrary.FTPSCOTIAPrimeVars.EMSXFileNameStartsWith;
            tb_Interval_seconds.Text = SystemLibrary.FTPSCOTIAPrimeVars.Interval_seconds.ToString();
            tb_LastUpdate.Text = SystemLibrary.FTPSCOTIAPrimeVars.LastUpdate.ToShortDateString();


            dt_System_Parameters = SystemLibrary.SQLSelectToDataTable("Select Parameter_name, P_header, p_width, p_int, p_number, p_date, p_string from system_parameters where parameter_name like '%scotia%'");
            dg_System_Parameters.DataSource = dt_System_Parameters;
            if (dt_System_Parameters.Columns.Count > 0)
            {
                dg_System_Parameters.Columns["Parameter_name"].ReadOnly = true;
                //Font FontBold = new Font(dg_System_Parameters.RowsDefaultCellStyle.Font, FontStyle.Bold);
                //dg_System_Parameters.Columns["Parameter_name"].DefaultCellStyle.Font = FontBold;
                dg_System_Parameters.Columns["Parameter_name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dg_System_Parameters.Columns["p_string"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dg_System_Parameters.Columns["Parameter_name"].Frozen = true; // Frozen - All columns prior to this.
            }
            else
                bt_Param_Save.Enabled = false;

        } // FTP_ScotiaPrime_Load()

        public void Startup(object myParent)
        {
            //Only way I know how to deal with Parent/Child
            frm1 = (Form1)myParent;
        } //Startup()

        private void bt_Save_Click(object sender, EventArgs e)
        {
            String retVal;
            Int32 Interval_seconds = 0;
            retVal = SystemLibrary.FTPTestStructure(tb_ServerIP.Text, tb_UserID.Text, tb_Password.Text);

            if (retVal.Length > 0)
            {
                if (MessageBox.Show(this, "Test Failed with the following error(s).\r\n\r\n" + retVal + "\r\n\r\n\r\nDo you wish to continue anyway?",
                                   this.Text + " - Test Connection", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                    == DialogResult.No)
                {
                    return;
                }
            }

            // Save to Database
            // Display the data
            SystemLibrary.FTPSCOTIAPrimeVars.ServerIP = tb_ServerIP.Text;
            SystemLibrary.FTPSCOTIAPrimeVars.ServerIP2 = tb_ServerIP2.Text;
            SystemLibrary.FTPSCOTIAPrimeVars.UserID = tb_UserID.Text;
            SystemLibrary.FTPSCOTIAPrimeVars.Password = tb_Password.Text;
            SystemLibrary.FTPSCOTIAPrimeVars.EMSXFileNameStartsWith = tb_EMSXFileNameStartsWith.Text;
            Interval_seconds = SystemLibrary.ToInt32(tb_Interval_seconds.Text);
            if (Interval_seconds < 60)
                Interval_seconds = 300;
            SystemLibrary.FTPSCOTIAPrimeVars.Interval_seconds = Interval_seconds;
            SystemLibrary.FTPSCOTIAPrimeVars.LastUpdate = System.DateTime.Now.AddSeconds(-Interval_seconds); // Now less Interval_seconds

            SystemLibrary.FTPSCOTIAPrimeSaveStructure();
            frm1.ScotiaPrimeConfigured = true;
            this.Close();

        } //bt_Save_Click()

        private void bt_Test_Click(object sender, EventArgs e)
        {
            // Local Variables
            String myMessage = "";
            String retVal;

            // Set the Cursor
            Cursor.Current = Cursors.WaitCursor;

            retVal = SystemLibrary.FTPTestStructure(tb_ServerIP.Text, tb_UserID.Text, tb_Password.Text);
            if (retVal.Length == 0)
            {
                myMessage = "Server URL: Test Sucessful.";
            }
            else
                myMessage = retVal;

            if (tb_ServerIP2.Text.Length > 0)
            {
                retVal = SystemLibrary.FTPTestStructure(tb_ServerIP2.Text, tb_UserID.Text, tb_Password.Text);
                if (retVal.Length == 0)
                    myMessage = myMessage + "\r\n\r\nBackup URL: Test Sucessful.";
                else
                    myMessage = myMessage + "Backup URL:\r\n\r\n" + retVal;
            }

            // reset the Cursor
            Cursor.Current = Cursors.Default;

            MessageBox.Show(myMessage, this.Text + " - Test Connection");
        } //bt_Test_Click()

        private void bt_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();

        } //bt_Cancel_Click()

        private void bt_Param_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        } //bt_Param_Cancel_Click()

        private void bt_Param_Test_Click(object sender, EventArgs e)
        {
            // Local variables
            String Checks = "";

            if (dt_System_Parameters.Columns.Count == 0)
                Checks = "No Parameters to test";
            else
            {
                DataRow[] dr = dt_System_Parameters.Select("Parameter_name='SCOTIAPrime_Path'");
                if (dr.Length < 1)
                    Checks = "Could not find 'SCOTIAPrime_Path' to check";
                else
                {
                    String myPath = SystemLibrary.ToString(dr[0]["p_string"]); 
                    if (Directory.Exists(myPath))
                    {
                        Checks = Checks + "Found '" + myPath + "'";
                        if (Directory.Exists(myPath + @"\Archive"))
                            Checks = Checks + "\r\nFound '" + myPath + @"\Archive" + "'";
                        else
                            Checks = Checks + "\r\nFAILED to Find '" + myPath + @"\Archive" + "'";
                        if (Directory.Exists(myPath + @"\OutBound"))
                            Checks = Checks + "\r\nFound '" + myPath + @"\OutBound" + "'";
                        else
                            Checks = Checks + "\r\nFAILED to Find '" + myPath + @"\OutBound" + "'";
                    }
                }
            }

            MessageBox.Show(Checks, "Parameter Tests");

        } //bt_Param_Test_Click

        private void bt_Param_Save_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("WARNING: You are about to save system configurations for Scotia Prime parameters.\r\n\r\n" +
                                "Do you wish to continue?", "Saving Scotia Prime System Parameters", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {


                for (int i = 0; i < dt_System_Parameters.Rows.Count; i++)
                {
                    /*
                     * Save changed records back to the database.
                     * - NB: This cannot be Delete or Insert
                     */
                    if (dt_System_Parameters.Rows[i].RowState == DataRowState.Modified)
                    {
                        String Parameter_name = SystemLibrary.ToString(dt_System_Parameters.Rows[i]["Parameter_name"]);
                        String P_header = SystemLibrary.ToStringForDatabase("String", dt_System_Parameters.Rows[i]["P_header"]);
                        String p_width = SystemLibrary.ToStringForDatabase("int32", dt_System_Parameters.Rows[i]["p_width"]);
                        String p_int = SystemLibrary.ToStringForDatabase("int32", dt_System_Parameters.Rows[i]["p_int"]);
                        String p_number = SystemLibrary.ToStringForDatabase("Decimal", dt_System_Parameters.Rows[i]["p_number"]);
                        String p_date = SystemLibrary.ToStringForDatabase("Date", dt_System_Parameters.Rows[i]["p_date"]);
                        String p_string = SystemLibrary.ToStringForDatabase("String", dt_System_Parameters.Rows[i]["p_string"]);

                        String mySql = "Update System_Parameters " +
                                       "Set P_header = " + P_header + ", " +
                                       "    p_width = " + p_width + ", " +
                                       "    p_int = " + p_int + ", " +
                                       "    p_number = " + p_number + ", " +
                                       "    p_date = " + p_date + ", " +
                                       "    p_string = " + p_string + " " +
                                       "Where Parameter_Name = '" + Parameter_name + "' ";
                        SystemLibrary.SQLExecute(mySql);
                    }
                }

                MessageBox.Show("Update Completed - Closing window", "Scotia Prime parameters");
                this.Close();
            }

        } //bt_Param_Save_Click()

        private void FTP_ScotiaPrime_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, frm1);

        } //FTP_ScotiaPrime_Shown()
    }
}
