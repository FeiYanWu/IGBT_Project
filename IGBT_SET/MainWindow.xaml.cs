using IGBT_SET.Common;
using IGBT_SET.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using static IGBT_V2Helper.IGBStructs;

namespace IGBT_SET
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow :Window
    {
        private MainWindowModel mainWindowModel;

        public string ProductModel;
        public MainWindow(string productModel)
        {
            InitializeComponent();
            mainWindowModel = MainWindowModel.GetInstance();
            this.DataContext = mainWindowModel;
            ProductModel = productModel;
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
            SetSystemInfo();
        }

        private void SetSystemInfo()
        {
            List<Product> p2list = XMLHelper.LoadXmlFromFile<List<Product>>("product.xml");

            Product product = p2list.FirstOrDefault(item=>ProductModel.Equals(item.Model));

            if(product != null)
            {
                
                igbt_fix_para_t igbtPara = new igbt_fix_para_t();
                MainWindowModel.devManager.wl7016Helper.GetIGBTPara(ref igbtPara);
                igbtPara.system_fix_para.smu_channel_map = StringToByteArray(product.SmuChannelMap);
                igbtPara.system_fix_para.ccs_channel_map = StringToByteArray(product.CcsChannelMap);
                igbtPara.system_fix_para.gd_channel_map = StringToByteArray(product.GdChannelMap);
                igbtPara.system_fix_para.gp_channel_map = StringToByteArray(product.GpChannelMap);
                igbtPara.system_fix_para.hv_channel_map = StringToByteArray(product.HvChannelMap);
                igbtPara.system_fix_para.rt1_channel_map = StringToByteArray(product.Rt1ChannelMap);
                igbtPara.system_fix_para.rt2_channel_map = StringToByteArray(product.Rt2ChannelMap);
                MainWindowModel.devManager.wl7016Helper.SetIGBTPara(ref igbtPara);
            }
        }

        public byte[] StringToByteArray(string str)
        {
            string[] numberStrings = str.Split(',');

            byte[] byteArray = new byte[8];
            for (int i = 0; i < numberStrings.Length; i++)
            {
                byte number = Convert.ToByte(numberStrings[i]);
                byteArray[i] = number;
            }
            return byteArray;
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
                MainWindowModel.devManager.siemensS1200Helper.WriteBoolData("DB5.0.0", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("手动模式配置错误！ info ：" + ex.Message);
                Environment.Exit(0);
            }
            mainWindowModel.IsInitSuccess = true;
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
                    for (uint i = 2048; i <= 2053; i++)
                    {
                        MainWindowModel.devManager.wL751301Helper.WriteGPIO(i, 0);
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
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                //plc 自动
                MainWindowModel.devManager?.siemensS1200Helper.WriteBoolData("DB5.0.0", false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("自动模式配置错误！ info ：" + ex.Message);
            }

            if (mainWindowModel != null)
            {
                mainWindowModel.IsInitSuccess = true;
                mainWindowModel.cts.Cancel();
            }
            Thread.Sleep(1000);
        }
    }
}
