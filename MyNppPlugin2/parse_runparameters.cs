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


namespace Kbg.NppPluginNET
{
    static class parse_runparameters
    {

        static IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());
        static INotepadPPGateway notepad = new NotepadPPGateway();

        static XmlDocument doc = new XmlDocument();
        static CultureInfo enUs = new CultureInfo("en-US");

        static string serial_path(string con)
        { return $"/RunParameters/{con}/SerialNumber"; }

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

            static Dictionary<string, (string con, Dictionary<string, string> dict)[]> consum_dict = new Dictionary<string, (string con, Dictionary<string, string> dict)[]>
            {
            ["MiSeq"] = new[] { 
                ("Flowcell", new Dictionary<string, string> {
                    ["Serial#"]=serial_path("SerialNumber"),
                    ["Lot#"]=lot_path("FlowcellRFIDTag"),
                    ["Part#"]=part_path("FlowcellRFIDTag"),
                    ["Expiry date"]=exp_path("FlowcellRFIDTag")
                }),
                ("PR2", new Dictionary<string, string> {
                    ["Serial#"]=serial_path("PR2BottleRFIDTag"),
                    ["Lot#"]=lot_path("PR2BottleRFIDTag"),
                    ["Part#"]=part_path("PR2BottleRFIDTag"),
                    ["Expiry date"]=exp_path("PR2BottleRFIDTag")
                }),
                ("ReagentKit", new Dictionary<string, string> {
                    ["Serial#"]=serial_path("ReagentKitRFIDTag"),
                    ["Lot#"]=lot_path("ReagentKitRFIDTag"),
                    ["Part#"]=part_path("ReagentKitRFIDTag"),
                    ["Expiry date"]=exp_path("ReagentKitRFIDTag")
                })},
            ["iSeq"] = new[] {
                ("Flowcell", new Dictionary<string, string> {
                    ["Serial#"]=serial_path("FlowcellEEPROMTag"),
                    ["Lot#"]=lot_path("FlowcellEEPROMTag"),
                    ["Part#"]=part_path("FlowcellEEPROMTag"),
                    ["Expiry date"]=exp_path("FlowcellEEPROMTag")
                }),
                ("ReagentKit", new Dictionary<string, string> {
                    ["Serial#"]=serial_path("ReagentKitRFIDTag"),
                    ["Lot#"]=lot_path("ReagentKitRFIDTag"),
                    ["Part#"]=part_path("ReagentKitRFIDTag"),
                    ["Expiry date"]=exp_path("ReagentKitRFIDTag")
                })},
            ["MiniSeq"] = new[] {
                ("Flowcell", new Dictionary<string, string> {
                    ["Serial#"]=serial_path("FlowCellRfidTag"),
                    ["Lot#"]=lot_path("FlowCellRfidTag"),
                    ["Part#"]=part_path("FlowCellRfidTag"),
                    ["Expiry date"]=exp_path("FlowCellRfidTag")
                }),
                ("ReagentKit", new Dictionary<string, string> {
                    ["Serial#"]=serial_path("ReagentKitRfidTag"),
                    ["Lot#"]=lot_path("ReagentKitRfidTag"),
                    ["Part#"]=part_path("ReagentKitRfidTag"),
                    ["Expiry date"]=exp_path("ReagentKitRfidTag")
                })},
            ["NextSeq 500/550"] = new[] {
                  ("Flowcell", new Dictionary<string, string> {
                    ["Serial#"]=serial_path("FlowCellRfidTag"),
                    ["Lot#"]=lot_path("FlowCellRfidTag"),
                    ["Part#"]=part_path("FlowCellRfidTag"),
                    ["Expiry date"]=exp_path("FlowCellRfidTag")
                }),
                ("PR2", new Dictionary<string, string> {
                    ["Serial#"]=serial_path("PR2BottleRfidTag"),
                    ["Lot#"]=lot_path("PR2BottleRfidTag"),
                    ["Part#"]=part_path("PR2BottleRfidTag"),
                    ["Expiry date"]=exp_path("PR2BottleRfidTag")
                }),
                ("ReagentKit", new Dictionary<string, string> {
                    ["Serial#"]=serial_path("ReagentKitRfidTag"),
                    ["Lot#"]=lot_path("ReagentKitRfidTag"),
                    ["Part#"]=part_path("ReagentKitRfidTag"),
                    ["Expiry date"]=exp_path("ReagentKitRfidTag")
                })},
            ["NextSeq 1000/2000"] = new[] {
                  ("Flowcell", new Dictionary<string, string> {
                    ["Serial#"]="/RunParameters/FlowCellSerialNumber",
                    ["Lot#"]="/RunParameters/FlowCellLotNumber",
                    ["Part#"]="/RunParameters/FlowCellPartNumber",
                    ["Expiry date"]="/RunParameters/FlowCellExpirationDate"
                }),
                   ("Cartridge", new Dictionary<string, string> {
                    ["Serial#"]="/RunParameters/CartridgeSerialNumber",
                    ["Lot#"]="/RunParameters/CartridgeLotNumber",
                    ["Part#"]="/RunParameters/CartridgePartNumber",
                    ["Expiry date"]="/RunParameters/CartridgeExpirationDate"
                })},
            ["NovaSeq"] = new[] {
                ("Flowcell", new Dictionary<string, string> {
                    ["Serial#"]="/RunParameters/RfidsInfo/FlowCellSerialBarcode",
                    ["Lot#"]="/RunParameters/RfidsInfo/FlowCellLotNumber",
                    ["Part#"]="/RunParameters/RfidsInfo/FlowCellPartNumber",
                    ["Expiry date"]="/RunParameters/RfidsInfo/FlowCellExpirationdate"
                }),
                ("SBS", new Dictionary<string, string> {
                    ["Serial#"]="/RunParameters/RfidsInfo/SbsSerialBarcode",
                    ["Lot#"]="/RunParameters/RfidsInfo/SbsLotNumber",
                    ["Part#"]="/RunParameters/RfidsInfo/SbsPartNumber",
                    ["Expiry date"]="/RunParameters/RfidsInfo/SbsExpirationdate"
                }),
                ("Clustering", new Dictionary<string, string> {
                    ["Serial#"]="/RunParameters/RfidsInfo/ClusterSerialBarcode",
                    ["Lot#"]="/RunParameters/RfidsInfo/ClusterLotNumber",
                    ["Part#"]="/RunParameters/RfidsInfo/ClusterPartNumber",
                    ["Expiry date"]="/RunParameters/RfidsInfo/ClusterExpirationdate"
                }),
                 ("Buffer", new Dictionary<string, string> {
                    ["Serial#"]="/RunParameters/RfidsInfo/BufferSerialBarcode",
                    ["Lot#"]="/RunParameters/RfidsInfo/BufferLotNumber",
                    ["Part#"]="/RunParameters/RfidsInfo/BufferPartNumber",
                    ["Expiry date"]="/RunParameters/RfidsInfo/BufferExpirationdate"
                })}
        };

        static void write_con(DateTime parsed_start_date, (string con, Dictionary<string, string> dict)[] platformarray)
        {
            foreach (var i in platformarray)
            {
                Font boldUnderFont = new Font(Main.frmMyDlg.parsedText.Font, FontStyle.Bold | FontStyle.Underline);
                Main.frmMyDlg.parsedText.SelectionFont = boldUnderFont;
                Main.frmMyDlg.parsedText.AppendText(i.con);
                Main.frmMyDlg.parsedText.AppendText(Environment.NewLine);
                foreach (string tag in Main.settings.options.checkedList_runparam_con)
                {
                    if (tag=="Expiry date")
                    {
                        XmlNode _con = doc.SelectSingleNode(i.dict[tag]);
                        DateTime parsed_expiry_date;
                        if (_con == null)
                        {
                            Main.frmMyDlg.parsedText.AppendText("Cannot find expiration date");
                            Main.frmMyDlg.parsedText.AppendText(Environment.NewLine);

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
                                Main.frmMyDlg.parsedText.AppendText(Environment.NewLine);
                                continue;
                            }
                            //daystoexpiry = (parsed_con - parsed_start_date).Days;
                            write_expiry(i.con, parsed_expiry_date, parsed_start_date);
                        }

                    }
                    else
                    {
                        write_info(tag, i.dict, false);
                    }
                    
                }
            }
        }
        static void write_info(string tag, Dictionary<string, string> platform_dict, bool literal)
        {
         
            string item_info;
            if (platform_dict.TryGetValue(tag, out string item_info_path))
            {
                 item_info = literal? item_info_path: doc.Getstring(item_info_path);
            }
            else
            {
                item_info = "Not available";
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
            string exp_data_string = daystoexpiry < 0 ? expiry_date.ToString("dd-MMM-yyyy") + " (expired)" : expiry_date.ToString("dd-MMM-yyyy");
            Main.frmMyDlg.parsedText.AppendText(exp_data_string);
            //Main.frmMyDlg.parsedText.AppendText($"{daystoexpiry.ToString()} days {preposition} expiry");
            Main.frmMyDlg.parsedText.AppendText(Environment.NewLine);
        }

        internal static void ProcessXML()
        {
            Main.runparameters_disp();

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
            Main.frmMyDlg.parsedText.Clear();
            string runid_path = platformname == "NovaSeq" || platformname == "iSeq" ? "/RunParameters/RunId" : 
                platformname== "NextSeq 1000/2000" ? "/RunParameters/BaseSpaceRunId" : "/RunParameters/RunID";

            var header_dict = new Dictionary<string, string>
            {
                {"Platform", platformname},
                {"RunID", doc.SelectSingleNode(runid_path).InnerText},
                {"ExperimentName", doc.SelectSingleNode("/RunParameters/ExperimentName").InnerText},
                {"StartDate",  parsed_start_date.ToString("dd-MMM-yyyy")}
            };
            
            //Write header
            foreach (string tag in Main.settings.options.checkedList_runparam_header)
            {
                write_info(tag, header_dict, true);
            }
            Main.frmMyDlg.parsedText.AppendText(Environment.NewLine);

            //Write consumables
            write_con(parsed_start_date, consum_dict[platformname]);
            Main.frmMyDlg.parsedText.AppendText(Environment.NewLine);

            //Write additional info
            foreach (string i in Main.settings.options.checkedList_runparam_additional)
            {
                write_info(i, add_info_dict[platformname], false);
            }
            
             //notepad.FileNew();
            //editor.SetText($"{parsed_flowcell.ToString()}");
        }


    }
}
