using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;


namespace T1MultiAsset.Registry
{
    //
    // Concepts taken from http://www.csharphelp.com/archives2/archive430.html
    //
    class Registry
    {
        public Object RegGetValue(string myRegTopKey, string myRegPath, string myRegKey)
        {

            // Local variables
            RegistryKey OurKey;
            string myValue;

            OurKey = GetKeyClass(myRegTopKey);

            // Open the Sub Key
            OurKey = OurKey.OpenSubKey(myRegPath, true);
            if (OurKey == null)
                myValue = "";
            else
            {
                // Return the Value
                try
                {
                    Object myObject = OurKey.GetValue(myRegKey, "");
                    if (myObject == null)
                        myValue = "";
                    else
                        myValue = myObject.ToString();
                    OurKey.Close();
                    OurKey = null;
                }
                catch // (Exception e)
                {
                    myValue = "";
                }
            }
            return (myValue);
        } // RegGetValue()

        public Boolean RegSetValue(string myRegTopKey, string myRegPath, string myRegKey, Object myValue)
        {
            // Local variables
            RegistryKey OurKey;
            RegistryKey OurKey2;
            Boolean RetVal;

            OurKey = GetKeyClass(myRegTopKey);

            // Open the Sub Key
            OurKey2 = OurKey.OpenSubKey(myRegPath, true);
            if (OurKey2 == null)
            {
                // Create the new path.
                OurKey2 = OurKey.CreateSubKey(myRegPath);
                OurKey2 = OurKey.OpenSubKey(myRegPath, true);
            }

            // Return the Value
            try
            {
                RetVal = true;
                OurKey2.SetValue(myRegKey, myValue);
            }
            catch //(Exception e)
            {
                RetVal = false;
            }
            //OurKey.Close();
            OurKey = null;
            OurKey2 = null;
            return (RetVal);
        } // RegSetValue()

        private RegistryKey GetKeyClass(String myRegTopKey)
        {
            // Local Variables
            RegistryKey OurKey;

            switch (myRegTopKey)
            {
                case "HKEY_CLASSES_ROOT":
                    OurKey = Microsoft.Win32.Registry.ClassesRoot;
                    break;
                case "HKEY_CURRENT_USER":
                    OurKey = Microsoft.Win32.Registry.CurrentUser;
                    break;
                case "HKEY_LOCAL_MACHINE":
                    OurKey = Microsoft.Win32.Registry.LocalMachine;
                    break;
                case "HKEY_USERS":
                    OurKey = Microsoft.Win32.Registry.Users;
                    break;
                case "HKEY_CURRENT_CONFIG":
                    OurKey = Microsoft.Win32.Registry.CurrentConfig;
                    break;
                default:
                    OurKey = Microsoft.Win32.Registry.CurrentUser;
                    break;
            }

            return (OurKey);
        }
    }
}
