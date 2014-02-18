/*
 * Class:   SystemLibrary
 * 
 * Purpose: General bucket for common code
 * 
 * Written: Colin Ritchie 2-Mar-2011
 * 
 * Modified:    
 * 
 * Notes:
 * // hourglass cursor
 * Cursor.Current = Cursors.WaitCursor;
 * Cursor.Current = Cursors.Default;
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Net; //Ftp
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient; // 21040206 - I had to include this so I could use DataTable passing techniques
using System.Collections; // ArrayList
using System.Data;
using System.Reflection;
using T1MultiAsset.Registry;
using System.Security.Cryptography;
using System.Threading;
using System.Drawing;
using NDde.Client;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.DirectoryServices.AccountManagement;
using System.Text.RegularExpressions;
using System.Globalization;


namespace T1MultiAsset
{
    class SystemLibrary
    {
        // Console related
        private static StreamWriter log_Out;
        private static StreamWriter log_Error;
        private static String myFile_stdOut = "";
        private static String myFile_stdError = "";
        private static int debugLevel = 0;
        private static DateTime myLastDebugDateTime = DateTime.Now;

        // PrintScreen Related
        [DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt
        (
            IntPtr hdcDest, // handle to destination DC
            int nXDest, // x-coord of destination upper-left corner
            int nYDest, // y-coord of destination upper-left corner
            int nWidth, // width of destination rectangle
            int nHeight, // height of destination rectangle
            IntPtr hdcSrc, // handle to source DC
            int nXSrc, // x-coordinate of source upper-left corner
            int nYSrc, // y-coordinate of source upper-left corner
            System.Int32 dwRop // raster operation code
        );
        private static Stream streamToPrint;
        private static String streamType;
        private static PrintDocument printDoc;


        // Bloomberg Menu Related
        [DllImport("Shell32.dll")]
        public extern static int ExtractIconEx(string libName, int iconIndex, IntPtr[] largeIcon, IntPtr[] smallIcon, int nIcons);
        public struct BloombergMenu
        {
            public ContextMenuStrip myMenu;
            public ImageList il_Menu;
            public DataTable dt_BBG_Menu;
            public String Ticker;
            public String Relative;
            public String Portfolio;
            public int FundID;
            public int PortfolioID;
            public BloombergMenu(Boolean myDummy)
            {
                myMenu = new ContextMenuStrip();
                il_Menu = new ImageList();
                Ticker = "IBM US Equity";
                Relative = "SPX Index";
                Portfolio = "SPX Index";
                FundID = -1;
                PortfolioID = -1;
                dt_BBG_Menu = new DataTable();
            }
        }
        public static BloombergMenu BBGMenu = new BloombergMenu(true);

        public struct BloombergMenuTag
        {
            public String Caption;
            public String Command;
            public String AppliesTo;
            public Form applyForm;
            public BloombergMenuTag(Form inForm, String inCaption, String inCommand, String inAppliesTo)
            {
                Caption = inCaption;
                Command = inCommand;
                AppliesTo = inAppliesTo;
                applyForm = inForm;
            }
        }
        private static DdeClient BloombergClient = null;

        // Encryption Key
        private struct ProtectStruct
        {
            public String KeyEncrpyt;
            public DataProtectionScope KeyScope;
            //{KeyEncrpyt = "G" + "o" + "llum";}
            public ProtectStruct(String inStr)
            {
                KeyEncrpyt = inStr;
                KeyScope = DataProtectionScope.CurrentUser;
            }
        }
        private static ProtectStruct ProtectVars = new ProtectStruct("G" + "o" + "l" + "l" + "u" + "m");

        // FTP Details should come from database
        public struct FTPStruct
        {
            public String ServerIP;
            public String ServerIP2;
            public String UserID;
            public String Password;
            public String EMSXFileNameStartsWith;
            public int Interval_seconds;
            public DateTime LastUpdate;
            public FTPStruct(Boolean dummy)
            {
                ServerIP = "";
                ServerIP2 = "";
                UserID = "";
                Password = "";
                EMSXFileNameStartsWith = "";
                Interval_seconds = 300; // Bloomberg minimum is 5 minutes 
                LastUpdate = SystemLibrary.f_Now().AddSeconds(-300); 
            }
        }
        public static FTPStruct FTPVars = new FTPStruct(true);
        public static FTPStruct FTPMLPrimeVars = new FTPStruct(true);
        public static FTPStruct FTPSCOTIAPrimeVars = new FTPStruct(true);

        public static int myOffsetSeconds = 0;
        //private static DateTime myToday = DateTime.Now.Date;
        //private static DateTime myNow = DateTime.Now;

        // Database
        public struct DBStruct
        {
            public String ConnString; // eg. Provider=SQLOLEDB;Password=<DBVars.DBPwd>;User ID=<DBVars.DBUser>;DataSource=<DBVars.ServerName>;Initial Catlog=<Database>
            public String SqlConnString; // eg. Provider=SQLOLEDB;Password=<DBVars.DBPwd>;User ID=<DBVars.DBUser>;DataSource=<DBVars.ServerName>;Initial Catlog=<Database>
            public String DataSourceName; // Internal text: MSSql="SQL Server", Oracle="Oracle", MSAccess="Access", WindowsAzure="Azure"
            public String ProviderName; //SqlServer="SQLOLEDB", Oracle="OraOLEDB.Oracle", Access="Microsoft.Jet.OLEDB.4.0"
            public String DatabaseName;
            public Boolean IntegratedSecurity; //= ";Integrated Security=SSPI;"; // For SQL Server ONLY
            public String ServerName;
            public String DBUser;
            public String DBPwd;
            public int DB_Timeout; 
            public DBStruct(int Timeout)
            {
                ConnString = "";
                SqlConnString = "";
                DataSourceName = "";
                ProviderName = "";
                DatabaseName = "";
                IntegratedSecurity = true;
                ServerName = "";
                DBUser = "";
                DBPwd = "";
                DB_Timeout = Timeout; // 120 Seconds timeout
            }
        }
        public static DBStruct DBVars = new DBStruct(120);

        // ML Prime
        public static String MLPrimeFilePath;
        public static String MLPrime_Crypt_Key;
        public static String MLPrime_Crypt_User;
        public static String BookingsFilePath;
        // Releates to encryption for ML Prime
        public static Process _processObject;
        public static String _outputString;
        public static String _errorString;

        public static String MLFuturesFilePath;

        // SCOTIA Prime
        public static String SCOTIAPrimeFilePath;
        public static String SCOTIAPrime_FilePart;



        // Console redirection
        #region Console
        public static void ConsoleSet(String stdOut, String stdError)
        {
            // Local Variables

            // Get the Temp Path
            String TempPath = "C:";

            ConsoleFlush();

            if (stdOut == "")
            {
                StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
            }
            else
            {
                log_Out = new StreamWriter(TempPath + @"\" + stdOut);
                log_Out.AutoFlush = true;
                Console.SetOut(log_Out);
            }
            if (stdError == "")
            {
                StreamWriter standardError = new StreamWriter(Console.OpenStandardError());
                standardError.AutoFlush = true;
                Console.SetError(standardError);
            }
            else
            {
                if (stdError == stdOut)
                    log_Error = log_Out;
                else
                    log_Error = new StreamWriter(TempPath + @"\" + stdError);

                log_Error.AutoFlush = true;
                Console.SetError(log_Error);
            }
        } // ConsoleSet()

        public static void ConsoleGet(out String i_stdOut, out String i_stdError)
        {
            i_stdOut = myFile_stdOut;
            i_stdError = myFile_stdError;

        } //ConsoleGet()

        public static void ConsoleFlush()
        {
            SystemLibrary.DebugLine("Flush Console");
            try
            {
                if (log_Out!=null)
                    log_Out.Flush();
                if (log_Error != null)
                    log_Error.Flush();
            }
            catch { }
            
        } // ConsoleFlush()

        public static void SetDebugLevel(int inDebugLevel)
        {
            debugLevel = inDebugLevel;

        } //SetDebugLevel()

        public static int GetDebugLevel()
        {
            return (debugLevel);

        } //GetDebugLevel()

        public static void DebugLine(object myObj)
        {
            /*
             * Procedure:   DebugLine
             * 
             * Written:  RitchViewer Pty Ltd
             *      Original extracted from other systems we have written.
             *      
             * Modified:
             * 
             */
            if (debugLevel == 0)
                return;

            DateTime myDate = DateTime.Now;
            TimeSpan howLong = myDate.Subtract(myLastDebugDateTime);

            //SystemLibrary.DebugLine(howLong.TotalSeconds.ToString() + "\t" + myDate.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - " + myObj);
            Console.WriteLine(myDate.ToString("yyyy-MM-dd HH:mm:ss.fffff") + " - " + myObj);
            myLastDebugDateTime = myDate;
        }

        public static void DebugLine(DateTime myStartDate, object myObj)
        {
            /*
             * Procedure:   DebugLine
             * 
             * Written:  RitchViewer Pty Ltd
             *      Original extracted from other systems we have written.
             *      
             * Modified:
             * 
             */
            if (debugLevel == 0)
                return;

            DateTime myDate = DateTime.Now;
            TimeSpan howLong = myDate.Subtract(myStartDate);

            SystemLibrary.DebugLine(howLong.TotalSeconds.ToString() + "\t" + myObj);
        }

        public static String GetCallStack()
        {
            StackTrace stack_Current = new StackTrace(true);
            return (stack_Current.ToString());
        }
  

        #endregion // Console

        // Data encryption calls
        #region DataProtection
        //
        // Used to encrypt some of the data to the Registry
        public static void ProtectSetKeyEncrpyt(String inStr)
        {
            // Allows parent code to pass in an alternative Key to increase encryption complexity
            ProtectVars.KeyEncrpyt = inStr;

        } //ProtectSetKeyEncrpyt()

        public static string ProtectEncrypt(String inVal)
        {
            // Local Variables
            var Entropy = Encoding.Unicode.GetBytes(ProtectVars.KeyEncrpyt);

            //encrypt data
            var data = Encoding.Unicode.GetBytes(inVal);
            byte[] encrypted = ProtectedData.Protect(data, Entropy, ProtectVars.KeyScope);


            //return as base64 string
            return (Convert.ToBase64String(encrypted));

        } //ProtectEncrypt()

        public static string ProtectDecrypt(String inVal)
        {
            // Local Variables
            var Entropy = Encoding.Unicode.GetBytes(ProtectVars.KeyEncrpyt);


            //parse base64 string
            byte[] data = Convert.FromBase64String(inVal);

            //decrypt data
            byte[] decrypted = ProtectedData.Unprotect(data, Entropy, ProtectVars.KeyScope);
            return (Encoding.Unicode.GetString(decrypted));

        } //ProtectDecrypt()


        #endregion // DataProtection

        // SQL Calls
        #region SQL

        public static Boolean SQLLoadConnectParams()
        {
            //
            // Purpose:  Load Database connection fields from Registry
            //

            // Local Variables
            Registry.Registry myReg = new T1MultiAsset.Registry.Registry();
            Boolean retVal = false;

            DBVars.DataSourceName = myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Database", "DataSourceName").ToString();
            if (DBVars.DataSourceName.Length > 0)
            {
                DBVars.ProviderName = myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Database", "ProviderName").ToString();
                DBVars.DatabaseName = myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Database", "DatabaseName").ToString();
                DBVars.ServerName = myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Database", "ServerName").ToString();
                DBVars.DBUser = myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Database", "DBUser").ToString();
                DBVars.DBPwd = ProtectDecrypt(myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Database", "DBPwd").ToString());
                try
                {
                    DBVars.IntegratedSecurity = Convert.ToBoolean(myReg.RegGetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Database", "IntegratedSecurity"));
                }
                catch
                {
                    DBVars.IntegratedSecurity = true;
                }
                // Rebuild ConnString
                DBVars.ConnString = SQLSetConnString(DBVars.DataSourceName, DBVars.ProviderName, DBVars.ServerName, DBVars.DatabaseName, DBVars.DBUser, DBVars.DBPwd, DBVars.IntegratedSecurity);
                DBVars.SqlConnString = DBVars.ConnString.Substring(DBVars.ConnString.IndexOf("Data Source="));
                retVal = true;
            }
            else
                DBVars.DataSourceName = "SQL Server";

            return (retVal);

        } //SQLLoadConnectParams()

        public static void SQLSaveConnectParams()
        {   
            //
            // Purpose:  Save Database connection fields to Registry
            //

            // Local Variables
            Registry.Registry myReg = new T1MultiAsset.Registry.Registry();
            myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Database", "DataSourceName", DBVars.DataSourceName);
            myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Database", "ProviderName", DBVars.ProviderName);
            myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Database", "DatabaseName", DBVars.DatabaseName);
            myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Database", "ServerName", DBVars.ServerName);
            myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Database", "DBUser", DBVars.DBUser);
            myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Database", "DBPwd", ProtectEncrypt(DBVars.DBPwd));
            myReg.RegSetValue("HKEY_CURRENT_USER", @"SOFTWARE\T1 MultiAsset\Database", "IntegratedSecurity", DBVars.IntegratedSecurity.ToString());

        } //SQLSaveConnectParams()

        public static int SQLAlterTimeOut(int inTimeOut)
        {
            // Local Variables
            int oldTimeout = DBVars.DB_Timeout;

            DBVars.DB_Timeout = inTimeOut;
            return (oldTimeout);
        } //SQLAlterTimeOut()

        public static Int32 SQLSelectRowsCount(String mySelect)
        {
            //
            // Procedure:   SQLSelectRowsCount
            //
            // Purpose:     Used when SQL is just there to test Rows.
            //              ie. When do a "Select 'x' from ...Where ..." to test Where clause.
            //

            // Local Variables
            DataTable dt_1;

            dt_1 = (DataTable)SQLSelectToObject(mySelect, "DataTable");
            return (dt_1.Rows.Count);

        } //SQLSelectRowsCount()

        public static Int32 SQLSelectInt32(String mySelect)
        {
            //
            // Procedure:   SQLSelectInt32
            //
            // Purpose:     Used when getting 1 Datapoint from the database
            //

            // Local Variables
            Int32 RetVal;
            DataTable dt_1 = (DataTable)SQLSelectToObject(mySelect, "DataTable");

            try
            {
                if (dt_1.Rows.Count > 0)
                    RetVal = SystemLibrary.ToInt32(dt_1.Rows[0][0]);
                else
                    RetVal = 0;
            }
            catch (Exception e)
            {
                // Write Failure to console
                SystemLibrary.DebugLine("SelectInt32:(" + mySelect + ")\r\nError:" + e.Message);
                RetVal = 0;
            }
            return (RetVal);

        } //SQLSelectInt32()

        public static Double SQLSelectDouble(String mySelect)
        {
            //
            // Procedure:   SQLSelectDouble
            //
            // Purpose:     Used when getting 1 Datapoint from the database
            //

            // Local Variables
            Double RetVal;
            DataTable dt_1 = (DataTable)SQLSelectToObject(mySelect, "DataTable");

            try
            {
                if (dt_1.Rows.Count > 0)
                    RetVal = SystemLibrary.ToDouble(dt_1.Rows[0][0]);
                else
                    RetVal = 0;
            }
            catch (Exception e)
            {
                // Write Failure to console
                SystemLibrary.DebugLine("SelectDouble:(" + mySelect + ")\r\nError:" + e.Message);
                RetVal = 0;
            }
            return (RetVal);

        } //SQLSelectDouble()

        public static Decimal SQLSelectDecimal(String mySelect)
        {
            //
            // Procedure:   SQLSelectDecimal
            //
            // Purpose:     Used when getting 1 Datapoint from the database
            //

            // Local Variables
            Decimal RetVal;
            DataTable dt_1 = (DataTable)SQLSelectToObject(mySelect, "DataTable");

            try
            {
                if (dt_1.Rows.Count > 0)
                    RetVal = SystemLibrary.ToDecimal(dt_1.Rows[0][0]);
                else
                    RetVal = 0;
            }
            catch (Exception e)
            {
                // Write Failure to console
                SystemLibrary.DebugLine("SelectDecimal:(" + mySelect + ")\r\nError:" + e.Message);
                RetVal = 0;
            }
            return (RetVal);

        } //SQLSelectDecimal()

        public static DateTime SQLSelectDateTime(String mySelect, DateTime myDefaultDateTime)
        {
            //
            // Procedure:   SQLSelectDouble
            //
            // Purpose:     Used when getting 1 Datapoint from the database
            //
            // Rules:   As I cant return a Null, I pass back myDefaultDateTime
            //

            // Local Variables
            DateTime RetVal;
            DataTable dt_1 = (DataTable)SQLSelectToObject(mySelect, "DataTable");

            try
            {
                if (dt_1.Rows.Count > 0 && dt_1.Rows[0][0] != DBNull.Value)
                    RetVal = Convert.ToDateTime(dt_1.Rows[0][0]);
                else
                    RetVal = myDefaultDateTime;
            }
            catch (Exception e)
            {
                // Write Failure to console
                SystemLibrary.DebugLine("SelectDateTime:(" + mySelect + ")\r\nError:" + e.Message);
                RetVal = myDefaultDateTime;
            }
            return (RetVal);

        } //SQLSelectDateTime()

        public static String SQLSelectString(String mySelect)
        {
            //
            // Procedure:   SelectString
            //
            // Purpose:     Used when getting 1 Datapoint from the database
            //

            // Local Variables
            String RetVal;
            DataTable dt_1 = (DataTable)SQLSelectToObject(mySelect, "DataTable");

            try
            {
                if (dt_1.Rows.Count > 0)
                    RetVal = dt_1.Rows[0][0].ToString();
                else
                    RetVal = "";
            }
            catch (Exception e)
            {
                // Write Failure to console
                SystemLibrary.DebugLine("SelectString:(" + mySelect + ")\r\nError:" + e.Message);
                RetVal = "";
            }
            return (RetVal);

        } //SQLSelectString()

        public static DataTable SQLSelectToDataTable(String mySelect)
        {
            //
            // Procedure:   SelectToDataTable
            //
            // Purpose:     Return Select data to a DataTable
            //
            return ((DataTable)SQLSelectToObject(mySelect, "DataTable"));

        } //SQLSelectToDataTable()

        public static DataSet SQLSelectToDataSet(String mySelect)
        {
            //
            // Procedure:   SelectToDataSet
            //
            // Purpose:     Return Select data to a DataSet
            //
            return ((DataSet)SQLSelectToObject(mySelect, "DataSet"));

        } //SQLSelectToDataSet()

        public static Object SQLSelectToObject(String mySelect, String myFill)
        {
            //
            // Procedure:   SQLSelectToObject
            //
            // Purpose:     Generic routine called with myFill as DataSet or DataTable
            //

            // Local Variables
            OleDbCommand myCommand;
            OleDbDataAdapter myAdapter;
            DataTable dt_SqlResult = new DataTable();
            DataSet ds_SqlResult = new DataSet();
            int old_debugLevel = debugLevel;
            DateTime myStartTime = DateTime.Now;

            using (OleDbConnection conn = new OleDbConnection(DBVars.ConnString))
            {
                try
                {
                    myCommand = new OleDbCommand(mySelect, conn);
                    myCommand.CommandTimeout = DBVars.DB_Timeout;
                    myAdapter = new OleDbDataAdapter(myCommand);
                    if (myFill == "DataTable")
                        myAdapter.Fill(dt_SqlResult);
                    else
                        myAdapter.Fill(ds_SqlResult);
                }
                catch (Exception e)
                {
                    // Write Failure to console
                    SystemLibrary.DebugLine("SelectToDataTable:(" + mySelect + ")\r\nError:" + e.Message);
                }

                // Colin DEBUG SQL SLOW
                debugLevel = 4;
                if (myFill == "DataTable")
                    SystemLibrary.DebugLine(myStartTime, "SQLSelectToObject: {" + dt_SqlResult.Rows.Count.ToString() + "} ({" + mySelect + "},{" + myFill + "})");
                else
                    SystemLibrary.DebugLine(myStartTime, "SQLSelectToObject: {" + ds_SqlResult.Tables[0].Rows.Count.ToString() + "} ({" + mySelect + "},{" + myFill + "})");
                debugLevel = old_debugLevel;

                if (myFill=="DataTable")
                    return (dt_SqlResult);
                else
                    return (ds_SqlResult);
            }
            

        } //SQLSelectToObject()

        public static void TestBulkSQL()
        {
            String myTable = "Colin";
            String myColumns = "";
            // Get the Datatable definition
            // Columns can be "" or I can supply the order - perhaps from the file to be uploaded
            DataTable dt_load = SQLBulk_GetDefinition(myColumns, myTable);

            // Now load some values
            for (int i = 0; i < 5; i++)
            {
                DataRow dr = dt_load.NewRow();
                dr[0] = i;
                dt_load.Rows.Add(dr);
            }
            //dt_load.AcceptChanges();

            // Now Save
            SystemLibrary.DebugLine("Rows Inserted=" + SQLBulkUpdate(dt_load, myColumns, myTable).ToString());
        } // TestBulkSQL()

        public static Int32 SQLBulkUpdate(DataTable dt_in, String myColumns, String myTable)
        {
            //
            // Note: 
            //   1) This is not a bulk update, but the OleDbCommandBuilder object creates seperate Insert, Update, Delete calls on changed data.
            //   2) Table Must have a Primary Key for it to generate the SQL's
            //

            // Local Variables
            OleDbCommand myCommand;
            OleDbDataAdapter myAdapter;
            DataTable dt_load = new DataTable();
            String mySelect;
            Int32 myRows = -1;
            DateTime myStartTime = DateTime.Now;

            // Create the select so it gets the definition into a DataTable.
            if (myColumns.Length == 0)
                myColumns = "*";
            mySelect = "Select " + myColumns + " From " + myTable + " WHERE 1=2 ";


            using (OleDbConnection conn = new OleDbConnection(DBVars.ConnString))
            {
                try
                {
                    // Recreate the Adapter
                    myCommand = new OleDbCommand(mySelect, conn);
                    myCommand.CommandTimeout = DBVars.DB_Timeout;
                    myAdapter = new OleDbDataAdapter(myCommand);
                    // This allows for insert, update, delete commands
                    OleDbCommandBuilder myBuilder = new OleDbCommandBuilder(myAdapter);
                    myAdapter.Fill(dt_load);

                    // Copy the Rows from dt_in to dt_load (appears to keep all the characteristics)
                    dt_load = dt_in.Copy();
                    // Now Save to Database
                    myRows = myAdapter.Update(dt_load);
                    // Mark dt_in changes as accepted - so cant reuse.
                    dt_in.AcceptChanges();
                }
                catch (Exception e)
                {
                    // Write Failure to console
                    SystemLibrary.DebugLine("SQLBulkUpdate:(" + mySelect + ")\r\nError:" + e.Message);
                }
            }

            // Colin DEBUG SQL SLOW
            int old_debugLevel = debugLevel;
            debugLevel = 4;
            SystemLibrary.DebugLine(myStartTime, "SQLBulkUpdate: {" + myRows.ToString() + "} {" + mySelect + "}");
            debugLevel = old_debugLevel;

            return (myRows);

        } //SQLBulkUpdate()

        public static DataTable SQLBulk_GetDefinition(String myColumns, String myTable)
        {
            // Use this when want an empty shell
            return(SQLBulk_GetDefinition(myColumns, myTable, "WHERE 1=2"));
        }

        public static DataTable SQLBulk_GetDefinition(String myColumns, String myTable, String myWhere)
        {
            // Local Variables
            OleDbCommand myCommand;
            OleDbDataAdapter myAdapter;
            DataTable dt_in = new DataTable();
            String mySelect;

            // Create the select so it gets the definition into a DataTable.
            if (myColumns.Length==0)
                myColumns = "*";
            mySelect = "Select "+myColumns+" From "+myTable+" "+myWhere;


            using (OleDbConnection conn = new OleDbConnection(DBVars.ConnString))
            {
                try
                {
                    myCommand = new OleDbCommand(mySelect, conn);
                    myCommand.CommandTimeout = DBVars.DB_Timeout;
                    myAdapter = new OleDbDataAdapter(myCommand);
                    myAdapter.Fill(dt_in);
                }
                catch (Exception e)
                {
                    // Write Failure to console
                    SystemLibrary.DebugLine("SQLBulk_GetDefinition:(" + mySelect + ")\r\nError:" + e.Message);
                }
            }
            return (dt_in);

        } //SQLBulk_GetDefinition()


        public static Int32 SQLExecute(String mySql)
        {
            //
            // Procedure:   SQLExecute
            //
            // Purpose:     Generic routine to allow for non-Select commands (ie. Execute, Insert, ...)
            //

            // Local Variables
            OleDbCommand myCommand;
            Int32 myRows = -1;
            int old_debugLevel = debugLevel;
            DateTime myStartTime = DateTime.Now;

            using (OleDbConnection conn = new OleDbConnection(DBVars.ConnString))
            {
                try
                {
                    conn.Open();
                    myCommand = new OleDbCommand(mySql, conn);
                    myCommand.CommandTimeout = DBVars.DB_Timeout;
                    myRows = myCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    // Write Failure to console
                    SystemLibrary.DebugLine("SQLExecute:(" + mySql + ")\r\nError:" + e.Message);
                }
                //Console.WriteLine("SQLExecute: - "+myRows.ToString() + " (" + mySql + ")");

                // Colin DEBUG SQL SLOW
                debugLevel = 4;
                SystemLibrary.DebugLine(myStartTime, "SQLExecute: {" + myRows.ToString() + "} {" + mySql + "}");
                debugLevel = old_debugLevel;

                return (myRows);
            }
            
        } //SQLExecute()


        public static String SQLTestConnection(String DataSourceName, String ProviderName, String ServerName, String DatabaseName, String DBUser, String DBPwd, Boolean IntegratedSecurity)
        {
            // 
            // Procedure:   SQLTestConnection
            //
            // Rules:        Returns "" if OK, and the error message on failure

            // Local Variables
            String myConnString;
            String myRetString;

            myConnString = SQLSetConnString(DataSourceName, ProviderName, ServerName, DatabaseName, DBUser, DBPwd, IntegratedSecurity);
            using (OleDbConnection conn = new OleDbConnection(myConnString))
            {
                try
                {
                    // test the connection with an open attempt
                    conn.Open();
                    myRetString = "";
                }
                catch (Exception ex)
                {
                    // inform the user if the connection failed
                    myRetString = ex.Message;
                }
            }

            return (myRetString);

        } //SQLTestConnection

        public static String SQLSetConnString(String DataSourceName, String ProviderName, String ServerName, String DatabaseName, String DBUser, String DBPwd, Boolean IntegratedSecurity)
        {
            // Local Variables
            String ConnString;

            // Build the ConnString
            switch (DataSourceName)
            {
                case "Oracle":
                    ConnString = "Provider=" + ProviderName +
                                 ";Data Source=" + DatabaseName +
                                 ";User ID=" + DBUser +
                                 ";Password=" + DBPwd;
                    break;
                case "Access":
                    ConnString = "Provider=" + ProviderName +
                                 ";Data Source=" + DatabaseName +
                                 ";User ID=" + DBUser +
                                 ";Password=" + DBPwd;
                    break;
                case "MySql":
                    ConnString = "Provider=" + ProviderName +
                                 ";Data Source=" + DatabaseName +
                                 ";User ID=" + DBUser +
                                 ";Password=" + DBPwd;
                    break;
                case "Azure":
                    ConnString = "Provider=" + ProviderName +
                                 ";Data Source=" + ServerName +
                                 ";Initial Catalog=" + DatabaseName +
                                 ";User ID=" + DBUser +
                                 ";Password=" + DBPwd +
                                 ";Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
                    break;
                default:    // "SQL Server"
                    // Default to Microsoft Sql Server
                    ConnString = "Provider=" + ProviderName +
                                 ";Data Source=" + ServerName +
                                 ";Initial Catalog=" + DatabaseName;
                    if (IntegratedSecurity)
                        ConnString = ConnString + ";Integrated Security=SSPI;";
                    else
                        ConnString = ConnString + ";User ID=" + DBUser + ";Password=" + DBPwd;

                    // Needed for the Azure connection
                    ConnString = ConnString + ";Encrypt=True;TrustServerCertificate=False;";
                    break;
            }
            //ConnString = ConnString + ";Application Name=" + thisApplicationName();
            return (ConnString);

        } //SQLSetConnString()

        public static String thisApplicationName()
        {
            //
            // Procedure:   thisApplicationName
            //
            // Purpose:     Built so "Application Name" can be passed to the Database, so can see who the connection is from.
            //              - extracted from Best-Practice notes on web, and may apply to other code like Log file?
            //

            // Local Variables
            Assembly myApp = System.Reflection.Assembly.GetExecutingAssembly();;

            return (myApp.ManifestModule.Name + "{" + myApp.ImageRuntimeVersion + "} Created: " + SystemLibrary.GetCreationTime(myApp.Location).ToString("dd-MMM-yyyy HH:mm:ss"));

        } //thisApplicationName

        public static Boolean SQLGetDatabaseSchema(ref ArrayList getTables)
        {
            // Local Variables
            ArrayList getViews = new ArrayList();

            if (SQLGetDatabaseSchema(ref getTables, ref getViews))
            {
                // Load the Views into the table array & sort
                getTables.AddRange(getViews);
                getTables.Sort();
                return (true);
            }
            else
                return (false);

        } //SQLGetDatabaseSchema(getTables)

        public static Boolean SQLGetDatabaseSchema(ref ArrayList getTables, ref ArrayList getViews)
        {
            //
            // Procedure:   SQLGetDatabaseSchema
            //
            // Purpose: To allow for adding to say a list box later;
            //          Example code:
            //          // clear internal lists (lst_getTables is a ListBox)
            //          lst_getTables.Items.Clear(); // Could do the same with Views?
            //          lst_getTables.Items.AddRange(getTables.ToArray());
            //

            // Local Variables
            DataTable SchemaResult;

            using (OleDbConnection conn = new OleDbConnection(DBVars.ConnString))
            {
                try
                {
                    conn.Open();

                    // Get the Tables
                    SchemaResult = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { null, null, null, "TABLE" });
                    for (int i = 0; i < SchemaResult.Rows.Count; i++)
                        getTables.Add(SchemaResult.Rows[i].ItemArray[2].ToString());

                    // Get the Views
                    SchemaResult = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { null, null, null, "VIEW" });
                    for (int i = 0; i < SchemaResult.Rows.Count; i++)
                        getViews.Add(SchemaResult.Rows[i].ItemArray[2].ToString());

                }
                catch (Exception e)
                {
                    // Write Failure to console
                    SystemLibrary.DebugLine("GetDatabaseSchema:(" + e.Message + ")");
                    return (false);
                }
            }

            return (true);

        } //SQLGetDatabaseSchema(getTables,getViews)

        public static Boolean SQLGetTableSchema(String myTableName, ref ArrayList getColumns)
        {
            //
            // Procedure:   SQLGetTableSchema
            //
            // Purpose:     Get a list of Columns on a table
            //

            // Local Variables
            DataTable SchemaResult;

            using (OleDbConnection conn = new OleDbConnection(DBVars.ConnString))
            {
                try
                {
                    // open the connection to the database 
                    conn.Open();

                    // Get the Tables
                    SchemaResult = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, myTableName });
                    for (int i = 0; i < SchemaResult.Rows.Count; i++)
                        getColumns.Add(SchemaResult.Rows[i]["COLUMN_NAME"].ToString());
                }
                catch (Exception e)
                {
                    // Write Failure to console
                    SystemLibrary.DebugLine("GetDatabaseSchema:(" + e.Message + ")");
                    return (false);
                }
            }

            return (true);

        } //SQLGetTableSchema()


        public static Boolean SQLGetColumnSchema(String myTableName, String myColumnName, ref String DataType, ref System.Type myType)
        {
            //
            // Procedure:   GetColumnSchema
            //
            // Purpose: Return a C# Variable type with correct precision.
            //          so can do dt_NAME.Columns.Add(myColumnName, myType)
            //

            // Local Variables
            Boolean RetVal;
            ArrayList getColumnAttributes = new ArrayList();
            int ColumnSize = 0;
            int NumericPrecision = 0;
            int NumericScale = 0;

            RetVal = SQLGetColumnSchema(myTableName, myColumnName, ref getColumnAttributes);
            if (RetVal)
            {
                for(Int32 i = 0;i<getColumnAttributes.Count;i++)
                {
                    String[] myObject = (String[])getColumnAttributes[i];
                    switch (myObject[0])
                    {
                        case "DataType":
                            DataType = myObject[1];
                            break;
                        case "ColumnSize":
                            ColumnSize = Convert.ToInt16(myObject[1]);
                            break;
                        case "NumericPrecision":
                            NumericPrecision = Convert.ToInt16(myObject[1]);
                            break;
                        case "NumericScale":
                            NumericScale = Convert.ToInt16(myObject[1]);
                            break;
                    }
                }
                // Now convert this to a c# Object type.
                // - Form now it appears procession is unused, but how do I limit in a datatable?
                if (DataType == "")
                    myType = System.Type.GetType("System.String");
                else
                    myType = System.Type.GetType(DataType);
            }

            return (RetVal);

        } // SQLGetColumnSchema()

        public static Boolean SQLGetColumnSchema(String myTableName, String myColumnName, ref ArrayList getColumnAttributes)
        {
            //
            // Procedure:   SQLGetColumnSchema
            //
            // Purpose:     Get a list of Schema Items.
            //              I have stored the field output as a 2 dimenional string.
            //              May need code to return:
            //                   "DataType" = "System.Decimal"  | "System.String" | "System.Double" | "System.DateTime"
            //                   "ColumnSize" =                 | 100             |
            //                   "NumericPrecision" = 18        |                 |
            //                   "NumericScale" = 6             |                 |
            //

            // Local Variables
            DataTable SchemaResult;
            OleDbDataReader DB_reader;
            OleDbCommand DB_Command;

            using (OleDbConnection conn = new OleDbConnection(DBVars.ConnString))
            {
                try
                {
                    String mySql = "SELECT [" + myColumnName + "] FROM [" + myTableName + "]";
                    DB_Command = new OleDbCommand(mySql, conn);
                    conn.Open();
                    DB_reader = DB_Command.ExecuteReader(CommandBehavior.KeyInfo);
                    SchemaResult = DB_reader.GetSchemaTable();

                    foreach (DataRow myColumn in SchemaResult.Rows)
                    {
                        foreach (DataColumn myItem in SchemaResult.Columns)
                        {
                            String[] myObject = new String[2];
                            myObject[0] = myItem.ColumnName;
                            myObject[1] = myColumn[myItem].ToString();
                            getColumnAttributes.Add(myObject);
                        }
                        // Only expect 1 Row
                        DB_reader.Close();
                        return (true);
                    }
                }
                catch (Exception e)
                {
                    // Write Failure to console
                    SystemLibrary.DebugLine("GetColumnSchema:(" + e.Message + ")");
                    return (false);
                }
            }

            return (true);

        } // SQLGetColumnSchema()

        public static Int32 SQLExecute(String myStoredProcedureName, String myColumnName, ref DataTable dt_in)
        {
            //
            // Procedure:   SQLExecute
            //
            // Purpose:     Custom Built so a single table can be sent to a stored procedure
            //              Contacting a remote Azure database takes 0.25 seconds per SQL.
            //              This technique allows for collapsing many SQL cals into 1.
            //
            // Written:     6-Feb-2014
            //

            // Local Variable
            SqlCommand myCommand;
            Int32 myRows = -1;
            int old_debugLevel = debugLevel;
            DateTime myStartTime = DateTime.Now;


            using (SqlConnection conn = new SqlConnection(SystemLibrary.DBVars.SqlConnString))
            {
                try
                {
                    conn.Open();
                    myCommand = new SqlCommand(myStoredProcedureName, conn);
                    myCommand.CommandTimeout = SystemLibrary.DBVars.DB_Timeout;
                    myCommand.CommandType = CommandType.StoredProcedure;

                    //Pass table Valued parameter to Store Procedure
                    SqlParameter SqlParam = myCommand.Parameters.AddWithValue(myColumnName, dt_in);
                    SqlParam.SqlDbType = SqlDbType.Structured;
                    //sqlParam.SqlDbType = SqlDbType.Structured;
                    myRows = myCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    // Write Failure to console
                    SystemLibrary.DebugLine("SQLExecute:("+myStoredProcedureName+")\r\nError:" + e.Message);
                }
            }

            // Colin DEBUG SQL SLOW
            debugLevel = 4;
            SystemLibrary.DebugLine(myStartTime, "SQLExecute: {" + dt_in.Rows.Count.ToString() + "} {" + myStoredProcedureName + "}");
            debugLevel = old_debugLevel;

            return (myRows);

        } //SQLExecute()


        #endregion //SQL

        // Process Calls
        #region Process Calls

        public static Boolean ProcessIsRunning(String inProcessName, String PassinType)
        {
            /*
             * Procedure:   ProcessIsRunning
             * 
             * Purpose:     To see if a process already exists.
             *              PassinType allows this to be by ????
             * 
             * Written: Colin Ritchie 2-Mar-2011
             * 
             * Modified:
             * 
             */

            // Local Variables
            Process[] ProcessList;


            // If no ProcessName passed in then assume CurrentProcess
            if (inProcessName.Length == 0)
            {
                inProcessName = Process.GetCurrentProcess().ProcessName;
                PassinType = "ProcessName";
            }

            // Get all windows of same type
            //ProcessList = Process.GetProcessesByName(inProcessName);
            ProcessList = Process.GetProcesses();

            if (ProcessList.Length > 1)
            {
                foreach (Process p in ProcessList)
                {
                    //SystemLibrary.DebugLine(p.MainModule.ModuleName);
                    if (PassinType.ToUpper() == "Title".ToUpper())
                    {
                        if (p.MainWindowTitle.ToUpper() == inProcessName.ToUpper())
                        {
                            return (true);
                        }
                    }
                    else if (PassinType.ToUpper() == "EXE".ToUpper())
                    {
                        try
                        {
                            // This is here to deal with system processes causing errors.
                            // I couldnt find out how to test for these, but I can at least ignore processes without a similar name.
                            if (inProcessName.ToUpper().StartsWith(p.ProcessName.ToUpper()))
                            {
                                if (p.MainModule.ModuleName.ToUpper() == inProcessName.ToUpper())
                                {
                                    return (true);
                                }
                            }
                        }
                        catch { }
                    }
                    else if (PassinType.ToUpper() == "PATH".ToUpper())
                    {
                        try
                        {
                            if (p.MainModule.FileName.ToUpper() == inProcessName.ToUpper())
                            {
                                return (true);
                            }
                        }
                        catch { }
                    }
                    else if (PassinType.ToUpper() == "ProcessName".ToUpper())
                    {
                        try
                        {
                            if (p.ProcessName.ToUpper() == inProcessName.ToUpper())
                            {
                                return (true);
                            }
                        }
                        catch { }
                    }
                }
            }
            return (false);

        } // ProcessIsRunning()
        #endregion // Process Calls

        // FTP related calls
        #region FTP Calls

        // Bloomberg FTP
        public static Boolean FTPLoadStructure(ref Boolean isConfigured)
        {
            // Local Variables
            Boolean retVal = true;
            DataTable dt_FTP;

            isConfigured = true;

            // Make sure FTP structure is loaded from the database
            if (SystemLibrary.FTPVars.ServerIP.Length == 0)
            {
                // Load from database
                // FTP Details should come from database
                dt_FTP = SystemLibrary.SQLSelectToDataTable("Select * from FTP_Parameters");
                if (dt_FTP.Rows.Count > 0)
                {
                    // Load the data
                    try
                    {
                        SystemLibrary.FTPVars.ServerIP = dt_FTP.Rows[0]["ServerIP"].ToString();
                        SystemLibrary.FTPVars.ServerIP2 = dt_FTP.Rows[0]["ServerIP2"].ToString();
                        SystemLibrary.FTPVars.UserID = dt_FTP.Rows[0]["UserID"].ToString();
                        SystemLibrary.FTPVars.Password = dt_FTP.Rows[0]["Password"].ToString();
                        SystemLibrary.FTPVars.EMSXFileNameStartsWith = dt_FTP.Rows[0]["EMSXFileNameStartsWith"].ToString();
                        SystemLibrary.FTPVars.Interval_seconds = Convert.ToInt32(dt_FTP.Rows[0]["Interval_seconds"]);
                        SystemLibrary.FTPVars.LastUpdate = Convert.ToDateTime(dt_FTP.Rows[0]["LastUpdate"].ToString());
                    }
                    catch (Exception e)
                    {
                        SystemLibrary.DebugLine("FTPLoadStructure:" + e.Message);
                        SystemLibrary.FTPVars.ServerIP = "";
                        SystemLibrary.FTPVars.ServerIP2 = "";
                        retVal = false;
                    }
                }
                else
                {
                    isConfigured = false;
                    retVal = false;
                }
            }
            return (retVal);

        } // FTPLoadStructure()

        public static Boolean FTPSaveStructure()
        {
            // Local Variables
            Boolean retVal = true;
            DataTable dt_FTP;


            // Save To database
            dt_FTP = SystemLibrary.SQLSelectToDataTable("Select * from FTP_Parameters");
            if (dt_FTP.Rows.Count < 1)
            {
                DataRow dr = dt_FTP.NewRow();
                dt_FTP.Rows.Add(dr);
            }
            // Save the data
            try
            {
                dt_FTP.Rows[0]["ServerIP"] = SystemLibrary.FTPVars.ServerIP;
                dt_FTP.Rows[0]["ServerIP2"] = SystemLibrary.FTPVars.ServerIP2;
                dt_FTP.Rows[0]["UserID"] = SystemLibrary.FTPVars.UserID;
                dt_FTP.Rows[0]["Password"] = SystemLibrary.FTPVars.Password;
                dt_FTP.Rows[0]["EMSXFileNameStartsWith"] = SystemLibrary.FTPVars.EMSXFileNameStartsWith;
                dt_FTP.Rows[0]["Interval_seconds"] = SystemLibrary.FTPVars.Interval_seconds;
                dt_FTP.Rows[0]["LastUpdate"] = SystemLibrary.FTPVars.LastUpdate;
                SQLBulkUpdate(dt_FTP, "", "FTP_Parameters");
            }
            catch (Exception e)
            {
                SystemLibrary.DebugLine("SaveFTPStructure:" + e.Message);
                retVal = false;
            }
            return (retVal);

        } // FTPSaveStructure()

        // ML Prime
        public static Boolean FTPMLPrimeLoadStructure(ref Boolean isConfigured)
        {
            // Local Variables
            Boolean retVal = true;
            DataTable dt_FTP;

            isConfigured = true;

            // Make sure FTP structure is loaded from the database
            if (SystemLibrary.FTPMLPrimeVars.ServerIP.Length == 0)
            {
                // Load from database
                // FTP Details should come from database
                dt_FTP = SystemLibrary.SQLSelectToDataTable("Select * from FTPMLPrime_Parameters");
                if (dt_FTP.Rows.Count > 0)
                {
                    // Load the data
                    try
                    {
                        SystemLibrary.FTPMLPrimeVars.ServerIP = dt_FTP.Rows[0]["ServerIP"].ToString();
                        SystemLibrary.FTPMLPrimeVars.ServerIP2 = dt_FTP.Rows[0]["ServerIP2"].ToString();
                        SystemLibrary.FTPMLPrimeVars.UserID = dt_FTP.Rows[0]["UserID"].ToString();
                        SystemLibrary.FTPMLPrimeVars.Password = dt_FTP.Rows[0]["Password"].ToString();
                        SystemLibrary.FTPMLPrimeVars.EMSXFileNameStartsWith = dt_FTP.Rows[0]["EMSXFileNameStartsWith"].ToString();
                        SystemLibrary.FTPMLPrimeVars.Interval_seconds = Convert.ToInt32(dt_FTP.Rows[0]["Interval_seconds"]);
                        SystemLibrary.FTPMLPrimeVars.LastUpdate = Convert.ToDateTime(dt_FTP.Rows[0]["LastUpdate"].ToString());
                    }
                    catch (Exception e)
                    {
                        SystemLibrary.DebugLine("FTPMLPrimeLoadStructure:" + e.Message);
                        SystemLibrary.FTPMLPrimeVars.ServerIP = "";
                        SystemLibrary.FTPMLPrimeVars.ServerIP2 = "";
                        retVal = false;
                    }
                }
                else
                {
                    isConfigured = false;
                    retVal = false;
                }
            }
            return (retVal);

        } // FTPMLPrimeLoadStructure()

        public static Boolean FTPMLPrimeSaveStructure()
        {
            // Local Variables
            Boolean retVal = true;
            DataTable dt_FTP;


            // Save To database
            dt_FTP = SystemLibrary.SQLSelectToDataTable("Select * from FTPMLPrime_Parameters");
            if (dt_FTP.Rows.Count < 1)
            {
                DataRow dr = dt_FTP.NewRow();
                dt_FTP.Rows.Add(dr);
            }
            // Save the data
            try
            {
                dt_FTP.Rows[0]["ServerIP"] = SystemLibrary.FTPMLPrimeVars.ServerIP;
                dt_FTP.Rows[0]["ServerIP2"] = SystemLibrary.FTPMLPrimeVars.ServerIP2;
                dt_FTP.Rows[0]["UserID"] = SystemLibrary.FTPMLPrimeVars.UserID;
                dt_FTP.Rows[0]["Password"] = SystemLibrary.FTPMLPrimeVars.Password;
                dt_FTP.Rows[0]["EMSXFileNameStartsWith"] = SystemLibrary.FTPMLPrimeVars.EMSXFileNameStartsWith;
                dt_FTP.Rows[0]["Interval_seconds"] = SystemLibrary.FTPMLPrimeVars.Interval_seconds;
                dt_FTP.Rows[0]["LastUpdate"] = SystemLibrary.FTPMLPrimeVars.LastUpdate;
                SQLBulkUpdate(dt_FTP, "", "FTPMLPrime_Parameters");
            }
            catch (Exception e)
            {
                SystemLibrary.DebugLine("SaveFTPMLPrimeStructure:" + e.Message);
                retVal = false;
            }
            return (retVal);

        } // FTPMLPrimeSaveStructure()

        // SCOTIA Prime
        public static Boolean FTPSCOTIAPrimeLoadStructure(ref Boolean isConfigured)
        {
            // Local Variables
            Boolean retVal = true;
            DataTable dt_FTP;

            isConfigured = true;

            // Make sure FTP structure is loaded from the database
            if (SystemLibrary.FTPSCOTIAPrimeVars.ServerIP.Length == 0)
            {
                // Load from database
                // FTP Details should come from database
                dt_FTP = SystemLibrary.SQLSelectToDataTable("Select * from FTPSCOTIAPrime_Parameters");
                if (dt_FTP.Rows.Count > 0)
                {
                    // Load the data
                    try
                    {
                        SystemLibrary.FTPSCOTIAPrimeVars.ServerIP = dt_FTP.Rows[0]["ServerIP"].ToString();
                        SystemLibrary.FTPSCOTIAPrimeVars.ServerIP2 = dt_FTP.Rows[0]["ServerIP2"].ToString();
                        SystemLibrary.FTPSCOTIAPrimeVars.UserID = dt_FTP.Rows[0]["UserID"].ToString();
                        SystemLibrary.FTPSCOTIAPrimeVars.Password = dt_FTP.Rows[0]["Password"].ToString();
                        SystemLibrary.FTPSCOTIAPrimeVars.EMSXFileNameStartsWith = dt_FTP.Rows[0]["EMSXFileNameStartsWith"].ToString();
                        SystemLibrary.FTPSCOTIAPrimeVars.Interval_seconds = Convert.ToInt32(dt_FTP.Rows[0]["Interval_seconds"]);
                        SystemLibrary.FTPSCOTIAPrimeVars.LastUpdate = Convert.ToDateTime(dt_FTP.Rows[0]["LastUpdate"].ToString());
                    }
                    catch (Exception e)
                    {
                        SystemLibrary.DebugLine("FTPSCOTIAPrimeLoadStructure:" + e.Message);
                        SystemLibrary.FTPSCOTIAPrimeVars.ServerIP = "";
                        SystemLibrary.FTPSCOTIAPrimeVars.ServerIP2 = "";
                        retVal = false;
                    }
                }
                else
                {
                    isConfigured = false;
                    retVal = false;
                }
            }
            return (retVal);

        } // FTPSCOTIAPrimeLoadStructure()

        public static Boolean FTPSCOTIAPrimeSaveStructure()
        {
            // Local Variables
            Boolean retVal = true;
            DataTable dt_FTP;


            // Save To database
            dt_FTP = SystemLibrary.SQLSelectToDataTable("Select * from FTPSCOTIAPrime_Parameters");
            if (dt_FTP.Rows.Count < 1)
            {
                DataRow dr = dt_FTP.NewRow();
                dt_FTP.Rows.Add(dr);
            }
            // Save the data
            try
            {
                dt_FTP.Rows[0]["ServerIP"] = SystemLibrary.FTPSCOTIAPrimeVars.ServerIP;
                dt_FTP.Rows[0]["ServerIP2"] = SystemLibrary.FTPSCOTIAPrimeVars.ServerIP2;
                dt_FTP.Rows[0]["UserID"] = SystemLibrary.FTPSCOTIAPrimeVars.UserID;
                dt_FTP.Rows[0]["Password"] = SystemLibrary.FTPSCOTIAPrimeVars.Password;
                dt_FTP.Rows[0]["EMSXFileNameStartsWith"] = SystemLibrary.FTPSCOTIAPrimeVars.EMSXFileNameStartsWith;
                dt_FTP.Rows[0]["Interval_seconds"] = SystemLibrary.FTPSCOTIAPrimeVars.Interval_seconds;
                dt_FTP.Rows[0]["LastUpdate"] = SystemLibrary.FTPSCOTIAPrimeVars.LastUpdate;
                SQLBulkUpdate(dt_FTP, "", "FTPSCOTIAPrime_Parameters");
            }
            catch (Exception e)
            {
                SystemLibrary.DebugLine("SaveFTPSCOTIAPrimeStructure:" + e.Message);
                retVal = false;
            }
            return (retVal);

        } // FTPSCOTIAPrimeSaveStructure()

        public static String FTPTestStructure(String ServerIP, String UserID, String Password)
        {
            // Local Variables
            String retVal = "";

            // Test Connect to the Server
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ServerIP + "/"));
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.Credentials = new NetworkCredential(UserID, Password);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                response.Close();
                retVal = "";
                reqFTP.Abort();
                reqFTP = null;
            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }

            return (retVal);

        } // FTPTestStructure()

        public static String[] FTPGetFileList(FTPStruct inVars, String RemotePath)
        {
            // Connect to the Server and return a list of files that are appropriate
            String[] downloadFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + inVars.ServerIP + RemotePath + "/"));
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.UsePassive = true;
                reqFTP.Credentials = new NetworkCredential(inVars.UserID, inVars.Password);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                //MessageBox.Show(reader.ReadToEnd());
                String line = reader.ReadLine();
                while (line != null)
                {
                    if (line.StartsWith(inVars.EMSXFileNameStartsWith))
                    {
                        result.Append(line);
                        result.Append("\n");
                    }
                    line = reader.ReadLine();
                }
                if(result.Length>0)
                    result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                //MessageBox.Show(response.StatusDescription);
                reqFTP.Abort();
                reqFTP = null;
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                SystemLibrary.DebugLine("FTPGetFileList:"+ ex.Message);
                downloadFiles = null;
                return downloadFiles;
            }
        } // FTPGetFileList

        public static Boolean FTPDownloadFile(FTPStruct inVars, string filePath, string RemotePath, string fileName)
        {
            // Local Variables
            Boolean retVal = true;
            FtpWebRequest reqFTP;

            SystemLibrary.DebugLine("FTPDownloadFile(START): " + fileName);
            try
            {
                //filePath = <<The full path where the file is to be created.>>, 
                //fileName = <<Name of the file to be created(Need not be the name of the file on FTP server).>>
                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + inVars.ServerIP + RemotePath + "/" + fileName));
                reqFTP.UsePassive = true;
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.Credentials = new NetworkCredential(inVars.UserID, inVars.Password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                SystemLibrary.DebugLine("FTPDownloadFile.DownloadFile(" + fileName + "):" + ex.Message);
                retVal = false;
            }

            if (retVal)
            {
                try
                {
                    // Now set the file creation time to same as FTP file
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + inVars.ServerIP + RemotePath + "/" + fileName));
                    reqFTP.UsePassive = true;
                    reqFTP.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                    reqFTP.UseBinary = true;
                    reqFTP.KeepAlive = false;
                    reqFTP.Credentials = new NetworkCredential(inVars.UserID, inVars.Password);
                    FtpWebResponse response1 = (FtpWebResponse)reqFTP.GetResponse();

                    File.SetCreationTime(filePath + "\\" + fileName, response1.LastModified);
                    File.SetLastWriteTime(filePath + "\\" + fileName, response1.LastModified);
                    File.SetLastAccessTime(filePath + "\\" + fileName, response1.LastModified);
                    reqFTP.Abort();
                    reqFTP = null;
                }
                catch (Exception ex1)
                {
                    SystemLibrary.DebugLine("FTPDownloadFile.GetDateTimestamp(" + fileName + "):" + ex1.Message);
                    // Even if we can't get the timestamp, just keep going
                    //retVal = false;
                }
            }

            SystemLibrary.DebugLine("FTPDownloadFile(END): " + retVal.ToString());
            return (retVal);

        } //FTPDownloadFile()

        public static void FTPDeleteFile(FTPStruct inVars, String RemotePath, string fileName)
        {
            try
            {
                string uri = "ftp://" + RemotePath + inVars.ServerIP + "/" + fileName;
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + inVars.ServerIP + RemotePath + "/" + fileName));

                reqFTP.Credentials = new NetworkCredential(inVars.UserID, inVars.Password);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
                reqFTP.Abort();
                reqFTP = null;
            }
            catch (Exception ex)
            {
                SystemLibrary.DebugLine("FTPDeleteFile(" + fileName + "):" + ex.Message);
            }
        } //FTPDeleteFile()

        public static Boolean FTPUploadFile(FTPStruct inVars, string filePath, String RemotePath, string fileName)
        {
            // Local Variables
            Boolean retVal = true;
            FtpWebRequest reqFTP;
            Stream sourceStream = new MemoryStream();
            Stream requestStream = sourceStream;

            SystemLibrary.DebugLine("FTPUploadFile(START): " + fileName);
            try
            {
                //filePath = <<The full path where the file is to be created.>>, 
                //fileName = <<Name of the file to be created(Need not be the name of the file on FTP server).>>

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + inVars.ServerIP + RemotePath + "/" + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.Credentials = new NetworkCredential(inVars.UserID, inVars.Password);

                // Copy the contents of the file to the request stream.

                sourceStream = new FileStream(filePath + "\\" + fileName, FileMode.Open);
                requestStream = reqFTP.GetRequestStream();
                reqFTP.ContentLength = 0;

                int bufferSize = 2048;
                byte[] buffer = new byte[bufferSize];
                int bytesRead = sourceStream.Read(buffer, 0, bufferSize);

                do
                {
                    requestStream.Write(buffer, 0, bytesRead);
                    reqFTP.ContentLength = reqFTP.ContentLength + bytesRead;
                    bytesRead = sourceStream.Read(buffer, 0, bufferSize);

                } while (bytesRead > 0);

                sourceStream.Close();
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
                reqFTP.Abort();
                reqFTP = null;
            }
            catch (Exception ex)
            {
                sourceStream.Close();
                requestStream.Close();
                SystemLibrary.DebugLine("FTPUploadFile(" + fileName + "):" + ex.Message);
                retVal = false;
            }
            SystemLibrary.DebugLine("FTPUploadFile(END): " + retVal.ToString());
            return (retVal);

        } //FTPUploadFile()


        #endregion //FTP Calls

        #region ML Futures Files

        public static int MLFuturesGetFiles(ref Boolean isConfigured)
        {
            // 
            // Procedure: MLFuturesGetFiles
            //
            // Purpose: Get a list of ML Futures files to Upload to the database
            //
            // Written: Colin Ritchie 20-Jun-2011
            //
            // Modified:
            //

            // Local Variables
            String myFileName;

            isConfigured = true;


            try
            {
                if (MLFuturesFilePath == null)
                    MLFuturesFilePath = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('MLFutures_Path')");

                if (MLFuturesFilePath.Length < 1)
                {
                    isConfigured = false;
                    return (0);
                }

                if (!Directory.Exists(MLFuturesFilePath))
                {
                    isConfigured = false;
                    return (0);
                }

                DirectoryInfo di = new DirectoryInfo(MLFuturesFilePath);
                FileInfo[] rgFiles = di.GetFiles("*.csv");

                for (int i = 0; i < rgFiles.Length; i++)
                {
                    myFileName = rgFiles[i].Name;
                    MLFuturesSaveData(MLFuturesFilePath, myFileName);
                }

                // Return the number of files 
                return (rgFiles.Length);
            }
            catch
            {
                return (0);
            }


        } //MLFuturesGetFiles()

        public static Boolean MLFuturesSaveData(String filePath, String fileName)
        {
            //
            // Rules: Expect fileName of the form <FUND>_FINAL_Cash_yyyymmdd.csv
            //

            // Local Variables
            Boolean retVal = true;
            Int32 myRows;


            // Header
            DataTable dt_Header = SystemLibrary.SQLSelectToDataTable("Select * from MLFut_File_header Where MLFileName='" + fileName + "' ");
            if (dt_Header.Rows.Count < 1)
            {
                retVal = false;

                // See if the file is locked
                try
                {
                    FileInfo fileInfo = new System.IO.FileInfo(filePath + "\\" + fileName);
                    if (fileInfo.IsReadOnly)
                        return (false);
                }
                catch
                {
                    return (false);
                }
                // Only deal with new data
                DataRow dr = dt_Header.NewRow();
                dt_Header.Rows.Add(dr);
                // ToDo (5) 11-Mar-2011 - Enable this "try" in EMSXSaveData() - once testing is completed.
                //try
                {
                    // Save the Header data
                    // Using Database rather than local users dates.
                    dt_Header.Rows[0]["MLFileName"] = fileName;
                    dt_Header.Rows[0]["MLFileDate"] = SystemLibrary.GetCreationTime(filePath + "\\" + fileName);
                    dt_Header.Rows[0]["UploadDate"] = SystemLibrary.f_Now();
                    myRows = SQLBulkUpdate(dt_Header, "", "MLFut_File_header");
                    if (myRows == 1)
                    {
                        // Now load the detail. Format "<Fund>_FINAL_trades_20110617.CSV"
                        int Pos1 = fileName.IndexOf('_');
                        int Pos2 = -1;
                        int Pos3 = -1;
                        if (Pos1 > 0)
                            Pos2 = fileName.IndexOf('_', Pos1 + 1);
                        if (Pos2 > 0)
                            Pos3 = fileName.Length - "_20110617.CSV".Length;
                        if (Pos1 < 0 || Pos3 < 0)
                            return (false);
                        String TableName = "MLFut_" + fileName.Substring(Pos2 + 1, Pos3 - Pos2 - 1);
                        DataTable dt_Detail = SystemLibrary.SQLSelectToDataTable("Select * from " + TableName + " where 1=2");
                        String[] MLData = File.ReadAllLines(filePath + "\\" + fileName);
                        if (MLData.Length > 0 && dt_Detail != null)
                        {
                            String[] Header;

                            Header = MLData[0].Split(',');
                            for (Int32 j = 0; j < Header.Length; j++)
                            {
                                // In the Header, replace spaces with _
                                Header[j] = Header[j].Trim().Replace(" ", "_");
                                // Remove leading & trailing quotes "<header>"
                                Header[j] = Header[j].Substring(1, Header[j].Length - 2);
                            }
                            for (Int32 i = 1; i < MLData.Length; i++)
                            {
                                DataRow dr_D = dt_Detail.NewRow();
                                dr_D["MLFileName"] = fileName;
                                String[] Detail = MLData[i].Split(',');
                                for (Int32 j = 0; j < Header.Length; j++) // NB: Some of the ML files have trailing
                                {
                                    // Remove leading & trailing quotes "<value>"
                                    if (Detail[j].Length>0)
                                        if (Detail[j].Substring(0,1)==@"""")
                                            Detail[j] = Detail[j].Substring(1, Detail[j].Length - 2);
                                    Detail[j] = Detail[j].Trim();

                                    if (dt_Detail.Columns[Header[j]].DataType.Name == "DateTime")
                                    {
                                        // Inbound format "YYYYMMDD"
                                        String[] mySplit = Detail[j].Split('/');
                                        if (Detail[j].Length == 8)
                                        {
                                            int myDay = Convert.ToInt16(Detail[j].Substring(6, 2));
                                            int myMonth = Convert.ToInt16(Detail[j].Substring(4, 2));
                                            int myYear = Convert.ToInt16(Detail[j].Substring(0, 4));

                                            if (!(myDay == 0 || myMonth == 0 || myYear == 0))
                                                dr_D[Header[j]] = new DateTime(myYear, myMonth, myDay);
                                        }
                                    }
                                    else
                                    {
                                        if (Detail[j] == "" && dt_Detail.Columns[Header[j]].DataType.Name == "Decimal")
                                            dr_D[Header[j]] = 0;
                                        else
                                            dr_D[Header[j]] = Detail[j];
                                    }

                                }
                                dt_Detail.Rows.Add(dr_D);
                            }
                            myRows = SQLBulkUpdate(dt_Detail, "", TableName);
                            // Process the Dtat & Remove the temporary file
                            if (myRows == MLData.Length - 1)
                            {
                                // Archive the File
                                // -- See if the Archive Folder exists & if not create one.
                                if (!System.IO.File.Exists(filePath + @"\Archive"))
                                    System.IO.Directory.CreateDirectory(filePath + @"\Archive");

                                // -- Move the file to Archive directory
                                try
                                {
                                    // See if File Already exists
                                    if (!System.IO.File.Exists(filePath + @"\Archive\" + fileName))
                                        System.IO.File.Move(filePath + @"\" + fileName, filePath + @"\Archive\" + fileName);
                                    else
                                    {
                                        System.IO.File.Move(filePath + @"\" + fileName, filePath + @"\Archive\" + fileName +
                                            "_" + System.IO.File.GetLastAccessTime(filePath + @"\" + fileName).ToString("HHmmss"));
                                    }
                                }
                                catch { }
                                
                                // Set LastUpdate
                                SystemLibrary.SQLExecute("Exec sp_MLFut_Process_File '" + fileName + "', '" + TableName + "' ");
                            }
                            retVal = true;
                        }
                        else
                            retVal = true; // Empty file, but OK.
                    }
                }
                /*
                catch (Exception e)
                {
                    SystemLibrary.DebugLine("MLFuturesSaveData(" + fileName + "):" + e.Message);
                    retVal = false;
                }
                */
            }
            else
            {
                // Archive the File
                // -- See if the Archive Folder exists & if not create one.
                if (!System.IO.File.Exists(filePath + @"\Archive"))
                    System.IO.Directory.CreateDirectory(filePath + @"\Archive");

                // -- Move the file to Archive directory
                try
                {
                    // See if File Already exists
                    if (!System.IO.File.Exists(filePath + @"\Archive\" + fileName))
                        System.IO.File.Move(filePath + @"\" + fileName, filePath + @"\Archive\" + fileName);
                    else
                    {
                        System.IO.File.Move(filePath + @"\" + fileName, filePath + @"\Archive\" + fileName +
                            "_" + System.IO.File.GetLastAccessTime(filePath + @"\" + fileName).ToString("HHmmss"));
                    }
                }
                catch { }
            }

            return (retVal);


        } //MLFuturesSaveData()

        #endregion //ML Futures Files


        #region Mainstream BPO Files
        public static int BPOGetFiles()
        {
            // 
            // Procedure: BPOGetFiles
            //
            // Purpose: Get a list of Mainstream BPO files to Upload to the database
            //
            // Written: Colin Ritchie 18-Jul-2011
            //
            // Modified:
            //

            // Local Variables
            String myFilePath = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('MainStreamBPOs_Path')");
            String myFileName;

            try
            {
                DirectoryInfo di = new DirectoryInfo(myFilePath);
                FileInfo[] rgFiles = di.GetFiles("*.csv");
                //FileInfo[] rgFiles = di.GetFiles("Cash.csv");

                for (int i = 0; i < rgFiles.Length; i++)
                {
                    myFileName = rgFiles[i].Name;
                    BPOSaveData(myFilePath, myFileName);
                }

                // Return the number of files 
                return (rgFiles.Length);
            }
            catch 
            {
                return (0);
            }

        } //BPOGetFiles()

        public static Boolean BPOSaveData(String filePath, String fileName)
        {
            //
            // Rules: Expect fileName of Cash.csv or Valuation.csv
            //

            // Local Variables
            Boolean retVal = true;
            Int32 myRows;
            Int32 myRows1;
            DateTime myFileDate = SystemLibrary.GetCreationTime(filePath + "\\" + fileName);
            String mySql;


            // Header
            mySql = "Select * " +
                    "From  BPO_File_header " +
                    "Where BPOFileName='" + fileName + "' " +
                    "And   BPOFileDate='" + myFileDate.ToString("dd-MMM-yyyy hh:mm:ss.fff") + "' ";
            DataTable dt_Header = SystemLibrary.SQLSelectToDataTable(mySql);
            if (dt_Header.Rows.Count < 1)
            {
                retVal = false;

                // See if the file is locked
                try
                {
                    FileInfo fileInfo = new System.IO.FileInfo(filePath + "\\" + fileName);
                    if (fileInfo.IsReadOnly)
                        return (false);
                }
                catch
                {
                    return (false);
                }
                // Only deal with new data
                DataRow dr = dt_Header.NewRow();
                dt_Header.Rows.Add(dr);
                // ToDo (5) 11-Mar-2011 - Enable this "try" - once testing is completed.
                //try
                {
                    // Save the Header data
                    // Using Database rather than local users dates.
                    dt_Header.Rows[0]["BPOFileName"] = fileName;
                    dt_Header.Rows[0]["BPOFileDate"] = myFileDate;
                    dt_Header.Rows[0]["UploadDate"] = SystemLibrary.f_Now();
                    myRows = SQLBulkUpdate(dt_Header, "", "BPO_File_header");
                    if (myRows == 1)
                    {
                        String TableName = "";
                        String TableName1 = "";
                        String[] BPOData;
                        if (fileName.ToLower().Contains("cash"))
                        {
                            // There are 2 tables that feed off Cash file.
                            //      Both share the same header data, with the "BPO_Cash_Balance" table reflecting the first and 
                            //      last records of the blocks.
                            DateTime Report_date = new DateTime();	// Extracted from Row7 - Take the "to" date.
                            String Client_name = ""; //		Varchar(254), -- Extracted from Row1
                            String Portfolio_Code = ""; //	Varchar(254), -- Extracted from Row5
                            // Too Hard to find a pattern 20110718 String PortfolioName = ""; //	Varchar(254),  -- Extracted from Row 3
                            String Account_code = ""; //	Varchar(254), -- Extracted from Row 4
                            String Currency = "";
                            Decimal Amount_SOD = 0;
                            Decimal Amount = 0;

                            TableName = "BPO_Cash_Transactions";
                            TableName1 = "BPO_Cash_Balance";
                            BPOData = File.ReadAllLines(filePath + "\\" + fileName);
                            if (BPOData.Length > 0)
                            {
                                DataTable dt_Detail = SystemLibrary.SQLSelectToDataTable("Select * from " + TableName + " where 1=2");
                                DataTable dt_Cash_Balance = SystemLibrary.SQLSelectToDataTable("Select * from " + TableName1 + " where 1=2");
                                for (Int32 i = 0; i < BPOData.Length; i++)
                                {
                                    // Find the portfolio Name
                                    if (BPOData[i].ToUpper().Substring(0, 1) == "C")
                                    {
                                        continue; //Get next record
                                    }
                                    else if (BPOData[i].ToUpper().Substring(0, 1) == "R" && BPOData[i].ToUpper().IndexOf("CASH ACCOUNT STATEMENT") > 0)
                                    {
                                        int Pos1 = BPOData[i].ToUpper().IndexOf("CASH ACCOUNT STATEMENT");
                                        if (Pos1 > 0)
                                        {
                                            String[] Detail = BPOData[i].Substring(0, Pos1).Trim().Split(',');
                                            if (Detail.Length > 1)
                                                Client_name = Detail[1].Trim();
                                        }
                                        continue; //Get next record
                                    }
                                    else if (BPOData[i].ToUpper().Substring(0, 1) == "R" && BPOData[i].ToUpper().IndexOf("BANK ACCOUNT:") > 0)
                                    {
                                        int Pos1 = BPOData[i].ToUpper().IndexOf("BANK ACCOUNT:");
                                        if (Pos1 > 0)
                                        {
                                            Account_code = BPOData[i].Substring(Pos1 + "BANK ACCOUNT:".Length).Trim();
                                        }
                                        continue; //Get next record
                                    }
                                    else if (BPOData[i].ToUpper().Substring(0, 1) == "R" && BPOData[i].ToUpper().IndexOf("PORTFOLIO CODE:") > 0)
                                    {
                                        int Pos1 = BPOData[i].ToUpper().IndexOf("PORTFOLIO CODE:");
                                        if (Pos1 > 0)
                                        {
                                            Portfolio_Code = BPOData[i].Substring(Pos1 + "PORTFOLIO CODE:".Length).Trim();
                                        }
                                        continue; //Get next record
                                    }
                                    else if (BPOData[i].ToUpper().Substring(0, 1) == "R" && BPOData[i].ToUpper().IndexOf("CURRENCY:") > 0)
                                    {
                                        int Pos1 = BPOData[i].ToUpper().IndexOf("CURRENCY:");
                                        if (Pos1 > 0)
                                        {
                                            Currency = BPOData[i].Substring(Pos1 + "CURRENCY:".Length).Trim();
                                        }
                                        continue; //Get next record
                                    }
                                    else if (BPOData[i].ToUpper().Substring(0, 1) == "R" && BPOData[i].ToUpper().IndexOf("STATEMENT OF ACCOUNT") > 0)
                                    {
                                        int Pos1 = BPOData[i].ToUpper().IndexOf("STATEMENT OF ACCOUNT");
                                        if (Pos1 > 0)
                                        {
                                            // lOOKING FOR 15/07/2011 ON THE RIGHT
                                            String StrReport_date = BPOData[i].Trim().Substring(BPOData[i].Length - 10);
                                            String[] mySplit = StrReport_date.Split('/');
                                            if (mySplit.Length == 3)
                                            {
                                                int myDay = Convert.ToInt32(mySplit[0]);
                                                int myMonth = Convert.ToInt32(mySplit[1]);
                                                int myYear = Convert.ToInt32(mySplit[2]);
                                                if (!(myDay == 0 || myMonth == 0 || myYear == 0))
                                                    Report_date = new DateTime(myYear, myMonth, myDay);
                                            }
                                        }
                                        continue; //Get next record
                                    }
                                    else if (BPOData[i].ToUpper().Substring(0, 1) == "D" && BPOData[i].IndexOf("Balance b/forward") > 0)
                                    {
                                        // Reset
                                        Amount_SOD = 0;
                                        Amount = 0;
                                        String[] mySplit = BPOData[i].Split(',');
                                        if (mySplit.Length == 8)
                                        {
                                            Amount_SOD = SystemLibrary.ToDecimal(mySplit[7].Trim());
                                        }
                                        continue; //Get next record
                                    }
                                    else if (BPOData[i].ToUpper().Substring(0, 1) == "T" && BPOData[i].IndexOf("TOTALS") > 0)
                                    {
                                        // Get Amount
                                        String[] mySplit = BPOData[i].Split(',');
                                        if (mySplit.Length == 8)
                                        {
                                            Amount = SystemLibrary.ToDecimal(mySplit[7].Trim());
                                        }
                                        // Process Row into Table.
                                        DataRow dr_D = dt_Cash_Balance.NewRow();
                                        dr_D["BPOFileDate"] = myFileDate;
                                        dr_D["Report_date"] = Report_date;
                                        dr_D["Client_name"] = Client_name;
                                        dr_D["Portfolio_Code"] = Portfolio_Code;
                                        //dr_D["PortfolioName"] = PortfolioName;
                                        dr_D["Account_code"] = Account_code;
                                        dr_D["Currency"] = Currency;
                                        dr_D["Amount_SOD"] = Amount_SOD;
                                        dr_D["Amount"] = Amount;
                                        dt_Cash_Balance.Rows.Add(dr_D);
                                        continue; //Get next record
                                    }
                                    else if (BPOData[i].ToUpper().Substring(0, 1) == "D")
                                    {
                                        // Need to deal with the fact that the "Details" coilumn can have commas in it also
                                        // - The 5th column in the row can contain multi-commas as it has qty like 2,012
                                        //      To deal with, do the split. 
                                        //      Columns 0-3 are OK. 
                                        //      Last 3 columns are OK
                                        //      Merge anything else back together using commas
                                        String[] mySplit = BPOData[i].Split(',');
                                        if (mySplit.Length >= 8)
                                        {
                                            // Process Row into Table.
                                            DataRow dr_D = dt_Detail.NewRow();
                                            dr_D["BPOFileDate"] = myFileDate;
                                            dr_D["Report_date"] = Report_date;
                                            dr_D["Client_name"] = Client_name;
                                            dr_D["Portfolio_Code"] = Portfolio_Code;
                                            //dr_D["PortfolioName"] = PortfolioName;
                                            dr_D["Account_code"] = Account_code;
                                            dr_D["Currency"] = Currency;

                                            // lOOKING FOR 15/07/2011 ON THE RIGHT
                                            String[] mySplitDate = mySplit[1].Split('/');
                                            if (mySplitDate.Length == 3)
                                            {
                                                int myDay = Convert.ToInt32(mySplitDate[0]);
                                                int myMonth = Convert.ToInt32(mySplitDate[1]);
                                                int myYear = Convert.ToInt32(mySplitDate[2]);
                                                if (!(myDay == 0 || myMonth == 0 || myYear == 0))
                                                    dr_D["EffectiveDate"] = new DateTime(myYear, myMonth, myDay);
                                            }
                                            dr_D["ID"] = SystemLibrary.ToDecimal(mySplit[2].Trim());
                                            dr_D["TranType"] = mySplit[3].Trim();
                                            Decimal Credit = SystemLibrary.ToDecimal(mySplit[mySplit.Length - 2].Trim());
                                            Decimal Debit = -1.0m * SystemLibrary.ToDecimal(mySplit[mySplit.Length - 3].Trim());
                                            dr_D["Amount"] = Credit + Debit; // NB: One of these will be zero.
                                            String Details = "";
                                            for (int j = 4; j <= mySplit.Length - 4; j++)
                                            {
                                                if (j == mySplit.Length - 4)
                                                    Details = Details + mySplit[j].Trim();
                                                else
                                                    Details = Details + mySplit[j].Trim() + ",";
                                            }
                                            dr_D["Details"] = Details;
                                            dt_Detail.Rows.Add(dr_D);
                                        }
                                        continue; //Get next record
                                    }
                                }
                                myRows = SQLBulkUpdate(dt_Detail, "", TableName);
                                myRows1 = SQLBulkUpdate(dt_Cash_Balance, "", TableName1);
                                // Process the Dtat & Remove the temporary file
                                if (myRows == dt_Detail.Rows.Count && myRows1 == dt_Cash_Balance.Rows.Count)
                                {
                                    // Archive the File
                                    // -- See if the Archive Folder exists & if not create one.
                                    if (!System.IO.Directory.Exists(filePath + @"\Archive"))
                                        System.IO.Directory.CreateDirectory(filePath + @"\Archive");

                                    // -- Move the file to Archive directory
                                    System.IO.File.Move(filePath + @"\" + fileName, filePath + @"\Archive\cash" + myFileDate.ToString(" yyyyMMdd-hhmmss") + ".csv");

                                    // Set LastUpdate
                                    SystemLibrary.SQLExecute("Exec sp_BPO_Process_File '" + myFileDate + "', '" + TableName + "' ");
                                }
                                retVal = true;
                            }
                            else
                                retVal = true; // Empty file, but OK.
                        }
                        else if (fileName.ToLower().Contains("valuation"))
                        {
                            String PortfolioName = "";
                            DateTime EffectiveDate = new DateTime();
                            String Currency_code = "";
                            Boolean inShareBlock = false;
                            Boolean inFuturesBlock = false;

                            TableName = "BPO_Valuation";
                            BPOData = File.ReadAllLines(filePath + "\\" + fileName);
                            if (BPOData.Length > 0)
                            {
                                DataTable dt_Detail = SystemLibrary.SQLSelectToDataTable("Select * from " + TableName + " where 1=2");
                                for (Int32 i = 0; i < BPOData.Length; i++)
                                {
                                    // First Line has the portfolio Name
                                    if (i == 0)
                                    {
                                        //R,  Portfolio Name:    Aust Equity - Mandate
                                        int Pos1 = BPOData[i].IndexOf(':');
                                        if (Pos1 > 0)
                                            PortfolioName = BPOData[i].Substring(Pos1 + 1).Trim();
                                        continue; //Get next record
                                    }
                                    // Second Line: EffectiveDate & Currency
                                    else if (i == 1)
                                    {
                                        //R,  Valuation Date:    15/07/2011   Portfolio Currency: AUD          Price Type: F
                                        int Pos1 = BPOData[i].IndexOf(':');
                                        if (Pos1 > 0)
                                        {
                                            int Pos2 = BPOData[i].IndexOf("Portfolio Currency:", Pos1 + 1);
                                            if (Pos2 > 0)
                                            {
                                                int Pos3 = BPOData[i].IndexOf("Price Type:", Pos2);
                                                if (Pos3 > 0)
                                                {
                                                    Currency_code = BPOData[i].Substring(Pos2 + "Portfolio Currency:".Length + 1, Pos3 - (Pos2 + "Portfolio Currency:".Length + 1)).Trim();
                                                }
                                                // Inbound format "dd/mm/yyyy"
                                                String StrValuationDate = BPOData[i].Substring(Pos1 + 1, Pos2 - (Pos1 + 1)).Trim();
                                                String[] mySplit = StrValuationDate.Split('/');
                                                if (mySplit.Length == 3)
                                                {
                                                    int myDay = Convert.ToInt32(mySplit[0]);
                                                    int myMonth = Convert.ToInt32(mySplit[1]);
                                                    int myYear = Convert.ToInt32(mySplit[2]);
                                                    if (!(myDay == 0 || myMonth == 0 || myYear == 0))
                                                        EffectiveDate = new DateTime(myYear, myMonth, myDay);
                                                }
                                            }
                                        }
                                        continue; //Get next record
                                    }

                                    // Process rest of File
                                    String[] Detail = BPOData[i].Split(',');
                                    if (Detail.Length < 2)
                                        continue; //Get next record
                                    if (Detail[0].Trim().ToUpper() == "S" && Detail[1].Trim().ToUpper() == "FUTURES")
                                    {
                                        inFuturesBlock = true;
                                        continue; //Get next record
                                    }
                                    else if (Detail[0].Trim().ToUpper() == "T" && Detail[1].Trim().ToUpper() == "TOTAL FUTURES")
                                    {
                                        inFuturesBlock = false;
                                        continue; //Get next record
                                    }
                                    else if (Detail[0].Trim().ToUpper() == "S" && Detail[1].Trim().ToUpper() == "ORDINARY SHARES")
                                    {
                                        inShareBlock = true;
                                        continue; //Get next record
                                    }
                                    else if (Detail[0].Trim().ToUpper() == "T" && Detail[1].Trim().ToUpper() == "TOTAL ORDINARY SHARES")
                                    {
                                        inShareBlock = false;
                                        continue; //Get next record
                                    }
                                    else if (Detail[0].Trim().ToUpper() == "D" && (inFuturesBlock || inShareBlock))
                                    {
                                        // Process Row into Table.
                                        DataRow dr_D = dt_Detail.NewRow();
                                        dr_D["BPOFileDate"] = myFileDate;
                                        dr_D["PortfolioName"] = PortfolioName;
                                        dr_D["EffectiveDate"] = EffectiveDate;
                                        dr_D["Currency_code"] = Currency_code;
                                        dr_D["SecurityName"] = Detail[1];
                                        dr_D["SEDOL"] = Detail[2];
                                        if (inFuturesBlock)
                                            dr_D["inBBG_Ticker"] = Detail[3].ToString().Trim() + " Index";
                                        else
                                            dr_D["inBBG_Ticker"] = Detail[3].ToString().Trim() + " Equity";
                                        dr_D["Quantity"] = SystemLibrary.ToDecimal(Detail[4].Trim());
                                        dr_D["UnitCost"] = SystemLibrary.ToDecimal(Detail[5].Trim());
                                        dr_D["ClosePrice"] = SystemLibrary.ToDecimal(Detail[6].Trim());
                                        dr_D["TotalCost"] = SystemLibrary.ToDecimal(Detail[7].Trim());
                                        dr_D["AccruedInterest"] = SystemLibrary.ToDecimal(Detail[8].Trim());
                                        dr_D["TotalValue"] = SystemLibrary.ToDecimal(Detail[9].Trim());
                                        dr_D["Pct_Total"] = SystemLibrary.ToDecimal(Detail[10].Trim());
                                        dt_Detail.Rows.Add(dr_D);
                                    }
                                }
                                myRows = SQLBulkUpdate(dt_Detail, "", TableName);
                                // Process the Data & Remove the temporary file
                                if (myRows == dt_Detail.Rows.Count)
                                {
                                    // Archive the File
                                    // -- See if the Archive Folder exists & if not create one.
                                    if (!System.IO.Directory.Exists(filePath + @"\Archive"))
                                        System.IO.Directory.CreateDirectory(filePath + @"\Archive");

                                    // -- Move the file to Archive directory
                                    System.IO.File.Move(filePath + @"\" + fileName, filePath + @"\Archive\valuation"  + myFileDate.ToString(" yyyyMMdd-hhmmss") + ".csv");
                                    
                                    // Set LastUpdate
                                    SystemLibrary.SQLExecute("Exec sp_BPO_Process_File '" + myFileDate + "', '" + TableName + "' ");
                                }
                                retVal = true;
                            }
                            else
                                retVal = true; // Empty file, but OK.
                        }
                    }
                }
            }
            else
            {
                // Archive the File
                // -- See if the Archive Folder exists & if not create one.
                if (!System.IO.Directory.Exists(filePath + @"\Archive"))
                    System.IO.Directory.CreateDirectory(filePath + @"\Archive");

                // -- Move the file to Archive directory
                //      Replace if previously archived
                if (System.IO.File.Exists(filePath + @"\Archive\" + fileName))
                    System.IO.File.Replace(filePath + @"\" + fileName, filePath + @"\Archive\" + fileName, filePath + @"\Archive\" + fileName + "_" + SystemLibrary.f_Now().ToString("yyyyMMMdd_HHmmss"));
                else
                    System.IO.File.Move(filePath + @"\" + fileName, filePath + @"\Archive\" + fileName);
            }

            return (retVal);


        } //BPOSaveData()

        #endregion // Mainstream BPO Files

        #region ML Prime Files

        public static int MLPrimeGetFiles(ref Boolean isConfigured)
        {
            // 
            // Procedure: MLPrimeGetFiles
            //
            // Purpose: Get a list of ML Prime files to Upload to the database
            //
            // Written: Colin Ritchie 17-Jun-2011
            //
            // Modified:
            //

            // Local Variables
            String myFileName;
            int TotFiles = 0;

            isConfigured = true;

            try
            {
                if (MLPrimeFilePath == null)
                    MLPrimeFilePath = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('MLPrime_Path')");

                if (MLPrimeFilePath.Length < 1)
                {
                    isConfigured = false;
                    return (0);
                }

                if (!Directory.Exists(MLPrimeFilePath))
                {
                    isConfigured = false;
                    return (0);
                }

                DirectoryInfo di = new DirectoryInfo(MLPrimeFilePath);
                FileInfo[] rgFiles = di.GetFiles("*_E*_*.csv");

                TotFiles = rgFiles.Length;
                for (int i = 0; i < rgFiles.Length; i++)
                {
                    myFileName = rgFiles[i].Name;
                    MLPrimeSaveData(MLPrimeFilePath, myFileName);
                }

                // Check for confirm and error files
                rgFiles = di.GetFiles("*.txt.*");

                TotFiles = TotFiles + rgFiles.Length;
                for (int i = 0; i < rgFiles.Length; i++)
                {
                    myFileName = rgFiles[i].Name;
                    MLPrimeSaveDataConfirm(MLPrimeFilePath, myFileName);
                }
            }
            catch (Exception e)
            {
                SystemLibrary.DebugLine("MLPrimeGetFiles (" + SystemLibrary.ToString(SCOTIAPrimeFilePath) + ") " + e.Message);
            }

            // Return the number of files 
            return (TotFiles);

        } //MLPrimeGetFiles()

        public static Boolean MLPrimeSaveDataConfirm(String filePath, String fileName)
        {
            //
            // Rules: Expect fileName of the form confirm.txt.* or error.txt.*
            //

            // Local Variables
            String FileConfirmed = "Y";
            String mlFileName = "";
            String Reason = "";
            String mySql;
            Boolean retVal = true;


            // See if the file is locked
            try
            {
                FileInfo fileInfo = new System.IO.FileInfo(filePath + "\\" + fileName);
                if (fileInfo.IsReadOnly)
                    return (false);
            }
            catch
            {
                return (false);
            }
            String[] MLData = File.ReadAllLines(filePath + "\\" + fileName);
            if (MLData.Length > 0)
            {
                // First Field is the Filename to check but has 34 extra characters "XAAM_UPLOAD_2012.03.22.csv.asc.12.03.22_17.12.12.03.22_02.12"
                String[] MLDataFields = MLData[0].Split('\t');
                if (MLDataFields.Length > 0)
                    if (MLDataFields[0].IndexOf("asc") > 0)
                        mlFileName = MLDataFields[0].Substring(0, MLDataFields[0].Length - 34);
                    else
                        mlFileName = MLDataFields[0];
            }

            if (fileName.StartsWith("error.txt"))
            {
                FileConfirmed = "N";
                // Get reason
                if (MLData.Length > 1)
                {
                    // First Field is the Filename to check but has 34 extra characters "XAAM_UPLOAD_2012.03.22.csv.asc.12.03.22_17.12.12.03.22_02.12"
                    String[] MLDataFields = MLData[1].Split('\t');
                    if (MLDataFields.Length > 1)
                    {
                        Reason = MLDataFields[1];
                    }
                }
            }

            if (mlFileName.Length > 0)
            {
                mySql = "Update ML_Out " +
                        "Set    FileConfirmed = '" + FileConfirmed + "' ";

                if (Reason.Length > 0)
                    mySql = mySql + ",    Reason = '" + Reason + "' ";

                mySql = mySql +
                       "Where  mlFileName = '" + mlFileName + "' ";
                SystemLibrary.SQLExecute(mySql);

                // Archive the File
                // -- See if the Archive Folder exists & if not create one.
                if (!System.IO.File.Exists(filePath + @"\Archive"))
                    System.IO.Directory.CreateDirectory(filePath + @"\Archive");

                // -- Move the file to Archive directory
                //      Replace if previously archived
                if (System.IO.File.Exists(filePath + @"\Archive\" + fileName))
                    System.IO.File.Replace(filePath + @"\" + fileName, filePath + @"\Archive\" + fileName, filePath + @"\Archive\" + fileName + "_" + SystemLibrary.f_Now().ToString("yyyyMMMdd_HHmmss"));
                else
                    System.IO.File.Move(filePath + @"\" + fileName, filePath + @"\Archive\" + fileName);

            }

            return (retVal);
        } //MLPrimeSaveDataConfirm()


        public static Boolean MLPrimeSaveData(String filePath, String fileName)
        {
            //
            // Rules: Expect fileName of the form <FUND>_E236_yyyy.mm.dd.csv
            //

            // Local Variables
            Boolean retVal = true;
            Int32 myRows;


            // Header
            DataTable dt_Header = SystemLibrary.SQLSelectToDataTable("Select * from ML_File_header Where MLFileName='" + fileName + "' ");
            if (dt_Header.Rows.Count < 1)
            {
                retVal = false;

                // See if the file is locked
                try
                {
                    FileInfo fileInfo = new System.IO.FileInfo(filePath + "\\" + fileName);
                    if (fileInfo.IsReadOnly)
                        return (false);
                }
                catch
                {
                    return (false);
                }
                // Only deal with new data
                DataRow dr = dt_Header.NewRow();
                dt_Header.Rows.Add(dr);
                // ToDo (5) 11-Mar-2011 - Enable this "try" in EMSXSaveData() - once testing is completed.
                //try
                {
                    // Save the Header data
                    // Using Database rather than local users dates.
                    dt_Header.Rows[0]["MLFileName"] = fileName;
                    dt_Header.Rows[0]["MLFileDate"] = SystemLibrary.GetCreationTime(filePath + "\\" + fileName);
                    dt_Header.Rows[0]["UploadDate"] = SystemLibrary.f_Now();
                    myRows = SQLBulkUpdate(dt_Header, "", "ML_File_header");
                    if (myRows == 1)
                    {
                        // Now load the detail.
                        int Pos1 = fileName.IndexOf('_');
                        int Pos2 = -1;
                        if (Pos1 > 0)
                            Pos2 = fileName.IndexOf('_', Pos1 + 1);
                        if (Pos1 < 0 || Pos2 < 0)
                            return (false);
                        String TableName = "ML_" + fileName.Substring(Pos1 + 1, Pos2 - Pos1 - 1);
                        DataTable dt_Detail = SystemLibrary.SQLSelectToDataTable("Select * from " + TableName + " where 1=2");
                        String[] MLData = File.ReadAllLines(filePath + "\\" + fileName);
                        if (MLData.Length > 0)
                        {
                            String[] Header = new String[1];
                            // A Special Case for files that have no header record (eg. E238T has a value of "null")
                            if (MLData[0].Trim().ToLower() == "null" || TableName == "ML_E238T")
                            {
                                //Header = new String[MLData[1].Split(',').Length];
                                // cfr 20111004 if (MLData.Length > 1)
                                {
                                    if (TableName == "ML_E238T")
                                        Header = new String[dt_Detail.Columns["Pref_Underlying_Intl"].Ordinal];
                                    else
                                        Header = new String[dt_Detail.Columns.Count];
                                    for (Int32 j = 0; j < dt_Detail.Columns.Count; j++)
                                    {
                                        if (j < Header.Length)
                                            Header[j] = dt_Detail.Columns[j + 1].ColumnName;
                                    }
                                }
                            }
                            else
                            {
                                Header = MLData[0].Split(',');
                            }
                            for (Int32 j = 0; j < Header.Length; j++)
                            {
                                // In the Header, replace spaces with _
                                // Remove "(debit/credit)" from the the Header columns
                                Header[j] = Header[j].Replace("(debit/credit)", String.Empty).Trim().Replace(" ", "_").Replace("#", "");
                                switch (Header[j])
                                {
                                    case "Preferred_Underlying_US":
                                        Header[j] = "Pref_Underlying_US";
                                        break;
                                    case "Preferred_Under_ID_INTL":
                                        Header[j] = "Pref_Underlying_Intl";
                                        break;
                                    case "Activity_Posting_Date":
                                        Header[j] = "Post_Date";
                                        break;
                                    case "Net_Amount":
                                        Header[j] = "Amount";
                                        break;
                                    case "Commission_Amount":
                                        Header[j] = "Commision_Amount";
                                        break;
                                }
                            }
                            for (Int32 i = 1; i < MLData.Length; i++)
                            {
                                DataRow dr_D = dt_Detail.NewRow();
                                dr_D["MLFileName"] = fileName;
                                String[] Detail = MLData[i].Split(',');
                                for (Int32 j = 0; j < Header.Length; j++) // NB: Some of the ML files have trailing
                                {
                                    if (dt_Detail.Columns.Contains(Header[j]))
                                    {
                                        if (dt_Detail.Columns[Header[j]].DataType.Name == "DateTime")
                                        {
                                            // Inbound format "mm/dd/yyyy" or "19/04/2011 15:40:34"
                                            String[] mySplit = Detail[j].Split('/');
                                            if (mySplit.Length == 3)
                                            {
                                                int myDay = Convert.ToInt32(mySplit[1]);
                                                int myMonth = Convert.ToInt32(mySplit[0]);
                                                int myYear = 0;
                                                int myHour = 0;
                                                int myMin = 0;
                                                int mySec = 0;

                                                // See if contains the time
                                                if (mySplit[2].IndexOf(':') < 0)
                                                {
                                                    myYear = Convert.ToInt32(mySplit[2]);
                                                }
                                                else
                                                {
                                                    String[] mySplitYearTime = mySplit[2].Split(' ');
                                                    myYear = Convert.ToInt32(mySplitYearTime[0]);
                                                    String[] mySplitTime = mySplitYearTime[1].Split(':');
                                                    myHour = Convert.ToInt32(mySplitTime[0]);
                                                    myMin = Convert.ToInt32(mySplitTime[1]);
                                                    mySec = Convert.ToInt32(mySplitTime[2]);
                                                }
                                                if (!(myDay == 0 || myMonth == 0 || myYear == 0))
                                                    dr_D[Header[j]] = new DateTime(myYear, myMonth, myDay, myHour, myMin, mySec);
                                            }
                                        }
                                        else
                                        {
                                            if (Header[j].StartsWith("Fee_Type_") && Detail[j] == "N/A")
                                                dr_D[Header[j]] = "";
                                            else
                                            {
                                                if (Detail[j] == "" && dt_Detail.Columns[Header[j]].DataType.Name == "Decimal")
                                                    dr_D[Header[j]] = 0;
                                                else if (Detail[j] == "N/A")
                                                    dr_D[Header[j]] = 0;
                                                else if (dt_Detail.Columns[Header[j]].DataType.Name == "Decimal")
                                                {
                                                    Decimal myDec = 0;
                                                    if (Decimal.TryParse(Detail[j], System.Globalization.NumberStyles.Any, null, out myDec))
                                                        dr_D[Header[j]] = myDec;
                                                }
                                                else
                                                    dr_D[Header[j]] = Detail[j];
                                            }
                                        }
                                    }
                                }
                                dt_Detail.Rows.Add(dr_D);
                            }
                            myRows = SQLBulkUpdate(dt_Detail, "", TableName);
                            // Process the Dtat & Remove the temporary file
                            if (myRows == MLData.Length - 1)
                            {
                                // Archive the File
                                // -- See if the Archive Folder exists & if not create one.
                                if (!System.IO.File.Exists(filePath + @"\Archive"))
                                    System.IO.Directory.CreateDirectory(filePath + @"\Archive");

                                // -- Move the file to Archive directory
                                try
                                {
                                    // See if File Already exists
                                    if (!System.IO.File.Exists(filePath + @"\Archive\" + fileName))
                                        System.IO.File.Move(filePath + @"\" + fileName, filePath + @"\Archive\" + fileName);
                                    else
                                    {
                                        System.IO.File.Move(filePath + @"\" + fileName, filePath + @"\Archive\" + fileName +
                                            "_" + System.IO.File.GetLastAccessTime(filePath + @"\" + fileName).ToString("HHmmss"));
                                    }
                                }
                                catch { }

                                // Set LastUpdate
                                SystemLibrary.SQLExecute("Exec sp_ML_Process_File '" + fileName + "', '" + TableName + "' ");
                            }
                            retVal = true;
                        }
                        else
                            retVal = true; // Empty file, but OK.
                    }
                }
                /*
                catch (Exception e)
                {
                    SystemLibrary.DebugLine("MLPrimeSaveData(" + fileName + "):" + e.Message);
                    retVal = false;
                }
                */
            }
            else
            {
                // Archive the File
                // -- See if the Archive Folder exists & if not create one.
                if (!System.IO.File.Exists(filePath + @"\Archive"))
                    System.IO.Directory.CreateDirectory(filePath + @"\Archive");

                // -- Move the file to Archive directory
                //      Replace if previously archived
                if (System.IO.File.Exists(filePath + @"\Archive\" + fileName))
                    System.IO.File.Replace(filePath + @"\" + fileName, filePath + @"\Archive\" + fileName, filePath + @"\Archive\" + fileName + "_" + SystemLibrary.f_Now().ToString("yyyyMMMdd_HHmmss"));
                else
                    System.IO.File.Move(filePath + @"\" + fileName, filePath + @"\Archive\" + fileName);
            }

            return (retVal);


        } //MLPrimeSaveData()

        #endregion //ML Prime Files

        #region SCOTIA Prime Files


        public static String[] SplitCSV(string input)
        {
            Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);
            List<string> result = new List<string>();

            foreach (Match match in csvSplit.Matches(input))
                result.Add(match.Value.TrimStart(',').TrimStart('"').TrimEnd('"'));

            String[] myStr = result.ToArray();

            return (myStr);
        } // SplitCSV()


        public static int SCOTIAPrimeGetFiles(ref Boolean isConfigured)
        {
            // 
            // Procedure: SCOTIAPrimeGetFiles
            //
            // Purpose: Get a list of SCOTIA Prime files to Upload to the database
            //
            // Written: Colin Ritchie 17-Jun-2011
            //
            // Modified:
            //

            // Local Variables
            String myFileName;
            int TotFiles = 0;

            isConfigured = true;

            try
            {
                if (SCOTIAPrimeFilePath == null)
                    SCOTIAPrimeFilePath = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('SCOTIAPrime_Path')");

                if (SCOTIAPrimeFilePath.Length < 1)
                {
                    isConfigured = false;
                    return (0);
                }

                if (SCOTIAPrime_FilePart == null)
                    SCOTIAPrime_FilePart = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('SCOTIAPrime_FilePart')");

                if (SCOTIAPrime_FilePart.Length < 1)
                {
                    isConfigured = false;
                    return (0);
                }

                if (!Directory.Exists(SCOTIAPrimeFilePath))
                {
                    isConfigured = false;
                    return (0);
                }

                DirectoryInfo di = new DirectoryInfo(SCOTIAPrimeFilePath);
                FileInfo[] rgFiles = di.GetFiles("*_*" + SCOTIAPrime_FilePart + "*_*.csv");

                TotFiles = rgFiles.Length;
                for (int i = 0; i < rgFiles.Length; i++)
                {
                    myFileName = rgFiles[i].Name;
                    SCOTIAPrimeSaveData(SCOTIAPrimeFilePath, myFileName);
                }

                /*
                // Check for confirm and error files
                rgFiles = di.GetFiles("*.txt.*");

                TotFiles = TotFiles + rgFiles.Length;
                for (int i = 0; i < rgFiles.Length; i++)
                {
                    myFileName = rgFiles[i].Name;
                    SCOTIAPrimeSaveDataConfirm(SCOTIAPrimeFilePath, myFileName);
                }
                */
            }
            catch (Exception e)
            {
                SystemLibrary.DebugLine("SCOTIAPrimeGetFiles (" + SystemLibrary.ToString(SCOTIAPrimeFilePath) + ") " + e.Message);
            }
            // Return the number of files 
            return (TotFiles);

        } //SCOTIAPrimeGetFiles()

        public static Boolean SCOTIAPrimeSaveData(String filePath, String fileName)
        {
            //
            // Rules: Expect fileName of the form <FUND>_E236_yyyy.mm.dd.csv
            //

            // Local Variables
            Boolean retVal = true;
            Int32 myRows;


            // Header
            DataTable dt_Header = SystemLibrary.SQLSelectToDataTable("Select * from SCOTIA_File_header Where SCOTIAFileName='" + fileName + "' ");
            if (dt_Header.Rows.Count < 1)
            {
                retVal = false;

                // See if the file is locked
                try
                {
                    FileInfo fileInfo = new System.IO.FileInfo(filePath + "\\" + fileName);
                    if (fileInfo.IsReadOnly)
                        return (false);
                }
                catch
                {
                    return (false);
                }
                // Only deal with new data
                DataRow dr = dt_Header.NewRow();
                dt_Header.Rows.Add(dr);
                // ToDo (5) 11-Mar-2011 - Enable this "try" in EMSXSaveData() - once testing is completed.
                //try
                {
                    // Save the Header data
                    // Using Database rather than local users dates.
                    dt_Header.Rows[0]["SCOTIAFileName"] = fileName;
                    dt_Header.Rows[0]["SCOTIAFileDate"] = SystemLibrary.GetCreationTime(filePath + "\\" + fileName);
                    dt_Header.Rows[0]["UploadDate"] = SystemLibrary.f_Now();
                    myRows = SQLBulkUpdate(dt_Header, "", "SCOTIA_File_header");
                    if (myRows == 1)
                    {
                        // Now load the detail.
                        int Pos1 = fileName.IndexOf('_');
                        if (Pos1 < 0)
                            return (false);
                        String TableName = "SCOTIA_" + fileName.Substring(0, Pos1);
                        DateTime fileDate = YYYYMMDD_ToDate(fileName.Substring(fileName.Length - 12, 8));
                        DataTable dt_Detail = SystemLibrary.SQLSelectToDataTable("Select * from " + TableName + " where 1=2");
                        String[] SCOTIAData = File.ReadAllLines(filePath + "\\" + fileName);
                        if (SCOTIAData.Length > 0)
                        {
                            String[] Header = new String[1];
                            Header = SCOTIAData[0].Split(',');
                            int HeaderCount = Header.Length;

                            for (Int32 j = 0; j < HeaderCount; j++)
                            {
                                // In the Header, replace spaces with _
                                // Remove " from the Start/End of Header columns
                                Header[j] = TrimQuotes(Header[j]);
                                Header[j] = Header[j].Replace("(", String.Empty).Replace(")", String.Empty).Replace("/", String.Empty).Trim().Replace(" ", "_").Replace("#", "");
                                if (TableName=="SCOTIA_ACCTHIST" && Header[j]=="Desc")
                                    Header[j] = "Description";
                                else if (TableName=="SCOTIA_ACCTHIST" && Header[j]=="ID" && Header[j-1]=="ID")
                                    Header[j] = "ID1";
                                else if ((TableName == "SCOTIA_POSITIONS" || TableName == "SCOTIA_CASHSTMNT") && Header[j] == "Security")
                                    Header[j] = "Security_Name";
                                else if (TableName == "SCOTIA_POSITIONS" && Header[j] == "Date")
                                    Header[j] = "EffectiveDate";
                            }
                            for (Int32 i = 1; i < SCOTIAData.Length; i++)
                            {
                                DataRow dr_D = dt_Detail.NewRow();
                                dr_D["SCOTIAFileName"] = fileName;
                                if (TableName == "SCOTIA_CASHBALANCE")
                                    dr_D["FileDate"] = fileDate;
                                if ( TableName == "SCOTIA_POSITIONS")
                                    dr_D["Balance_date"] = fileDate;
                                //String[] Detail = SCOTIAData[i].Split(',');
                                String[] Detail = SplitCSV(SCOTIAData[i]);
                               
                                for (Int32 j = 0; j < Header.Length; j++) // NB: Some of the SCOTIA files have trailing
                                {
                                    if (dt_Detail.Columns.Contains(Header[j]) && Detail.Length == HeaderCount)
                                    {
                                        if (dt_Detail.Columns[Header[j]].DataType.Name == "DateTime")
                                        {
                                            DateTime inDate;
                                            if (DateTime.TryParse(Detail[j], out inDate))
                                                dr_D[Header[j]] = inDate;
                                        }
                                        else
                                        {
                                            if (Detail[j] == "" && dt_Detail.Columns[Header[j]].DataType.Name == "Decimal")
                                                dr_D[Header[j]] = 0;
                                            else if (Detail[j] == "N/A")
                                                dr_D[Header[j]] = 0;
                                            else if (dt_Detail.Columns[Header[j]].DataType.Name == "Decimal")
                                            {
                                                Decimal myDec = 0;
                                                if (Decimal.TryParse(Detail[j], System.Globalization.NumberStyles.Any, null, out myDec))
                                                    dr_D[Header[j]] = myDec;
                                            }
                                            else
                                                dr_D[Header[j]] = Detail[j];
                                        }
                                    }
                                }
                                if (Detail.Length == HeaderCount)
                                    dt_Detail.Rows.Add(dr_D);
                            }
                            myRows = SQLBulkUpdate(dt_Detail, "", TableName);

                            // Process the Dtat & Remove the temporary file
                            if (myRows == SCOTIAData.Length - 2)
                            {
                                // Archive the File
                                // -- See if the Archive Folder exists & if not create one.
                                if (!System.IO.File.Exists(filePath + @"\Archive"))
                                    System.IO.Directory.CreateDirectory(filePath + @"\Archive");

                                // -- Move the file to Archive directory
                                try
                                {
                                    // See if File Already exists
                                    if (!System.IO.File.Exists(filePath + @"\Archive\" + fileName))
                                        System.IO.File.Move(filePath + @"\" + fileName, filePath + @"\Archive\" + fileName);
                                    else
                                    {
                                        System.IO.File.Move(filePath + @"\" + fileName, filePath + @"\Archive\" + fileName +
                                            "_" + System.IO.File.GetLastAccessTime(filePath + @"\" + fileName).ToString("HHmmss"));
                                    }
                                }
                                catch { }

                                // Set LastUpdate
                                SystemLibrary.SQLExecute("Exec sp_SCOTIA_Process_File '" + fileName + "', '" + TableName + "' ");
                            }
                            retVal = true;
                        }
                        else
                            retVal = true; // Empty file, but OK.
                    }
                }
                /*
                catch (Exception e)
                {
                    SystemLibrary.DebugLine("SCOTIAPrimeSaveData(" + fileName + "):" + e.Message);
                    retVal = false;
                }
                */
            }
            else
            {
                // Archive the File
                // -- See if the Archive Folder exists & if not create one.
                if (!System.IO.File.Exists(filePath + @"\Archive"))
                    System.IO.Directory.CreateDirectory(filePath + @"\Archive");

                // -- Move the file to Archive directory
                //      Replace if previously archived
                if (System.IO.File.Exists(filePath + @"\Archive\" + fileName))
                    System.IO.File.Replace(filePath + @"\" + fileName, filePath + @"\Archive\" + fileName, filePath + @"\Archive\" + fileName + "_" + SystemLibrary.f_Now().ToString("yyyyMMMdd_HHmmss"));
                else
                    System.IO.File.Move(filePath + @"\" + fileName, filePath + @"\Archive\" + fileName);
            }

            return (retVal);


        } //SCOTIAPrimeSaveData()

        #endregion //SCOTIA Prime Files

        // EMSX Calls
        #region EMXS Calls

        public static Boolean EMSXSaveData(String filePath, String fileName)
        {
            // Local Variables
            Boolean retVal = false;
            Int32 myRows;


            // Header
            DataTable dt_Header = SystemLibrary.SQLSelectToDataTable("Select * from EMS_Fills_Header Where FillsFileName='" + fileName + "' ");
            if (dt_Header.Rows.Count < 1)
            {
                // Only deal with new data
                DataRow dr = dt_Header.NewRow();
                dt_Header.Rows.Add(dr);
                // ToDo (5) 11-Mar-2011 - Enable this "try" in EMSXSaveData() - once testing is completed.
                // TODO (2) Look at Data Field ExecType - Is there another type than "FILL"? ie. Cancel, etc al
                //try
                {
                    // Save the Header data
                    // Using Database rather than local users dates.
                    dt_Header.Rows[0]["FillsFileName"] = fileName;
                    dt_Header.Rows[0]["FillsFileDate"] = SystemLibrary.GetCreationTime(filePath + "\\" + fileName);
                    dt_Header.Rows[0]["UploadDate"] = SystemLibrary.f_Now();
                    myRows = SQLBulkUpdate(dt_Header, "", "EMS_Fills_Header");
                    if (myRows == 1)
                    {
                        // Now load the detail.
                        DataTable dt_Detail = SystemLibrary.SQLSelectToDataTable("Select * from EMS_Fills_Detail where 1=2");
                        String[] EMSXData = File.ReadAllLines(filePath + "\\" + fileName);
                        if (EMSXData.Length > 0)
                        {
                            String[] Header = EMSXData[0].Replace(" ", String.Empty).Split(',');
                            for (Int32 i = 1; i < EMSXData.Length; i++)
                            {
                                DataRow dr_D = dt_Detail.NewRow();
                                dr_D["FillsFileName"] = fileName;
                                String[] Detail = EMSXData[i].Split(',');
                                for (Int32 j = 0; j < Detail.Length; j++)
                                {
                                    if (dt_Detail.Columns[Header[j]].DataType.Name == "DateTime")
                                    {
                                        // Inbound format "22/03/2011" or "19/04/2011 15:40:34"
                                        String[] mySplit = Detail[j].Split('/');
                                        if (mySplit.Length == 3)
                                        {
                                            int myDay = Convert.ToInt32(mySplit[0]);
                                            int myMonth = Convert.ToInt32(mySplit[1]);
                                            int myYear = 0;
                                            int myHour = 0;
                                            int myMin = 0;
                                            int mySec = 0;

                                            // See if contains the time
                                            if (mySplit[2].IndexOf(':') < 0)
                                            {
                                                myYear = Convert.ToInt32(mySplit[2]);
                                            }
                                            else
                                            {
                                                String[] mySplitYearTime = mySplit[2].Split(' ');
                                                myYear = Convert.ToInt32(mySplitYearTime[0]);
                                                String[] mySplitTime = mySplitYearTime[1].Split(':');
                                                myHour = Convert.ToInt32(mySplitTime[0]);
                                                myMin = Convert.ToInt32(mySplitTime[1]);
                                                mySec = Convert.ToInt32(mySplitTime[2]);
                                            }
                                            if (!(myDay == 0 || myMonth == 0 || myYear == 0))
                                                dr_D[Header[j]] = new DateTime(myYear, myMonth, myDay, myHour, myMin, mySec);
                                        }
                                    }
                                    else
                                        dr_D[Header[j]] = Detail[j];

                                }
                                dt_Detail.Rows.Add(dr_D);
                            }
                            myRows = SQLBulkUpdate(dt_Detail, "", "EMS_Fills_Detail");
                            // Process the Dtat & Remove the temporary file
                            if (myRows == EMSXData.Length - 1)
                            {
                                // ToDo (5) 11-Mar-2011 - Enable this Delete of Temp EMS File - once testing is completed.
                                //File.Delete(filePath + "\\" + fileName);

                                // Set LastUpdate
                                SystemLibrary.SQLExecute("Exec sp_EMS_Process_Export_File '" + fileName + "' ");
                            }

                            // Set LastUpdate
                            SystemLibrary.SQLExecute("Update FTP_Parameters Set LastUpdate='" + SystemLibrary.f_Now().ToString("dd-MMM-yyyy HH:mm:ss") + "'");
                            retVal = true;
                        }
                        else
                            retVal = true; // Empty file, but OK.
                    }
                }
                /*
                catch (Exception e)
                {
                    SystemLibrary.DebugLine("EMSXSaveData(" + fileName + "):" + e.Message);
                    retVal = false;
                }
                */
            }
            else
            {
                // Already processed the file
                retVal = true;
            }
            return (retVal);

        } //EMSXSaveData()

        #endregion //EMSX Calls

        // BLoomberg DDE Send
        #region Bloomberg DDE Send

        public static void BBGShowMenu(int FundID, int PortfolioID, String Ticker, String Portfolio, String Relative, Int32 inX, Int32 inY)
        {
            // Screen Location
            Point myLocation = new Point(inX, inY);

            BBGMenu.Ticker = Ticker;
            BBGMenu.Relative = Relative;
            BBGMenu.Portfolio = Portfolio;
            BBGMenu.FundID = FundID;
            BBGMenu.PortfolioID = PortfolioID;

            // Adjust Text if Caption has KeyWords in it.
            // TODO (5) - I need to read down the nesting. This is just the top level menu.
            SystemLibrary.DebugLine("Menu: START");
            for (int i = 0; i < BBGMenu.myMenu.Items.Count; i++)
            {
                try
                {
                    SystemLibrary.DebugLine("Menu:" + i.ToString()+","+BBGMenu.myMenu.Items[i].Text);
                    BBGMenu.myMenu.Items[i].Visible = true;
                    if (BBGMenu.myMenu.Items[i].Tag != null)
                    {
                        Boolean FoundApliesTo = false;
                        BloombergMenuTag myTag = (BloombergMenuTag)BBGMenu.myMenu.Items[i].Tag;
                        String Caption = myTag.Caption.Replace("[Ticker]", BBGMenu.Ticker);
                        Caption = Caption.Replace("[Relative]", BBGMenu.Relative);
                        Caption = Caption.Replace("[Portfolio]", BBGMenu.Portfolio);
                        BBGMenu.myMenu.Items[i].Text = Caption;
                       
                        if (myTag.AppliesTo.Length > 0)
                        {
                            String[] AppliesToArray = myTag.AppliesTo.Split(@",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            foreach (String AppliesTo in AppliesToArray)
                            {
                                if (BBGMenu.Ticker.EndsWith(AppliesTo))
                                    FoundApliesTo = true;
                            }
                            if (!FoundApliesTo)
                            {
                                BBGMenu.myMenu.Items[i].Visible = false;
                                // If the line above is a seperator, then Hide this also
                                if (i > 0)
                                    if (BBGMenu.myMenu.Items[i - 1].Text == "")
                                        BBGMenu.myMenu.Items[i - 1].Visible = false;
                            }
                        }
                    }
                    if (BBGMenu.myMenu.Items[i].GetType().BaseType.Name == "ToolStripDropDownItem")
                    {
                        foreach (ToolStripItem Colin in (BBGMenu.myMenu.Items[i] as ToolStripDropDownItem).DropDownItems)
                        {
                            if (Colin.Tag != null)
                            {
                                BloombergMenuTag myTag1 = (BloombergMenuTag)Colin.Tag;
                                String SubItemCaption = myTag1.Caption.Replace("[Ticker]", BBGMenu.Ticker);
                                SubItemCaption = SubItemCaption.Replace("[Relative]", BBGMenu.Relative);
                                SubItemCaption = SubItemCaption.Replace("[Portfolio]", BBGMenu.Portfolio);
                                Colin.Text = SubItemCaption;
                            }
                            //Colin.AutoSize=false; 
                            //Colin.Height=200; 
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    SystemLibrary.DebugLine(e.Message);
                }

            }
            SystemLibrary.DebugLine("Menu: END");
            // Loop down the menu and set the text where needed & Hide Data where needed.
            BBGMenu.myMenu.Show(myLocation);
        } //BBGShowMenu()

        public static void BBGBloombergCommand(int myTerminal, String myCommand)
        {
            //
            // Notes: Seperated from BBGBloombergCommandMenuItem_Click(), so I can use from say a Button.
            //

            // Set Terminal Format ie. "<blp-1><blp-1>" - as per Bloomberg Buttons macro record.
            String strTerminal = "<blp-" + myTerminal.ToString("0").Trim() + ">";
            strTerminal = strTerminal + strTerminal;

            // Replace items in command with real data
            myCommand = myCommand.Replace("[Ticker]", BBGGetTickerDDEFormat(BBGMenu.Ticker));
            myCommand = myCommand.Replace("[Relative]", BBGGetTickerDDEFormat(BBGMenu.Relative));
            myCommand = myCommand.Replace("[Portfolio]", BBGGetTickerDDEFormat(BBGMenu.Portfolio));

            try
            {
                if (BloombergClient == null)
                {
                    // First time, make the connection
                    BloombergClient = new DdeClient("winblp", "bbk");
                    BloombergClient.Connect();
                }
                
                // Asynchronous Execute Operation
                BloombergClient.BeginExecute(strTerminal + myCommand, null, BloombergClient);
                //SystemLibrary.DebugLine("BBGBloombergCommand: "+strTerminal + myCommand+", "+ir.IsCompleted.ToString());
            }
            catch { }
        } //BBGBloombergCommand()

        public static void BBGBloombergCommandMenuItem_Click(object sender, MouseEventArgs e)
        {
            //
            // Purpose: A generic menu click event for the Bloomberg Right-Click Menu
            //

            // Local Variables
            ToolStripMenuItem ts_From = (ToolStripMenuItem)sender;
            BloombergMenuTag myTag = (BloombergMenuTag)ts_From.Tag;
            int myTerminal;

            switch (e.Button.ToString())
            {
                case "Right":
                    myTerminal = 0; 
                    break;
                case "Left":
                    myTerminal = 1;
                    break;
                default:
                    myTerminal = 2;
                    break;
            }
            // Instruction in the Menu Tag
            BBGBloombergCommand(myTerminal, myTag.Command);

        } //BBGBloombergCommandMenuItem_Click()

        public static void BBGBloombergSystemMenuItem_Click(object sender, MouseEventArgs e)
        {
            //
            // Purpose: A generic menu click event for the Bloomberg Right-Click Menu
            //

            // Local Variables
            ToolStripMenuItem ts_From = (ToolStripMenuItem)sender;
            BloombergMenuTag myTag = (BloombergMenuTag)ts_From.Tag;
            Boolean isOpen = false;


            // Instruction in the Menu Tag
            switch (myTag.Command)
            {
                case "ReportTrade":
                    ReportTrade rt = new ReportTrade();
                    // See if already open
                    if (e.Button.ToString()=="Left")
                        foreach (Form frm in Application.OpenForms)
                            if (frm is ReportTrade)
                            {
                                isOpen = true;
                                rt.Dispose();
                                rt = (ReportTrade)frm;
                                break; // Leave loop
                            }
                    rt.FromParent(myTag.applyForm, BBGMenu.FundID, BBGMenu.PortfolioID, BBGMenu.Ticker, null, null, -1);
                    if (isOpen)
                    {
                        //rt.LoadTrades();
                        rt.WindowState = FormWindowState.Normal;
                        rt.BringToFront();
                    }
                    else
                        rt.Show(); // (myTag.applyForm);
                    break;
                case "ReportPosition":
                    ReportPosition rpos = new ReportPosition();
                    // See if already open
                    if (e.Button.ToString() == "Left")
                        foreach (Form frm in Application.OpenForms)
                            if (frm is ReportPosition)
                            {
                                isOpen = true;
                                rpos.Dispose();
                                rpos = (ReportPosition)frm;
                                break; // Leave loop
                            }
                    rpos.FromParent(myTag.applyForm, BBGMenu.FundID, BBGMenu.PortfolioID, BBGMenu.Ticker, null, null, -1);
                    if (isOpen)
                    {
                        //rpos.LoadPositions();
                        rpos.WindowState = FormWindowState.Normal;
                        rpos.BringToFront();
                    }
                    else
                        rpos.Show(); //(myTag.applyForm);
                    break;
                case "ReportProfit":
                    ReportProfit rp = new ReportProfit();
                    // See if already open
                    if (e.Button.ToString() == "Left")
                        foreach (Form frm in Application.OpenForms)
                            if (frm is ReportProfit)
                            {
                                isOpen = true;
                                rp.Dispose();
                                rp = (ReportProfit)frm;
                                break; // Leave loop
                            }

                    rp.FromParent(myTag.applyForm, BBGMenu.FundID, BBGMenu.PortfolioID, BBGMenu.Ticker, null, null, -1);
                    if (isOpen)
                    {
                        rp.LoadTrades();
                        rp.WindowState = FormWindowState.Normal;
                        rp.BringToFront();
                    }
                    else
                        rp.Show(); // (myTag.applyForm);
                    break;
                case "CommissionSummary":
                    CommissionSummary cs = new CommissionSummary();
                    // See if already open
                    if (e.Button.ToString() == "Left")
                        foreach (Form frm in Application.OpenForms)
                            if (frm is CommissionSummary)
                            {
                                isOpen = true;
                                cs.Dispose();
                                cs = (CommissionSummary)frm;
                                break; // Leave loop
                            }

                    cs.FromParent(myTag.applyForm, BBGMenu.FundID, null, null, -1);
                    if (isOpen)
                    {
                        cs.LoadCommissions();
                        cs.WindowState = FormWindowState.Normal;
                        cs.BringToFront();
                    }
                    else
                        cs.Show(); // (myTag.applyForm);
                    break;

            }

        } //BBGBloombergSystemMenuItem_Click()


        public static void BBGSetUpMenu(Form inForm)
        {
            // Load the ImageList object from Shell32.dll
            // TODO (99) - Change Bloomberg Menu ImageList to another source. (now Shell32.dll)
            int numIcons = 500;//if you want 10 icons for example
            IntPtr[] largeIcon = new IntPtr[numIcons];
            IntPtr[] smallIcon = new IntPtr[numIcons];
            int myRet = ExtractIconEx("shell32.dll", 0, largeIcon, smallIcon, numIcons);
            ToolStripMenuItem mySubMenu = new ToolStripMenuItem();
            int myNextLevel;
            int FaceID;

            //retrieve icon from array
            for (int i = 0; i < myRet; i++)
                BBGMenu.il_Menu.Images.Add(Icon.FromHandle(smallIcon[i]));

            // Loaded from the Database
            BBGMenu.myMenu.Items.Clear();
            BBGMenu.myMenu.ImageList = BBGMenu.il_Menu;
            // Add system menus
            mySubMenu = new ToolStripMenuItem("Reports");
            mySubMenu.ImageIndex = 80;
            //BBGMenu.myMenu.ImageList = BBGMenu.il_Menu;
            BBGMenu.myMenu.Items.Add(mySubMenu);
            {
                ToolStripItem ts = mySubMenu.DropDownItems.Add("Trade List");
                ts.Tag = new BloombergMenuTag(inForm, "Trade List", "ReportTrade", "");
                ts.MouseUp += new MouseEventHandler(BBGBloombergSystemMenuItem_Click);
                ts.Owner.ImageList = BBGMenu.il_Menu; // Not sure why I needed this, but did not inherit from parent?
                ts.ImageIndex = 171;

                ts = mySubMenu.DropDownItems.Add("Position History");
                ts.Tag = new BloombergMenuTag(inForm, "Position History", "ReportPosition", "");
                ts.MouseUp += new MouseEventHandler(BBGBloombergSystemMenuItem_Click);
                ts.Owner.ImageList = BBGMenu.il_Menu; // Not sure why I needed this, but did not inherit from parent?
                //ts.ImageIndex = 171;

                ts = mySubMenu.DropDownItems.Add("Profit & Loss");
                ts.Tag = new BloombergMenuTag(inForm, "Profit & Loss", "ReportProfit", "");
                ts.MouseUp += new MouseEventHandler(BBGBloombergSystemMenuItem_Click);
                ts.Owner.ImageList = BBGMenu.il_Menu; // Not sure why I needed this, but did not inherit from parent?
                ts.ImageIndex = 80;

                mySubMenu.DropDownItems.Add("-");

                ts = mySubMenu.DropDownItems.Add("Portfolio Commission");
                ts.Tag = new BloombergMenuTag(inForm, "Portfolio Commission", "CommissionSummary", "");
                ts.MouseUp += new MouseEventHandler(BBGBloombergSystemMenuItem_Click);
                ts.Owner.ImageList = BBGMenu.il_Menu; // Not sure why I needed this, but did not inherit from parent?
                //ts.ImageIndex = 80;
            }
            BBGMenu.myMenu.Items.Add("-");

            // Now the Bloomberg Menus
            BBGMenu.dt_BBG_Menu = SystemLibrary.SQLSelectToDataTable("Select Pos, menuLevel, Caption, Command, Divider, FaceID, AppliesTo from BBG_Menu Order by Pos");
            for (int i = 0; i < BBGMenu.dt_BBG_Menu.Rows.Count; i++)
            {
                // Add the menu Item
                FaceID = Convert.ToInt16(BBGMenu.dt_BBG_Menu.Rows[i]["FaceID"]);
                switch (BBGMenu.dt_BBG_Menu.Rows[i]["menuLevel"].ToString())
                {
                    case "2":
                        // Add a Divider if needed
                        if (YN_To_Bool(BBGMenu.dt_BBG_Menu.Rows[i]["Divider"].ToString()))
                            BBGMenu.myMenu.Items.Add("-");
                        if (i + 1 < BBGMenu.dt_BBG_Menu.Rows.Count)
                            myNextLevel = Convert.ToInt16(BBGMenu.dt_BBG_Menu.Rows[i + 1]["menuLevel"]);
                        else
                            myNextLevel = 2;
                        if (myNextLevel == 3)
                        {
                            mySubMenu = new ToolStripMenuItem(BBGMenu.dt_BBG_Menu.Rows[i]["Caption"].ToString());
                            if (FaceID != -1)
                                mySubMenu.ImageIndex = FaceID;
                            BBGMenu.myMenu.ImageList = BBGMenu.il_Menu;
                            BBGMenu.myMenu.Items.Add(mySubMenu);
                        }
                        else
                        {
                            ToolStripItem ts = BBGMenu.myMenu.Items.Add(BBGMenu.dt_BBG_Menu.Rows[i]["Caption"].ToString());
                            ts.Tag = new BloombergMenuTag(inForm, BBGMenu.dt_BBG_Menu.Rows[i]["Caption"].ToString(), BBGMenu.dt_BBG_Menu.Rows[i]["Command"].ToString(), BBGMenu.dt_BBG_Menu.Rows[i]["AppliesTo"].ToString()); ;
                            if (FaceID != -1)
                            {
                                ts.ImageIndex = FaceID;
                                //ts.Owner.ImageList = BBGMenu.il_Menu; // Not sure why I needed ths
                            }
                            ts.MouseUp += new MouseEventHandler(BBGBloombergCommandMenuItem_Click);
                        }
                        break;
                    case "3":
                        try
                        {
                            // Add a Divider if needed
                            if (YN_To_Bool(BBGMenu.dt_BBG_Menu.Rows[i]["Divider"].ToString()))
                                mySubMenu.DropDownItems.Add("-");
                            ToolStripItem ts = mySubMenu.DropDownItems.Add(BBGMenu.dt_BBG_Menu.Rows[i]["Caption"].ToString());
                            ts.Tag = new BloombergMenuTag(inForm, BBGMenu.dt_BBG_Menu.Rows[i]["Caption"].ToString(), BBGMenu.dt_BBG_Menu.Rows[i]["Command"].ToString(), BBGMenu.dt_BBG_Menu.Rows[i]["AppliesTo"].ToString()); ;
                            if (FaceID != -1)
                            {
                                ts.ImageIndex = FaceID;
                                ts.Owner.ImageList = BBGMenu.il_Menu; // Not sure why I needed this, but did not inherit from parent?
                            }
                            ts.MouseUp += new MouseEventHandler(BBGBloombergCommandMenuItem_Click);
                        }
                        catch { }
                        break;
                }
            }
        } //BBGSetUpMenu()


        #endregion //Bloomberg DDE Send

        // Bloomberg Ticker Extraction
        #region Bloomberg Ticker Extraction

        public static String BBGRelativeTicker(String BBGTicker)
        {
            // TODO (5) - BBGRelativeTicker(). Can I do this from a database table, so say Copper stocks actually return the commondity rather than the Index
            //              OR - Download field REL_INDEX into Securities table

            // Local Variables
            String RetVal = "";

            // These are in the order of WEI, and I have only covered markets I think we will trade in first.
            if (BBGTicker.ToUpper().EndsWith("COMDTY")) RetVal = "DJUBS"; // Dow-Jones UBS Commodity Index
            else if (BBGTicker.ToUpper().EndsWith("CURNCY")) RetVal = "SPX"; // For want of something better
            else if (BBGTicker.ToUpper().EndsWith("INDEX")) RetVal = "INDU"; // Bloomberg seems to use Dow Jones for Beta??
            else if (BBGTicker.ToUpper().Contains(" US ")) RetVal = "SPX"; // S&P 500
            else if (BBGTicker.ToUpper().Contains(" UN ")) RetVal = "SPX";
            else if (BBGTicker.ToUpper().Contains(" UW ")) RetVal = "SPX";
            else if (BBGTicker.ToUpper().Contains(" LN ")) RetVal = "UKX"; // FTSE100
            else if (BBGTicker.ToUpper().Contains(" FP ")) RetVal = "CAC"; // CAC 40 Index
            else if (BBGTicker.ToUpper().Contains(" GR ")) RetVal = "DAX"; // DAX Index
            else if (BBGTicker.ToUpper().Contains(" GY ")) RetVal = "DAX";
            else if (BBGTicker.ToUpper().Contains(" SW ")) RetVal = "SMI"; // Swiss Market Index
            else if (BBGTicker.ToUpper().Contains(" VX ")) RetVal = "SMI";
            else if (BBGTicker.ToUpper().Contains(" JP ")) RetVal = "NKY"; // Nikkie 225
            else if (BBGTicker.ToUpper().Contains(" JT ")) RetVal = "NKY";
            else if (BBGTicker.ToUpper().Contains(" HK ")) RetVal = "HSI"; // Hang Seng Index
            else if (BBGTicker.ToUpper().Contains(" AU ")) RetVal = "AS51"; // S&P/ASX 200 Index
            else if (BBGTicker.ToUpper().Contains(" AT ")) RetVal = "AS51";
            else if (BBGTicker.ToUpper().Contains(" TT ")) RetVal = "TWSE"; // Taiwan Taiex Index
            else if (BBGTicker.ToUpper().Contains(" KP ")) RetVal = "KOSPI"; // KOSPI Index
            else if (BBGTicker.ToUpper().Contains(" KS ")) RetVal = "KOSPI";
            else if (BBGTicker.ToUpper().Contains(" SP ")) RetVal = "FSSTI";
            else if (BBGTicker.ToUpper().Contains(" NZ ")) RetVal = "NZSE"; // NZX 50 Index

            if (RetVal == "")
                RetVal = BBGTicker;
            else
                RetVal = RetVal + " Index";
            return (RetVal);

        } // BBGRelativeTicker()

        public static String BBGGetTicker(String sSecurity)
        {
            // Convert ticker to the correct case.
            String Ticker;
            String YellowKey;

            Ticker = BBGTicker(sSecurity);
            if (Ticker == "Invalid Ticker")
                return (sSecurity);
            YellowKey = BBGDataType(sSecurity);
            if (YellowKey == "Invalid Ticker")
                return (sSecurity);
            return (Ticker + " " + YellowKey);

        } // BBGGetTicker()

        public static String BBGGetTickerNormalised(String sSecurity)
        {
            // Deal with Multi-exchanges on Ticker
            String Ticker;
            String YellowKey;

            Ticker = BBGTicker(sSecurity);
            if (Ticker == "Invalid Ticker")
                return (sSecurity);
            YellowKey = BBGDataType(sSecurity);
            if (YellowKey == "Invalid Ticker")
                return (sSecurity);
            sSecurity = Ticker + " " + YellowKey;

            switch (YellowKey.ToUpper())
            {
                case "COMDTY":
                case "INDEX":
                    // Remove " COMB " from commodity/index - it means multiple exchanges.
                    sSecurity = sSecurity.Replace(" COMB ", " ");
                    break;
                case "EQUITY":
                    // TODO (5) Exchange substitution into the database.
                    sSecurity = sSecurity.Replace(" AT ", " AU ");
                    sSecurity = sSecurity.Replace(" UN ", " US ");
                    sSecurity = sSecurity.Replace(" UW ", " US ");
                    sSecurity = sSecurity.Replace(" GY ", " GR ");
                    sSecurity = sSecurity.Replace(" JT ", " JP ");
                    sSecurity = sSecurity.Replace(" KP ", " KS ");
                    break;
                default:
                break;
            }

            return (sSecurity);


        } // BBGGetTicker()


        public static String BBGGetTickerDDEFormat(String sSecurity)
        {
            return (BBGTicker(sSecurity) + " " + BBGYellowKey(sSecurity) + " ");

        } // BBGGetTickerDDEFormat()


        public static String BBGDataType(String TheSecurity)
        {
            // Local Variables
            String myString = TheSecurity.ToLower().Trim();

            // In order of most likely return
            if (myString.EndsWith(" equity")) return ("Equity");
            if (myString.EndsWith(" index")) return ("Index");
            if (myString.EndsWith(" comdty")) return ("Comdty");
            if (myString.EndsWith(" curncy")) return ("Curncy");
            if (myString.EndsWith(" govt")) return ("Govt");
            if (myString.EndsWith(" corp")) return ("Corp");
            if (myString.EndsWith(" mtge")) return ("Mtge");
            if (myString.EndsWith(" m-mkt")) return ("M-Mkt");
            if (myString.EndsWith(" muni")) return ("Muni");
            if (myString.EndsWith(" pfd")) return ("Pfd");

            return ("Invalid Ticker");

        } //BBGDataType()

        public static String BBGYellowKey(String TheSecurity)
        {
            // Local Variables
            String myString = TheSecurity.ToLower().Trim();

            if (myString.EndsWith(" equity")) return ("<Equity>");
            if (myString.EndsWith(" index")) return ("<Index>");
            if (myString.EndsWith(" comdty")) return ("<Cmdty>");
            if (myString.EndsWith(" curncy")) return ("<Crncy>");
            if (myString.EndsWith(" govt")) return ("<Govt>");
            if (myString.EndsWith(" corp")) return ("<Corp>");
            if (myString.EndsWith(" mtge")) return ("<Mtge>");
            if (myString.EndsWith(" m-mkt")) return ("<M-Mkt>");
            if (myString.EndsWith(" muni")) return ("<Muni>");
            if (myString.EndsWith(" pfd")) return ("<Pfd>");

            return ("Invalid Ticker");

        } //YellowKey()

        public static String BBGTicker(String TheSecurity)
        {
            // Local Variables
            Int32 pos;
            String TestTicker;

            pos = TheSecurity.Trim().ToUpper().IndexOf(BBGDataType(TheSecurity).ToUpper(), 0);
            if (pos > 1)
            {
                TestTicker = TheSecurity.Substring(0, pos - 1);
                // Strip out Multi spaces down to just 1 space.
                Char[] delimeters = new Char[] {' '};
                String[] mySplit = TestTicker.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
                TestTicker="";
                foreach (String myVal in mySplit)
                {
                    TestTicker = TestTicker + myVal + " ";
                }
                TestTicker = TestTicker.Trim();
            }
            else
            {
                return ("");
            }

            pos = TheSecurity.IndexOf("@", 0);
            if (pos > 1)
            {
                return (TestTicker.ToUpper().Substring(0, pos - 1));
            }
            else
            {
                return (TestTicker.ToUpper());
            }

        } //BBGTicker()

        public static void BBGOnPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            BBGOnPreviewKeyDown(sender, e.KeyCode.ToString());
        }

        public static void BBGOnPreviewKeyDown(object sender, String inKeyCode)
        {
            //
            // Purpose: Capture the Bloomberg Function Keys in a DataGridViewCell
            //

            // Local Variables
            TextBoxBase tb = (TextBoxBase)sender;
            String AddText = "";
            String NewText;
            int CurrentPos;


            // In order of most likely used keys
            switch (inKeyCode)
            {
                case "Return":
                    tb.Parent.Focus();
                    break;
                case "F8":  AddText = "Equity"; break;
                case "F10": AddText = "Index";  break;
                case "F9":  AddText = "Comdty"; break;
                case "F11": AddText = "Curncy"; break;
                case "F2":  AddText = "Govt";   break;
                case "F3":  AddText = "Corp";   break;
                case "F4":  AddText = "Mtge";   break;
                case "F5":  AddText = "M-Mkt";  break;
                case "F6":  AddText = "Muni";   break;
                case "F7":  AddText = "Pfd";    break;
                case "Back":
                case "Delete": // Shouldn't really allow this and doesnt remove the space, but...
                    // See if the previous text is on of these keywords and delete.
                    if (tb.SelectionLength == 0 && tb.SelectionStart ==tb.Text.Length)
                    {
                        // NB: The [Delete] will still happen & take away the [SPACE]
                        try
                        {
                            if (tb.SelectionStart > 6)
                            {
                                switch (tb.Text.Substring(tb.SelectionStart - 6, 6))
                                {
                                    case "Equity":
                                    case "Comdty":
                                    case "Curncy":
                                        tb.Text = tb.Text.Substring(0, tb.SelectionStart - 6);
                                        tb.SelectionStart = tb.Text.Length;
                                        break;
                                    case " Index":
                                    case " M-Mkt":
                                        tb.Text = tb.Text.Substring(0, tb.SelectionStart - 5);
                                        tb.SelectionStart = tb.Text.Length;
                                        break;
                                }
                            }
                            if (tb.SelectionStart > 4)
                            {
                                switch (tb.Text.Substring(tb.SelectionStart - 4, 4))
                                {
                                    case "Govt":
                                    case "Corp":
                                    case "Mtge":
                                    case "Muni":
                                    case "Pfd":
                                        tb.Text = tb.Text.Substring(0, tb.SelectionStart - 4);
                                        tb.SelectionStart = tb.Text.Length;
                                        break;
                                }
                            }
                        }
                        catch { }
                    }
                    break;
            }

            if (AddText.Length > 0)
            {
                // Put the Keystroke in the correct position in the text
                CurrentPos = tb.SelectionStart + tb.SelectionLength;
                if (CurrentPos == tb.Text.Length)
                {
                    // Remove trailing Blanks
                    tb.Text = tb.Text.TrimEnd(); 
                    CurrentPos = tb.Text.Length;
                }
                NewText = tb.Text.Substring(0, CurrentPos) + " " + AddText;
                if (CurrentPos != tb.Text.Length)
                    NewText = NewText+tb.Text.Substring(CurrentPos);
                tb.Text = NewText;
                tb.SelectionLength = 0;
                tb.SelectionStart = CurrentPos + 1 + AddText.Length;
            }

        } //BBGOnPreviewKeyDown()


        #endregion // Bloomberg Ticker Extraction


        // Any standalone File calls
        #region File Manipulation

        public static Boolean FileExists(String filePath)
        {
            return (File.Exists(filePath));

        } //FileExists()

        public static byte[] FileRead(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;

        } // FileRead()

        #endregion File Manipulation


        // InputBox Code
        #region InputBox code

        public static DialogResult InputBox(string title, string promptText, ref string value, InputBoxValidation validation, MessageBoxIcon ErrorIcon)
        {
            // Dynamically create an Input Box form for things like SaveAs button.
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            //form.ClientSize = new Size(Math.Max(300,label.Right+10), form.ClientSize.Height);
            textBox.Top = label.Top + label.Height + 3;
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), Math.Min(1000, form.ClientSize.Height + label.Height + textBox.Height + 10));
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
            if (validation != null) 
            {
              form.FormClosing += delegate(object sender, FormClosingEventArgs e) 
              {
                if (form.DialogResult == DialogResult.OK) 
                {
                  String errorText = validation(textBox.Text);
                  if (e.Cancel = (errorText != "")) 
                  {
                    MessageBox.Show(form, errorText, title, MessageBoxButtons.OK, ErrorIcon);
                    //MessageBox.Show(form, "hello", "This is the Title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    textBox.Focus();
                  }
                }
              };
            }
            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        public delegate string InputBoxValidation(string errorMessage);

        #endregion InputBox code

        #region MessageBox

        public static DialogResult iMessageBox(String promptText, String title, MessageBoxIcon Icon) //, MessageBoxButtons buttons
        {
            return (iMessageBox(promptText, title, Icon, true));
        } //iMessageBox()

        public static DialogResult iMessageBox(String promptText, String title, MessageBoxIcon Icon, Boolean isWaitForResult) //, MessageBoxButtons buttons
        {
            // Dynamically create an Input Box form for things like SaveAs button.
            Form form = new Form();
            Label label = new Label();
            //TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();
            DialogResult dialogResult = new DialogResult();
            
            form.Text = title;
            label.Text = promptText;
            label.BackColor = Color.White;
            //textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            //textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            //textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            if (isWaitForResult)
            {
                form.Controls.AddRange(new Control[] { label, buttonOk, buttonCancel });
                form.ClientSize = new Size(Math.Max(300, label.Right + 10), Math.Min(600, form.ClientSize.Height + label.Height + 10));
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonCancel;
                dialogResult = form.ShowDialog();
            }
            else
            {
                form.Controls.AddRange(new Control[] { label, buttonOk });
                form.ClientSize = new Size(Math.Max(300, label.Right + 10), Math.Min(600, form.ClientSize.Height + label.Height + 10));
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonOk;
                buttonOk.Click += new System.EventHandler(SystemLibrary.iMessageBox_buttonOk_Click);
                form.Show();
            }
            
            //value = textBox.Text;
            return dialogResult;
        } //iMessageBox()

        public static void iMessageBox_buttonOk_Click(object sender, EventArgs e)
        {
            Form frm = (Form)((Button)sender).Parent;
            frm.Close();
        } //iMessageBox_buttonOk_Click()

        #endregion MessageBox

        // HTML Builder - simple code
        #region HTML Builder code

        public static String HTMLStart()
        {
            //   "        <title>Trade Bookings</title>\r\n"+

            String myStr;
            myStr = "<html>\r\n" +
                    "    <head>\r\n" +
                   @"        <meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />" + "\r\n" +
                   @"   <style type=""text/css"">" + "\r\n" +
                    "      table.sample {\r\n" +
                    "          border-color: #600;\r\n" +
                    "          border-width: 0 0 1px 1px;\r\n" +
                    "          border-style: solid;\r\n" +
                    "         border-collapse: collapse;\r\n" +
                    "      }\r\n" +
                    "      table.sample th {\r\n" +
                    "          border-color: #600;\r\n" +
                    "          border-width: 1px 1px 0 0;\r\n" +
                    "          border-style: solid;\r\n" +
                    "          margin: 0;\r\n" +
                    "          padding: 4px;\r\n" +
                    "          background-color: Gainsboro;\r\n" +
                    "      }\r\n" +
                    "      table.sample td {\r\n" +
                    "          border-color: #600;\r\n" +
                    "          border-width: 1px 1px 0 0;\r\n" +
                    "          border-style: solid;\r\n" +
                    "          margin: 0;\r\n" +
                    "          padding: 4px;\r\n" +
                    "          background-color: #F0FFFF;\r\n" +
                    "      }\r\n" +
                    "   </style>\r\n" +
                    "</head>\r\n" +
                    "<body>\r\n";

            return (myStr);
        }

        public static String HTMLEnd()
        {
            return ("</body>\r\n</html>\r\n");
        }

        public static String HTMLTableStart()
        {
            return (@"<table class=""sample"""+"\r\n");
        }

        public static String HTMLTableEnd()
        {
            return ("</table>\r\n");
        }

        public static String HTMLHeaderStart()
        {
            return ("<tr>");
        }


        public static String HTMLHeaderEnd()
        {
            return ("</tr>\r\n");
        }


        public static String HTMLRowStart()
        {
            return ("<tr>");
        }


        public static String HTMLRowEnd()
        {
            return ("</tr>\r\n");
        }

        public static String HTMLHeaderField(String myField)
        {
            return ("<th>" + myField + "</th>\r\n");
        }


        public static String HTMLRowField(String myField, String myFontColour)
        {
            String myStr = "<td NOWRAP >";
            if (myFontColour.Length > 0)
                myStr = myStr + "<Font Color=" + myFontColour + ">";
            myStr = myStr + myField;
            if (myFontColour.Length > 0)
                myStr = myStr + "</Font>";
            myStr = myStr + "</td>\r\n";

            return (myStr);
        }

        public static String HTMLLine(String myInStr)
        {
            // Local Variables
            String myStr;
            Char[] mySeperatorRow = { '\r', '\n' };
            String[] myField = myInStr.Split(mySeperatorRow); //, StringSplitOptions.RemoveEmptyEntries);

            myStr = "<p>";

            for (int i = 0; i < myField.Length; i++)
                myStr = myStr + myField[i] + "<br />\r\n";

            myStr = myStr + "</p>\r\n";
            return (myStr);
        }



        #endregion HTML Builder code

        // Miscelaneous functions
        #region Misc

        public static Boolean AreRowsAltered(DataTable dt_in)
        {
            /*
             * Purpose: To let the Parent code know if it needs to process the altered rows
             */

            // Local Variables
            Boolean FoundAlteredRow = false;

            for (int i = 0; i < dt_in.Rows.Count; i++)
            {
                if (dt_in.Rows[i].RowState != DataRowState.Unchanged)
                {
                    FoundAlteredRow = true;
                    break;
                }
            }

            return (FoundAlteredRow);
        } //AreRowsAltered()

        public static void MergeCells(DataGridView dataGrid, int RowId1, int RowId2, int Column, bool isSelected)
        {
            //DataGridView dataGrid = inDGV;
            Graphics g = dataGrid.CreateGraphics();
            Pen gridPen = new Pen(dataGrid.GridColor);

            //Cells Rectangles
            Rectangle CellRectangle1 = dataGrid.GetCellDisplayRectangle(Column, RowId1, true);
            Rectangle CellRectangle2 = dataGrid.GetCellDisplayRectangle(Column, RowId2, true);

            int rectHeight = 0;
            string MergedRows = String.Empty;

            for (int i = RowId1; i <= RowId2; i++)
            {
                rectHeight += dataGrid.GetCellDisplayRectangle(Column, i, false).Height;
            }

            Rectangle newCell = new Rectangle(CellRectangle1.X, CellRectangle1.Y, CellRectangle1.Width, rectHeight);

            g.FillRectangle(new SolidBrush(isSelected ? dataGrid.DefaultCellStyle.SelectionBackColor : dataGrid.DefaultCellStyle.BackColor), newCell);

            g.DrawRectangle(gridPen, newCell);

            g.DrawString(dataGrid.Rows[RowId1].Cells[Column].Value.ToString(), dataGrid.DefaultCellStyle.Font, new SolidBrush(isSelected ? dataGrid.DefaultCellStyle.SelectionForeColor : dataGrid.DefaultCellStyle.ForeColor), newCell.X + newCell.Width / 3, newCell.Y + newCell.Height / 3);

        } //MergeCells()

        public static String SplitCase(String inStr)
        {
            // Remove Underscore and replace with Spaces
            inStr = inStr.Replace('_', ' ');
            // Split at capitalised text
            return (SplitCase(inStr, " "));
        } //SplitCase()

        public static String SplitCase(String inStr, String inReplacement)
        {
            //
            // Purpose: This was built to split text like "MyCurrencyIsUSD" into "My Currency Is USD"
            // 
            // Rules: I couldn't work out how to deal with existing splits like "Fund Name", so assume that we should not split already split text.
            //

            if (inStr.Contains(inReplacement))
                return (inStr);
            else
                return (Regex.Replace(inStr, @"(?<=[A-Z])(?=[A-Z][a-z]) | (?<=[^A-Z])(?=[A-Z]) | (?<=[A-Za-z])(?=[^A-Za-z])", inReplacement, RegexOptions.IgnorePatternWhitespace));

        } //SplitCase()

        public static void SetDataGridView(DataGridView dg_In)
        {
            // Standard setup

            
            for (int i=0;i<dg_In.Columns.Count;i++)
            {
                // Hide ID Columns
                if (dg_In.Columns[i].Name.EndsWith("ID") && dg_In.Columns[i].ValueType.Name.ToLower().StartsWith("int"))
                    dg_In.Columns[i].Visible = false;
                else
                {
                    // Bust up column name display splitting on Uppercase (eg. "EffectiveDate" to "Effective Date")
                    dg_In.Columns[i].HeaderText = SplitCase(dg_In.Columns[i].HeaderText);
                    if (dg_In.Columns[i].ValueType.Name.ToLower().StartsWith("int"))
                    {
                        dg_In.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dg_In.Columns[i].DefaultCellStyle.Format = "#,##0";
                    }
                    else if (dg_In.Columns[i].ValueType.Name.ToLower().StartsWith("decimal"))
                    {
                        dg_In.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        if (dg_In.Columns[i].Name.ToLower().Contains("price"))
                        {
                            dg_In.Columns[i].DefaultCellStyle.Format = "#,##0.000000";
                        }
                        else
                        {
                            dg_In.Columns[i].DefaultCellStyle.Format = "#,##0.00";
                            SetColumn(dg_In, dg_In.Columns[i].Name);
                        }
                    }
                    else if (dg_In.Columns[i].ValueType.Name.ToLower().StartsWith("datetime"))
                    {
                        dg_In.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dg_In.Columns[i].DefaultCellStyle.Format = "dd-MMM-yyyy";
                    }
                    else if (dg_In.Columns[i].ValueType.Name.ToLower().StartsWith("string"))
                    {
                        dg_In.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    }
                    else
                    {
                        Console.WriteLine("SetDataGridView() - Found a column type not dealt with:'" + dg_In.Columns[i].ValueType.Name+"'");
                    }
                }
            }

            for (int i = 0; i < dg_In.Columns.Count; i++)
            {
                // Autowidth columns
                dg_In.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        } // SetDataGridView()

        public static void SetColumn(DataGridView dgr, String ColName)
        {
            for (int i = 0; i < dgr.Rows.Count; i++)
                SystemLibrary.SetColumn(dgr, ColName, i);
        } // SetColumn()

        public static void SetColumn(DataGridView dgr, String ColName, Int32 myRow)
        {
            if (SystemLibrary.ToDouble(dgr[ColName, myRow].Value) < 0)
                dgr[ColName, myRow].Style.ForeColor = Color.Red;
            else
                dgr[ColName, myRow].Style.ForeColor = Color.Green;
        } // SetColumn()

        public static void SetTextBoxColour(TextBox tb_1, object inVal)
        {
            if (SystemLibrary.ToDecimal(inVal) < 0)
                tb_1.ForeColor = Color.Red;
            else if (SystemLibrary.ToDecimal(inVal) > 0)
                tb_1.ForeColor = Color.Green;
            else
                tb_1.ForeColor = Color.Black;
        } //SetTextBoxColour()

        /*
         * Procedure:   SetTimeZoneOffset()
         * 
         * Purpose: The windows Azure database GetDate() function is not releiable, so will use the GetUtcDate() and this offset
         * 
         * Written: 4-Feb-2014
         */
        public static void SetTimeZoneOffset()
        {
            DateTime UTCVal = DateTime.UtcNow;
            DateTime TimeZoneVal;
            String myTimeZone = "";
            try
            {
                myTimeZone = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('TimeZone')");

                // Convert back to the Database time zone
                TimeZoneVal = TimeZoneInfo.ConvertTimeFromUtc(UTCVal, TimeZoneInfo.FindSystemTimeZoneById(myTimeZone));

                // Work out the difference
                TimeSpan span = (TimeZoneVal - UTCVal);
                myOffsetSeconds = SystemLibrary.ToInt32(span.TotalSeconds);

                SystemLibrary.SQLExecute("Update System_Parameters Set p_number = " + SystemLibrary.ToString(myOffsetSeconds) + " Where Parameter_Name = 'TimeZone'");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to Set the 'TimeZone' offset for '" + myTimeZone + "'\r\n\r\n" + ex.Message, "Please report error to you administrator");
            }

            //myToday = DateTime.UtcNow.AddSeconds(SystemLibrary.myOffsetSeconds).Date;
            //myNow = DateTime.UtcNow.AddSeconds(SystemLibrary.myOffsetSeconds);

        } //SetTimeZoneOffset()

        public static DateTime f_Today()
        {
            return (DateTime.UtcNow.AddSeconds(SystemLibrary.myOffsetSeconds).Date);
            //return (myToday);
        }

        public static DateTime f_Now()
        {
            return (DateTime.UtcNow.AddSeconds(SystemLibrary.myOffsetSeconds));
            //return (myNow);
        }


        public static DateTime GetCreationTime(String myFileName)
        {
            return (File.GetCreationTimeUtc(myFileName).AddSeconds(myOffsetSeconds));
        }

        public static DateTime FirstDayOfMonth(DateTime inDateTime)
        {
            return new DateTime(inDateTime.Year, inDateTime.Month, 1);
        } //FirstDayOfMonth()

        public static DateTime LastDayOfMonth(DateTime inDateTime)
        {
            return new DateTime(inDateTime.Year, inDateTime.Month, 1).AddMonths(1).AddDays(-1);
        } //LastDayOfMonth()

        public static DateTime YYYYMMDD_ToDate(String inDateTime)
        {
            // Local Variables
            DateTime retVal = DateTime.MinValue;

            if (inDateTime.Length == 8)
            {
                int myDay = Convert.ToInt16(inDateTime.Substring(6, 2));
                int myMonth = Convert.ToInt16(inDateTime.Substring(4, 2));
                int myYear = Convert.ToInt16(inDateTime.Substring(0, 4));

                if (!(myDay == 0 || myMonth == 0 || myYear == 0))
                    retVal = new DateTime(myYear, myMonth, myDay);
            }

            return (retVal);
        } //YYYYMMDD_ToDate

        public static String TrimQuotes(String inStr)
        {
            // Local Variables
            String RetVal;

            if (inStr.StartsWith(@"""") && inStr.EndsWith(@""""))
                RetVal = inStr.TrimStart('"').TrimEnd('"');
            else
                RetVal = inStr;

            return (RetVal);

        } //TrimQuotes()

        public static Boolean YN_To_Bool(String inVal)
        {
            if (inVal.ToUpper().StartsWith("Y"))
                return (true);
            else
                return (false);
        } //YN_To_Bool()

        public static String Bool_To_YN(Boolean inVal)
        {
            if (inVal)
                return ("Y");
            else
                return ("N");
        } //Bool_To_YN()

        public static Decimal ToDecimal(object inArg)
        {
            // Local Variables
            Decimal RetVal;

            if (inArg == DBNull.Value)
                RetVal = 0;
            else if (inArg == null)
                RetVal = 0;
            else if (inArg.ToString().Length == 0)
                RetVal = 0;
            else
            {
                try
                {
                    RetVal = Convert.ToDecimal(inArg);
                }
                catch
                {
                    RetVal = 0;
                }
            }

            return (RetVal);

        } // ToDecimal()

        public static Double ToDouble(object inArg)
        {
            // Local Variables
            Double RetVal;

            if (inArg == DBNull.Value)
                RetVal = 0;
            else
            {
                try
                {
                    RetVal = Convert.ToDouble(inArg);
                }
                catch
                {
                    RetVal = 0;
                }
            }

            return (RetVal);

        } // ToDouble()

        public static Int16 ToInt16(object inArg)
        {
            // Local Variables
            Int16 RetVal;

            if (inArg == DBNull.Value)
                RetVal = 0;
            else
            {
                try
                {
                    RetVal = Convert.ToInt16(inArg);
                }
                catch
                {
                    RetVal = 0;
                }
            }

            return (RetVal);

        } // ToInt16()

        public static Int32 ToInt32(object inArg)
        {
            // Local Variables
            Int32 RetVal;

            if (inArg == DBNull.Value || inArg == null)
                RetVal = 0;
            else
            {
                try
                {
                    if (!Int32.TryParse(inArg.ToString(),System.Globalization.NumberStyles.Any, null, out RetVal))
                        RetVal = 0;
                }
                catch
                {
                    RetVal = 0;
                }
            }

            return (RetVal);

        } // ToInt32()


        public static String ToString(object inArg)
        {
            // Local Variables
            String RetVal;

            if (inArg == DBNull.Value)
                RetVal = "";
            else
            {
                try
                {
                    RetVal = Convert.ToString(inArg);
                }
                catch
                {
                    RetVal = "";
                }
            }

            return (RetVal);

        } // ToString()

        public static void PrintScreen(Form inForm)
        {
            String myFileName = "";
            String myPath = "";
            printDoc = new System.Drawing.Printing.PrintDocument();
            printDoc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDoc_PrintPage);
            
            Graphics g1 = inForm.CreateGraphics();
            Image MyImage = new Bitmap(inForm.ClientRectangle.Width, inForm.ClientRectangle.Height, g1);
            Graphics g2 = Graphics.FromImage(MyImage);
            IntPtr dc1 = g1.GetHdc();
            IntPtr dc2 = g2.GetHdc();
            BitBlt(dc2, 0, 0, inForm.ClientRectangle.Width, inForm.ClientRectangle.Height, dc1, 0, 0, 13369376);
            g1.ReleaseHdc(dc1);
            g2.ReleaseHdc(dc2);
            myPath = Path.GetTempPath();
            if (myPath.Length==0)
                myPath = @"C:";
            myFileName = myPath + @"\PrintPage.jpg"; 
            MyImage.Save(myFileName, ImageFormat.Jpeg);
            FileStream fileStream = new FileStream(myFileName, FileMode.Open, FileAccess.Read);
            StartPrint(fileStream, "Image");
            fileStream.Close();

            if (System.IO.File.Exists(myFileName))
            {
                System.IO.File.Delete(myFileName);
            }

        } //PrintScreen()

        private static void StartPrint(Stream instreamToPrint, string instreamType)
        {
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
            streamToPrint = instreamToPrint;
            streamType = instreamType;
            System.Windows.Forms.PrintDialog PrintDialog1 = new PrintDialog();
            PrintDialog1.AllowSomePages = true;
            PrintDialog1.ShowHelp = true;
            PrintDialog1.Document = printDoc;
            PrintDialog1.UseEXDialog = true;

            DialogResult result = PrintDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                printDoc.DefaultPageSettings.Landscape = true;
                // Margins with 1-inch  = 100   (Left, Right, Top, Bottom)
                Margins margins = new Margins(50, 50, 50, 50);
                printDoc.DefaultPageSettings.Margins = margins;
                //docToPrint.Print();
                printDoc.Print();
            }

        } //StartPrint()

        private static void printDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            System.Drawing.Image image = System.Drawing.Image.FromStream(streamToPrint);
            int x = e.MarginBounds.X;
            int y = e.MarginBounds.Y;
            int width = image.Width;
            int height = image.Height;
            if ((width / e.MarginBounds.Width) > (height / e.MarginBounds.Height))
            {
                width = e.MarginBounds.Width;
                height = image.Height * e.MarginBounds.Width / image.Width;
            }
            else
            {
                height = e.MarginBounds.Height;
                width = image.Width * e.MarginBounds.Height / image.Height;
            }
            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(x, y, width, height);
            e.Graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, System.Drawing.GraphicsUnit.Pixel);
        }

        public static String GetUserName()
        {
            return UserPrincipal.Current.GivenName + ' ' + UserPrincipal.Current.Surname;
        } //GetUserName()

        public static void PositionFormOverParent(Form ThisForm, Form ParentForm)
        {
            // Local Variables
            int myX;
            int myY;
            // Position This form above the Parent
            if (ParentForm != null)
            {
                Rectangle rect = new Rectangle(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);

                foreach (Screen screen in Screen.AllScreens)
                    rect = Rectangle.Union(rect, screen.Bounds);


                myX = ParentForm.Location.X + (ParentForm.Width - ThisForm.Width) / 2;
                myY = ParentForm.Location.Y + (ParentForm.Height - ThisForm.Height) / 2;
                if (myX < rect.X || myX > (rect.X + rect.Width))
                    myX = rect.X + 20;
                if (myY < rect.Y)
                    myY = rect.Y + 20;
                ThisForm.Location = new Point(myX, myY);
            }
        } //PositionFormOverParent()

        public static Form FormExists(Form inForm, Boolean CloseFormDown)
        {
            // Local Variables
            Form RetVal = null;

            // Ensure only 1 version of a form is open
            for (int i = 0; i < Application.OpenForms.Count; i++)
            {
                if (Application.OpenForms[i].Name == inForm.Name)
                {
                    if (CloseFormDown)
                        Application.OpenForms[i].Close();
                    else
                        RetVal = Application.OpenForms[i];
                }
            }

            return (RetVal);

        } //FormExists()

        #endregion // Misc

        // GPG redirection
        #region GPG
        public static Boolean MLPrime_Encrypt(String myFilePath, String inFile, ref String outFile)
        {
            // Local Variables
            // TODO (5) I need to get these from the Registry I think.
            String _bindirectory = @"C:\gnupg";
            int ProcessTimeOutMilliseconds = 10000; // 10 seconds
            int _exitcode = 0;

            if (MLPrime_Crypt_Key == null)
                MLPrime_Crypt_Key = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('MLPrime_Crypt_Key')");

            if (MLPrime_Crypt_User == null)
                MLPrime_Crypt_User = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('MLPrime_Crypt_User')");

            if (MLPrimeFilePath == null)
                MLPrimeFilePath = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('MLPrime_Path')");

            if (MLPrimeFilePath.Length < 1)
                return (false);

            String gpgOptions = "";
            String gpgExecutable = _bindirectory + "\\gpg.exe";
            DateTime lastWriteTime = File.GetLastWriteTime(myFilePath + @"\" + inFile);


            // Get oufilename
            outFile = inFile + @".asc." + lastWriteTime.ToString("yy.MM.dd_HH.mm");
            if (File.Exists(myFilePath + @"\" + outFile))
                File.Delete(myFilePath + @"\" + outFile);

            // Setup the file options
            gpgOptions = @"--homedir """ + MLPrimeFilePath + @"\keyrings"" " +
                         @"--output """ + myFilePath + @"\" + outFile + @""" " +
                         @"--armor --verbose --verbose --batch --yes --local-user " + MLPrime_Crypt_User + " --passphrase-fd --no-tty " +
                         @"--recipient ""Merrill Lynch CLEAR system DH <clear@ml.com>"" " +
                         @"--comment """ + outFile + @""" --sign " +
                         @"--encrypt """ + myFilePath + @"\" + inFile + @""" ";

            // Create startinfo object
            ProcessStartInfo pInfo = new ProcessStartInfo(gpgExecutable, gpgOptions);
            pInfo.WorkingDirectory = _bindirectory;
            pInfo.CreateNoWindow = true;
            pInfo.UseShellExecute = false;
            // Redirect everything: 
            // stdin to send the passphrase, stdout to get encrypted message, stderr in case of errors...
            pInfo.RedirectStandardInput = true;
            pInfo.RedirectStandardOutput = true;
            pInfo.RedirectStandardError = true;
            _processObject = Process.Start(pInfo);

            // Send input text
            _processObject.StandardInput.Write(MLPrime_Crypt_Key);
            _processObject.StandardInput.Flush();
            _processObject.StandardInput.Close();

            _outputString = "";
            _errorString = "";

            // Create two threads to read both output/error streams without creating a deadlock
            ThreadStart outputEntry = new ThreadStart(StandardOutputReader);
            Thread outputThread = new Thread(outputEntry);
            outputThread.Start();
            ThreadStart errorEntry = new ThreadStart(StandardErrorReader);
            Thread errorThread = new Thread(errorEntry);
            errorThread.Start();

            if (_processObject.WaitForExit(ProcessTimeOutMilliseconds))
            {
                // Process exited before timeout...
                // Wait for the threads to complete reading output/error (but use a timeout!)
                if (!outputThread.Join(ProcessTimeOutMilliseconds / 2))
                {
                    outputThread.Abort();
                }
                if (!errorThread.Join(ProcessTimeOutMilliseconds / 2))
                {
                    errorThread.Abort();
                }
            }
            else
            {
                // Process timeout: PGP hung somewhere... kill it (as well as the threads!)
                _outputString = "";
                _errorString = "Timed out after " + ProcessTimeOutMilliseconds.ToString() + " milliseconds";
                _processObject.Kill();
                if (outputThread.IsAlive)
                {
                    outputThread.Abort();
                }
                if (errorThread.IsAlive)
                {
                    errorThread.Abort();
                }
            }

            // Check results and prepare output
            _exitcode = _processObject.ExitCode;
            TextWriter stdoutWriter = Console.Error;
            stdoutWriter.WriteLine(_outputString);
            stdoutWriter.Flush();
            stdoutWriter.Close();

            TextWriter stderrWriter = Console.Error;
            stderrWriter.WriteLine(_errorString);
            stderrWriter.Flush();
            stderrWriter.Close();

            if (_exitcode == 0)
                return (true);
            else
            {
                MessageBox.Show("Encrypt Failed:\r\n\r\n" +
                                "Stdout:\r\n" + _outputString + "\r\n\r\n" +
                                "Stderr:\r\n" + _errorString + "\r\n\r\n",
                                "Encrypt Failed: " + myFilePath + @"\" + inFile
                                );
                return (false);
            }

        } //MLPrime_Encrypt()

        public static Boolean MLPrime_Decrypt(String myFilePath, String inFile, ref String outFile)
        {
            // Local Variables
            // TODO (5) I need to get these from the Registry I think.
            String _bindirectory = @"C:\gnupg";
            int ProcessTimeOutMilliseconds = 10000; // 10 seconds
            int _exitcode = 0;

            if(MLPrime_Crypt_Key == null)
                MLPrime_Crypt_Key = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('MLPrime_Crypt_Key')");

            if (MLPrimeFilePath == null)
                MLPrimeFilePath = SystemLibrary.SQLSelectString("Select dbo.f_GetParamString('MLPrime_Path')");

            if (MLPrimeFilePath.Length < 1)
                return (false);

            String gpgOptions = "";
            String gpgExecutable = _bindirectory + "\\gpg.exe";
            DateTime lastWriteTime = File.GetLastWriteTime(myFilePath + @"\" + inFile);


            // Get oufilename
            outFile = inFile.Substring(0, inFile.Length - 19); // Last 19 chars are <outFile>.asc.yy.MM.dd_HH.mm
            if (File.Exists(myFilePath + @"\" + outFile))
                File.Delete(myFilePath + @"\" + outFile);

            // Setup the file options
            gpgOptions = @"--homedir """ + MLPrimeFilePath + @"\keyrings"" " +
                         @"--output """ + myFilePath + @"\" + outFile + @""" " +
                         @"--verbose --batch --yes --passphrase-fd --no-tty " +
                         @"--decrypt """ + myFilePath + @"\" + inFile + @""" ";

            // Create startinfo object
            ProcessStartInfo pInfo = new ProcessStartInfo(gpgExecutable, gpgOptions);
            pInfo.WorkingDirectory = _bindirectory;
            pInfo.CreateNoWindow = true;
            pInfo.UseShellExecute = false;
            // Redirect everything: 
            // stdin to send the passphrase, stdout to get encrypted message, stderr in case of errors...
            pInfo.RedirectStandardInput = true;
            pInfo.RedirectStandardOutput = true;
            pInfo.RedirectStandardError = true;
            _processObject = Process.Start(pInfo);

            // Send input text
            _processObject.StandardInput.Write(MLPrime_Crypt_Key);
            _processObject.StandardInput.Flush();
            _processObject.StandardInput.Close();

            _outputString = "";
            _errorString = "";

            // Create two threads to read both output/error streams without creating a deadlock
            ThreadStart outputEntry = new ThreadStart(StandardOutputReader);
            Thread outputThread = new Thread(outputEntry);
            outputThread.Start();
            ThreadStart errorEntry = new ThreadStart(StandardErrorReader);
            Thread errorThread = new Thread(errorEntry);
            errorThread.Start();

            if (_processObject.WaitForExit(ProcessTimeOutMilliseconds))
            {
                // Process exited before timeout...
                // Wait for the threads to complete reading output/error (but use a timeout!)
                if (!outputThread.Join(ProcessTimeOutMilliseconds / 2))
                {
                    outputThread.Abort();
                }
                if (!errorThread.Join(ProcessTimeOutMilliseconds / 2))
                {
                    errorThread.Abort();
                }
            }
            else
            {
                // Process timeout: PGP hung somewhere... kill it (as well as the threads!)
                _outputString = "";
                _errorString = "Timed out after " + ProcessTimeOutMilliseconds.ToString() + " milliseconds";
                _processObject.Kill();
                if (outputThread.IsAlive)
                {
                    outputThread.Abort();
                }
                if (errorThread.IsAlive)
                {
                    errorThread.Abort();
                }
            }

            // Check results and prepare output
            _exitcode = _processObject.ExitCode;
            TextWriter stdoutWriter = Console.Error;
            stdoutWriter.WriteLine(_outputString);
            stdoutWriter.Flush();
            stdoutWriter.Close();

            TextWriter stderrWriter = Console.Error;
            stderrWriter.WriteLine(_errorString);
            stderrWriter.Flush();
            stderrWriter.Close();

            if (_exitcode == 0)
            {
                // Use the file extension to set the file create time same as on ML Prime server.
                // inFile.Substring(0, inFile.Length - 19); // Last 19 chars are <inFile>.asc.yy.MM.dd_HH.mm
                int myYear = SystemLibrary.ToInt16(inFile.Substring(inFile.Length-14,2));
                if (myYear < 2000) myYear = myYear + 2000;
                int myMonth = SystemLibrary.ToInt16(inFile.Substring(inFile.Length-11,2));
                int myDay = SystemLibrary.ToInt16(inFile.Substring(inFile.Length-8,2));
                int myHH = SystemLibrary.ToInt16(inFile.Substring(inFile.Length-5,2));
                int myMin = SystemLibrary.ToInt16(inFile.Substring(inFile.Length - 2, 2));
                DateTime ExtractedDate = new DateTime(myYear, myMonth, myDay, myHH, myMin, 0, DateTimeKind.Utc);
                File.SetCreationTimeUtc(myFilePath + @"\" + outFile, ExtractedDate);
                File.SetLastWriteTimeUtc(myFilePath + @"\" + outFile, ExtractedDate);
                File.SetLastAccessTimeUtc(myFilePath + @"\" + outFile, ExtractedDate);
                return (true);
            }
            else
            {
                MessageBox.Show("Decrypt Failed:\r\n\r\n" +
                                "Stdout:\r\n" + _outputString + "\r\n\r\n" +
                                "Stderr:\r\n" + _errorString + "\r\n\r\n",
                                "Decrypt Failed: " + myFilePath + @"\" + inFile
                                );
                return (false);
            }

        } //MLPrime_Decrypt()

        /// Reader thread for standard output
        /// 
        /// <p/>Updates the private variable _outputString (locks it first)
        /// </summary>
        public static void StandardOutputReader()
        {
            string output = _processObject.StandardOutput.ReadToEnd();
            //lock (this)
            {
                _outputString = output;
            }
        }

        /// <summary>
        /// Reader thread for standard error
        /// 
        /// <p/>Updates the private variable _errorString (locks it first)
        /// </summary>
        public static void StandardErrorReader()
        {
            string error = _processObject.StandardError.ReadToEnd();
            //lock (this)
            {
                _errorString = error;
            }
        }

        #endregion // GPG

        #region ProcessInputToDataTable
        /*
         * Source: Modified from RitchViewer 12 December 2013
         */

        public static String CopyDataFromDataTable(DataTable dt_in, ref DataTable dt_out)
        {
            /* 
             * We want the data from dt_in imported into the Schema in dt_out
             */

            // Local Variables
            String myMsg = "";
            String LastFailMessage = "";
            int FailedCount = 0;

            try
            {
                // Unfortunatley I cant fi
                for (int i = 0; i < dt_in.Rows.Count; i++)
                {
                    try
                    {
                        dt_out.Rows.Add(dt_in.Rows[i].ItemArray);
                    }
                    catch (Exception e)
                    { 
                        FailedCount = FailedCount + 1;
                        LastFailMessage = e.Message;
                    }
                }
                myMsg = "";
                if (FailedCount>0)
                {
                    if (myMsg.Length>0)
                        myMsg = myMsg + "\r\n";
                    myMsg = myMsg + FailedCount.ToString() + " rows failed to import with last message of\r\n" + LastFailMessage;
                }
            }
            catch 
            {
                myMsg = "Failed to map imported data";
            }

            return (myMsg);

        } //CopyDataFromDataTable()

        public static DataTable ProcesInput(IDataObject t)
        {
            /*
             * As we dont want formatting, I have put Text Format ahead of any other.
             */
            // Local Variables
            String strPastedText;
            DataTable dt = new DataTable();


            // See if there is a HTML Format
            if (t.GetDataPresent(DataFormats.FileDrop))
            {
                String[] fileNames;

                //strDataFormat = "FileDrop";
                fileNames = (string[])t.GetData(DataFormats.FileDrop);
                if (fileNames.Length > 0)
                {
                    // See if this is an Excel file
                    if (fileNames[0].Contains(".xls"))
                    {
                        try
                        {
                            dt = ExcelImport.Import.Query(fileNames[0]);
                        }
                        catch
                        {
                            MessageBox.Show("Sorry failed to Read in file\r\n\r\n\t" + fileNames[0], "Import Data");
                        }
                    }
                    else
                    {
                        dt = LoadToDataTable(GetFileData(fileNames[0]), "", "");
                    }
                }
            }
            else if (t.GetDataPresent(DataFormats.Text))
            {
                strPastedText = (string)t.GetData(DataFormats.Text);
                //strDataFormat = "TEXT";

                dt = LoadToDataTable(strPastedText, "", "");
            }
            else if (t.GetDataPresent(DataFormats.Html))
            {
                strPastedText = (string)t.GetData(DataFormats.Html);
                //strDataFormat = "HTML";
                dt = ProcessHTML(strPastedText);
            }

            /*
            String[] allFormats = t.GetFormats();
            // Creates the string that contains the formats.
            string theResult = "The format(s) associated with the data are: " + '\n';

            for (int i = 0; i < allFormats.Length; i++)
            {
                if (allFormats[i] == "ASEL")
                    FoundPDF = true;
                theResult += allFormats[i] + '\n';
            }
            // Displays the result in a message box.
            //MessageBox.Show(theResult, "File Types Available");
            Console.WriteLine(theResult);
            */

            return (dt);

        } // ProcessInput()

        private static DataTable ProcessHTML(String inStr)
        {
            // Local Variables
            DataTable dt = new DataTable();
            List<List<string>> rowContents = new List<List<string>>();

            // Remove html tags to just extract row information and store it in rowContents
            System.Text.RegularExpressions.Regex TRregex = new System.Text.RegularExpressions.Regex(@"<( )*tr([^>])*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex TDregex = new System.Text.RegularExpressions.Regex(@"<( )*td([^>])*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Match trMatch = TRregex.Match(inStr);
            while (!String.IsNullOrEmpty(trMatch.Value))
            {
                int rowStart = trMatch.Index + trMatch.Length;
                int rowEnd = inStr.IndexOf("</tr>", rowStart, StringComparison.InvariantCultureIgnoreCase);
                System.Text.RegularExpressions.Match tdMatch = TDregex.Match(inStr, rowStart, rowEnd - rowStart);
                List<string> rowContent = new List<string>();
                while (!String.IsNullOrEmpty(tdMatch.Value))
                {
                    int cellStart = tdMatch.Index + tdMatch.Length;
                    int cellEnd = inStr.IndexOf("</td>", cellStart, StringComparison.InvariantCultureIgnoreCase);
                    String cellContent = inStr.Substring(cellStart, cellEnd - cellStart);
                    cellContent = System.Text.RegularExpressions.Regex.Replace(cellContent, @"<( )*br( )*>", "\r\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    cellContent = System.Text.RegularExpressions.Regex.Replace(cellContent, @"<( )*li( )*>", "\r\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    cellContent = System.Text.RegularExpressions.Regex.Replace(cellContent, @"<( )*div([^>])*>", "\r\n\r\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    cellContent = System.Text.RegularExpressions.Regex.Replace(cellContent, @"<( )*p([^>])*>", "\r\n\r\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    // Colin stuff
                    string fred = System.Text.RegularExpressions.Regex.Replace(cellContent, @"<( )*([^>])*>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    cellContent = System.Text.RegularExpressions.Regex.Replace(cellContent, @"<( )*([^>])*>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    cellContent = cellContent.Replace("&nbsp;", " ");
                    cellContent = cellContent.Replace("Â", "");
                    cellContent = HTMLCharSwap(cellContent);
                    cellContent = cellContent.Replace("\r\n", "");

                    // Colin stuff
                    rowContent.Add(cellContent);
                    tdMatch = tdMatch.NextMatch();
                }
                if (rowContent.Count > 0)
                {
                    rowContents.Add(rowContent);
                }
                trMatch = trMatch.NextMatch();
            }

            // Create Column Headers
            foreach (List<String> rowContent in rowContents)
            {
                while (rowContent.Count > dt.Columns.Count)
                {
                    //dt.Columns.Add(new DataColumn(((char)(dt.Columns.Count + 65)).ToString()));
                    dt.Columns.Add(new DataColumn("Col " + (dt.Columns.Count + 1).ToString()));
                }
            }

            // Now add all the data
            foreach (List<String> rowContent in rowContents)
            {
                dt.Rows.Add();
                int myRow = dt.Rows.Count - 1;
                int myCol = 0;
                foreach (String cellContent in rowContent)
                {
                    dt.Rows[myRow][myCol] = cellContent;
                    myCol = myCol + 1;
                }
            }


            return (dt);
        } // ProcessHTML()

        private static String HTMLCharSwap(String InString)
        {
            //
            // Function: HTMLCharSwap
            //
            // Purpose: Switch html special chars to real values
            //
            // Written: Colin Ritchie 26 Aug 2010
            //
            // Modified:
            //

            // Local Variables
            String strNormal;


            strNormal = InString.Replace("&ndash;", "–").Replace("&mdash;", "—").Replace("&iexcl;", "¡").Replace("&iquest;", "¿");
            strNormal = strNormal.Replace("&quot;", "\"").Replace("&ldquo;", "“").Replace("&rdquo;", "”").Replace("&lsquo;", "‘");
            strNormal = strNormal.Replace("&rsquo;", "’").Replace("&laquo;", "«").Replace("&raquo;", "»").Replace("&nbsp;", " ");
            strNormal = strNormal.Replace("&amp;", "&").Replace("&cent;", "¢").Replace("&copy;", "©").Replace("&divide;", "÷");
            strNormal = strNormal.Replace("&gt;", ">").Replace("&lt;", "<").Replace("&micro;", "µ").Replace("&middot;", "•");
            strNormal = strNormal.Replace("&para;", "¶").Replace("&plusmn;", "±").Replace("&euro;", "€").Replace("&pound;", "£");
            strNormal = strNormal.Replace("&reg;", "®").Replace("&sect;", "§").Replace("&trade;", "™").Replace("&yen;", "¥");


            return (strNormal);

        } // 'HTMLCharSwap()


        public static String GetFileData(string strFileName)
        {
            // Purpose: Extract data from a file and return in a string to be parsed to input into a datatable
            //
            //  Written:    Colin Ritchie 6-Aug-2010
            //              Got sick of Microsoft product not working, so creating my own parser routines
            //
            // Modified:    
            //

            // Local Variables
            String myRetStr = File.ReadAllText(strFileName);

            return (myRetStr);

        } // GetFileData()


        public static DataTable LoadToDataTable(String InputStr, String strFileName, String NominatedType)
        {
            // Purpose: Evaluate data in the string and convert it to a preferred data type
            //
            //  Written:    Colin Ritchie 6-Aug-2010
            //              Last 2 arguments are optional
            //
            // Modified:    
            //

            // Local Variables
            DataTable dt = new DataTable();
            String[] myRows;
            String[] myFields;
            char[] mySeperatorRow = { '\r', '\n' }; // { '\r', '\n', '\t' };
            char[] mySeperatorField = { ',' };
            Boolean IsSameLen = false;
            int LastLength = -12345;
            //String strPastedText = "";
            //String strDataFormat = "";
            //Boolean FoundPDF = false;
            int CountTabs = 0;
            int CountCommas = 0;



            // Lets see what kind of data this is, unless already nominated by the user
            if (NominatedType.Length == 0 && InputStr.Length > 0)
            {
                myRows = InputStr.Split(mySeperatorRow, StringSplitOptions.RemoveEmptyEntries);
                foreach (string myRow in myRows)
                {
                    if (myRow.Trim().Length > 0)
                    {
                        // See if a fixed length array
                        if (LastLength == -12345)
                        {
                            LastLength = myRow.Length;
                            IsSameLen = true;
                        }
                        if (IsSameLen && LastLength != myRow.Length)
                        {
                            IsSameLen = false;
                        }
                        // See how many rows are comma seperated
                        if (myRow.Contains("\t"))
                        {
                            CountTabs++;
                        }
                        if (myRow.Contains(","))
                        {
                            CountCommas++;
                        }
                    }
                }
                if (CountTabs > CountCommas && (CountTabs + CountCommas) != 0)
                {
                    mySeperatorField[0] = '\t';
                }
                else if (CountCommas > CountTabs && (CountTabs + CountCommas) != 0)
                {
                    mySeperatorField[0] = ',';
                }
                else
                {
                    mySeperatorField[0] = (char)0;
                }
            }

            // Process File into DataTable
            myRows = InputStr.Split(mySeperatorRow, StringSplitOptions.RemoveEmptyEntries);
            foreach (string myRow in myRows)
            {
                if (myRow.Length > 0)
                {
                    // Now split the Fields
                    if (mySeperatorField[0] == (char)0)
                    {
                        myFields = new string[1];
                        myFields[0] = myRow;
                    }
                    else
                    {
                        //myFields = myRow.Split(mySeperatorField, StringSplitOptions.RemoveEmptyEntries);
                        myFields = myRow.Split(mySeperatorField);
                    }
                    // See if this is the first Row & there fore header
                    if (dt.Columns.Count == -12345 && myFields.Length > 0)
                    {
                        for (int i = 0; i < myFields.Length; i++)
                            dt.Columns.Add(new DataColumn(myFields[i], typeof(string)));
                    }
                    else
                    {
                        while (myFields.Length > dt.Columns.Count)
                        {
                            dt.Columns.Add(new DataColumn("Col" + Convert.ToString(dt.Columns.Count + 1), typeof(string)));
                        }
                        if (myFields.Length > 0)
                        {
                            for (int i = 0; i < myFields.Length; i++)
                                if (myFields[i] == "NULL")
                                    myFields[i] = null;
                            dt.Rows.Add(myFields);
                        }
                    }
                }
            }

            return (dt);

        } // LoadToDataTable()





        #endregion // ProcessInputToDataTable

    }
}
