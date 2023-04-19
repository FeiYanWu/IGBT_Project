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
    /// MultiPulseTest.xaml 的交互逻辑
    /// </summary>
    public partial class MultiPulseTest : UserControl
    {
        public float Volt;
        public float Ele;
        public float ProtectVolt;
        public float ProtectEle;
        public uint PulseWidth;
        private MainWindowModel windowModel;
        public MultiPulseTest()
        {
            InitializeComponent();

            tb_Volt.TextChanged += new TextChangedEventHandler(DataWidth);
            tb_SetEle.TextChanged += new TextChangedEventHandler(DataWidth);
            cbx_inductance.SelectionChanged += new SelectionChangedEventHandler(DataWidth);
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (windowModel == null)
                    windowModel = MainWindowModel.GetInstance();
            }
        }

        private void btn_PowerOn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PulseWidth = uint.Parse(tb_PulseWidth.Text.ToString());
                Volt = float.Parse(tb_Volt.Text.ToString());
                Ele = float.Parse(tb_Ele.Text.ToString());
                ProtectVolt = float.Parse(tb_VoltProtect.Text.ToString());
                ProtectEle = float.Parse(tb_EleProtect.Text.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            lock (MainWindowModel.dataCfg.tcpDC_High)
            {
                //参数配置
                MainWindowModel.dataCfg.tcpDC_High.WriteLine("VOLT" + Volt.ToString());
                MainWindowModel.dataCfg.tcpDC_High.WriteLine("CURR" + Ele.ToString());
                MainWindowModel.dataCfg.tcpDC_High.WriteLine("VOLT:PROT" + ProtectVolt.ToString());
                MainWindowModel.dataCfg.tcpDC_High.WriteLine("CURR:PROT" + ProtectEle.ToString());
                //执行
                MainWindowModel.dataCfg.tcpDC_High.WriteLine("OUTP:START");
            }

            windowModel.SetVolt = tb_Volt.Text;
            windowModel.SetEle = tb_Ele.Text;
            windowModel.ProtectEle = tb_EleProtect.Text;
            windowModel.ProtectVolt = tb_VoltProtect.Text;
        }

        private void btn_PowerOff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PulseWidth = uint.Parse(tb_PulseWidth.Text.ToString());
                Volt = float.Parse(tb_Volt.Text.ToString());
                Ele = float.Parse(tb_Ele.Text.ToString());
                ProtectVolt = float.Parse(tb_VoltProtect.ToString());
                ProtectEle = float.Parse(tb_EleProtect.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            lock (MainWindowModel.dataCfg.tcpDC_High)
            {
                MainWindowModel.dataCfg.tcpDC_High.WriteLine("OUTP:STOP");
            }
        }

        private void btn_PulseOutput_Click(object sender, RoutedEventArgs e)
        {
            btn_PulseOutput.IsEnabled = false;
            try
            {
                byte data = 0xFD;
                //正转180
                MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.GridDriveCard, (byte)FuncCode.DoublePulse, data);
            }
            catch (Exception ex)
            {
                btn_PulseOutput.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            windowModel.PulseSendSuccessJudge(0xFD, (byte)(cbx_select.SelectedIndex + 1));
        }
        /// <summary>
        /// 栅极正压输出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PVOutput_Click(object sender, RoutedEventArgs e)
        {
            windowModel.GridPositiveVolt_Set = tb_PositiveVolt.Text;

            MainWindowModel.dataCfg.DcPowerHandle.WriteCommand("CHAN 1");

            MainWindowModel.dataCfg.DcPowerHandle.WriteCommand("VOLT " + tb_PositiveVolt.Text);
            MainWindowModel.dataCfg.DcPowerHandle.WriteCommand("OUTP ON");
            Thread.Sleep(100);

            windowModel.IsDCGetValue = true;
        }
        /// <summary>
        /// 栅极负压输出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_NVOutput_Click(object sender, RoutedEventArgs e)
        {
            windowModel.GridNegativeVolt_Set = tb_NegativeVolt.Text;
            MainWindowModel.dataCfg.DcPowerHandle.WriteCommand("CHAN 2");
            MainWindowModel.dataCfg.DcPowerHandle.WriteCommand("VOLT " + tb_NegativeVolt.Text);
            MainWindowModel.dataCfg.DcPowerHandle.WriteCommand("OUTP ON");
            Thread.Sleep(100);
        }
        /// <summary>
        /// 充电
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Charge_Click(object sender, RoutedEventArgs e)
        {
            //单双脉冲、短路配置  通道配置
            btn_Charge.IsEnabled = false;
            try
            {
                byte data = 0xFD;
                //正转180
                MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.GridDriveCard, (byte)FuncCode.DoublePulse, data);
            }
            catch (Exception ex)
            {
                btn_Charge.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            windowModel.PulseSendSuccessJudge(0xFD, (byte)(cbx_select.SelectedIndex + 1));
        }

        private void btn_Discharge_Click(object sender, RoutedEventArgs e)
        {
            btn_Discharge.IsEnabled = false;
            try
            {
                byte[] data = new byte[10];
                data[0] = 0xC;
                data[2] = 2;
                data[3] = (byte)(cbx_select.SelectedIndex + 1);
                ViewModel.MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.PLCCard, (byte)FuncCode.ICES, data);
            }
            catch (Exception ex)
            {
                btn_Discharge.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            windowModel.DataSendSuccessJudge(0xC);
        }

        private void DataWidth(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                tb_PulseWidth.Text = (float.Parse(cbx_inductance.SelectionBoxItem.ToString()) * float.Parse(tb_SetEle.Text) / float.Parse(tb_Volt.Text)).ToString();
            }));
        }
        /// <summary>
        /// 栅极电阻启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ck_ResistanceEnable_Checked(object sender, RoutedEventArgs e)
        {
            ck_ResistanceEnable.IsEnabled = false;
            byte data = 0xFC;
            try
            {
                MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.GridDriveCard, (byte)FuncCode.DoublePulse, data);
            }
            catch (Exception ex)
            {
                ck_ResistanceEnable.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            windowModel.PulseSendSuccessJudge(data, (byte)(cbx_select.SelectedIndex+1));
        }
        /// <summary>
        /// 栅极电阻禁用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ck_ResistanceEnable_Unchecked(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 栅极通道改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbx_select.IsEnabled = false;
            byte data = (byte)(cbx_select.SelectedIndex + 1);
            try
            {
                //MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.GridDriveCard, (byte)FuncCode.DoublePulse, data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("通道修改失败 ! " + ex.Message);
            }
        }

        #region 使能手动配置
       
        /// <summary>
        /// 栅极通道改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool cbx_selectChanged()
        {
            cbx_select.IsEnabled = false;
            byte data = (byte)(cbx_select.SelectedIndex + 1);
            try
            {
                MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.GridDriveCard, (byte)FuncCode.SinglePulse, data);
            }
            catch (Exception ex)
            {
                cbx_select.IsEnabled = true;
                MessageBox.Show("服务链接异常 ! " + ex.Message);
                return false;
            }
            //数据响应判断
            if (windowModel.PulseSendSuccessJudge((byte)(cbx_select.SelectedIndex + 1), (byte)(cbx_select.SelectedIndex + 1)))
            {
                cbx_select.IsEnabled = true;
                return true;
            }
            cbx_select.IsEnabled = true;
            return false;
        }
        /// <summary>
        /// 开通电阻配置
        /// </summary>
        /// <returns></returns>
        private bool OpenResistanceSet()
        {
            byte[] data = new byte[10];
            data[0] = 10;
            data[2] = byte.Parse(tb_SetResistance.Text);
            try
            {
                MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.GridDriveCard, (byte)FuncCode.ParamerSet, data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务链接异常 ! " + ex.Message);
                return false;
            }
            if (windowModel.DataSendSuccessJudge(data[0]))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 关断电阻配置
        /// </summary>
        /// <returns></returns>
        private bool OffResistanceSet()
        {
            byte[] data = new byte[10];
            data[0] = 11;
            data[2] = byte.Parse(tb_OffResistance.Text);
            try
            {
                MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.GridDriveCard, (byte)FuncCode.ParamerSet, data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务链接异常 ! " + ex.Message);
                return false;
            }
            if (windowModel.DataSendSuccessJudge(data[0]))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 过流保护配置
        /// </summary>
        /// <returns></returns>
        private bool OverEleSet()
        {
            byte[] data = new byte[10];
            data[0] = 0;
            data[2] = byte.Parse(tb_OverEleProtect.Text);
            try
            {
                MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.LoadDetectionUnit, (byte)FuncCode.ParamerSet, data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务链接异常 ! " + ex.Message);
                return false;
            }
            if (windowModel.DataSendSuccessJudge(data[0]))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 第二脉冲宽度配置
        /// </summary>
        /// <returns></returns>
        private bool PulseWidth2Set()
        {
            uint pulse2width;
            byte[] tmpwidth;
            byte[] data;
            try
            {
                pulse2width = uint.Parse(tb_PulseWidth.Text);
                tmpwidth = BitConverter.GetBytes(pulse2width);
                data = new byte[10];
                data[0] = 3;
                data[2] = tmpwidth[0];
                data[3] = tmpwidth[1];
                data[4] = tmpwidth[2];
                data[5] = tmpwidth[3];
            }
            catch (Exception e)
            {
                MessageBox.Show("第二脉冲宽度配置错误!");
                return false;
            }
            try
            {
                MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.GridDriveCard, (byte)FuncCode.ParamerSet, data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务链接异常 ! " + ex.Message);
                return false;
            }
            if (windowModel.DataSendSuccessJudge(data[0]))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 第二脉冲宽度配置
        /// </summary>
        /// <returns></returns>
        private bool Pulsecount2Set()
        {
            uint pulse2Count;
            byte[] tmpwidth;
            byte[] data;
            try
            {
                pulse2Count = uint.Parse(tb_PulseWidth.Text);
                tmpwidth = BitConverter.GetBytes(pulse2Count);
                data = new byte[10];
                data[0] = 13;
                data[2] = tmpwidth[0];
                data[3] = tmpwidth[1];
                data[4] = tmpwidth[2];
                data[5] = tmpwidth[3];
            }
            catch (Exception e)
            {
                MessageBox.Show("第二脉冲重复次数配置错误!");
                return false;
            }
            try
            {
                MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.GridDriveCard, (byte)FuncCode.ParamerSet, data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务链接异常 ! " + ex.Message);
                return false;
            }
            if (windowModel.DataSendSuccessJudge(data[0]))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 脉冲间隔配置
        /// </summary>
        /// <returns></returns>
        private bool tb_PulseIntervalSet()
        {
            uint PulseInterval;
            byte[] tmpwidth;
            byte[] data;
            try
            {
                PulseInterval = uint.Parse(tb_PulseInterval.Text);
                tmpwidth = BitConverter.GetBytes(PulseInterval);
                data = new byte[10];
                data[0] = 4;
                data[2] = tmpwidth[0];
                data[3] = tmpwidth[1];
                data[4] = tmpwidth[2];
                data[5] = tmpwidth[3];
            }
            catch (Exception e)
            {
                MessageBox.Show("第二脉冲重复次数配置错误!");
                return false;
            }
            try
            {
                MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.GridDriveCard, (byte)FuncCode.ParamerSet, data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务链接异常 ! " + ex.Message);
                return false;
            }
            if (windowModel.DataSendSuccessJudge(data[0]))
            {
                return true;
            }
            return false;
        }

        private void btn_MultiPulseEnable_Click(object sender, RoutedEventArgs e)
        {
            btn_MultiPulseEnable.IsEnabled = false;
            //先关闭前一tab页面任务
            if (windowModel.CloseOldEnable())
            {
                try
                {
                    //通道配置
                    if (!cbx_selectChanged())
                    {
                        btn_MultiPulseEnable.IsEnabled = true;
                        return;
                    }
                    //开通电阻配置
                    if (!OpenResistanceSet())
                    {
                        btn_MultiPulseEnable.IsEnabled = true;
                        return;
                    }
                    //关断电阻配置
                    if (!OffResistanceSet())
                    {
                        btn_MultiPulseEnable.IsEnabled = true;
                        return;
                    }
                    //过流保护配置
                    if (!OverEleSet())
                    {
                        btn_MultiPulseEnable.IsEnabled = true;
                        return;
                    }
                    //第二脉冲宽度设置
                    if (!PulseWidth2Set())
                    {
                        btn_MultiPulseEnable.IsEnabled = true;
                        return;
                    }
                    //第二脉冲重复次数配置
                    if (!Pulsecount2Set())
                    {
                        btn_MultiPulseEnable.IsEnabled = true;
                        return;
                    }
                    //双脉冲间隔配置
                    if (!tb_PulseIntervalSet())
                    {
                        btn_MultiPulseEnable.IsEnabled = true;
                        return;
                    }
                    byte data = 0xFF;
                    MainWindowModel.dataCfg.SendProtocolDataCFG((byte)CardType.GridDriveCard, (byte)FuncCode.DoublePulse, data);
                }
                catch (Exception ex)
                {
                    btn_Discharge.IsEnabled = true;
                    MessageBox.Show(ex.Message);
                }
                if (windowModel.PulseSendSuccessJudge(0xFF, (byte)(cbx_select.SelectedIndex + 1)))
                {
                    windowModel.EnableTabIndex = 2;
                    windowModel.OldChannel = (byte)(cbx_select.SelectedIndex + 1);
                }
            }
        }
        #endregion
    }
}
