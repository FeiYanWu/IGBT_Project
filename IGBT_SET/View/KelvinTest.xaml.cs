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
            MainWindowModel.devManager.ClearAllFault();
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
            KelvinCfg = MainWindowModel.devManager.wl7020Helper.GetKrwStrategy();
            UpdateUI();
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
            uint Cfg = 0;
            for (int icnt = 0; icnt < 24; icnt++)
            {
                Cfg += (uint)(KelvinCfg[icnt] << icnt);
            }
            //策略下发
            bool isSuccess = MainWindowModel.devManager.wl7020Helper.SetKrwStrategy(Cfg);

            if (isSuccess)
            {
                MessageBox.Show("下发策略成功");
            }
        }

        #endregion


        #region 执行开尔文测试
        private void btn_EelvinExec_Click(object sender, RoutedEventArgs e)
        {
            btn_EelvinExec.IsEnabled = false;
            try
            {
                if (MainWindowModel.devManager.wl7016Helper.SetSequeenceInfo(TestItemsEnum.ITEM_KRW))
                {
                    if (MainWindowModel.devManager.wl7016Helper.ExecuteSequence())
                    {
                        uint length = MainWindowModel.devManager.wl7020Helper.GetResultKrwLength();
                        if (length > 0)
                        {
                            result_krw_t[] resultKrwArray;
                            if(MainWindowModel.devManager.wl7020Helper.GetResultKrwArray(out resultKrwArray,length,ref length))
                            {

                                windowModel.KelvinTest(resultKrwArray[0]);

                            }
                        }
                    }
                }
            }
            finally
            {
                btn_EelvinExec.IsEnabled = true;
            }
        }
        #endregion
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

        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == false)
            {
                KelvinCfg[((CheckBox)sender).TabIndex - 1] = 0;
            }
            else
            {
                KelvinCfg[((CheckBox)sender).TabIndex - 1] = 1;
            }
        }
    }
}
