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
        public class Options
        {
            public string[] checkedList_runparam_header;
            public string[] checkedList_runparam_con;
            public string[] checkedList_runparam_additional;
            public string[] checkedList_finderror;
            public string[] checkedList_finderror_allitems;
            internal string checkedList_finderror_str
            {
                get => String.Join("|", checkedList_finderror);
            }
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
           
            
            if (File.Exists(iniFilePath))
            {
                object options = this.options;
                foreach (FieldInfo field in this.options.GetType().GetFields())
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
            options.checkedList_runparam_header = new[] { "Platform", "RunID", "ExperimentName", "StartDate" };
            options.checkedList_runparam_con = new[] { "Part#", "Lot#", "Expiry date" };
            options.checkedList_runparam_additional = new[] { "Control software version" };
            options.checkedList_finderror = new[] { "FATAL", "\\sERR\\s", "\\sERROR", "\\sError", "\\serror"};
            options.checkedList_finderror_allitems = options.checkedList_finderror;

        }

        public void Save()
        {
            //Save ini file settings based on struct members
            foreach (FieldInfo field in this.options.GetType().GetFields())
            {
                if (field.FieldType == typeof(string[]))

                {
                    string[] value = (string[])field.GetValue(this.options);
                    string towrite = String.Join(",", value);
                    Win32.WritePrivateProfileString(this.assemblyName, field.Name, towrite, iniFilePath);
                }
            }
        }
        public void Update_from_form(SettingsForm settingsForm)
        {
            void getcheckeditems(FieldInfo form_field)
            {
                System.Windows.Forms.CheckedListBox checkedlistbox = (System.Windows.Forms.CheckedListBox)form_field.GetValue(settingsForm);
                string[] items_array = checkedlistbox.CheckedItems.Cast<string>().ToArray();
                this.options.GetType().GetField(form_field.Name).SetValue(this.options, items_array);
            }
            void getallitems(FieldInfo form_field)
            {
                System.Windows.Forms.CheckedListBox checkedlistbox = (System.Windows.Forms.CheckedListBox)form_field.GetValue(settingsForm);
                string[] items_array = checkedlistbox.Items.Cast<string>().ToArray();
                this.options.GetType().GetField(form_field.Name+"_allitems").SetValue(this.options, items_array);
            }
            foreach (FieldInfo form_field in settingsForm.GetType().GetFields())
            {
                if (form_field.Name.StartsWith("checkedList_runparam"))
                {
                    getcheckeditems(form_field);
                }
                else if (form_field.Name == "checkedList_finderror")
                {
                    getcheckeditems(form_field);
                    getallitems(form_field);
                }

            }


        }

    }

}

