namespace T1MultiAsset
{
    partial class ForwardDividends
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ForwardDividends));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.dg_Dividends = new System.Windows.Forms.DataGridView();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.bt_Save = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dg_MissingDivs = new System.Windows.Forms.DataGridView();
            this.lb_MissingDivs = new System.Windows.Forms.Label();
            this.bt_Reconcile = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bt_Update = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dg_Transactions = new System.Windows.Forms.DataGridView();
            this.sc_Reconcile = new System.Windows.Forms.SplitContainer();
            this.lb_Amount = new System.Windows.Forms.Label();
            this.tb_Amount = new System.Windows.Forms.TextBox();
            this.lb_TotalAmount = new System.Windows.Forms.Label();
            this.tb_TotalAmount = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Dividends)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_MissingDivs)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Transactions)).BeginInit();
            this.sc_Reconcile.Panel1.SuspendLayout();
            this.sc_Reconcile.Panel2.SuspendLayout();
            this.sc_Reconcile.SuspendLayout();
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
            this.richTextBox1.Size = new System.Drawing.Size(1229, 147);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // dg_Dividends
            // 
            this.dg_Dividends.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_Dividends.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dg_Dividends.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_Dividends.DefaultCellStyle = dataGridViewCellStyle2;
            this.dg_Dividends.Location = new System.Drawing.Point(3, 3);
            this.dg_Dividends.Name = "dg_Dividends";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.MediumSeaGreen;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_Dividends.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dg_Dividends.Size = new System.Drawing.Size(1226, 226);
            this.dg_Dividends.TabIndex = 5;
            this.dg_Dividends.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dg_Dividends_UserDeletingRow);
            this.dg_Dividends.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_Dividends_CellMouseClick);
            this.dg_Dividends.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dg_Dividends_CellBeginEdit);
            this.dg_Dividends.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dg_Dividends_UserAddedRow);
            this.dg_Dividends.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dg_Dividends_MouseClick);
            this.dg_Dividends.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_Dividends_CellEndEdit);
            this.dg_Dividends.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dg_Dividends_EditingControlShowing);
            this.dg_Dividends.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dg_Dividends_DataError);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Location = new System.Drawing.Point(93, 189);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(208, 20);
            this.dateTimePicker1.TabIndex = 7;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // bt_Save
            // 
            this.bt_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Save.ForeColor = System.Drawing.Color.MediumBlue;
            this.bt_Save.Location = new System.Drawing.Point(12, 188);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(75, 23);
            this.bt_Save.TabIndex = 6;
            this.bt_Save.Text = "Save";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(90, 173);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "From Date:";
            // 
            // dg_MissingDivs
            // 
            this.dg_MissingDivs.AllowUserToAddRows = false;
            this.dg_MissingDivs.AllowUserToDeleteRows = false;
            this.dg_MissingDivs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_MissingDivs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dg_MissingDivs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_MissingDivs.DefaultCellStyle = dataGridViewCellStyle5;
            this.dg_MissingDivs.Location = new System.Drawing.Point(6, 32);
            this.dg_MissingDivs.Name = "dg_MissingDivs";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.MediumSeaGreen;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_MissingDivs.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dg_MissingDivs.RowHeadersVisible = false;
            this.dg_MissingDivs.Size = new System.Drawing.Size(1223, 115);
            this.dg_MissingDivs.TabIndex = 9;
            this.dg_MissingDivs.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_MissingDivs_CellMouseClick);
            this.dg_MissingDivs.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_MissingDivs_RowEnter);
            this.dg_MissingDivs.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dg_MissingDivs_MouseClick);
            this.dg_MissingDivs.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_MissingDivs_CellClick);
            // 
            // lb_MissingDivs
            // 
            this.lb_MissingDivs.AutoSize = true;
            this.lb_MissingDivs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_MissingDivs.ForeColor = System.Drawing.Color.Blue;
            this.lb_MissingDivs.Location = new System.Drawing.Point(8, 8);
            this.lb_MissingDivs.Name = "lb_MissingDivs";
            this.lb_MissingDivs.Size = new System.Drawing.Size(755, 13);
            this.lb_MissingDivs.TabIndex = 10;
            this.lb_MissingDivs.Text = "To apply, Add this Dividend to the Window above and [Save]; otherwise to remove t" +
                "his message,  mark as reconcilled and [Confirm]";
            // 
            // bt_Reconcile
            // 
            this.bt_Reconcile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Reconcile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Reconcile.ForeColor = System.Drawing.Color.MediumBlue;
            this.bt_Reconcile.Location = new System.Drawing.Point(1154, 3);
            this.bt_Reconcile.Name = "bt_Reconcile";
            this.bt_Reconcile.Size = new System.Drawing.Size(75, 23);
            this.bt_Reconcile.TabIndex = 11;
            this.bt_Reconcile.Text = "Confirm";
            this.bt_Reconcile.UseVisualStyleBackColor = true;
            this.bt_Reconcile.Click += new System.EventHandler(this.bt_Reconcile_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 212);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dg_Dividends);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.sc_Reconcile);
            this.splitContainer1.Size = new System.Drawing.Size(1232, 535);
            this.splitContainer1.SplitterDistance = 231;
            this.splitContainer1.TabIndex = 12;
            // 
            // bt_Update
            // 
            this.bt_Update.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Update.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Update.ForeColor = System.Drawing.Color.MediumBlue;
            this.bt_Update.Location = new System.Drawing.Point(1155, 2);
            this.bt_Update.Name = "bt_Update";
            this.bt_Update.Size = new System.Drawing.Size(75, 23);
            this.bt_Update.TabIndex = 14;
            this.bt_Update.Text = "Update";
            this.bt_Update.UseVisualStyleBackColor = true;
            this.bt_Update.Click += new System.EventHandler(this.bt_Update_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(282, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Alternatively Alter the Amount on the transaction";
            // 
            // dg_Transactions
            // 
            this.dg_Transactions.AllowUserToAddRows = false;
            this.dg_Transactions.AllowUserToDeleteRows = false;
            this.dg_Transactions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_Transactions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dg_Transactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_Transactions.DefaultCellStyle = dataGridViewCellStyle8;
            this.dg_Transactions.Location = new System.Drawing.Point(3, 31);
            this.dg_Transactions.Name = "dg_Transactions";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.MediumSeaGreen;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_Transactions.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dg_Transactions.RowHeadersVisible = false;
            this.dg_Transactions.Size = new System.Drawing.Size(1226, 112);
            this.dg_Transactions.TabIndex = 12;
            // 
            // sc_Reconcile
            // 
            this.sc_Reconcile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sc_Reconcile.Location = new System.Drawing.Point(0, 0);
            this.sc_Reconcile.Name = "sc_Reconcile";
            this.sc_Reconcile.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // sc_Reconcile.Panel1
            // 
            this.sc_Reconcile.Panel1.Controls.Add(this.lb_TotalAmount);
            this.sc_Reconcile.Panel1.Controls.Add(this.tb_TotalAmount);
            this.sc_Reconcile.Panel1.Controls.Add(this.dg_MissingDivs);
            this.sc_Reconcile.Panel1.Controls.Add(this.lb_MissingDivs);
            this.sc_Reconcile.Panel1.Controls.Add(this.bt_Reconcile);
            // 
            // sc_Reconcile.Panel2
            // 
            this.sc_Reconcile.Panel2.Controls.Add(this.lb_Amount);
            this.sc_Reconcile.Panel2.Controls.Add(this.tb_Amount);
            this.sc_Reconcile.Panel2.Controls.Add(this.bt_Update);
            this.sc_Reconcile.Panel2.Controls.Add(this.label2);
            this.sc_Reconcile.Panel2.Controls.Add(this.dg_Transactions);
            this.sc_Reconcile.Size = new System.Drawing.Size(1232, 300);
            this.sc_Reconcile.SplitterDistance = 150;
            this.sc_Reconcile.TabIndex = 15;
            // 
            // lb_Amount
            // 
            this.lb_Amount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_Amount.AutoSize = true;
            this.lb_Amount.ForeColor = System.Drawing.Color.DarkGreen;
            this.lb_Amount.Location = new System.Drawing.Point(967, 7);
            this.lb_Amount.Name = "lb_Amount";
            this.lb_Amount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lb_Amount.Size = new System.Drawing.Size(70, 13);
            this.lb_Amount.TabIndex = 34;
            this.lb_Amount.Text = "Total Amount";
            // 
            // tb_Amount
            // 
            this.tb_Amount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_Amount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Amount.ForeColor = System.Drawing.Color.DarkGreen;
            this.tb_Amount.Location = new System.Drawing.Point(1047, 4);
            this.tb_Amount.Name = "tb_Amount";
            this.tb_Amount.Size = new System.Drawing.Size(94, 20);
            this.tb_Amount.TabIndex = 33;
            // 
            // lb_TotalAmount
            // 
            this.lb_TotalAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_TotalAmount.AutoSize = true;
            this.lb_TotalAmount.ForeColor = System.Drawing.Color.DarkGreen;
            this.lb_TotalAmount.Location = new System.Drawing.Point(967, 9);
            this.lb_TotalAmount.Name = "lb_TotalAmount";
            this.lb_TotalAmount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lb_TotalAmount.Size = new System.Drawing.Size(70, 13);
            this.lb_TotalAmount.TabIndex = 36;
            this.lb_TotalAmount.Text = "Total Amount";
            // 
            // tb_TotalAmount
            // 
            this.tb_TotalAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_TotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_TotalAmount.ForeColor = System.Drawing.Color.DarkGreen;
            this.tb_TotalAmount.Location = new System.Drawing.Point(1047, 6);
            this.tb_TotalAmount.Name = "tb_TotalAmount";
            this.tb_TotalAmount.Size = new System.Drawing.Size(94, 20);
            this.tb_TotalAmount.TabIndex = 35;
            // 
            // ForwardDividends
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1245, 755);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.richTextBox1);
            this.Name = "ForwardDividends";
            this.Text = "Forward Dividends";
            this.Load += new System.EventHandler(this.ForwardDividends_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ForwardDividends_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dg_Dividends)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_MissingDivs)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg_Transactions)).EndInit();
            this.sc_Reconcile.Panel1.ResumeLayout(false);
            this.sc_Reconcile.Panel1.PerformLayout();
            this.sc_Reconcile.Panel2.ResumeLayout(false);
            this.sc_Reconcile.Panel2.PerformLayout();
            this.sc_Reconcile.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.DataGridView dg_Dividends;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dg_MissingDivs;
        private System.Windows.Forms.Label lb_MissingDivs;
        private System.Windows.Forms.Button bt_Reconcile;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dg_Transactions;
        private System.Windows.Forms.Button bt_Update;
        private System.Windows.Forms.SplitContainer sc_Reconcile;
        private System.Windows.Forms.Label lb_TotalAmount;
        private System.Windows.Forms.TextBox tb_TotalAmount;
        private System.Windows.Forms.Label lb_Amount;
        private System.Windows.Forms.TextBox tb_Amount;
    }
}