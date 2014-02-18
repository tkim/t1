namespace T1MultiAsset
{
    partial class ASICShortReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ASICShortReport));
            this.button1 = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dg_ASICShorts = new System.Windows.Forms.DataGridView();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.cb_AllMissingDates = new System.Windows.Forms.CheckBox();
            this.bt_ASIX = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dg_ASICShorts)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.MediumBlue;
            this.button1.Location = new System.Drawing.Point(9, 252);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Location = new System.Drawing.Point(90, 253);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(208, 20);
            this.dateTimePicker1.TabIndex = 1;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // dg_ASICShorts
            // 
            this.dg_ASICShorts.AllowUserToAddRows = false;
            this.dg_ASICShorts.AllowUserToDeleteRows = false;
            this.dg_ASICShorts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_ASICShorts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_ASICShorts.Location = new System.Drawing.Point(8, 282);
            this.dg_ASICShorts.Name = "dg_ASICShorts";
            this.dg_ASICShorts.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.MediumSeaGreen;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_ASICShorts.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dg_ASICShorts.Size = new System.Drawing.Size(739, 245);
            this.dg_ASICShorts.TabIndex = 2;
            this.dg_ASICShorts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dg_ASICShorts_MouseDown);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.Green;
            this.richTextBox1.Location = new System.Drawing.Point(8, 9);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(739, 237);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // cb_AllMissingDates
            // 
            this.cb_AllMissingDates.AutoSize = true;
            this.cb_AllMissingDates.Checked = true;
            this.cb_AllMissingDates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_AllMissingDates.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_AllMissingDates.ForeColor = System.Drawing.Color.MediumBlue;
            this.cb_AllMissingDates.Location = new System.Drawing.Point(321, 256);
            this.cb_AllMissingDates.Name = "cb_AllMissingDates";
            this.cb_AllMissingDates.Size = new System.Drawing.Size(123, 17);
            this.cb_AllMissingDates.TabIndex = 4;
            this.cb_AllMissingDates.Text = "All Missing Dates";
            this.cb_AllMissingDates.UseVisualStyleBackColor = true;
            this.cb_AllMissingDates.CheckedChanged += new System.EventHandler(this.cb_AllMissingDates_CheckedChanged);
            // 
            // bt_ASIX
            // 
            this.bt_ASIX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_ASIX.ForeColor = System.Drawing.Color.Green;
            this.bt_ASIX.Location = new System.Drawing.Point(205, 100);
            this.bt_ASIX.Name = "bt_ASIX";
            this.bt_ASIX.Size = new System.Drawing.Size(74, 22);
            this.bt_ASIX.TabIndex = 5;
            this.bt_ASIX.Text = "ASIX<go>";
            this.bt_ASIX.UseVisualStyleBackColor = true;
            this.bt_ASIX.Click += new System.EventHandler(this.bt_ASIX_Click);
            // 
            // ASICShortReport
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 530);
            this.Controls.Add(this.bt_ASIX);
            this.Controls.Add(this.cb_AllMissingDates);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.dg_ASICShorts);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.button1);
            this.Name = "ASICShortReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ASIC Short Report";
            this.Load += new System.EventHandler(this.ASICShortReport_Load);
            this.Shown += new System.EventHandler(this.ASICShortReport_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ASICShortReport_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dg_ASICShorts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DataGridView dg_ASICShorts;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox cb_AllMissingDates;
        private System.Windows.Forms.Button bt_ASIX;
    }
}