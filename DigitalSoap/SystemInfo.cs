using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace DigitalSoap
{
    public class OSInfo
    {
        public string Name { get; }

        public string Version { get; }

        public string Architecture { get; }

        public string SerialNumber { get; }

        public string Build { get; }

        public uint MaxProcessCount { get; }

        public ulong MaxMemorySize { get; }

        public DateTime InstallDate { get; }

        public OSInfo()
        {
            ManagementObject OS = new ManagementObjectSearcher(
                "SELECT * FROM Win32_OperatingSystem").Get().Cast<ManagementObject>().First();

            this.Name = OS["Caption"].ToString().Trim();
            this.Version = OS["Version"].ToString();
            this.Architecture = OS["OSArchitecture"].ToString();
            this.SerialNumber = OS["SerialNumber"].ToString();
            this.Build = OS["BuildNumber"].ToString();

            this.MaxProcessCount = (uint)OS["MaxNumberOfProcesses"];
            this.MaxMemorySize = (ulong)OS["MaxProcessMemorySize"];

            string DateData = (string)OS["InstallDate"];
            this.InstallDate = new DateTime(int.Parse(DateData.Substring(0, 4)),
                int.Parse(DateData.Substring(4, 2)), int.Parse(DateData.Substring(6, 2)));
        }

        public string GetOSName()
        {
            return SystemInfo.OperatingSystem.Name + " "
                + SystemInfo.OperatingSystem.Architecture;
        }
    }

    public class CPUInfo
    {
        public string ID { get; }

        public string Socket { get; }

        public string Name { get; }

        public string Description { get; }

        public ushort DataWidth { get; }

        public ushort Architecture { get; }

        public uint SpeedMHz { get; }

        public uint BusSpeedMHz { get; }

        public uint L2CacheSize { get; }

        public uint L3CacheSize { get; }

        public uint Cores { get; }

        public uint Threads { get; }

        public UInt16 Load => GetCurrentLoad();
        private UInt16 LastLoadValue = 0;

        private ManagementObject CPUObject;

        public CPUInfo()
        {
            CPUObject = new ManagementObjectSearcher(
                "SELECT * FROM Win32_Processor").Get().Cast<ManagementObject>().First();

            this.ID = CPUObject["ProcessorID"].ToString();
            this.Socket = CPUObject["SocketDesignation"].ToString();
            this.Name = CPUObject["Name"].ToString().Replace("(R)", "").Replace("(TM)", "");
            this.Description = CPUObject["Caption"].ToString();

            this.DataWidth = (ushort)CPUObject["DataWidth"];
            this.Architecture = (ushort)CPUObject["Architecture"];
            this.SpeedMHz = (uint)CPUObject["MaxClockSpeed"];
            this.BusSpeedMHz = (uint)CPUObject["ExtClock"];
            this.L2CacheSize = (uint)CPUObject["L2CacheSize"] * (uint)1024;
            this.L3CacheSize = (uint)CPUObject["L3CacheSize"] * (uint)1024;
            this.Cores = (uint)CPUObject["NumberOfCores"];
            this.Threads = (uint)CPUObject["NumberOfLogicalProcessors"];
        }

        private UInt16 GetCurrentLoad()
        {
            Thread T = new Thread(() =>
            {
                CPUObject = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_Processor").Get().Cast<ManagementObject>().First();

                if (CPUObject["LoadPercentage"] != null)
                    LastLoadValue = (UInt16)CPUObject["LoadPercentage"];
            });
            T.Start();

            return LastLoadValue;
        }
    }

    public class RAMInfo
    {
        public uint Capacity { get; }

        public uint Speed { get; }

        public UInt16 Other { get; }

        public RAMInfo()
        {
            ManagementObject[] RAM = new ManagementObjectSearcher(
                "SELECT * FROM Win32_PhysicalMemory").Get().Cast<ManagementObject>().ToArray();

            this.Capacity = (uint)((UInt64)(RAM[0]["Capacity"])
                / 1024.0f / 1024.0f / 1024.0f);
            this.Capacity += (uint)((UInt64)(RAM[1]["Capacity"])
                / 1024.0f / 1024.0f / 1024.0f);

            this.Speed = (uint)(RAM[0]["Speed"]);

            this.Other = (UInt16)(RAM[0]["TotalWidth"]);
        }
    }

    public class GPUInfo
    {
        public string Name { get; }
        
        public string VideoProcessor { get; }

        public uint Memory { get; }

        public GPUInfo()
        {
            ManagementObject GPU = new ManagementObjectSearcher(
                "SELECT * FROM Win32_VideoController").Get().Cast<ManagementObject>().First();

            this.Name = GPU["Name"].ToString();
            this.VideoProcessor = GPU["VideoProcessor"].ToString();

            this.Memory = (uint)GPU["AdapterRAM"] / 1024 / 1024;
        }
    }

    public static class SystemInfo
    {
        public static OSInfo OperatingSystem { get; } = new OSInfo();
        
        public static CPUInfo Processor { get; } = new CPUInfo();

        public static RAMInfo Memory { get; } = new RAMInfo();

        public static GPUInfo GraphicsDevice { get; } = new GPUInfo();

        public static string GetSystemInfoString()
        {
            string SystemStats = "";
            string PhysMem = Math.Ceiling((float)Memory.Capacity).ToString("N2");
            string AvailMem = (((float)Memory.Capacity) - 0.284f).ToString("N2");
            string GPUMem = Math.Ceiling((float)SystemInfo.GraphicsDevice.Memory / 1024).ToString("N2") + " GB VRAM";

            SystemStats = SystemInfo.OperatingSystem.GetOSName() + " (Admin)\n" +
                 PhysMem + " GB RAM @ " + SystemInfo.Memory.Speed.ToString() + " MHz ("
                + AvailMem + " GB Available)\n" + SystemInfo.Processor.Name + " ("
                + SystemInfo.Processor.Cores + " Cores @ " + SystemInfo.Processor.SpeedMHz.ToString() + " MHz)\n"
                + SystemInfo.GraphicsDevice.Name + " (" + GPUMem + ")";

            return SystemStats;
        }
    }
}
