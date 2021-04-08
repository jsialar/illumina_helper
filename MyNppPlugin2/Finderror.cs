using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kbg.NppPluginNET.PluginInfrastructure;
using System.ComponentModel;
using System.Threading;

namespace Kbg.NppPluginNET
{
    static class Finderror
    {
        
        public static IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());
        public static List<int> foundpos_l = new List<int>();
        static List<string> foundstring_l = new List<string>();
        static FindOption searchflags = FindOption.REGEXP | FindOption.CXX11REGEX | FindOption.MATCHCASE;

        internal static void execute()
        {
            Main.finderror_disp();
            Main.finderror_form.listbox_findresults.Items.Clear();
            if (Main.finderror_form.backgroundWorker1.IsBusy)
            {
                Main.finderror_form.backgroundWorker1.CancelAsync();
                Main.finderror_form.GenerateBGW();

            }
            Main.finderror_form.backgroundWorker1.RunWorkerAsync();
        }

        internal static void find(BackgroundWorker worker, DoWorkEventArgs e)
        {
            int foundpos;
            foundpos_l.Clear();
            foundstring_l.Clear();
            int foundline;
            int cpmin = 0;
            int cpmax = editor.GetLength();
            string searchstring = Main.settings.options.checkedList_finderror_str;
            if (searchstring == "")
            { return; }
            CharacterRange chrRange = new CharacterRange(cpmin, cpmax);
            TextToFind ttf = new TextToFind(chrRange, searchstring);
            while (true)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    foundpos = editor.FindText(searchflags, ttf);
                    if (foundpos != -1)
                    {
                        foundline = editor.LineFromPosition(foundpos);
                        foundpos_l.Add(foundline);
                        foundstring_l.Add(editor.GetLine(foundline));
                        ttf.chrg = new CharacterRange(foundpos + 1, cpmax);
                    }
                    else
                    {
                        break;
                    }
                }
                
            }

            ttf.Dispose();
            
            //updatedisplay();

        }

        internal static void updatedisplay()
        {
            Main.finderror_form.listbox_findresults.Items.Clear();
            if (foundstring_l.Count > 0)
            {
                Main.finderror_form.listbox_findresults.Items.AddRange(foundstring_l.ToArray());
            }
            else
            {
                Main.finderror_form.listbox_findresults.Items.Add("Nothing found");
            }

            
        }
    }
}
