using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IGBT_V2Helper.IGBStructs;

namespace IGBT_V2Helper
{
    public class WL7016Helper : IDisposable
    {
        private static uint Handle = 0;
        public string IP;
        public WL7016Helper(string ip)
        {
            this.IP = ip;
            ConnectWL7016();
        }

        public bool TestIsFinished()
        {
            byte is_valid = 0;
            byte test_item = 0;
            byte is_success = 0;


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
          
            while (is_valid == 0)
            {
                if(stopwatch.ElapsedMilliseconds > 10000)
                {
                    return false;
                }
                WLIGBTHelper.wl_igbt_get_test_complete_state(Handle, ref is_valid, ref test_item, ref is_success);
            }
            return true;
        }
        
        
        public bool ClearResult(TestItemsEnum itemType)
        {
            return WLIGBTHelper.ccb_result_clear(Handle, itemType) == 0 ? true : false;
        }

        public bool ClearAllResult()
        {
            return WLIGBTHelper.cbb_result_clear_all(Handle) == 0 ? true : false;
        }

        #region 流程策略
        public bool SetSequeenceInfo(TestItemsEnum testItemsEnum)
        {
            igbt_fix_para_t igbt_fix_para = new igbt_fix_para_t();
            int errCode = WLIGBTHelper.cbb_para_get_para(Handle, ref igbt_fix_para);
            if (errCode != 0)
            {
                MessageBox.Show($"获取系统管理板参数错误,错误代码：{errCode}");
                return false;
            }

            byte[] sequenceInfo = new byte[24];
            sequenceInfo[0] = (byte)testItemsEnum;
            igbt_fix_para.sequence_fix_para.sequence_public_fix_para.sequence = sequenceInfo;
            errCode = WLIGBTHelper.cbb_para_set_para(Handle, ref igbt_fix_para);
            if (errCode != 0)
            {
                MessageBox.Show($"系统管理板卡设置参数错误,错误代码：{errCode}");
                return false;
            }
            return true;
        }

        public bool SetUnitStrategy(byte unitNo)
        {
            igbt_fix_para_t igbt_fix_para = new igbt_fix_para_t();
            int errCode = WLIGBTHelper.cbb_para_get_para(Handle, ref igbt_fix_para);
            if (errCode != 0)
            {
                MessageBox.Show($"获取系统管理板参数错误,错误代码：{errCode}");
                return false;
            }

            byte[] strategy = new byte[24];
            strategy[0] = unitNo;
            igbt_fix_para.mpulse_fix_para.mpulse_public_fix_para.strategy = strategy;
            errCode = WLIGBTHelper.cbb_para_set_para(Handle, ref igbt_fix_para);
            if (errCode != 0)
            {
                MessageBox.Show($"系统管理板卡设置参数错误,错误代码：{errCode}");
                return false;
            }
            return true;
        }

        public bool GetSystemPara(out system_fix_para_t systemFixPara)
        {
            systemFixPara = default;
            
            igbt_fix_para_t igbt_fix_para = new igbt_fix_para_t();
            int errCode = WLIGBTHelper.cbb_para_get_para(Handle, ref igbt_fix_para);
            if (errCode != 0)
            {
                MessageBox.Show($"获取系统管理板参数错误,错误代码：{errCode}");
                return false;
            }
            systemFixPara = igbt_fix_para.system_fix_para;
            return true;
        }
        public bool ClearFault()
        {
            int errCode = WLIGBTHelper.cbb_fault_clear(Handle);
            if (errCode != 0)
            {
                MessageBox.Show($"清除系统管理板故障错误：{errCode}");
                return false;
            }
            return true;
        }
        public bool ExecuteSequence()
        {

            int errCode = WLIGBTHelper.wl_igbt_flow_stop(Handle);
          
            if (errCode != 0)
            {
                MessageBox.Show($"系统管理板执行序列错误,错误代码：{errCode}");
                return false;
            }
            errCode = WLIGBTHelper.wl_igbt_flow_start(Handle);
            if (errCode != 0)
            {
                MessageBox.Show($"系统管理板执行序列错误,错误代码：{errCode}");
                return false;
            }
            return true;
        }
        #endregion

        #region uart

        public bool Write485Data(byte boardType,byte[] buff, uint length)
        {
            uint uartIndex = 0;
            if (boardType == 5)
            {
                uartIndex = 1;
            }
            if (boardType == 7)
            {
                uartIndex = 2;
            }
            if (boardType == 8)
            {
                uartIndex = 3;
            }
            if (0 == WLIGBTHelper.cbb_wluart_frame_write(Handle, uartIndex, buff, length))
            {
                return true;
            }
            return false;
        }
        #endregion

        public bool GetResultICESArray(out result_ices_t[] resultArray, uint length, ref uint realLength)
        {
            resultArray = new result_ices_t[length];
            realLength = 0;

            IntPtr resultPtr = Marshal.AllocHGlobal(Marshal.SizeOf<result_ices_t>() * (int)length);
            IntPtr realLengthPtr = Marshal.AllocHGlobal(Marshal.SizeOf<uint>());

            try
            {
                int status = WLIGBTHelper.cbb_result_ices_read(Handle, resultPtr, length, realLengthPtr);
                if (status == 0)
                {
                    realLength = Marshal.PtrToStructure<uint>(realLengthPtr);

                    for (int i = 0; i < realLength; i++)
                    {
                        IntPtr currentResultPtr = new IntPtr(resultPtr.ToInt64() + i * Marshal.SizeOf<result_ices_t>());
                        resultArray[i] = Marshal.PtrToStructure<result_ices_t>(currentResultPtr);
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

        public uint GetResultICESLength()
        {
            uint length = 0;
            int errCode = WLIGBTHelper.cbb_result_read_length(Handle, TestItemsEnum.ITEM_ICES, ref length);
            if (errCode != 0)
            {
                MessageBox.Show($"操作系统管理板卡错误,错误代码：{errCode}");
                return 0;
            }
            return length;
        }

        public uint GetResultVcesatLength()
        {
            uint length = 0;
            int errCode = WLIGBTHelper.cbb_result_read_length(Handle, TestItemsEnum.ITEM_VCESAT, ref length);
            if (errCode != 0)
            {
                MessageBox.Show($"操作系统管理板卡错误,错误代码：{errCode}");
                return 0;
            }
            return length;
        }

        public uint GetResultVFLength()
        {
            uint length = 0;
            int errCode = WLIGBTHelper.cbb_result_read_length(Handle, TestItemsEnum.ITEM_VF,ref length);
            if (errCode != 0)
            {
                MessageBox.Show($"操作系统管理板卡错误,错误代码：{errCode}");
                return 0;
            }
            return length;
        }

        #region 参数
        public bool GetIGBTPara(ref igbt_fix_para_t igbt_fix_para)
        {
            int errCode = WLIGBTHelper.cbb_para_get_para(Handle, ref igbt_fix_para);
            if (errCode != 0)
            {
                MessageBox.Show($"获取系统管理板参数错误,错误代码：{errCode}");
                return false;
            }
            return true;
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
        #endregion

       

        public bool Trigger(byte type)
        {
            return WLIGBTHelper.wl_igbt_flow_trigger(Handle, type) == 0 ? true : false;
        }
        public int[] GetDeviceWithFault()
        {
            fault_list_t fault_list = new fault_list_t();
            int errorCode = WLIGBTHelper.cbb_fault_get_current_fault_list(Handle,ref fault_list);
            if (errorCode != 0)
            {
                MessageBox.Show("获取故障列表错误，错误码："+errorCode);
                return null;
            }

            List<int> deviceIdList = new List<int>();
            fault_list_item_t[] items = fault_list.fault_list_item;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].hw_fault1 == 1 || items[i].hw_fault2 == 1 || items[i].soft_fault == 1)
                {
                    deviceIdList.Add(i);
                }
            }
            return deviceIdList.ToArray();
        }


        private Dictionary<int, string> firstLevelFaultDic = new Dictionary<int, string>
        {
            {0,"系统管理板-FirstLevel-外部一级故障"},
            {1,"系统管理板-FirstLevel-光口故障总线0"},
            {2,"系统管理板-FirstLevel-光口故障总线1"},
            {3,"系统管理板-FirstLevel-光口故障总线2"},
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
        private  Dictionary<int, string> secondLevelFaultDic = new Dictionary<int, string>
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

        private  Dictionary<int, string> softFaultCodeDic = new Dictionary<int, string>
        {
            {0,"485通讯超时"},
            {1,"DMA超时"},
            {2,"结果超时"},
            {3,"结果错误"},
            {4,"流程故障"},
        };

        public  string[] GetFirstLevelFault(fault_info_t faultInfo)
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

        public  string[] GetSecondLevelFault(fault_info_t faultInfo)
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

        public  string[] GetSoftFault(fault_info_t faultInfo)
        {
            soft_fault_t[] softFaultList = faultInfo.soft_fault;

            List<string> faultList = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                if (softFaultList[i].status == 1)
                {
                    string errorMessage = "系统保护板-SOFTFAULT-软件故障类型:" + softFaultCodeDic[i];
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
            int errorCode = WLIGBTHelper.cbb_fault_get_current_fault_info(Handle,ref faultInfo);

            if (errorCode != 0)
            {
                MessageBox.Show("获取当前故障失败，错误码："+ errorCode);
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
                    string errorMessage = "系统保护板-SOFTFAULT-软件故障类型:" + softFaultCodeDic[i];
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
        private void ConnectWL7016()
        {
            int errCode = WLIGBTHelper.WLIGBT_C01_Open(IP, ref Handle);
            if (errCode != 0)
            {
                MessageBox.Show("系统管理板卡链接失败");
                Environment.Exit(0);
            }
        }
        public void Dispose()
        {
            int errCode = WLIGBTHelper.WLIGBT_C01_Close(Handle);
            if (errCode != 0)
            {
                MessageBox.Show($"操作系统管理板卡错误,错误代码：{errCode}");
                throw new Exception($"操作系统管理板卡错误,错误代码：{errCode}");
            }
        }
    }
}
