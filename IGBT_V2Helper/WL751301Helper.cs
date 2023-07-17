using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IGBT_V2Helper.IGBStructs;

namespace IGBT_V2Helper
{
    public class WL751301Helper:IDisposable
    {
        private static uint Handle = 0;
        public string IP;
        public WL751301Helper(string ip)
        {
            this.IP = ip;
            OpenWl7513_01();
        }

        #region 针床操作
        public bool NeedleBedOperation( byte option)
        {
            UInt32 is_success = 0;
            int result = WLIGBTHelper.cbb_rt1000_needle_bed(Handle, option, ref is_success);

            if (result == 0)
            {
                if (is_success == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                throw new Exception("An error occurred while executing the NeedleBedOperation method.");
            }
        }

        public rt1000_info_t GetRt100001Info()
        {
            uint length = 0;
            int errorCode = WLIGBTHelper.cbb_rt1000_read_rt1000_info_length(Handle, ref length);
            rt1000_info_t rt1000_info = new rt1000_info_t()
            {
                rt_t2_01_di = new byte[24],
                rt_t2_02_di = new byte[24],
                adc_upload_data = new double[8]
            };
            errorCode = WLIGBTHelper.cbb_rt1000_read_rt1000_info(Handle,ref rt1000_info, length, ref length);
            return rt1000_info;
        }
        #endregion

        #region 充放电操作
        public bool WriteGPIO(uint id,uint onOff)
        {
            UInt32 is_success = 0;
            int result = WLIGBTHelper.cbb_rt1000_write_do(Handle, id, onOff);

            if (result == 0)
            {
                if (is_success == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                throw new Exception("An error occurred while executing the TestUnitOperation method.");
            }
        }

        public bool Charge()
        {
            charge_state_t chargeState = new charge_state_t();
            int errorCode = WLIGBTHelper.cbb_rt1000_charge_start(Handle,ref chargeState,10000);
            if (errorCode == 0)
            {
                return true;
            }
            else
            {
                throw new Exception("An error occurred while executing the Charge method.");
            }
        }

        public bool DisCharge()
        {
            charge_state_t chargeState = new charge_state_t();
            int errorCode = WLIGBTHelper.cbb_rt1000_discharge_start(Handle, ref chargeState);
            if (errorCode == 0)
            {
                return true;
            }
            else
            {
                throw new Exception("An error occurred while executing the DisCharge method.");
            }
        }


        #endregion

        #region 参数
        public bool GetIGBTPara(ref igbt_fix_para_t igbt_fix_para)
        {
            int errCode = WLIGBTHelper.cbb_para_get_para(Handle, ref igbt_fix_para);
            if (errCode != 0)
            {
                MessageBox.Show($"获取RT1000_01参数错误,错误代码：{errCode}");
                return false;
            }
            return true;
        }
        public bool SetIGBTPara(ref igbt_fix_para_t igbt_fix_para)
        {
            int errCode = WLIGBTHelper.cbb_para_set_para(Handle, ref igbt_fix_para);
            if (errCode != 0)
            {
                MessageBox.Show($"设置RT1000_01参数错误,错误代码：{errCode}");
                return false;
            }
            return true;
        }
        #endregion


        public void RogwskiCoilDynamicTest()
        {
            rt1000_info_t rt1000Info = GetRt100001Info();

            if (rt1000Info.rt_t2_01_di[19] == 0)
            {
                //上罗氏线圈下降
                WriteGPIO(2059, 1);
            }

            if (rt1000Info.rt_t2_01_di[20] == 1)
            {
                //下罗氏线圈上升
               WriteGPIO(2060, 1);
            }
        }

        public void RogwskiCoilStaticTest()
        {
            rt1000_info_t rt1000Info = GetRt100001Info();

            if (rt1000Info.rt_t2_01_di[19] == 0)
            {
                //上罗氏线圈上升
                WriteGPIO(2059, 0);
            }

            if (rt1000Info.rt_t2_01_di[20] == 0)
            {
                //下罗氏线圈上升
                WriteGPIO(2060, 0);
            }
        }


        private static Dictionary<int, string> firstLevelFaultDic = new Dictionary<int, string>
        {
            {0,"RT1000_01-FirstLevel-外部一级故障"},
            {1,"RT1000_01-FirstLevel-主IGBT故障"},
            {2,"RT1000_01-FirstLevel-上半桥电流采集"},
            {3,"RT1000_01-FirstLevel-下半桥电流采集"},
            {4,"RT1000_01-FirstLevel-初级电压故障"},
            {5,"RT1000_01-FirstLevel-次级电压故障"},
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
            {0,"PLC故障"},
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
                    string errorMessage = "RT1000_01-SOFTFAULT-软件故障类型:" + softFaultCodeDic[i];
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
                    string errorMessage = "RT1000-01-SOFTFAULT-软件故障类型:" + softFaultCodeDic[i];
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
        public bool ClearFault()
        {
            int errCode = WLIGBTHelper.cbb_fault_clear(Handle);
            if (errCode != 0)
            {
                MessageBox.Show($"清除7513_01故障错误：{errCode}");
                return false;
            }
            return true;
        }
        #region Self参数
        public bool GetRTPara(ref cbb_rt1000_para_t cbb_rt1000_para)
        {
            int errCode = WLIGBTHelper.cbb_rt1000_get_para(Handle, ref cbb_rt1000_para);
            if (errCode != 0)
            {
                MessageBox.Show($"获取RT1000_01参数错误,错误代码：{errCode}");
                return false;
            }
            return true;
        }
        public bool SetRTPara(ref cbb_rt1000_para_t cbb_rt1000_para)
        {
            int errCode = WLIGBTHelper.cbb_rt1000_set_para(Handle, ref cbb_rt1000_para);
            if (errCode != 0)
            {
                MessageBox.Show($"获取RT1000_01参数错误,错误代码：{errCode}");
                return false;
            }
            return true;
        }
        #endregion
        private void OpenWl7513_01()
        {
            int errCode = WLIGBTHelper.WLIGBT_C01_Open(IP, ref Handle);
            if (errCode != 0)
            {
                MessageBox.Show("7513_01板卡链接失败");
                Environment.Exit(0);
            }
        }

        public void Dispose()
        {
            int errCode = WLIGBTHelper.WLIGBT_C01_Close(Handle);
            if (errCode != 0)
            {
                MessageBox.Show($"操作751301板卡错误,错误代码：{errCode}");
                throw new Exception($"操作751301板卡错误,错误代码：{errCode}");
            }
        }
    }
}

