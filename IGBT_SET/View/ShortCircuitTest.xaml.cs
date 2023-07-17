using IGBT_SET.ViewModel;
using IGBT_V2Helper;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

using static IGBT_V2Helper.IGBStructs;
using ProtocolData = IGBT_V2Helper.ProtocolData;

namespace IGBT_SET.View
{
    /// <summary>
    /// ShortCircuitTest.xaml 的交互逻辑
    /// </summary>
    public partial class ShortCircuitTest : UserControl
    {
        public float Volt;
        public float Ele;
        public float ProtectVolt;
        public float ProtectEle;
        public uint PulseWidth;
        private MainWindowModel windowModel;
        public ShortCircuitTest()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (windowModel == null)
                    windowModel = MainWindowModel.GetInstance();
              
            }
            cbx_select.SelectionChanged += cbx_select_SelectionChanged;

            MainWindowModel.devManager.ClearAllFault();
            InitData();
        }

       
        private void InitData()
        {
            igbt_fix_para_t igbtPara = new igbt_fix_para_t();
            MainWindowModel.devManager.wl7016Helper.GetIGBTPara(ref igbtPara);

            tb_SetResistance.Text = igbtPara.lshort_fix_para.lshort_public_fix_para.gd_ge_on_resistance.ToString();
            tb_OffResistance.Text = igbtPara.lshort_fix_para.lshort_public_fix_para.gd_ge_off_resistance.ToString();

            cbb_rt1000_para_t rtPara = new cbb_rt1000_para_t();
            MainWindowModel.devManager.wL751301Helper.GetRTPara(ref rtPara);
            tb_OverEleProtect.Text = rtPara.dac_default_output[0].ToString();
            tb_OverEleProtect_down.Text = rtPara.dac_default_output[1].ToString();
        }


        #region 高压电源上电
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
            lock (MainWindowModel.devManager.tcpDC_High)
            {
                //参数配置
                MainWindowModel.devManager.tcpDC_High.WriteLine("VOLT" + Volt.ToString());
                MainWindowModel.devManager.tcpDC_High.WriteLine("CURR" + Ele.ToString());
                MainWindowModel.devManager.tcpDC_High.WriteLine("VOLT:PROT" + ProtectVolt.ToString());
                MainWindowModel.devManager.tcpDC_High.WriteLine("CURR:PROT" + ProtectEle.ToString());
                //执行
                MainWindowModel.devManager.tcpDC_High.WriteLine("OUTP:START");
            }

            windowModel.SetVolt = tb_Volt.Text;
            windowModel.SetEle = tb_Ele.Text;
            windowModel.ProtectEle = tb_EleProtect.Text;
            windowModel.ProtectVolt = tb_VoltProtect.Text;
        }
        #endregion

        #region 高压电源下电
        private void btn_PowerOff_Click(object sender, RoutedEventArgs e)
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
            lock (MainWindowModel.devManager.tcpDC_High)
            {
                MainWindowModel.devManager.tcpDC_High.WriteLine("OUTP:STOP");
            }
        }
        #endregion 高压电源下电

        #region 脉冲输出按钮
        private void btn_PulseOutput_Click(object sender, RoutedEventArgs e)
        {
            btn_PulseOutput.IsEnabled = false;
            try
            {
                bool s = MainWindowModel.devManager.wl7016Helper.Trigger(1);
            }
            catch (Exception ex)
            {
                btn_PulseOutput.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btn_PulseOutput.IsEnabled = true;
            }
        }
        #endregion  

        #region 栅极正压输出按钮
        private void btn_PVOutput_Click(object sender, RoutedEventArgs e)
        {
            windowModel.GridPositiveVolt_Set = tb_PositiveVolt.Text;

            MainWindowModel.devManager.DcPowerHandle.WriteCommand("CHAN 1");
            MainWindowModel.devManager.DcPowerHandle.WriteCommand("VOLT " + tb_PositiveVolt.Text);
            MainWindowModel.devManager.DcPowerHandle.WriteCommand("CURR " + 1);
            MainWindowModel.devManager.DcPowerHandle.WriteCommand("OUTP ON");
            Thread.Sleep(100);

            windowModel.IsDCGetValue = true;
        }
        #endregion

        #region 栅极负压输出按钮
        private void btn_NVOutput_Click(object sender, RoutedEventArgs e)
        {
            windowModel.GridNegativeVolt_Set = tb_NegativeVolt.Text;
            MainWindowModel.devManager.DcPowerHandle.WriteCommand("CHAN 2");
            MainWindowModel.devManager.DcPowerHandle.WriteCommand("VOLT " + tb_NegativeVolt.Text);
            MainWindowModel.devManager.DcPowerHandle.WriteCommand("CURR " + 1);
            MainWindowModel.devManager.DcPowerHandle.WriteCommand("OUTP ON");
            Thread.Sleep(100);
        }
        #endregion

        #region 充电按钮
        private void btn_Charge_Click(object sender, RoutedEventArgs e)
        {
            btn_Charge.IsEnabled = false;
            try
            {
                //MainWindowModel.devManager.wL751301Helper.WriteGPIO(2063,1);

                rt1000_info_t rt1000Info = MainWindowModel.devManager.wL751301Helper.GetRt100001Info();

                if (rt1000Info.rt_t2_01_di[22] == 0)
                {
                    //充电主级
                    MainWindowModel.devManager.wL751301Helper.WriteGPIO(2061, 0);
                }

                if (rt1000Info.rt_t2_01_di[24] == 0)
                {
                    //充电次级
                    MainWindowModel.devManager.wL751301Helper.WriteGPIO(2062, 0);
                }


                MessageBox.Show("充电成功");
            }
            catch (Exception ex)
            {
                btn_Charge.IsEnabled = true;
                MessageBox.Show("充电失败");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btn_Charge.IsEnabled = true;
            }
        }
        #endregion

        #region 放电按钮
        private void btn_Discharge_Click(object sender, RoutedEventArgs e)
        {
            btn_Discharge.IsEnabled = false;
            try
            {

                rt1000_info_t rt1000Info = MainWindowModel.devManager.wL751301Helper.GetRt100001Info();

                if (rt1000Info.rt_t2_01_di[22] == 1)
                {
                    //放电主级
                    MainWindowModel.devManager.wL751301Helper.WriteGPIO(2061, 1);
                }

                if (rt1000Info.rt_t2_01_di[24] == 1)
                {
                    //放电次级
                    MainWindowModel.devManager.wL751301Helper.WriteGPIO(2062, 1);
                }

                MainWindowModel.devManager.wL751301Helper.WriteGPIO(2062, 1);
                if (MainWindowModel.devManager.wL751301Helper.DisCharge())
                {
                    MessageBox.Show("放电成功");
                }
                else
                {
                    MessageBox.Show("放电失败");
                }
            }
            catch (Exception ex)
            {
                btn_Discharge.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btn_Discharge.IsEnabled = true;
            }
        }
        #endregion

        #region 选择测试单元
        private void cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            MainWindowModel.devManager.wl7016Helper.SetUnitStrategy(1);

            byte data = (byte)cbx_select.SelectedIndex;
            if (data == 0)
            {
                for (uint i = 2049; i <= 2056; i++)
                {
                    if (i == 2053 || i == 2055)
                    {
                        continue;
                    }
                    MainWindowModel.devManager.wL751301Helper.WriteGPIO(i, 0);
                }
            }
            else
            {

                for (uint i = 2049; i <= 2056; i++)
                {
                    if (i == 2053 || i == 2055)
                    {
                        continue;
                    }
                    MainWindowModel.devManager.wL751301Helper.WriteGPIO(i, 0);
                }
                cbx_select.IsEnabled = false;
                try
                {
                    uint bridgeArm = 0;
                    switch (data)
                    {
                        case 1:
                        case 2:
                            bridgeArm = 2049;
                            break;
                        case 3:
                        case 4:
                            bridgeArm = 2050;
                            break;
                        case 5:
                        case 6:
                            bridgeArm = 2051;
                            break;
                        case 7:
                        case 8:
                            bridgeArm = 2052;
                            break;
                    }
                    uint bridgeArmLocation = 0;
                    if (data == 1 || data == 3 || data == 5 || data == 7)
                    {
                        bridgeArmLocation = 2054;
                    }
                    else if (data == 2 || data == 4 || data == 6 || data == 8)
                    {
                        bridgeArmLocation = 2056;
                    }
                    MainWindowModel.devManager.wL751301Helper.WriteGPIO(bridgeArm, 1);
                    MainWindowModel.devManager.wL751301Helper.WriteGPIO(bridgeArmLocation, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("通道修改失败 ! " + ex.Message);
                }
                finally
                {
                    cbx_select.IsEnabled = true;
                }
            }
        }
        #endregion

        #region 参数下发

        /// <summary>
        /// 栅极通道改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool cbx_selectChanged()
        {
            byte data = (byte)cbx_select.SelectedIndex;

            if (data == 0)
            {
                MessageBox.Show("请先选择测试单元");
                return false;
            }
            return true;
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

                    igbt_fix_para_t igbtPara = new igbt_fix_para_t();
                    MainWindowModel.devManager.wl7016Helper.GetIGBTPara(ref igbtPara);

                    for (int i = 0; i < igbtPara.lshort_fix_para.lshort_special_fix_para.Length; i++)
                    {
                        igbtPara.lshort_fix_para.lshort_special_fix_para[i].rt01_oscilloscope_trigger_pulse1_width = Convert.ToUInt32(tb_PulseWidth.Text.ToString());
                        igbtPara.lshort_fix_para.lshort_special_fix_para[i].gd_double_pulse_trigger_pulse1_width = Convert.ToUInt32(tb_PulseWidth.Text.ToString());
                    }
                    igbtPara.lshort_fix_para.lshort_public_fix_para.gd_ge_on_resistance = Convert.ToUInt32(tb_SetResistance.Text.ToString());
                    igbtPara.lshort_fix_para.lshort_public_fix_para.gd_ge_off_resistance = Convert.ToUInt32(tb_OffResistance.Text.ToString());

                    MainWindowModel.devManager.LoadParam(ref igbtPara);


                    //// 保护电流
                    cbb_rt1000_para_t rtPara = new cbb_rt1000_para_t();
                    MainWindowModel.devManager.wL751301Helper.GetRTPara(ref rtPara);
                    rtPara.dac_default_output[0] = Convert.ToDouble(tb_OverEleProtect.Text.ToString());
                    rtPara.dac_default_output[1] = Convert.ToDouble(tb_OverEleProtect_down.Text.ToString());
                    MainWindowModel.devManager.wL751301Helper.SetRTPara(ref rtPara);



                    //罗氏线圈

                    MainWindowModel.devManager.wL751301Helper.RogwskiCoilDynamicTest();

                    //管理板-- > 其他板卡配置信息
                    //栅极驱动
                    byte[] gdConfigData = ProtocolData.Get485Datas(null, (byte)HardwareAddressEnum.HW_ADDR_GD, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_CONFIG);
                    MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_GD, gdConfigData, (uint)gdConfigData.Length);
                    //恒流源
                    byte[] cssConfigData = ProtocolData.Get485Datas(null, (byte)HardwareAddressEnum.HW_ADDR_CCS, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_CONFIG);
                    MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_CCS, cssConfigData, (uint)cssConfigData.Length);
                    //高压脉冲板
                    byte[] hvConfigData = ProtocolData.Get485Datas(null, (byte)HardwareAddressEnum.HW_ADDR_HV, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_CONFIG);
                    MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_HV, hvConfigData, (uint)hvConfigData.Length);
                    //栅极保护
                    byte[] gpConfigData = ProtocolData.Get485Datas(null, (byte)HardwareAddressEnum.HW_ADDR_GP, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_CONFIG);
                    MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_GP, gpConfigData, (uint)gpConfigData.Length);
                    //RT01
                    byte[] rt01ConfigData = ProtocolData.Get485Datas(null, (byte)HardwareAddressEnum.HW_ADDR_RT1000_01, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_CONFIG);
                    MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_RT1000_01, rt01ConfigData, (uint)rt01ConfigData.Length);


                    system_fix_para_t systemFixPara = new system_fix_para_t();
                    MainWindowModel.devManager.wl7016Helper.GetSystemPara(out systemFixPara);

                    byte data = (byte)cbx_select.SelectedIndex;

                    //栅极通道号
                    byte gdChannel = systemFixPara.gd_channel_map[data - 1];
                    //CSS通道号
                    byte cssChannel = systemFixPara.ccs_channel_map[data - 1];
                    //高压脉冲板通道号
                    byte hvChannel = systemFixPara.hv_channel_map[data - 1];
                    //栅极保护板通道号
                    byte gpChannel = systemFixPara.gp_channel_map[data - 1];
                    //栅极保护板通道号
                    byte rt01Channel = systemFixPara.rt1_channel_map[data - 1];


                    byte[] gdReadyData = ProtocolData.Get485Datas(new byte[1] { gdChannel }, (byte)HardwareAddressEnum.HW_ADDR_GD, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_READY);

                    byte[] cssReadyData = ProtocolData.Get485Datas(new byte[1] { cssChannel }, (byte)HardwareAddressEnum.HW_ADDR_CCS, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_READY);
                    byte[] hvReadyData = ProtocolData.Get485Datas(new byte[1] { hvChannel }, (byte)HardwareAddressEnum.HW_ADDR_HV, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_READY);

                    byte[] gpReadyData = ProtocolData.Get485Datas(new byte[1] { gpChannel }, (byte)HardwareAddressEnum.HW_ADDR_GP, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_READY);

                    byte[] rt01ReadyData = ProtocolData.Get485Datas(new byte[1] { gpChannel }, (byte)HardwareAddressEnum.HW_ADDR_RT1000_01, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_READY);


                    MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_GD, gdReadyData, (uint)gdReadyData.Length);
                    MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_CCS, cssReadyData, (uint)cssReadyData.Length);
                    MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_HV, hvReadyData, (uint)hvReadyData.Length);
                    MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_GP, gpReadyData, (uint)gpReadyData.Length);
                    MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_RT1000_01, rt01ReadyData, (uint)rt01ReadyData.Length);
                }
                catch (Exception ex)
                {
                    btn_Discharge.IsEnabled = true;
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    btn_MultiPulseEnable.IsEnabled = true;

                    MessageBox.Show("参数下发完成");
                }
            }
        }
        #endregion
    }
}

