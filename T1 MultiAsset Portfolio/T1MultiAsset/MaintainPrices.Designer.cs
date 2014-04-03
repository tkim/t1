namespace T1MultiAsset
{
    partial class MaintainPrices
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
            this.bt_Request = new System.Windows.Forms.Button();
            this.bt_Save = new System.Windows.Forms.Button();
            this.tb_BBG_Ticker = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dg_Prices = new System.Windows.Forms.DataGridView();
            this.tb_RebuildProfit = new System.Windows.Forms.Button();
            this.lb_Message = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Prices)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_Request
            // 
            this.bt_Request.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Request.ForeColor = System.Drawing.Color.Blue;
            this.bt_Request.Location = new System.Drawing.Point(715, 12);
            this.bt_Request.Name = "bt_Request";
            this.bt_Request.Size = new System.Drawing.Size(75, 23);
            this.bt_Request.TabIndex = 0;
            this.bt_Request.Text = "&Request";
            this.bt_Request.UseVisualStyleBackColor = true;
            this.bt_Request.Click += new System.EventHandler(this.bt_Request_Click);
            // 
            // bt_Save
            // 
            this.bt_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Save.ForeColor = System.Drawing.Color.DarkGreen;
            this.bt_Save.Location = new System.Drawing.Point(706, 357);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(75, 23);
            this.bt_Save.TabIndex = 1;
            this.bt_Save.Text = "&Save";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // tb_BBG_Ticker
            // 
            this.tb_BBG_Ticker.Location = new System.Drawing.Point(58, 17);
            this.tb_BBG_Ticker.Name = "tb_BBG_Ticker";
            this.tb_BBG_Ticker.Size = new System.Drawing.Size(147, 20);
            this.tb_BBG_Ticker.TabIndex = 2;
            this.tb_BBG_Ticker.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_BBG_Ticker_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ticker:";
            // 
            // dg_Prices
            // 
            this.dg_Prices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_Prices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_Prices.Location = new System.Drawing.Point(12, 48);
            this.dg_Prices.Name = "dg_Prices";
            this.dg_Prices.Size = new System.Drawing.Size(787, 303);
            this.dg_Prices.TabIndex = 4;
            this.dg_Prices.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dg_Prices_CellBeginEdit);
            this.dg_Prices.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dg_Prices_UserAddedRow);
            this.dg_Prices.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_Prices_CellEndEdit);
            this.dg_Prices.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dg_Prices_DataError);
            // 
            // tb_RebuildProfit
            // 
            this.tb_RebuildProfit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_RebuildProfit.Enabled = false;
            this.tb_RebuildProfit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_RebuildProfit.ForeColor = System.Drawing.Color.DarkGreen;
            this.tb_RebuildProfit.Location = new System.Drawing.Point(677, 386);
            this.tb_RebuildProfit.Name = "tb_RebuildProfit";
            this.tb_RebuildProfit.Size = new System.Drawing.Size(104, 23);
            this.tb_RebuildProfit.TabIndex = 5;
            this.tb_RebuildProfit.Text = "&Rebuild Profit";
            this.tb_RebuildProfit.UseVisualStyleBackColor = true;
            this.tb_RebuildProfit.Click += new System.EventHandler(this.tb_RebuildProfit_Click);
            // 
            // lb_Message
            // 
            this.lb_Message.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lb_Message.ForeColor = System.Drawing.Color.DarkGreen;
            this.lb_Message.Location = new System.Drawing.Point(17, 365);
            this.lb_Message.Name = "lb_Message";
            this.lb_Message.Size = new System.Drawing.Size(521, 44);
            this.lb_Message.TabIndex = 6;
            this.lb_Message.Text = "label2";
            this.lb_Message.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // MaintainPrices
            // 
            this.AcceptButton = this.bt_Request;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 421);
            this.Controls.Add(this.lb_Message);
            this.Controls.Add(this.tb_RebuildProfit);
            this.Controls.Add(this.dg_Prices);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_BBG_Ticker);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.bt_Request);
            this.Name = "MaintainPrices";
            this.Text = "MaintainPrices";
            this.Shown += new System.EventHandler(this.MaintainPrices_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MaintainPrices_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dg_Prices)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_Request;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.TextBox tb_BBG_Ticker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dg_Prices;
        private System.Windows.Forms.Button tb_RebuildProfit;
        private System.Windows.Forms.Label lb_Message;
    }
}