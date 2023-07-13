using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IGBT_V2Helper.IGBStructs;

namespace IGBT_V2Helper
{
    public class WL7010Helper : IDisposable
    {
        private static uint Handle = 0;
        public string IP;
        public WL7010Helper(string ip)
        {
            this.IP = ip;
            ConnectWL7010();
        }

        #region 参数
        public bool GetIGBTPara(ref igbt_fix_para_t igbt_fix_para)
        {
            int errCode = WLIGBTHelper.cbb_para_get_para(Handle, ref igbt_fix_para);
            if (errCode != 0)
            {
                MessageBox.Show($"获取栅极驱动板参数错误,错误代码：{errCode}");
                return false;
            }
            return true;
        }
        public bool SetIGBTPara(ref igbt_fix_para_t igbt_fix_para)
        {
            int errCode = WLIGBTHelper.cbb_para_set_para(Handle, ref igbt_fix_para);
            if (errCode != 0)
            {
                MessageBox.Show($"设置栅极驱动板参数错误,错误代码：{errCode}");
                return false;
            }
            return true;
        }
        #endregion
        public bool ClearFault()
        {
            int errCode = WLIGBTHelper.cbb_fault_clear(Handle);
            if (errCode != 0)
            {
                MessageBox.Show($"清除栅极驱动板故障错误：{errCode}");
                return false;
            }
            return true;
        }
        private void ConnectWL7010()
        {
            int errCode = WLIGBTHelper.WLIGBT_C01_Open(IP, ref Handle);
            if (errCode != 0)
            {
                MessageBox.Show("栅极驱动板卡链接失败");
                Environment.Exit(0);
            }
        }
        private static Dictionary<int, string> firstLevelFaultDic = new Dictionary<int, string>
        {
            {0,"栅极驱动板-FirstLevel-外部一级故障"},
            {1,"栅极驱动板-FirstLevel-A路栅极驱动门级过压故障"},
            {2,"栅极驱动板-FirstLevel-B路栅极驱动门级过压故障"},
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

        private static Dictionary<int, string> secondLevelFaultDic = new Dictionary<int, string>
        {
            {0,"栅极驱动板-SecondLevel-A路正向驱动电源欠压故障"},
            {1,"栅极驱动板-SecondLevel-A路负向驱动电源欠压故障"},
            {2,"栅极驱动板-SecondLevel-B路正向驱动电源欠压故障"},
            {3,"栅极驱动板-SecondLevel-B路负向驱动电源欠压故障"},
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

        public string[] GetAllFaultMessage()
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
                    string errorMessage = "栅极驱动板-SOFTFAULT-软件故障类型:" + softFaultCodeDic[i];
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
        public void Dispose()
        {
            int errCode = WLIGBTHelper.WLIGBT_C01_Close(Handle);
            if (errCode != 0)
            {
                MessageBox.Show($"操作栅极驱动板卡错误,错误代码：{errCode}");
                throw new Exception($"操作栅极驱动板卡错误,错误代码：{errCode}");
            }
        }
    }
}
