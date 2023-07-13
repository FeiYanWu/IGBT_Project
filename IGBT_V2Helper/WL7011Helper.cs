using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IGBT_V2Helper.IGBStructs;

namespace IGBT_V2Helper
{
    //GP
    public class WL7011Helper : IDisposable
    {
        private static uint Handle = 0;
        public string IP;
        public WL7011Helper(string ip)
        {
            this.IP = ip;
            ConnectWL7011();
        }
        public bool SetIGBTPara(ref igbt_fix_para_t igbt_fix_para)
        {
            int errCode = WLIGBTHelper.cbb_para_set_para(Handle, ref igbt_fix_para);
            if (errCode != 0)
            {
                MessageBox.Show($"设置系统管理板参数错误,错误代码：{errCode}");
                return false;
            }
            return true;
        }

        public bool ClearFault()
        {
            int errCode = WLIGBTHelper.cbb_fault_clear(Handle);
            if (errCode != 0)
            {
                MessageBox.Show($"清除高压脉冲板故障错误：{errCode}");
                return false;
            }
            return true;
        }
        private static Dictionary<int, string> firstLevelFaultDic = new Dictionary<int, string>
        {
            {0,"栅极保护板-FirstLevel-外部一级故障"},
            {1,"栅极保护板-FirstLevel-GE0检测电压超限"},
            {2,"栅极保护板-FirstLevel-GE1检测电压超限"},
            {3,"栅极保护板-FirstLevel-GE2检测电压超限"},
            {4,"栅极保护板-FirstLevel-GE3检测电压超限"},
            {5,"栅极保护板-FirstLevel-GE4检测电压超限"},
            {6,"栅极保护板-FirstLevel-GE5检测电压超限"},
            {7,"栅极保护板-FirstLevel-GE6检测电压超限"},
            {8,"栅极保护板-FirstLevel-GE7检测电压超限"},
            {9,"预留"},
            {10,"预留"},
            {11,"预留"},
            {12,"预留"},
            {13,"预留"},
            {14,"预留"},
            {15,"预留"},
        };
        private static Dictionary<int, string> secondLevelFaultDic = new Dictionary<int, string>
        {
            {0,"预留"},
            {1,"预留"},
            {2,"预留"},
            {3,"预留"},
            {4,"预留"},
            {5,"预留"},
            {6,"预留"},
            {7,"预留"},
            {8,"预留"},
            {9,"预留"},
            {10,"预留"},
            {11,"预留"},
            {12,"预留"},
            {13,"预留"},
            {14,"预留"},
            {15,"预留"},
        };
        private static Dictionary<int, string> softFaultCodeDic = new Dictionary<int, string>
        {
            {0,"485通讯超时"},
            {1,"DMA超时"},
            {2,"结果超时"},
            {3,"结果错误"},
            {4,"流程故障"},
        };
        public static string[] GetFirstLevelFault(fault_info_t faultInfo)
        {
            List<string> faultList = new List<string>();
            uint hwFault1 = faultInfo.hw_fault1;
            if (hwFault1 != 0)
            {
                for (int i = 0; i < 32; i++)
                {
                    if ((hwFault1 & (1 << i)) != 0)
                    {
                        faultList.Add(firstLevelFaultDic[i]);
                    }
                }
            }
            return faultList.ToArray();
        }
        public static string[] GetSecondLevelFault(fault_info_t faultInfo)
        {
            List<string> faultList = new List<string>();
            uint hwFault2 = faultInfo.hw_fault2;
            if (hwFault2 != 0)
            {
                for (int i = 0; i < 32; i++)
                {
                    if ((hwFault2 & (1 << i)) != 0)
                    {
                        faultList.Add(secondLevelFaultDic[i]);
                    }
                }
            }
            return faultList.ToArray();
        }
        public static string[] GetSoftFault(fault_info_t faultInfo)
        {
            soft_fault_t[] softFaultList = faultInfo.soft_fault;

            List<string> faultList = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                if (softFaultList[i].status == 1)
                {
                    string errorMessage = "开尔文板-SOFTFAULT-软件故障类型:" + softFaultCodeDic[i];
                    uint errorTestItem = (softFaultList[i].code >> 24);

                    uint mask16 = 0xFF0000;
                    uint processState = (softFaultList[i].code & mask16) >> 16;

                    uint mask8 = 0xFF00; // Mask with 1s in bits 8 to 15
                    uint processFaultType = (softFaultList[i].code & mask8) >> 8;

                    byte faultParam = (byte)softFaultList[i].code;
                    errorMessage = errorMessage + "，测试项：" + Enum.GetName(typeof(TestItemsEnum), errorTestItem)
                        + ",流程状态:" + processState + ",流程故障类型:" + processFaultType + ",故障参数:" + faultParam;
                    faultList.Add(errorMessage);
                }
            }
            return faultList.ToArray();
        }

        public  string[] GetAllFaultMessage()
        {
            fault_info_t faultInfo = new fault_info_t();
            int errorCode = WLIGBTHelper.cbb_fault_get_current_fault_info(Handle, ref faultInfo);

            if (errorCode != 0)
            {
                MessageBox.Show("获取当前故障失败，错误码：" + errorCode);
                return null;
            }
            List<string> faultList = new List<string>();

            uint hwFault1 = faultInfo.hw_fault1;
            if (hwFault1 != 0)
            {
                for (int i = 0; i < 32; i++)
                {
                    if ((hwFault1 & (1 << i)) != 0)
                    {
                        faultList.Add(firstLevelFaultDic[i]);
                    }
                }
            }

            uint hwFault2 = faultInfo.hw_fault2;
            if (hwFault2 != 0)
            {
                for (int i = 0; i < 32; i++)
                {
                    if ((hwFault2 & (1 << i)) != 0)
                    {
                        faultList.Add(secondLevelFaultDic[i]);
                    }
                }
            }

            soft_fault_t[] softFaultList = faultInfo.soft_fault;
            for (int i = 0; i < 5; i++)
            {
                if (softFaultList[i].status == 1)
                {
                    string errorMessage = "栅极保护板-SOFTFAULT-软件故障类型:" + softFaultCodeDic[i];
                    uint errorTestItem = (softFaultList[i].code >> 24);

                    uint mask16 = 0xFF0000;
                    uint processState = (softFaultList[i].code & mask16) >> 16;

                    uint mask8 = 0xFF00; // Mask with 1s in bits 8 to 15
                    uint processFaultType = (softFaultList[i].code & mask8) >> 8;

                    byte faultParam = (byte)softFaultList[i].code;
                    errorMessage = errorMessage + "，测试项：" + Enum.GetName(typeof(TestItemsEnum), errorTestItem)
                        + ",流程状态:" + processState + ",流程故障类型:" + processFaultType + ",故障参数:" + faultParam;
                    faultList.Add(errorMessage);
                }
            }

            return faultList.ToArray();
        }
        private void ConnectWL7011()
        {
            int errCode = WLIGBTHelper.WLIGBT_C01_Open(IP, ref Handle);
            if (errCode != 0)
            {
                MessageBox.Show("栅极保护卡链接失败");
                Environment.Exit(0);
            }
        }

        public void Dispose()
        {
            int errCode = WLIGBTHelper.WLIGBT_C01_Close(Handle);
            if (errCode != 0)
            {
                MessageBox.Show($"操作栅极保护卡错误,错误代码：{errCode}");
                throw new Exception($"操作栅极保护卡错误,错误代码：{errCode}");
            }
        }
    }
}
