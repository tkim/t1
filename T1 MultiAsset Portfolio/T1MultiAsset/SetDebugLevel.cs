using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//SystemLibrary.ConsoleSet("T1MultiAsset_Out.txt", "T1MultiAsset_Error.txt");

namespace T1MultiAsset
{
    public partial class SetDebugLevel : Form
    {
        public SetDebugLevel()
        {
            InitializeComponent();
        }

        private void SetDebugLevel_Load(object sender, EventArgs e)
        {
            // Local Variables
            String myStdOut = "";
            String myStdError = "";
            int myDebugLevel = 0;

            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            myDebugLevel = SystemLibrary.GetDebugLevel();
            if (myDebugLevel == 0)
                cb_DebugOn.Checked = false;
            else
            {
                cb_DebugOn.Checked = true;
                nUD_debugLevel.Value = myDebugLevel;
            }

            nUD_debugLevel.Enabled = cb_DebugOn.Checked;

            SystemLibrary.ConsoleGet(out myStdOut, out myStdError);
            if (myStdOut == "")
                cb_SendConsoleToFile.Checked = false;
            else
            {
                tb_stdOut.Text = myStdOut;
                tb_stdError.Text = myStdError;
            }

        } //SetDebugLevel_Load()

        private void cb_DebugOn_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_DebugOn.Checked)
                SystemLibrary.SetDebugLevel(Convert.ToInt32(nUD_debugLevel.Value));
            else
                SystemLibrary.SetDebugLevel(0);
            nUD_debugLevel.Enabled = cb_DebugOn.Checked;

        } //cb_DebugOn_CheckedChanged()

        private void cb_Close_Click(object sender, EventArgs e)
        {
            // Close the form
            this.Close();

        } //cb_Close_Click()

        private void nUD_debugLevel_ValueChanged(object sender, EventArgs e)
        {
            SystemLibrary.SetDebugLevel(Convert.ToInt32(nUD_debugLevel.Value));

        } //nUD_debugLevel_ValueChanged()

        private void cb_SendConsoleToFile_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_SendConsoleToFile.Checked)
            {
                String myOut = tb_stdOut.Text;
                String myError = tb_stdError.Text;
                SystemLibrary.ConsoleSet(myOut, myError);
            }
            else
                SystemLibrary.ConsoleSet("", "");

        } //cb_SendConsoleToFile_CheckedChanged()

    }
}
