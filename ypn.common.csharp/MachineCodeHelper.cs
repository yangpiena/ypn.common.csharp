/**
* 命名空间： ypn.common.csharp
*
* 功    能： 机器码辅助类
* 类    名： MachineCodeHelper
*
* 版本  变更日期            负责人   变更内容
* ───────────────────────────────────
* V0.01 2018-11-21 22:56:02 YPN      初版
*
* Copyright (c) 2018 Fimeson. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：宁夏菲麦森流程控制技术有限公司 　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace ypn.common.csharp
{
    public class MachineCodeHelper
    {
        /// <summary>
        /// 获取机器码
        /// 机器码 = CPU序列号 + 硬盘ID + 网卡地址
        /// </summary>
        /// <returns></returns>
        public static string GetMachineCode()
        {
            return GetCPUInfo() + GetHDId() + GetMACAddress();
        }
        
        /// <summary>
        /// 获取cpu序列号
        /// </summary>
        /// <returns></returns>
        public static string GetCPUInfo()
        {
            string cpuInfo = "";
            try
            {
                using (ManagementClass cimobject = new ManagementClass("Win32_Processor"))
                {
                    ManagementObjectCollection moc = cimobject.GetInstances();

                    foreach (ManagementObject mo in moc)
                    {
                        cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return cpuInfo;
        }

        /// <summary>
        /// 获取硬盘ID 
        /// </summary>
        /// <returns></returns>
        public static string GetHDId()
        {
            string HDid = "";
            try
            {
                using (ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive"))
                {
                    ManagementObjectCollection moc1 = cimobject1.GetInstances();
                    foreach (ManagementObject mo in moc1)
                    {
                        HDid = (string)mo.Properties["Model"].Value;
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return HDid;
        }

        /// <summary>
        /// 获取网卡硬件地址 
        /// </summary>
        /// <returns></returns>
        public static string GetMACAddress()
        {
            string macAddress = "";
            try
            {
                using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
                {
                    ManagementObjectCollection moc2 = mc.GetInstances();
                    foreach (ManagementObject mo in moc2)
                    {
                        if ((bool)mo["IPEnabled"] == true)
                            macAddress = mo["MacAddress"].ToString();
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return macAddress;
        }

        /// <summary>
        /// 获取操作系统版本(PC,PDA均支持)
        /// </summary>
        /// <returns></returns>
        public static string GetOSVersion()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();
            return name != null ? name.ToString() : "Unknown";

            //return Environment.OSVersion.Version.ToString();
        }
    }
}
