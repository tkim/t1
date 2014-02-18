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
    public partial class frm_DBConnect : Form
    {
        // The parent code can attach a routine to here, that will be triggered on a sucessful close
        public delegate void Message();
        public event Message OnMessage;

        public frm_DBConnect()
        {
            InitializeComponent();
        }

        private void cb_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_Microsoft_Authentification.Visible = false;
            tb_ServerName.Visible = false;
            lb_ServerName.Visible = false;
            bt_Browse.Visible = false;
            tb_DBUser.Enabled = true;
            tb_DBPwd.Enabled = true;
            switch (cb_Type.Text)
            {
                case "Oracle":
                    tb_ProviderName.Text = "OraOLEDB.Oracle";
                    break;
                case "Access":
                    tb_ProviderName.Text = "Microsoft.Jet.OLEDB.4.0";
                    bt_Browse.Visible = true;
                    break;
                case "MySql":
                    tb_ProviderName.Text = "MySqlProv";
                    break;
                case "Azure":
                    tb_ProviderName.Text = "SQLOLEDB";
                    tb_ServerName.Visible = true;
                    lb_ServerName.Visible = true;
                    break;
                default:    // "SQL Server"
                    // Default to Microsoft Sql Server
                    tb_ProviderName.Text = "SQLOLEDB";
                    cb_Microsoft_Authentification.Visible = true;
                    cb_Microsoft_Authentification_CheckedChanged(null, null);
                    tb_ServerName.Visible = true;
                    lb_ServerName.Visible = true;
                    break;
            }
        } // cb_Type_SelectedIndexChanged()

        private void bt_Browse_Click(object sender, EventArgs e)
        {
            // Open a dialogue and browse for acces files
            OpenFileDialog fileOpen = new OpenFileDialog();
            fileOpen.InitialDirectory = ".\\";
            fileOpen.Filter = "Access Database (*.mdb)|*.mdb|All files (*.*)|*.*";
            fileOpen.FilterIndex = 0;
            fileOpen.RestoreDirectory = false; //true;
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                // Set the dialogue box
                tb_DatabaseName.Text = fileOpen.FileName;
            }
        } // bt_Browse_Click()

        private void bt_Ok_Click(object sender, EventArgs e)
        {
            String myMessage = SystemLibrary.SQLTestConnection(cb_Type.Text, tb_ProviderName.Text, tb_ServerName.Text, tb_DatabaseName.Text, tb_DBUser.Text, tb_DBPwd.Text, cb_Microsoft_Authentification.Checked);
            if (myMessage.Length > 0)
            {
                if (MessageBox.Show(this, "Connection Failed with the following error.\r\n\r\n" + myMessage + "\r\n\r\n\r\nDo you wish to continue anyway?",
                                   "Test Sql Connection", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                    == DialogResult.No)
                {
                    return;
                }
            }

            // Load the SystemLibrary data
            SystemLibrary.DBVars.DataSourceName = cb_Type.Text;
            SystemLibrary.DBVars.ProviderName = tb_ProviderName.Text;
            SystemLibrary.DBVars.DatabaseName = tb_DatabaseName.Text;
            SystemLibrary.DBVars.ServerName = tb_ServerName.Text;
            SystemLibrary.DBVars.DBUser = tb_DBUser.Text;
            SystemLibrary.DBVars.DBPwd = tb_DBPwd.Text;
            SystemLibrary.DBVars.IntegratedSecurity = cb_Microsoft_Authentification.Checked;
            // - ConnString
            SystemLibrary.DBVars.ConnString = SystemLibrary.SQLSetConnString(SystemLibrary.DBVars.DataSourceName, SystemLibrary.DBVars.ProviderName, SystemLibrary.DBVars.ServerName, SystemLibrary.DBVars.DatabaseName, SystemLibrary.DBVars.DBUser, SystemLibrary.DBVars.DBPwd, SystemLibrary.DBVars.IntegratedSecurity);

            // Close the form
            this.OnMessage(); // Let the parent code know a Save has occured.
            this.Close();

        } // bt_Ok_Click()

        private void bt_Close_Click(object sender, EventArgs e)
        {
            this.Close();

        } // bt_Close_Click()

        private void bt_Test_Click(object sender, EventArgs e)
        {
            String myMessage = SystemLibrary.SQLTestConnection(cb_Type.Text, tb_ProviderName.Text, tb_ServerName.Text, tb_DatabaseName.Text, tb_DBUser.Text, tb_DBPwd.Text, cb_Microsoft_Authentification.Checked);
            if (myMessage.Length==0)
                MessageBox.Show("Connection Ok", "Test Sql Connection");
            else
                MessageBox.Show("Connection Failed.\r\n\r\n" + myMessage, "Test Sql Connection");

        } // bt_Test_Click()

        private void frm_DBConnect_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            // Force the selection of the first option
            cb_Type.SelectedIndex = 0;
            cb_Type_SelectedIndexChanged(null, null);

            // Load the SystemLibrary data
            cb_Type.Text = SystemLibrary.DBVars.DataSourceName;
            cb_Type_SelectedIndexChanged(null, null);
            tb_ProviderName.Text = SystemLibrary.DBVars.ProviderName;
            tb_DatabaseName.Text = SystemLibrary.DBVars.DatabaseName;
            tb_ServerName.Text = SystemLibrary.DBVars.ServerName;
            tb_DBUser.Text = SystemLibrary.DBVars.DBUser;
            tb_DBPwd.Text = SystemLibrary.DBVars.DBPwd;
            if (cb_Microsoft_Authentification.Visible)
                cb_Microsoft_Authentification.Checked = SystemLibrary.DBVars.IntegratedSecurity;
            else
                cb_Microsoft_Authentification.Checked = false;
            cb_Microsoft_Authentification_CheckedChanged(null, null);

        } // bt_Test_Click()

        private void cb_Microsoft_Authentification_CheckedChanged(object sender, EventArgs e)
        {
            tb_DBUser.Enabled = !cb_Microsoft_Authentification.Checked;
            tb_DBPwd.Enabled = !cb_Microsoft_Authentification.Checked;

        } // cb_Microsoft_Authentification_CheckedChanged()


    }
}
