using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bloomberglp.Blpapi;
using System.Threading;
using System.Windows.Forms;
using System.Data;
using System.Collections;


namespace T1MultiAsset
{
    public class Bloomberg_Realtime
    {
        // Main thread sets this event to stop worker thread:
        ManualResetEvent m_EventStop;
        // Worker thread sets this event when it is stopped:
        ManualResetEvent m_EventStopped;
        static DataTable dt_MktFields = new DataTable();
        public static Boolean Connected = false;

        /// An event that is signalled when we receieve an api sequence message from EMSX
        /// </summary>
        static private AutoResetEvent sm_sessionStatus = new AutoResetEvent(false);
        static private AutoResetEvent sm_serviceStatus = new AutoResetEvent(false);
        static private AutoResetEvent sm_subscriptionStatus = new AutoResetEvent(false);
        static private AutoResetEvent sm_apiSeqReceived = new AutoResetEvent(false);

        static String mktFields = "LAST_PRICE,THEO_PRICE";
        static String refFields = "PX_POS_MULT_FACTOR,PX_ROUND_LOT_SIZE,PREV_CLOSE_VAL,UNDERLYING_SECURITY_DES,UNDL_CURRENCY,UNDL_SPOT_TICKER," +
                           "SECURITY_NAME,CRNCY,ID_BB_COMPANY,ID_BB_UNIQUE,ID_BB_GLOBAL,COUNTRY_FULL_NAME,INDUSTRY_SECTOR,INDUSTRY_GROUP,INDUSTRY_SUBGROUP,ID_CUSIP,ID_ISIN,ID_SEDOL1,SECURITY_TYP,SECURITY_TYP2,EXCH_CODE" +
                           ",MARKET_SECTOR_DES,FUTURES_CATEGORY,NAME";
        //static String refFieldsIndex = ",MARKET_SECTOR_DES,FUTURES_CATEGORY"; // INDUSTRY_SECTOR,INDUSTRY_GROUP
        //static String refFieldsComdty = ",MARKET_SECTOR_DES,FUTURES_CATEGORY";
        static private Boolean isClosing = false;
        static private SessionOptions d_sessionOptions;
        static Service mktService;
        static Service refService;


        // Reference to main form used to make syncronous user interface calls:
        public static Form1 m_form;

        public static Session d_session;
        private static Boolean Bloomberg_SlowConsumerWarning = false;
        private static int Bloomberg_Option_Interval = 0;

        public Bloomberg_Realtime(ManualResetEvent eventStop, ManualResetEvent eventStopped, Form1 form)
        {
            m_EventStop = eventStop;
            m_EventStopped = eventStopped;
            m_form = form;
        }

        public void SetUpBloomberg_Realtime()
        {
            LocalDebugLine("SetUpBloomberg_Realtime() - Start");
            // Create the dt_MktFields structure
            dt_MktFields.Columns.Add("BBG_Ticker");

            String[] mktFieldsElements = mktFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < mktFieldsElements.Length; j++)
            {
                DataColumn myCol = dt_MktFields.Columns.Add(mktFieldsElements[j]);
                myCol.DefaultValue = "";
            }

            
            try
            {
                while (!m_EventStop.WaitOne(0, true) && !m_form.IsDisposed)
                {
                    Thread.Sleep(1000);
                    Application.DoEvents();
                }

                Bloomberg_RealtimeDisconnect();

            }
            catch (Exception e)
            {
                SystemLibrary.DebugLine(e);
            }

            // Inform Parent Code that the Thread has stopped
            m_EventStopped.Set();
            LocalDebugLine("SetUpBloomberg_Realtime() - End");

        } //SetUpBloomberg_Realtime()

        public static void Set_isClosing(Boolean myVal)
        {
            isClosing = myVal;
        }

        public void Bloomberg_RealtimeConnect()
        {
            // Local Variables
            DataTable indt_Port = new DataTable();
            DataTable indt_Last_Price = new DataTable();
            String inFund_Crncy = "";
            String inBPS_Index_Ticker = "";

            // Go back to m_form and get this data
            m_form.GetParameters(ref indt_Port, ref indt_Last_Price, ref inFund_Crncy, ref inBPS_Index_Ticker);
            Bloomberg_RealtimeConnect(indt_Port, indt_Last_Price, inFund_Crncy, inBPS_Index_Ticker);
        }

        public void Bloomberg_RealtimeConnect(DataTable indt_Port, DataTable indt_Last_Price, String inFund_Crncy, String inBPS_Index_Ticker)
        {
            if (!m_form.isBloombergUser1)
                return;
            try
            {
                // Local Variables
                List<string> TickerCheck = new List<string>();
                Subscription subscription;
                String sub_options = "";
                DataTable dt_Port = indt_Port.Copy();
                DataTable dt_Last_Price = indt_Last_Price.Copy();
                String myTicker = "";
                String[] myFields = new String[1];
                String[] myItems = new String[1];
                int myColumns = dt_MktFields.Columns.Count;


                // Check for "Bloomberg_SlowConsumerWarning" 
                if (Bloomberg_Option_Interval > 0)
                    sub_options = "interval=" + Bloomberg_Option_Interval.ToString().Trim();

                // Special Underlying Securities - used to stop them beiong called over & over
                m_form.LoadedUndelying.Clear();

                /*
                 * For speed, try to see if any of the tickers in the dt_Port are also in dt_MktFields
                 */
                for (int i = 0; i < dt_MktFields.Rows.Count; i++)
                {
                    myTicker = dt_MktFields.Rows[i]["BBG_Ticker"].ToString();
                    if (dt_Port.Select("Ticker='" + myTicker + "'").Length > 0)
                    {
                        Array.Resize(ref myFields, myColumns-1);
                        Array.Resize(ref myItems, myColumns-1);
                        for (int j = 1; j < myColumns; j++)
                        {
                            myFields[j - 1] = dt_MktFields.Columns[j].ColumnName;
                            myItems[j - 1] = dt_MktFields.Rows[i][j].ToString();
                            //Console.WriteLine("{0} - {1}={2}", myTicker, myFields[j - 1], myItems[j - 1]);
                        }
                        m_form.SetValue(myTicker, myFields, myItems);
                    }
                }
                dt_MktFields.Rows.Clear();
                // Now update all the calcs
                m_form.SetCalc_From_TickerList();

                if (!m_form.isBloombergUser1 || d_session != null)
                    Bloomberg_RealtimeDisconnect();

                if (d_session == null)
                {
                    d_sessionOptions = new SessionOptions();
                    d_sessionOptions.ServerHost = "localhost";
                    d_sessionOptions.ServerPort = 8194;
                    d_sessionOptions.MaxEventQueueSize = 2000000;  // Past experience say make this larger than the default
                    //d_session = new Session(d_sessionOptions, new EventHandler(ProcessEvent));
                    d_sessionOptions.ConnectTimeout = 100;
                    d_session = new Session(d_sessionOptions, ProcessEvent);
                }

                // Now start the session asynchronously
                //Bloomberglp.Blpapi.SeatType mySeat = new SeatType();
                //mySeat.ToString
                
                LocalDebugLine("Bloomberg_RealtimeConnect: Pre:d_session.Start()");
                if (!m_form.isBloombergUser1 || !d_session.Start())
                {
                    d_session = null;
                    LocalDebugLine("Bloomberg Session Failed");
                }
                else
                {
                    // Wait for the session to be established
                    sm_sessionStatus.WaitOne();
                    // Next connect to the Market Data service
                    d_session.OpenServiceAsync("//blp/mktdata");
                    // Wait for the service to be connected
                    sm_serviceStatus.WaitOne();
                    sm_serviceStatus.Reset();
                    d_session.OpenServiceAsync("//blp/refdata");
                    // Wait for the service to be connected
                    sm_serviceStatus.WaitOne();

                    // Now get the a Blp Api service object representing EMSX API Service
                    mktService = d_session.GetService("//blp/mktdata");
                    refService = d_session.GetService("//blp/refdata");


                    // Reference data request preparation
                    Request refDataRequest; refDataRequest = refService.CreateRequest("ReferenceDataRequest");
                    Element securities = refDataRequest.GetElement("securities");
                    Element fields = refDataRequest.GetElement("fields");

                    String[] refFieldsElements = refFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < refFieldsElements.Length; i++)
                        fields.AppendValue(refFieldsElements[i]);



                    List<Subscription> subscriptions = new List<Subscription>();

                    // Since there can be multiple subscriptions on a session, 
                    // a correlation id serves to distinguish between messages 
                    // for various subscriptions
                    for (int i = 0; i < dt_Port.Rows.Count; i++)
                    {
                        String TickerID = SystemLibrary.ToString(dt_Port.Rows[i]["Ticker"]);
                        Boolean IsAggregate = SystemLibrary.YN_To_Bool(SystemLibrary.ToString(dt_Port.Rows[i]["IsAggregate"]));
                        if (TickerID.Length > 0 && !IsAggregate && !TickerCheck.Contains(TickerID))
                        {
                            TickerCheck.Add(TickerID);
                            securities.AppendValue(TickerID); // For Ref Data
                            if (TickerID.ToUpper().EndsWith(" CURNCY"))
                                subscription = new Subscription(TickerID, mktFields, "interval=1");  // Slow down high frequency fields like Currency
                            else
                                subscription = new Subscription(TickerID, mktFields, sub_options);
                            subscriptions.Add(subscription);

                            // Check for Currency First (Otherwise a Ticker will cause the FX call later)
                            m_form.CheckFX(dt_Port.Rows[i]["crncy"].ToString());
                            if (dt_Port.Rows[i]["crncy"].ToString() != inFund_Crncy && dt_Port.Rows[i]["crncy"].ToString() != "")
                            {
                                TickerID = dt_Port.Rows[i]["crncy"].ToString() + inFund_Crncy + " Curncy";
                                TickerCheck.Add(TickerID);
                                securities.AppendValue(TickerID); // For Ref Data
                                subscription = new Subscription(TickerID, mktFields, "interval=1");  // Slow down high frequency fields like Currency
                                subscriptions.Add(subscription);
                            }

                            // Also request details of Undl_Ticker if it exists.
                            if (dt_Port.Rows[i]["Undl_Ticker"].ToString().Length > 0)
                            {
                                TickerID = dt_Port.Rows[i]["Undl_Ticker"].ToString();
                                TickerCheck.Add(TickerID);
                                securities.AppendValue(TickerID); // For Ref Data
                                if (TickerID.ToUpper().EndsWith(" CURNCY"))
                                    subscription = new Subscription(TickerID, mktFields, "interval=1");  // Slow down high frequency fields like Currency
                                else
                                    subscription = new Subscription(TickerID, mktFields, sub_options);
                                subscriptions.Add(subscription);
                            }
                        }
                    }

                    // Add extra Ticker from dt_Last_Price
                    for (int i = 0; i < dt_Last_Price.Rows.Count; i++)
                    {
                        String TickerID = SystemLibrary.ToString(dt_Last_Price.Rows[i]["Ticker"]);
                        if (TickerID.Length > 0 && !TickerCheck.Contains(TickerID))
                        {
                            TickerCheck.Add(TickerID);
                            securities.AppendValue(TickerID); // For Ref Data
                            if (TickerID.ToUpper().EndsWith(" CURNCY"))
                                subscription = new Subscription(TickerID, mktFields, "interval=1");  // Slow down high frequency fields like Currency
                            else
                                subscription = new Subscription(TickerID, mktFields, sub_options);
                            subscriptions.Add(subscription);
                        }
                    }

                    // Load the Index Ticker
                    if (inBPS_Index_Ticker.Length > 0)
                    {
                        TickerCheck.Add(inBPS_Index_Ticker);
                        securities.AppendValue(inBPS_Index_Ticker); // For Ref Data
                        subscription = new Subscription(inBPS_Index_Ticker, mktFields, "interval=1");  // Slow down high frequency fields like Index
                        subscriptions.Add(subscription);
                    }

                    /*
                     * Market data request
                     */
                    d_session.Subscribe(subscriptions);

                    /*
                     * Reference data request
                     */
                    CorrelationID cID = new CorrelationID(1);
                    d_session.Cancel(cID);
                    d_session.SendRequest(refDataRequest, cID);

                }
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - " + "Bloomberg_RealtimeConnect()");

            }
            catch (Exception ex)
            {
                SystemLibrary.DebugLine("Bloomberg_RealtimeConnect:" + ex.Message);
            }
            Connected = true;
            LocalDebugLine("Bloomberg_RealtimeConnect()");

        } //Bloomberg_RealtimeConnect

        public static void Bloomberg_RealtimeDisconnect()
        {
            try
            {
                // Dispose our session
                if (d_session != null)
                {
                    /*
                     * As at 18-Dec-2012, these commands would lock up the code on a Form1.dispose?
                     */
                    d_session.Stop(Session.StopOption.ASYNC);
                    //d_session.Dispose();
                }
                d_session = null;
            }
            catch (Exception ex)
            {
                SystemLibrary.DebugLine("Bloomberg_RealtimeDisconnect:" + ex.Message);
            }

            Connected = false;
            LocalDebugLine("Bloomberg_RealtimeDisconnect() - End");

        } //Bloomberg_RealtimeDisconnect()

        public void Bloomberg_Request(String myTicker)
        {
            try
            {
                if (d_session == null)
                    Bloomberg_RealtimeConnect();

                //if (myTicker.ToUpper() == "XPU2 Index".ToUpper() || myTicker.ToUpper() == "AS51 Index".ToUpper())
                    LocalDebugLine("Bloomberg_Request(" + myTicker + ")");

                /*
                 * Market data request
                 */
                List<Subscription> subscriptions = new List<Subscription>();

                Subscription subscription = new Subscription(myTicker, mktFields);
                subscriptions.Add(subscription);
                d_session.Subscribe(subscriptions);

                /*
                 * Reference data request
                 */
                // Reference data request preparation
                Request refDataRequest = refService.CreateRequest("ReferenceDataRequest");
                Element securities = refDataRequest.GetElement("securities");
                Element fields = refDataRequest.GetElement("fields");

                String[] refFieldsElements = refFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < refFieldsElements.Length; i++)
                    fields.AppendValue(refFieldsElements[i]);
                securities.AppendValue(myTicker);
                CorrelationID cID = new CorrelationID(SystemLibrary.f_Now());
                d_session.Cancel(cID);
                d_session.SendRequest(refDataRequest, cID);
            }
            catch { }

        } //Bloomberg_Request()



        
        private static void ProcessEvent(Event evt, Session session)
        {
            try
            {
                if (isClosing)
                    return;
                // Depending on the event type - signal the appropriate event
                switch (evt.Type)
                {
                    case Event.EventType.SESSION_STATUS:
                        sm_sessionStatus.Set();
                        break;
                    case Event.EventType.SERVICE_STATUS:
                        sm_serviceStatus.Set();
                        break;
                    case Event.EventType.SUBSCRIPTION_STATUS:
                        sm_subscriptionStatus.Set();
                        break;
                    case Event.EventType.SUBSCRIPTION_DATA: //mktdata
                        //sm_apiSeqReceived.Set();
                        ProcessMktDataEvent(evt);
                        // Now update all the calcs
                        m_form.SetCalc_From_TickerList();
                        break;
                    case Event.EventType.RESPONSE: //refdata
                    case Event.EventType.PARTIAL_RESPONSE: //refdata
                        //sm_apiSeqReceived.Set();
                        ProcessRefDataEvent(evt);
                        // Now update all the calcs
                        m_form.SetCalc_From_TickerList();
                        break;
                    default:
                        // Look for Slow session messages
                        Console.WriteLine(evt.Type.ToString());
                        foreach (Bloomberglp.Blpapi.Message message in evt.GetMessages())
                        {
                            switch (message.MessageType.ToString())
                            {
                                case "SlowConsumerWarning":
                                    // Temporarily Reconnect with a slower interval
                                    Bloomberg_SlowConsumerWarning = true;
                                    Bloomberg_RealtimeDisconnect();
                                    Bloomberg_Option_Interval = Bloomberg_Option_Interval + 3;
                                    Console.WriteLine("Bloomberg Realtime: Bloomberg_SlowConsumerWarning Setting Interval to " + Bloomberg_Option_Interval.ToString() + " secs");
                                    m_form.ReconnectBRT();
                                    break;
                                case "SlowConsumerWarningCleared":
                                    // Reestablish a normal connection
                                    Bloomberg_SlowConsumerWarning = false;
                                    Console.WriteLine("Bloomberg Realtime: Bloomberg_SlowConsumerWarning Cleared");
                                    if (Bloomberg_SlowConsumerWarning)
                                    {
                                        Bloomberg_RealtimeDisconnect();
                                        Bloomberg_Option_Interval = 0;
                                        m_form.ReconnectBRT();
                                    }
                                    break;
                            }
                        }
                        break;

                }
            }
            catch { }
        } //ProcessEvent()

        private static void ProcessRefDataEvent(Event evt)
        {
            // Local Variables
            String myTicker = "";
            String[] myFields = new String[1];
            String[] myItems = new String[1];


            foreach (Bloomberglp.Blpapi.Message message in evt.GetMessages())
            {
                /*
                 * See if Field exists
                 */
                if (message.MessageType.ToString() == "ReferenceDataResponse")
                {
                    // Comes as an array of securities - securityData[]
                    //Element securityDataArr = message.GetElement(new Name("securityData"));
                    if (!message.HasElement("securityData"))
                        Console.WriteLine(message.ToString());
                    else
                    {
                        Element securityDataArr = message.GetElement("securityData");
                        for (int i = 0; i < securityDataArr.NumValues; i++)
                        {
                            // Two key blocks
                            Element securityData = securityDataArr.GetValueAsElement(i); // Can have exceptions, but not important
                            Element fieldData = securityData.GetElement("fieldData");

                            myTicker = securityData.GetElementAsString("security");
                            SystemLibrary.DebugLine(myTicker);

                            String[] refFieldsElements = refFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            Array.Resize(ref myFields, refFieldsElements.Length);
                            Array.Resize(ref myItems, refFieldsElements.Length);
                            for (int j = 0; j < refFieldsElements.Length; j++)
                            {
                                myFields[j] = refFieldsElements[j];
                                if (fieldData.HasElement(refFieldsElements[j]))
                                {
                                    switch (myFields[j])
                                    {
                                        case "MARKET_SECTOR_DES":
                                        case "FUTURES_CATEGORY":
                                            String myType = SystemLibrary.BBGDataType(myTicker).ToUpper();
                                            if (myType == "INDEX" || myType == "COMDTY" || myType == "CURNCY")
                                            {
                                                myItems[j] = fieldData.GetElementAsString(refFieldsElements[j]);
                                            }
                                            else
                                                myItems[j] = "#N/A";
                                            break;
                                        default:
                                            myItems[j] = fieldData.GetElementAsString(refFieldsElements[j]);
                                            break;
                                    }
                                }
                                else
                                {
                                    myItems[j] = "#N/A";
                                }
                                SystemLibrary.DebugLine(myFields[j] + "=" + myItems[j]);
                            }
                            //LocalDebugLine("ProcessRefData SetValue(" + myTicker + ",(" + myFields.Length + ")myFields[0]=" + myFields[0]);
                            m_form.SetValue(myTicker, myFields, myItems);
                        }
                    }
                }
                //Console.WriteLine(message.MessageType);
                //Console.WriteLine(message.CorrelationID.ToString());
                //Console.WriteLine(message.ToString());
            }

        } //ProcessRefDataEvent()

        private static void ProcessMktDataEvent(Event evt)
        {
            // Local Variables
            Boolean DataHasChanged = false;
            String myTicker = "";
            String[] myFields = new String[1];
            String[] myItems = new String[1];

            foreach (Bloomberglp.Blpapi.Message message in evt.GetMessages())
            {
                /*
                 * See if Field exists
                 */
                if (message.MessageType.ToString() == "MarketDataEvents")
                {
                    myTicker = message.TopicName;
                    DataRow[] FoundRows = dt_MktFields.Select("BBG_Ticker='" + myTicker + "'");
                    if (FoundRows.Length == 0)
                    {
                        DataRow dr = dt_MktFields.NewRow();
                        //for (int i=0;i<dr.ItemArray.Length;i++)

                        //for (int i=0;i<dr.cell
                        dr["BBG_Ticker"] = myTicker;
                        dt_MktFields.Rows.Add(dr);
                        FoundRows = dt_MktFields.Select("BBG_Ticker='" + myTicker + "'");
                    }

                    //if (myTicker.Contains("Index"))
                    //    LocalDebugLine(myTicker+"\r\n"+message.ToString());

                    SystemLibrary.DebugLine(myTicker);

                    String[] mktFieldsElements = mktFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    Array.Resize(ref myFields, mktFieldsElements.Length);
                    Array.Resize(ref myItems, mktFieldsElements.Length);
                    for (int j = 0; j < mktFieldsElements.Length; j++)
                    {
                        myFields[j] = mktFieldsElements[j];
                        if (message.HasElement(mktFieldsElements[j]))
                        {
                            if (message.GetElement(mktFieldsElements[j]).IsNull)
                                myItems[j] = "#N/A";
                            else
                            {
                                switch (myFields[j])
                                {
                                    case "MARKET_SECTOR_DES":
                                    case "FUTURES_CATEGORY":
                                        String myType = SystemLibrary.BBGDataType(myTicker).ToUpper();
                                        if (myType == "INDEX" || myType == "COMDTY" || myType == "CURNCY")
                                            myItems[j] = message.GetElementAsString(mktFieldsElements[j]);
                                        else
                                            myItems[j] = "#N/A";
                                        break;
                                    default:
                                        myItems[j] = message.GetElementAsString(mktFieldsElements[j]);
                                        // See if data has changed
                                        if (FoundRows[0][myFields[j]].ToString() != myItems[j])
                                        {
                                            try
                                            {
                                                FoundRows[0][myFields[j]] = myItems[j];
                                            }
                                            catch { }
                                            DataHasChanged = true;
                                        }
                                        break;
                                }
                            }
                        }
                        else
                        {
                            myItems[j] = "#N/A";
                        }
                        SystemLibrary.DebugLine(myFields[j] + "=" + myItems[j]);
                    }
                    if (DataHasChanged)
                    {
                        /*
                        LocalDebugLine("ProcessMktData (" + SystemLibrary.Bool_To_YN(DataHasChanged) + ")SetValue(" + myTicker + ",(" + myFields.Length +
                                        ")myFields[0]=" + myFields[0] + ",myItems[0]=" + myItems[0] +
                                        ",myFields[1]=" + myFields[1] + ",myItems[1]=" + myItems[1]);
                        */
                        m_form.SetValue(myTicker, myFields, myItems);
                        DataHasChanged = false;
                    }

                }
                //Console.WriteLine(message.MessageType);
                //Console.WriteLine(message.CorrelationID.ToString());
                //Console.WriteLine(message.ToString());
            }

        } //ProcessMktDataEvent()


        public static void LocalDebugLine(object myObj)
        {
            //if (myObj.ToString().Contains("HeartBeat") || myObj.ToString().Contains("NewEMSXOrders"))
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - " + myObj);
        }

    }
}
