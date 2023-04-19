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
    /// PLCCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class PLCCtrl : UserControl
    {

        private MainWindowModel WindowModel;

        public PLCCtrl()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (WindowModel == null)
                    WindowModel = MainWindowModel.GetInstance();
                //this.DataContext = WindowModel;
            }

            //BackgroundWorker SignalJudge = new BackgroundWorker();
            //SignalJudge.DoWork += PlcSignalJudge;
            //SignalJudge.RunWorkerAsync();
        }


        #region 正反转重置
        /// <summary>
        /// 正转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_foreward_Click(object sender, RoutedEventArgs e)
        {
            btn_foreward.IsEnabled = false;
            if (WindowModel.FWDArrivedSignal1)
            {
                btn_foreward.IsEnabled = true;
                MessageBox.Show("当前位置无法正转");
                return;
            }
            try
            {
                byte[] data = new byte[10];
                data[0] = 2;
                data[2] = 1;

                //正转180
                MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.PLCCard, (byte)FuncCode.ICES, data);
               
            }
            catch (Exception ex)
            {
                btn_foreward.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            if(WindowModel.DataSendSuccessJudge(2))
            {
                WindowModel.FWDArrivedSignal1 = false;
                WindowModel.FWDArrivedSignal2 = false;
                WindowModel.FWDArrivedSignal3 = false;
                WindowModel.TrayTurnDirection = 1;
                btn_foreward.IsEnabled = true;
            }
        }
        /// <summary>
        /// 反转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reversal_Click(object sender, RoutedEventArgs e)
        {
            btn_reversal.IsEnabled = false;
            if (WindowModel.FWDArrivedSignal2)
            {
                btn_reversal.IsEnabled = true;
                MessageBox.Show("当前位置无法反转");
                return;
            }
            try
            {
                byte[] data = new byte[10];
                data[0] = 2;
                data[2] = 2;
                //反转180
                ViewModel.MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.PLCCard, (byte)FuncCode.ICES, data);
            }
            catch (Exception ex)
            {
                btn_reversal.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            if (WindowModel.DataSendSuccessJudge(2))
            {
                WindowModel.FWDArrivedSignal1 = false;
                WindowModel.FWDArrivedSignal2 = false;
                WindowModel.FWDArrivedSignal3 = false;
                WindowModel.TrayTurnDirection = 2;
                btn_reversal.IsEnabled = true;
            }
        }
        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //旋转复位
                byte[] data = new byte[10];
                data[0] = 2;
                data[2] = 3;
                ViewModel.MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.PLCCard, (byte)FuncCode.ICES, data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务器异常");
            }
            if (WindowModel.DataSendSuccessJudge(2))
            {
                WindowModel.FWDArrivedSignal1 = false;
                WindowModel.FWDArrivedSignal2 = false;
                WindowModel.FWDArrivedSignal3 = false;
                WindowModel.TrayTurnDirection = 3;
            }
        }

        #endregion 

        #region 产品上升下降
        /// <summary>
        /// 产品上升下降
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Product_Click(object sender, RoutedEventArgs e)
        {
            btn_Product.IsEnabled = false;
            try
            {
                byte[] data = new byte[10];
                data[0] = 4;

                if (WindowModel.NeedleUpedSignal)
                {
                    data[2] = 1;   //配置托盘上升
                }
                else
                {
                    data[2] = 2;
                }
                //反转180
                MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.PLCCard, (byte)FuncCode.ICES, data);
                WindowModel.ProductDownedSignal = false;
                WindowModel.ProductUpedSignal = false;
            }
            catch (Exception ex)
            {
                btn_Product.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            if (WindowModel.DataSendSuccessJudge(4))
            {
                btn_Product.IsEnabled = true;
            }
        }
#endregion

        #region 针床上升下降
        /// <summary>
        /// 针床上升下降
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Needle_Click(object sender, RoutedEventArgs e)
        {
            btn_Needle.IsEnabled = false;
            try
            {
                byte[] data = new byte[10];
                data[0] = 6;

                if (WindowModel.ProductDownedSignal)
                {
                    data[2] = 2;   //配置针床下降
                }
                else
                {
                    data[2] = 1;
                }
                //反转180
                MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.PLCCard, (byte)FuncCode.ICES, data);
                WindowModel.NeedleDownedSignal = false;
                WindowModel.NeedleUpedSignal = false;
            }
            catch (Exception ex)
            {
                btn_Needle.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            if(WindowModel.DataSendSuccessJudge(6))
            {
                btn_Needle.IsEnabled = true;
            }
        }
#endregion

#region　温度设定

        /// <summary>
        /// 温度设定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_tempterature_Click(object sender, RoutedEventArgs e)
        {
            //获取界面数据
            int data = int.Parse(tb_SetTemp.Text);
            //将数据转换为byte[]
            byte[] senddata = BitConverter.GetBytes(data);
            //将数据转换为小端模式
            senddata = senddata.Reverse().ToArray();
            //数据发送
            ViewModel.MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.PLCCard, (byte)FuncCode.ICES, senddata);

            WindowModel.DataSendSuccessJudge(0);

        }
        /// <summary>
        /// 温度开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_tempterature_Open_Click(object sender, RoutedEventArgs e)
        {
            btn_tempterature_Open.IsEnabled = false;
            try
            {
                byte[] data = new byte[10];
                data[0] = 0xD;
                data[2] = 1;   
                ViewModel.MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.PLCCard, (byte)FuncCode.ICES, data);
            }
            catch (Exception ex)
            {
                btn_tempterature_Open.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            if (WindowModel.DataSendSuccessJudge(0xD))
            {
                btn_tempterature_Open.IsEnabled = true;
            }
        }
        /// <summary>
        /// 温度关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_tempterature_Close_Click(object sender, RoutedEventArgs e)
        {
            btn_tempterature_Close.IsEnabled = false;
            try
            {
                byte[] data = new byte[10];
                data[0] = 0xD;
                data[2] = 2;  
                ViewModel.MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.PLCCard, (byte)FuncCode.ICES, data);
            }
            catch (Exception ex)
            {
                btn_tempterature_Close.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            if (WindowModel.DataSendSuccessJudge(0xD))
            {
                btn_tempterature_Close.IsEnabled = true;
            }
        }
#endregion
    }
}
