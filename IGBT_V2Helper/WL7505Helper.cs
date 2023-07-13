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
    //CCS
    public class WL7505Helper:IDisposable
    {
        private static uint Handle = 0;
        public string IP;
        public WL7505Helper(string ip)
        {
            this.IP = ip;
            ConnectWL7505();
        }
       
        public bool SetIGBTPara(ref igbt_fix_para_t igbt_fix_para)
        {
            int errCode = WLIGBTHelper.cbb_para_set_para(Handle, ref igbt_fix_para);
            if (errCode != 0)
            {
                MessageBox.Show($"设置恒流源板参数错误,错误代码：{errCode}");
                return false;
            }
            return true;
        }
        public bool ClearFault()
        {
            int errCode = WLIGBTHelper.cbb_fault_clear(Handle);
            if (errCode != 0)
            {
                MessageBox.Show($"清除恒流源板故障错误：{errCode}");
                return false;
            }
            return true;
        }

        private void ConnectWL7505()
        {
            int errCode = WLIGBTHelper.WLIGBT_C01_Open(IP, ref Handle);
            if (errCode != 0)
            {
                MessageBox.Show("恒流源板链接失败");
                Environment.Exit(0);
            }
        }
        public bool GetResultVcesatArray(out result_vcesat_t[] resultArray, uint length, ref uint realLength)
        {
            resultArray = new result_vcesat_t[length];
            realLength = 0;

            IntPtr resultPtr = Marshal.AllocHGlobal(Marshal.SizeOf<result_vcesat_t>() * (int)length);
            IntPtr realLengthPtr = Marshal.AllocHGlobal(Marshal.SizeOf<uint>());

            try
            {
                int status = WLIGBTHelper.cbb_result_vcesat_read(Handle, resultPtr, length, realLengthPtr);
                if (status == 0)
                {
                    realLength = Marshal.PtrToStructure<uint>(realLengthPtr);

                    for (int i = 0; i < realLength; i++)
                    {
                        IntPtr currentResultPtr = new IntPtr(resultPtr.ToInt64() + i * Marshal.SizeOf<result_vcesat_t>());
                        resultArray[i] = Marshal.PtrToStructure<result_vcesat_t>(currentResultPtr);
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

        public bool GetResultVFtArray(out result_vf_t[] resultArray, uint length, ref uint realLength)
        {
            resultArray = new result_vf_t[length];
            realLength = 0;

            IntPtr resultPtr = Marshal.AllocHGlobal(Marshal.SizeOf<result_vf_t>() * (int)length);
            IntPtr realLengthPtr = Marshal.AllocHGlobal(Marshal.SizeOf<uint>());

            try
            {
                int status = WLIGBTHelper.cbb_result_vf_read(Handle, resultPtr, length, realLengthPtr);
                if (status == 0)
                {
                    realLength = Marshal.PtrToStructure<uint>(realLengthPtr);

                    for (int i = 0; i < realLength; i++)
                    {
                        IntPtr currentResultPtr = new IntPtr(resultPtr.ToInt64() + i * Marshal.SizeOf<result_vf_t>());
                        resultArray[i] = Marshal.PtrToStructure<result_vf_t>(currentResultPtr);
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
        public uint GetResultVcesatLength()
        {
            uint length = 0;
            int errCode = WLIGBTHelper.cbb_result_read_length(Handle, TestItemsEnum.ITEM_VCESAT, ref length);
            if (errCode != 0)
            {
                MessageBox.Show($"操作恒流源板错误,错误代码：{errCode}");
                return 0;
            }
            return length;
        }

        public uint GetResultVFLength()
        {
            uint length = 0;
            int errCode = WLIGBTHelper.cbb_result_read_length(Handle, TestItemsEnum.ITEM_VF, ref length);
            if (errCode != 0)
            {
                MessageBox.Show($"操作恒流源板错误,错误代码：{errCode}");
                return 0;
            }
            return length;
        }
        private static Dictionary<int, string> firstLevelFaultDic = new Dictionary<int, string>
        {
            {0,"恒流源板-FirstLevel-外部一级故障"},
            {1,"恒流源板-FirstLevel-大电流输出过流1故障"},
            {2,"恒流源板-FirstLevel-大电流输出过流2故障"},
            {3,"恒流源板-FirstLevel-大电流输出过流3故障"},
            {4,"恒流源板-FirstLevel-大电流输出过流4故障"},
            {5,"恒流源板-FirstLevel-PS7501故障1"},
            {6,"恒流源板-FirstLevel-PS7501故障2"},
            {7,"恒流源板-FirstLevel-PS7501故障3"},
            {8,"恒流源板-FirstLevel-PS7501故障4"},
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
                    string errorMessage = "恒流源板-SOFTFAULT-软件故障类型:" + softFaultCodeDic[i];
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
                    string errorMessage = "恒流源板-SOFTFAULT-软件故障类型:" + softFaultCodeDic[i];
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
                MessageBox.Show($"操作高压脉冲板卡错误,错误代码：{errCode}");
                throw new Exception($"操作高压脉冲板卡错误,错误代码：{errCode}");
            }
        }
    }
}
