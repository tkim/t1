namespace T1MultiAsset
{
    partial class FutureMargins
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
            this.bt_Save = new System.Windows.Forms.Button();
            this.bt_Refresh = new System.Windows.Forms.Button();
            this.dg_FutureMargins = new System.Windows.Forms.DataGridView();
            this.bt_ProcessOutstandingTrades = new System.Windows.Forms.Button();
            this.bt_Check = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dg_FutureMargins)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_Save
            // 
            this.bt_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Save.ForeColor = System.Drawing.Color.Green;
            this.bt_Save.Location = new System.Drawing.Point(560, 12);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(75, 23);
            this.bt_Save.TabIndex = 0;
            this.bt_Save.Text = "Save";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // bt_Refresh
            // 
            this.bt_Refresh.Location = new System.Drawing.Point(12, 41);
            this.bt_Refresh.Name = "bt_Refresh";
            this.bt_Refresh.Size = new System.Drawing.Size(75, 23);
            this.bt_Refresh.TabIndex = 3;
            this.bt_Refresh.Text = "Refresh";
            this.bt_Refresh.UseVisualStyleBackColor = true;
            // 
            // dg_FutureMargins
            // 
            this.dg_FutureMargins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_FutureMargins.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_FutureMargins.Location = new System.Drawing.Point(12, 70);
            this.dg_FutureMargins.Name = "dg_FutureMargins";
            this.dg_FutureMargins.Size = new System.Drawing.Size(645, 360);
            this.dg_FutureMargins.TabIndex = 4;
            // 
            // bt_ProcessOutstandingTrades
            // 
            this.bt_ProcessOutstandingTrades.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_ProcessOutstandingTrades.Location = new System.Drawing.Point(475, 41);
            this.bt_ProcessOutstandingTrades.Name = "bt_ProcessOutstandingTrades";
            this.bt_ProcessOutstandingTrades.Size = new System.Drawing.Size(160, 23);
            this.bt_ProcessOutstandingTrades.TabIndex = 5;
            this.bt_ProcessOutstandingTrades.Text = "Process Outstanding Trades";
            this.bt_ProcessOutstandingTrades.UseVisualStyleBackColor = true;
            this.bt_ProcessOutstandingTrades.Click += new System.EventHandler(this.bt_ProcessOutstandingTrades_Click);
            // 
            // bt_Check
            // 
            this.bt_Check.ForeColor = System.Drawing.Color.Blue;
            this.bt_Check.Location = new System.Drawing.Point(12, 12);
            this.bt_Check.Name = "bt_Check";
            this.bt_Check.Size = new System.Drawing.Size(205, 23);
            this.bt_Check.TabIndex = 6;
            this.bt_Check.Text = "Check for Trades with no Future Margin  set";
            this.bt_Check.UseVisualStyleBackColor = true;
            this.bt_Check.Click += new System.EventHandler(this.bt_Check_Click);
            // 
            // FutureMargins
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 432);
            this.Controls.Add(this.bt_Check);
            this.Controls.Add(this.bt_ProcessOutstandingTrades);
            this.Controls.Add(this.dg_FutureMargins);
            this.Controls.Add(this.bt_Refresh);
            this.Controls.Add(this.bt_Save);
            this.Name = "FutureMargins";
            this.Text = "Maintain Future Margins";
            this.Load += new System.EventHandler(this.FutureMargins_Load);
            this.Shown += new System.EventHandler(this.FutureMargins_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dg_FutureMargins)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.Button bt_Refresh;
        private System.Windows.Forms.DataGridView dg_FutureMargins;
        private System.Windows.Forms.Button bt_ProcessOutstandingTrades;
        private System.Windows.Forms.Button bt_Check;
    }
}