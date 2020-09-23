using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace TaskClone.src.helpers
{
    class helpers
    {
        public bool hasRowSelected(DataGridView dataGridVIew) {
            if (dataGridVIew.SelectedRows.Count > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        
        public int numberOfRunningProcess()
        {
            int total = 0;
            foreach (Process proc in Process.GetProcesses()) {
                total++;
            }
           return total;
        }

       

        public void killProccessApplications(string processName) {
           
            try
            {
                foreach (System.Diagnostics.Process process in Process.GetProcesses())
                {
                    if (process.MainWindowTitle == processName) {
                        process.Kill();
                    }
                    
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public void KillProcessByPid(int pid) {
            try {
                foreach (System.Diagnostics.Process process in Process.GetProcesses()) {
                    if (process.Id == pid) {
                        process.Kill();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }
    }
}
