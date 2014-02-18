namespace T1MultiAsset
{
    partial class ImportData
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
            this.bt_LoadDefinition = new System.Windows.Forms.Button();
            this.bt_LoadFile = new System.Windows.Forms.Button();
            this.dg_ImportData = new System.Windows.Forms.DataGridView();
            this.tb_TableName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_Save = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dg_ImportData)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_LoadDefinition
            // 
            this.bt_LoadDefinition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_LoadDefinition.Location = new System.Drawing.Point(679, 56);
            this.bt_LoadDefinition.Name = "bt_LoadDefinition";
            this.bt_LoadDefinition.Size = new System.Drawing.Size(90, 23);
            this.bt_LoadDefinition.TabIndex = 0;
            this.bt_LoadDefinition.Text = "Load Definition";
            this.bt_LoadDefinition.UseVisualStyleBackColor = true;
            this.bt_LoadDefinition.Click += new System.EventHandler(this.bt_LoadDefinition_Click);
            // 
            // bt_LoadFile
            // 
            this.bt_LoadFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_LoadFile.Location = new System.Drawing.Point(15, 365);
            this.bt_LoadFile.Name = "bt_LoadFile";
            this.bt_LoadFile.Size = new System.Drawing.Size(76, 23);
            this.bt_LoadFile.TabIndex = 1;
            this.bt_LoadFile.Text = "Load File";
            this.bt_LoadFile.UseVisualStyleBackColor = true;
            this.bt_LoadFile.Click += new System.EventHandler(this.bt_LoadFile_Click);
            // 
            // dg_ImportData
            // 
            this.dg_ImportData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_ImportData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_ImportData.Location = new System.Drawing.Point(12, 84);
            this.dg_ImportData.Name = "dg_ImportData";
            this.dg_ImportData.Size = new System.Drawing.Size(757, 266);
            this.dg_ImportData.TabIndex = 2;
            this.dg_ImportData.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dg_ImportData_UserAddedRow);
            this.dg_ImportData.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dg_ImportData_KeyUp);
            // 
            // tb_TableName
            // 
            this.tb_TableName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_TableName.Location = new System.Drawing.Point(12, 58);
            this.tb_TableName.Name = "tb_TableName";
            this.tb_TableName.Size = new System.Drawing.Size(661, 20);
            this.tb_TableName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(757, 46);
            this.label1.TabIndex = 4;
            this.label1.Text = "BEWARE: This form should only be used by System Administrators for the database. " +
                "It is used to import raw data into the database and needs extensive knowledge of" +
                " the database to proceed.";
            // 
            // bt_Save
            // 
            this.bt_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Save.Enabled = false;
            this.bt_Save.Location = new System.Drawing.Point(654, 365);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(115, 23);
            this.bt_Save.TabIndex = 5;
            this.bt_Save.Text = "Save To Database";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ImportData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 400);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_TableName);
            this.Controls.Add(this.dg_ImportData);
            this.Controls.Add(this.bt_LoadFile);
            this.Controls.Add(this.bt_LoadDefinition);
            this.Name = "ImportData";
            this.Text = "ImportData";
            this.Load += new System.EventHandler(this.ImportData_Load);
            this.Shown += new System.EventHandler(this.ImportData_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ImportData_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dg_ImportData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_LoadDefinition;
        private System.Windows.Forms.Button bt_LoadFile;
        private System.Windows.Forms.DataGridView dg_ImportData;
        private System.Windows.Forms.TextBox tb_TableName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}