using System;
using System.Collections.Generic;
using System.Data;
using T1MultiAsset;

using SessionOptions = Bloomberglp.Blpapi.SessionOptions;
using Session = Bloomberglp.Blpapi.Session;
using Service = Bloomberglp.Blpapi.Service;
using Request = Bloomberglp.Blpapi.Request;
using Element = Bloomberglp.Blpapi.Element;
using Message = Bloomberglp.Blpapi.Message;
using Event = Bloomberglp.Blpapi.Event;
using Name = Bloomberglp.Blpapi.Name;

namespace OvernightPrices
{
    class Price
    {
        public string security;
        public DateTime date;
        public double px_last;
        public double eqy_weighted_avg_px;
    }

    class OvernightPrices
    {

        private List<Price> prices = new List<Price>();

        private Session session;
        private Service refDataService;

        private static readonly Name SECURITY_DATA = new Name("securityData");
        private static readonly Name SECURITY_ERROR = new Name("securityError");
        private static readonly Name MESSAGE = new Name("message");
        private static readonly Name FIELD_DATA = new Name("fieldData");
        private static readonly Name RESPONSE_ERROR = new Name("responseError");

        private void init()
        {
            SessionOptions sessionOptions = new SessionOptions();
            sessionOptions.ServerHost = "localhost";
            sessionOptions.ServerPort = 8194;

            session = new Session(sessionOptions);

            if(!session.Start())
            {
                throw new Exception("OvernightPrices: Failed to start session");
            }

            if(!session.OpenService("//blp/refdata"))
            {
                throw new Exception("OvernightPrices: Failed to open service");
            }

            refDataService = session.GetService("//blp/refdata");
        }

        public List<Price> getPrices(List<string> securities, DateTime startDate, DateTime endDate)
        {
            // Return prices for list of securities with common date range

            init();

            Request request = refDataService.CreateRequest("HistoricalDataRequest");

            Element secs = request.GetElement("securities");

            foreach(string s in securities)
            {
                secs.AppendValue(s);
            }

            Element fields = request.GetElement("fields");

            fields.AppendValue("PX_LAST");
            fields.AppendValue("EQY_WEIGHTED_AVG_PX");

            request.Set("startDate", startDate.ToString("yyyyMMdd"));
            request.Set("endDate", endDate.ToString("yyyyMMdd"));

            request.Set("nonTradingDayFillMethod", "PREVIOUS_VALUE");
            request.Set("nonTradingDayFillOption", "ALL_CALENDAR_DAYS");

            session.SendRequest(request, null);

            while(true)
            {
                Event eventObj = session.NextEvent(10000);
                if(eventObj.Type == Event.EventType.RESPONSE || eventObj.Type == Event.EventType.PARTIAL_RESPONSE)
                {
                    foreach (Message msg in eventObj)
                    {
                        if(msg.HasElement(RESPONSE_ERROR))
                        {
                            throw new Exception("OvernightPrices: Response error received - " + msg.ToString());
                        }

                        Element securityData = msg.GetElement(SECURITY_DATA);

                        if(securityData.HasElement(SECURITY_ERROR))
                        {
                            Element securityError = securityData.GetElement(SECURITY_ERROR);
                            Element errorMessage = securityError.GetElement(MESSAGE);
                            throw new Exception("OvernightPrices: Security error detected - " + errorMessage);  
                        }

                        Element fieldData = securityData.GetElement(FIELD_DATA);

                        if(fieldData.NumValues > 0)
                        {
                            for(int j = 0; j < fieldData.NumValues; j++)
                            {
                                Element element = fieldData.GetValueAsElement(j);
                                Price p = new Price();
                                p.security = securityData.GetElementAsString("security");
                                p.date = (element.GetElementAsDatetime("date").ToSystemDateTime());
                                p.px_last = element.GetElementAsFloat64("PX_LAST");
                                p.eqy_weighted_avg_px = element.GetElementAsFloat64("EQY_WEIGHTED_AVG_PX");
                                prices.Add(p);
                            }
                        }
                    }

                    if(eventObj.Type == Event.EventType.RESPONSE) break;
                }
            }
            return prices;
        } //getPrices()


        public DataTable getPricesDataTable(List<string> securities, DateTime startDate, DateTime endDate, ref String ErrorMessages)
        {
            //Console.WriteLine("getPricesDataTable(securities count =" + securities.Count.ToString() + "," + startDate.ToString("yyyyMMdd") + "," + endDate.ToString("yyyyMMdd") + ",ref  ErrorMessages)");

            // Return prices for list of securities with common date range
            DataTable dt_Prices = new DataTable();
            dt_Prices.Columns.Add("BBG_Ticker");
            dt_Prices.Columns.Add("EffectiveDate", System.Type.GetType("System.DateTime"));
            dt_Prices.Columns.Add("px_last", System.Type.GetType("System.Decimal"));
            dt_Prices.Columns.Add("eqy_weighted_avg_px", System.Type.GetType("System.Decimal"));

            init();
            ErrorMessages = "";

            Request request = refDataService.CreateRequest("HistoricalDataRequest");

            Element secs = request.GetElement("securities");

            foreach (string s in securities)
            {
                secs.AppendValue(s);
                //Console.WriteLine("secs.AppendValue(" + s +")");
            }

            Element fields = request.GetElement("fields");

            fields.AppendValue("PX_LAST");
            fields.AppendValue("EQY_WEIGHTED_AVG_PX");

            request.Set("startDate", startDate.ToString("yyyyMMdd"));
            request.Set("endDate", endDate.ToString("yyyyMMdd"));

            request.Set("periodicityAdjustment", "ACTUAL");
            request.Set("periodicitySelection", "DAILY");

            request.Set("nonTradingDayFillMethod", "PREVIOUS_VALUE");
            request.Set("nonTradingDayFillOption", "ALL_CALENDAR_DAYS");

            /*
                FX = "Default"
        
    With blpControl
        .AutoRelease = False
        .QueueEvents = True
        .SubscriptionMode = BySecurity
        .ReverseChronological = False
        .Periodicity = bbDaily
        .NonTradingDayValue = PreviousDays
        .DisplayNonTradingDays = AllCalendar
        
        .GetHistoricalData2 sec, 10, Flds, CDate(StartDate), FX, CDate(EndDate), Results:=vtResults
            */


            session.SendRequest(request, null);

            while (true)
            {
                Event eventObj = session.NextEvent(10000);
                if (eventObj.Type == Event.EventType.RESPONSE || eventObj.Type == Event.EventType.PARTIAL_RESPONSE)
                {
                    foreach (Message msg in eventObj)
                    {
                        if (msg.HasElement(RESPONSE_ERROR))
                        {
                            Console.WriteLine("OvernightPrices: Response error received - " + msg.ToString());
                            ErrorMessages = ErrorMessages + "OvernightPrices: Response error received - " + msg.ToString() + "\r\n";
                            continue;
                            //throw new Exception("OvernightPrices: Response error received - " + msg.ToString());
                        }

                        Element securityData = msg.GetElement(SECURITY_DATA);

                        if (securityData.HasElement(SECURITY_ERROR))
                        {
                            Element securityError = securityData.GetElement(SECURITY_ERROR);
                            Element errorMessage = securityError.GetElement(MESSAGE);
                            Console.WriteLine("OvernightPrices: Security error detected - " + errorMessage);
                            ErrorMessages = ErrorMessages + "OvernightPrices: Security error received - " + msg.ToString() + "\r\n";
                            continue;
                            //throw new Exception("OvernightPrices: Security error detected - " + errorMessage);
                        }

                        Element fieldData = securityData.GetElement(FIELD_DATA);

                        if (fieldData.NumValues > 0)
                        {
                            for (int j = 0; j < fieldData.NumValues; j++)
                            {
                                Element element = fieldData.GetValueAsElement(j);
                                DataRow dr = dt_Prices.NewRow();
                                dr["BBG_Ticker"] = securityData.GetElementAsString("security");
                                dr["EffectiveDate"] = element.GetElementAsDatetime("date").ToSystemDateTime();
                                dr["px_last"] = element.GetElementAsFloat64("PX_LAST");
                                if (element.HasElement("EQY_WEIGHTED_AVG_PX"))
                                    dr["eqy_weighted_avg_px"] = element.GetElementAsFloat64("EQY_WEIGHTED_AVG_PX");
                                dt_Prices.Rows.Add(dr);
                                //Console.WriteLine(SystemLibrary.ToString(dr["BBG_Ticker"]) + "," + Convert.ToDateTime(dr["EffectiveDate"]).ToString("ddMMyy") + "," +
                                //                  SystemLibrary.ToString(dr["px_last"]) + "," + SystemLibrary.ToString(dr["eqy_weighted_avg_px"]));

                            }
                        }
                    }

                    if (eventObj.Type == Event.EventType.RESPONSE) break;
                }
            }
            //Console.WriteLine("getPricesDataTable - END");
            return dt_Prices;
        } // DataTable getPricesDataTable()

    }
}
