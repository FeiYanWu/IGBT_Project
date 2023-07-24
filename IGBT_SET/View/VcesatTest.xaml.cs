
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
    /// Interaction logic for VCESat.xaml
    /// </summary>
    public partial class VCESatTest : UserControl
    {
        private MainWindowModel windowModel;

        public VCESatTest()
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
            tb_ccsoutcurrent.Text = igbtPara.vcesat_fix_para.vcesat_special_fix_para[0].ccs_output_current_value.ToString();
        }

       
        private void cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
      
        private void btn_VcesatTest_Click(object sender, RoutedEventArgs e)
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
                            MessageBox.Show("读取VCESAT结果超时");
                            return;
                        }
                        length = MainWindowModel.devManager.wl7016Helper.GetResultVcesatLength();
                        n--;
                    }

                    if (length > 0)
                    {
                        result_vcesat_t[] resultVcesatArray;

                        if (MainWindowModel.devManager.wl7016Helper.GetResultVcesatArray(out resultVcesatArray, length, ref length))
                        {
                            tb_TestResult.Text = "VCESAT电压：" + resultVcesatArray[0].ce_voltage.ToString()+"V" + "\r\n";
                        }
                    }




                    uint curLength = 0;
                    n = 3;

                    while (curLength == 0)
                    {
                        Thread.Sleep(1000);
                        if (n == 0)
                        {
                            MessageBox.Show("读取VCESAT结果超时");
                            return;
                        }
                        curLength = MainWindowModel.devManager.wl7505Helper.GetResultVcesatLength();
                        n--;
                    }
                    if (curLength > 0)
                    {
                        result_vcesat_t[] resultVcesatArray;

                        if (MainWindowModel.devManager.wl7505Helper.GetResultVcesatArray(out resultVcesatArray, curLength, ref curLength))
                        {
                            tb_TestResult.Text = tb_TestResult.Text + "VCESAT电流：" + resultVcesatArray[0].ce_current + "A".ToString();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("获取结果超时");
                }
               
            }
        }

        private void btn_VcesatParam_Click(object sender, RoutedEventArgs e)
        {
            if (cbx_select.SelectedIndex == 0)
            {
                MessageBox.Show("请先选择测试单元");
                return;
            }


            btn_VcesatParam.IsEnabled = false;
            try
            {
                MainWindowModel.devManager.wL751301Helper.RogwskiCoilStaticTest();

                igbt_fix_para_t igbtPara = new igbt_fix_para_t();
                MainWindowModel.devManager.wl7016Helper.GetIGBTPara(ref igbtPara);

                for (int i = 0; i < igbtPara.iges_fix_para.iges_special_fix_para.Length; i++)
                {
                    igbtPara.vcesat_fix_para.vcesat_special_fix_para[i].ccs_output_current_value = Convert.ToDouble(tb_ccsoutcurrent.Text);

                }
                igbtPara.vcesat_fix_para.vcesat_public_fix_para.strategy = new byte[24];
                igbtPara.vcesat_fix_para.vcesat_public_fix_para.strategy[0] = Convert.ToByte(cbx_select.SelectedIndex);

                igbtPara.sequence_fix_para.sequence_public_fix_para.sequence = new byte[24];
                igbtPara.sequence_fix_para.sequence_public_fix_para.sequence[0] = (byte)TestItemsEnum.ITEM_VCESAT;


                MainWindowModel.devManager.wl7016Helper.ClearAllResult();
                MainWindowModel.devManager.wl7505Helper.ClearResult(TestItemsEnum.ITEM_VCESAT);

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
                btn_VcesatParam.IsEnabled = true;
            }
        }
    }
}
