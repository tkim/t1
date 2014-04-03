using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace T1MultiAsset
{
    public partial class MLPrime_Reconcile : Form
    {
        // Public Variables
        public Form1 ParentForm1;
        public DataTable dt_ML_E238T;
        public DataTable dt_Trade;
        public object LastValue;
        Boolean NeedFullUpdate = false;

        public struct OrderStruct
        {
            public String OrderRefID;
            public String BBG_Ticker;
            public String Side;
            public String crncy;
            public String Country;
            public DateTime TradeDate;
            public Int32 Quantity;
            public Int32 Round_Lot_Size;
            public Boolean ProcessedEOD;
            public Boolean ChangeMade;
            public Boolean ChangeMadeFill;
        }

        public static OrderStruct Order = new OrderStruct();

        public MLPrime_Reconcile()
        {
            InitializeComponent();
        }

        private void MLPrime_Reconcile_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            // Swap back & forth from Hourglass so user knows
            Cursor.Current = Cursors.WaitCursor;
            LoadTrades();
            Cursor.Current = Cursors.Default;

        } //MLPrime_Reconcile_Load()


        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;
            //ParentForm1.CopyDataTable(ref dt_Fund, ref dt_Portfolio);

        } //FromParent()

        private void ProcessOrders_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Let the Parent know this has close
            if (ParentForm1 != null)
                ParentForm1.LoadActionTab(true);

        } //ProcessOrders_FormClosed()

        private void LoadTrades()
        {
            // Local Variables
            String mySql;

            // Get the Data
            // 23 Oct 2013 replaced   "       ML_E239.Price, Round(ML_E239.Quantity * ML_E239.Price * Securities.Pos_Mult_Factor,2) NetValue, " +
            // - ML Prime records
            mySql = "Select	ML_E238T.GPB_Transaction_ID, ML_E238T.Client_Transaction_ID, ML_E238T.Trade_Date as TradeDate, ML_E238T.FundID, Fund.FundName, ML_E238T.BBG_Ticker, " +
                    "       IsNull(ML_E238T.Actual_Settlement_Date,ML_E238T.Contractual_Settlement_Date) SettlementDate, ML_E238T.Quantity, " +
                    "       ML_E238T.Price, (-ML_E238T.Amount) NetValue, ((ML_E238T.Commision_Amount)) Commission, " +
                    "       dbo.f_MLTranType(ML_E238T.Transaction_Type_Code) as Side, ML_E238T.Reason, 'ML_E238T' as Source, ML_E238T.Product_Short_Name, Fund.ParentFundID, 'Merrill Lynch' as CustodianName,  ML_E238T.Executing_Broker_Code as Broker " +
                    "From	ML_E238T, " +
                    "		Fund  " +
                    "Where	isNull(ML_E238T.reconcilled,'N') = 'N' " +
                    "And		ML_E238T.ISIN <> 'CASH' " +
                    "And		ML_E238T.FundID in (Select FundID from Fund Where Active = 'Y') " +
                    "And		Fund.FundID = ML_E238T.FundID " +
                    "Union " +
                    "Select	ML_E239.GPB_Transaction_ID, ML_E239.Client_Transaction_ID, ML_E239.Trade_Date as TradeDate, ML_E239.FundID, Fund.FundName, ML_E239.BBG_Ticker,  " +
                    "       ML_E239.Contractual_Settlement_Date SettlementDate, ML_E239.Quantity, " +
                    "       ML_E239.Price, (-ML_E239.Amount) NetValue, " +
                    "	   Round((-Round(ML_E239.Quantity * ML_E239.Price * Securities.Pos_Mult_Factor,2) - ML_E239.Amount)/(1+ExchangeFees.VATRate),2) Commission, " +
                    "       dbo.f_MLTranType(ML_E239.Transaction_Type) as Side, ML_E239.Reason, 'ML_E239' as Source, ML_E239.Product_Short_Name, Fund.ParentFundID, 'Merrill Lynch' as CustodianName,  ML_E239.Executing_Broker_Code as Broker " +
                    "From	ML_E239, " +
                    "		Fund, " +
                    "		Securities, " +
                    "		ExchangeFees " +
                    "Where	isNull(ML_E239.reconcilled,'N') = 'N' " +
                    "And	ML_E239.ISIN <> 'CASH' " +
                    "And	ML_E239.FundID in (Select FundID from Fund Where Active = 'Y') " +
                    "And	Fund.FundID = ML_E239.FundID " +
                    "And	Securities.BBG_Ticker = ML_E239.BBG_Ticker " +
                    "And	Securities.ID_BB_UNIQUE = ML_E239.ID_BB_UNIQUE " +
                    "And	ML_E239.Transaction_Type in ('Buy','Sell','Sell Short','Cover Short') " +
                    "And	ExchangeFees.Exchange = Securities.BBG_Exchange " +
                    "And Not Exists (Select 'x' from ML_E238T " +
                    "                Where  ML_E238T.GPB_Transaction_ID = ML_E239.GPB_Transaction_ID " +
                    "                And    isNull(ML_E238T.reconcilled,'N') = 'N' " +
                    "               ) " +
                    "Union " +
                    "Select	SCOTIA_AcctHist.ID1, SCOTIA_AcctHist.ID, SCOTIA_AcctHist.Trad_Date as TradeDate, SCOTIA_AcctHist.FundID, Fund.FundName, SCOTIA_AcctHist.BBG_Ticker, " +
                    "       SCOTIA_AcctHist.Val_Date as SettlementDate, Case When SCOTIA_AcctHist.Amt < 0 Then 1 Else -1 End * SCOTIA_AcctHist.Quantity as Quantity, " +
                    "       SCOTIA_AcctHist.Price_gross as Price, (-SCOTIA_AcctHist.Amt) NetValue, SCOTIA_AcctHist.Commission, " +
                    "       Case When SCOTIA_AcctHist.Amt < 0 Then 'B' Else 'S' End as Side, SCOTIA_AcctHist.Reason, 'SCOTIA_AcctHist' as Source, SCOTIA_AcctHist.Security as Product_Short_Name, Fund.ParentFundID, 'SCOTIA' as CustodianName,  SCOTIA_AcctHist.Cpty as Broker " +
                    "From	SCOTIA_AcctHist, " +
                    "		Fund  " +
                    "Where	isNull(SCOTIA_AcctHist.reconcilled,'N') = 'N' " +
                    "And	SCOTIA_AcctHist.Trad_Type <> 'Cash Event' " +
                    "And	SCOTIA_AcctHist.FundID in (Select FundID from Fund Where Active = 'Y') " +
                    "And	Fund.FundID = SCOTIA_AcctHist.FundID " +
                    "Order by 3, FundID, BBG_Ticker, Quantity ";
            dt_ML_E238T = SystemLibrary.SQLSelectToDataTable(mySql);

            dg_ML_E238T.Rows.Clear();
            foreach (DataRow dr in dt_ML_E238T.Rows)
            {
                int myRow = dg_ML_E238T.Rows.Add();
                dg_ML_E238T["CustodianName", myRow].Value = dr["CustodianName"];
                dg_ML_E238T["GPB_Transaction_ID", myRow].Value = dr["GPB_Transaction_ID"];
                dg_ML_E238T["Client_Transaction_ID", myRow].Value = dr["Client_Transaction_ID"];
                dg_ML_E238T["TradeDate", myRow].Value = dr["TradeDate"];
                dg_ML_E238T["FundID", myRow].Value = dr["FundID"];
                dg_ML_E238T["FundName", myRow].Value = dr["FundName"];
                dg_ML_E238T["BBG_Ticker", myRow].Value = dr["BBG_Ticker"];
                dg_ML_E238T["SettlementDate", myRow].Value = dr["SettlementDate"];
                dg_ML_E238T["Quantity", myRow].Value = dr["Quantity"];
                dg_ML_E238T["Price", myRow].Value = dr["Price"];
                dg_ML_E238T["NetValue", myRow].Value = dr["NetValue"];
                dg_ML_E238T["Commission", myRow].Value = dr["Commission"];
                dg_ML_E238T["Side", myRow].Value = dr["Side"];
                dg_ML_E238T["Reason", myRow].Value = dr["Reason"];
                dg_ML_E238T["Source", myRow].Value = dr["Source"];
                dg_ML_E238T["Product_Short_Name", myRow].Value = dr["Product_Short_Name"];
                dg_ML_E238T["ParentFundID", myRow].Value = dr["ParentFundID"];
                ParentForm1.SetColumn(dg_ML_E238T, "Quantity", myRow);
                ParentForm1.SetColumn(dg_ML_E238T, "NetValue", myRow);
                ParentForm1.SetColumn(dg_ML_E238T, "Commission", myRow);
            }
            SetSideColour(dg_ML_E238T, "Side");

            // - Trade records
            mySql = "Select	Trade.TradeID, Trade.TradeDate, Trade.FundID, Fund.FundName, Trade.BBG_Ticker, " +
                    "       Trade.SettlementDate, Trade.Quantity, Trade.Price, Trade.NetValue, Trade.Commission, Trade.Side, Fund.ParentFundID, Custodian.CustodianName, Broker.ExtBrokerID as Broker " +
                    "From	Trade, " +
                    "		Fund,  " +
                    "       Custodian, " +
                    "       Broker " +
                    "Where	Trade.custodianconfirmed is null " +
                    "And		Trade.FundID in (Select FundID from Fund Where Active = 'Y') " +
                    "And		Fund.FundID = Trade.FundID " +
                    "And		Custodian.CustodianID = Trade.CustodianID " +
                    "And    Broker.BrokerID = Trade.BrokerID " +
                    "And	exists (select 'x' " +
                    "			From	Securities " +
                    "			Where	Securities.BBG_Ticker = Trade.BBG_Ticker " +
                    "			And		isNull(Securities.ID_BB_UNIQUE,'') = isNull(Trade.ID_BB_UNIQUE,'') " +
                    "			And		isNull(Securities.Security_Typ2,'') <> 'Future' " +
                    "			) " +
                    "UNION " +
                    "Select	Trade.TradeID, Trade.TradeDate, Trade.FundID, Fund.FundName, Trade.BBG_Ticker, " +
                    "       Trade.SettlementDate, Trade.Quantity, Trade.Price, Trade.NetValue, Trade.Commission, Trade.Side, Fund.ParentFundID, Custodian.CustodianName, Broker.ExtBrokerID as Broker " +
                    "From	Trade, " +
                    "		Fund,  " +
                    "       Custodian, " +
                    "       Broker " +
                    "Where	Trade.FundID in (Select FundID from Fund Where Active = 'Y') " +
                    "And	Fund.FundID = Trade.FundID " +
                    "And	Custodian.CustodianID = Trade.CustodianID " +
                    "And    Broker.BrokerID = Trade.BrokerID " +
                    "And	exists (select 'x' " +
                    "			From	Securities " +
                    "			Where	Securities.BBG_Ticker = Trade.BBG_Ticker " +
                    "			And		isNull(Securities.ID_BB_UNIQUE,'') = isNull(Trade.ID_BB_UNIQUE,'') " +
                    "			And		isNull(Securities.Security_Typ2,'') <> 'Future' " +
                    "			) " +
                    "And (Exists (	Select 'x' " +
			        "               From	ml_E238T " +
			        "               Where	ml_E238T.Client_Transaction_ID = Cast(Trade.TradeID	as Varchar) " +
			        "               And		isNull(ml_E238T.reconcilled,'N') = 'N' " +
		            "           ) " +
                    "   OR " +
                    "     Exists (	Select 'x' " +
                    "               From	Scotia_AcctHist " +
                    "               Where	Scotia_AcctHist.ID = Cast(Trade.TradeID	as Varchar) " +
                    "               And		isNull(Scotia_AcctHist.reconcilled,'N') = 'N' " +
                    "           ) " +
                    "    ) " +
                    "Order by Trade.TradeDate, Trade.FundID, Trade.BBG_Ticker, Trade.Quantity ";
            dt_Trade = SystemLibrary.SQLSelectToDataTable(mySql);

            dg_Trade.Rows.Clear();
            foreach (DataRow dr in dt_Trade.Rows)
            {
                int myRow = dg_Trade.Rows.Add();
                dg_Trade["t_CustodianName", myRow].Value = dr["CustodianName"];
                dg_Trade["t_TradeID", myRow].Value = dr["TradeID"];
                dg_Trade["t_TradeDate", myRow].Value = dr["TradeDate"];
                dg_Trade["t_FundID", myRow].Value = dr["FundID"];
                dg_Trade["t_FundName", myRow].Value = dr["FundName"];
                dg_Trade["t_BBG_Ticker", myRow].Value = dr["BBG_Ticker"];
                dg_Trade["t_SettlementDate", myRow].Value = dr["SettlementDate"];
                dg_Trade["t_Quantity", myRow].Value = dr["Quantity"];
                dg_Trade["t_Price", myRow].Value = dr["Price"];
                dg_Trade["t_NetValue", myRow].Value = dr["NetValue"];
                dg_Trade["t_Commission", myRow].Value = dr["Commission"];
                dg_Trade["t_Side", myRow].Value = dr["Side"];
                dg_Trade["t_ParentFundID", myRow].Value = dr["ParentFundID"];
                ParentForm1.SetColumn(dg_Trade, "t_Quantity", myRow);
                ParentForm1.SetColumn(dg_Trade, "t_NetValue", myRow);
                ParentForm1.SetColumn(dg_Trade, "t_Commission", myRow);
            }
            SetSideColour(dg_Trade, "t_Side");

        } //LoadTrades()

        private void dg_Trade_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DataGridView.HitTestInfo info = dg_Trade.HitTest(e.X, e.Y);
                if (info.RowIndex >= 0)
                {
                    DataGridViewRow view = (DataGridViewRow)dg_Trade.Rows[info.RowIndex];
                    if (view != null)
                        dg_Trade.DoDragDrop(view, DragDropEffects.Copy);
                }
            }

        } //dg_Trade_MouseDown()

        private void dg_ML_E238T_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo info = dg_ML_E238T.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            {
                if (info.RowIndex >= 0)
                {
                    DataGridViewRow view = (DataGridViewRow)dg_ML_E238T.Rows[info.RowIndex];
                    if (view != null)
                        dg_ML_E238T.DoDragDrop(view, DragDropEffects.Copy);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (info.RowIndex >= 0)
                {
                    String GPB_Transaction_ID = dg_ML_E238T["GPB_Transaction_ID", info.RowIndex].Value.ToString();
                    String FundName = dg_ML_E238T["FundName", info.RowIndex].Value.ToString();
                    String BBG_Ticker = dg_ML_E238T["BBG_Ticker", info.RowIndex].Value.ToString();
                    if (MessageBox.Show(this, "Do you want to create a NEW Trade for Ticker = '" + BBG_Ticker + "' in Fund = '" + FundName + "'.\r\n\r\n" +
                                        "Only do this if there is NO matching trade or Order.",
                                        "WARNING: About to Create a NEW Trade", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (FundName.Trim().Length == 0)
                        {
                            MessageBox.Show("Sorry no Fund Attached to this Trade", "Create a New Trade");
                        }
                        else
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            // TODO (5) Do I need this right-click
                            //SystemLibrary.SQLExecute("exec sp_Trade_From_ML_E238T '" + GPB_Transaction_ID + "', NEED PORTFOLIOID HERE ");
                            LoadTrades();
                            Cursor.Current = Cursors.Default;
                        }
                    }
                }
            }

        } //dg_ML_E238T_MouseDown()

        private void dg_ML_E238T_DragEnter(object sender, DragEventArgs e)
        {
            DataGridViewRow Source = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));
            DataGridView Target = (DataGridView)sender;
            if (Source.DataGridView.Name != Target.Name)
                e.Effect = DragDropEffects.Copy;
        } //dg_ML_E238T_DragEnter()

        private void dg_ML_E238T_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = dg_ML_E238T.PointToClient(new Point(e.X, e.Y));
            int myTargetRow = dg_ML_E238T.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            if (myTargetRow >= 0)
            {
                // Get the ExtId from the source.
                DataGridViewRow Source = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));
                String t_TradeID = SystemLibrary.ToString(Source.Cells["t_TradeID"].Value);

                // See What the Target table has
                DataGridViewRow Target = (DataGridViewRow)dg_ML_E238T.Rows[myTargetRow];
                String CustodianName = SystemLibrary.ToString(Target.Cells["CustodianName"].Value);
                String SourceCustodianName = SystemLibrary.ToString(Source.Cells["t_CustodianName"].Value);
                String TradeID = SystemLibrary.ToString(Target.Cells["TradeID"].Value);
                String FundName = SystemLibrary.ToString(Target.Cells["FundName"].Value) + " - Parent FundID =" +
                                  SystemLibrary.ToString(Target.Cells["ParentFundID"].Value);
                String SourceFundName = SystemLibrary.ToString(Source.Cells["t_FundName"].Value) + " - Parent FundID =" +
                                        SystemLibrary.ToString(Source.Cells["t_ParentFundID"].Value);
                String FundID = SystemLibrary.ToString(Target.Cells["ParentFundID"].Value);
                String BBG_Ticker = SystemLibrary.ToString(Target.Cells["BBG_Ticker"].Value);
                String SourceFundID = SystemLibrary.ToString(Source.Cells["t_ParentFundID"].Value);
                String SourceBBG_Ticker = SystemLibrary.ToString(Source.Cells["t_BBG_Ticker"].Value);
                String SourceSettlementDate = SystemLibrary.ToString(Source.Cells["t_SettlementDate"].Value);
                String SettlementDate = SystemLibrary.ToString(Target.Cells["SettlementDate"].Value);
                String SourceSide = SystemLibrary.ToString(Source.Cells["t_Side"].Value);
                String Side = SystemLibrary.ToString(Target.Cells["Side"].Value);

                if (SourceSettlementDate != SettlementDate)
                {
                    MessageBox.Show("Settlement Dates do not match.\r\n\r\nWill be Applying the Prime Brokers Settlement Date to Trade.");
                }

                if ((SourceSide != Side && CustodianName != "SCOTIA") || (SourceSide.Substring(0, 1) != Side.Substring(0, 1) && CustodianName == "SCOTIA"))
                {
                    MessageBox.Show("Side's do not match.\r\n\r\nWill keep existing Side on Trade.");
                }

                if (CustodianName != SourceCustodianName)
                {
                    MessageBox.Show(this, "Failed: You have an mis-matched a Custodian on this Trade.\r\n\r\n" +
                                          "Matching\r\n\t'" + CustodianName + "'\r\nwith\r\n\t'" + SourceCustodianName + "'.\r\n\r\n" +
                                          "Match Failed");
                    return;
                }

                if (FundID != SourceFundID)
                {
                    if (MessageBox.Show(this, "WARNING: You have an mis-matched a Fund on this Trade.\r\n\r\n" +
                                        "Matching\r\n\t'" + FundName + "'\r\nwith\r\n\t'" + SourceFundName + "'.\r\n\r\n" +
                                        "Do you really wish to do this'?",
                                        "Change Custodian ID for Trade", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                if (BBG_Ticker != SourceBBG_Ticker)
                {
                    if (MessageBox.Show(this, "WARNING: You have an mis-matched a Ticker on this Trade.\r\n\r\n" +
                                        "Matching\r\n\t'" + BBG_Ticker + "'\r\nwith\r\n\t'" + SourceBBG_Ticker + "'.\r\n\r\n" +
                                        "Do you really wish to do this'?",
                                        "Change Custodian ID for Trade", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                // See if there is already a value in the Target
                if (TradeID != "")
                {
                    if (MessageBox.Show(this, "There is already a Value of '" + TradeID + "' on this Trade (" + FundName + ").\r\n\r\n" +
                                        "Do you really wish to replace this with '" + t_TradeID + "'?",
                                        "Change Custodian ID for Trade", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                Target.Cells["TradeID"].Value = t_TradeID;
                String MatchValue = SystemLibrary.ToString(Source.Cells["t_GPB_Transaction_ID"].Value);
                if (MatchValue.Length>0)
                    MatchValue = MatchValue + ",";
                Source.Cells["t_GPB_Transaction_ID"].Value = MatchValue + Target.Cells["GPB_Transaction_ID"].Value.ToString();
            }
        } //dg_ML_E238T_DragDrop()

        private void dg_Trade_DragEnter(object sender, DragEventArgs e)
        {
            DataGridViewRow Source = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));
            DataGridView Target = (DataGridView)sender;
            if (Source.DataGridView.Name!=Target.Name)
                e.Effect = DragDropEffects.Copy;
        } //dg_Trade_DragEnter()

        private void dg_Trade_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = dg_Trade.PointToClient(new Point(e.X, e.Y));
            int myTargetRow = dg_Trade.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            if (myTargetRow >= 0)
            {
                // Get the ExtId from the source.
                DataGridViewRow Source = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));
                String GPB_Transaction_ID = SystemLibrary.ToString(Source.Cells["GPB_Transaction_ID"].Value);

                // See What the Target table has
                DataGridViewRow Target = (DataGridViewRow)dg_Trade.Rows[myTargetRow];
                String t_GPB_Transaction_ID = SystemLibrary.ToString(Target.Cells["t_GPB_Transaction_ID"].Value);
                String CustodianName = SystemLibrary.ToString(Target.Cells["t_CustodianName"].Value);
                String SourceCustodianName = SystemLibrary.ToString(Source.Cells["CustodianName"].Value);
                String FundName = SystemLibrary.ToString(Target.Cells["t_FundName"].Value) + " - Parent FundID =" +
                                  SystemLibrary.ToString(Target.Cells["t_ParentFundID"].Value);
                String SourceFundName = SystemLibrary.ToString(Source.Cells["FundName"].Value) + " - Parent FundID =" +
                                        SystemLibrary.ToString(Source.Cells["ParentFundID"].Value);

                String FundID = SystemLibrary.ToString(Target.Cells["t_ParentFundID"].Value);
                String BBG_Ticker = SystemLibrary.ToString(Target.Cells["t_BBG_Ticker"].Value);
                String SourceFundID = SystemLibrary.ToString(Source.Cells["ParentFundID"].Value);
                String SourceBBG_Ticker = SystemLibrary.ToString(Source.Cells["BBG_Ticker"].Value);

                String SourceSettlementDate = SystemLibrary.ToString(Source.Cells["SettlementDate"].Value);
                String SettlementDate = SystemLibrary.ToString(Target.Cells["t_SettlementDate"].Value);
                String SourceSide = SystemLibrary.ToString(Source.Cells["Side"].Value);
                String Side = SystemLibrary.ToString(Target.Cells["t_Side"].Value);

                if (SourceSettlementDate != SettlementDate)
                {
                    MessageBox.Show("Settlement Dates do not match.\r\n\r\nWill be Applying the Prime Brokers Settlement Date to Trade.");
                }

                if ((SourceSide != Side && CustodianName != "SCOTIA") || (SourceSide.Substring(0, 1) != Side.Substring(0, 1) && CustodianName == "SCOTIA"))
                {
                    MessageBox.Show("Side's do not match.\r\n\r\nWill keep existing Side on Trade.");
                }

                if (CustodianName != SourceCustodianName)
                {
                    MessageBox.Show(this, "Failed: You have an mis-matched a Custodian on this Trade.\r\n\r\n" +
                                          "Matching\r\n\t'" + CustodianName + "'\r\nwith\r\n\t'" + SourceCustodianName + "'.\r\n\r\n" +
                                          "Match Failed");
                    return;
                }

                if (FundID != SourceFundID)
                {
                    if (MessageBox.Show(this, "WARNING: You have an mis-matched a Fund on this Trade.\r\n\r\n" +
                                        "Matching\r\n\t'" + FundName + "'\r\nwith\r\n\t'" + SourceFundName + "'.\r\n\r\n" +
                                        "Do you really wish to do this'?",
                                        "Change Custodian ID for Trade", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                if (BBG_Ticker != SourceBBG_Ticker)
                {
                    if (MessageBox.Show(this, "WARNING: You have an mis-matched a Ticker on this Trade.\r\n\r\n" +
                                        "Matching\r\n\t'" + BBG_Ticker + "'\r\nwith\r\n\t'" + SourceBBG_Ticker + "'.\r\n\r\n" +
                                        "Do you really wish to do this'?",
                                        "Change Custodian ID for Trade", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                // See if there is already a value in the Target
                if (t_GPB_Transaction_ID != "" && t_GPB_Transaction_ID != GPB_Transaction_ID)
                {
                    if (MessageBox.Show(this, "There is already a Value of '" + t_GPB_Transaction_ID + "' on this Trade (" + FundName + ").\r\n\r\n" +
                                        "Do you really wish to replace this with '" + GPB_Transaction_ID + "'?",
                                        "Change E238T ID for Trade", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                else if (t_GPB_Transaction_ID != GPB_Transaction_ID)
                {
                    Target.Cells["t_GPB_Transaction_ID"].Value = GPB_Transaction_ID;
                    String MatchValue = SystemLibrary.ToString(Source.Cells["TradeID"].Value);
                    if (MatchValue.Length > 0)
                        MatchValue = MatchValue + ",";
                    Source.Cells["TradeID"].Value = MatchValue + Target.Cells["t_TradeID"].Value.ToString();
                }
            }

        } //dg_Trade_DragDrop()

        private void bt_Mark_As_Confirmed_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "This function ignores the differences and marks both the Prime Trades and the system Trades as reconcilled.\r\n\r\n" +
                                "This is NOT an advised process, but is sometimes needed.\r\n\r\n" +
                                "Do you really wish to do this'?",
                                "WARNING: Marking as Confirmed with NO RECONCILLIATION", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Loop around each of the gridviews and update the database
                for (int i = 0; i < dg_ML_E238T.Rows.Count; i++)
                {
                    String mySql = "";
                    switch (dg_ML_E238T["CustodianName", i].Value.ToString())
                    {
                        case "Merrill Lynch":
                            mySql = "Update " + dg_ML_E238T["Source", i].Value.ToString() + " " +
                                    "Set Reconcilled = 'Y', " +
                                    "    Reason = 'Override: '+Reason " +
                                    "Where   GPB_Transaction_ID = " + dg_ML_E238T["GPB_Transaction_ID", i].Value.ToString();
                            break;
                        case "SCOTIA":
                            mySql = "Update Scotia_accthist " +
                                    "Set Reconcilled = 'Y', " +
                                    "    Reason = 'Override: '+Reason " +
                                    "Where   ID1 = " + dg_ML_E238T["GPB_Transaction_ID", i].Value.ToString();
                            break;
                    }
                    SystemLibrary.SQLExecute(mySql);
                }
                // Loop around each of the gridviews and update the database
                for (int i = 0; i < dg_Trade.Rows.Count; i++)
                {
                    String mySql = "Update Trade " +
                                   "Set custodianconfirmed = dbo.f_Today() " +
                                   "Where   TradeID = " + dg_Trade["t_TradeID", i].Value.ToString();
                    SystemLibrary.SQLExecute(mySql);
                }
                // Refresh
                LoadTrades();
                // Let the Parent know this has close
                if (ParentForm1 != null)
                    ParentForm1.LoadActionTab(true);

                NeedFullUpdate = true;

                MessageBox.Show("Update complete", bt_Mark_As_Confirmed.Text);
            }

        } //bt_Mark_As_Confirmed_Click()

        private void bt_Save_Click(object sender, EventArgs e)
        {
            DateTime recalcDate = DateTime.MaxValue;
            Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < dg_ML_E238T.Rows.Count; i++)
            {
                // See if TradeID is set
                Boolean setMatch = false;
                String mySql = "";
                String CustodianName = SystemLibrary.ToString(dg_ML_E238T["CustodianName", i].Value);
                String mySource = SystemLibrary.ToString(dg_ML_E238T["Source", i].Value);
                String myTrades = SystemLibrary.ToString(dg_ML_E238T["TradeID", i].Value);
                String GPB_Transaction_ID = SystemLibrary.ToString(dg_ML_E238T["GPB_Transaction_ID", i].Value);
                String ML_SettlementDate = "";
                if (dg_ML_E238T["SettlementDate", i].Value != null)
                {
                    DateTime myTmpDate = Convert.ToDateTime(dg_ML_E238T["SettlementDate", i].Value);
                    ML_SettlementDate = myTmpDate.ToString("dd-MMM-yyyy");
                }

                if (myTrades.Length>0)
                {
                    // Can be 123/456/789
                    String[] myTradeID = myTrades.Split(',');
                    String myFilter = "TradeID=" + myTradeID[0];
                    for (int t = 1; t < myTradeID.Length; t++)
                    {
                        myFilter = myFilter + " or TradeID=" + myTradeID[t];
                    }
                    // See if NetValue matches between these records.
                    Double t_Quantity = SystemLibrary.ToDouble(dt_Trade.Compute("sum(Quantity)", myFilter));
                    Double Quantity = SystemLibrary.ToDouble(dg_ML_E238T["Quantity", i].Value);
                    Double t_NetValue = SystemLibrary.ToDouble(dt_Trade.Compute("sum(NetValue)", myFilter));
                    Double NetValue = SystemLibrary.ToDouble(dg_ML_E238T["NetValue", i].Value);
                    Double t_Commission = SystemLibrary.ToDouble(dt_Trade.Compute("sum(Commission)", myFilter));
                    Double Commission = SystemLibrary.ToDouble(dg_ML_E238T["Commission", i].Value);
                    Double net_diff = NetValue - t_NetValue;
                    Double comm_diff = Commission - t_Commission;

                    if (t_Quantity != Quantity)
                    {
                        MessageBox.Show(this, "Sorry Quantities do not match, so aborting transaction=" + GPB_Transaction_ID + "\r\n" +
                                            "\t" + t_Quantity.ToString("#,###") + "\tversus\t" + Quantity.ToString("#,###") +"\r\n\r\n" +
                                            "Any trades already processed will have been saved.",
                                            "Save: Aborted");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (net_diff != 0)
                    {
                        Cursor.Current = Cursors.Default;
                        DialogResult dr = MessageBox.Show(this, "There is a difference in Net Value between these records. [Transaction=" + GPB_Transaction_ID + "]\r\n" +
                                            "\tThe Prime Broker record = " + NetValue.ToString("$#,###.##")+"\r\n"+
                                            "\tThe internal record =     " + t_NetValue.ToString("$#,###.##") + "\r\n\r\n" +
                                            "If small, it is recommended you make the adjustment.\r\n\r\n"+
                                            "Do you want to ADJUST the Trade to match the PRIME broker record?",
                                            "Reconcilliation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (dr==DialogResult.Yes)
                        {
                            setMatch = true;
                        }
                        else if (dr==DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                    else
                        setMatch = true;

                    Cursor.Current = Cursors.WaitCursor;

                    switch (CustodianName)
                    {
                        case "Merrill Lynch":
                            mySql = "Update " + mySource + " " +
                                    "Set Reconcilled = 'Y', ";
                            if (myTradeID.Length == 1)
                                mySql = mySql + "TradeID = " + myTradeID[0] + ", ";
                            mySql = mySql +
                                    "    Reason = 'Matched to: " + myTrades + "' " +
                                    "Where   GPB_Transaction_ID = " + dg_ML_E238T["GPB_Transaction_ID", i].Value.ToString();
                            SystemLibrary.SQLExecute(mySql);
                            break;
                        case "SCOTIA":
                            mySql = "Update Scotia_accthist  " +
                                    "Set Reconcilled = 'Y', ";
                            if (myTradeID.Length == 1)
                                mySql = mySql + "TradeID = " + myTradeID[0] + ", ";
                            mySql = mySql +
                                    "    Reason = 'Matched to: " + myTrades + "' " +
                                    "Where   ID1 = " + dg_ML_E238T["GPB_Transaction_ID", i].Value.ToString();
                            SystemLibrary.SQLExecute(mySql);
                            break;
                    }


                    for (int t = 0; t < myTradeID.Length; t++)
                    {
                        // Deal on Settlement Date. Apply the ML Settlement Date to our records.
                        if (ML_SettlementDate != "")
                        {
                            mySql = "Update Trade " +
                                    "Set SettlementDate = '" + ML_SettlementDate + "' " +
                                    "Where 	TradeID = " + myTradeID[t] + " " +
                                    "And    SettlementDate <> '" + ML_SettlementDate + "' ";
                            SystemLibrary.SQLExecute(mySql);
                            mySql = "Update Transactions " +
                                    "Set EffectiveDate = '" + ML_SettlementDate + "' " +
                                    "Where 	TradeID = " + myTradeID[t] + " " +
                                    "And    EffectiveDate <> '" + ML_SettlementDate + "' ";
                            SystemLibrary.SQLExecute(mySql);
                        }

                        if (setMatch)
                        {
                            // Make the small adjustment on the first record, then setMatch = false
                            // - Need to do an SQL on the trade record, then need to recalc P&l from this point
                            mySql = "Update Trade " +
				                    "Set	NetValue = NetValue + " + net_diff.ToString() + ", " +
				                    "		Commission = Commission + " + comm_diff.ToString() + ", " +
				                    "		GrossValue = GrossValue + " + (net_diff + comm_diff).ToString() + ", " +
                                    "       custodianconfirmed = dbo.f_Today() " +
                                    "Where 	TradeID = " + myTradeID[t];
                            SystemLibrary.SQLExecute(mySql);

                            // Fix the Transaction Record
                            mySql = "exec sp_ApplyTradeChangeToTransaction " + myTradeID[t];
                            SystemLibrary.SQLExecute(mySql);
                            setMatch = false;

                            DateTime myDate = Convert.ToDateTime(dg_ML_E238T["TradeDate", i].Value);
                            if (myDate < recalcDate)
                                recalcDate = myDate;

                        }
                        else
                        {
                            mySql = "Update Trade " +
                                    "Set custodianconfirmed = dbo.f_Today() " +
                                    "Where   TradeID = " + myTradeID[t];
                            SystemLibrary.SQLExecute(mySql);

                        }
                        // Mark Transactions as reconcilled
                        mySql = "Update Transactions " +
                                "Set Reconcilled = 'Y' " +
                                "Where   TradeID = " + myTradeID[t];
                        SystemLibrary.SQLExecute(mySql);
                    }
                    // Clear the record on the 2 datagrids

                }
            }

            // No recalc historic P&L if needed
            if (recalcDate != DateTime.MaxValue)
            {
                SystemLibrary.SQLExecute("exec sp_Calc_Profit_RebuildFrom '" + recalcDate.ToString("dd-MMM-yyyy") + "' ");
                SystemLibrary.SQLExecute("Exec sp_SOD_Positions");
            }
            // Refresh
            LoadTrades();
            // Let the Parent know this has close
            if (ParentForm1 != null)
                ParentForm1.LoadActionTab(true);

            Cursor.Current = Cursors.Default;
            NeedFullUpdate = true;
            MessageBox.Show("Update complete", bt_Save.Text);


        } //bt_Save_Click()

        public void SetSideColour(DataGridView dv, String myColumn)
        {
            for (int i = 0; i < dv.Rows.Count; i++)
            {
                String mySide = SystemLibrary.ToString(dv[myColumn,i].Value);
                if (mySide.StartsWith("S"))
                    dv[myColumn, i].Style.ForeColor = Color.Red;
                else
                    dv[myColumn, i].Style.ForeColor = Color.Green;
            }

        } //SetSideColour()

        private void bt_Calculator_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc");
        } //bt_Calculator_Click()

        private void bt_BackEnd_Reprocess_Click(object sender, EventArgs e)
        {
            // Force through the processing scripts
            Cursor.Current = Cursors.WaitCursor;
            int myRows = SystemLibrary.SQLSelectRowsCount("Select CustodianName from Custodian where CustodianName = 'Merrill Lynch'");
            if (myRows > 0)
            {
                SystemLibrary.SQLExecute("Exec sp_ML_Process_File '', 'ML_E236' ");
                SystemLibrary.SQLExecute("Exec sp_ML_Process_File '', 'ML_E237' ");
                SystemLibrary.SQLExecute("Exec sp_ML_Process_File '', 'ML_E238T' ");
                SystemLibrary.SQLExecute("Exec sp_ML_Process_File '', 'ML_E239' ");
            }
            myRows = SystemLibrary.SQLSelectRowsCount("Select CustodianName from Custodian where CustodianName = 'SCOTIA'");
            if (myRows > 0)
            {
                SystemLibrary.SQLExecute("Exec sp_SCOTIA_Process_File '', 'SCOTIA_CASHBALANCE' ");
                SystemLibrary.SQLExecute("Exec sp_SCOTIA_Process_File '', 'SCOTIA_POSITIONS' ");
                SystemLibrary.SQLExecute("Exec sp_SCOTIA_Process_File '', 'SCOTIA_ACCTHIST' ");
            }
            LoadTrades();

            Cursor.Current = Cursors.Default;
            NeedFullUpdate = true;
            MessageBox.Show("Completed.\r\n\r\nThis has forced a reprocess of uploaded files which will have Reconcile records if able.\r\n" +
                            "This is not normally needed but is here for completeness.", ((Button)sender).Text);


        } //bt_BackEnd_Reprocess_Click()

        private void MLPrime_Reconcile_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (NeedFullUpdate)
                SystemLibrary.SQLExecute("Exec sp_actionsNeeded 2, 'N'");

            Cursor.Current = Cursors.Default;

        } //MLPrime_Reconcile_FormClosing()


    }
}
