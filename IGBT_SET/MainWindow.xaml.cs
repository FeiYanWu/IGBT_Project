using IGBT_SET.Common;
using IGBT_SET.ViewModel;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Wolei_485Trans;

namespace IGBT_SET
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow :Window
    {
        private MainWindowModel mainWindowModel;
        public MainWindow()
        {
            InitializeComponent();
            mainWindowModel = MainWindowModel.GetInstance();
            this.DataContext = mainWindowModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.ResizeMode =  ResizeMode.CanResize;

            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            DeskTopInit();
            SetHandModel();
        }

        private void DeskTopInit()
        {
            try
            {
                var window = new WinformCtrLib.RemoteCtrl();
                window.Server = MainWindowModel.devCollocation.devInfos["Oscilloscope"].DevIP;
                window.UserName = "LeCroyUser";
                window.Port = (int)MainWindowModel.devCollocation.devInfos["Oscilloscope"].Port;
                window.ClearTextPassword = "lecroyservice";
                window.InitView();

                window.KeyUp += window.window_KeyUp;
                window.DoubleClick += window.RemoteCtrl_DoubleClick;

                winform.Child = window;

                window.Show();

                //var window = new WinformCtrLib.RemoteCtrl();
                //window.Server = "192.168.71.128";
                //window.UserName = "lx";
                //window.Port = 3389;
                //window.ClearTextPassword = "admin";
                //window.InitView();

                //window.KeyUp += window.window_KeyUp;
                //window.DoubleClick += window.RemoteCtrl_DoubleClick;

                //winform.Child = window;

                //window.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SetHandModel()
        {
            try
            {
                byte[] data = new byte[10];
                data[0] = 8;
                data[2] = 1;
                //手动模式配置
                if (MainWindowModel.dataCfg != null)
                {
                    MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.SysMangerCard, (byte)FuncCode.ParamerSet, data, (byte)CardType.PLCCard);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("手动模式配置错误！ info ：" + ex.Message);
                this.Close();
            }
            if (mainWindowModel.DataSendSuccessJudge(13))
            {
                mainWindowModel.IsInitSuccess = true;
            }
        }

        private void tab_ctrl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (tab_ctrl.SelectedIndex == 0)
            {
                return;
            }
            //单脉冲或多脉冲
            if (tab_ctrl.SelectedIndex == 1 || tab_ctrl.SelectedIndex == 2)
            {
                try
                {
                    byte[] data = new byte[10];
                    data[0] = 0xA;
                    data[1] = 1;
                    data[2] = 1;
                    if (MainWindowModel.dataCfg != null)
                    {
                        MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.PLCCard, (byte)FuncCode.ICES, data);
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (tab_ctrl.SelectedIndex == 3)
            {
                try
                {
                    byte[] data = new byte[10];
                    data[0] = 0xA;
                    data[1] = 2;
                    data[2] = 1;
                    if (MainWindowModel.dataCfg != null)
                    {
                        MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.PLCCard, (byte)FuncCode.ICES, data);
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            mainWindowModel.DataSendSuccessJudge(0xA);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                byte[] data = new byte[10];
                data[0] = 8;
                data[2] = 2;
                //自动模式设置
                if (MainWindowModel.dataCfg != null)
                {
                    MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.PLCCard, (byte)FuncCode.ICES, data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("自动模式配置错误！ info ：" + ex.Message);
            }
            if (mainWindowModel.DataSendSuccessJudge(8))
            {
                mainWindowModel.IsInitSuccess = true;
            }
            mainWindowModel.cts.Cancel();
            Thread.Sleep(1000);
        }
    }
}
