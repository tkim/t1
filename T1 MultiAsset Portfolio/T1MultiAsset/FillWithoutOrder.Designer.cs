namespace T1MultiAsset
{
    partial class FillWithoutOrder
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dg_FillWithoutOrder = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.dg_Match = new System.Windows.Forms.DataGridView();
            this.FundID = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.PortfolioID = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.BBG_Ticker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty_Fill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Round_Lot_Size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.bt_CreateOrder = new System.Windows.Forms.Button();
            this.tb_TotalQuantity = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_MissingQuantity = new System.Windows.Forms.TextBox();
            this.bt_Calculator = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dg_FillWithoutOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Match)).BeginInit();
            this.SuspendLayout();
            // 
            // dg_FillWithoutOrder
            // 
            this.dg_FillWithoutOrder.AllowUserToAddRows = false;
            this.dg_FillWithoutOrder.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightCyan;
            this.dg_FillWithoutOrder.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dg_FillWithoutOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_FillWithoutOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_FillWithoutOrder.DefaultCellStyle = dataGridViewCellStyle2;
            this.dg_FillWithoutOrder.Location = new System.Drawing.Point(12, 25);
            this.dg_FillWithoutOrder.MultiSelect = false;
            this.dg_FillWithoutOrder.Name = "dg_FillWithoutOrder";
            this.dg_FillWithoutOrder.ReadOnly = true;
            this.dg_FillWithoutOrder.Size = new System.Drawing.Size(803, 192);
            this.dg_FillWithoutOrder.TabIndex = 5;
            this.dg_FillWithoutOrder.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_FillWithoutOrder_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkBlue;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(625, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Need to assign fills without Orders. Click on Row to allocate to an existing posi" +
                "tion or Create a New Position.";
            // 
            // dg_Match
            // 
            this.dg_Match.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.LightCyan;
            this.dg_Match.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dg_Match.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_Match.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_Match.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FundID,
            this.PortfolioID,
            this.BBG_Ticker,
            this.Quantity,
            this.Qty_Fill,
            this.Round_Lot_Size});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_Match.DefaultCellStyle = dataGridViewCellStyle7;
            this.dg_Match.Enabled = false;
            this.dg_Match.Location = new System.Drawing.Point(12, 278);
            this.dg_Match.MultiSelect = false;
            this.dg_Match.Name = "dg_Match";
            this.dg_Match.Size = new System.Drawing.Size(803, 204);
            this.dg_Match.TabIndex = 7;
            this.dg_Match.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dg_Match_CellBeginEdit);
            this.dg_Match.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dg_Match_UserAddedRow);
            this.dg_Match.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_Match_CellEndEdit);
            this.dg_Match.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dg_Match_DataError);
            // 
            // FundID
            // 
            this.FundID.HeaderText = "Fund Name";
            this.FundID.Name = "FundID";
            this.FundID.Width = 170;
            // 
            // PortfolioID
            // 
            this.PortfolioID.HeaderText = "Portfolio Name";
            this.PortfolioID.Name = "PortfolioID";
            this.PortfolioID.Width = 170;
            // 
            // BBG_Ticker
            // 
            this.BBG_Ticker.HeaderText = "Ticker";
            this.BBG_Ticker.Name = "BBG_Ticker";
            this.BBG_Ticker.ReadOnly = true;
            // 
            // Quantity
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N0";
            dataGridViewCellStyle4.NullValue = null;
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle4;
            this.Quantity.HeaderText = "Quantity";
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            // 
            // Qty_Fill
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle5.Format = "N0";
            dataGridViewCellStyle5.NullValue = null;
            this.Qty_Fill.DefaultCellStyle = dataGridViewCellStyle5;
            this.Qty_Fill.HeaderText = "Fill Quantity";
            this.Qty_Fill.Name = "Qty_Fill";
            // 
            // Round_Lot_Size
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle6.Format = "N0";
            dataGridViewCellStyle6.NullValue = null;
            this.Round_Lot_Size.DefaultCellStyle = dataGridViewCellStyle6;
            this.Round_Lot_Size.HeaderText = "Round Lot Size";
            this.Round_Lot_Size.Name = "Round_Lot_Size";
            this.Round_Lot_Size.ReadOnly = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DarkBlue;
            this.label2.Location = new System.Drawing.Point(12, 262);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(328, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Click on Rows and Allocate Quantity, or Add a New Row";
            // 
            // bt_CreateOrder
            // 
            this.bt_CreateOrder.Enabled = false;
            this.bt_CreateOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_CreateOrder.ForeColor = System.Drawing.Color.DarkBlue;
            this.bt_CreateOrder.Location = new System.Drawing.Point(12, 488);
            this.bt_CreateOrder.Name = "bt_CreateOrder";
            this.bt_CreateOrder.Size = new System.Drawing.Size(94, 23);
            this.bt_CreateOrder.TabIndex = 9;
            this.bt_CreateOrder.Text = "Create Order";
            this.bt_CreateOrder.UseVisualStyleBackColor = true;
            this.bt_CreateOrder.Click += new System.EventHandler(this.bt_CreateOrder_Click);
            // 
            // tb_TotalQuantity
            // 
            this.tb_TotalQuantity.Location = new System.Drawing.Point(595, 223);
            this.tb_TotalQuantity.Name = "tb_TotalQuantity";
            this.tb_TotalQuantity.Size = new System.Drawing.Size(94, 20);
            this.tb_TotalQuantity.TabIndex = 10;
            this.tb_TotalQuantity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_TotalQuantity_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.DarkGreen;
            this.label3.Location = new System.Drawing.Point(516, 226);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Total Quantity";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.DarkGreen;
            this.label4.Location = new System.Drawing.Point(524, 252);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Unbalanced";
            // 
            // tb_MissingQuantity
            // 
            this.tb_MissingQuantity.Location = new System.Drawing.Point(595, 249);
            this.tb_MissingQuantity.Name = "tb_MissingQuantity";
            this.tb_MissingQuantity.Size = new System.Drawing.Size(94, 20);
            this.tb_MissingQuantity.TabIndex = 13;
            this.tb_MissingQuantity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_MissingQuantity_KeyPress);
            // 
            // bt_Calculator
            // 
            this.bt_Calculator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bt_Calculator.ForeColor = System.Drawing.Color.MediumBlue;
            this.bt_Calculator.Location = new System.Drawing.Point(743, 226);
            this.bt_Calculator.Margin = new System.Windows.Forms.Padding(0);
            this.bt_Calculator.Name = "bt_Calculator";
            this.bt_Calculator.Size = new System.Drawing.Size(72, 25);
            this.bt_Calculator.TabIndex = 14;
            this.bt_Calculator.Text = "Calculator";
            this.bt_Calculator.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bt_Calculator.UseVisualStyleBackColor = true;
            this.bt_Calculator.Click += new System.EventHandler(this.bt_Calculator_Click);
            // 
            // FillWithoutOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 513);
            this.Controls.Add(this.bt_Calculator);
            this.Controls.Add(this.tb_MissingQuantity);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_TotalQuantity);
            this.Controls.Add(this.bt_CreateOrder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dg_Match);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dg_FillWithoutOrder);
            this.Name = "FillWithoutOrder";
            this.Text = "Assign Fills Without an Order";
            this.Load += new System.EventHandler(this.FillWithoutOrder_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FillWithoutOrder_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dg_FillWithoutOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Match)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_FillWithoutOrder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dg_Match;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bt_CreateOrder;
        private System.Windows.Forms.DataGridViewComboBoxColumn FundID;
        private System.Windows.Forms.DataGridViewComboBoxColumn PortfolioID;
        private System.Windows.Forms.DataGridViewTextBoxColumn BBG_Ticker;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty_Fill;
        private System.Windows.Forms.DataGridViewTextBoxColumn Round_Lot_Size;
        private System.Windows.Forms.TextBox tb_TotalQuantity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_MissingQuantity;
        private System.Windows.Forms.Button bt_Calculator;
    }
}