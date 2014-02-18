using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Text.RegularExpressions;

namespace T1MultiAsset
{
    public partial class PortTabLayout : Form
    {
        // The parent code can attach a routine to here, that will be triggered on a sucessful close
        public delegate void Message();
        public event Message OnMessage;

        // Global Variables
        public DataTable dt_Port_Columns;
        public DataTable dt_Port_Tabs;
        public DataTable dt_Port_Tab_Detail;
        private Point screenOffset;
        private String Str_lb_Port_Columns="";
        private String Str_lb_Port_Tab_Detail="";
        private String Str_lb_Tab_Order = "";
        private Boolean lb_Port_Columns_Accept = false;
        private Boolean lb_Tab_Order_Accept = false;
        private Boolean bt_Save_PrevStatus = false;
        private Boolean ChangeMade = false;


        public PortTabLayout()
        {
            InitializeComponent();
            // Load Database parameter
            //SystemLibrary.SQLLoadConnectParams();

        }

        private void PortTabLayout_Load(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;

            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            LoadTabs();

            // Load the List of Available Colummns
            mySql = "Select ColName " +
                    "From   dg_Port_Columns " +
                    "Where  isVisible = 'Y' " +
                    "Order By ColName ";
            dt_Port_Columns = SystemLibrary.SQLSelectToDataTable(mySql);
            lb_Port_Columns.DataSource = dt_Port_Columns;
            lb_Port_Columns.DisplayMember = "ColName";

        } //PortTabLayout_Load()


        //
        // Main Code
        //

        private void LoadTabs()
        {
            // Local Variables
            String mySql;

            cb_Tabs.Items.Clear();

            // Load the List of Tabs
            mySql = "Select TabName, IsSystem, IsTabVisible, FrozenColName, TabOrder, RowFilter " +
                    "From   dg_Port_Tabs " +
                    "Order By TabOrder ";
            dt_Port_Tabs = SystemLibrary.SQLSelectToDataTable(mySql);

            // Cant use DataSource as not all Tabs allowed to be visible to the user.
            if (dt_Port_Tabs.Rows.Count > 0)
            {
                for (int i = 0; i < dt_Port_Tabs.Rows.Count; i++)
                {
                    if (dt_Port_Tabs.Rows[i]["IsTabVisible"].ToString() == "Y")
                        cb_Tabs.Items.Add(dt_Port_Tabs.Rows[i]["TabName"].ToString());
                }
            }
        }
        
        private void cb_Tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Local Variables
            ComboBox cbIn = (ComboBox)(sender);
            String mySql;
            String TabName = cbIn.Text;

            // Clean up the list box
            lb_Port_Tab_Detail.Items.Clear();

            mySql = "Select TabName, ColName, ColOrder  " +
                    "From   dg_Port_Tab_Detail " +
                    "Where TabName = '" + TabName + "' " +
                    "Order By TabName, ColOrder ";
            dt_Port_Tab_Detail = SystemLibrary.SQLSelectToDataTable(mySql);

            if (dt_Port_Tab_Detail.Rows.Count > 0)
                for (int i = 0; i < dt_Port_Tab_Detail.Rows.Count; i++)
                    lb_Port_Tab_Detail.Items.Add(dt_Port_Tab_Detail.Rows[i]["ColName"].ToString());

            // Set the FrozenColumn & RowFilter
            DataRow[] drFrozen = dt_Port_Tabs.Select("TabName='" + TabName + "'");
            if (drFrozen.Length > 0)
            {
                tb_FrozenColumn.Text = drFrozen[0]["FrozenColName"].ToString();
                tb_RowFilter.Text = drFrozen[0]["RowFilter"].ToString();
            }
            else
            {
                tb_FrozenColumn.Text = "";
                tb_RowFilter.Text = "";
            }
            tb_FrozenColumn.Tag = tb_FrozenColumn.Text; // Store the Value in the Tag, so I can rollback when user edits.
            tb_RowFilter.Tag = tb_RowFilter.Text; // Store the Value in the Tag, so I can rollback when user edits.


            // Check to See if this is a System Tab
            bt_Save.Enabled = false;
            bt_Delete.Enabled = false;
            DataRow[] dr = dt_Port_Tabs.Select("TabName = '" + TabName + "' ");
            if (dr.Length > 0)
            {
                if (dr[0]["IsSystem"].ToString() == "N")
                {
                    bt_Save.Enabled = true;
                    bt_Delete.Enabled = true;
                }
            }
        } //cb_Tabs_SelectedIndexChanged()

        private void lb_Port_Tab_Detail_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                String str = (string)e.Data.GetData(DataFormats.StringFormat);
                if (str == Str_lb_Port_Tab_Detail)
                {
                    // Sourced from this Object
                    Point point = lb_Port_Tab_Detail.PointToClient(new Point(e.X, e.Y));
                    int index = lb_Port_Tab_Detail.IndexFromPoint(point);
                    if (index < 0)
                        index = lb_Port_Tab_Detail.Items.Count - 1;
                    //object data = e.Data.GetData(typeof(DateTime));
                    lb_Port_Tab_Detail.Items.Remove(str);
                    lb_Port_Tab_Detail.Items.Insert(index, str);
                }
                else if (str == Str_lb_Port_Columns)
                {
                    // Sourced from the Column List
                    if (!lb_Port_Tab_Detail.Items.Contains(str))
                    {
                        Point point = lb_Port_Tab_Detail.PointToClient(new Point(e.X, e.Y));
                        int index = lb_Port_Tab_Detail.IndexFromPoint(point);
                        if (index < 0)
                            lb_Port_Tab_Detail.Items.Add(str);
                        else
                            lb_Port_Tab_Detail.Items.Insert(index, str);
                    }
                }
            }
        }

        private void lb_Port_Tab_Detail_DragEnter(object sender, DragEventArgs e)
        {
            //bool b = e.Data.GetDataPresent(DataFormats.);
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && (e.AllowedEffect == DragDropEffects.Copy))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.Move;

        }

        private void lb_Port_Tab_Detail_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void lb_Port_Columns_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            // Prevent this being Dragged to another application
            screenOffset = SystemInformation.WorkingArea.Location;

            ListBox lb = sender as ListBox;

            if (lb != null)
            {
                Form f = lb.FindForm();
                // Cancel the drag if the mouse moves off the form. The screenOffset
                // takes into account any desktop bands that may be at the top or left
                // side of the screen.
                if (((Control.MousePosition.X - screenOffset.X) < f.DesktopBounds.Left) ||
                    ((Control.MousePosition.X - screenOffset.X) > f.DesktopBounds.Right) ||
                    ((Control.MousePosition.Y - screenOffset.Y) < f.DesktopBounds.Top) ||
                    ((Control.MousePosition.Y - screenOffset.Y) > f.DesktopBounds.Bottom))
                {
                    e.Action = DragAction.Cancel;
                }
            }

        }

        private void lb_Port_Columns_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && (e.AllowedEffect == DragDropEffects.Copy))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.Move;

        }

        private void lb_Port_Columns_MouseDown(object sender, MouseEventArgs e)
        {
            // Local Variables
            int myItem;

            if (lb_Port_Columns.Items.Count == 0)
                return;
            myItem = lb_Port_Columns.IndexFromPoint(e.X, e.Y);
            if (myItem < 0)
                return;
            Str_lb_Port_Columns = dt_Port_Columns.Rows[myItem]["ColName"].ToString();
            DragDropEffects dde1 = DoDragDrop(Str_lb_Port_Columns, DragDropEffects.Copy);
            // reset after the drag drop
            Str_lb_Port_Columns = "";
        }

        private void lb_Port_Tab_Detail_MouseDown(object sender, MouseEventArgs e)
        {
            // Local Variables
            int myItem;

            if (lb_Port_Tab_Detail.Items.Count == 0)
                return;
            lb_Port_Columns_Accept = false;
            myItem = lb_Port_Tab_Detail.IndexFromPoint(e.X, e.Y);
            if (myItem < 0)
                return;
            Str_lb_Port_Tab_Detail = lb_Port_Tab_Detail.Items[myItem].ToString();
            DragDropEffects dde1 = DoDragDrop(Str_lb_Port_Tab_Detail, DragDropEffects.All);
            
            if ((dde1 == DragDropEffects.Move || dde1 == DragDropEffects.Copy) && lb_Port_Columns_Accept == true)
            {
                lb_Port_Tab_Detail.Items.RemoveAt(lb_Port_Tab_Detail.IndexFromPoint(e.X, e.Y));
                if (tb_FrozenColumn.Text == Str_lb_Port_Tab_Detail)
                {
                    tb_FrozenColumn.Text = "";
                    tb_FrozenColumn.Tag = tb_FrozenColumn.Text;
                }
                lb_Port_Columns_Accept = false;
            }
            // reset after the drag drop
            Str_lb_Port_Tab_Detail = "";
        }


        private void lb_Port_Tab_Detail_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            // Prevent this being Dragged to another application
            screenOffset = SystemInformation.WorkingArea.Location;

            ListBox lb = sender as ListBox;

            if (lb != null)
            {
                Form f = lb.FindForm();
                // Cancel the drag if the mouse moves off the form. The screenOffset
                // takes into account any desktop bands that may be at the top or left
                // side of the screen.
                if (((Control.MousePosition.X - screenOffset.X) < f.DesktopBounds.Left) ||
                    ((Control.MousePosition.X - screenOffset.X) > f.DesktopBounds.Right) ||
                    ((Control.MousePosition.Y - screenOffset.Y) < f.DesktopBounds.Top) ||
                    ((Control.MousePosition.Y - screenOffset.Y) > f.DesktopBounds.Bottom))
                {
                    e.Action = DragAction.Cancel;
                }
            }
        }

        private void lb_Port_Columns_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string str = (string)e.Data.GetData(DataFormats.StringFormat);
                lb_Port_Columns_Accept = true;
                // Do Nothing, but allows the calling code to delete the entry
            }
        }

        private void bt_SaveAs_Click(object sender, EventArgs e)
        {
            // Local Variables
            String TabName = "";
            String mySql;
            String TabOrder;
            String myColumns ="TabName, ColName, ColOrder";
            String myTable = "dg_Port_Tab_Detail";

            
            ChangeMade = true;

            if (SystemLibrary.InputBox(this.Text + " - Save As", "New Portfolio Tab Name", ref TabName, validate_SaveAs, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                // Now Update 
                TabOrder = dt_Port_Tabs.Compute("Max(TabOrder)+1","").ToString();
                mySql = "Update dg_Port_Tabs " +
                        "Set    FrozenColName = '" + tb_FrozenColumn.Text + "', " +
                        "       RowFilter = '" + tb_RowFilter.Text.Replace("'", "''") + "', " +
                        "       TabOrder = " + TabOrder + " " +
                        "Where TabName = '" + TabName + "' ";
                try { SystemLibrary.SQLExecute(mySql); } catch { }
                // Save the Tab Details
                if (lb_Port_Tab_Detail.Items.Count > 0)
                {
                    DataTable dt_Upload = SystemLibrary.SQLBulk_GetDefinition(myColumns, myTable);
                    for (int i = 0; i < lb_Port_Tab_Detail.Items.Count; i++)
                    {
                        DataRow dr = dt_Upload.NewRow();
                        dr["TabName"] = TabName;
                        dr["ColName"] = lb_Port_Tab_Detail.Items[i].ToString();
                        dr["ColOrder"] = i;
                        dt_Upload.Rows.Add(dr);
                    }
                    try { SystemLibrary.SQLBulkUpdate(dt_Upload, myColumns, myTable); }
                    catch { }
                }
                LoadTabs();
                cb_Tabs.Text = TabName;
                MessageBox.Show("'"+TabName+"' Saved", this.Text);
            }
        } //bt_SaveAs_Click()

        
        SystemLibrary.InputBoxValidation validate_SaveAs = delegate(String TabName)
        {
            String mySql;
            mySql = "Select TabName " +
                    "From   dg_Port_Tabs " +
                    "Where TabName = '"+TabName+"' " +
                    "Order By TabOrder ";
            mySql = "Insert into dg_Port_Tabs (TabName, IsSystem, IsTabVisible, FrozenColName, TabOrder) " +
                    "Select '" + TabName + "', 'N', 'Y', '', 99 " +
                    "Where not exists (Select 'x' from dg_Port_Tabs Where TabName = '" + TabName + "') ";
            if (SystemLibrary.SQLExecute(mySql) < 1)
                return "'"+TabName + "' already exists.\r\n\r\nPlease choose another name.";
            else
                return "";
        }; //validate_SaveAs()

        private void bt_Save_Click(object sender, EventArgs e)
        {
            // Local Variables
            String TabName = cb_Tabs.Text;
            String mySql;
            String myColumns = "TabName, ColName, ColOrder";
            String myTable = "dg_Port_Tab_Detail";

            ChangeMade = true;
            if (lb_Tab_Order.Visible)
            {
                // In TabOrder Mode
                // - Loop down items and Update the database table
                if (lb_Tab_Order.Items.Count > 0)
                {
                    for (int i = 0; i < lb_Tab_Order.Items.Count; i++)
                    {
                        TabName = lb_Tab_Order.Items[i].ToString();
                        if (TabName.Length > 0)
                        {
                            mySql = "Update dg_Port_Tabs " +
                                    "Set    TabOrder = " + i.ToString() + " " +
                                    "Where  TabName = '" + TabName + "' ";
                            try { SystemLibrary.SQLExecute(mySql); }
                            catch { }
                        }
                    }
                }
                MessageBox.Show("Tab Order Saved", this.Text);
                bt_cancel_Click(null, null);
            }
            else
            {
                // Save the Frozen Column
                mySql = "Update dg_Port_Tabs " +
                        "Set    FrozenColName = '" + tb_FrozenColumn.Text + "', " +
                        "       RowFilter = '" + tb_RowFilter.Text.Replace("'","''") + "' " +
                        "Where TabName = '" + TabName + "' ";
                try { SystemLibrary.SQLExecute(mySql); } catch { }

                // Remove the Old records
                mySql = "Delete from dg_Port_Tab_Detail " +
                        "Where TabName = '" + TabName + "' ";
                try { SystemLibrary.SQLExecute(mySql); }
                catch { }
                // Save the Tab Details
                if (lb_Port_Tab_Detail.Items.Count > 0)
                {
                    DataTable dt_Upload = SystemLibrary.SQLBulk_GetDefinition(myColumns, myTable);
                    for (int i = 0; i < lb_Port_Tab_Detail.Items.Count; i++)
                    {
                        DataRow dr = dt_Upload.NewRow();
                        dr["TabName"] = TabName;
                        dr["ColName"] = lb_Port_Tab_Detail.Items[i].ToString();
                        dr["ColOrder"] = i;
                        dt_Upload.Rows.Add(dr);
                    }
                    try { SystemLibrary.SQLBulkUpdate(dt_Upload, myColumns, myTable); }
                    catch { }
                }
                LoadTabs();
                cb_Tabs.Text = TabName;
                MessageBox.Show("'" + TabName + "' Saved", this.Text);
            }

        } //bt_Save_Click()

        private void bt_Delete_Click(object sender, EventArgs e)
        {
            // Local Variables
            String mySql;
            String TabName = cb_Tabs.Text;

            ChangeMade = true;
            if (TabName.Length > 0)
            {
                if (MessageBox.Show(this, "Do you really want to Delete '" + TabName + "' Tab?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Remove the Detail records
                    mySql = "Delete from dg_Port_Tab_Detail " +
                            "Where TabName = '" + TabName + "' ";
                    try { SystemLibrary.SQLExecute(mySql); }
                    catch { }
                    // Remove the Header record
                    mySql = "Delete from dg_Port_Tabs " +
                            "Where TabName = '" + TabName + "' ";
                    try { SystemLibrary.SQLExecute(mySql); }
                    catch { }
                    LoadTabs();
                    cb_Tabs.Text = "";
                    MessageBox.Show("'" + TabName + "' Deleted", this.Text);
                }
            }

        } //bt_Delete_Click()

        private void tb_FrozenColumn_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && (e.AllowedEffect == DragDropEffects.Copy))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.Move;

        } //tb_FrozenColumn_DragEnter()

        private void tb_FrozenColumn_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                String str = (string)e.Data.GetData(DataFormats.StringFormat);
                if (str == Str_lb_Port_Tab_Detail)
                {
                    // Sourced from the Tab Detail Lits
                    tb_FrozenColumn.Text = str;
                    tb_FrozenColumn.Tag = tb_FrozenColumn.Text;
                }
            }

        } //tb_FrozenColumn_DragDrop()


        private void tb_FrozenColumn_Validating(object sender, CancelEventArgs e)
        {
            SystemLibrary.DebugLine("cc");

            if (!lb_Port_Tab_Detail.Items.Contains(tb_FrozenColumn.Text))
            {
                MessageBox.Show("Invalid Text '" + tb_FrozenColumn.Text + "'\r\n\r\nPlease Drag & Drop from the 'Display Columns'.", lb_FrozenColumn.Text);
                tb_FrozenColumn.Text = tb_FrozenColumn.Tag.ToString();
            }
        } //tb_FrozenColumn_TextChanged()

        private void bt_SetTabOrder_Click(object sender, EventArgs e)
        {
            // Hide & Show boxes for this feature
            this.Width = 262;
            bt_cancel.Left = this.Width - 100;
            lb_Port_Tab_Detail.Visible = false;
            cb_Tabs.Visible = false;
            lb_Port_Columns.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            bt_SaveAs.Visible = false;
            tb_FrozenColumn.Visible = false;
            lb_FrozenColumn.Visible = false;
            tb_RowFilter.Visible = false;
            lb_RowFilter.Visible = false;
            bt_Delete.Visible = false;
            label5.Visible = false;
            bt_SetTabOrder.Visible = false;
            //Show the 
            lb_Tab_Order.Visible = true;
            lb_Tab_Order.Top = lb_TabName.Top + lb_TabName.Height + 30;
            lb_Tab_Order.Left = lb_Port_Tab_Detail.Left;
            lb_Tab_Order.Width = lb_Port_Tab_Detail.Width;
            lb_Tab_Order.Height = lb_Port_Tab_Detail.Height;
            label_Tab_Order_Desc.Visible = true;
            label_Tab_Order_Desc.Top = lb_Tab_Order.Top + lb_Tab_Order.Height+20;
            label_Tab_Order_Desc.Left = lb_Tab_Order.Left;
            bt_Save_PrevStatus = bt_Save.Enabled;
            bt_Save.Enabled = true;

            // Load the Tabs
            lb_Tab_Order.Items.Clear();
            if (dt_Port_Tabs.Rows.Count > 0)
                for (int i = 0; i < dt_Port_Tabs.Rows.Count; i++)
                    lb_Tab_Order.Items.Add(dt_Port_Tabs.Rows[i]["TabName"].ToString());


        } //bt_SetTabOrder_Click()

        private void bt_cancel_Click(object sender, EventArgs e)
        {
            if (lb_Tab_Order.Visible)
            {
                this.Width = 510;
                bt_cancel.Left = this.Width - 100;
                // This is a Close on Changing Tab Order
                lb_Port_Tab_Detail.Visible = true;
                cb_Tabs.Visible = true;
                lb_Port_Columns.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                bt_SaveAs.Visible = true;
                tb_FrozenColumn.Visible = true;
                lb_FrozenColumn.Visible = true;
                tb_RowFilter.Visible = true;
                lb_RowFilter.Visible = true;
                bt_Delete.Visible = true;
                label5.Visible = true;
                bt_SetTabOrder.Visible = true;
                //Restore the form
                lb_Tab_Order.Visible = false;
                label_Tab_Order_Desc.Visible = false;
                bt_Save.Enabled = bt_Save_PrevStatus;
            }
            else
                this.Close();

        } //bt_cancel_Click()

        private void lb_Tab_Order_MouseDown(object sender, MouseEventArgs e)
        {
            // Local Variables
            int myItem;

            if (lb_Tab_Order.Items.Count == 0)
                return;
            lb_Tab_Order_Accept = false;
            myItem = lb_Tab_Order.IndexFromPoint(e.X, e.Y);
            if (myItem < 0)
                return;
            Str_lb_Tab_Order = lb_Tab_Order.Items[myItem].ToString();
            DragDropEffects dde1 = DoDragDrop(Str_lb_Tab_Order, DragDropEffects.All);

            if ((dde1 == DragDropEffects.Move || dde1 == DragDropEffects.Copy) && lb_Tab_Order_Accept == true)
            {
                lb_Tab_Order.Items.RemoveAt(lb_Tab_Order.IndexFromPoint(e.X, e.Y));
                lb_Tab_Order_Accept = false;
            }
            // reset after the drag drop
            Str_lb_Tab_Order = "";

        } //lb_Tab_Order_MouseDown()

        private void lb_Tab_Order_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && (e.AllowedEffect == DragDropEffects.Copy))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.Move;

        } //lb_Tab_Order_DragEnter()

        private void lb_Tab_Order_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                String str = (string)e.Data.GetData(DataFormats.StringFormat);
                if (str == Str_lb_Tab_Order)
                {
                    // Sourced from this Object
                    Point point = lb_Tab_Order.PointToClient(new Point(e.X, e.Y));
                    int index = lb_Tab_Order.IndexFromPoint(point);
                    if (index < 0)
                        index = lb_Tab_Order.Items.Count - 1;
                    //object data = e.Data.GetData(typeof(DateTime));
                    lb_Tab_Order.Items.Remove(str);
                    lb_Tab_Order.Items.Insert(index, str);
                }
                else if (str == Str_lb_Port_Columns)
                {
                    // Sourced from the Column List
                    if (!lb_Tab_Order.Items.Contains(str))
                        lb_Tab_Order.Items.Add(str);
                }
            }
        } //lb_Tab_Order_DragDrop()

        private void PortTabLayout_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ChangeMade)
                this.OnMessage(); // Let the parent code know a Save has occured.
        } //PortTabLayout_FormClosing()

        
    }
}
