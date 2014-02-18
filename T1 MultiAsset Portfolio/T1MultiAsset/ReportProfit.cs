using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace T1MultiAsset
{
    public partial class ReportProfit : Form
    {
        // Public Variables
        public Form1 ParentForm1;
        DataTable dt_ReportProfit;
        DataTable dt_ChartProfit;
        DataTable dt_Fund;
        DataTable dt_Portfolio;

        int     FundID = -1;
        String  Fund_Name = "";
        int     PortfolioID = -1;
        String  Portfolio_Name = "";
        String  BBG_Ticker = "";
        DateTime FromDate = DateTime.MinValue;
        DateTime ToDate = DateTime.MaxValue;


        // Allow for a Back button 
        // TODO (5) Make this a datatable so can have an extensive history
        Boolean Prev_cb_hasFromDate;
        Boolean Prev_cb_hasToDate;
        DateTime prev_dtp_FromDate;
        DateTime prev_dtp_ToDate;
        Boolean prev_cb_OnlyTotal;
        Boolean prev_cb_DailyProfitnLoss;


        public ReportProfit()
        {
            // Local Variables
            String mySql;

            InitializeComponent();

            // RULE: Need to have this data before FromParent() is called.
            //dtp_FromDate.Value = DateTime.Today.AddYears(-1);
            dtp_FromDate.Value = SystemLibrary.f_Today().AddMonths(-2);
            dtp_ToDate.Value = SystemLibrary.f_Today();
            // Load the Drop down boxes
            mySql = "Select FundID, FundName, ShortName, CreatedDate " +
                    "From   Fund " +
                    "Where  Active = 'Y' " +
                    "And    AllowTrade = 'Y' " +
                    "Order by FundName";
            dt_Fund = SystemLibrary.SQLSelectToDataTable(mySql);
            cb_Fund.DataSource = dt_Fund;
            cb_Fund.DisplayMember = "FundName";
            cb_Fund.ValueMember = "FundId";
            cb_Fund.SelectedIndex = 0;

            mySql = "Select PortfolioID, PortfolioName, CreatedDate, ClosedDate, Active " +
                    "From   Portfolio " +
                    "Where  Active = 'Y' " + 
                    "Order by PortfolioName";
            dt_Portfolio = SystemLibrary.SQLSelectToDataTable(mySql);
            cb_Portfolio.DataSource = dt_Portfolio;
            cb_Portfolio.DisplayMember = "PortfolioName";
            cb_Portfolio.ValueMember = "PortfolioId";
            cb_Portfolio.SelectedIndex = 0;


        } //ReportProfit()

        public void FromParent(Form inForm, int inFundID, int inPortfolioID, String inBBG_Ticker, object inFromDate, object inToDate, int inBrokerID)
        {
            ParentForm1 = (Form1)inForm;

            // Set the Global variables
            FundID = inFundID;
            PortfolioID = inPortfolioID;
            BBG_Ticker = inBBG_Ticker;

            if (inFromDate != null)
                FromDate = (DateTime)inFromDate;
            if (inToDate != null)
                ToDate = (DateTime)inToDate;

            // Set up the boxes
            if (FundID == -1)
                cb_hasFundID.Checked = false;
            else
            {
                cb_Fund.SelectedValue = FundID;
                cb_hasFundID.Checked = true;
            }

            if (PortfolioID == -1)
                cb_hasPortfolioID.Checked = false;
            else
            {
                cb_Portfolio.SelectedValue = PortfolioID;
                cb_hasPortfolioID.Checked = true;
            }

            if (BBG_Ticker == "")
                cb_hasBBG_Ticker.Checked = false;
            else
            {
                tb_BBG_Ticker.Text = BBG_Ticker;
                cb_hasBBG_Ticker.Checked = true;
            }
            cb_hasBBG_Ticker_Exclude.Checked = false;
            

            //LoadTrades();

            //ParentForm1.CopyDataTable(ref dt_Fund, ref dt_Portfolio);
        } //FromParent()


        private void ReportProfit_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            LoadTrades();
            FormatTrades();
        } //ReportProfit_Load

        private void cb_Fund_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FundID = Convert.ToInt16(((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[0].ToString());
            Fund_Name = ((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[1].ToString();
            //Fund_Amount = Convert.ToDecimal(((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[2].ToString());
            //Fund_Crncy = ((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[3].ToString();
            //LoadTrades();

        } // cb_Fund_SelectionChangeCommitted()

        private void cb_Portfolio_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PortfolioID = Convert.ToInt16(((DataRowView)(cb_Portfolio.SelectedItem)).Row.ItemArray[0].ToString());
            Portfolio_Name = ((DataRowView)(cb_Portfolio.SelectedItem)).Row.ItemArray[1].ToString();
            //Portfolio_Amount = Convert.ToDecimal(((DataRowView)(cb_Portfolio.SelectedItem)).Row.ItemArray[2].ToString());
            //Portfolio_Crncy = ((DataRowView)(cb_Portfolio.SelectedItem)).Row.ItemArray[3].ToString();
            //LoadTrades();

        } // cb_Portfolio_SelectionChangeCommitted()

        public void LoadTrades()
        {
            /*
             * Trade report
             */

            // Local Variables
            String mySql;
            String mySqlChart;


            mySqlChart = "Exec sp_ChartData_Profit ";

            mySql = "Select	FundName, PortfolioName, EffectiveDate, BBG_Ticker, LS, Profit, Profit_Day [Excl Trades], Profit_Trade [Trading Profit], Country_Full_Name, Sector, Industry_Group, Industry_SubGroup " +
                    "from	Profit, " +
                    "		Fund, " +
                    "		Portfolio " +
                    "Where	Fund.FundID = Profit.FundID " +
                    "And	Portfolio.PortfolioID = Profit.PortfolioID ";
            if (cb_hasFundID.Checked)
            {
                mySql = mySql + "And		Profit.FundID = " + FundID.ToString() + " ";
                mySqlChart = mySqlChart + FundID.ToString() + ", ";
            }
            else
                 mySqlChart = mySqlChart + "null, ";

            if (cb_hasPortfolioID.Checked)
            {
                mySql = mySql + "And		Profit.PortfolioID = " + PortfolioID.ToString() + " ";
                mySqlChart = mySqlChart + PortfolioID.ToString() + ", ";
            }
            else
                 mySqlChart = mySqlChart + "null, ";

            if (cb_hasBBG_Ticker.Checked && tb_BBG_Ticker.Text.Trim().Length > 0)
            {
                if (cb_hasBBG_Ticker_Exclude.Checked)
                {
                    mySql = mySql + "And		Profit.BBG_Ticker <> '" + tb_BBG_Ticker.Text + "' ";
                    mySqlChart = mySqlChart + "'" + tb_BBG_Ticker.Text + "', 'Exclude', ";
                }
                else
                {
                    mySql = mySql + "And		Profit.BBG_Ticker = '" + tb_BBG_Ticker.Text + "' ";
                    mySqlChart = mySqlChart + "'" + tb_BBG_Ticker.Text + "', 'Include', ";
                }
            }
            else
                 mySqlChart = mySqlChart + "null, null, ";

            if (cb_hasFromDate.Checked)
            {
                mySql = mySql + "And		Profit.EffectiveDate >= '" + dtp_FromDate.Value.ToString("dd-MMM-yyyy") + "' ";
                mySqlChart = mySqlChart + "'" + dtp_FromDate.Value.ToString("dd-MMM-yyyy") + "', ";
            }
            else
                 mySqlChart = mySqlChart + "null, ";

            if (cb_hasToDate.Checked)
            {
                mySql = mySql +
                        "And		Profit.EffectiveDate <= '" + dtp_ToDate.Value.ToString("dd-MMM-yyyy") + "' ";
                mySqlChart = mySqlChart + "'" + dtp_ToDate.Value.ToString("dd-MMM-yyyy") + "' ";
            }
            else
                 mySqlChart = mySqlChart + "null ";

            mySql = mySql +
                    "Order by FundName, PortfolioName, EffectiveDate, BBG_Ticker, LS ";

            mySqlChart = mySqlChart + ", '" + SystemLibrary.Bool_To_YN(cb_OnlyTotal.Checked) + "' ";

            dt_ReportProfit = SystemLibrary.SQLSelectToDataTable(mySql);
            dt_ChartProfit = SystemLibrary.SQLSelectToDataTable(mySqlChart);

            dg_ReportProfit.DataSource = dt_ReportProfit;

            // Scroll to the last row
            if (dg_ReportProfit.Rows.Count>0)
                dg_ReportProfit.CurrentCell = dg_ReportProfit[0, dg_ReportProfit.Rows.Count - 1];

            FormatTrades();
            LoadChart();

        } //LoadTrades()

        private void FormatTrades()
        {
            if (dg_ReportProfit.Rows.Count < 1)
                return;
            ParentForm1.SetFormatColumn(dg_ReportProfit, "FundName", Color.RoyalBlue, Color.Gainsboro, "", "");
            ParentForm1.SetFormatColumn(dg_ReportProfit, "BBG_Ticker", Color.RoyalBlue, Color.Gainsboro, "", "");
            ParentForm1.SetFormatColumn(dg_ReportProfit, "Profit", Color.Empty, Color.LightSkyBlue, "N0", "0");
            ParentForm1.SetFormatColumn(dg_ReportProfit, "Excl Trades", Color.Empty, Color.Empty, "N0", "0");
            ParentForm1.SetFormatColumn(dg_ReportProfit, "Trading Profit", Color.Empty, Color.Empty, "N0", "0");
            ParentForm1.SetFormatColumn(dg_ReportProfit, "EffectiveDate", Color.DarkBlue, Color.Empty, "dd-MMM-yy", "");


            // Loop over the Tickers 
            for (Int32 i = 0; i < dg_ReportProfit.Rows.Count; i++) // Last row in dg_ReportProfit is a blank row
            {
                if (dg_ReportProfit["BBG_Ticker", i].Value != null)
                {
                    if (dg_ReportProfit["LS", i].Value.ToString().StartsWith("L"))
                        dg_ReportProfit["LS", i].Style.ForeColor = Color.Green;
                    else
                        dg_ReportProfit["LS", i].Style.ForeColor = Color.Red;
                    ParentForm1.SetColumn(dg_ReportProfit, "Profit", i);
                    ParentForm1.SetColumn(dg_ReportProfit, "Excl Trades", i);
                    ParentForm1.SetColumn(dg_ReportProfit, "Trading Profit", i);
                }
            }
        } //FormatTrades()

        private void LoadChart()
        {
            String myColumn = "Profit";
            String myColumnAccum = "Accum_Profit";

            if (cb_PCT.Checked)
            {
                myColumn = "PCT";
                myColumnAccum = "PCT";
            }

            chart1.ChartAreas[0].AxisY.IsStartedFromZero = false;

            // Dispose of old series
            for (int i = chart1.Series.Count; i > 0; i--)
            {
                //chart1.Series[0].Points.AddXY("fred", 6);
                chart1.Series.RemoveAt(i - 1);
            }

            if (cb_DailyProfitnLoss.Checked)
            {
                // Not Accum
                DataView dvData = new DataView(dt_ChartProfit);
                chart1.DataBindCrossTable(dvData, "BBG_Ticker", "EffectiveDate", myColumn, ""); //, "Label=Profit{C}");

                // Now set layout
                for (int i = 0; i < chart1.Series.Count; i++)
                {
                    chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    chart1.Series[i].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
                }
                if (cb_PCT.Checked)
                    chart1.ChartAreas[0].AxisY.LabelStyle.Format = "0.00%"; 
                else
                    chart1.ChartAreas[0].AxisY.LabelStyle.Format = "$#,###";
            }
            else
            {
                // Not Accum
                DataView dvData = new DataView(dt_ChartProfit);
                chart1.DataBindCrossTable(dvData, "BBG_Ticker", "EffectiveDate", myColumnAccum, ""); //, "Label=Profit{C}");

                // Now set layout as Stacked Area
                int mySeriesCount = chart1.Series.Count;
                for (int i = 0; i < mySeriesCount; i++)
                {
                    if (chart1.Series[i].Name == "Total")
                    {
                        chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        chart1.Series[i].Color = Color.Black;
                        chart1.Series[i].BorderWidth = 3;
                    }
                    else
                        chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedArea;
                    chart1.Series[i].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
                    chart1.Series[i].Tag = chart1.Series[i].Name;
                }
                if (mySeriesCount == 1)
                {
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    //chart1.Series[0].Color = Color.Black;
                    chart1.Series[0].BorderWidth = 2;
                }
                if (cb_PCT.Checked)
                    chart1.ChartAreas[0].AxisY.LabelStyle.Format = "0.00%";
                else
                    chart1.ChartAreas[0].AxisY.LabelStyle.Format = "$#,###";
            }

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "dd-MMM-yy";
            if (chart1.Legends.Count>0)
            {
                chart1.Legends[0].LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Table;
                chart1.Legends[0].LegendItemOrder = System.Windows.Forms.DataVisualization.Charting.LegendItemOrder.ReversedSeriesOrder;
            }
            // Redraw
            chart1.Invalidate();
            chart1.ResetAutoValues();

        } //LoadChart()

        private void bt_Request_Click(object sender, EventArgs e)
        {
            LoadTrades();

        } //bt_Request_Click()

        private void cb_hasFundID_CheckedChanged(object sender, EventArgs e)
        {
            cb_Fund.Enabled = cb_hasFundID.Checked;
            cb_Fund_SelectionChangeCommitted(null, null);
        }

        private void cb_hasPortfolioID_CheckedChanged(object sender, EventArgs e)
        {
            cb_Portfolio.Enabled = cb_hasPortfolioID.Checked;
            cb_Portfolio_SelectionChangeCommitted(null, null);
        }

        private void cb_hasBBG_Ticker_CheckedChanged(object sender, EventArgs e)
        {
            tb_BBG_Ticker.Enabled = cb_hasBBG_Ticker.Checked;
            cb_hasBBG_Ticker_Exclude.Enabled = cb_hasBBG_Ticker.Checked;
        }

        private void cb_hasFromDate_CheckedChanged(object sender, EventArgs e)
        {
            dtp_FromDate.Enabled = cb_hasFromDate.Checked;
        }

        private void cb_hasToDate_CheckedChanged(object sender, EventArgs e)
        {
            dtp_ToDate.Enabled = cb_hasToDate.Checked;
        }

        private void dg_ReportProfit_Sorted(object sender, EventArgs e)
        {
            FormatTrades();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormatTrades();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage.Name == "tp_Chart")
                cb_DailyProfitnLoss.Visible = true;
            else
                cb_DailyProfitnLoss.Visible = false;
        }

        private void tb_BBG_Ticker_KeyDown(object sender, KeyEventArgs e)
        {
            String inKeyCode = e.KeyCode.ToString();
            SystemLibrary.BBGOnPreviewKeyDown(sender, inKeyCode);
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            // Call HitTest
            HitTestResult result = chart1.HitTest(e.X, e.Y);

            // Reset Data Point Attributes
            foreach (Series mySeries in chart1.Series)
                foreach (DataPoint point in mySeries.Points)
                {
                    point.BackSecondaryColor = Color.Black;
                    point.BackHatchStyle = ChartHatchStyle.None;
                    //point.BorderWidth = 1;
                    point.IsValueShownAsLabel = false;
                    point.Label = "";
                    
                }

            // If the mouse if over a data point
            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                // Find selected data point
                String myFormat = chart1.ChartAreas[0].AxisY.LabelStyle.Format;
                DataPoint point = chart1.Series[result.Series.Name].Points[result.PointIndex];

                // Change the appearance of the data point
                point.BackSecondaryColor = Color.White;
                point.BackHatchStyle = ChartHatchStyle.Percent25;
                //point.BorderWidth = 2;
                if (chart1.Series.Count>1)
                    point.Label = result.Series.Name + "\r\n" + Convert.ToDouble(point.YValues[0]).ToString(myFormat);
                else
                    point.Label = Convert.ToDouble(point.YValues[0]).ToString(myFormat);
                point.LabelForeColor = Color.Blue;
                //point.Font.Bold = true;
                Font FontBold = new Font(point.Font, FontStyle.Bold);
                //point.Font = FontBold;
            }
            else
            {
                // Set default cursor
                this.Cursor = Cursors.Default;
            }

        } //chart1_MouseMove()

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            // Call HitTest
            HitTestResult result = chart1.HitTest(e.X, e.Y);

            // If the mouse if over a data point
            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                DataPoint point = chart1.Series[result.Series.Name].Points[result.PointIndex];

                DateTime myDate = DateTime.FromOADate(point.XValue);

                if (dtp_FromDate.Value == dtp_ToDate.Value && bt_Back.Visible == true)
                {
                    bt_Back_Click(null, null);
                }
                else
                {
                    // Set up fields for back button
                    Prev_cb_hasFromDate = cb_hasFromDate.Checked;
                    Prev_cb_hasToDate = cb_hasToDate.Checked;
                    prev_cb_OnlyTotal = cb_OnlyTotal.Checked;
                    prev_dtp_FromDate = dtp_FromDate.Value;
                    prev_dtp_ToDate = dtp_ToDate.Value;
                    prev_cb_DailyProfitnLoss = cb_DailyProfitnLoss.Checked;

                    // Set new fields
                    cb_hasFromDate.Checked = true;
                    dtp_FromDate.Value = myDate;
                    cb_hasToDate.Checked = true;
                    dtp_ToDate.Value = myDate;
                    cb_OnlyTotal.Checked = false;
                    cb_DailyProfitnLoss.Checked = true;
                    bt_Back.Visible = true;
                    LoadTrades();
                }
            }

        } //chart1_MouseDown()

        private void bt_Back_Click(object sender, EventArgs e)
        {
            // Set new fields
            cb_hasFromDate.Checked = Prev_cb_hasFromDate;
            dtp_FromDate.Value = prev_dtp_FromDate;
            cb_hasToDate.Checked = Prev_cb_hasToDate;
            dtp_ToDate.Value = prev_dtp_ToDate;
            cb_OnlyTotal.Checked = prev_cb_OnlyTotal;
            cb_DailyProfitnLoss.Checked = prev_cb_DailyProfitnLoss;
            LoadTrades();
            bt_Back.Visible = false;

        } //bt_Back_Click()

        private void ReportProfit_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);

        } //ReportProfit_Shown()


    }
}
