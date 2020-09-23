using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskClone.src
{
    class Users
    {
        public Dictionary<string, object> getListuser() {
            string computer = Environment.MachineName.ToString();
            string domain = $"Domain='{computer}'";
            SelectQuery selectQuery = new SelectQuery("win32_UserAccount", domain);
            Dictionary<string, object> user = new Dictionary<string, object>();
            try {
                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(selectQuery);
                foreach (ManagementObject managementObject in managementObjectSearcher.Get()) {
                    string[] obj = new string[] {
                        managementObject["name"].ToString(),
                    };

                    user.Add(managementObject["name"].ToString(), obj);
                }
                return user;
            }
            catch (Exception ) {
                throw;
                
            }
            
        }
    }
}
