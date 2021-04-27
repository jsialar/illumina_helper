using System;
using System.Reflection;
using System.Windows.Forms;

namespace Kbg.NppPluginNET
{
    public partial class SettingsForm : Form
        
    {
        public string runparameters_settings;
        public SettingsForm(Settings.Options options)
        {
            
            InitializeComponent();
            
            foreach (FieldInfo form_field in this.GetType().GetFields())
            {
                
                if (form_field.Name.StartsWith("checkedList_runparam"))
                {
                    System.Windows.Forms.CheckedListBox checkedlist_runparam = (System.Windows.Forms.CheckedListBox)form_field.GetValue(this);
                    string[] checkeditems_array = (string[])options.GetType().GetField(form_field.Name).GetValue(options);
                    settochecked(checkeditems_array, checkedlist_runparam);

                }
                else if (form_field.Name == "checkedList_finderror")
                {
                    System.Windows.Forms.CheckedListBox checkedlist_finderror = (System.Windows.Forms.CheckedListBox)form_field.GetValue(this);
                    string[] allitems_array = (string[])options.GetType().GetField(form_field.Name+"_allitems").GetValue(options);
                    string[] checkeditems_array = (string[])options.GetType().GetField(form_field.Name).GetValue(options);

                    checkedList_finderror.Items.AddRange(allitems_array);
                    settochecked(checkeditems_array, checkedlist_finderror);

                }

            }


        }

        void settochecked(string[] checkeditems_array, CheckedListBox checkedlist)
        {
            foreach (string item in checkeditems_array)
            {
                int ind_to_check = checkedlist.Items.IndexOf(item);
                if (ind_to_check == -1)
                {
                    continue;
                }
                checkedlist.SetItemChecked(ind_to_check, true);
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtb_newexpression.Text))
                return;
            checkedList_finderror.Items.Add(txtb_newexpression.Text);
            txtb_newexpression.Clear();
            txtb_newexpression.Focus();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (checkedList_finderror.Items.Count > 0)
            {
                checkedList_finderror.Items.RemoveAt(checkedList_finderror.SelectedIndex);
            }
        }

        private void toggleall_Click(object sender, EventArgs e)
        {
            if (checkedList_finderror.CheckedIndices.Count > 0)
            {
                for (int i = 0; i < checkedList_finderror.Items.Count; i++)
                {
                    checkedList_finderror.SetItemChecked(i, false);
                }
            }
            else
            {
                for (int i = 0; i < checkedList_finderror.Items.Count; i++)
                {
                    checkedList_finderror.SetItemChecked(i, true);
                }
            }
        }
    }
}
