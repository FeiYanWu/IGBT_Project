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
    /// IGESTest.xaml 的交互逻辑
    /// </summary>
    public partial class IGESTest : UserControl
    {
        private MainWindowModel windowModel;

        public IGESTest()
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

            tb_PosPulseAmp.Text = igbtPara.iges_fix_para.iges_special_fix_para[0].smu_ge_positive_pulse_voltage.ToString();
            tb_NegPulseAmp.Text = igbtPara.iges_fix_para.iges_special_fix_para[0].smu_ge_negative_pulse_voltage.ToString();
            tb_PosPulseWidth.Text = igbtPara.iges_fix_para.iges_special_fix_para[0].smu_ge_positive_pulse_width.ToString();
            tb_NegPulseWidth.Text = igbtPara.iges_fix_para.iges_special_fix_para[0].smu_ge_negative_pulse_width.ToString();
            tb_Pulse_Sampling_Time.Text = igbtPara.iges_fix_para.iges_public_fix_para.smu_ge_wave_sampling_time.ToString();
        }

        private void btn_IGESParam_Click(object sender, RoutedEventArgs e)
        {
            
            if (cbx_select.SelectedIndex == 0)
            {
                MessageBox.Show("请先选择测试单元");
                return;
            }
            btn_IGESParam.IsEnabled = false;

            try
            {
                MainWindowModel.devManager.wL751301Helper.RogwskiCoilStaticTest();
                igbt_fix_para_t igbtPara = new igbt_fix_para_t();
                MainWindowModel.devManager.wl7016Helper.GetIGBTPara(ref igbtPara);

                for (int i = 0; i < igbtPara.iges_fix_para.iges_special_fix_para.Length; i++)
                {
                    igbtPara.iges_fix_para.iges_special_fix_para[i].smu_ge_positive_pulse_voltage = Convert.ToDouble(tb_PosPulseAmp.Text);
                    igbtPara.iges_fix_para.iges_special_fix_para[i].smu_ge_negative_pulse_voltage = Convert.ToDouble(tb_NegPulseAmp.Text);
                    igbtPara.iges_fix_para.iges_special_fix_para[i].smu_ge_positive_pulse_width = Convert.ToUInt32(tb_PosPulseWidth.Text);
                    igbtPara.iges_fix_para.iges_special_fix_para[i].smu_ge_negative_pulse_width = Convert.ToUInt32(tb_NegPulseWidth.Text);

                }
                igbtPara.iges_fix_para.iges_public_fix_para.smu_ge_wave_sampling_time = Convert.ToUInt32(tb_Pulse_Sampling_Time.Text);

                igbtPara.iges_fix_para.iges_public_fix_para.strategy = new byte[24];
                igbtPara.iges_fix_para.iges_public_fix_para.strategy[0] = Convert.ToByte(cbx_select.SelectedIndex);

                igbtPara.sequence_fix_para.sequence_public_fix_para.sequence = new byte[24];
                igbtPara.sequence_fix_para.sequence_public_fix_para.sequence[0] = (byte)TestItemsEnum.ITEM_IGES;


                // 清除结果
                MainWindowModel.devManager.wl7005Helper.ClearResult(TestItemsEnum.ITEM_IGES);

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
                btn_IGESParam.IsEnabled = true;
            }
        }

        private void btn_IGESTest_Click(object sender, RoutedEventArgs e)
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
                            MessageBox.Show("读取IGES结果超时");
                            return;
                        }
                        length = MainWindowModel.devManager.wl7005Helper.GetResultIGESLength();
                        n--;
                    }


                    if (length > 0)
                    {
                        result_iges_t[] resultIGESArray;
                        if (MainWindowModel.devManager.wl7005Helper.GetResultIGESArray(out resultIGESArray, length, ref length))
                        {
                            tb_TestResult.Text = "正向电流：" + (resultIGESArray[0].positive_current * Math.Pow(10, 6)).ToString("#0.000")+"uA" + "\r\n" + "负向电流：" + (resultIGESArray[0].negative_current * Math.Pow(10, 6)).ToString("#0.000");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("获取结果超时"); 
                }
            }
        }

        private void cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
