using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using Bloomberglp.Blpapi;
using System.Windows.Forms;


namespace T1MultiAsset
{
    class SendToBloomberg
    {
        // Public Variables
        public static DataTable dt_SendToBloomberg = new DataTable();

        // Bloomberg EMSX API variables
        private static SessionOptions m_sessionOptions;
        private static Session m_session;
        private static Service m_service;

        public static void InitDataTable()
        {
            // AOL,US,C,,USD,,MKT,B,1000,,DAY,,A0000007,,,,,,,,,,,,,,,,,,,,,,,,EQUITY,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            // Initialise the Send To Bloomberg Datatable
            // - leave blank columns to match output format, so can extend later.
            // - See Upload Manual for more on fields

            // Used as at 14-Feb-2011
            dt_SendToBloomberg.Columns.Add("BloombergTicker");
            // Used as at 14-Feb-2011
            dt_SendToBloomberg.Columns.Add("Exchange");
            dt_SendToBloomberg.Columns.Add("isFuture");
            // Used as at 14-Feb-2011 - set to C
            dt_SendToBloomberg.Columns.Add("IdentifierType"); // C = Cusip, S=Sedol
            dt_SendToBloomberg.Columns.Add("Identifier");   // Required if using identifier other than Sedol
            // Used as at 14-Feb-2011
            dt_SendToBloomberg.Columns.Add("Currency");
            dt_SendToBloomberg.Columns.Add("Broker");
            // Used as at 14-Feb-2011
            dt_SendToBloomberg.Columns.Add("OrderType"); //MKT = market or LMT = Limit
            // Used as at 14-Feb-2011
            dt_SendToBloomberg.Columns.Add("Side"); // B = Buy, S = Sell, SS = Sell Short, BS = Cover Buy, SX = Short Sell Exempt
            // Used as at 14-Feb-2011
            dt_SendToBloomberg.Columns.Add("OrderQuantity", System.Type.GetType("System.Int32")); // Always positive # of shares
            dt_SendToBloomberg.Columns.Add("Limit", System.Type.GetType("System.Decimal"));
            // Used as at 14-Feb-2011
            dt_SendToBloomberg.Columns.Add("TimeinForce"); // DAY, GTC
            dt_SendToBloomberg.Columns.Add("FundName"); // For use by Bloomberh PTS clients.
            // Used as at 14-Feb-2011
            dt_SendToBloomberg.Columns.Add("OrderRefID");
            dt_SendToBloomberg.Columns.Add("E-CrossnetAllocationType", System.Type.GetType("System.Int32")); //NA
            dt_SendToBloomberg.Columns.Add("E-CrossnetMinimumFill", System.Type.GetType("System.Int32"));//NA
            dt_SendToBloomberg.Columns.Add("E-CrossnetAccount");
            dt_SendToBloomberg.Columns.Add("E-CrossnetCrossType");
            dt_SendToBloomberg.Columns.Add("TradebookStrategyFlag");
            dt_SendToBloomberg.Columns.Add("TradebookPTYE");
            dt_SendToBloomberg.Columns.Add("Trade1bookPSPR", System.Type.GetType("System.Int32"));
            dt_SendToBloomberg.Columns.Add("TradebookPCLF", System.Type.GetType("System.Int32"));
            dt_SendToBloomberg.Columns.Add("TradebookDISC", System.Type.GetType("System.Int32"));
            dt_SendToBloomberg.Columns.Add("TradebookDCAM", System.Type.GetType("System.Int32"));
            dt_SendToBloomberg.Columns.Add("TradebookDCFL", System.Type.GetType("System.Int32"));
            dt_SendToBloomberg.Columns.Add("PTSFirmTrader");
            dt_SendToBloomberg.Columns.Add("PTSSettlementDate", System.Type.GetType("System.Int32"));
            dt_SendToBloomberg.Columns.Add("PTSInstruction");
            dt_SendToBloomberg.Columns.Add("PTSInvestorId");
            dt_SendToBloomberg.Columns.Add("PTSreasonCode");
            dt_SendToBloomberg.Columns.Add("OrderNumber");
            dt_SendToBloomberg.Columns.Add("ExecutingBroker");
            dt_SendToBloomberg.Columns.Add("ExecutionQuantity");
            dt_SendToBloomberg.Columns.Add("ExecutionPrice");
            dt_SendToBloomberg.Columns.Add("ExecutionAsofDate");
            dt_SendToBloomberg.Columns.Add("ExecutionAsofTime");
            dt_SendToBloomberg.Columns.Add("ExecutionNumber");
            // Used as at 14-Feb-2011
            dt_SendToBloomberg.Columns.Add("YellowKey");

            // Extra fields - Extracted from LSTM as documentation stopped at Yellow Key
            dt_SendToBloomberg.Columns.Add("Account");
            dt_SendToBloomberg.Columns.Add("ExecInstruction");
            dt_SendToBloomberg.Columns.Add("HandlingInstruction");
            dt_SendToBloomberg.Columns.Add("GTDDate");
            dt_SendToBloomberg.Columns.Add("StopPrice");
            dt_SendToBloomberg.Columns.Add("JPTradeDate");
            dt_SendToBloomberg.Columns.Add("ExecJPTradeAmount");
            dt_SendToBloomberg.Columns.Add("ExecJPCommisionAmount");
            dt_SendToBloomberg.Columns.Add("ExecJPTaxAmount");
            dt_SendToBloomberg.Columns.Add("ExecJPOtherMarketFees");
            dt_SendToBloomberg.Columns.Add("ExecJPNetMoney");
            dt_SendToBloomberg.Columns.Add("ExecYield");
            dt_SendToBloomberg.Columns.Add("ExecDiscount");
            dt_SendToBloomberg.Columns.Add("ShortNote1");
            dt_SendToBloomberg.Columns.Add("ShortNote2");
            dt_SendToBloomberg.Columns.Add("ShortNote3");
            dt_SendToBloomberg.Columns.Add("ShortNote4");
            dt_SendToBloomberg.Columns.Add("LongNote1");
            dt_SendToBloomberg.Columns.Add("LongNote2");
            dt_SendToBloomberg.Columns.Add("LongNote3");
            dt_SendToBloomberg.Columns.Add("LongNote4");
            dt_SendToBloomberg.Columns.Add("AgencyPrincipalFlag");
            dt_SendToBloomberg.Columns.Add("TradeFlatFlag");
            dt_SendToBloomberg.Columns.Add("AccruedInterest");
            dt_SendToBloomberg.Columns.Add("Spread");
            dt_SendToBloomberg.Columns.Add("BenchMark");
            dt_SendToBloomberg.Columns.Add("BenchMarkYellowKey");
            dt_SendToBloomberg.Columns.Add("BenchMarkPrice");
            dt_SendToBloomberg.Columns.Add("BenchMarkYield");
            dt_SendToBloomberg.Columns.Add("StrategyFlag");
            dt_SendToBloomberg.Columns.Add("UpdateCashSettlementDateFlag");
            dt_SendToBloomberg.Columns.Add("CompetingQuoteFlag");
        } // InitDataTable()


        public static void Send(String myPath, String myFileName, String myUUID, String mySerialNumber, String myBasket)
        {
            // 
            // Procedure:   SendToBloomberg
            //
            // Purpose:     Prepare a file for Bloomberg upload and send it via openfl.exe
            //
            // Written:     SoftPark21/Quantitative Systems 11-Feb-2011
            //
            // Modified:
            //

            // Local Variables
            String mySendStr = "";
            Char LineFeed = '\n';
            Int32 myRows = dt_SendToBloomberg.Rows.Count;
            Int32 myColumns = dt_SendToBloomberg.Columns.Count;


            if (myRows < 1 || myColumns < 1)
                return;

            // Create new file and open it for read and write, 
            // - If the file exists throw exception, as can catch here rather than no idea why bloomberg does not accept
            // - Have adopted using as C# then cleans up properly
            using (FileStream stream = new FileStream(myPath + myFileName, FileMode.CreateNew, FileAccess.Write))
            {
                //using (BinaryWriter writer = new BinaryWriter(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write("START-OF-FILE");
                    writer.Write(LineFeed); // Line Feed
                    writer.Write("LOGIN=");
                    writer.Write(LineFeed); // Line Feed
                    writer.Write("SN=" + mySerialNumber);
                    writer.Write(LineFeed); // Line Feed
                    writer.Write("WS=");
                    writer.Write(LineFeed); // Line Feed
                    writer.Write("UUID=" + myUUID);
                    writer.Write(LineFeed); // Line Feed
                    writer.Write("PRICINGNUMBER=0");
                    writer.Write(LineFeed); // Line Feed
                    writer.Write("BASKETID=A-" + myBasket + "-" + myUUID);
                    writer.Write(LineFeed); // Line Feed91
                    writer.Write("DELIMITER=,");
                    writer.Write(LineFeed); // Line Feed
                    // Add the rows
                    for (int i = 0; i < myRows; i++)
                    {
                        mySendStr = "";
                        for (int j = 0; j < myColumns; j++)
                        {
                            mySendStr = mySendStr + dt_SendToBloomberg.Rows[i][j].ToString() + ",";
                            /*
                            writer.Write("IBM,US,C,,USD,,MKT,B,1000,,DAY,,1,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                            writer.Write(LineFeed); // Line Feed
                            writer.Write("MSFT,US,C,,USD,,MKT,SS,500,,DAY,,1,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                            writer.Write(LineFeed); // Line Feed
                            */
                        }
                        writer.Write(mySendStr.ToUpper().Substring(0, mySendStr.Length - 1)); // Needs to be all upper case
                        writer.Write(LineFeed); // Line Feed
                    }
                    writer.Write("END-OF-FILE");
                    // NO Line Feed at end of file
                    writer.Close();

                    // Now send to Bloomberg
                    System.Diagnostics.Process.Start(@"C:\blp\wintrv\openfl", @" -P ""C:\blp\wintrv\@Profile.lmu"" -S """ + myPath + myFileName + "");
                }
            }


        }  // SendToBloomberg()


        public static String GetSide(String Ticker, String YellowKey, String Country, int ExistingOrder, int myQty)
        {
            // 
            // Procedure:   GetSide
            //
            // Purpose: A generic routine to work out Side.
            //          May go to a stored procedure with Rules like Singapore is a S versus a SS even when Short.
            //
            //  Rule:   
            //      1) Non-Equities are just B/S.
            //      2) B = Buy, S = Sell, SS = Sell Short, BS = Cover Buy, SX = Short Sell Exempt
            //

            // Local Variables
            String RetVal = "";


            if ((YellowKey.ToUpper() != "EQUITY" || 
                  Country.ToUpper() == "SINGAPORE" ||
                  (YellowKey == "EQUITY" && Ticker.IndexOf(' ') > -1))
                )
            {
                // Non Equity, Or Singapore Exchange or Equity Option
                if (myQty < 0)
                    RetVal = "S";
                else
                    RetVal = "B";
            }
            else
            {
                // Ordinary Equities
                if (ExistingOrder <= 0 && myQty < 0)
                    RetVal = "SS"; // Getting Shorter
                else if (ExistingOrder < 0 && myQty > 0)
                    RetVal = "BS"; // Closing Short
                else if (ExistingOrder >= 0 && myQty > 0)
                    RetVal = "B"; // Getting Longer
                else if (ExistingOrder > 0 && myQty < 0)
                    RetVal = "S"; // Closing Long
                else
                    RetVal = ""; // Should not get here??
            }

            return (RetVal);

        } //GetSide()

        public static int RoundLot(int ExistingOrder, int myQty, int Round_Lot_Size)
        {
            // 
            // Procedure:   RoundLot
            //
            // Purpose:     Take a proposed Trade Quantity and size it for Round_Lot_Size
            //
            // Written:     Quantitative Systems 11-Feb-2011
            //
            // Modified:    Quantitative Systems 10-Apr-2011
            //              Reduced code time, but doing return if Round_Lot_Size==1
            //
            //              Quantitative Systems 14-Apr-2011
            //              Special case if closing position ignore Round_Lot_Size
            //

            // Local Variables
            int RetVal = myQty;
            int FullOrder = ExistingOrder + myQty;
            int FullDirection = Math.Sign(FullOrder);
            int NewDirection = Math.Sign(myQty);


            // Now work of Round_Lot_Size==1
            if (Round_Lot_Size == 1 || Round_Lot_Size <=0)
                return (myQty);

            // Allow for closing positions
            if (myQty == -ExistingOrder)
                return (myQty);

            switch (FullDirection)
            {
                case 0:
                case 1:
                    // Long
                    switch (NewDirection)
                    {
                        case 1:
                            // Increase Long
                            RetVal = Convert.ToInt32(Math.Floor(Convert.ToDouble(myQty) / Convert.ToDouble(Round_Lot_Size))) * Round_Lot_Size;
                            break;
                        default:
                            // Decrease Long
                            RetVal = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(myQty) / Convert.ToDouble(Round_Lot_Size))) * Round_Lot_Size;
                            break;
                    }
                    break;
                case -1:
                    // Long
                    switch (NewDirection)
                    {
                        case 1:
                            // Buy Back Short
                            RetVal = Convert.ToInt32(Math.Floor(Convert.ToDouble(myQty) / Convert.ToDouble(Round_Lot_Size))) * Round_Lot_Size;
                            break;
                        default:
                            // Increase Short
                            RetVal = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(myQty) / Convert.ToDouble(Round_Lot_Size))) * Round_Lot_Size;
                            break;
                    }
                    break;
            }


            return (RetVal);

        } //RoundLot()

        public static String EMSTickerCheck(String inTicker)
        {
            // Cleans up the Ticker for EMSX API.

            // Local Variables
            String Ticker = "";
            String Exch = "";
            String YellowKey = "";



            SendToBloomberg.EMSTickerSplit(inTicker, ref Ticker, ref Exch, ref YellowKey);

            return (Ticker + " " + Exch + " " + YellowKey);
        }

        public static void EMSTickerSplit(String TheSecurity, ref String Ticker, ref String Exch, ref String YellowKey)
        {
            // 
            // Procedure:   EMSTickerSplit
            //
            // Purpose:     Split a ticker up, so can supply correct fields for EMSX
            //
            // Rules:   IBM US Equity / BHP AU 4/11 C47.5 Equity / SPK1C 1310 Index / W Z2 COMD Comdty / W Z2 COMDTY
            //
            // Written:     SoftPark21/Quantitative Systems 18-Apr-2011
            //
            // Modified:    
            //
            
            //

            // Local Variables
            String TickerExch;

            TheSecurity = TheSecurity.ToUpper();
            YellowKey = SystemLibrary.BBGDataType(TheSecurity); // Use function BBGDataType(), not YellowKey

            switch(YellowKey.ToUpper())
            {
                case "COMDTY":
                case "INDEX":
                    // Remove " COMB " from commodity/index - it means multiple exchanges.
                    TickerExch = TheSecurity.Replace(" COMB "," ");
                    Ticker = SystemLibrary.BBGTicker(TickerExch);
                    Exch = "";
                    break;
                case "EQUITY":
                    // Get the TickerExch (ie. Ex YellowKey)
                    TickerExch = SystemLibrary.BBGTicker(TheSecurity);
                    String[] myStr = TickerExch.Split(' ');
                    if (myStr.Length <= 1)
                    {
                        // Shouldn't get here, but need to code for it.
                        Ticker = TickerExch;
                        Exch = "";
                    }
                    else if (myStr.Length == 2)
                    {
                        // Normal Equity
                        Ticker = myStr[0];
                        Exch = myStr[1];
                    }
                    else
                    {
                        // Options will have more than one space, and not sure how to represent Ticker in Upload?
                        // - BHP AU 4/11 C47.5
                        Ticker = TickerExch;
                        Exch = myStr[1];
                    }
                    break;
                default:
                    Ticker = TheSecurity; // SystemLibrary.BBGTicker(TheSecurity);
                    Exch = "";
                    YellowKey = "";
                    break;
            }
        } //EMSTickerSplit()

        public static String GetNextOrderRefID()
        {
            return (SystemLibrary.SQLSelectString("Exec sp_GetNextId 'OrderRefID'"));

        } // GetNextOrderRefID

        public static String GetNextFileName(ref String Basket)
        {
            // Local Variables Colin000009.inc";
            String RetVal;
            Char FoundChar;

            RetVal = SystemLibrary.SQLSelectString("Exec sp_GetNextId 'BasketNo'");
            // Strip off leading text and leading zeros to get the BasketNo
            Char[] myArray = RetVal.ToCharArray();
            for (int i = myArray.Length - 1; i >= 0; i--)
            {
                FoundChar = myArray[i];
                if (FoundChar>='0' && FoundChar<='9')
                    Basket = FoundChar.ToString() + Basket;
                else
                    break;
            }
            Basket = Convert.ToInt16(Basket).ToString().Trim();
            return (RetVal+".inc");

        } // GetNextFileName

        #region Bloomberg EMSX API

        public static String GetEMSXSide(String inSide)
        {
            String RetVal = "BUY";
            switch (inSide)
            {
                case "B":
                    RetVal = "BUY";
                    break;
                case "S":
                    RetVal = "SELL";
                    break;
                case "SS":
                    RetVal = "SHRT";
                    break;
                case "BC":
                    RetVal = "COVR";
                    break;
            }

            return (RetVal);
        } //GetEMSXSide()

        public static String GetFromEMSXSide(String inEMSXSide)
        {
            String RetVal = "B";
            switch (inEMSXSide)
            {
                case "BUY":
                case "BUYM": // Buy on a Negative or down-tick
                    RetVal = "B";
                    break;
                case "SELL":
                case "SLPL": // Sell on a Positive or up-tick
                    RetVal = "S";
                    break;
                case "SHRT":
                case "SHRX": // Sell Short exempt
                    RetVal = "SS";
                    break;
                case "COVR":
                    RetVal = "BC";
                    break;
            }

            return (RetVal);
        } //GetEMSXSide()

        public static String EMSXAPI_Modify(String OrderRefID, String inTicker, Int32 NewQuantity)
        {
            return (EMSXAPI_Modify(OrderRefID, "", inTicker, NewQuantity));
        } //EMSXAPI_Modify()

        public static String EMSXAPI_Modify(String OrderRefID, String EMSX_Sequence, String inTicker, Int32 NewQuantity)
        {
            // 
            // Procedure:   EMSXAPI_Modify
            //
            // Purpose:     Modify the Orders via EMSX API
            //
            // Written:     SoftPark21/Quantitative Systems 26-Sep-2012
            //
            // Modified:
            //

            // Local Variables
            String RetMessage = "";
            String BBG_Ticker = EMSTickerCheck(inTicker);

            //return ("");

            RetMessage = EMSXAPI_Connect();
            if (RetMessage.Length > 0)
            {
                return (RetMessage);
            }

                        // See if I have the EMSX_Sequence
            if (EMSX_Sequence.Length == 0)
            {
                // Get the sequence
                EMSX_Sequence = SystemLibrary.SQLSelectString("Select EMSX_Sequence From Orders Where OrderRefID = '" + OrderRefID + "'");
                if (EMSX_Sequence.Length == 0)
                    return ("Could not find the EMSX Sequence, so you need to Modify the Order manually using EMSX");
            }


            // Create a create order request and populate it with the order details
            Request request = m_service.CreateRequest("ModifyOrder");


            request.Set("EMSX_SEQUENCE", EMSX_Sequence);
            request.Set("EMSX_TICKER", BBG_Ticker); // It needs a valid ticker, but actually ignore it.
            request.Set("EMSX_AMOUNT", Convert.ToInt32(Math.Abs(NewQuantity)));

            // Submit the request
            String myMessage = SubmitRequest(request, OrderRefID);
            if (myMessage.Length > 0)
            {
                RetMessage = RetMessage + BBG_Ticker + " " +
                             NewQuantity.ToString() + "\r\n" +
                             myMessage + "\r\n\r\n";
            }
            Console.WriteLine(myMessage);

            EMSXAPI_Disconnect();

            return (RetMessage);

        } //EMSXAPI_Modify()

        public static String EMSXAPI_Delete(String OrderRefID)
        {
            return (EMSXAPI_Delete(OrderRefID, ""));
        } //EMSXAPI_Delete()

        public static String EMSXAPI_Delete(String OrderRefID, String EMSX_Sequence)
        {
            // 
            // Procedure:   EMSXAPI_Delete
            //
            // Purpose:     Delete an existing Order via EMSX API
            //
            // Written:     SoftPark21/Quantitative Systems 26-Sep-2012
            //
            // Modified:
            //

            // Local Variables
            String RetMessage = "";

            RetMessage = EMSXAPI_Connect();
            if (RetMessage.Length > 0)
            {
                return (RetMessage);
            }


            // See if I have the EMSX_Sequence
            if (EMSX_Sequence.Length == 0)
            {
                // Get the sequence
                EMSX_Sequence = SystemLibrary.SQLSelectString("Select EMSX_Sequence From Orders Where OrderRefID = '" + OrderRefID + "'");
                if (EMSX_Sequence.Length == 0)
                    return ("Could not find the EMSX Sequence, so you need to Delete the Order using EMSX");
            }
            if (OrderRefID.Length == 0)
            {
                // Get the OrderRefID
                OrderRefID = SystemLibrary.SQLSelectString("Select OrderRefID From Orders Where EMSX_Sequence = " + EMSX_Sequence);
                if (OrderRefID.Length == 0)
                    return ("Could not find the internal Order Reference ID, so you need to Delete the Order using EMSX");
            }

            // Create a delete order request and populate it with the order number
            Request request = m_service.CreateRequest("DeleteOrder");
            request.GetElement("EMSX_SEQUENCE").AppendValue(EMSX_Sequence);

            // Submit the request
            String myMessage = SubmitRequest(request, OrderRefID);
            if (myMessage.Length > 0)
            {
                RetMessage = RetMessage + "EMSX_Sequence = " + EMSX_Sequence + "\r\n" +
                             myMessage + "\r\n\r\n";
            }
            SystemLibrary.DebugLine(myMessage);

            // Clean up
            EMSXAPI_Disconnect();

            return (RetMessage);

        } //EMSXAPI_Delete()

        public static String EMSXAPI_Send()
        {
            // 
            // Procedure:   EMSXAPI_Send
            //
            // Purpose:     Send the Orders via EMSX API
            //
            // Written:     SoftPark21/Quantitative Systems 26-Sep-2012
            //
            // Modified:
            //

            // Local Variables
            String RetMessage = "";

            RetMessage = EMSXAPI_Connect();
            if (RetMessage.Length > 0)
            {
                return (RetMessage);
            }

            // Loop down data and send a request for each row
            for (int i = 0; i < dt_SendToBloomberg.Rows.Count; i++)
            {
                String BBG_Ticker = dt_SendToBloomberg.Rows[i]["BloombergTicker"].ToString() + " " +
                                    dt_SendToBloomberg.Rows[i]["Exchange"].ToString() + " " +
                                    dt_SendToBloomberg.Rows[i]["YellowKey"].ToString();
                String Broker = SystemLibrary.ToString(dt_SendToBloomberg.Rows[i]["Broker"]);
                String Account = SystemLibrary.ToString(dt_SendToBloomberg.Rows[i]["Account"]);
                String isFuture = SystemLibrary.ToString(dt_SendToBloomberg.Rows[i]["isFuture"]);

                // Create a create order request and populate it with the order details
                if (isFuture == "Y")
                {
                    // Cause the Splits to become seperate Orders if they dont share the same EMSX_Account and get back the details
                    String OrderRefID = SystemLibrary.ToString(dt_SendToBloomberg.Rows[i]["OrderRefID"]);
                    DataTable dt_Splits = SystemLibrary.SQLSelectToDataTable("Exec sp_FuturesOrderSplit '" + OrderRefID + "' ");
                    for (int j = 0; j < dt_Splits.Rows.Count; j++)
                    {
                        String NewOrderRefID = SystemLibrary.ToString(dt_Splits.Rows[j]["OrderRefID"]);
                        Request request = m_service.CreateRequest("CreateOrder");
                        request.Set("EMSX_TICKER", BBG_Ticker);
                        request.Set("EMSX_AMOUNT", SystemLibrary.ToInt32(dt_Splits.Rows[j]["Quantity"]));
                        request.Set("EMSX_ORDER_TYPE", "MKT"); // The EMSX code forces a value - LMT, MKT, etc
                        if (Broker.Length > 0)
                            request.Set("EMSX_BROKER", Broker);
                        //request.Set("EMSX_TIF", cbTif.Text); // DAY, GTC, etc
                        //request.Set("EMSX_HAND_INSTRUCTION", "This is a Test");
                        Account = SystemLibrary.ToString(dt_Splits.Rows[j]["Account"]);
                        if (Account.Length > 0)
                            request.Set("EMSX_ACCOUNT", Account);
                        request.Set("EMSX_SIDE", GetEMSXSide(dt_SendToBloomberg.Rows[i]["Side"].ToString()));
                        //request.Set("EMSX_LIMIT_PRICE", double.Parse(txtLimit.Text));

                        // Submit the request
                        String myMessage = SubmitRequest(request, NewOrderRefID);
                        if (myMessage.Length > 0)
                        {
                            RetMessage = RetMessage + BBG_Ticker + " " +
                                         dt_SendToBloomberg.Rows[i]["Side"].ToString() + " " +
                                         SystemLibrary.ToString(dt_Splits.Rows[j]["Quantity"]) + "\r\n" +
                                         myMessage + "\r\n\r\n";
                        }
                        Console.WriteLine(myMessage);
                    }
                }
                else
                {
                    Request request = m_service.CreateRequest("CreateOrder");
                    request.Set("EMSX_TICKER", BBG_Ticker);
                    request.Set("EMSX_AMOUNT", SystemLibrary.ToInt32(dt_SendToBloomberg.Rows[i]["OrderQuantity"]));
                    request.Set("EMSX_ORDER_TYPE", "MKT"); // The EMSX code forces a value - LMT, MKT, etc
                    if (Broker.Length > 0)
                        request.Set("EMSX_BROKER", Broker);
                    //request.Set("EMSX_TIF", cbTif.Text); // DAY, GTC, etc
                    //request.Set("EMSX_HAND_INSTRUCTION", "This is a Test");
                    if (Account.Length > 0)
                        request.Set("EMSX_ACCOUNT", Account);
                    request.Set("EMSX_SIDE", GetEMSXSide(dt_SendToBloomberg.Rows[i]["Side"].ToString()));
                    //request.Set("EMSX_LIMIT_PRICE", double.Parse(txtLimit.Text));

                    // Submit the request
                    String myMessage = SubmitRequest(request, dt_SendToBloomberg.Rows[i]["OrderRefID"].ToString());
                    if (myMessage.Length > 0)
                    {
                        RetMessage = RetMessage + BBG_Ticker + " " +
                                     dt_SendToBloomberg.Rows[i]["Side"].ToString() + " " +
                                     SystemLibrary.ToString(dt_SendToBloomberg.Rows[i]["OrderQuantity"]) + "\r\n" +
                                     myMessage + "\r\n\r\n";
                    }
                    Console.WriteLine(myMessage);
                }
            }

            EMSXAPI_Disconnect();

            return (RetMessage);

        } //EMSXAPI_Send()

        private static String SubmitRequest(Request request, String OrderRefID)
        {
            // Local Variables
            String RetVal = "";


            // Create an event queue to hold the response
            EventQueue myEventQueue = new EventQueue();
            // Create a correlation id identifying the request
            CorrelationID requestId = new CorrelationID(OrderRefID);

            m_session.SendRequest(request, myEventQueue, requestId);

            // Set a reasonable time out after which we will stop waiting for a response
            int timeoutInMilliSeconds = 5000;
            Event evt = myEventQueue.NextEvent(timeoutInMilliSeconds);
            StringBuilder stringBuilder = new StringBuilder();

            do
            {
                // We should only get a response event in return
                if (evt.Type == Event.EventType.RESPONSE)
                {
                    foreach (Bloomberglp.Blpapi.Message message in evt.GetMessages())
                    {
                        if (message.CorrelationID == requestId)
                        {
                            String myMessageType = message.MessageType.ToString();
                            switch (myMessageType.ToUpper())
                            {
                                case "ERRORINFO":
                                    if (message.HasElement("ERROR_CODE"))
                                    {
                                        int ERROR_CODE = message.GetElementAsInt32("ERROR_CODE");
                                        switch (ERROR_CODE)
                                        {
                                            case 22912:
                                                String myTicker = request.GetElement("EMSX_TICKER").GetValueAsString();
                                                // Invalid Ticker or exchange
                                                if (SystemLibrary.iMessageBox(myTicker + " is is an invalid Ticker.\r\n\r\n" +
                                                                              "If this is a non-Bloomberg ticker, such as an unlisted instrument,\r\nthen press [Cancel] and follow up with the creation of a Security and Price record.\r\n\r\nOtherwise Delete the Order.\r\n\r\n" +
                                                                              "Press [Ok] if you want to Delete this Order.\r\n", 
                                                                              "Invalid Ticker - '" + myTicker + "'", MessageBoxIcon.Question) == DialogResult.OK)
                                                {
                                                    // Delete the Order record
                                                    SystemLibrary.SQLExecute("Exec sp_ReverseOrder '" + OrderRefID + "'");
                                                }
                                                RetVal = "";
                                                break;
                                            default:
                                                // Might as well give the full record
                                                RetVal = message.ToString();
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        // Might as well give the full record
                                        RetVal = message.ToString();
                                    }
                                    break;
                                case "CREATEORDER":
                                    if (message.HasElement("EMSX_SEQUENCE"))
                                    {
                                        String EMSX_SEQUENCE = message.GetElementAsString("EMSX_SEQUENCE");
                                        String mySQL = "Update Orders Set EMSX_Sequence = " + EMSX_SEQUENCE + " Where OrderRefID = '" + OrderRefID + "'";
                                        SystemLibrary.SQLExecute(mySQL);
                                    }
                                    if (message.HasElement("MESSAGE"))
                                    {
                                        String myMESSAGE = message.GetElementAsString("MESSAGE");
                                        if (myMESSAGE.ToUpper() != "Order created".ToUpper())
                                            if (RetVal.Length>0)
                                                RetVal = RetVal + "\r\n" + myMESSAGE;
                                            else
                                                RetVal = myMESSAGE;
                                    }
                                    break;
                                case "DELETEORDER":
                                    if (message.HasElement("MESSAGE"))
                                    {
                                        String myMESSAGE = message.GetElementAsString("MESSAGE");
                                        if (myMESSAGE.ToUpper() != "Order deleted".ToUpper())
                                            if (RetVal.Length > 0)
                                                RetVal = RetVal + "\r\n" + myMESSAGE;
                                            else
                                                RetVal = myMESSAGE;
                                    }
                                    else
                                    {
                                        // Give the full message
                                        RetVal = message.ToString();
                                    }
                                    break;
                                case "MODIFYORDER":
                                    if (message.HasElement("MESSAGE"))
                                    {
                                        String myMESSAGE = message.GetElementAsString("MESSAGE");
                                        if (myMESSAGE.ToUpper() != "Order Modified".ToUpper())
                                            if (RetVal.Length > 0)
                                                RetVal = RetVal + "\r\n" + myMESSAGE;
                                            else
                                                RetVal = myMESSAGE;
                                    }
                                    else
                                    {
                                        // Give the full message
                                        RetVal = message.ToString();
                                    }
                                    break;
                                default:
                                    if (RetVal.Length > 0)
                                        RetVal = RetVal + "\r\n" + message.ToString();
                                    else
                                        RetVal = message.ToString();
                                    break;
                            }
                        }
                    }
                    return (RetVal);
                }
                evt = myEventQueue.NextEvent(timeoutInMilliSeconds);
            } while (evt.Type != Event.EventType.TIMEOUT);

            // Only way it gets here is if the data was not sent
            return("Not sure if request worked as timed out.");

        } // SubmitRequest()

        public static String EMSXAPI_Connect()
        {
            // The session options determine how the connection to bbcomm is established
            m_sessionOptions = new SessionOptions();
            m_sessionOptions.ServerHost = "localhost";
            m_sessionOptions.ServerPort = 8194;
            m_sessionOptions.MaxEventQueueSize = 1000000; // Past experience say make this large
            m_session = new Session(m_sessionOptions);

            // Now start the session
            if (!m_session.Start())
            {
                // Failure to start indicates problem connecting to bbcomm
                return("Failed to Start a Session");
            }

            // Now connect to the EMSX API service
            if (!m_session.OpenService("//blp/emapisvc"))
            {
                // Failure to open service indicates EMSX API service is down
                m_session.Stop();
                return (@"Failed to open '//blp/emapisvc' service");
            }

            // Now get the a Blp Api service object representing EMSX API Service
            m_service = m_session.GetService("//blp/emapisvc");

            return ("");

        } //EMSXAPI_Connect()

        public static void EMSXAPI_Disconnect()
        {
            // Dispose our session
            m_session.Stop();
            m_session = null;

        } //EMSXAPI_Disconnect()


        #endregion Bloomberg EMSX API

    }
}

