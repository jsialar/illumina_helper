using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;
using System.Xml;
using System.Globalization;
using Kbg.NppPluginNET;
using Kbg.NppPluginNET.Properties;

using runparameters;

namespace runparameters
{
    static class parse_runparameters
    {

        static IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());
        static INotepadPPGateway notepad = new NotepadPPGateway();

        static XmlDocument doc = new XmlDocument();
        static CultureInfo enUs = new CultureInfo("en-US");

        static string exp_path(string con)
        { return $"/RunParameters/{con}/ExpirationDate"; }

        static string lot_path(string con)
        { return $"/RunParameters/{con}/LotNumber"; }

        static string part_path(string con)
        { return $"/RunParameters/{con}/PartNumber"; }

        static Dictionary<string, Dictionary<string, string>> add_info_dict = new Dictionary<string, Dictionary<string, string>>
        {
            ["MiSeq"] = new Dictionary<string, string> { 
                ["Control software name"] = "/RunParameters/Setup/ApplicationName",
                ["Control software version"] = "/RunParameters/Setup/ApplicationVersion"
            },
            ["iSeq"] = new Dictionary<string, string> {
                ["Control software name"] = "/RunParameters/ApplicationName",
                ["Control software version"] = "/RunParameters/ApplicationVersion"
            },
            ["MiniSeq"] = new Dictionary<string, string> {
                ["Control software name"] = "/RunParameters/Setup/ApplicationName",
                ["Control software version"] = "/RunParameters/Setup/ApplicationVersion",
                ["Custom read 1 primer"]="/RunParameters/UsesCustomReadOnePrimer",
                ["Custom read 2 primer"]="/RunParameters/UsesCustomReadTwoPrimer",
                ["Custom index 1 primer"]="/RunParameters/UsesCustomIndexPrimer",
                ["Custom index 2 primer"]="/RunParameters/UsesCustomIndexTwoPrimer"
            },
            ["NextSeq 500/550"] = new Dictionary<string, string> {
                ["Control software name"] = "/RunParameters/Setup/ApplicationName",
                ["Control software version"] = "/RunParameters/Setup/ApplicationVersion",
                ["Custom read 1 primer"] = "/RunParameters/UsesCustomReadOnePrimer",
                ["Custom read 2 primer"] = "/RunParameters/UsesCustomReadTwoPrimer",
                ["Custom index 1 primer"] = "/RunParameters/UsesCustomIndexPrimer",
                ["Custom index 2 primer"] = "/RunParameters/UsesCustomIndexTwoPrimer"
            },
            ["NextSeq 1000/2000"] = new Dictionary<string, string> {
                ["Control software name"] = "/RunParameters/ApplicationName",
                ["Control software version"] = "/RunParameters/ApplicationVersion"
            },
            ["NovaSeq"] = new Dictionary<string, string> {
                ["Control software name"] = "/RunParameters/Application",
                ["Control software version"] = "RunParameters/ApplicationVersion",
                ["Custom read 1 primer"] = "/RunParameters/UseCustomRead1Primer",
                ["Custom read 2 primer"] = "/RunParameters/UseCustomRead2Primer",
                ["Custom index 1 primer"]= "/RunParameters/UseCustomIndexRead1Primer",
                ["Custom index 2 primer"]= "/RunParameters/UseCustomIndexRead2Primer"
            },
        };

            static Dictionary<string, (string con, string part, string lot, string xpath)[]> consum_path = new Dictionary<string, (string con, string part, string lot, string xpath)[]>()
        {
            ["MiSeq"] = new[] { 
                ("Flowcell", part_path("FlowcellRFIDTag"), lot_path("FlowcellRFIDTag"), exp_path("FlowcellRFIDTag")),
                ("PR2", part_path("PR2BottleRFIDTag"), lot_path("PR2BottleRFIDTag"),exp_path("PR2BottleRFIDTag")),
                ("ReagentKit", part_path("ReagentKitRFIDTag"), lot_path("ReagentKitRFIDTag"),exp_path("ReagentKitRFIDTag"))
            },
            ["iSeq"] = new[] { 
                ("Flowcell", part_path("FlowcellEEPROMTag"), lot_path("FlowcellEEPROMTag"), exp_path("FlowcellEEPROMTag")),
                ("ReagentKit", part_path("ReagentKitRFIDTag"), lot_path("ReagentKitRFIDTag"), exp_path("ReagentKitRFIDTag"))
            },
            ["MiniSeq"] = new[] {
                ("Flowcell", part_path("FlowCellRfidTag"), lot_path("FlowCellRfidTag"), exp_path("FlowCellRfidTag")),
                ("ReagentKit", part_path("ReagentKitRfidTag"), lot_path("ReagentKitRfidTag"), exp_path("ReagentKitRfidTag"))
            },
            ["NextSeq 500/550"] = new[] {
                ("Flowcell", part_path("FlowCellRfidTag"), lot_path("FlowCellRfidTag"), exp_path("FlowCellRfidTag")),
                ("PR2", part_path("PR2BottleRfidTag"), lot_path("PR2BottleRfidTag"), exp_path("PR2BottleRfidTag")),
                ("ReagentKit", part_path("ReagentKitRfidTag"), lot_path("ReagentKitRfidTag"), exp_path("ReagentKitRfidTag"))
            },
            ["NextSeq 1000/2000"] = new[] {
                ("Flowcell","/RunParameters/FlowCellPartNumber", "/RunParameters/FlowCellLotNumber", "/RunParameters/FlowCellExpirationDate"),
                ("Cartridge","/RunParameters/CartridgePartNumber", "/RunParameters/CartridgeLotNumber", "/RunParameters/CartridgeExpirationDate")
            },
            ["NovaSeq"] = new[] {
                ("Flowcell", "/RunParameters/RfidsInfo/FlowCellPartNumber", "/RunParameters/RfidsInfo/FlowCellLotNumber", "/RunParameters/RfidsInfo/FlowCellExpirationdate"),
                ("SBS","/RunParameters/RfidsInfo/SbsPartNumber", "/RunParameters/RfidsInfo/SbsLotNumber", "/RunParameters/RfidsInfo/SbsExpirationdate"),
                ("Clustering","/RunParameters/RfidsInfo/ClusterPartNumber", "/RunParameters/RfidsInfo/ClusterLotNumber", "/RunParameters/RfidsInfo/ClusterExpirationdate"),
                ("Buffer","/RunParameters/RfidsInfo/BufferPartNumber", "/RunParameters/RfidsInfo/BufferLotNumber", "/RunParameters/RfidsInfo/BufferExpirationdate")
            }
        };

        static void write_info(string platformname, string tag, Dictionary<string, Dictionary<string, string>> info_dict)
        {
         
            string item_info;
            if (info_dict[platformname].TryGetValue(tag, out string item_info_path))
            {
                 item_info = doc.Getstring(item_info_path);
            }
            else
            {
                item_info = "Not available for this platform";
            }
            Main.frmMyDlg.parsedText.AppendText($"{tag}: {item_info}");
            Main.frmMyDlg.parsedText.AppendText(Environment.NewLine);
        }
        static DateTime get_startdate(string applicationname)
        {
        DateTime startdate;
            if (applicationname == "NextSeq 1000/2000")
            {
                startdate = DateTime.Parse(doc.SelectSingleNode("/RunParameters/RunStartTime").InnerText, enUs);
            }
            else if (applicationname == "iSeq")
            {
                startdate = DateTime.Parse(doc.SelectSingleNode("/RunParameters/RunStartDate").InnerText, enUs);
            }
            else
            {
                startdate= DateTime.ParseExact(doc.SelectSingleNode("/RunParameters/RunStartDate").InnerText,
                    "yyMMdd", CultureInfo.InvariantCulture);                 
            }
            return startdate;
        }

        static string get_platformname()
        {
            
            string get_platform(string app_name)
            {
                return app_name.Substring(0, app_name.IndexOf(' '));
            }
            XmlNode platformnode = doc.SelectSingleNode("//Application") ?? doc.SelectSingleNode("//ApplicationName");
            if (platformnode == null)
            { return null; }
            string applicationname = get_platform(platformnode.InnerText);

            if (applicationname== "NextSeq")
            {
                if (doc.SelectSingleNode("/RunParameters/FocusMethod") != null) //Nextseq500
                {applicationname += " 500/550";}
                else
                { applicationname += " 1000/2000";}
            }
            return applicationname;
        }

        static string Getstring(this XmlDocument xmlDocument, string xpath)
        {
            XmlNode node_find;
            node_find= xmlDocument.SelectSingleNode(xpath);
            string found_string;

            if (node_find == null)
            {
                found_string = "Cannot find";
            }
            else
            {
                found_string = node_find.InnerText;
            }
            return found_string;
        }

        static void write_expiry(string consumable, DateTime expiry_date, DateTime start_date)
        {
            string preposition;
            Color textcolor;
            double daystoexpiry = (expiry_date - start_date).Days;
            if (daystoexpiry < 0)
            {
                preposition = "pass";
                textcolor = Color.Red;
            }
            else
            {
                preposition = "to";
                textcolor = Color.Green;
            }
            Main.frmMyDlg.parsedText.AppendText("Expiry date: ");
            Main.frmMyDlg.parsedText.SelectionColor = textcolor;
            Main.frmMyDlg.parsedText.AppendText(expiry_date.ToString("dd-MMM-yyyy"));
            //Main.frmMyDlg.parsedText.AppendText($"{daystoexpiry.ToString()} days {preposition} expiry");
            Main.frmMyDlg.parsedText.AppendText(Environment.NewLine + Environment.NewLine);
        }

        internal static void ProcessXML()
        {
            Kbg.NppPluginNET.Main.myDockableDialog();

            int xml_length = editor.GetTextLength();
            string xml_string = editor.GetText(xml_length + 1);
            
            //notepad.FileNew();
            //editor.SetText(xml_string);
            try
            { doc.LoadXml(xml_string); }
            catch (XmlException)
            {
                
                Main.frmMyDlg.parsedText.Clear();                
                Main.frmMyDlg.parsedText.AppendText("Cannot parse");
                return;
            }
            
            string platformname = get_platformname();
            if (platformname ==null)
            {
                Main.frmMyDlg.parsedText.Clear();
                Main.frmMyDlg.parsedText.AppendText("Not a runparameters.xml file");
                return; 
            }
            DateTime parsed_start_date = get_startdate(platformname);
            DateTime parsed_expiry_date;
            string parsed_part;
            string parsed_lot;
            Main.frmMyDlg.parsedText.Clear();
            string runid_path = platformname == "NovaSeq" || platformname == "iSeq" ? "/RunParameters/RunId" : 
                platformname== "NextSeq 1000/2000" ? "/RunParameters/BaseSpaceRunId" : "/RunParameters/RunID";
            string runid;

            runid = doc.SelectSingleNode(runid_path).InnerText;

            Main.frmMyDlg.parsedText.AppendText("Platform: " + platformname);
            Main.frmMyDlg.parsedText.AppendText(Environment.NewLine);
            Main.frmMyDlg.parsedText.AppendText("ID: "+ runid);
            Main.frmMyDlg.parsedText.AppendText(Environment.NewLine);
            Main.frmMyDlg.parsedText.AppendText("Name: " + doc.SelectSingleNode("/RunParameters/ExperimentName").InnerText);
            Main.frmMyDlg.parsedText.AppendText(Environment.NewLine);
            Main.frmMyDlg.parsedText.AppendText("Start date: " + parsed_start_date.ToString("dd-MMM-yyyy"));
            Main.frmMyDlg.parsedText.AppendText(Environment.NewLine+ Environment.NewLine);

            foreach ((string con, string part, string lot, string xpath) i in consum_path[platformname])
            {
                parsed_part = doc.Getstring(i.part);
                parsed_lot = doc.Getstring(i.lot);

                    
                Font boldUnderFont = new Font(Main.frmMyDlg.parsedText.Font, FontStyle.Bold | FontStyle.Underline);
                Main.frmMyDlg.parsedText.SelectionFont = boldUnderFont;
                Main.frmMyDlg.parsedText.AppendText(i.con);
                Main.frmMyDlg.parsedText.AppendText(Environment.NewLine);
                Main.frmMyDlg.parsedText.AppendText("Part no: " + parsed_part);
                Main.frmMyDlg.parsedText.AppendText(Environment.NewLine);
                Main.frmMyDlg.parsedText.AppendText("Lot no: "+parsed_lot);
                Main.frmMyDlg.parsedText.AppendText(Environment.NewLine);

                XmlNode _con = doc.SelectSingleNode(i.xpath);
                if (_con == null)
                {
                    Main.frmMyDlg.parsedText.AppendText("Cannot find expiration date");
                    Main.frmMyDlg.parsedText.AppendText(Environment.NewLine + Environment.NewLine);
                }
                else
                {
                    try
                    {
                        parsed_expiry_date = DateTime.Parse(_con.InnerText, enUs);
                    }
                    catch (Exception)
                    {
                        Main.frmMyDlg.parsedText.AppendText("Cannot parse expiration date");
                        Main.frmMyDlg.parsedText.AppendText(Environment.NewLine + Environment.NewLine);
                        continue;
                    }
                    //daystoexpiry = (parsed_con - parsed_start_date).Days;
                    write_expiry(i.con, parsed_expiry_date, parsed_start_date);
                }


            }

            //Write additional info

            foreach (string i in Main.settings.checkedListBox1.CheckedItems)
            {
                write_info(platformname, i, add_info_dict);
            }
            
             //notepad.FileNew();
            //editor.SetText($"{parsed_flowcell.ToString()}");
        }

    }
}
