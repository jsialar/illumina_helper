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
    class Settings
    {
        public struct Options
        {
            public string[] checkedList_header;
            public string[] checkedList_con
            public string[] checkedList_additional;
        }

        string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
        public Settings()
        {
           
            // Get ini file path
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            string iniFilePath = sbIniFilePath.ToString();
            // if config path doesn't exist, we create it
            if (!Directory.Exists(iniFilePath))
            {
                Directory.CreateDirectory(iniFilePath);
            }
            iniFilePath = Path.Combine(iniFilePath, this.assemblyName + ".ini");



        }
        public Options getdefault()
        {
            Options options = new Options()
            {
                checkedList_header = new[] { "Platform", "RunID", "ExperimentName", "StartDate" },
                checkedList_con = new[] {  "Part#","Lot#","Expiry date"},
                checkedList_additional = new[] {"Control software version"}
            };
            return options;
        }
    }
}