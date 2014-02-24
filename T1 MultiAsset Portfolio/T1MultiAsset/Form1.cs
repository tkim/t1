/*
 * Notes:
 * 1) PREV_CLOSE_VAL - Needs to come from the database if existing holding
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices; // Registry services
using System.Reflection; // RTD
using System.Threading;  // Threads
using PrintDataGrid;
using OvernightPrices;
using System.Net.Mail;



/*
 *  Extract from Bloomberg examples - Start
 */
using Event = Bloomberglp.Blpapi.Event;
using Message = Bloomberglp.Blpapi.Message;
using Element = Bloomberglp.Blpapi.Element;
using Name = Bloomberglp.Blpapi.Name;
using Request = Bloomberglp.Blpapi.Request;
using Service = Bloomberglp.Blpapi.Service;
using Session = Bloomberglp.Blpapi.Session;
using SessionOptions = Bloomberglp.Blpapi.SessionOptions;
using StopOption = Bloomberglp.Blpapi.Session.StopOption;
using EventHandler = Bloomberglp.Blpapi.EventHandler;
using CorrelationID = Bloomberglp.Blpapi.CorrelationID;
using Subscription = Bloomberglp.Blpapi.Subscription;
/*
 *  Extract from Bloomberg examples - End
 */




namespace T1MultiAsset
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("Shell32.dll")]
        public extern static int ExtractIconEx(string libName, int iconIndex, IntPtr[] largeIcon, IntPtr[] smallIcon, int nIcons);
        ImageList il_icons = new ImageList();

       

        public Hashtable LoadedUndelying = new Hashtable();

        /*
         *  Extract from Bloomberg examples - End
         */

        public int DebugLevel = 0;

        // Variable to stop multiple CheckForBloombergEMSXFile threads
        public Boolean isCheckForBloombergEMSXFileRunning = false;
        public Boolean isBloombergUser = true;
        public Boolean isBloombergUser1 = true;

        // Flags for file access, etc - to reduce unneeded calls to database & directories
        public Boolean BloombergEMSXFileConfigured = true; // Must start as True
        public Boolean ScotiaPrimeConfigured = true; // Must start as True
        public Boolean MLPrimeConfigured = true; // Must start as True
        public Boolean MLFuturesConfigured = true; // Must start as True
        
        public SetValueCallback m_SetValueCallback;
        //object m_locker;
        //Semaphore _sem = new Semaphore(1, 1);

        Thread m_EMSX_WorkerThread;
        // events used to stop worker thread
        ManualResetEvent m_EMSX_EventStopThread;
        ManualResetEvent m_EMSX_EventThreadStopped;
        EMSX_API EMSX_API;

        Thread m_Bloomberg_Realtime_WorkerThread;
        // events used to stop worker thread
        ManualResetEvent m_Bloomberg_Realtime_EventStopThread;
        ManualResetEvent m_Bloomberg_Realtime_EventThreadStopped;
        Bloomberg_Realtime BRT;
        SortedList<String, Color> TickerList = new SortedList<String, Color>();

        //Local Variables
        public DataTable dt_Fund;
        public DataTable dt_Portfolio;
        public DataTable dt_PortfolioTranspose;
        public DataTable dt_Securities;
        public DataView myDataView;

        public DataTable dt_Port;
        public DataTable dt_Port_Tabs;
        public DataTable dt_Port_Tab_Detail;
        public DataGridViewCellStyle dg_Port_AltStyle = new System.Windows.Forms.DataGridViewCellStyle();
        public DateTime LastUpdated; // Used for dt_Port background calls to Update table if needed.
        private Boolean inStartUp = true;
        public DataTable dt_FX = new DataTable();
        public DataTable dt_Action = new DataTable();
        public DataTable dt_Last_Price = new DataTable();

        public DataTable GrossPctTable = new DataTable();

        public Boolean isAlive_PortfolioTranspose = false;

        public String DatabaseVersion = "";


        // Fund Globals
        public int FundID;
        public Boolean ShowFutureHeaderLine = true;
        public String Fund_Name;
        public Decimal Fund_Amount;
        public Decimal Gross_Amount;
        public Decimal Long_Amount;
        public Decimal Short_Amount;
        public Decimal Future_Amount;
        public Decimal Long_Amount_Filled;
        public Decimal Short_Amount_Filled;
        public Decimal Future_Amount_Filled;
        public int Long_Positions;
        public int Short_Positions;
        public int Future_Positions;
        public Decimal Long_PL_MTD = 0;
        public Decimal Short_PL_MTD = 0;
        public Decimal Future_PL_MTD = 0;
        public Decimal Long_PL_Yest = 0;
        public Decimal Short_PL_Yest = 0;
        public Decimal Future_PL_Yest = 0;
        public Decimal Long_PL_WRoll = 0;
        public Decimal Short_PL_WRoll = 0;
        public Decimal Future_PL_WRoll = 0;
        public Decimal Long_PL_MRoll = 0;
        public Decimal Short_PL_MRoll = 0;
        public Decimal Future_PL_MRoll = 0;
        public Decimal Long_PL_DeltaMax = 0;
        public Decimal Short_PL_DeltaMax = 0;
        public Decimal Future_PL_DeltaMax = 0;
        public Decimal Long_PL_Inception = 0;
        public Decimal Short_PL_Inception = 0;
        public Decimal Future_PL_Inception = 0;
        public Decimal Long_PL_YTD = 0;
        public Decimal Short_PL_YTD = 0;
        public Decimal Future_PL_YTD = 0;
        public Decimal Long_PL_YTD_July = 0;
        public Decimal Short_PL_YTD_July = 0;
        public Decimal Future_PL_YTD_July = 0;
        public Decimal Long_PL;
        public Decimal Short_PL;
        public Decimal Future_PL;
        public Decimal BPS_PL_MTD = 0;
        public Decimal BPS_PL_Yest = 0;
        public Decimal BPS_PL_WRoll = 0;
        public Decimal BPS_PL_MRoll = 0;
        public Decimal BPS_PL_DeltaMax = 0;
        public Decimal BPS_PL_Inception = 0;
        public Decimal BPS_PL_YTD = 0;
        public Decimal BPS_PL_YTD_July = 0;

        public String BPS_Index_Ticker = "";
        public Decimal BPS_Index_Close = 1;
        public Decimal BPS_Index_Prev_Close = 1; // have made non-zero, so I can divide by.
        public Decimal BPS_Index_DIV_TODAY = 0;
        public Decimal BPS_Index_MTD = 0;
        public Decimal BPS_Index_Yest = 0;
        public Decimal BPS_Index_WRoll = 0;
        public Decimal BPS_Index_MRoll = 0;
        public Decimal BPS_Index_DeltaMax = 0;
        public Decimal BPS_Index_Inception = 0;
        public Decimal BPS_Index_YTD = 0;
        public Decimal BPS_Index_YTD_July = 0;

        
        public int Long_Winners;
        public int Short_Winners;
        public int Future_Winners;
        public String Fund_Crncy = "USD"; // Needs some default
        // Portfolio Globals
        public int PortfolioID;
        public String Portfolio_Name;
        public Decimal Portfolio_Amount; // ??? Real ??
        public String Portfolio_Crncy; // ??? Real ??

        // dg_Port
        private object LastValue;
        private Boolean inEditMode = false;
        private int CXLocation = 0;
        private int CYLocation = 0;

        // I need these flags for hidden rows
        private String flag_HideAggregateRows = "";

        // dg_Port Drag flag
        Boolean inDragMode = false;

        // Pre Market Prices flag
        Boolean UseTheo_Price = false;

        // Splash screen
        SplashScreen frm_Splash;
        Boolean CloseForm = false;

        // Process Creation DateTime
        DateTime ProcessDateTime;

        /*
        public class CommandKeyInterceptingDataGridView : DataGridView
        {
            protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
            {
                Debug.WriteLine(keyData);
                return base.ProcessCmdKey(ref msg, keyData);
            }
        } 
        */

        public Form1()
        {
            // Local Variables

            //SystemLibrary.SetDebugLevel(4);//CFR 20140130

            // Open the splash screen
            frm_Splash = new SplashScreen();
            frm_Splash.FromParent(this);
            frm_Splash.Show();
            Application.DoEvents();

            InitializeComponent();
            dg_Port_AltStyle = dg_Port.AlternatingRowsDefaultCellStyle;
            isBloombergUserLoad();
            isBloombergUser1Load();
            //isBloombergUser = false; //CFR 20140130

            // initialize events
            m_SetValueCallback = new SetValueCallback(this.SetValue);
            //m_locker = new object();
            m_EMSX_EventStopThread = new ManualResetEvent(false);
            m_EMSX_EventThreadStopped = new ManualResetEvent(false);

            m_Bloomberg_Realtime_EventStopThread = new ManualResetEvent(false);
            m_Bloomberg_Realtime_EventThreadStopped = new ManualResetEvent(false);

            // Load Database parameter
            if (!SystemLibrary.SQLLoadConnectParams())
            {
                // First Time, so get some parameters from the user.
                frm_DBConnect f = new frm_DBConnect();
                f.OnMessage += new frm_DBConnect.Message(SystemLibrary.SQLSaveConnectParams);
                f.ShowDialog(this);
                if (!SystemLibrary.SQLLoadConnectParams())
                {
                    MessageBox.Show("Closing application until database parameters are set", this.Text);
                    CloseForm = true;
                }
            }
            // Set the database timezone offset
            SystemLibrary.SetTimeZoneOffset();
            DatabaseVersion = SystemLibrary.SQLSelectString("Select @@VERSION");

            if (CloseForm)
            {
                frm_Splash.Close();
                this.Close();
            }
            else
            {
                frm_Splash.SetPanelStatus(0.20, "Loading Tabs...");
                LoadTabs();

                // Load basic Data
                frm_Splash.SetPanelStatus(0.40, "Loading Fund...");
                LoadPortfolioGroup();

                SendToBloomberg.InitDataTable();

                frm_Splash.SetPanelStatus(0.50, "Starting Bloomberg EMSX connection...");

                // create worker thread instance
                m_EMSX_EventStopThread.Reset();
                m_EMSX_EventThreadStopped.Reset();
                m_EMSX_WorkerThread = new Thread(new ThreadStart(this.EMSX_WorkerThreadFunction));
                m_EMSX_WorkerThread.Name = "EMSX API Processsing";
                m_EMSX_WorkerThread.Start();

                // create worker thread instance
                m_Bloomberg_Realtime_EventStopThread.Reset();
                m_Bloomberg_Realtime_EventThreadStopped.Reset();
                m_Bloomberg_Realtime_WorkerThread = new Thread(new ThreadStart(this.Bloomberg_Realtime_WorkerThreadFunction));
                m_Bloomberg_Realtime_WorkerThread.Name = "Bloomberg_Realtime Processsing";
                m_Bloomberg_Realtime_WorkerThread.Start();

            }
        }

        public void FromParent(DateTime inProcessDateTime)
        {
            ProcessDateTime = inProcessDateTime;
            Console.WriteLine("Process DateTime [UTC]: " + TimeZoneInfo.ConvertTimeToUtc(inProcessDateTime).ToString("dd-MMM-yyyy HH:mm:ss"));
        } //FromParent()


        private void EMSX_WorkerThreadFunction()
        {
            String EMSX_TEAM = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('EMSX_TEAM')");
            EMSX_API = new EMSX_API(m_EMSX_EventStopThread, m_EMSX_EventThreadStopped, this, EMSX_TEAM);
            EMSX_API.SetUpEMSX_API();

        } //EMSX_WorkerThreadFunction()

        private void Bloomberg_Realtime_WorkerThreadFunction()
        {
            BRT = new Bloomberg_Realtime(m_Bloomberg_Realtime_EventStopThread, m_Bloomberg_Realtime_EventThreadStopped, this);
            BRT.SetUpBloomberg_Realtime();

        } //Bloomberg_Realtime_WorkerThreadFunction()

        private void Form1_Load(object sender, EventArgs e)
        {
            if (CloseForm)
            {
                this.Close();
            }
            else
            {
                this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

                if (SystemLibrary.GetUserName() == "Colin Ritchie")
                    button4.Visible = true;
                else
                    button4.Visible = false;

                button4.Visible = true;

                // SOme objects Need to be called after the form has been created. Hence the Load event.
                PositionLoad();
                frm_Splash.Top = this.Top + this.Height / 2 - frm_Splash.Height / 2;
                frm_Splash.Left = this.Left + this.Width / 2 - frm_Splash.Width / 2; ;

                frm_Splash.SetPanelStatus(0.7, "Setting up data...");
                SetUpFX_DataTable();
                SetUpHeader();

                // Get FundID & PortfolioID from the Registry
                Registry.Registry myReg = new T1MultiAsset.Registry.Registry();
                if (!int.TryParse(SystemLibrary.ToString(myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\StartUp", "FundID")), out FundID))
                    FundID = -1;
                if (!int.TryParse(SystemLibrary.ToString(myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\StartUp", "PortfolioID")), out PortfolioID))
                    PortfolioID = -1;

                // Deal with times when there is no database yet.
                try
                {
                    // Need to find FundID in cb_Fund && PortfolioID in cb_Portfo
                    DataRow[] dr_Find = dt_Fund.Select("FundId=" + FundID.ToString());
                    if (dr_Find.Length > 0)
                        cb_Fund.SelectedValue = FundID;
                    // Now do Portfolio
                    LoadFund(PortfolioID);
                    DataRow[] dr_FindP = dt_Portfolio.Select("PortfolioId=" + PortfolioID.ToString());
                    if (dr_FindP.Length > 0)
                        cb_Portfolio.SelectedValue = PortfolioID;

                    frm_Splash.SetPanelStatus(0.90, "Loading Portfolio...");
                    cb_Portfolio_SelectionChangeCommitted(null, null);
                    cb_Fund_SelectionChangeCommitted(null, null);
                }
                catch { }

                // Setup the menu used for Sending commands to Bloomberg
                SystemLibrary.BBGSetUpMenu(this);
            }
        } //Form1_Load()

        private void Form1_Shown(object sender, EventArgs e)
        {
            // Loading the Fund & Portfolio will trigger a Portfolio Load. Avoid until ready
            inStartUp = false;
            SetUpSecurities_DataTable();
            LoadPortfolio(true);
            /* CFR 20140203
            if (LoadPortfolio(true))
                ResetHiddenRows();
            */
            // Select the first non-System tab
            for (int i = 0; i < tabControl_Port.TabCount; i++)
            {
                if (SystemLibrary.ToString(tabControl_Port.TabPages[i].Tag) != "SYSTEM")
                {
                    tabControl_Port.SelectTab(i);
                    break;
                }
            }

            // Close the splash screen
            if (frm_Splash != null)
            {
                frm_Splash.Close();
                Application.DoEvents();
            }


            if (EMSX_API != null)
            {
                EMSX_API.EMSX_APIReady();
                if (!EMSX_API.Connected)
                    EMSX_API.EMSX_APIConnect();
            }


            // Fire up the background Timer
            //timer_LoadPortfolio.Enabled = false;
            timer_LoadPortfolio.Enabled = true;
            timer_LoadPortfolio.Interval = 60000; //Every 60 seconds
            timer_LoadPortfolio.Start();

            // Fire up the background Timer for Saving Prices
            timer_SavePrices.Enabled = true;
            timer_SavePrices.Interval = 300000; //Every 300 seconds
            timer_SavePrices.Start();

        } //Form1_Shown()



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                SystemLibrary.DebugLine("Form1 Closing event");
                Bloomberg_Realtime.Set_isClosing(true);
                if (this.WindowState != FormWindowState.Minimized)
                    PositionSave();
                m_Bloomberg_Realtime_EventStopThread.Set();
                //Bloomberg_Realtime.Bloomberg_RealtimeDisconnect();
                timer_LoadPortfolio.Stop();
                timer_SavePrices.Stop();
                // set event "Stop"
                m_EMSX_EventStopThread.Set();
                Application.DoEvents();
                Application.DoEvents();
            }
            catch { }
        } // Form1_FormClosing()

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Make sure all windows & threads get told to close.
            try
            {
                Application.Exit();
            }
            catch
            { 
            }

        }  // Form1_FormClosed()

        private void isBloombergUserSave()
        {
            Registry.Registry myReg = new T1MultiAsset.Registry.Registry();

            // Save the position based on Registry details
            myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset", "isBloombergUser", SystemLibrary.Bool_To_YN(isBloombergUserToolStripMenuItem.Checked));

        } //isBloombergUserSave()

        private void isBloombergUserLoad()
        {
            // Local Variables
            String myValue = "";

            Registry.Registry myReg = new T1MultiAsset.Registry.Registry();

            myValue = myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset", "isBloombergUser").ToString();
            if (myValue.Length > 0)
                isBloombergUserToolStripMenuItem.Checked = SystemLibrary.YN_To_Bool(myValue);
            else
            {
                isBloombergUserToolStripMenuItem.Checked = true;
                isBloombergUserSave();
            }
            isBloombergUser = isBloombergUserToolStripMenuItem.Checked;
        } //isBloombergUserLoad()

        private void isBloombergUser1Save()
        {
            Registry.Registry myReg = new T1MultiAsset.Registry.Registry();

            // Save the position based on Registry details
            myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset", "isBloombergUser1", SystemLibrary.Bool_To_YN(isBloombergUser1ToolStripMenuItem.Checked));

        } //isBloombergUser1Save()

        private void isBloombergUser1Load()
        {
            // Local Variables
            String myValue = "";

            Registry.Registry myReg = new T1MultiAsset.Registry.Registry();

            myValue = myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset", "isBloombergUser1").ToString();
            if (myValue.Length > 0)
                isBloombergUser1ToolStripMenuItem.Checked = SystemLibrary.YN_To_Bool(myValue);
            else
            {
                isBloombergUser1ToolStripMenuItem.Checked = true;
                isBloombergUser1Save();
            }
            isBloombergUser1 = isBloombergUser1ToolStripMenuItem.Checked;
        } //isBloombergUser1Load()


        private void FontSizeSave()
        {
            Registry.Registry myReg = new T1MultiAsset.Registry.Registry();

            // Save the Font based on Registry details
            myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Position", "Font", this.Font.SizeInPoints);
        } //FontSizeSave()

        private void FontSizeLoad()
        {
            String myValue;
            float myFontSize;
            //return;

            Registry.Registry myReg = new T1MultiAsset.Registry.Registry();
            myValue = myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Position", "Font").ToString();
            if (myValue.Length > 0)
            {
                // Set the default value
                myFontSize = float.Parse(myValue.ToString());
                // Bacause controls may have differnt sizes, I am looping on ChangeFontSize()
                this.SuspendLayout();
                try
                {
                    if (myFontSize > this.Font.SizeInPoints)
                    {
                        do
                        {
                            ChangeFontSize("Up");
                        } while (myFontSize > this.Font.SizeInPoints && this.Font.SizeInPoints < 14.0f);
                    }
                    else if (myFontSize < this.Font.SizeInPoints)
                    {
                        do
                        {
                            ChangeFontSize("Down");
                        } while (myFontSize > this.Font.SizeInPoints && this.Font.SizeInPoints > 6.0f);
                    }
                }
                catch { }
                this.ResumeLayout(true);
            }
            else
            {
                myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Position", "Font", this.Font.SizeInPoints);
            }
        } //FontSizeLoad()


        private void PositionSave()
        {
            Registry.Registry myReg = new T1MultiAsset.Registry.Registry();

            // Save the position based on Registry details
            myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Position", "Top", this.Top);
            myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Position", "Left", this.Left);
            myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Position", "Width", this.Width);
            myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Position", "Height", this.Height);
        } //PositionSave()

        private void PositionLoad()
        {
            
            // Load the font size first as this seems to impact position & size
            FontSizeLoad();

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
                if (myTop < rect.Y)
                    myTop = rect.Y + 20;
                this.Top = myTop;


                myValue = myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Position", "Left").ToString();
                myLeft = SystemLibrary.ToInt32(myValue.ToString());
                if (myLeft < rect.X || myLeft > (rect.X + rect.Width))
                    myLeft = rect.X + 20;
                this.Left = SystemLibrary.ToInt32(myValue.ToString());

                myValue = myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Position", "Width").ToString();
                myWidth = SystemLibrary.ToInt32(myValue.ToString());
                if ((myLeft + myWidth) > (rect.X + rect.Width)) // Need to test the width not greater than 1 screen??
                    myWidth = rect.Width / 2;
                this.Width = myWidth;

                myValue = myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Position", "Height").ToString();
                myHeight = SystemLibrary.ToInt32(myValue.ToString());
                if ((myTop + myHeight) > (rect.Y + rect.Height))
                    myHeight = rect.Height / 2;
                this.Height = myHeight;
            }
            else
            {
                myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Position", "Top", 200);
            }
        } //PositionLoad()

        public delegate void SetFlagsCallback(Boolean inBloombergEMSXFileConfigured, Boolean inScotiaPrimeConfigured, Boolean inMLPrimeConfigured, Boolean inMLFuturesConfigured);
        public void SetFlags(Boolean inBloombergEMSXFileConfigured, Boolean inScotiaPrimeConfigured, Boolean inMLPrimeConfigured, Boolean inMLFuturesConfigured)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                SetFlagsCallback cb = new SetFlagsCallback(SetFlags);
                this.Invoke(cb, new object[] { inBloombergEMSXFileConfigured, inScotiaPrimeConfigured, inMLPrimeConfigured, inMLFuturesConfigured });
            }
            else
            {
                BloombergEMSXFileConfigured = inBloombergEMSXFileConfigured;
                ScotiaPrimeConfigured = inScotiaPrimeConfigured;
                MLPrimeConfigured = inMLPrimeConfigured;
                MLFuturesConfigured = inMLFuturesConfigured;
            }

        } //SetFlags()

        public delegate void GetParametersCallback(ref DataTable outdt_Port, ref DataTable outdt_Last_Price, ref String outFund_Crncy, ref String outBPS_Index_Ticker);
        public void GetParameters(ref DataTable outdt_Port, ref DataTable outdt_Last_Price, ref String outFund_Crncy, ref String outBPS_Index_Ticker)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                GetParametersCallback cb = new GetParametersCallback(GetParameters);
                this.Invoke(cb, new object[] { outdt_Port, outdt_Last_Price, outFund_Crncy, outBPS_Index_Ticker });
            }
            else
            {
                outdt_Port = dt_Port;
                outdt_Last_Price = dt_Last_Price;
                outFund_Crncy = Fund_Crncy;
                outBPS_Index_Ticker = BPS_Index_Ticker;
            }
        } //GetParameters()
        
        /*
         * Deals with real-time changes from Bloomberg EMSX.
         * Rules: This code structure matches that of sp_Update_Positions.
         * 
         * TODO(1) Would be better if I was also given a list of what has changed and then I only need to work on that.
         *          This is complictaed by the inter-relation of the items, plus the concept of deletions
         */
        public delegate void NewEMSXOrdersCallback(DataTable dt_in);
        public void NewEMSXOrders(DataTable dt_in)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                NewEMSXOrdersCallback cb = new NewEMSXOrdersCallback(NewEMSXOrders);
                this.Invoke(cb, new object[] { dt_in });
            }
            else
            {
                //Console.WriteLine("NewEMSXOrders - COLIN TO PUT THIS CODE BACK");
                //return;
                // See if User is in the "TRADE" Tab & do nothing if they are
                if (tabControl_Port.SelectedTab.Text == "TRADE" || tabControl_Port.SelectedTab.Text == "ALIGN")
                    return;

                //SystemLibrary.SetDebugLevel(4);
                SystemLibrary.DebugLine(" NewEMSXOrders() - Start (dt_Port.Rows.Count=" + dt_Port.Rows.Count.ToString() + ")");

                // Local Variables
                Boolean NewRowRequired = false;
                DataTable dt_Open_Orders = dt_in.Copy();
                String myFilter = "";
                Hashtable ProcessedSequence = new Hashtable();

                // Filter the DataTable down to what is needed for this display (Sort by Side, the Ticker)
                if (FundID != -1)
                    myFilter = "FundID=" + FundID.ToString();
                if (PortfolioID != -1)
                {
                    if (myFilter.Length > 0)
                        myFilter = myFilter + " AND ";
                    myFilter = myFilter + "PortfolioID=" + PortfolioID.ToString();
                }
                if (myFilter.Length > 0)
                    myFilter = myFilter + " AND ";
                // Deals with unfilled data that has been closed off for the day.
                //myFilter = myFilter + "Not (ProcessedEOD = 'Y' and Fill_Quantity = 0)";
                myFilter = myFilter + "Not (Order_Completed = 'Y' and Fill_Quantity = 0)";
                    
                // The .Net documentation says I can put this expression in the Sort, but it did not work, so I am adding a new columns.
                DataColumn mySort = dt_Open_Orders.Columns.Add("mySort", typeof(int));
                mySort.Expression = "IIF(Side='B',1,IIF(Side='S',2,IIF(Side='SS',3,4)))";
                DataColumn myFill_Value = dt_Open_Orders.Columns.Add("myFill_Value", typeof(Decimal));
                myFill_Value.Expression = "Qty_Fill * Fill_Price";
                DataRow[] dr_Orders = dt_Open_Orders.Select(myFilter, "mySort, BBG_Ticker");

                SystemLibrary.DebugLine(" NewEMSXOrders() - Post dt_Open_Orders");
                /*
                 * sp_Update_Positions at this point Deletes any new Position (Where Quantity = 0)
                 * We will do that at the end if the resultant row has no Quantity columns with data, as there is no need to delete a row
                 * only to put it back later.
                 */

                // TODO(1) I Need to look at dt_PortfolioTranspose as well.
                // TODO(1) I Need to look at dt_PortfolioTranspose as well.
                // TODO(1) I Need to look at dt_PortfolioTranspose as well.

                dg_Port.SuspendLayout();

                // Reset Fills
                for (int i=0;i<dt_Port.Rows.Count;i++)
                {
                    // In theory should never get to 2nd & 3rd 'if' statement
                    if (SystemLibrary.ToDecimal(dt_Port.Rows[i]["Qty_Order"]) != Decimal.Zero)
                    {
                        dt_Port.Rows[i]["Qty_Order"] = 0;
                        dt_Port.Rows[i]["Qty_Routed"] = 0;
                        dt_Port.Rows[i]["Qty_Fill"] = 0;
                        dt_Port.Rows[i]["Avg_Price"] = DBNull.Value;
                        //Console.WriteLine(dt_Port.Rows[i]["Ticker"].ToString() + " Set to ZERO");
                    }
                    else if (SystemLibrary.ToDecimal(dt_Port.Rows[i]["Qty_Routed"]) != Decimal.Zero)
                    {
                        dt_Port.Rows[i]["Qty_Order"] = 0;
                        dt_Port.Rows[i]["Qty_Routed"] = 0;
                        dt_Port.Rows[i]["Qty_Fill"] = 0;
                        dt_Port.Rows[i]["Avg_Price"] = DBNull.Value;
                    }
                    else if (SystemLibrary.ToDecimal(dt_Port.Rows[i]["Qty_Fill"]) != Decimal.Zero)
                    {
                        dt_Port.Rows[i]["Qty_Order"] = 0;
                        dt_Port.Rows[i]["Qty_Routed"] = 0;
                        dt_Port.Rows[i]["Qty_Fill"] = 0;
                        dt_Port.Rows[i]["Avg_Price"] = DBNull.Value;
                    }
                }
                SystemLibrary.DebugLine(" NewEMSXOrders() - Post setting Qty_Order to zero");

                // Loop on Order/Fills and assign to parent record
                String prev_groupFilter = "";
                for (int i = 0; i < dr_Orders.Length; i++)
                {
                    /*
                    Console.WriteLine("mySort=" + dr_Orders[i]["IdeaOwner"].ToString() + ", BBG_Ticker=" + dr_Orders[i]["BBG_Ticker"].ToString() +
                                      ", EMSX_Sequence=" + dr_Orders[i]["EMSX_Sequence"].ToString() +
                                      ", FillNo=" + dr_Orders[i]["FillNo"].ToString() +
                                      ", PortfolioName=" + dr_Orders[i]["PortfolioName"].ToString());
                    */
                    //dt_Port.AcceptChanges();
                    String isFuture = dr_Orders[i]["isFuture"].ToString();
                    String BBG_Ticker = dr_Orders[i]["BBG_Ticker"].ToString();
                    String PM = dr_Orders[i]["PM"].ToString();
                    String IdeaOwner = dr_Orders[i]["IdeaOwner"].ToString();
                    String Strategy1 = dr_Orders[i]["Strategy1"].ToString();
                    String Strategy2 = dr_Orders[i]["Strategy2"].ToString();
                    String Strategy3 = dr_Orders[i]["Strategy3"].ToString();
                    String Strategy4 = dr_Orders[i]["Strategy4"].ToString();
                    Decimal Qty_Fill = SystemLibrary.ToDecimal(dr_Orders[i]["Qty_Fill"]);
                    Decimal Qty_Order = SystemLibrary.ToDecimal(dr_Orders[i]["Qty_Order"]);
                    Decimal Qty_Routed = SystemLibrary.ToDecimal(dr_Orders[i]["Qty_Routed"]);
                    Decimal Fill_Price = SystemLibrary.ToDecimal(dr_Orders[i]["Fill_Price"]);
                    String CheckSequence = dr_Orders[i]["EMSX_Sequence"].ToString();
                    if (CheckSequence.Length == 0)
                        CheckSequence = dr_Orders[i]["OrderRefID"].ToString();
                    CheckSequence = CheckSequence + dr_Orders[i]["FundID"].ToString();

                    // For Futures, Bundle dr_Orders Qty_Order > 0 & Qty_Order <0 per group
                    if (isFuture == "Y")
                    {
                        String groupFilter = "BBG_Ticker='" + BBG_Ticker + "' And isNull(PM,'')='" + PM + "' And isNull(IdeaOwner,'')='" + IdeaOwner +
                                             "' And isNull(Strategy1,'')='" + Strategy1 + "' And isNull(Strategy2,'')='" + Strategy2 +
                                             "' And isNull(Strategy3,'')='" + Strategy3 + "' And isNull(Strategy4,'')='" + Strategy4 + "' ";

                        //if (BBG_Ticker == "XPZ2 Index")
                        //    Console.WriteLine("A");

                        if (SystemLibrary.ToDecimal(dr_Orders[i]["Qty_Order"]) > 0)
                        {
                            groupFilter = groupFilter + " And Qty_Order > 0 ";
                            CheckSequence = BBG_Ticker + " Qty_Order > 0";
                        }
                        else
                        {
                            groupFilter = groupFilter + " And Qty_Order < 0 ";
                            CheckSequence = BBG_Ticker + " Qty_Order < 0";
                        }

                        // Ignore already Grouped records
                        if (groupFilter == prev_groupFilter)
                            continue;
                        //Console.WriteLine("groupFilter=" + groupFilter);


                        Qty_Order = SystemLibrary.ToDecimal(dt_Open_Orders.Compute("Sum(Qty_Order)", groupFilter + " AND " + myFilter + " AND FillNo <= 1"));
                        Qty_Routed = SystemLibrary.ToDecimal(dt_Open_Orders.Compute("Sum(Qty_Routed)", groupFilter + " AND " + myFilter));
                        Qty_Fill = SystemLibrary.ToDecimal(dt_Open_Orders.Compute("Sum(Qty_Fill)", groupFilter + " AND " + myFilter));
                        if (Qty_Fill == 0)
                            Fill_Price = 0;
                        else
                            Fill_Price = SystemLibrary.ToDecimal(dt_Open_Orders.Compute("Sum(myFill_Value)/ Sum(Qty_Fill)", groupFilter + " AND " + myFilter));
                        prev_groupFilter = groupFilter;

                    }

                    // Update an existing row
                    //                     "IsNull(Strategy5,'') = IsNull('" + dr_Orders[i]["Strategy5"].ToString() + "','') AND " +
                    String myPortFilter = "Ticker = '" + BBG_Ticker + "' AND " +
                                          "isNull(IsAggregate,'N') = 'N' AND " +
                                          "IsNull(Status,'') = '' AND " +
                                          "IsNull(PM,'') = IsNull('" + PM + "','') AND " +
                                          "IsNull(IdeaOwner,'') = IsNull('" + IdeaOwner + "','') AND " +
                                          "IsNull(Strategy1,'') = IsNull('" + Strategy1 + "','') AND " +
                                          "IsNull(Strategy2,'') = IsNull('" + Strategy2 + "','') AND " +
                                          "IsNull(Strategy3,'') = IsNull('" + Strategy3 + "','') AND " +
                                          "IsNull(Strategy4,'') = IsNull('" + Strategy4 + "','') AND " +
                                          "NOT (isNull(Qty_Fill,0) = - IsNull(" + Qty_Fill.ToString() + ",0) AND isNull(Qty_Fill,0) <> 0)";
                    
                    String my_Port_Sort = "";
                    if (isFuture != "Y")
                    {
                        myPortFilter = myPortFilter +
                                       " AND (((IsNull(Quantity,0)+isNull(Qty_Order,0)) = 0) OR IIF((IsNull(Quantity,0)+isNull(Qty_Order,0)) >= 0,'L','S') = IIF(Len('" + dr_Orders[i]["Side"].ToString() + "')=1,'L','S'))";
                    }
                    else
                    {
                        //myPortFilter = myPortFilter +
                         //              " AND NOT ((IsNull(Quantity,0)+IsNull(Qty_Order,0)) = -1 * " + Qty_Order.ToString() + ")";
                        // Make sure all the Longs go together and all the shorts go together
                        if (Qty_Order > 0)
                            my_Port_Sort = "Qty_Order Desc";
                        else
                            my_Port_Sort = "Qty_Order Asc";
                    }
                   
                    DataRow[] dr = dt_Port.Select(myPortFilter, my_Port_Sort);
                    //Console.WriteLine(myPortFilter);
                    //Console.WriteLine("dr.Length=" + dr.Length.ToString());
                    //if (dr_Orders[i]["BBG_Ticker"].ToString()=="XPZ2 Index" && dr_Orders[i]["Qty_Fill"].ToString()=="-2")
                    //    Console.WriteLine("A");
                    //SystemLibrary.DebugLine(" NewEMSXOrders() - DataRow[] dr = dt_Port.Select()");
                    if (dr.Length > 0)
                    {
                        //if (dr_Orders[i]["BBG_Ticker"].ToString().ToUpper() == "XPZ2 INDEX")
                        //    Console.WriteLine("dr_Orders - Seq/FillNo=" + dr_Orders[i]["EMSX_Sequence"].ToString() + "/" + dr_Orders[i]["FillNo"].ToString() + ", " + dr_Orders[i]["BBG_Ticker"].ToString() + ", Qty_Order=" + SystemLibrary.ToString(dr_Orders[i]["Qty_Order"]));

                        if (SystemLibrary.ToDecimal(dr[0]["Qty_Fill"]) + Qty_Fill == 0)
                            dr[0]["Avg_Price"] = 0;
                        else
                            dr[0]["Avg_Price"] = (SystemLibrary.ToDecimal(dr[0]["Qty_Fill"]) * SystemLibrary.ToDecimal(dr[0]["Avg_Price"]) +
                                                 Qty_Fill * Fill_Price)
                                                 / (SystemLibrary.ToDecimal(dr[0]["Qty_Fill"]) + Qty_Fill);

                        // Only work once with Qty_Order for each EMSX_Sequence
                        if (!ProcessedSequence.ContainsKey(CheckSequence))
                        {
                            dr[0]["Qty_Order"] = SystemLibrary.ToDecimal(dr[0]["Qty_Order"]) + Qty_Order;
                            ProcessedSequence.Add(CheckSequence, CheckSequence);
                        }

                        dr[0]["Qty_Routed"] = SystemLibrary.ToDecimal(dr[0]["Qty_Routed"]) + Qty_Routed;
                        dr[0]["Qty_Fill"] = SystemLibrary.ToDecimal(dr[0]["Qty_Fill"]) + Qty_Fill;
                    }
                    else
                    {
                        // Insert a New Row, so go back to the database for all the data.
                        NewRowRequired = true;
                        break;
                    }
                }

                SystemLibrary.DebugLine(" NewEMSXOrders() - Pre:     if (!NewRowRequired)");

                if (!NewRowRequired)
                {
                    // Clear Positions where Quantity fields are all zero
                    for (int i = dt_Port.Rows.Count - 1; i >= 0; i--)
                    {
                        if (SystemLibrary.ToString(dt_Port.Rows[i]["IsAggregate"]) == "N" &&
                            SystemLibrary.ToInt32(dt_Port.Rows[i]["Quantity"]) == 0 &&
                            SystemLibrary.ToInt32(dt_Port.Rows[i]["Qty_Order"]) == 0 &&
                            SystemLibrary.ToInt32(dt_Port.Rows[i]["Qty_Routed"]) == 0 &&
                            SystemLibrary.ToInt32(dt_Port.Rows[i]["Qty_Fill"]) == 0
                            )
                        {
                            dt_Port.Rows[i].Delete();
                        }
                    }
                    SystemLibrary.DebugLine(" NewEMSXOrders() - Post:     dt_Port.Rows[i].Delete();");

                    dt_Port.AcceptChanges();
                    SystemLibrary.DebugLine(" NewEMSXOrders() - Post:     AcceptChanges();");
                    SetCalc();
                    SystemLibrary.DebugLine(" NewEMSXOrders() - Post:     SetCalc();");
                    SetFormat();
                    SystemLibrary.DebugLine(" NewEMSXOrders() - Post:     SetFormat();");
                    dg_Port.ResumeLayout(true);
                    SystemLibrary.DebugLine(" NewEMSXOrders() - Post:     ResumeLayout;");
                }
                else
                {
                    // Relaod all data from the database
                    dg_Port.ResumeLayout(true);
                    //Console.WriteLine("NewEMSXOrders - LoadPortfolio(true);");
                    SystemLibrary.DebugLine(" NewEMSXOrders - LoadPortfolio(true);");
                    LoadPortfolio(true);
                    /* CFR 20140203
                    if(LoadPortfolio(true))
                        ResetHiddenRows();
                     */
                }
                //Console.WriteLine("NewEMSXOrders (records={0})", dr_Orders.Length);
                SystemLibrary.DebugLine(" NewEMSXOrders((records=" + dr_Orders.Length + ")");
                SystemLibrary.DebugLine(" NewEMSXOrders() - End (dt_Port.Rows.Count=" + dt_Port.Rows.Count.ToString() + ")");
                //SystemLibrary.SetDebugLevel(0);

            }

        } //NewEMSXOrders()

        public delegate Boolean[] FTPStructureCallback(String Command);
        public Boolean[] FTPStructure(String Command)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                FTPStructureCallback cb = new FTPStructureCallback(FTPStructure);
                return ((Boolean[])this.Invoke(cb, new object[] { Command }));
            }
            else
            {
                // Local Variables
                Boolean[] RetVal = new Boolean[] { false, false };
                Boolean isConfigured = true;
                switch (Command.ToUpper())
                {
                    case "LOAD":
                        RetVal[0] = SystemLibrary.FTPLoadStructure(ref isConfigured);
                        break;
                    case "SAVE":
                        RetVal[0] = SystemLibrary.FTPSaveStructure();
                        break;
                }
                RetVal[1] = isConfigured;
                return (RetVal);
            }
        }


        public delegate Boolean[] FTPMLPrimeStructureCallback(String Command);
        public Boolean[] FTPMLPrimeStructure(String Command)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                FTPMLPrimeStructureCallback cb = new FTPMLPrimeStructureCallback(FTPMLPrimeStructure);
                return ((Boolean[])this.Invoke(cb, new object[] { Command }));
            }
            else
            {
                // Local Variables
                Boolean[] RetVal = new Boolean[] { false, false };
                Boolean isConfigured = true;
                switch (Command.ToUpper())
                {
                    case "LOAD":
                        RetVal[0] = SystemLibrary.FTPMLPrimeLoadStructure(ref isConfigured);
                        break;
                    case "SAVE":
                        RetVal[0] = SystemLibrary.FTPMLPrimeSaveStructure();
                        break;
                }
                RetVal[1] = isConfigured;
                return (RetVal);
            }
        } //FTPMLPrimeStructure()


        public delegate Boolean[] FTPSCOTIAPrimeStructureCallback(String Command);
        public Boolean[] FTPSCOTIAPrimeStructure(String Command)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                FTPSCOTIAPrimeStructureCallback cb = new FTPSCOTIAPrimeStructureCallback(FTPSCOTIAPrimeStructure);
                return ((Boolean[])this.Invoke(cb, new object[] { Command }));
            }
            else
            {
                // Local Variables
                Boolean[] RetVal = new Boolean[] { false, false };
                Boolean isConfigured = true;
                switch (Command.ToUpper())
                {
                    case "LOAD":
                        RetVal[0] = SystemLibrary.FTPSCOTIAPrimeLoadStructure(ref isConfigured);
                        break;
                    case "SAVE":
                        RetVal[0] = SystemLibrary.FTPSCOTIAPrimeSaveStructure();
                        break;
                }
                RetVal[1] = isConfigured;
                return (RetVal);
            }
        } //FTPSCOTIAPrimeStructure()

        public delegate void SetUpSecurities_DataTableCallback();
        public void SetUpSecurities_DataTable()
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                SetUpSecurities_DataTableCallback cb = new SetUpSecurities_DataTableCallback(SetUpSecurities_DataTable);
                this.Invoke(cb, new object[] { });
            }
            else
            {
                // Load up all the active securities
                String mySql = "Select * From Securities " +
                               "Where BBG_Ticker in (Select BBG_Ticker From Positions_Today) " +
                               "Or	  BBG_Ticker in (select EMSX_TICKER from emsx_api where emsx_date >= dbo.f_Today()) " + 
                               "Order by BBG_Ticker";
                dt_Securities = SystemLibrary.SQLSelectToDataTable(mySql);
            }
        } //SetUpSecurities_DataTable()

        private void SetUpLast_Price_DataTable()
        {
            String mySql = "Select BBG_Ticker as Ticker, ID_BB_UNIQUE, Max(Prev_Close) as LAST_PRICE, 'N' as isNew From Positions_today Group By BBG_Ticker, ID_BB_UNIQUE Order BY BBG_Ticker";
            dt_Last_Price = SystemLibrary.SQLSelectToDataTable(mySql);

            if (BPS_Index_Ticker.Length > 0)
            {
                DataRow dr = dt_Last_Price.NewRow();
                dr["Ticker"] = BPS_Index_Ticker;
                //dr["ID_BB_UNIQUE"] = "";
                dr["LAST_PRICE"] = BPS_Index_Close;
                dr["isNew"] = "N";
                dt_Last_Price.Rows.Add(dr);
            }

            int SavePriceInterval = SystemLibrary.SQLSelectInt32("Select dbo.f_GetParamValue('SavePriceInterval')");
            //Console.WriteLine("COLIN SavePriceInterval="+SavePriceInterval.ToString());
            if (SavePriceInterval >= 30000)
            {
                timer_SavePrices.Stop();
                timer_SavePrices.Interval = SavePriceInterval;
                timer_SavePrices.Start();
                //Console.WriteLine("COLIN timer_SavePrices.Start()");
            }
        }

        private void SetUpFX_DataTable()
        {
            dt_FX.Columns.Add("Ticker");
            dt_FX.Columns.Add("FromFX");
            dt_FX.Columns.Add("ToFX");
            dt_FX.Columns.Add("PX_POS_MULT_FACTOR", System.Type.GetType("System.Decimal"));
            dt_FX.Columns.Add("LAST_PRICE", System.Type.GetType("System.Decimal"));
        }

        private void SetUpHeader()
        {
            // Local Variables
            Font FontBold = new Font(dg_Header.RowsDefaultCellStyle.Font, FontStyle.Bold);

            dg_Header.Rows.Add(7);
            dg_Header.Rows[0].Cells["Label"].Value = "Long";
            dg_Header.Rows[1].Cells["Label"].Value = "Short";
            dg_Header.Rows[2].Cells["Label"].Value = "Future";
            if (!ShowFutureHeaderLine)
                dg_Header.Rows[2].Visible = false;
            //dg_Header.Rows[3].DefaultCellStyle.Font = FontBold;
            dg_Header.Rows[3].DefaultCellStyle.BackColor = Color.LightGray;
            dg_Header.Rows[3].Cells["Label"].Value = "Net";
            //dg_Header.Rows[4].DefaultCellStyle.Font = FontBold;
            dg_Header.Rows[4].DefaultCellStyle.ForeColor = Color.DarkBlue;
            dg_Header.Rows[4].Cells["Label"].Value = "Gross";
            dg_Header.Rows[5].DefaultCellStyle.BackColor = Color.LightGray;
            dg_Header.Rows[5].Cells["Label"].Value = @"%";
            dg_Header.Rows[5].Cells["PL"].Style.Format = "N2";
            dg_Header.Rows[5].Cells["PL_MTD"].Style.Format = "N2";
            dg_Header.Rows[5].Cells["PL_Yest"].Style.Format = "N2";
            dg_Header.Rows[5].Cells["PL_WRoll"].Style.Format = "N2";
            dg_Header.Rows[5].Cells["PL_MRoll"].Style.Format = "N2";
            dg_Header.Rows[5].Cells["PL_DeltaMax"].Style.Format = "N2";
            dg_Header.Rows[5].Cells["PL_Inception"].Style.Format = "N2";
            dg_Header.Rows[5].Cells["PL_YTD"].Style.Format = "N2";
            dg_Header.Rows[5].Cells["PL_YTD_July"].Style.Format = "N2";
            dg_Header.Rows[6].DefaultCellStyle.BackColor = Color.AliceBlue;
            if (BPS_Index_Ticker.Length > 0)
                dg_Header.Rows[6].Cells["Label"].Value = BPS_Index_Ticker.Substring(0, BPS_Index_Ticker.Length - 6);
            else
                dg_Header.Rows[6].Cells["Label"].Value = @"Index %";
            dg_Header.Rows[6].Cells["Label"].Style.ForeColor = Color.Black;
            dg_Header.Rows[6].Cells["PL"].Style.Format = "N2";
            dg_Header.Rows[6].Cells["PL_MTD"].Style.Format = "N2";
            dg_Header.Rows[6].Cells["PL_Yest"].Style.Format = "N2";
            dg_Header.Rows[6].Cells["PL_WRoll"].Style.Format = "N2";
            dg_Header.Rows[6].Cells["PL_MRoll"].Style.Format = "N2";
            dg_Header.Rows[6].Cells["PL_DeltaMax"].Style.Format = "N2";
            dg_Header.Rows[6].Cells["PL_Inception"].Style.Format = "N2";
            dg_Header.Rows[6].Cells["PL_YTD"].Style.Format = "N2";
            dg_Header.Rows[6].Cells["PL_YTD_July"].Style.Format = "N2";

            DataGridViewCellStyle dGVCS = new DataGridViewCellStyle();

            dGVCS.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dGVCS.BackColor = Color.DarkSlateBlue;
            dGVCS.Font = new Font("Microsoft Sans Serif", 8.36F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            dGVCS.ForeColor = Color.White;
            dGVCS.SelectionBackColor = SystemColors.Highlight;
            dGVCS.SelectionForeColor = SystemColors.HighlightText;
            dGVCS.WrapMode = DataGridViewTriState.True;

            dg_Header.Columns["PL_Yest"].HeaderCell.Style = dGVCS;
            dg_Header.Columns["PL_WRoll"].HeaderCell.Style = dGVCS;
            dg_Header.Columns["PL_MRoll"].HeaderCell.Style = dGVCS;
            dg_Header.Columns["PL_DeltaMax"].HeaderCell.Style = dGVCS;
            dg_Header.Columns["PL_Inception"].HeaderCell.Style = dGVCS;
            dg_Header.Columns["PL_YTD"].HeaderCell.Style = dGVCS;
            dg_Header.Columns["PL_YTD_July"].HeaderCell.Style = dGVCS;

            // Create the Datatable
            if (GrossPctTable.Columns.Count < 1)
            {
                DataColumn BBG_Ticker = new DataColumn("BBG_Ticker");
                DataColumn Exposure = new DataColumn("Exposure");
                DataColumn Exposure_Filled = new DataColumn("Exposure_Filled");
                DataColumn LS = new DataColumn("LS");
                DataColumn PL = new DataColumn("PL");
                DataColumn isFuture = new DataColumn("isFuture");


                BBG_Ticker.DataType = System.Type.GetType("System.String");
                Exposure.DataType = System.Type.GetType("System.Decimal");
                Exposure_Filled.DataType = System.Type.GetType("System.Decimal");
                LS.DataType = System.Type.GetType("System.String");
                PL.DataType = System.Type.GetType("System.Decimal");
                isFuture.DataType = System.Type.GetType("System.String");

                GrossPctTable.Columns.Add(BBG_Ticker);
                GrossPctTable.Columns.Add(Exposure);
                GrossPctTable.Columns.Add(Exposure_Filled);
                GrossPctTable.Columns.Add(LS);
                GrossPctTable.Columns.Add(PL);
                GrossPctTable.Columns.Add(isFuture);
            }

        } //SetUpHeader()

        private void SetHeader()
        {
            // Put this Try{} here as could be closing down
            try
            {
                // lb_Fund_Amount
                lb_Fund_Amount.Text = (Fund_Amount + Long_PL + Short_PL + Future_PL).ToString("FUM  $#,###") + Fund_Amount.ToString("    [SOD:$#,### ]");
                if (Fund_Amount + Long_PL + Short_PL + Future_PL == 0)
                    lb_Leverage.Text = "";
                else
                    lb_Leverage.Text = (Gross_Amount / (Fund_Amount + Long_PL + Short_PL + Future_PL)).ToString("Leverage  0.#x") +
                                       ((Math.Abs(Long_Amount_Filled) + Math.Abs(Short_Amount_Filled) + Math.Abs(Future_Amount_Filled)) / (Fund_Amount + Long_PL + Short_PL + Future_PL)).ToString("   [Fill 0.#x]");
                // Long
                SetValueRG(dg_Header.Rows[0], "Exposure", Long_Amount);
                SetValueRG(dg_Header.Rows[0], "Positions", Long_Positions);
                SetValueRG(dg_Header.Rows[0], "Winners", Long_Winners);
                SetValueRG(dg_Header.Rows[0], "PL", Long_PL);
                SetValueRG(dg_Header.Rows[0], "PL_MTD", Long_PL_MTD + Long_PL);
                SetValueRG(dg_Header.Rows[0], "PL_Yest", Long_PL_Yest);  // Yest is exception as no today P&L
                SetValueRG(dg_Header.Rows[0], "PL_WRoll", Long_PL_WRoll + Long_PL);
                SetValueRG(dg_Header.Rows[0], "PL_MRoll", Long_PL_MRoll + Long_PL);
                SetValueRG(dg_Header.Rows[0], "PL_DeltaMax", Long_PL_DeltaMax + Long_PL);
                SetValueRG(dg_Header.Rows[0], "PL_Inception", Long_PL_Inception + Long_PL);
                SetValueRG(dg_Header.Rows[0], "PL_YTD", Long_PL_YTD + Long_PL);
                SetValueRG(dg_Header.Rows[0], "PL_YTD_July", Long_PL_YTD_July + Long_PL);
                if (Gross_Amount != 0)
                    SetValueRG(dg_Header.Rows[0], "PCT_Gross", Long_Amount / Gross_Amount);
                if (Fund_Amount != 0)
                {
                    SetValueRG(dg_Header.Rows[0], "PCT_FUM", Long_Amount / Fund_Amount);
                    SetValueRG(dg_Header.Rows[0], "PCT_FUM_Filled", Long_Amount_Filled / Fund_Amount);
                }

                // Short
                SetValueRG(dg_Header.Rows[1], "Exposure", Short_Amount);
                SetValueRG(dg_Header.Rows[1], "Positions", -Short_Positions);
                SetValueRG(dg_Header.Rows[1], "Winners", Short_Winners);
                SetValueRG(dg_Header.Rows[1], "PL", Short_PL);
                SetValueRG(dg_Header.Rows[1], "PL_MTD", Short_PL_MTD + Short_PL);
                SetValueRG(dg_Header.Rows[1], "PL_Yest", Short_PL_Yest); // Yest is exception as no today P&L
                SetValueRG(dg_Header.Rows[1], "PL_WRoll", Short_PL_WRoll + Short_PL);
                SetValueRG(dg_Header.Rows[1], "PL_MRoll", Short_PL_MRoll + Short_PL);
                SetValueRG(dg_Header.Rows[1], "PL_DeltaMax", Short_PL_DeltaMax + Short_PL);
                SetValueRG(dg_Header.Rows[1], "PL_Inception", Short_PL_Inception + Short_PL);
                SetValueRG(dg_Header.Rows[1], "PL_YTD", Short_PL_YTD + Short_PL);
                SetValueRG(dg_Header.Rows[1], "PL_YTD_July", Short_PL_YTD_July + Short_PL);
                if (Gross_Amount != 0)
                    SetValueRG(dg_Header.Rows[1], "PCT_Gross", Short_Amount / Gross_Amount);
                if (Fund_Amount != 0)
                {
                    SetValueRG(dg_Header.Rows[1], "PCT_FUM", Short_Amount / Fund_Amount);
                    SetValueRG(dg_Header.Rows[1], "PCT_FUM_Filled", Short_Amount_Filled / Fund_Amount);
                }

                // Future
                SetValueRG(dg_Header.Rows[2], "Exposure", Future_Amount);
                SetValueRG(dg_Header.Rows[2], "Positions", Future_Positions);
                SetValueRG(dg_Header.Rows[2], "Winners", Future_Winners);
                SetValueRG(dg_Header.Rows[2], "PL", Future_PL);
                SetValueRG(dg_Header.Rows[2], "PL_MTD", Future_PL_MTD + Future_PL);
                SetValueRG(dg_Header.Rows[2], "PL_Yest", Future_PL_Yest); // Yest is exception as no today P&L
                SetValueRG(dg_Header.Rows[2], "PL_WRoll", Future_PL_WRoll + Future_PL);
                SetValueRG(dg_Header.Rows[2], "PL_MRoll", Future_PL_MRoll + Future_PL);
                SetValueRG(dg_Header.Rows[2], "PL_DeltaMax", Future_PL_DeltaMax + Future_PL);
                SetValueRG(dg_Header.Rows[2], "PL_Inception", Future_PL_Inception + Future_PL);
                SetValueRG(dg_Header.Rows[2], "PL_YTD", Future_PL_YTD + Future_PL);
                SetValueRG(dg_Header.Rows[2], "PL_YTD_July", Future_PL_YTD_July + Future_PL);
                if (Gross_Amount != 0)
                    SetValueRG(dg_Header.Rows[2], "PCT_Gross", Future_Amount / Gross_Amount);
                if (Fund_Amount != 0)
                {
                    SetValueRG(dg_Header.Rows[2], "PCT_FUM", Future_Amount / Fund_Amount);
                    SetValueRG(dg_Header.Rows[2], "PCT_FUM_Filled", Future_Amount_Filled / Fund_Amount);
                }

                // Net
                SetValueRG(dg_Header.Rows[3], "Exposure", Long_Amount + Short_Amount + Future_Amount);
                SetValueRG(dg_Header.Rows[3], "Positions", Long_Positions - Short_Positions + Future_Positions);
                if (Gross_Amount != 0)
                    SetValueRG(dg_Header.Rows[3], "PCT_Gross", (Long_Amount + Short_Amount + Future_Amount) / Gross_Amount);
                if (Fund_Amount != 0)
                {
                    SetValueRG(dg_Header.Rows[3], "PCT_FUM", (Long_Amount + Short_Amount + Future_Amount) / Fund_Amount);
                    SetValueRG(dg_Header.Rows[3], "PCT_FUM_Filled", (Long_Amount_Filled + Short_Amount_Filled + Future_Amount_Filled) / Fund_Amount);
                }

                // Gross
                SetValueRG(dg_Header.Rows[4], "Exposure", Long_Amount + Math.Abs(Short_Amount) + Math.Abs(Future_Amount));
                SetValueRG(dg_Header.Rows[4], "Positions", Long_Positions + Math.Abs(Short_Positions) + Math.Abs(Future_Positions));
                SetValueRG(dg_Header.Rows[4], "Winners", Long_Winners + Math.Abs(Short_Winners) + Math.Abs(Future_Winners));
                SetValueRG(dg_Header.Rows[4], "PL", Long_PL + Short_PL + Future_PL);
                SetValueRG(dg_Header.Rows[4], "PL_MTD", Long_PL_MTD + Long_PL + Short_PL_MTD + Short_PL + Future_PL_MTD + Future_PL);
                SetValueRG(dg_Header.Rows[4], "PL_Yest", Long_PL_Yest + Short_PL_Yest + Future_PL_Yest); // Yest is exception as no today P&L
                SetValueRG(dg_Header.Rows[4], "PL_WRoll", Long_PL_WRoll + Long_PL + Short_PL_WRoll + Short_PL + Future_PL_WRoll + Future_PL);
                SetValueRG(dg_Header.Rows[4], "PL_MRoll", Long_PL_MRoll + Long_PL + Short_PL_MRoll + Short_PL + Future_PL_MRoll + Future_PL);
                SetValueRG(dg_Header.Rows[4], "PL_DeltaMax", Long_PL_DeltaMax + Long_PL + Short_PL_DeltaMax + Short_PL + Future_PL_DeltaMax + Future_PL);
                SetValueRG(dg_Header.Rows[4], "PL_Inception", Long_PL_Inception + Long_PL + Short_PL_Inception + Short_PL + Future_PL_Inception + Future_PL);
                SetValueRG(dg_Header.Rows[4], "PL_YTD", Long_PL_YTD + Long_PL + Short_PL_YTD + Short_PL + Future_PL_YTD + Future_PL);
                SetValueRG(dg_Header.Rows[4], "PL_YTD_July", Long_PL_YTD_July + Long_PL + Short_PL_YTD_July + Short_PL + Future_PL_YTD_July + Future_PL);

                if (Gross_Amount != 0)
                    SetValueRG(dg_Header.Rows[4], "PCT_Gross", (Long_Amount + Math.Abs(Short_Amount) + Math.Abs(Future_Amount)) / Gross_Amount);
                if (Fund_Amount != 0)
                {
                    SetValueRG(dg_Header.Rows[4], "PCT_FUM", (Long_Amount + Math.Abs(Short_Amount) + Math.Abs(Future_Amount)) / Fund_Amount);
                    SetValueRG(dg_Header.Rows[4], "PCT_FUM_Filled", (Long_Amount_Filled + Math.Abs(Short_Amount_Filled) + Math.Abs(Future_Amount_Filled)) / Fund_Amount);
                }

                // BPS
                if (Fund_Amount != 0)
                {
                    SetValueRG(dg_Header.Rows[5], "PL", (Long_PL + Short_PL + Future_PL) / Fund_Amount * 100m);
                    SetValueRG(dg_Header.Rows[5], "PL_Yest", BPS_PL_Yest * 100m);
                    SetValueRG(dg_Header.Rows[5], "PL_MTD", (((1 + ((Long_PL + Short_PL + Future_PL) / Fund_Amount)) * (1 + BPS_PL_MTD)) - 1m) * 100m);
                    SetValueRG(dg_Header.Rows[5], "PL_WRoll", (((1 + ((Long_PL + Short_PL + Future_PL) / Fund_Amount)) * (1 + BPS_PL_WRoll)) - 1m) * 100m);
                    SetValueRG(dg_Header.Rows[5], "PL_MRoll", (((1 + ((Long_PL + Short_PL + Future_PL) / Fund_Amount)) * (1 + BPS_PL_MRoll)) - 1m) * 100m);
                    SetValueRG(dg_Header.Rows[5], "PL_Inception", (((1 + ((Long_PL + Short_PL + Future_PL) / Fund_Amount)) * (1 + BPS_PL_Inception)) - 1m) * 100m);
                    SetValueRG(dg_Header.Rows[5], "PL_YTD", (((1 + ((Long_PL + Short_PL + Future_PL) / Fund_Amount)) * (1 + BPS_PL_YTD)) - 1m) * 100m);
                    SetValueRG(dg_Header.Rows[5], "PL_YTD_July", (((1 + ((Long_PL + Short_PL + Future_PL) / Fund_Amount)) * (1 + BPS_PL_YTD_July)) - 1m) * 100m);
                }

                // BPS Index
                if (BPS_Index_Ticker.Length != 0)
                {
                    Decimal Day_Perf;
                    if (BPS_Index_Prev_Close == 0 || BPS_Index_Ticker.Length == 0)
                        Day_Perf = 0;
                    else
                        Day_Perf = BPS_Index_Close / BPS_Index_Prev_Close - 1 + BPS_Index_DIV_TODAY;

                    //Console.WriteLine("BPS_Index_Close="+BPS_Index_Close.ToString()+",BPS_Index_Prev_Close=" + BPS_Index_Prev_Close.ToString() + ",BPS_Index_DIV_TODAY=" + BPS_Index_DIV_TODAY.ToString());
                    SetValueRG(dg_Header.Rows[6], "PL", Day_Perf * 100m);
                    SetValueRG(dg_Header.Rows[6], "PL_Yest", BPS_Index_Yest * 100m);
                    SetValueRG(dg_Header.Rows[6], "PL_MTD", (((1 + (Day_Perf)) * (1 + BPS_Index_MTD)) - 1m) * 100m);
                    SetValueRG(dg_Header.Rows[6], "PL_WRoll", (((1 + (Day_Perf)) * (1 + BPS_Index_WRoll)) - 1m) * 100m);
                    SetValueRG(dg_Header.Rows[6], "PL_MRoll", (((1 + (Day_Perf)) * (1 + BPS_Index_MRoll)) - 1m) * 100m);
                    SetValueRG(dg_Header.Rows[6], "PL_Inception", (((1 + (Day_Perf)) * (1 + BPS_Index_Inception)) - 1m) * 100m);
                    SetValueRG(dg_Header.Rows[6], "PL_YTD", (((1 + (Day_Perf)) * (1 + BPS_Index_YTD)) - 1m) * 100m);
                    SetValueRG(dg_Header.Rows[6], "PL_YTD_July", (((1 + (Day_Perf)) * (1 + BPS_Index_YTD_July)) - 1m) * 100m);
                }
            }
            catch (Exception e)
            {
                SystemLibrary.DebugLine("Exception: SetHeader():" + e.Message);
            }
        } //SetHeader


        // For All of these Update Security Table where original is Null
        // -- NB: This should only occur on NEW tickers, hence little control on the number of Update Statements.
        //
        //  "Price"  = "LAST_PRICE"
        //  "Price"  = "THEO_PRICE" // This is pre-market open price.
        //  "Security_Name" = "NAME"
        //  "crncy" = "CRNCY"
        // "prev_Close" = "PREV_CLOSE_VAL" - But Only if missing
        //      21-Nov-2012 swapped if for "PREV_CLOSE_VALUE_REALTIME"
        //      31-Jan-2012 swapped it for "YEST_LAST_TRADE"
        // "Pos_Mult_Factor" = "PX_POS_MULT_FACTOR"
        // "Round_Lot_size" = "PX_ROUND_LOT_SIZE"
        //  "Country_Full_Name" = "COUNTRY_FULL_NAME"
        //  "Sector" = "INDUSTRY_SECTOR"
        //  "Industry_Group" = "INDUSTRY_GROUP"
        //  "Industry_SubGroup" = "INDUSTRY_SUBGROUP"
        //  "Undl_Ticker" = "UNDL_TICKER"
        //  "Undl_currency" = "UNDL_CURRENCY"
        //  ID_BB_COMPANY, ID_BB_UNIQUE, ID_BB_GLOBAL = ID_BB_COMPANY, ID_BB_UNIQUE, ID_BB_GLOBAL
        //  "CUSIP" = "ID_CUSIP"
        //  "ISIN" = "ID_ISIN"
        //  "SEDOL" = "ID_SEDOL1"
        //  "Sector" = "MARKET_SECTOR_DES"
        //  "Industry_Group" = "FUTURES_CATEGORY"
        //  "Security_Typ" = "SECURITY_TYP"
        //  "Security_Typ2" = "SECURITY_TYP2"
        //  "BBG_Exchange" = "EXCH_CODE"

        public delegate void SetValueCallback(String myTicker, String[] myFields, String[] myItems);
        public void SetValue(String myTicker, String[] myFields, String[] myItems)
        {
            if (InvokeRequired)
            {
                // Is from a different thread.
                SystemLibrary.DebugLine("SetValue() - InvokeRequired");
                SetValueCallback cb = new SetValueCallback(SetValue);
                this.Invoke(cb, new object[] { myTicker, myFields, myItems });
            }
            else
            {
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - Start: SetValue(" + myTicker + ")");

                //if (myTicker.ToUpper() == "GBPUSD CURNCY")
                //    Console.WriteLine("A");

                // Local Variables
                Boolean NeedCalc = false;
                String TestPrice = "";
                Color PriceColour = Color.Empty;
                String Undl_Ticker;

                // Load the Update of Securities Table into an array object
                Hashtable Securities = new Hashtable();

                if (myTicker.StartsWith(".COLIN")) // == "DOW AU Equity")
                    Console.WriteLine("{0},{1}", myFields, myItems);
                //SystemLibrary.DebugLine("SetValue("+myTicker+","+myFields[0]+") - Start");

                // TODO (5) ?? Want to set a "BBG_last_updatetime", so can reset colours every 3 seconds? Or do I want to leave last tick?
                //SystemLibrary.DebugLine("inEditMode="+SystemLibrary.Bool_To_YN(inEditMode));
                if (myFields.Length > 0 && !inEditMode)
                {
                    /*
                     * This can be valid as system allows for hidden rows.
                    if ((dg_Port.ReadOnly == true && dt_Port.Rows.Count != dg_Port.Rows.Count) || (dg_Port.ReadOnly == false && dt_Port.Rows.Count != dg_Port.Rows.Count - 1))
                    {
                        if (LastValue == null)
                        {
                            // I was having issues here but have worked out how to Sync buffers.
                            // - So code should never get here.
                            SystemLibrary.DebugLine("dg_Port/dt_Port not in Sync (dg_port=" + dg_Port.Rows.Count + ", dt_Port=" + dt_Port.Rows.Count + ")");
                        }
                    }
                    */

                    // See if this is the Index Ticker
                    if (BPS_Index_Ticker.ToUpper() == myTicker.ToUpper())
                    {
                        // Loop around fields
                        for (int i = 0; i < myFields.Length; i++)
                        {
                            if (myItems[i].Substring(0, Math.Min("#N/A".Length, myItems[i].Length)) != "#N/A" && myItems[i].Length > 0)
                            {
                                if (myFields[i] == "LAST_PRICE")
                                {
                                    // On high frequency fields make sure that it really needs to be updated
                                    if (Math.Round(SystemLibrary.ToDecimal(BPS_Index_Close), 8) != Math.Round(SystemLibrary.ToDecimal(myItems[i]), 8))
                                    {

                                        BPS_Index_Close = SystemLibrary.ToDecimal(myItems[i]);
                                        NeedCalc = true;

                                        DataRow[] FoundTickerRows = dt_Last_Price.Select("Ticker='" + BPS_Index_Ticker + "'");
                                        //Console.WriteLine("BPS_Index_Close=" + BPS_Index_Close.ToString() + "/" + FoundTickerRows.Length.ToString());
                                        foreach (DataRow dr in FoundTickerRows)
                                        {
                                            dr["LAST_PRICE"] = BPS_Index_Close;
                                            dr["isNew"] = "Y";
                                        }
                                    }
                                    //Console.WriteLine("BPS_Index_Close=" + BPS_Index_Close.ToString());
                                }
                                else if (myFields[i] == "ID_BB_UNIQUE")
                                {
                                    DataRow[] FoundTickerRows = dt_Last_Price.Select("Ticker='" + BPS_Index_Ticker + "'");
                                    //Console.WriteLine("BPS_Index_Close=" + BPS_Index_Close.ToString() + "/" + FoundTickerRows.Length.ToString());
                                    foreach (DataRow dr in FoundTickerRows)
                                    {
                                        dr["ID_BB_UNIQUE"] = SystemLibrary.ToString(myItems[i]);
                                        dr["isNew"] = "Y";
                                    }
                                }
                            }
                        }
                    }

                    //if (myTicker.ToUpper() == "XPM2 INDEX")
                    //    Console.WriteLine("XPM2 INDEX");
                    if (isAlive_PortfolioTranspose)
                    {
                        //Update prices OnActivated this data
                        DataRow[] FoundTickerRowTrans = dt_PortfolioTranspose.Select("BBG_Ticker='" + myTicker + "'");

                        foreach (DataRow drt in FoundTickerRowTrans)
                        {
                            // Loop around fields
                            for (int i = 0; i < myFields.Length; i++)
                            {
                                try
                                {
                                    if (myItems[i].Substring(0, Math.Min("#N/A".Length, myItems[i].Length)) != "#N/A" && myItems[i].Length > 0)
                                    {
                                        //SystemLibrary.DebugLine(myTicker + "\t" + myFields[i] + "\t" + myItems[i]);
                                        switch (myFields[i])
                                        {
                                            case "LAST_PRICE":
                                                // On high frequency fields make sure that it really needs to be updated
                                                if (Math.Round(SystemLibrary.ToDecimal(drt["Price"]),8) != Math.Round(SystemLibrary.ToDecimal(myItems[i]),8))
                                                {
                                                    drt["Price"] = myItems[i];
                                                    NeedCalc = true;
                                                }
                                                TestPrice = myItems[i];
                                                break;
                                            case "THEO_PRICE":
                                                if (UseTheo_Price)
                                                {
                                                    // On high frequency fields make sure that it really needs to be updated
                                                    if (Math.Round(SystemLibrary.ToDecimal(drt["Price"]), 8) != Math.Round(SystemLibrary.ToDecimal(myItems[i]), 8))
                                                    {
                                                        drt["Price"] = myItems[i];
                                                        NeedCalc = true;
                                                    }
                                                }
                                                break;
                                            case "PX_POS_MULT_FACTOR":
                                                if (Math.Round(SystemLibrary.ToDecimal(drt["Pos_Mult_Factor"]), 8) != Math.Round(SystemLibrary.ToDecimal(myItems[i]), 8))
                                                {
                                                    drt["Pos_Mult_Factor"] = myItems[i];
                                                    NeedCalc = true;
                                                }
                                                break;
                                            case "PX_ROUND_LOT_SIZE":
                                                drt["Round_Lot_size"] = myItems[i];
                                                break;
                                            case "COUNTRY_FULL_NAME":
                                                drt["Country_Full_Name"] = myItems[i];
                                                break;
                                            case "CRNCY":
                                                // See if this is the First Time this record has crncy
                                                if (SystemLibrary.ToString(drt["crncy"]) == "")
                                                {
                                                    //Need to Load latest FX data into this record.
                                                    SetFXRate(drt, myItems[i]);
                                                    NeedCalc = true;
                                                }
                                                drt["crncy"] = myItems[i];
                                                CheckFX(myItems[i].ToUpper(), true);
                                                break;
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                    }

                    // Load up each Ticker
                    DataRow[] FoundTickerRow = dt_Port.Select("Ticker='" + myTicker + "'");

                    foreach (DataRow drt in FoundTickerRow)
                    {
                        // Loop around fields
                        for (int i = 0; i < myFields.Length; i++)
                        {
                            try
                            {
                                if (myItems[i].Substring(0, Math.Min("#N/A".Length, myItems[i].Length)) != "#N/A" && myItems[i].Length > 0)
                                {
                                    //SystemLibrary.SetDebugLevel(4);
                                    SystemLibrary.DebugLine(myTicker+"\t"+myFields[i] + "\t" + myItems[i]);
                                    switch (myFields[i])
                                    {
                                        case "NAME":
                                            // Only use Name is Security_Name is missing
                                            if (SystemLibrary.ToString(drt["SECURITY_NAME"]).Length == 0)
                                                LoadValue(drt, "Security_Name", true, myItems[i], Securities);
                                            break;
                                        case "SECURITY_NAME":
                                            LoadValue(drt, "Security_Name", true, myItems[i], Securities);
                                            break;
                                        case "CRNCY":
                                            // See if this is the First Time this record has crncy
                                            if (SystemLibrary.ToString(drt["crncy"]) == "")
                                            {
                                                //Need to Load latest FX data into this record.
                                                SetFXRate(drt, myItems[i]);
                                                NeedCalc = true;
                                            }
                                            LoadValue(drt, "crncy", true, myItems[i].ToUpper(), Securities);
                                            CheckFX(myItems[i].ToUpper(), true);
                                            break;
                                        case "COUNTRY_FULL_NAME":
                                            LoadValue(drt, "Country_Full_Name", true, myItems[i], Securities);
                                            break;
                                        case "INDUSTRY_SECTOR":
                                            LoadValue(drt, "Sector", true, myItems[i], Securities);
                                            break;
                                        case "INDUSTRY_GROUP":
                                            LoadValue(drt, "Industry_Group", true, myItems[i], Securities);
                                            break;
                                        case "INDUSTRY_SUBGROUP":
                                            LoadValue(drt, "Industry_SubGroup", true, myItems[i], Securities);
                                            break;
                                        case "MARKET_SECTOR_DES":
                                            if (myTicker.ToUpper().EndsWith("CURNCY"))
                                                LoadValue(drt, "Sector", true, "Currency", Securities);
                                            else
                                                LoadValue(drt, "Sector", true, myItems[i], Securities);
                                            break;
                                        case "SECURITY_TYP":
                                            LoadValue(drt, "Security_Typ", true, myItems[i], Securities);
                                            if (myTicker.ToUpper().EndsWith("CURNCY"))
                                                LoadValue(drt, "Industry_Group", true, myItems[i], Securities);
                                            break;
                                        case "SECURITY_TYP2":
                                            LoadValue(drt, "Security_Typ2", true, myItems[i], Securities);
                                            if (myTicker.ToUpper().EndsWith("CURNCY"))
                                                LoadValue(drt, "Industry_SubGroup", true, myItems[i], Securities);
                                            break;
                                        case "EXCH_CODE":
                                            LoadValue(drt, "BBG_Exchange", true, myItems[i], Securities);
                                            break;
                                        case "FUTURES_CATEGORY":
                                            LoadValue(drt, "Industry_Group", true, myItems[i], Securities);
                                            // Is there a better field for this - eg. Tell me a Gold Future is "Gold"?
                                            LoadValue(drt, "Industry_SubGroup", true, myItems[i], Securities);
                                            break;
                                        case "UNDERLYING_SECURITY_DES":
                                            // Bloomberg can put in a Generic exchange of "COMB" in Index & Commodity
                                            Undl_Ticker = myItems[i].Replace(" COMB ", " ");
                                            LoadValue(drt, "Undl_Ticker", true, Undl_Ticker, Securities);
                                            if (!LoadedUndelying.ContainsKey(Undl_Ticker))
                                            {
                                                LoadedUndelying.Add(Undl_Ticker, Undl_Ticker);
                                                BRT.Bloomberg_Request(Undl_Ticker);
                                            }
                                            break;
                                        case "UNDL_SPOT_TICKER":
                                            if (myTicker.Trim().Substring(myTicker.Length - 6, 6).ToUpper() == " INDEX")
                                            {
                                                Undl_Ticker = myItems[i] + " Index";
                                                LoadValue(drt, "Undl_Ticker", true, Undl_Ticker, Securities);
                                                if (!LoadedUndelying.ContainsKey(Undl_Ticker))
                                                {
                                                    LoadedUndelying.Add(Undl_Ticker, Undl_Ticker);
                                                    BRT.Bloomberg_Request(Undl_Ticker);
                                                }
                                            }
                                            else
                                            {
                                                Undl_Ticker = myItems[i];
                                                LoadValue(drt, "Undl_Ticker", true, Undl_Ticker, Securities);
                                                if (!LoadedUndelying.ContainsKey(Undl_Ticker))
                                                {
                                                    LoadedUndelying.Add(Undl_Ticker, Undl_Ticker);
                                                    BRT.Bloomberg_Request(Undl_Ticker);
                                                }
                                            }
                                            break;
                                        case "UNDL_CURRENCY":
                                            LoadValue(drt, "Undl_currency", true, myItems[i], Securities);
                                            NeedCalc = true; // Not sure about this one
                                            break;
                                        case "ID_BB_COMPANY":
                                            LoadValue(drt, "ID_BB_COMPANY", true, myItems[i], Securities);
                                            break;
                                        case "ID_BB_UNIQUE":
                                            LoadValue(drt, "ID_BB_UNIQUE", true, myItems[i], Securities);
                                            break;
                                        case "ID_BB_GLOBAL":
                                            LoadValue(drt, "ID_BB_GLOBAL", true, myItems[i], Securities);
                                            break;
                                        case "ID_CUSIP":
                                            LoadValue(drt, "CUSIP", true, myItems[i], Securities);
                                            break;
                                        case "ID_ISIN":
                                            LoadValue(drt, "ISIN", true, myItems[i], Securities);
                                            break;
                                        case "ID_SEDOL1":
                                            LoadValue(drt, "SEDOL", true, myItems[i], Securities);
                                            break;
                                        case "PREV_CLOSE_VAL":
                                        case "PREV_CLOSE_VALUE_REALTIME":
                                        case "YEST_LAST_TRADE":
                                            // Dont load if there was a SOD position
                                            if (SystemLibrary.ToInt32(drt["Quantity"]) == 0 && SystemLibrary.ToDouble(drt["prev_Close"]) != SystemLibrary.ToDouble(myItems[i]))
                                            {
                                                LoadValue(drt, "prev_Close", false, myItems[i], Securities);
                                                NeedCalc = true;
                                            }
                                            break;
                                        case "PX_POS_MULT_FACTOR":
                                            LoadValue(drt, "Pos_Mult_Factor", false, myItems[i], Securities);
                                            NeedCalc = true;
                                            break;
                                        case "PX_ROUND_LOT_SIZE":
                                            LoadValue(drt, "Round_Lot_size", false, myItems[i], Securities);
                                            break;
                                        case "LAST_PRICE":
                                        case "THEO_PRICE":
                                            //if (myTicker == "RIO AU Equity")
                                            //    Console.Write("A");
                                            // Set the foreground color, but probably just want the format to be up/down arrow?
                                            if (myFields[i]=="LAST_PRICE" || (UseTheo_Price && myFields[i]=="THEO_PRICE"))
                                            {
                                                try
                                                {
                                                    if (SystemLibrary.ToDecimal(myItems[i]) < SystemLibrary.ToDecimal(drt["Price"]))
                                                    {
                                                        PriceColour = Color.Red;
                                                    }
                                                    else if (SystemLibrary.ToDecimal(myItems[i]) > SystemLibrary.ToDecimal(drt["Price"]))
                                                    {
                                                        PriceColour = Color.Green;
                                                    }
                                                    else
                                                        PriceColour = Color.Black;
                                                }
                                                catch (Exception e)
                                                {
                                                    SystemLibrary.DebugLine("SetValue(" + myTicker + ") " + myFields[i] + " - " + e.Message);
                                                }

                                                // Set the item value
                                                if (Math.Round(SystemLibrary.ToDecimal(drt["Price"]), 8) != Math.Round(SystemLibrary.ToDecimal(myItems[i]), 8))
                                                {
                                                    drt["Price"] = myItems[i];
                                                    NeedCalc = true;
                                                    TestPrice = myItems[i];
                                                }
                                                drt["BBG_last_updatetime"] = SystemLibrary.f_Now();
                                            }
                                            break;
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    // Update Securities Table
                    String mySql = "";
                    String mySqlAnd = "";
                    DataRow[] drSecurities = dt_Securities.Select("BBG_Ticker='" + myTicker + "'");

                    foreach (String myKey in Securities.Keys)
                    {
                        if (drSecurities.Length > 0)
                        {
                            if (!(SystemLibrary.ToString(drSecurities[0][myKey]) == Securities[myKey].ToString() || "'" + SystemLibrary.ToString(drSecurities[0][myKey]) + "'" == Securities[myKey].ToString()))
                            {
                                // Double check rounding for a decimal
                                if (dt_Securities.Columns[myKey].DataType.Name.ToString() == "Decimal")
                                {
                                    try
                                    {
                                        int Ordinal = SystemLibrary.ToInt32(dt_Securities.Columns[myKey].Ordinal) - 1;
                                        if (Math.Round(SystemLibrary.ToDecimal(drSecurities[0][myKey]), Ordinal) == Math.Round(SystemLibrary.ToDecimal(Securities[myKey]), Ordinal))
                                            continue;
                                    }
                                    catch { }
                                }
                                mySql = mySql + myKey + " = " + Securities[myKey].ToString() + ", ";
                                mySqlAnd = mySqlAnd + myKey + " is Null Or " + myKey + " <> " + Securities[myKey].ToString() + " Or ";
                            }
                        }
                        else
                        {
                            mySql = mySql + myKey + " = " + Securities[myKey].ToString() + ", ";
                            mySqlAnd = mySqlAnd + myKey + " is Null Or " + myKey + " <> " + Securities[myKey].ToString() + " Or ";
                        }
                    }
                    if (mySql.Length > 0)
                    {
                        if (SystemLibrary.SQLSelectRowsCount("Select BBG_Ticker from Securities Where BBG_Ticker='" + myTicker + "' ") == 0)
                        {
                            String myInsert = "Insert into Securities (BBG_Ticker, Round_Lot_Size) Select '" + myTicker + "', 1 " +
                                              "Where not Exists (Select 'x' From Securities Where BBG_Ticker = '" + myTicker + "') ";
                            SystemLibrary.SQLExecute(myInsert);
                        }
                        mySql = mySql.Substring(0, mySql.Length - 2); // Strip off last ", "
                        if (mySqlAnd.Length > 0)
                        {
                            mySqlAnd = "And (" + mySqlAnd.Substring(0, mySqlAnd.Length - 3) + ") "; // Strip off last "Or "
                        }
                        mySql = "Update Securities Set " + mySql + " Where BBG_Ticker='" + myTicker + "' " + mySqlAnd;
                        SystemLibrary.SQLExecute(mySql);
                        //Console.WriteLine("pos2: " + mySql);
                    }
                    Securities.Clear();
                    mySql = "";
                    mySqlAnd = "";
                    // Load up each Undl_Ticker
                    FoundTickerRow = dt_Port.Select("Undl_Ticker='" + myTicker + "'");
                    foreach (DataRow drt in FoundTickerRow)
                    {
                        // Loop around fields
                        for (int i = 0; i < myFields.Length; i++)
                        {
                            try
                            {
                                if (myItems[i].Substring(0, Math.Min("#N/A".Length, myItems[i].Length)) != "#N/A" && myItems[i].Length > 0)
                                {
                                    switch (myFields[i])
                                    {
                                        case "SECURITY_NAME":
                                            LoadValue_undl(drt, "Security_Name", "", true, myItems[i], Securities);
                                            break;
                                        case "CRNCY":
                                            LoadValue_undl(drt, "crncy", "undl_currency", true, myItems[i].ToUpper(), Securities); //?? undl_Currency
                                            CheckFX(myItems[i].ToUpper(), true);
                                            NeedCalc = true;
                                            break;
                                        case "COUNTRY_FULL_NAME":
                                            LoadValue_undl(drt, "Country_Full_Name", "", true, myItems[i], Securities);
                                            break;
                                        case "INDUSTRY_SECTOR":
                                            LoadValue_undl(drt, "Sector", "Sector", true, myItems[i], Securities);
                                            break;
                                        case "INDUSTRY_GROUP":
                                            LoadValue_undl(drt, "Industry_Group", "Industry_Group", true, myItems[i], Securities);
                                            break;
                                        case "INDUSTRY_SUBGROUP":
                                            LoadValue_undl(drt, "Industry_SubGroup", "Industry_SubGroup", true, myItems[i], Securities);
                                            break;
                                        case "MARKET_SECTOR_DES":
                                            if (myTicker.ToUpper().EndsWith("CURNCY"))
                                                LoadValue_undl(drt, "Sector", "Sector", true, "Currency", Securities);
                                            else
                                                LoadValue_undl(drt, "Sector", "Sector", true, myItems[i], Securities);
                                            break;
                                        case "FUTURES_CATEGORY":
                                            LoadValue_undl(drt, "Industry_Group", "Industry_Group", true, myItems[i], Securities);
                                            // Is there a better field for this - eg. Tell me a Gold Future is "Gold"?
                                            LoadValue_undl(drt, "Industry_SubGroup", "Industry_SubGroup", true, myItems[i], Securities);
                                            break;
                                        case "ID_BB_COMPANY":
                                            LoadValue_undl(drt, "ID_BB_COMPANY", "", true, myItems[i], Securities);
                                            break;
                                        case "ID_BB_UNIQUE":
                                            LoadValue_undl(drt, "ID_BB_UNIQUE", "", true, myItems[i], Securities);
                                            break;
                                        case "ID_BB_GLOBAL":
                                            LoadValue_undl(drt, "ID_BB_GLOBAL", "", true, myItems[i], Securities);
                                            break;
                                        case "ID_CUSIP":
                                            LoadValue_undl(drt, "CUSIP", "", true, myItems[i], Securities);
                                            break;
                                        case "ID_ISIN":
                                            LoadValue_undl(drt, "ISIN", "", true, myItems[i], Securities);
                                            break;
                                        case "ID_SEDOL1":
                                            LoadValue_undl(drt, "SEDOL", "", true, myItems[i], Securities);
                                            break;
                                        case "PREV_CLOSE_VAL":
                                        case "PREV_CLOSE_VALUE_REALTIME":
                                        case "YEST_LAST_TRADE":
                                            // Dont load if there was a SOD position
                                            if (SystemLibrary.ToInt32(drt["Quantity"]) == 0 && SystemLibrary.ToInt32(drt["isAggregate"]) == 'N')
                                            {
                                                LoadValue_undl(drt, "prev_Close", "undl_prev_Close", false, myItems[i], Securities);
                                                NeedCalc = true;
                                            }
                                            break;
                                        case "PX_POS_MULT_FACTOR":
                                            LoadValue_undl(drt, "Pos_Mult_Factor", "", false, myItems[i], Securities);
                                            NeedCalc = true;
                                            break;
                                        case "PX_ROUND_LOT_SIZE":
                                            LoadValue_undl(drt, "Round_Lot_size", "", false, myItems[i], Securities);
                                            break;
                                        case "LAST_PRICE":
                                            if (Math.Round(SystemLibrary.ToDecimal(drt["undl_Price"]), 8) != Math.Round(SystemLibrary.ToDecimal(myItems[i]), 8))
                                            {
                                                drt["undl_Price"] = myItems[i];
                                                NeedCalc = true;
                                            }
                                            //LoadValue_undl(drt, "undl_Price", "", false, myItems[i], Securities);
                                            break;
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    // Update Securities Table for the Security record of the Underlying Ticker
                    drSecurities = dt_Securities.Select("BBG_Ticker='" + myTicker + "'");
                    foreach (String myKey in Securities.Keys)
                    {
                        if (drSecurities.Length > 0)
                        {
                            if (!(SystemLibrary.ToString(drSecurities[0][myKey]) == Securities[myKey].ToString() || "'" + SystemLibrary.ToString(drSecurities[0][myKey]) + "'" == Securities[myKey].ToString()))
                            {
                                // Double check rounding for a decimal
                                if (dt_Securities.Columns[myKey].DataType.Name.ToString() == "Decimal")
                                {
                                    try
                                    {
                                        int Ordinal = SystemLibrary.ToInt32(dt_Securities.Columns[myKey].Ordinal) - 1;
                                        if (Math.Round(SystemLibrary.ToDecimal(drSecurities[0][myKey]), Ordinal) == Math.Round(SystemLibrary.ToDecimal(Securities[myKey]), Ordinal))
                                            continue;
                                    }
                                    catch { }
                                }
                                mySql = mySql + myKey + " = " + Securities[myKey].ToString() + ", ";
                                mySqlAnd = mySqlAnd + myKey + " is Null Or " + myKey + " <> " + Securities[myKey].ToString() + " Or ";
                            }
                        }
                        else
                        {
                            mySql = mySql + myKey + " = " + Securities[myKey].ToString() + ", ";
                            mySqlAnd = mySqlAnd + myKey + " is Null Or " + myKey + " <> " + Securities[myKey].ToString() + " Or ";
                        }
                    }
                    if (mySql.Length > 0)
                    {
                        if (SystemLibrary.SQLSelectRowsCount("Select BBG_Ticker from Securities Where BBG_Ticker='" + myTicker + "' ") == 0)
                        {
                            String myInsert = "Insert into Securities (BBG_Ticker, Round_Lot_Size) Select '" + myTicker + "', 1 " +
                                              "Where not Exists (Select 'x' From Securities Where BBG_Ticker = '" + myTicker + "') ";
                            SystemLibrary.SQLExecute(myInsert);
                        }
                        
                        mySql = mySql.Substring(0, mySql.Length - 2); // Strip off last ", "
                        if (mySqlAnd.Length > 0)
                        {
                            mySqlAnd = "And (" + mySqlAnd.Substring(0, mySqlAnd.Length - 3) + ") "; // Strip off last "Or "
                        }
                        mySql = "Update Securities Set " + mySql + " Where BBG_Ticker='" + myTicker + "' " + mySqlAnd;
                        SystemLibrary.SQLExecute(mySql);
                        //Console.WriteLine("pos3: " + mySql);
                    }
                    Securities.Clear();

                    // Update Last Price table for database storage
                    int myPos = Array.IndexOf(myFields, "LAST_PRICE");
                    if (myPos > -1)
                    {
                        if (!myItems[myPos].StartsWith("#N/A") && myItems[myPos].Length > 0)
                        {
                            DataRow[] FoundTickerRows = dt_Last_Price.Select("Ticker='" + myTicker + "'");
                            //Console.WriteLine("BPS_Index_Close=" + BPS_Index_Close.ToString() + "/" + FoundTickerRows.Length.ToString());
                            foreach (DataRow dr in FoundTickerRows)
                            {
                                dr["LAST_PRICE"] = SystemLibrary.ToDecimal(myItems[myPos]);
                                dr["isNew"] = "Y";
                            }
                        }
                    }

                    // - Is the Ticker an FX
                    if (myTicker.ToUpper().EndsWith("CURNCY"))
                    {
                        // Update the dt_FX table
                        DataRow[] FoundRows = dt_FX.Select("Ticker='" + myTicker + "'");
                        foreach (DataRow dr in FoundRows)
                        {
                            Securities.Clear();
                            for (int i = 0; i < myFields.Length; i++)
                            {
                                // Set the item value
                                if (!myItems[i].StartsWith("#N/A") && myItems[i].Length > 0)
                                {
                                    switch (myFields[i])
                                    {
                                        case "LAST_PRICE":
                                            if (Math.Round(SystemLibrary.ToDecimal(dr[myFields[i]]), 8) != Math.Round(SystemLibrary.ToDecimal(myItems[i]), 8))
                                            {
                                                dr[myFields[i]] = myItems[i];
                                                NeedCalc = true;
                                                TestPrice = myItems[i];
                                            }
                                            break;
                                        case "PX_POS_MULT_FACTOR":
                                            if (Math.Round(SystemLibrary.ToDecimal(dr[myFields[i]]), 8) != Math.Round(SystemLibrary.ToDecimal(myItems[i]), 8))
                                            {
                                                dr[myFields[i]] = myItems[i];
                                                if (!Securities.ContainsKey("Pos_Mult_Factor"))
                                                {
                                                    Securities.Add("Pos_Mult_Factor", myItems[i]);
                                                    NeedCalc = true;
                                                }
                                            }
                                            break;
                                        // For Securities Table where an implicit FX - versus a True FX Position
                                        case "CRNCY":
                                            if (!Securities.ContainsKey(myFields[i]))
                                            {
                                                Securities.Add(myFields[i], "'" + myItems[i].ToUpper() + "'");
                                                NeedCalc = true;
                                            }
                                            break;
                                        case "SECURITY_NAME":
                                        case "COUNTRY_FULL_NAME":
                                        case "INDUSTRY_GROUP":
                                        case "INDUSTRY_SUBGROUP":
                                        case "FUTURES_CATEGORY":
                                        case "ID_BB_COMPANY":
                                        case "ID_BB_UNIQUE":
                                        case "ID_BB_GLOBAL":
                                            if (!Securities.ContainsKey(myFields[i]))
                                                Securities.Add(myFields[i], "'" + myItems[i] + "'");
                                            break;
                                        case "SECURITY_TYP": // Maybe should be Industry_Group
                                            if (!Securities.ContainsKey(myFields[i]))
                                            {
                                                Securities.Add(myFields[i], "'" + myItems[i].Replace("'", "''") + "'");
                                                Securities.Add("Industry_Group", "'" + myItems[i].Replace("'", "''") + "'");
                                            }
                                            break;
                                        case "SECURITY_TYP2": // Maybe should be Industry_SubGroup
                                            if (!Securities.ContainsKey(myFields[i]))
                                            {
                                                Securities.Add(myFields[i], "'" + myItems[i].Replace("'", "''") + "'");
                                                Securities.Add("Industry_SubGroup", "'" + myItems[i].Replace("'", "''") + "'");
                                            }
                                            break;
                                        case "EXCH_CODE":
                                            if (!Securities.ContainsKey("BBG_Exchange"))
                                                Securities.Add("BBG_Exchange", "'" + myItems[i] + "'");
                                            break;
                                        /*
                                        case "PREV_CLOSE_VAL":
                                        case "PREV_CLOSE_VALUE_REALTIME":
                                        case "YEST_LAST_TRADE":
                                            if (!Securities.ContainsKey("prev_Close"))
                                                Securities.Add("prev_Close", "'" + myItems[i] + "'");
                                            break;
                                        */
                                        case "INDUSTRY_SECTOR":
                                        case "MARKET_SECTOR_DES":
                                            if (!Securities.ContainsKey("Sector"))
                                            {
                                                if (myTicker.ToUpper().EndsWith("CURNCY"))
                                                    Securities.Add("Sector", "'Currency'");
                                                else
                                                    Securities.Add("Sector", "'" + myItems[i].Replace("'", "''") + "'");
                                            }
                                            break;
                                        case "ID_CUSIP":
                                            if (!Securities.ContainsKey("CUSIP"))
                                                Securities.Add("CUSIP", "'" + myItems[i] + "'");
                                            break;
                                        case "ID_ISIN":
                                            if (!Securities.ContainsKey("ISIN"))
                                                Securities.Add("ISIN", "'" + myItems[i] + "'");
                                            break;
                                        case "ID_SEDOL1":
                                            if (!Securities.ContainsKey("SEDOL"))
                                                Securities.Add("SEDOL", "'" + myItems[i] + "'");
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            // Update Securities Table for the Security record of the Underlying Ticker
                            drSecurities = dt_Securities.Select("BBG_Ticker='" + myTicker + "'");
                            foreach (String myKey in Securities.Keys)
                            {
                                if (drSecurities.Length > 0)
                                {
                                    if (!(SystemLibrary.ToString(drSecurities[0][myKey]) == Securities[myKey].ToString() || "'" + SystemLibrary.ToString(drSecurities[0][myKey]) + "'" == Securities[myKey].ToString()))
                                    {
                                        // Double check rounding for a decimal
                                        if (dt_Securities.Columns[myKey].DataType.Name.ToString() == "Decimal")
                                        {
                                            try
                                            {
                                                int Ordinal = SystemLibrary.ToInt32(dt_Securities.Columns[myKey].Ordinal) - 1;
                                                if (Math.Round(SystemLibrary.ToDecimal(drSecurities[0][myKey]), Ordinal) == Math.Round(SystemLibrary.ToDecimal(Securities[myKey]), Ordinal))
                                                    continue;
                                            }
                                            catch { }
                                        }
                                        mySql = mySql + myKey + " = " + Securities[myKey].ToString() + ", ";
                                        mySqlAnd = mySqlAnd + myKey + " is Null Or " + myKey + " <> " + Securities[myKey].ToString() + " Or ";
                                    }
                                }
                                else
                                {
                                    mySql = mySql + myKey + " = " + Securities[myKey].ToString() + ", ";
                                    mySqlAnd = mySqlAnd + myKey + " is Null Or " + myKey + " <> " + Securities[myKey].ToString() + " Or ";
                                }

                            }
                            if (mySql.Length > 0)
                            {
                                if (SystemLibrary.SQLSelectRowsCount("Select BBG_Ticker from Securities Where BBG_Ticker='" + myTicker + "' ") == 0)
                                {
                                    String myInsert = "Insert into Securities (BBG_Ticker, Round_Lot_Size, Sector, Industry_Group, Industry_SubGroup) Select '" + myTicker + "', 1, 'Currency', 'Currency', 'Currency' " +
                                                      "Where not Exists (Select 'x' From Securities Where BBG_Ticker = '" + myTicker + "') ";
                                    SystemLibrary.SQLExecute(myInsert);
                                }
                                mySql = mySql.Substring(0, mySql.Length - 2); // Strip off last ", "
                                if (mySqlAnd.Length > 0)
                                {
                                    mySqlAnd = "And (" + mySqlAnd.Substring(0, mySqlAnd.Length - 3) + ") "; // Strip off last "Or "
                                }
                                mySql = "Update Securities Set " + mySql + " Where BBG_Ticker='" + myTicker + "' " + mySqlAnd;
                                SystemLibrary.SQLExecute(mySql);
                                //Console.WriteLine("pos4: " + mySql);
                            }

                        }
                    }
                }

                // Now do the calculations
                //if (myTicker.EndsWith("Curncy"))
                //    Console.Write("A");
                //Application.DoEvents();
                if (myTicker == "DOW AU Equity")
                    Console.WriteLine(myTicker + "," + inEditMode.ToString() + NeedCalc.ToString());
                if (!inEditMode && NeedCalc)
                {
                    //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - pre: SetCalc(" + myTicker + "," + TestPrice + ")");
                    if (TickerList.ContainsKey(myTicker))
                        TickerList.Remove(myTicker);
                    TickerList.Add(myTicker, PriceColour);
                        
                    // CFR 20140203 SetCalc(myTicker, PriceColour);
                }
                //SystemLibrary.DebugLine("SetValue() - End");
                //Application.DoEvents();
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - End: SetValue(" + myTicker + ")");
            }

        } // SetValue()


        public void LoadValue(DataRow drt, String ColName, Boolean IsString, String myItem, Hashtable Securities)
        {
            // Local Variables
            Boolean itemChanged = true;

            SystemLibrary.DebugLine("LoadValue("+drt["Ticker"].ToString() + "," + ColName + "," + myItem+")");

            // TODO (5) - UK shares: Is there a Bloomberg fields to deal with UK prices in Pence. (NB: Currency = GBp, but doesn't help Local Value)
            try
            {
                if (ColName == "Pos_Mult_Factor" &&
                   drt["Ticker"].ToString().ToUpper().EndsWith("EQUITY") &&
                   drt["Ticker"].ToString().ToUpper().Contains(" LN ")
                  )
                {
                    myItem = Convert.ToString(SystemLibrary.ToDecimal(myItem) / 100.0M);
                }
            }
            catch { }

            try
            {
                if (ColName == "Security_Typ2")
                    if (myItem.ToUpper() == "Future".ToUpper())
                    {
                        if (drt["isFuture"].ToString() == "Y")
                            itemChanged = false;
                        else
                            drt["isFuture"] = "Y";
                    }
                    else
                    {
                        if (drt["isFuture"].ToString() == "N")
                            itemChanged = false;
                        else
                            drt["isFuture"] = "N";
                    }
                if (drt.Table.Columns.Contains(ColName))
                {
                    switch(drt[ColName].GetType().Name)
                    {
                        case "Decimal":
                            if (SystemLibrary.ToDecimal(drt[ColName]) == SystemLibrary.ToDecimal(myItem))
                                itemChanged = false;  
                            break;
                        case "String":
                        case "DBNull":
                            if (drt[ColName].ToString() == myItem)
                                itemChanged = false;  
                            break;
                        default:
                            Console.WriteLine(ColName+"="+drt[ColName].GetType().Name);
                            break;
                    }

                    if (drt["Ticker"].ToString().ToUpper().EndsWith(" CURNCY") && 
                        (ColName == "Industry_Group" || ColName == "Industry_SubGroup" || ColName == "Sector") &&
                        (SystemLibrary.ToString(drt[ColName]) == "Currency" || SystemLibrary.ToString(drt[ColName]) == "Cash Equiv" || SystemLibrary.ToString(drt[ColName]) == "Equity Equiv")
                        )
                    {
                        itemChanged = false;
                    }
                    else
                        drt[ColName] = myItem;
                }
            }
            catch (Exception e)
            {
                SystemLibrary.DebugLine("LoadValue(" + ColName + "):" + e.Message);
            }

            try
            {
                //drt.ItemArray.Contains(ColName)
                if (drt.Table.Columns.Contains(ColName) || ColName == "Security_Typ" || ColName == "Security_Typ2" || ColName == "BBG_Exchange")
                {
                    if (itemChanged)
                    {
                        // Only need to do this if the item is different than last time
                        if (!Securities.ContainsKey(ColName))
                        {
                            if (IsString)
                            {
                                // Deal with Quotes in Strings
                                Securities.Add(ColName, "'" + myItem.Replace("'", "''") + "'");
                            }
                            else
                                Securities.Add(ColName, myItem);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SystemLibrary.DebugLine("LoadValue(" + ColName + ").Securities.Add():" + e.Message);
            }

        } //LoadValue()

        // There are two issues here:
        //  1) The fields on the ticker (MainTicker_ColName)    [eg. undl_Price]
        //  2) A Security that represents the Underlying ticker (Undl_ColName)  [eg. Price]
        public void LoadValue_undl(DataRow drt, String Undl_ColName, String MainTicker_ColName, Boolean IsString, String myItem, Hashtable Securities)
        {
            // Local Variables
            Boolean itemChanged = true;
            String myTicker = drt["Ticker"].ToString();
            String mySql;
            String mySqlAnd;
            SystemLibrary.DebugLine("LoadValue_undl(" + drt["Ticker"].ToString() + "," + Undl_ColName + "," + MainTicker_ColName + "," + myItem + ")");

            // Update the DataRow record for the Main Ticker (eg. Where this applies to Options, the main Ticker is the Option Ticker)
            try
            {
                // cfr 20120215 if (Undl_ColName.Length > 0)
                if (MainTicker_ColName.Length > 0)
                {
                    if (drt.Table.Columns.Contains(MainTicker_ColName))
                    {
                        switch (drt[MainTicker_ColName].GetType().Name)
                        {
                            case "Decimal":
                                if (SystemLibrary.ToDecimal(drt[MainTicker_ColName]) == SystemLibrary.ToDecimal(myItem))
                                    itemChanged = false;
                                break;
                            case "String":
                            case "DBNull":
                                if (drt[MainTicker_ColName].ToString() == myItem)
                                    itemChanged = false;
                                break;
                            default:
                                Console.WriteLine(MainTicker_ColName + "=" + drt[MainTicker_ColName].GetType().Name);
                                break;
                        }

                        drt[MainTicker_ColName] = myItem;
                    }
                    // cfr 20120215 drt[Undl_ColName] = myItem;
                }
            }
            catch (Exception e)
            {
                SystemLibrary.DebugLine("LoadValue_undl(" + Undl_ColName + "):" + e.Message);
            }
            // Update the Database record for the Main Ticker (eg. Where this applies to Options, the main Ticker is the Option Ticker)
            try
            {
                if (MainTicker_ColName.Length > 0)
                {
                    if (itemChanged)
                    {
                        // I Have the SQL's in here for the main ticker updates rather than batched as low occurence and few columns
                        // Don't need to insert a row as this happened when the ticker was updated with the undl_ticker
                        if (IsString)
                        {
                            mySql = MainTicker_ColName + " = '" + myItem + "' ";
                            mySqlAnd = "( " + MainTicker_ColName + " <> '" + myItem + "' OR " +
                                       MainTicker_ColName + " is null ) ";
                        }
                        else
                        {
                            mySql = MainTicker_ColName + " = " + myItem + " ";
                            mySqlAnd = "(" + MainTicker_ColName + " is Null Or " + MainTicker_ColName + " <> " + myItem + ") ";
                        }
                        mySql = "Update Securities Set " + mySql + " Where BBG_Ticker='" + myTicker + "' And " + mySqlAnd;
                        SystemLibrary.SQLExecute(mySql);
                        //Console.WriteLine("pos1: " + mySql);
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLibrary.DebugLine("LoadValue_undl(Undl_ColName=" + Undl_ColName + "):" + ex.Message);
            }

            // Update the Securities hastable for the Underlying Ticker (eg. "MSFT US Equity" for a Microsft Option)
            try
            {
                if (Undl_ColName.Length > 0)
                {
                    if (itemChanged)
                    {
                        if (!Securities.ContainsKey(Undl_ColName))
                        {
                            if (IsString)
                                Securities.Add(Undl_ColName, "'" + myItem.Replace("'", "''") + "'");
                            else
                                Securities.Add(Undl_ColName, myItem);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SystemLibrary.DebugLine("LoadValue_undl(" + Undl_ColName + ").Securities.Add():" + e.Message);
            }

        } //LoadValue_undl()

        public void SetFXRate(DataRow drt, String myCrncy)
        {
            // Local Variables
            Decimal FXRate;
            String myTicker = myCrncy + Fund_Crncy + " Curncy";

            DataRow[] FoundRows = dt_FX.Select("Ticker='" + myTicker + "'");
            if (FoundRows.Length > 0)
            {
                // Need to Update the FX rate incorporating the multiplier
                FXRate = SystemLibrary.ToDecimal(FoundRows[0]["LAST_PRICE"]) *
                         SystemLibrary.ToDecimal(FoundRows[0]["PX_POS_MULT_FACTOR"]);
                if (FXRate > 0)
                {
                    drt["FXRate"] = FXRate;
                }
            }

        } //SetFXRate()

        public delegate void CheckFXCallback(String Ticker_Crncy);
        public void CheckFX(String Ticker_Crncy)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                CheckFXCallback cb = new CheckFXCallback(CheckFX);
                this.Invoke(cb, new object[] { Ticker_Crncy });
            }
            else
            {

                CheckFX(Ticker_Crncy, false);
            }
        } //CheckFX()

        public void CheckFX(String Ticker_Crncy, Boolean FromAdvise)
        {
            // Purpose: See if this is a new currency & if yes, load it into the FX table.
            //

            if (Ticker_Crncy == "")
                return;

            // See if it is in dt_FX and if not then add
            DataRow[] FoundRows = dt_FX.Select("FromFX='" + Ticker_Crncy + "'");
            if (FoundRows.Length == 0)
            {
                // Start a Bloomberg Request for the new currency
                DataRow dr = dt_FX.NewRow();
                dr["Ticker"] = Ticker_Crncy + Fund_Crncy + " Curncy";
                dr["FromFX"] = Ticker_Crncy;
                dr["ToFX"] = Fund_Crncy;
                dr["PX_POS_MULT_FACTOR"] = 1;
                dr["LAST_PRICE"] = 1;
                dt_FX.Rows.Add(dr);
                if (Ticker_Crncy != Fund_Crncy && FromAdvise)
                {
                    BRT.Bloomberg_Request(dr["Ticker"].ToString());
                }
            }

        } // CheckFX()

        public delegate void SetColumnCallback(DataGridView dgr, String ColName, Int32 myRow);
        public void SetColumn(DataGridView dgr, String ColName, Int32 myRow)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                SetColumnCallback cb = new SetColumnCallback(SetColumn);
                this.Invoke(cb, new object[] { dgr, ColName, myRow });
            }
            else
            {
                Color myColour;
                if (SystemLibrary.ToDouble(dgr[ColName, myRow].Value) < 0)
                    myColour = Color.Red;
                else
                    myColour = Color.Green;

                if (dgr[ColName, myRow].Style.ForeColor != myColour)
                    dgr[ColName, myRow].Style.ForeColor = myColour;
            }
        } // SetColumn()

        /*
        public delegate void SetColumnCallback(DataRow dr, String ColName);
        public void SetColumn(DataRow dr, String ColName)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                SetColumnCallback cb = new SetColumnCallback(SetColumn);
                this.Invoke(cb, new object[] { dr, ColName });
            }
            else
            {
                if (SystemLibrary.ToDouble(dr[ColName].Value) < 0)
                    dr[ColName].Style.ForeColor = Color.Red;
                else
                    dr[ColName].Style.ForeColor = Color.Green;
            }
        } // SetColumn()
        */

        public delegate void SetFormatColumnCallback(DataGridView dgv, String ColName, Color myForeColor, Color myBackColor, String myFormat, String myNullValue);
        public void SetFormatColumn(DataGridView dgv, String ColName, Color myForeColor, Color myBackColor, String myFormat, String myNullValue)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                SetFormatColumnCallback cb = new SetFormatColumnCallback(SetFormatColumn);
                this.Invoke(cb, new object[] { dgv, ColName, myForeColor, myBackColor, myFormat, myNullValue });
            }
            else
            {
                // Local Variables
                DataGridViewCellStyle dGVCS = new DataGridViewCellStyle();

                if (myForeColor != Color.Empty)
                    dGVCS.ForeColor = myForeColor;
                if (myBackColor != Color.Empty)
                    dGVCS.BackColor = myBackColor;
                if (myFormat.Length > 0)
                    dGVCS.Format = myFormat;
                if (myNullValue.Length > 0)
                    dGVCS.NullValue = myNullValue;
                dgv.Columns[ColName].DefaultCellStyle = dGVCS;
            }
        } // SetFormat


        public void SetValueRG(DataGridViewRow dgr, String myColumn, Decimal myValue)
        {
            // Local Variables
            Color myColour;
            try
            {
                if (SystemLibrary.ToDecimal(dgr.Cells[myColumn].Value) != myValue)
                    dgr.Cells[myColumn].Value = myValue;
                /*
                 * Colin Ritchie; 22-Feb-2014
                 * Trying to use the dg_Port_CellPainting() event
                 * If this works then need to search for all references of this call and add the same event to dg_Header & dg_Port_Transpose
                 */
                if (myValue < 0)
                    myColour = Color.Red;
                else if (myValue > 0)
                    myColour = Color.Green;
                else
                    myColour = Color.Black;
                if (dgr.Cells[myColumn].Style.ForeColor != myColour)
                    dgr.Cells[myColumn].Style.ForeColor = myColour;
            }
            catch (Exception e)
            {
                SystemLibrary.DebugLine("SetValueRG(" + dgr.Cells["Ticker"].Value.ToString() + "," + myColumn + ") - Error=" + e.Message);
            }

        } // SetValueRG()

        public delegate void SetCalc_From_TickerListCallback();
        public void SetCalc_From_TickerList()
        {
            if (InvokeRequired)
            {
                // Is from a different thread.
                SystemLibrary.DebugLine("SetCalc_From_TickerList() - InvokeRequired");
                SetCalc_From_TickerListCallback cb = new SetCalc_From_TickerListCallback(SetCalc_From_TickerList);
                this.Invoke(cb, new object[] { });
            }
            else
            {
                foreach (KeyValuePair<String, Color> kvp in TickerList)
                {
                    SetCalc((String)kvp.Key, (Color)kvp.Value, true);
                }
                SetGrossPCT();
                TickerList.Clear();
            }
        } //SetCalc_From_TickerList()

        public void SetCalc()
        {
            // Local Variables
            String Ticker = "";
            String LastTicker = "";

            // Calc All LoadPortfolio() - pre: StartRTD()
            foreach (DataGridViewRow dgr in dg_Port.Rows)
            {
                if (dgr.Cells["Ticker"].Value != null)
                {
                    // Only do for live ticker
                    if (dgr.Cells["IsAggregate"].Value.ToString().ToUpper() == "N")
                    {
                        Ticker = dgr.Cells["Ticker"].Value.ToString();
                        if (LastTicker != Ticker)
                        {
                            SetCalc(Ticker, Color.Empty, true);
                            LastTicker = Ticker;
                        }
                    }
                }
            }
            if (dt_Port != null)
            {
                SetGrossPCT();
            }
        } // SetCalc()

        public void SetCalc(String myTicker, Color PriceColour)
        {
            SetCalc(myTicker, PriceColour, false);
        } //SetCalc()

        public void SetCalc(String myTicker, Color PriceColour, Boolean PartOfArray)
        {
            // Procedure: SetCalc
            //
            // Purpose: Update dg_Port for all the calculated columns.
            //          I know at this stage that a refresh is needed for the ticker
            //

            // Local Variables
            Decimal POS_Mult_Factor;
            Decimal PL_Today_Prev;
            Decimal PL_Day;
            Decimal PL_Exec;
            Decimal PL_Diff;
            Decimal Value_Prev;
            Decimal LocalValue;
            Decimal Value;
            Decimal Value_SOD;
            Decimal Value_Filled;
            Decimal Exposure;
            Decimal Pct_FUM;
            Decimal Pct_SOD;
            Decimal FXRate = -12345;
            Decimal PCT_Change;
            Decimal Price;
            Decimal Prev_Close;
            Decimal Quantity;
            Decimal Qty_Order;
            Decimal Qty_Fill;
            Decimal Div_Adjusted;
            Color PriceColour_From_dg_Port;


            //SystemLibrary.DebugLine("SetCalc(" + myTicker + "," + PriceColour.ToString() + ", " + PartOfArray.ToString() + ")");

            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - Start: SetCalc(" + myTicker + ", Array=" + SystemLibrary.Bool_To_YN(PartOfArray) + ")");

            SystemLibrary.DebugLine("SetCalc(" + myTicker+") - Start");

            // See if this is a valid Ticker, or Undl_Ticker and update all the calculated fields
            DataGridViewRow[] dg_in = FindValue(dg_Port, "Ticker", myTicker);
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - Post FindValue()=" + dg_in.Length.ToString());
            if (dg_in.Length > 0)
            {
                Prev_Close = SystemLibrary.ToDecimal(dg_in[0].Cells["prev_Close"].Value);
                Price = SystemLibrary.ToDecimal(dg_in[0].Cells["Price"].Value);
                PriceColour_From_dg_Port = dg_in[0].Cells["Price"].Style.ForeColor;
                for (Int32 j = 0; j < dg_in.Length; j++)
                {
                    // Only do for live ticker
                    if (dg_in[j].Cells["IsAggregate"].Value.ToString().ToUpper() == "N")
                    {
                        FXRate = SystemLibrary.ToDecimal(dg_in[j].Cells["FXRate"].Value);
                        if (FXRate == 0)
                            if (SystemLibrary.ToString(dg_in[j].Cells["Crncy"].Value) == Fund_Crncy)
                            {
                                dg_in[j].Cells["FXRate"].Value = 1;
                                FXRate = 1;
                            }

                        // Set the PriceColour
                        if (PriceColour != Color.Empty)
                            dg_in[j].Cells["Price"].Style.ForeColor = PriceColour;
                        // Get The POS_Mult_Factor
                        POS_Mult_Factor = SystemLibrary.ToDecimal(dg_in[j].Cells["POS_Mult_Factor"].Value);
                        if (POS_Mult_Factor <= 0)
                            POS_Mult_Factor = 1;
                        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - Part 2399: SetCalc(" + myTicker + ")");

                        // Calcs 
                        Quantity = SystemLibrary.ToDecimal(dg_in[j].Cells["Quantity"].Value);
                        Qty_Order = SystemLibrary.ToDecimal(dg_in[j].Cells["Qty_Order"].Value);
                        Qty_Fill = SystemLibrary.ToDecimal(dg_in[j].Cells["Qty_Fill"].Value);
                        Div_Adjusted = SystemLibrary.ToDecimal(dg_in[j].Cells["Div_Adjusted"].Value);
                        LocalValue = SystemLibrary.ToDecimal(Price * POS_Mult_Factor * (Quantity + Qty_Order + SystemLibrary.ToDecimal(dg_in[j].Cells["Quantity_incr"].Value)));
                        if (Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells["LocalValue"].Value),2) != Math.Round(LocalValue,2))
                            dg_in[j].Cells["LocalValue"].Value = LocalValue;
                        //SetValueRG(dg_in[j], "LocalValue", SystemLibrary.ToDecimal(Price * POS_Mult_Factor * (Quantity + Qty_Order + SystemLibrary.ToDecimal(dg_in[j].Cells["Quantity_incr"].Value))));
                        Value_Filled = SystemLibrary.ToDecimal(Price * POS_Mult_Factor * (Quantity + Qty_Fill)) * FXRate;
                        Value_Prev = SystemLibrary.ToDecimal(dg_in[j].Cells["Value"].Value);
                        Value = SystemLibrary.ToDecimal(dg_in[j].Cells["LocalValue"].Value) * FXRate;
                        Value_SOD = SystemLibrary.ToDecimal(Price * POS_Mult_Factor * (Quantity)) * FXRate;
                        Gross_Amount = Gross_Amount - Math.Abs(Value_Prev) + Math.Abs(Value);
                        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - Part 2412: SetCalc(" + myTicker + ")");
                        if (Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells["Value"].Value), 2) != Math.Round(Value, 2))
                            dg_in[j].Cells["Value"].Value = Value;
                        //SetValueRG(dg_in[j], "Value", Value);
                        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - Part 2414: SetCalc(" + myTicker + ")");
                        if (Fund_Amount != 0)
                        {
                            Pct_FUM = Value / Fund_Amount;
                            Pct_SOD = Value_SOD / Fund_Amount;
                            if (Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells[@"% FUM"].Value),4) != Math.Round(Pct_FUM,4))
                                dg_in[j].Cells[@"% FUM"].Value = Pct_FUM;
                            if (Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells[@"% SOD"].Value),4) != Math.Round(Pct_SOD,4))
                                dg_in[j].Cells[@"% SOD"].Value = Pct_SOD;
                            //SetValueRG(dg_in[j], @"% FUM", Value / Fund_Amount);
                            //SetValueRG(dg_in[j], @"% SOD", Value_SOD / Fund_Amount);
                        }
                        // TODO (4) Exposure calc needs to be changed for options - ie. Using Underlying price & Delta. (Now Exposures = Value)
                        // TODO (4) Exposure then needs to be used for %FUM & %GROSS
                        Exposure = Value * FXRate;
                        if (Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells["Exposure"].Value),2) != Math.Round(Exposure,2))
                            dg_in[j].Cells["Exposure"].Value = Exposure;
                        if(Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells["Exposure_Filled"].Value),2) != Math.Round(Value_Filled,2))
                            dg_in[j].Cells["Exposure_Filled"].Value = Value_Filled;
                        //SetValueRG(dg_in[j], "Exposure", Value * FXRate);
                        //SetValueRG(dg_in[j], "Exposure_Filled", Value_Filled);
                        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - Part 2424: SetCalc(" + myTicker + ")");

                        // MTD, et al P&L's rely on Todays data, so extract the old values for PL_DAY & PL_EXEC before updating
                        PL_Today_Prev = SystemLibrary.ToDecimal(dg_in[j].Cells["PL_Day"].Value) + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_Exec"].Value);
                        PL_Day = (Quantity * POS_Mult_Factor * (Price - (Prev_Close - Div_Adjusted))) * FXRate;
                        PL_Exec = (Qty_Fill * POS_Mult_Factor * (Price - SystemLibrary.ToDecimal(dg_in[j].Cells["Avg_Price"].Value))) * FXRate;
                        if(Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells["PL_Day"].Value), 2) != Math.Round(PL_Day, 2))
                            dg_in[j].Cells["PL_Day"].Value = PL_Day;
                        if(Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells["PL_Exec"].Value),2) != Math.Round(PL_Exec,2))
                            dg_in[j].Cells["PL_Exec"].Value = PL_Exec;
                        //SetValueRG(dg_in[j], "PL_Day", PL_Day);
                        //SetValueRG(dg_in[j], "PL_Exec", PL_Exec);
                        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - Part 2432: SetCalc(" + myTicker + ")");
                        if (Fund_Amount != 0)
                        {
                            Decimal Pct_Fill;
                            Decimal PL_BPS;
                            Decimal PL_EOD;
                            Decimal Qty_EOD;
                            Decimal Qty_EOD_Fill;
                            Pct_Fill = Value_Filled / Fund_Amount;
                            PL_BPS = (PL_Day + PL_Exec) / Fund_Amount * 100m;
                            PL_EOD = PL_Day + PL_Exec;
                            Qty_EOD = Quantity + Qty_Order;
                            Qty_EOD_Fill = Quantity + Qty_Fill;

                            if(Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells[@"% Fill"].Value),4) != Math.Round(Pct_Fill,4))
                                dg_in[j].Cells[@"% Fill"].Value = Pct_Fill;
                            if(Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells[@"PL BPS"].Value),2) != Math.Round(PL_BPS,2))
                                dg_in[j].Cells[@"PL BPS"].Value = PL_BPS;
                            if(Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells[@"PL_EOD"].Value),2) != Math.Round(PL_EOD,2))
                                dg_in[j].Cells[@"PL_EOD"].Value = PL_EOD;
                            if(Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells[@"Qty_EOD"].Value),2) != Math.Round(Qty_EOD,2))
                                dg_in[j].Cells[@"Qty_EOD"].Value = Qty_EOD;
                            if (Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells[@"Qty_EOD_Fill"].Value), 2) != Math.Round(Qty_EOD_Fill, 2))
                                dg_in[j].Cells[@"Qty_EOD_Fill"].Value = Qty_EOD_Fill;
                            //SetValueRG(dg_in[j], @"% Fill", Value_Filled / Fund_Amount);
                            //SetValueRG(dg_in[j], @"PL BPS", (PL_Day + PL_Exec) / Fund_Amount * 100m);
                            //SetValueRG(dg_in[j], @"PL_EOD", PL_Day + PL_Exec);
                            //SetValueRG(dg_in[j], @"Qty_EOD", Quantity + Qty_Order);
                            //SetValueRG(dg_in[j], @"Qty_EOD_Fill", Quantity + Qty_Fill);
                        }
                        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - Part 2440: SetCalc(" + myTicker + ")");

                        // - Below are P&L's that need diff between this update & last update
                        PL_Diff = PL_Day + PL_Exec - PL_Today_Prev;

                        Decimal PL_TradePeriod = PL_Diff + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_TradePeriod"].Value);
                        Decimal PL_WRoll = PL_Diff + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_WRoll"].Value);
                        Decimal PL_MRoll = PL_Diff + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_MRoll"].Value);
                        Decimal PL_MTD = PL_Diff + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_MTD"].Value);
                        Decimal PL_DeltaMax = PL_Diff + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_DeltaMax"].Value);
                        Decimal PL_Inception = PL_Diff + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_Inception"].Value);
                        Decimal PL_YTD = PL_Diff + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_YTD"].Value);
                        Decimal PL_YTD_July = PL_Diff + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_YTD_July"].Value);

                        if (Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells[@"PL_TradePeriod"].Value),2) != Math.Round(PL_TradePeriod,2))
                            dg_in[j].Cells[@"PL_TradePeriod"].Value = PL_TradePeriod;
                        if (Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells[@"PL_WRoll"].Value),2) != Math.Round(PL_WRoll, 2))
                            dg_in[j].Cells[@"PL_WRoll"].Value = PL_WRoll;
                        if (Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells[@"PL_MRoll"].Value),2) != Math.Round(PL_MRoll,2))
                            dg_in[j].Cells[@"PL_MRoll"].Value = PL_MRoll;
                        if (Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells[@"PL_MTD"].Value),2) != Math.Round(PL_MTD,2))
                            dg_in[j].Cells[@"PL_MTD"].Value = PL_MTD;
                        if (Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells[@"PL_DeltaMax"].Value),2) != Math.Round(PL_DeltaMax,2))
                            dg_in[j].Cells[@"PL_DeltaMax"].Value = PL_DeltaMax;
                        if (Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells[@"PL_Inception"].Value),2) != Math.Round(PL_Inception,2))
                            dg_in[j].Cells[@"PL_Inception"].Value = PL_Inception;
                        if (Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells[@"PL_YTD"].Value),2) != Math.Round(PL_YTD,2))
                            dg_in[j].Cells[@"PL_YTD"].Value = PL_YTD;
                        if (Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells[@"PL_YTD_July"].Value),2) != Math.Round(PL_YTD_July,2))
                            dg_in[j].Cells[@"PL_YTD_July"].Value = PL_YTD_July;

                        
                        //SetValueRG(dg_in[j], "PL_TradePeriod", PL_Diff + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_TradePeriod"].Value));
                        //SetValueRG(dg_in[j], "PL_WRoll", PL_Diff + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_WRoll"].Value));
                        //SetValueRG(dg_in[j], "PL_MRoll", PL_Diff + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_MRoll"].Value));
                        //SetValueRG(dg_in[j], "PL_MTD", PL_Diff + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_MTD"].Value));
                        //SetValueRG(dg_in[j], "PL_DeltaMax", PL_Diff + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_DeltaMax"].Value));
                        //SetValueRG(dg_in[j], "PL_Inception", PL_Diff + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_Inception"].Value));
                        //SetValueRG(dg_in[j], "PL_YTD", PL_Diff + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_YTD"].Value));
                        //SetValueRG(dg_in[j], "PL_YTD_July", PL_Diff + SystemLibrary.ToDecimal(dg_in[j].Cells["PL_YTD_July"].Value));
                        
                        // PCT Change
                        try
                        {
                            if ((Prev_Close - Div_Adjusted) != 0)
                                PCT_Change = (Price / (Prev_Close - Div_Adjusted) - 1.0M);
                            else
                                PCT_Change = 0;
                        }
                        catch
                        {
                            PCT_Change = 0;
                        }
                        if (Math.Round(SystemLibrary.ToDecimal(dg_in[j].Cells[@"% Chg"].Value), 4) != Math.Round(PCT_Change, 4))
                            dg_in[j].Cells[@"% Chg"].Value = PCT_Change;
                        //SetValueRG(dg_in[j], @"% Chg", PCT_Change);
                        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - Part 2466: SetCalc(" + myTicker + ")");

                        //if (myTicker.ToUpper() == "XPM2 INDEX")
                        //    Console.WriteLine("here XPM2 INDEX");
                        if (isAlive_PortfolioTranspose)
                        {
                            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - Part 2472: SetCalc(" + myTicker + ")");
                            // See if this is a valid Ticker, or Undl_Ticker and update all the calculated fields
                            DataGridViewRow[] dg_tran = FindValue(dg_PortfolioTranspose, "BBG_Ticker", myTicker);
                            if (dg_tran.Length > 0)
                            {
                                Char[] mySeperatorRow = { ',' };
                                String[] AllFunds = dg_PortfolioTranspose.Columns["BBG_Ticker"].Tag.ToString().Split(mySeperatorRow, StringSplitOptions.RemoveEmptyEntries);

                                for (Int32 k = 0; k < dg_tran.Length; k++)
                                {
                                    //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - Part 2482: SetCalc(" + myTicker + ")");
                                    // Set the PriceColour
                                    if (PriceColour != Color.Empty)
                                        dg_tran[k].Cells["Price"].Style.ForeColor = PriceColour;
                                    else
                                        dg_tran[k].Cells["Price"].Style.ForeColor = PriceColour_From_dg_Port;
                                    dg_tran[k].Cells["Price"].Value = Price;

                                    if (AllFunds.Length > 0)
                                    {
                                        for (Int32 af = 0; af < AllFunds.Length; af++)
                                        {
                                            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - Part 2494: SetCalc(" + myTicker + ")");
                                            String[] myFundIDFundAmount = AllFunds[af].Split('\t');
                                            if (myFundIDFundAmount.Length == 2)
                                            {
                                                int myFund = Convert.ToInt32(myFundIDFundAmount[0]);
                                                // TODO (5) Using Start-of-Day Fund_Amount where could also take into account value change on the day using Price & Prev_Close.
                                                //          Very little incremental value though. If Portfolio is up 5% on the day, then position would be 5% out, so on a 5% pos, this would be 0.25%.
                                                Decimal myFund_Amount = Convert.ToDecimal(myFundIDFundAmount[1]);
                                                LocalValue = SystemLibrary.ToDecimal(Price * POS_Mult_Factor * (SystemLibrary.ToDecimal(dg_tran[k].Cells["Quantity_" + myFundIDFundAmount[0]].Value) + SystemLibrary.ToDecimal(dg_tran[k].Cells["Incr_" + myFundIDFundAmount[0]].Value)));
                                                if (Math.Round(SystemLibrary.ToDecimal(dg_tran[k].Cells["LocalValue_" + myFundIDFundAmount[0]].Value), 2) != Math.Round(LocalValue, 2))
                                                    dg_tran[k].Cells["LocalValue_" + myFundIDFundAmount[0]].Value = LocalValue;
                                                //SetValueRG(dg_tran[k], "LocalValue_" + myFundIDFundAmount[0], SystemLibrary.ToDecimal(Price * POS_Mult_Factor * (SystemLibrary.ToDecimal(dg_tran[k].Cells["Quantity_" + myFundIDFundAmount[0]].Value) + SystemLibrary.ToDecimal(dg_tran[k].Cells["Incr_" + myFundIDFundAmount[0]].Value))));
                                                //Value = SystemLibrary.ToDecimal(dg_tran[k].Cells["LocalValue_" + myFundIDFundAmount[0]].Value) * FXRate;
                                                Value = LocalValue * FXRate;
                                                if (Math.Round(SystemLibrary.ToDecimal(dg_tran[k].Cells["Value_" + myFundIDFundAmount[0]].Value), 2) != Math.Round(Value, 2))
                                                    dg_tran[k].Cells["Value_" + myFundIDFundAmount[0]].Value = Value;
                                                //SetValueRG(dg_tran[k], "Value_" + myFundIDFundAmount[0], Value);
                                                if (myFund_Amount != 0)
                                                {
                                                    Decimal Weight = Value / myFund_Amount;
                                                    if (Math.Round(SystemLibrary.ToDecimal(dg_tran[k].Cells[@"Weight_" + myFundIDFundAmount[0]].Value),2) != Math.Round(Weight,2))
                                                        dg_tran[k].Cells[@"Weight_" + myFundIDFundAmount[0]].Value = Weight;
                                                    //SetValueRG(dg_tran[k], @"Weight_" + myFundIDFundAmount[0], Value / myFund_Amount);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //drt["Price"] = myItems[i];
                    //drt["BBG_last_updatetime"] = System.DateTime.Now;
                }
            }
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - Part: SetCalc(" + myTicker + ")");

 
            // See if the Ticker is FX, and if so update all the records with this currency
            if (myTicker.ToUpper().EndsWith("CURNCY"))
            {
                //SystemLibrary.DebugLine(myTicker + " - " + DateTime.Now.ToString());
                // Get the FXRate from dt_FX
                //Ticker_Crncy = SystemLibrary.BBGTicker(myTicker);
                DataRow[] FoundRows = dt_FX.Select("Ticker='" + myTicker + "'");
                if (FoundRows.Length > 0)
                {
                    // Need to Update the FX rate incorporating the multiplier
                    FXRate = SystemLibrary.ToDecimal(FoundRows[0]["LAST_PRICE"]) *
                             SystemLibrary.ToDecimal(FoundRows[0]["PX_POS_MULT_FACTOR"]);
                    if (FXRate > 0)
                    {
                        // Update the dg_Port table for each row that is this FX
                        DataGridViewRow[] dg_fxUpdate = FindValue(dg_Port, "CRNCY", myTicker.Substring(0, 3));
                        if (dg_fxUpdate.Length > 0)
                        {
                            for (int i = 0; i < dg_fxUpdate.Length; i++)
                            {
                                // Only bother if significant
                                if (Math.Round(SystemLibrary.ToDecimal(dg_fxUpdate[i].Cells["FXRate"].Value) * SystemLibrary.ToDecimal(dg_fxUpdate[i].Cells["LocalValue"].Value), 2) !=
                                    Math.Round(SystemLibrary.ToDecimal(FXRate) * SystemLibrary.ToDecimal(dg_fxUpdate[i].Cells["LocalValue"].Value), 2)
                                   )
                                {
                                    dg_fxUpdate[i].Cells["FXRate"].Value = FXRate;
                                    // SystemLibrary.DebugLine(dg_fxUpdate[i].Cells["Ticker"].Value.ToString() + "=" + FXRate.ToString()+" - "+DateTime.Now.ToString());
                                    // Need to do calcs that require this FX rate
                                    SetCalc(dg_fxUpdate[i].Cells["Ticker"].Value.ToString(), Color.Empty, true);
                                }
                            }
                        }
                    }
                }
            }

            // Send screen updates
            if (!PartOfArray)
            {
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - Pre: SetGrossPCT()");
                SetGrossPCT();
            }
            //            SystemLibrary.DebugLine("SetCalc(" + myTicker + ") - END");
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - End: SetCalc(" + myTicker + ")");

        } // SetCalc()


        public void SetGrossPCT()
        {
            // Local Variables
            //String myRowFilter = myDataView.RowFilter;
            Decimal myExposure;
            Decimal myExposure_Filled;
            Decimal PL_Day;
            String myLS;
            String AddFilter = "";

            SystemLibrary.DebugLine("SetGrossPCT() - Start");

            Long_Amount = 0;
            Short_Amount = 0;
            Future_Amount = 0;
            Long_Amount_Filled = 0;
            Short_Amount_Filled = 0;
            Future_Amount_Filled = 0;
            Long_Positions = 0;
            Short_Positions = 0;
            Future_Positions = 0;
            Long_Winners = 0;
            Short_Winners = 0;
            Future_Winners = 0;
            Long_PL = 0;
            Short_PL = 0;
            Future_PL = 0;
            Gross_Amount = 0;

            // Need to do this otherwise just get the filtered rows
            //this.SuspendLayout();
            //myDataView.RowFilter = "";

            //if (Gross_Amount != 0)
            {
                // Clear the Datatable
                GrossPctTable.Rows.Clear();

                // Load the Datatable
                //CFR Console.WriteLine("Start");
                for (Int32 i = 0; i <  dt_Port.Rows.Count; i++)
                {

                    String IsAggregate = SystemLibrary.ToString(dt_Port.Rows[i]["IsAggregate"]);

                    if (dt_Port.Rows[i]["Ticker"].ToString() != "" && IsAggregate == "N")
                    {
                        myExposure = SystemLibrary.ToDecimal(dt_Port.Rows[i]["Exposure"]);
                        myExposure_Filled = SystemLibrary.ToDecimal(dt_Port.Rows[i]["Exposure_Filled"]);
                        myLS = SystemLibrary.ToString(dt_Port.Rows[i]["LS"]);
                        PL_Day = SystemLibrary.ToDecimal(dt_Port.Rows[i]["PL_Day"]) + SystemLibrary.ToDecimal(dt_Port.Rows[i]["PL_Exec"]);

                        // See if ticker already exists
                        String myTicker = dt_Port.Rows[i]["Ticker"].ToString();
                        DataRow[] dr = GrossPctTable.Select("BBG_Ticker='" + myTicker + "'");
                        if (dr.Length > 0)
                        {
                            /*
                            if (myTicker == "RIO AU Equity")
                            {
                                Console.WriteLine(myTicker + " (dr.Length > 0) - " + SystemLibrary.ToDecimal(dr[0]["Exposure"]).ToString() + "," + myExposure.ToString() + ",Filled=" + SystemLibrary.ToDecimal(dr[0]["Exposure_Filled"]).ToString() + "," + myExposure_Filled.ToString());
                            }
                            */
                            // Exposure is at the Ticker level so that same stocks are calculated on a net basis.
                            dr[0]["Exposure"] = SystemLibrary.ToDecimal(dr[0]["Exposure"]) + myExposure;
                            dr[0]["Exposure_Filled"] = SystemLibrary.ToDecimal(dr[0]["Exposure_Filled"]) + myExposure_Filled;
                            dr[0]["PL"] = SystemLibrary.ToDecimal(dr[0]["PL"]) + PL_Day;
                        }
                        else
                        {
                            DataRow myRow = GrossPctTable.NewRow();
                            myRow["BBG_Ticker"] = myTicker;
                            myRow["Exposure"] = myExposure;
                            myRow["Exposure_Filled"] = myExposure_Filled;
                            myRow["LS"] = myLS;
                            myRow["PL"] = PL_Day;
                            // TODO (1) I need a real flag passeed back that says if it is a future
                            if (SystemLibrary.ToString(dt_Port.Rows[i]["isFuture"]) == "Y")
                                myRow["isFuture"] = "Y";
                            else
                                myRow["isFuture"] = "N";

                            GrossPctTable.Rows.Add(myRow);
                            /*
                            if (myTicker == "RIO AU Equity")
                            {
                                Console.WriteLine(myTicker + " - " + myExposure.ToString() + ",Filled=" + myExposure_Filled.ToString() + ", myLS=" + myLS);
                            }
                            */
                        }

                        /* Colin: Not sure what to do here yet.
                         * 
                        // Special case. Did it come from a long or a short?
                        Decimal myQuantity = SystemLibrary.ToDecimal(dg_Port.Rows[i].Cells["Quantity"].Value);
                        Decimal myDelta = SystemLibrary.ToDecimal(dg_Port.Rows[i].Cells["Delta"].Value);
                        if (myDelta == 0)
                            myDelta = 1;
                        if (myQuantity * myDelta < 0)
                        {
                            Short_PL = Short_PL + PL_Day;
                        }
                        else
                        {
                            Long_PL = Long_PL + PL_Day;
                        }
                         */
                    }
                }

                // Set LS in GrossPctTable the direction of Exposure when it is not zero.
                // - in 99% of cases, will set back to what it already is.
                for (int i = 0; i < GrossPctTable.Rows.Count; i++)
                {
                    Decimal myExp = SystemLibrary.ToDecimal(GrossPctTable.Rows[i]["Exposure"]);
                    /*
                    String myTicker = GrossPctTable.Rows[i]["BBG_Ticker"].ToString();
                    if (myTicker == "RIO AU Equity")
                    {
                        Console.WriteLine("myExp=" + myExp.ToString());
                    }
                    */
                    if (myExp > 0)
                        GrossPctTable.Rows[i]["LS"] = "L";
                    else if (myExp < 0)
                        GrossPctTable.Rows[i]["LS"] = "S";
                    else
                    {
                        // work out the start of day exposure
                        // TODO (4) Exposure calc needs to be changed for options - ie. Using Underlying price & Delta. (Now Exposures = Value)
                        Decimal myExp_SOD = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(Quantity)", "Ticker='" + GrossPctTable.Rows[i]["BBG_Ticker"].ToString() + "'"));
                        /*
                        if (myTicker == "RIO AU Equity")
                        {
                            Console.WriteLine("myExp_SOD=" + myExp_SOD.ToString());
                        }
                        */
                        if (myExp_SOD > 0)
                            GrossPctTable.Rows[i]["LS"] = "L";
                        else if (myExp_SOD < 0)
                            GrossPctTable.Rows[i]["LS"] = "S";
                    }
                }

                // Extract the results
                if (ShowFutureHeaderLine)
                {
                    AddFilter = " and isFuture='N'";
                    Future_Amount = SystemLibrary.ToDecimal(GrossPctTable.Compute("Sum(Exposure)", "isFuture='Y'"));
                    Future_Amount_Filled = SystemLibrary.ToDecimal(GrossPctTable.Compute("Sum(Exposure_Filled)", "isFuture='Y'"));
                    Future_Positions = SystemLibrary.ToInt32(GrossPctTable.Compute("Count(Exposure)", "isFuture='Y'"));
                    if (Future_Amount == 0)
                        Future_Positions = 0;
                    else if (Future_Amount < 0)
                        Future_Positions = -Future_Positions;
                    Future_Winners = SystemLibrary.ToInt32(GrossPctTable.Compute("Count(PL)", "PL>0 and isFuture='Y'"));
                    Future_PL = SystemLibrary.ToDecimal(GrossPctTable.Compute("Sum(PL)", "isFuture='Y'")); 
                }
                Long_Amount = SystemLibrary.ToDecimal(GrossPctTable.Compute("Sum(Exposure)", "LS='L'" + AddFilter));
                Short_Amount = SystemLibrary.ToDecimal(GrossPctTable.Compute("Sum(Exposure)", "LS='S'" + AddFilter));
                Long_Amount_Filled = SystemLibrary.ToDecimal(GrossPctTable.Compute("Sum(Exposure_Filled)", "LS='L'" + AddFilter));
                Short_Amount_Filled = SystemLibrary.ToDecimal(GrossPctTable.Compute("Sum(Exposure_Filled)", "LS='S'" + AddFilter));
                Long_Positions = SystemLibrary.ToInt32(GrossPctTable.Compute("Count(Exposure)", "LS='L'" + AddFilter));
                Short_Positions = SystemLibrary.ToInt32(GrossPctTable.Compute("Count(Exposure)", "LS='S'" + AddFilter));
                Long_Winners = SystemLibrary.ToInt32(GrossPctTable.Compute("Count(PL)", "PL>0 and LS='L'" + AddFilter));
                Short_Winners = SystemLibrary.ToInt32(GrossPctTable.Compute("Count(PL)", "PL>0 and LS='S'" + AddFilter));
                Long_PL = SystemLibrary.ToDecimal(GrossPctTable.Compute("Sum(PL)", "LS='L'" + AddFilter));
                Short_PL = SystemLibrary.ToDecimal(GrossPctTable.Compute("Sum(PL)", "LS='S'" + AddFilter));
                Gross_Amount = Math.Abs(Long_Amount) + Math.Abs(Short_Amount) + Math.Abs(Future_Amount);
                //CFR Console.WriteLine("End:" + Long_PL.ToString() + "," + Short_PL.ToString());
                // Reset %Gross
                for (Int32 i = 0; i < dg_Port.Rows.Count; i++)
                {
                    if (dg_Port.Rows[i].Cells["Ticker"].Value != null)
                    {
                        myExposure = SystemLibrary.ToDecimal(dg_Port.Rows[i].Cells["Exposure"].Value);
                        if (Gross_Amount != 0)
                        {
                            Decimal PCT_Gross = myExposure / Gross_Amount;
                            if (Math.Round(SystemLibrary.ToDecimal(dg_Port.Rows[i].Cells[@"% Gross"].Value), 4) != Math.Round(PCT_Gross, 4))
                                dg_Port.Rows[i].Cells[@"% Gross"].Value = PCT_Gross;
                            //SetValueRG(dg_Port.Rows[i], @"% Gross", myExposure / Gross_Amount);
                        }
                    }
                }

            }

            // Put back the RowFilter
            //myDataView.RowFilter = myRowFilter;
            //this.ResumeLayout();

            // Update Header record
            SetHeader();
            SystemLibrary.DebugLine("SetGrossPCT() - End");

        } //SetGrossPCT()


        public void SetGrossPCT_pre20120404()
        {
            // Local Variables
            //String myRowFilter = myDataView.RowFilter;
            Decimal myExposure;
            Decimal myExposure_Filled;
            Decimal PL_Day;
            String myLS;

            Long_Amount = 0;
            Short_Amount = 0;
            Long_Amount_Filled = 0;
            Short_Amount_Filled = 0;
            Long_Positions = 0;
            Short_Positions = 0;
            Long_Winners = 0;
            Short_Winners = 0;
            Long_PL = 0;
            Short_PL = 0;
            Gross_Amount = 0;

            // Need to do this otherwise just get the filtered rows
            //this.SuspendLayout();
            //myDataView.RowFilter = "";

            //if (Gross_Amount != 0)
            {
                // Create the Datatable
                DataTable myTable = new DataTable();

                DataColumn BBG_Ticker = new DataColumn("BBG_Ticker");
                DataColumn Exposure = new DataColumn("Exposure");
                DataColumn Exposure_Filled = new DataColumn("Exposure_Filled");
                DataColumn LS = new DataColumn("LS");
                DataColumn PL = new DataColumn("PL");


                BBG_Ticker.DataType = System.Type.GetType("System.String");
                Exposure.DataType = System.Type.GetType("System.Decimal");
                Exposure_Filled.DataType = System.Type.GetType("System.Decimal");
                LS.DataType = System.Type.GetType("System.String");
                PL.DataType = System.Type.GetType("System.Decimal");

                myTable.Columns.Add(BBG_Ticker);
                myTable.Columns.Add(Exposure);
                myTable.Columns.Add(Exposure_Filled);
                myTable.Columns.Add(LS);
                myTable.Columns.Add(PL);

                // Load the Datatable
                //CFR Console.WriteLine("Start");
                for (Int32 i = 0; i < dg_Port.Rows.Count; i++)
                {
                    String IsAggregate = SystemLibrary.ToString(dg_Port.Rows[i].Cells["IsAggregate"].Value);

                    if (dg_Port.Rows[i].Cells["Ticker"].Value != null && IsAggregate == "N")
                    {
                        myExposure = SystemLibrary.ToDecimal(dg_Port.Rows[i].Cells["Exposure"].Value);
                        myExposure_Filled = SystemLibrary.ToDecimal(dg_Port.Rows[i].Cells["Exposure_Filled"].Value);
                        myLS = SystemLibrary.ToString(dg_Port.Rows[i].Cells["LS"].Value);
                        PL_Day = SystemLibrary.ToDecimal(dg_Port.Rows[i].Cells["PL_Day"].Value) + SystemLibrary.ToDecimal(dg_Port.Rows[i].Cells["PL_Exec"].Value);
                        //CFR Console.WriteLine(dg_Port.Rows[i].Cells["Ticker"].Value.ToString()+","+myExposure.ToString()+","+ PL_Day.ToString());
                        if (Gross_Amount != 0)
                            SetValueRG(dg_Port.Rows[i], @"% Gross", myExposure / Gross_Amount);

                        // See if ticker already exists
                        String myTicker = dg_Port.Rows[i].Cells["Ticker"].Value.ToString();
                        DataRow[] dr = myTable.Select("BBG_Ticker='" + myTicker + "'");
                        if (dr.Length > 0)
                        {
                            // Exposure is at the Ticker level so that same stocks are calculated on a net basis.
                            dr[0]["Exposure"] = SystemLibrary.ToDecimal(dr[0]["Exposure"]) + myExposure;
                            dr[0]["Exposure_Filled"] = SystemLibrary.ToDecimal(dr[0]["Exposure_Filled"]) + myExposure_Filled;
                            dr[0]["PL"] = SystemLibrary.ToDecimal(dr[0]["PL"]) + PL_Day;
                        }
                        else
                        {
                            DataRow myRow = myTable.NewRow();
                            myRow["BBG_Ticker"] = myTicker;
                            myRow["Exposure"] = myExposure;
                            myRow["Exposure_Filled"] = myExposure_Filled;
                            myRow["LS"] = myLS;
                            myRow["PL"] = PL_Day;
                            myTable.Rows.Add(myRow);
                        }

                        /* Colin: Not sure what to do here yet.
                         * 
                        // Special case. Did it come from a long or a short?
                        Decimal myQuantity = SystemLibrary.ToDecimal(dg_Port.Rows[i].Cells["Quantity"].Value);
                        Decimal myDelta = SystemLibrary.ToDecimal(dg_Port.Rows[i].Cells["Delta"].Value);
                        if (myDelta == 0)
                            myDelta = 1;
                        if (myQuantity * myDelta < 0)
                        {
                            Short_PL = Short_PL + PL_Day;
                        }
                        else
                        {
                            Long_PL = Long_PL + PL_Day;
                        }
                         */
                    }
                }

                // Set LS in myTable the direction of Exposure when it is not zero.
                // - in 99% of cases, will set back to what it already is.
                for (int i = 0; i < myTable.Rows.Count; i++)
                {
                    Decimal myExp = SystemLibrary.ToDecimal(myTable.Rows[i]["Exposure"]);
                    if (myExp > 0)
                        myTable.Rows[i]["LS"] = "L";
                    else if (myExp < 0)
                        myTable.Rows[i]["LS"] = "S";
                }

                // Extract the results
                Long_Amount = SystemLibrary.ToDecimal(myTable.Compute("Sum(Exposure)", "LS='L'"));
                Short_Amount = SystemLibrary.ToDecimal(myTable.Compute("Sum(Exposure)", "LS='S'"));
                Long_Amount_Filled = SystemLibrary.ToDecimal(myTable.Compute("Sum(Exposure_Filled)", "LS='L'"));
                Short_Amount_Filled = SystemLibrary.ToDecimal(myTable.Compute("Sum(Exposure_Filled)", "LS='S'"));
                Long_Positions = SystemLibrary.ToInt32(myTable.Compute("Count(Exposure)", "LS='L'"));
                Short_Positions = SystemLibrary.ToInt32(myTable.Compute("Count(Exposure)", "LS='S'"));
                Long_Winners = SystemLibrary.ToInt32(myTable.Compute("Count(PL)", "PL>0 and LS='L'"));
                Short_Winners = SystemLibrary.ToInt32(myTable.Compute("Count(PL)", "PL>0 and LS='S'"));
                Long_PL = SystemLibrary.ToDecimal(myTable.Compute("Sum(PL)", "LS='L'"));
                Short_PL = SystemLibrary.ToDecimal(myTable.Compute("Sum(PL)", "LS='S'"));
                Gross_Amount = Math.Abs(Long_Amount) + Math.Abs(Short_Amount);
                //CFR Console.WriteLine("End:" + Long_PL.ToString() + "," + Short_PL.ToString());
                // Reset %Gross
                for (Int32 i = 0; i < dg_Port.Rows.Count; i++)
                {
                    if (dg_Port.Rows[i].Cells["Ticker"].Value != null)
                    {
                        myExposure = SystemLibrary.ToDecimal(dg_Port.Rows[i].Cells["Exposure"].Value);
                        if (Gross_Amount != 0)
                            SetValueRG(dg_Port.Rows[i], @"% Gross", myExposure / Gross_Amount);
                    }
                }

            }

            // Put back the RowFilter
            //myDataView.RowFilter = myRowFilter;
            //this.ResumeLayout();

            // Update Header record
            SetHeader();

        } //SetGrossPCT_pre20120404()

        public DataGridViewRow[] FindValue(DataGridView dg_in, String Column_in, String Value_in)
        {
            // Local Variables
            DataGridViewRow[] RetVal = new DataGridViewRow[0];
            Int32 RetVal_Count = 0;

            foreach (DataGridViewRow currentRow in dg_in.Rows)
            {
                if (currentRow.Cells[Column_in].Value != null)
                {
                    if (currentRow.Cells[Column_in].Value.ToString() == Value_in)
                    {
                        Array.Resize<DataGridViewRow>(ref RetVal, RetVal_Count + 1);
                        RetVal[RetVal_Count] = currentRow;
                        RetVal_Count++;
                    }
                }
            }
            return (RetVal);

        } // FindValue()



        #region Registry Example Code
        public void ColinSampleRegistry()
        {
            String myValue;
            Registry.Registry myReg = new global::T1MultiAsset.Registry.Registry();

            myValue = myReg.RegGetValue("HKEY_LOCAL_MACHINE", @"SOFTWARE\Bloomberg L.P.\GSP.INI\Setting\App Folders", "EMS").ToString();
            //myValue = myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1MultiAsset\Position", "Top").ToString();
            if (myValue.Length > 0)
            {
                MessageBox.Show(myValue);
            }

        } //ColinSampleRegistry()
        #endregion //Registry Example Code

        private void AddTickersPortfolioTranspose(String inString)
        {
            AddTickers(inString, true, true);

        } //AddTickersPortfolioTranspose()

        private void AddTickers(String inString)
        {
            AddTickers(inString, false, false);
        }

        private void AddTickers(String inString, Boolean FromPortfolioTranspose, Boolean AddToPortfolioTranspose)
        {
            // Local Variables
            String DispTicker;
            // Seperate the Tickers
            Char[] mySeperatorRow = { '\r', '\n' };
            //Char[] mySeperatorField = { '\t', ',' }; // // - Could use '\t' or ',' [Comma] separators to allow Ticker, Weight
            String[] TickerList = inString.Split(mySeperatorRow, StringSplitOptions.RemoveEmptyEntries);

            if (!FromPortfolioTranspose)
            {
                // Make sure I am on the correct Tab first
                if (tabControl_Port.SelectedTab.Text != "TRADE")
                    tabControl_Port.SelectTab("tp_Trade");
            }

            // Add the rows
            foreach (String Ticker in TickerList)
            {
                if (Ticker.Trim().Length > 0) // Allows for having both CR/LF issues
                {
                    if (Ticker.Trim().Contains(" "))
                    {
                        // Adding new Ticker to dt_Port.
                        // - ? Should I ignore if already in dt_Port??
                        // Make sure Ticker is represented in Bloomberg fashion
                        DispTicker = SystemLibrary.BBGGetTickerNormalised(Ticker);
                        DataRow myRow = dt_Port.NewRow();
                        myRow["Ticker"] = DispTicker;
                        myRow["Quantity"] = 0;
                        myRow["Qty_Order"] = 0;
                        myRow["Qty_Routed"] = 0;
                        myRow["Qty_Fill"] = 0;
                        myRow["IsAggregate"] = "N";
                        myRow["FXRate"] = 0;
                        myRow["FX_Pos_Mult_Factor"] = 1;
                        myRow["FundId"] = FundID;
                        myRow["PortfolioID"] = PortfolioID;
//colin
                        // For speed
                        // - See if we already have this ticker in live data
                        DataRow[] dr = dt_Port.Select("Ticker='" + DispTicker + "'");
                        if (dr.Length > 0)
                        {
                            myRow["Price"] = dr[0]["Price"];
                            myRow["Security_Name"] = dr[0]["Security_Name"];
                            myRow["crncy"] = dr[0]["crncy"];
                            myRow["prev_Close"] = dr[0]["prev_Close"];
                            myRow["Div_Adjusted"] = dr[0]["Div_Adjusted"];
                            myRow["Pos_Mult_Factor"] = dr[0]["Pos_Mult_Factor"];
                            myRow["Round_Lot_Size"] = dr[0]["Round_Lot_Size"];
                            myRow["FXRate"] = dr[0]["FXRate"];
                            myRow["FX_Pos_Mult_Factor"] = dr[0]["FX_Pos_Mult_Factor"];
                            myRow["Undl_Ticker"] = dr[0]["Undl_Ticker"];
                            myRow["Undl_Currency"] = dr[0]["Undl_Currency"];
                            myRow["Undl_Price"] = dr[0]["Undl_Price"];
                            myRow["Delta"] = dr[0]["Delta"];
                            myRow["Country_Full_Name"] = dr[0]["Country_Full_Name"];
                            myRow["Sector"] = dr[0]["Sector"];
                            myRow["Industry_Group"] = dr[0]["Industry_Group"];
                            myRow["Industry_SubGroup"] = dr[0]["Industry_SubGroup"];
                            myRow["Beta"] = dr[0]["Beta"];
                            myRow["ID_BB_COMPANY"] = dr[0]["ID_BB_COMPANY"];
                            myRow["ID_BB_UNIQUE"] = dr[0]["ID_BB_UNIQUE"];
                            myRow["ID_BB_GLOBAL"] = dr[0]["ID_BB_GLOBAL"];
                            myRow["CUSIP"] = dr[0]["CUSIP"];
                            myRow["ISIN"] = dr[0]["ISIN"];
                            myRow["SEDOL"] = dr[0]["SEDOL"];
                            myRow["isFuture"] = dr[0]["isFuture"];
                        }
                        else
                        {
                            // Go to the database and get as much as can
                            String mySql = "Select BBG_Ticker as Ticker, Security_Name, crncy, Pos_Mult_Factor, Round_Lot_Size, Undl_Ticker, Undl_Currency, Delta, " +
                                           "       Country_Full_Name, Sector, Industry_Group, Industry_SubGroup, Beta, ID_BB_COMPANY, ID_BB_UNIQUE, " +
                                           "        Case When isNull(Security_Typ2,'') = 'Future' Then 'Y' Else 'N' End as isFuture " +
                                           "From Securities " +
                                           "Where BBG_Ticker = '" + DispTicker + "' ";
                            DataTable dt_new = SystemLibrary.SQLSelectToDataTable(mySql);

                            if (dt_new.Rows.Count == 1)
                            {
                                myRow["Security_Name"] = dt_new.Rows[0]["Security_Name"];
                                myRow["crncy"] = dt_new.Rows[0]["crncy"];
                                myRow["Pos_Mult_Factor"] = dt_new.Rows[0]["Pos_Mult_Factor"];
                                myRow["Round_Lot_Size"] = dt_new.Rows[0]["Round_Lot_Size"];
                                myRow["Undl_Ticker"] = dt_new.Rows[0]["Undl_Ticker"];
                                myRow["Undl_Currency"] = dt_new.Rows[0]["Undl_Currency"];
                                myRow["Delta"] = dt_new.Rows[0]["Delta"];
                                myRow["Country_Full_Name"] = dt_new.Rows[0]["Country_Full_Name"];
                                myRow["Sector"] = dt_new.Rows[0]["Sector"];
                                myRow["Industry_Group"] = dt_new.Rows[0]["Industry_Group"];
                                myRow["Industry_SubGroup"] = dt_new.Rows[0]["Industry_SubGroup"];
                                myRow["Beta"] = dt_new.Rows[0]["Beta"];
                                myRow["ID_BB_COMPANY"] = dt_new.Rows[0]["ID_BB_COMPANY"];
                                myRow["ID_BB_UNIQUE"] = dt_new.Rows[0]["ID_BB_UNIQUE"];
                                myRow["isFuture"] = dt_new.Rows[0]["isFuture"];
                                // Force an FX Update
                                // Force an FX Update
                                // Force an FX Update
                                // Force an FX Update
                            }
                        }
                        //colin
                        dt_Port.Rows.Add(myRow);

                        if (AddToPortfolioTranspose)
                        {
                            DataRow myRowTrans = dt_PortfolioTranspose.NewRow();
                            myRowTrans["BBG_Ticker"] = DispTicker;
                            myRowTrans["FXRate"] = 0;
                            myRowTrans["Pos_Mult_Factor"] = 1;
                            dt_PortfolioTranspose.Rows.Add(myRowTrans);
                        }
                    }
                }
            }

            // Add the Ticker to the Last_Price watch
            foreach (String Ticker in TickerList)
            {
                String myTicker = SystemLibrary.BBGGetTickerNormalised(Ticker);
                DataRow[] FoundTickerRows = dt_Last_Price.Select("Ticker='" + myTicker + "'");
                if (FoundTickerRows.Length<1)
                {
                    DataRow dr = dt_Last_Price.NewRow();
                    dr["Ticker"] = myTicker;
                    dr["LAST_PRICE"] = 0;
                    dr["isNew"] = "N";
                    dt_Last_Price.Rows.Add(dr);
                }
            }

            // Request the Bloomberg Data
            foreach (String Ticker in TickerList)
            {
                if (Ticker.Trim().Length > 0) // Allows for having both CR/LF issues
                {
                    if (Ticker.Trim().Contains(" "))
                    {
                        DispTicker = SystemLibrary.BBGGetTickerNormalised(Ticker);
                        if (FromPortfolioTranspose)
                            this.BeginInvoke(new myStartRTDCallback(myStartRTD), DispTicker);
                        else
                        {
                            BRT.Bloomberg_Request(DispTicker);
                        }
                    }
                }
            }

        } // AddTickers

        private void dg_Port_KeyUp(object sender, KeyEventArgs e)
        {
            // Local Variables

            // Deal with Ctrl-V            
            if (e.Control == true && e.KeyValue == (int)Keys.V)
            {
                // Takes a Tab seperated or CR/LF seperated set of data
                IDataObject inClipboard = Clipboard.GetDataObject();

                AddTickers(inClipboard.GetData(DataFormats.Text).ToString());
            }

        } //dg_Port_KeyUp()

        private void dg_Port_DragEnter(object sender, DragEventArgs e)
        {
            // Set up for Drag Drop
            if (e.Data.GetDataPresent(DataFormats.FileDrop) ||
                e.Data.GetDataPresent(DataFormats.Text)
                )
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;

        } // dg_Port_DragEnter()

        private void dg_Port_DragDrop(object sender, DragEventArgs e)
        {
            // Local Variables
            String[] myFormats = e.Data.GetFormats();
            if (inDragMode)
            {
                // Dont want this data as is from a Drag "from" dg_Port
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                AddTickers(e.Data.GetData(DataFormats.Text).ToString());
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                String[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (fileNames.Length > 0)
                {
                    AddTickers(File.ReadAllText(fileNames[0]));
                }
            }

        } // dg_Port_DragDrop()

        private void dg_Port_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Seems the only way I can get the previous value before CellEndEdit().
            inEditMode = true;
            LastValue = dg_Port[e.ColumnIndex, e.RowIndex].Value;
            toolStripMenuItem1.Enabled = false;
            toolStripMenuItem1.Visible = false;
        } // dg_Port_CellBeginEdit()

        private void dg_Port_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Local Variables
            Decimal POS_Mult_Factor;
            int Round_Lot_Size;
            Decimal FXRate;
            Decimal myValue;
            int myQty;
            Decimal myFUM_incr;
            int ExistingOrder;
            Decimal myPrice;
            String myTicker;
            SystemLibrary.DebugLine("dg_Port_CellEndEdit() - START");

            myTicker = dg_Port["Ticker", e.RowIndex].Value.ToString();
            POS_Mult_Factor = SystemLibrary.ToDecimal(dg_Port["POS_Mult_Factor", e.RowIndex].Value);
            if (POS_Mult_Factor <= 0)
            {
                POS_Mult_Factor = 1;
                //dg_Port["POS_Mult_Factor", e.RowIndex].Value = 1;
            }

            Round_Lot_Size = Convert.ToInt32(SystemLibrary.ToDecimal(dg_Port["Round_Lot_Size", e.RowIndex].Value));
            if (Round_Lot_Size <= 0)
                Round_Lot_Size = 1;
            FXRate = SystemLibrary.ToDecimal(dg_Port["FXRate", e.RowIndex].Value);
            /*
            if (FXRate <= 0)
            {
                FXRate = 1;
                dg_Port["FXRate", e.RowIndex].Value = 1;
            }
            */

            // What column is changing
            switch (dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.ToString().ToUpper())
            {
                case @"% FUM":
                    // TODO (1) EXPOSURE - Needs to deal with Delta
                    myValue = SystemLibrary.ToDecimal(dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    myPrice = SystemLibrary.ToDecimal(dg_Port["Price", e.RowIndex].Value);
                    ExistingOrder = Convert.ToInt32(SystemLibrary.ToDecimal(dg_Port["Quantity", e.RowIndex].Value) + SystemLibrary.ToDecimal(dg_Port["Qty_Order", e.RowIndex].Value));
                    if (myPrice > 0 && FXRate > 0)
                        myQty = SendToBloomberg.RoundLot(ExistingOrder, Convert.ToInt32((myValue / FXRate / 100m * Fund_Amount) / myPrice / POS_Mult_Factor - ExistingOrder), Round_Lot_Size); //100m is %
                    else
                        myQty = 0;
                    if (Fund_Amount != 0)
                        myFUM_incr = myQty * myPrice * POS_Mult_Factor * FXRate / Fund_Amount;
                    else
                        myFUM_incr = 0;

                    if (myPrice <= 0)
                    {
                        MessageBox.Show("Sorry:  Price is not valid=" + myPrice.ToString(),
                                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else if (FXRate <= 0)
                    {
                        MessageBox.Show("Sorry:  FXRate is still to be updated or is not valid=" + FXRate.ToString(),
                                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else if (ExistingOrder != 0 && Math.Sign(ExistingOrder) != Math.Sign(ExistingOrder + myQty) && ExistingOrder != -myQty)
                    {
                        MessageBox.Show("Sorry: Cannot switch from 'Long to Short' or 'Short to Long' in one step\r\n" +
                                        "Please Close out Order to Zero and add a Second trade for the switch",
                                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else
                    {
                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myValue; // Deals with non-numeric
                        SetValueRG(dg_Port.Rows[e.RowIndex], "Quantity_incr", myQty);
                        SetValueRG(dg_Port.Rows[e.RowIndex], "FUM_incr", myFUM_incr);
                        // Set the Background color to show a change was driven by this column
                        TidyTRADEColumns(dg_Port.Rows[e.RowIndex]);
                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Goldenrod; //Color.Honeydew

                        // Now calculate column values
                        SetCalc(dg_Port["Ticker", e.RowIndex].Value.ToString(), Color.Empty);
                        SetGrossPCT();
                        SetHeader();
                    }
                    break;
                case @"FUM_INCR":
                    // TODO (1) EXPOSURE - Needs to deal with Delta
                    myValue = SystemLibrary.ToDecimal(dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    myPrice = SystemLibrary.ToDecimal(dg_Port["Price", e.RowIndex].Value);
                    ExistingOrder = Convert.ToInt32(SystemLibrary.ToDecimal(dg_Port["Quantity", e.RowIndex].Value) + SystemLibrary.ToDecimal(dg_Port["Qty_Order", e.RowIndex].Value));
                    if (myPrice > 0 && FXRate > 0)
                        myQty = SendToBloomberg.RoundLot(ExistingOrder, Convert.ToInt32((myValue / FXRate / 100m * Fund_Amount) / myPrice / POS_Mult_Factor), Round_Lot_Size); //100m is %
                    else
                        myQty = 0;
                    if (Fund_Amount != 0)
                        myFUM_incr = myQty * myPrice * POS_Mult_Factor * FXRate / Fund_Amount;
                    else
                        myFUM_incr = 0;

                    if (myPrice <= 0)
                    {
                        MessageBox.Show("Sorry:  Price is not valid=" + myPrice.ToString(),
                                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else if (FXRate <= 0)
                    {
                        MessageBox.Show("Sorry:  FXRate is still to be updated or is not valid=" + FXRate.ToString(),
                                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else if (ExistingOrder != 0 && Math.Sign(ExistingOrder) != Math.Sign(ExistingOrder + myQty) && ExistingOrder != -myQty)
                    {
                        MessageBox.Show("Sorry: Cannot switch from 'Long to Short' or 'Short to Long' in one step\r\n" +
                                        "Please Close out Order to Zero and add a Second trade for the switch",
                                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else
                    {
                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myValue; // Deals with non-numeric
                        SetValueRG(dg_Port.Rows[e.RowIndex], "Quantity_incr", myQty);
                        SetValueRG(dg_Port.Rows[e.RowIndex], "FUM_incr", myFUM_incr);
                        // Set the Background color to show a change was driven by this column
                        TidyTRADEColumns(dg_Port.Rows[e.RowIndex]);
                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Goldenrod; //Color.Honeydew

                        // Now calculate column values
                        SetCalc(dg_Port["Ticker", e.RowIndex].Value.ToString(), Color.Empty);
                        SetGrossPCT();
                        SetHeader();
                    }
                    break;
                case "VALUE":
                case "EXPOSURE":
                    // TODO (1) EXPOSURE - Needs to deal with Delta
                    myPrice = SystemLibrary.ToDecimal(dg_Port["Price", e.RowIndex].Value);
                    if (myPrice <= 0)
                    {
                        MessageBox.Show("Sorry:  Price is not valid=" + myPrice.ToString(),
                                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else if (FXRate <= 0)
                    {
                        MessageBox.Show("Sorry:  FXRate is still to be updated or is not valid=" + FXRate.ToString(),
                                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else if (SystemLibrary.ToString(dg_Port.Rows[e.RowIndex].Cells["Sector"].Value).ToUpper() == "Currency".ToUpper())
                    {
                        // Allow 2 decimal places.
                        //dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Format = "N2";
                        //dg_Port.Rows[e.RowIndex].Cells["Quantity_incr"].Style.Format = "N2";
                        Decimal myFXValue = Math.Round(SystemLibrary.ToDecimal(dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value), 2);
                        Decimal myFXQty = 0;

                        Decimal myFXExistingOrder = SystemLibrary.ToDecimal(dg_Port["Quantity", e.RowIndex].Value) + SystemLibrary.ToDecimal(dg_Port["Qty_Order", e.RowIndex].Value);
                        if (myPrice > 0 && FXRate > 0)
                            myFXQty = myFXValue / FXRate / myPrice / POS_Mult_Factor - myFXExistingOrder;
                        else
                            myFXQty = 0;
                        if (Fund_Amount != 0)
                            myFUM_incr = myFXQty * myPrice * POS_Mult_Factor * FXRate / Fund_Amount;
                        else
                            myFUM_incr = 0;

                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myFXValue; // Deals with non-numeric
                        SetValueRG(dg_Port.Rows[e.RowIndex], "Quantity_incr", myFXQty);
                        SetValueRG(dg_Port.Rows[e.RowIndex], "FUM_incr", myFUM_incr);
                        // Set the Background color to show a change was driven by this column
                        TidyTRADEColumns(dg_Port.Rows[e.RowIndex]);
                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Goldenrod;

                        // Now calculate column values
                        SetCalc(dg_Port["Ticker", e.RowIndex].Value.ToString(), Color.Empty);
                        SetGrossPCT();
                        SetHeader();                    
                    }
                    else
                    {
                        myValue = SystemLibrary.ToDecimal(dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        ExistingOrder = Convert.ToInt32(SystemLibrary.ToDecimal(dg_Port["Quantity", e.RowIndex].Value) + SystemLibrary.ToDecimal(dg_Port["Qty_Order", e.RowIndex].Value));
                        if (myPrice > 0 && FXRate > 0)
                            myQty = SendToBloomberg.RoundLot(ExistingOrder, Convert.ToInt32(myValue / FXRate / myPrice / POS_Mult_Factor - ExistingOrder), Round_Lot_Size);
                        else
                            myQty = 0;
                        if (Fund_Amount != 0)
                            myFUM_incr = myQty * myPrice * POS_Mult_Factor * FXRate / Fund_Amount;
                        else
                            myFUM_incr = 0;

                        if (ExistingOrder != 0 && Math.Sign(ExistingOrder) != Math.Sign(ExistingOrder + myQty) && ExistingOrder != -myQty)
                        {
                            MessageBox.Show("Sorry: Cannot switch from 'Long to Short' or 'Short to Long' in one step\r\n" +
                                            "Please Close out Order to Zero and add a Second trade for the switch",
                                            dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                            dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                        }
                        else
                        {
                            dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myValue; // Deals with non-numeric
                            SetValueRG(dg_Port.Rows[e.RowIndex], "Quantity_incr", myQty);
                            SetValueRG(dg_Port.Rows[e.RowIndex], "FUM_incr", myFUM_incr);
                            // Set the Background color to show a change was driven by this column
                            TidyTRADEColumns(dg_Port.Rows[e.RowIndex]);
                            dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Goldenrod;

                            // Now calculate column values
                            SetCalc(dg_Port["Ticker", e.RowIndex].Value.ToString(), Color.Empty);
                            SetGrossPCT();
                            SetHeader();
                        }
                    }
                    break;
                case "QUANTITY_INCR":
                    if (SystemLibrary.ToString(dg_Port.Rows[e.RowIndex].Cells["Sector"].Value).ToUpper() == "Currency".ToUpper())
                    {
                        // Allow 2 decimal places.
                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Format = "N2";
                        Decimal myFX = Math.Round(Convert.ToDecimal(dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value), 2);
                        myPrice = SystemLibrary.ToDecimal(dg_Port["Price", e.RowIndex].Value);
                        if (Fund_Amount != 0)
                            myFUM_incr = myFX * myPrice * POS_Mult_Factor * FXRate / Fund_Amount;
                        else
                            myFUM_incr = 0;

                        SetValueRG(dg_Port.Rows[e.RowIndex], "Quantity_incr", myFX);
                        SetValueRG(dg_Port.Rows[e.RowIndex], "FUM_incr", myFUM_incr);
                        // Set the Background color to show a change was driven by this column
                        TidyTRADEColumns(dg_Port.Rows[e.RowIndex]);
                        // Now calculate column values
                        SetCalc(dg_Port["Ticker", e.RowIndex].Value.ToString(), Color.Empty);
                        SetGrossPCT();
                        SetHeader();
                    }
                    else
                    {
                        // Cant switch short/Long or Long/Short - Make sure This Quantity_incr < - Quantity
                        myQty = Convert.ToInt32(SystemLibrary.ToDecimal(dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value));
                        ExistingOrder = Convert.ToInt32(SystemLibrary.ToDecimal(dg_Port["Quantity", e.RowIndex].Value) + SystemLibrary.ToDecimal(dg_Port["Qty_Order", e.RowIndex].Value));
                        if (ExistingOrder != 0 && Math.Sign(ExistingOrder) != Math.Sign(SystemLibrary.ToDecimal(ExistingOrder) + myQty) && (SystemLibrary.ToDecimal(ExistingOrder) + myQty) != 0)
                        {
                            MessageBox.Show("Can't switch from 'Long to Short' or 'Short to Long' in one step",
                                            dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                            dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                        }
                        else
                        {
                            myQty = SendToBloomberg.RoundLot(ExistingOrder, myQty, Round_Lot_Size);
                            myPrice = SystemLibrary.ToDecimal(dg_Port["Price", e.RowIndex].Value);
                            if (Fund_Amount != 0)
                                myFUM_incr = myQty * myPrice * POS_Mult_Factor * FXRate / Fund_Amount;
                            else
                                myFUM_incr = 0;

                            SetValueRG(dg_Port.Rows[e.RowIndex], "Quantity_incr", myQty);
                            SetValueRG(dg_Port.Rows[e.RowIndex], "FUM_incr", myFUM_incr);
                            // Set the Background color to show a change was driven by this column
                            TidyTRADEColumns(dg_Port.Rows[e.RowIndex]);
                            // Now calculate column values
                            SetCalc(dg_Port["Ticker", e.RowIndex].Value.ToString(), Color.Empty);
                            SetGrossPCT();
                            SetHeader();
                        }
                    }
                    break;
                case "TICKER": // MUST BE Uppercase
                    // If this is an existing record, then set back to original value
                    if (dg_Port["Quantity", e.RowIndex].Value == DBNull.Value)
                    {
                        // First time
                        dg_Port["Quantity", e.RowIndex].Value = 0;
                        dg_Port["Qty_Order", e.RowIndex].Value = 0;
                        dg_Port["Qty_Routed", e.RowIndex].Value = 0;
                        dg_Port["Qty_Fill", e.RowIndex].Value = 0;
                        dg_Port["IsAggregate", e.RowIndex].Value = "N";
                        dg_Port["FXRate", e.RowIndex].Value = 0;
                        dg_Port["FX_Pos_Mult_Factor", e.RowIndex].Value = 1;
                        dg_Port["FundId", e.RowIndex].Value = FundID;
                        dg_Port["PortfolioID", e.RowIndex].Value = PortfolioID;
                    }
                    if (Math.Abs(SystemLibrary.ToDouble(dg_Port["Quantity", e.RowIndex].Value)) > 0 ||
                       Math.Abs(SystemLibrary.ToDouble(dg_Port["Qty_Order", e.RowIndex].Value)) > 0)
                    {
                        dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    //dg_Port.CurrentRow.Cells["FXRate"].Value = 1;
                    // Make sure Ticker is represented in Bloomberg fashion
                    dg_Port.CurrentCell.Value = SystemLibrary.BBGGetTicker(dg_Port.CurrentCell.Value.ToString());
                    // Force the data to the DataTable by Changing Focus. (Can this be done smarter?)   
                    lb_Fund.Focus();
                    dg_Port.Focus();

                    String NewTicker = dg_Port.CurrentCell.Value.ToString();

                    // For speed
                    // - See if we already have this ticker in live data
                    DataRow[] dr = dt_Port.Select("Ticker='"+NewTicker+"'");
                    if (dr.Length>1)
                    {
                        dg_Port["Price", e.RowIndex].Value = dr[0]["Price"];
                        dg_Port["Security_Name", e.RowIndex].Value = dr[0]["Security_Name"];
                        dg_Port["crncy", e.RowIndex].Value = dr[0]["crncy"];
                        dg_Port["prev_Close", e.RowIndex].Value = dr[0]["prev_Close"];
                        dg_Port["Div_Adjusted", e.RowIndex].Value = dr[0]["Div_Adjusted"];
                        dg_Port["Pos_Mult_Factor", e.RowIndex].Value = dr[0]["Pos_Mult_Factor"];
                        dg_Port["Round_Lot_Size", e.RowIndex].Value = dr[0]["Round_Lot_Size"];
                        dg_Port["FXRate", e.RowIndex].Value = dr[0]["FXRate"];
                        dg_Port["FX_Pos_Mult_Factor", e.RowIndex].Value = dr[0]["FX_Pos_Mult_Factor"];
                        dg_Port["Undl_Ticker", e.RowIndex].Value = dr[0]["Undl_Ticker"];
                        dg_Port["Undl_Currency", e.RowIndex].Value = dr[0]["Undl_Currency"];
                        dg_Port["Undl_Price", e.RowIndex].Value = dr[0]["Undl_Price"];
                        dg_Port["Delta", e.RowIndex].Value = dr[0]["Delta"];
                        dg_Port["Country_Full_Name", e.RowIndex].Value = dr[0]["Country_Full_Name"];
                        dg_Port["Sector", e.RowIndex].Value = dr[0]["Sector"];
                        dg_Port["Industry_Group", e.RowIndex].Value = dr[0]["Industry_Group"];
                        dg_Port["Industry_SubGroup", e.RowIndex].Value = dr[0]["Industry_SubGroup"];
                        dg_Port["Beta", e.RowIndex].Value = dr[0]["Beta"];
                        dg_Port["ID_BB_COMPANY", e.RowIndex].Value = dr[0]["ID_BB_COMPANY"];
                        dg_Port["ID_BB_UNIQUE", e.RowIndex].Value = dr[0]["ID_BB_UNIQUE"];
                        dg_Port["ID_BB_GLOBAL", e.RowIndex].Value = dr[0]["ID_BB_GLOBAL"];
                        dg_Port["CUSIP", e.RowIndex].Value = dr[0]["CUSIP"];
                        dg_Port["ISIN", e.RowIndex].Value = dr[0]["ISIN"];
                        dg_Port["SEDOL", e.RowIndex].Value = dr[0]["SEDOL"];
                        dg_Port["isFuture", e.RowIndex].Value = dr[0]["isFuture"];
                    }
                    else
                    {
                        // Go to the database and get as much as can
                        String mySql = "Select BBG_Ticker as Ticker, Security_Name, crncy, Pos_Mult_Factor, Round_Lot_Size, Undl_Ticker, Undl_Currency, Delta, " +
                                       "       Country_Full_Name, Sector, Industry_Group, Industry_SubGroup, Beta, ID_BB_COMPANY, ID_BB_UNIQUE, " +
                                       "        Case When isNull(Security_Typ2,'') = 'Future' Then 'Y' Else 'N' End as isFuture " +
                                       "From Securities " +
                                       "Where BBG_Ticker = '" + NewTicker + "' ";
                        DataTable dt_new = SystemLibrary.SQLSelectToDataTable(mySql);

                        if (dt_new.Rows.Count == 1)
                        {
                            dg_Port["Security_Name", e.RowIndex].Value = dt_new.Rows[0]["Security_Name"];
                            dg_Port["crncy", e.RowIndex].Value = dt_new.Rows[0]["crncy"];
                            dg_Port["Pos_Mult_Factor", e.RowIndex].Value = dt_new.Rows[0]["Pos_Mult_Factor"];
                            dg_Port["Round_Lot_Size", e.RowIndex].Value = dt_new.Rows[0]["Round_Lot_Size"];
                            dg_Port["Undl_Ticker", e.RowIndex].Value = dt_new.Rows[0]["Undl_Ticker"];
                            dg_Port["Undl_Currency", e.RowIndex].Value = dt_new.Rows[0]["Undl_Currency"];
                            dg_Port["Delta", e.RowIndex].Value = dt_new.Rows[0]["Delta"];
                            dg_Port["Country_Full_Name", e.RowIndex].Value = dt_new.Rows[0]["Country_Full_Name"];
                            dg_Port["Sector", e.RowIndex].Value = dt_new.Rows[0]["Sector"];
                            dg_Port["Industry_Group", e.RowIndex].Value = dt_new.Rows[0]["Industry_Group"];
                            dg_Port["Industry_SubGroup", e.RowIndex].Value = dt_new.Rows[0]["Industry_SubGroup"];
                            dg_Port["Beta", e.RowIndex].Value = dt_new.Rows[0]["Beta"];
                            dg_Port["ID_BB_COMPANY", e.RowIndex].Value = dt_new.Rows[0]["ID_BB_COMPANY"];
                            dg_Port["ID_BB_UNIQUE", e.RowIndex].Value = dt_new.Rows[0]["ID_BB_UNIQUE"];
                            dg_Port["isFuture", e.RowIndex].Value = dt_new.Rows[0]["isFuture"];
                            // Force an FX Update
                            // Force an FX Update
                            // Force an FX Update
                            // Force an FX Update
                        }
                    }
                    // Get Bloomberg to update data
                    inEditMode = false;
                    this.BeginInvoke(new myStartRTDCallback(myStartRTD), NewTicker);
                    break;
                default:
                    // Dont allow column change
                    dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    break;
            }
            // Set the LS column where needed
            if (Convert.ToInt32(SystemLibrary.ToDecimal(dg_Port["Quantity", e.RowIndex].Value))==0)
            {
                if (Convert.ToInt32(SystemLibrary.ToDecimal(dg_Port["Quantity_incr", e.RowIndex].Value)) >= 0)
                {
                    dg_Port["LS", e.RowIndex].Value = "L";
                    dg_Port["LS", e.RowIndex].Style.ForeColor = Color.Green;
                }
                else
                {
                    dg_Port["LS", e.RowIndex].Value = "S";
                    dg_Port["LS", e.RowIndex].Style.ForeColor = Color.Red;
                }
            }
            LastValue = null;
            toolStripMenuItem1.Enabled = true;
            toolStripMenuItem1.Visible = true;
            inEditMode = false;
            SystemLibrary.DebugLine("dg_Port_CellEndEdit() - END");
        } //dg_Port_CellEndEdit()


        public delegate void myStartRTDCallback(String Ticker);
        public void myStartRTD(String Ticker)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                SystemLibrary.DebugLine("myStartRTD(" + Ticker + ") - InvokeRequired");
                myStartRTDCallback cb = new myStartRTDCallback(myStartRTD);
                this.Invoke(cb, new object[] { Ticker });
            }
            else
            {
                SystemLibrary.DebugLine("myStartRTD(" + Ticker + ")");
                BRT.Bloomberg_Request(Ticker);
            }
        } //myStartRTD()

        public delegate void CheckTickerExistsCallback(String Ticker);
        public void CheckTickerExists(String Ticker)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                CheckTickerExistsCallback cb = new CheckTickerExistsCallback(CheckTickerExists);
                this.Invoke(cb, new object[] { Ticker });
            }
            else
            {
                // See if it an active ticker
                DataRow[] dr = dt_Securities.Select("BBG_Ticker='" + Ticker + "'");
                if (dr.Length < 1)
                {
                    // See if it exists in the database
                    if (SystemLibrary.SQLSelectRowsCount("Select BBG_Ticker from Securities Where BBG_Ticker = '" + Ticker + "' ") < 1)
                    {
                        // Request the new ticker
                        SystemLibrary.DebugLine("CheckTickerExists - Added(" + Ticker + ")");
                        //SystemLibrary.SetDebugLevel(4);
                        AddTickers(Ticker);
                        //BRT.Bloomberg_Request(Ticker);
                    }
                }
            }
        } //CheckTickerExists()


        private void dg_Port_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Allow Function Keys to work on Bloomberg Tickers

            // Local Variables
            DataGridView dg = (DataGridView)sender;
            if (dg.CurrentCell.OwningColumn.Name.ToUpper().Contains("TICKER"))
            {
                // Allow for the capture of Bloomberg Function Keys while in Cell Edit mode.
                e.Control.PreviewKeyDown -= new PreviewKeyDownEventHandler(SystemLibrary.BBGOnPreviewKeyDown);
                e.Control.PreviewKeyDown += new PreviewKeyDownEventHandler(SystemLibrary.BBGOnPreviewKeyDown);
            }
            else
            {
                e.Control.PreviewKeyDown -= new PreviewKeyDownEventHandler(SystemLibrary.BBGOnPreviewKeyDown);
            }
        } //dg_Port_EditingControlShowing()

        private void TidyTRADEColumns()
        {
            foreach (DataGridViewRow dgr in dg_Port.Rows)
                TidyTRADEColumns(dgr);
        }

        private void TidyTRADEColumns(DataGridViewRow dgr)
        {
            dgr.Cells["Value"].Style.BackColor = Color.Empty;
            dgr.Cells["Exposure"].Style.BackColor = Color.Empty;
            dgr.Cells["% FUM"].Style.BackColor = Color.Empty;
            dgr.Cells["FUM_incr"].Style.BackColor = Color.Empty;
        }

        private void dg_Port_Sorted(object sender, EventArgs e)
        {
            // When a Sort occurs, the Datagrid looses colour formating, so reset
            try
            {
                this.SuspendLayout();
                //ResetHiddenRows();
                SetFormat();
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - pre SetCalc()");
                //SetCalc();
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - post SetCalc()");
                /*
                if (tabControl_Port.SelectedTab.Text == "TRADE")
                {
                    // Hide Rows That are for FX balance
                    UnHideRows();
                    HideFXbalanceRows();
                }
                */
                /* 
                 * RitchViewer Pty Ltd 3-Feb-2014
                 * - Already in ResetHiddenRows
                SetFormat();
                SetCalc();
                 */
            }
            catch { }
            this.ResumeLayout(true);
        } // dg_Port_Sorted


        #region Right-Click menu

        private void dg_Port_MouseClick(object sender, MouseEventArgs e)
        {
            CXLocation = dg_Port.Location.X + e.Location.X + 5;
            CYLocation = dg_Port.Location.Y + e.Location.Y;
            
        } //dg_Port_MouseClick()

        private void dg_Port_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Show the Bloomberg popup menu
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    String Ticker = SystemLibrary.ToString(dg_Port.Rows[e.RowIndex].Cells["Ticker"].Value);
                    if (Ticker.Length > 0)
                    {
                        switch (dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.ToString())
                        {
                            case "Ticker":
                                SystemLibrary.BBGShowMenu(FundID, PortfolioID, Ticker, Portfolio_Name, SystemLibrary.BBGRelativeTicker(Ticker), this.Location.X + CXLocation, this.Location.Y + CYLocation);
                                break;
                            case "Price":
                                Decimal Price = SystemLibrary.ToDecimal(dg_Port.Rows[e.RowIndex].Cells["Price"].Value);
                                String myValue = Price.ToString();
                                if (SystemLibrary.InputBox("WhatIf Price for " + Ticker + " from " + dg_Port.Rows[e.RowIndex].Cells["Price"].FormattedValue.ToString(), "Temporarily Change the Price OR Cancel", ref myValue, this.validate_WhatIfPrice, MessageBoxIcon.Question) == DialogResult.OK)
                                {
                                    dg_Port.Rows[e.RowIndex].Cells["Price"].Value = SystemLibrary.ToDecimal(myValue);
                                    SetCalc(Ticker, Color.DarkBlue);
                                }
                                break;
                            case "PL_Day":
                            case "PL_Exec":
                                // Display the Unrealised & realised profit
                                Decimal PL_SOLD_Today = -SystemLibrary.ToDecimal(dg_Port.Rows[e.RowIndex].Cells["Qty_Fill"].Value) *
                                                        (SystemLibrary.ToDecimal(dg_Port.Rows[e.RowIndex].Cells["Avg_Price"].Value) -
                                                         SystemLibrary.ToDecimal(dg_Port.Rows[e.RowIndex].Cells["prev_Close"].Value));
                                Decimal Today_Profit = SystemLibrary.ToDecimal(dg_Port.Rows[e.RowIndex].Cells["PL_Day"].Value);
                                Decimal Today_EXEC_Profit = SystemLibrary.ToDecimal(dg_Port.Rows[e.RowIndex].Cells["PL_Exec"].Value);

                                /*
                                PL_SOLD_Today = (Decimal)(1500000 * (0.065133 - 0.059));
                                Today_Profit = (Decimal)30303.10;
                                Today_EXEC_Profit = (Decimal)(-5800.50);
                                */

                                MessageBox.Show(Ticker + "\r\n\r\n" +
                                                "Total Profit for the day  = " + (Today_Profit + Today_EXEC_Profit).ToString("$#,###") + "\r\n" +
                                                "= PL Day     " + Today_Profit.ToString("$#,###") + " plus \r\n" +
                                                "    PL Exec    " + Today_EXEC_Profit.ToString("$#,###") + "\r\n\r\n" +
                                                "Cash Profit           = " + PL_SOLD_Today.ToString("$#,###") + "\r\n" +
                                                "Unrealised Profit = " + ((Today_Profit + Today_EXEC_Profit) - PL_SOLD_Today).ToString("$#,###"),
                                                "Realised & Unrealised Profit ");
                                break;
                            case "PL_TradePeriod":
                            case "PL_WRoll":
                            case "PL_MRoll":
                            case "PL_MTD":
                            case "PL_Inception":
                            case "PL_YTD":
                            case "PL_YTD_July":
                                // Display the Unrealised & realised profit
                                /*
                                 * NB: This doesnt work for a multi-row situation yet (ie. L/S)
                                 */

                                // Get the realised profit prior to today
                                String mySql = "Select  Sum(profit) " +
                                               "From    StockLocation " +
                                               "Where   TradeID in (Select TradeID " +
                                               "                    From    Trade " +
                                               "                    Where   BBG_Ticker = '" + Ticker + "' " +
                                               "                    And     FundID = " + FundID.ToString() + " " +
                                               "                    And     (PortfolioID = " + PortfolioID.ToString() + " Or " + PortfolioID.ToString() + " = -1 ) " +
                                               "                    And     IsNull(PM,'') = '" + SystemLibrary.ToString(dg_Port.Rows[e.RowIndex].Cells["PM"].Value) + "' " +
                                               "                    And     IsNull(IdeaOwner,'') = '" + SystemLibrary.ToString(dg_Port.Rows[e.RowIndex].Cells["IdeaOwner"].Value) + "' " +
                                               "                    And     IsNull(Strategy1,'') = '" + SystemLibrary.ToString(dg_Port.Rows[e.RowIndex].Cells["Strategy1"].Value) + "' " +
                                               "                    And     IsNull(Strategy2,'') = '" + SystemLibrary.ToString(dg_Port.Rows[e.RowIndex].Cells["Strategy2"].Value) + "' " +
                                               "                    And     IsNull(Strategy3,'') = '" + SystemLibrary.ToString(dg_Port.Rows[e.RowIndex].Cells["Strategy3"].Value) + "' " +
                                               "                    And     IsNull(Strategy4,'') = '" + SystemLibrary.ToString(dg_Port.Rows[e.RowIndex].Cells["Strategy4"].Value) + "' " +
                                               "                    ) " +
                                               "And	TradeDate < dbo.f_Today() " +
                                               "And	Match_TradeDate < dbo.f_Today() ";
                                //             "                    And     And	IsNull(Strategy5,'') = '" + SystemLibrary.ToString(dg_Port.Rows[e.RowIndex].Cells["Strategy5"].Value) + "' " +

                                Decimal PL_Realised = SystemLibrary.SQLSelectDecimal(mySql);
                                Decimal PL_SOLD = -SystemLibrary.ToDecimal(dg_Port.Rows[e.RowIndex].Cells["Qty_Fill"].Value) *
                                                  (SystemLibrary.ToDecimal(dg_Port.Rows[e.RowIndex].Cells["Avg_Price"].Value) -
                                                   SystemLibrary.ToDecimal(dg_Port.Rows[e.RowIndex].Cells["prev_Close"].Value));
                                Decimal PL_Unrealised = SystemLibrary.ToDecimal(dg_Port.Rows[e.RowIndex].Cells["PL_Inception"].Value) -
                                                        (PL_Realised + PL_SOLD);
                                Decimal This_Profit = SystemLibrary.ToDecimal(dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                                String This_Header = dg_Port.Columns[e.ColumnIndex].HeaderText.ToString();
                                MessageBox.Show(Ticker + "\r\n\r\n" +
                                                This_Header + "  = " + This_Profit.ToString("$#,###") + "\r\n\r\n" +
                                                "Realised profit     = " + (This_Profit - PL_Unrealised).ToString("$#,###") + "\r\n" +
                                                "Unrealised profit = " + (PL_Unrealised).ToString("$#,###"),
                                                "Realised & Unrealised Profit ");
                                break;
                        }
                    }
                }
            }
            catch { }

        } // dg_Port_CellMouseClick()

        SystemLibrary.InputBoxValidation validate_WhatIfPrice = delegate(String myValue)
        {
            // Rules: Must be > 0
            Decimal myPrice = SystemLibrary.ToDecimal(myValue);
            if (myPrice>0)
                return "";
            else
                return "'" + myValue + "' cannot be less than or equal to zero.";

        }; //validate_WhatIfPrice()

        #endregion //Right-Click menu



        private void button4_Click(object sender, EventArgs e)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - Start: SetCalc()");
            SetCalc();
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - End: SetCalc()");
            return;
            String Checks = "Can T1 access directories:\r\n";

            //MessageBox.Show("This will take 30 seconds. Press OK to continue", "FTP Check");
            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            SystemLibrary.SetDebugLevel(4);
            String[] myFiles = SystemLibrary.FTPGetFileList(SystemLibrary.FTPSCOTIAPrimeVars, @"/FPOBELYS");
            String FTPFileNames = "Scotia Files\r\n";

            if (myFiles != null)
            {
                SystemLibrary.DebugLine("myFiles: " + myFiles.Length.ToString());
                // This block of code is so that I only go to the database once against the SCOTIA_File_FTPList table
                foreach (String myFileName in myFiles)
                {
                    FTPFileNames = FTPFileNames + "'" + myFileName + "'\r\n";
                }
            }

            String myPath = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('SCOTIAPrime_Path')");
            if (Directory.Exists(myPath))
            {
                Checks = Checks + "Found '" + myPath + "'";
                if (Directory.Exists(myPath + @"\Archive"))
                {
                    Checks = Checks + "\r\nFound '" + myPath + @"\Archive" + "'";
                }
                if (Directory.Exists(myPath + @"\OutBound"))
                {
                    Checks = Checks + "\r\nFound '" + myPath + @"\OutBound" + "'";
                }
            }

            SystemLibrary.SetDebugLevel(0);
            Cursor.Current = Cursors.Default;
            Clipboard.SetText(Checks + "\r\n\r\n" + FTPFileNames);
            MessageBox.Show(Checks + "\r\n\r\n" + FTPFileNames, "FTP TEST: Please Paste from clipboard to Skype.");



            return;

            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            int screenWidth = SystemInformation.VirtualScreen.Width;
            int screenHeight = SystemInformation.VirtualScreen.Height;

            this.Size = new System.Drawing.Size(screenWidth, screenHeight);
            this.Location = new System.Drawing.Point(screenLeft, screenTop);

            //SavePrices();
            return;

            /*
            String myTimeZone = "";
            try
            {
                myTimeZone = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('TimeZone')");
                int myOffsetSeconds = SystemLibrary.ToInt32(TimeZoneInfo.FindSystemTimeZoneById(myTimeZone).BaseUtcOffset.TotalSeconds);

                SystemLibrary.SQLExecute("Update System_Parameters Set p_number = " + SystemLibrary.ToString(myOffsetSeconds) + " Where Parameter_Name = 'TimeZone'");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to Set the 'TimeZone' offset for '" + myTimeZone + "'\r\n\r\n"+ex.Message,"Please report error to you administrator");
            }
          

            //SystemLibrary.SQLExecute("Update System_Parameters Set p_number = "+

            ReadOnlyCollection<TimeZoneInfo> tzCollection;
            tzCollection = TimeZoneInfo.GetSystemTimeZones();
            foreach (TimeZoneInfo timeZone in tzCollection)
            {
                Console.WriteLine("{0}\t{1}", timeZone.Id, timeZone.DisplayName);
                if (timeZone.Id.Contains("AUS Eastern Standard Time"))
                    Console.WriteLine("   {0}: {1}", timeZone.Id, timeZone.DisplayName);
                if (timeZone.Id == "India Standard Time")
                    Console.WriteLine("   {0}: {1}", timeZone.Id, timeZone.DisplayName);
            }
            return;

            DateTime retVal = DateTime.Now;
            retVal = DateTime.SpecifyKind(retVal, DateTimeKind.Unspecified);

            TimeZoneInfo.ConvertTimeBySystemTimeZoneId(retVal, "Eastern Standard Time", TimeZoneInfo.Local.Id);

            return;

            // hourglass cursor
            //Cursor.Current = Cursors.WaitCursor;
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - " + "button4_Click: Start()");
            SetFormat();
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - " + "button4_Click: Post SetFormat()");
            SetCalc();
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - " + "button4_Click: Post SetCalc()");

            return;
            
            */

        } //button4_Click()


        #region Drop Down Boxes

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open an instance the connect form so the user
            // can define a new connection
            AboutBox1 f = new AboutBox1();
            SystemLibrary.FormExists(f, true);
            f.FromParent(this, ProcessDateTime);
            f.Show(); //(this);

        } //aboutToolStripMenuItem_Click()

        private void maintainCommissiontoolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open an instance the connect form so the user
            // can define a new connection
            Commission f = new Commission();
            f = (Commission)SystemLibrary.FormExists(f, false);
            f.BringToFront();
            f.FromParent(this);
            f.Show(); //(this);

        } //maintainCommissiontoolStripMenuItem_Click()

        private void databaseConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open an instance the connect form so the user
            // can define a new connection
            frm_DBConnect f = new frm_DBConnect();
            f.OnMessage += new frm_DBConnect.Message(SystemLibrary.SQLSaveConnectParams);
            f.Show(); //(this);
        } // databaseConnectionToolStripMenuItem_Click()

        private void bloombergFTPParamsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open an instance the connect form so the user
            // can define a new connection
            FTP_Parameters f = new FTP_Parameters();
            //f.OnMessage += new FTP_Parameters.Message(SystemLibrary.SQLSaveConnectParams);
            f.Startup(this);
            f.Show(); //(this);

        } //bloombergFTPParamsToolStripMenuItem_Click()

        private void SMTPParamsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open an instance the connect form so the user
            // can define a new connection
            SMTPSetup f = new SMTPSetup();
            f.Startup(this);
            f.Show(); //(this);

        } //SMTPParamsToolStripMenuItem_Click()


        private void MLPrimeFTPParamstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open an instance the connect form so the user
            // can define a new connection
            FTP_MLPrime f = new FTP_MLPrime();
            //f.OnMessage += new FTP_Parameters.Message(SystemLibrary.SQLSaveConnectParams);
            f.Startup(this);
            f.Show(); //(this);

        } //MLPrimeFTPParamstoolStripMenuItem_Click()

        private void ScotiaPrimeFTPParamstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open an instance the connect form so the user
            // can define a new connection
            FTP_ScotiaPrime f = new FTP_ScotiaPrime();
            //f.OnMessage += new FTP_Parameters.Message(SystemLibrary.SQLSaveConnectParams);
            f.Startup(this);
            f.Show(); //(this);

        } //ScotiaPrimeFTPParamstoolStripMenuItem_Click()

        private void eMSXFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Local Variables
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "Bloomberg EMSI File|*.csv";
            openFileDialog1.Title = "Select the Bloomberg EMSI File to be processed";

            // Show the Dialog.
            // If the user clicked OK in the dialog and
            // a .CUR file was selected, open it.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String myFilePath = Path.GetDirectoryName(openFileDialog1.FileName);
                String myFileName = Path.GetFileName(openFileDialog1.FileName);
                //openFileDialog1.Multiselect 
                if (SystemLibrary.EMSXSaveData(myFilePath, myFileName))
                    MessageBox.Show(myFileName + " processed sucessfully", "Bloomberg EMSI File process");
                else
                    MessageBox.Show(myFileName + " failed to processs.", "Bloomberg EMSI File process");
            }
        } //eMSXFilesToolStripMenuItem_Click()

        private void isBloombergUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isBloombergUserSave();
            isBloombergUser = isBloombergUserToolStripMenuItem.Checked;

        } //isBloombergUserToolStripMenuItem_Click()

        private void isBloombergUser1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isBloombergUser1Save();
            isBloombergUser1 = isBloombergUser1ToolStripMenuItem.Checked;

        } //isBloombergUser1ToolStripMenuItem_Click()

        private void importDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportData f = new ImportData();
            f.FromParent(this);
            f.Show();
        } //importDataToolStripMenuItem_Click()

        private void designPortfolioTabsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open an instance the connect form so the user
            // can define a new connection
            PortTabLayout f = new PortTabLayout();
            f.OnMessage += new PortTabLayout.Message(ReLoadTabs);
            f.Show(); //(this);

        } //designPortfolioTabsToolStripMenuItem_Click()

        private void tradeReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Local Variables
            DateTime FromDate = SystemLibrary.f_Now().AddDays(-7);

            ReportTrade rt = new ReportTrade();
            rt.FromParent(this, FundID, PortfolioID, "", FromDate, null, -1);
            rt.Show();

        } //tradeReportToolStripMenuItem_Click()

        private void borrowReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportBorrow rt = new ReportBorrow();
            rt.FromParent(this);
            rt.Show();

        } //borrowReportToolStripMenuItem_Click()

        private void mLPrimeBalancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MLPrime_Balance rt = new MLPrime_Balance();
            rt.FromParent(this, FundID);
            rt.Show();

        } //mLPrimeBalancesToolStripMenuItem_Click()

        private void mLFuturesBalancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MLFutures_Balance rt = new MLFutures_Balance();
            rt.FromParent(this, FundID);
            rt.Show();

        } //mLFuturesBalancesToolStripMenuItem_Click()

        private void FuturesExplainWireTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FuturesExplainWireTransfer rt = new FuturesExplainWireTransfer();
            rt.FromParent(this, FundID, DateTime.MinValue);
            rt.Show();

        } //FuturesExplainWireTransferToolStripMenuItem_Click()

        private void alterMarginRatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FutureMargins frm_FutMargin = new FutureMargins();
            frm_FutMargin.FromParent(this, true);
            frm_FutMargin.Show();
        } //alterMarginRatesToolStripMenuItem_Click()

        private void mLPrimeTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MLPrime_Reconcile rt = new MLPrime_Reconcile();
            rt.FromParent(this);
            rt.Show();

        } //mLPrimeTransactionsToolStripMenuItem_Click()

        private void mLFuturesTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MLFuture_Reconcile rt = new MLFuture_Reconcile();
            rt.FromParent(this);
            rt.Show();

        } //mLFuturesTransactionsToolStripMenuItem_Click()

        private void fillsWithoutOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FillWithoutOrder rt = new FillWithoutOrder();
            rt.FromParent(this);
            rt.Show();

        } //fillsWithoutOrdersToolStripMenuItem_Click()

        private void setDebugLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetDebugLevel rt = new SetDebugLevel();
            rt.Show();

        } //setDebugLevelToolStripMenuItem_Click()

        private void defineInterestAccualsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DefineInterestAccrual rt = new DefineInterestAccrual();
            rt.FromParent(this, FundID, Fund_Crncy);
            rt.Show();

        } //defineInterestAccualsToolStripMenuItem_Click()


        private void chartOfAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChartOfAccounts rt = new ChartOfAccounts();
            rt.FromParent(this, FundID);
            rt.Show();

        } //chartOfAccountsToolStripMenuItem_Click()

        private void EMSXMultiMappingStripMenuItem_Click(object sender, EventArgs e)
        {
            EMSX_MULTI_Override rt = new EMSX_MULTI_Override();
            rt.Show();

        } //EMSXMultiMappingStripMenuItem_Click()

        private void reportAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportAccounts rt = new ReportAccounts();
            rt.FromParent(this, FundID);
            rt.Show();

        } //reportAccountsToolStripMenuItem_Click()

        private void newJournalEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessJournals rt = new ProcessJournals();
            rt.FromParent(this);
            rt.Show();

        } //newJournalEntryToolStripMenuItem_Click()

        private void NAVReconciliationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NAVReconcileReport rt = new NAVReconcileReport();
            rt.FromParent(this);
            rt.Show();

        } //NAVReconciliationToolStripMenuItem_Click()



        public delegate void CopyDataTableCallback(ref DataTable inFund, ref DataTable inPortfolio);
        public void CopyDataTable(ref DataTable inFund, ref DataTable inPortfolio)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                CopyDataTableCallback cb = new CopyDataTableCallback(CopyDataTable);
                this.Invoke(cb, new object[] { inFund, inPortfolio });
            }
            else
            {
                inFund = dt_Fund.Copy();
                inPortfolio = dt_Portfolio.Copy();
            }

        } //CopyDataTable()

        private void LoadTabs()
        {
            // Procedure: LoadTabs
            //
            // Purpose: Load the Tabs that will Drive dg_Port
            //

            // Local Variables
            String mySql;

            try
            {
                mySql = "Select TabName, IsSystem, IsTabVisible, FrozenColName, TabOrder, RowFilter " +
                        "From   dg_Port_Tabs " +
                        "Where  TabName not in ('TRADE','ACTION','CLOSED') " + // Have hardcoded this for now.
                        "Order By TabOrder ";
                dt_Port_Tabs = SystemLibrary.SQLSelectToDataTable(mySql);
            }
            catch { }

            if (dt_Port_Tabs.Rows.Count > 0)
            {
                // Delete the old tabs
                if (tabControl_Port.TabPages.Count > 0)
                    for (int i = tabControl_Port.TabPages.Count - 1; i >= 0; i--)
                        if (tabControl_Port.TabPages[i].Tag == null)
                            tabControl_Port.TabPages[i].Dispose();
                        else
                            if (tabControl_Port.TabPages[i].Tag.ToString() != "SYSTEM")
                                tabControl_Port.TabPages[i].Dispose();

                for (int i = 0; i < dt_Port_Tabs.Rows.Count; i++)
                    tabControl_Port.TabPages.Add(dt_Port_Tabs.Rows[i]["TabName"].ToString());
            }


            try
            {
                mySql = "Select TabName, ColName, ColOrder  " +
                        "From   dg_Port_Tab_Detail " +
                        "Order By TabName, ColOrder ";
                dt_Port_Tab_Detail = SystemLibrary.SQLSelectToDataTable(mySql);
            }
            catch { }

        } //LoadTabs()

        private void LoadFund(int inPortfolioID)
        {
            // Procedure: LoadFund
            //
            // Purpose: Load the Fund DDLB & Portfolio DDLB
            //
            // Rules: To access Fund crncy, use syntax below (NB: ItemArray[0] = 1st column)
            //          Fund_Crncy = ((System.Data.DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[3].ToString();

            // Local Variables
            String mySql = "";
            String CurrentSelection = "<All>";
            int myIndex;

            if (dt_Fund != null)
            {
                CurrentSelection = cb_Fund.Text.ToString();
            }
            // Load the Fund ddlb
            try
            {
                if (inPortfolioID == -1)
                {
                    // Need FundAmount as Currency in Top half of SQL
                    mySql = "Select -1 as FundId, '<All>' as FundName, Sum(FundAmount) as FundAmount, 'AUD' as crncy, '<All>' as ShortName " +
                            "From   Fund " +
                            "Where  Active = 'Y' " +
                            "And   AllowTrade = 'Y' " +
                            "Union " +
                            "Select FundId, FundName, FundAmount, crncy, ShortName " +
                            "From   Fund " +
                            "Where  Active = 'Y' " +
                            "And   AllowTrade = 'Y' " +
                            "Order By 2 ";
                }
                else
                {
                    mySql = "Select -1 as FundId, '<All>' as FundName, Sum(FundAmount) as FundAmount, 'AUD' as crncy, '<All>' as ShortName " +
                            "From   Fund " +
                            "Where  Active = 'Y' " +
                            "And   AllowTrade = 'Y' " +
                            "Union " +
                            "Select FundId, FundName, FundAmount, crncy, ShortName " +
                            "From   Fund " +
                            "Where  Active = 'Y' " +
                            "And   AllowTrade = 'Y' " +
                            "And    FundID in ( Select  FundID " +
                            "                   From    Portfolio_Group " +
                            "                   Where   PortfolioID = " + Convert.ToString(inPortfolioID) + " " +
                            "                   And     StartDate <= dbo.f_GetDate() " +
                            "                   And     isNull(EndDate,dbo.f_GetDate()+90) > dbo.f_GetDate() " +
                            "                 ) " +
                            "Order By 2 ";
                }
                dt_Fund = SystemLibrary.SQLSelectToDataTable(mySql);
                cb_Fund.DataSource = dt_Fund;
                cb_Fund.DisplayMember = "FundName";
                cb_Fund.ValueMember = "FundId";
                myIndex = cb_Fund.FindStringExact(CurrentSelection);
                if (myIndex < 0)
                {
                    myIndex = 0;
                    FundID = Convert.ToInt16(((DataRowView)(cb_Fund.Items[myIndex])).Row.ItemArray[0].ToString());
                    Fund_Name = ((DataRowView)(cb_Fund.Items[myIndex])).Row.ItemArray[1].ToString();
                    Fund_Amount = Convert.ToDecimal(((DataRowView)(cb_Fund.Items[myIndex])).Row.ItemArray[2].ToString());
                    Fund_Crncy = ((DataRowView)(cb_Fund.Items[myIndex])).Row.ItemArray[3].ToString();
                }
                cb_Fund.SelectedIndex = myIndex;

                LoadFundAmount();
            }
            catch { }

        } // LoadFund()

        private void LoadPortfolioGroup()
        {
            // TODO(2) Stop a Group where the Funds are of differing Currency.

            // Local Variables
            String mySql = "";
            String CurrentSelection = "<All>";
            int myIndex;

            if (dt_Portfolio != null)
            {
                CurrentSelection = cb_Portfolio.Text.ToString();
            }
            // Load the Portfolio ddlb
            try
            {
                // Need PortfolioAmount as Currency in Top half of SQL
                // - Use Currency from default portfolio.
                // All Funds, All Portfolios
                mySql = "Select -1 as PortfolioId, '<All>' as PortfolioName, Sum(PortfolioAmount) as PortfolioAmount, 'AUD' as crncy " +
                        "From   Portfolio " +
                        "Where  Active = 'Y' " +
                        "Union " +
                        "Select PortfolioId, PortfolioName, PortfolioAmount, crncy " +
                        "From   Portfolio " +
                        "Where  Active = 'Y' " +
                        "Order By 2 ";
                
                dt_Portfolio = SystemLibrary.SQLSelectToDataTable(mySql);
                cb_Portfolio.DataSource = dt_Portfolio;
                cb_Portfolio.DisplayMember = "PortfolioName";
                cb_Portfolio.ValueMember = "PortfolioId";
                myIndex = cb_Portfolio.FindStringExact(CurrentSelection);
                if (myIndex < 0)
                {
                    myIndex = 0;
                    PortfolioID = Convert.ToInt16(((DataRowView)(cb_Portfolio.Items[myIndex])).Row.ItemArray[0].ToString());
                    Portfolio_Name = ((DataRowView)(cb_Portfolio.Items[myIndex])).Row.ItemArray[1].ToString();
                    Portfolio_Amount = Convert.ToDecimal(((DataRowView)(cb_Portfolio.Items[myIndex])).Row.ItemArray[2].ToString());
                    Portfolio_Crncy = ((DataRowView)(cb_Portfolio.Items[myIndex])).Row.ItemArray[3].ToString();
                }
                cb_Portfolio.SelectedIndex = myIndex;

            }
            catch { }

            LoadFund(-1);

        } // LoadPortfolioGroup()

        private void LoadFundAmount()
        {
            // Local Variables
            String mySql = "";

            // Reset BPS fields
            BPS_PL_MTD = 0;
            BPS_PL_Yest = 0;
            BPS_PL_WRoll = 0;
            BPS_PL_MRoll = 0;
            BPS_PL_DeltaMax = 0;
            BPS_PL_Inception = 0;
            BPS_PL_YTD = 0;
            BPS_PL_YTD_July = 0;

            // Reset Index fields
            BPS_Index_Ticker = "";
            BPS_Index_DIV_TODAY = 0;
            BPS_Index_MTD = 0;
            BPS_Index_Yest = 0;
            BPS_Index_WRoll = 0;
            BPS_Index_MRoll = 0;
            BPS_Index_DeltaMax = 0;
            BPS_Index_Inception = 0;
            BPS_Index_YTD = 0;
            BPS_Index_YTD_July = 0;

            // Load the Total Fund Amount for this Fund/Portfolio combination
            mySql = "Exec sp_FundAmount " + cb_Fund.SelectedValue.ToString() + ", " + cb_Portfolio.SelectedValue.ToString();
            //Fund_Amount = SystemLibrary.SQLSelectDecimal(mySql);
            DataTable dt_1 = SystemLibrary.SQLSelectToDataTable(mySql);
            if (dt_1.Rows.Count > 0)
            {
                Fund_Amount = SystemLibrary.ToDecimal(dt_1.Rows[0][0]);
                if (dt_1.Columns.Count > 1)
                {
                    BPS_PL_MTD = SystemLibrary.ToDecimal(dt_1.Rows[0][1]);
                    BPS_PL_Yest = SystemLibrary.ToDecimal(dt_1.Rows[0][2]);
                    BPS_PL_WRoll = SystemLibrary.ToDecimal(dt_1.Rows[0][3]);
                    BPS_PL_MRoll = SystemLibrary.ToDecimal(dt_1.Rows[0][4]);
                    BPS_PL_DeltaMax = SystemLibrary.ToDecimal(dt_1.Rows[0][5]);
                    BPS_PL_Inception = SystemLibrary.ToDecimal(dt_1.Rows[0][6]);
                    BPS_PL_YTD = SystemLibrary.ToDecimal(dt_1.Rows[0][7]);
                    BPS_PL_YTD_July = SystemLibrary.ToDecimal(dt_1.Rows[0][8]);
                }
                if (dt_1.Rows.Count > 1)
                {
                    BPS_Index_Prev_Close = SystemLibrary.ToDecimal(dt_1.Rows[1][0]);
                    BPS_Index_Close = BPS_Index_Prev_Close;
                    BPS_Index_MTD = SystemLibrary.ToDecimal(dt_1.Rows[1][1]);
                    BPS_Index_Yest = SystemLibrary.ToDecimal(dt_1.Rows[1][2]);
                    BPS_Index_WRoll = SystemLibrary.ToDecimal(dt_1.Rows[1][3]);
                    BPS_Index_MRoll = SystemLibrary.ToDecimal(dt_1.Rows[1][4]);
                    BPS_Index_DeltaMax = SystemLibrary.ToDecimal(dt_1.Rows[1][5]);
                    BPS_Index_Inception = SystemLibrary.ToDecimal(dt_1.Rows[1][6]);
                    BPS_Index_YTD = SystemLibrary.ToDecimal(dt_1.Rows[1][7]);
                    BPS_Index_YTD_July = SystemLibrary.ToDecimal(dt_1.Rows[1][8]);
                    BPS_Index_DIV_TODAY = SystemLibrary.ToDecimal(dt_1.Rows[1][9]);
                    BPS_Index_Ticker = Convert.ToString(dt_1.Rows[1][10]);
                    if (dt_1.Columns.Count>12)
                        BPS_Index_Close = SystemLibrary.ToDecimal(dt_1.Rows[1][12]);

                    // Get this saved back to the database
                    if (dt_Last_Price.Columns.Count > 0)
                    {
                        DataRow[] FoundTickerRows = dt_Last_Price.Select("Ticker='" + BPS_Index_Ticker + "'");
                        if (FoundTickerRows.Length < 1)
                        {
                            DataRow dr = dt_Last_Price.NewRow();
                            dr["Ticker"] = BPS_Index_Ticker;
                            dr["LAST_PRICE"] = BPS_Index_Close;
                            dr["isNew"] = "N";
                            dt_Last_Price.Rows.Add(dr);
                        }
                    }

                }
            }
            else
                Fund_Amount = 0;

        } // LoadFundAmount()

        private void cb_Fund_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //SystemLibrary.SetDebugLevel(4);
            SystemLibrary.DebugLine("cb_Fund_SelectionChangeCommitted - Start(" + cb_Fund.Text + ")");
            if (cb_Fund.Items.Count > 0)
            {
                //SystemLibrary.SetDebugLevel(0);
                FundID = Convert.ToInt16(((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[0].ToString());
                Fund_Name = ((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[1].ToString();
                Fund_Amount = SystemLibrary.ToDecimal(((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[2].ToString());
                Fund_Crncy = ((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[3].ToString();
                LoadFundAmount();
                SystemLibrary.DebugLine("cb_Fund_SelectionChangeCommitted - Post LoadFundAmount()");
                /*this.SuspendLayout();*/
                LoadPortfolio(true);
                SystemLibrary.DebugLine("cb_Fund_SelectionChangeCommitted - Post LoadPortfolio(true)");
                /*
                ResetHiddenRows();
                SystemLibrary.DebugLine("cb_Fund_SelectionChangeCommitted - Post ResetHiddenRows()");
                this.ResumeLayout(true);
                */

                // Store the FundID to the Registry
                Registry.Registry myReg = new T1MultiAsset.Registry.Registry();
                myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\StartUp", "FundID", FundID);
            }
            //SystemLibrary.SetDebugLevel(4);
            SystemLibrary.DebugLine("cb_Fund_SelectionChangeCommitted - End");
            //SystemLibrary.SetDebugLevel(0);
        } // cb_Fund_SelectionChangeCommitted()

        private void cb_Portfolio_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SystemLibrary.DebugLine("cb_Portfolio_SelectionChangeCommitted - Start");
            if (cb_Portfolio.Items.Count > 0)
            {
                PortfolioID = Convert.ToInt16(((DataRowView)(cb_Portfolio.SelectedItem)).Row.ItemArray[0].ToString());
                Portfolio_Name = ((DataRowView)(cb_Portfolio.SelectedItem)).Row.ItemArray[1].ToString();
                Portfolio_Amount = SystemLibrary.ToDecimal(((DataRowView)(cb_Portfolio.SelectedItem)).Row.ItemArray[2].ToString());
                Portfolio_Crncy = ((DataRowView)(cb_Portfolio.SelectedItem)).Row.ItemArray[3].ToString();
                LoadFund(PortfolioID);
                this.SuspendLayout();
                LoadPortfolio(true);
                //cfr 20130918 ResetHiddenRows();
                this.ResumeLayout(true);

                // Store the FundID to the Registry
                Registry.Registry myReg = new T1MultiAsset.Registry.Registry();
                myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\StartUp", "PortfolioID", PortfolioID);
            }
            SystemLibrary.DebugLine("cb_Portfolio_SelectionChangeCommitted - End");
        } // cb_Portfolio_SelectionChangeCommitted()


        #endregion // Drop Down Boxes

        public delegate void LoadPostMaintainFundsCallback();
        public void LoadPostMaintainFunds()
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                LoadPostMaintainFundsCallback cb = new LoadPostMaintainFundsCallback(LoadPostMaintainFunds);
                this.Invoke(cb, new object[] { });
            }
            else
            {
                // Reload Fund/Portfolio
                LoadPortfolioGroup();
                cb_Portfolio_SelectionChangeCommitted(null, null);
                cb_Fund_SelectionChangeCommitted(null, null);
                LoadActionTab(true);
            }
        } //LoadPostMaintainFunds()


        public delegate void LoadActionTabCallback(Boolean NormalRun);
        public void LoadActionTab(Boolean NormalRun)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                LoadActionTabCallback cb = new LoadActionTabCallback(LoadActionTab);
                this.Invoke(cb, new object[] { NormalRun });
            }
            else
            {
                if (inStartUp)
                    return;

                // Check the Action Tab
                String RunLevel = "0";
                if (NormalRun)
                    RunLevel = "1";
                dt_Action = SystemLibrary.SQLSelectToDataTable("exec sp_ActionsNeeded " + RunLevel + ", 'Y' ");
                dg_Action.DataSource = dt_Action;
                tp_Action.Text = "ACTION (" + dt_Action.Rows.Count.ToString() + ")";
                // Occasionally the dt_Action query fails, so allow for this.
                if (dt_Action.Columns.Contains("DisplayOrder"))
                {
                    dg_Action.Columns["DisplayOrder"].Visible = false;
                    dg_Action.Columns["OpenForm"].Visible = false;
                    if (dt_Action.Rows.Count == 0)
                        dg_Action.Columns["ActionText"].HeaderText = "NO Actions required at the moment";
                    else
                        dg_Action.Columns["ActionText"].HeaderText = "Double-Click Row to solve required Action";
                }

            }
        }

        public delegate void SavePricesCallback();
        public void SavePrices()
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                SavePricesCallback cb = new SavePricesCallback(SavePrices);
                this.Invoke(cb, new object[] { });
            }
            else
            {
                // Local Variables
                DateTime myStartTime = SystemLibrary.f_Now();

                if (!isBloombergUser1)
                    return;

                // Save prices to Securities Table on database.
                if (DatabaseVersion.StartsWith("Microsoft SQL Server 2005"))
                {
                    for (int i = 0; i < dt_Last_Price.Rows.Count; i++)
                    {
                        SystemLibrary.DebugLine("SavePrices:" + dt_Last_Price.Rows[i]["Ticker"].ToString() + ", " + dt_Last_Price.Rows[i]["isNew"].ToString() + ", " + dt_Last_Price.Rows[i]["LAST_PRICE"].ToString());
                        if (dt_Last_Price.Rows[i]["isNew"].ToString() == "Y")
                        {
                            String myUpdate = "Update Securities " +
                                              "Set Last_Price = " + dt_Last_Price.Rows[i]["LAST_PRICE"].ToString() + " " +
                                              "Where BBG_Ticker = '" + dt_Last_Price.Rows[i]["Ticker"].ToString() + "' " +
                                              "And   ID_BB_UNIQUE = '" + dt_Last_Price.Rows[i]["ID_BB_UNIQUE"].ToString() + "' ";
                            SystemLibrary.SQLExecute(myUpdate);
                            SystemLibrary.DebugLine(myUpdate);
                            Application.DoEvents(); // Yield control back to the user inbetween these.
                        }
                        dt_Last_Price.Rows[i]["isNew"] = "N";
                    }
                }
                else
                {
                    SystemLibrary.SQLExecute("sp_Last_Price_TableParameter", "@TempTable", ref dt_Last_Price);
                    for (int i = 0; i < dt_Last_Price.Rows.Count; i++)
                        dt_Last_Price.Rows[i]["isNew"] = "N";
                }
                SystemLibrary.DebugLine(myStartTime, "SavePrices");
                //if (dt_load.Rows.Count>0)
                //    SystemLibrary.SQLBulkUpdate(dt_load, "BBG_Ticker,ID_BB_UNIQUE,LAST_PRICE", "Securities");
            }
        }


        public delegate void LoadPortfolioIncrCallback();
        public void LoadPortfolioIncr()
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                LoadPortfolioIncrCallback cb = new LoadPortfolioIncrCallback(LoadPortfolioIncr);
                this.Invoke(cb, new object[] { });
            }
            else
            {
                //Console.WriteLine("LoadPortfolioIncr() - Colin need to put this back together");
                //return;
                // Deal with incremental Portfolio changes from the database
                LoadPortfolio(false);
                /* CFR 20140203
                if (LoadPortfolio(false))
                    ResetHiddenRows();
                */
            }
        }

        public void LoadPortfolioTranspose(Boolean FullRefresh)
        {
            // Local Variables
            String mySql;
            String myFund;
            String myFundLast="";
            String myCol;
            String myShortName = "";
            String AllFunds = "";

            mySql = "Exec sp_PortfolioTranspose_New " + FundID.ToString() + ", " + PortfolioID.ToString();
            // Go to Database and Load dg_Port
            dt_PortfolioTranspose = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_PortfolioTranspose.DataSource = dt_PortfolioTranspose;

            // Start Build of dg_PortfolioTranspose
            ((System.ComponentModel.ISupportInitialize)(this.dg_PortfolioTranspose)).BeginInit();
            this.SuspendLayout();

            // Loop on all columns and set the Autosize mode & Header Text
            for (int i = 0; i < dg_PortfolioTranspose.Columns.Count; i++)
            {

                String myColName = dg_PortfolioTranspose.Columns[i].Name;
                switch (myColName)
                {
                    case "isFuture":
                        dg_PortfolioTranspose.Columns[i].Visible = false;
                        break;
                    case "BBG_Ticker":
                    case "Price":
                    case "Round_Lot_Size":
                    case "ModelWeight":
                    case "Incr":
                    case "crncy":
                    case "Pos_Mult_Factor":
                    case "FXRate":
                    case "Country_Full_Name":
                        dg_PortfolioTranspose.Columns[i].HeaderText = dg_PortfolioTranspose.Columns[i].HeaderText.Replace('_', ' ');
                        break;
                    default:
                        // Find the Fund details
                        myFund = myColName.Substring(myColName.IndexOf('_') + 1);
                        myCol = myColName.Substring(0,myColName.IndexOf('_'));
                        DataRow[] dr = dt_Fund.Select("FundId=" + myFund);
                        if (dr.Length > 0)
                        {
                            myShortName = dr[0]["ShortName"].ToString();
                            dg_PortfolioTranspose.Columns[i].HeaderText = myShortName + " " + myCol;
                            String myFundAmount = dr[0]["FundAmount"].ToString();
                            if (myFund != myFundLast)
                                AllFunds = AllFunds + myFund + "\t" + myFundAmount + ",";
                            myFundLast = myFund;
                        }
                        dg_PortfolioTranspose.Columns[i].Tag = myFund;
                        break;
                }
            }
            // Strips of last ,
            if (AllFunds.Length>0)
                AllFunds = AllFunds.Substring(0, AllFunds.Length - 1); 
            if (dg_PortfolioTranspose.Columns.Contains("BBG_Ticker"))
                dg_PortfolioTranspose.Columns["BBG_Ticker"].Tag = AllFunds;

            FormatPortfolioTranspose();
            ResetAlignColumns();

            // Complete
            ((System.ComponentModel.ISupportInitialize)(this.dg_PortfolioTranspose)).EndInit();
            this.ResumeLayout(true); //??
            //this.PerformLayout();

            isAlive_PortfolioTranspose = true;
            SetCalc();

        } //LoadPortfolioTranspose()

        private void FormatPortfolioTranspose()
        {
            // Needed to seperate this out for the Sort function that trashes all formats.
            // Deal with standard columns
            if (!dg_PortfolioTranspose.Columns.Contains("BBG_Ticker"))
                return;
            dg_PortfolioTranspose.Columns["BBG_Ticker"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg_PortfolioTranspose.Columns["BBG_Ticker"].HeaderText = "Ticker";
            SetFormatColumn(dg_PortfolioTranspose, "BBG_Ticker", Color.RoyalBlue, Color.Gainsboro, "####", "");
            dg_PortfolioTranspose.Columns["Incr"].HeaderText = "Incr Quantity";
            dg_PortfolioTranspose.Columns["ModelWeight"].HeaderText = "Model Weight";
            dg_PortfolioTranspose.Columns["Incr"].Frozen = true; // Frozen - All columns prior to this.
            SetFormatColumn(dg_PortfolioTranspose, "ModelWeight", Color.Empty, Color.LightBlue, "0.00%", ""); 
            SetFormatColumn(dg_PortfolioTranspose, "Incr", Color.Empty, Color.LightGreen, "N0", "");
            SetFormatColumn(dg_PortfolioTranspose, "Price", Color.Empty, Color.Empty, "N4", "");
            SetFormatColumn(dg_PortfolioTranspose, "FXRate", Color.RoyalBlue, Color.Gainsboro, "N4", "");
            //SetFormatColumn(dg_PortfolioTranspose, "LocalValue", Color.Empty, Color.Empty, "N0", "0");
            SetFormatColumn(dg_PortfolioTranspose, "Round_Lot_Size", Color.RoyalBlue, Color.Gainsboro, "N0", "1");
            dg_PortfolioTranspose.Columns["Country_Full_Name"].Visible = false;

            // Loop on all columns and set the Autosize mode & Header Text
            for (int i = 0; i < dg_PortfolioTranspose.Columns.Count; i++)
            {
                dg_PortfolioTranspose.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                String myColName = dg_PortfolioTranspose.Columns[i].Name;

                switch (myColName)
                {
                    case "isFuture":
                    case "BBG_Ticker":
                    case "Price":
                    case "Round_Lot_Size":
                    case "ModelWeight":
                    case "Incr":
                    case "crncy":
                    case "Pos_Mult_Factor":
                    case "FXRate":
                    case "Country_Full_Name":
                        break;
                    default:
                        if (myColName.StartsWith("Weight"))
                            SetFormatColumn(dg_PortfolioTranspose, myColName, Color.Empty, Color.Empty, "0.00%", "0");
                        else if (myColName.StartsWith("Incr"))
                            SetFormatColumn(dg_PortfolioTranspose, myColName, Color.Empty, Color.LightGreen, "N0", "");
                        else
                            SetFormatColumn(dg_PortfolioTranspose, myColName, Color.Empty, Color.Empty, "N0", "0");

                        // Loop over the Tickers 
                        for (Int32 j = 0; j < dg_PortfolioTranspose.Rows.Count; j++) // Last row in dg_Port is a blank row
                            if (dg_PortfolioTranspose["BBG_Ticker", j].Value != null)
                                SetColumn(dg_PortfolioTranspose, myColName, j);
                        break;
                }
            }

            // Complete
            ((System.ComponentModel.ISupportInitialize)(this.dg_PortfolioTranspose)).EndInit();
            this.ResumeLayout(false); //??
            this.PerformLayout();

        } //FormatPortfolioTranspose()

        public delegate Boolean LoadPortfolioCallback(Boolean FullRefresh);
        public Boolean LoadPortfolio(Boolean FullRefresh)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                LoadPortfolioCallback cb = new LoadPortfolioCallback(LoadPortfolio);
                return ((Boolean)this.Invoke(cb, new object[] { FullRefresh }));
            }
            else
            {
                // Local Variables
                String mySql;
                Boolean IsSameTickers = false;

                // Don't load portfolio until ready
                if (inStartUp)
                    return (false);

                try
                {
                    SystemLibrary.DebugLine("LoadPortfolio(" + SystemLibrary.Bool_To_YN(FullRefresh) + ") - Start");
                    // Check the Action Tab (if in Action tab, then will be called later in this block)
                    if (!tabControl_Port.SelectedTab.Text.StartsWith("ACTION"))
                        LoadActionTab(false);

                    if (FullRefresh)
                    {
                        mySql = "Exec sp_Positions 'Y', '1-jan-1900', " + FundID.ToString() + ", " + PortfolioID.ToString();
                        // Get the latest open tickers
                        SetUpLast_Price_DataTable();
                    }
                    else
                        mySql = "Exec sp_Positions 'N', '" + LastUpdated.ToString("d-MMM-yyyy HH:mm:ss.fff") + "', " + FundID.ToString() + ", " + PortfolioID.ToString();

                    // Go to Database and Load dg_Port
                    DataTable dt_PortLoad = SystemLibrary.SQLSelectToDataTable(mySql);
                    //Console.WriteLine(mySql+"\r\ndt_PortLoad.Rows.Count=" + dt_PortLoad.Rows.Count.ToString());
                    
                    //DataTable dt_Port = new System.Data.DataTable();
                    if (FullRefresh)
                    {
                        this.SuspendLayout();
                        try
                        {
                            dt_Port = dt_PortLoad.Copy();
                        }
                        catch { }
                        this.ResumeLayout(true);
                        // Check PortfolioTranspose
                        if (isAlive_PortfolioTranspose)
                            LoadPortfolioTranspose(FullRefresh);
                    }
                    else
                    {
                        // See if found new data
                        if (dt_PortLoad.Rows.Count == 0)
                            return (false);
                        // See if User is in the "TRADE" Tab & do nothing if they are
                        if (tabControl_Port.SelectedTab.Text == "TRADE" || tabControl_Port.SelectedTab.Text == "ALIGN")
                            return (false);
                        // Check PortfolioTranspose
                        if (isAlive_PortfolioTranspose)
                            LoadPortfolioTranspose(FullRefresh);

                        // For NOW
                        // - replace all the data.
                        this.SuspendLayout();
                        try
                        {
                            IsSameTickers = PortDataTableCompare(dt_Port, dt_PortLoad);
                            //IsSameTickers = false;
                            dt_Port = dt_PortLoad.Copy();
                            // Redo the Calcs
                            SetCalc();
                        }
                        catch { }
                        this.ResumeLayout(true);
                    }

                    // Calculate the MTD long/Short P&L
                    ResetMTD();

                    // Fail routine if no columns
                    if (dt_Port.Columns.Count < 1)
                    {
                        SystemLibrary.DebugLine("LoadPortfolio() - dt_Port has no columns");
                        return (false);
                    }

                    //dt_Port = SystemLibrary.SQLSelectToDataTable(mySql);
                    if (dt_Port.Rows.Count > 0)
                        LastUpdated = Convert.ToDateTime(dt_Port.Compute("max(LastUpdated)", ""));
                    else
                        LastUpdated = SystemLibrary.f_Now();
                    SystemLibrary.DebugLine(mySql+" = dt_PortRows:" + dt_Port.Rows.Count.ToString());
                    //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - dt_PortRows:" + dt_Port.Rows.Count.ToString());

                    //if (!IsSameTickers)
                    ((System.ComponentModel.ISupportInitialize)(this.dg_Port)).BeginInit();
                    this.SuspendLayout();
                    try
                    {
                        foreach (DataGridViewColumn dgvc in dg_Port.Columns)
                            dgvc.Frozen = false;
                        myDataView = dt_Port.DefaultView;
                        dg_Port.DataSource = myDataView;
                        // cfr 20120130 dg_Port.DataSource = dt_Port;

                        // Set Columns that will be frozen at the start
                        dg_Port.Columns["Price"].Frozen = true; // All columns prior to this.

                        // Add Column that allows for incremental trades
                        if (dg_Port.Columns["Quantity_incr"] == null)
                        {
                            dg_Port.Columns.Add("Quantity_incr", "Incremental Qty");
                            dg_Port.Columns["Quantity_incr"].ValueType = typeof(Decimal);
                        }
                        if (dg_Port.Columns["FUM_incr"] == null)
                        {
                            dg_Port.Columns.Add("FUM_incr", "Incr FUM %");
                            dg_Port.Columns["FUM_incr"].ValueType = typeof(Decimal);
                        }



                        // Hide System Columns
                        dg_Port.Columns["IsAggregate"].Visible = false;
                        dg_Port.Columns["isFuture"].Visible = false;
                        dg_Port.Columns["FundID"].Visible = false;
                        dg_Port.Columns["PortfolioID"].Visible = false;
                        dg_Port.Columns["Strategy1"].Visible = false;
                        dg_Port.Columns["Strategy2"].Visible = false;
                        dg_Port.Columns["Strategy3"].Visible = false;
                        dg_Port.Columns["Strategy4"].Visible = false;
                        dg_Port.Columns["Pos_Mult_Factor"].Visible = false;
                        dg_Port.Columns["Round_Lot_Size"].Visible = false;
                        dg_Port.Columns["FX_Pos_Mult_Factor"].Visible = false;
                        dg_Port.Columns["Undl_Ticker"].Visible = false;
                        dg_Port.Columns["Undl_Currency"].Visible = false;
                        dg_Port.Columns["Undl_Price"].Visible = false;
                        dg_Port.Columns["Delta"].Visible = false;
                        dg_Port.Columns["Undl_Price"].Visible = false;
                        dg_Port.Columns["ID_BB_COMPANY"].Visible = false;
                        dg_Port.Columns["ID_BB_UNIQUE"].Visible = false;
                        dg_Port.Columns["ID_BB_GLOBAL"].Visible = false;
                        //dg_Port.Columns["BBG_Exchange"].Visible = false;
                        dg_Port.Columns["CUSIP"].Visible = false;
                        dg_Port.Columns["ISIN"].Visible = false;
                        dg_Port.Columns["SEDOL"].Visible = false;
                        //dg_Port.Columns["LocalValue"].Visible = false;
                        dg_Port.Columns["Exposure"].Visible = false;
                        dg_Port.Columns["Exposure_Filled"].Visible = false;
                    }
                    catch { }
                    this.ResumeLayout(true);
                    ((System.ComponentModel.ISupportInitialize)(this.dg_Port)).EndInit();


                    Gross_Amount = 0;


                   SystemLibrary.DebugLine("LoadPortfolio() - pre: CheckFX()");

                    // TODO (2) Add underlying Indicies to this?
                    // TODO (3) Had to Add known currencies as haven't got RTD call correct.
                    CheckFX("GBP"); // Pounds
                    CheckFX("HKD"); // HKD
                    CheckFX("USD"); // US$
                    CheckFX("SGD"); // Singapore
                    CheckFX("EUR"); // Euro $
                    CheckFX("CHF"); // Swiss Franc
                    CheckFX("CAD"); // Canadian Dollar
                    CheckFX("JPY"); // Japanese Yen
                    CheckFX("MYR"); // Malaysian Ringgit
                    CheckFX("KRW"); // Korean Wan
                    CheckFX("TWD"); // Taiwanese Dollar
                    SystemLibrary.DebugLine("LoadPortfolio() - post: CheckFX()");

                    // I Split the formatting out so the Sort capture can use the same code
                    this.SuspendLayout();
                    try
                    {
                        /* CFR 20130918 SetFormat() & SetCalc() called indirectly via ResetHiddenRows()
                        SetFormat();
                        SystemLibrary.DebugLine("LoadPortfolio() - post: SetFormat()");
                        SetCalc();
                        SystemLibrary.DebugLine("LoadPortfolio() - post: SetCalc()");
                        */
                        ResetHiddenRows();
                    }
                    catch { }
                    this.ResumeLayout(true);
                    SystemLibrary.DebugLine("LoadPortfolio() - post: ResumeLayout()");
                    //SetGrossPCT();
                    //SetHeader();
                    if (!IsSameTickers)
                        Application.DoEvents();
                    else
                        SystemLibrary.SetColumn(dg_Port, "Price");


                    if (!IsSameTickers)
                    {
                        SystemLibrary.DebugLine("LoadPortfolio() - pre: Bloomberg_Connect()");
                        BRT.Bloomberg_RealtimeConnect(dt_Port, dt_Last_Price, Fund_Crncy, BPS_Index_Ticker);
                        SystemLibrary.DebugLine("LoadPortfolio() - post: Bloomberg_Connect()");
                    }
                }
                catch
                {
                }
                // I Split the formatting out so the Sort capture can use the same code
                //SetFormat();

                // Connect EMSX
                if (EMSX_API != null && isBloombergUser)
                    if (!EMSX_API.Connected)
                        EMSX_API.EMSX_APIConnect();
            }

            SystemLibrary.DebugLine("LoadPortfolio() - END");
            return (true);

        } // LoadPortfolio()

        private void ResetMTD()
        {
            if (dt_Port.Rows.Count > 0)
            {
                String AddFilter = "";
                if (ShowFutureHeaderLine)
                {
                    AddFilter = " and Ticker Not Like '%Index'";
                    Future_PL_MTD = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_MTD)", "Ticker Like '%Index'"));
                    Future_PL_Yest = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_Yest)", "Ticker Like '%Index'"));
                    Future_PL_WRoll = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_WRoll)", "Ticker Like '%Index'"));
                    Future_PL_MRoll = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_MRoll)", "Ticker Like '%Index'"));
                    Future_PL_DeltaMax = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_DeltaMax)", "Ticker Like '%Index'"));
                    Future_PL_Inception = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_Inception)", "Ticker Like '%Index'"));
                    Future_PL_YTD = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_YTD)", "Ticker Like '%Index'"));
                    Future_PL_YTD_July = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_YTD_July)", "Ticker Like '%Index'"));
                }
                Long_PL_MTD = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_MTD)", "LS='L'" + AddFilter));
                Short_PL_MTD = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_MTD)", "LS='S'" + AddFilter));
                Long_PL_Yest = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_Yest)", "LS='L'" + AddFilter));
                Short_PL_Yest = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_Yest)", "LS='S'" + AddFilter));
                Long_PL_WRoll = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_WRoll)", "LS='L'" + AddFilter));
                Short_PL_WRoll = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_WRoll)", "LS='S'" + AddFilter));
                Long_PL_MRoll = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_MRoll)", "LS='L'" + AddFilter));
                Short_PL_MRoll = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_MRoll)", "LS='S'" + AddFilter));
                Long_PL_DeltaMax = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_DeltaMax)", "LS='L'" + AddFilter));
                Short_PL_DeltaMax = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_DeltaMax)", "LS='S'" + AddFilter));
                Long_PL_Inception = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_Inception)", "LS='L'" + AddFilter));
                Short_PL_Inception = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_Inception)", "LS='S'" + AddFilter));
                Long_PL_YTD = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_YTD)", "LS='L'" + AddFilter));
                Short_PL_YTD = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_YTD)", "LS='S'" + AddFilter));
                Long_PL_YTD_July = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_YTD_July)", "LS='L'" + AddFilter));
                Short_PL_YTD_July = SystemLibrary.ToDecimal(dt_Port.Compute("Sum(PL_YTD_July)", "LS='S'" + AddFilter));
            }
            else
            {
                Long_PL_MTD = 0;
                Short_PL_MTD = 0;
                Future_PL_MTD = 0;
                Long_PL_Yest = 0;
                Short_PL_Yest = 0;
                Future_PL_Yest = 0;
                Long_PL_WRoll = 0;
                Short_PL_WRoll = 0;
                Future_PL_WRoll = 0;
                Long_PL_MRoll = 0;
                Short_PL_MRoll = 0;
                Future_PL_MRoll = 0;
                Long_PL_DeltaMax = 0;
                Short_PL_DeltaMax = 0;
                Future_PL_DeltaMax = 0;
                Long_PL_Inception = 0;
                Short_PL_Inception = 0;
                Future_PL_Inception = 0;
                Long_PL_YTD = 0;
                Short_PL_YTD = 0;
                Future_PL_YTD = 0;
                Long_PL_YTD_July = 0;
                Short_PL_YTD_July = 0;
                Future_PL_YTD_July = 0;
            }

        } // ResetMTD()

        private void SetFormat()
        {
            if (dg_Port.Columns.Count == 0)
                return;
            try
            {
                SystemLibrary.DebugLine("SetFormat() - Start");
                // I seperated out the formatting because a sort destroys formatting
                dg_Port.SuspendLayout();
                // Loop on all columns and set the Autosize mode & Header Text
                for (int i = 0; i < dg_Port.Columns.Count; i++)
                {
                    dg_Port.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dg_Port.Columns[i].HeaderText = dg_Port.Columns[i].HeaderText.Replace('_', ' ');
                }
                SystemLibrary.DebugLine("SetFormat() - Post Loop Columns");

                dg_Port.Columns["crncy"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dg_Port.Columns["crncy"].Width = 40;


                SetFormatColumn(dg_Port, "Quantity_incr", Color.Empty, Color.LightGreen, "N0", "");
                SetFormatColumn(dg_Port, "FUM_incr", Color.Empty, Color.LightGreen, "0.00%", "");
                //dg_Port.Columns["Quantity_incr"].
                // - Override every 2nd row format
                //dg_Port.Columns["Quantity_incr"].DefaultCellStyle.

                // Apply formats 
                SetFormatColumn(dg_Port, "Security_Name", Color.RoyalBlue, Color.Gainsboro, "", "");
                SetFormatColumn(dg_Port, "Quantity", Color.Empty, Color.Empty, "N0", "0");
                //SetFormatColumn(dg_Port, "Qty_EOD", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "Qty_EOD_Fill", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "Qty_Order", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "Qty_Routed", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "Qty_Fill", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "Quantity", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "Avg_Price", Color.Empty, Color.Empty, "N2", "0");
                SetFormatColumn(dg_Port, "Price", Color.Empty, Color.Empty, "N4", "");
                SetFormatColumn(dg_Port, "Prev_Close", Color.Empty, Color.Empty, "N4", "");
                SetFormatColumn(dg_Port, "FXRate", Color.RoyalBlue, Color.Gainsboro, "N4", "");
                SetFormatColumn(dg_Port, "LocalValue", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "Value", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "Exposure", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "Exposure_Filled", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, @"% Chg", Color.Empty, Color.Empty, "0.00%", "0");
                SetFormatColumn(dg_Port, "PL_EOD", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "PL BPS", Color.Empty, Color.Empty, "N2", "0");
                SetFormatColumn(dg_Port, "PL_Day", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "PL_Exec", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "PL_TradePeriod", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "PL_Yest", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "PL_WRoll", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "PL_MRoll", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "PL_MTD", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "PL_DeltaMax", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "PL_Inception", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "PL_YTD", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, "PL_YTD_July", Color.Empty, Color.Empty, "N0", "0");
                SetFormatColumn(dg_Port, @"% SOD", Color.Empty, Color.Empty, "0.00%", "0");
                SetFormatColumn(dg_Port, @"% FUM", Color.Empty, Color.Empty, "0.00%", "0");
                //SetFormatColumn(dg_Port, @"% Fill", Color.Empty, Color.Empty, "0.00%", "0");
                SetFormatColumn(dg_Port, @"% Gross", Color.Empty, Color.Empty, "0.00%", "0");
                SetFormatColumn(dg_Port, "AvgInPrice", Color.Empty, Color.Empty, "N2", "0");
                SetFormatColumn(dg_Port, "LastTradeDate", Color.Empty, Color.Empty, "dd-MMM-yy", "");
                SetFormatColumn(dg_Port, "OrigTradeDate", Color.Empty, Color.Empty, "dd-MMM-yy", "");
                SetFormatColumn(dg_Port, "Country_Full_Name", Color.RoyalBlue, Color.Empty, "", "");
                SetFormatColumn(dg_Port, "Sector", Color.RoyalBlue, Color.Empty, "", "");
                SetFormatColumn(dg_Port, "Industry_Group", Color.RoyalBlue, Color.Empty, "", "");
                SetFormatColumn(dg_Port, "Industry_SubGroup", Color.RoyalBlue, Color.Empty, "", "");
                SetFormatColumn(dg_Port, "Round_Lot_Size", Color.RoyalBlue, Color.Gainsboro, "N0", "1");
                SetFormatColumn(dg_Port, "Div_Adjusted", Color.Empty, Color.Empty, "N4", "");
                //dg_Port.Columns["Round_Lot_Size"].HeaderText = "Lot Size";

                // Align Text columns to the Left (default is right)
                dg_Port.Columns["Ticker"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dg_Port.Columns["Security_Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dg_Port.Columns["LS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dg_Port.Columns["crncy"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dg_Port.Columns["Country_Full_Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dg_Port.Columns["Sector"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dg_Port.Columns["Industry_Group"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dg_Port.Columns["Industry_SubGroup"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dg_Port.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dg_Port.Columns["FundName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dg_Port.Columns["PortfolioName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dg_Port.Columns["PM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dg_Port.Columns["IdeaOwner"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                SystemLibrary.DebugLine("SetFormat() - Post Individual Columns");

                // Loop over the Tickers 
                for (Int32 i = 0; i < dg_Port.Rows.Count; i++) // Last row in dg_Port is a blank row
                {
                    if (dg_Port["Ticker", i].Value != null)
                    {
                        if (dg_Port["LS", i].Value.ToString() == "L")
                            dg_Port["LS", i].Style.ForeColor = Color.Green;
                        else
                            dg_Port["LS", i].Style.ForeColor = Color.Red;
                        SetColumn(dg_Port, "Quantity", i);
                        SetColumn(dg_Port, "Qty_Order", i);
                        SetColumn(dg_Port, "Qty_Routed", i);
                        SetColumn(dg_Port, "Qty_Fill", i);
                        SetColumn(dg_Port, "PL_Yest", i);
                    }
                }
                SystemLibrary.DebugLine("SetFormat() - End");
            }
            catch 
            { }

            dg_Port.ResumeLayout(true);
        } //SetFormat()

        private Boolean PortDataTableCompare(DataTable dt_Port, DataTable dt_PortLoad)
        {  
            /*
             * Purpose: To see if new dt_Port datatable is just a quantity update or more
             * 
             * Return:  True if just a quantity update, False if anything else
             */

            // Local Variables
            DataTable dt_Copy_Port;
            DataTable dt_Copy_PortLoad;
            int rows_Port = dt_Port.Rows.Count;
            int rows_PortLoad = dt_PortLoad.Rows.Count;

            
            if (rows_Port != rows_PortLoad)
                return (false);

            // Force a sort on both datatables
            dt_Copy_Port = dt_Port.Copy();
            dt_Copy_PortLoad = dt_PortLoad.Copy();
            dt_Copy_Port.Select("", "FundName, PortfolioName, Ticker, FundID, PortfolioID, IsAggregate, LS, Quantity, QTY_Order");
            dt_Copy_PortLoad.Select("", "FundName, PortfolioName, Ticker, FundID, PortfolioID, IsAggregate, LS, Quantity, QTY_Order");

            for (int i = 0; i < rows_Port; i++)
            {
                if (dt_Copy_Port.Rows[i]["Ticker"].ToString() != dt_Copy_PortLoad.Rows[i]["Ticker"].ToString()) return (false);
                if (dt_Copy_Port.Rows[i]["IsAggregate"].ToString() != dt_Copy_PortLoad.Rows[i]["IsAggregate"].ToString()) return (false);
                if (dt_Copy_Port.Rows[i]["FundID"].ToString() != dt_Copy_PortLoad.Rows[i]["FundID"].ToString()) return (false);
                if (dt_Copy_Port.Rows[i]["PortfolioID"].ToString() != dt_Copy_PortLoad.Rows[i]["PortfolioID"].ToString()) return (false);
                if (dt_Copy_Port.Rows[i]["LS"].ToString() != dt_Copy_PortLoad.Rows[i]["LS"].ToString()) return (false);
                if (dt_Copy_Port.Rows[i]["Quantity"].ToString() != dt_Copy_PortLoad.Rows[i]["Quantity"].ToString()) return (false);

                // Copy the price across.
                dt_PortLoad.Rows[i]["Price"] = dt_Port.Rows[i]["Price"];
                dt_PortLoad.Rows[i]["FXRate"] = dt_Port.Rows[i]["FXRate"];
                //for (int j=0;j<
                //dt_Port.Rows[i]

            }

            return (true);

        } //PortDataTableCompare()

        private void ReLoadTabs()
        {
            String CurrentTab;
            int TabIndex = -1;

            CurrentTab = tabControl_Port.SelectedTab.Text;
            LoadTabs();

            for (int i = 0; i < tabControl_Port.TabPages.Count; i++)
            {
                if (tabControl_Port.TabPages[i].Text == CurrentTab)
                    TabIndex = i;
            }
            if (TabIndex == -1)
                tabControl_Port.SelectTab(tabControl_Port.TabPages.Count - 1);
            else
            {
                try
                {
                    tabControl_Port.SelectTab(TabIndex);
                }
                catch
                {
                    tabControl_Port.SelectTab(tabControl_Port.TabPages.Count - 1);
                }
            }
            //tabControl_Port_SelectedIndexChanged(tabControl_Port, null);

        } //ReLoadTabs()

        private void FixDataGridHeight()
        {
            // For some reason Windows does not fit dg_Port in the correct spot on occasions - even though dg_Port is anchored.
            if (dg_Port.Top + dg_Port.Height > menuStrip1.Top)
                dg_Port.Height = menuStrip1.Top - dg_Port.Top;

            // Align Datagridview Tab with Port Datagridview
            dg_Action.Top = dg_Port.Top;
            dg_Action.Left = dg_Port.Left;
            dg_Action.Width = dg_Port.Width;
            dg_Action.Height = dg_Port.Height;

            dg_PortfolioTranspose.Top = gb_Align.Top + gb_Align.Height + 2;
            dg_PortfolioTranspose.Left = dg_Port.Left;
            dg_PortfolioTranspose.Width = dg_Port.Width;
            dg_PortfolioTranspose.Height = dg_Port.Height - (gb_Align.Height + 2);

        } //FixDataGridHeight()

        private void tabControl_Port_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Local Variables
            int ColOrder = 0;
            TabControl tc_Port = tabControl_Port; // (TabControl)sender;
            String TabName = tc_Port.SelectedTab.Text;
            bt_Create_Orders.Visible = false;
            bt_EMSX.Visible = false;

            // Deals with lack of database while running
            try
            {
                FixDataGridHeight();

                toolStripMenuItem1.Enabled = true;
                toolStripMenuItem1.Visible = true;

                // UNhide hidden rows
                // Hide Rows That are for FX balance
                for (int i = 0; i < dg_Port.Rows.Count; i++)
                {
                    dg_Port.Rows[i].Visible = true;
                }
                if (dg_Port.Columns.Count > 0)
                {
                    SetFormatColumn(dg_Port, "Qty_EOD", Color.Empty, Color.Empty, "N0", "0");
                    SetFormatColumn(dg_Port, @"% Fill", Color.Empty, Color.Empty, "0.00%", "0");
                }

                switch (TabName)
                {
                    case "TRADE":
                        // Allow user to change
                        dg_Port.Visible = true;
                        dg_Action.Visible = false;
                        dg_PortfolioTranspose.Visible = false;
                        gb_Align.Visible = false;
                        dg_Port.AllowUserToAddRows = true;
                        dg_Port.ReadOnly = false;
                        dg_Port.AlternatingRowsDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
                        bt_Create_Orders.Visible = true;
                        bt_EMSX.Visible = true;

                        // Organise columns
                        // - Hide All columns
                        foreach (DataGridViewColumn dc in dg_Port.Columns)
                            dc.Frozen = false;

                        foreach (DataGridViewColumn dc in dg_Port.Columns)
                        {
                            switch (dc.Name)
                            {
                                case "Ticker":
                                    dc.ReadOnly = false;
                                    dc.Visible = true;
                                    break;
                                case "Price":
                                    dc.ReadOnly = true;
                                    dc.Visible = true;
                                    break;
                                case "Security_Name":
                                    dc.ReadOnly = true;
                                    dc.Visible = true;
                                    break;
                                case "Quantity":
                                    dc.ReadOnly = true;
                                    dc.Visible = true;
                                    break;
                                case "Qty_Order":
                                    dc.ReadOnly = true;
                                    dc.Visible = true;
                                    break;
                                case "Qty_Fill":
                                    dc.ReadOnly = true;
                                    dc.Visible = true;
                                    break;
                                case "Qty_EOD":
                                    dc.ReadOnly = true;
                                    dc.Visible = true;
                                    break;
                                case "Quantity_incr":
                                    dc.ReadOnly = false;
                                    dc.Visible = true;
                                    break;
                                case "FUM_incr":
                                    dc.ReadOnly = false;
                                    dc.Visible = true;
                                    break;
                                case "Value":
                                    dc.ReadOnly = false;
                                    dc.Visible = true;
                                    break;
                                case "Exposure":
                                    dc.ReadOnly = false;
                                    dc.Visible = true;
                                    break;
                                case @"% FUM":
                                    dc.ReadOnly = false;
                                    dc.Visible = true;
                                    break;
                                case @"% Fill":
                                    dc.ReadOnly = true;
                                    dc.Visible = true;
                                    break;
                                case "Round_Lot_Size":
                                    dc.ReadOnly = true;
                                    dc.Visible = true;
                                    break;
                                case "FXRate":
                                    dc.ReadOnly = true;
                                    dc.Visible = true;
                                    break;
                                default:
                                    if (dc.Visible == true)
                                        dc.Visible = false;
                                    break;
                            }
                        }
                        // Now set the Column Order, followed by Frozen
                        dg_Port.Columns["Ticker"].DisplayIndex = ColOrder++;
                        dg_Port.Columns["Price"].DisplayIndex = ColOrder++;
                        dg_Port.Columns["Security_Name"].DisplayIndex = ColOrder++;
                        dg_Port.Columns["Quantity"].DisplayIndex = ColOrder++;
                        dg_Port.Columns["Qty_Order"].DisplayIndex = ColOrder++;
                        dg_Port.Columns["Qty_Fill"].DisplayIndex = ColOrder++;
                        dg_Port.Columns["Qty_EOD"].DisplayIndex = ColOrder++;
                        dg_Port.Columns["Quantity_incr"].DisplayIndex = ColOrder++;
                        dg_Port.Columns["FUM_incr"].DisplayIndex = ColOrder++;
                        dg_Port.Columns["Value"].DisplayIndex = ColOrder++;
                        dg_Port.Columns["Exposure"].DisplayIndex = ColOrder++;
                        dg_Port.Columns[@"% FUM"].DisplayIndex = ColOrder++;
                        dg_Port.Columns[@"% Fill"].DisplayIndex = ColOrder++;
                        dg_Port.Columns["Round_Lot_Size"].DisplayIndex = ColOrder++;
                        dg_Port.Columns["FXRate"].DisplayIndex = ColOrder++;
                        dg_Port.Columns["Ticker"].Frozen = true;
                        if (dg_Port.Columns.Count > 0)
                        {
                            SetFormatColumn(dg_Port, "Qty_EOD", Color.Empty, Color.Gainsboro, "N0", "0");
                            SetFormatColumn(dg_Port, @"% Fill", Color.Empty, Color.Gainsboro, "0.00%", "0");
                        }
                        // Hide Rows That are for FX balance
                        // CFR 20120327 UnHideRows();
                        // CFR 20120327 HideAggregateRows("Y");
                        // CFR 20120327 HideFXbalanceRows();
                        myDataView.RowFilter = "IsAggregate <> 'Y' AND " +
                                               "ISNULL(Industry_Group,'') Not In ('Equity Equiv','Cash Equiv')";

                        // Deal with Currency display
                        for (int i = 0; i < dg_Port.Rows.Count; i++)
                        {
                            if (SystemLibrary.ToString(dg_Port.Rows[i].Cells["Sector"].Value) == "Currency")
                            {
                                dg_Port.Rows[i].Cells["Quantity"].Style.Format = "N2";
                                dg_Port.Rows[i].Cells["Qty_Order"].Style.Format = "N2";
                                dg_Port.Rows[i].Cells["Qty_Fill"].Style.Format = "N2";
                                dg_Port.Rows[i].Cells["Qty_EOD"].Style.Format = "N2";
                                dg_Port.Rows[i].Cells["Quantity_incr"].Style.Format = "N2";
                                dg_Port.Rows[i].Cells["Value"].Style.Format = "N2";
                                dg_Port.Rows[i].Cells["Exposure"].Style.Format = "N2";
                            }
                        }

                        break;
                    case @"ColinFullList":
                        // Allow user to change
                        dg_Port.Visible = true;
                        dg_Action.Visible = false;
                        dg_PortfolioTranspose.Visible = false;
                        gb_Align.Visible = false;
                        dg_Port.AllowUserToAddRows = false;
                        dg_Port.ReadOnly = true;
                        dg_Port.AlternatingRowsDefaultCellStyle = dg_Port_AltStyle;
                        // Reset background column on the Modelling columns
                        TidyTRADEColumns();

                        // Organise columns
                        foreach (DataGridViewColumn dc in dg_Port.Columns)
                        {
                            dc.Visible = true;
                        }
                        // unHide Rows
                        // CFR 20120327 UnHideRows();
                        myDataView.RowFilter = "";
                        break;
                    default:
                        dg_Port.Visible = true;
                        dg_Action.Visible = false;
                        dg_PortfolioTranspose.Visible = false;
                        gb_Align.Visible = false;
                        // See if this is the ACTION Tab
                        if (tc_Port.SelectedTab.Text.StartsWith("ACTION"))
                        {
                            if (tc_Port.SelectedTab.Tag != null)
                            {
                                if (tc_Port.SelectedTab.Tag.ToString() == "SYSTEM")
                                {
                                    LoadActionTab(true);
                                    dg_Action.Visible = true;
                                    dg_Port.Visible = false;
                                    dg_PortfolioTranspose.Visible = false;
                                    gb_Align.Visible = false;
                                }
                            }
                        }
                        // See if this is the Portfolio Transpose Tab
                        if (tc_Port.SelectedTab.Text.StartsWith("ALIGN"))
                        {
                            if (tc_Port.SelectedTab.Tag != null)
                            {
                                if (tc_Port.SelectedTab.Tag.ToString() == "SYSTEM")
                                {
                                    if (!isAlive_PortfolioTranspose)
                                        LoadPortfolioTranspose(true);
                                    dg_PortfolioTranspose.Visible = true;
                                    gb_Align.Visible = true;
                                    dg_Port.Visible = false;
                                    dg_Action.Visible = false;
                                    bt_Create_Orders.Visible = true;
                                    bt_EMSX.Visible = true;
                                }
                            }
                        }
                        if (dg_Port.Visible == true)
                        {
                            // Allow user to change
                            dg_Port.AllowUserToAddRows = false;
                            dg_Port.ReadOnly = true;
                            dg_Port.AlternatingRowsDefaultCellStyle = dg_Port_AltStyle;
                            // Reset background column on the Modelling columns
                            TidyTRADEColumns();

                            // See if any columns for this Tab - Only occurs on a fresh database.
                            if (dg_Port.Columns.Count < 1)
                                return;

                            // Organise columns
                            foreach (DataGridViewColumn dc in dg_Port.Columns)
                            {
                                DataRow[] dr = dt_Port_Tab_Detail.Select("TabName='" + TabName + "' and ColName='" + dc.Name + "'");
                                if (dr.Length == 0)
                                    dc.Visible = false;
                                else
                                {
                                    dc.Frozen = false;
                                    dc.Visible = true;
                                }
                                if (dc.Name == "Ticker")
                                    dc.DefaultCellStyle.ForeColor = Color.DarkBlue;
                                //dc.Visible = true;
                                //SystemLibrary.DebugLine(dc.Name);
                            }
                            // I found I needed to set the Sort Order in its own loop
                            SystemLibrary.DebugLine("StartLoop:" + TabName);
                            // Sort on ColOrder Descending otherwise can still have wrong order columns
                            foreach (DataRow dr_TD in dt_Port_Tab_Detail.Select("TabName='" + TabName + "'", "ColOrder desc"))
                            {
                                SystemLibrary.DebugLine(TabName + "," + dr_TD["ColName"].ToString() + "," + dr_TD["ColOrder"].ToString());
                                dg_Port.Columns[dr_TD["ColName"].ToString()].DisplayIndex = Convert.ToInt16(dr_TD["ColOrder"]);
                            }
                            SystemLibrary.DebugLine("EndLoop:" + TabName);

                            // Organise the Frozen Column
                            DataRow[] dr1 = dt_Port_Tabs.Select("TabName='" + TabName + "'");
                            if (dr1.Length > 0)
                            {
                                String FrozenColumn = SystemLibrary.ToString(dr1[0]["FrozenColName"]);
                                if (FrozenColumn.Length > 0)
                                {
                                    if (dg_Port.Columns.Contains(FrozenColumn))
                                    {
                                        try
                                        {
                                            dg_Port.Columns[FrozenColumn].Frozen = true;
                                        }
                                        catch { }
                                    }
                                }
                            }

                            // Hide Rows where isAggregate ="Y"
                            String myRowFilter = "";
                            if (dr1.Length > 0)
                                myRowFilter = dr1[0]["RowFilter"].ToString();
                            if (myRowFilter.Length > 0)
                                myDataView.RowFilter = myRowFilter;
                            else
                            {
                                // CFR 20120327 UnHideRows();
                                // CFR 20120327 HideAggregateRows("Y");
                                myDataView.RowFilter = "IsAggregate <> 'Y' ";
                            }
                            //SetFormat();
                        }
                        break;
                }
                SetFormat();
                SetCalc();
            }
            catch (Exception ex)
            {
                Console.WriteLine("tabControl_Port_SelectedIndexChanged(): " + SystemLibrary.GetFullErrorMessage(ex));
            }

        } //tabControl_Port_SelectedIndexChanged()

        public delegate void ResetHiddenRowsCallback();
        public void ResetHiddenRows()
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                ResetHiddenRowsCallback cb = new ResetHiddenRowsCallback(ResetHiddenRows);
                this.Invoke(cb, new object[] { });
            }
            else
            {
                // Needs to be in this order
                tabControl_Port_SelectedIndexChanged(null, null);
                // CFR 20120327 HideAggregateRows(flag_HideAggregateRows);
                // CFR 20120327 if (flag_HideFXbalanceRows)
                // CFR 20120327     HideFXbalanceRows();
            }
        } //ResetHiddenRows()

        private void UnHideRows()
        {
            flag_HideAggregateRows = "";
            // Stripped out so programmer calls explicitly to stop issues with HideFXbalanceRows() & HideAggregateRows()
            // Hide Rows That are for FX balance
            for (int i = 0; i < dg_Port.Rows.Count - 1; i++) // LastRow is a New one
            {
                dg_Port.Rows[i].Visible = true;
            }
        } // UnHideRows()

        private void HideFXbalanceRows()
        {
            // RULES: May/Will need to call UnHideRows() from Parent routine before calling this.

            // For when user in "TRADE" tab
            // Hide Rows That are for FX balance
            for (int i = 0; i < dg_Port.Rows.Count - 1; i++)// LastRow is a New one
            {
                if ((dg_Port["Industry_Group", i].Value != null && dg_Port["Ticker", i].Value.ToString().ToUpper().EndsWith(" CURNCY")))
                {
                    if (dg_Port["Industry_Group", i].Value.ToString() == "Equity Equiv" ||
                        dg_Port["Industry_Group", i].Value.ToString() == "Cash Equiv"
                        )
                    {
                        // Cannot hide the selected row, so if needed, then move the selection to the first row or last row
                        dg_Port.ClearSelection();
                        if (dg_Port.CurrentRow != null)
                            if (dg_Port.CurrentRow.Index == i)
                            {
                                // Choose the next available row - NB: dg_Port.CurrentCell = null; doesn't work.
                                for (int j = 0; j < dg_Port.Rows.Count - 1; j++)// LastRow is a New one
                                {
                                    if (dg_Port.Rows[j].Visible == true && j != dg_Port.CurrentRow.Index)
                                    {
                                        dg_Port.CurrentCell = dg_Port.Rows[j].Cells[dg_Port.CurrentCell.OwningColumn.Name];
                                        break;
                                    }
                                }
                            }
                        dg_Port.Rows[i].Visible = false;
                    }
                }
            }
        } //HideFXbalanceRows()

        private void HideAggregateRows(String AggTriState)
        {
            SystemLibrary.DebugLine("HideAggregateRows(" + AggTriState+") - Start");
            //
            // Rules: 
            //  1) AggTriState
            //      "Y" = Hide IsAggregate = "Y"
            //      "N" = Hide IsAggregate = "N"
            //      "" = Do Not filter on IsAggregate.
            //
            flag_HideAggregateRows = AggTriState;
            if (dg_Port.DataSource == null)
                return;

            if (AggTriState == "")
                myDataView.RowFilter = "";
            else if (AggTriState == "Y")
                myDataView.RowFilter = "IsAggregate <> 'Y'";
            else
                myDataView.RowFilter = "IsAggregate <> 'N'";
            SetFormat();

            SystemLibrary.DebugLine("HideAggregateRows(" + AggTriState + ") - End");

        } //HideAggregateRows()

        private void bt_Create_Orders_Click(object sender, EventArgs e)
        {
            //
            // Procedure:   bt_Create_Orders_Click
            //
            // Purpose: Create the Order
            //
            // Rules:
            // 1) All Alphabetic Characters Should Be Uppercase
            //

            // TODO (1) Manual, vs Dealer, vs Direct to Bloomberg
            // TODO (1) SHORTS - Getting Borrow & Nominating Source of Borrow & Rate of Borrow (I have a Table for this)
            // TODO (1) Trade Reason ???
            // TODO (1) Strategy, Owner, etc

            // Local Variables
            Registry.Registry myReg = new T1MultiAsset.Registry.Registry();
            String myTable = "Orders";
            String mySplitsTable = "Orders_Split";
            String myColumns = "";
            //String myPath;
            //String myUUID;
            //String mySerialNumber;
            //String myFileName;
            //String myBasket = "";
            String BBGTicker;
            String Country;
            String Ticker = "";
            String Exch = "";
            String YellowKey = "";
            String isFuture = "N";
            int ExistingOrder;
            Decimal Quantity_incr;
            int myRows;
            int FundID;
            int PortfolioID;
            int Round_Lot_Size;
            int NonEMSXTradeCount = 0;


            // Prepare for Send


            // Make sure the Fund is setup
            if (Fund_Amount == 0 || dt_Fund.Rows.Count < 2 || dt_Portfolio.Rows.Count < 2)
            {
                MessageBox.Show("Before you can trade, you must first Create a Fund/Portfolio with Capital.", "Create Order");
                return;
            }

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            // Define The Trades
            // - Remove existing rows
            for (int i = SendToBloomberg.dt_SendToBloomberg.Rows.Count - 1; i >= 0; i--)
                SendToBloomberg.dt_SendToBloomberg.Rows.RemoveAt(i);

            // Determin Which Tab this is coming from
            switch (tabControl_Port.SelectedTab.Text)
            {
                case "TRADE":
                    // See if this is just 1 fund or All
                    if (MessageBox.Show(this, "Creating Orders for:\r\n\r\n    Fund:     '" + Fund_Name + "' \r\n\t\tand \r\n    Portfolio:     '" + Portfolio_Name + "'\r\n\r\n\r\n" +
                                  "Do you wish to Continue?", "Create Orders", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Orders have not been created.", "Create Orders - Aborted");
                        return;
                    }

                    // Flush the edit data if any
                    dg_Port.Refresh();
                    // - Add new rows
                    foreach (DataGridViewRow currentRow in dg_Port.Rows)
                    {
                        if (SystemLibrary.ToString(currentRow.Cells["Sector"].Value).ToUpper() == "CURRENCY")
                            Quantity_incr = SystemLibrary.ToDecimal(currentRow.Cells["Quantity_incr"].Value);
                        else
                            Quantity_incr = Convert.ToInt32(currentRow.Cells["Quantity_incr"].Value);

                        if (Quantity_incr != 0)
                        {
                            // Build up the Trades
                            BBGTicker = currentRow.Cells["Ticker"].Value.ToString();
                            Country = currentRow.Cells["Country_Full_Name"].Value.ToString();
                            isFuture = SystemLibrary.ToString(currentRow.Cells["isFuture"].Value);
                            ExistingOrder = Convert.ToInt32(SystemLibrary.ToDecimal(currentRow.Cells["Quantity"].Value) + SystemLibrary.ToDecimal(currentRow.Cells["Qty_Order"].Value));
                            SendToBloomberg.EMSTickerSplit(BBGTicker, ref Ticker, ref Exch, ref YellowKey);
                            DataRow dr = SendToBloomberg.dt_SendToBloomberg.NewRow();
                            dr["BloombergTicker"] = Ticker.ToUpper();
                            dr["Exchange"] = Exch.ToUpper();
                            dr["isFuture"] = isFuture.ToUpper();
                            dr["IdentifierType"] = "C"; // C = Cusip, But unused field
                            dr["Currency"] = currentRow.Cells["crncy"].Value.ToString().ToUpper();
                            dr["OrderType"] = "MKT"; //MKT = market, But unused field
                            dr["Side"] = SendToBloomberg.GetSide(Ticker, YellowKey, Country, ExistingOrder, (int)Quantity_incr).ToUpper();
                            dr["OrderQuantity"] = Math.Abs(Quantity_incr); // Always positive # of shares
                            dr["TimeinForce"] = "DAY";
                            dr["OrderRefID"] = SendToBloomberg.GetNextOrderRefID();
                            dr["YellowKey"] = YellowKey.ToUpper();
                            // Dont Send Currencies to EMSX
                            if (SystemLibrary.ToString(currentRow.Cells["Sector"].Value).ToUpper() == "CURRENCY")
                                NonEMSXTradeCount++;
                            else
                                SendToBloomberg.dt_SendToBloomberg.Rows.Add(dr);


                            // -- Place the Order
                            DataTable dt_load = SystemLibrary.SQLBulk_GetDefinition(myColumns, myTable);
                            DataRow drSQL = dt_load.NewRow();
                            drSQL["OrderRefID"] = dr["OrderRefID"];
                            drSQL["EffectiveDate"] = SystemLibrary.f_Today();
                            drSQL["BBG_Ticker"] = BBGTicker;
                            drSQL["Exchange"] = Exch.ToUpper();
                            drSQL["Crncy"] = dr["Currency"];
                            drSQL["Quantity"] = Quantity_incr;
                            drSQL["Side"] = dr["Side"];
                            drSQL["OrderType"] = dr["OrderType"];
                            // drSQL["Limit"] = dr["Limit"];
                            drSQL["TimeinForce"] = dr["TimeinForce"];
                            drSQL["UserName"] = SystemInformation.UserName;
                            drSQL["UpdateDate"] = SystemLibrary.f_Now();
                            drSQL["CreatedDate"] = SystemLibrary.f_Now();
                            drSQL["ProcessedEOD"] = "N";
                            drSQL["ManualOrder"] = "N";
                            drSQL["Order_Completed"] = "N";
                            dt_load.Rows.Add(drSQL);
                            myRows = SystemLibrary.SQLBulkUpdate(dt_load, myColumns, myTable);
                            // Process the Splits
                            // TODO (1) BAD CODE HERE - Need to properly assign a FundID & PortfolioID for a NEW row.
                            if (currentRow.Cells["FundID"].Value == null)
                                FundID = this.FundID;
                            else
                                FundID = SystemLibrary.ToInt16(currentRow.Cells["FundID"].Value);
                            if (currentRow.Cells["PortfolioID"].Value == null)
                                PortfolioID = this.PortfolioID;
                            else
                                PortfolioID = SystemLibrary.ToInt16(currentRow.Cells["PortfolioID"].Value);
                            if (currentRow.Cells["Round_Lot_Size"].Value==DBNull.Value)
                                Round_Lot_Size = 1;
                            else
                                Round_Lot_Size = Convert.ToInt32(currentRow.Cells["Round_Lot_Size"].Value);
                            if (Round_Lot_Size <= 0)
                                Round_Lot_Size = 1;
                            SystemLibrary.SQLExecute("Exec sp_OrderSplits '" + dr["OrderRefID"].ToString() + "', " + FundID.ToString() + ", " + PortfolioID.ToString() + ", " + Round_Lot_Size.ToString());
                        }
                    }
                    break;
                case "ALIGN":
                    // - Add new rows
                    Boolean FoundLong = false;
                    Boolean FoundShort = false;
                    Boolean FoundExistingLong = false;
                    Boolean FoundExistingShort = false;
                    Char[] mySeperatorRow = { ',' };

                    // Flush the edit data if any
                    dg_PortfolioTranspose.Refresh();
                    String[] AllFunds = dg_PortfolioTranspose.Columns["BBG_Ticker"].Tag.ToString().Split(mySeperatorRow, StringSplitOptions.RemoveEmptyEntries);
                    if (AllFunds.Length == 0)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("No funds available.", "Create Orders");
                        return;
                    }

                    // FIRST - Loop down the rows looking for orders with Long & Short
                    foreach (DataGridViewRow currentRow in dg_PortfolioTranspose.Rows)
                    {
                        // See if there is a value in Incr
                        if (SystemLibrary.ToInt32(currentRow.Cells["Incr"].Value) == 0)
                            continue;

                        // Reset the flags
                        FoundLong = false;
                        FoundShort = false;

                        for (Int32 af = 0; af < AllFunds.Length; af++)
                        {
                            String[] myFundIDFundAmount = AllFunds[af].Split('\t');
                            if (myFundIDFundAmount.Length == 2)
                            {
                                Int32 myQty = SystemLibrary.ToInt32(currentRow.Cells["Incr_" + myFundIDFundAmount[0]].Value);
                                if (myQty > 0)
                                    FoundLong = true;
                                else if (myQty < 0)
                                    FoundShort = true;
                                Int32 myExistingQty = SystemLibrary.ToInt32(currentRow.Cells["Quantity_" + myFundIDFundAmount[0]].Value);
                                if (myExistingQty > 0)
                                    FoundExistingLong = true;
                                else if (myExistingQty < 0)
                                    FoundExistingShort = true;
                            }
                        }
                        if (FoundExistingLong && FoundExistingShort)
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Sorry cannot deal with Long & Short Original Positions on the same ticker.\r\n\r\n" +
                                            currentRow.Cells["BBG_Ticker"].Value.ToString() + "\r\n\r\n" +
                                            "The problem relates to mixing an order with either Buy Long & Buy-To-Cover Or Sell Long & Sell-Short.\r\n\r\n" +
                                            "Please put them through as seperate orders", "Create Orders");
                            return;
                        }
                        if (FoundLong && FoundShort)
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Sorry cannot deal with Long & Short trades on the same ticker.\r\n\r\n" +
                                            currentRow.Cells["BBG_Ticker"].Value.ToString() + "\r\n\r\n" +
                                            "Please put them through as seperate orders", "Create Orders");
                            return;
                        }
                    }

                    // Now create the Order/Orders_Split/EMSX record
                    foreach (DataGridViewRow currentRow in dg_PortfolioTranspose.Rows)
                    {
                        // See if there is a value in Incr
                        Quantity_incr = SystemLibrary.ToInt32(currentRow.Cells["Incr"].Value);
                        if (Quantity_incr == 0)
                            continue;

                        // Create the Bloomberg record & the Orders record
                        // Build up the Trades
                        String OrderRefID = SendToBloomberg.GetNextOrderRefID();
                        BBGTicker = currentRow.Cells["BBG_Ticker"].Value.ToString();
                        Country = currentRow.Cells["Country_Full_Name"].Value.ToString();
                        isFuture = SystemLibrary.ToString(currentRow.Cells["isFuture"].Value);

                        Round_Lot_Size = SystemLibrary.ToInt32(currentRow.Cells["Round_Lot_Size"].Value);
                        if (Round_Lot_Size <= 0)
                            Round_Lot_Size = 1;

                        // Get the existing Qty
                        ExistingOrder = 0;
                        for (Int32 af = 0; af < AllFunds.Length; af++)
                        {
                            String[] myFundIDFundAmount = AllFunds[af].Split('\t');
                            if (myFundIDFundAmount.Length == 2)
                            {
                                Int32 myExistingQty = SystemLibrary.ToInt32(currentRow.Cells["Quantity_" + myFundIDFundAmount[0]].Value);
                                ExistingOrder = ExistingOrder + myExistingQty;
                            }
                        }

                        /*
                        if (FoundExistingShort)
                            ExistingOrder = -1 * Math.Abs(Quantity_incr);
                        else
                            ExistingOrder = Math.Abs(Quantity_incr);
                        */
                        
                        SendToBloomberg.EMSTickerSplit(BBGTicker, ref Ticker, ref Exch, ref YellowKey);
                        DataRow dr = SendToBloomberg.dt_SendToBloomberg.NewRow();
                        dr["BloombergTicker"] = Ticker.ToUpper();
                        dr["Exchange"] = Exch.ToUpper();
                        dr["isFuture"] = isFuture.ToUpper();
                        dr["IdentifierType"] = "C"; // C = Cusip, But unused field
                        dr["Currency"] = currentRow.Cells["crncy"].Value.ToString().ToUpper();
                        dr["OrderType"] = "MKT"; //MKT = market, But unused field
                        dr["Side"] = SendToBloomberg.GetSide(Ticker, YellowKey, Country, ExistingOrder, (int)Quantity_incr).ToUpper();
                        dr["OrderQuantity"] = Math.Abs(Quantity_incr); // Always positive # of shares
                        dr["TimeinForce"] = "DAY";
                        dr["OrderRefID"] = OrderRefID;
                        dr["YellowKey"] = YellowKey.ToUpper();
                        SendToBloomberg.dt_SendToBloomberg.Rows.Add(dr);

                        // -- Place the Order
                        DataTable dt_load = SystemLibrary.SQLBulk_GetDefinition(myColumns, myTable);
                        DataRow drSQL = dt_load.NewRow();
                        drSQL["OrderRefID"] = dr["OrderRefID"];
                        drSQL["EffectiveDate"] = SystemLibrary.f_Today();
                        drSQL["BBG_Ticker"] = BBGTicker;
                        drSQL["Exchange"] = Exch.ToUpper();
                        drSQL["Crncy"] = dr["Currency"];
                        drSQL["Quantity"] = Quantity_incr;
                        drSQL["Side"] = dr["Side"];
                        drSQL["OrderType"] = dr["OrderType"];
                        // drSQL["Limit"] = dr["Limit"];
                        drSQL["TimeinForce"] = dr["TimeinForce"];
                        drSQL["UserName"] = SystemInformation.UserName;
                        drSQL["UpdateDate"] = SystemLibrary.f_Now();
                        drSQL["CreatedDate"] = SystemLibrary.f_Now();
                        drSQL["ProcessedEOD"] = "N";
                        drSQL["ManualOrder"] = "N";
                        drSQL["Order_Completed"] = "N";
                        dt_load.Rows.Add(drSQL);
                        myRows = SystemLibrary.SQLBulkUpdate(dt_load, myColumns, myTable);

                        // Create the Orders_Split record
                        DataTable dt_load_Split = SystemLibrary.SQLBulk_GetDefinition(myColumns, mySplitsTable);
                        for (Int32 af = 0; af < AllFunds.Length; af++)
                        {
                            String[] myFundIDFundAmount = AllFunds[af].Split('\t');
                            if (myFundIDFundAmount.Length == 2)
                            {
                                String myFund = myFundIDFundAmount[0];
                                Int32 myQty = SystemLibrary.ToInt32(currentRow.Cells["Incr_" + myFund].Value);
                                if (myQty != 0)
                                {
                                    // Process the Splits
                                    DataRow drSQL_Split = dt_load_Split.NewRow();
                                    drSQL_Split["OrderRefID"] = OrderRefID;
                                    drSQL_Split["FundID"] = SystemLibrary.ToInt16(myFund);
                                    drSQL_Split["PortfolioID"] = this.PortfolioID; // PortfolioID from Form
                                    drSQL_Split["Quantity"] = myQty;
                                    drSQL_Split["Round_Lot_Size"] = Round_Lot_Size;
                                    dt_load_Split.Rows.Add(drSQL_Split);
                                }
                            }
                        }
                        myRows = SystemLibrary.SQLBulkUpdate(dt_load_Split, myColumns, mySplitsTable);
                        if (this.PortfolioID==-1)
                            SystemLibrary.SQLExecute("Exec sp_OrderSplits_Align '" + dr["OrderRefID"].ToString() + "' ");
                    }
                    break;
            }

            if (SendToBloomberg.dt_SendToBloomberg.Rows.Count < 1 && NonEMSXTradeCount == 0)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("No Orders to Send at this time.\r\n\r\nModel Up Orders and submit again.", "Create Orders");
                return;
            }

            // Send to EMSX
            String myMessage = "";
            if (SendToBloomberg.dt_SendToBloomberg.Rows.Count > 0 && isBloombergUser)
                myMessage = SendToBloomberg.EMSXAPI_Send();

            // Update The Positions Table
            SystemLibrary.SQLExecute("Exec sp_Update_Positions 'Y' ");
            // Refresh the Portfolio Tab
            LoadPortfolio(true);
            /* CFR 20140203
            if(LoadPortfolio(true))
                ResetHiddenRows();
            */
            Cursor.Current = Cursors.Default; 

            // Let the User know orders sent to Bloomberg
            if (myMessage.Length > 0)
                MessageBox.Show(this, myMessage, "Create Orders");
            else
                MessageBox.Show(this, (NonEMSXTradeCount + SendToBloomberg.dt_SendToBloomberg.Rows.Count).ToString() + " Orders Created", "Create Orders");

        } //bt_Create_Orders_Click()

        private void dg_Action_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // TODO (1) Code needs to Find any existing window and close before the reopen.
            // Perform the Action
            if (e.RowIndex < 0)
                return;

            String OpenForm = dg_Action["OpenForm", e.RowIndex].Value.ToString();

            switch (OpenForm.ToUpper())
            {
                case "RECONCILE":
                    NAVReconcileReport frm_NAV = new NAVReconcileReport();
                    frm_NAV.FromParent(this);
                    frm_NAV.Show();
                    break;
                case "MISSINGSETTLEMENTDATE":
                    MissingSettlementDate frm_MissSett = new MissingSettlementDate();
                    frm_MissSett.Show();
                    break;
                case "SOFTWAREERROR":
                    SystemLibrary.iMessageBox("It appears there is an error being caused in the backend of the system that helps with reporting to the ACTION tab.\r\n\r\n" +
                                    "Please report this to your administrator with the following details.\r\n\r\n\r\n\r\n" +
                                    dg_Action["ActionText", e.RowIndex].Value.ToString(), "Portfolio Management System - Reporting Software Error",MessageBoxIcon.Warning,false);
                    break;
                case "EOD_PRICES":
                    eODPricesToolStripMenuItem_Click(null, null);
                    break;
                case "START_OF_DAY":
                    startOfDayToolStripMenuItem_Click(null, null);
                    break;
                case "BROKERMAINTENANCE":
                    BrokerMaintenance frmNew = new BrokerMaintenance();
                    frmNew.FromParent(this);
                    frmNew.Show();
                    break;
                case "FILLWITHOUTORDER":
                    FillWithoutOrder frmNew1 = new FillWithoutOrder();
                    frmNew1.FromParent(this);
                    frmNew1.Show();
                    break;
                case "PROCESSORDERS":
                    ProcessOrders frm_po = new ProcessOrders();
                    frm_po.FromParent(this);
                    frm_po.Show();
                    break;
                case "PROCESSTRADES":
                    ProcessTrades frm_pt = new ProcessTrades();
                    frm_pt.FromParent(this);
                    frm_pt.Show();
                    break;
                case "ASICSHORTREPORT":
                    ASICShortReport frm_a = new ASICShortReport();
                    frm_a.FromParent(this);
                    frm_a.Show();
                    break;
                case "MISSINGSECURITIES":
                    MissingSecurities frm_ms = new MissingSecurities();
                    frm_ms.FromParent(this);
                    frm_ms.Show();
                    break;
                case "MAINTAINFUNDS":
                    MaintainFunds frm_mf = new MaintainFunds();
                    frm_mf.FromParent(this);
                    frm_mf.Show();
                    break;
                case "MLPRIME_RECONCILE":
                    MLPrime_Reconcile frm_mlp = new MLPrime_Reconcile();
                    frm_mlp.FromParent(this);
                    frm_mlp.Show();
                    break;
                case "MLFUTURE_RECONCILE":
                    MLFuture_Reconcile frm_mlf = new MLFuture_Reconcile();
                    frm_mlf.FromParent(this);
                    frm_mlf.Show();
                    break;
                case "FORWARDDIVIDENDS":
                    ForwardDividends frm_fd = new ForwardDividends();
                    frm_fd.FromParent(this);
                    frm_fd.Show();
                    break;
                case "TRANSACTIONS":
                    Transactions frm_trans = new Transactions();
                    frm_trans.FromParent(this, FundID);
                    frm_trans.Show();
                    break;
                case "MLFUTURES_BALANCE":
                    MLFutures_Balance frm_fut_bal = new MLFutures_Balance();
                    frm_fut_bal.FromParent(this, FundID);
                    frm_fut_bal.Show();
                    break;
                case "MLPRIME_BALANCE":
                    MLPrime_Balance frm_Prime_bal = new MLPrime_Balance();
                    frm_Prime_bal.FromParent(this, FundID);
                    frm_Prime_bal.Show();
                    break;
                case "PROCESSUNALLOCATEDTRANSACTIONS":
                    ProcessUnallocatedTransactions frm_Unalloc = new ProcessUnallocatedTransactions();
                    frm_Unalloc.FromParent(this);
                    frm_Unalloc.Show();
                    break;
                case "PROCESSFUNDCLOSEDTRANSACTIONS":
                    ProcessFundClosedTransactions frm_ClosedTrans = new ProcessFundClosedTransactions();
                    frm_ClosedTrans.FromParent(this);
                    frm_ClosedTrans.Show();
                    break;
                case "EMSX_MULTI":
                    EMSX_MULTI_Override frm_MULTI = new EMSX_MULTI_Override();
                    //frm_MULTI.FromParent(this, FundID);
                    frm_MULTI.Show();
                    break;
                case "REPORTBORROW":
                    ReportBorrow frm_rb = new ReportBorrow();
                    frm_rb.FromParent(this);
                    frm_rb.Show();
                    break;
                case "SYSTEMINTEGRITY":
                    SystemIntegrity frm_si = new SystemIntegrity();
                    frm_si.FromParent(this);
                    frm_si.Show();
                    break;
            }

        } //dg_Action_CellDoubleClick()

        private void dg_Port_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Local Variables
            SystemLibrary.DebugLine("Hello");

            // Deal with Ctrl-V            
            //      if (e.Control == true && e.KeyValue == (int)Keys.V)
            {
            }

        }

        private void processOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessOrders f = new ProcessOrders();
            f.FromParent(this);
            f.Show(); //(this);
        }

        private void aSICShortReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ASICShortReport f = new ASICShortReport();
            f.FromParent(this);
            f.Show(); //(this);
        }

        private void processTradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessTrades f = new ProcessTrades();
            f.FromParent(this);
            f.Show(); //(this);
        }

        private void processTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Transactions f = new Transactions();
            f.FromParent(this, FundID);
            f.Show(); //(this);

        } 

        private void timer_LoadPortfolio_Tick(object sender, EventArgs e)
        {
            // Stop the Timer until this everything has finished
            timer_LoadPortfolio.Stop();
            Thread t = new Thread(new ThreadStart(LoadPortfolioIncr));
            t.Start();

            // Restart the Timer
            timer_LoadPortfolio.Start();
        }

        private void timer_SavePrices_Tick(object sender, EventArgs e)
        {
            timer_SavePrices.Stop();
            Thread t = new Thread(new ThreadStart(SavePrices));
            t.Start();

            // Restart the Timer
            timer_SavePrices.Start();
        }

        private void eODPricesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            String myFilePath = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('EOD_Prices')");
            if (File.Exists(myFilePath))
                System.Diagnostics.Process.Start(myFilePath);
            else
                MessageBox.Show("End of Day Processing - Cannot find file\r\n\r\n" + myFilePath, "EOD Prices");
            */
            
            EOD_Prices f = new EOD_Prices();
            f.FromParent(this);
            f.Show(); //(this);
        }

        private void startOfDayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Local Variables
            String myMSG = "";

            LoadActionTab(true);
            if (dt_Action.Rows.Count > 0)
                myMSG = "Are you Sure?\r\n- You still have " + dt_Action.Rows.Count.ToString() + " items in the [ACTION]'s tab.\r\n\r\n";

            if (MessageBox.Show(this, myMSG +
                          "Make sure you have Run End-of-Day Prices.\r\n\n" +
                          "Do you wish to Continue?", "Start of Day", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Run Start of Day
                SystemLibrary.SQLExecute("Exec sp_Calc_Profit_RebuildFrom null"); // Force last date profit recalc.
                SystemLibrary.SQLExecute("Exec sp_SOD_Positions");
                // Load basic Data
                LoadPortfolioGroup();
                LoadPortfolio(true);
                /* CFR 20140203
                if(LoadPortfolio(true))
                    ResetHiddenRows();
                */

                // Need to find FundID in cb_Fund && PortfolioID in cb_Portfo
                DataRow[] dr_FindP = dt_Portfolio.Select("PortfolioId=" + PortfolioID.ToString());
                if (dr_FindP.Length > 0)
                    cb_Portfolio.SelectedValue = PortfolioID;
                // Now do Fund
                LoadFund(PortfolioID);
                DataRow[] dr_Find = dt_Fund.Select("FundID=" + FundID.ToString());
                if (dr_Find.Length > 0)
                    cb_Fund.SelectedValue = FundID;
                cb_Portfolio_SelectionChangeCommitted(null, null);
                cb_Fund_SelectionChangeCommitted(null, null);

                MessageBox.Show("Start of Day Completed", "Start of Day");
            }
        }

        private void forwardDividendsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ForwardDividends f = new ForwardDividends();
            f.FromParent(this);
            f.Show(); //(this);

        }

        private void brokerMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BrokerMaintenance f = new BrokerMaintenance();
            f.FromParent(this);
            f.Show(); //(this);
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Save the form width
            int myWidth = this.Width;
            int myHeight = this.Height;
            this.SuspendLayout();
            try
            {
                if (pictureBox1.Tag.ToString().Length == 0)
                    pictureBox1.Tag = "Up";
                if (this.Font.SizeInPoints > 12.0f)
                    pictureBox1.Tag = "Down";
                else if (this.Font.SizeInPoints < 6.0f)
                    pictureBox1.Tag = "Up";
                ChangeFontSize(pictureBox1.Tag.ToString());
                FontSizeSave();
                // Need to reset where the screen goes as this code seems to increase/decrease the screen size.
                PositionLoad();
            }
            catch { }
            // Load the form width
            this.Width = myWidth;
            this.Height = myHeight;
            this.ResumeLayout(true);
        }

        private void ChangeFontSize(String Direction)
        {
            switch (Direction.ToUpper())
            {
                case "UP":
                    ChangeControlFontSize(this, +1);
                    break;
                default:
                    ChangeControlFontSize(this, -1);
                    break;
            }

        }

        private void ChangeControlFontSize(Control myControl, float FontSizeIncrement)
        {
            // See if this is the Top level
            String CallerName = new StackTrace().GetFrame(1).GetMethod().Name;
            if (CallerName != "ChangeControlFontSize")
                myControl.Font = new Font(myControl.Font.FontFamily, myControl.Font.SizeInPoints + FontSizeIncrement);
            foreach (Control mySubControl in myControl.Controls)
            {
                mySubControl.Font = new Font(mySubControl.Font.FontFamily, mySubControl.Font.SizeInPoints + FontSizeIncrement);
                ChangeControlFontSize(mySubControl, FontSizeIncrement);
            }

        }

        private void dg_Port_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            // See if allowed to delete
            if (tabControl_Port.SelectedTab.Text != "TRADE")
            {
                // Abort delete
                e.Cancel = true;
            }
            else if (SystemLibrary.ToDecimal(dg_Port["Quantity_incr", e.Row.Index].Value) != 0 ||
                     SystemLibrary.ToDecimal(dg_Port["Quantity", e.Row.Index].Value) != 0 ||
                     SystemLibrary.ToDecimal(dg_Port["Qty_Order", e.Row.Index].Value) != 0
                    )
            {
                // Abort delete
                e.Cancel = true;
            }

        }

        public delegate void ReconnectBRTCallback();
        public void ReconnectBRT()
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                ReconnectBRTCallback cb = new ReconnectBRTCallback(ReconnectBRT);
                this.Invoke(cb, new object[] { });
            }
            else
            {
                BRT.Bloomberg_RealtimeConnect();
            }

        } //ReconnectBRT()



        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            BRT.Bloomberg_RealtimeConnect(dt_Port, dt_Last_Price, Fund_Crncy, BPS_Index_Ticker);
        } //bt_Refresh_Click()

        private void mLPrimeBrokerFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Processed " + SystemLibrary.MLPrimeGetFiles(ref MLPrimeConfigured).ToString() + " files", "Process ML Prime Broker Files");

        } //mLPrimeBrokerFilesToolStripMenuItem_Click()

        private void scotiaPrimeBrokerFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Processed " + SystemLibrary.SCOTIAPrimeGetFiles(ref ScotiaPrimeConfigured).ToString() + " files", "Process Scotia Prime Broker Files");

        } //scotiaPrimeBrokerFilesToolStripMenuItem_Click()

        private void mLFuturesFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Processed " + SystemLibrary.MLFuturesGetFiles(ref MLFuturesConfigured).ToString() + " files", "Process ML Futures Files");

        } //mLFuturesFilesToolStripMenuItem_Click()

        private void missingSecuritiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MissingSecurities f = new MissingSecurities();
            f.FromParent(this);
            f.Show(); //(this);

        } //missingSecuritiesToolStripMenuItem_Click()

        private void maintainFundstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            MaintainFunds f = new MaintainFunds();
            f.FromParent(this);
            f.Show(); //(this);

        } //maintainFundstoolStripMenuItem_Click()

        private void ResetAlignColumns()
        {
            // Local Variables
            String myName;
            int myPos;

            // Loop around the columns and Hide/Show as appropriate
            for (int i = 0; i < dg_PortfolioTranspose.Columns.Count; i++)
            {
                myName = dg_PortfolioTranspose.Columns[i].Name;
                myPos = myName.IndexOf('_');
                if (myPos > 0)
                {
                    myName = myName.Substring(0, myPos);
                    switch (myName)
                    {
                        case "Weight":
                            dg_PortfolioTranspose.Columns[i].Visible = cb_Align_Weight.Checked;
                            break;
                        case "Incr":
                            dg_PortfolioTranspose.Columns[i].Visible = cb_Align_Incr.Checked;
                            break;
                        case "Quantity":
                            dg_PortfolioTranspose.Columns[i].Visible = cb_Align_Quantity.Checked;
                            break;
                        case "Fill":
                            dg_PortfolioTranspose.Columns[i].Visible = cb_Align_Qty_Fill.Checked;
                            break;
                        case "LocalValue":
                            dg_PortfolioTranspose.Columns[i].Visible = cb_Align_LocalValue.Checked;
                            break;
                        case "Value":
                            dg_PortfolioTranspose.Columns[i].Visible = cb_Align_Value.Checked;
                            break;
                    }
                }
            }
            // Information Columns
            if (dg_PortfolioTranspose.Columns.Count > 0)
            {
                dg_PortfolioTranspose.Columns["Round_Lot_Size"].Visible = cb_Align_InfoColumns.Checked;
                dg_PortfolioTranspose.Columns["Pos_Mult_Factor"].Visible = false;
                dg_PortfolioTranspose.Columns["crncy"].Visible = cb_Align_InfoColumns.Checked;
                dg_PortfolioTranspose.Columns["FXRate"].Visible = cb_Align_InfoColumns.Checked;
            }

        } //ResetAlignColumns()

        private void cb_Align_WeightOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_Align_WeightOnly.Checked)
            {
                cb_Align_Weight.Checked = true;
                cb_Align_Incr.Checked = false;
                cb_Align_Quantity.Checked = false;
                cb_Align_Qty_Fill.Checked = false;
                cb_Align_LocalValue.Checked = false;
                cb_Align_Value.Checked = false;
                cb_Align_InfoColumns.Checked = false;
                ResetAlignColumns();
            }
        } //cb_Align_WeightOnly_CheckedChanged()

        private void cb_Align_Qty_Fill_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_Align_Qty_Fill.Checked)
            {
                cb_Align_Weight.Checked = false;
                cb_Align_Incr.Checked = false;
                cb_Align_Quantity.Checked = true;
                cb_Align_LocalValue.Checked = false;
                cb_Align_WeightOnly.Checked = false;
                cb_Align_Value.Checked = false;
                cb_Align_InfoColumns.Checked = false;
            }
            ResetAlignColumns();
        } //cb_Align_Qty_Fill_CheckedChanged()


        private void cb_Align_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb_in = (CheckBox)sender;

            if(cb_in.Checked)
                cb_Align_WeightOnly.Checked = false;
            ResetAlignColumns();
        } //cb_Align_CheckedChanged()

        private void dg_PortfolioTranspose_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Allow Function Keys to work on Bloomberg Tickers

            // Local Variables
            DataGridView dg = (DataGridView)sender;
            if (dg.CurrentCell.OwningColumn.Name.ToUpper().Contains("TICKER"))
            {
                // Allow for the capture of Bloomberg Function Keys while in Cell Edit mode.
                e.Control.PreviewKeyDown -= new PreviewKeyDownEventHandler(SystemLibrary.BBGOnPreviewKeyDown);
                e.Control.PreviewKeyDown += new PreviewKeyDownEventHandler(SystemLibrary.BBGOnPreviewKeyDown);
            }
            else
            {
                e.Control.PreviewKeyDown -= new PreviewKeyDownEventHandler(SystemLibrary.BBGOnPreviewKeyDown);
            }

        } //dg_PortfolioTranspose_EditingControlShowing()

        private void dg_PortfolioTranspose_MouseClick(object sender, MouseEventArgs e)
        {
            CXLocation = dg_Port.Location.X + e.Location.X + 5;
            CYLocation = dg_Port.Location.Y + e.Location.Y;

        } //dg_PortfolioTranspose_MouseClick()

        private void dg_PortfolioTranspose_MouseDown(object sender, MouseEventArgs e)
        {
            // Local Variables
            String myData = "";

            CXLocation = dg_Port.Location.X + e.Location.X + 5;
            CYLocation = dg_Port.Location.Y + e.Location.Y;

            FixDataGridHeight();


            // Show the Bloomberg popup menu
            try
            {
                DataGridView.HitTestInfo info = dg_PortfolioTranspose.HitTest(e.X, e.Y);
                if (e.Button == MouseButtons.Right && info.RowIndex > -1 && info.ColumnIndex > -1)
                {
                    if (dg_PortfolioTranspose.Rows[info.RowIndex].Cells[info.ColumnIndex].OwningColumn.Name.ToString() == "BBG_Ticker")
                    {
                        String Ticker = SystemLibrary.ToString(dg_PortfolioTranspose.Rows[info.RowIndex].Cells["BBG_Ticker"].Value);

                        if (Ticker.Length > 0)
                        {
                            SystemLibrary.BBGShowMenu(FundID, PortfolioID, Ticker, Portfolio_Name, SystemLibrary.BBGRelativeTicker(Ticker), this.Location.X + CXLocation, this.Location.Y + CYLocation);
                        }
                    }
                    else if (dg_PortfolioTranspose.Rows[info.RowIndex].Cells[info.ColumnIndex].OwningColumn.Name.ToString().StartsWith("Quantity_") ||
                             dg_PortfolioTranspose.Rows[info.RowIndex].Cells[info.ColumnIndex].OwningColumn.Name.ToString().StartsWith("Fill_") 
                             )
                    {
                        // Quantity Column
                        ShowFillStatus fs = new ShowFillStatus();
                        SystemLibrary.FormExists(fs, true);
                        String myFundName = "";
                        String myColName = dg_PortfolioTranspose.Rows[info.RowIndex].Cells[info.ColumnIndex].OwningColumn.Name.ToString();
                        int myFund = SystemLibrary.ToInt32(myColName.Substring(myColName.IndexOf('_') + 1));

                        // Get the Fund Name
                        DataRow[] dr = dt_Fund.Select("FundId=" + myFund);
                        if (dr.Length > 0)
                        {
                            myFundName = dr[0]["ShortName"].ToString();
                        }
                        else
                        {
                            myColName = dg_PortfolioTranspose.Rows[info.RowIndex].Cells[info.ColumnIndex].OwningColumn.Name.ToString();
                            myFundName = myColName.Substring(myColName.IndexOf('_') + 1);
                        }

                        fs.FromParent(this, myFund, PortfolioID, myFundName, SystemLibrary.ToString(dg_PortfolioTranspose.Rows[info.RowIndex].Cells["BBG_Ticker"].Value), SystemLibrary.ToDecimal(dg_PortfolioTranspose.Rows[info.RowIndex].Cells["Price"].Value));
                        fs.Show();
                    }
                }
                else
                {
                    if (info.RowIndex >= 0)
                    {
                        myData = SystemLibrary.ToString(dg_PortfolioTranspose.Rows[info.RowIndex].Cells["BBG_Ticker"].Value);
                        if (myData.Length > 0)
                        {
                            inDragMode = true;
                            dg_PortfolioTranspose.DoDragDrop(myData, DragDropEffects.Copy);
                        }
                    }
                }
            }
            catch { }



        } //dg_PortfolioTranspose_MouseDown()

        private void dg_PortfolioTranspose_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            // See if allowed to delete
            // Loop around the columns looking for an existing quantity
            for (int i = 0; i < dg_PortfolioTranspose.Columns.Count; i++)
            {
                if (dg_PortfolioTranspose.Columns[i].Name.StartsWith("Quantity"))
                {
                    // Look at the value on this row
                    if (SystemLibrary.ToDecimal(e.Row.Cells[i].Value) != 0)
                    {
                        // Abort delete
                        e.Cancel = true;
                        return;
                    }
                }
            }
        } //dg_PortfolioTranspose_UserDeletingRow()

        private Decimal GetFundAmount(int FundID, ref String ShortName)
        {
            return (GetFundAmount(FundID.ToString(), ref ShortName));

        } //GetFundAmount()

        private Decimal GetFundAmount(int FundID)
        {
            return (GetFundAmount(FundID.ToString()));

        } //GetFundAmount()

        private Decimal GetFundAmount(String myFund)
        {
            String ShortName = "";
            return (GetFundAmount(myFund, ref ShortName));

        } //GetFundAmount()

        private Decimal GetFundAmount(String myFund, ref String ShortName)
        {
            // Local Variables
            Decimal myFundAmount = 0;
            // Get the Fund Amount
            DataRow[] dr = dt_Fund.Select("FundId=" + myFund);
            if (dr.Length > 0)
            {
                ShortName = dr[0]["ShortName"].ToString();
                myFundAmount = SystemLibrary.ToDecimal(dr[0]["FundAmount"]);
            }

            return (myFundAmount);

        } //GetFundAmount()

        private void SetIncr_PortfolioTranspose(int myRow)
        {
            SetValueRG(dg_PortfolioTranspose.Rows[myRow], "Incr", GetIncr_PortfolioTranspose(myRow));

        } //SetIncr_PortfolioTranspose()

        private int GetIncr_PortfolioTranspose(int myRow)
        {
            // Find all the funds & set Incr = Sum(Incr_<FundID>)

            // Local Variables
            int Incr = 0;


            Char[] mySeperatorRow = { ',' };
            String[] AllFunds = dg_PortfolioTranspose.Columns["BBG_Ticker"].Tag.ToString().Split(mySeperatorRow, StringSplitOptions.RemoveEmptyEntries);
            if (AllFunds.Length > 0)
            {
                for (Int32 af = 0; af < AllFunds.Length; af++)
                {
                    String[] myFundIDFundAmount = AllFunds[af].Split('\t');
                    if (myFundIDFundAmount.Length == 2)
                    {
                        String myFund = myFundIDFundAmount[0];
                        int myFundIncr = SystemLibrary.ToInt32(dg_PortfolioTranspose["Incr_"+myFund,myRow].Value);
                        Incr = Incr + myFundIncr;
                    }
                }
            }

            return (Incr);

        } //SetIncr_PortfolioTranspose()

        private void dg_PortfolioTranspose_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dg_PortfolioTranspose.Columns[e.ColumnIndex].Name.StartsWith("Quantity_"))
                e.Cancel = true;

            // Seems the only way I can get the previous value before CellEndEdit().
            inEditMode = true;
            LastValue = dg_PortfolioTranspose[e.ColumnIndex, e.RowIndex].Value;
            toolStripMenuItem1.Enabled = false;
            toolStripMenuItem1.Visible = false;

        } //dg_PortfolioTranspose_CellBeginEdit()

        private void dg_PortfolioTranspose_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Local Variables
            Decimal POS_Mult_Factor;
            int Round_Lot_Size;
            Decimal FXRate;
            Decimal myValue;
            int myQty;
            int ExistingOrder;
            Decimal myPrice;
            String myTicker;
            Decimal myFund_Amount = 0;
            Decimal myTotal_Amount = 0;
            Boolean FoundLong = false;
            Boolean FoundShort = false;
            Char[] mySeperatorRow = { ',' };
            String[] AllFunds;
            int myIncr_Sum = 0;
            String myFund = "";

            SystemLibrary.DebugLine("dg_PortfolioTranspose_CellEndEdit() - START");

            myTicker = dg_PortfolioTranspose["BBG_Ticker", e.RowIndex].Value.ToString();
            POS_Mult_Factor = SystemLibrary.ToDecimal(dg_PortfolioTranspose["Pos_Mult_Factor", e.RowIndex].Value);
            if (POS_Mult_Factor <= 0)
            {
                POS_Mult_Factor = 1;
                //dg_Port["POS_Mult_Factor", e.RowIndex].Value = 1;
            }

            Round_Lot_Size = Convert.ToInt32(SystemLibrary.ToDecimal(dg_PortfolioTranspose["Round_Lot_Size", e.RowIndex].Value));
            if (Round_Lot_Size <= 0)
                Round_Lot_Size = 1;
            FXRate = SystemLibrary.ToDecimal(dg_PortfolioTranspose["FXRate", e.RowIndex].Value);
            /*
            if (FXRate <= 0)
            {
                FXRate = 1;
                dg_Port["FXRate", e.RowIndex].Value = 1;
            }
            */

            // What column is changing
            String myColName = dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.ToString();
            if (myColName != "BBG_Ticker")
            {
                int myPos = myColName.IndexOf('_');
                myFund = "";
                if (myPos > 0)
                {
                    myFund = myColName.Substring(myPos + 1);
                    myColName = myColName.Substring(0, myPos + 1); // Deliberately leave the _ in place so I can differentiate from pure columns
                }
            }
            switch (myColName)
            {
                case "ModelWeight":
                    // Cant switch short/Long or Long/Short - Make sure This Quantity_incr < - Quantity
                    // - If the individual funds are both L & S, then dont allow this. NB: Zero is neutral
                    AllFunds = dg_PortfolioTranspose.Columns["BBG_Ticker"].Tag.ToString().Split(mySeperatorRow, StringSplitOptions.RemoveEmptyEntries);
                    if (AllFunds.Length == 0)
                    {
                        MessageBox.Show("No funds available.",
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                        LastValue = null;
                        toolStripMenuItem1.Enabled = true;
                        toolStripMenuItem1.Visible = true;
                        inEditMode = false;
                        return;
                    }

                    for (Int32 af = 0; af < AllFunds.Length; af++)
                    {
                        String[] myFundIDFundAmount = AllFunds[af].Split('\t');
                        if (myFundIDFundAmount.Length == 2)
                        {
                            Decimal Value = SystemLibrary.ToDecimal(dg_PortfolioTranspose.Rows[e.RowIndex].Cells["Weight_" + myFundIDFundAmount[0]].Value);
                            if (Value > 0)
                                FoundLong = true;
                            else if (Value < 0)
                                FoundShort = true;
                        }
                    }

                    myPrice = SystemLibrary.ToDecimal(dg_PortfolioTranspose["Price", e.RowIndex].Value);
                    myValue = SystemLibrary.ToDecimal(dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    if (SystemLibrary.ToString(dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value).Length==0)
                    {
                        // Blank string so do nothing
                    }
                    else if (FoundLong && FoundShort)
                    {
                        MessageBox.Show("Can't switch from 'Long to Short' or 'Short to Long' in one step",
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                        LastValue = null;
                        toolStripMenuItem1.Enabled = true;
                        toolStripMenuItem1.Visible = true;
                        inEditMode = false;
                        return;
                    }
                    else if (myPrice <= 0)
                    {
                        MessageBox.Show("Sorry:  Price is not valid=" + myPrice.ToString(),
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else if (FXRate <= 0)
                    {
                        MessageBox.Show("Sorry:  FXRate is still to be updated or is not valid=" + FXRate.ToString(),
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else
                    {
                        // Convert to % as user type 20 for 20% = 0.20
                        //dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myValue / 100m; // Deals with non-numeric
                        SetValueRG(dg_PortfolioTranspose.Rows[e.RowIndex], "ModelWeight", myValue / 100m);

                        // Set all the individual
                        for (Int32 af = 0; af < AllFunds.Length; af++)
                        {
                            String[] myFundIDFundAmount = AllFunds[af].Split('\t');
                            if (myFundIDFundAmount.Length == 2)
                            {
                                myFund = myFundIDFundAmount[0];
                                myFund_Amount = SystemLibrary.ToDecimal(myFundIDFundAmount[1]);
                                ExistingOrder = Convert.ToInt32(SystemLibrary.ToDecimal(dg_PortfolioTranspose["Quantity_" + myFund, e.RowIndex].Value));
                                myQty = Convert.ToInt32(myValue / 100m * myFund_Amount / (myPrice * POS_Mult_Factor * FXRate));
                                myQty = myQty - ExistingOrder;
                                myQty = SendToBloomberg.RoundLot(ExistingOrder, myQty, Round_Lot_Size);
                                myIncr_Sum = myIncr_Sum + myQty;
                                SetValueRG(dg_PortfolioTranspose.Rows[e.RowIndex], "Incr_" + myFund, myQty);
                            }
                        }

                        // Set the Total Incr
                        SetValueRG(dg_PortfolioTranspose.Rows[e.RowIndex], "Incr", myIncr_Sum);

                        // Now calculate the other column values
                        SetCalc(dg_PortfolioTranspose["BBG_Ticker", e.RowIndex].Value.ToString(), Color.Empty);
                    }
                    LastValue = null;
                    break;
                case "Incr":
                    // Cant switch short/Long or Long/Short - Make sure This Quantity_incr < - Quantity
                    // - If the individual funds are both L & S, then dont allow this. NB: Zero is neutral
                    int myIncr = Convert.ToInt32(SystemLibrary.ToDecimal(dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value));
                    int myTotal_Quantity = 0;

                    AllFunds = dg_PortfolioTranspose.Columns["BBG_Ticker"].Tag.ToString().Split(mySeperatorRow, StringSplitOptions.RemoveEmptyEntries);
                    if (AllFunds.Length == 0)
                    {
                        MessageBox.Show("No funds available.",
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                        LastValue = null;
                        toolStripMenuItem1.Enabled = true;
                        toolStripMenuItem1.Visible = true;
                        inEditMode = false;
                        dg_PortfolioTranspose["ModelWeight", e.RowIndex].Value = DBNull.Value;
                        return;
                    }
                    if (myIncr == 0)
                    {
                        for (Int32 af = 0; af < AllFunds.Length; af++)
                        {
                            String[] myFundIDFundAmount = AllFunds[af].Split('\t');
                            if (myFundIDFundAmount.Length == 2)
                            {
                                myFund = myFundIDFundAmount[0];
                                SetValueRG(dg_PortfolioTranspose.Rows[e.RowIndex], "Incr_" + myFund, 0);
                            }
                        }
                        SetCalc(dg_PortfolioTranspose["BBG_Ticker", e.RowIndex].Value.ToString(), Color.Empty);
                        LastValue = null;
                        toolStripMenuItem1.Enabled = true;
                        toolStripMenuItem1.Visible = true;
                        inEditMode = false;
                        dg_PortfolioTranspose["ModelWeight", e.RowIndex].Value = DBNull.Value;
                        return;
                    }
                    for (Int32 af = 0; af < AllFunds.Length; af++)
                    {
                        String[] myFundIDFundAmount = AllFunds[af].Split('\t');
                        if (myFundIDFundAmount.Length == 2)
                        {
                            myTotal_Amount = myTotal_Amount + SystemLibrary.ToDecimal(myFundIDFundAmount[1]);
                            // Add the Total Quantity - ignore the existing Incr_<Fund> as that is what we are replacing
                            myTotal_Quantity = myTotal_Quantity + SystemLibrary.ToInt32(dg_PortfolioTranspose.Rows[e.RowIndex].Cells["Quantity_" + myFundIDFundAmount[0]].Value);
                            Decimal Value = SystemLibrary.ToDecimal(dg_PortfolioTranspose.Rows[e.RowIndex].Cells["Quantity_" + myFundIDFundAmount[0]].Value) +
                                            SystemLibrary.ToDecimal(dg_PortfolioTranspose.Rows[e.RowIndex].Cells["Incr_" + myFundIDFundAmount[0]].Value);
                            if (Value > 0)
                                FoundLong = true;
                            else if (Value < 0)
                                FoundShort = true;
                        }
                    }
                    
                    // See if closing all positions
                    if (myIncr == -myTotal_Quantity)
                    {
                        // Loop around an set Incr_ = Quantity_
                        for (Int32 af = 0; af < AllFunds.Length; af++)
                        {
                            String[] myFundIDFundAmount = AllFunds[af].Split('\t');
                            if (myFundIDFundAmount.Length == 2)
                            {
                                myFund = myFundIDFundAmount[0];
                                ExistingOrder = Convert.ToInt32(SystemLibrary.ToDecimal(dg_PortfolioTranspose["Quantity_" + myFund, e.RowIndex].Value));
                                myQty = -ExistingOrder;
                                SetValueRG(dg_PortfolioTranspose.Rows[e.RowIndex], "Incr_" + myFund, myQty);
                            }
                        }
                        LastValue = null;
                        toolStripMenuItem1.Enabled = true;
                        toolStripMenuItem1.Visible = true;
                        inEditMode = false;
                        dg_PortfolioTranspose["ModelWeight", e.RowIndex].Value = DBNull.Value;
                        return;
                    }
                    myIncr = SendToBloomberg.RoundLot(myTotal_Quantity, myIncr, Round_Lot_Size);

                    if (FoundLong && FoundShort)
                    {
                        MessageBox.Show("Can't switch from 'Long to Short' or 'Short to Long' in one step",
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                        LastValue = null;
                        toolStripMenuItem1.Enabled = true;
                        toolStripMenuItem1.Visible = true;
                        inEditMode = false;
                        dg_PortfolioTranspose["ModelWeight", e.RowIndex].Value = DBNull.Value;
                        return;
                    }


                    // Now Pro-rata the Qty across the funds
                    //   (Quantity_<Fund> + Incr_<Fund>) = myFund_Amount / myTotal_Amount * myTotal_Quantity
                    //   Incr_<Fund> = (myFund_Amount / myTotal_Amount * myTotal_Quantity) - Quantity_<Fund>
                    //   Identify the Largest Fund along the way, so can put residual after rounding back into that fund.
                    Decimal Largest_Amount = 0;
                    String Largest_FundID = "";
                    for (Int32 af = 0; af < AllFunds.Length; af++)
                    {
                        String[] myFundIDFundAmount = AllFunds[af].Split('\t');
                        if (myFundIDFundAmount.Length == 2)
                        {
                            myFund = myFundIDFundAmount[0];
                            myFund_Amount = SystemLibrary.ToDecimal(myFundIDFundAmount[1]);
                            if (myFund_Amount > Largest_Amount)
                            {
                                Largest_Amount = myFund_Amount;
                                Largest_FundID = myFund;
                            }
                            ExistingOrder = Convert.ToInt32(SystemLibrary.ToDecimal(dg_PortfolioTranspose["Quantity_" + myFund, e.RowIndex].Value));
                            myQty = Convert.ToInt32((myFund_Amount / myTotal_Amount * Convert.ToDecimal(myIncr + myTotal_Quantity))) - ExistingOrder;
                            if (Math.Sign(myQty) != Math.Sign(myIncr))
                                myQty = 0;
                            myQty = SendToBloomberg.RoundLot(ExistingOrder, myQty, Round_Lot_Size);
                            if (Math.Abs(myQty) > Math.Abs(myIncr))
                                myQty = myIncr;
                            myIncr_Sum = myIncr_Sum + myQty;
                            SetValueRG(dg_PortfolioTranspose.Rows[e.RowIndex], "Incr_" + myFund, myQty);
                            // Set the Background color to show a change was driven by this column
                            // Set the Background color to show a change was driven by this column
                            // TODO(1) - Need to mimic TidyTRADEColumns() for this object
                            //TidyTRADEColumns(dg_Port.Rows[e.RowIndex]);
                            //dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Goldenrod;
                        }
                    }
                    // Now allocate the remainder.
                    if (myIncr != myIncr_Sum)
                    {
                        myFund = Largest_FundID;
                        myQty = (myIncr - myIncr_Sum) + Convert.ToInt32(SystemLibrary.ToDecimal(dg_PortfolioTranspose["Incr_" + myFund, e.RowIndex].Value));
                        SetValueRG(dg_PortfolioTranspose.Rows[e.RowIndex], "Incr_" + myFund, myQty);
                    }
                    SetValueRG(dg_PortfolioTranspose.Rows[e.RowIndex], "Incr", myIncr);

                    // Now calculate column values
                    SetCalc(dg_PortfolioTranspose["BBG_Ticker", e.RowIndex].Value.ToString(), Color.Empty);
                    // TODO (1) - Doesn't fit this model???
                    //SetGrossPCT();
                    //SetHeader();
                    LastValue = null;
                    dg_PortfolioTranspose["ModelWeight", e.RowIndex].Value = DBNull.Value;
                    break;

                case @"Weight_": // Individual Weight Column, not ModelWeight
                    // TODO (1) EXPOSURE - Needs to deal with Delta
                    myValue = SystemLibrary.ToDecimal(dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    myPrice = SystemLibrary.ToDecimal(dg_PortfolioTranspose["Price", e.RowIndex].Value);
                    ExistingOrder = Convert.ToInt32(SystemLibrary.ToDecimal(dg_PortfolioTranspose["Quantity_"+myFund, e.RowIndex].Value));
                    myFund_Amount = GetFundAmount(myFund);

                    if (myPrice > 0 && FXRate > 0)
                        myQty = SendToBloomberg.RoundLot(ExistingOrder, Convert.ToInt32((myValue / FXRate / 100m * myFund_Amount) / myPrice / POS_Mult_Factor - ExistingOrder), Round_Lot_Size); //100m is %
                    else
                        myQty = 0;

                    if (myPrice <= 0)
                    {
                        MessageBox.Show("Sorry:  Price is not valid=" + myPrice.ToString(),
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else if (FXRate <= 0)
                    {
                        MessageBox.Show("Sorry:  FXRate is still to be updated or is not valid=" + FXRate.ToString(),
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else if (ExistingOrder != 0 && Math.Sign(ExistingOrder) != Math.Sign(ExistingOrder + myQty) && ExistingOrder != -myQty)
                    {
                        MessageBox.Show("Sorry: Cannot switch from 'Long to Short' or 'Short to Long' in one step\r\n" +
                                        "Please Close out Order to Zero and add a Second trade for the switch",
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else
                    {
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myValue/100m; // Deals with non-numeric
                        SetValueRG(dg_PortfolioTranspose.Rows[e.RowIndex], "Incr_"+myFund, myQty);
                        SetIncr_PortfolioTranspose(e.RowIndex);
                        // Set the Background color to show a change was driven by this column
                        // TODO(1) - Need to mimic TidTyradeColumns() for this object
                            //TidyTRADEColumns(dg_Port.Rows[e.RowIndex]);
                            //dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Goldenrod; //Color.Honeydew

                        // Now calculate column values
                        SetCalc(dg_PortfolioTranspose["BBG_Ticker", e.RowIndex].Value.ToString(), Color.Empty);
                        // TODO (1) - Doesn't fit this model???
                            //SetGrossPCT();
                            //SetHeader();
                    }
                    LastValue = null;
                    dg_PortfolioTranspose["ModelWeight", e.RowIndex].Value = DBNull.Value;
                    break;
                case "Value_":
                    // TODO (1) EXPOSURE - Needs to deal with Delta
                    myValue = SystemLibrary.ToDecimal(dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    myPrice = SystemLibrary.ToDecimal(dg_PortfolioTranspose["Price", e.RowIndex].Value);
                    ExistingOrder = Convert.ToInt32(SystemLibrary.ToDecimal(dg_PortfolioTranspose["Quantity_"+myFund, e.RowIndex].Value));
                    if (myPrice > 0 && FXRate > 0)
                        myQty = SendToBloomberg.RoundLot(ExistingOrder, Convert.ToInt32(myValue / FXRate / myPrice / POS_Mult_Factor - ExistingOrder), Round_Lot_Size);
                    else
                        myQty = 0;

                    if (myPrice <= 0)
                    {
                        MessageBox.Show("Sorry:  Price is not valid=" + myPrice.ToString(),
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else if (FXRate <= 0)
                    {
                        MessageBox.Show("Sorry:  FXRate is still to be updated or is not valid=" + FXRate.ToString(),
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else if (ExistingOrder != 0 && Math.Sign(ExistingOrder) != Math.Sign(ExistingOrder + myQty) && ExistingOrder != -myQty)
                    {
                        MessageBox.Show("Sorry: Cannot switch from 'Long to Short' or 'Short to Long' in one step\r\n" +
                                        "Please Close out Order to Zero and add a Second trade for the switch",
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else
                    {
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myValue; // Deals with non-numeric
                        SetValueRG(dg_PortfolioTranspose.Rows[e.RowIndex], "Incr_" + myFund, myQty);
                        SetIncr_PortfolioTranspose(e.RowIndex);
                        // Set the Background color to show a change was driven by this column
                        // TODO(1) - Need to mimic TidyTRADEColumns() for this object
                            //TidyTRADEColumns(dg_Port.Rows[e.RowIndex]);
                            //dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Goldenrod;

                        // Now calculate column values
                        SetCalc(dg_PortfolioTranspose["BBG_Ticker", e.RowIndex].Value.ToString(), Color.Empty);
                        // TODO (1) - Doesn't fit this model???
                        //SetGrossPCT();
                        //SetHeader();
                    }
                    LastValue = null;
                    dg_PortfolioTranspose["ModelWeight", e.RowIndex].Value = DBNull.Value;
                    break;
                case "LocalValue_":
                    // TODO (1) EXPOSURE - Needs to deal with Delta
                    myValue = SystemLibrary.ToDecimal(dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    myPrice = SystemLibrary.ToDecimal(dg_PortfolioTranspose["Price", e.RowIndex].Value);
                    ExistingOrder = Convert.ToInt32(SystemLibrary.ToDecimal(dg_PortfolioTranspose["Quantity_" + myFund, e.RowIndex].Value));
                    if (myPrice > 0 && FXRate > 0)
                        myQty = SendToBloomberg.RoundLot(ExistingOrder, Convert.ToInt32(myValue / myPrice / POS_Mult_Factor - ExistingOrder), Round_Lot_Size);
                    else
                        myQty = 0;

                    if (myPrice <= 0)
                    {
                        MessageBox.Show("Sorry:  Price is not valid=" + myPrice.ToString(),
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else if (FXRate <= 0)
                    {
                        MessageBox.Show("Sorry:  FXRate is still to be updated or is not valid=" + FXRate.ToString(),
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else if (ExistingOrder != 0 && Math.Sign(ExistingOrder) != Math.Sign(ExistingOrder + myQty) && ExistingOrder != -myQty)
                    {
                        MessageBox.Show("Sorry: Cannot switch from 'Long to Short' or 'Short to Long' in one step\r\n" +
                                        "Please Close out Order to Zero and add a Second trade for the switch",
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else
                    {
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myValue; // Deals with non-numeric
                        SetValueRG(dg_PortfolioTranspose.Rows[e.RowIndex], "Incr_" + myFund, myQty);
                        SetIncr_PortfolioTranspose(e.RowIndex);
                        // Set the Background color to show a change was driven by this column
                        // TODO(1) - Need to mimic TidTyradeColumns() for this object
                        //TidyTRADEColumns(dg_Port.Rows[e.RowIndex]);
                        //dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Goldenrod;

                        // Now calculate column values
                        SetCalc(dg_PortfolioTranspose["BBG_Ticker", e.RowIndex].Value.ToString(), Color.Empty);
                        // TODO (1) - Doesn't fit this model???
                        //SetGrossPCT();
                        //SetHeader();
                    }
                    LastValue = null;
                    dg_PortfolioTranspose["ModelWeight", e.RowIndex].Value = DBNull.Value;
                    break;
                case "Incr_":
                    // Cant switch short/Long or Long/Short - Make sure This Quantity_incr < - Quantity
                    myQty = Convert.ToInt32(SystemLibrary.ToDecimal(dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value));
                    ExistingOrder = Convert.ToInt32(SystemLibrary.ToDecimal(dg_PortfolioTranspose["Quantity_"+myFund    , e.RowIndex].Value));
                    if (ExistingOrder != 0 && Math.Sign(ExistingOrder) != Math.Sign(SystemLibrary.ToDecimal(ExistingOrder) + myQty) && (ExistingOrder + myQty) !=0)
                    {
                        MessageBox.Show("Can't switch from 'Long to Short' or 'Short to Long' in one step",
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    }
                    else
                    {
                        myQty = SendToBloomberg.RoundLot(ExistingOrder, myQty, Round_Lot_Size);
                        SetValueRG(dg_PortfolioTranspose.Rows[e.RowIndex], "Incr_"+myFund, myQty);
                        SetIncr_PortfolioTranspose(e.RowIndex);
                        // Set the Background color to show a change was driven by this column
                        // Set the Background color to show a change was driven by this column
                        // TODO(1) - Need to mimic TidyTRADEColumns() for this object
                        //TidyTRADEColumns(dg_Port.Rows[e.RowIndex]);
                        //dg_Port.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Goldenrod;

                        // Now calculate column values
                        SetCalc(dg_PortfolioTranspose["BBG_Ticker", e.RowIndex].Value.ToString(), Color.Empty);
                        // TODO (1) - Doesn't fit this model???
                        //SetGrossPCT();
                        //SetHeader();
                    }
                    LastValue = null;
                    dg_PortfolioTranspose["ModelWeight", e.RowIndex].Value = DBNull.Value;
                    break;
                case "BBG_Ticker":
                    // Get Ticker to correct format
                    myTicker = SystemLibrary.BBGGetTicker(dg_PortfolioTranspose.CurrentCell.Value.ToString());

                    // Set the Quantity columns for each fund to zero
                    AllFunds = dg_PortfolioTranspose.Columns["BBG_Ticker"].Tag.ToString().Split(mySeperatorRow, StringSplitOptions.RemoveEmptyEntries);
                    if (AllFunds.Length == 0)
                    {
                        MessageBox.Show("No funds available.",
                                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText + " = " + dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                        LastValue = null;
                        toolStripMenuItem1.Enabled = true;
                        toolStripMenuItem1.Visible = true;
                        inEditMode = false;
                        return;
                    }

                    // Check if this is an existing row
                    for (Int32 af = 0; af < AllFunds.Length; af++)
                    {
                        String[] myFundIDFundAmount = AllFunds[af].Split('\t');
                        if (myFundIDFundAmount.Length == 2)
                        {
                            myFund = myFundIDFundAmount[0];
                            if (SystemLibrary.ToInt32(dg_PortfolioTranspose["Quantity_" + myFund, e.RowIndex].Value) != 0)
                            {
                                dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                                LastValue = null;
                                toolStripMenuItem1.Enabled = true;
                                toolStripMenuItem1.Visible = true;
                                inEditMode = false;
                                return;
                            }
                        }
                    }

                    
                    // CFR 20120201 dg_PortfolioTranspose.CurrentCell.Value = myTicker;
                    // Make sure Ticker is represented in Bloomberg fashion
                    dg_PortfolioTranspose.CurrentCell.Value = SystemLibrary.BBGGetTicker(dg_PortfolioTranspose.CurrentCell.Value.ToString());
                    // Force the data to the DataTable by Changing Focus. (Can this be done smarter?)   
                    lb_Fund.Focus();
                    dg_PortfolioTranspose.Focus();
                    
                    String NewTicker = dg_PortfolioTranspose.CurrentCell.Value.ToString();


                    // For speed
                    // - See if we already have this ticker in live data
                    DataRow[] dr = dt_PortfolioTranspose.Select("BBG_Ticker='"+NewTicker+"'");
                    if (dr.Length > 0)
                    {
                        dg_PortfolioTranspose["Price", e.RowIndex].Value = dr[0]["Price"];
                        dg_PortfolioTranspose["Round_Lot_Size", e.RowIndex].Value = dr[0]["Round_Lot_Size"];
                        dg_PortfolioTranspose["Pos_Mult_Factor", e.RowIndex].Value = dr[0]["Pos_Mult_Factor"];
                        dg_PortfolioTranspose["crncy", e.RowIndex].Value = dr[0]["crncy"];
                        dg_PortfolioTranspose["FXRate", e.RowIndex].Value = dr[0]["FXRate"];
                        dg_PortfolioTranspose["Country_Full_Name", e.RowIndex].Value = dr[0]["Country_Full_Name"];
                    }
                    else
                    {
                        // Go to the database and get as much as can
                        String mySql = "Select BBG_Ticker as Ticker, Security_Name, crncy, Pos_Mult_Factor, Round_Lot_Size, Undl_Ticker, Undl_Currency, Delta, " +
                                       "       Country_Full_Name, Sector, Industry_Group, Industry_SubGroup, Beta, ID_BB_COMPANY, ID_BB_UNIQUE " +
                                       "From Securities " +
                                       "Where BBG_Ticker = '" + NewTicker + "' ";
                        DataTable dt_new = SystemLibrary.SQLSelectToDataTable(mySql);

                        if (dt_new.Rows.Count == 1)
                        {
                            dg_Port["Round_Lot_Size", e.RowIndex].Value = dt_new.Rows[0]["Round_Lot_Size"];
                            dg_Port["Pos_Mult_Factor", e.RowIndex].Value = dt_new.Rows[0]["Pos_Mult_Factor"];
                            dg_Port["crncy", e.RowIndex].Value = dt_new.Rows[0]["crncy"];
                            dg_Port["Country_Full_Name", e.RowIndex].Value = dt_new.Rows[0]["Country_Full_Name"];
                        }
                        else
                        {
                            // Set up the base columns
                            dg_PortfolioTranspose["Pos_Mult_Factor", e.RowIndex].Value = 1;
                            dg_PortfolioTranspose["Round_Lot_Size", e.RowIndex].Value = 1;
                        }
                        dg_PortfolioTranspose["FXRate", e.RowIndex].Value = 0;
                    }
                  
                    for (Int32 af = 0; af < AllFunds.Length; af++)
                    {
                        String[] myFundIDFundAmount = AllFunds[af].Split('\t');
                        if (myFundIDFundAmount.Length == 2)
                        {
                            myFund = myFundIDFundAmount[0];
                            SetValueRG(dg_PortfolioTranspose.Rows[e.RowIndex], "Quantity_" + myFund, 0);
                        }
                    }

                    // Add the Ticker to the "TRADE" tab, so it invokes all the structure needed for the database
                    AddTickers(myTicker,true,false);
                    LastValue = null;
                    break;
                default:
                    // Dont allow column change
                    dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    LastValue = null;
                    break;
            }
            LastValue = null;
            toolStripMenuItem1.Enabled = true;
            toolStripMenuItem1.Visible = true;
            inEditMode = false;

            SystemLibrary.DebugLine("dg_PortfolioTranspose_CellEndEdit() - END");
            //Console.WriteLine("dt_PortfolioTranspose.Rows[10][0]="+dt_PortfolioTranspose.Rows[10][0].ToString());
        } //dg_PortfolioTranspose_CellEndEdit()

        private void dg_PortfolioTranspose_Sorted(object sender, EventArgs e)
        {
            // When a Sort occurs, the Datagrid looses colour formating, so reset
            try
            {
                this.SuspendLayout();
                FormatPortfolioTranspose();
                SetCalc();
            }
            catch { }
            this.ResumeLayout(true);

        } //dg_PortfolioTranspose_Sorted()

        private void dg_PortfolioTranspose_DragDrop(object sender, DragEventArgs e)
        {
            // Local Variables

            if (inDragMode)
            {
                // Dont want this data as is from a Drag "from" dg_Port
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                AddTickersPortfolioTranspose(e.Data.GetData(DataFormats.Text).ToString());
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                String[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (fileNames.Length > 0)
                {
                    AddTickersPortfolioTranspose(File.ReadAllText(fileNames[0]));
                }
            }
        } //dg_PortfolioTranspose_DragDrop()

        private void dg_PortfolioTranspose_DragEnter(object sender, DragEventArgs e)
        {
            // Set up for Drag Drop
            if (e.Data.GetDataPresent(DataFormats.FileDrop) ||
                e.Data.GetDataPresent(DataFormats.Text)
                )
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;

        } //dg_PortfolioTranspose_DragEnter()

        private void dg_PortfolioTranspose_KeyUp(object sender, KeyEventArgs e)
        {
            // Local Variables

            // Deal with Ctrl-V            
            if (e.Control == true && e.KeyValue == (int)Keys.V)
            {
                // Takes a Tab seperated or CR/LF seperated set of data
                IDataObject inClipboard = Clipboard.GetDataObject();

                AddTickersPortfolioTranspose(inClipboard.GetData(DataFormats.Text).ToString());
            }

        } //dg_PortfolioTranspose_KeyUp()

        private void dg_PortfolioTranspose_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            FixDataGridHeight();

            // Show the Bloomberg popup menu
            try
            {
                if (e.Button == MouseButtons.Right &&
                    dg_PortfolioTranspose.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.ToString() == "BBG_Ticker"
                    )
                {
                    String Ticker = SystemLibrary.ToString(dg_PortfolioTranspose.Rows[e.RowIndex].Cells["BBG_Ticker"].Value);

                    if (Ticker.Length > 0)
                    {
                        SystemLibrary.BBGShowMenu(FundID, PortfolioID, Ticker, Portfolio_Name, SystemLibrary.BBGRelativeTicker(Ticker), this.Location.X + CXLocation, this.Location.Y + CYLocation);
                    }
                }
            }
            catch { }

        } //dg_PortfolioTranspose_CellMouseClick()

        private void bt_EMSX_Click(object sender, EventArgs e)
        {
            // Fire up EMSX
            SystemLibrary.BBGBloombergCommand(1, "EMSX<go>");

        } //bt_EMSX_Click()

   
        private void pb_Refresh_Click(object sender, EventArgs e)
        {
            // Local Variables
            String myFundID = FundID.ToString();
            String myPortfolioID = PortfolioID.ToString();


            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            // Reduce the number of calls to reload the portfolio
            inStartUp = true;

            try
            {
                // Force a Position refresh
                SystemLibrary.SQLExecute("Exec sp_Update_Positions 'Y' ");

                // Force a Full Action Tab Refresh
                if (tabControl_Port.SelectedTab.Text.StartsWith("ACTION"))
                {
                    SystemLibrary.SQLExecute("Exec sp_ActionsNeeded 200, 'N'");
                    LoadActionTab(false);
                }

                // Reload all the securities to be updated back to the Securities table
                SetUpLast_Price_DataTable();

                // Reload dt_Securities
                SetUpSecurities_DataTable();

                // Get the latest fund sizes.
                LoadPortfolioGroup();

                // Need to find FundID in cb_Fund && PortfolioID in cb_Portfo
                DataRow[] dr_FindP = dt_Portfolio.Select("PortfolioId=" + myPortfolioID);
                if (dr_FindP.Length > 0)
                    cb_Portfolio.SelectedValue = PortfolioID;
                // Now do Fund
                LoadFund(PortfolioID);
                DataRow[] dr_Find = dt_Fund.Select("FundID=" + myFundID);
                if (dr_Find.Length > 0)
                    cb_Fund.SelectedValue = FundID;
                cb_Portfolio_SelectionChangeCommitted(null, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("pb_Refresh_Click(): " + SystemLibrary.GetFullErrorMessage(ex));
            }

            // Reduce the number of calls to reload the portfolio
            inStartUp = false;
            cb_Fund_SelectionChangeCommitted(null, null);

            // ReConnect EMSX
            if (EMSX_API != null)
            {
                if (EMSX_API.Connected)
                {
                    EMSX_API.EMSX_APIDisconnect();
                }
                EMSX_API.EMSX_APIConnect();
            }

            Cursor.Current = Cursors.Default;

        } //pb_Refresh_Click()

        private void cb_PreMarketPrice_CheckedChanged(object sender, EventArgs e)
        {
            UseTheo_Price = cb_PreMarketPrice.Checked;
            bt_Refresh_Click(null, null);

        } //cb_PreMarketPrice_CheckedChanged()

        private void dg_Port_MouseUp(object sender, MouseEventArgs e)
        {
            inDragMode = false;
        }

        private void dg_Port_MouseDown(object sender, MouseEventArgs e)
        {
            // Local Variables
            String myData = "";

            CXLocation = dg_Port.Location.X + e.Location.X + 5;
            CYLocation = dg_Port.Location.Y + e.Location.Y;

            FixDataGridHeight();


            // Show the Bloomberg popup menu
            try
            {
                 DataGridView.HitTestInfo info = dg_Port.HitTest(e.X, e.Y);
                if (e.Button == MouseButtons.Right && info.RowIndex > -1 && info.ColumnIndex > -1)
                {
                    if (dg_Port.Rows[info.RowIndex].Cells[info.ColumnIndex].OwningColumn.Name.ToString() == "Ticker")
                    {
                        String Ticker = SystemLibrary.ToString(dg_Port.Rows[info.RowIndex].Cells["Ticker"].Value);

                        if (Ticker.Length > 0)
                        {
                            SystemLibrary.BBGShowMenu(FundID, PortfolioID, Ticker, Portfolio_Name, SystemLibrary.BBGRelativeTicker(Ticker), this.Location.X + CXLocation, this.Location.Y + CYLocation);
                        }
                    }
                    else if (dg_Port.Rows[info.RowIndex].Cells[info.ColumnIndex].OwningColumn.Name.ToString().StartsWith("Q"))
                    {
                        // Quantity Column
                        ShowFillStatus fs = new ShowFillStatus();
                        SystemLibrary.FormExists(fs, true);

                        fs.FromParent(this, FundID, PortfolioID, cb_Portfolio.Text + " / " +cb_Fund.Text, SystemLibrary.ToString(dg_Port.Rows[info.RowIndex].Cells["Ticker"].Value), SystemLibrary.ToDecimal(dg_Port.Rows[info.RowIndex].Cells["Price"].Value));
                        fs.Show();
                    }
                }
                else
                {
                    if (info.RowIndex >= 0)
                    {
                        myData = SystemLibrary.ToString(dg_Port.Rows[info.RowIndex].Cells["Ticker"].Value);
                        if (myData.Length > 0)
                        {
                            inDragMode = true;
                            dg_Port.DoDragDrop(myData, DragDropEffects.Copy);
                        }
                    }
                }
            }
            catch { }


        } //dg_Port_MouseUp()

        private void dg_Port_MouseEnter(object sender, EventArgs e)
        {
            inDragMode = false;
        } //dg_Port_MouseEnter()

        private void bt_Print_Click(object sender, EventArgs e)
        {
            // Local Variables
            String OrigText;

            // Ensure a Date/Time is on the printout by replacing the Top Menu text.
            OrigText = toolStripMenuItem1.Text;
            toolStripMenuItem1.Text = DateTime.Now.ToString();
            Application.DoEvents(); 
            SystemLibrary.PrintScreen(this);
            Application.DoEvents(); // PrintScreen relies on Events, so allow it to run before setting the Menustrip back

            // Put the menu back together
            toolStripMenuItem1.Text = OrigText;
        } //bt_Print_Click()

        private void pb_Beta_Click(object sender, EventArgs e)
        {
            String myFilePath = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('PortfolioBeta')");
            if (File.Exists(myFilePath))
                System.Diagnostics.Process.Start(myFilePath, "/r ");
            else
                MessageBox.Show("Portfolio Beta - Cannot find file\r\n\r\n" + myFilePath, "Beta");
        } //pb_Beta_Click()

        private void cb_ShowFutureHeaderLine_CheckedChanged(object sender, EventArgs e)
        {
            // hourglass cursor
            int myHeight = dg_Header.Rows[0].Height;
            Cursor.Current = Cursors.WaitCursor;

            ShowFutureHeaderLine = cb_ShowFutureHeaderLine.Checked;
            dg_Header.Rows[2].Visible = ShowFutureHeaderLine;
            this.SuspendLayout();
            if (ShowFutureHeaderLine)
            {
                // Move everything down 1 row
                dg_Port.Top = dg_Port.Top + myHeight;
                dg_Action.Top = dg_Action.Top + myHeight;
                dg_Header.Height = dg_Header.Height + myHeight;
                tabControl_Port.Top = tabControl_Port.Top + myHeight;
                gb_Align.Top = gb_Align.Top + myHeight;
                dg_PortfolioTranspose.Top = dg_PortfolioTranspose.Top + myHeight;

                dg_Port.Height = dg_Port.Height - myHeight;
                dg_Action.Height = dg_Action.Height - myHeight;
                dg_PortfolioTranspose.Height = dg_PortfolioTranspose.Height - myHeight;
            }
            else
            {
                // Move everyting up 1 row
                dg_Port.Top = dg_Port.Top - myHeight;
                dg_Action.Top = dg_Action.Top - myHeight;
                dg_Header.Height = dg_Header.Height - myHeight;
                tabControl_Port.Top = tabControl_Port.Top - myHeight;
                gb_Align.Top = gb_Align.Top - myHeight;
                dg_PortfolioTranspose.Top = dg_PortfolioTranspose.Top - myHeight;

                dg_Port.Height = dg_Port.Height + myHeight;
                dg_Action.Height = dg_Action.Height + myHeight;
                dg_PortfolioTranspose.Height = dg_PortfolioTranspose.Height + myHeight;
            }
            this.ResumeLayout(true);

            // Force a recalc at all levels
            cb_Portfolio_SelectionChangeCommitted(null, null);

            Cursor.Current = Cursors.Default;

        } //cb_ShowFutureHeaderLine_CheckedChanged()

        /*
         * Built to see if it alters the way EMSX_API interacts with this form
         */
        public delegate String SQLSelectStringCallback(String mySql);
        public String SQLSelectString(String mySql)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                SQLSelectStringCallback cb = new SQLSelectStringCallback(SQLSelectString);
                return ((String)this.Invoke(cb, new object[] { mySql }));
            }
            else
            {
                return(SystemLibrary.SQLSelectString(mySql));
            }
        } //SQLSelectString()

        public delegate int SQLExecuteCallback(String mySql);
        public int SQLExecute(String mySql)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                SQLExecuteCallback cb = new SQLExecuteCallback(SQLExecute);
                return ((int)this.Invoke(cb, new object[] { mySql }));
            }
            else
            {
                /*
                SystemLibrary.SetDebugLevel(4);
                SystemLibrary.DebugLine(mySql);
                SystemLibrary.SetDebugLevel(0);
                 */
                return (SystemLibrary.SQLExecute(mySql));
            }
        } //SQLExecute()

        public delegate DataTable SQLSelectToDataTableCallback(String mySql);
        public DataTable SQLSelectToDataTable(String mySql)
        {
            if (this.InvokeRequired)
            {
                // Is from a different thread.
                SQLSelectToDataTableCallback cb = new SQLSelectToDataTableCallback(SQLSelectToDataTable);
                return ((DataTable)this.Invoke(cb, new object[] { mySql }));
            }
            else
            {
                /*
                SystemLibrary.SetDebugLevel(4);
                SystemLibrary.DebugLine(mySql);
                SystemLibrary.SetDebugLevel(0);
                 */
                return (SystemLibrary.SQLSelectToDataTable(mySql));
            }
        } //SQLSelectToDataTable()

        private void dg_PortfolioTranspose_MouseUp(object sender, MouseEventArgs e)
        {
            inDragMode = false;

        } //dg_PortfolioTranspose_MouseUp()

        private void dg_Port_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        } //dg_Port_CellPainting()

        private void dg_Port_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (((System.Windows.Forms.DataGridView)(sender)).Columns[e.ColumnIndex].Name != "Price")
                Generic_CellFormatting(sender, e);
        } // dg_Port_CellFormatting()

        private void dg_Header_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Generic_CellFormatting(sender, e);
        } // dg_Header_CellFormatting()

        private void dg_PortfolioTranspose_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (((System.Windows.Forms.DataGridView)(sender)).Columns[e.ColumnIndex].Name != "Price")
                Generic_CellFormatting(sender, e);
        } // dg_PortfolioTranspose_CellFormatting()

        private void Generic_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                try
                {
                    /*
                     * NOTE to Programmer:
                     * If you want the colours to work on columns not attached to a datatable then
                     * make sure you set program the dg_XXX.Columns[<ColName>].ValueType = typeof(Decimal);
                     */
                    String DataTypeName = "";
                    if (((System.Windows.Forms.DataGridView)(sender)).Columns[e.ColumnIndex].ValueType == null)
                        DataTypeName = "System.String"; 
                    else
                        DataTypeName = SystemLibrary.ToString(((System.Windows.Forms.DataGridView)(sender)).Columns[e.ColumnIndex].ValueType.FullName);
                    switch (DataTypeName)
                    {
                        case "System.DateTime":
                        case "System.String":
                        case "System.Char":
                            break;
                        default:
                            object myTestValue = ((System.Windows.Forms.DataGridView)(sender)).Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                            if (!(myTestValue == null || myTestValue == DBNull.Value))
                            {
                                Decimal myValue = Convert.ToDecimal(myTestValue);
                                if ((Decimal)myValue < Decimal.Zero)
                                {
                                    if (e.CellStyle.ForeColor != Color.Red)
                                        e.CellStyle.ForeColor = Color.Red;
                                }
                                else if ((Decimal)myValue > Decimal.Zero)
                                {
                                    if (e.CellStyle.ForeColor != Color.Green)
                                        e.CellStyle.ForeColor = Color.Green;
                                }
                                else
                                {
                                    if (e.CellStyle.ForeColor != Color.Black)
                                        e.CellStyle.ForeColor = Color.Black;
                                }
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("For Programmer: Generic_CellFormatting(" + ((System.Windows.Forms.DataGridView)(sender)).Name + "," + ((System.Windows.Forms.DataGridView)(sender)).Columns[e.ColumnIndex].Name + "):"+SystemLibrary.GetFullErrorMessage(ex));
                }
            }

        } //Generic_CellFormatting()

   
    }

}

