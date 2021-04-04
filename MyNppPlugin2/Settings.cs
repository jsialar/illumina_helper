using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Kbg.NppPluginNET.PluginInfrastructure;
using System.IO;

namespace Kbg.NppPluginNET
{
    public class Settings
    {
        public struct Options
        {
            public string[] checkedList_header;
            public string[] checkedList_con;
            public string[] checkedList_additional;
        }

        public Options options = new Options();
        string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
        string iniFilePath;
        public Settings()
        {
           
            // Get ini file path
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            iniFilePath = sbIniFilePath.ToString();
            // if config path doesn't exist, we create it
            if (!Directory.Exists(iniFilePath))
            {
                Directory.CreateDirectory(iniFilePath);
            }
            iniFilePath = Path.Combine(iniFilePath, this.assemblyName + ".ini");

            Load();

        }
        public void Load()
        {
            //Grab ini file settings based on struct members
            
            
            if (File.Exists(iniFilePath))
            {
                object options = this.options;
                foreach (FieldInfo field in this.options.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {

                    if (field.FieldType == typeof(string[]))
                    {
                        StringBuilder sbFieldValue = new StringBuilder(32767);
                        Win32.GetPrivateProfileString(this.assemblyName, field.Name, "", sbFieldValue, 32767, iniFilePath);
                        field.SetValue(options, sbFieldValue.ToString().Split(','));
                    }

                }
                
                this.options = (Options)options;
            }
            else
            {
                GetDefaultOptions();
            }

        }

        public void GetDefaultOptions()
        {
            options.checkedList_header = new[] { "Platform", "RunID", "ExperimentName", "StartDate" };
            options.checkedList_con = new[] { "Part#", "Lot#", "Expiry date" };
            options.checkedList_additional = new[] { "Control software version" };
         }

        public void Save()
        {
            //Save ini file settings based on struct members
            foreach (FieldInfo field in this.options.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var value = field.GetValue(this.options);
                value = value != null ? value : "";
                if (field.FieldType == typeof(string[]))
                {
                    Win32.WritePrivateProfileString(this.assemblyName, field.Name, String.Join(",", value), iniFilePath);
                }
            }
        }
    }

}

