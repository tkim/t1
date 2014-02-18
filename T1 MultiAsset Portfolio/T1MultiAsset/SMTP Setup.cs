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
            String mySql = "Update System_Parameters Set p_string = '" + tb_SmtpClient.Text + "' Where Parameter_Name = 'SmtpClient'";
            int myRows = SystemLibrary.SQLExecute(mySql);

            if (myRows > 0)
                MessageBox.Show("Saved.", "SMTP Setup");
            else
                MessageBox.Show("Save Failed.", "SMTP Setup");
        } //bt_Save_Click()

        private void bt_Test_Click(object sender, EventArgs e)
        {
            // Local Variables
            MailMessage mail = null;

            if (tb_SmtpClient.Text.Trim().Length==0)
            {
                MessageBox.Show("Must supply a SMTP server","Test smtp server");
                return;
            }
            
            if (tb_email.Text.Trim().Length==0)
            {
                MessageBox.Show("Must supply an email address","Test smtp server");
                return;
            }


            // TODO (5) When upgrade to .NET 4.0, then can use this
            // using (SmtpClient SmtpServer = new SmtpClient(MLPrime_SmtpClient))
            SmtpClient SmtpServer = new SmtpClient(tb_SmtpClient.Text.Trim());
            {
                SmtpServer.Port = 25;
                //SmtpServer.Credentials = new System.Net.NetworkCredential("username", "pwd");
                SmtpServer.EnableSsl = cb_SSL.Checked;
                //SmtpServer.Timeout

                // hourglass cursor
                Cursor.Current = Cursors.WaitCursor;


                // Create a new mail header record
                mail = new MailMessage();
                mail.From = new MailAddress(tb_email.Text);
                mail.To.Add(tb_email.Text);
                mail.Subject = "Test Email related to testing smtp server parameters";
                mail.Body = "This is a test";
                try
                {
                    SmtpServer.Send(mail);
                    MessageBox.Show("A Test email will be sent to the '" + tb_email.Text + "' address.", "Send email");
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

        } //bt_Test_Click()

    }
}
