namespace T1MultiAsset
{
    partial class SetDebugLevel
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
            this.cb_DebugOn = new System.Windows.Forms.CheckBox();
            this.nUD_debugLevel = new System.Windows.Forms.NumericUpDown();
            this.bt_Close = new System.Windows.Forms.Button();
            this.gb_Console = new System.Windows.Forms.GroupBox();
            this.lb_Database = new System.Windows.Forms.Label();
            this.tb_stdError = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_stdOut = new System.Windows.Forms.TextBox();
            this.cb_SendConsoleToFile = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_debugLevel)).BeginInit();
            this.gb_Console.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_DebugOn
            // 
            this.cb_DebugOn.AutoSize = true;
            this.cb_DebugOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_DebugOn.ForeColor = System.Drawing.Color.MediumBlue;
            this.cb_DebugOn.Location = new System.Drawing.Point(12, 12);
            this.cb_DebugOn.Name = "cb_DebugOn";
            this.cb_DebugOn.Size = new System.Drawing.Size(118, 17);
            this.cb_DebugOn.TabIndex = 0;
            this.cb_DebugOn.Text = "Debugging is on";
            this.cb_DebugOn.UseVisualStyleBackColor = true;
            this.cb_DebugOn.CheckedChanged += new System.EventHandler(this.cb_DebugOn_CheckedChanged);
            // 
            // nUD_debugLevel
            // 
            this.nUD_debugLevel.Enabled = false;
            this.nUD_debugLevel.Location = new System.Drawing.Point(146, 12);
            this.nUD_debugLevel.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nUD_debugLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nUD_debugLevel.Name = "nUD_debugLevel";
            this.nUD_debugLevel.Size = new System.Drawing.Size(62, 20);
            this.nUD_debugLevel.TabIndex = 1;
            this.nUD_debugLevel.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nUD_debugLevel.ValueChanged += new System.EventHandler(this.nUD_debugLevel_ValueChanged);
            // 
            // bt_Close
            // 
            this.bt_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_Close.ForeColor = System.Drawing.Color.MediumBlue;
            this.bt_Close.Location = new System.Drawing.Point(12, 238);
            this.bt_Close.Name = "bt_Close";
            this.bt_Close.Size = new System.Drawing.Size(75, 23);
            this.bt_Close.TabIndex = 2;
            this.bt_Close.Text = "Close";
            this.bt_Close.UseVisualStyleBackColor = true;
            this.bt_Close.Click += new System.EventHandler(this.cb_Close_Click);
            // 
            // gb_Console
            // 
            this.gb_Console.Controls.Add(this.cb_SendConsoleToFile);
            this.gb_Console.Controls.Add(this.lb_Database);
            this.gb_Console.Controls.Add(this.tb_stdError);
            this.gb_Console.Controls.Add(this.label1);
            this.gb_Console.Controls.Add(this.tb_stdOut);
            this.gb_Console.Location = new System.Drawing.Point(12, 79);
            this.gb_Console.Name = "gb_Console";
            this.gb_Console.Size = new System.Drawing.Size(246, 142);
            this.gb_Console.TabIndex = 9;
            this.gb_Console.TabStop = false;
            this.gb_Console.Text = "Console Logs";
            // 
            // lb_Database
            // 
            this.lb_Database.AutoSize = true;
            this.lb_Database.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Database.ForeColor = System.Drawing.Color.DarkBlue;
            this.lb_Database.Location = new System.Drawing.Point(6, 64);
            this.lb_Database.Name = "lb_Database";
            this.lb_Database.Size = new System.Drawing.Size(145, 13);
            this.lb_Database.TabIndex = 5;
            this.lb_Database.Text = "Error Message (strError):";
            // 
            // tb_stdError
            // 
            this.tb_stdError.Location = new System.Drawing.Point(9, 80);
            this.tb_stdError.Name = "tb_stdError";
            this.tb_stdError.Size = new System.Drawing.Size(217, 20);
            this.tb_stdError.TabIndex = 2;
            this.tb_stdError.Text = "PortfolioSystem_out.txt";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkBlue;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Messages (stdOut):";
            // 
            // tb_stdOut
            // 
            this.tb_stdOut.Location = new System.Drawing.Point(9, 41);
            this.tb_stdOut.Name = "tb_stdOut";
            this.tb_stdOut.Size = new System.Drawing.Size(217, 20);
            this.tb_stdOut.TabIndex = 1;
            this.tb_stdOut.TabStop = false;
            this.tb_stdOut.Text = "PortfolioSystem_out.txt";
            // 
            // cb_SendConsoleToFile
            // 
            this.cb_SendConsoleToFile.AutoSize = true;
            this.cb_SendConsoleToFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_SendConsoleToFile.ForeColor = System.Drawing.Color.MediumBlue;
            this.cb_SendConsoleToFile.Location = new System.Drawing.Point(83, 106);
            this.cb_SendConsoleToFile.Name = "cb_SendConsoleToFile";
            this.cb_SendConsoleToFile.Size = new System.Drawing.Size(143, 17);
            this.cb_SendConsoleToFile.TabIndex = 10;
            this.cb_SendConsoleToFile.Text = "Send Console to File";
            this.cb_SendConsoleToFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_SendConsoleToFile.UseVisualStyleBackColor = true;
            this.cb_SendConsoleToFile.CheckedChanged += new System.EventHandler(this.cb_SendConsoleToFile_CheckedChanged);
            // 
            // SetDebugLevel
            // 
            this.AcceptButton = this.bt_Close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bt_Close;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.gb_Console);
            this.Controls.Add(this.bt_Close);
            this.Controls.Add(this.nUD_debugLevel);
            this.Controls.Add(this.cb_DebugOn);
            this.Name = "SetDebugLevel";
            this.Text = "SetDebugLevel";
            this.Load += new System.EventHandler(this.SetDebugLevel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nUD_debugLevel)).EndInit();
            this.gb_Console.ResumeLayout(false);
            this.gb_Console.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cb_DebugOn;
        private System.Windows.Forms.NumericUpDown nUD_debugLevel;
        private System.Windows.Forms.Button bt_Close;
        private System.Windows.Forms.GroupBox gb_Console;
        private System.Windows.Forms.CheckBox cb_SendConsoleToFile;
        private System.Windows.Forms.Label lb_Database;
        private System.Windows.Forms.TextBox tb_stdError;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_stdOut;
    }
}