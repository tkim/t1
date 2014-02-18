namespace T1MultiAsset
{
    partial class FuturesExplainWireTransfer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FuturesExplainWireTransfer));
            this.dg_Trades = new System.Windows.Forms.DataGridView();
            this.dg_MarginMovement = new System.Windows.Forms.DataGridView();
            this.dtp_TransferDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lb_TotalMargin = new System.Windows.Forms.Label();
            this.bt_Refresh = new System.Windows.Forms.Button();
            this.cb_ParentFund = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_TransferAmount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_Expected = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_Diff = new System.Windows.Forms.TextBox();
            this.bt_recalc_margins = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.bt_JournalDiff = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.bt_Print = new System.Windows.Forms.Button();
            this.bt_AlterMarginAmounts = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Trades)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_MarginMovement)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dg_Trades
            // 
            this.dg_Trades.AllowUserToAddRows = false;
            this.dg_Trades.AllowUserToDeleteRows = false;
            this.dg_Trades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_Trades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_Trades.Location = new System.Drawing.Point(0, 0);
            this.dg_Trades.Name = "dg_Trades";
            this.dg_Trades.ReadOnly = true;
            this.dg_Trades.Size = new System.Drawing.Size(1120, 175);
            this.dg_Trades.TabIndex = 0;
            // 
            // dg_MarginMovement
            // 
            this.dg_MarginMovement.AllowUserToAddRows = false;
            this.dg_MarginMovement.AllowUserToDeleteRows = false;
            this.dg_MarginMovement.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_MarginMovement.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_MarginMovement.Location = new System.Drawing.Point(6, 3);
            this.dg_MarginMovement.Name = "dg_MarginMovement";
            this.dg_MarginMovement.ReadOnly = true;
            this.dg_MarginMovement.Size = new System.Drawing.Size(1108, 186);
            this.dg_MarginMovement.TabIndex = 1;
            // 
            // dtp_TransferDate
            // 
            this.dtp_TransferDate.Location = new System.Drawing.Point(123, 9);
            this.dtp_TransferDate.Name = "dtp_TransferDate";
            this.dtp_TransferDate.Size = new System.Drawing.Size(184, 20);
            this.dtp_TransferDate.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.MediumBlue;
            this.label1.Location = new System.Drawing.Point(15, 14);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Wire Transfer Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.MediumBlue;
            this.label2.Location = new System.Drawing.Point(6, 60);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Previous Day\'s Trades";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.MediumBlue;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(133, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Profit && Margin Movements";
            // 
            // lb_TotalMargin
            // 
            this.lb_TotalMargin.AutoSize = true;
            this.lb_TotalMargin.ForeColor = System.Drawing.Color.Green;
            this.lb_TotalMargin.Location = new System.Drawing.Point(645, 33);
            this.lb_TotalMargin.Name = "lb_TotalMargin";
            this.lb_TotalMargin.Size = new System.Drawing.Size(35, 13);
            this.lb_TotalMargin.TabIndex = 6;
            this.lb_TotalMargin.Text = "label4";
            // 
            // bt_Refresh
            // 
            this.bt_Refresh.Location = new System.Drawing.Point(273, 27);
            this.bt_Refresh.Name = "bt_Refresh";
            this.bt_Refresh.Size = new System.Drawing.Size(75, 23);
            this.bt_Refresh.TabIndex = 8;
            this.bt_Refresh.Text = "Request";
            this.bt_Refresh.UseVisualStyleBackColor = true;
            this.bt_Refresh.Click += new System.EventHandler(this.bt_Refresh_Click);
            // 
            // cb_ParentFund
            // 
            this.cb_ParentFund.FormattingEnabled = true;
            this.cb_ParentFund.Location = new System.Drawing.Point(121, 30);
            this.cb_ParentFund.Name = "cb_ParentFund";
            this.cb_ParentFund.Size = new System.Drawing.Size(145, 21);
            this.cb_ParentFund.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.MediumBlue;
            this.label4.Location = new System.Drawing.Point(81, 33);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Fund:";
            // 
            // tb_TransferAmount
            // 
            this.tb_TransferAmount.Location = new System.Drawing.Point(535, 7);
            this.tb_TransferAmount.Name = "tb_TransferAmount";
            this.tb_TransferAmount.Size = new System.Drawing.Size(104, 20);
            this.tb_TransferAmount.TabIndex = 11;
            this.tb_TransferAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.MediumBlue;
            this.label5.Location = new System.Drawing.Point(424, 10);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Transfer Into Futures:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tb_Expected
            // 
            this.tb_Expected.Location = new System.Drawing.Point(535, 29);
            this.tb_Expected.Name = "tb_Expected";
            this.tb_Expected.Size = new System.Drawing.Size(104, 20);
            this.tb_Expected.TabIndex = 13;
            this.tb_Expected.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.MediumBlue;
            this.label6.Location = new System.Drawing.Point(473, 33);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Expected:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(501, 56);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label7.Size = new System.Drawing.Size(27, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Diff";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tb_Diff
            // 
            this.tb_Diff.Location = new System.Drawing.Point(535, 53);
            this.tb_Diff.Name = "tb_Diff";
            this.tb_Diff.Size = new System.Drawing.Size(104, 20);
            this.tb_Diff.TabIndex = 16;
            this.tb_Diff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // bt_recalc_margins
            // 
            this.bt_recalc_margins.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_recalc_margins.ForeColor = System.Drawing.Color.Green;
            this.bt_recalc_margins.Location = new System.Drawing.Point(648, 52);
            this.bt_recalc_margins.Name = "bt_recalc_margins";
            this.bt_recalc_margins.Size = new System.Drawing.Size(119, 20);
            this.bt_recalc_margins.TabIndex = 17;
            this.bt_recalc_margins.Text = "Recalc Margins";
            this.bt_recalc_margins.UseVisualStyleBackColor = true;
            this.bt_recalc_margins.Click += new System.EventHandler(this.bt_recalc_margins_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.Green;
            this.richTextBox1.Location = new System.Drawing.Point(3, 6);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(1108, 205);
            this.richTextBox1.TabIndex = 18;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // bt_JournalDiff
            // 
            this.bt_JournalDiff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_JournalDiff.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_JournalDiff.ForeColor = System.Drawing.Color.Green;
            this.bt_JournalDiff.Location = new System.Drawing.Point(1008, 3);
            this.bt_JournalDiff.Name = "bt_JournalDiff";
            this.bt_JournalDiff.Size = new System.Drawing.Size(119, 20);
            this.bt_JournalDiff.TabIndex = 19;
            this.bt_JournalDiff.Text = "Journal Diff";
            this.bt_JournalDiff.UseVisualStyleBackColor = true;
            this.bt_JournalDiff.Click += new System.EventHandler(this.bt_JournalDiff_Click);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(3, 214);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(976, 17);
            this.label8.TabIndex = 20;
            this.label8.Text = "THIS MAY ALSO HAPPEN IF THE MARGIN PER CONTRACT REQUIRED CHANGE - Please Update v" +
                "ia the [Alter Margin Amounts] button and then [Recalc Margins]";
            // 
            // bt_Print
            // 
            this.bt_Print.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Print.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bt_Print.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Print.ForeColor = System.Drawing.Color.MediumBlue;
            this.bt_Print.Location = new System.Drawing.Point(1039, 208);
            this.bt_Print.Margin = new System.Windows.Forms.Padding(0);
            this.bt_Print.Name = "bt_Print";
            this.bt_Print.Size = new System.Drawing.Size(72, 25);
            this.bt_Print.TabIndex = 25;
            this.bt_Print.Text = "Print";
            this.bt_Print.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bt_Print.UseVisualStyleBackColor = true;
            this.bt_Print.Click += new System.EventHandler(this.bt_Print_Click);
            // 
            // bt_AlterMarginAmounts
            // 
            this.bt_AlterMarginAmounts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_AlterMarginAmounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_AlterMarginAmounts.ForeColor = System.Drawing.Color.Blue;
            this.bt_AlterMarginAmounts.Location = new System.Drawing.Point(1008, 53);
            this.bt_AlterMarginAmounts.Name = "bt_AlterMarginAmounts";
            this.bt_AlterMarginAmounts.Size = new System.Drawing.Size(119, 20);
            this.bt_AlterMarginAmounts.TabIndex = 26;
            this.bt_AlterMarginAmounts.Text = "Alter Margin Amounts";
            this.bt_AlterMarginAmounts.UseVisualStyleBackColor = true;
            this.bt_AlterMarginAmounts.Click += new System.EventHandler(this.bt_AlterMarginAmounts_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(4, 78);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dg_Trades);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Size = new System.Drawing.Size(1123, 632);
            this.splitContainer1.SplitterDistance = 176;
            this.splitContainer1.TabIndex = 27;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(3, 16);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dg_MarginMovement);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.bt_Print);
            this.splitContainer2.Panel2.Controls.Add(this.label8);
            this.splitContainer2.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer2.Size = new System.Drawing.Size(1117, 433);
            this.splitContainer2.SplitterDistance = 192;
            this.splitContainer2.TabIndex = 6;
            // 
            // FuturesExplainWireTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 726);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.bt_AlterMarginAmounts);
            this.Controls.Add(this.bt_JournalDiff);
            this.Controls.Add(this.bt_recalc_margins);
            this.Controls.Add(this.tb_Diff);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb_Expected);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_TransferAmount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cb_ParentFund);
            this.Controls.Add(this.bt_Refresh);
            this.Controls.Add(this.lb_TotalMargin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtp_TransferDate);
            this.Name = "FuturesExplainWireTransfer";
            this.Text = "Explain Futures Wire Transfer";
            this.Load += new System.EventHandler(this.FuturesExplainWireTransfer_Load);
            this.Shown += new System.EventHandler(this.FuturesExplainWireTransfer_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dg_Trades)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_MarginMovement)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_Trades;
        private System.Windows.Forms.DataGridView dg_MarginMovement;
        private System.Windows.Forms.DateTimePicker dtp_TransferDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lb_TotalMargin;
        private System.Windows.Forms.Button bt_Refresh;
        private System.Windows.Forms.ComboBox cb_ParentFund;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_TransferAmount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_Expected;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_Diff;
        private System.Windows.Forms.Button bt_recalc_margins;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button bt_JournalDiff;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button bt_Print;
        private System.Windows.Forms.Button bt_AlterMarginAmounts;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
    }
}