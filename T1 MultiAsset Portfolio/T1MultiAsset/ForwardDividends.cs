using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace T1MultiAsset
{
    public partial class ForwardDividends : Form
    {
        // Public Variables
        public Form1 ParentForm1;
        DataTable dt_Dividends;
        DataTable dt_Prime_Divs;
        DataTable dt_Transactions;
        Object LastValue;
        Hashtable DeletedData = new Hashtable();
        private int CXLocation = 0;
        private int CYLocation = 0;
        private Boolean HasChanged = false;

        public struct DivStruct
        {
            public int ParentFundID;
            public String BBG_Ticker;
            public DateTime RecordDate; // Ex Date
            public DateTime EffectiveDate; // Payable Date
            public Decimal Quantity;
            public Decimal Amount;
        }

        public static DivStruct Div = new DivStruct();


        public ForwardDividends()
        {
            InitializeComponent();
        }

        private void ForwardDividends_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker1_ValueChanged(null, null);
            LoadMissingDivs();
        }

        public void FromParent(Form inForm)
        {
            ParentForm1 = (Form1)inForm;
        } //FromParent()

        private void ForwardDividends_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Let the Parent know this has closed
            if (ParentForm1 != null && HasChanged)
            {
                ParentForm1.LoadActionTab(true);
                ParentForm1.LoadPortfolioIncr(); // If it has changed the underlying tables, then this will pick up new data.
            }

        } //ForwardDividends_FormClosed()

        private void LoadMissingDivs()
        {
            // Local Variables
            String mySql = "Exec sp_LoadMissingDivs";
            
            dt_Prime_Divs = SystemLibrary.SQLSelectToDataTable(mySql);

            tb_TotalAmount.Text = SystemLibrary.ToDecimal(dt_Prime_Divs.Compute("Sum(Amount)","")).ToString("$#,###.00");

            if (dt_Prime_Divs.Rows.Count > 0)
            {
                // Force the Prime file to be reprocessed to see if this clears the problem.
                // As this takes a few seconds, better to call mySQL twice on occasions where there are problems, rather than each time we enter divs screen
                DataRow[] dr = dt_Prime_Divs.Select("CustodianName='Merrill Lynch'");
                if (dr.Length>0)
                    SystemLibrary.SQLExecute("Exec sp_ML_Process_File '', 'ML_E239' ");

                dr = dt_Prime_Divs.Select("CustodianName='SCOTIA'");
                if (dr.Length > 0)
                {
                    SystemLibrary.SQLExecute("Exec sp_Scotia_Process_File '', 'SCOTIA_CASHSTMNT'");
                    SystemLibrary.SQLExecute("Exec sp_Scotia_Process_File '', 'SCOTIA_ACCTHIST'");
                }
                
                dt_Prime_Divs = SystemLibrary.SQLSelectToDataTable(mySql);
            }

            dg_MissingDivs.DataSource = dt_Prime_Divs;
            if (dt_Prime_Divs.Rows.Count == 0)
            {
                splitContainer1.Panel2.Enabled = false;
                splitContainer1.IsSplitterFixed = true;
                lb_MissingDivs.Visible = false;
                dg_MissingDivs.Visible = false;
                bt_Reconcile.Visible = false;
                lb_TotalAmount.Visible = false;
                tb_TotalAmount.Visible = false;
                lb_Amount.Visible = false;
                tb_Amount.Visible = false;
                this.Height = splitContainer1.Top + splitContainer1.Panel1.Height + 50;
                splitContainer1.SplitterDistance = splitContainer1.Height;
                /*
                this.Height = lb_MissingDivs.Top;
                dg_Dividends.Height = this.Height - dg_Dividends.Top - 50;
                */
            }
            else
            {
                for (int i = 0; i < dg_MissingDivs.Columns.Count;i++ )
                {
                    dg_MissingDivs.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                dg_MissingDivs.Columns["FundID"].Visible = false;
                dg_MissingDivs.Columns["Ex Date"].DefaultCellStyle.Format = "dd-MMM-yyyy";
                dg_MissingDivs.Columns["Payable Date"].DefaultCellStyle.Format = "dd-MMM-yyyy";
                dg_MissingDivs.Columns["GPB_Transaction_ID"].Visible = false;
                ParentForm1.SetFormatColumn(dg_MissingDivs, "Quantity", Color.Empty, Color.Empty, "N0", "0");
                ParentForm1.SetFormatColumn(dg_MissingDivs, "Amount", Color.Empty, Color.Empty, "N4", "0");
                dg_MissingDivs.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dg_MissingDivs.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                // Loop over the Rows 
                for (Int32 i = 0; i < dg_MissingDivs.Rows.Count; i++) // Last row in dg_ReportPosition is a blank row
                {
                    if (dg_MissingDivs["BBG_Ticker", i].Value != null)
                    {
                        ParentForm1.SetColumn(dg_MissingDivs, "Quantity", i);
                        ParentForm1.SetColumn(dg_MissingDivs, "Amount", i);
                    }
                }

                dg_MissingDivs.Columns["Reconcilled"].Visible = false;

                if (!dg_MissingDivs.Columns.Contains("IsReconcilled"))
                {
                    System.Windows.Forms.DataGridViewCheckBoxColumn IsReconcilled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
                    IsReconcilled.DataPropertyName = "Reconcilled";
                    IsReconcilled.FalseValue = "N";
                    IsReconcilled.HeaderText = "Reconcilled";
                    IsReconcilled.Name = "IsReconcilled";
                    IsReconcilled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
                    IsReconcilled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
                    IsReconcilled.TrueValue = "Y";
                    IsReconcilled.Width = 67;
                    dg_MissingDivs.Columns.Insert(0,IsReconcilled);
                }
            }

        } //LoadMissingDivs()

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;
            String myDate;

            //dg_Dividends.Rows.Clear();
            dg_Dividends.Update();
            myDate = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
            mySql = "Select	BBG_Ticker, Div_Adjusted as [DPS $], Div_Crncy as Currency, Franking, ExDate as [Ex Date], PayableDate as [Payable Date], ID_BB_UNIQUE, 'N' as Changed, ExDate as [Orig_ExDate], CapitalReturn, WithholdingTaxRate as [Withholding Tax Rate]" +
                    "From	Dividends " +
                    "Where	ExDate >= '" + myDate + "' " +
                    "Order by ExDate, BBG_Ticker ";

            dt_Dividends = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_Dividends.DataSource = dt_Dividends;
            ParentForm1.SetFormatColumn(dg_Dividends, @"DPS $", Color.Empty, Color.LightCyan, "N6", "0");
            ParentForm1.SetFormatColumn(dg_Dividends, "Franking", Color.Empty, Color.Empty, "0.0%", "0");
            ParentForm1.SetFormatColumn(dg_Dividends, "Withholding Tax Rate", Color.Empty, Color.Empty, "0.000%", "0.000%");
            dg_Dividends.Columns["BBG_Ticker"].HeaderText = "Ticker";
            dg_Dividends.Columns["Ex Date"].DefaultCellStyle.Format = "dd-MMM-yy"; // M = dd-Mon, D = Short Date/Time, F = Long Date/Time
            dg_Dividends.Columns["Ex Date"].DefaultCellStyle.ForeColor = Color.Blue;
            dg_Dividends.Columns["Payable Date"].DefaultCellStyle.Format = "dd-MMM-yy"; // M = dd-Mon, D = Short Date/Time, F = Long Date/Time
            dg_Dividends.Columns["CapitalReturn"].Visible = false;

            if (!dg_Dividends.Columns.Contains("IsCapitalReturn"))
            {
                System.Windows.Forms.DataGridViewCheckBoxColumn IsCapitalReturn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
                IsCapitalReturn.DataPropertyName = "CapitalReturn";
                IsCapitalReturn.FalseValue = "N";
                IsCapitalReturn.HeaderText = "IsCapitalReturn";
                IsCapitalReturn.Name = "IsCapitalReturn";
                IsCapitalReturn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
                IsCapitalReturn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
                IsCapitalReturn.TrueValue = "Y";
                IsCapitalReturn.Width = 67;
                dg_Dividends.Columns.Add(IsCapitalReturn);
            }
            dg_Dividends.Columns["DPS $"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dg_Dividends.Columns["Changed"].Visible=false;
            dg_Dividends.Columns["Orig_ExDate"].Visible = false;
            

        } //dateTimePicker1_ValueChanged()

        private void bt_Save_Click(object sender, EventArgs e)
        {
            // Local Variables
            DateTime minDate = SystemLibrary.f_Now().AddYears(1);
            String mySQL;

            if (MessageBox.Show(this, "This [Save] will replace any Fund Transactions && Prime Broker reconcilliations for these dividends.\r\n\r\n" +
                                      "Is this OK?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            else
            {
                HasChanged = true;

                // Delete the Deleted Rows
                foreach (object myKey in DeletedData.Keys)
                {
                    mySQL = DeletedData[myKey].ToString();
                    SystemLibrary.SQLExecute(mySQL);
                }
                DeletedData.Clear();

                // In here Flag the shorts positions having been sent to ASIC.
                for (int i = 0; i < dg_Dividends.Rows.Count-1; i++) // Last Row is not real.
                {
                    if (dg_Dividends["Changed", i].Value.ToString() == "Y" && dg_Dividends["BBG_Ticker", i].Value.ToString()!="")
                    {
                        // Record the minDate
                        DateTime exDate = Convert.ToDateTime(dg_Dividends["Ex Date", i].Value);
                        if (exDate < minDate)
                            minDate = exDate;

                        //Make sure CapitalReturn is set
                        if (SystemLibrary.ToString(dg_Dividends["CapitalReturn", i].Value) == "")
                            dg_Dividends["CapitalReturn", i].Value = "N";

                        //Make sure Withholding Tax Rate is set
                        if (SystemLibrary.ToString(dg_Dividends["Withholding Tax Rate", i].Value) == "")
                            dg_Dividends["Withholding Tax Rate", i].Value = 0;

                        // See if the ExDate has changed
                        if (dg_Dividends.CurrentRow.Cells["Orig_ExDate"].Value != DBNull.Value)
                        {
                            if (Convert.ToDateTime(dg_Dividends.CurrentRow.Cells["Orig_ExDate"].Value) != Convert.ToDateTime(dg_Dividends.CurrentRow.Cells["Ex Date"].Value))
                            {
                                exDate = Convert.ToDateTime(dg_Dividends["Orig_ExDate", i].Value);
                                if (exDate < minDate)
                                    minDate = exDate;
                                // Set old record with a Zero DPS & it will be removed
                                mySQL = "Exec sp_Make_Dividend '" + dg_Dividends["BBG_Ticker", i].Value.ToString() + "', " +
                                                                        "0, " +
                                                                        "'" + dg_Dividends["Currency", i].Value.ToString() + "', " +
                                                                        dg_Dividends["Franking", i].Value.ToString() + ", " +
                                                                        "'" + Convert.ToDateTime(dg_Dividends["Orig_ExDate", i].Value).ToString("dd-MMM-yyyy") + "', " +
                                                                        "'" + Convert.ToDateTime(dg_Dividends["Payable Date", i].Value).ToString("dd-MMM-yyyy") + "', " +
                                                                        "'" + dg_Dividends["ID_BB_UNIQUE", i].Value.ToString() + "', " +
                                                                        "'" + dg_Dividends["CapitalReturn", i].Value.ToString() + "', " +
                                                                        dg_Dividends["Withholding Tax Rate", i].Value.ToString() + " ";
                                SystemLibrary.SQLExecute(mySQL);
                            }
                        }
                        // Save the Data
                        mySQL = "Exec sp_Make_Dividend '" + dg_Dividends["BBG_Ticker", i].Value.ToString() + "', " +
                                                                dg_Dividends[@"DPS $", i].Value.ToString() + ", " +
                                                                "'" + dg_Dividends["Currency", i].Value.ToString() + "', " +
                                                                SystemLibrary.ToDecimal(dg_Dividends["Franking", i].Value).ToString() + ", " +
                                                                "'" + Convert.ToDateTime(dg_Dividends["Ex Date", i].Value).ToString("dd-MMM-yyyy") + "', " +
                                                                "'" + Convert.ToDateTime(dg_Dividends["Payable Date", i].Value).ToString("dd-MMM-yyyy") + "', " +
                                                                "'" + dg_Dividends["ID_BB_UNIQUE", i].Value.ToString() + "', " +
                                                                "'" + dg_Dividends["CapitalReturn", i].Value.ToString() + "', " +
                                                                dg_Dividends["Withholding Tax Rate", i].Value.ToString() + " ";
                        SystemLibrary.SQLExecute(mySQL);
                    }
                }

                // Apply Dividends From minDate
                mySQL = "Exec sp_Apply_Dividends_From '" + minDate.ToString("dd-MMM-yyyy") + "' ";
                SystemLibrary.SQLExecute(mySQL);
                SystemLibrary.SQLExecute("Exec sp_SOD_Positions");

                // Refresh the Data
                dateTimePicker1_ValueChanged(null, null);
                LoadMissingDivs();
            }

            MessageBox.Show("Saved", this.Text);

        } //bt_Save_Click()

        private void dg_Dividends_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            dg_Dividends.CurrentRow.Cells["Changed"].Value = "Y";
        }

        private void dg_Dividends_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (!((System.Data.DataRowView)(e.Row.DataBoundItem)).IsNew)
            {
                String myStr = "Exec sp_Make_Dividend '" + dg_Dividends.CurrentRow.Cells["BBG_Ticker"].Value.ToString() + "',0,null,null,'" + Convert.ToDateTime(dg_Dividends.CurrentRow.Cells["Ex Date"].Value).ToString("dd-MMM-yyyy") + "',null,null,null,null ";
                DeletedData.Add(DeletedData.Count, myStr);
            }
        }


        private void dg_Dividends_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            LastValue = dg_Dividends[e.ColumnIndex, e.RowIndex].Value;
        }

        private void dg_Dividends_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Local Variables
            String  mySql;

            //BBG_Ticker, Div_Adjusted as [DPS $], Div_Crncy as Currency, Franking, ExDate as [Ex Date], PayableDate as [Payable Date], ID_BB_UNIQUE, 'N' as Changed, ExDate as [Orig_ExDate] " +
            dg_Dividends.CurrentRow.Cells["Changed"].Value = "Y";
            switch(dg_Dividends.Columns[e.ColumnIndex].Name)
            {
                case "BBG_Ticker":
                    // Make sure the Ticker is correct case.
                    dg_Dividends.CurrentRow.Cells["BBG_Ticker"].Value = SystemLibrary.BBGGetTicker(dg_Dividends.CurrentRow.Cells["BBG_Ticker"].Value.ToString());
                    // See if I can get the Bloomberg ID from the Database
                    if (dg_Dividends.CurrentRow.Cells["ID_BB_UNIQUE"].Value.ToString() == "")
                    {
                        mySql = "Select ID_BB_UNIQUE from Securities where BBG_Ticker = '" + dg_Dividends.CurrentRow.Cells["BBG_Ticker"].Value.ToString() + "' ";
                        String ID_BB_UNIQUE = SystemLibrary.SQLSelectString(mySql);
                        if (ID_BB_UNIQUE.Length > 0)
                            dg_Dividends.CurrentRow.Cells["ID_BB_UNIQUE"].Value = ID_BB_UNIQUE;
                    }
                    if (dg_Dividends.CurrentRow.Cells["Currency"].Value.ToString() == "")
                    {
                        mySql = "Select crncy from Securities where BBG_Ticker = '" + dg_Dividends.CurrentRow.Cells["BBG_Ticker"].Value.ToString() + "' ";
                        String Currency = SystemLibrary.SQLSelectString(mySql);
                        if (Currency.Length > 0)
                            dg_Dividends.CurrentRow.Cells["Currency"].Value = Currency;
                    }
                    break;
                case "Currency":
                    // Make sure currency is in Upper Case
                    dg_Dividends.CurrentRow.Cells["Currency"].Value = dg_Dividends.CurrentRow.Cells["Currency"].Value.ToString().ToUpper();
                    break;
                case "Ex Date":
                    // Make sure currency is in Upper Case
                    if (dg_Dividends.CurrentRow.Cells["Orig_ExDate"].Value==null)
                        dg_Dividends.CurrentRow.Cells["Orig_ExDate"].Value = Convert.ToDateTime(dg_Dividends.CurrentRow.Cells["Ex Date"].Value);
                    break;
                case "Franking":
                    // Divide by 100 for %
                    dg_Dividends.CurrentRow.Cells["Franking"].Value = SystemLibrary.ToDecimal(dg_Dividends.CurrentRow.Cells["Franking"].Value) / 100m;
                    break;
                case "Withholding Tax Rate":
                    // Divide by 100 for %
                    dg_Dividends.CurrentRow.Cells["Withholding Tax Rate"].Value = SystemLibrary.ToDecimal(dg_Dividends.CurrentRow.Cells["Withholding Tax Rate"].Value) / 100m;
                    break;
                    
            }
        }


        private void dg_Dividends_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Allow Function Keys to work on Bloomberg Tickers

            // Local Variables
            DataGridView dg = (DataGridView)sender;
            if (dg.CurrentCell.OwningColumn.Name.ToUpper().Contains("TICKER"))
            {
                // Allow for the capture of Bloomberg Function Keys while in Cell Edit mode.
                e.Control.PreviewKeyDown -= new PreviewKeyDownEventHandler(SystemLibrary.BBGOnPreviewKeyDown);
                e.Control.PreviewKeyDown += new PreviewKeyDownEventHandler(SystemLibrary.BBGOnPreviewKeyDown);
            }
            else
            {
                e.Control.PreviewKeyDown -= new PreviewKeyDownEventHandler(SystemLibrary.BBGOnPreviewKeyDown);
            }
        } //dg_Dividends_EditingControlShowing()

        private void bt_Reconcile_Click(object sender, EventArgs e)
        {
            String mySql = "";
            HasChanged = true;
            for (int i = 0; i < dg_MissingDivs.Rows.Count; i++)
            {
                if (dg_MissingDivs["Reconcilled", i].Value.ToString() == "Y")
                {
                    String CustodianName = SystemLibrary.ToString(dg_MissingDivs["CustodianName", i].Value);
                    switch (CustodianName)
                    {
                        case "Merrill Lynch":
                            mySql = "Update ML_E239 " +
                                    "Set Reconcilled = 'Y', Reason = 'Manual: " + SystemLibrary.f_Now().ToString("d-MMM-yyyy HH:mm") + "' " +
                                    "Where isNull(Reconcilled,'N') = 'N' " +
                                    "And     GPB_Transaction_ID = '" + dg_MissingDivs["GPB_Transaction_ID", i].Value.ToString() + "' ";
                            SystemLibrary.SQLExecute(mySql);
                            break;
                        case "SCOTIA":
                            mySql = "Update SCOTIA_CASHSTMNT " +
                                    "Set Reconcilled = 'Y', Reason = 'Manual: " + SystemLibrary.f_Now().ToString("d-MMM-yyyy HH:mm") + "' " +
                                    "Where   isNull(Reconcilled,'N') = 'N' " +
                                    "And	 Trans_Type = 'Cash Events' " +
                                    "And     Tckt = '" + dg_MissingDivs["GPB_Transaction_ID", i].Value.ToString() + "' ";
                            SystemLibrary.SQLExecute(mySql);
                            mySql = "Update Scotia_AcctHist " +
                                    "Set Reconcilled = 'Y', Reason = 'Manual: " + SystemLibrary.f_Now().ToString("d-MMM-yyyy HH:mm") + "' " +
                                    "Where   isNull(Reconcilled,'N') = 'N' " +
                                    "And	 Trad_Type = 'Cash Event' " +
                                    "And     ID = '" + dg_MissingDivs["GPB_Transaction_ID", i].Value.ToString() + "' ";
                            SystemLibrary.SQLExecute(mySql);
                            break;
                    }
                }
            }
            LoadMissingDivs();

        } //bt_Reconcile_Click()


        private void dg_Dividends_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            SystemLibrary.DebugLine("dg_Dividends DataError: " + e.RowIndex.ToString() + "," + e.ColumnIndex.ToString());

        } //dg_Dividends_DataError()

        private void dg_Dividends_MouseClick(object sender, MouseEventArgs e)
        {
            CXLocation = dg_MissingDivs.Location.X + e.Location.X + 5;
            CYLocation = dg_MissingDivs.Location.Y + e.Location.Y;

        } //dg_Dividends_MouseClick()

        private void dg_MissingDivs_MouseClick(object sender, MouseEventArgs e)
        {
            CXLocation = dg_MissingDivs.Location.X + e.Location.X + 5;
            CYLocation = dg_MissingDivs.Location.Y + e.Location.Y;

        }  //dg_MissingDivs_MouseClick()

        private void dg_MissingDivs_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Show the Bloomberg popup menu
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (dg_MissingDivs.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.ToString() == "BBG_Ticker")
                    {
                        String Ticker = SystemLibrary.ToString(dg_MissingDivs.Rows[e.RowIndex].Cells["BBG_Ticker"].Value);
                        int FundID = SystemLibrary.ToInt32(dg_MissingDivs.Rows[e.RowIndex].Cells["FundID"].Value);
                        int PortfolioID = -1;
                        String Portfolio_Name = "";

                        if (Ticker.Length > 0)
                        {
                            SystemLibrary.BBGShowMenu(FundID, PortfolioID, Ticker, Portfolio_Name, SystemLibrary.BBGRelativeTicker(Ticker), this.Location.X + CXLocation, this.Location.Y + CYLocation);
                        }
                    }
                }
            }
            catch { }

        } //dg_MissingDivs_CellMouseClick()

        private void dg_Dividends_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Show the Bloomberg popup menu
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (dg_Dividends.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name.ToString() == "BBG_Ticker")
                    {
                        String Ticker = SystemLibrary.ToString(dg_Dividends.Rows[e.RowIndex].Cells["BBG_Ticker"].Value);
                        int FundID = -1;
                        int PortfolioID = -1;
                        String Portfolio_Name = "";

                        if (Ticker.Length > 0)
                        {
                            SystemLibrary.BBGShowMenu(FundID, PortfolioID, Ticker, Portfolio_Name, SystemLibrary.BBGRelativeTicker(Ticker), this.Location.X + CXLocation, this.Location.Y + CYLocation);
                        }
                    }
                }
            }
            catch { }

        } //dg_Dividends_CellMouseClick()

        private void dg_MissingDivs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadTransaction(e.RowIndex);

        } //dg_MissingDivs_CellClick()

        private void dg_MissingDivs_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            // Local Variables
            DateTime ExDate; // SCOTIA can have no ExDate on their record.

            if (dg_MissingDivs["Ex Date", e.RowIndex].Value == DBNull.Value)
                ExDate = DateTime.MaxValue;
            else
                ExDate = Convert.ToDateTime(dg_MissingDivs["Ex Date", e.RowIndex].Value);

            if (Div.BBG_Ticker != null && !(Div.ParentFundID == SystemLibrary.ToInt32(dg_MissingDivs["FundID", e.RowIndex].Value)
                                              && Div.BBG_Ticker == SystemLibrary.ToString(dg_MissingDivs["BBG_Ticker", e.RowIndex].Value)
                                              && Div.RecordDate == ExDate
                                              && Div.EffectiveDate == Convert.ToDateTime(dg_MissingDivs["Payable Date", e.RowIndex].Value)
                                              && Div.Quantity == SystemLibrary.ToDecimal(dg_MissingDivs["Quantity", e.RowIndex].Value)
                                              && Div.Amount == SystemLibrary.ToDecimal(dg_MissingDivs["Amount", e.RowIndex].Value)
                                             )
                )
            {
                LoadTransaction(e.RowIndex);
            }
        } //dg_MissingDivs_RowEnter()

        public void LoadTransaction(int inRow)
        {
            // Local Variables
            String mySql;
            String RecordDateString;


            // Make sure its a valid row
            if (inRow < 0)
                return;

            Div.ParentFundID = SystemLibrary.ToInt32(dg_MissingDivs["FundID", inRow].Value);
            Div.BBG_Ticker = SystemLibrary.ToString(dg_MissingDivs["BBG_Ticker", inRow].Value);
            if (dg_MissingDivs["Ex Date", inRow].Value == DBNull.Value)
            {
                Div.RecordDate = DateTime.MinValue;
                RecordDateString = " ";
            }
            else
            {
                Div.RecordDate = Convert.ToDateTime(dg_MissingDivs["Ex Date", inRow].Value);
                RecordDateString = "And	Transactions.RecordDate = '" + Div.RecordDate.ToString("dd-MMM-yyyy") + "'  ";
            }
            Div.EffectiveDate = Convert.ToDateTime(dg_MissingDivs["Payable Date", inRow].Value);
            Div.Quantity = SystemLibrary.ToDecimal(dg_MissingDivs["Quantity", inRow].Value);
            Div.Amount = SystemLibrary.ToDecimal(dg_MissingDivs["Amount", inRow].Value);

            mySql = "Select Transactions.TranID, Transactions.RecordDate as [Ex Date], Transactions.EffectiveDate as [Payable Date], Transactions.Description, Transactions.Amount, pf.FundName " +
                    "From   Transactions, " +
                    "       Fund, " +
                    "       Fund pf " +
                    "where	Transactions.TranType = 'Dividend' " +
                    "And	Transactions.Description like 'Div: %" + Div.BBG_Ticker + "%'  " +
                    RecordDateString +
                    "And	Transactions.EffectiveDate = '" + Div.EffectiveDate.ToString("dd-MMM-yyyy") + "'  " +
                    "And	Fund.FundID = Transactions.FundID " +
                    "And    pf.FundID = Fund.ParentFundID " +
                    "And    pf.FundID = " + Div.ParentFundID.ToString() + " ";

            dt_Transactions = SystemLibrary.SQLSelectToDataTable(mySql);
            dg_Transactions.DataSource = dt_Transactions;
            SystemLibrary.SetDataGridView(dg_Transactions);

            tb_Amount.Text = SystemLibrary.ToDecimal(dt_Transactions.Compute("Sum(Amount)","")).ToString("$#,###.00");

            for (int i = 0; i < dg_Transactions.Columns.Count; i++)
            {
                if (dg_Transactions.Columns[i].Name != "Amount")
                    dg_Transactions.Columns[i].ReadOnly = true;
            }
            ParentForm1.SetFormatColumn(dg_Transactions, "Amount", Color.Empty, Color.LightCyan, "N2", "0");

        } //LoadTransaction()

        private void bt_Update_Click(object sender, EventArgs e)
        {
            HasChanged = true;
            dg_Transactions.Refresh();
            if (dg_Transactions.Rows.Count == 1)
            {
                if (SystemLibrary.ToDecimal(dg_Transactions["Amount", 0].Value) != Div.Amount)
                {
                    if (MessageBox.Show("Dividend Amount of "+SystemLibrary.ToDecimal(dg_Transactions["Amount", 0].Value).ToString("$#,###.00") + "\r\n" +
                                        "does not match the selected record of " + Div.Amount.ToString("$#,###.00") + "\r\n\r\n" +
                                        "Do you really wish to continue?", "Update", 
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        return;
                }

                // hourglass cursor
                Cursor.Current = Cursors.WaitCursor;

                String mySql = "Update Transactions Set Amount = " + dg_Transactions["Amount", 0].Value.ToString() + " Where TranID = " + dg_Transactions["TranID", 0].Value.ToString();
                SystemLibrary.SQLExecute(mySql);
                SystemLibrary.SQLExecute("Exec sp_ML_Process_File '', 'ML_E239'");
                SystemLibrary.SQLExecute("Exec sp_Scotia_Process_File '', 'SCOTIA_CASHSTMNT'");
                SystemLibrary.SQLExecute("Exec sp_Scotia_Process_File '', 'SCOTIA_ACCTHIST'");
                SystemLibrary.SQLExecute("Exec sp_Calc_Profit_RebuildFrom '" + Div.EffectiveDate.ToString("dd-MMM-yyyy") + "' ");
                SystemLibrary.SQLExecute("Exec sp_SOD_Positions ");
                dt_Transactions.Rows.Clear();
                LoadMissingDivs();

                Cursor.Current = Cursors.Default;
            }
        } //bt_Update_Click()

        
    }
}
