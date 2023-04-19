using Wolei_485Trans;
using IGBT_SET.Common;
using System.Windows.Media;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System;
using System.Text;
using Microsoft.Win32;

namespace IGBT_SET.ViewModel
{
    public class MainWindowModel : Notify
    {
        #region 公共属性 
        private bool channelChangeFinish;
        /// <summary>
        /// tabcontrol 修改通道配置成功
        /// </summary>
        public bool ChannelChangeFinish
        {
            get
            {
                return channelChangeFinish;
            }
            set
            {
                if (channelChangeFinish != value)
                {
                    channelChangeFinish = value;
                    RaisePropertyChanged("ChannelChangeFinish");
                }
            }
        }

        private float pulse1_width;
        /// <summary>
        /// 脉冲1宽度
        /// </summary>
        public float Pulse1_width
        {
            get
            {
                return pulse1_width;
            }
            set
            {
                if (pulse1_width != value)
                {
                    pulse1_width = value;
                    RaisePropertyChanged("Pulse1_width");
                }
            }
        }

        public static DataCfg dataCfg;

        public byte EnableTabIndex = 0;

        public byte OldChannel = 0xFF;


        #endregion

        #region 高压测试属性
        private string curVolt;
        /// <summary>
        ///  当前电压值（V）
        /// </summary>
        public string CurVolt
        {
            get
            {
                return curVolt;
            }
            set
            {
                if (curVolt != value)
                {
                    curVolt = value;
                    RaisePropertyChanged("CurVolt");
                }
            }
        }

        private string curEle;
        /// <summary>
        ///  当前电流值（A）
        /// </summary>
        public string CurEle
        {
            get
            {
                return curEle;
            }
            set
            {
                if (curEle != value)
                {
                    curEle = value;
                    RaisePropertyChanged("CurEle");
                }
            }
        }
        private string setVolt;

        /// <summary>
        ///  设置电压（V）
        /// </summary>
        public string SetVolt
        {
            get
            {
                return setVolt;
            }
            set
            {
                if (setVolt != value)
                { 
                    setVolt = value;
                    RaisePropertyChanged("SetVolt");
                }
            }
        }

        private string setEle;
        /// <summary>
        ///  设置电流
        /// </summary>
        public string SetEle
        {
            get
            {
                return setEle;
            }
            set
            {
                if (setEle != value)
                {
                    setEle = value;
                    RaisePropertyChanged("SetEle");
                }
            }
        }

        private string protectVolt;
        /// <summary>
        ///  保护电压（V）
        /// </summary>
        public string ProtectVolt
        {
            get
            {
                return protectVolt;
            }
            set
            {
                if (protectVolt != value)
                {
                    protectVolt = value;
                    RaisePropertyChanged("ProtectVolt");
                }
            }
        }

        private string protectEle;
        /// <summary>
        ///  保护电流 (A)
        /// </summary>
        public string ProtectEle
        {
            get
            {
                return protectEle;
            }
            set
            {
                if (protectEle != value)
                {
                    protectEle = value;
                    RaisePropertyChanged("ProtectEle");
                }
            }
        }

        private bool isOnline;
        /// <summary>
        ///  上电状态--高压测试
        /// </summary>
        public bool IsOnline
        {
            get
            {
                return isOnline;
            }
            set
            {
                if (isOnline != value)
                {
                    isOnline = value;
                    RaisePropertyChanged("IsOnline");
                }
            }
        }
        //private Brush isOnline;
        ///// <summary>
        /////  上电状态--高压测试
        ///// </summary>
        //public Brush IsOnline
        //{
        //    get
        //    {
        //        return isOnline;
        //    }
        //    set
        //    {
        //        isOnline = value;
        //        RaisePropertyChanged("isOnline");
        //    }
        //}

        #endregion

        #region 栅极测试属性
        private string curPositiveVolt;
        /// <summary>
        /// 实测正压
        /// </summary>
        public string CurPositiveVolt
        {
            get
            {
                return curPositiveVolt;
            }
            set
            {
                if (curPositiveVolt != value)
                {
                    curPositiveVolt = value;
                    RaisePropertyChanged("CurPositiveVolt");
                }
            }
        }

        private string curNegativeVolt;
        /// <summary>
        /// 实测负压
        /// </summary>
        public string CurNegativeVolt
        {
            get
            {
                return curNegativeVolt;
            }
            set
            {
                if (curNegativeVolt != value)
                {
                    curNegativeVolt = value;
                    RaisePropertyChanged("CurNegativeVolt");
                }
            }
        }

        private string gridPositiveVolt_Set;
        /// <summary>
        /// 栅极设置正压
        /// </summary>
        public string GridPositiveVolt_Set
        {
            get
            {
                return gridPositiveVolt_Set;
            }
            set
            {
                if (gridPositiveVolt_Set != value)
                {
                    gridPositiveVolt_Set = value;
                    RaisePropertyChanged("GridPositiveVolt_Set");
                }
            }
        }
        private string gridNegativeVolt_Set;
        /// <summary>
        /// 栅极设置负压
        /// </summary>
        public string GridNegativeVolt_Set
        {
            get
            {
                return gridNegativeVolt_Set;
            }
            set
            {
                if (gridNegativeVolt_Set != value)
                {
                    gridNegativeVolt_Set = value;
                    RaisePropertyChanged("GridNegativeVolt_Set");
                }
            }
        }

        private bool channelStatus_1;

        /// <summary>
        /// 通道1上电状态
        /// </summary>
        public bool ChannelStatus_1
        {
            get
            {
                return channelStatus_1;
            }
            set
            {
                if (channelStatus_1 != value)
                {
                    channelStatus_1 = value;
                    RaisePropertyChanged("ChannelStatus_1");
                }
            }
        }

        private bool channelStatus_2;
        /// <summary>
        /// 通道2上电状态
        /// </summary>
        public bool ChannelStatus_2
        {
            get
            {
                return channelStatus_2;
            }
            set
            {
                if (channelStatus_2 != value)
                {
                    channelStatus_2 = value;
                    RaisePropertyChanged("ChannelStatus_2");
                }
            }
        }
        #endregion

        #region PLC_View
        private bool fWDarrivedSignal1;
        /// <summary>
        /// 正向旋转到位信号 
        /// </summary>
        public bool FWDArrivedSignal1
        {
            get
            {
                return fWDarrivedSignal1;
            }
            set
            {
                if (value != fWDarrivedSignal1)
                {
                    fWDarrivedSignal1 = value;
                    RaisePropertyChanged("FWDArrivedSignal1");
                }
            }
        }
        private bool fWDarrivedSignal2;
        /// <summary>
        /// 反向旋转到位信号 
        /// </summary>
        public bool FWDArrivedSignal2
        {
            get
            {
                return fWDarrivedSignal2;
            }
            set
            {
                if (value != fWDarrivedSignal2)
                {
                    fWDarrivedSignal2 = value;
                    RaisePropertyChanged("FWDArrivedSignal2");
                }
            }
        }
        private bool fWDarrivedSignal3;
        /// <summary>
        /// 复位旋转到位信号 
        /// </summary>
        public bool FWDArrivedSignal3
        {
            get
            {
                return fWDarrivedSignal3;
            }
            set
            {
                if (value != fWDarrivedSignal3)
                {
                    fWDarrivedSignal3 = value;
                    RaisePropertyChanged("FWDArrivedSignal3");
                }
            }
        }

        //private bool reversalarrivedSignal;
        ///// <summary>
        ///// 反转180到位信号 
        ///// </summary>
        //public bool ReversalArrivedSignal
        //{
        //    get
        //    {
        //        return reversalarrivedSignal;
        //    }
        //    set
        //    {
        //        reversalarrivedSignal = value;
        //        RaisePropertyChanged("reversalarrivedSignal");
        //    }
        //}

        private bool productUpedSignal;
        /// <summary>
        /// 产品上升到位信号 
        /// </summary>
        public bool ProductUpedSignal
        {
            get
            {
                return productUpedSignal;
            }
            set
            {
                if(value != productUpedSignal)
                {
                    productUpedSignal = value;
                    RaisePropertyChanged("ProductUpedSignal");
                }
            }
        }

        private bool productDownedSignal;
        /// <summary>
        /// 产品下降到位信号 
        /// </summary>
        public bool ProductDownedSignal
        {
            get
            {
                return productDownedSignal;
            }
            set
            {
                if (productDownedSignal != value)
                {
                    productDownedSignal = value;
                    RaisePropertyChanged("ProductDownedSignal");
                }
            }
        }

        private bool needleUpedSignal;
        /// <summary>
        /// 针床上升到位信号 
        /// </summary>
        public bool NeedleUpedSignal
        {
            get
            {
                return needleUpedSignal;
            }
            set
            {
                if (needleUpedSignal != value)
                {
                    needleUpedSignal = value;
                    RaisePropertyChanged("NeedleUpedSignal");
                }
            }
        }

        private bool needleDownedSignal;
        /// <summary>
        /// 针床下降到位信号 
        /// </summary>
        public bool NeedleDownedSignal
        {
            get
            {
                return needleDownedSignal;
            }
            set
            {
                if (value != needleDownedSignal)
                {
                    needleDownedSignal = value;
                    RaisePropertyChanged("NeedleDownedSignal");
                }
            }
        }

        private int operationTemperature_1;
        /// <summary>
        /// 工装1温度
        /// </summary>
        public int OperationTemperature_1
        {
            get
            {
                return operationTemperature_1;
            }
            set
            {
                if (operationTemperature_1 != value)
                {
                    operationTemperature_1 = value;
                    RaisePropertyChanged("OperationTemperature_1");
                }
            }
        }

        private int operationTemperature_2;
        /// <summary>
        /// 工装2温度 
        /// </summary>
        public int OperationTemperature_2
        {
            get
            {
                return operationTemperature_2;
            }
            set
            {
                if (operationTemperature_2 != value)
                {
                    operationTemperature_2 = value;
                    RaisePropertyChanged("OperationTemperature_2");
                }
            }
        }
        #endregion

        #region 开尔文配置
        //private byte[] kelvinCfg;
        ///// <summary>
        /////  开尔文测试配置
        ///// </summary>
        //public byte[] KelvinCfg
        //{
        //    get
        //    {
        //        return kelvinCfg;
        //    }
        //    set
        //    {
        //        kelvinCfg = value;
        //        RaisePropertyChanged("kelvinCfg");
        //    }
        //}
        private bool isKelvinSuccess = false;

        public bool IsKelvinSuccess
        {
            get
            {
                return isKelvinSuccess;
            }
            set
            {
                isKelvinSuccess = value;
                RaisePropertyChanged("IsKelvinSuccess");
            }
        }

        private bool isInitSuccess = true;
        public bool IsInitSuccess
        {
            get
            {
                return isInitSuccess;
            }
            set
            {
                isInitSuccess = value;
                RaisePropertyChanged("IsInitSuccess");
            }
        }

        private string kelvinTestString;
        public string KelvinTestString
        {
            get
            {
                return kelvinTestString;
            }
            set
            {
                kelvinTestString = value;
                RaisePropertyChanged("kelvinTestString");
            }
        }

        private bool kelvinTestStatus;
        public bool KelvinTestStatus
        {
            get
            {
                return kelvinTestStatus;
            }
            set
            {
                kelvinTestStatus = value;
                RaisePropertyChanged("kelvinTestStatus");
            }
        }
        #endregion


        private static MainWindowModel windowModel;

        public static DevCollocation devCollocation;

        public CancellationTokenSource cts = new CancellationTokenSource();

        public MainWindowModel()
        {
            if (windowModel == null)
            {
                if (devCollocation == null)
                    devCollocation = new DevCollocation();
#if false
                if (dataCfg == null)
                    dataCfg = new DataCfg("192.168.1.215", 1024);
#endif


#if true
                if (dataCfg == null)
                    dataCfg = new DataCfg();

                BackgroundWorker highValueGet = new BackgroundWorker();
                highValueGet.DoWork += HighValueGet;
                highValueGet.RunWorkerAsync();

                BackgroundWorker lowValueGet = new BackgroundWorker();
                lowValueGet.DoWork += LowValueGet;
                lowValueGet.RunWorkerAsync();

                BackgroundWorker SignalJudge = new BackgroundWorker();
                SignalJudge.DoWork += PlcDatagGet;
                SignalJudge.RunWorkerAsync();

                BackgroundWorker SignalGet= new BackgroundWorker();
                SignalGet.DoWork += PlcDatagRead;
                SignalGet.RunWorkerAsync();
#endif
                windowModel = this;
            }
        }

        /// <summary>
        /// 获取当前实例
        /// </summary>
        /// <returns></returns>
        public static MainWindowModel GetInstance()
        {
            if (windowModel != null)
            {
                return windowModel;
            }
            else
            {
                return SetInstance();
            }
        }
        /// <summary>
        /// 创建当前实例
        /// </summary>
        /// <returns></returns>
        private static MainWindowModel SetInstance()
        {
            windowModel = new MainWindowModel();
            return windowModel;
        }

        /// <summary>
        /// Plc参获取
        /// </summary>
        public void PlcParameterGet( ushort register)
        {
            try
            {
                //旋转复位
                byte[] data = new byte[10];
                data[0] = (byte)(register & 0xFF);
                data[1] = (byte)((register >> 8)&0xFF);
                dataCfg.SendProtocolDataCFG((byte)CardType.SysMangerCard,(byte)FuncCode.ParamerGet, data, (byte)CardType.PLCCard);
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务器异常 :"+ ex.Message);
            }
        }
        private void PlcDatagRead(object sender, DoWorkEventArgs e)
        {
            //托盘到位信号
            PlcParameterGet(0x03);

            //ushort register = 3;
            //while (!cts.IsCancellationRequested)
            //{
            //    Thread.Sleep(100);
            //    try
            //    {
            //        PlcParameterGet(register);

            //        Thread.Sleep(500);
            //        register += 2;
            //        PlcParameterGet(register);

            //        Thread.Sleep(500);
            //        register += 2;
            //        PlcParameterGet(register);

            //        Thread.Sleep(500);
            //        register += 2;
            //        PlcParameterGet(register);

            //        Thread.Sleep(500);
            //        register += 2;
            //        PlcParameterGet(register);
            //        Thread.Sleep(1000);
            //        register = 3;
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("PLC 服务器数据获取异常！ info:" + ex.Message);
            //    }
            //}
        }

        //当前托盘配置转动方向
        public byte TrayTurnDirection = 0;
        /// <summary>
        /// PLC数据读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlcDatagGet(object sender, DoWorkEventArgs e)
        {
            byte[] data;
            ushort register = 0;
            //while (!cts.IsCancellationRequested)
            //{
            //    Thread.Sleep(100);
            //    try
            //    {
            //        data = dataCfg.protocolDataTransceiver.GetCurrentProtocolData()?.Data;
            //        if (data == null && data.Length < 9)
            //        {
            //            continue;
            //        }
            //        register = (ushort)(data[7] + data[8] * 0x100);
            //        switch (register)
            //        {
            //            case 3:
            //                if (data[9] == 1 && TrayTurnDirection == 1)
            //                    FWDArrivedSignal1 = true;
            //                else if (data[9] == 1 && TrayTurnDirection == 2)
            //                    FWDArrivedSignal2 = true;
            //                else if (data[9] == 1 && TrayTurnDirection == 3)
            //                    FWDArrivedSignal3 = true;
            //                else
            //                {
            //                    FWDArrivedSignal1 = false;
            //                    FWDArrivedSignal2 = false;
            //                    FWDArrivedSignal3 = false;
            //                }
            //                break;
            //            case 5:
            //                if (data[9] == 1) //产品上升到位
            //                {
            //                    ProductUpedSignal = true;
            //                    ProductDownedSignal = false;
            //                }
            //                else
            //                {
            //                    ProductUpedSignal = false;
            //                    ProductDownedSignal = true;
            //                }
            //                break;
            //            case 7:
            //                if (data[9] == 1) //针床到位
            //                {
            //                    NeedleUpedSignal = true;
            //                    NeedleDownedSignal = false;
            //                }
            //                else
            //                {
            //                    NeedleUpedSignal = false;
            //                    NeedleDownedSignal = true;
            //                }
            //                break;
            //            case 9:
            //                if (data[9] == 1) //工装1温度
            //                {
            //                    OperationTemperature_1 = data[10] + data[11] * 0x100 + data[12] * 0x10000 + data[13] * 0x1000000;
            //                }
            //                else if (data[9] == 2)  //工装2温度
            //                {
            //                    OperationTemperature_2 = data[10] + data[11] * 0x100 + data[12] * 0x10000 + data[13] * 0x1000000;
            //                }
            //                break;
            //            case 0xB:
            //                if (data[9] == 1) //测试通道到位信号
            //                {
            //                    ChannelChangeFinish = true;
            //                }
            //                else if (data[9] == 2)//测试通道未到位信号
            //                {
            //                    ChannelChangeFinish = false;
            //                }
            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("PLC 服务器数据获取异常！ info:" + ex.Message);
            //    }
            //}
        }
        /// <summary>
        /// 高压电源数据获取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HighValueGet(object sender, DoWorkEventArgs e)
        {
            bool remote = false;
            while (!cts.IsCancellationRequested)
            {
                Thread.Sleep(1000);
                try
                {
                    lock (dataCfg.tcpDC_High)
                    {
                        if (!remote)
                        {
                            //设置远程模式
                            dataCfg.tcpDC_High.WriteLine("CONF:SETPT 3");
                            remote = true;
                        }
                        ////选择通道命令
                        //dataCfg.tcpDC_High.WriteLine("OUTP 1,(@1)");

                        //发送获取电压请求
                        dataCfg.tcpDC_High.WriteLine("MEAS:VOLT?");
                        Thread.Sleep(50);
                        //读取
                        CurVolt = dataCfg.tcpDC_High.ReadLine().Trim();

                        //读取当前通道 上电状态
                        dataCfg.tcpDC_High.WriteLine("OUTPut?");
                        Thread.Sleep(50);
                        //当前通道信息
                        IsOnline = int.Parse(dataCfg.tcpDC_High.ReadLine().Trim()) > 0 ? true : false;

                        //读电流
                        dataCfg.tcpDC_High.WriteLine("MEAS:CURR?");
                        Thread.Sleep(50);
                        //获取读取结果
                        CurEle = dataCfg.tcpDC_High.ReadLine().Trim();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("获取当前高压电源数据 错误 ！");
                }
            }
        }

        public volatile bool IsDCGetValue = true;
        
        /// <summary>
        /// 低压电源数据获取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LowValueGet(object sender, DoWorkEventArgs e)
        { 
            bool remote = false;
            while (!cts.IsCancellationRequested)
            {
                Thread.Sleep(1000);
                if (IsDCGetValue)
                {
                    if (!remote)
                    {
                        //设置远程模式
                        dataCfg.DcPowerHandle.WriteCommand("SYST:REM");
                        remote = true;
                    }
                    dataCfg.DcPowerHandle.WriteCommand("CHANnel 1");
                    dataCfg.DcPowerHandle.ReadCommand("MEAS:VOLT?");
                    CurPositiveVolt = dataCfg.DcPowerHandle.ReadCommand("MEAS:VOLT?");
                    string outputStatus = dataCfg.DcPowerHandle.ReadCommand("OUTP?");
                    ChannelStatus_1 = int.Parse(outputStatus?.Trim()) > 0 ? true : false;


                    dataCfg.DcPowerHandle.WriteCommand("CHANnel 2");
                    dataCfg.DcPowerHandle.ReadCommand("MEAS:VOLT?");
                    CurNegativeVolt = dataCfg.DcPowerHandle.ReadCommand("MEAS:VOLT?");
                    string outputStatus1 = dataCfg.DcPowerHandle.ReadCommand("OUTP?");
                    ChannelStatus_2 = int.Parse(outputStatus1?.Trim()) > 0 ? true : false;
                }
                //IsDCGetValue = false;
            }

            //bool remote = false;
            //while (!cts.IsCancellationRequested)
            //{
            //    Thread.Sleep(1000);
            //    try
            //    {
            //        lock (dataCfg.tcpDC_Low)
            //        {
            //            if (!remote)
            //            {
            //                //设置远程模式
            //                dataCfg.tcpDC_Low.WriteLine("SYST:REM");
            //                remote = true;
            //            }
            //            dataCfg.tcpDC_Low.WriteLine("CHANnel 1");
            //            dataCfg.tcpDC_Low.WriteLine("MEAS:VOLT?");
            //            Thread.Sleep(2000);
            //            dataCfg.tcpDC_Low.ReadText(8, false);
            //            dataCfg.tcpDC_Low.WriteLine("MEAS:VOLT?");
            //            Thread.Sleep(2000);
            //            CurPositiveVolt = dataCfg.tcpDC_Low.ReadText(8, false);
            //            dataCfg.tcpDC_Low.WriteLine("OUTP?");
            //            Thread.Sleep(2000);
            //            //当前通道信息
            //            ChannelStatus_1 = int.Parse(dataCfg.tcpDC_Low.ReadLine().Trim()) > 0 ? true : false;
            //        }
            //        lock (dataCfg.tcpDC_Low_Ch2)
            //        {
            //            ////通道2
            //            dataCfg.tcpDC_Low_Ch2.WriteLine("CHANnel 2");
            //            dataCfg.tcpDC_Low_Ch2.WriteLine("MEAS:VOLT?");
            //            Thread.Sleep(1000);
            //            dataCfg.tcpDC_Low_Ch2.ReadText(8, false);
            //            dataCfg.tcpDC_Low_Ch2.WriteLine("MEAS:VOLT?");
            //            Thread.Sleep(1000);
            //            //读取 当前正压
            //            CurNegativeVolt = dataCfg.tcpDC_Low_Ch2.ReadText(8, false);
            //            dataCfg.tcpDC_Low_Ch2.WriteLine("OUTP?");
            //            Thread.Sleep(1000);
            //            //当前通道信息
            //            ChannelStatus_2 = int.Parse(dataCfg.tcpDC_Low_Ch2.ReadLine().Trim()) > 0 ? true : false;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("获取当前栅极数据 错误 ！" + ex);
            //    }
            //}
        }
        /// <summary>
        /// 任务下发成功判断
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        public bool DataSendSuccessJudge(byte register)
        {
            int count = 0;
            while (!windowModel.cts.IsCancellationRequested)
            {
                Thread.Sleep(100);
                count++;
                try
                {
                    if (dataCfg == null)
                    {
                        return false;
                    }
                    byte[] data = dataCfg.protocolDataTransceiver.GetCurrentProtocolData()?.Data;
                    if (data != null && data[7] == register && data[9] == 0xF8)
                    {
                        return true;
                    }
                    else if (data != null && data[7] == register && data[9] != 0xF8)
                    {
                        MessageBox.Show("寄存器配置异常");
                    }
                    if (count == 100)
                    {
                        MessageBox.Show("数据下发失败，请在复位成功后重试！");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("服务器异常！");
                }
            }
            return false;
        }
        /// <summary>
        /// 栅极配置成功判断
        /// </summary>
        /// <param name="JudgeInfo"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public bool PulseSendSuccessJudge(byte JudgeInfo, byte channel)
        {
            int count = 0;
            while (!windowModel.cts.IsCancellationRequested)
            {
                Thread.Sleep(100);
                count++;
                try
                {
                    if (dataCfg == null)
                    {
                        return false;
                    }
                    byte[] data = ViewModel.MainWindowModel.dataCfg.protocolDataTransceiver.GetCurrentProtocolData()?.Data;
                    if (data != null && data[7] == JudgeInfo && data[8] == 0)
                    {
                        return true;
                    }
                    else if (data != null && data[7] == JudgeInfo && data[8] != 0)
                    {
                        MessageBox.Show("异常! 通道号: " + channel.ToString());
                        return false;
                    }
                    if (count == 100)
                    {
                        MessageBox.Show("任务下发失败，请在复位成功后重试！");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("服务器异常！");
                }
            }
            return false;
        }
        /// <summary>
        /// 开尔文测试
        /// </summary>
        public bool KelVinTest()
        {
            int count = 0;
            while (!windowModel.cts.IsCancellationRequested)
            {
                Thread.Sleep(500);
                try
                {
                    byte[] data = dataCfg.protocolDataTransceiver.GetCurrentProtocolData()?.Data;
                    if (count == 50)
                    {
                        MessageBox.Show("开尔文测试超时未响应！");
                        //配置数据源  开尔文执行失败
                        windowModel.IsKelvinSuccess = false;
                        return false;
                    }
                    if (data[7] != 0xFF && data[7] != 0x15)
                    {
                        //执行异常
                        if (data[8] == 0xF5)
                        {
                            byte[] tmpdata = new byte[4] { data[12], data[11], data[10], data[9] };
                            float err = BitConverter.ToSingle(tmpdata, 0);
                            string ErrMsg = "错误通道：" + data[7].ToString() + "测量值： " + err.ToString();
                            KelvinTestString = ErrMsg;
                            KelvinTestStatus = false;

                            //配置数据源  开尔文执行失败
                            windowModel.IsKelvinSuccess = false;
                            return false;
                        }
                        else
                        {
                            //配置数据源  开尔文执行成功
                            IsKelvinSuccess = true;
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("开尔文等待测试结果时! info: " + ex.Message);
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// 关闭上一tab页面的任务
        /// </summary>
        /// <returns></returns>
        public bool CloseOldEnable()
        {
            if (EnableTabIndex == 0)
            {
                return true;
            }
            else if (EnableTabIndex == 1 || EnableTabIndex == 3)  //单脉冲测试
            {
                byte data = 0;
                try
                {
                    dataCfg.SendProtocolDataCFG((byte)CardType.GridDriveCard, (byte)FuncCode.SinglePulse, data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (PulseSendSuccessJudge(0, OldChannel))
                {
                    return true;
                }
            }
            else if (EnableTabIndex == 2)  // 多脉冲测试关闭
            {
                byte data = 0;
                try
                {
                    dataCfg.SendProtocolDataCFG((byte)CardType.GridDriveCard, (byte)FuncCode.DoublePulse, data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (PulseSendSuccessJudge(0, OldChannel))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
