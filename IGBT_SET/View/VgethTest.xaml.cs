using IGBT_SET.ViewModel;
using IGBT_V2Helper;
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
using static IGBT_V2Helper.IGBStructs;

namespace IGBT_SET.View
{
    /// <summary>
    /// Interaction logic for VgethTest.xaml
    /// </summary>
    public partial class VgethTest : UserControl
    {
        private MainWindowModel windowModel;

        public VgethTest()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (windowModel == null)
                    windowModel = MainWindowModel.GetInstance();
                MainWindowModel.devManager.ClearAllFault();
                InitData();
            }
        }

        private void InitData()
        {
            igbt_fix_para_t igbtPara = new igbt_fix_para_t();
            MainWindowModel.devManager.wl7016Helper.GetIGBTPara(ref igbtPara);
            tb_SetGeCurrent.Text = igbtPara.vgeth_fix_para.vgeth_special_fix_para[0].smu_ge_current.ToString();
        }

        private void btn_VgethParam_Click(object sender, RoutedEventArgs e)
        {
            if (cbx_select.SelectedIndex == 0)
            {
                MessageBox.Show("请先选择测试单元");
                return;
            }

            igbt_fix_para_t igbtPara = new igbt_fix_para_t();
            MainWindowModel.devManager.wl7016Helper.GetIGBTPara(ref igbtPara);

            for (int i = 0; i < igbtPara.iges_fix_para.iges_special_fix_para.Length; i++)
            {
                igbtPara.vgeth_fix_para.vgeth_special_fix_para[i].smu_ge_current = Convert.ToDouble(tb_SetGeCurrent.Text);

            }
            igbtPara.vgeth_fix_para.vgeth_public_fix_para.strategy = new byte[24];
            igbtPara.vgeth_fix_para.vgeth_public_fix_para.strategy[0] = Convert.ToByte(cbx_select.SelectedIndex);

            igbtPara.sequence_fix_para.sequence_public_fix_para.sequence = new byte[24];
            igbtPara.sequence_fix_para.sequence_public_fix_para.sequence[0] = (byte)TestItemsEnum.ITEM_VGETH;

            if (MainWindowModel.devManager.wl7016Helper.SetIGBTPara(ref igbtPara) &&

                 MainWindowModel.devManager.wl7505Helper.SetIGBTPara(ref igbtPara) &&
                 MainWindowModel.devManager.wl7001Helper.SetIGBTPara(ref igbtPara) &&
                 MainWindowModel.devManager.wl7010Helper.SetIGBTPara(ref igbtPara) &&
                 MainWindowModel.devManager.wl7001Helper.SetIGBTPara(ref igbtPara) &&
                 MainWindowModel.devManager.wl7011Helper.SetIGBTPara(ref igbtPara) &&
                 MainWindowModel.devManager.wL751301Helper.SetIGBTPara(ref igbtPara) &&
                 MainWindowModel.devManager.wL751302Helper.SetIGBTPara(ref igbtPara))
            {
                MessageBox.Show("测试准备完成");
            }
            else
            {
                MessageBox.Show("测试准备失败，下发参数错误");
            }
        }

        private void btn_VgethTest_Click(object sender, RoutedEventArgs e)
        {
            tb_TestResult.Text = "";
            if (cbx_select.SelectedIndex == 0)
            {
                MessageBox.Show("请先选择测试单元");
                return;
            }



            if (MainWindowModel.devManager.wl7016Helper.ExecuteSequence())
            {
                Thread.Sleep(1000);
                uint length = MainWindowModel.devManager.wl7005Helper.GetResultVGETHLength();
                if (length > 0)
                {
                    result_vgeth_t[] resultVgethArray;

                    if (MainWindowModel.devManager.wl7005Helper.GetResultVgethArray(out resultVgethArray, length, ref length))
                    {
                        tb_TestResult.Text = "Vgeth电流：" + resultVgethArray[0].vgeth_current.ToString() + "\r\n" + "Vgeth电压：" + resultVgethArray[0].vgeth.ToString();
                    }
                }
            }

        }

        private void cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
