using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Security;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace TaskClone.src
{
    
    class Processes
    {
        Process[] PROCESSES = Process.GetProcesses();
        public Dictionary<string, object> getProcesses() {
            Dictionary<string, object> processes = new Dictionary<string,object>();
            string description = "";
            foreach (Process proc in PROCESSES) {
               
                
                try
                {
                    if (proc.MainModule.FileVersionInfo.FileDescription == null) {
                        description = " ";
                    }
                    else {
                        description = proc.MainModule.FileVersionInfo.FileDescription;
                    }
                    string[] ob = new string[]{
                             proc.ProcessName, proc.Id.ToString(),
                             proc.MachineName,
                             proc.TotalProcessorTime.ToString(),
                             Convert.ToString(proc.PagedMemorySize64/1024),
                             description
                           
                           };
                    processes.Add(proc.Id.ToString(), ob);
                    
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Console.WriteLine(processes);
            return processes;
           
        }

        
    }
}
