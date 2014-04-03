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
    public partial class MaintainSecurities : Form
    {
        // Public variables
        public Form1 ParentForm1;
        public DataTable dt_crncy;
        public DataTable dt_BBG_Exchange;
        public DataTable dt_Country_Full_Name;
        public DataTable dt_Security_Typ;
        public DataTable dt_Security_Typ2;
        public DataTable dt_Sector;
        public DataTable dt_Industry_Group;
        public DataTable dt_Industry_SubGroup;
        public DataTable dt_ID_BB_COMPANY;
        public DataTable dt_ID_BB_UNIQUE;
        public DataTable dt_ID_BB_GLOBAL;
        public DataTable dt_Securities;
        public object LastValue;
        Boolean FoundChanges = false;

        
        public MaintainSecurities()
        {
            InitializeComponent();
        }
    
        public void FromParent(Form inForm, String inBBG_Ticker)
        {
            ParentForm1 = (Form1)inForm;
            tb_BBG_Ticker.Text = inBBG_Ticker.Trim().ToUpper();

        } // FromParent()

        private void MaintainSecurities_Shown(object sender, EventArgs e)
        {
            SystemLibrary.PositionFormOverParent(this, ParentForm1);
            if (tb_BBG_Ticker.Text.Length > 0)
            {
                if (dt_Securities == null)
                    LoadSecurities();
                else if (dt_Securities.Rows.Count>0)
                    if (SystemLibrary.ToString(dt_Securities.Rows[0]["BBG_Ticker"]).ToLower() != tb_BBG_Ticker.Text.Trim().ToLower())
                        LoadSecurities();
            }

        } //MaintainSecurities_Shown()

        public void LoadSecurities()
        {
            // Local Variables
            String mySql;
            String BBG_Ticker = tb_BBG_Ticker.Text.Trim().ToUpper();

            if (BBG_Ticker.Length == 0)
                return;

            //try
            {
                // Load up the reference data for drop downs
                if (dt_Country_Full_Name == null)
                {
                    dt_crncy = SystemLibrary.SQLSelectToDataTable("Select Distinct crncy from Securities Order By 1");
                    dt_BBG_Exchange = SystemLibrary.SQLSelectToDataTable("Select Distinct BBG_Exchange from Securities Order By 1");
                    dt_Country_Full_Name = SystemLibrary.SQLSelectToDataTable("Select Distinct Country_Full_Name from Securities Order By 1");
                    dt_Security_Typ = SystemLibrary.SQLSelectToDataTable("Select Distinct Security_Typ from Securities Order By 1");
                    dt_Security_Typ2 = SystemLibrary.SQLSelectToDataTable("Select Distinct Security_Typ2 from Securities Order By 1");
                    dt_Sector = SystemLibrary.SQLSelectToDataTable("Select Distinct Sector from Securities Order By 1");
                    dt_Industry_Group = SystemLibrary.SQLSelectToDataTable("Select Distinct Industry_Group from Securities Order By 1");
                    dt_Industry_SubGroup = SystemLibrary.SQLSelectToDataTable("Select Distinct Industry_SubGroup from Securities Order By 1");
                    dt_ID_BB_COMPANY = SystemLibrary.SQLSelectToDataTable("Select Distinct ID_BB_COMPANY from Securities Order By 1");
                    dt_ID_BB_UNIQUE = SystemLibrary.SQLSelectToDataTable("Select Distinct ID_BB_UNIQUE from Securities Order By 1");
                    dt_ID_BB_GLOBAL = SystemLibrary.SQLSelectToDataTable("Select Distinct ID_BB_GLOBAL from Securities Order By 1");
                }

                // Load up the security data
                tb_BBG_Ticker.Text = BBG_Ticker;
                mySql = "Select BBG_Ticker, Last_Price, BBG_Exchange, Security_Name, crncy, CUSIP, ISIN, SEDOL, Pos_Mult_Factor, Round_Lot_Size, Country_Full_Name, " +
                        "Security_Typ, Security_Typ2, Sector, Industry_Group, Industry_SubGroup, ID_BB_COMPANY, ID_BB_UNIQUE, ID_BB_GLOBAL, OCC_SYMBOL " +
                        "From Securities " +
                        "Where BBG_Ticker = '" + BBG_Ticker + "' ";
                dt_Securities = SystemLibrary.SQLSelectToDataTable(mySql);
                dg_Securities.DataSource = dt_Securities;

                if (dt_Securities.Rows.Count == 0)
                {
                    // See if they want to get the data from an existing ticker
                    String myCopyTicker = "";
                    if (SystemLibrary.InputBox("Maintain Securities'" + BBG_Ticker + "' is a New Ticker",
                                               "We note that '" + BBG_Ticker + "' is a new ticker.\r\n\r\n" +
                                               "You can copy the details from an Existing Ticker that you have already modelled.\r\n" +
                                               "To do this, type in a Ticker and Press OK\r\n\r\n" +
                                               "Otherwise press Cancel to start from Scratch.\r\n\r\n", ref myCopyTicker, null, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        mySql = "Select Top 1 '" + BBG_Ticker + "' as BBG_Ticker, Last_Price, BBG_Exchange, '" + BBG_Ticker + " - ' + Security_Name as Security_Name, crncy, '' as CUSIP, '' as ISIN, '" + BBG_Ticker + "' as SEDOL, Pos_Mult_Factor, Round_Lot_Size, Country_Full_Name, " +
                                "Security_Typ, Security_Typ2, Sector, Industry_Group, Industry_SubGroup, ID_BB_COMPANY, '" + BBG_Ticker + "' as ID_BB_UNIQUE, ID_BB_GLOBAL " +
                                "From Securities " +
                                "Where BBG_Ticker = '" + myCopyTicker + "' ";
                        dt_Securities = SystemLibrary.SQLSelectToDataTable(mySql);
                        if (dt_Securities.Rows.Count == 0)
                        {
                            mySql = "Select Top 1 '" + BBG_Ticker + "' as BBG_Ticker, Last_Price, BBG_Exchange, '" + BBG_Ticker + " - ' + Security_Name as Security_Name, crncy, '' as CUSIP, '' as ISIN, '" + BBG_Ticker + "' as SEDOL, Pos_Mult_Factor, Round_Lot_Size, Country_Full_Name, " +
                                    "Security_Typ, Security_Typ2, Sector, Industry_Group, Industry_SubGroup, ID_BB_COMPANY, '" + BBG_Ticker + "' as ID_BB_UNIQUE, ID_BB_GLOBAL " +
                                    "From Securities " +
                                    "Where BBG_Ticker like '" + myCopyTicker + "%' ";
                            dt_Securities = SystemLibrary.SQLSelectToDataTable(mySql);
                            dt_Securities.Rows[0].SetAdded();
                        }
                        else
                            dt_Securities.Rows[0].SetAdded();
                        dg_Securities.DataSource = dt_Securities;
                    }
                    // No existing ticker data, so Create a new record
                    if (dt_Securities.Rows.Count == 0)
                    {
                        DataRow dr = dt_Securities.NewRow();
                        dr["BBG_Ticker"] = BBG_Ticker;
                        dr["BBG_Exchange"] = "US";
                        dr["Security_Name"] = BBG_Ticker;
                        dr["SEDOL"] = BBG_Ticker; // For Back Office
                        dr["crncy"] = "USD";
                        dr["Last_Price"] = 0;
                        dr["Pos_Mult_Factor"] = 1;
                        dr["Round_Lot_Size"] = 1;
                        dr["Country_Full_Name"] = "UNITED STATES";
                        dr["Security_Typ"] = "Common Stock";
                        dr["Security_Typ2"] = "Common Stock";
                        dt_Securities.Rows.Add(dr);
                    }
                }

                // Now set the dropdown columns
                if (dg_Securities.Columns["crncy"].CellType.ToString().Contains("DataGridViewTextBoxCell"))
                {
                    dg_Securities.Columns.Remove("crncy");
                    DataGridViewComboBoxColumn crncy = new DataGridViewComboBoxColumn();
                    crncy.Name = "crncy";
                    crncy.HeaderText = "crncy";
                    crncy.DataSource = dt_crncy;
                    crncy.DisplayMember = "crncy";
                    crncy.ValueMember = "crncy";
                    crncy.DataPropertyName = "crncy";
                    crncy.AutoComplete = true;
                    crncy.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dg_Securities.Columns.Insert(2, crncy);

                    dg_Securities.Columns.Remove("BBG_Exchange");
                    DataGridViewComboBoxColumn BBG_Exchange = new DataGridViewComboBoxColumn();
                    BBG_Exchange.Name = "BBG_Exchange";
                    BBG_Exchange.HeaderText = "BBG_Exchange";
                    BBG_Exchange.DataSource = dt_BBG_Exchange;
                    BBG_Exchange.DisplayMember = "BBG_Exchange";
                    BBG_Exchange.ValueMember = "BBG_Exchange";
                    BBG_Exchange.DataPropertyName = "BBG_Exchange";
                    BBG_Exchange.AutoComplete = true;
                    BBG_Exchange.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dg_Securities.Columns.Insert(2, BBG_Exchange);

                    dg_Securities.Columns.Remove("Country_Full_Name");
                    DataGridViewComboBoxColumn Country_Full_Name = new DataGridViewComboBoxColumn();
                    Country_Full_Name.Name = "Country_Full_Name";
                    Country_Full_Name.HeaderText = "Country Full Name";
                    Country_Full_Name.DataSource = dt_Country_Full_Name;
                    Country_Full_Name.DisplayMember = "Country_Full_Name";
                    Country_Full_Name.ValueMember = "Country_Full_Name";
                    Country_Full_Name.DataPropertyName = "Country_Full_Name";
                    Country_Full_Name.AutoComplete = true;
                    Country_Full_Name.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dg_Securities.Columns.Add(Country_Full_Name);

                    dg_Securities.Columns.Remove("Security_Typ");
                    DataGridViewComboBoxColumn Security_Typ = new DataGridViewComboBoxColumn();
                    Security_Typ.Name = "Security_Typ";
                    Security_Typ.HeaderText = "Security_Typ";
                    Security_Typ.DataSource = dt_Security_Typ;
                    Security_Typ.DisplayMember = "Security_Typ";
                    Security_Typ.ValueMember = "Security_Typ";
                    Security_Typ.DataPropertyName = "Security_Typ";
                    Security_Typ.AutoComplete = true;
                    Security_Typ.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dg_Securities.Columns.Add(Security_Typ);

                    dg_Securities.Columns.Remove("Security_Typ2");
                    DataGridViewComboBoxColumn Security_Typ2 = new DataGridViewComboBoxColumn();
                    Security_Typ2.Name = "Security_Typ2";
                    Security_Typ2.HeaderText = "Security_Typ2";
                    Security_Typ2.DataSource = dt_Security_Typ2;
                    Security_Typ2.DisplayMember = "Security_Typ2";
                    Security_Typ2.ValueMember = "Security_Typ2";
                    Security_Typ2.DataPropertyName = "Security_Typ2";
                    Security_Typ2.AutoComplete = true;
                    Security_Typ2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dg_Securities.Columns.Add(Security_Typ2);

                    dg_Securities.Columns.Remove("Sector");
                    DataGridViewComboBoxColumn Sector = new DataGridViewComboBoxColumn();
                    Sector.Name = "Sector";
                    Sector.HeaderText = "Sector";
                    Sector.DataSource = dt_Sector;
                    Sector.DisplayMember = "Sector";
                    Sector.ValueMember = "Sector";
                    Sector.DataPropertyName = "Sector";
                    Sector.AutoComplete = true;
                    Sector.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dg_Securities.Columns.Add(Sector);

                    dg_Securities.Columns.Remove("Industry_Group");
                    DataGridViewComboBoxColumn Industry_Group = new DataGridViewComboBoxColumn();
                    Industry_Group.Name = "Industry_Group";
                    Industry_Group.HeaderText = "Industry_Group";
                    Industry_Group.DataSource = dt_Industry_Group;
                    Industry_Group.DisplayMember = "Industry_Group";
                    Industry_Group.ValueMember = "Industry_Group";
                    Industry_Group.DataPropertyName = "Industry_Group";
                    Industry_Group.AutoComplete = true;
                    Industry_Group.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dg_Securities.Columns.Add(Industry_Group);

                    dg_Securities.Columns.Remove("Industry_SubGroup");
                    DataGridViewComboBoxColumn Industry_SubGroup = new DataGridViewComboBoxColumn();
                    Industry_SubGroup.Name = "Industry_SubGroup";
                    Industry_SubGroup.HeaderText = "Industry_SubGroup";
                    Industry_SubGroup.DataSource = dt_Industry_SubGroup;
                    Industry_SubGroup.DisplayMember = "Industry_SubGroup";
                    Industry_SubGroup.ValueMember = "Industry_SubGroup";
                    Industry_SubGroup.DataPropertyName = "Industry_SubGroup";
                    Industry_SubGroup.AutoComplete = true;
                    Industry_SubGroup.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dg_Securities.Columns.Add(Industry_SubGroup);
                }

                SystemLibrary.SetDataGridView(dg_Securities);
            }
            /*catch
            {
                dt_Securities.Rows.Clear();
            }*/

        } //LoadSecurities()

        private void bt_Request_Click(object sender, EventArgs e)
        {
            // Check to make sure rows that have been chamnged have been saved.
            if (SystemLibrary.AreRowsAltered(dt_Securities))
            {
                if(MessageBox.Show("You have altered some of this data.\r\nThis request will loose your changes.\r\n\r\n" +
                                   "Do you wish to continue?","Request Security",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2)==DialogResult.No)
                    return;
            }

            // Now load the ticker.
            LoadSecurities();

        } //bt_Request_Click()

        private void bt_Save_Click(object sender, EventArgs e)
        {
            // Local Variables
            String BBG_Ticker = "";
            String mySql;
            int myRows;


            // Now Save the Changed rows back to the database
            for (int i = 0; i < dt_Securities.Rows.Count; i++)
            {
                switch (dt_Securities.Rows[i].RowState)
                {
                    case DataRowState.Added:
                        FoundChanges = true;
                        BBG_Ticker = SystemLibrary.ToString(dt_Securities.Rows[0]["BBG_Ticker"]);
                        mySql = "Insert into Securities (BBG_Ticker, BBG_Exchange, Security_Name, crncy, CUSIP, ISIN, SEDOL, Last_Price, Pos_Mult_Factor, Round_Lot_Size, Country_Full_Name, " +
                                "                        Security_Typ, Security_Typ2, Sector, Industry_Group, Industry_SubGroup, ID_BB_COMPANY, ID_BB_UNIQUE, ID_BB_GLOBAL, OCC_SYMBOL) values (" +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["BBG_Ticker"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["BBG_Exchange"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["Security_Name"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["crncy"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["CUSIP"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["ISIN"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["SEDOL"]) + "', " +
                                "      " + SystemLibrary.ToString(SystemLibrary.ToDecimal(dt_Securities.Rows[i]["Last_Price"])) + ", " +
                                "      " + SystemLibrary.ToString(SystemLibrary.ToDecimal(dt_Securities.Rows[i]["Pos_Mult_Factor"])) + ", " +
                                "      " + SystemLibrary.ToString(SystemLibrary.ToDecimal(dt_Securities.Rows[i]["Round_Lot_Size"])) + ", " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["Country_Full_Name"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["Security_Typ"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["Security_Typ2"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["Sector"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["Industry_Group"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["Industry_SubGroup"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["ID_BB_COMPANY"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["ID_BB_UNIQUE"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["ID_BB_GLOBAL"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["OCC_SYMBOL"]) + "') ";
                        myRows = SystemLibrary.SQLExecute(mySql);
                        // Make sure any existing Order has the correct values
                        mySql = "Update Orders " +
                                "Set    crncy = Securities.crncy, " +
                                "       Exchange = Securities.BBG_Exchange " +
                                "From   Securities " +
                                "Where  Orders.BBG_Ticker = '" + BBG_Ticker + "' " +
                                "And    isNull(Orders.ProcessedEOD,'N') = 'N' " +
                                "And    Securities.BBG_Ticker = Orders.BBG_Ticker ";
                        myRows = SystemLibrary.SQLExecute(mySql);
                        break;
                    case DataRowState.Deleted:
                        FoundChanges = true;
                        BBG_Ticker = SystemLibrary.ToString(dt_Securities.Rows[0]["BBG_Ticker", DataRowVersion.Original]);
                        mySql = "Delete From Securities " +
                                "Where BBG_Ticker='" + SystemLibrary.ToString(dt_Securities.Rows[i]["BBG_Ticker", DataRowVersion.Original]) + "' ";
                        myRows = SystemLibrary.SQLExecute(mySql);
                        break;
                    case DataRowState.Modified:
                        FoundChanges = true;
                        // Remove old row
                        BBG_Ticker = SystemLibrary.ToString(dt_Securities.Rows[0]["BBG_Ticker"]);
                        mySql = "Delete From Securities " +
                                "Where BBG_Ticker='" + SystemLibrary.ToString(dt_Securities.Rows[i]["BBG_Ticker", DataRowVersion.Original]) + "' ";
                        myRows = SystemLibrary.SQLExecute(mySql);

                        // Add New row
                        mySql = "Insert into Securities (BBG_Ticker, BBG_Exchange, Security_Name, crncy, CUSIP, ISIN, SEDOL, Last_Price, Pos_Mult_Factor, Round_Lot_Size, Country_Full_Name, " +
                                "                        Security_Typ, Security_Typ2, Sector, Industry_Group, Industry_SubGroup, ID_BB_COMPANY, ID_BB_UNIQUE, ID_BB_GLOBAL, OCC_SYMBOL) values (" +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["BBG_Ticker"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["BBG_Exchange"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["Security_Name"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["crncy"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["CUSIP"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["ISIN"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["SEDOL"]) + "', " +
                                "      " + SystemLibrary.ToString(SystemLibrary.ToDecimal(dt_Securities.Rows[i]["Last_Price"])) + ", " +
                                "      " + SystemLibrary.ToString(SystemLibrary.ToDecimal(dt_Securities.Rows[i]["Pos_Mult_Factor"])) + ", " +
                                "      " + SystemLibrary.ToString(SystemLibrary.ToDecimal(dt_Securities.Rows[i]["Round_Lot_Size"])) + ", " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["Country_Full_Name"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["Security_Typ"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["Security_Typ2"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["Sector"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["Industry_Group"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["Industry_SubGroup"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["ID_BB_COMPANY"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["ID_BB_UNIQUE"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["ID_BB_GLOBAL"]) + "', " +
                                "      '" + SystemLibrary.ToString(dt_Securities.Rows[i]["OCC_SYMBOL"]) + "') ";
                        myRows = SystemLibrary.SQLExecute(mySql);

                        // Make sure any existing Order has the correct values
                        mySql = "Update Orders " +
                                "Set    crncy = Securities.crncy, " +
                                "       Exchange = Securities.BBG_Exchange " +
                                "From   Securities " +
                                "Where  Orders.BBG_Ticker = '" + BBG_Ticker + "' " +
                                "And    isNull(Orders.ProcessedEOD,'N') = 'N' " +
                                "And    Securities.BBG_Ticker = Orders.BBG_Ticker ";
                        myRows = SystemLibrary.SQLExecute(mySql);
                        break;
                }
            }
            dt_Securities.AcceptChanges();

            MessageBox.Show("Saved '" + BBG_Ticker + "'", "Maintain Prices");

        } //bt_Save_Click()

        private void MaintainSecurities_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ParentForm1 != null && FoundChanges)
            {
                SystemLibrary.SQLExecute("exec sp_update_Positions");
                ParentForm1.SetUpSecurities_DataTable();
                ParentForm1.LoadActionTab(true);
                ParentForm1.LoadPortfolioIncr(); // If it has changed the underlying tables, then this will pick up new data.
            }

        } //MaintainSecurities_FormClosing()


        private void tb_BBG_Ticker_KeyDown(object sender, KeyEventArgs e)
        {
            SystemLibrary.BBGOnPreviewKeyDown(sender, e.KeyCode.ToString());

        } //tb_BBG_Ticker_KeyDown()

        private void dg_Securities_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        } //dg_Securities_DataError()

        private void dg_Securities_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            LastValue = dg_Securities[e.ColumnIndex, e.RowIndex].Value;

        } //dg_Securities_CellBeginEdit()

        private void dg_Securities_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            switch(dg_Securities.Columns[e.ColumnIndex].Name)
            {
                case "Last_Price":
                case "Pos_Mult_Factor":
                case "Round_Lot_Size":
                    // Validate this is a number
                    Decimal myResult = new Decimal();
                    if (!Decimal.TryParse(dg_Securities.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim(), out myResult))
                        dg_Securities.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                    else
                    {
                        if (myResult < 0)
                        {
                            MessageBox.Show("Cannot have a value less than Zero", dg_Securities.Columns[e.ColumnIndex].Name);
                            dg_Securities.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                        }
                        else
                            dg_Securities.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myResult.ToString(dg_Securities.Columns[e.ColumnIndex].DefaultCellStyle.Format);
                    }
                    break;
            }

        } //dg_Securities_CellEndEdit()


    }
}
