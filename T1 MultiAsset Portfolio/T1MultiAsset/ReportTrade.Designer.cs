namespace T1MultiAsset
{
    partial class ReportTrade
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
            this.cb_hasFundID = new System.Windows.Forms.CheckBox();
            this.cb_Fund = new System.Windows.Forms.ComboBox();
            this.cb_Portfolio = new System.Windows.Forms.ComboBox();
            this.cb_hasPortfolioID = new System.Windows.Forms.CheckBox();
            this.cb_hasBBG_Ticker = new System.Windows.Forms.CheckBox();
            this.tb_BBG_Ticker = new System.Windows.Forms.TextBox();
            this.dtp_FromDate = new System.Windows.Forms.DateTimePicker();
            this.cb_hasFromDate = new System.Windows.Forms.CheckBox();
            this.cb_hasToDate = new System.Windows.Forms.CheckBox();
            this.dtp_ToDate = new System.Windows.Forms.DateTimePicker();
            this.dg_ReportTrade = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cb_hasBrokerID = new System.Windows.Forms.CheckBox();
            this.cb_Broker = new System.Windows.Forms.ComboBox();
            this.bt_Request = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dg_ReportTrade)).BeginInit();
            this.SuspendLayout();
            // 
            // cb_hasFundID
            // 
            this.cb_hasFundID.AutoSize = true;
            this.cb_hasFundID.ForeColor = System.Drawing.Color.DarkGreen;
            this.cb_hasFundID.Location = new System.Drawing.Point(12, 25);
            this.cb_hasFundID.Name = "cb_hasFundID";
            this.cb_hasFundID.Size = new System.Drawing.Size(15, 14);
            this.cb_hasFundID.TabIndex = 0;
            this.cb_hasFundID.UseVisualStyleBackColor = true;
            this.cb_hasFundID.CheckedChanged += new System.EventHandler(this.cb_hasFundID_CheckedChanged);
            // 
            // cb_Fund
            // 
            this.cb_Fund.Enabled = false;
            this.cb_Fund.FormattingEnabled = true;
            this.cb_Fund.Location = new System.Drawing.Point(36, 24);
            this.cb_Fund.Name = "cb_Fund";
            this.cb_Fund.Size = new System.Drawing.Size(147, 21);
            this.cb_Fund.TabIndex = 1;
            this.cb_Fund.SelectionChangeCommitted += new System.EventHandler(this.cb_Fund_SelectionChangeCommitted);
            // 
            // cb_Portfolio
            // 
            this.cb_Portfolio.Enabled = false;
            this.cb_Portfolio.FormattingEnabled = true;
            this.cb_Portfolio.Location = new System.Drawing.Point(218, 25);
            this.cb_Portfolio.Name = "cb_Portfolio";
            this.cb_Portfolio.Size = new System.Drawing.Size(147, 21);
            this.cb_Portfolio.TabIndex = 3;
            this.cb_Portfolio.SelectionChangeCommitted += new System.EventHandler(this.cb_Portfolio_SelectionChangeCommitted);
            // 
            // cb_hasPortfolioID
            // 
            this.cb_hasPortfolioID.AutoSize = true;
            this.cb_hasPortfolioID.ForeColor = System.Drawing.Color.DarkGreen;
            this.cb_hasPortfolioID.Location = new System.Drawing.Point(194, 26);
            this.cb_hasPortfolioID.Name = "cb_hasPortfolioID";
            this.cb_hasPortfolioID.Size = new System.Drawing.Size(15, 14);
            this.cb_hasPortfolioID.TabIndex = 2;
            this.cb_hasPortfolioID.UseVisualStyleBackColor = true;
            this.cb_hasPortfolioID.CheckedChanged += new System.EventHandler(this.cb_hasPortfolioID_CheckedChanged);
            // 
            // cb_hasBBG_Ticker
            // 
            this.cb_hasBBG_Ticker.AutoSize = true;
            this.cb_hasBBG_Ticker.ForeColor = System.Drawing.Color.DarkGreen;
            this.cb_hasBBG_Ticker.Location = new System.Drawing.Point(381, 27);
            this.cb_hasBBG_Ticker.Name = "cb_hasBBG_Ticker";
            this.cb_hasBBG_Ticker.Size = new System.Drawing.Size(15, 14);
            this.cb_hasBBG_Ticker.TabIndex = 4;
            this.cb_hasBBG_Ticker.UseVisualStyleBackColor = true;
            this.cb_hasBBG_Ticker.CheckedChanged += new System.EventHandler(this.cb_hasBBG_Ticker_CheckedChanged);
            // 
            // tb_BBG_Ticker
            // 
            this.tb_BBG_Ticker.Enabled = false;
            this.tb_BBG_Ticker.Location = new System.Drawing.Point(408, 25);
            this.tb_BBG_Ticker.Name = "tb_BBG_Ticker";
            this.tb_BBG_Ticker.Size = new System.Drawing.Size(141, 20);
            this.tb_BBG_Ticker.TabIndex = 5;
            // 
            // dtp_FromDate
            // 
            this.dtp_FromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_FromDate.Enabled = false;
            this.dtp_FromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_FromDate.Location = new System.Drawing.Point(36, 73);
            this.dtp_FromDate.Name = "dtp_FromDate";
            this.dtp_FromDate.Size = new System.Drawing.Size(208, 20);
            this.dtp_FromDate.TabIndex = 6;
            // 
            // cb_hasFromDate
            // 
            this.cb_hasFromDate.AutoSize = true;
            this.cb_hasFromDate.ForeColor = System.Drawing.Color.DarkGreen;
            this.cb_hasFromDate.Location = new System.Drawing.Point(12, 77);
            this.cb_hasFromDate.Name = "cb_hasFromDate";
            this.cb_hasFromDate.Size = new System.Drawing.Size(15, 14);
            this.cb_hasFromDate.TabIndex = 7;
            this.cb_hasFromDate.UseVisualStyleBackColor = true;
            this.cb_hasFromDate.CheckedChanged += new System.EventHandler(this.cb_hasFromDate_CheckedChanged);
            // 
            // cb_hasToDate
            // 
            this.cb_hasToDate.AutoSize = true;
            this.cb_hasToDate.ForeColor = System.Drawing.Color.DarkGreen;
            this.cb_hasToDate.Location = new System.Drawing.Point(256, 77);
            this.cb_hasToDate.Name = "cb_hasToDate";
            this.cb_hasToDate.Size = new System.Drawing.Size(15, 14);
            this.cb_hasToDate.TabIndex = 9;
            this.cb_hasToDate.UseVisualStyleBackColor = true;
            this.cb_hasToDate.CheckedChanged += new System.EventHandler(this.cb_hasToDate_CheckedChanged);
            // 
            // dtp_ToDate
            // 
            this.dtp_ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_ToDate.Enabled = false;
            this.dtp_ToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_ToDate.Location = new System.Drawing.Point(280, 73);
            this.dtp_ToDate.Name = "dtp_ToDate";
            this.dtp_ToDate.Size = new System.Drawing.Size(208, 20);
            this.dtp_ToDate.TabIndex = 8;
            // 
            // dg_ReportTrade
            // 
            this.dg_ReportTrade.AllowUserToAddRows = false;
            this.dg_ReportTrade.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_ReportTrade.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_ReportTrade.Location = new System.Drawing.Point(4, 111);
            this.dg_ReportTrade.Name = "dg_ReportTrade";
            this.dg_ReportTrade.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.MidnightBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_ReportTrade.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dg_ReportTrade.RowHeadersVisible = false;
            this.dg_ReportTrade.Size = new System.Drawing.Size(1102, 450);
            this.dg_ReportTrade.TabIndex = 10;
            this.dg_ReportTrade.Sorted += new System.EventHandler(this.dg_ReportTrade_Sorted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DarkBlue;
            this.label1.Location = new System.Drawing.Point(33, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Fund Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.DarkBlue;
            this.label2.Location = new System.Drawing.Point(215, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Portfolio Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.DarkBlue;
            this.label3.Location = new System.Drawing.Point(405, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Bloomberg Ticker";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.DarkBlue;
            this.label4.Location = new System.Drawing.Point(33, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "From Date";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.DarkBlue;
            this.label5.Location = new System.Drawing.Point(277, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "To Date";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.DarkBlue;
            this.label6.Location = new System.Drawing.Point(581, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Broker Name";
            // 
            // cb_hasBrokerID
            // 
            this.cb_hasBrokerID.AutoSize = true;
            this.cb_hasBrokerID.ForeColor = System.Drawing.Color.DarkGreen;
            this.cb_hasBrokerID.Location = new System.Drawing.Point(557, 27);
            this.cb_hasBrokerID.Name = "cb_hasBrokerID";
            this.cb_hasBrokerID.Size = new System.Drawing.Size(15, 14);
            this.cb_hasBrokerID.TabIndex = 16;
            this.cb_hasBrokerID.UseVisualStyleBackColor = true;
            this.cb_hasBrokerID.CheckedChanged += new System.EventHandler(this.cb_hasBrokerID_CheckedChanged);
            // 
            // cb_Broker
            // 
            this.cb_Broker.Enabled = false;
            this.cb_Broker.FormattingEnabled = true;
            this.cb_Broker.Location = new System.Drawing.Point(578, 27);
            this.cb_Broker.Name = "cb_Broker";
            this.cb_Broker.Size = new System.Drawing.Size(147, 21);
            this.cb_Broker.TabIndex = 19;
            this.cb_Broker.SelectionChangeCommitted += new System.EventHandler(this.cb_Broker_SelectionChangeCommitted);
            // 
            // bt_Request
            // 
            this.bt_Request.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Request.ForeColor = System.Drawing.Color.DarkBlue;
            this.bt_Request.Location = new System.Drawing.Point(534, 72);
            this.bt_Request.Name = "bt_Request";
            this.bt_Request.Size = new System.Drawing.Size(85, 20);
            this.bt_Request.TabIndex = 20;
            this.bt_Request.Text = "Request";
            this.bt_Request.UseVisualStyleBackColor = true;
            this.bt_Request.Click += new System.EventHandler(this.bt_Request_Click);
            // 
            // ReportTrade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 564);
            this.Controls.Add(this.bt_Request);
            this.Controls.Add(this.cb_Broker);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cb_hasBrokerID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dg_ReportTrade);
            this.Controls.Add(this.cb_hasToDate);
            this.Controls.Add(this.dtp_ToDate);
            this.Controls.Add(this.cb_hasFromDate);
            this.Controls.Add(this.dtp_FromDate);
            this.Controls.Add(this.tb_BBG_Ticker);
            this.Controls.Add(this.cb_hasBBG_Ticker);
            this.Controls.Add(this.cb_Portfolio);
            this.Controls.Add(this.cb_hasPortfolioID);
            this.Controls.Add(this.cb_Fund);
            this.Controls.Add(this.cb_hasFundID);
            this.Name = "ReportTrade";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ReportTrade";
            this.Load += new System.EventHandler(this.ReportTrade_Load);
            this.Shown += new System.EventHandler(this.ReportTrade_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dg_ReportTrade)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cb_hasFundID;
        private System.Windows.Forms.ComboBox cb_Fund;
        private System.Windows.Forms.ComboBox cb_Portfolio;
        private System.Windows.Forms.CheckBox cb_hasPortfolioID;
        private System.Windows.Forms.CheckBox cb_hasBBG_Ticker;
        private System.Windows.Forms.TextBox tb_BBG_Ticker;
        private System.Windows.Forms.DateTimePicker dtp_FromDate;
        private System.Windows.Forms.CheckBox cb_hasFromDate;
        private System.Windows.Forms.CheckBox cb_hasToDate;
        private System.Windows.Forms.DateTimePicker dtp_ToDate;
        private System.Windows.Forms.DataGridView dg_ReportTrade;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cb_hasBrokerID;
        private System.Windows.Forms.ComboBox cb_Broker;
        private System.Windows.Forms.Button bt_Request;
    }
}