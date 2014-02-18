namespace T1MultiAsset
{
    partial class ShowFillStatus
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowFillStatus));
            this.bt_Refresh = new System.Windows.Forms.Button();
            this.bt_Close = new System.Windows.Forms.Button();
            this.bt_EMSX = new System.Windows.Forms.Button();
            this.bt_Print = new System.Windows.Forms.Button();
            this.dgv_ShowFills = new System.Windows.Forms.DataGridView();
            this.dgv_Summary = new System.Windows.Forms.DataGridView();
            this.Profit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fill_Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fill_Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty_Working = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty_Routed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Order_Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Side = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ShowFills)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Summary)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_Refresh
            // 
            this.bt_Refresh.Location = new System.Drawing.Point(12, 12);
            this.bt_Refresh.Name = "bt_Refresh";
            this.bt_Refresh.Size = new System.Drawing.Size(75, 23);
            this.bt_Refresh.TabIndex = 2;
            this.bt_Refresh.Text = "Refresh";
            this.bt_Refresh.UseVisualStyleBackColor = true;
            this.bt_Refresh.Click += new System.EventHandler(this.bt_Refresh_Click);
            // 
            // bt_Close
            // 
            this.bt_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_Close.Location = new System.Drawing.Point(997, 12);
            this.bt_Close.Name = "bt_Close";
            this.bt_Close.Size = new System.Drawing.Size(75, 23);
            this.bt_Close.TabIndex = 3;
            this.bt_Close.Text = "&Close";
            this.bt_Close.UseVisualStyleBackColor = true;
            this.bt_Close.Click += new System.EventHandler(this.bt_Close_Click);
            // 
            // bt_EMSX
            // 
            this.bt_EMSX.BackColor = System.Drawing.Color.MediumOrchid;
            this.bt_EMSX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_EMSX.ForeColor = System.Drawing.Color.Black;
            this.bt_EMSX.Location = new System.Drawing.Point(93, 12);
            this.bt_EMSX.Name = "bt_EMSX";
            this.bt_EMSX.Size = new System.Drawing.Size(58, 23);
            this.bt_EMSX.TabIndex = 24;
            this.bt_EMSX.Text = "&EMSX";
            this.bt_EMSX.UseVisualStyleBackColor = false;
            this.bt_EMSX.Click += new System.EventHandler(this.bt_EMSX_Click);
            // 
            // bt_Print
            // 
            this.bt_Print.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Print.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bt_Print.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Print.ForeColor = System.Drawing.Color.DarkGreen;
            this.bt_Print.Location = new System.Drawing.Point(915, 12);
            this.bt_Print.Margin = new System.Windows.Forms.Padding(0);
            this.bt_Print.Name = "bt_Print";
            this.bt_Print.Size = new System.Drawing.Size(75, 23);
            this.bt_Print.TabIndex = 25;
            this.bt_Print.Text = "Print";
            this.bt_Print.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bt_Print.UseVisualStyleBackColor = true;
            this.bt_Print.Click += new System.EventHandler(this.bt_Print_Click);
            // 
            // dgv_ShowFills
            // 
            this.dgv_ShowFills.AllowUserToAddRows = false;
            this.dgv_ShowFills.AllowUserToDeleteRows = false;
            this.dgv_ShowFills.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_ShowFills.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgv_ShowFills.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_ShowFills.Location = new System.Drawing.Point(12, 191);
            this.dgv_ShowFills.Name = "dgv_ShowFills";
            this.dgv_ShowFills.ReadOnly = true;
            this.dgv_ShowFills.RowHeadersVisible = false;
            this.dgv_ShowFills.Size = new System.Drawing.Size(1060, 187);
            this.dgv_ShowFills.TabIndex = 1;
            this.dgv_ShowFills.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_ShowFills_CellMouseClick);
            this.dgv_ShowFills.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgv_ShowFills_MouseClick);
            // 
            // dgv_Summary
            // 
            this.dgv_Summary.AllowUserToAddRows = false;
            this.dgv_Summary.AllowUserToDeleteRows = false;
            this.dgv_Summary.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgv_Summary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_Summary.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgv_Summary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Summary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Side,
            this.Order_Quantity,
            this.Qty_Routed,
            this.Qty_Working,
            this.Fill_Quantity,
            this.Fill_Price,
            this.Profit});
            this.dgv_Summary.Location = new System.Drawing.Point(12, 44);
            this.dgv_Summary.Name = "dgv_Summary";
            this.dgv_Summary.ReadOnly = true;
            this.dgv_Summary.RowHeadersVisible = false;
            this.dgv_Summary.Size = new System.Drawing.Size(534, 141);
            this.dgv_Summary.TabIndex = 4;
            // 
            // Profit
            // 
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle21.Format = "N2";
            this.Profit.DefaultCellStyle = dataGridViewCellStyle21;
            this.Profit.HeaderText = "Profit";
            this.Profit.Name = "Profit";
            this.Profit.ReadOnly = true;
            this.Profit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Profit.Width = 90;
            // 
            // Fill_Price
            // 
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle20.Format = "N4";
            this.Fill_Price.DefaultCellStyle = dataGridViewCellStyle20;
            this.Fill_Price.HeaderText = "Fill Price";
            this.Fill_Price.Name = "Fill_Price";
            this.Fill_Price.ReadOnly = true;
            this.Fill_Price.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Fill_Price.ToolTipText = "The Average Fill Price across the selected Fills.";
            this.Fill_Price.Width = 80;
            // 
            // Fill_Quantity
            // 
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle19.Format = "N0";
            this.Fill_Quantity.DefaultCellStyle = dataGridViewCellStyle19;
            this.Fill_Quantity.HeaderText = "EOD/Fill Quantity";
            this.Fill_Quantity.Name = "Fill_Quantity";
            this.Fill_Quantity.ReadOnly = true;
            this.Fill_Quantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Fill_Quantity.ToolTipText = "This is the current Filll Quantity, or the End-of-Day final quantity.";
            this.Fill_Quantity.Width = 80;
            // 
            // Qty_Working
            // 
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle18.Format = "N0";
            this.Qty_Working.DefaultCellStyle = dataGridViewCellStyle18;
            this.Qty_Working.HeaderText = "Qty Working";
            this.Qty_Working.Name = "Qty_Working";
            this.Qty_Working.ReadOnly = true;
            this.Qty_Working.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Qty_Working.ToolTipText = "The Quantity remaining at the Broker, but yet to be filled.";
            this.Qty_Working.Width = 80;
            // 
            // Qty_Routed
            // 
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle17.Format = "N0";
            this.Qty_Routed.DefaultCellStyle = dataGridViewCellStyle17;
            this.Qty_Routed.HeaderText = "Routed Quantity";
            this.Qty_Routed.Name = "Qty_Routed";
            this.Qty_Routed.ReadOnly = true;
            this.Qty_Routed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Qty_Routed.ToolTipText = "The Quantity that has been sent to a Broker to Fill.";
            this.Qty_Routed.Width = 80;
            // 
            // Order_Quantity
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.Format = "N0";
            this.Order_Quantity.DefaultCellStyle = dataGridViewCellStyle16;
            this.Order_Quantity.HeaderText = "Order Quantity";
            this.Order_Quantity.Name = "Order_Quantity";
            this.Order_Quantity.ReadOnly = true;
            this.Order_Quantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Order_Quantity.ToolTipText = "The quantity that has been Modelled and sent to EMSX.";
            this.Order_Quantity.Width = 80;
            // 
            // Side
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.RoyalBlue;
            this.Side.DefaultCellStyle = dataGridViewCellStyle15;
            this.Side.HeaderText = "Side";
            this.Side.Name = "Side";
            this.Side.ReadOnly = true;
            this.Side.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Side.Width = 40;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Info;
            this.richTextBox1.ForeColor = System.Drawing.Color.Green;
            this.richTextBox1.Location = new System.Drawing.Point(552, 44);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(520, 141);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // ShowFillStatus
            // 
            this.AcceptButton = this.bt_Refresh;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bt_Close;
            this.ClientSize = new System.Drawing.Size(1084, 389);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.dgv_ShowFills);
            this.Controls.Add(this.bt_Print);
            this.Controls.Add(this.dgv_Summary);
            this.Controls.Add(this.bt_EMSX);
            this.Controls.Add(this.bt_Close);
            this.Controls.Add(this.bt_Refresh);
            this.Name = "ShowFillStatus";
            this.Text = "ShowFillStatus";
            this.Load += new System.EventHandler(this.ShowFillStatus_Load);
            this.Shown += new System.EventHandler(this.ShowFillStatus_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ShowFillStatus_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ShowFills)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Summary)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bt_Refresh;
        private System.Windows.Forms.Button bt_Close;
        private System.Windows.Forms.Button bt_EMSX;
        private System.Windows.Forms.Button bt_Print;
        private System.Windows.Forms.DataGridView dgv_ShowFills;
        private System.Windows.Forms.DataGridView dgv_Summary;
        private System.Windows.Forms.DataGridViewTextBoxColumn Side;
        private System.Windows.Forms.DataGridViewTextBoxColumn Order_Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty_Routed;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty_Working;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fill_Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fill_Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Profit;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}