using System.Windows.Forms;
using System.Configuration;
using System;

namespace Kbg.NppPluginNET
{
    public partial class Settings : Form
        
    {
        public string runparameters_settings;
        public Settings()
        {
            InitializeComponent();
            
            var appConfig  = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var appSettings = appConfig.AppSettings;
            this.runparameters_settings = appSettings.Settings["runparameters"].Value;
            int[] runparameters_index=Array.ConvertAll(runparameters_settings.Split(','), Int32.Parse);
           
            foreach (int index in runparameters_index)
            {
                checkedListBox1.SetItemChecked(index, true);
            } 
            
            
        }


    }
}
