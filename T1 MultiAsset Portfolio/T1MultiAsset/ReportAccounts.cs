using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PrintDataGrid;
using DGVPrinterHelper;
using Microsoft.Reporting.WinForms;


namespace T1MultiAsset
{
    public partial class ReportAccounts : Form
    {
        // Global Variables
        public Form1 ParentForm1;
        public String FundID;
        DataTable dt_TaxParcelsSecuritiesOnHand;
        DataTable dt_Fund;
        DataTable dt_ShowAccruals;
        DataTable dt_PayableReceivable;
        DataTable dt_45DayRuleSummary;
        DataTable dt_45DayRuleTest;
        DataTable dt_TaxParcels;
        DataTable dt_BAS_Australia;
        DataTable dt_ClosingStock_UnrealisedProfit;
        DataTable dt_UnrealisedProfit_PayableReceivable;
        DataTable dt_BS_Positions;
        DataTable dt_BalanceSheet;
        DataTable dt_ProfitLoss;
        DataTable dt_TrialBalance;
        //private int CXLocation = 0;
        //private int CYLocation = 0;

        public struct ReportParameters
        {
            public DateTime FromDate;
            public DateTime ToDate;
            public String FundID;
            public String Title;
            public String SubTitle;
            public String Footer;
            public Boolean Landscape;
            public ReportParameters(Boolean myDummy)
            {
                FromDate = SystemLibrary.f_Today();
                ToDate = SystemLibrary.f_Today();
                FundID = "";
                Title = "";
                SubTitle = "";
                Footer = "";
                Landscape = false;
            }
        }
  


        public ReportAccounts()
        {
            InitializeComponent();
            LoadFund();
        }

        private void ReportAccounts_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            // Set the To Date and From Date to match End of Last Financial Year? Or Quarter? Or ???
            dtp_FromDate.Value = DateTime.Parse("1-Jul-2012");
            dtp_ToDate.Value = DateTime.Parse("30-Jun-2013");
            cb_BAS.Text = "Quarter to: " + dtp_ToDate.Value.ToString("dd-MMM-yyyy");

        } //ReportAccounts_Load()


        public void FromParent(Form inForm, int inFundID)
        {
            ParentForm1 = (Form1)inForm;
            FundID = inFundID.ToString();

        } //FromParent()

        private void LoadFund()
        {
            // Local Variables
            String mySql;

            // Load the Fund ddlb
            try
            {
                // Need FundAmount as Currency in Top half of SQL
                mySql = "Select FundId, FundName, FundAmount, crncy, ShortName, ParentFundID " +
                        "From   Fund " +
                        "Where  AllowTrade = 'Y' " +
                        "And	exists (Select 'x' " +
                        "			From	Fund b " +
                        "			Where	b.ParentFundID = Fund.ParentFundID " +
                        "			And		b.FundID = Fund.ParentFundID " +
                        "			And		b. Active = 'Y'  " +
                        "			) " +
                        "Order By FundName ";
                dt_Fund = SystemLibrary.SQLSelectToDataTable(mySql);
                cb_Fund.DataSource = dt_Fund;
                cb_Fund.DisplayMember = "FundName";
                cb_Fund.ValueMember = "FundId";
                if (cb_Fund.Items.Count>0)
                    cb_Fund.SelectedIndex = 0;
            }
            catch { }

        } // LoadFund()


        private void bt_Request_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;
            String myFromDate = dtp_FromDate.Value.ToString("dd-MMM-yyyy");
            String myFromDate_Minus1 = dtp_FromDate.Value.AddDays(-1).ToString("dd-MMM-yyyy");
            String myToDate = dtp_ToDate.Value.ToString("dd-MMM-yyyy");
            String myFundID = ((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[0].ToString();;
            String myParentFundID = ((DataRowView)(cb_Fund.SelectedItem)).Row.ItemArray[5].ToString();
            ReportParameters myParams = new ReportParameters(true);
            String myFX_Prev;


            // hourglass cursor
            Cursor.Current = Cursors.WaitCursor;


            if (cb_UseParentFund.Checked)
                myFundID = myParentFundID;
                

            switch(tabControl1.SelectedTab.Name)
            {
                case "tp_ProfitLoss":
                    mySql = "Exec sp_PL_Statement @ParentFundID = " + myFundID + ", @FromDate = '" + myFromDate_Minus1 + "', @ToDate = '" + myToDate + "', @CashAccountsOnly = '" + SystemLibrary.Bool_To_YN(cb_CashAccountsOnly_PL.Checked) + "' ";
                    dt_ProfitLoss = SystemLibrary.SQLSelectToDataTable(mySql);

                    {
                        // Crystal Report
                        cr_ProfitLoss objRpt = new cr_ProfitLoss();
                        objRpt.SetDataSource(dt_ProfitLoss);

                        String FundName = "";
                        String BalanceDate = "";
                        String BalanceDate_Title = "";
                        if (dt_ProfitLoss.Rows.Count > 0)
                        {
                            FundName = SystemLibrary.ToString(dt_ProfitLoss.Rows[0]["FundName"]);
                            BalanceDate = Convert.ToDateTime(dt_ProfitLoss.Rows[0]["FromDate"]).ToString("MMM yy") +
                                          " - " +
                                          Convert.ToDateTime(dt_ProfitLoss.Rows[0]["ToDate"]).ToString("MMM yy");
                            BalanceDate_Title = Convert.ToDateTime(dt_ProfitLoss.Rows[0]["FromDate"]).ToString("MMMM yyyy") +
                                          " through " +
                                          Convert.ToDateTime(dt_ProfitLoss.Rows[0]["ToDate"]).ToString("MMMM yyyy");
                        }

                        CrystalDecisions.CrystalReports.Engine.TextObject root;
                        root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["TextTitle_FundName"];
                        root.Text = FundName;
                        root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["TextTitle_ReportName"];
                        root.Text = @"Profit & Loss";
                        root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["TextTitle_BalanceDate"];
                        root.Text = BalanceDate_Title;
                        root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["TextTitle_Basis"];
                        if (cb_CashAccountsOnly_PL.Checked)
                            root.Text = "Cash Basis";
                        else
                            root.Text = "Accrual Basis";

                        root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["Text_EffectiveDate"];
                        root.Text = BalanceDate;

                        // Binding the crystalReportViewer with our report object. 
                        crv_ProfitLoss.ReportSource = objRpt;
                        HideTheTabControl(crv_ProfitLoss);
                    }
                    break;
                case "tp_BalanceSheet":
                    mySql = "Exec sp_BS_Statement @ParentFundID = " + myFundID + ", @BalanceDate = '" + myToDate + "', @CashAccountsOnly = '" + SystemLibrary.Bool_To_YN(cb_CashAccountsOnly.Checked) + "' ";
                    dt_BalanceSheet = SystemLibrary.SQLSelectToDataTable(mySql);
                    {
                        // Crystal Report
                        cr_BalanceSheet objRpt = new cr_BalanceSheet();
                        objRpt.SetDataSource(dt_BalanceSheet);

                        String FundName = "";
                        String BalanceDate = "";
                        String BalanceDate_Title = "";
                        if (dt_BalanceSheet.Rows.Count > 0)
                        {
                            FundName = SystemLibrary.ToString(dt_BalanceSheet.Rows[0]["FundName"]);
                            BalanceDate = Convert.ToDateTime(dt_BalanceSheet.Rows[0]["BalanceDate"]).ToString("MMM dd, yy");
                            BalanceDate_Title = Convert.ToDateTime(dt_BalanceSheet.Rows[0]["BalanceDate"]).ToString("MMMM dd, yyyy");
                        }

                        CrystalDecisions.CrystalReports.Engine.TextObject root;
                        root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["TextTitle_FundName"];
                        root.Text = FundName;
                        root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["TextTitle_ReportName"];
                        root.Text = "Balance Sheet";
                        root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["TextTitle_BalanceDate"];
                        root.Text = "As of " + BalanceDate_Title;
                        root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["TextTitle_Basis"];
                        if (cb_CashAccountsOnly.Checked)
                            root.Text = "Cash Basis";
                        else
                            root.Text = "Accrual Basis";

                        root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["Text_EffectiveDate"];
                        root.Text = BalanceDate;


                        // Binding the crystalReportViewer with our report object. 
                        crv_BalanceSheet.ReportSource = objRpt;
                        HideTheTabControl(crv_BalanceSheet);
                    }
                    break;
                case "tp_TrialBalance":
                    mySql = "Exec sp_TrialBalance @ParentFundID = " + myFundID + ", @FromDate = '" + myFromDate_Minus1 + "', @ToDate = '" + myToDate + "', @CashAccountsOnly = '" + SystemLibrary.Bool_To_YN(cb_CashAccountsOnly_PL.Checked) + "' ";
                    dt_TrialBalance = SystemLibrary.SQLSelectToDataTable(mySql);
                    {
                        // Crystal Report
                        cr_TrialBalance objRpt = new cr_TrialBalance();
                        objRpt.SetDataSource(dt_TrialBalance);

                        String FundName = "";
                        String BalanceDate = "";
                        String BalanceDate_Title = "";
                        if (dt_TrialBalance.Rows.Count > 0)
                        {
                            FundName = SystemLibrary.ToString(dt_TrialBalance.Rows[0]["FundName"]);
                            BalanceDate = Convert.ToDateTime(dt_TrialBalance.Rows[0]["BalanceDate"]).ToString("MMM dd, yy");
                            BalanceDate_Title = Convert.ToDateTime(dt_TrialBalance.Rows[0]["BalanceDate"]).ToString("MMMM dd, yyyy");
                        }

                        CrystalDecisions.CrystalReports.Engine.TextObject root;
                        root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["TextTitle_FundName"];
                        root.Text = FundName;
                        root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["TextTitle_ReportName"];
                        root.Text = "Trial Balance";
                        root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["TextTitle_BalanceDate"];
                        root.Text = "As of " + BalanceDate_Title;
                        root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["TextTitle_Basis"];
                        if (cb_CashAccountsOnly_TB.Checked)
                            root.Text = "Cash Basis";
                        else
                            root.Text = "Accrual Basis";

                        root = (CrystalDecisions.CrystalReports.Engine.TextObject)objRpt.ReportDefinition.ReportObjects["Text_EffectiveDate"];
                        root.Text = BalanceDate;


                        // Binding the crystalReportViewer with our report object. 
                        crv_TrialBalance.ReportSource = objRpt;
                        HideTheTabControl(crv_TrialBalance);
                    }
                    break;
                case "tp_TaxParcels":
                    mySql = "Exec sp_TaxLots_Report @FundID = " + myFundID + ", @FromDate = '" + myFromDate + "', @ToDate = '" + myToDate + "' ";
                    dt_TaxParcels = SystemLibrary.SQLSelectToDataTable(mySql);
                    dgv_TaxParcels.DataSource = dt_TaxParcels;
                    SystemLibrary.SetDataGridView(dgv_TaxParcels);
                    dgv_TaxParcels.Refresh();

                    // Microsoft Report
                    /*
                    rv_TaxParcels.LocalReport.DataSources.Clear();
                    ReportDataSource rprtDTSource = new ReportDataSource("DataSet_TaxParcels_DataTable1", dt_TaxParcels);

                    rv_TaxParcels.LocalReport.DataSources.Add(rprtDTSource);
                    rv_TaxParcels.RefreshReport(); 
                    */

                    // Create a Totals line for each currency / AccountName
                    DataTable dt_TaxParcelsGroups = new DataTable();
                    dt_TaxParcelsGroups.Columns.Add("crncy");
                    dt_TaxParcelsGroups.Columns.Add("AccountName");
                    dt_TaxParcelsGroups.Columns.Add("ProfitFX", System.Type.GetType("System.Decimal"));
                    dt_TaxParcelsGroups.Columns.Add("Profit", System.Type.GetType("System.Decimal"));
                    String myAccountName_Prev = "";
                    myFX_Prev = "";
                    for (int i = 0; i < dt_TaxParcels.Rows.Count; i++)
                    {
                        String myAccountName = dt_TaxParcels.Rows[i]["AccountName"].ToString();
                        String myFX = dt_TaxParcels.Rows[i]["crncy"].ToString();
                        if (myFX != myFX_Prev || myAccountName != myAccountName_Prev)
                        {
                            DataRow[] FoundRows = dt_TaxParcelsGroups.Select("AccountName='" + myAccountName + "' And crncy = '" + myFX +"' ");
                            if (FoundRows.Length == 0)
                            {
                                DataRow dr = dt_TaxParcelsGroups.Rows.Add();
                                dr["crncy"] = myFX;
                                dr["AccountName"] = myAccountName;
                                dr["ProfitFX"] = SystemLibrary.ToDecimal(dt_TaxParcels.Compute("Sum(ProfitFX)", "crncy='" + myFX + "' and AccountName = '" + myAccountName + "'"));
                                dr["Profit"] = SystemLibrary.ToDecimal(dt_TaxParcels.Compute("Sum(Profit)", "crncy='" + myFX + "' and AccountName = '" + myAccountName + "'"));
                            }
                        }
                    }
                    // - Now add the Total Rows
                    for (int i = 0; i < dt_TaxParcelsGroups.Rows.Count; i++)
                    {
                        DataRow dr = dt_TaxParcels.Rows.Add();
                        //dr["BBG_Ticker"] = "Total";
                        dr["crncy"] = dt_TaxParcelsGroups.Rows[i]["crncy"];
                        dr["AccountName"] = dt_TaxParcelsGroups.Rows[i]["AccountName"];
                        dr["ProfitFX"] = dt_TaxParcelsGroups.Rows[i]["ProfitFX"];
                        dr["Profit"] = dt_TaxParcelsGroups.Rows[i]["Profit"];
                        dgv_TaxParcels.Refresh();
                        int myRow = dgv_TaxParcels.Rows.Count - 1;
                        dgv_TaxParcels["AccountName", myRow].Style.BackColor = Color.LightGreen;
                        dgv_TaxParcels["crncy", myRow].Style.BackColor = Color.LightGreen;
                        dgv_TaxParcels["ProfitFX", myRow].Style.BackColor = Color.LightGreen;
                        dgv_TaxParcels["Profit", myRow].Style.BackColor = Color.LightGreen;
                    }
                    SystemLibrary.SetDataGridView(dgv_TaxParcels);

                    myParams.FromDate = dtp_FromDate.Value;
                    myParams.ToDate = dtp_ToDate.Value;
                    myParams.FundID = myFundID;
                    myParams.Title = "Trading Profit & Loss - Tax Parcels";
                    myParams.SubTitle = "Between Settlement Dates of '" + myFromDate + "' and '" + myToDate + "'";
                    myParams.Footer = SystemLibrary.SQLSelectString("Select FundName from Fund Where FundID = " + myFundID);
                    myParams.Landscape = true;
                    dgv_TaxParcels.Tag = myParams;
                   
                    break;
                case "tp_BS_Positions":
                    mySql = "Exec sp_BS_Positions @FundID = " + myFundID + ", @EffectiveDate = '" + myToDate + "' ";
                    dt_BS_Positions = SystemLibrary.SQLSelectToDataTable(mySql);
                    dgv_BS_Positions.DataSource = dt_BS_Positions;
                    SystemLibrary.SetDataGridView(dgv_BS_Positions);
                    dgv_BS_Positions.Refresh();

                    // Create a Totals line for each currency & Long/Short Shares
                    myFX_Prev = "";
                    for (int i = 0; i < dt_BS_Positions.Rows.Count; i++)
                    {
                        String myFX = dt_BS_Positions.Rows[i]["crncy"].ToString();
                        if (myFX != myFX_Prev)
                        {
                            myFX_Prev = myFX;
                            DataRow dr = dt_BS_Positions.Rows.Add();
                            dr["BBG_Ticker"] = "Total";
                            dr["crncy"] = myFX;
                            dr["AmountLocalCCY"] = SystemLibrary.ToDecimal(dt_BS_Positions.Compute("Sum(AmountLocalCCY)", "crncy='" + myFX + "'"));
                            dgv_BS_Positions.Refresh();
                            int myRow = dgv_BS_Positions.Rows.Count - 1;
                            dgv_BS_Positions["crncy", myRow].Style.BackColor = Color.LightGreen;
                            dgv_BS_Positions["AmountLocalCCY", myRow].Style.BackColor = Color.LightGreen;
                        }
                    }
                    SystemLibrary.SetDataGridView(dgv_BS_Positions);

                    myParams.FromDate = dtp_FromDate.Value;
                    myParams.ToDate = dtp_ToDate.Value;
                    myParams.FundID = myFundID;
                    myParams.Title = "Shares - Realised & Unrealised";
                    myParams.SubTitle = "As at: '" + myToDate + "'";
                    myParams.Footer = SystemLibrary.SQLSelectString("Select FundName from Fund Where FundID = " + myFundID);
                    myParams.Landscape = true;
                    dgv_BS_Positions.Tag = myParams;
                    break;
                case "tp_ClosingStock_UnrealisedProfit":
                    mySql = "Exec sp_TaxParcels_SecuritiesOnHand @FundID = " + myFundID + ", @EffectiveDate = '" + myToDate + "', @ReportType = 'Unrealised Profit', @CashBasis = 'Y', @ToTable = null ";
                    dt_ClosingStock_UnrealisedProfit = SystemLibrary.SQLSelectToDataTable(mySql);
                    dgv_ClosingStock_UnrealisedProfit.DataSource = dt_ClosingStock_UnrealisedProfit;
                    SystemLibrary.SetDataGridView(dgv_ClosingStock_UnrealisedProfit);
                    dgv_ClosingStock_UnrealisedProfit.Refresh();

                    // Create a Totals line for each currency
                    myFX_Prev = "";
                    for (int i = 0; i < dt_ClosingStock_UnrealisedProfit.Rows.Count; i++)
                    {
                        String myFX = dt_ClosingStock_UnrealisedProfit.Rows[i]["crncy"].ToString();
                        if (myFX != myFX_Prev)
                        {
                            myFX_Prev = myFX;
                            DataRow dr = dt_ClosingStock_UnrealisedProfit.Rows.Add();
                            dr["BBG_Ticker"] = "Total";
                            dr["crncy"] = myFX;
                            dr["UnrealisedProfit"] = SystemLibrary.ToDecimal(dt_ClosingStock_UnrealisedProfit.Compute("Sum(UnrealisedProfit)", "crncy='" + myFX + "'"));
                            dgv_ClosingStock_UnrealisedProfit.Refresh();
                            int myRow = dgv_ClosingStock_UnrealisedProfit.Rows.Count - 1;
                            dgv_ClosingStock_UnrealisedProfit["crncy", myRow].Style.BackColor = Color.LightGreen;
                            dgv_ClosingStock_UnrealisedProfit["UnrealisedProfit", myRow].Style.BackColor = Color.LightGreen;
                        }
                    }
                    SystemLibrary.SetDataGridView(dgv_ClosingStock_UnrealisedProfit);

                    myParams.FromDate = dtp_FromDate.Value;
                    myParams.ToDate = dtp_ToDate.Value;
                    myParams.FundID = myFundID;
                    myParams.Title = "Closing Stock - Unrealised Profit";
                    myParams.SubTitle = "As at: '" + myToDate + "'";
                    myParams.Footer = SystemLibrary.SQLSelectString("Select FundName from Fund Where FundID = " + myFundID);
                    myParams.Landscape = true;
                    dgv_ClosingStock_UnrealisedProfit.Tag = myParams;
                    break;
                case "tp_UnrealisedProfit_PayableReceivable":
                    mySql = "Exec sp_UnrealisedProfit_on_PayableReceivable @FundID = " + myFundID + ", @EffectiveDate = '" + myToDate + "' ";
                    dt_UnrealisedProfit_PayableReceivable = SystemLibrary.SQLSelectToDataTable(mySql);
                    dgv_UnrealisedProfit_PayableReceivable.DataSource = dt_UnrealisedProfit_PayableReceivable;
                    SystemLibrary.SetDataGridView(dgv_UnrealisedProfit_PayableReceivable);
                    dgv_UnrealisedProfit_PayableReceivable.Refresh();

                    // Create a Totals line for each currency
                    myFX_Prev = "";
                    for (int i = 0; i < dt_UnrealisedProfit_PayableReceivable.Rows.Count; i++)
                    {
                        String myFX = dt_UnrealisedProfit_PayableReceivable.Rows[i]["crncy"].ToString();
                        if (myFX != myFX_Prev)
                        {
                            myFX_Prev = myFX;
                            DataRow dr = dt_UnrealisedProfit_PayableReceivable.Rows.Add();
                            dr["BBG_Ticker"] = "Total";
                            dr["crncy"] = myFX;
                            dr["UnrealisedProfitBaseCCY"] = SystemLibrary.ToDecimal(dt_UnrealisedProfit_PayableReceivable.Compute("Sum(UnrealisedProfitBaseCCY)", "crncy='" + myFX + "'"));
                            dgv_UnrealisedProfit_PayableReceivable.Refresh();
                            int myRow = dgv_UnrealisedProfit_PayableReceivable.Rows.Count - 1;
                            dgv_UnrealisedProfit_PayableReceivable["crncy", myRow].Style.BackColor = Color.LightGreen;
                            dgv_UnrealisedProfit_PayableReceivable["UnrealisedProfitBaseCCY", myRow].Style.BackColor = Color.LightGreen;
                        }
                    }
                    SystemLibrary.SetDataGridView(dgv_UnrealisedProfit_PayableReceivable);

                    myParams.FromDate = dtp_FromDate.Value;
                    myParams.ToDate = dtp_ToDate.Value;
                    myParams.FundID = myFundID;
                    myParams.Title = "Unrealised Profit on Payable/Receivable";
                    myParams.SubTitle = "As at: '" + myToDate + "'";
                    myParams.Footer = SystemLibrary.SQLSelectString("Select FundName from Fund Where FundID = " + myFundID);
                    myParams.Landscape = true;
                    dgv_UnrealisedProfit_PayableReceivable.Tag = myParams;
                    break;
                case "tp_45DayRuleSummary":
                    mySql = "Exec sp_Report_45_Day_Summary_Fund @FundID = " + myFundID + ", @FromDate = '" + myFromDate + "', @ToDate = '" + myToDate + "' ";
                    dt_45DayRuleSummary = SystemLibrary.SQLSelectToDataTable(mySql);
                    dgv_45DayRuleSummary.DataSource = dt_45DayRuleSummary;
                    SystemLibrary.SetDataGridView(dgv_45DayRuleSummary);
                    dgv_45DayRuleSummary.Refresh();

                    // Create summaries
                    {
                        DataRow dr = dt_45DayRuleSummary.Rows.Add();
                        dr["Dividend Amount"] = SystemLibrary.ToDecimal(dt_45DayRuleSummary.Compute("Sum([Dividend Amount])", ""));
                        dr["FC's >= 45 Day"] = SystemLibrary.ToDecimal(dt_45DayRuleSummary.Compute("Sum([FC's >= 45 Day])", ""));
                        dr["Item K"] = SystemLibrary.ToDecimal(dt_45DayRuleSummary.Compute("Sum([Item K])", ""));
                        dr["Item L"] = SystemLibrary.ToDecimal(dt_45DayRuleSummary.Compute("Sum([Item L])", ""));
                        dr["Item M"] = SystemLibrary.ToDecimal(dt_45DayRuleSummary.Compute("Sum([Item M])", ""));
                        dgv_45DayRuleSummary.Refresh();
                        int myRow = dgv_45DayRuleSummary.Rows.Count - 1;
                        dgv_45DayRuleSummary["Dividend Amount", myRow].Style.BackColor = Color.LightGreen;
                        dgv_45DayRuleSummary["FC's >= 45 Day", myRow].Style.BackColor = Color.LightGreen;
                        dgv_45DayRuleSummary["Item K", myRow].Style.BackColor = Color.LightGreen;
                        dgv_45DayRuleSummary["Item L", myRow].Style.BackColor = Color.LightGreen;
                        dgv_45DayRuleSummary["Item M", myRow].Style.BackColor = Color.LightGreen;
                    }
                    SystemLibrary.SetDataGridView(dgv_45DayRuleSummary);
                    dgv_45DayRuleSummary.Refresh();

                    myParams.FromDate = dtp_FromDate.Value;
                    myParams.ToDate = dtp_ToDate.Value;
                    myParams.FundID = myFundID;
                    myParams.Title = "Dividend / Franking Credit - 45 Day Holding Period (LIFO based)";
                    myParams.SubTitle = "As at: " + myToDate;
                    myParams.Footer = SystemLibrary.SQLSelectString("Select FundName from Fund Where FundID = " + myFundID);
                    myParams.Landscape = true;
                    dgv_45DayRuleSummary.Tag = myParams;
                    break;
                case "tp_45DayRuleTest":
                    mySql = "Exec sp_Report_45_Day_Rule_Fund @FundID = " + myFundID + ", @FromDate = '" + myFromDate + "', @ToDate = '" + myToDate + "' ";
                    dt_45DayRuleTest = SystemLibrary.SQLSelectToDataTable(mySql);
                    dgv_45DayRuleTest.DataSource = dt_45DayRuleTest;
                    SystemLibrary.SetDataGridView(dgv_45DayRuleTest);
                    dgv_45DayRuleTest.Refresh();

                    myParams.FromDate = dtp_FromDate.Value;
                    myParams.ToDate = dtp_ToDate.Value;
                    myParams.FundID = myFundID;
                    myParams.Title = "Franking Credit - 45 Day Holding Period Test (LIFO based)";
                    myParams.SubTitle = "As at: " + myToDate;
                    myParams.Footer = SystemLibrary.SQLSelectString("Select FundName from Fund Where FundID = " + myFundID);
                    myParams.Landscape = true;
                    dgv_45DayRuleTest.Tag = myParams;
                    break;
                case "tp_ShowAccruals":
                    mySql = "Exec sp_ShowAccruals @FundID = " + myFundID + ", @EffectiveDate = '" + myToDate + "' ";
                    dt_ShowAccruals = SystemLibrary.SQLSelectToDataTable(mySql);
                    dgv_ShowAccruals.DataSource = dt_ShowAccruals;
                    SystemLibrary.SetDataGridView(dgv_ShowAccruals);
                    dgv_ShowAccruals.Refresh();

                    myParams.FromDate = dtp_FromDate.Value;
                    myParams.ToDate = dtp_ToDate.Value;
                    myParams.FundID = myFundID;
                    myParams.Title = "Accruals at end of period";
                    myParams.SubTitle = "As at: '" + myToDate + "'  (excluding Trades)";
                    myParams.Footer = SystemLibrary.SQLSelectString("Select FundName from Fund Where FundID = " + myFundID);
                    myParams.Landscape = true;
                    dgv_ShowAccruals.Tag = myParams;
                    break;
                case "tp_PayableReceivable":
                    mySql = "Exec sp_ShowPayableReceiveable @FundID = " + myFundID + ", @EffectiveDate = '" + myToDate + "' ";
                    dt_PayableReceivable = SystemLibrary.SQLSelectToDataTable(mySql);
                    dgv_PayableReceivable.DataSource = dt_PayableReceivable;
                    SystemLibrary.SetDataGridView(dgv_PayableReceivable);
                    dgv_PayableReceivable.Refresh();

                    // Create a Totals line for each currency
                    myFX_Prev = "";
                    for (int i = 0; i < dt_PayableReceivable.Rows.Count; i++)
                    {
                        String myFX = dt_PayableReceivable.Rows[i]["crncy"].ToString();
                        if (myFX != myFX_Prev)
                        {
                            myFX_Prev = myFX;
                            DataRow dr = dt_PayableReceivable.Rows.Add();
                            dr["BBG_Ticker"] = "Total";
                            dr["crncy"] = myFX;
                            dr["Amount"] = SystemLibrary.ToDecimal(dt_PayableReceivable.Compute("Sum(Amount)", "crncy='" + myFX + "'"));
                            dgv_PayableReceivable.Refresh();
                            int myRow = dgv_PayableReceivable.Rows.Count - 1;
                            dgv_PayableReceivable["crncy", myRow].Style.BackColor = Color.LightGreen;
                            dgv_PayableReceivable["Amount", myRow].Style.BackColor = Color.LightGreen;
                        }
                    }
                    SystemLibrary.SetDataGridView(dgv_PayableReceivable);

                    myParams.FromDate = dtp_FromDate.Value;
                    myParams.ToDate = dtp_ToDate.Value;
                    myParams.FundID = myFundID;
                    myParams.Title = "Payables / Receivables - Trades not Settled";
                    myParams.SubTitle = "As at: '" + myToDate + "'";
                    myParams.Footer = SystemLibrary.SQLSelectString("Select FundName from Fund Where FundID = " + myFundID);
                    myParams.Landscape = true;
                    dgv_PayableReceivable.Tag = myParams;
                    break;
                case "tp_TaxParcels_SecuritiesOnHand":

                    mySql = "Exec sp_TaxParcels_SecuritiesOnHand @FundID = " + myFundID + ", @EffectiveDate = '" + myToDate + "', @CashBasis = '" + SystemLibrary.Bool_To_YN(cb_UseCashBasis.Checked) + "' ";
                    dt_TaxParcelsSecuritiesOnHand = SystemLibrary.SQLSelectToDataTable(mySql);

                    dgv_TaxParcelsSecuritiesOnHand.DataSource = dt_TaxParcelsSecuritiesOnHand;
                    SystemLibrary.SetDataGridView(dgv_TaxParcelsSecuritiesOnHand);
                    dgv_TaxParcelsSecuritiesOnHand.Refresh();

                    // Create a Totals line for each currency
                    myFX_Prev = "";
                    for (int i = 0; i < dt_TaxParcelsSecuritiesOnHand.Rows.Count; i++)
                    {
                        String myFX = dt_TaxParcelsSecuritiesOnHand.Rows[i]["crncy"].ToString();
                        if (myFX != myFX_Prev)
                        {
                            myFX_Prev = myFX;
                            DataRow dr = dt_TaxParcelsSecuritiesOnHand.Rows.Add();
                            dr["BBG_Ticker"] = "Total";
                            dr["crncy"] = myFX;
                            dr["Value_At_Cost"] = SystemLibrary.ToDecimal(dt_TaxParcelsSecuritiesOnHand.Compute("Sum(Value_At_Cost)", "crncy='" + myFX + "'"));
                            dgv_TaxParcelsSecuritiesOnHand.Refresh();
                            int myRow = dgv_TaxParcelsSecuritiesOnHand.Rows.Count - 1;
                            dgv_TaxParcelsSecuritiesOnHand["crncy", myRow].Style.BackColor = Color.LightGreen;
                            dgv_TaxParcelsSecuritiesOnHand["Value_At_Cost", myRow].Style.BackColor = Color.LightGreen;
                        }
                    }
                    SystemLibrary.SetDataGridView(dgv_TaxParcelsSecuritiesOnHand);

                    myParams.FromDate = dtp_FromDate.Value;
                    myParams.ToDate = dtp_ToDate.Value;
                    myParams.FundID = myFundID;
                    myParams.Title = "Tax Parcels Securities on Hand";
                    myParams.SubTitle = "As at: " + myToDate;
                    if (cb_UseCashBasis.Checked)
                        myParams.SubTitle = myParams.SubTitle + " (Cash Basis)";
                    myParams.Footer = SystemLibrary.SQLSelectString("Select FundName from Fund Where FundID = " + myFundID);
                    myParams.Landscape = true;
                    dgv_TaxParcelsSecuritiesOnHand.Tag = myParams;

                    // Highlight Dates where SettlementDate > ReportDate.
                    for (int i = 0; i < dgv_TaxParcelsSecuritiesOnHand.Rows.Count - 1; i++)
                    {
                        if (Convert.ToDateTime(dgv_TaxParcelsSecuritiesOnHand["SettlementDate", i].Value).CompareTo(Convert.ToDateTime(dgv_TaxParcelsSecuritiesOnHand["EffectiveDate", i].Value)) > 0)
                        {
                            dgv_TaxParcelsSecuritiesOnHand["SettlementDate", i].Style.BackColor = Color.LightGreen;
                        }
                    }
                    break;
                case "tp_BAS_Australia":
                    String LocalFromDate;
                    if (cb_BAS.Checked)
                        LocalFromDate = dtp_ToDate.Value.AddMonths(-3).ToString("dd-MMM-yyyy");
                    else
                        LocalFromDate = dtp_FromDate.Value.AddDays(-1).ToString("dd-MMM-yyyy");

                    // NB: The FromDate needs to be 1 Day before that specified.
                    mySql = "Exec sp_BAS_Statement @ParentFundID = " + myParentFundID + ", @FromDate = '" + LocalFromDate + "', @ToDate = '" + myToDate + "', @ExcludeZeroAmounts ='" + SystemLibrary.Bool_To_YN(cb_ExcludedZeroAmounts.Checked) + "'";
                    dt_BAS_Australia = SystemLibrary.SQLSelectToDataTable(mySql);

                    if (dt_BAS_Australia.Rows.Count == 0)
                    {
                        if (dgv_BAS_Australia.Rows.Count > 0)
                            dgv_BAS_Australia.Rows.Clear();
                    }
                    else
                    {
                        dgv_BAS_Australia.DataSource = dt_BAS_Australia;

                        dgv_BAS_Australia.Columns["FromDate"].Visible = false;
                        dgv_BAS_Australia.Columns["ToDate"].Visible = false;
                        dgv_BAS_Australia.Columns["Type"].Visible = false;
                        dgv_BAS_Australia.Columns["GL"].Visible = false;
                        dgv_BAS_Australia.Columns["FromLocalCCY"].HeaderText = LocalFromDate + " (LocalCCY)";
                        dgv_BAS_Australia.Columns["ToLocalCCY"].HeaderText = "Up to" + myToDate + " (LocalCCY)";
                        dgv_BAS_Australia.Columns["FromBaseCCY"].HeaderText = "Up to" + LocalFromDate;
                        dgv_BAS_Australia.Columns["ToBaseCCY"].HeaderText = myToDate;
                        dgv_BAS_Australia.Columns["ToLocalCCY"].Visible = false;
                        dgv_BAS_Australia.Columns["ToBaseCCY"].Visible = false;

                        SystemLibrary.SetDataGridView(dgv_BAS_Australia);
                        dgv_BAS_Australia.Refresh();

                        myParams.FromDate = dtp_FromDate.Value;
                        myParams.ToDate = dtp_ToDate.Value;
                        myParams.FundID = myFundID;
                        myParams.Title = "Trading BAS Statement (Cash Basis)";
                        if (cb_BAS.Checked)
                            myParams.SubTitle = cb_BAS.Text;
                        else
                            myParams.SubTitle = "Previous Period End " + myFromDate + " to Period End " + myToDate;
                        myParams.Footer = SystemLibrary.SQLSelectString("Select FundName from Fund Where FundID = " + myParentFundID);
                        myParams.Landscape = true;
                        dgv_BAS_Australia.Tag = myParams;


                        // Highlight Revenue versus Expenses records.
                        for (int i = 0; i < dgv_BAS_Australia.Rows.Count; i++)
                        {
                            switch (SystemLibrary.ToString(dgv_BAS_Australia["GL", i].Value))
                            {
                                case "Revenue":
                                    //dgv_BAS_Australia.Rows[i].Style.BackColor = Color.LightGreen;
                                    dgv_BAS_Australia["AccountName", i].Style.BackColor = Color.LightGreen;
                                    dgv_BAS_Australia["AmountThisPeriod", i].Style.BackColor = Color.LightGreen;
                                    dgv_BAS_Australia["BASCode", i].Style.BackColor = Color.LightGreen;
                                    dgv_BAS_Australia["QuickBooks", i].Style.BackColor = Color.LightGreen;
                                    dgv_BAS_Australia["BAS", i].Style.BackColor = Color.LightGreen; // Currency
                                    break;
                                case "Expenses":
                                    //dgv_BAS_Australia.Rows[i].Style.BackColor = Color.LightCoral;
                                    dgv_BAS_Australia["AccountName", i].Style.BackColor = Color.LightCyan;
                                    dgv_BAS_Australia["AmountThisPeriod", i].Style.BackColor = Color.LightCyan;
                                    dgv_BAS_Australia["BASCode", i].Style.BackColor = Color.LightCyan;
                                    dgv_BAS_Australia["QuickBooks", i].Style.BackColor = Color.LightCyan;
                                    dgv_BAS_Australia["BAS", i].Style.BackColor = Color.LightCyan; // Currency
                                    break;
                                default:
                                    dgv_BAS_Australia["AccountName", i].Style.BackColor = Color.LightGray;
                                    dgv_BAS_Australia["AmountThisPeriod", i].Style.BackColor = Color.LightGray;
                                    dgv_BAS_Australia["BAS", i].Style.BackColor = Color.LightGray; // Currency
                                    break;
                            }
                        }
                    }
                    break;
            }

            Cursor.Current = Cursors.Default;

        } //bt_Request_Click()

        private void ReportAccounts_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);
        } //ReportAccounts_Shown()

        private void bt_Print_Click(object sender, EventArgs e)
        {
            ReportParameters myParams = new ReportParameters();
            DGVPrinter printer = new DGVPrinter();

            switch (tabControl1.SelectedTab.Name)
            {
                case "tp_ProfitLoss":
                    // Looking for the document name for the print queue.
                    crv_ProfitLoss.PrintReport();
                    break;
                case "tp_BalanceSheet":
                    crv_BalanceSheet.PrintReport();
                    break;
                case "tp_TaxParcels":
                    myParams = (ReportParameters)dgv_TaxParcels.Tag;

                    printer.PrintColumnHeaders = true;
                    printer.Title = myParams.Title;
                    printer.SubTitle = myParams.SubTitle;
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer = myParams.Footer;
                    printer.FooterSpacing = 15;
                    printer.PageSettings.Landscape = myParams.Landscape;
                    printer.PrintPreviewDataGridView(dgv_TaxParcels);
                    break;
                case "tp_BS_Positions":
                    myParams = (ReportParameters)dgv_BS_Positions.Tag;

                    printer.PrintColumnHeaders = true;
                    printer.Title = myParams.Title;
                    printer.SubTitle = myParams.SubTitle;
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer = myParams.Footer;
                    printer.FooterSpacing = 15;
                    printer.PageSettings.Landscape = myParams.Landscape;
                    printer.PrintPreviewDataGridView(dgv_BS_Positions);
                    break;
                case "tp_UnrealisedProfit_PayableReceivable":
                    myParams = (ReportParameters)dgv_UnrealisedProfit_PayableReceivable.Tag;

                    printer.PrintColumnHeaders = true;
                    printer.Title = myParams.Title;
                    printer.SubTitle = myParams.SubTitle;
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer = myParams.Footer;
                    printer.FooterSpacing = 15;
                    printer.PageSettings.Landscape = myParams.Landscape;
                    printer.PrintPreviewDataGridView(dgv_UnrealisedProfit_PayableReceivable);
                    break;
                case "tp_ClosingStock_UnrealisedProfit":
                    myParams = (ReportParameters)dgv_ClosingStock_UnrealisedProfit.Tag;

                    printer.PrintColumnHeaders = true;
                    printer.Title = myParams.Title;
                    printer.SubTitle = myParams.SubTitle;
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer = myParams.Footer;
                    printer.FooterSpacing = 15;
                    printer.PageSettings.Landscape = myParams.Landscape;
                    printer.PrintPreviewDataGridView(dgv_ClosingStock_UnrealisedProfit);
                    break;
                case "tp_45DayRuleSummary":
                    myParams = (ReportParameters)dgv_45DayRuleSummary.Tag;

                    printer.PrintColumnHeaders = true;
                    printer.Title = myParams.Title;
                    printer.SubTitle = myParams.SubTitle;
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer = myParams.Footer;
                    printer.FooterSpacing = 15;
                    printer.PageSettings.Landscape = myParams.Landscape;
                    printer.PrintPreviewDataGridView(dgv_45DayRuleSummary);
                    break;
                case "tp_45DayRuleTest":
                    myParams = (ReportParameters)dgv_45DayRuleTest.Tag;

                    printer.PrintColumnHeaders = true;
                    printer.Title = myParams.Title;
                    printer.SubTitle = myParams.SubTitle;
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer = myParams.Footer;
                    printer.FooterSpacing = 15;
                    printer.PageSettings.Landscape = myParams.Landscape;
                    printer.PrintPreviewDataGridView(dgv_45DayRuleTest);
                    break;
                case "tp_TaxParcels_SecuritiesOnHand":
                    myParams = (ReportParameters)dgv_TaxParcelsSecuritiesOnHand.Tag;

                    printer.PrintColumnHeaders = true;
                    printer.Title = myParams.Title;
                    printer.SubTitle = myParams.SubTitle;
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer = myParams.Footer;
                    printer.FooterSpacing = 15;
                    printer.PageSettings.Landscape = myParams.Landscape;
                    printer.PrintPreviewDataGridView(dgv_TaxParcelsSecuritiesOnHand);
                    break;
                case "tp_ShowAccruals":
                    myParams = (ReportParameters)dgv_ShowAccruals.Tag;

                    printer.PrintColumnHeaders = true;
                    printer.Title = myParams.Title;
                    printer.SubTitle = myParams.SubTitle;
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer = myParams.Footer;
                    printer.FooterSpacing = 15;
                    printer.PageSettings.Landscape = myParams.Landscape;
                    printer.PrintPreviewDataGridView(dgv_ShowAccruals);
                    break;
                case "tp_PayableReceivable":
                    myParams = (ReportParameters)dgv_PayableReceivable.Tag;

                    printer.PrintColumnHeaders = true;
                    printer.Title = myParams.Title;
                    printer.SubTitle = myParams.SubTitle;
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer = myParams.Footer;
                    printer.FooterSpacing = 15;
                    printer.PageSettings.Landscape = myParams.Landscape;
                    printer.PrintPreviewDataGridView(dgv_PayableReceivable);
                    break;
                case "tp_BAS_Australia":
                    myParams = (ReportParameters)dgv_BAS_Australia.Tag;

                    printer.PrintColumnHeaders = true;
                    printer.Title = myParams.Title;
                    printer.SubTitle = myParams.SubTitle;
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer = myParams.Footer;
                    printer.FooterSpacing = 15;
                    printer.PageSettings.Landscape = myParams.Landscape;
                    printer.PrintPreviewDataGridView(dgv_BAS_Australia);
                    break;
            }
        } //bt_Print_Click()

        private void dtp_ToDate_ValueChanged(object sender, EventArgs e)
        {
            cb_BAS.Text = "Quarter to: " + dtp_ToDate.Value.ToString("dd-MMM-yyyy");
        }

        private void bt_SaveAs_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedTab.Name)
            {
                case "tp_ProfitLoss":
                    {
                        crv_ProfitLoss.ExportReport();
                        //cr_ProfitLoss objRpt = (cr_ProfitLoss)crv_ProfitLoss.ReportSource;
                        //objRpt.Export();
                    }
                    break;
                case "tp_BalanceSheet":
                    {
                        crv_BalanceSheet.ExportReport();
                        //cr_BalanceSheet objRpt = (cr_BalanceSheet)crv_BalanceSheet.ReportSource;
                        //objRpt.Export();
                    }
                    break;
                case "tp_TrialBalance":
                    {
                        crv_TrialBalance.ExportReport();
                        //cr_TrialBalance objRpt = (cr_TrialBalance)crv_TrialBalance.ReportSource;
                        //objRpt.Export();
                    }
                    break;
            }
        } //bt_SaveAs_Click()

        public void HideTheTabControl(CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1)
        {
            System.Diagnostics.Debug.Assert(
                crystalReportViewer1.ReportSource != null,
                "you have to set the ReportSource first");

            foreach (Control c1 in crystalReportViewer1.Controls)
            {
                if (c1 is CrystalDecisions.Windows.Forms.PageView)
                {
                    CrystalDecisions.Windows.Forms.PageView pv = (CrystalDecisions.Windows.Forms.PageView)c1;
                    foreach (Control c2 in pv.Controls)
                    {
                        if (c2 is TabControl)
                        {
                            TabControl tc = (TabControl)c2;
                            tc.ItemSize = new Size(0, 1);
                            tc.SizeMode = TabSizeMode.Fixed;
                        }
                    }
                }
            }
        }
    }
}
