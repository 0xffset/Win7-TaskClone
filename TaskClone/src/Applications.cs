using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms.VisualStyles;
using System.Security;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TaskClone.src
{
    class Applications
    {
        /* Global variables*/ 
        Process[] PROCESSES = Process.GetProcesses();
        public Dictionary<string, string> get_task() {
           
         
            Dictionary<string, string> output = new Dictionary<string, string>();
            List<Icon> icons = new List<Icon>();
            
            foreach (Process proc in PROCESSES) {
                if (!String.IsNullOrEmpty(proc.MainWindowTitle)) 
                {   string status = "Running";
                    if (!proc.Responding) {
                        status = "Not Responding";
                    }
                    Icon icon = Icon.ExtractAssociatedIcon(proc.MainModule.FileName);
                    icons.Add(icon);
                    output.Add(proc.MainWindowTitle, status);
                }
            }

            return output;
        }

        public bool NewTask(string pathExe) {
            try
            {
                Process.Start(pathExe);
                return true;

            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
        }
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr intPtr, int nCmdShow);
        public void SwitchToApp(string mainWindowTitle) {
            try 
            {
                foreach (Process proc in PROCESSES) {
                    ShowWindow(proc.MainWindowHandle, 2);
                }

                foreach (Process process in PROCESSES) {
                    if (process.MainWindowTitle.ToUpper().Contains(mainWindowTitle.ToUpper())) {
                        ShowWindow(process.MainWindowHandle, 9);
                        
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    } 
}
