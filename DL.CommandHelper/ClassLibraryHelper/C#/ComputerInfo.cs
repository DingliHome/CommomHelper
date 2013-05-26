using System;
using System.Management;

namespace ClassLibraryHelper.C_
{
    /// <summary>
    /// 电脑硬件信息
    /// </summary>
    public class ComputerInfo
    {
        public string CpuId;
        public int CpuCount;
        public string MacAddress;
        public string DiskId;
        public string IpAddress;
        public string LoginUserName;
        public string ComputerName;
        public string SystemType;
        public string TotalPhysicalMemory; //单位：M
        private static ComputerInfo _instance;
        public static ComputerInfo Instance()
        {
            if (_instance == null)
                _instance = new ComputerInfo();
            return _instance;
        }
        public ComputerInfo()
        {
            CpuId = GetCpuId();
            CpuCount = GetCpuCount();
            MacAddress = GetMacAddress();
            DiskId = GetDiskId();
            IpAddress = GetIpAddress();
            LoginUserName = GetUserName();
            SystemType = GetSystemType();
            TotalPhysicalMemory = GetTotalPhysicalMemory();
            ComputerName = GetComputerName();
        }

        static string GetCpuId()
        {
            try
            {
                //获取CPU序列号代码
                string cpuInfo = "";//cpu序列号
                var mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }

        }

        static int GetCpuCount()
        {
            try
            {
                //获取CPU个数
                var mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                return moc.Count;
            }
            catch
            {
                return 0;
            }
        }

        static string GetMacAddress()
        {
            try
            {
                //获取网卡硬件地址
                string mac = "";
                var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
        }

        static string GetIpAddress()
        {
            try
            {
                //获取IP地址
                string st = "";
                var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        //st=mo["IpAddress"].ToString();
                        var ar = (Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
        }

        static string GetDiskId()
        {
            try
            {
                //获取硬盘ID
                var hDid = string.Empty;
                var mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    hDid = (string)mo.Properties["Model"].Value;
                }
                moc = null;
                mc = null;
                return hDid;
            }
            catch
            {
                return "unknow";
            }
        }
        /// <summary>
        /// 操作系统的登录用户名
        /// </summary>
        /// <returns></returns>
        static string GetUserName()
        {
            try
            {
                string st = "";
                var mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {

                    st = mo["UserName"].ToString();

                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
        }

        /// <summary>
        /// PC类型
        /// </summary>
        /// <returns></returns>
        static string GetSystemType()
        {
            try
            {
                string st = string.Empty;
                var mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {

                    st = mo["SystemType"].ToString();

                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
        }
        /// <summary>
        /// 物理内存
        /// </summary>
        /// <returns></returns>
        static string GetTotalPhysicalMemory()
        {
            try
            {

                string st = string.Empty;
                var mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {

                    st = mo["TotalPhysicalMemory"].ToString();

                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
        }
        /// <summary>
        /// 电脑名
        /// </summary>
        /// <returns></returns>
        static string GetComputerName()
        {
            try
            {
                return Environment.GetEnvironmentVariable("ComputerName");
            }
            catch
            {
                return "unknow";
            }
        }
    }
}
