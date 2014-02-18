using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace T1MultiAsset
{
    public partial class SplashScreen : Form
    {
        // Global Variables
        private Rectangle m_rProgress;
        private Boolean NoPanelStatus = true;
        Form1 ParentForm1;

        public SplashScreen()
        {
            InitializeComponent();
            lb_Message.Text = "";
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            // Set position
            PositionLoad();

        } //SplashScreen_Load()

        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;

        } //FromParent()



        private void PositionLoad()
        {
            // Load the position based on Registry details
            // Get the Current Screen Height & Width
            Rectangle rect = new Rectangle(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);

            foreach (Screen screen in Screen.AllScreens)
                rect = Rectangle.Union(rect, screen.Bounds);


            // See if Window parameters are in Registry. If not, then set them (USE A CENTRAL ROUTINE, SO CAn also have menu to reset)
            String myValue;
            Int32 myTop;
            Int32 myLeft;
            Int32 myWidth;
            Int32 myHeight;

            Registry.Registry myReg = new T1MultiAsset.Registry.Registry();

            myValue = myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Position", "Top").ToString();
            if (myValue.Length > 0)
            {
                // Set the default value
                myTop = SystemLibrary.ToInt32(myValue.ToString());
                if (myTop < 0)
                    myTop = 20;

                myValue = myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Position", "Left").ToString();
                myLeft = SystemLibrary.ToInt32(myValue.ToString());
                if (myLeft < 0)
                    myLeft = 20;

                myValue = myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Position", "Width").ToString();
                myWidth = SystemLibrary.ToInt32(myValue.ToString());
                if ((myLeft + myWidth) < 0) // Need to test the width not greater than 1 screen??
                    myWidth = 20;

                myValue = myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Position", "Height").ToString();
                myHeight = SystemLibrary.ToInt32(myValue.ToString());
                if ((myLeft + myHeight) < 0) // Need to test the width not greater than 1 screen??
                    myHeight = 20;

                this.Top = myTop + (myHeight - this.Height) / 2;
                this.Left = myLeft + (myWidth - this.Width) / 2;

            }
            /*
            else
            {
                myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Position", "Top", 200);
            }
             */
        } //PositionLoad()

        public delegate void SetPanelStatusCallback(Double myPCT, String myMessage);
        public void SetPanelStatus(Double myPCT, String myMessage)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                SetPanelStatusCallback cb = new SetPanelStatusCallback(SetPanelStatus);
                this.Invoke(cb, new object[] { myPCT, myMessage });
            }
            else
            {

                int width = (int)Math.Floor(pnl_Status.ClientRectangle.Width * myPCT);
                int height = pnl_Status.ClientRectangle.Height;
                int x = pnl_Status.ClientRectangle.X;
                int y = pnl_Status.ClientRectangle.Y;
                if (width > 0 && height > 0 ) //&& NoPanelStatus == true)
                {
                    NoPanelStatus = false;
                    m_rProgress = new Rectangle(x, y, width, height);
                }
                lb_Message.Text = myMessage;
                Application.DoEvents();
                pnl_Status.Invalidate(m_rProgress);
                Application.DoEvents();
            }
        }

        private void SplashScreen_DoubleClick(object sender, EventArgs e)
        {
            // Close the form on double-click
            this.Close();
        }

        private void pnl_Status_Paint(object sender, PaintEventArgs e)
        {
            if (NoPanelStatus == false && e.ClipRectangle.Width > 0)
            {
                LinearGradientBrush brBackground = new LinearGradientBrush(m_rProgress, Color.FromArgb(58, 96, 151), Color.FromArgb(181, 237, 254), LinearGradientMode.Horizontal);
                e.Graphics.FillRectangle(brBackground, m_rProgress);
            }

        } //pnl_Status_Paint()

        private void SplashScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
           /*
           if (ParentForm1 != null)
           {
               ParentForm1.ResetHiddenRows();
               //ParentForm1.LoadPortfolioIncr();
               //ParentForm1.LoadActionTab();
           }
            */
        } //SplashScreen_FormClosed()

    }
}
