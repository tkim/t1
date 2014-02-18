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
    public partial class BrokerMaintenance : Form
    {
        public DataTable dt_ParentFund;
        public DataTable dt_Broker;
        public DataTable dt_BrokerMapping = new DataTable();
        public DataTable dt_BrokerFundMapping;
        public DataTable dt_ExchangeFees;
        public DataTable dt_MissingBroker;
        private Object LastValue;
        public Form1 ParentForm1;

        public BrokerMaintenance()
        {
            InitializeComponent();
        }

        private void BrokerMaintenance_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            LoadFund();
            LoadBroker();
            LoadExchangeFees();
            LoadBrokerMapping();
            LoadBrokerFundMapping();
            LoadMissingBroker();

        } //BrokerMaintenance_Load()

        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;

        } //FromParent()

        public void LoadFund()
        {
            // Get just the Parent Fund's as these are the ones that can be settled against.
            // Local Variables
            String mySql;

            mySql = "Select FundID, FundName, ShortName " +
                    "From	Fund " +
                    "Where	isNull(Active,'N') = 'Y' " +
                    "And    ParentFundID = FundID " +
                    "Order by FundName ";
            dt_ParentFund = SystemLibrary.SQLSelectToDataTable(mySql);

        } //LoadFund()

        public void LoadBroker()
        {
            // Local Variables
            String mySql;

            mySql = "Select BrokerID, ExtBrokerID, Exchange, BrokerName, IsInternal, Contact, EmailSalutation, Email, Phone, Fax, IsFuture, EquityCommRate, FutureComm, MinimumFee, DecimalPlaces, IsActive " +
                    "from	Broker " +
                    "Order by BrokerName, Exchange, ExtBrokerID ";
            dt_Broker = SystemLibrary.SQLSelectToDataTable(mySql);

            dg_Broker.Rows.Clear();
            foreach (DataRow dr in dt_Broker.Rows)
            {
                int myRow = dg_Broker.Rows.Add();
                dg_Broker["_BrokerID", myRow].Value = dr["BrokerID"];
                dg_Broker["ExtBrokerID", myRow].Value = dr["ExtBrokerID"];
                dg_Broker["_Exchange", myRow].Value = dr["Exchange"];
                dg_Broker["BrokerName", myRow].Value = dr["BrokerName"];
                dg_Broker["IsInternal", myRow].Value = dr["IsInternal"];
                dg_Broker["Contact", myRow].Value = dr["Contact"];
                dg_Broker["EmailSalutation", myRow].Value = dr["EmailSalutation"];
                dg_Broker["Email", myRow].Value = dr["Email"];
                dg_Broker["Phone", myRow].Value = dr["Phone"];
                dg_Broker["Fax", myRow].Value = dr["Fax"];
                dg_Broker["IsFuture", myRow].Value = dr["IsFuture"];
                dg_Broker["EquityCommRate", myRow].Value = dr["EquityCommRate"];
                dg_Broker["FutureComm", myRow].Value = dr["FutureComm"];
                dg_Broker["MinimumFee", myRow].Value = dr["MinimumFee"];
                dg_Broker["DecimalPlaces", myRow].Value = dr["DecimalPlaces"];
                dg_Broker["IsActive", myRow].Value = dr["IsActive"];
            }

        } //LoadBroker()

        public void LoadExchangeFees()
        {
            // Local Variables
            String mySql;

            mySql = "Select Exchange, YellowKey, StampDutyRate, RoundUp, VATRate " +
                    "From	ExchangeFees " +
                    "Order by Exchange, YellowKey ";
            dt_ExchangeFees = SystemLibrary.SQLSelectToDataTable(mySql);

            dg_ExchangeFees.Rows.Clear();
            foreach(DataRow dr in dt_ExchangeFees.Rows)
            {
                int myRow = dg_ExchangeFees.Rows.Add();
                dg_ExchangeFees["Exchange", myRow].Value = dr["Exchange"];
                dg_ExchangeFees["YellowKey", myRow].Value = dr["YellowKey"].ToString();
                dg_ExchangeFees["StampDutyRate", myRow].Value = dr["StampDutyRate"];
                dg_ExchangeFees["RoundUp", myRow].Value = dr["RoundUp"];
                dg_ExchangeFees["VATRate", myRow].Value = dr["VATRate"];
            }

        } //LoadExchangeFees()

        public void LoadBrokerMapping()
        {
            // Local Variables
            String mySql;

            mySql = "Select Broker, StrategyType, TranAccount, BrokerID " +
                    "From	BrokerMapping " +
                    "union " +
                    "Select BBG_Broker, IsNull(BBG_StrategyType,''), IsNull(BBG_TranAccount,''), null " +
                    "From	Fills " +
                    "Where	Confirmed = 'N' " +
                    "And	BrokerID is null " +
                    "and not exists (select 'x' " +
                    "				From    BrokerMapping b " +
                    "				Where	b.Broker = fills.BBG_Broker " +
                    "				And	    b.StrategyType  = IsNull(fills.BBG_StrategyType,'') " +
                    "				And	    b.TranAccount  = IsNull(fills.BBG_TranAccount,'') " +
                    "				) " +
                    "Order by 1, 2, 3 ";
            dt_BrokerMapping = SystemLibrary.SQLSelectToDataTable(mySql);

            DataGridViewComboBoxColumn dcb = (DataGridViewComboBoxColumn)dg_MapBroker.Columns["BrokerID"];
            dcb.DataSource = dt_Broker;
            dcb.DisplayMember = "BrokerName";
            dcb.ValueMember = "BrokerId";
            dcb.DataPropertyName = "BrokerID";

            dg_MapBroker.Rows.Clear();
            lb_ActionNeeded.Visible = false;
            foreach (DataRow dr in dt_BrokerMapping.Rows)
            {
                int myRow = dg_MapBroker.Rows.Add();
                dg_MapBroker["Broker", myRow].Value = dr["Broker"];
                dg_MapBroker["StrategyType", myRow].Value = dr["StrategyType"];
                dg_MapBroker["TranAccount", myRow].Value = dr["TranAccount"];
                dg_MapBroker["BrokerID", myRow].Value = dr["BrokerID"];
                // Flag an Action if needed
                if (dr["BrokerID"].ToString().Trim().Length==0)
                    lb_ActionNeeded.Visible = true;

            }


        } //LoadBrokerMapping()


        public void LoadBrokerFundMapping()
        {
            // Local Variables
            String mySql;

            mySql = "Select BrokerID, FundID, crncy, ExtID " +
                    "From	BrokerFundMapping " +
                    "Order by 1, 2, 3, 4 ";
            dt_BrokerFundMapping = SystemLibrary.SQLSelectToDataTable(mySql);

            DataGridViewComboBoxColumn dcb = (DataGridViewComboBoxColumn)dg_BrokerFundMapping.Columns["bfm_BrokerID"];
            dcb.DataSource = dt_Broker;
            dcb.DisplayMember = "BrokerName";
            dcb.ValueMember = "BrokerId";
            dcb.DataPropertyName = "bfm_BrokerID";

            DataGridViewComboBoxColumn dcb1 = (DataGridViewComboBoxColumn)dg_BrokerFundMapping.Columns["bfm_FundID"];
            dcb1.DataSource = dt_ParentFund;
            dcb1.DisplayMember = "FundName";
            dcb1.ValueMember = "FundId";
            dcb1.DataPropertyName = "bfm_FundID";

            dg_BrokerFundMapping.Rows.Clear();
            foreach (DataRow dr in dt_BrokerFundMapping.Rows)
            {
                int myRow = dg_BrokerFundMapping.Rows.Add();
                dg_BrokerFundMapping["bfm_BrokerID", myRow].Value = dr["BrokerID"];
                dg_BrokerFundMapping["bfm_FundID", myRow].Value = dr["FundID"];
                dg_BrokerFundMapping["bfm_crncy", myRow].Value = dr["crncy"];
                dg_BrokerFundMapping["bfm_ExtID", myRow].Value = dr["ExtID"];
            }


        } //LoadBrokerFundMapping()

        public void LoadMissingBroker()
        {
            // Local Variables
            String mySql;

            mySql = "Exec sp_MissingBroker 'N' ";
            dt_MissingBroker = SystemLibrary.SQLSelectToDataTable(mySql);

            dg_MissingBroker.Rows.Clear();
            foreach (DataRow dr in dt_MissingBroker.Rows)
            {
                int myRow = dg_MissingBroker.Rows.Add();
                dg_MissingBroker["mb_BrokerID", myRow].Value = dr["BrokerID"];
                dg_MissingBroker["mb_ExtBrokerID", myRow].Value = dr["ExtBrokerID"];
                dg_MissingBroker["mb_Exchange", myRow].Value = dr["Exchange"];
                dg_MissingBroker["mb_BrokerName", myRow].Value = dr["BrokerName"];
                dg_MissingBroker["mb_IsInternal", myRow].Value = dr["IsInternal"];
                dg_MissingBroker["mb_Contact", myRow].Value = dr["Contact"];
                dg_MissingBroker["mb_EmailSalutation", myRow].Value = dr["EmailSalutation"];
                dg_MissingBroker["mb_Email", myRow].Value = dr["Email"];
                dg_MissingBroker["mb_Phone", myRow].Value = dr["Phone"];
                dg_MissingBroker["mb_Fax", myRow].Value = dr["Fax"];
                dg_MissingBroker["mb_IsFuture", myRow].Value = dr["IsFuture"];
                dg_MissingBroker["mb_EquityCommRate", myRow].Value = dr["EquityCommRate"];
                dg_MissingBroker["mb_FutureComm", myRow].Value = dr["FutureComm"];
                dg_MissingBroker["mb_MinimumFee", myRow].Value = dr["MinimumFee"];
                dg_MissingBroker["mb_DecimalPlaces", myRow].Value = dr["DecimalPlaces"];
            }

        } //LoadMissingBroker()


        private void bt_SaveBroker_Click(object sender, EventArgs e)
        {
            // Local Variables
            int myBrokerID;
            String myColumns="";
            String myTable = "Broker";
            DataTable dt_load = SystemLibrary.SQLBulk_GetDefinition(myColumns, myTable, "");
            dg_Broker.Refresh();
            //dg_Broker.re
            // There must be a way to tell which rows have been altered.
            foreach (DataGridViewRow dgr in dg_Broker.Rows)
            {
                if (!dgr.IsNewRow)
                {
                    // Save to Database
                    if (dgr.Cells["_BrokerID"].Value == null)
                    {
                        // Add a new row & Update with new BrokerId
                        myBrokerID = SystemLibrary.SQLSelectInt32("exec sp_GetNextID 'BrokerID'");
                        DataRow dr = dt_load.NewRow();
                        dr["BrokerID"] = myBrokerID;
                        dgr.Cells["_BrokerID"].Value = myBrokerID;
                        if (dgr.Cells["EquityCommRate"].Value == null) dgr.Cells["EquityCommRate"].Value = 0;
                        if (dgr.Cells["FutureComm"].Value == null) dgr.Cells["FutureComm"].Value = 0;
                        if (dgr.Cells["MinimumFee"].Value == null) dgr.Cells["MinimumFee"].Value = 0;
                        if (dgr.Cells["DecimalPlaces"].Value == null) dgr.Cells["DecimalPlaces"].Value = 2;
                        dt_load.Rows.Add(dr);
                    }
                    DataRow[] df = dt_load.Select("BrokerID=" + dgr.Cells["_BrokerID"].Value.ToString());
                    if (df.Length > 0)
                    {
                        df[0]["BrokerID"] = dgr.Cells["_BrokerID"].Value;
                        df[0]["ExtBrokerID"] = dgr.Cells["ExtBrokerID"].Value;
                        df[0]["Exchange"] = dgr.Cells["_Exchange"].Value;
                        df[0]["BrokerName"] = dgr.Cells["BrokerName"].Value;
                        df[0]["IsInternal"] = dgr.Cells["IsInternal"].Value;
                        df[0]["Contact"] = dgr.Cells["Contact"].Value;
                        df[0]["EmailSalutation"] = dgr.Cells["EmailSalutation"].Value;
                        df[0]["Email"] = dgr.Cells["Email"].Value;
                        df[0]["Phone"] = dgr.Cells["Phone"].Value;
                        df[0]["Fax"] = dgr.Cells["Fax"].Value;
                        df[0]["IsFuture"] = dgr.Cells["IsFuture"].Value;
                        df[0]["EquityCommRate"] = dgr.Cells["EquityCommRate"].Value;
                        df[0]["FutureComm"] = dgr.Cells["FutureComm"].Value;
                        df[0]["MinimumFee"] = dgr.Cells["MinimumFee"].Value;
                        df[0]["DecimalPlaces"] = dgr.Cells["DecimalPlaces"].Value;
                        df[0]["IsActive"] = dgr.Cells["IsActive"].Value;
                    }
                }
            }

            // After Save to Database, make sure DDLB in dg_MapBroker is refreshed
            Int32 myRows = SystemLibrary.SQLBulkUpdate(dt_load, myColumns, myTable);
            LoadBroker();
            LoadMissingBroker();
            dg_MapBroker.Refresh();
            DataGridViewComboBoxColumn dcb = (DataGridViewComboBoxColumn)dg_MapBroker.Columns["BrokerID"];
            dcb.DataSource = dt_Broker;
            dcb.DisplayMember = "BrokerName";
            dcb.ValueMember = "BrokerId";
            dcb.DataPropertyName = "BrokerID";
            MessageBox.Show("Saved", "Broker Details");

        } //bt_SaveBroker_Click()

        private void bt_SaveBrokerMap_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;

            // Update the database
            foreach (DataGridViewRow dr in dg_MapBroker.Rows)
            {
                //Broker, StrategyType, BrokerID
                if (dr.Cells["BrokerID"].Value.ToString().Length>0)
                {
                    Int16 BrokerID = Convert.ToInt16(dr.Cells["BrokerID"].Value);
                    String StrategyType = dr.Cells["StrategyType"].Value.ToString();
                    String TranAccount = dr.Cells["TranAccount"].Value.ToString();
                    String Broker = dr.Cells["Broker"].Value.ToString();
                    mySql = "Update BrokerMapping " +
                            "Set BrokerID = " + BrokerID.ToString() + " " +
                            "Where  Broker = '" + Broker + "' " +
                            "And  StrategyType = '" + StrategyType + "' " +
                            "And  TranAccount = '" + TranAccount + "' ";
                    if (SystemLibrary.SQLExecute(mySql) < 1)
                    {
                        mySql = "Insert into BrokerMapping(Broker, StrategyType, TranAccount, BrokerID) " +
                                "Values ('" + Broker + "','" + StrategyType + "','" + TranAccount + "'," + BrokerID.ToString() + ")";
                        SystemLibrary.SQLExecute(mySql);
                    }
                }
            }
            // Sort out Non-ProcessedEOD Order/Fills for New BrokerID's
            mySql = "Update Fills_Allocation " +
                    "Set	BrokerID = BrokerMapping.BrokerID " +
                    "From	BrokerMapping " +
                    "Where	Fills_Allocation.OrderRefID in (Select	OrderRefID " +
                    "										From	Orders " +
                    "										Where	ProcessedEOD = 'N' " +
                    "									    ) " +
                    "And		BrokerMapping.Broker = Fills_Allocation.BBG_Broker " +
                    "And		BrokerMapping.StrategyType = Fills_Allocation.BBG_StrategyType " +
                    "And		BrokerMapping.TranAccount = Fills_Allocation.BBG_TranAccount ";
            SystemLibrary.SQLExecute(mySql);

            // Reload
            LoadBrokerMapping();
            MessageBox.Show("Saved", "Broker Mapping");

        } //bt_SaveBrokerMap_Click()

        private void bt_SaveExchangeFees_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;

            // Update the database
            SystemLibrary.SQLExecute("Delete from ExchangeFees");
            foreach (DataGridViewRow dr in dg_ExchangeFees.Rows)
            {
                //Exchange, YellowKey, StampDutyRate, RoundUp, VATRate
                if (dr.Cells["Exchange"].Value!=null)
                {
                    String Exchange = dr.Cells["Exchange"].Value.ToString();
                    String YellowKey = SystemLibrary.ToString(dr.Cells["YellowKey"].Value);
                    Double StampDutyRate = SystemLibrary.ToDouble(dr.Cells["StampDutyRate"].Value);
                    Int16 RoundUp = SystemLibrary.ToInt16(dr.Cells["RoundUp"].Value);
                    Double VATRate = SystemLibrary.ToDouble(dr.Cells["VATRate"].Value);
                    mySql = "Update ExchangeFees  " +
                            "Set StampDutyRate = " + StampDutyRate.ToString() + ", " +
                            "    RoundUp = " + RoundUp.ToString() + ", " +
                            "    VATRate = " + VATRate.ToString() + " " +
                            "Where  Exchange = '" + Exchange + "' " +
                            "And    IsNull(YellowKey,'') = '" + YellowKey + "' ";
                    if (SystemLibrary.SQLExecute(mySql) < 1)
                    {
                        mySql = "Insert into ExchangeFees(Exchange, YellowKey, StampDutyRate, RoundUp, VATRate) " +
                                "Values ('" + Exchange + "','" + YellowKey + "'," + StampDutyRate.ToString() + "," + RoundUp.ToString() + "," + VATRate.ToString() + ")";
                        SystemLibrary.SQLExecute(mySql);
                    }
                }
            }
            // Reload
            LoadExchangeFees();
            MessageBox.Show("Saved", "Exchange Fees");

        } //bt_SaveExchangeFees_Click()

        private void dg_Broker_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Seems the only way I can get the previous value before CellEndEdit().
            LastValue = dg_Broker[e.ColumnIndex, e.RowIndex].Value;
        } //dg_Broker_CellBeginEdit()

        private void dg_Broker_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // What column is changing
            // - dg_Broker.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.ToString().ToUpper()
            if (dg_Broker.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.DefaultCellStyle.Format.ToString().IndexOf('%')>-1)
            {
                dg_Broker.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = SystemLibrary.ToDouble(dg_Broker.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) / 100.0;
            }
        } //dg_Broker_CellEndEdit()

        private void dg_ExchangeFees_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Seems the only way I can get the previous value before CellEndEdit().
            LastValue = dg_ExchangeFees[e.ColumnIndex, e.RowIndex].Value;

        } //dg_ExchangeFees_CellBeginEdit()

        private void dg_ExchangeFees_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // What column is changing
            // - dg_Broker.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.ToString().ToUpper()
            if (dg_ExchangeFees.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.DefaultCellStyle.Format.ToString().IndexOf('%') > -1)
            {
                dg_ExchangeFees.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = SystemLibrary.ToDouble(dg_ExchangeFees.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) / 100.0;
            }
            if (dg_ExchangeFees.Columns[e.ColumnIndex].Name == "Exchange")
                dg_ExchangeFees.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dg_ExchangeFees.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper();

        } //dg_ExchangeFees_CellEndEdit()

        private void BrokerMaintenance_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Let the Parent know this has close
            if (ParentForm1 != null)
                ParentForm1.LoadActionTab(true);
        } //BrokerMaintenance_FormClosed()

        private void dg_MissingBroker_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DataGridView.HitTestInfo info = dg_MissingBroker.HitTest(e.X, e.Y);
                if (info.RowIndex >= 0)
                {
                    //DataGridViewRow view = (DataGridViewRow)dg_MissingFunds.Rows[info.RowIndex].DataBoundItem;
                    DataGridViewRow view = (DataGridViewRow)dg_MissingBroker.Rows[info.RowIndex];
                    if (view != null)
                        dg_MissingBroker.DoDragDrop(view, DragDropEffects.Copy);
                }
            }
        } //dg_MissingBroker_MouseDown()

        private void dg_Broker_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;

        } //dg_Broker_DragEnter()

        private void dg_Broker_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = dg_Broker.PointToClient(new Point(e.X, e.Y));
            int myTargetRow = dg_Broker.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            DataGridViewRow Source = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));

            if (myTargetRow >= 0 && !dg_Broker.Rows[myTargetRow].IsNewRow)
            {
                // Get the ExtId from the source.
                String ExtID = SystemLibrary.ToString(Source.Cells["mb_ExtBrokerID"].Value);

                // See What the Target table has
                DataGridViewRow Target = (DataGridViewRow)dg_Broker.Rows[myTargetRow];
                String ExtBrokerID = SystemLibrary.ToString(Target.Cells["ExtBrokerID"].Value);
                String BrokerName = SystemLibrary.ToString(Target.Cells["BrokerName"].Value);
                Console.Write("A");

                // See if there is already a value in the Target
                if (ExtBrokerID != "")
                {
                    if (MessageBox.Show(this, "There is already a Value of '" + ExtBrokerID + "' on this broker (" + BrokerName + ").\r\n\r\n" +
                                        "Do you really wish to replace this with '" + ExtID + "'?",
                                        "Change External ID for Broker", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                Target.Cells["ExtBrokerID"].Value = ExtID;
            }
            else
            {
                // New Row
                int myRow = dg_Broker.Rows.Add();
                //dg_Broker["_BrokerID", myRow].Value = Source.Cells["mb_BrokerID"].Value;
                dg_Broker["ExtBrokerID", myRow].Value = Source.Cells["mb_ExtBrokerID"].Value;
                dg_Broker["_Exchange", myRow].Value = Source.Cells["mb_Exchange"].Value;
                dg_Broker["BrokerName", myRow].Value = Source.Cells["mb_BrokerName"].Value;
                dg_Broker["IsInternal", myRow].Value = Source.Cells["mb_IsInternal"].Value;
                dg_Broker["Contact", myRow].Value = Source.Cells["mb_Contact"].Value;
                dg_Broker["EmailSalutation", myRow].Value = Source.Cells["mb_EmailSalutation"].Value;
                dg_Broker["Email", myRow].Value = Source.Cells["mb_Email"].Value;
                dg_Broker["Phone", myRow].Value = Source.Cells["mb_Phone"].Value;
                dg_Broker["Fax", myRow].Value = Source.Cells["mb_Fax"].Value;
                dg_Broker["IsFuture", myRow].Value = Source.Cells["mb_IsFuture"].Value;
                dg_Broker["EquityCommRate", myRow].Value = Source.Cells["mb_EquityCommRate"].Value;
                dg_Broker["FutureComm", myRow].Value = Source.Cells["mb_FutureComm"].Value;
                dg_Broker["MinimumFee", myRow].Value = Source.Cells["mb_MinimumFee"].Value;
                dg_Broker["DecimalPlaces", myRow].Value = Source.Cells["mb_DecimalPlaces"].Value;

            }
        }  //dg_Broker_DragDrop()

        private void bt_SavedBrokerFundMapping_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;


            // Update the database
            SystemLibrary.SQLExecute("Delete from BrokerFundMapping");
            foreach (DataGridViewRow dr in dg_BrokerFundMapping.Rows)
            {
                //Exchange, YellowKey, StampDutyRate, RoundUp, VATRate
                if (dr.Cells["bfm_BrokerID"].Value != null)
                {
                    String BrokerID = dr.Cells["bfm_BrokerID"].Value.ToString();
                    String FundID = dr.Cells["bfm_FundID"].Value.ToString();
                    String crncy = SystemLibrary.ToString(dr.Cells["bfm_crncy"].Value);
                    String ExtID = SystemLibrary.ToString(dr.Cells["bfm_ExtID"].Value);
                    mySql = "Update BrokerFundMapping  " +
                            "Set BrokerID = " + BrokerID + ", " +
                            "    FundID = " + FundID + ", " +
                            "    crncy = '" + crncy + "', " +
                            "    ExtID = '" + ExtID + "' " +
                            "Where  BrokerID = " + BrokerID + " " +
                            "And    FundID = " + FundID + " " +
                            "And    crncy = '" + crncy + "' ";
                    if (SystemLibrary.SQLExecute(mySql) < 1)
                    {
                        mySql = "Insert into BrokerFundMapping(BrokerID, FundID, crncy, ExtID) " +
                                "Values (" + BrokerID + "," + FundID + ",'" + crncy + "','" + ExtID + "')";
                        SystemLibrary.SQLExecute(mySql);
                    }
                }
            }
            // Reload
            LoadBrokerFundMapping();
            MessageBox.Show("Saved", "Broker Fund Mapping");

        }
     
    }
}





