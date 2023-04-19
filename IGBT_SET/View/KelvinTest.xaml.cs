using IGBT_SET.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wolei_485Trans;

namespace IGBT_SET.View
{
    /// <summary>
    /// KelvinTest.xaml 的交互逻辑
    /// </summary>
    public partial class KelvinTest : UserControl
    {
        private MainWindowModel windowModel;
        public byte[] KelvinCfg = new byte[24];
        public KelvinTest()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (windowModel == null)
                {
                    windowModel = MainWindowModel.GetInstance();
                }
            }
        }

        //全选
        private void btn_all_Click(object sender, RoutedEventArgs e)
        {
            KelvinCfg = Enumerable.Repeat((byte)0x01, 24).ToArray();
            foreach (var tmp in Kelvin_view.Children)
            {
                if(tmp is CheckBox )
                ((CheckBox)tmp).IsChecked = true;
            }
        }
        //清空
        private void btn_clr_Click(object sender, RoutedEventArgs e)
        {
            KelvinCfg = new byte[24];
            foreach (var tmp in Kelvin_view.Children)
            {
                if (tmp is CheckBox)
                ((CheckBox)tmp).IsChecked = false;
            }
        }
        //反选
        private void btn_inverse_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => {
                foreach (var tmp in Kelvin_view.Children)
                {
                    if (tmp is CheckBox)
                    {
                        if (((CheckBox)tmp).IsChecked == true)
                        {
                            ((CheckBox)tmp).IsChecked = false;
                            KelvinCfg[((CheckBox)tmp).TabIndex-1] = 0;
                        }
                        else
                        {
                            ((CheckBox)tmp).IsChecked = true;
                            KelvinCfg[((CheckBox)tmp).TabIndex-1] = 1;
                        }
                    }
                }
            }));
        }

        private bool ISWating = false;
        private bool ReadBackSuccess = false;

        #region 策略回读
        private void btn_CfgGet_Click(object sender, RoutedEventArgs e)
        {
            DataCfgRead();
          
        }
        /// <summary>
        /// 策略回读入口
        /// </summary>
        private void DataCfgRead()
        {
            byte[] data = new byte[2] { 21, 0 };
            //策略获取请求下发
            MainWindowModel.dataCfg.SendProtocolDataCFG_7016((byte)Dev_7016ActionType.Read, data);
            if (!ISWating)
            {
                ISWating = true;

                //判断返回值  配置成功执行
                Thread temp = new Thread(WaitingRecvBack);
                temp.Start();
            }
            
        }
        /// <summary>
        /// 等待回读结果
        /// </summary>
        private void WaitingRecvBack()
        {
            ReadBackSuccess = false;
            uint icnt;
            byte[] tmpdata;
            for (icnt = 0; icnt < 10; icnt++)
            {
                tmpdata = MainWindowModel.dataCfg.protocolDataTransceiver.GetCurrentProtocolData().Data;
                if (tmpdata == null || tmpdata.Length <6)
                {
                    Thread.Sleep(100);
                    continue;
                }
                if (tmpdata[0] == 21)
                {
                    uint TmpCfg = (uint)(tmpdata[2] + tmpdata[3]*0x100 + tmpdata[4] * 0x10000 + tmpdata[5]*0x1000000);
                    for (int jcnt = 0; jcnt < 24; jcnt++)
                    {
                        KelvinCfg[jcnt] = (byte)((TmpCfg >> jcnt) & 1);
                    }
                    UpdateUI();
                    ISWating = false;
                    ReadBackSuccess = true;
                    break;
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            if (icnt == 10)
            {
                MessageBox.Show("策略回读失败! ");
                ReadBackSuccess = false;
            }
            ISWating = false;
        }
        /// <summary>
        /// 更新界面UI
        /// </summary>
        private void UpdateUI()
        {
            Dispatcher.Invoke(new Action(() => {
                foreach (var tmp in Kelvin_view.Children)
                {
                    if (tmp is CheckBox)
                    {
                        if (KelvinCfg[((CheckBox)tmp).TabIndex - 1] == 0)
                        {
                            ((CheckBox)tmp).IsChecked = false;
                        }
                        else
                        {
                            ((CheckBox)tmp).IsChecked = true;
                        }
                    }
                }
            }));
        }
        #endregion

        #region  策略下发
        private void btn_CfgSet_Click(object sender, RoutedEventArgs e)
        {
            DataCfgSend();
        }
        /// <summary>  
        /// 策略下发入口
        /// </summary>
        private void DataCfgSend()
        {
            byte[] data = new byte[10];
            data[0] = 0x15;
            uint Cfg = 0;
            for (int icnt = 0; icnt < 24; icnt++)
            {
                Cfg += (uint)(KelvinCfg[icnt] << icnt);
            }
            byte[] tmp = BitConverter.GetBytes(Cfg);
            data[2] = tmp[0];
            data[3] = tmp[1];
            data[4] = tmp[2];
            data[5] = tmp[3];
            //策略下发

            MainWindowModel.dataCfg.SendProtocolDataCFG_7016((byte)Dev_7016ActionType.write,  data);
        }

        #endregion

        private void btn_EelvinExec_Click(object sender, RoutedEventArgs e)
        {
            btn_EelvinExec.IsEnabled = false;

            //策略回读
            DataCfgRead();
            if (ReadBackSuccess)
            {
                Thread.Sleep(200);

                //开始执行
                MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.KelvinCard, (byte)FuncCode.NeedleContact, 0xFF);

                //等待测试结果
                windowModel.KelVinTest();
            }
            btn_EelvinExec.IsEnabled = true;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                KelvinCfg[((CheckBox)sender).TabIndex-1] = 1;
            }
            else
            {
                KelvinCfg[((CheckBox)sender).TabIndex-1] = 0;
            }
        }
    }
}
