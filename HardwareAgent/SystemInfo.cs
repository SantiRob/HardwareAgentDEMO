using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;


namespace HardwareAgent
{
    public class SystemInfo
    {
        public string UserName { get; set; }
        public string MachineName { get; set; }
        public string MACAddress { get; set; }
        public string IPAddress { get; set; }
        public ulong TotalRAM { get; set; }
        public string CPUInfo { get; set; }
        public float CPUUsage { get; set; }



        public SystemInfo()
        {
            UserName = Environment.UserName;
            MachineName = Environment.MachineName;
            MACAddress = GetMACAddress();
            IPAddress = GetIPAddress();
            TotalRAM = GetTotalRAM();
            CPUInfo = GetCPUInfo();
            CPUUsage = GetCPUUsage();
        }

        public string GetSystemData()
        {
            var data = new StringBuilder();
            data.AppendLine("{");
            data.AppendLine($"\"UserName\": \"{UserName}\",");
            data.AppendLine($"\"MachineName\": \"{MachineName}\",");
            data.AppendLine($"\"MACAddress\": \"{MACAddress}\",");
            data.AppendLine($"\"IPAddress\": \"{IPAddress}\",");
            data.AppendLine($"\"TotalRAM\": {TotalRAM},");
            data.AppendLine($"\"CPUInfo\": \"{CPUInfo}\",");
            data.AppendLine($"\"CPUUsage\": {CPUUsage}");
            data.AppendLine("}");

            return data.ToString();
        }

        private string GetMACAddress()
        {
            var macAddresses = NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .ToList();

            return string.Join(", ", macAddresses);
        }
        private string GetIPAddress()
        {
            var hostName = Dns.GetHostName();
            var addresses = Dns.GetHostAddresses(hostName)
                .Where(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .Select(address => address.ToString())
                .ToList();

            return string.Join(", ", addresses);
        }
        private ulong GetTotalRAM()
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem"))
            {
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return Convert.ToUInt64(queryObj["TotalVisibleMemorySize"]) * 1024;
                }
            }

            return 0;
        }

        private string GetCPUInfo()
        {
            return Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER");
        }

        private float GetCPUUsage()
        {
            using (var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total"))
            {
                cpuCounter.NextValue();
                System.Threading.Thread.Sleep(1000);
                return cpuCounter.NextValue();
            }
        }
    }
}
