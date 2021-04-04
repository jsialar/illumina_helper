using System.Windows.Forms;
using System.Reflection;
using System;

namespace Kbg.NppPluginNET
{
    public partial class SettingsForm : Form
        
    {
        public string runparameters_settings;
        public SettingsForm(Settings.Options options)
        {
            InitializeComponent();

            object options = options;
            foreach (FieldInfo field in options.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {

                if (field.Name.StartsWith("checkedList"))
                {
                    string[] items_array = field.GetValue(options);

                }



                this.options = (Options)options;
            }

            int[] runparameters_index=Array.ConvertAll(runparameters_settings.Split(','), Int32.Parse);
           
            foreach (int index in runparameters_index)
            {
                checkedList_additional.SetItemChecked(index, true);
            } 
            
            
        }


    }
}
