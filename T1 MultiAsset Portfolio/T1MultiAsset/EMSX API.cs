using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bloomberglp.Blpapi;
using System.Threading;
using System.Windows.Forms;
using System.Data;
using System.Collections;

/*
 * Special Note:
 * This block of software has 2 ways of updating the database.
 * 1) The first was written for a local SQL Server 2005 database.
 * 2) The second was written for a remote database passing DataTables to a stored procedure. 
 *    This is not supported by SQL Server 2005. Hence the 2 ways of updating the database.
 *    It was altered to fit a remote Azure database where each SQL statement take 0.25 seconds.
 */
namespace T1MultiAsset
{
    public class EMSX_API
    {
        // Main thread sets this event to stop worker thread:
        ManualResetEvent m_EventStop;
        // Worker thread sets this event when it is stopped:
        ManualResetEvent m_EventStopped;
        String m_EMSX_Team = "";
        public static Boolean Connected = false;
        public static ChartOfAccounts OutputWindow;
        public static DataTable dt_EMSX_API = new DataTable();
        public static DataTable dt_Open_Orders;
        private static Int64 EMSX_Order_seq = Int64.MinValue;
        private static Int64 EMSX_Route_seq = Int64.MinValue;
        public static Boolean NeedResetSequenceFailed = false;
        private static Boolean NeedToRun_NewEMSXOrders = false;
        private static Boolean inStartup = false;
        private static readonly Name RESPONSE_ERROR = new Name("responseError");
        private static readonly Name EXCEPTIONS = new Name("exceptions");
        private static readonly Name FIELD_ID = new Name("fieldId");
        private static readonly Name REASON = new Name("reason");
        private static readonly Name CATEGORY = new Name("category");
        private static readonly Name DESCRIPTION = new Name("description");

        /// An event that is signalled when we receieve an api sequence message from EMSX
        /// </summary>
        static private AutoResetEvent sm_sessionStatus = new AutoResetEvent(false);
        static private AutoResetEvent sm_serviceStatus = new AutoResetEvent(false);
        static private AutoResetEvent sm_subscriptionStatus = new AutoResetEvent(false);
        static private AutoResetEvent sm_apiSeqReceived = new AutoResetEvent(false);

        // Reference to main form used to make syncronous user interface calls:
        public static Form1 m_form;
        private Boolean EMSX_API_Ready = false;
        private static Boolean isSql2005 = false;
        // Load the database with the desired timezone, which is not necessarily that of the user.
        private static String DatabaseTimeZone = TimeZoneInfo.Local.Id;

        public static Session d_session;

        public struct ProcessedStruct
        {
            public String Action;
            public Boolean DatabaseWasUpdated;
        }


        public EMSX_API(ManualResetEvent eventStop, ManualResetEvent eventStopped, Form1 form, String EMSX_Team)
        {
            m_EventStop = eventStop;
            m_EventStopped = eventStopped;
            m_form = form;
            m_EMSX_Team = EMSX_Team;
        }

        public void SetUpEMSX_API()
        {
            // Create the dt_EMSX_API structure
            dt_EMSX_API.Columns.Add("EMSX_SEQUENCE");
            dt_EMSX_API.Columns.Add("MSG_SUB_TYPE");
            dt_EMSX_API.Columns.Add("EVENT_STATUS");
            dt_EMSX_API.Columns.Add("EMSX_STATUS");

            // Order Record
            dt_EMSX_API.Columns.Add("EMSX_TICKER");
            dt_EMSX_API.Columns.Add("EMSX_EXCHANGE");
            dt_EMSX_API.Columns.Add("EMSX_SIDE");
            dt_EMSX_API.Columns.Add("EMSX_TRADER");
            dt_EMSX_API.Columns.Add("EMSX_TRAD_UUID");
            dt_EMSX_API.Columns.Add("EMSX_ARRIVAL_PRICE", System.Type.GetType("System.Decimal"));
            dt_EMSX_API.Columns.Add("EMSX_TIME_STAMP", System.Type.GetType("System.DateTime"));
            dt_EMSX_API.Columns.Add("EMSX_DATE", System.Type.GetType("System.DateTime"));
            dt_EMSX_API.Columns.Add("ROUTED", System.Type.GetType("System.Decimal"));

            // Routed Record
            dt_EMSX_API.Columns.Add("EMSX_ROUTE_ID", System.Type.GetType("System.Int32"));
            dt_EMSX_API.Columns.Add("EMSX_AMOUNT", System.Type.GetType("System.Decimal"));
            dt_EMSX_API.Columns.Add("EMSX_WORKING", System.Type.GetType("System.Decimal"));
            dt_EMSX_API.Columns.Add("EMSX_FILLED", System.Type.GetType("System.Decimal"));
            dt_EMSX_API.Columns.Add("EMSX_AVG_PRICE", System.Type.GetType("System.Decimal"));
            dt_EMSX_API.Columns.Add("EMSX_TYPE");
            dt_EMSX_API.Columns.Add("EMSX_LIMIT_PRICE", System.Type.GetType("System.Decimal"));
            dt_EMSX_API.Columns.Add("EMSX_BROKER");
            dt_EMSX_API.Columns.Add("EMSX_STRATEGY_TYPE");
            dt_EMSX_API.Columns.Add("EMSX_CUSTOM_ACCOUNT");
            dt_EMSX_API.Columns.Add("EMSX_LAST_FILL_TIME", System.Type.GetType("System.DateTime"));
            dt_EMSX_API.Columns.Add("EMSX_SETTLE_DATE", System.Type.GetType("System.DateTime"));

            try
            {
                while (!m_EventStop.WaitOne(0, true) && !m_form.IsDisposed)
                {
                    Thread.Sleep(1000);
                    Application.DoEvents();
                }

                EMSX_APIDisconnect();

            }
            catch (Exception e)
            {
                SystemLibrary.DebugLine(e);
            }

            // Inform Parent Code that the Thread has stopped
            m_EventStopped.Set();

        } //SetUpEMSX_API()

        public void EMSX_APIReady()
        {
            EMSX_API_Ready = true;
        }

        public void EMSX_APIConnect()
        {
            if (!EMSX_API_Ready)
                return;

            // For Debug reasons
            if (SystemLibrary.GetUserName() == "Colin Ritchie" && m_form.isBloombergUser)
            {
                if (OutputWindow == null)
                {
                    OutputWindow = new ChartOfAccounts();
                    OutputWindow.Show();
                }
            }            
            try
            {
                // Is this SQL Server 2005 - called before any SQL
                isSql2005 = m_form.DatabaseVersion.StartsWith("Microsoft SQL Server 2005");
                DatabaseTimeZone = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('TimeZone')");
                if (DatabaseTimeZone.Length==0)
                    DatabaseTimeZone = TimeZoneInfo.Local.Id;

                // Clean up dt_EMSX_API
                if (dt_EMSX_API.Rows.Count > 0)
                    dt_EMSX_API.Rows.Clear();
                LoadOpenOrders();



                // Reset the expected Sequences.
                EMSX_Order_seq = Int64.MinValue;
                EMSX_Route_seq = Int64.MinValue;


                // The session options determine how the connection to bbcomm is established
                SessionOptions d_sessionOptions = new SessionOptions();
                d_sessionOptions.ServerHost = "localhost";
                d_sessionOptions.ServerPort = 8194;
                d_sessionOptions.MaxEventQueueSize = 1000000;  // Past experience say make this larger than the default
                d_sessionOptions.ConnectTimeout = 100;
                d_session = new Session(d_sessionOptions, ProcessEvent);

                // Now start the session asynchronously
                SystemLibrary.DebugLine("EMSX_APIConnect: Pre:d_session.Start()");
                if (!m_form.isBloombergUser || !d_session.Start())
                    SystemLibrary.DebugLine("EMSX_APIConnect: Bloomberg Session Failed");
                else
                {
                    // Wait for the session to be established
                    sm_sessionStatus.WaitOne();

                    // Next connect to the EMSX API service
                    d_session.OpenServiceAsync("//blp/emapisvc");

                    // Wait for the service to be connected
                    sm_serviceStatus.WaitOne();

                    // Now get the a Blp Api service object representing EMSX API Service
                    Service service = d_session.GetService("//blp/emapisvc");

                    /*
                     * Build the Order Request.
                     */
                    String EMSX_Team = "";
                    if (m_EMSX_Team.Length > 0)
                        EMSX_Team = ";team=" + m_EMSX_Team;
                    
                    String topicString = "//blp/emapisvc/order" + EMSX_Team + "?fields=EMSX_SIDE,EMSX_AMOUNT,EMSX_FILLED,EMSX_AVG_PRICE,EMSX_WORKING,EMSX_TICKER,EMSX_EXCHANGE,EMSX_TIME_STAMP,EMSX_TRADER,EMSX_DATE,EMSX_ARRIVAL_PRICE,EMSX_TRAD_UUID,EMSX_STATUS";

                    // We can now create a subscription based on the topic string 
                    // and add it to the session
                    //int timeoutInMilliSeconds = 5000;
                    List<Subscription> subscriptions = new List<Subscription>();

                    // Since there can be multiple subscriptions on a session, 
                    // a correlation id serves to distinguish between messages 
                    // for various subscriptions
                    CorrelationID subscriptionId_Order = new CorrelationID(1);
                    Subscription subscription = new Subscription(topicString, subscriptionId_Order);
                    subscriptions.Add(subscription);


                    /*
                     * Build the Route Request.
                     */
                    String topicString1 = "//blp/emapisvc/route" + EMSX_Team + "?fields=EMSX_AMOUNT,EMSX_WORKING,EMSX_FILLED,EMSX_AVG_PRICE,EMSX_TYPE,EMSX_LIMIT_PRICE,EMSX_BROKER,EMSX_STRATEGY_TYPE,EMSX_CUSTOM_ACCOUNT,EMSX_LAST_FILL_TIME,EMSX_SETTLE_DATE,EMSX_ROUTE_LAST_UPDATE_TIME,EMSX_STATUS,EMSX_LAST_FILL_DATE";
                    CorrelationID subscriptionId_Route = new CorrelationID(2);
                    Subscription subscription1 = new Subscription(topicString1, subscriptionId_Route);
                    subscriptions.Add(subscription1);

                    // Calling subscribe will fetch the events that we have subscribed from EMSX
                    d_session.Subscribe(subscriptions);

                    inStartup = true;

                    // Wait for the subscription to be started
                    //sm_subscriptionStatus.WaitOne();

                    // Wait till we receive an api seq message
                    //sm_apiSeqReceived.WaitOne();
                }
            }
            catch (Exception ex)
            {
                SystemLibrary.DebugLine("EMSX_APIConnect:" + ex.Message);
            }
            Connected = true;
            SystemLibrary.DebugLine("EMSX_APIConnect()");

        } //EMSX_APIConnect

        public static void EMSX_APIDisconnect()
        {
            try
            {
                // Dispose our session
                if (d_session != null)
                {

                    d_session.Stop(Session.StopOption.ASYNC);
                    //d_session.Dispose();
                }
                d_session = null;
            }
            catch (Exception ex)
            {
                SystemLibrary.DebugLine("EMSX_APIDisconnect:" + ex.Message);
            }

            Connected = false;
            SystemLibrary.DebugLine("EMSX_APIDisconnect()");

        } //EMSX_APIDisconnect()

        private static void LoadOpenOrders()
        {
            SystemLibrary.DebugLine("LoadOpenOrders() - Start");
            dt_Open_Orders = SystemLibrary.SQLSelectToDataTable("Exec sp_Open_Orders");
            if (OutputWindow != null && OutputWindow.Visible)
            {
                OutputWindow.dt_Open_Orders = dt_Open_Orders.Copy();
                OutputWindow.Refresh_dgv_Open_Orders();
            }
            SystemLibrary.DebugLine("LoadOpenOrders() - End");

        } //LoadOpenOrders()

        private static Boolean TestSequence(String MSG_SUB_TYPE, Int64 API_SEQ_NUM)
        {
            switch (MSG_SUB_TYPE)
            {
                case "O":
                    if (EMSX_Order_seq == Int64.MinValue)
                        EMSX_Order_seq = API_SEQ_NUM - 1;
                    if (API_SEQ_NUM != EMSX_Order_seq + 1)
                    {
                        SystemLibrary.DebugLine("TestSequence(" + MSG_SUB_TYPE + "," + API_SEQ_NUM.ToString() + ") versus EMSX_Order_seq+1=" + (EMSX_Order_seq + 1).ToString());
                        return (false);
                    }
                    EMSX_Order_seq = API_SEQ_NUM;
                    break;
                case "R":
                    if (EMSX_Route_seq == Int64.MinValue)
                        EMSX_Route_seq = API_SEQ_NUM - 1;
                    if (API_SEQ_NUM != EMSX_Route_seq + 1)
                    {
                        SystemLibrary.DebugLine("TestSequence(" + MSG_SUB_TYPE + "," + API_SEQ_NUM.ToString() + ") versus EMSX_Order_seq+1=" + (EMSX_Order_seq + 1).ToString());
                        return (false);
                    }
                    EMSX_Route_seq = API_SEQ_NUM;
                    break;
            }
            return (true);

        } //TestSequence()

        private static DateTime BloombergNYTime_toDateTime(String inBloombergDate, Int32 inTime)
        {
            // Local Variables
            DateTime retVal;

            // Use the Bloomberg supplied Date in the Form YYYYMMDD
            retVal = SystemLibrary.YYYYMMDD_ToDate(inBloombergDate.Trim());

            // Truncate to midnight and add the seconds from midnight
            retVal = retVal.Date.AddSeconds(inTime);
            retVal = DateTime.SpecifyKind(retVal, DateTimeKind.Unspecified);

            // Convert back to current time zone
            TimeZoneInfo.ConvertTimeBySystemTimeZoneId(retVal, "Eastern Standard Time", DatabaseTimeZone);

            return (retVal);
        }

        private static DateTime BloombergNYTime_toDateTime(Int32 inTime)
        {
            // Local Variables
            DateTime retVal;

            // Convert Local Time to NY Time
            retVal = DateTime.Now;
            TimeZoneInfo.ConvertTimeBySystemTimeZoneId(retVal, TimeZoneInfo.Local.Id, "Eastern Standard Time");

            // Truncate to midnight and add the seconds from midnight
            retVal = retVal.Date.AddSeconds(inTime);
            retVal = DateTime.SpecifyKind(retVal, DateTimeKind.Unspecified);

            // Convert back to current time zone
            TimeZoneInfo.ConvertTimeBySystemTimeZoneId(retVal, "Eastern Standard Time", DatabaseTimeZone);

            return (retVal);
        }

        private static void ReportException(Event evt)
        {
            foreach (Bloomberglp.Blpapi.Message msg in evt.GetMessages())
            {
                if (msg.HasElement(REASON))
                {
                    // This occurs if a bad security is subscribed to
                    Element reason = msg.GetElement(REASON);
                    try
                    {
                        String CATEGORY = "";
                        String DESCRIPTION = "";
                        if (reason.HasElement(CATEGORY))
                            CATEGORY = reason.GetElement(CATEGORY).GetValueAsString();
                        if (reason.HasElement(DESCRIPTION))
                            DESCRIPTION = reason.GetElement(DESCRIPTION).GetValueAsString();

                        Console.WriteLine("EMSX API[" + evt.Type.ToString() + " - " + msg.MessageType.ToString() + "(" + msg.TopicName + "] Reason=" +
                                CATEGORY + ": " + DESCRIPTION);
                    }
                    catch
                    {
                        Console.WriteLine("EMSX API[" + evt.Type.ToString() + " - " + msg.MessageType.ToString() + "(" + msg.TopicName + "]");
                    }
                }

                if (msg.HasElement(EXCEPTIONS))
                {
                    // This can occur on SubscriptionStarted if an
                    // invalid field is passed in
                    Element exceptions = msg.GetElement(EXCEPTIONS);
                    for (int i = 0; i < exceptions.NumValues; ++i)
                    {
                        Element exInfo = exceptions.GetValueAsElement(i);
                        Element fieldId = exInfo.GetElement(FIELD_ID);
                        Element reason = exInfo.GetElement(REASON);
                        Console.WriteLine("EMSX API[" + evt.Type.ToString() + " - " + msg.MessageType.ToString() + "(" + msg.TopicName + "] Exception=" + fieldId.GetValueAsString() +
                                ": " + reason.GetElement(CATEGORY).GetValueAsString());
                    }
                }
            }
        } //ReportException()

        private static void ProcessEvent(Event evt, Session session)
        {
             // Depending on the event type - signal the appropriate event
            SystemLibrary.DebugLine("ProcessEvent - " + evt.Type.ToString());
            switch (evt.Type)
            {
                case Event.EventType.SESSION_STATUS:
                    sm_sessionStatus.Set();
                    ReportException(evt);
                    break;
                case Event.EventType.SERVICE_STATUS:
                    sm_serviceStatus.Set();
                    ReportException(evt);
                    break;
                case Event.EventType.SUBSCRIPTION_STATUS:
                    sm_subscriptionStatus.Set();
                    ReportException(evt);
                    break;
                case Event.EventType.SUBSCRIPTION_DATA:
                    sm_apiSeqReceived.Set();
                    //foreach (Bloomberglp.Blpapi.Message message in evt.GetMessages())
                    //    SystemLibrary.DebugLine("message=" + message.ToString());
                    ProcessBlpEvent(evt);
                    break;
            }
            SystemLibrary.DebugLine("ProcessEvent - END");
        } //ProcessEvent()

       /*
        * CreateRecord() 
        *  - return True    if it sucessfully alters the database.
        *           False   if the database is already up to date.
        */
       private static Boolean CreateRecord(Bloomberglp.Blpapi.Message message, String EMSX_SEQUENCE, String MSG_SUB_TYPE, String Event_Status)
        {
            // Local Variables
            Boolean retVal = false;
            String mySql;
            DataRow dr = dt_EMSX_API.NewRow();
            String STR_EMSX_LAST_FILL_TIME;
            String STR_EMSX_TIME_STAMP;

            dr["EMSX_SEQUENCE"] = EMSX_SEQUENCE;
            dr["MSG_SUB_TYPE"] = MSG_SUB_TYPE;
            dr["EVENT_STATUS"] = Event_Status;
            dr["EMSX_STATUS"] = message.GetElementAsString("EMSX_STATUS").ToString().Trim();

            switch (MSG_SUB_TYPE)
            {
                case "O":
                    // Order
                    dr["EMSX_TICKER"] = message.GetElementAsString("EMSX_TICKER").ToString().Trim();
                    dr["EMSX_EXCHANGE"] = message.GetElementAsString("EMSX_EXCHANGE").ToString().Trim();
                    dr["EMSX_SIDE"] = message.GetElementAsString("EMSX_SIDE").ToString().Trim();
                    dr["EMSX_AMOUNT"] = message.GetElementAsFloat64("EMSX_AMOUNT");
                    dr["EMSX_WORKING"] = message.GetElementAsFloat64("EMSX_WORKING");
                    dr["EMSX_FILLED"] = message.GetElementAsFloat64("EMSX_FILLED");
                    dr["EMSX_AVG_PRICE"] = message.GetElementAsFloat64("EMSX_AVG_PRICE");
                    dr["EMSX_TRADER"] = message.GetElementAsString("EMSX_TRADER").ToString().Trim();
                    dr["EMSX_TRAD_UUID"] = message.GetElementAsString("EMSX_TRAD_UUID").ToString().Trim();
                    dr["EMSX_ARRIVAL_PRICE"] = message.GetElementAsFloat64("EMSX_ARRIVAL_PRICE");
                    dr["EMSX_TIME_STAMP"] = BloombergNYTime_toDateTime(message.GetElementAsString("EMSX_DATE").ToString().Trim(),message.GetElementAsInt32("EMSX_TIME_STAMP"));
                    dr["EMSX_DATE"] = SystemLibrary.YYYYMMDD_ToDate(message.GetElementAsString("EMSX_DATE").ToString().Trim());

                    dt_EMSX_API.Rows.Add(dr);

                    if (isSql2005)
                    {
                        mySql = "Insert into EMSX_API (EMSX_SEQUENCE,MSG_SUB_TYPE,EVENT_STATUS,EMSX_STATUS,EMSX_TICKER,EMSX_EXCHANGE,EMSX_SIDE,EMSX_AMOUNT," +
                                "                      EMSX_WORKING,EMSX_FILLED,EMSX_AVG_PRICE,EMSX_TRADER,EMSX_TRAD_UUID,EMSX_ARRIVAL_PRICE,EMSX_TIME_STAMP,EMSX_DATE) " +
                                "Select " + dr["EMSX_SEQUENCE"].ToString() + ",'" + dr["MSG_SUB_TYPE"].ToString() + "','" + dr["EVENT_STATUS"].ToString() + "','" + dr["EMSX_STATUS"].ToString() + "','" + dr["EMSX_TICKER"].ToString() + "','" + dr["EMSX_EXCHANGE"].ToString() + "','" + dr["EMSX_SIDE"].ToString() + "'," + dr["EMSX_AMOUNT"].ToString() + "," +
                                "        " + dr["EMSX_WORKING"].ToString() + "," + dr["EMSX_FILLED"].ToString() + "," + dr["EMSX_AVG_PRICE"].ToString() + ",'" + dr["EMSX_TRADER"].ToString() + "','" + dr["EMSX_TRAD_UUID"].ToString() + "'," + dr["EMSX_ARRIVAL_PRICE"].ToString() + ",'" + Convert.ToDateTime(dr["EMSX_TIME_STAMP"]).ToString("dd-MMM-yyyy HH:mm:ss") + "','" + Convert.ToDateTime(dr["EMSX_DATE"]).ToString("dd-MMM-yyyy") + "' " +
                                "Where Not Exists (Select 'x' From EMSX_API Where EMSX_SEQUENCE = " + dr["EMSX_SEQUENCE"].ToString() + " AND MSG_SUB_TYPE = 'O')";
                        if (SystemLibrary.SQLExecute(mySql) > 0)
                        {
                            SystemLibrary.DebugLine("DatabaseWasUpdated=\r\n" + mySql);
                            retVal = true;
                        }
                        else
                        {
                            // Record already exists, so Update it
                            mySql = "Update EMSX_API " +
                                    "Set EVENT_STATUS = '" + dr["EVENT_STATUS"].ToString() + "', " +
                                    "    EMSX_STATUS = '" + dr["EMSX_STATUS"].ToString() + "', " +
                                    "    EMSX_TICKER = '" + dr["EMSX_TICKER"].ToString() + "', " +
                                    "    EMSX_EXCHANGE = '" + dr["EMSX_EXCHANGE"].ToString() + "', " +
                                    "    EMSX_SIDE = '" + dr["EMSX_SIDE"].ToString() + "', " +
                                    "    EMSX_AMOUNT = " + dr["EMSX_AMOUNT"].ToString() + ", " +
                                    "    EMSX_WORKING = " + dr["EMSX_WORKING"].ToString() + ", " +
                                    "    EMSX_FILLED = " + dr["EMSX_FILLED"].ToString() + ", " +
                                    "    EMSX_AVG_PRICE = " + dr["EMSX_AVG_PRICE"].ToString() + ", " +
                                    "    EMSX_TRADER = '" + dr["EMSX_TRADER"].ToString() + "', " +
                                    "    EMSX_TRAD_UUID = '" + dr["EMSX_TRAD_UUID"].ToString() + "', " +
                                    "    EMSX_ARRIVAL_PRICE = " + dr["EMSX_ARRIVAL_PRICE"].ToString() + ", " +
                                    "    EMSX_TIME_STAMP = '" + Convert.ToDateTime(dr["EMSX_TIME_STAMP"]).ToString("dd-MMM-yyyy HH:mm:ss") + "', " +
                                    "    EMSX_DATE = '" + Convert.ToDateTime(dr["EMSX_DATE"]).ToString("dd-MMM-yyyy") + "' " +
                                    "Where EMSX_SEQUENCE='" + EMSX_SEQUENCE + "' and MSG_SUB_TYPE='O' " +
                                    "And Not Exists (   Select 'x' " +
                                    "                   From    EMSX_API b " +
                                    "                   Where   b.EMSX_SEQUENCE = '" + EMSX_SEQUENCE + "' " +
                                    "                   And     b.MSG_SUB_TYPE = 'O' ";
                            if (!(Event_Status == "NEW" || Event_Status == "UPDATE"))
                            {
                                mySql = mySql +
                                        "                   And     b.EVENT_STATUS = '" + dr["EVENT_STATUS"].ToString() + "' ";
                            }
                            mySql = mySql +
                                    "                   And     b.EMSX_STATUS = '" + dr["EMSX_STATUS"].ToString() + "' " +
                                    "                   And     b.EMSX_TICKER = '" + dr["EMSX_TICKER"].ToString() + "' " +
                                    "                   And     b.EMSX_EXCHANGE = '" + dr["EMSX_EXCHANGE"].ToString() + "' " +
                                    "                   And     b.EMSX_SIDE = '" + dr["EMSX_SIDE"].ToString() + "' " +
                                    "                   And     b.EMSX_AMOUNT = " + dr["EMSX_AMOUNT"].ToString() + " " +
                                    "                   And     b.EMSX_WORKING = " + dr["EMSX_WORKING"].ToString() + " " +
                                    "                   And     b.EMSX_FILLED = " + dr["EMSX_FILLED"].ToString() + " " +
                                    "                   And     Round(b.EMSX_AVG_PRICE,8) = Round(" + dr["EMSX_AVG_PRICE"].ToString() + ",8) " +
                                    "                   And     b.EMSX_TRADER = '" + dr["EMSX_TRADER"].ToString() + "' " +
                                    "                   And     b.EMSX_TRAD_UUID = '" + dr["EMSX_TRAD_UUID"].ToString() + "' " +
                                    "                   And     Round(b.EMSX_ARRIVAL_PRICE,8) = Round(" + dr["EMSX_ARRIVAL_PRICE"].ToString() + ",8) " +
                                    "                   And     b.EMSX_TIME_STAMP = '" + Convert.ToDateTime(dr["EMSX_TIME_STAMP"]).ToString("dd-MMM-yyyy HH:mm:ss") + "' " +
                                    "                   And     b.EMSX_DATE = '" + Convert.ToDateTime(dr["EMSX_DATE"]).ToString("dd-MMM-yyyy") + "' " +
                                    "                )";
                            if (SystemLibrary.SQLExecute(mySql) > 0)
                            {
                                SystemLibrary.DebugLine("DatabaseWasUpdated=\r\n" + mySql);
                                retVal = true;
                            }
                        }
                    }
                    else
                        retVal = false;
                    break;
                case "R":
                    // Route
                    dr["EMSX_ROUTE_ID"] = message.GetElementAsInt32("EMSX_ROUTE_ID");
                    dr["EMSX_AMOUNT"] = message.GetElementAsFloat64("EMSX_AMOUNT");
                    dr["EMSX_WORKING"] = message.GetElementAsFloat64("EMSX_WORKING");
                    dr["EMSX_FILLED"] = message.GetElementAsFloat64("EMSX_FILLED");
                    dr["EMSX_AVG_PRICE"] = message.GetElementAsFloat64("EMSX_AVG_PRICE");
                    dr["EMSX_TYPE"] = message.GetElementAsString("EMSX_TYPE").ToString().Trim();
                    dr["EMSX_LIMIT_PRICE"] = message.GetElementAsFloat64("EMSX_LIMIT_PRICE");
                    dr["EMSX_BROKER"] = message.GetElementAsString("EMSX_BROKER").ToString().Trim();
                    dr["EMSX_STRATEGY_TYPE"] = message.GetElementAsString("EMSX_STRATEGY_TYPE").ToString().Trim();
                    dr["EMSX_CUSTOM_ACCOUNT"] = message.GetElementAsString("EMSX_CUSTOM_ACCOUNT").ToString().Trim();
                    Int32 EMSX_LAST_FILL_TIME = message.GetElementAsInt32("EMSX_LAST_FILL_TIME");
                    Int32 EMSX_ROUTE_LAST_UPDATE_TIME = message.GetElementAsInt32("EMSX_ROUTE_LAST_UPDATE_TIME");
                    if (EMSX_LAST_FILL_TIME > 0)
                        dr["EMSX_LAST_FILL_TIME"] = BloombergNYTime_toDateTime(message.GetElementAsString("EMSX_LAST_FILL_DATE").ToString().Trim(),EMSX_LAST_FILL_TIME);
                    else if (EMSX_ROUTE_LAST_UPDATE_TIME > 0)
                        dr["EMSX_LAST_FILL_TIME"] = BloombergNYTime_toDateTime(EMSX_ROUTE_LAST_UPDATE_TIME);
                    dr["EMSX_TIME_STAMP"] = dr["EMSX_LAST_FILL_TIME"];
                    String EMSX_SETTLE_DATE = message.GetElementAsString("EMSX_SETTLE_DATE").ToString().Trim();
                    if (EMSX_SETTLE_DATE.Length == 8)
                    {
                        dr["EMSX_SETTLE_DATE"] = SystemLibrary.YYYYMMDD_ToDate(EMSX_SETTLE_DATE);
                        EMSX_SETTLE_DATE = "'" + Convert.ToDateTime(dr["EMSX_SETTLE_DATE"]).ToString("dd-MMM-yyyy") + "'";
                    }
                    else
                        EMSX_SETTLE_DATE = "null";

                    if (dr["EMSX_LAST_FILL_TIME"] == DBNull.Value)
                        STR_EMSX_LAST_FILL_TIME = "null";
                    else
                        STR_EMSX_LAST_FILL_TIME = "'" + Convert.ToDateTime(dr["EMSX_LAST_FILL_TIME"]).ToString("dd-MMM-yyyy HH:mm:ss") + "'";

                    if (dr["EMSX_TIME_STAMP"] == DBNull.Value)
                        STR_EMSX_TIME_STAMP = "null";
                    else
                        STR_EMSX_TIME_STAMP = "'" + Convert.ToDateTime(dr["EMSX_TIME_STAMP"]).ToString("dd-MMM-yyyy HH:mm:ss") + "'";

                    dt_EMSX_API.Rows.Add(dr);

                    if (isSql2005)
                    {
                        mySql = "Insert into EMSX_API (EMSX_SEQUENCE,MSG_SUB_TYPE,EVENT_STATUS,EMSX_STATUS,EMSX_ROUTE_ID,EMSX_AMOUNT,EMSX_WORKING," +
                                "                      EMSX_FILLED,EMSX_AVG_PRICE,EMSX_TYPE,EMSX_LIMIT_PRICE,EMSX_BROKER,EMSX_STRATEGY_TYPE," +
                                "                      EMSX_CUSTOM_ACCOUNT,EMSX_LAST_FILL_TIME,EMSX_TIME_STAMP,EMSX_SETTLE_DATE) " +
                                "Select  " + dr["EMSX_SEQUENCE"].ToString() + ",'" + dr["MSG_SUB_TYPE"].ToString() + "','" + dr["EVENT_STATUS"].ToString() + "','" + dr["EMSX_STATUS"].ToString() + "'," + dr["EMSX_ROUTE_ID"].ToString() + "," + dr["EMSX_AMOUNT"].ToString() + "," + dr["EMSX_WORKING"].ToString() + "," +
                                "        " + dr["EMSX_FILLED"].ToString() + "," + dr["EMSX_AVG_PRICE"].ToString() + ",'" + dr["EMSX_TYPE"].ToString() + "'," + dr["EMSX_LIMIT_PRICE"].ToString() + ",'" + dr["EMSX_BROKER"].ToString() + "','" + dr["EMSX_STRATEGY_TYPE"].ToString() + "'," +
                                "        '" + dr["EMSX_CUSTOM_ACCOUNT"].ToString() + "'," + STR_EMSX_LAST_FILL_TIME + "," + STR_EMSX_TIME_STAMP + "," + EMSX_SETTLE_DATE + " " +
                                "Where Not Exists (Select 'x' From EMSX_API Where EMSX_SEQUENCE = " + dr["EMSX_SEQUENCE"].ToString() + " AND MSG_SUB_TYPE = 'R' AND EMSX_ROUTE_ID = " + dr["EMSX_ROUTE_ID"].ToString() + ")";
                        if (SystemLibrary.SQLExecute(mySql) > 0)
                        {
                            SystemLibrary.DebugLine("DatabaseWasUpdated=\r\n" + mySql);
                            retVal = true;
                        }
                        else
                        {
                            // Record already exists, so Update it
                            mySql = "Update EMSX_API " +
                                    "Set EVENT_STATUS = '" + dr["EVENT_STATUS"].ToString() + "', " +
                                    "    EMSX_STATUS = '" + dr["EMSX_STATUS"].ToString() + "', " +
                                    "    EMSX_AMOUNT = " + dr["EMSX_AMOUNT"].ToString() + ", " +
                                    "    EMSX_WORKING = " + dr["EMSX_WORKING"].ToString() + ", " +
                                    "    EMSX_FILLED = " + dr["EMSX_FILLED"].ToString() + ", " +
                                    "    EMSX_AVG_PRICE = " + dr["EMSX_AVG_PRICE"].ToString() + ", " +
                                    "    EMSX_TYPE = '" + dr["EMSX_TYPE"].ToString() + "', " +
                                    "    EMSX_LIMIT_PRICE = " + dr["EMSX_LIMIT_PRICE"].ToString() + ", " +
                                    "    EMSX_BROKER = '" + dr["EMSX_BROKER"].ToString() + "', " +
                                    "    EMSX_STRATEGY_TYPE = '" + dr["EMSX_STRATEGY_TYPE"].ToString() + "', " +
                                    "    EMSX_CUSTOM_ACCOUNT = '" + dr["EMSX_CUSTOM_ACCOUNT"].ToString() + "', " +
                                    "    EMSX_LAST_FILL_TIME = '" + Convert.ToDateTime(dr["EMSX_LAST_FILL_TIME"]).ToString("dd-MMM-yyyy HH:mm:ss") + "', " +
                                    "    EMSX_TIME_STAMP = '" + Convert.ToDateTime(dr["EMSX_TIME_STAMP"]).ToString("dd-MMM-yyyy HH:mm:ss") + "', " +
                                    "    EMSX_SETTLE_DATE = " + EMSX_SETTLE_DATE + " " +
                                    "Where  EMSX_SEQUENCE='" + EMSX_SEQUENCE + "' and MSG_SUB_TYPE='" + MSG_SUB_TYPE + "' " +
                                    "And    EMSX_ROUTE_ID = " + dr["EMSX_ROUTE_ID"].ToString() + " " +
                                    "And Not Exists (   Select 'x' " +
                                    "                   From    EMSX_API b " +
                                    "                   Where   b.EMSX_SEQUENCE = '" + EMSX_SEQUENCE + "' " +
                                    "                   And     b.MSG_SUB_TYPE = '" + MSG_SUB_TYPE + "' " +
                                    "                   And     b.EMSX_ROUTE_ID = " + dr["EMSX_ROUTE_ID"].ToString() + " ";
                            if (!(Event_Status == "NEW" || Event_Status == "UPDATE"))
                            {

                                mySql = mySql +
                                        "                   And     b.EVENT_STATUS = '" + dr["EVENT_STATUS"].ToString() + "' ";
                            }
                            mySql = mySql +
                                    "                   And     b.EMSX_STATUS = '" + dr["EMSX_STATUS"].ToString() + "' " +
                                    "                   And     b.EMSX_AMOUNT = " + dr["EMSX_AMOUNT"].ToString() + " " +
                                    "                   And     b.EMSX_WORKING = " + dr["EMSX_WORKING"].ToString() + " " +
                                    "                   And     b.EMSX_FILLED = " + dr["EMSX_FILLED"].ToString() + " " +
                                    "                   And     Round(b.EMSX_AVG_PRICE,8) = Round(" + dr["EMSX_AVG_PRICE"].ToString() + ",8) " +
                                    "                   And     b.EMSX_TYPE = '" + dr["EMSX_TYPE"].ToString() + "' " +
                                    "                   And     b.EMSX_LIMIT_PRICE = " + dr["EMSX_LIMIT_PRICE"].ToString() + " " +
                                    "                   And     b.EMSX_BROKER = '" + dr["EMSX_BROKER"].ToString() + "' " +
                                    "                   And     b.EMSX_STRATEGY_TYPE = '" + dr["EMSX_STRATEGY_TYPE"].ToString() + "' " +
                                    "                   And     b.EMSX_CUSTOM_ACCOUNT = '" + dr["EMSX_CUSTOM_ACCOUNT"].ToString() + "' " +
                                    "                   And     b.EMSX_LAST_FILL_TIME = '" + Convert.ToDateTime(dr["EMSX_LAST_FILL_TIME"]).ToString("dd-MMM-yyyy HH:mm:ss") + "' " +
                                    "                   And     b.EMSX_TIME_STAMP = '" + Convert.ToDateTime(dr["EMSX_TIME_STAMP"]).ToString("dd-MMM-yyyy HH:mm:ss") + "' ";
                            if (EMSX_SETTLE_DATE == "null")
                                mySql = mySql + "                   And     b.EMSX_SETTLE_DATE is null ";
                            else
                                mySql = mySql + "                   And     b.EMSX_SETTLE_DATE = " + EMSX_SETTLE_DATE + " ";
                            mySql = mySql +
                                    "                )";
                            if (SystemLibrary.SQLExecute(mySql) > 0)
                            {
                                SystemLibrary.DebugLine("DatabaseWasUpdated=\r\n" + mySql);
                                retVal = true;
                            }
                        }
                    }
                    else
                        retVal = false;
                    break;
                case "F":
                    // Fill ('F' not implemented Nov 2012)
                    SystemLibrary.DebugLine(message.ToString());
                    break;
            }

            return (retVal);

        } //CreateRecord()

        private static Boolean UpdateRecord(Bloomberglp.Blpapi.Message message, String EMSX_SEQUENCE, String MSG_SUB_TYPE, ref Boolean DatabaseWasUpdated)
        {
            // Local Variables
            String mySql;
            Boolean retVal = false;

            DatabaseWasUpdated = false;

            DataRow[] myUpdateRows = dt_EMSX_API.Select("EMSX_SEQUENCE='" + EMSX_SEQUENCE + "' and MSG_SUB_TYPE='" + MSG_SUB_TYPE + "'", "");
            foreach (DataRow drUpdate in myUpdateRows)
            {
                drUpdate["EVENT_STATUS"] = "UPDATE";
                drUpdate["EMSX_STATUS"] = message.GetElementAsString("EMSX_STATUS").ToString().Trim();

                switch (MSG_SUB_TYPE)
                {
                    case "O":
                        // Order - Only update non blank fields (a user cannot change Side or Ticker elements)
                        if (message.HasElement("EMSX_AMOUNT", true)) drUpdate["EMSX_AMOUNT"] = message.GetElementAsFloat64("EMSX_AMOUNT");
                        if (message.HasElement("EMSX_WORKING", true)) drUpdate["EMSX_WORKING"] = message.GetElementAsFloat64("EMSX_WORKING");
                        if (message.HasElement("EMSX_FILLED", true)) drUpdate["EMSX_FILLED"] = message.GetElementAsFloat64("EMSX_FILLED");
                        if (message.HasElement("EMSX_AVG_PRICE", true)) drUpdate["EMSX_AVG_PRICE"] = message.GetElementAsFloat64("EMSX_AVG_PRICE");
                        if (message.HasElement("EMSX_TRADER", true)) drUpdate["EMSX_TRADER"] = message.GetElementAsString("EMSX_TRADER").ToString().Trim();
                        if (message.HasElement("EMSX_TRAD_UUID", true)) drUpdate["EMSX_TRAD_UUID"] = message.GetElementAsString("EMSX_TRAD_UUID").ToString().Trim();
                        if (message.HasElement("EMSX_ARRIVAL_PRICE", true)) drUpdate["EMSX_ARRIVAL_PRICE"] = message.GetElementAsFloat64("EMSX_ARRIVAL_PRICE");
                        if (message.HasElement("EMSX_TIME_STAMP", true))
                        {
                            if (message.HasElement("EMSX_DATE", true))
                                drUpdate["EMSX_TIME_STAMP"] = BloombergNYTime_toDateTime(message.GetElementAsString("EMSX_DATE").ToString().Trim(), message.GetElementAsInt32("EMSX_TIME_STAMP"));
                            else
                                drUpdate["EMSX_TIME_STAMP"] = BloombergNYTime_toDateTime(message.GetElementAsInt32("EMSX_TIME_STAMP"));
                        }
                        if (message.HasElement("EMSX_DATE", true)) drUpdate["EMSX_DATE"] = SystemLibrary.YYYYMMDD_ToDate(message.GetElementAsString("EMSX_DATE").ToString().Trim());

                        if (isSql2005)
                        {
                            //      "                   And     b.EMSX_STATUS = '" + drUpdate["EMSX_STATUS"].ToString() + "' " +
                            mySql = "Update EMSX_API " +
                                    "Set EVENT_STATUS = '" + drUpdate["EVENT_STATUS"].ToString() + "', " +
                                    "    EMSX_STATUS = '" + drUpdate["EMSX_STATUS"].ToString() + "', " +
                                    "    EMSX_AMOUNT = " + drUpdate["EMSX_AMOUNT"].ToString() + ", " +
                                    "    EMSX_WORKING = " + drUpdate["EMSX_WORKING"].ToString() + ", " +
                                    "    EMSX_FILLED = " + drUpdate["EMSX_FILLED"].ToString() + ", " +
                                    "    EMSX_AVG_PRICE = " + drUpdate["EMSX_AVG_PRICE"].ToString() + ", " +
                                    "    EMSX_TRADER = '" + drUpdate["EMSX_TRADER"].ToString() + "', " +
                                    "    EMSX_TRAD_UUID = '" + drUpdate["EMSX_TRAD_UUID"].ToString() + "', " +
                                    "    EMSX_ARRIVAL_PRICE = " + drUpdate["EMSX_ARRIVAL_PRICE"].ToString() + ", " +
                                    "    EMSX_TIME_STAMP = '" + Convert.ToDateTime(drUpdate["EMSX_TIME_STAMP"]).ToString("dd-MMM-yyyy HH:mm:ss") + "', " +
                                    "    EMSX_DATE = '" + Convert.ToDateTime(drUpdate["EMSX_DATE"]).ToString("dd-MMM-yyyy") + "' " +
                                    "Where EMSX_SEQUENCE='" + EMSX_SEQUENCE + "' and MSG_SUB_TYPE='" + MSG_SUB_TYPE + "' " +
                                    "And Not Exists (   Select 'x' " +
                                    "                   From    EMSX_API b " +
                                    "                   Where   b.EMSX_SEQUENCE = '" + EMSX_SEQUENCE + "' " +
                                    "                   And     b.MSG_SUB_TYPE = '" + MSG_SUB_TYPE + "' " +
                                    "                   And     b.EVENT_STATUS = '" + drUpdate["EVENT_STATUS"].ToString() + "' " +
                                    "                   And     b.EMSX_AMOUNT = " + drUpdate["EMSX_AMOUNT"].ToString() + " " +
                                    "                   And     b.EMSX_WORKING = " + drUpdate["EMSX_WORKING"].ToString() + " " +
                                    "                   And     b.EMSX_FILLED = " + drUpdate["EMSX_FILLED"].ToString() + " " +
                                    "                   And     Round(b.EMSX_AVG_PRICE,8) = Round(" + drUpdate["EMSX_AVG_PRICE"].ToString() + ",8) " +
                                    "                   And     b.EMSX_TRADER = '" + drUpdate["EMSX_TRADER"].ToString() + "' " +
                                    "                   And     b.EMSX_TRAD_UUID = '" + drUpdate["EMSX_TRAD_UUID"].ToString() + "' " +
                                    "                   And     Round(b.EMSX_ARRIVAL_PRICE,8) = Round(" + drUpdate["EMSX_ARRIVAL_PRICE"].ToString() + ",8) " +
                                    "                   And     b.EMSX_TIME_STAMP = '" + Convert.ToDateTime(drUpdate["EMSX_TIME_STAMP"]).ToString("dd-MMM-yyyy HH:mm:ss") + "' " +
                                    "                   And     b.EMSX_DATE = '" + Convert.ToDateTime(drUpdate["EMSX_DATE"]).ToString("dd-MMM-yyyy") + "' " +
                                    "                )";
                            if (SystemLibrary.SQLExecute(mySql) > 0)
                            {
                                SystemLibrary.DebugLine("DatabaseWasUpdated=\r\n" + mySql);
                                DatabaseWasUpdated = true;
                            }
                            retVal = true;
                        }
                        else
                            retVal = false;
                        break;
                    case "R":
                        // Route
                        if (SystemLibrary.ToInt32(drUpdate["EMSX_ROUTE_ID"]) == message.GetElementAsInt32("EMSX_ROUTE_ID"))
                        {
                            if (message.HasElement("EMSX_ROUTE_ID", true)) drUpdate["EMSX_ROUTE_ID"] = message.GetElementAsInt32("EMSX_ROUTE_ID");
                            if (message.HasElement("EMSX_AMOUNT", true)) drUpdate["EMSX_AMOUNT"] = message.GetElementAsFloat64("EMSX_AMOUNT");
                            if (message.HasElement("EMSX_WORKING", true)) drUpdate["EMSX_WORKING"] = message.GetElementAsFloat64("EMSX_WORKING");
                            if (message.HasElement("EMSX_FILLED", true)) drUpdate["EMSX_FILLED"] = message.GetElementAsFloat64("EMSX_FILLED");
                            if (message.HasElement("EMSX_AVG_PRICE", true)) drUpdate["EMSX_AVG_PRICE"] = message.GetElementAsFloat64("EMSX_AVG_PRICE");
                            if (message.HasElement("EMSX_TYPE", true)) drUpdate["EMSX_TYPE"] = message.GetElementAsString("EMSX_TYPE").ToString().Trim();
                            if (message.HasElement("EMSX_LIMIT_PRICE", true)) drUpdate["EMSX_LIMIT_PRICE"] = message.GetElementAsFloat64("EMSX_LIMIT_PRICE");
                            if (message.HasElement("EMSX_BROKER", true)) drUpdate["EMSX_BROKER"] = message.GetElementAsString("EMSX_BROKER").ToString().Trim();
                            if (message.HasElement("EMSX_STRATEGY_TYPE", true)) drUpdate["EMSX_STRATEGY_TYPE"] = message.GetElementAsString("EMSX_STRATEGY_TYPE").ToString().Trim();
                            if (message.HasElement("EMSX_CUSTOM_ACCOUNT", true)) drUpdate["EMSX_CUSTOM_ACCOUNT"] = message.GetElementAsString("EMSX_CUSTOM_ACCOUNT").ToString().Trim();
                            Int32 EMSX_LAST_FILL_TIME = message.GetElementAsInt32("EMSX_LAST_FILL_TIME");
                            Int32 EMSX_ROUTE_LAST_UPDATE_TIME = message.GetElementAsInt32("EMSX_ROUTE_LAST_UPDATE_TIME");
                            if (EMSX_LAST_FILL_TIME > 0)
                            {
                                if (message.HasElement("EMSX_LAST_FILL_DATE", true))
                                    drUpdate["EMSX_LAST_FILL_TIME"] = BloombergNYTime_toDateTime(message.GetElementAsString("EMSX_LAST_FILL_DATE").ToString().Trim(), EMSX_LAST_FILL_TIME);
                                else
                                    drUpdate["EMSX_LAST_FILL_TIME"] = BloombergNYTime_toDateTime(EMSX_LAST_FILL_TIME);
                            }
                            else if (EMSX_ROUTE_LAST_UPDATE_TIME > 0)
                                drUpdate["EMSX_LAST_FILL_TIME"] = BloombergNYTime_toDateTime(EMSX_ROUTE_LAST_UPDATE_TIME);
                            drUpdate["EMSX_TIME_STAMP"] = drUpdate["EMSX_LAST_FILL_TIME"];
                            String EMSX_SETTLE_DATE = message.GetElementAsString("EMSX_SETTLE_DATE").ToString().Trim();
                            if (EMSX_SETTLE_DATE.Length == 8)
                            {
                                drUpdate["EMSX_SETTLE_DATE"] = SystemLibrary.YYYYMMDD_ToDate(EMSX_SETTLE_DATE);
                                EMSX_SETTLE_DATE = "'" + Convert.ToDateTime(drUpdate["EMSX_SETTLE_DATE"]).ToString("dd-MMM-yyyy") + "'";
                            }
                            else
                                EMSX_SETTLE_DATE = "null";

                            String EMSX_LIMIT_PRICE = "null";
                            if (drUpdate["EMSX_LIMIT_PRICE"].ToString().Length>0)
                                EMSX_LIMIT_PRICE = drUpdate["EMSX_LIMIT_PRICE"].ToString();

                            if (isSql2005)
                            {

                                //      "                   And     b.EVENT_STATUS = '" + drUpdate["EVENT_STATUS"].ToString() + "' " +
                                mySql = "Update EMSX_API " +
                                        "Set EVENT_STATUS = '" + drUpdate["EVENT_STATUS"].ToString() + "', " +
                                        "    EMSX_STATUS = '" + drUpdate["EMSX_STATUS"].ToString() + "', " +
                                        "    EMSX_AMOUNT = " + drUpdate["EMSX_AMOUNT"].ToString() + ", " +
                                        "    EMSX_WORKING = " + drUpdate["EMSX_WORKING"].ToString() + ", " +
                                        "    EMSX_FILLED = " + drUpdate["EMSX_FILLED"].ToString() + ", " +
                                        "    EMSX_AVG_PRICE = " + drUpdate["EMSX_AVG_PRICE"].ToString() + ", " +
                                        "    EMSX_TYPE = '" + drUpdate["EMSX_TYPE"].ToString() + "', " +
                                        "    EMSX_LIMIT_PRICE = " + EMSX_LIMIT_PRICE + ", " +
                                        "    EMSX_BROKER = '" + drUpdate["EMSX_BROKER"].ToString() + "', " +
                                        "    EMSX_STRATEGY_TYPE = '" + drUpdate["EMSX_STRATEGY_TYPE"].ToString() + "', " +
                                        "    EMSX_CUSTOM_ACCOUNT = '" + drUpdate["EMSX_CUSTOM_ACCOUNT"].ToString() + "', " +
                                        "    EMSX_LAST_FILL_TIME = '" + Convert.ToDateTime(drUpdate["EMSX_LAST_FILL_TIME"]).ToString("dd-MMM-yyyy HH:mm:ss") + "', " +
                                        "    EMSX_TIME_STAMP = '" + Convert.ToDateTime(drUpdate["EMSX_TIME_STAMP"]).ToString("dd-MMM-yyyy HH:mm:ss") + "', " +
                                        "    EMSX_SETTLE_DATE = " + EMSX_SETTLE_DATE + " " +
                                        "Where  EMSX_SEQUENCE='" + EMSX_SEQUENCE + "' and MSG_SUB_TYPE='" + MSG_SUB_TYPE + "' " +
                                        "And    EMSX_ROUTE_ID = " + drUpdate["EMSX_ROUTE_ID"].ToString() + " " +
                                        "And Not Exists (   Select 'x' " +
                                        "                   From    EMSX_API b " +
                                        "                   Where   b.EMSX_SEQUENCE = '" + EMSX_SEQUENCE + "' " +
                                        "                   And     b.MSG_SUB_TYPE = '" + MSG_SUB_TYPE + "' " +
                                        "                   And     b.EMSX_ROUTE_ID = " + drUpdate["EMSX_ROUTE_ID"].ToString() + " " +
                                        "                   And     b.EMSX_STATUS = '" + drUpdate["EMSX_STATUS"].ToString() + "' " +
                                        "                   And     b.EMSX_AMOUNT = " + drUpdate["EMSX_AMOUNT"].ToString() + " " +
                                        "                   And     b.EMSX_WORKING = " + drUpdate["EMSX_WORKING"].ToString() + " " +
                                        "                   And     b.EMSX_FILLED = " + drUpdate["EMSX_FILLED"].ToString() + " " +
                                        "                   And     Round(b.EMSX_AVG_PRICE,8) = Round(" + drUpdate["EMSX_AVG_PRICE"].ToString() + ",8) " +
                                        "                   And     b.EMSX_TYPE = '" + drUpdate["EMSX_TYPE"].ToString() + "' " +
                                        "                   And     b.EMSX_LIMIT_PRICE = " + EMSX_LIMIT_PRICE + " " +
                                        "                   And     b.EMSX_BROKER = '" + drUpdate["EMSX_BROKER"].ToString() + "' " +
                                        "                   And     b.EMSX_STRATEGY_TYPE = '" + drUpdate["EMSX_STRATEGY_TYPE"].ToString() + "' " +
                                        "                   And     b.EMSX_CUSTOM_ACCOUNT = '" + drUpdate["EMSX_CUSTOM_ACCOUNT"].ToString() + "' " +
                                        "                   And     b.EMSX_LAST_FILL_TIME = '" + Convert.ToDateTime(drUpdate["EMSX_LAST_FILL_TIME"]).ToString("dd-MMM-yyyy HH:mm:ss") + "' " +
                                        "                   And     b.EMSX_TIME_STAMP = '" + Convert.ToDateTime(drUpdate["EMSX_TIME_STAMP"]).ToString("dd-MMM-yyyy HH:mm:ss") + "' ";
                                if (EMSX_SETTLE_DATE == "null")
                                    mySql = mySql + "                   And     b.EMSX_SETTLE_DATE is null ";
                                else
                                    mySql = mySql + "                   And     b.EMSX_SETTLE_DATE = " + EMSX_SETTLE_DATE + " ";
                                mySql = mySql +
                                        "                )";
                                if (SystemLibrary.SQLExecute(mySql) > 0)
                                {
                                    SystemLibrary.DebugLine("DatabaseWasUpdated=\r\n" + mySql);
                                    DatabaseWasUpdated = true;
                                }
                                retVal = true;
                            }
                            else
                                retVal = false;
                        }
                        break;
                    default:
                        if (isSql2005)
                            retVal = true;
                        else
                            retVal = false;
                        break;
                }
            }

            return (retVal);

        } //UpdateRecord()

        private static void UpdateRoutedAmount(String EMSX_SEQUENCE)
        {
            // Update the ORDER record with the Sum of the ROUTE records
            Decimal Routed_Amount = SystemLibrary.ToDecimal(dt_EMSX_API.Compute("Sum(EMSX_WORKING) + Sum(EMSX_FILLED)", "EMSX_SEQUENCE='" + EMSX_SEQUENCE + "' and MSG_SUB_TYPE='R'"));

            // There will only be 1 row, but this is how datatables get filtered.
            DataRow[] myUpdateRows = dt_EMSX_API.Select("EMSX_SEQUENCE='" + EMSX_SEQUENCE + "' and MSG_SUB_TYPE='O'", "");
            for (int i = 0; i < myUpdateRows.Length; i++)
            {
                myUpdateRows[i]["ROUTED"] = Routed_Amount;
                String mySql = "Update EMSX_API " +
                               "Set ROUTED = " + Routed_Amount.ToString() + " " +
                               "Where  EMSX_SEQUENCE = " + EMSX_SEQUENCE + " " +
                               "And    MSG_SUB_TYPE = 'O' " +
                               "And    ROUTED <> " + Routed_Amount.ToString() + " ";
                SystemLibrary.SQLExecute(mySql);
            }

        } //UpdateRoutedAmount()

        /*
         * For this code to work, I need to have a Table with Start-of-Day positions, 
         *                        plus a table with the Fills - manual & EMSX.
         *                        The Fill Table would carry the EMSX_Sequence, so I can update.
         * Should I impliment a Tree? 
         *                        That way the user can expand to see each order.
         */
        private static void ProcessBlpEvent(Event evt)
        {
            // Local Variables
            ProcessedStruct Processed = new ProcessedStruct();
            SortedList<String, ProcessedStruct> ProcessedSequences = new SortedList<String, ProcessedStruct>();
            Boolean RunUpdatePositions = false;
            String mySql;
            DataRow[] myDeleteRows;
            Boolean DatabaseWasUpdated = false;
            String StartDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff");
            Boolean NeedToLoadOpenOrders = false;

            SystemLibrary.DebugLine("ProcessBlpEvent() - Start - " + StartDateTime);

            foreach (Bloomberglp.Blpapi.Message message in evt.GetMessages())
            {
                // Even though this code is on a seperate thread it seems to lock up the main code, so release resources on each loop.
                Application.DoEvents();
                SystemLibrary.DebugLine("ProcessBlpEvent - message Loop ");
                //Console.WriteLine("ProcessBlpEvent - message Loop {0}", message);

                if (message.HasElement("MSG_TYPE") && message.HasElement("MSG_SUB_TYPE") && message.HasElement("EVENT_STATUS"))
                {
                    String MSG_TYPE = message.GetElementAsString("MSG_TYPE").ToString().Trim();
                    if (MSG_TYPE == "E") // This indicates the msg is an EMSX API message.
                    {
                        // MSG_SUB_TYPE - O=Order, R=Route, F=Fill ('F' not implemented Nov 2012)
                        String MSG_SUB_TYPE = message.GetElementAsString("MSG_SUB_TYPE").ToString().Trim();
                        String EVENT_STATUS = message.GetElementAsString("EVENT_STATUS").ToString().Trim();
                        Int64 API_SEQ_NUM = message.GetElementAsInt64("API_SEQ_NUM");
                        String EMSX_SEQUENCE = message.GetElementAsString("EMSX_SEQUENCE").ToString().Trim();

                        switch (EVENT_STATUS)
                        {
                            case "1": // Heartbeat
                                SystemLibrary.DebugLine("HeartBeat");
                                if (inStartup && NeedToRun_NewEMSXOrders)
                                {
                                    SystemLibrary.DebugLine("NewEMSXOrders - Start (HeartBeat)");
                                    m_form.NewEMSXOrders(dt_Open_Orders);
                                    SystemLibrary.DebugLine("NewEMSXOrders - End (HeartBeat)");
                                    NeedToRun_NewEMSXOrders = false;
                                    inStartup = false;
                                }
                                break;
                            case "4": // INIT_PAINT - Latest Status when first connect
                            case "6": // NEW - when a new Order occurs after connected
                                // Code to Start the Record in the datatable
                                // Add this Data to the internal structure
                                DatabaseWasUpdated = CreateRecord(message, EMSX_SEQUENCE, MSG_SUB_TYPE, "NEW");
                                if (DatabaseWasUpdated)
                                    UpdateRoutedAmount(EMSX_SEQUENCE);
                                if (!ProcessedSequences.ContainsKey(EMSX_SEQUENCE))
                                {
                                    Processed.Action = "NEW";
                                    Processed.DatabaseWasUpdated = DatabaseWasUpdated;
                                    ProcessedSequences.Add(EMSX_SEQUENCE, Processed);
                                }
                                break;
                            case "7": // UPDATE - update to an existing Order on fields that can change
                                // Code to Update the Record in the datatable
                                // - If the record does not exist, then Create a new one.
                                if (!UpdateRecord(message, EMSX_SEQUENCE, MSG_SUB_TYPE, ref DatabaseWasUpdated))
                                    DatabaseWasUpdated = CreateRecord(message, EMSX_SEQUENCE, MSG_SUB_TYPE, "UPDATE");
                                if (DatabaseWasUpdated)
                                    UpdateRoutedAmount(EMSX_SEQUENCE);
                                if (!ProcessedSequences.ContainsKey(EMSX_SEQUENCE))
                                {
                                    Processed.Action = "UPDATE";
                                    Processed.DatabaseWasUpdated = DatabaseWasUpdated;
                                    ProcessedSequences.Add(EMSX_SEQUENCE, Processed);
                                }
                                break;
                            case "8": // DELETE - Deletion of Order, so mark existing record as Deleted if it exists

                                switch (MSG_SUB_TYPE)
                                {
                                    case "O":
                                        myDeleteRows = dt_EMSX_API.Select("EMSX_SEQUENCE='" + EMSX_SEQUENCE + "' and MSG_SUB_TYPE='" + MSG_SUB_TYPE + "'", "");
                                        foreach (DataRow drDelete in myDeleteRows)
                                        {
                                            drDelete["EVENT_STATUS"] = "DELETED";
                                            if (isSql2005)
                                            {
                                                // Code to mark the Record as Deleted in the datatable
                                                // - Should Also delete in the Database, but HOW do i distinguish if this was done by EMSX or the APP. Do I care?
                                                // - Could add "source" to this stored proc

                                                /* 
                                                 * This SQL makes sure we don't delete Filled Orders
                                                 * When a Market reopens, EMSX API 'Deletes' yesterdays trades.
                                                 * In the case of the Futures market, this can be 30 minutes after close & before back office has processed the Trades.
                                                 */
                                                String mySQL = "Select OrderRefID " +
                                                               "From Orders " +
                                                               "Where EMSX_Sequence = " + EMSX_SEQUENCE + " " +
                                                               "And	isNull(Order_Completed,'N') = 'N' " +
                                                               "And	isNull(ProcessedEOD,'N') = 'N' " +
                                                               "And	Not Exists (Select 'x' " +
                                                               "				From	Fills " +
                                                               "				Where	Fills.OrderRefID = Orders.OrderRefID " +
                                                               "				)";
                                                String OrderRefID = SystemLibrary.SQLSelectString(mySQL);
                                                // Cannot Delete processed or Filled Orders
                                                if (OrderRefID.Length > 0 && SystemLibrary.ToDecimal(drDelete["EMSX_FILLED"]) == 0)
                                                {
                                                    SystemLibrary.SQLExecute("Exec sp_ReverseOrder '" + OrderRefID + "'");
                                                    SystemLibrary.SQLExecute("Exec sp_SOD_Positions ");
                                                }
                                                else
                                                {
                                                    // Trying to work out what a EOD Delete looks like versus a normal delete.
                                                    //System.SystemLibrary.DebugLine(message.ToString());
                                                }
                                                // Mark as Deleted in the EMSX_API table
                                                mySql = "Update EMSX_API " +
                                                        "Set    EVENT_STATUS = 'DELETED' " +
                                                        "Where  EMSX_SEQUENCE='" + EMSX_SEQUENCE + "' and MSG_SUB_TYPE='" + MSG_SUB_TYPE + "' " +
                                                        "And    EVENT_STATUS <> 'DELETED'";
                                                SystemLibrary.SQLExecute(mySql);
                                            }
                                        }
                                        break;
                                    case "R":
                                        String EMSX_ROUTE_ID = message.GetElementAsInt32("EMSX_ROUTE_ID").ToString().Trim();

                                        myDeleteRows = dt_EMSX_API.Select("EMSX_SEQUENCE='" + EMSX_SEQUENCE + "' and MSG_SUB_TYPE='" + MSG_SUB_TYPE + "' and EMSX_ROUTE_ID=" + EMSX_ROUTE_ID, "");
                                        foreach (DataRow drDelete in myDeleteRows)
                                        {
                                            drDelete["EVENT_STATUS"] = "DELETED";

                                            if (isSql2005)
                                            {
                                                // Mark as Deleted in the EMSX_API table
                                                mySql = "Update EMSX_API " +
                                                        "Set    EVENT_STATUS = 'DELETED' " +
                                                        "Where  EMSX_SEQUENCE='" + EMSX_SEQUENCE + "' and MSG_SUB_TYPE='" + MSG_SUB_TYPE + "' and EMSX_ROUTE_ID=" + EMSX_ROUTE_ID + " " +
                                                        "And    EVENT_STATUS <> 'DELETED'";
                                                SystemLibrary.SQLExecute(mySql);
                                            }
                                        }
                                        break;
                                }
                                if (!ProcessedSequences.ContainsKey(EMSX_SEQUENCE))
                                {
                                    Processed.Action = "DELETE";
                                    Processed.DatabaseWasUpdated = true;
                                    ProcessedSequences.Add(EMSX_SEQUENCE, Processed);
                                }
                                break;
                            default:
                                // What is this record
                                SystemLibrary.DebugLine(message.ToString());
                                break;
                        }
                        // If the Sequence is wrong, then force a disconnect.
                        // It is up to the parent code to reconnect.
                        if (!TestSequence(MSG_SUB_TYPE, API_SEQ_NUM))
                            EMSX_APIDisconnect();

                    }
                }


                /*
                if (SystemLibrary.GetUserName() == "Colin Ritchie")
                {
                    System.SystemLibrary.DebugLine(message.ToString());
                }
                */
            }

            SystemLibrary.DebugLine("ProcessBlpEvent() - Loop End");

            // Send this to the database
            if (1==1) //!isSql2005)
            {
                DataTable dt_Send_EMSX_API = dt_EMSX_API.Copy();
                for (int i = dt_Send_EMSX_API.Rows.Count - 1; i >= 0 ; i--)
                {
                    // Only send the unchanged rows.
                    switch (dt_Send_EMSX_API.Rows[i].RowState.ToString())
                    {
                        case "Unchanged":
                            dt_Send_EMSX_API.Rows[i].Delete();
                            break;
                        default:
                            // For the underlying code and database to have this ticker.
                            String myTicker = SystemLibrary.ToString(dt_Send_EMSX_API.Rows[i]["EMSX_TICKER"]).Trim();
                            if (myTicker.Length>0)
                                m_form.CheckTickerExists(myTicker);
                            break;
                    }
                }
                dt_Send_EMSX_API.AcceptChanges();
                dt_EMSX_API.AcceptChanges();
                if (!isSql2005)
                {
                    if (dt_Send_EMSX_API.Rows.Count > 0)
                        SystemLibrary.SQLExecute("sp_EMSX_API_TableParameter", "@TempTable", ref dt_Send_EMSX_API);
                }
            }


            // Copy this to OutputWindow (Sort the DataTable)
            if (dt_EMSX_API.Rows.Count > 0 && OutputWindow != null && OutputWindow.Visible)
            {
                OutputWindow.dt_EMSX_API = dt_EMSX_API.Clone();
                OutputWindow.dt_EMSX_API = dt_EMSX_API.Select("", "EMSX_SEQUENCE, MSG_SUB_TYPE, EMSX_ROUTE_ID").CopyToDataTable();
                OutputWindow.Refresh_dgv_EMSX_API();
            }

            // See if the processed EMSX_Sequence lives in dt_Open_Orders
            //foreach (Object item in ProcessedSequences.Values)
            foreach (KeyValuePair<String, ProcessedStruct> kvp in ProcessedSequences)
            {
                String EMSX_Sequence = (String)kvp.Key;
                SystemLibrary.DebugLine("ProcessedSequences=" + EMSX_Sequence);
                DataRow[] dr = dt_Open_Orders.Select("EMSX_Sequence='" + EMSX_Sequence + "'");
                if (dr.Length < 1 && ((ProcessedStruct)kvp.Value).Action != "DELETE") // DELETE - Deletion of Order
                {
                    // The most basic reason this happens is that the dt_Open_Orders structure is not up to date.
                    NeedToLoadOpenOrders = true;
                }

                dr = dt_Open_Orders.Select("EMSX_Sequence='" + EMSX_Sequence + "'");
                if (dr.Length < 1 && ((ProcessedStruct)kvp.Value).Action != "DELETE")
                {
                    // Add to the Database
                    dr = dt_EMSX_API.Select("EMSX_SEQUENCE='" + EMSX_Sequence + "' And MSG_SUB_TYPE = 'R'");
                    if (dr.Length > 0)
                    {
                        // Add the Route to the Fills Table
                    }

                    if (isSql2005)
                    {
                        // Add the Order to the Orders Table
                        dr = dt_EMSX_API.Select("EMSX_SEQUENCE='" + EMSX_Sequence + "' And MSG_SUB_TYPE = 'O'");
                        if (dr.Length > 0)
                        {
                            // This can kick in as the Order is being placed and as such need to avoid creating a new record at that time.
                            // - Hence, release resource to other threads to see if this helps, plus avoid when EMSX_Sequence is null and rest of parameters look like the new order.
                            Application.DoEvents();
                            Decimal EMSX_Amount = SystemLibrary.ToDecimal(dr[0]["EMSX_AMOUNT"]);
                            String EMSX_SIDE = SendToBloomberg.GetFromEMSXSide(SystemLibrary.ToString(dr[0]["EMSX_SIDE"]));
                            if (EMSX_SIDE.StartsWith("S"))
                                EMSX_Amount = -EMSX_Amount;
                            mySql = "Insert into Orders (OrderRefID, EffectiveDate,BBG_Ticker, Exchange, " +
                                           "                    crncy, Quantity, Side, UserName, " +
                                           "                    UpdateDate, ProcessedEOD, ManualOrder, Order_Completed, EMSX_Sequence, CreatedDate) " +
                                           "Select  '" + EMSX_Sequence + "','" + Convert.ToDateTime(dr[0]["EMSX_DATE"]).ToString("dd-MMM-yyyy") + "','" + SystemLibrary.ToString(dr[0]["EMSX_TICKER"]) + "','" + SystemLibrary.ToString(dr[0]["EMSX_EXCHANGE"]) + "'," +
                                           "        crncy, " + EMSX_Amount.ToString() + ", '" + EMSX_SIDE + "','" + SystemLibrary.ToString(dr[0]["EMSX_TRADER"]) + "'," +
                                           "        '" + Convert.ToDateTime(dr[0]["EMSX_TIME_STAMP"]).ToString("dd-MMM-yyyy HH:mm:ss") + "','N','N','N','" + EMSX_Sequence + "','" + Convert.ToDateTime(dr[0]["EMSX_TIME_STAMP"]).ToString("dd-MMM-yyyy HH:mm:ss") + "' " +
                                           "From    Securities " +
                                           "Where   BBG_Ticker = '" + SystemLibrary.ToString(dr[0]["EMSX_TICKER"]) + "' " +
                                           "And Not Exists (Select 'x' from Orders where EMSX_Sequence = '" + EMSX_Sequence + "') " +
                                           "And Not Exists (Select 'x' from Orders " +
                                           "                Where   EMSX_Sequence is null " +
                                           "                And     EffectiveDate = '" + Convert.ToDateTime(dr[0]["EMSX_DATE"]).ToString("dd-MMM-yyyy") + "'" +
                                           "                And     BBG_Ticker = '" + SystemLibrary.ToString(dr[0]["EMSX_TICKER"]) + "' " +
                                           "                And     Quantity = " + EMSX_Amount + " " +
                                           "               ) ";
                            SystemLibrary.DebugLine(mySql);
                            SystemLibrary.SQLExecute(mySql);
                        }
                        // Flag that an sp_Update_Positions needs calling 
                        SystemLibrary.DebugLine("RunUpdatePositions - " + EMSX_Sequence);
                        RunUpdatePositions = true;
                    }
                }
                else if (dr.Length > 0)
                {
                    // Check that the dt_Open_Orders record is not a manual fill, in which case don't override
                    if (dr[0]["ManualFill"].ToString() != "Y")
                    {
                        // Compare Total_Quantity with Order Record EMSX_AMOUNT
                        // Compare Abs(Sum(Qty_Fill)) with Order Record ABS(EMSX_FILLED)
                        // Compare Sum() with Order Record ABS(ROUTED)
                    }
                    // Flag that an sp_Update_Positions needs calling 
                    if (((ProcessedStruct)kvp.Value).DatabaseWasUpdated)
                    {
                        SystemLibrary.DebugLine("RunUpdatePositions(DatabaseWasUpdated=true) - " + EMSX_Sequence);
                        RunUpdatePositions = true;
                    }
                }
                SystemLibrary.DebugLine("sp_FillsAllocation - Start");
                sp_FillsAllocation(EMSX_Sequence);
                SystemLibrary.DebugLine("sp_FillsAllocation - End");
            }
            if (RunUpdatePositions && isSql2005)
            {
                Application.DoEvents(); // This is here too reduce Deadlocks with other processes using SQL.
                SystemLibrary.DebugLine("exec sp_Update_Positions 'Y'");
                SystemLibrary.SQLExecute("exec sp_Update_Positions 'Y'");
            }
            if (NeedToLoadOpenOrders)
                LoadOpenOrders();


            // Pass the DataTable with the open Orders back to the parent form
            if (ProcessedSequences.Count > 0)
            {
                if (inStartup)
                    NeedToRun_NewEMSXOrders = true;
                else
                {
                    SystemLibrary.DebugLine("NewEMSXOrders - Start");
                    m_form.NewEMSXOrders(dt_Open_Orders);
                    SystemLibrary.DebugLine("NewEMSXOrders - End");
                }
            }

            // Copy this to OutputWindow (Sort the DataTable)
            if (OutputWindow != null && OutputWindow.Visible)
            {
                SystemLibrary.DebugLine("OutputWindow.dt_Open_Orders - Start");
                OutputWindow.dt_Open_Orders = dt_Open_Orders.Copy();
                OutputWindow.Refresh_dgv_Open_Orders();
                SystemLibrary.DebugLine("OutputWindow.dt_Open_Orders - End");
            }
            SystemLibrary.DebugLine("ProcessBlpEvent() - END - " + StartDateTime);

        } //ProcessBlpEvent()

        private static void sp_FillsAllocation(String EMSX_Sequence)
        {
             // Local Variables
             int myRow;
             int myLastRow;
             int myQty = 0;
             int mySumQty;
             int myQty_Routed;
             int mySumQty_Routed;
             int SignMultiplier = 1;

             int Amount;
             int Order_Quantity;
             int TotalOrder_Quantity;
             int Filled_Quantity;
             int Round_Lot_Size;
             int FillAmount;
             Decimal FillPrice;
             int RoutedAmount;
             Decimal Proportion;
             String Confirmed;
             String ManualFill;
             String MaxFillNo;
             String FundID;
             String PortfolioID;
             DataRow[] dr_Open_Orders;


             // Stage 1 - Make sure Delete records in dt_EMSX_API are reflected in dt_Open_Orders
             DataRow[] dr_EMSX_API = dt_EMSX_API.Select("EVENT_STATUS = 'DELETED' AND EMSX_FILLED = 0", "EMSX_Sequence,EMSX_ROUTE_ID");
             for (int i = 0; i < dr_EMSX_API.Length; i++)
             {
                 if (dr_EMSX_API[i]["MSG_SUB_TYPE"].ToString() == "O")
                 {
                     dr_Open_Orders = dt_Open_Orders.Select("EMSX_Sequence=" + dr_EMSX_API[i]["EMSX_Sequence"].ToString());
                     for (int j = dr_Open_Orders.Length - 1; j >= 0; j--)
                     {
                         dr_Open_Orders[j].Delete();
                     }
                     dt_Open_Orders.AcceptChanges();
                 }
                 else
                 {
                     dr_Open_Orders = dt_Open_Orders.Select("EMSX_Sequence=" + dr_EMSX_API[i]["EMSX_Sequence"].ToString() + " AND FillNo=" + dr_EMSX_API[i]["EMSX_ROUTE_ID"].ToString());
                     for (int j = dr_Open_Orders.Length - 1; j >= 0; j--)
                     {
                         dr_Open_Orders[j].Delete();
                     }
                     dt_Open_Orders.AcceptChanges();
                 }
             }

             // Stage 2 - Process Order Changes
            dr_Open_Orders = dt_Open_Orders.Select("EMSX_Sequence='" + EMSX_Sequence + "'");
            dr_EMSX_API = dt_EMSX_API.Select("EMSX_Sequence='" + EMSX_Sequence + "' AND MSG_SUB_TYPE = 'O'", "");
            if (dr_Open_Orders.Length > 0 && dr_EMSX_API.Length > 0)
            {
                // Compare Order Quantity
                if (Math.Abs(SystemLibrary.ToDecimal(dr_Open_Orders[0]["Total_Quantity"])) != Math.Abs(SystemLibrary.ToDecimal(dr_EMSX_API[0]["EMSX_AMOUNT"])))
                {
                    // Go back to the database as it will have the splits
                    LoadOpenOrders();
                    return;
                }
            }

             // Stage 3 - Process Fills
             dr_EMSX_API = dt_EMSX_API.Select("EMSX_Sequence='" + EMSX_Sequence + "' AND MSG_SUB_TYPE = 'R'", "EMSX_ROUTE_ID");

             // Make sure more than just an order
             if (dr_EMSX_API.Length < 1)
                return;

            // At this Stage dr_Open_Orders[] can be reflected in several Routes (ie. Different FillNo's)
            dr_Open_Orders = dt_Open_Orders.Select("EMSX_Sequence='" + EMSX_Sequence + "'");
            if (dr_Open_Orders.Length < 1)
            {
                // Deal with New Orders created after dt_Open_Orders was last updated
                LoadOpenOrders();
                dr_Open_Orders = dt_Open_Orders.Select("EMSX_Sequence='" + EMSX_Sequence + "'");
            }
            if (dr_Open_Orders.Length > 0)
            {
                // Make sure this is not already confirmed
                Confirmed = SystemLibrary.ToString(dt_Open_Orders.Compute("Max(Confirmed)", "EMSX_Sequence='" + EMSX_Sequence + "'"));
                ManualFill = SystemLibrary.ToString(dt_Open_Orders.Compute("Max(ManualFill)", "EMSX_Sequence='" + EMSX_Sequence + "'"));
                MaxFillNo = SystemLibrary.ToString(dt_Open_Orders.Compute("Max(FillNo)", "EMSX_Sequence='" + EMSX_Sequence + "'"));

                // Special case if Manual Fill
                if (ManualFill == "Y")
                {
                    Confirmed = SystemLibrary.ToString(dt_Open_Orders.Compute("Max(Order_Completed)", "EMSX_Sequence='" + EMSX_Sequence + "'"));

                    for (int i = 0; i < dr_Open_Orders.Length; i++)
                    {
                        // TODO(1) DOES THIS ALTER THE DATATABLE?
                        dr_Open_Orders[i]["Confirmed"] = 'N';
                    }
                }

                if (Confirmed == "Y")
                    return;
            }
            else
            {
                // Should only get here if the Orders_Split record does not exist.
                return;
            }

            /*
             * At this point:
             * 1) We need to get the "Total_Quantity is the Full Order" and "Qty_Order is the Order Split Quantity for each FundID/PortfolioID"
             * 2) Delete existing records - as we have caputured all the data we need in dr_Open_Orders?
             */
            DataTable dt_Copy = dt_Open_Orders.Copy();
            dr_Open_Orders = dt_Copy.Select("EMSX_Sequence='" + EMSX_Sequence + "' AND FillNo=" + MaxFillNo, "Qty_Order");
            int RowCount = dt_Open_Orders.Rows.Count;
            for (int i = RowCount - 1; i >= 0; i--)
                if (dt_Open_Orders.Rows[i]["EMSX_Sequence"].ToString() == EMSX_Sequence)
                    dt_Open_Orders.Rows[i].Delete();
            dt_Open_Orders.AcceptChanges();

            /*
             * Now Take the Fills for this order from dr_EMSX_API[] and rebuild the dt_Open_Orders rows
             * - This is the Equivalent of "DECLARE Outer_Cursor CURSOR FOR" in the STored Procedure sp_FillsAllocation
             */
            Filled_Quantity = 0;
            myLastRow = dr_Open_Orders.Length;
            // The dr_EMSX_API record has no concept of Long/Short so Quantity is always +ve, hence need a multiplier.
            if (dr_Open_Orders.Length > 0)
            {
                if (SystemLibrary.ToInt32(dr_Open_Orders[0]["Total_Quantity"]) < 0)
                    SignMultiplier = -1;
                else
                    SignMultiplier = 1;

            }

            foreach (DataRow dr_Fill in dr_EMSX_API)
            {
                myRow = 0; mySumQty = 0; mySumQty_Routed = 0;
                // TODO(1) I dont think I distinguish yet between Routed & Amount?
                RoutedAmount = SystemLibrary.ToInt32(dr_Fill["EMSX_AMOUNT"]) * SignMultiplier;
                FillAmount = SystemLibrary.ToInt32(dr_Fill["EMSX_FILLED"]) * SignMultiplier;
                // 20130717 - Added a test for REJECTED as well as CANCEL and CXLR
                if (SystemLibrary.ToString(dr_Fill["EMSX_STATUS"]).ToUpper() == "CANCEL" || SystemLibrary.ToString(dr_Fill["EMSX_STATUS"]).ToUpper() == "CXLREQ" || SystemLibrary.ToString(dr_Fill["EMSX_STATUS"]).ToUpper() == "REJECTED")
                    RoutedAmount = FillAmount;
                FillPrice =  SystemLibrary.ToDecimal(dr_Fill["EMSX_AVG_PRICE"]);
                foreach (DataRow dr_Split in dr_Open_Orders)
                {
                    myRow = myRow + 1;
                    FundID = SystemLibrary.ToString(dr_Split["FundID"]);
                    PortfolioID = SystemLibrary.ToString(dr_Split["PortfolioID"]);
                    Amount = SystemLibrary.ToInt32(dr_Split["Total_Quantity"]);
                    TotalOrder_Quantity = SystemLibrary.ToInt32(dr_Split["Qty_Order"]);
                    Order_Quantity = SystemLibrary.ToInt32(dr_Split["Qty_Order"]) - SystemLibrary.ToInt32(dt_Open_Orders.Compute("Sum(Qty_Fill)","EMSX_Sequence="+EMSX_Sequence+" AND FundID="+FundID+" AND PortfolioID="+PortfolioID));
                    Round_Lot_Size = SystemLibrary.ToInt32(dr_Split["Round_Lot_Size"]);
                    //Select '@myRow=',@myRow,'@myLastRow=',@myLastRow,'@Order_Quantity=',@Order_Quantity,'@Amount=',@Amount,'@FillAmount=',@FillAmount,'@RoutedAmount=',@RoutedAmount,'@Filled_Quantity=',@Filled_Quantity
                    if (Amount==0)
                        Proportion = 0;
                    else
                        Proportion = (Decimal)TotalOrder_Quantity / (Decimal)Amount;

                    if (myRow == myLastRow)
                    {
                        myQty = FillAmount - mySumQty;
                        myQty_Routed = RoutedAmount - mySumQty_Routed;

                        // Insert Row
                        DataRow dr = dt_Open_Orders.NewRow();
                        // Fill the known columns with existing data
                        for (int i=0;i<dr_Split.ItemArray.Length;i++)
                            dr[i] = dr_Split[i];
                        // Override with new data
                        dr["Fill_Quantity"] = myQty;
                        dr["Qty_Fill"] = myQty;
                        dr["Fill_Price"] = FillPrice;
                        dr["Qty_Routed"] = myQty_Routed;
                        dr["FillNo"] = dr_Fill["EMSX_ROUTE_ID"];
                        dt_Open_Orders.Rows.Add(dr);
                        //SystemLibrary.DebugLine("LastRow(myQty={4},Proportion={0},Amount={1},TotalOrder_Quantity={2},Order_Quantity={3},FillPrice={5}",
                        //                    Proportion, Amount, TotalOrder_Quantity, Order_Quantity, myQty, FillPrice);
                    }
                    else
                    {
                        // Get the Existing Order size 			
                        if (Amount - Filled_Quantity == FillAmount)
                        {
                            // Filled
                            myQty = Order_Quantity;
                        }
                        else
                        {
                            // Stored Procedure uses Cast() which does a Floor
                            myQty = SendToBloomberg.RoundLot(0, Convert.ToInt32(Math.Floor(FillAmount * Proportion)), Round_Lot_Size);
                        }

                        if ( Amount - Filled_Quantity == RoutedAmount)
                        {
	                        // Filled
	                        myQty_Routed = Order_Quantity;
                        }
                        else
                        {
                            // Stored Procedure uses Cast() which does a Floor
                            myQty_Routed = SendToBloomberg.RoundLot(0, Convert.ToInt32(Math.Floor(RoutedAmount * Proportion)), Round_Lot_Size);
                        }

                        // Insert Row
                        DataRow dr = dt_Open_Orders.NewRow();
                        // Fill the known columns with existing data
                        for (int i = 0; i < dr_Split.ItemArray.Length; i++)
                            dr[i] = dr_Split[i];
                        // Override with new data
                        dr["Fill_Quantity"] = myQty;
                        dr["Qty_Fill"] = myQty;
                        dr["Fill_Price"] = FillPrice;
                        dr["Qty_Routed"] = myQty_Routed;
                        dr["FillNo"] = dr_Fill["EMSX_ROUTE_ID"];
                        dt_Open_Orders.Rows.Add(dr);
                        //SystemLibrary.DebugLine("Row(myQty={4},Proportion={0},Amount={1},TotalOrder_Quantity={2},Order_Quantity={3},FillPrice={5}",
                        //                    Proportion, Amount, TotalOrder_Quantity, Order_Quantity, myQty, FillPrice);


                        mySumQty = mySumQty + myQty;
                        mySumQty_Routed = mySumQty_Routed + myQty_Routed;
                    }

                    Filled_Quantity = Filled_Quantity + myQty;
                }
            }
            //dt_Open_Orders.AcceptChanges();

        } //sp_FillsAllocation()

        public static void LocalDebugLine(object myObj)
        {
            //if (myObj.ToString().Contains("HeartBeat") || myObj.ToString().Contains("NewEMSXOrders"))
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - " + myObj);
        }

    }
}
