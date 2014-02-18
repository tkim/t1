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
    public partial class ImportData : Form
    {
        // Global Variables
        public Form ParentForm1;
        public String ImportedTableName = "";
        public DataTable dt_ImportData;
        
        public ImportData()
        {
            InitializeComponent();
        }

        private void ImportData_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); 

        }

        public void FromParent(Form inForm)
        {
            ParentForm1 = inForm;

        } //FromParent()

        private void ImportData_Shown(object sender, EventArgs e)
        {
            if (ParentForm1 != null)
            {
                SystemLibrary.PositionFormOverParent(this, ParentForm1);
            }

        } //ImportData_Shown()

        private void ImportData_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Refresh the ActionsTab
            if (ParentForm1 != null)
            {
                if (ParentForm1.Name == "Form1")
                {
                    Form1 f = (Form1)ParentForm1;
                    f.LoadActionTab(true);
                }
            }
        }

        private void bt_LoadFile_Click(object sender, EventArgs e)
        {


            // If more than 1 row, then Save to Database
            if (dg_ImportData.Rows.Count > 1)
                bt_Save.Enabled = true;

        }

        private void bt_LoadDefinition_Click(object sender, EventArgs e)
        {
            // Local Variables

            if (tb_TableName.Text.Trim().Length > 0)
            {
                ImportedTableName = tb_TableName.Text.Trim();
                dt_ImportData = SystemLibrary.SQLBulk_GetDefinition("", ImportedTableName);
                dg_ImportData.DataSource = dt_ImportData;
            }

        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            // Local Variables
            Int32 myRows = 0;

            // Warn the user that this will overright, or ask overright or append?
            DialogResult dr1 = MessageBox.Show("Choose 'Yes' to Overright any Data that may exist\r\n" +
                                               "Choose 'No' to Append to existing Data\r\n" +
                                               "Choose 'Cancel' to Abort saving Data",
                                               "Save to " + ImportedTableName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1
                                               );

            if (dr1 == DialogResult.Cancel)
            {
                return;
            }
            else if (dr1 == DialogResult.Yes)
            {
                // Overright existing data (ie.Delete existing data first).
                SystemLibrary.SQLExecute("Delete from [" + ImportedTableName + "]");
            }



            // Save back to the database
            try
            {
                myRows = SystemLibrary.SQLBulkUpdate(dt_ImportData, "", ImportedTableName);
            }
            catch
            {
                myRows = 0;
            }

            MessageBox.Show("Loaded " + myRows.ToString() + " rows.", ImportedTableName);

        }

        private void dg_ImportData_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            // If more than 1 row, then Save to Database
            if (dg_ImportData.Rows.Count > 1)
                bt_Save.Enabled = true;

        } //dg_ImportData_UserAddedRow()

        private void dg_ImportData_KeyUp(object sender, KeyEventArgs e)
        {
            // Local Variables

            // Deal with Ctrl-V            
            if (e.Control == true && e.KeyValue == (int)Keys.V)
            {
                if (dt_ImportData != null && dt_ImportData.Columns.Count > 0)
                {
                    // Takes a Tab seperated or CR/LF seperated set of data
                    IDataObject inClipboard = Clipboard.GetDataObject();

                    DataTable dt_in = SystemLibrary.ProcesInput(inClipboard);
                    String myMsg = SystemLibrary.CopyDataFromDataTable(dt_in, ref dt_ImportData);
                    if (myMsg.Length > 0)
                        MessageBox.Show(myMsg, "Paste");
                }
            }

            // If more than 1 row, then Save to Database
            if (dg_ImportData.Rows.Count > 1)
                bt_Save.Enabled = true;

        } //dg_ImportData_KeyUp()
    
    }
}
