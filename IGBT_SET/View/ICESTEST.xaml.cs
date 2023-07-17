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
    /// Interaction logic for ICESTEST.xaml
    /// </summary>
    public partial class ICESTEST : UserControl
    {
        private MainWindowModel windowModel;

        public ICESTEST()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (windowModel == null)
                    windowModel = MainWindowModel.GetInstance();
               
            }
            MainWindowModel.devManager.ClearAllFault();
            InitData();

        }

        private void InitData()
        {
            igbt_fix_para_t igbtPara = new igbt_fix_para_t();
            MainWindowModel.devManager.wl7016Helper.GetIGBTPara(ref igbtPara);
            hv_ce_pulse_voltage.Text = igbtPara.ices_fix_para.ices_special_fix_para[0].hv_ce_pulse_voltage.ToString();
        }
       
        private void btn_ICESParam_Click(object sender, RoutedEventArgs e)
        {
            if (cbx_select.SelectedIndex == 0)
            {
                MessageBox.Show("请先选择测试单元");
                return;
            }

            btn_ICESParam.IsEnabled = false;
            
            try
            {
                MainWindowModel.devManager.wL751301Helper.RogwskiCoilStaticTest();

                igbt_fix_para_t igbtPara = new igbt_fix_para_t();
                MainWindowModel.devManager.wl7016Helper.GetIGBTPara(ref igbtPara);

                for (int i = 0; i < igbtPara.ices_fix_para.ices_special_fix_para.Length; i++)
                {
                    igbtPara.ices_fix_para.ices_special_fix_para[i].hv_ce_pulse_voltage = Convert.ToDouble(hv_ce_pulse_voltage.Text);

                }
                igbtPara.ices_fix_para.ices_public_fix_para.strategy = new byte[24];
                igbtPara.ices_fix_para.ices_public_fix_para.strategy[0] = Convert.ToByte(cbx_select.SelectedIndex);

                igbtPara.sequence_fix_para.sequence_public_fix_para.sequence = new byte[24];
                igbtPara.sequence_fix_para.sequence_public_fix_para.sequence[0] = (byte)TestItemsEnum.ITEM_ICES;



                // 清除结果
                MainWindowModel.devManager.wl7016Helper.ClearAllResult();

                if (MainWindowModel.devManager.LoadParam(ref igbtPara))
                {
                    MessageBox.Show("测试准备完成");
                }
                else
                {
                    MessageBox.Show("测试准备失败，下发参数错误");
                }
            }
            finally
            {
                btn_ICESParam.IsEnabled = true;
            }
        }

        private void btn_ICESTest_Click(object sender, RoutedEventArgs e)
        {
            tb_TestResult.Text = "";
            if (cbx_select.SelectedIndex == 0)
            {
                MessageBox.Show("请先选择测试单元");
                return;
            }

            if (MainWindowModel.devManager.wl7016Helper.ExecuteSequence())
            {


                if (MainWindowModel.devManager.wl7016Helper.TestIsFinished())
                {
                    uint length = 0;
                    int n = 3;
                    while (length == 0)
                    {
                        Thread.Sleep(1000);
                        if (n == 0)
                        {
                            MessageBox.Show("读取ICES结果超时");
                            return;
                        }
                        length = MainWindowModel.devManager.wl7016Helper.GetResultICESLength();
                        n--;
                    }

                    if (length > 0)
                    {
                        result_ices_t[] resultICESArray;

                        if (MainWindowModel.devManager.wl7016Helper.GetResultICESArray(out resultICESArray, length, ref length))
                        {
                            tb_TestResult.Text = "ICES电流：" + resultICESArray[0].ce_current.ToString() + "\r\n" + "ICES电压：" + resultICESArray[0].ce_voltage.ToString();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("获取结果超时");
                }
            }
        }
    }
}
