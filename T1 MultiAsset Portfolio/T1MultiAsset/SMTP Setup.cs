using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;

namespace T1MultiAsset
{
    public partial class SMTPSetup : Form
    {
        // Public Variables
        public Form1 ParentForm1;

        
        public SMTPSetup()
        {
            InitializeComponent();
        }

        private void SMTPSetup_Load(object sender, EventArgs e)
        {
            tb_SmtpClient.Text = SystemLibrary.SQLSelectString("select dbo.f_GetParamString('SmtpClient')");
            cb_SSL.Checked = SystemLibrary.SQLSelectYN_To_Bool("select dbo.f_GetParamString('SmtpClient_SSL')");
            tb_Port.Text = SystemLibrary.SQLSelectString("select dbo.f_GetParamString('SmtpClient_Port')");
            tb_UserName.Text = SystemLibrary.SQLSelectString("select dbo.f_GetParamString('SmtpClient_UserName')");
            tb_Password.Text = SystemLibrary.SQLSelectString("select dbo.f_GetParamString('SmtpClient_Password')");
            tb_SmtpClient_Secondary.Text = SystemLibrary.SQLSelectString("select dbo.f_GetParamString('SmtpClient_Secondary')");
            cb_SSL_Secondary.Checked = SystemLibrary.SQLSelectYN_To_Bool("select dbo.f_GetParamString('SmtpClient_SSL_Secondary')");
            tb_Port_Secondary.Text = SystemLibrary.SQLSelectString("select dbo.f_GetParamString('SmtpClient_Port_Secondary')");
            tb_UserName_Secondary.Text = SystemLibrary.SQLSelectString("select dbo.f_GetParamString('SmtpClient_UserName_Secondary')");
            tb_Password_Secondary.Text = SystemLibrary.SQLSelectString("select dbo.f_GetParamString('SmtpClient_Password_Secondary')");
        } //SMTPSetup_Load()

        public void Startup(Form inForm)
        {
            ParentForm1 = (Form1)inForm;
        } //Startup()

        private void SMTPSetup_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);
        } //SMTPSetup_Shown()

        private void bt_Save_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;
            int myRows;

            // SmtpClient
            mySql = "Update System_Parameters Set p_string = '" + tb_SmtpClient.Text + "' Where Parameter_Name = 'SmtpClient'";
            myRows = SystemLibrary.SQLExecute(mySql);
            if (myRows == -1)
            {
                mySql = "Insert Into System_Parameters (Parameter_Name, p_string) Select 'SmtpClient','" + tb_SmtpClient.Text + "' Where Not Exists (Select 'x' From System_Parameters Where Parameter_Name = 'SmtpClient')";
                myRows = SystemLibrary.SQLExecute(mySql);
            }

            // SmtpClient_SSL
            mySql = "Update System_Parameters Set p_string = '" + SystemLibrary.Bool_To_YN(cb_SSL.Checked) + "' Where Parameter_Name = 'SmtpClient_SSL'";
            myRows = SystemLibrary.SQLExecute(mySql);
            if (myRows < 1)
            {
                mySql = "Insert Into System_Parameters (Parameter_Name, p_string) Select 'SmtpClient_SSL','" + SystemLibrary.Bool_To_YN(cb_SSL.Checked) + "' Where Not Exists (Select 'x' From System_Parameters Where Parameter_Name = 'SmtpClient_SSL')";
                myRows = SystemLibrary.SQLExecute(mySql);
            }

            // SmtpClient_Port
            mySql = "Update System_Parameters Set p_string = '" + tb_Port.Text + "' Where Parameter_Name = 'SmtpClient_Port'";
            myRows = SystemLibrary.SQLExecute(mySql);
            if (myRows < 1)
            {
                mySql = "Insert Into System_Parameters (Parameter_Name, p_string) Select 'SmtpClient_Port','" + tb_Port.Text + "' Where Not Exists (Select 'x' From System_Parameters Where Parameter_Name = 'SmtpClient_Port')";
                myRows = SystemLibrary.SQLExecute(mySql);
            }

            // SmtpClient_UserName
            mySql = "Update System_Parameters Set p_string = '" + tb_UserName.Text + "' Where Parameter_Name = 'SmtpClient_UserName'";
            myRows = SystemLibrary.SQLExecute(mySql);
            if (myRows < 1)
            {
                mySql = "Insert Into System_Parameters (Parameter_Name, p_string) Select 'SmtpClient_UserName','" + tb_UserName.Text + "' Where Not Exists (Select 'x' From System_Parameters Where Parameter_Name = 'SmtpClient_UserName')";
                myRows = SystemLibrary.SQLExecute(mySql);
            }

            // SmtpClient_Password
            mySql = "Update System_Parameters Set p_string = '" + tb_Password.Text + "' Where Parameter_Name = 'SmtpClient_Password'";
            myRows = SystemLibrary.SQLExecute(mySql);
            if (myRows < 1)
            {
                mySql = "Insert Into System_Parameters (Parameter_Name, p_string) Select 'SmtpClient_Password','" + tb_Password.Text + "' Where Not Exists (Select 'x' From System_Parameters Where Parameter_Name = 'SmtpClient_Password')";
                myRows = SystemLibrary.SQLExecute(mySql);
            }

            // SmtpClient_Secondary
            mySql = "Update System_Parameters Set p_string = '" + tb_SmtpClient_Secondary.Text + "' Where Parameter_Name = 'SmtpClient_Secondary'";
            myRows = SystemLibrary.SQLExecute(mySql);
            if (myRows < 1)
            {
                mySql = "Insert Into System_Parameters (Parameter_Name, p_string) Select 'SmtpClient_Secondary','" + tb_SmtpClient_Secondary.Text + "' Where Not Exists (Select 'x' From System_Parameters Where Parameter_Name = 'SmtpClient_Secondary')";
                myRows = SystemLibrary.SQLExecute(mySql);
            }

            // SmtpClient_SSL_Secondary
            mySql = "Update System_Parameters Set p_string = '" + SystemLibrary.Bool_To_YN(cb_SSL_Secondary.Checked) + "' Where Parameter_Name = 'SmtpClient_SSL_Secondary'";
            myRows = SystemLibrary.SQLExecute(mySql);
            if (myRows < 1)
            {
                mySql = "Insert Into System_Parameters (Parameter_Name, p_string) Select 'SmtpClient_SSL_Secondary','" + SystemLibrary.Bool_To_YN(cb_SSL_Secondary.Checked) + "' Where Not Exists (Select 'x' From System_Parameters Where Parameter_Name = 'SmtpClient_SSL_Secondary')";
                myRows = SystemLibrary.SQLExecute(mySql);
            }

            // SmtpClient_Port_Secondary
            mySql = "Update System_Parameters Set p_string = '" + tb_Port_Secondary.Text + "' Where Parameter_Name = 'SmtpClient_Port_Secondary'";
            myRows = SystemLibrary.SQLExecute(mySql);
            if (myRows < 1)
            {
                mySql = "Insert Into System_Parameters (Parameter_Name, p_string) Select 'SmtpClient_Port_Secondary','" + tb_Port_Secondary.Text + "' Where Not Exists (Select 'x' From System_Parameters Where Parameter_Name = 'SmtpClient_Port_Secondary')";
                myRows = SystemLibrary.SQLExecute(mySql);
            }

            // SmtpClient_UserName_Secondary
            mySql = "Update System_Parameters Set p_string = '" + tb_UserName_Secondary.Text + "' Where Parameter_Name = 'SmtpClient_UserName_Secondary'";
            myRows = SystemLibrary.SQLExecute(mySql);
            if (myRows < 1)
            {
                mySql = "Insert Into System_Parameters (Parameter_Name, p_string) Select 'SmtpClient_UserName_Secondary','" + tb_UserName_Secondary.Text + "' Where Not Exists (Select 'x' From System_Parameters Where Parameter_Name = 'SmtpClient_UserName_Secondary')";
                myRows = SystemLibrary.SQLExecute(mySql);
            }

            // SmtpClient_Password_Secondary
            mySql = "Update System_Parameters Set p_string = '" + tb_Password_Secondary.Text + "' Where Parameter_Name = 'SmtpClient_Password_Secondary'";
            myRows = SystemLibrary.SQLExecute(mySql);
            if (myRows < 1)
            {
                mySql = "Insert Into System_Parameters (Parameter_Name, p_string) Select 'SmtpClient_Password_Secondary','" + tb_Password_Secondary.Text + "' Where Not Exists (Select 'x' From System_Parameters Where Parameter_Name = 'SmtpClient_Password_Secondary')";
                myRows = SystemLibrary.SQLExecute(mySql);
            }

            MessageBox.Show("Saved.", "SMTP Setup");

        } //bt_Save_Click()

        private void bt_Test_Click(object sender, EventArgs e)
        {
            TestSMTP(tb_email.Text, tb_SmtpClient.Text, cb_SSL.Checked, tb_Port.Text, tb_UserName.Text, tb_Password.Text);
        } //bt_Test_Click()

        private void bt_Test_Secondary_Click(object sender, EventArgs e)
        {
            TestSMTP(tb_email.Text, tb_SmtpClient_Secondary.Text, cb_SSL_Secondary.Checked, tb_Port_Secondary.Text, tb_UserName_Secondary.Text, tb_Password_Secondary.Text);
        } //bt_Test_Click()


        private void TestSMTP(String inEmail, String inSMTP, Boolean inSSL, String inPort, String inUsername, String inPassword)
        {
            // Local Variables
            MailMessage mail = null;

            if (inSMTP.Trim().Length == 0)
            {
                MessageBox.Show("Must supply a SMTP server", "Test smtp server");
                return;
            }

            if (inEmail.Trim().Length == 0)
            {
                MessageBox.Show("Must supply an email address", "Test smtp server");
                return;
            }


            // TODO (5) When upgrade to .NET 4.0, then can use this
            // using (SmtpClient SmtpServer = new SmtpClient(MLPrime_SmtpClient))
            SmtpClient SmtpServer = new SmtpClient(inSMTP.Trim());
            {
                if (inPort.Trim().Length > 0)
                    SmtpServer.Port = SystemLibrary.ToInt32(inPort);
                else
                    SmtpServer.Port = 25;
                if (inUsername.Trim().Length > 0 && inPassword.Trim().Length > 0)
                    SmtpServer.Credentials = new System.Net.NetworkCredential(inUsername, inPassword);

                SmtpServer.EnableSsl = inSSL;
                //SmtpServer.Timeout

                // hourglass cursor
                Cursor.Current = Cursors.WaitCursor;


                // Create a new mail header record
                mail = new MailMessage();
                mail.From = new MailAddress(inEmail);
                mail.To.Add(inEmail);
                mail.Subject = "Test Email related to testing smtp server parameters [" + inSMTP + "," + inSSL.ToString() + "]";
                mail.Body = "This is a test";
                try
                {
                    SmtpServer.Send(mail);
                    MessageBox.Show("A Test email will be sent to the '" + inEmail + "' address.", "Send email");
                }
                catch (Exception em)
                {
                    Exception CheckMessage = em;
                    String myMessage = CheckMessage.Message;

                    while (CheckMessage.InnerException != null)
                    {
                        CheckMessage = CheckMessage.InnerException;
                        myMessage = myMessage + "\r\n" + CheckMessage.Message;
                    }
                    MessageBox.Show(myMessage, "Failed to Send email");
                }

                // Clean up
                mail.Dispose();
                mail = null;

            }

            Cursor.Current = Cursors.Default;

        } //TestSMTP()
        
    } 
}
