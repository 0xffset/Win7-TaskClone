using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Security;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;
namespace TaskClone.src
{
    class Services
    {
        ServiceController[] SERVICES = ServiceController.GetServices();
        Process[] PROCESSES = Process.GetProcesses();
        public Dictionary<string, object> getServices()
        {
            Dictionary<string, object> services = new Dictionary<string, object>();
            int counter = 0;
            string description = "";
            foreach (ServiceController service in SERVICES)
            {

                string path = string.Format("Win32_Service.Name='{0}'", service.ServiceName);
                using (ManagementObject ad = new ManagementObject(new ManagementPath(path)))
                {
                    description = (string)ad["Description"];
                }

                try
                {
                    string[] objectService = new string[] {
                        service.ServiceName.ToString(),
                        Convert.ToString(GetServiceId(service.ServiceName)),
                        description,
                        service.Status.ToString()
                     };


                    services.Add(counter.ToString(), objectService);
                    description = "";
                    counter++;
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            return services;
        }

        private uint GetServiceId(string serviceName)
        {

            uint id = 0;
            string query = "SELECT PROCESSID FROM WIN32_SERVICE WHERE NAME= '" + serviceName + "'";
            System.Management.ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (System.Management.ManagementObject @object in searcher.Get())
            {
                id = (uint)@object["PROCESSID"];
            }
            return id;
        }
    }
}
