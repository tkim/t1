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
    public partial class FTP_Parameters : Form
    {
        // The parent code can attach a routine to here, that will be triggered on a sucessful close
        //public delegate void Message();
        //public event Message OnMessage;

        private Form1 frm1;
        private Boolean isConfigured = true;

        public FTP_Parameters()
        {
            InitializeComponent();
        }

        private void FTP_Parameters_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            // Ask the parent form to get data from database.
            Boolean[] RetVal = frm1.FTPStructure("LOAD");
            isConfigured = RetVal[1];
            // Display the data
            tb_ServerIP.Text = SystemLibrary.FTPVars.ServerIP;
            tb_UserID.Text = SystemLibrary.FTPVars.UserID;
            tb_Password.Text = SystemLibrary.FTPVars.Password;
            tb_EMSXFileNameStartsWith.Text = SystemLibrary.FTPVars.EMSXFileNameStartsWith;
            tb_Interval_seconds.Text = SystemLibrary.FTPVars.Interval_seconds.ToString();
            tb_LastUpdate.Text = SystemLibrary.FTPVars.LastUpdate.ToShortDateString();

        } // FTP_Parameters_Load()

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
            // Now make sure EMSX fields loaded
            if (tb_EMSXFileNameStartsWith.Text.Length < 1)
                retVal = retVal + "\r\n\r\nALSO: You need to set the EMSX file path.";
            try
            {
                Interval_seconds = Convert.ToInt32(tb_Interval_seconds.Text);
            }
            catch { }
            if (Interval_seconds < 60)
                retVal = retVal + "\r\n\r\nALSO: Check Interval minimum is 60 Seconds.";

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
            SystemLibrary.FTPVars.ServerIP = tb_ServerIP.Text;
            SystemLibrary.FTPVars.UserID = tb_UserID.Text;
            SystemLibrary.FTPVars.Password = tb_Password.Text;
            SystemLibrary.FTPVars.EMSXFileNameStartsWith = tb_EMSXFileNameStartsWith.Text;
            if (Interval_seconds < 60)
                Interval_seconds = 300;
            SystemLibrary.FTPVars.Interval_seconds = Interval_seconds;
            SystemLibrary.FTPVars.LastUpdate = System.DateTime.Now.AddSeconds(-Interval_seconds); // Now less Interval_seconds

            SystemLibrary.FTPSaveStructure();
            frm1.BloombergEMSXFileConfigured = true;
            this.Close();

        } //bt_Save_Click()

        private void bt_Test_Click(object sender, EventArgs e)
        {
            // Local Variables
            String retVal;
            Int32 Interval_seconds = 0;

            // Set the Cursor
            Cursor.Current = Cursors.WaitCursor;

            retVal = SystemLibrary.FTPTestStructure(tb_ServerIP.Text, tb_UserID.Text, tb_Password.Text);
            if (retVal.Length == 0)
                retVal = "Test Sucessful.";
            // Now make sure EMSX fields loaded
            if (tb_EMSXFileNameStartsWith.Text.Length < 1)
                retVal = retVal + "\r\n\r\nALSO: You need to set the EMSX file path.";
            try
            {
                Interval_seconds = Convert.ToInt32(tb_Interval_seconds.Text);
            }
            catch{}
            if (Interval_seconds < 60)
                retVal = retVal + "\r\n\r\nALSO: Check Interval minimum is 60 Seconds.";

            // reset the Cursor
            Cursor.Current = Cursors.Default;

            MessageBox.Show(retVal, this.Text + " - Test Connection");
        } //bt_Test_Click()

        private void bt_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();

        } //bt_Cancel_Click()
    }
}
