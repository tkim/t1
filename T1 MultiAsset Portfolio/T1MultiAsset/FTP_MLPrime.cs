using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace T1MultiAsset
{
    public partial class FTP_MLPrime : Form
    {
        // The parent code can attach a routine to here, that will be triggered on a sucessful close
        //public delegate void Message();
        //public event Message OnMessage;

        private Form1 frm1;

        public FTP_MLPrime()
        {
            InitializeComponent();
        }

        private void FTP_MLPrime_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            // Ask the parent form to get data from database.
            frm1.FTPMLPrimeStructure("LOAD");
            // Display the data
            tb_ServerIP.Text = SystemLibrary.FTPMLPrimeVars.ServerIP;
            tb_ServerIP2.Text = SystemLibrary.FTPMLPrimeVars.ServerIP2;
            tb_UserID.Text = SystemLibrary.FTPMLPrimeVars.UserID;
            tb_Password.Text = SystemLibrary.FTPMLPrimeVars.Password;
            tb_EMSXFileNameStartsWith.Text = SystemLibrary.FTPMLPrimeVars.EMSXFileNameStartsWith;
            tb_Interval_seconds.Text = SystemLibrary.FTPMLPrimeVars.Interval_seconds.ToString();
            tb_LastUpdate.Text = SystemLibrary.FTPMLPrimeVars.LastUpdate.ToShortDateString();

        } // FTP_MLPrime_Load()

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
            SystemLibrary.FTPMLPrimeVars.ServerIP = tb_ServerIP.Text;
            SystemLibrary.FTPMLPrimeVars.ServerIP2 = tb_ServerIP2.Text;
            SystemLibrary.FTPMLPrimeVars.UserID = tb_UserID.Text;
            SystemLibrary.FTPMLPrimeVars.Password = tb_Password.Text;
            SystemLibrary.FTPMLPrimeVars.EMSXFileNameStartsWith = tb_EMSXFileNameStartsWith.Text;
            Interval_seconds = SystemLibrary.ToInt32(tb_Interval_seconds.Text);
            if (Interval_seconds < 60)
                Interval_seconds = 300;
            SystemLibrary.FTPMLPrimeVars.Interval_seconds = Interval_seconds;
            SystemLibrary.FTPMLPrimeVars.LastUpdate = System.DateTime.Now.AddSeconds(-Interval_seconds); // Now less Interval_seconds

            SystemLibrary.FTPMLPrimeSaveStructure();
            frm1.MLPrimeConfigured = true;
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
    }
}
