using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BLP_DATA_CTRLLib;
using blpControl = BLP_DATA_CTRLLib.BlpData;

namespace T1MultiAsset
{
    public partial class EOD_Prices : Form
    {
        // Public Variables
        public Form1 ParentForm1;

        
        public EOD_Prices()
        {
            InitializeComponent();
        }

        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;
        } //FromParent()

        private void EOD_Prices_Shown(object sender, EventArgs e)
        {
            // Local Variables
            String ErrorMessages = "";
            int oldTimeOut;

            this.Text = this.Text + " - Started: " + DateTime.Now.ToShortTimeString();

            SystemLibrary.PositionFormOverParent(this, ParentForm1);

            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;

            // Increase the database timout to infinite as some of these commands can take a long time
            oldTimeOut = SystemLibrary.SQLAlterTimeOut(0);

            SystemLibrary.DebugLine("OvernightPrices - Start");
            lb_Status.Text = "OvernightPrices - Start"; Application.DoEvents(); 
            List<string> securities = new List<string>();
            DataTable dt_prices = new DataTable();

            SystemLibrary.DebugLine("pre-sp_updateModelFundPosition");
            lb_Status.Text = "OvernightPrices - sp_updateModelFundPosition"; Application.DoEvents(); 
            // Support the Simple Fund System
            SystemLibrary.SQLExecute("Exec ClientReporting.dbo.sp_updateModelFundPosition");

            SystemLibrary.DebugLine("OvernightPrices - Pre Delete");
            lb_Status.Text = "Clean out last 3 days of prices"; Application.DoEvents(); 
            SystemLibrary.SQLExecute("Delete from EOD_Prices where EffectiveDate >= dbo.f_Today() - 3 and isNull(Locked,'N') = 'N' ");

            SystemLibrary.DebugLine("OvernightPrices - Pre sp_EOD_Prices_Required");
            lb_Status.Text = "Obtain list of End of Day Prices Required"; Application.DoEvents(); 
            DataTable dt_Tickers = SystemLibrary.SQLSelectToDataTable("Exec sp_EOD_Prices_Required");

            for (int i = 0; i < dt_Tickers.Rows.Count; i++)
            {
                securities.Add(SystemLibrary.ToString(dt_Tickers.Rows[i]["BBG_Ticker"]));
            }

            DateTime StartDate = Convert.ToDateTime(dt_Tickers.Compute("min(StartDate)", string.Empty));
            DateTime EndDate = SystemLibrary.f_Today();
            String FirstTicker = SystemLibrary.ToString(dt_Tickers.Select("StartDate='" + StartDate.ToString("dd-MMM-yyyy") + "'")[0]["BBG_Ticker"]);

            SystemLibrary.DebugLine("OvernightPrices - Pre OvernightPrices() - Ticker #=" + dt_Tickers.Rows.Count.ToString());
            lb_Status.Text = "Request Bloomberg Prices (MinDate=" + StartDate.ToString("dd-MMM-yyyy") + " for at least " + FirstTicker + ")"; Application.DoEvents(); 
            OvernightPrices.OvernightPrices op = new OvernightPrices.OvernightPrices();

            try
            {
                dt_prices = op.getPricesDataTable(securities, StartDate, EndDate, ref ErrorMessages);
            }
            catch (Exception ex)
            {
                SystemLibrary.DebugLine("Error Occured: " + ex.Message);
                lb_Status.Text = "Failed to Run: Error Occured: " + ex.Message; Application.DoEvents();
                // Reset the database timout
                SystemLibrary.SQLAlterTimeOut(oldTimeOut);
                return;
            }

            lb_Status.Text = "Save Prices"; Application.DoEvents(); Application.DoEvents();
            int myRows = dt_Tickers.Rows.Count;
            for (int i = 0; i < myRows; i++)
            {
                String BBG_Ticker = SystemLibrary.ToString(dt_Tickers.Rows[i]["BBG_Ticker"]);
                String ID_BB_UNIQUE = SystemLibrary.ToString(dt_Tickers.Rows[i]["ID_BB_UNIQUE"]);
                String ID_BB_GLOBAL = SystemLibrary.ToString(dt_Tickers.Rows[i]["ID_BB_GLOBAL"]);
                String Pos_Mult_Factor = SystemLibrary.ToString(dt_Tickers.Rows[i]["Pos_Mult_Factor"]);
                String TickerStartDate = Convert.ToDateTime(dt_Tickers.Rows[i]["StartDate"]).ToString("dd-MMM-yyyy");
                SystemLibrary.DebugLine("OvernightPrices - '" + BBG_Ticker + "'");
                String mySQlHeader = "";
                String mySqlDetail = "";

                DataRow[] dr = dt_prices.Select("BBG_Ticker='" + BBG_Ticker + "' And EffectiveDate >= '" + StartDate + "'", "EffectiveDate");

                lb_Status.Text = "Save Prices for '" + BBG_Ticker + "' [" + i.ToString() + " / " + dt_Tickers.Rows.Count.ToString() + "]"; Application.DoEvents(); 
                int drRows = dr.Length;
                if (drRows > 0)
                {
                    /*
                    String mySql = "Delete from EOD_Prices " +
                                   "Where BBG_Ticker = '" + BBG_Ticker + "' " +
                                   "And ID_BB_UNIQUE = '" + ID_BB_UNIQUE + "' " +
                                   "And ID_BB_GLOBAL = '" + ID_BB_GLOBAL + "' " +
                                   "And EffectiveDate between '" + TickerStartDate + "' and '" + EndDate.ToString("dd-MMM-yyyy") + "' ";
                    SystemLibrary.DebugLine(mySql);
                    SystemLibrary.SQLExecute(mySql);
                    */

                    mySQlHeader = "Exec sp_Insert_EOD_Price '" + BBG_Ticker + "','" + ID_BB_UNIQUE + "','" + ID_BB_GLOBAL + "'," + Pos_Mult_Factor + ", '" + StartDate.ToString("dd-MMM-yyyy") + "', '" + EndDate.ToString("dd-MMM-yyyy") + "', '";
                    mySqlDetail = mySQlHeader;
                    for (int j = 0; j < drRows; j++)
                    {
                        String EffectiveDate = Convert.ToDateTime(dr[j]["EffectiveDate"]).ToString("dd-MMM-yyyy");
                        Decimal px_last = SystemLibrary.ToDecimal(dr[j]["px_last"]);
                        Decimal eqy_weighted_avg_px = SystemLibrary.ToDecimal(dr[j]["eqy_weighted_avg_px"]);
                        // Deal with lack of VWAP by setting it to Close
                        if (eqy_weighted_avg_px == 0)
                            eqy_weighted_avg_px = px_last;

                        /*
                        mySql = "Insert into EOD_Prices(EffectiveDate,BBG_Ticker,ID_BB_UNIQUE,ID_BB_GLOBAL,Pos_Mult_Factor,Close_Price,vwap) " +
                                "Values ('" + EffectiveDate + "','" + BBG_Ticker + "','" + ID_BB_UNIQUE + "','" + ID_BB_GLOBAL + "'," + Pos_Mult_Factor + "," + px_last.ToString() + "," + eqy_weighted_avg_px.ToString() + ")";
                        SystemLibrary.DebugLine(mySql);
                        SystemLibrary.SQLExecute(mySql);
                         */
                        String mySqlLineData = EffectiveDate + "," + px_last.ToString() + "," + eqy_weighted_avg_px.ToString() + "|";
                        if ((mySqlDetail + mySqlLineData).Length > 5000)
                        {
                            // Send compile string to database
                            mySqlDetail = mySqlDetail + "'";
                            SystemLibrary.SQLExecute(mySqlDetail);
                            mySqlDetail = mySQlHeader;
                        }
                        else
                            mySqlDetail = mySqlDetail + mySqlLineData;
                    }

                    // Send compile string to database
                    if (mySqlDetail != mySQlHeader)
                    {
                        mySqlDetail = mySqlDetail + "'";
                        SystemLibrary.SQLExecute(mySqlDetail);
                    }
                }
            }

            lb_Status.Text = "Running sp_updateModelFundPosition"; Application.DoEvents(); 
            SystemLibrary.SQLExecute("Exec ClientReporting.dbo.sp_updateModelFundPosition");

            SystemLibrary.DebugLine("pre-sp_BalanceDates_Maintain");
            lb_Status.Text = "Running sp_BalanceDates_Maintain"; Application.DoEvents(); 
            SystemLibrary.SQLExecute("Exec sp_BalanceDates_Maintain");

            lb_Status.Text = "Rebuild Profit From '" + StartDate.ToString("dd-MMM-yyyy") + "'" + " - Started: " + DateTime.Now.ToShortTimeString(); Application.DoEvents(); 
            SystemLibrary.DebugLine("pre-sp_Calc_Profit_RebuildFrom '" + StartDate.ToString("dd-MMM-yyyy") + "'");

            // Cause a Profit Calculation
            if (StartDate > EndDate)
                SystemLibrary.SQLExecute("Exec sp_Calc_Profit_RebuildFrom null");
            else
                SystemLibrary.SQLExecute("Exec sp_Calc_Profit_RebuildFrom '" + StartDate.ToString("dd-MMM-yyyy") + "' ");

            // Create Interest Accruals
            SystemLibrary.SQLExecute("Exec sp_ApplyInterestAccrualAll");

            SystemLibrary.DebugLine("pre-sp_SOD_Positions ");
            lb_Status.Text = "Run Start-Of-Day"; Application.DoEvents(); 
            SystemLibrary.SQLExecute("Exec sp_SOD_Positions ");

            // Force the parent form to reset dt_securities
            if (ParentForm1 != null)
                ParentForm1.SetUpSecurities_DataTable();

            SystemLibrary.DebugLine("OvernightPrices - End");
            Cursor.Current = Cursors.Default;
            lb_Status.Text = "Completed. Please close this window"; Application.DoEvents();
            if (ErrorMessages.Length > 0)
                lb_Status.Text = lb_Status.Text + "\r\n" + ErrorMessages;
            //MessageBox.Show("EOD Prioes - Completed");

            // Reset the database timout
            SystemLibrary.SQLAlterTimeOut(oldTimeOut);

            this.Text = this.Text + " / Completed: " + DateTime.Now.ToShortTimeString();

        } //EOD_Prices_Shown()

        private void EOD_Prices_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Let the Parent know this has close
            if (ParentForm1 != null)
                ParentForm1.LoadActionTab(true);
        }

        private void EOD_Prices_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); 
        }
    }
}
