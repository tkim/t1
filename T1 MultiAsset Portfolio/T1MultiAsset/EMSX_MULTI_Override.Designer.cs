namespace T1MultiAsset
{
    partial class EMSX_MULTI_Override
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EMSX_MULTI_Override));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.dg_MULTI = new System.Windows.Forms.DataGridView();
            this.bt_Save = new System.Windows.Forms.Button();
            this.cb_Mapped = new System.Windows.Forms.CheckBox();
            this.bt_ViewBrokerMapping = new System.Windows.Forms.Button();
            this.bt_EMSX = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dg_MULTI)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.Green;
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(723, 186);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // dg_MULTI
            // 
            this.dg_MULTI.AllowUserToAddRows = false;
            this.dg_MULTI.AllowUserToDeleteRows = false;
            this.dg_MULTI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_MULTI.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_MULTI.Location = new System.Drawing.Point(12, 261);
            this.dg_MULTI.MultiSelect = false;
            this.dg_MULTI.Name = "dg_MULTI";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.MediumSeaGreen;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_MULTI.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dg_MULTI.Size = new System.Drawing.Size(723, 173);
            this.dg_MULTI.TabIndex = 6;
            this.dg_MULTI.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_MULTI_CellEndEdit);
            // 
            // bt_Save
            // 
            this.bt_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Save.Enabled = false;
            this.bt_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Save.ForeColor = System.Drawing.Color.Green;
            this.bt_Save.Location = new System.Drawing.Point(647, 232);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(75, 23);
            this.bt_Save.TabIndex = 5;
            this.bt_Save.Text = "Save";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // cb_Mapped
            // 
            this.cb_Mapped.AutoSize = true;
            this.cb_Mapped.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Mapped.ForeColor = System.Drawing.Color.MediumBlue;
            this.cb_Mapped.Location = new System.Drawing.Point(12, 232);
            this.cb_Mapped.Name = "cb_Mapped";
            this.cb_Mapped.Size = new System.Drawing.Size(257, 17);
            this.cb_Mapped.TabIndex = 7;
            this.cb_Mapped.Text = "Show Previously Mapped Strategies (Today only)";
            this.cb_Mapped.UseVisualStyleBackColor = true;
            this.cb_Mapped.CheckedChanged += new System.EventHandler(this.cb_Mapped_CheckedChanged);
            // 
            // bt_ViewBrokerMapping
            // 
            this.bt_ViewBrokerMapping.ForeColor = System.Drawing.Color.MediumBlue;
            this.bt_ViewBrokerMapping.Location = new System.Drawing.Point(286, 231);
            this.bt_ViewBrokerMapping.Name = "bt_ViewBrokerMapping";
            this.bt_ViewBrokerMapping.Size = new System.Drawing.Size(124, 23);
            this.bt_ViewBrokerMapping.TabIndex = 8;
            this.bt_ViewBrokerMapping.Text = "View Broker Mapping";
            this.bt_ViewBrokerMapping.UseVisualStyleBackColor = true;
            this.bt_ViewBrokerMapping.Click += new System.EventHandler(this.bt_ViewBrokerMapping_Click);
            // 
            // bt_EMSX
            // 
            this.bt_EMSX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_EMSX.BackColor = System.Drawing.Color.MediumOrchid;
            this.bt_EMSX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_EMSX.ForeColor = System.Drawing.Color.Black;
            this.bt_EMSX.Location = new System.Drawing.Point(416, 231);
            this.bt_EMSX.Name = "bt_EMSX";
            this.bt_EMSX.Size = new System.Drawing.Size(58, 23);
            this.bt_EMSX.TabIndex = 24;
            this.bt_EMSX.Text = "EMSX";
            this.bt_EMSX.UseVisualStyleBackColor = false;
            this.bt_EMSX.Click += new System.EventHandler(this.bt_EMSX_Click);
            // 
            // EMSX_MULTI_Override
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 443);
            this.Controls.Add(this.bt_EMSX);
            this.Controls.Add(this.bt_ViewBrokerMapping);
            this.Controls.Add(this.cb_Mapped);
            this.Controls.Add(this.dg_MULTI);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.richTextBox1);
            this.Name = "EMSX_MULTI_Override";
            this.Text = "EMSX StrategyType \'MULTI\' Override";
            this.Load += new System.EventHandler(this.EMSX_MULTI_Override_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dg_MULTI)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.DataGridView dg_MULTI;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.CheckBox cb_Mapped;
        private System.Windows.Forms.Button bt_ViewBrokerMapping;
        private System.Windows.Forms.Button bt_EMSX;
    }
}