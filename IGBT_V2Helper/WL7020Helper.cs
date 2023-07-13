using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IGBT_V2Helper.IGBStructs;

namespace IGBT_V2Helper
{
    public class WL7020Helper:IDisposable
    {
        private static uint Handle = 0;
        public string IP;
        public WL7020Helper(string ip)
        {
            this.IP = ip;
            ConnectWL7020();
        }
        public bool ClearFault()
        {
            int errCode = WLIGBTHelper.cbb_fault_clear(Handle);
            if (errCode != 0)
            {
                MessageBox.Show($"清除开尔文板故障错误：{errCode}");
                return false;
            }
            return true;
        }
        public byte[] GetKrwStrategy()
        {
            igbt_fix_para_t igbt_fix_para = new igbt_fix_para_t();
            int errCode = WLIGBTHelper.cbb_para_get_para(Handle, ref igbt_fix_para);
            if (errCode == 0)
            {
                uint strategy = igbt_fix_para.krw_fix_para.krw_public_fix_para.strategy;

                byte[] bytes = new byte[24]; // 用于存储结果的byte数组

                for (int i = 0; i < 24; i++)
                {
                    // 通过位掩码将对应位提取出来
                    uint bit = (strategy >> i) & 1;
                    // 将提取的位存储到byte数组中
                    bytes[i] = (byte)bit;
                }
                return bytes;
            }
            else
            {
                MessageBox.Show($"操作开尔文板卡错误,错误代码：{errCode}");
                return new byte[24];
            }
        }
        private static Dictionary<int, string> firstLevelFaultDic = new Dictionary<int, string>
        {
            {0,"开尔文板-FirstLevel-外部一级故障"},
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
        public bool SetKrwStrategy(uint strategyValue)
        {
            igbt_fix_para_t igbt_fix_para = new igbt_fix_para_t();
            int errCode = WLIGBTHelper.cbb_para_get_para(Handle, ref igbt_fix_para);
            if (errCode != 0)
            {
                MessageBox.Show($"操作开尔文板卡错误,错误代码：{errCode}");
                return false;
            }
            igbt_fix_para.krw_fix_para.krw_public_fix_para.strategy = strategyValue;
            errCode = WLIGBTHelper.cbb_para_set_para(Handle, ref igbt_fix_para);
            if (errCode != 0)
            {
                MessageBox.Show($"操作开尔文板卡错误,错误代码：{errCode}");
                return false;
            }
            return true;
        }

       
        public bool GetResultKrwArray(out result_krw_t[] resultKrwArray, uint length, ref uint realLength)
        {
            resultKrwArray = new result_krw_t[length];
            realLength = 0;

            IntPtr resultPtr = Marshal.AllocHGlobal(Marshal.SizeOf<result_krw_t>() * (int)length);
            IntPtr realLengthPtr = Marshal.AllocHGlobal(Marshal.SizeOf<uint>());

            try
            {
                int status = WLIGBTHelper.cbb_result_krw_read(Handle, resultPtr, length, realLengthPtr);
                if (status == 0)
                {
                    realLength = Marshal.PtrToStructure<uint>(realLengthPtr);

                    for (int i = 0; i < realLength; i++)
                    {
                        IntPtr currentResultPtr = new IntPtr(resultPtr.ToInt64() + i * Marshal.SizeOf<result_krw_t>());
                        resultKrwArray[i] = Marshal.PtrToStructure<result_krw_t>(currentResultPtr);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            finally
            {
                Marshal.FreeHGlobal(resultPtr);
                Marshal.FreeHGlobal(realLengthPtr);
            }
        }

        public uint GetResultKrwLength()
        {
            uint length = 0;
            int errCode = WLIGBTHelper.cbb_result_read_length(Handle, TestItemsEnum.ITEM_KRW, ref length);
            if (errCode != 0)
            {
                MessageBox.Show($"操作开尔文板卡错误,错误代码：{errCode}");
                return 0;
            }
            return length;
        }

        private void ConnectWL7020()
        {
            int errCode = WLIGBTHelper.WLIGBT_C01_Open(IP, ref Handle);
            if (errCode != 0)
            {
                MessageBox.Show("开尔文板卡链接失败");
                Environment.Exit(0);
            }
        }
        public void Dispose()
        {
            int errCode = WLIGBTHelper.WLIGBT_C01_Close(Handle);
            if (errCode != 0)
            {
                MessageBox.Show($"操作开尔文板卡错误,错误代码：{errCode}");
                throw new Exception($"操作开尔文板卡错误,错误代码：{errCode}");
            }
        }
    }
}
