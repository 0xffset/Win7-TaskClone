using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Management;
using System.Runtime.ExceptionServices;

namespace TaskClone.src
{
    class Performance
    {
        Process[] PROCESSES = Process.GetProcesses();
        public string getAmoungMemory()
        {
            return Convert.ToString(new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory / 1024 / 1024);
        }

        public string getAvailableMemory() {
            return (Convert.ToString(new Microsoft.VisualBasic.Devices.ComputerInfo().AvailablePhysicalMemory / 1024 / 1024));
        }

        public string getCachedMemory() {
            return (Convert.ToString(Convert.ToInt32(getAmoungMemory()) - (Convert.ToInt32(getAmoungMemory()) - Convert.ToInt32(getAvailableMemory())) - Convert.ToInt32(UInt64.Parse(getFreeMemory()))));
        }

        public string getFreeMemory() {
            ObjectQuery objectQuery = new System.Management.ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);
            ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
            var PhysicalMemoryInUse = Convert.ToInt32(getAmoungMemory()) - Convert.ToInt32(getAvailableMemory());
            Object Free = new object();
            foreach (var result in managementObjectCollection)
            {

                Free = result["FreePhysicalMemory"];
            }

            return Convert.ToString(Convert.ToInt32(UInt64.Parse(Free.ToString())));
        }

        public string getHandlesSystemCount() {
            int handlers = 0;
            foreach (Process process in PROCESSES) {
                handlers += process.HandleCount;
            }
            return Convert.ToString(handlers.ToString());

        }

        public string getThreadsSystemCount() {
            return Convert.ToString(System.Diagnostics.Process.GetCurrentProcess().Threads.Count);
        }

        public TimeSpan getUpTimeSystem() {
           using (var uptime = new PerformanceCounter("System", "System Up Time"))
            {
                uptime.NextValue();
                return TimeSpan.FromSeconds(uptime.NextValue());    
            }
        }

        public string getCommitSize() {
            float commintSize = 0;
            var process = Process.GetProcesses();
            foreach (Process proc in process) {
                commintSize += (proc.PagedMemorySize64 / 1024f) / 1024f;
            }

            return Convert.ToString(commintSize);
        }

        public string getPagedKernelMemory() {
            float pagedSize = 0;
            var process = Process.GetProcesses();
            foreach (Process proc in process)
            {
                pagedSize += (proc.PagedMemorySize64 / 1024f) / 1024f;
            }

            return Convert.ToString(pagedSize);
        }
    }
}
