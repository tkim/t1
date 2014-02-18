using System;
using System.Collections; // ArrayList
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace T1MultiAsset
{
    public partial class MLFuture_Reconcile : Form
    {
        // Public Variables
        public Form1 ParentForm1;
        public DataTable dt_ML_Futures;
        public DataTable dt_Trade;
        public object LastValue;
        Boolean NeedFullUpdate = false;

        public int CXLocation;
        public int CYLocation;

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

        public MLFuture_Reconcile()
        {
            InitializeComponent();
        }

        private void MLFuture_Reconcile_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            // Swap back & forth from Hourglass so user knows
            Cursor.Current = Cursors.WaitCursor;
            LoadTrades();
            Cursor.Current = Cursors.Default;

        } //MLFuture_Reconcile_Load()


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
            // - ML Futures records
            // 20120215: Changed from ABS(MLFut_Trades.Commissions + MLFut_Trades.ExchangeFee)  as Commission
            mySql = "SELECT  MLFut_Trades.TradeID, MLFut_Trades.ContractID, " +
                    "		MLFut_Trades.TRADEDATE, MLFut_Trades.FundID, Fund.ShortName as Fund,  " +
                    "		MLFut_Trades.BBG_Ticker, " +
                    "		Case When MLFut_Trades.BuySellCode = 1 then 1 else -1 end * MLFut_Trades.Quantity as Quantity, " +
                    "		MLFut_Trades.TradePrice as Price, " +
                    "		Round(Case when MLFut_Trades.BUYSELLCODE = 2 Then -1 Else 1 end * MLFut_Trades.Quantity * MLFut_Trades.MultFactor * MLFut_Trades.TRADEPRICE,2) as GrossValue, " +
                    "		-(MLFut_Trades.Commissions + MLFut_Trades.ClearingFee + MLFut_Trades.ExchangeFee + MLFut_Trades.NFAFee + MLFut_Trades.GiveUpCharge + MLFut_Trades.BrokerageCharge + MLFut_Trades.ElecExecCharge + MLFut_Trades.OtherCharges) as Commission, MLFut_Trades.Reason, " +
                    "		Case When MLFut_Trades.BuySellCode = 1 then 'B' else 'S' end as Side, MLFut_Trades.Currency as crncy, " +
                    "		Case When MLFut_Trades.TRANSACTIONTYPE = 'T' Then 'Trade' Else 'Cancel ('+MLFut_Trades.TRANSACTIONTYPE+')' End as TRANSACTIONTYPE, " +
                    "       MLFut_Trades.MLFileName, Fund.FundName, MLFut_Trades.MultFactor as Pos_Mult_Factor " +
                    "FROM    MLFut_Trades LEFT OUTER JOIN " +
                    "                      Fund ON MLFut_Trades.FundId = Fund.FundID " +
                    "WHERE	ISNULL(MLFut_Trades.Reconcilled, 'N') = 'N' " +
                    "AND		MLFut_Trades.FundId in (Select FundID from Fund Where Active = 'Y') " +
                    "ORDER BY MLFut_Trades.TRADEDATE, Fund.ShortName, MLFut_Trades.BuySellCode, MLFut_Trades.TradePrice, MLFut_Trades.ContractID, MLFut_Trades.TransactionType "; 
            dt_ML_Futures = SystemLibrary.SQLSelectToDataTable(mySql);
            lb_Future.Text = "Unreconcilled ML Future Trade Records (" + dt_ML_Futures.Rows.Count.ToString() + ")";

            dg_ML_Futures.Rows.Clear();
            foreach (DataRow dr in dt_ML_Futures.Rows)
            {
                int myRow = dg_ML_Futures.Rows.Add();
                dg_ML_Futures["ContractID", myRow].Value = dr["ContractID"];
                dg_ML_Futures["TradeID", myRow].Value = dr["TradeID"];
                dg_ML_Futures["TradeDate", myRow].Value = dr["TradeDate"];
                dg_ML_Futures["FundID", myRow].Value = dr["FundID"];
                dg_ML_Futures["Fund", myRow].Value = dr["Fund"];
                dg_ML_Futures["BBG_Ticker", myRow].Value = dr["BBG_Ticker"];
                dg_ML_Futures["Quantity", myRow].Value = dr["Quantity"];
                dg_ML_Futures["Price", myRow].Value = dr["Price"];
                dg_ML_Futures["GrossValue", myRow].Value = dr["GrossValue"];
                dg_ML_Futures["Commission", myRow].Value = dr["Commission"];
                dg_ML_Futures["Side", myRow].Value = dr["Side"];
                dg_ML_Futures["crncy", myRow].Value = dr["crncy"];
                dg_ML_Futures["Reason", myRow].Value = dr["Reason"];
                dg_ML_Futures["TransactionType", myRow].Value = dr["TransactionType"];
                dg_ML_Futures["MLFileName", myRow].Value = dr["MLFileName"];
                dg_ML_Futures["Pos_Mult_Factor", myRow].Value = dr["Pos_Mult_Factor"];
                dg_ML_Futures["FundName", myRow].Value = dr["FundName"];
                ParentForm1.SetColumn(dg_ML_Futures, "Quantity", myRow);
                ParentForm1.SetColumn(dg_ML_Futures, "GrossValue", myRow);
                ParentForm1.SetColumn(dg_ML_Futures, "Commission", myRow);
            }
            SetSideColour(dg_ML_Futures, "Side");

            // - Trade records
            mySql = "Select	Trade.CustodianConfirmed, Trade.TradeID, Trade.TradeDate, Trade.SettlementDate, Trade.FundID, Fund.ShortName as Fund, " +
                    "		Trade.BBG_Ticker, Trade.Quantity, Trade.Price, Trade.GrossValue, Trade.Commission, " +
                    "		Trade.Side, Trade.crncy, Broker.BrokerName, Trade.SentToBroker, Trade.Pos_Mult_Factor, Fund.FundName, " +
                    "       Fund.ParentFundID, FundParent.FundName ParentFundName " +
                    "From	Trade, " +
                    "		Fund, " +
                    "		Fund FundParent, " +
                    "		Broker " +
                    "Where	Trade.CustodianConfirmed is null " +
                    "And		Trade.FundID in (Select FundID from Fund Where Active = 'Y') " +
                    "And		Fund.FundId = Trade.FundID " +
                    "And		Broker.BrokerID = Trade.BrokerID " +
                    "And        FundParent.FundID = Fund.ParentFundID " +
                    "And	Exists (Select 'x' " +
                    "			From	Securities " +
                    "			Where	Securities.BBG_Ticker = Trade.BBG_Ticker " +
                    "			And		Securities.ID_BB_UNIQUE = isNull(Trade.ID_BB_UNIQUE,Securities.ID_BB_UNIQUE) " +
                    "			And		isNull(Securities.Security_Typ2,'') = 'Future' " +
                    "		    ) " +
                    "Order by Trade.TradeDate, Fund.ShortName, Trade.Side";
            dt_Trade = SystemLibrary.SQLSelectToDataTable(mySql);
            lb_Trade.Text = "Unreconcilled Trades (" + dt_Trade.Rows.Count.ToString() + ")";

            dg_Trade.Rows.Clear();
            foreach (DataRow dr in dt_Trade.Rows)
            {
                int myRow = dg_Trade.Rows.Add();
                dg_Trade["t_CustodianConfirmed", myRow].Value = dr["CustodianConfirmed"];
                dg_Trade["t_TradeID", myRow].Value = dr["TradeID"];
                dg_Trade["t_TradeDate", myRow].Value = dr["TradeDate"];
                dg_Trade["t_SettlementDate", myRow].Value = dr["SettlementDate"];
                dg_Trade["t_FundID", myRow].Value = dr["FundID"];
                dg_Trade["t_Fund", myRow].Value = dr["Fund"];
                dg_Trade["t_BBG_Ticker", myRow].Value = dr["BBG_Ticker"];
                dg_Trade["t_Quantity", myRow].Value = dr["Quantity"];
                dg_Trade["t_Price", myRow].Value = dr["Price"];
                dg_Trade["t_GrossValue", myRow].Value = dr["GrossValue"];
                dg_Trade["t_Commission", myRow].Value = dr["Commission"];
                dg_Trade["t_Side", myRow].Value = dr["Side"];
                dg_Trade["t_crncy", myRow].Value = dr["crncy"];
                dg_Trade["t_BrokerName", myRow].Value = dr["BrokerName"];
                dg_Trade["t_SentToBroker", myRow].Value = dr["SentToBroker"];
                dg_Trade["t_Pos_Mult_Factor", myRow].Value = dr["Pos_Mult_Factor"];
                dg_Trade["t_FundName", myRow].Value = dr["FundName"];
                dg_Trade["t_ParentFundID", myRow].Value = dr["ParentFundID"];
                dg_Trade["t_ParentFundName", myRow].Value = dr["ParentFundName"];
                ParentForm1.SetColumn(dg_Trade, "t_Quantity", myRow);
                ParentForm1.SetColumn(dg_Trade, "t_GrossValue", myRow);
                ParentForm1.SetColumn(dg_Trade, "t_Commission", myRow);
            }
            SetSideColour(dg_Trade, "t_Side");

            // Now Set the DatePicker for the All tab
            dtp_AllFutures.Value = SystemLibrary.f_Today().AddDays(-1);

            if (dt_Trade.Rows.Count > 0)
            {
                DateTime myMinDate = (DateTime)dt_Trade.Compute("min(TradeDate)", "");
                dtp_AllFutures.Value = myMinDate;
            }

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

        private void dg_ML_Futures_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DataGridView.HitTestInfo info = dg_ML_Futures.HitTest(e.X, e.Y);
                if (info.RowIndex >= 0)
                {
                    DataGridViewRow view = (DataGridViewRow)dg_ML_Futures.Rows[info.RowIndex];
                    if (view != null)
                        dg_ML_Futures.DoDragDrop(view, DragDropEffects.Copy);
                }
            }

        } //dg_ML_Futures_MouseDown()

        private void dg_ML_Futures_DragEnter(object sender, DragEventArgs e)
        {
            DataGridViewRow Source = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));
            DataGridView Target = (DataGridView)sender;
            if (Source.DataGridView.Name != Target.Name)
                e.Effect = DragDropEffects.Copy;
        } //dg_ML_Futures_DragEnter()

        private void dg_ML_Futures_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = dg_ML_Futures.PointToClient(new Point(e.X, e.Y));
            int myTargetRow = dg_ML_Futures.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            if (myTargetRow >= 0)
            {
                // Get the ExtId from the source.
                DataGridViewRow Source = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));
                if (Source.DataGridView.Name == dg_ML_Futures.Name)
                    return;


                String t_TradeID = SystemLibrary.ToString(Source.Cells["t_TradeID"].Value);

                // See What the Target table has
                DataGridViewRow Target = (DataGridViewRow)dg_ML_Futures.Rows[myTargetRow];
                String TradeID = SystemLibrary.ToString(Target.Cells["TradeID"].Value);
                String FundName = SystemLibrary.ToString(Target.Cells["FundName"].Value) + " - " +
                                  SystemLibrary.ToString(Target.Cells["BBG_Ticker"].Value);
                String SourceFundName = SystemLibrary.ToString(Source.Cells["t_ParentFundName"].Value) + " - " +
                                        SystemLibrary.ToString(Source.Cells["t_BBG_Ticker"].Value);
                String SourceTradeDate = Convert.ToDateTime(Source.Cells["t_TradeDate"].Value).ToString("dd-MMM-yyyy");
                String TradeDate = Convert.ToDateTime(Target.Cells["TradeDate"].Value).ToString("dd-MMM-yyyy");
                String SourceSide = SystemLibrary.ToString(Source.Cells["t_Side"].Value);
                String Side = SystemLibrary.ToString(Target.Cells["Side"].Value);
                String TransactionType = SystemLibrary.ToString(Target.Cells["TransactionType"].Value);

                if (TransactionType != "Trade")
                {
                    if (TransactionType == "Cancel (C)")
                    {
                        if (MessageBox.Show(this, "WARNING: You are matching this trade to a Cancel.\r\n\r\n" +
                                            "Do you really wish to do this'?",
                                            "Match Futures", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "Can only match to Transaction Type of 'Trade'\r\n\r\n" +
                                              "This is a transaction of type '" + TransactionType + "'", "Not Matched");
                        return;
                    }
                }
                if (SourceTradeDate != TradeDate)
                {
                    if (MessageBox.Show(this, "WARNING: You have a mis-matched on Trade Dates.\r\n\r\n" +
                                        "Matching\r\n\t'" + SourceTradeDate + "'\r\nwith\r\n\t'" + TradeDate + "'.\r\n\r\n" +
                                        "Do you really wish to do this'?",
                                        "Match Futures", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                if (SourceSide != Side)
                {
                    MessageBox.Show("Side's do not match.","Abort");
                    return;
                }

                if (FundName != SourceFundName)
                {
                    if (MessageBox.Show(this, "WARNING: You have an mis-matched a Fund or Ticker on this Trade.\r\n\r\n" +
                                        "Matching\r\n\t'" + FundName + "'\r\nwith\r\n\t'" + SourceFundName + "'.\r\n\r\n" +
                                        "Do you really wish to do this'?",
                                        "Match Futures", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                // See if there is already a value in the Target
                if (TradeID != "")
                {
                    DialogResult dr1 = MessageBox.Show(this, "There is already a Value of '" + TradeID + "' on this Trade (" + FundName + ").\r\n\r\n" +
                                        "Do you wish to replace this with '" + t_TradeID + "'?\r\n"+
                                        "[Yes = replace, No = Append, Cancel = Abort]",
                                        "Match Futures", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dr1 == DialogResult.Cancel)
                    {
                        return;
                    }
                    else if (dr1 == DialogResult.No)
                    {
                        t_TradeID = TradeID + "," + t_TradeID;
                    }
                }
                Target.Cells["TradeID"].Value = t_TradeID;
                String MatchValue = SystemLibrary.ToString(Source.Cells["t_CustodianConfirmed"].Value);
                String ContractID = SystemLibrary.ToString(Target.Cells["ContractID"].Value);
                if (MatchValue.Length>0)
                    MatchValue = MatchValue + ",";
                MatchValue = MatchValue + ContractID;
                Source.Cells["t_CustodianConfirmed"].Value = EnsureUnique(MatchValue);

                // Now see if I can match the 'L','T' pair off.
                // TransactionType = "L" will match on Gross, "T" will match on Comm

                for (int i = 0; i < dg_ML_Futures.Rows.Count; i++)
                {
                    if (SystemLibrary.ToString(dg_ML_Futures["ContractID", i].Value) == ContractID &&
                        SystemLibrary.ToString(dg_ML_Futures["TradeID", i].Value) == "")
                    {
                        if (TransactionType == "L")
                        {
                            // Looking for T record
                            if (SystemLibrary.ToDouble(dg_ML_Futures["Commission", i].Value) == SystemLibrary.ToDouble(Source.Cells["t_Commission"].Value))
                            {
                                dg_ML_Futures["TradeID", i].Value = t_TradeID;
                            }
                        }
                        else
                        {
                            if (SystemLibrary.ToDouble(dg_ML_Futures["GrossValue", i].Value) == SystemLibrary.ToDouble(Source.Cells["t_GrossValue"].Value))
                            {
                                dg_ML_Futures["TradeID", i].Value = t_TradeID;
                            }
                        }
                    }
                }

            }
        } //dg_ML_Futures_DragDrop()

        private String EnsureUnique(String inStr)
        {
            /*
             * Make sure comma seperated string only has unique values
             */
            // Local Variables
            String RetVal = "";
            ArrayList arr_Str = new ArrayList();

            String[] mySplit = inStr.Split(',');

            if (mySplit.Length == 1)
                RetVal = inStr;
            else
            {
                for (int i = 0; i < mySplit.Length; i++)
                {
                    if (!arr_Str.Contains(mySplit[i]) && mySplit[i]!="")
                    {
                        arr_Str.Add(mySplit[i]);
                        arr_Str.Sort();
                    }
                }

                for (int i = 0; i < arr_Str.Count; i++)
                    RetVal = RetVal + arr_Str[i].ToString() + ",";
                //Strip of last ","
                if (RetVal.EndsWith(","))
                    RetVal = RetVal.Substring(0, RetVal.Length-1);
            }

            return (RetVal);

        } //EnsureUnique()

        private void dg_Trade_DragEnter(object sender, DragEventArgs e)
        {
            DataGridViewRow Source = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));
            DataGridView Target = (DataGridView)sender;
            if (Source.DataGridView.Name!=Target.Name)
                e.Effect = DragDropEffects.Copy;
        } //dg_Trade_DragEnter()

        private void dg_Trade_DragDrop(object sender, DragEventArgs e)
        {
            return;
        } //dg_Trade_DragDrop()

        private void bt_Mark_As_Confirmed_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "This function ignores the differences and marks both the ML Futures and the system Trades as reconcilled.\r\n\r\n" +
                                "This is NOT an advised process, but is sometimes needed.\r\n\r\n" +
                                "Do you really wish to do this'?",
                                "WARNING: Marking as Confirmed with NO RECONCILLIATION", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Loop around each of the gridviews and update the database
                for (int i = 0; i < dg_ML_Futures.Rows.Count; i++)
                {
                    String mySql = "Update MLFut_Trades " +
                                   "Set Reconcilled = 'Y', " +
                                   "    Reason = 'Override: '+IsNull(Reason,'') " +
                                   "Where   ContractID = '" + dg_ML_Futures["ContractID", i].Value.ToString() + "' " +
                                   "And     TradeDate = '" + dg_ML_Futures["TradeDate", i].EditedFormattedValue.ToString() + "' " +
                                   "And     BBG_Ticker = '" + dg_ML_Futures["BBG_Ticker", i].Value.ToString() + "' " +
                                   "And     Case When BuySellCode = 1 then 1 else -1 end * Quantity = " + dg_ML_Futures["Quantity", i].Value.ToString() + " " +
                                   "And     ISNULL(MLFut_Trades.Reconcilled, 'N') = 'N' ";
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
            //return;
            DateTime recalcDate = DateTime.MaxValue;
            ArrayList arr_Processed = new ArrayList();


            for (int i = 0; i < dg_ML_Futures.Rows.Count; i++)
            {
                // See if TradeID is set
                Boolean setMatch = false;
                Boolean setMatchComm = false;
                String mySql = "";
                String myTrades = SystemLibrary.ToString(dg_ML_Futures["TradeID", i].Value);
                String ContractID = SystemLibrary.ToString(dg_ML_Futures["ContractID", i].Value);
                String TransactionType = SystemLibrary.ToString(dg_ML_Futures["TransactionType", i].Value);
                String inContractID = "";

                if (myTrades.Length > 0 && TransactionType=="Trade")
                {
                    // See if this ContractID already processd
                    if (arr_Processed.Contains(ContractID))
                        continue;
                    arr_Processed.Add(ContractID);
                    arr_Processed.Sort();

                    // Can be 123/456/789
                    String[] myTradeID = myTrades.Split(',');
                    String myFilter = "TradeID=" + myTradeID[0];
                    for (int t = 1; t < myTradeID.Length; t++)
                    {
                        myFilter = myFilter + " or TradeID=" + myTradeID[t];
                    }
                    // Now get all the ContractID's that go with this Trade
                    String myFilterContracts = "";
                    foreach (DataRow dr in dt_Trade.Select(myFilter))
                    {
                        String myContractID = "";
                        // Now use this information to Scan dg_Trade which has the real data :-) It would be nice to be able to filter a dgv.
                        for (int j = 0; j < dg_Trade.Rows.Count; j++)
                        {
                            if (dg_Trade["t_TradeID", j].Value.ToString() == dr["TradeID"].ToString())
                            {
                                myContractID = dg_Trade["t_CustodianConfirmed", j].Value.ToString();
                                continue;
                            }
                        }
                        
                        String[] myID = myContractID.Split(',');
                        for (int t = 0; t < myID.Length; t++)
                        {
                            if (!arr_Processed.Contains(myID[t])) //ContractID))
                                arr_Processed.Add(myID[t]); //ContractID);
                            if (myFilterContracts.Length == 0)
                            {
                                myFilterContracts = "ContractID='" + myID[t] + "'";
                                inContractID = inContractID + "'" + myID[t] + "'";
                            }
                            else
                            {
                                myFilterContracts = myFilterContracts + " or ContractID='" + myID[t] + "'";
                                inContractID = inContractID + ",'" + myID[t] + "'";
                            }
                        }
                    }


                    // See if GrossValue matches between these records.
                    Double t_Quantity = SystemLibrary.ToDouble(dt_Trade.Compute("sum(Quantity)", myFilter));
                    //Double Quantity = SystemLibrary.ToDouble(dg_ML_Futures["Quantity", i].Value);
                    Double Quantity = SystemLibrary.ToDouble(dt_ML_Futures.Compute("sum(Quantity)", myFilterContracts));
                    Double t_GrossValue = SystemLibrary.ToDouble(dt_Trade.Compute("sum(GrossValue)", myFilter));
                    Double GrossValue = SystemLibrary.ToDouble(dt_ML_Futures.Compute("sum(GrossValue)", myFilterContracts));
                    Double t_Commission = SystemLibrary.ToDouble(dt_Trade.Compute("sum(Commission)", myFilter));
                    Double Commission = SystemLibrary.ToDouble(dt_ML_Futures.Compute("sum(Commission)", myFilterContracts));
                    Double gross_diff = GrossValue - t_GrossValue;
                    Double comm_diff = Commission - t_Commission;

                    if (t_Quantity != Quantity)
                    {
                        MessageBox.Show(this, "Sorry Quantities do not match on the record(s), so aborting ContractID='" + ContractID + "'\r\n" +
                                            "\t" + t_Quantity.ToString("#,###") + "\tversus\t" + Quantity.ToString("#,###") + "\r\n\r\n" +
                                            "Any trades already processed will have been saved.",
                                            "Save: Aborted");
                        return;
                    }

                    if (gross_diff != 0 && GrossValue != 0)
                    {
                        DialogResult dr = MessageBox.Show(this, "There is a difference in Gross Value between these records. [ContractID='" + ContractID + "']\r\n" +
                                            "\tThe Futures Broker record = " + GrossValue.ToString("$#,###.##") + "\r\n" +
                                            "\tThe internal record =     " + t_GrossValue.ToString("$#,###.##") + "\r\n\r\n" +
                                            "If small, it is recommended you make the adjustment.\r\n\r\n" +
                                            "Do you want to ADJUST the Trade to match the Futures broker record?",
                                            "Reconcilliation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            setMatch = true;
                        }
                        else if (dr == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                    else
                        setMatch = false;

                    if (comm_diff != 0 && Commission != 0)
                    {
                        DialogResult dr = MessageBox.Show(this, "There is a difference in Commission between these records. [ContractID=" + ContractID + "]\r\n" +
                                            "\tThe Futures Broker record = " + Commission.ToString("$#,###.##") + "\r\n" +
                                            "\tThe internal record =     " + t_Commission.ToString("$#,###.##") + "\r\n\r\n" +
                                            "If small, it is recommended you make the adjustment.\r\n\r\n" +
                                            "Do you want to ADJUST the Trade to match the Futures broker record?",
                                            "Reconcilliation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            setMatchComm = true;
                        }
                        else if (dr == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                    else
                        setMatchComm = false;

                    mySql = "Update MLFut_Trades " +
                            "Set Reconcilled = 'Y', ";
                    if (myTradeID.Length == 1)
                        mySql = mySql + "TradeID = " + myTradeID[0] + ", ";
                    mySql = mySql +
                            "    Reason = 'Matched to: " + myTrades + "' " +
                            "Where   ContractID in (" + inContractID + ") ";
                    SystemLibrary.SQLExecute(mySql);


                    for (int t = 0; t < myTradeID.Length; t++)
                    {
                        if (setMatch || setMatchComm)
                        {
                            // Fudge the update by setting the diff to zero if the user did not want to apply.
                            if (!setMatch)
                                gross_diff = 0;
                            if (!setMatchComm)
                                comm_diff = 0;

                            // Make the small adjustment on the first record, then setMatch = false
                            // - Need to do an SQL on the trade record, then need to recalc P&l from this point
                            mySql = "Update Trade " +
				                    "Set	" +
                                    "       GrossValue = GrossValue + " + gross_diff.ToString() + ", " +
				                    "		Commission = Commission + " + comm_diff.ToString() + ", " +
				                    "		NetValue = NetValue + " + (gross_diff + comm_diff).ToString() + ", " +
                                    "       Price = GrossValue / (Quantity * isNull(Pos_Mult_Factor,1)), " +
                                    "       custodianconfirmed = dbo.f_Today() " +
                                    "Where 	TradeID = " + myTradeID[t];
                            SystemLibrary.SQLExecute(mySql);

                            // Fix the Transaction Record
                            mySql = "exec sp_ApplyTradeChangeToTransaction " + myTradeID[t];
                            SystemLibrary.SQLExecute(mySql);

                            setMatch = false;
                            setMatchComm = false;

                            DateTime myDate = Convert.ToDateTime(dg_ML_Futures["TradeDate", i].Value);
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
                    }
                    // Clear the record on the 2 datagrids

                }
            }

            // No recalc historic P&L if needed
            if (recalcDate != DateTime.MaxValue)
            {
                Cursor.Current = Cursors.WaitCursor;
                SystemLibrary.SQLExecute("exec sp_Calc_Profit_RebuildFrom '" + recalcDate.ToString("dd-MMM-yyyy") + "' ");
                SystemLibrary.SQLExecute("Exec sp_SOD_Positions");
                Cursor.Current = Cursors.Default;
            }
            // Refresh
            LoadTrades();
            // Let the Parent know this has close
            if (ParentForm1 != null)
                ParentForm1.LoadActionTab(true);

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
        }

        private void bt_Request_ALL_Click(object sender, EventArgs e)
        {
            String mySql;
            String myDate = dtp_AllFutures.Value.ToString("dd-MMM-yyyy");

            mySql = "SELECT     ISNULL(MLFut_Trades.Reconcilled, 'N') AS Reconcilled, MLFut_Trades.OFFICE + '-' + MLFut_Trades.ACCOUNT AS Account, Fund.ShortName, " +
                    "			MLFut_Trades.TRADEDATE,  " +
                    "                      CASE WHEN MLFut_Trades.BuySellCode = 1 THEN 'B' ELSE 'S' END AS Side,  " +
                    "                      CASE WHEN MLFut_Trades.BuySellCode = 1 THEN 1 ELSE - 1 END * MLFut_Trades.QUANTITY AS Quantity, MLFut_Trades.TRADEDESCRIPTION,  " +
                    "                      MLFut_Trades.TRADEPRICE, MLFut_Trades.BBG_Ticker, MLFut_Trades.CURRENCY, MLFut_Trades.MLFileName " +
                    "FROM       MLFut_Trades LEFT OUTER JOIN " +
                    "                      Fund ON MLFut_Trades.FundId = Fund.FundID  " +
                    "WHERE      MLFut_Trades.TRADEDATE = '" + myDate + "' " +
                    "AND		MLFut_Trades.TRANSACTIONTYPE = 'T' " +
                    "Order by 2, 7, 5, 6 ";

            DataTable dt_AllFutures = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_AllFutures.DataSource = dt_AllFutures;
            ParentForm1.SetFormatColumn(dg_AllFutures, "TRADEDATE", Color.Empty, Color.Empty, "dd-MMM-yy", "");
            ParentForm1.SetFormatColumn(dg_AllFutures, "Quantity", Color.Empty, Color.Empty, "0", "0");
            ParentForm1.SetFormatColumn(dg_AllFutures, "Account", Color.DarkBlue, Color.Cyan, "", "");

            dg_AllFutures.Columns.Remove("Reconcilled");
            DataGridViewCheckBoxColumn Reconcilled = new DataGridViewCheckBoxColumn();
            Reconcilled.HeaderText = "Reconcilled Trade";
            Reconcilled.FalseValue = "N";
            Reconcilled.TrueValue = "Y";
            Reconcilled.Name = "Reconcilled";
            Reconcilled.DataPropertyName = "Reconcilled";
            dg_AllFutures.Columns.Insert(0, Reconcilled);


            for (int i = 0; i < dt_AllFutures.Rows.Count; i++)
            {
                if (dg_AllFutures["Reconcilled", i].Value.ToString() == "Y")
                {
                    dg_AllFutures["Reconcilled", i].Style.ForeColor = Color.DarkGreen;
                    dg_AllFutures["Reconcilled", i].Style.BackColor = Color.LightGreen;
                }
                else
                {
                    dg_AllFutures["Reconcilled", i].Style.ForeColor = Color.DarkRed;
                    dg_AllFutures["Reconcilled", i].Style.BackColor = Color.LightPink;
                }
            }

        } //bt_Calculator_Click()

        private void bt_BackEnd_Reprocess_Click(object sender, EventArgs e)
        {
            // Force through the processing scripts
            Cursor.Current = Cursors.WaitCursor;
            SystemLibrary.SQLExecute("Exec sp_MLFut_Process_File '', 'MLFut_Cash' ");
            SystemLibrary.SQLExecute("Exec sp_MLFut_Process_File '', 'MLFut_Money' ");
            SystemLibrary.SQLExecute("Exec sp_MLFut_Process_File '', 'MLFut_Trades' ");
            SystemLibrary.SQLExecute("Exec sp_MLFut_Process_File '', 'MLFut_Open_Positions' ");
            LoadTrades();

            Cursor.Current = Cursors.Default;
            NeedFullUpdate = true;
            MessageBox.Show("Completed.\r\n\r\nThis has forced a reprocess of uploaded files which will have Reconcile records if able.\r\n" +
                            "This is not normally needed but is here for completeness.", ((Button)sender).Text);

        } //bt_BackEnd_Reprocess_Click()

        private void MLFuture_Reconcile_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (NeedFullUpdate)
                SystemLibrary.SQLExecute("Exec sp_actionsNeeded 2, 'N'");

            Cursor.Current = Cursors.Default;

        } //bt_BackEnd_Reprocess_Click()

        private void dg_Trade_MouseClick(object sender, MouseEventArgs e)
        {
            CXLocation = dg_Trade.Location.X + e.Location.X + 5;
            CYLocation = dg_Trade.Location.Y + e.Location.Y;

        } //dg_Trade_MouseClick()

        private void dg_Trade_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Show the Bloomberg popup menu
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    String t_ParentFundID = SystemLibrary.ToString(dg_Trade.Rows[e.RowIndex].Cells["t_ParentFundID"].Value);
                    if (t_ParentFundID.Length > 0)
                    {
                        switch (dg_Trade.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.ToString())
                        {
                            case "t_TradeDate":
                                DateTime TradeDate = Convert.ToDateTime(dg_Trade.Rows[e.RowIndex].Cells["t_TradeDate"].Value);
                                String TradeID = SystemLibrary.ToString(dg_Trade.Rows[e.RowIndex].Cells["t_TradeID"].Value);
                                String myValue = TradeDate.ToString("dd-MMM-yyyy");
                                if (SystemLibrary.InputBox("Alter Trade Date for TradeID " + TradeID + " from " + dg_Trade.Rows[e.RowIndex].Cells["t_TradeDate"].FormattedValue.ToString(), "Change the Trade Date OR Cancel", ref myValue, this.validate_WhatIfTradeDate, MessageBoxIcon.Question) == DialogResult.OK)
                                {
                                    dg_Trade.Rows[e.RowIndex].Cells["t_TradeDate"].Value = Convert.ToDateTime(myValue);
                                    dg_Trade.Rows[e.RowIndex].Cells["t_SettlementDate"].Value = Convert.ToDateTime(myValue);
                                    // Now alter the underlying data to match the new dates
                                    String mySql = "Update trade " +
                                                   "Set TradeDate = '" + myValue + "', " +
                                                   "    SettlementDate = '" + myValue + "' " +
                                                   "Where tradeid = " + TradeID;
                                    SystemLibrary.SQLExecute(mySql);
                                    mySql = "Update Transactions " +
                                            "Set RecordDate = '" + myValue + "', " +
                                            "    EffectiveDate  = '" + myValue + "' " +
                                            "Where tradeid = " + TradeID + " " +
                                            "And TranType = 'futures comm & interest'";
                                    SystemLibrary.SQLExecute(mySql);
                                    mySql = "Exec sp_ReapplyMargin " + t_ParentFundID + ", '" + TradeDate.ToString("dd-MMM-yyyy") + "' ";
                                    SystemLibrary.SQLExecute(mySql);
                                    mySql = "Exec sp_ReapplyMargin " + t_ParentFundID + ", '" + myValue + "' ";
                                    SystemLibrary.SQLExecute(mySql);
                                }
                                break;
                        }
                    }
                }
            }
            catch { }


        } //dg_Trade_CellMouseClick()

        SystemLibrary.InputBoxValidation validate_WhatIfTradeDate = delegate(String myValue)
        {
            // Rules: Must be a valid date
            try
            {
                DateTime myDateTime = Convert.ToDateTime(myValue);
                return "";
            }
            catch
            {
                return "'" + myValue + "' Must be a valid Date.";
            }

        }; //validate_WhatIfTradeDate()


    }
}
