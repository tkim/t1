using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Data;


namespace T1MultiAsset
{
    static class Program
    {
        // Used to See if Process Alreday Running
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] inCommands)
        {
            // See if Process already running (Allow multiple development versions xxx.vshost)
            Process myProcess = Process.GetCurrentProcess();

            Process[] sameProcesses = Process.GetProcessesByName(myProcess.ProcessName);
            if (sameProcesses.Length > 1 && !myProcess.ProcessName.EndsWith(".vshost"))
            {
                // Bring existing Process to the Top
                Program.SetForegroundWindow(sameProcesses[sameProcesses[0].Id == myProcess.Id ? 1 : 0].MainWindowHandle);
                return;
            }

            // See what arguments are passed in
            foreach (String inArg in inCommands)
            {
                if (inArg.ToUpper().StartsWith(@"FIRM="))
                {
                    try
                    {
                        String myVal = inArg.Substring(inArg.ToUpper().IndexOf(@"FIRM=")+@"FIRM=".Length);
                        SystemLibrary.ProtectSetKeyEncrpyt(myVal);
                    }
                    catch{}
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Redirect the console to logfiles
            //ToDo (5) 11-Mar-2011 - Build own wrapper to replace SystemLibrary.DebugLine() - has Date & flushes data to file.
            //ToDo (5) 11-Mar-2011 - Build a window to read file - like tail using offset function.
            //SystemLibrary.ConsoleSet("T1Port_Out.txt", "T1Port_Error.txt");

            // Pass the form into the timers, as they get data from here
            Form1 frm = new Form1();

            try
            {
                String pFileName = myProcess.MainModule.FileName;
                DateTime pDateTime = File.GetLastWriteTime(pFileName);
                frm.FromParent(pDateTime);
            }
            catch { }

            // Timers
            // - Check for inbound EMSX files
            System.Threading.Timer tmrEMSXFile = new System.Threading.Timer(CheckForBloombergEMSXFile, frm, 10000, 60000);

            // Now load the form
            Application.Run(frm);
            //Application.Run(new ProcessOrders());
            //Application.Run(new TreeViewControl.Form1());

            // Cleanup
            SystemLibrary.ConsoleFlush();
        }

        static void CheckForBloombergEMSXFile(object inData)
        {
            //
            // Procedure: CheckForBloombergEMSXFile()
            //
            
            // Local Variables
            Form1 frm = (Form1)inData;
            Boolean isBloombergEMSXFileConfigured = frm.BloombergEMSXFileConfigured;
            Boolean isScotiaPrimeConfigured = frm.ScotiaPrimeConfigured;
            Boolean isMLPrimeConfigured = frm.MLPrimeConfigured;
            Boolean isMLFuturesConfigured = frm.MLFuturesConfigured;
            String FTPFileNames = "";

            // See if this thread is already running
            if (frm.isCheckForBloombergEMSXFileRunning)
                return;
            // Indicate thread running
            frm.isCheckForBloombergEMSXFileRunning = true;

            // Get the EMSX files
            try
            {
                SystemLibrary.DebugLine("CheckForBloombergEMSXFile: Start");
                // get the FTP parameters from the form
                if (frm.BloombergEMSXFileConfigured)
                {
                    Boolean[] RetVal = frm.FTPStructure("LOAD");
                    isBloombergEMSXFileConfigured = RetVal[1];
                    if (RetVal[0])
                    {
                        SystemLibrary.DebugLine("FTPStructure: OK");
                        if (SystemLibrary.f_Now().Subtract(SystemLibrary.FTPVars.LastUpdate).TotalSeconds >= SystemLibrary.FTPVars.Interval_seconds)
                        {
                            SystemLibrary.FTPVars.LastUpdate = SystemLibrary.f_Now();
                            frm.FTPStructure("SAVE");

                            SystemLibrary.DebugLine("DateTime: OK");
                            // Look for EMSX Files via FTP
                            // FTP takes 1.9 seconds, so may be best to stick with harddisk.
                            String[] myFiles = SystemLibrary.FTPGetFileList(SystemLibrary.FTPVars, "");
                            String myFilePath = Path.GetTempPath();

                            if (myFiles != null)
                            {
                                SystemLibrary.DebugLine("myFiles: " + myFiles.Length.ToString());
                                foreach (String myFileName in myFiles)
                                {
                                    // See if file already exists on disk
                                    SystemLibrary.DebugLine("CheckForBloombergEMSXFile: File='" + myFileName + "'");
                                    //ToDo (2) 18-Mar-2011 - This test doesn't allow for a file that gets on disk, but not on database
                                    if (myFileName.Length > 0 && !SystemLibrary.FileExists(myFilePath + @"\" + myFileName))
                                    {
                                        // Get the file & remove from FTP server
                                        if (SystemLibrary.FTPDownloadFile(SystemLibrary.FTPVars, myFilePath, "", myFileName))
                                        {
                                            // Load into the Database
                                            //ToDo (3) 11-Mar-2011 - FTPDeleteFile() not removing files - why?
                                            if (SystemLibrary.EMSXSaveData(myFilePath, myFileName))
                                                SystemLibrary.FTPDeleteFile(SystemLibrary.FTPVars, "", myFileName);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                SystemLibrary.DebugLine("CheckForBloombergEMSXFile: End");
            }
            catch (Exception e_emsx)
            {
                SystemLibrary.DebugLine("Failed in 'Get the EMSX files'." + e_emsx.Message);
            }

            // Get the ML Prime files
            try
            {
                SystemLibrary.DebugLine("CheckForMLPrimeFile: Start");
                // get the FTP parameters from the form
                if (frm.MLPrimeConfigured)
                {
                    SystemLibrary.DebugLine("FTPMLPrimeStructure: OK");
                    Boolean[] RetVal = frm.FTPMLPrimeStructure("LOAD");
                    isScotiaPrimeConfigured = RetVal[1];
                    if (RetVal[0])
                    {
                        if (SystemLibrary.f_Now().Subtract(SystemLibrary.FTPMLPrimeVars.LastUpdate).TotalSeconds >= SystemLibrary.FTPMLPrimeVars.Interval_seconds)
                        {

                            if (SystemLibrary.MLPrimeFilePath == null)
                                SystemLibrary.MLPrimeFilePath = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('MLPrime_Path')");

                            if (SystemLibrary.MLPrimeFilePath.Length > 0)
                            {
                                SystemLibrary.FTPMLPrimeVars.LastUpdate = SystemLibrary.f_Now();
                                frm.FTPMLPrimeStructure("SAVE");
                                SystemLibrary.DebugLine("DateTime: OK");
                                // Look for ML Prime Files via FTP
                                String[] myFiles = SystemLibrary.FTPGetFileList(SystemLibrary.FTPMLPrimeVars, @"/outgoing");
                                String myFilePath = Path.GetTempPath();

                                if (myFiles != null)
                                {
                                    SystemLibrary.DebugLine("myFiles: " + myFiles.Length.ToString());
                                    // This block of code is so that I only go to the database once against the ML_File_FTPList table
                                    foreach (String myFileName in myFiles)
                                    {
                                        FTPFileNames = FTPFileNames + "'" + myFileName + "',";
                                    }
                                    // Strip off trailing ','
                                    FTPFileNames = FTPFileNames.TrimEnd(',');
                                    DataTable dt_Processed = SystemLibrary.SQLSelectToDataTable("Select FTPFileName from ML_File_FTPList Where FTPFileName in (" + FTPFileNames + ")");

                                    foreach (String myFileName in myFiles)
                                    {
                                        // See if file already exists in Database
                                        SystemLibrary.DebugLine("CheckForMLPrimeFile: File='" + myFileName + "'");
                                        Boolean FoundNewFile = false;
                                        if (dt_Processed.Rows.Count == 0)
                                            FoundNewFile = true;
                                        else
                                        {
                                            DataRow[] drCheck = dt_Processed.Select("FTPFileName='" + myFileName + "'");
                                            if (drCheck.Length == 0)
                                                FoundNewFile = true;
                                        }

                                        if (FoundNewFile)
                                        {
                                            // Get the file & decrypt it for processing
                                            if (SystemLibrary.FTPDownloadFile(SystemLibrary.FTPMLPrimeVars, myFilePath, @"/outgoing", myFileName))
                                            {
                                                // Load into the Database
                                                String myDecryptName = "";
                                                if (SystemLibrary.MLPrime_Decrypt(myFilePath, myFileName, ref myDecryptName))
                                                {
                                                    if (!File.Exists(SystemLibrary.MLPrimeFilePath + @"\" + myDecryptName))
                                                        File.Move(myFilePath + @"\" + myDecryptName, SystemLibrary.MLPrimeFilePath + @"\" + myDecryptName);
                                                    File.Delete(myFilePath + @"\" + myFileName);
                                                    SystemLibrary.SQLExecute("Insert Into ML_File_FTPList (FTPFileName, MLFileName) values ('" + myFileName + "','" + myDecryptName + "')");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                SystemLibrary.DebugLine("CheckForMLPrimeFile: End");
            }
            catch (Exception e_ML)
            {
                SystemLibrary.DebugLine("Failed in 'Get the ML Prime files'." + e_ML.Message);
            }

            // Get the SCOTIA Prime files
            try
            {
                SystemLibrary.DebugLine("CheckForSCOTIAPrimeFile: Start");
                // get the FTP parameters from the form
                if (frm.ScotiaPrimeConfigured)
                {
                    SystemLibrary.DebugLine("FTPSCOTIAPrimeStructure: OK");
                    Boolean[] RetVal = frm.FTPSCOTIAPrimeStructure("LOAD");
                    isScotiaPrimeConfigured = RetVal[1];
                    if (RetVal[0])
                    {
                        if (SystemLibrary.f_Now().Subtract(SystemLibrary.FTPSCOTIAPrimeVars.LastUpdate).TotalSeconds >= SystemLibrary.FTPSCOTIAPrimeVars.Interval_seconds)
                        {

                            if (SystemLibrary.SCOTIAPrimeFilePath == null)
                                SystemLibrary.SCOTIAPrimeFilePath = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('SCOTIAPrime_Path')");

                            if (SystemLibrary.SCOTIAPrimeFilePath.Length > 0)
                            {
                                SystemLibrary.FTPSCOTIAPrimeVars.LastUpdate = SystemLibrary.f_Now();
                                frm.FTPSCOTIAPrimeStructure("SAVE");
                                SystemLibrary.DebugLine("DateTime: OK");
                                // Look for SCOTIA Prime Files via FTP
                                String[] myFiles = SystemLibrary.FTPGetFileList(SystemLibrary.FTPSCOTIAPrimeVars, @"/FPOBELYS");
                                String myFilePath = Path.GetTempPath();

                                if (myFiles != null)
                                {
                                    SystemLibrary.DebugLine("myFiles: " + myFiles.Length.ToString());
                                    // This block of code is so that I only go to the database once against the SCOTIA_File_FTPList table
                                    foreach (String myFileName in myFiles)
                                    {
                                        FTPFileNames = FTPFileNames + "'" + myFileName + "',";
                                    }
                                    // Strip off trailing ','
                                    FTPFileNames = FTPFileNames.TrimEnd(',');
                                    DataTable dt_Processed = SystemLibrary.SQLSelectToDataTable("Select FTPFileName from SCOTIA_File_FTPList Where FTPFileName in (" + FTPFileNames + ")");

                                    foreach (String myFileName in myFiles)
                                    {
                                        // See if file already exists in Database
                                        SystemLibrary.DebugLine("CheckForSCOTIAPrimeFile: File='" + myFileName + "'");
                                        Boolean FoundNewFile = false;
                                        if (dt_Processed.Rows.Count == 0)
                                            FoundNewFile = true;
                                        else
                                        {
                                            DataRow[] drCheck = dt_Processed.Select("FTPFileName='" + myFileName + "'");
                                            if (drCheck.Length == 0)
                                                FoundNewFile = true;
                                        }

                                        if (FoundNewFile)
                                        {
                                            // Get the file & decrypt it for processing
                                            if (SystemLibrary.FTPDownloadFile(SystemLibrary.FTPSCOTIAPrimeVars, myFilePath, @"/FPOBELYS", myFileName))
                                            {
                                                if (!File.Exists(SystemLibrary.SCOTIAPrimeFilePath + @"\" + myFileName))
                                                    File.Move(myFilePath + @"\" + myFileName, SystemLibrary.SCOTIAPrimeFilePath + @"\" + myFileName);
                                                File.Delete(myFilePath + @"\" + myFileName);
                                                SystemLibrary.SQLExecute("Insert Into SCOTIA_File_FTPList (FTPFileName, SCOTIAFileName) values ('" + myFileName + "','" + myFileName + "')");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                SystemLibrary.DebugLine("CheckForSCOTIAPrimeFile: End");
            }
            catch (Exception e_Scotia)
            {
                SystemLibrary.DebugLine("Failed in 'Get the Scotia Prime files'." + e_Scotia.Message);
            }
             
            // Now look for Prime Broker & Futures Files
            if (frm.MLPrimeConfigured)
                SystemLibrary.MLPrimeGetFiles(ref isMLPrimeConfigured);

            if (frm.ScotiaPrimeConfigured)
                SystemLibrary.SCOTIAPrimeGetFiles(ref isScotiaPrimeConfigured);

            //SystemLibrary.BPOGetFiles();
            if (frm.MLFuturesConfigured)
                SystemLibrary.MLFuturesGetFiles(ref isMLFuturesConfigured);

            frm.SetFlags(isBloombergEMSXFileConfigured, isScotiaPrimeConfigured, isMLPrimeConfigured, isMLFuturesConfigured);
            
            // Indicate thread complete
            frm.isCheckForBloombergEMSXFileRunning = false;


        } // CheckForBloombergEMSXFile()

    }
}
