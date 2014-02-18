namespace T1MultiAsset
{
    partial class NAVReconcileReport
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
            this.cb_Fund = new System.Windows.Forms.ComboBox();
            this.dtp_AsAtDate = new System.Windows.Forms.DateTimePicker();
            this.crv_NAV_Rec = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // bt_Request
            // 
            this.bt_Request.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Request.ForeColor = System.Drawing.Color.DarkBlue;
            this.bt_Request.Location = new System.Drawing.Point(493, 10);
            this.bt_Request.Name = "bt_Request";
            this.bt_Request.Size = new System.Drawing.Size(75, 23);
            this.bt_Request.TabIndex = 0;
            this.bt_Request.Text = "Request";
            this.bt_Request.UseVisualStyleBackColor = true;
            this.bt_Request.Click += new System.EventHandler(this.bt_Request_Click);
            // 
            // cb_Fund
            // 
            this.cb_Fund.FormattingEnabled = true;
            this.cb_Fund.Location = new System.Drawing.Point(12, 12);
            this.cb_Fund.Name = "cb_Fund";
            this.cb_Fund.Size = new System.Drawing.Size(269, 21);
            this.cb_Fund.TabIndex = 1;
            // 
            // dtp_AsAtDate
            // 
            this.dtp_AsAtDate.Location = new System.Drawing.Point(287, 13);
            this.dtp_AsAtDate.Name = "dtp_AsAtDate";
            this.dtp_AsAtDate.Size = new System.Drawing.Size(200, 20);
            this.dtp_AsAtDate.TabIndex = 2;
            // 
            // crv_NAV_Rec
            // 
            this.crv_NAV_Rec.ActiveViewIndex = -1;
            this.crv_NAV_Rec.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.crv_NAV_Rec.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crv_NAV_Rec.DisplayBackgroundEdge = false;
            this.crv_NAV_Rec.DisplayGroupTree = false;
            this.crv_NAV_Rec.Location = new System.Drawing.Point(12, 48);
            this.crv_NAV_Rec.Name = "crv_NAV_Rec";
            this.crv_NAV_Rec.SelectionFormula = "";
            this.crv_NAV_Rec.ShowCloseButton = false;
            this.crv_NAV_Rec.ShowGroupTreeButton = false;
            this.crv_NAV_Rec.ShowRefreshButton = false;
            this.crv_NAV_Rec.Size = new System.Drawing.Size(1194, 604);
            this.crv_NAV_Rec.TabIndex = 17;
            this.crv_NAV_Rec.ViewTimeSelectionFormula = "";
            // 
            // NAVReconcileReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1218, 654);
            this.Controls.Add(this.crv_NAV_Rec);
            this.Controls.Add(this.dtp_AsAtDate);
            this.Controls.Add(this.cb_Fund);
            this.Controls.Add(this.bt_Request);
            this.Name = "NAVReconcileReport";
            this.Text = "NAVReconcileReport";
            this.Load += new System.EventHandler(this.NAVReconcileReport_Load);
            this.Shown += new System.EventHandler(this.NAVReconcileReport_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bt_Request;
        private System.Windows.Forms.ComboBox cb_Fund;
        private System.Windows.Forms.DateTimePicker dtp_AsAtDate;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crv_NAV_Rec;
    }
}