namespace T1MultiAsset
{
    partial class MaintainSecurities
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
            this.dg_Securities = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Securities)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_Request
            // 
            this.bt_Request.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Request.ForeColor = System.Drawing.Color.Blue;
            this.bt_Request.Location = new System.Drawing.Point(952, 12);
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
            this.bt_Save.Location = new System.Drawing.Point(952, 199);
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
            // dg_Securities
            // 
            this.dg_Securities.AllowUserToAddRows = false;
            this.dg_Securities.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_Securities.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_Securities.Location = new System.Drawing.Point(12, 48);
            this.dg_Securities.Name = "dg_Securities";
            this.dg_Securities.Size = new System.Drawing.Size(1024, 145);
            this.dg_Securities.TabIndex = 4;
            this.dg_Securities.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dg_Securities_CellBeginEdit);
            this.dg_Securities.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_Securities_CellEndEdit);
            this.dg_Securities.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dg_Securities_DataError);
            // 
            // MaintainSecurities
            // 
            this.AcceptButton = this.bt_Request;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 234);
            this.Controls.Add(this.dg_Securities);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_BBG_Ticker);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.bt_Request);
            this.Name = "MaintainSecurities";
            this.Text = "MaintainSecurities";
            this.Shown += new System.EventHandler(this.MaintainSecurities_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MaintainSecurities_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dg_Securities)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_Request;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.TextBox tb_BBG_Ticker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dg_Securities;
    }
}