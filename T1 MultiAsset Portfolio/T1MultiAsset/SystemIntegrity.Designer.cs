namespace T1MultiAsset
{
    partial class SystemIntegrity
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
            this.bt_Refresh = new System.Windows.Forms.Button();
            this.bt_Close = new System.Windows.Forms.Button();
            this.dgv_SystemIntegrity = new System.Windows.Forms.DataGridView();
            this.bt_Repair = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SystemIntegrity)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_Refresh
            // 
            this.bt_Refresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Refresh.ForeColor = System.Drawing.Color.RoyalBlue;
            this.bt_Refresh.Location = new System.Drawing.Point(12, 12);
            this.bt_Refresh.Name = "bt_Refresh";
            this.bt_Refresh.Size = new System.Drawing.Size(68, 27);
            this.bt_Refresh.TabIndex = 0;
            this.bt_Refresh.Text = "Refresh";
            this.bt_Refresh.UseVisualStyleBackColor = true;
            this.bt_Refresh.Click += new System.EventHandler(this.bt_Refresh_Click);
            // 
            // bt_Close
            // 
            this.bt_Close.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Close.ForeColor = System.Drawing.Color.RoyalBlue;
            this.bt_Close.Location = new System.Drawing.Point(662, 12);
            this.bt_Close.Name = "bt_Close";
            this.bt_Close.Size = new System.Drawing.Size(68, 27);
            this.bt_Close.TabIndex = 1;
            this.bt_Close.Text = "Close";
            this.bt_Close.UseVisualStyleBackColor = true;
            this.bt_Close.Click += new System.EventHandler(this.bt_Close_Click);
            // 
            // dgv_SystemIntegrity
            // 
            this.dgv_SystemIntegrity.AllowUserToAddRows = false;
            this.dgv_SystemIntegrity.AllowUserToDeleteRows = false;
            this.dgv_SystemIntegrity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_SystemIntegrity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_SystemIntegrity.Location = new System.Drawing.Point(12, 45);
            this.dgv_SystemIntegrity.Name = "dgv_SystemIntegrity";
            this.dgv_SystemIntegrity.ReadOnly = true;
            this.dgv_SystemIntegrity.RowHeadersVisible = false;
            this.dgv_SystemIntegrity.Size = new System.Drawing.Size(718, 278);
            this.dgv_SystemIntegrity.TabIndex = 2;
            // 
            // bt_Repair
            // 
            this.bt_Repair.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Repair.ForeColor = System.Drawing.Color.Green;
            this.bt_Repair.Location = new System.Drawing.Point(184, 12);
            this.bt_Repair.Name = "bt_Repair";
            this.bt_Repair.Size = new System.Drawing.Size(68, 27);
            this.bt_Repair.TabIndex = 3;
            this.bt_Repair.Text = "Repair";
            this.bt_Repair.UseVisualStyleBackColor = true;
            this.bt_Repair.Click += new System.EventHandler(this.bt_Repair_Click);
            // 
            // SystemIntegrity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 325);
            this.Controls.Add(this.bt_Repair);
            this.Controls.Add(this.dgv_SystemIntegrity);
            this.Controls.Add(this.bt_Close);
            this.Controls.Add(this.bt_Refresh);
            this.Name = "SystemIntegrity";
            this.Text = "System Integrity Checker";
            this.Load += new System.EventHandler(this.SystemIntegrity_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SystemIntegrity_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SystemIntegrity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bt_Refresh;
        private System.Windows.Forms.Button bt_Close;
        private System.Windows.Forms.DataGridView dgv_SystemIntegrity;
        private System.Windows.Forms.Button bt_Repair;
    }
}