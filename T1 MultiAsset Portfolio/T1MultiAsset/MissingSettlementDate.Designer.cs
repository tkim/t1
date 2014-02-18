namespace T1MultiAsset
{
    partial class MissingSettlementDate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MissingSettlementDate));
            this.dg_ApplSettlementDate = new System.Windows.Forms.DataGridView();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.bt_Apply = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dg_ApplSettlementDate)).BeginInit();
            this.SuspendLayout();
            // 
            // dg_ApplSettlementDate
            // 
            this.dg_ApplSettlementDate.AllowUserToAddRows = false;
            this.dg_ApplSettlementDate.AllowUserToDeleteRows = false;
            this.dg_ApplSettlementDate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_ApplSettlementDate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_ApplSettlementDate.Location = new System.Drawing.Point(12, 198);
            this.dg_ApplSettlementDate.MultiSelect = false;
            this.dg_ApplSettlementDate.Name = "dg_ApplSettlementDate";
            this.dg_ApplSettlementDate.RowHeadersVisible = false;
            this.dg_ApplSettlementDate.Size = new System.Drawing.Size(904, 288);
            this.dg_ApplSettlementDate.TabIndex = 0;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.Green;
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(904, 149);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // bt_Apply
            // 
            this.bt_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Apply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Apply.ForeColor = System.Drawing.Color.Green;
            this.bt_Apply.Location = new System.Drawing.Point(836, 167);
            this.bt_Apply.Name = "bt_Apply";
            this.bt_Apply.Size = new System.Drawing.Size(80, 25);
            this.bt_Apply.TabIndex = 6;
            this.bt_Apply.Text = "Apply";
            this.bt_Apply.UseVisualStyleBackColor = true;
            this.bt_Apply.Click += new System.EventHandler(this.bt_Apply_Click);
            // 
            // MissingSettlementDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 498);
            this.Controls.Add(this.bt_Apply);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.dg_ApplSettlementDate);
            this.Name = "MissingSettlementDate";
            this.Text = "Missing Settlement Date";
            this.Load += new System.EventHandler(this.MissingSettlementDate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dg_ApplSettlementDate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_ApplSettlementDate;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button bt_Apply;
    }
}