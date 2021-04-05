using System.Windows.Forms;
using System.Reflection;
using System;

namespace Kbg.NppPluginNET
{
    public partial class SettingsForm : Form
        
    {
        public string runparameters_settings;
        public SettingsForm(Settings.Options options_obj)
        {
            InitializeComponent();
            //object options_obj = options;
            foreach (FieldInfo field in options_obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {

                if (field.Name.StartsWith("checkedList"))
                {
                    string[] items_array = (string[])field.GetValue(options_obj);
                    System.Windows.Forms.CheckedListBox checkedlist_field = (System.Windows.Forms.CheckedListBox)this.GetType().GetField(field.Name).GetValue(this);
                    foreach (string item in items_array)
                    {
                        int ind_to_check = checkedlist_field.Items.IndexOf(item);
                        if (ind_to_check == -1)
                        {
                            continue;
                        }
                        checkedlist_field.SetItemChecked(ind_to_check, true);
                    }

                }

            }

            /*
                  int[] runparameters_index=Array.ConvertAll(runparameters_settings.Split(','), Int32.Parse);

                  foreach (int index in runparameters_index)
                  {
                      checkedList_additional.SetItemChecked(index, true);
                  } 

                  */
        }





    }
}
