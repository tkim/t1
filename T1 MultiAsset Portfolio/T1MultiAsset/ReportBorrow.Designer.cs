namespace T1MultiAsset
{
    partial class ReportBorrow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportBorrow));
            this.bt_Refresh = new System.Windows.Forms.Button();
            this.dg_StockBorrow = new System.Windows.Forms.DataGridView();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.bt_Acknowledge = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dg_StockBorrow)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_Refresh
            // 
            this.bt_Refresh.ForeColor = System.Drawing.Color.RoyalBlue;
            this.bt_Refresh.Location = new System.Drawing.Point(3, 3);
            this.bt_Refresh.Name = "bt_Refresh";
            this.bt_Refresh.Size = new System.Drawing.Size(75, 23);
            this.bt_Refresh.TabIndex = 0;
            this.bt_Refresh.Text = "Refresh";
            this.bt_Refresh.UseVisualStyleBackColor = true;
            this.bt_Refresh.Click += new System.EventHandler(this.bt_Refresh_Click);
            // 
            // dg_StockBorrow
            // 
            this.dg_StockBorrow.AllowUserToAddRows = false;
            this.dg_StockBorrow.AllowUserToDeleteRows = false;
            this.dg_StockBorrow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_StockBorrow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_StockBorrow.Location = new System.Drawing.Point(3, 101);
            this.dg_StockBorrow.Name = "dg_StockBorrow";
            this.dg_StockBorrow.ReadOnly = true;
            this.dg_StockBorrow.RowHeadersVisible = false;
            this.dg_StockBorrow.Size = new System.Drawing.Size(1253, 216);
            this.dg_StockBorrow.TabIndex = 1;
            this.dg_StockBorrow.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_StockBorrow_CellMouseClick);
            this.dg_StockBorrow.Sorted += new System.EventHandler(this.dg_StockBorrow_Sorted);
            this.dg_StockBorrow.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dg_StockBorrow_MouseClick);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.Green;
            this.richTextBox1.Location = new System.Drawing.Point(114, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(1142, 92);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // bt_Acknowledge
            // 
            this.bt_Acknowledge.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Acknowledge.ForeColor = System.Drawing.Color.Green;
            this.bt_Acknowledge.Location = new System.Drawing.Point(3, 72);
            this.bt_Acknowledge.Name = "bt_Acknowledge";
            this.bt_Acknowledge.Size = new System.Drawing.Size(105, 23);
            this.bt_Acknowledge.TabIndex = 5;
            this.bt_Acknowledge.Text = "Acknowledge";
            this.bt_Acknowledge.UseVisualStyleBackColor = true;
            this.bt_Acknowledge.Click += new System.EventHandler(this.bt_Acknowledge_Click);
            // 
            // ReportBorrow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1258, 324);
            this.Controls.Add(this.bt_Acknowledge);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.dg_StockBorrow);
            this.Controls.Add(this.bt_Refresh);
            this.Name = "ReportBorrow";
            this.Text = "Borrow Report";
            this.Load += new System.EventHandler(this.ReportBorrow_Load);
            this.Shown += new System.EventHandler(this.ReportBorrow_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ReportBorrow_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dg_StockBorrow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bt_Refresh;
        private System.Windows.Forms.DataGridView dg_StockBorrow;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button bt_Acknowledge;
    }
}