using IGBT_SET.ViewModel;
using IGBT_V2Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
using static IGBT_V2Helper.IGBStructs;

namespace IGBT_SET.View
{
    /// <summary>
    /// SystemConfig.xaml 的交互逻辑
    /// </summary>
    public partial class SystemConfig : UserControl
    {
        private MainWindowModel windowModel;
        private SystemConfigViewModel systemConfigViewModel;
        public SystemConfig()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (windowModel == null)
                    windowModel = MainWindowModel.GetInstance();
                    systemConfigViewModel = SystemConfigViewModel.GetInstance();



            }

            MainWindowModel.devManager.ClearAllFault();
        }



        private void smu_cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (smu_cbx_select.SelectedIndex == 0) return;
            byte channel = (byte)(smu_cbx_select.SelectedIndex);
            byte[] readyData = ProtocolData.Get485Datas(new byte[1] { channel }, (byte)HardwareAddressEnum.HW_ADDR_SMU, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_READY);
            MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_SMU, readyData, (uint)readyData.Length);
        }

        private void gd_cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gd_cbx_select.SelectedIndex == 0) return;
            byte channel = (byte)(gd_cbx_select.SelectedIndex );
            byte[] readyData = ProtocolData.Get485Datas(new byte[1] { channel }, (byte)HardwareAddressEnum.HW_ADDR_GD, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_READY);
            MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_GD, readyData, (uint)readyData.Length);

        }

        private void ccs_cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ccs_cbx_select.SelectedIndex == 0) return;
            byte channel = (byte)(ccs_cbx_select.SelectedIndex);
            byte[] readyData = ProtocolData.Get485Datas(new byte[1] { channel }, (byte)HardwareAddressEnum.HW_ADDR_CCS, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_READY);
            MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_CCS, readyData, (uint)readyData.Length);

        }

        private void hv_cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (hv_cbx_select.SelectedIndex == 0) return;

            byte channel = (byte)(hv_cbx_select.SelectedIndex);
            byte[] readyData = ProtocolData.Get485Datas(new byte[1] { channel }, (byte)HardwareAddressEnum.HW_ADDR_HV, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_READY);
            MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_HV, readyData, (uint)readyData.Length);

        }

        private void gp_cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gp_cbx_select.SelectedIndex == 0) return;
            byte channel = (byte)(gp_cbx_select.SelectedIndex);
            byte[] readyData = ProtocolData.Get485Datas(new byte[1] { channel }, (byte)HardwareAddressEnum.HW_ADDR_GP, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_READY);
            MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_GP, readyData, (uint)readyData.Length);

        }

        private void rt1_cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rt1_cbx_select.SelectedIndex == 0) return;

            byte channel = (byte)(rt1_cbx_select.SelectedIndex);
            byte[] readyData = ProtocolData.Get485Datas(new byte[1] { channel }, (byte)HardwareAddressEnum.HW_ADDR_RT1000_01, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_READY);
            MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_RT1000_01, readyData, (uint)readyData.Length);

        }

        private void rt2_cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rt2_cbx_select.SelectedIndex == 0) return;
            byte channel = (byte)(rt2_cbx_select.SelectedIndex);
            byte[] readyData = ProtocolData.Get485Datas(new byte[1] { channel }, (byte)HardwareAddressEnum.HW_ADDR_RT1000_02, (byte)TestItemsEnum.ITEM_MPULSE, (byte)FunctionCodeEnum.FUNC_TEST_READY);
            MainWindowModel.devManager.wl7016Helper.Write485Data((byte)HardwareAddressEnum.HW_ADDR_RT1000_02, readyData, (uint)readyData.Length);
        }

        private void btn_UpRogwskiCoil_Click(object sender, RoutedEventArgs e)
        {
            rt1000_info_t rt1000Info = MainWindowModel.devManager.wL751301Helper.GetRt100001Info();


            //19 == 1 代表挨着针床， 会上升
            if (rt1000Info.rt_t2_01_di[19] == 0)
            {
                //上罗氏线圈下降挨着板卡
                MainWindowModel.devManager.wL751301Helper.WriteGPIO(2059, 1);
            }
            else
            {
                MainWindowModel.devManager.wL751301Helper.WriteGPIO(2059, 0);
            }
        }

        private void btn_DownRogwskiCoil_Click(object sender, RoutedEventArgs e)
        {
            rt1000_info_t rt1000Info = MainWindowModel.devManager.wL751301Helper.GetRt100001Info();

            // 20 == 1 代表在下面没有挨着针床
            if (rt1000Info.rt_t2_01_di[20] == 1)
            {
                //下罗氏线圈上升
                MainWindowModel.devManager.wL751301Helper.WriteGPIO(2060, 1);
            }
            else
            {
                //下罗氏线圈下降
                MainWindowModel.devManager.wL751301Helper.WriteGPIO(2060, 0);
            }
        }

        private void clear_all_fault_Click(object sender, RoutedEventArgs e)
        {
            MainWindowModel.devManager.ClearAllFault();
        }

        private void query_fault_Click(object sender, RoutedEventArgs e)
        {
            tb_FaultInfo.Text = "";

            string[] faults = null;
            switch (board_cbx_select.SelectedIndex)
            {
                case 0:

                    faults = MainWindowModel.devManager.wl7016Helper.GetAllFaultMessage();
                    break;
                case 1:
                    faults = MainWindowModel.devManager.wl7020Helper.GetAllFaultMessage();
                    break;
                case 2:
                    faults = MainWindowModel.devManager.wl7005Helper.GetAllFaultMessage();
                    break;
                case 3:
                    faults = MainWindowModel.devManager.wl7001Helper.GetAllFaultMessage();
                    break;
                case 4:
                    faults = MainWindowModel.devManager.wl7010Helper.GetAllFaultMessage();

                    break;
                case 5:
                    faults = MainWindowModel.devManager.wl7505Helper.GetAllFaultMessage();
                    break;
                case 6:
                    faults = MainWindowModel.devManager.wl7011Helper.GetAllFaultMessage();

                    break;
                case 7:
                    faults = MainWindowModel.devManager.wL751301Helper.GetAllFaultMessage();
                    break;
                case 8:
                    faults = MainWindowModel.devManager.wL751302Helper.GetAllFaultMessage();
                    break;
                default:
                    break;
            }

            if (faults == null)
            {
                return;
            }

            if (faults.Length == 0) 
            {
                tb_FaultInfo.Text = "无故障";
            }

            if (faults.Length > 0)
            {
                foreach (string fault in faults)
                {
                    if (tb_FaultInfo.Text == "")
                    {
                        tb_FaultInfo.Text = fault;
                    }
                    else
                    {
                        tb_FaultInfo.Text = tb_FaultInfo.Text + "\r\n" + fault;
                    }
                }
            }
        }
    }
}
