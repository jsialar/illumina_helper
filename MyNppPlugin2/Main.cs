using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;
using System.Threading;


namespace Kbg.NppPluginNET
{
    class Main
    {
        internal const string PluginName = "Illumina Helper";
        internal static frmMyDlg frmMyDlg = null;
        internal static Finderror_form finderror_form = null;
        internal static SettingsForm settingsform = null;
        internal static Settings settings = new Settings();
        static int idMyDlg ;
        //static Bitmap tbBmp = Properties.Resources.star;
        static Bitmap tbBmp = Properties.Resources.ilmn_icon_draw;
        //static Bitmap tbBmp_tbTab = Properties.Resources.star_bmp;
        static Icon tbIcon = null;
        static IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());
       
        static INotepadPPGateway notepad = new NotepadPPGateway();

        public static void OnNotification(ScNotification notification)
        {
            // This method is invoked whenever something is happening in notepad++
            // use eg. as
            // if (notification.Header.Code == (uint)NppMsg.NPPN_xxx)
            // { ... }
            // or
            //
            // if (notification.Header.Code == (uint)SciMsg.SCNxxx)
            // { ... }
            if (notification.Header.Code == (uint)NppMsg.NPPN_BUFFERACTIVATED)
            {
                if (frmMyDlg != null && frmMyDlg.Visible)
                {
                    parse_runparameters.ProcessXML();
                }
                if (finderror_form != null && finderror_form.Visible)
                {
                    Finderror.execute();
                }
            }
                
        }

        internal static void CommandMenuInit()
        {


            PluginBase.SetCommand(0, "Parse runparameters", parse_runparameters.ProcessXML, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(1, "Find errors", Finderror.execute, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(6, "-", null);
            PluginBase.SetCommand(2, "Settings", process_settings);
            PluginBase.SetCommand(6, "-", null);
            PluginBase.SetCommand(4, "About", about);
            #if DEBUG
            PluginBase.SetCommand(5, "debug", debug);
        #endif
            idMyDlg = 1;
        }

        
        internal static void SetToolBarIcon()
        {
            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = tbBmp.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_ADDTOOLBARICON, PluginBase._funcItems.Items[1]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }

        internal static void PluginCleanUp()
        {
            settings.Save();
        }

        internal static void process_settings()
        {
            var ctx1 = SynchronizationContext.Current;
            settingsform = new SettingsForm(settings.options);
            /* Get old values for checkedlistboxes
            CheckedListBox[] checkedboxes_arr = {settingsform.checkedList_header, settingsform.checkedList_con, settingsform.checkedList_additional };
            CheckedListBox.CheckedIndexCollection[] current_indices = new CheckedListBox.CheckedIndexCollection[checkedboxes_arr.Length];
            
            for (int i =0; i < checkedboxes_arr.Length; i++)
            {
                current_indices[i] = checkedboxes_arr[i].CheckedIndices;
            }
            */
            if (settingsform.ShowDialog(Control.FromHandle(PluginBase.GetCurrentScintilla())) == DialogResult.OK)
            {
                
                SynchronizationContext.SetSynchronizationContext(ctx1);
                settings.Update_from_form(settingsform);
                if (frmMyDlg != null && frmMyDlg.Visible)
                {
                    parse_runparameters.ProcessXML();
                }
                if (finderror_form != null && finderror_form.Visible)
                {
                    Finderror.execute();
                }
                
            }
            else
            {
                SynchronizationContext.SetSynchronizationContext(ctx1);
            }
            settingsform.Dispose();
            
        }

        internal static void close_dialogs()
        {
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMHIDE, 0, frmMyDlg.Handle);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_MODELESSDIALOG, 1, frmMyDlg.Handle);

            frmMyDlg.Dispose();
            frmMyDlg = null;

        }

        internal static void about()
        {
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            MessageBox.Show($@"Illumina helper plugin

Version {version}

Jaren Junren Sia 2021
Illumina APJ-RIS
", "About");
        }
#if DEBUG
        internal static void debug()
        {

            //var appConfig = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            //var apploc= System.Reflection.Assembly.GetExecutingAssembly().Location;
            //string msg= settings.runparameters_settings;
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            string msg = sbIniFilePath.ToString();
            MessageBox.Show(msg, "debug");
        }

#endif

        internal static void runparameters_disp()
        {
            if (frmMyDlg == null)
            {
                frmMyDlg = new frmMyDlg();
                using (Bitmap newBmp = new Bitmap(16, 16))
                {
                    Graphics g = Graphics.FromImage(newBmp);
                    ColorMap[] colorMap = new ColorMap[1];
                    colorMap[0] = new ColorMap();
                    colorMap[0].OldColor = Color.Fuchsia;
                    colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetRemapTable(colorMap);
                    g.DrawImage(tbBmp, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                    tbIcon = Icon.FromHandle(newBmp.GetHicon());
                }

                NppTbData _nppTbData = new NppTbData();
                _nppTbData.hClient = frmMyDlg.Handle;
                _nppTbData.pszName = "Parse runparameters";
                _nppTbData.dlgID = -1;
                _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                _nppTbData.hIconTab = (uint)tbIcon.Handle;
                _nppTbData.pszModuleName = PluginName;
                IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
                Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
            }
            else
            {
                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_DMMSHOW, 0, frmMyDlg.Handle);
            }
        }

        internal static void finderror_disp()
        {
            if (finderror_form == null)
            {
                finderror_form = new Finderror_form();
                using (Bitmap newBmp = new Bitmap(16, 16))
                {
                    Graphics g = Graphics.FromImage(newBmp);
                    ColorMap[] colorMap = new ColorMap[1];
                    colorMap[0] = new ColorMap();
                    colorMap[0].OldColor = Color.Fuchsia;
                    colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetRemapTable(colorMap);
                    g.DrawImage(tbBmp, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                    tbIcon = Icon.FromHandle(newBmp.GetHicon());
                }

                NppTbData _nppTbData2 = new NppTbData();
                _nppTbData2.hClient = finderror_form.Handle;
                _nppTbData2.pszName = "Find errors";
                _nppTbData2.dlgID = -1;
                _nppTbData2.uMask = NppTbMsg.DWS_DF_CONT_BOTTOM | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                _nppTbData2.hIconTab = (uint)tbIcon.Handle;
                _nppTbData2.pszModuleName = "plugin2";
                IntPtr _ptrNppTbData2 = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData2));
                Marshal.StructureToPtr(_nppTbData2, _ptrNppTbData2, false);

                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData2);
            }
            else
            {
                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMSHOW, 0, finderror_form.Handle);
            }
        }
    }
}