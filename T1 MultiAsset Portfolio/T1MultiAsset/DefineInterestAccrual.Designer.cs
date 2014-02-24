namespace T1MultiAsset
{
    partial class DefineInterestAccrual
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefineInterestAccrual));
            this.cb_Fund = new System.Windows.Forms.ComboBox();
            this.cb_Accounts = new System.Windows.Forms.ComboBox();
            this.cb_crncy = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.bt_Save = new System.Windows.Forms.Button();
            this.bt_Request = new System.Windows.Forms.Button();
            this.dg_AccountInterest = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_Selected = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_RemoveReconcilled = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dg_AccountInterest)).BeginInit();
            this.SuspendLayout();
            // 
            // cb_Fund
            // 
            this.cb_Fund.FormattingEnabled = true;
            this.cb_Fund.Location = new System.Drawing.Point(12, 12);
            this.cb_Fund.Name = "cb_Fund";
            this.cb_Fund.Size = new System.Drawing.Size(208, 21);
            this.cb_Fund.TabIndex = 0;
            this.cb_Fund.SelectedIndexChanged += new System.EventHandler(this.cb_Fund_SelectedIndexChanged);
            // 
            // cb_Accounts
            // 
            this.cb_Accounts.FormattingEnabled = true;
            this.cb_Accounts.Location = new System.Drawing.Point(226, 12);
            this.cb_Accounts.Name = "cb_Accounts";
            this.cb_Accounts.Size = new System.Drawing.Size(190, 21);
            this.cb_Accounts.TabIndex = 1;
            this.cb_Accounts.SelectedIndexChanged += new System.EventHandler(this.cb_Account_SelectedIndexChanged);
            // 
            // cb_crncy
            // 
            this.cb_crncy.FormattingEnabled = true;
            this.cb_crncy.Location = new System.Drawing.Point(422, 12);
            this.cb_crncy.Name = "cb_crncy";
            this.cb_crncy.Size = new System.Drawing.Size(69, 21);
            this.cb_crncy.TabIndex = 2;
            this.cb_crncy.SelectedIndexChanged += new System.EventHandler(this.cb_crncy_SelectedIndexChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker1.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Location = new System.Drawing.Point(689, 41);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(208, 20);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // bt_Save
            // 
            this.bt_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Save.ForeColor = System.Drawing.Color.Green;
            this.bt_Save.Location = new System.Drawing.Point(829, 7);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(68, 28);
            this.bt_Save.TabIndex = 4;
            this.bt_Save.Text = "Save";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // bt_Request
            // 
            this.bt_Request.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Request.ForeColor = System.Drawing.Color.MediumBlue;
            this.bt_Request.Location = new System.Drawing.Point(12, 39);
            this.bt_Request.Name = "bt_Request";
            this.bt_Request.Size = new System.Drawing.Size(68, 28);
            this.bt_Request.TabIndex = 5;
            this.bt_Request.Text = "Request";
            this.bt_Request.UseVisualStyleBackColor = true;
            this.bt_Request.Click += new System.EventHandler(this.bt_Request_Click);
            // 
            // dg_AccountInterest
            // 
            this.dg_AccountInterest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.MidnightBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_AccountInterest.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dg_AccountInterest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_AccountInterest.Location = new System.Drawing.Point(12, 219);
            this.dg_AccountInterest.MultiSelect = false;
            this.dg_AccountInterest.Name = "dg_AccountInterest";
            this.dg_AccountInterest.Size = new System.Drawing.Size(885, 354);
            this.dg_AccountInterest.TabIndex = 6;
            this.dg_AccountInterest.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dg_AccountInterest_CellBeginEdit);
            this.dg_AccountInterest.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_AccountInterest_CellEndEdit);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Green;
            this.label1.Location = new System.Drawing.Point(578, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Apply Updates From:";
            // 
            // lb_Selected
            // 
            this.lb_Selected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_Selected.AutoSize = true;
            this.lb_Selected.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Selected.ForeColor = System.Drawing.Color.DarkBlue;
            this.lb_Selected.Location = new System.Drawing.Point(12, 199);
            this.lb_Selected.Name = "lb_Selected";
            this.lb_Selected.Size = new System.Drawing.Size(81, 17);
            this.lb_Selected.TabIndex = 8;
            this.lb_Selected.Text = "Selected: ";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.ForeColor = System.Drawing.Color.DarkGreen;
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(719, 124);
            this.label2.TabIndex = 9;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // cb_RemoveReconcilled
            // 
            this.cb_RemoveReconcilled.AutoSize = true;
            this.cb_RemoveReconcilled.CheckAlign = System.Drawing.ContentAlignment.TopRight;
            this.cb_RemoveReconcilled.ForeColor = System.Drawing.Color.Maroon;
            this.cb_RemoveReconcilled.Location = new System.Drawing.Point(634, 14);
            this.cb_RemoveReconcilled.Name = "cb_RemoveReconcilled";
            this.cb_RemoveReconcilled.Size = new System.Drawing.Size(189, 17);
            this.cb_RemoveReconcilled.TabIndex = 10;
            this.cb_RemoveReconcilled.Text = "Remove Reconcilled Transactions";
            this.cb_RemoveReconcilled.UseVisualStyleBackColor = true;
            // 
            // DefineInterestAccrual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 585);
            this.Controls.Add(this.cb_RemoveReconcilled);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lb_Selected);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dg_AccountInterest);
            this.Controls.Add(this.bt_Request);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.cb_crncy);
            this.Controls.Add(this.cb_Accounts);
            this.Controls.Add(this.cb_Fund);
            this.Name = "DefineInterestAccrual";
            this.Text = "Define Interest Accruals";
            this.Load += new System.EventHandler(this.DefineInterestAccrual_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dg_AccountInterest)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_Fund;
        private System.Windows.Forms.ComboBox cb_Accounts;
        private System.Windows.Forms.ComboBox cb_crncy;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.Button bt_Request;
        private System.Windows.Forms.DataGridView dg_AccountInterest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_Selected;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cb_RemoveReconcilled;
    }
}