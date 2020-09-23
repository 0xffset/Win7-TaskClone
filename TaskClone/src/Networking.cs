using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace TaskClone.src
{
    class Networking
    {
        public Dictionary<string, object> getNetworkAdapters() {
            Dictionary<string, object> nic = new Dictionary<string, object>();
            
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces()) {
            string[] objnic = new string[] {
              // GetNetworkUtilization(networkInterface.Description).ToString()
               "ts",
                Convert.ToString( networkInterface.Speed / 1e+6) + " Mbps",
                networkInterface.OperationalStatus.ToString() == "Up" ? "Connected" : "Not connected",
                };
                nic.Add(networkInterface.Name, objnic);
            }
            return nic;
        }
        // https://stackoverflow.com/questions/31957943/how-do-i-get-the-information-of-network-utilization-link-speed-and-state-of-ad

        /// <summary> Calculate network utilization </summary>
        /// <param name="networkCard">Description of network card from NetworkInterface</param>
        /// <returns>NetworkUtilization</returns>
        public static double GetNetworkUtilization(string networkCard)
        {
            const int numberOfIterations = 10;

            // Condition networkCard;
            networkCard = networkCard.Replace("\\", "_");
            networkCard = networkCard.Replace("/", "_");
            networkCard = networkCard.Replace("(", "[");
            networkCard = networkCard.Replace(")", "]");
            networkCard = networkCard.Replace("#", "_");

            var bandwidthCounter = new PerformanceCounter("Network Interface", "Current Bandwidth", networkCard);

            var bandwidth = bandwidthCounter.NextValue() > 0 ?  bandwidthCounter.NextValue() : 0;

            var dataSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", networkCard);
            var dataReceivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", networkCard);

            float sendSum = 0;
            float receiveSum = 0;

            for (var index = 0; index < numberOfIterations; index++)
            {
                sendSum += dataSentCounter.NextValue();
                receiveSum += dataReceivedCounter.NextValue();
            }

            var dataSent = sendSum;
            var dataReceived = receiveSum;

            double utilization = (8 * (dataSent + dataReceived)) / (bandwidth * numberOfIterations) * 100;

            return utilization;
        }

    }
}
