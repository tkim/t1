namespace T1MultiAsset
{
    partial class ReportProfit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint9 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(39083, 10000);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint10 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(39448, -5000);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint11 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(39814, 45000);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint12 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(40179, 20000);
            this.cb_hasFundID = new System.Windows.Forms.CheckBox();
            this.cb_Fund = new System.Windows.Forms.ComboBox();
            this.cb_Portfolio = new System.Windows.Forms.ComboBox();
            this.cb_hasPortfolioID = new System.Windows.Forms.CheckBox();
            this.cb_hasBBG_Ticker = new System.Windows.Forms.CheckBox();
            this.tb_BBG_Ticker = new System.Windows.Forms.TextBox();
            this.dtp_FromDate = new System.Windows.Forms.DateTimePicker();
            this.cb_hasFromDate = new System.Windows.Forms.CheckBox();
            this.cb_hasToDate = new System.Windows.Forms.CheckBox();
            this.dtp_ToDate = new System.Windows.Forms.DateTimePicker();
            this.dg_ReportProfit = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.bt_Request = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tp_Chart = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tp_Table = new System.Windows.Forms.TabPage();
            this.cb_DailyProfitnLoss = new System.Windows.Forms.CheckBox();
            this.cb_hasBBG_Ticker_Exclude = new System.Windows.Forms.CheckBox();
            this.cb_PCT = new System.Windows.Forms.CheckBox();
            this.cb_OnlyTotal = new System.Windows.Forms.CheckBox();
            this.bt_Back = new System.Windows.Forms.Button();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.cb_BySector = new System.Windows.Forms.CheckBox();
            this.cb_ByCountry = new System.Windows.Forms.CheckBox();
            this.cb_ByIndustry = new System.Windows.Forms.CheckBox();
            this.cb_BySubIndustry = new System.Windows.Forms.CheckBox();
            this.cb_TotalByStock = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dg_ReportProfit)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tp_Chart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tp_Table.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_hasFundID
            // 
            this.cb_hasFundID.AutoSize = true;
            this.cb_hasFundID.ForeColor = System.Drawing.Color.DarkGreen;
            this.cb_hasFundID.Location = new System.Drawing.Point(12, 25);
            this.cb_hasFundID.Name = "cb_hasFundID";
            this.cb_hasFundID.Size = new System.Drawing.Size(15, 14);
            this.cb_hasFundID.TabIndex = 0;
            this.cb_hasFundID.UseVisualStyleBackColor = true;
            this.cb_hasFundID.CheckedChanged += new System.EventHandler(this.cb_hasFundID_CheckedChanged);
            // 
            // cb_Fund
            // 
            this.cb_Fund.Enabled = false;
            this.cb_Fund.FormattingEnabled = true;
            this.cb_Fund.Location = new System.Drawing.Point(36, 24);
            this.cb_Fund.Name = "cb_Fund";
            this.cb_Fund.Size = new System.Drawing.Size(147, 21);
            this.cb_Fund.TabIndex = 1;
            this.cb_Fund.SelectionChangeCommitted += new System.EventHandler(this.cb_Fund_SelectionChangeCommitted);
            // 
            // cb_Portfolio
            // 
            this.cb_Portfolio.Enabled = false;
            this.cb_Portfolio.FormattingEnabled = true;
            this.cb_Portfolio.Location = new System.Drawing.Point(218, 25);
            this.cb_Portfolio.Name = "cb_Portfolio";
            this.cb_Portfolio.Size = new System.Drawing.Size(147, 21);
            this.cb_Portfolio.TabIndex = 3;
            this.cb_Portfolio.SelectionChangeCommitted += new System.EventHandler(this.cb_Portfolio_SelectionChangeCommitted);
            // 
            // cb_hasPortfolioID
            // 
            this.cb_hasPortfolioID.AutoSize = true;
            this.cb_hasPortfolioID.ForeColor = System.Drawing.Color.DarkGreen;
            this.cb_hasPortfolioID.Location = new System.Drawing.Point(194, 26);
            this.cb_hasPortfolioID.Name = "cb_hasPortfolioID";
            this.cb_hasPortfolioID.Size = new System.Drawing.Size(15, 14);
            this.cb_hasPortfolioID.TabIndex = 2;
            this.cb_hasPortfolioID.UseVisualStyleBackColor = true;
            this.cb_hasPortfolioID.CheckedChanged += new System.EventHandler(this.cb_hasPortfolioID_CheckedChanged);
            // 
            // cb_hasBBG_Ticker
            // 
            this.cb_hasBBG_Ticker.AutoSize = true;
            this.cb_hasBBG_Ticker.ForeColor = System.Drawing.Color.DarkGreen;
            this.cb_hasBBG_Ticker.Location = new System.Drawing.Point(381, 27);
            this.cb_hasBBG_Ticker.Name = "cb_hasBBG_Ticker";
            this.cb_hasBBG_Ticker.Size = new System.Drawing.Size(15, 14);
            this.cb_hasBBG_Ticker.TabIndex = 4;
            this.cb_hasBBG_Ticker.UseVisualStyleBackColor = true;
            this.cb_hasBBG_Ticker.CheckedChanged += new System.EventHandler(this.cb_hasBBG_Ticker_CheckedChanged);
            // 
            // tb_BBG_Ticker
            // 
            this.tb_BBG_Ticker.Enabled = false;
            this.tb_BBG_Ticker.Location = new System.Drawing.Point(408, 25);
            this.tb_BBG_Ticker.Name = "tb_BBG_Ticker";
            this.tb_BBG_Ticker.Size = new System.Drawing.Size(141, 20);
            this.tb_BBG_Ticker.TabIndex = 5;
            this.tb_BBG_Ticker.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_BBG_Ticker_KeyDown);
            // 
            // dtp_FromDate
            // 
            this.dtp_FromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_FromDate.Enabled = false;
            this.dtp_FromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_FromDate.Location = new System.Drawing.Point(36, 73);
            this.dtp_FromDate.Name = "dtp_FromDate";
            this.dtp_FromDate.Size = new System.Drawing.Size(208, 20);
            this.dtp_FromDate.TabIndex = 6;
            // 
            // cb_hasFromDate
            // 
            this.cb_hasFromDate.AutoSize = true;
            this.cb_hasFromDate.ForeColor = System.Drawing.Color.DarkGreen;
            this.cb_hasFromDate.Location = new System.Drawing.Point(12, 77);
            this.cb_hasFromDate.Name = "cb_hasFromDate";
            this.cb_hasFromDate.Size = new System.Drawing.Size(15, 14);
            this.cb_hasFromDate.TabIndex = 7;
            this.cb_hasFromDate.UseVisualStyleBackColor = true;
            this.cb_hasFromDate.CheckedChanged += new System.EventHandler(this.cb_hasFromDate_CheckedChanged);
            // 
            // cb_hasToDate
            // 
            this.cb_hasToDate.AutoSize = true;
            this.cb_hasToDate.ForeColor = System.Drawing.Color.DarkGreen;
            this.cb_hasToDate.Location = new System.Drawing.Point(256, 77);
            this.cb_hasToDate.Name = "cb_hasToDate";
            this.cb_hasToDate.Size = new System.Drawing.Size(15, 14);
            this.cb_hasToDate.TabIndex = 9;
            this.cb_hasToDate.UseVisualStyleBackColor = true;
            this.cb_hasToDate.CheckedChanged += new System.EventHandler(this.cb_hasToDate_CheckedChanged);
            // 
            // dtp_ToDate
            // 
            this.dtp_ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_ToDate.Enabled = false;
            this.dtp_ToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_ToDate.Location = new System.Drawing.Point(280, 73);
            this.dtp_ToDate.Name = "dtp_ToDate";
            this.dtp_ToDate.Size = new System.Drawing.Size(208, 20);
            this.dtp_ToDate.TabIndex = 8;
            // 
            // dg_ReportProfit
            // 
            this.dg_ReportProfit.AllowUserToAddRows = false;
            this.dg_ReportProfit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_ReportProfit.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dg_ReportProfit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_ReportProfit.Location = new System.Drawing.Point(6, 6);
            this.dg_ReportProfit.Name = "dg_ReportProfit";
            this.dg_ReportProfit.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.MidnightBlue;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_ReportProfit.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dg_ReportProfit.RowHeadersVisible = false;
            this.dg_ReportProfit.Size = new System.Drawing.Size(1007, 394);
            this.dg_ReportProfit.TabIndex = 10;
            this.dg_ReportProfit.Sorted += new System.EventHandler(this.dg_ReportProfit_Sorted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DarkBlue;
            this.label1.Location = new System.Drawing.Point(33, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Fund Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.DarkBlue;
            this.label2.Location = new System.Drawing.Point(215, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Portfolio Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.DarkBlue;
            this.label3.Location = new System.Drawing.Point(405, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Bloomberg Ticker";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.DarkBlue;
            this.label4.Location = new System.Drawing.Point(33, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "From Date";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.DarkBlue;
            this.label5.Location = new System.Drawing.Point(277, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "To Date";
            // 
            // bt_Request
            // 
            this.bt_Request.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Request.ForeColor = System.Drawing.Color.DarkBlue;
            this.bt_Request.Location = new System.Drawing.Point(534, 72);
            this.bt_Request.Name = "bt_Request";
            this.bt_Request.Size = new System.Drawing.Size(85, 20);
            this.bt_Request.TabIndex = 20;
            this.bt_Request.Text = "Request";
            this.bt_Request.UseVisualStyleBackColor = true;
            this.bt_Request.Click += new System.EventHandler(this.bt_Request_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tp_Chart);
            this.tabControl1.Controls.Add(this.tp_Table);
            this.tabControl1.Location = new System.Drawing.Point(12, 120);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1027, 432);
            this.tabControl1.TabIndex = 21;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tp_Chart
            // 
            this.tp_Chart.Controls.Add(this.chart1);
            this.tp_Chart.Location = new System.Drawing.Point(4, 22);
            this.tp_Chart.Name = "tp_Chart";
            this.tp_Chart.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Chart.Size = new System.Drawing.Size(1019, 406);
            this.tp_Chart.TabIndex = 0;
            this.tp_Chart.Text = "Profit & Loss";
            this.tp_Chart.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            this.chart1.AllowDrop = true;
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chart1.BackColor = System.Drawing.Color.PowderBlue;
            this.chart1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            this.chart1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chart1.BackImageWrapMode = System.Windows.Forms.DataVisualization.Charting.ChartImageWrapMode.Scaled;
            this.chart1.BorderlineColor = System.Drawing.Color.DimGray;
            this.chart1.BorderSkin.BackImageWrapMode = System.Windows.Forms.DataVisualization.Charting.ChartImageWrapMode.Scaled;
            this.chart1.BorderSkin.BorderWidth = 0;
            chartArea3.BackColor = System.Drawing.Color.Transparent;
            chartArea3.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chartArea3.BackSecondaryColor = System.Drawing.Color.White;
            chartArea3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea3.Name = "ChartArea1";
            chartArea3.ShadowColor = System.Drawing.Color.Transparent;
            this.chart1.ChartAreas.Add(chartArea3);
            legend3.BackColor = System.Drawing.Color.Transparent;
            legend3.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            legend3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend3.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row;
            legend3.Name = "Legend1";
            this.chart1.Legends.Add(legend3);
            this.chart1.Location = new System.Drawing.Point(3, 3);
            this.chart1.Margin = new System.Windows.Forms.Padding(0);
            this.chart1.Name = "chart1";
            series3.BorderWidth = 3;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedArea;
            series3.Legend = "Legend1";
            series3.Name = "Profit";
            series3.Points.Add(dataPoint9);
            series3.Points.Add(dataPoint10);
            series3.Points.Add(dataPoint11);
            series3.Points.Add(dataPoint12);
            series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            series3.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(1013, 403);
            this.chart1.TabIndex = 5;
            this.chart1.Text = "chart1";
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            this.chart1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseDown);
            // 
            // tp_Table
            // 
            this.tp_Table.Controls.Add(this.dg_ReportProfit);
            this.tp_Table.Location = new System.Drawing.Point(4, 22);
            this.tp_Table.Name = "tp_Table";
            this.tp_Table.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Table.Size = new System.Drawing.Size(1019, 406);
            this.tp_Table.TabIndex = 1;
            this.tp_Table.Text = "Table";
            this.tp_Table.UseVisualStyleBackColor = true;
            // 
            // cb_DailyProfitnLoss
            // 
            this.cb_DailyProfitnLoss.AutoSize = true;
            this.cb_DailyProfitnLoss.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_DailyProfitnLoss.ForeColor = System.Drawing.Color.DarkGreen;
            this.cb_DailyProfitnLoss.Location = new System.Drawing.Point(654, 100);
            this.cb_DailyProfitnLoss.Name = "cb_DailyProfitnLoss";
            this.cb_DailyProfitnLoss.Size = new System.Drawing.Size(129, 17);
            this.cb_DailyProfitnLoss.TabIndex = 22;
            this.cb_DailyProfitnLoss.Text = "Daily Profit && Loss";
            this.cb_DailyProfitnLoss.UseVisualStyleBackColor = true;
            // 
            // cb_hasBBG_Ticker_Exclude
            // 
            this.cb_hasBBG_Ticker_Exclude.AutoSize = true;
            this.cb_hasBBG_Ticker_Exclude.Enabled = false;
            this.cb_hasBBG_Ticker_Exclude.ForeColor = System.Drawing.Color.Black;
            this.cb_hasBBG_Ticker_Exclude.Location = new System.Drawing.Point(555, 28);
            this.cb_hasBBG_Ticker_Exclude.Name = "cb_hasBBG_Ticker_Exclude";
            this.cb_hasBBG_Ticker_Exclude.Size = new System.Drawing.Size(102, 17);
            this.cb_hasBBG_Ticker_Exclude.TabIndex = 23;
            this.cb_hasBBG_Ticker_Exclude.Text = "Exclude Tickers";
            this.cb_hasBBG_Ticker_Exclude.UseVisualStyleBackColor = true;
            // 
            // cb_PCT
            // 
            this.cb_PCT.AutoSize = true;
            this.cb_PCT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_PCT.ForeColor = System.Drawing.Color.DarkGreen;
            this.cb_PCT.Location = new System.Drawing.Point(799, 100);
            this.cb_PCT.Name = "cb_PCT";
            this.cb_PCT.Size = new System.Drawing.Size(70, 17);
            this.cb_PCT.TabIndex = 24;
            this.cb_PCT.Text = "Percent";
            this.cb_PCT.UseVisualStyleBackColor = true;
            // 
            // cb_OnlyTotal
            // 
            this.cb_OnlyTotal.AutoSize = true;
            this.cb_OnlyTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_OnlyTotal.ForeColor = System.Drawing.Color.DarkBlue;
            this.cb_OnlyTotal.Location = new System.Drawing.Point(674, 25);
            this.cb_OnlyTotal.Name = "cb_OnlyTotal";
            this.cb_OnlyTotal.Size = new System.Drawing.Size(84, 17);
            this.cb_OnlyTotal.TabIndex = 25;
            this.cb_OnlyTotal.Text = "Only Total";
            this.cb_OnlyTotal.UseVisualStyleBackColor = true;
            // 
            // bt_Back
            // 
            this.bt_Back.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Back.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Back.ForeColor = System.Drawing.Color.RoyalBlue;
            this.bt_Back.Location = new System.Drawing.Point(964, 113);
            this.bt_Back.Name = "bt_Back";
            this.bt_Back.Size = new System.Drawing.Size(75, 23);
            this.bt_Back.TabIndex = 6;
            this.bt_Back.Text = "Back";
            this.bt_Back.UseVisualStyleBackColor = true;
            this.bt_Back.Visible = false;
            this.bt_Back.Click += new System.EventHandler(this.bt_Back_Click);
            // 
            // toolTip2
            // 
            this.toolTip2.IsBalloon = true;
            // 
            // cb_BySector
            // 
            this.cb_BySector.AutoSize = true;
            this.cb_BySector.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_BySector.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cb_BySector.Location = new System.Drawing.Point(854, 23);
            this.cb_BySector.Name = "cb_BySector";
            this.cb_BySector.Size = new System.Drawing.Size(81, 17);
            this.cb_BySector.TabIndex = 26;
            this.cb_BySector.Text = "By Sector";
            this.cb_BySector.UseVisualStyleBackColor = true;
            // 
            // cb_ByCountry
            // 
            this.cb_ByCountry.AutoSize = true;
            this.cb_ByCountry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_ByCountry.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cb_ByCountry.Location = new System.Drawing.Point(764, 23);
            this.cb_ByCountry.Name = "cb_ByCountry";
            this.cb_ByCountry.Size = new System.Drawing.Size(87, 17);
            this.cb_ByCountry.TabIndex = 27;
            this.cb_ByCountry.Text = "By Country";
            this.cb_ByCountry.UseVisualStyleBackColor = true;
            // 
            // cb_ByIndustry
            // 
            this.cb_ByIndustry.AutoSize = true;
            this.cb_ByIndustry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_ByIndustry.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cb_ByIndustry.Location = new System.Drawing.Point(941, 22);
            this.cb_ByIndustry.Name = "cb_ByIndustry";
            this.cb_ByIndustry.Size = new System.Drawing.Size(89, 17);
            this.cb_ByIndustry.TabIndex = 28;
            this.cb_ByIndustry.Text = "By Industry";
            this.cb_ByIndustry.UseVisualStyleBackColor = true;
            // 
            // cb_BySubIndustry
            // 
            this.cb_BySubIndustry.AutoSize = true;
            this.cb_BySubIndustry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_BySubIndustry.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cb_BySubIndustry.Location = new System.Drawing.Point(854, 53);
            this.cb_BySubIndustry.Name = "cb_BySubIndustry";
            this.cb_BySubIndustry.Size = new System.Drawing.Size(115, 17);
            this.cb_BySubIndustry.TabIndex = 29;
            this.cb_BySubIndustry.Text = "By Sub-Industry";
            this.cb_BySubIndustry.UseVisualStyleBackColor = true;
            // 
            // cb_TotalByStock
            // 
            this.cb_TotalByStock.AutoSize = true;
            this.cb_TotalByStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_TotalByStock.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cb_TotalByStock.Location = new System.Drawing.Point(674, 53);
            this.cb_TotalByStock.Name = "cb_TotalByStock";
            this.cb_TotalByStock.Size = new System.Drawing.Size(110, 17);
            this.cb_TotalByStock.TabIndex = 30;
            this.cb_TotalByStock.Text = "Total By Stock";
            this.cb_TotalByStock.UseVisualStyleBackColor = true;
            this.cb_TotalByStock.Visible = false;
            // 
            // ReportProfit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 564);
            this.Controls.Add(this.cb_TotalByStock);
            this.Controls.Add(this.cb_BySubIndustry);
            this.Controls.Add(this.cb_ByIndustry);
            this.Controls.Add(this.cb_ByCountry);
            this.Controls.Add(this.cb_BySector);
            this.Controls.Add(this.bt_Back);
            this.Controls.Add(this.cb_OnlyTotal);
            this.Controls.Add(this.cb_PCT);
            this.Controls.Add(this.cb_hasBBG_Ticker_Exclude);
            this.Controls.Add(this.cb_DailyProfitnLoss);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.bt_Request);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_hasToDate);
            this.Controls.Add(this.dtp_ToDate);
            this.Controls.Add(this.cb_hasFromDate);
            this.Controls.Add(this.dtp_FromDate);
            this.Controls.Add(this.tb_BBG_Ticker);
            this.Controls.Add(this.cb_hasBBG_Ticker);
            this.Controls.Add(this.cb_Portfolio);
            this.Controls.Add(this.cb_hasPortfolioID);
            this.Controls.Add(this.cb_Fund);
            this.Controls.Add(this.cb_hasFundID);
            this.Name = "ReportProfit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Profit & Loss";
            this.Load += new System.EventHandler(this.ReportProfit_Load);
            this.Shown += new System.EventHandler(this.ReportProfit_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dg_ReportProfit)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tp_Chart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tp_Table.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cb_hasFundID;
        private System.Windows.Forms.ComboBox cb_Fund;
        private System.Windows.Forms.ComboBox cb_Portfolio;
        private System.Windows.Forms.CheckBox cb_hasPortfolioID;
        private System.Windows.Forms.CheckBox cb_hasBBG_Ticker;
        private System.Windows.Forms.TextBox tb_BBG_Ticker;
        private System.Windows.Forms.DateTimePicker dtp_FromDate;
        private System.Windows.Forms.CheckBox cb_hasFromDate;
        private System.Windows.Forms.CheckBox cb_hasToDate;
        private System.Windows.Forms.DateTimePicker dtp_ToDate;
        private System.Windows.Forms.DataGridView dg_ReportProfit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button bt_Request;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tp_Chart;
        private System.Windows.Forms.TabPage tp_Table;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.CheckBox cb_DailyProfitnLoss;
        private System.Windows.Forms.CheckBox cb_hasBBG_Ticker_Exclude;
        private System.Windows.Forms.CheckBox cb_PCT;
        private System.Windows.Forms.CheckBox cb_OnlyTotal;
        private System.Windows.Forms.Button bt_Back;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.CheckBox cb_BySector;
        private System.Windows.Forms.CheckBox cb_ByCountry;
        private System.Windows.Forms.CheckBox cb_ByIndustry;
        private System.Windows.Forms.CheckBox cb_BySubIndustry;
        private System.Windows.Forms.CheckBox cb_TotalByStock;
    }
}