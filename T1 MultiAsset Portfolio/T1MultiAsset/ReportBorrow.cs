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
    public partial class ReportBorrow : Form
    {
        // Global Variables
        public Form1 ParentForm1;
        DataTable dt_StockBorrow;
        private int CXLocation = 0;
        private int CYLocation = 0;

        public struct BorrowMenuStruct
        {
            public ReportBorrow myParentForm;
            public Boolean ReturnAllExcess;
            public String StockLoanAccount;
            public Boolean isReturn;
            public String myUpdate;

            public String ReturnMessage;
        }


        public ReportBorrow()
        {
            InitializeComponent();
        }

        private void ReportBorrow_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            bt_Refresh_Click(null, null);
        } //ReportBorrow_Load()

        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;

        } //FromParent()

        private void ReportBorrow_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Refresh the ActionsTab
            if (ParentForm1 != null)
                ParentForm1.LoadActionTab(true);
        } //ReportBorrow_FormClosed()

        private void ReportBorrow_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);
        } //ReportBorrow_Shown()

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            dt_StockBorrow = SystemLibrary.SQLSelectToDataTable("Exec sp_StockBorrow");
            dg_StockBorrow.DataSource = dt_StockBorrow;
            setFormat();

        }  //bt_Refresh_Click()

        private void setFormat()
        {
            SystemLibrary.SetDataGridView(dg_StockBorrow);

            for (int i = 0; i < dg_StockBorrow.Columns.Count; i++)
            {
                // Autowidth columns
                dg_StockBorrow.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }


            if (dt_StockBorrow.Rows.Count == 0)
                bt_Acknowledge.Enabled = false;
            else
            {
                bt_Acknowledge.Enabled = true;
                for (int i = 0; i < dg_StockBorrow.Rows.Count; i++)
                {
                    if (SystemLibrary.ToString(dg_StockBorrow.Rows[i].Cells["Days to ExDate"].Value).ToUpper().StartsWith("TO"))
                    {
                        // Today ot Tomorrow
                        dg_StockBorrow.Rows[i].Cells["Days to ExDate"].Style.ForeColor = Color.Red;
                        dg_StockBorrow.Rows[i].Cells["Days to ExDate"].Style.BackColor = Color.LightPink;
                    }
                    else if (SystemLibrary.ToString(dg_StockBorrow.Rows[i].Cells["Days to ExDate"].Value).Length == 6)
                    {
                        // "2 days" to "9 days"
                        dg_StockBorrow.Rows[i].Cells["Days to ExDate"].Style.ForeColor = Color.Red;
                    }
                    else
                        dg_StockBorrow.Rows[i].Cells["Days to ExDate"].Style.ForeColor = Color.Gray;

                    if (SystemLibrary.ToDouble(dg_StockBorrow.Rows[i].Cells["Div Rate %"].Value) > 100.0)
                    {
                        dg_StockBorrow.Rows[i].Cells["Div Rate %"].Style.BackColor = Color.LightPink;
                    }
                    if (SystemLibrary.ToDouble(dg_StockBorrow.Rows[i].Cells["Fee Rate %"].Value) > 2.5)
                    {
                        dg_StockBorrow.Rows[i].Cells["Fee Rate %"].Style.BackColor = Color.LightPink;
                    }
                    if (SystemLibrary.ToString(dg_StockBorrow.Rows[i].Cells["Stock Loan Account"].Value).ToUpper().Contains("UNCOVERED"))
                    {
                        dg_StockBorrow.Rows[i].Cells["Stock Loan Account"].Style.ForeColor = Color.Red;
                        dg_StockBorrow.Rows[i].Cells["Stock Loan Account"].Style.Font =
                            new System.Drawing.Font(dg_StockBorrow.DefaultCellStyle.Font.FontFamily,
                                                    dg_StockBorrow.DefaultCellStyle.Font.SizeInPoints, 
                                                    System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        //dg_StockBorrow.Rows[i].Cells["Stock Loan Account"].Style.BackColor = Color.LightPink;
                        dg_StockBorrow.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
                        dg_StockBorrow.Columns["Stock Loan Account"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                }
            }

        } //setFormat()

        private void bt_Acknowledge_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;

            if (MessageBox.Show(this, "This Acknowledgement will mark the data as processed for the day.\r\n\n" +
                          "This way the message will be removed from the [Action] tab for the rest of the day.\r\n\r\n" +
                          "Do you wish to Continue?", "Start of Day", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // hourglass cursor
                Cursor.Current = Cursors.WaitCursor;
                
                mySql = "Update ML_E52 " +
                        "Set    Reconcilled = 'Y' " +
                        "Where  BusDate = (select Max(Balance_Date) from ML_E236 where Balance_Date = Report_date)";

                SystemLibrary.SQLExecute(mySql);

                mySql = "Update ML_E50 " +
                        "Set    Reconcilled = 'Y' " +
                        "Where  BusDate = (select Max(Balance_Date) from ML_E236 where Balance_Date = Report_date)";

                SystemLibrary.SQLExecute(mySql);
                bt_Refresh_Click(null, null);
                Cursor.Current = Cursors.Default;
            }

        } //bt_Acknowledge_Click()

        private void dg_StockBorrow_Sorted(object sender, EventArgs e)
        {
            setFormat();

        } //dg_StockBorrow_Sorted()

        private void dg_StockBorrow_MouseClick(object sender, MouseEventArgs e)
        {
            CXLocation = dg_StockBorrow.Location.X + e.Location.X + 5;
            CYLocation = dg_StockBorrow.Location.Y + e.Location.Y;

        } //dg_StockBorrow_MouseClick()

        private void dg_StockBorrow_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Show the Bloomberg popup menu
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    String Ticker = SystemLibrary.ToString(dg_StockBorrow.Rows[e.RowIndex].Cells["Ticker"].Value);
                    if (Ticker.Length > 0)
                    {
                        switch (dg_StockBorrow.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.ToString())
                        {
                            case "Ticker":
                                SystemLibrary.BBGShowMenu(-1, -1, Ticker, "", SystemLibrary.BBGRelativeTicker(Ticker), this.Location.X + CXLocation, this.Location.Y + CYLocation);
                                break;
                            default:
                                Point myLocation = new Point(this.Location.X + CXLocation, this.Location.Y + CYLocation);
                                // Select the Order as needed in code later on.
                                int FundID = SystemLibrary.ToInt32(dg_StockBorrow.Rows[e.RowIndex].Cells["FundID"].Value);
                                String StockLoanAccount = SystemLibrary.ToString(dg_StockBorrow.Rows[e.RowIndex].Cells["Stock Loan Account"].Value);
                                int CanBeReturned = -SystemLibrary.ToInt32(dg_StockBorrow.Rows[e.RowIndex].Cells["Can Be Returned Today"].Value);
                                int PosUncovered = StockLoanAccount.IndexOf(" - UNCOVERED");

                                ContextMenuStrip myMenu = new ContextMenuStrip();
                                ToolStripMenuItem mySubMenu = new ToolStripMenuItem();

                                BorrowMenuStruct myBorrowStruct = new BorrowMenuStruct();
                                myBorrowStruct.myParentForm = this;
                                myBorrowStruct.isReturn = true;
                                myBorrowStruct.StockLoanAccount = StockLoanAccount;

                                if (dg_StockBorrow.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.ToString() == "Stock Loan Account" && PosUncovered > 0)
                                {
                                    // Get Borrow for this line
                                    StockLoanAccount = StockLoanAccount.Substring(0, PosUncovered);
                                    myBorrowStruct.StockLoanAccount = StockLoanAccount;
                                    CanBeReturned = SystemLibrary.ToInt32(dg_StockBorrow.Rows[e.RowIndex].Cells["Excess Stock on Trade Date"].Value);

                                    mySubMenu = new ToolStripMenuItem("Borrow " + CanBeReturned.ToString("#,##0") + " shortfall '" + Ticker + "' for account '" + StockLoanAccount + "'");
                                    myBorrowStruct.ReturnAllExcess = false;
                                    myBorrowStruct.isReturn = false;
                                    myBorrowStruct.ReturnMessage = "Please Borrow " + CanBeReturned.ToString("#,##0") + " of '" +
                                                                    Ticker + "' for stock loan account '" + StockLoanAccount + "'.";
                                    myBorrowStruct.myUpdate = "Update	ML_E50 " +
                                                              "Set	SL_Account_ID = '" + StockLoanAccount + "' " +
                                                              "WHERE	ML_E50.BusDate = (	SELECT	Max(BusDate) " +
                                                              "         	                FROM	ML_E50 " +
                                                              "			                 ) " +
                                                              "And isNull(SL_Account_ID,'') = '' " +
                                                              "And FundID = " + FundID.ToString() + " " +
                                                              "And	BBG_Ticker = '" + Ticker + "' ";
                                }
                                else
                                {
                                    // Return This Lines 
                                    mySubMenu = new ToolStripMenuItem("Return '" + Ticker + "' for account '" + StockLoanAccount + "'");
                                    myBorrowStruct.ReturnAllExcess = false;
                                    myBorrowStruct.ReturnMessage = "Please return " + CanBeReturned.ToString("#,##0") + " of '" +
                                                                    Ticker + "' for stock loan account '" + StockLoanAccount + "' for t+1 settlement.";
                                    mySubMenu.Tag = myBorrowStruct;
                                    mySubMenu.MouseUp += new MouseEventHandler(dg_StockBorrowSystemMenuItem_Click);
                                    myMenu.Items.Add(mySubMenu);

                                    // Return All Lines 
                                    mySubMenu = new ToolStripMenuItem("Return All Borrow for account '" + StockLoanAccount + "'");
                                    myBorrowStruct.ReturnAllExcess = true;
                                    myBorrowStruct.ReturnMessage = "";
                                    for (int i = 0; i < dg_StockBorrow.Rows.Count; i++)
                                    {
                                        String Row_Ticker = SystemLibrary.ToString(dg_StockBorrow.Rows[i].Cells["Ticker"].Value);
                                        int Row_FundID = SystemLibrary.ToInt32(dg_StockBorrow.Rows[i].Cells["FundID"].Value);
                                        String Row_StockLoanAccount = SystemLibrary.ToString(dg_StockBorrow.Rows[i].Cells["Stock Loan Account"].Value);
                                        int Row_CanBeReturned = -SystemLibrary.ToInt32(dg_StockBorrow.Rows[i].Cells["Can Be Returned Today"].Value);

                                        if (Row_FundID == FundID && Row_StockLoanAccount == StockLoanAccount && Row_CanBeReturned > 0)
                                        {
                                            myBorrowStruct.ReturnMessage = myBorrowStruct.ReturnMessage +
                                                                        "Please return " + Row_CanBeReturned.ToString("#,##0") + " of '" +
                                                                            Row_Ticker + "' for stock loan account '" + Row_StockLoanAccount + "' for t+1 settlement.\r\n";
                                        }
                                    }
                                }

                                mySubMenu.Tag = myBorrowStruct;
                                mySubMenu.MouseUp += new MouseEventHandler(dg_StockBorrowSystemMenuItem_Click);
                                myMenu.Items.Add(mySubMenu);

                                // Show the Menu
                                myMenu.Show(myLocation);
                                break;
                        }
                    }
                }
            }
            catch { }

        } // dg_StockBorrow_CellMouseClick()

        public static void dg_StockBorrowSystemMenuItem_Click(object sender, MouseEventArgs e)
        {
            //
            // Purpose: A generic menu click event for the Order Right-Click
            //

            // Local Variables
            String htmlBody;
            String BorrowContactNumber = "";
            ToolStripMenuItem ts_From = (ToolStripMenuItem)sender;
            BorrowMenuStruct myBorrowStruct = (BorrowMenuStruct)ts_From.Tag;
            Boolean RetVal = true;
            
            // Mail message
            MailMessage mail = null;
            String ToEmail = SystemLibrary.SQLSelectString("Select a.returnBorrowEmail From Custodian a, CustodianMap b Where b.ExtID_Loan = '" + myBorrowStruct.StockLoanAccount + "' and a.CustodianID = b.CustodianID");
            String Custodian_FromEmail = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('Custodian:FromEmail')");
            String Custodian_CCEmail = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('Custodian:CCEmail')");
            String Custodian_BCCEmail = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('Custodian:BCCEmail')");
            String Custodian_Signature = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('Custodian:Signature')");
            String Custodian_SmtpClient = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('SmtpClient')");

            if (!myBorrowStruct.isReturn)
            {
                ToEmail = SystemLibrary.SQLSelectString("Select a.BorrowEmail From Custodian a, CustodianMap b Where b.ExtID_Loan = '" + myBorrowStruct.StockLoanAccount + "' and a.CustodianID = b.CustodianID");
                myBorrowStruct.ReturnMessage = SystemLibrary.SQLSelectString("Select a.BorrowContact+',' From Custodian a, CustodianMap b Where b.ExtID_Loan = '" + myBorrowStruct.StockLoanAccount + "' and a.CustodianID = b.CustodianID") + "\r\n\r\n" +
                                               "Hi. " + myBorrowStruct.ReturnMessage;
                BorrowContactNumber = SystemLibrary.SQLSelectString("Select 'Please Phone ' + a.BorrowContact + ' on ' + a.BorrowPhone + ' to Organise / Confirm Borrow.\r\n\r\n\r\n' From Custodian a, CustodianMap b Where b.ExtID_Loan = '" + myBorrowStruct.StockLoanAccount + "' and a.CustodianID = b.CustodianID");

                // Indicate in the table that this email has happened.
                SystemLibrary.SQLExecute(myBorrowStruct.myUpdate);
            }

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            // TODO (5) When upgrade to .NET 4.0, then can use this
            // using (SmtpClient SmtpServer = new SmtpClient(Custodian_SmtpClient))
            SmtpClient SmtpServer = new SmtpClient(Custodian_SmtpClient);
            {
                SmtpServer.Port = 25;
                //SmtpServer.Credentials = new System.Net.NetworkCredential("username", "pwd");
                SmtpServer.EnableSsl = false;

                // Create a new mail header record
                mail = new MailMessage();
                mail.From = new MailAddress(Custodian_FromEmail);
                if (ToEmail.Length == 0)
                    mail.To.Add(Custodian_FromEmail);
                else
                {
                    //String 
                    foreach (String myStr in ToEmail.Split(",;".ToCharArray()))
                        mail.To.Add(myStr);
                }
                if (Custodian_CCEmail != "")
                {
                    //String 
                    foreach (String myStr in Custodian_CCEmail.Split(",;".ToCharArray()))
                        mail.CC.Add(myStr);
                }
                if (Custodian_BCCEmail != "")
                {
                    //String 
                    foreach (String myStr in Custodian_BCCEmail.Split(",;".ToCharArray()))
                        mail.Bcc.Add(myStr);
                }
                if (myBorrowStruct.isReturn)
                    mail.Subject = "Return of Stock Borrow on " + DateTime.Now.ToString("dd-MMM-yyyy") + " " + DateTime.Now.ToShortTimeString();
                else
                    mail.Subject = "Request Borrow on " + DateTime.Now.ToString("dd-MMM-yyyy") + " " + DateTime.Now.ToShortTimeString();
                mail.IsBodyHtml = true;
 
                htmlBody = SystemLibrary.HTMLStart() +
                           SystemLibrary.HTMLLine(myBorrowStruct.ReturnMessage + "\r\n") +
                           SystemLibrary.HTMLLine(Custodian_Signature) +
                           SystemLibrary.HTMLEnd();
                mail.Body = htmlBody;

                //mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.OnSuccess;
                try
                {
                    SmtpServer.Send(mail);
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
                    RetVal = false;
                    MessageBox.Show(myMessage, "Failed to Send email");
                }

                // Clean up
                mail.Dispose();
                mail = null;
            }

            // Cleanup
            Cursor.Current = Cursors.Default;
            if (RetVal)
            {
                MessageBox.Show("The following message was Sent to '" + ToEmail + "' and will appear in your inbox shortly\r\n\r\n" +
                                BorrowContactNumber +
                                myBorrowStruct.ReturnMessage, "Return Stock Borrow");
            }
            myBorrowStruct.myParentForm.bt_Refresh_Click(null, null);

        } //dg_StockBorrowSystemMenuItem_Click()

    }
}
