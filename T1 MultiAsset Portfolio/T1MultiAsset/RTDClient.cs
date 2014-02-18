using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using BBRTDLib;
using bbrtd = BBRTDLib.bbrtdClass;
using Event = BBRTDLib.IRTDUpdateEvent;

namespace T1MultiAsset
{
    ///
    /// This console application is a client for an
    /// Excel real-time data (RTD) server. It works
    /// by emulating the low level method calls
    /// and interactions that Excel makes when
    /// using a RTD.
    ///
    class RTDClient
    {
        // ProgIDs for COM components.
        private const String RTDProgID = "Bloomberg.RTD";
        private const String RTDUpdateEventProgID = "Bloomberg.DataUpdate";
        //IRTDUpdateEvent
        // UpdateNotify

        // Dummy topic.
        private const int topicID = 12345;
        private const String topic = "BHP AU Equity";

        // Test both in-process (DLL) and out-of-process (EXE)
        // RTD servers.
        static void myMain(string[] args)
        {
            SystemLibrary.DebugLine("Test in-process (DLL) RTD server.");
            TestMyRTD(RTDProgID, RTDUpdateEventProgID);

            SystemLibrary.DebugLine("Press enter to exit ...");
            Console.ReadLine();
        }

        //processUpdateEvent

        sealed class ColinWasHere : IRTDUpdateEvent
        {
            private int _HeartbeatInterval=12345;
            //public Form1 myParent;
            public int HeartbeatInterval 
            {
                set { _HeartbeatInterval = value; }
                get { return _HeartbeatInterval; }
            }
            
            public void UpdateNotify()
            {
                SystemLibrary.DebugLine("Inside UpdateNotify");
            }

            public void Disconnect()
            {
                SystemLibrary.DebugLine("Inside Disconnect");
            }

            public ColinWasHere(String myHello)
                : base()
            {
                SystemLibrary.DebugLine("Inside ColinWasHere("+myHello+")");
                //myParent = myPassParent;
            }
            /*
            static void Main()
            {
                IRTDUpdateEvent obj = new ColinWasHere();
                obj.UpdateNotify();
            }
             * */
        }

        

        public static void BBGRTD_Connect()
        {
            // Expect to pass in an array of tickers and an array of fields
            //BBRTDLib.bbrtdClass.ServerStart(BBRTDLib.IRTDUpdateEvent);

            // Create rtdServer
            bbrtd d_bbrtd;
            int TopicId = 0;

            bool GetNetValues = true;
            
            //System.Array Topics = new Strings[3];
            //string[] Topics = new string[3];
            Array Topics = Array.CreateInstance(Type.GetType("System.Object"), 2); //.Object   .String[]
            //System.Array.CreateInstance(String, params long[])
            String[] Stocks = new String[2];
            Stocks[0] = "BHP AU Equity";
            Stocks[1] = "ANZ AU Equity";
            String[] myFields = new String[2];
            myFields[0] = "NAME";
            myFields[1] = "LAST_PRICE";
            Topics.SetValue(Stocks, 0);
            Topics.SetValue(myFields, 1);
            //Topics.SetValue("{LAST_PRICE}", 2);
            //Topics.SetValue("NAME",2);
            //Topics.SetValue("PX_LAST",3);
            
            //IRTDUpdateEvent CallbackObject;
            d_bbrtd = new bbrtd();

            int myResult = d_bbrtd.ServerStart(new ColinWasHere("Hello"));
            SystemLibrary.DebugLine("ret for 'ServerStart()' = "+ myResult.ToString());

            object test1 = d_bbrtd.ConnectData(TopicId,ref Topics,ref GetNetValues);
            SystemLibrary.DebugLine("Object="+test1.ToString());
            int iBeat = d_bbrtd.Heartbeat();
            SystemLibrary.DebugLine("Heartbeat="+iBeat.ToString());
            Array Out = d_bbrtd.RefreshData(ref TopicId);
            if (Out==null)
                SystemLibrary.DebugLine("Out=null");
            else
                SystemLibrary.DebugLine("Out=" + Out.ToString());
            d_bbrtd.DisconnectData(TopicId);

            //Event colin = new eve;
            //BBRTDLib.IRtdServer rtdServer = new BBRTDLib.IRtdServer();
            //BBRTDLib.IRTDUpdateEvent updateEvent; // = new BBRTDLib.IRTDUpdateEvent();
            //int myResult = rtdServer.ServerStart(new EventHandler(processUpdateEvent));
            //IRTDUpdateEvent Colin = IRTDUpdateEvent.
            //int myResult = d_bbrtd.ServerStart(Colin);
            //BBRTDLib.bbrtdClass.ServerStart(BBRTDLib.IRTDUpdateEvent)


            // Terminate the server
            d_bbrtd.ServerTerminate();

        } //BBGRTD_Connect()

        // Test harness that emulates the interaction of
        // Excel with an RTD server.
        public static void TestMyRTD(String rtdID, String eventID)
        {
            //try
            {
                // Create the RTD server.
                Type rtd;
                Object rtdServer = null;
                rtd = Type.GetTypeFromProgID(rtdID);
                rtdServer = Activator.CreateInstance(rtd);
                SystemLibrary.DebugLine("rtdServer = " + rtdServer.ToString());

                // Create a callback event.
                Type update;
                Object updateEvent = null;
                //eventID = "Bloomberg.RTD";
                //IRTDUpdateEvent
                // UpdateNotify
                update = Type.GetTypeFromProgID(eventID);
                String[] myTest = new String[10];
                myTest[0] = "Bloomberg.RTD";
                if (update == null)
                {
                    update = Type.GetTypeFromProgID(eventID);
                }

                updateEvent = Activator.CreateInstance(update);
                SystemLibrary.DebugLine("updateEvent = " + updateEvent.ToString());

                // Start the RTD server passing in the callback
                // object.
                Object[] param = new Object[1];
                param[0] = updateEvent;
                MethodInfo method = rtd.GetMethod("ServerStart");
                Object ret; // Return value.
                ret = method.Invoke(rtdServer, param);
                SystemLibrary.DebugLine("ret for 'ServerStart()' = " + ret.ToString());
                

                // Request data from the RTD server.
                Object[] topics = new Object[2];
                topics[0] = topic;
                topics[1] = "NAME";
                Boolean newData = true; // Request new data, not cached data.
                param = new Object[3];
                //cfr 20110223 Object[] param = new Object[3]; // cfr 20110223
                param[0] = topicID;
                param[1] = topics;
                param[2] = newData;
                method = rtd.GetMethod("ConnectData");
                ret = method.Invoke(rtdServer, param); //cfr 20110223 
                //cfr 20110223 MethodInfo method = rtd.GetMethod("ConnectData");
                //cfr 20110223 Object ret = method.Invoke(rtdServer, param); //cfr 20110223 
                SystemLibrary.DebugLine("ret for 'ConnectData()' = " + ret.ToString());

                // Loop and wait for RTD to notify (via callback) that
                // data is available.
                int count = 0;
                do
                {
                    count++;

                    // Check that the RTD server is still alive.
                    Object status;
                    param = null;
                    method = rtd.GetMethod("Heartbeat");
                    status = method.Invoke(rtdServer, param);
                    SystemLibrary.DebugLine("status for 'Heartbeat()' = " + status.ToString());

                    // Get data from the RTD server.
                    int topicCount = 0;
                    param = new Object[1];
                    param[0] = topicCount;
                    method = rtd.GetMethod("RefreshData");
                    Object[,] retval = new Object[2, 1];
                    retval = (Object[,])method.Invoke(rtdServer, param);
                    SystemLibrary.DebugLine("retval for 'RefreshData()' = " + retval[1, 0].ToString());

                    // Wait for 2 seconds before getting
                    // more data from the RTD server. This
                    // it the default update period for Excel.
                    // This client can requested data at a
                    // much higher frequency if wanted.
                    Thread.Sleep(2000);

                } while (count < 5); // Loop 5 times for test.

                // Disconnect from data topic.
                param = new Object[1];
                param[0] = topicID;
                method = rtd.GetMethod("DisconnectData");
                method.Invoke(rtdServer, param);

                // Shutdown the RTD server.
                param = null;
                method = rtd.GetMethod("ServerTerminate");
                method.Invoke(rtdServer, param);
            }
            /*catch (Exception e)
            {
                SystemLibrary.DebugLine("Error: {0} ", e.Message);
            }*/
        }
    }
}
