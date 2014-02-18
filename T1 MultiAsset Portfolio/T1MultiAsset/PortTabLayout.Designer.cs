namespace T1MultiAsset
{
    partial class PortTabLayout
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
            this.lb_Port_Tab_Detail = new System.Windows.Forms.ListBox();
            this.cb_Tabs = new System.Windows.Forms.ComboBox();
            this.bt_Save = new System.Windows.Forms.Button();
            this.lb_Port_Columns = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lb_TabName = new System.Windows.Forms.Label();
            this.bt_SaveAs = new System.Windows.Forms.Button();
            this.bt_cancel = new System.Windows.Forms.Button();
            this.tb_FrozenColumn = new System.Windows.Forms.TextBox();
            this.lb_FrozenColumn = new System.Windows.Forms.Label();
            this.bt_Delete = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.bt_SetTabOrder = new System.Windows.Forms.Button();
            this.lb_Tab_Order = new System.Windows.Forms.ListBox();
            this.label_Tab_Order_Desc = new System.Windows.Forms.Label();
            this.lb_RowFilter = new System.Windows.Forms.Label();
            this.tb_RowFilter = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lb_Port_Tab_Detail
            // 
            this.lb_Port_Tab_Detail.AllowDrop = true;
            this.lb_Port_Tab_Detail.FormattingEnabled = true;
            this.lb_Port_Tab_Detail.Location = new System.Drawing.Point(23, 100);
            this.lb_Port_Tab_Detail.Name = "lb_Port_Tab_Detail";
            this.lb_Port_Tab_Detail.Size = new System.Drawing.Size(219, 225);
            this.lb_Port_Tab_Detail.TabIndex = 0;
            this.lb_Port_Tab_Detail.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.lb_Port_Tab_Detail_QueryContinueDrag);
            this.lb_Port_Tab_Detail.DragOver += new System.Windows.Forms.DragEventHandler(this.lb_Port_Tab_Detail_DragOver);
            this.lb_Port_Tab_Detail.DragDrop += new System.Windows.Forms.DragEventHandler(this.lb_Port_Tab_Detail_DragDrop);
            this.lb_Port_Tab_Detail.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lb_Port_Tab_Detail_MouseDown);
            this.lb_Port_Tab_Detail.DragEnter += new System.Windows.Forms.DragEventHandler(this.lb_Port_Tab_Detail_DragEnter);
            // 
            // cb_Tabs
            // 
            this.cb_Tabs.FormattingEnabled = true;
            this.cb_Tabs.Location = new System.Drawing.Point(23, 26);
            this.cb_Tabs.Name = "cb_Tabs";
            this.cb_Tabs.Size = new System.Drawing.Size(217, 21);
            this.cb_Tabs.TabIndex = 1;
            this.cb_Tabs.SelectedIndexChanged += new System.EventHandler(this.cb_Tabs_SelectedIndexChanged);
            // 
            // bt_Save
            // 
            this.bt_Save.Enabled = false;
            this.bt_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Save.ForeColor = System.Drawing.Color.Navy;
            this.bt_Save.Location = new System.Drawing.Point(23, 505);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(82, 29);
            this.bt_Save.TabIndex = 2;
            this.bt_Save.Text = "&Save";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // lb_Port_Columns
            // 
            this.lb_Port_Columns.AllowDrop = true;
            this.lb_Port_Columns.Location = new System.Drawing.Point(288, 98);
            this.lb_Port_Columns.Name = "lb_Port_Columns";
            this.lb_Port_Columns.Size = new System.Drawing.Size(200, 225);
            this.lb_Port_Columns.TabIndex = 3;
            this.lb_Port_Columns.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.lb_Port_Columns_QueryContinueDrag);
            this.lb_Port_Columns.DragDrop += new System.Windows.Forms.DragEventHandler(this.lb_Port_Columns_DragDrop);
            this.lb_Port_Columns.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lb_Port_Columns_MouseDown);
            this.lb_Port_Columns.DragEnter += new System.Windows.Forms.DragEventHandler(this.lb_Port_Columns_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkGreen;
            this.label1.Location = new System.Drawing.Point(285, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Available Columns";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DarkGreen;
            this.label2.Location = new System.Drawing.Point(21, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Displayed Columns";
            // 
            // lb_TabName
            // 
            this.lb_TabName.AutoSize = true;
            this.lb_TabName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_TabName.ForeColor = System.Drawing.Color.DarkGreen;
            this.lb_TabName.Location = new System.Drawing.Point(21, 9);
            this.lb_TabName.Name = "lb_TabName";
            this.lb_TabName.Size = new System.Drawing.Size(65, 13);
            this.lb_TabName.TabIndex = 6;
            this.lb_TabName.Text = "Tab Name";
            // 
            // bt_SaveAs
            // 
            this.bt_SaveAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_SaveAs.ForeColor = System.Drawing.Color.Navy;
            this.bt_SaveAs.Location = new System.Drawing.Point(120, 505);
            this.bt_SaveAs.Name = "bt_SaveAs";
            this.bt_SaveAs.Size = new System.Drawing.Size(82, 29);
            this.bt_SaveAs.TabIndex = 7;
            this.bt_SaveAs.Text = "Save &As";
            this.bt_SaveAs.UseVisualStyleBackColor = true;
            this.bt_SaveAs.Click += new System.EventHandler(this.bt_SaveAs_Click);
            // 
            // bt_cancel
            // 
            this.bt_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_cancel.ForeColor = System.Drawing.Color.Navy;
            this.bt_cancel.Location = new System.Drawing.Point(406, 505);
            this.bt_cancel.Name = "bt_cancel";
            this.bt_cancel.Size = new System.Drawing.Size(82, 29);
            this.bt_cancel.TabIndex = 8;
            this.bt_cancel.Text = "&Close";
            this.bt_cancel.UseVisualStyleBackColor = true;
            this.bt_cancel.Click += new System.EventHandler(this.bt_cancel_Click);
            // 
            // tb_FrozenColumn
            // 
            this.tb_FrozenColumn.AllowDrop = true;
            this.tb_FrozenColumn.Location = new System.Drawing.Point(23, 349);
            this.tb_FrozenColumn.Name = "tb_FrozenColumn";
            this.tb_FrozenColumn.Size = new System.Drawing.Size(215, 20);
            this.tb_FrozenColumn.TabIndex = 9;
            this.tb_FrozenColumn.DragDrop += new System.Windows.Forms.DragEventHandler(this.tb_FrozenColumn_DragDrop);
            this.tb_FrozenColumn.Validating += new System.ComponentModel.CancelEventHandler(this.tb_FrozenColumn_Validating);
            this.tb_FrozenColumn.DragEnter += new System.Windows.Forms.DragEventHandler(this.tb_FrozenColumn_DragEnter);
            // 
            // lb_FrozenColumn
            // 
            this.lb_FrozenColumn.AutoSize = true;
            this.lb_FrozenColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_FrozenColumn.ForeColor = System.Drawing.Color.DarkGreen;
            this.lb_FrozenColumn.Location = new System.Drawing.Point(21, 333);
            this.lb_FrozenColumn.Name = "lb_FrozenColumn";
            this.lb_FrozenColumn.Size = new System.Drawing.Size(116, 13);
            this.lb_FrozenColumn.TabIndex = 10;
            this.lb_FrozenColumn.Text = "Scroll Lock Column";
            // 
            // bt_Delete
            // 
            this.bt_Delete.Enabled = false;
            this.bt_Delete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Delete.ForeColor = System.Drawing.Color.Navy;
            this.bt_Delete.Location = new System.Drawing.Point(208, 505);
            this.bt_Delete.Name = "bt_Delete";
            this.bt_Delete.Size = new System.Drawing.Size(82, 29);
            this.bt_Delete.TabIndex = 11;
            this.bt_Delete.Text = "&Delete";
            this.bt_Delete.UseVisualStyleBackColor = true;
            this.bt_Delete.Click += new System.EventHandler(this.bt_Delete_Click);
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(275, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(212, 61);
            this.label5.TabIndex = 12;
            this.label5.Text = "Drag && Drop between \'Available Columns\', \'Display Columns\', and the \'Scroll Lock" +
                " Column\'.";
            // 
            // bt_SetTabOrder
            // 
            this.bt_SetTabOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_SetTabOrder.ForeColor = System.Drawing.Color.Green;
            this.bt_SetTabOrder.Location = new System.Drawing.Point(151, 2);
            this.bt_SetTabOrder.Name = "bt_SetTabOrder";
            this.bt_SetTabOrder.Size = new System.Drawing.Size(87, 20);
            this.bt_SetTabOrder.TabIndex = 13;
            this.bt_SetTabOrder.Text = "Set Tab Order";
            this.bt_SetTabOrder.UseVisualStyleBackColor = true;
            this.bt_SetTabOrder.Click += new System.EventHandler(this.bt_SetTabOrder_Click);
            // 
            // lb_Tab_Order
            // 
            this.lb_Tab_Order.AllowDrop = true;
            this.lb_Tab_Order.FormattingEnabled = true;
            this.lb_Tab_Order.Location = new System.Drawing.Point(509, 100);
            this.lb_Tab_Order.Name = "lb_Tab_Order";
            this.lb_Tab_Order.Size = new System.Drawing.Size(219, 225);
            this.lb_Tab_Order.TabIndex = 14;
            this.lb_Tab_Order.Visible = false;
            this.lb_Tab_Order.DragDrop += new System.Windows.Forms.DragEventHandler(this.lb_Tab_Order_DragDrop);
            this.lb_Tab_Order.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lb_Tab_Order_MouseDown);
            this.lb_Tab_Order.DragEnter += new System.Windows.Forms.DragEventHandler(this.lb_Tab_Order_DragEnter);
            // 
            // label_Tab_Order_Desc
            // 
            this.label_Tab_Order_Desc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_Tab_Order_Desc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Tab_Order_Desc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Tab_Order_Desc.ForeColor = System.Drawing.Color.DarkGreen;
            this.label_Tab_Order_Desc.Location = new System.Drawing.Point(509, 11);
            this.label_Tab_Order_Desc.Name = "label_Tab_Order_Desc";
            this.label_Tab_Order_Desc.Size = new System.Drawing.Size(212, 61);
            this.label_Tab_Order_Desc.TabIndex = 15;
            this.label_Tab_Order_Desc.Text = "Drag && Drop Columns in the Order you need them. Press [Save] when done.";
            this.label_Tab_Order_Desc.Visible = false;
            // 
            // lb_RowFilter
            // 
            this.lb_RowFilter.AutoSize = true;
            this.lb_RowFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_RowFilter.ForeColor = System.Drawing.Color.DarkGreen;
            this.lb_RowFilter.Location = new System.Drawing.Point(21, 377);
            this.lb_RowFilter.Name = "lb_RowFilter";
            this.lb_RowFilter.Size = new System.Drawing.Size(91, 13);
            this.lb_RowFilter.TabIndex = 17;
            this.lb_RowFilter.Text = "Filter Rows by:";
            // 
            // tb_RowFilter
            // 
            this.tb_RowFilter.AllowDrop = true;
            this.tb_RowFilter.Location = new System.Drawing.Point(23, 394);
            this.tb_RowFilter.Multiline = true;
            this.tb_RowFilter.Name = "tb_RowFilter";
            this.tb_RowFilter.Size = new System.Drawing.Size(267, 105);
            this.tb_RowFilter.TabIndex = 16;
            // 
            // PortTabLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bt_cancel;
            this.ClientSize = new System.Drawing.Size(498, 541);
            this.Controls.Add(this.lb_RowFilter);
            this.Controls.Add(this.tb_RowFilter);
            this.Controls.Add(this.label_Tab_Order_Desc);
            this.Controls.Add(this.lb_Tab_Order);
            this.Controls.Add(this.bt_SetTabOrder);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bt_Delete);
            this.Controls.Add(this.lb_FrozenColumn);
            this.Controls.Add(this.tb_FrozenColumn);
            this.Controls.Add(this.bt_cancel);
            this.Controls.Add(this.bt_SaveAs);
            this.Controls.Add(this.lb_TabName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lb_Port_Columns);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.cb_Tabs);
            this.Controls.Add(this.lb_Port_Tab_Detail);
            this.Name = "PortTabLayout";
            this.Text = "Design Portfolio Tab Layout";
            this.Load += new System.EventHandler(this.PortTabLayout_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PortTabLayout_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lb_Port_Tab_Detail;
        private System.Windows.Forms.ComboBox cb_Tabs;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.ListBox lb_Port_Columns;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb_TabName;
        private System.Windows.Forms.Button bt_SaveAs;
        private System.Windows.Forms.Button bt_cancel;
        private System.Windows.Forms.TextBox tb_FrozenColumn;
        private System.Windows.Forms.Label lb_FrozenColumn;
        private System.Windows.Forms.Button bt_Delete;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button bt_SetTabOrder;
        private System.Windows.Forms.ListBox lb_Tab_Order;
        private System.Windows.Forms.Label label_Tab_Order_Desc;
        private System.Windows.Forms.Label lb_RowFilter;
        private System.Windows.Forms.TextBox tb_RowFilter;
    }
}