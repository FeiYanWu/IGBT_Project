
using IGBT_SET.Common;
using System.Windows.Media;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System;
using System.Text;
using Microsoft.Win32;
using IGBT_V2Helper;
using System.Windows.Threading;

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

        
        public static DevManager devManager;

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

        internal void PulseSendSuccessJudge(int v1, byte v2)
        {
            throw new NotImplementedException();
        }

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

        private DispatcherTimer timer;
        private bool revArrivedSignal;
        /// <summary>
        /// 反向180旋转到位信号 
        /// </summary>
        public bool REVArrivedSignal
        {
            get
            {
                return revArrivedSignal;
            }
            set
            {
                if (value != revArrivedSignal)
                {
                    revArrivedSignal = value;
                    RaisePropertyChanged("FWDArrivedSignal1");
                }
            }
        }
        private bool fwdArrivedSignal;
        /// <summary>
        /// 正转旋转到位信号 
        /// </summary>
        public bool FWDArrivedSignal
        {
            get
            {
                return fwdArrivedSignal;
            }
            set
            {
                if (value != fwdArrivedSignal)
                {
                    fwdArrivedSignal = value;
                    RaisePropertyChanged("FWDArrivedSignal2");
                }
            }
        }
        private bool resetDarrivedSignal;
        /// <summary>
        /// 复位旋转到位信号 
        /// </summary>
        public bool ResetArrivedSignal
        {
            get
            {
                return resetDarrivedSignal;
            }
            set
            {
                if (value != resetDarrivedSignal)
                {
                    resetDarrivedSignal = value;
                    RaisePropertyChanged("FWDArrivedSignal3");
                }
            }
        }

       

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

        private float operationTemperature_1;
        /// <summary>
        /// 工装1温度
        /// </summary>
        public float OperationTemperature_1
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

        private float operationTemperature_2;
        /// <summary>
        /// 工装2温度 
        /// </summary>
        public float OperationTemperature_2
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

        #region 高压电源值显示
        public void HighValueGet(object sender, DoWorkEventArgs e)
        {
            bool remote = false;
            while (!cts.IsCancellationRequested)
            {
                Thread.Sleep(1000);
                try
                {
                    lock (devManager.tcpDC_High)
                    {
                        if (!remote)
                        {
                            //设置远程模式
                            devManager.tcpDC_High.WriteLine("CONF:SETPT 3");
                            remote = true;
                        }
                        ////选择通道命令
                        //devManager.tcpDC_High.WriteLine("OUTP 1,(@1)");

                        //发送获取电压请求
                        devManager.tcpDC_High.WriteLine("MEAS:VOLT?");
                        Thread.Sleep(50);
                        //读取
                        CurVolt = devManager.tcpDC_High.ReadLine().Trim();

                        //读取当前通道 上电状态
                        devManager.tcpDC_High.WriteLine("OUTPut?");
                        Thread.Sleep(50);
                        //当前通道信息
                        IsOnline = int.Parse(devManager.tcpDC_High.ReadLine().Trim()) > 0 ? true : false;

                        //读电流
                        devManager.tcpDC_High.WriteLine("MEAS:CURR?");
                        Thread.Sleep(50);
                        //获取读取结果
                        CurEle = devManager.tcpDC_High.ReadLine().Trim();
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
        }
        #endregion

        #region 低压电源值显示
        public volatile bool IsDCGetValue = true;
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
                        devManager.DcPowerHandle.WriteCommand("SYST:REM");
                        remote = true;
                    }
                    devManager.DcPowerHandle.WriteCommand("CHANnel 1");
                    devManager.DcPowerHandle.ReadCommand("MEAS:VOLT?");
                    CurPositiveVolt = devManager.DcPowerHandle.ReadCommand("MEAS:VOLT?");
                    string outputStatus = devManager.DcPowerHandle.ReadCommand("OUTP?");
                    ChannelStatus_1 = int.Parse(outputStatus?.Trim()) > 0 ? true : false;


                    devManager.DcPowerHandle.WriteCommand("CHANnel 2");
                    devManager.DcPowerHandle.ReadCommand("MEAS:VOLT?");
                    CurNegativeVolt = devManager.DcPowerHandle.ReadCommand("MEAS:VOLT?");
                    string outputStatus1 = devManager.DcPowerHandle.ReadCommand("OUTP?");
                    ChannelStatus_2 = int.Parse(outputStatus1?.Trim()) > 0 ? true : false;
                }
            }
        }
        #endregion

        public void KelvinTest(IGBStructs.result_krw_t result_krw_t)
        {
            windowModel.KelvinTestString = "";
            if (result_krw_t.is_success == 1)
            {
                IsKelvinSuccess = true;
                KelvinTestStatus = true;
                windowModel.KelvinTestString = "";
            }
            if (result_krw_t.is_success == 0)
            {
                IsKelvinSuccess = false;
                KelvinTestStatus = false;
                KelvinTestString = "失败通道号：" + result_krw_t.err_ch + ",失败通道值：" + Math.Round(result_krw_t.err_cr_value, 3).ToString();
            }
        }
        private static MainWindowModel windowModel;
        public static DevCollocation devCollocation;
        public CancellationTokenSource cts = new CancellationTokenSource();
        public MainWindowModel()
        {
            if (windowModel == null)
            {
                if (devCollocation == null)
                    devCollocation = new DevCollocation();

               
                if (devManager == null)
                    devManager = new DevManager();

                BackgroundWorker highValueGet = new BackgroundWorker();
                highValueGet.DoWork += HighValueGet;
                highValueGet.RunWorkerAsync();

                BackgroundWorker lowValueGet = new BackgroundWorker();
                lowValueGet.DoWork += LowValueGet;
                lowValueGet.RunWorkerAsync();

                // 初始化定时器
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(1000); // 设置定时器间隔为10毫秒
                timer.Tick += Timer_Tick;
                timer.Start();

                windowModel = this;

                //KelvinTestStatus = true;
            }
        }
        /// <summary>
        /// 读取PLC信号状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            // 定时读取PLC数据并更新状态
            ProductUpedSignal = devManager.siemensS1200Helper.ReadBoolData("DB5.22.1"); //产品顶升到位信号
            ProductDownedSignal = devManager.siemensS1200Helper.ReadBoolData("DB5.22.3");//产品下到位信号

            IGBStructs.rt1000_info_t rt1000_info = devManager.wL751301Helper.GetRt100001Info();
            if (rt1000_info.rt_t2_01_di[16] == 1)
            {
                NeedleDownedSignal = true;
                NeedleUpedSignal = false;
            }
            if(rt1000_info.rt_t2_01_di[17] == 1)
            {
                NeedleUpedSignal = true;
                NeedleDownedSignal = false;
            }

            OperationTemperature_1 = MainWindowModel.devManager.siemensS1200Helper.ReadFloat("DB5.28.0");
            OperationTemperature_2 = MainWindowModel.devManager.siemensS1200Helper.ReadFloat("DB5.32.0");

            
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
        public bool CloseOldEnable()
        {
            if (EnableTabIndex == 0)
            {
                return true;
            }
            else if (EnableTabIndex == 1 || EnableTabIndex == 3)  //单脉冲测试
            {
               
               
            }
            else if (EnableTabIndex == 2)  // 多脉冲测试关闭
            {
               
            }
            return false;
        }
    }
}
