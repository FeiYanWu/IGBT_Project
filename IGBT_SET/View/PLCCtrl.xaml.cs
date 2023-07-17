using IGBT_SET.ViewModel;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

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
            }

            if (MainWindowModel.devManager.siemensS1200Helper.ReadBoolData("DB5.1.0"))
            {
                btn_tempterature_Open.IsEnabled = false;
                btn_tempterature_Close.IsEnabled = true;
            }
            else
            {
                btn_tempterature_Open.IsEnabled = true;
                btn_tempterature_Close.IsEnabled = false;
            }

            float productLocation = MainWindowModel.devManager.siemensS1200Helper.ReadFloat("DB5.18.0");
            tb_product_location.Text = productLocation.ToString("2");
        }


        #region 正反转重置
        /// <summary>
        /// 反转180°
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reversal_Click(object sender, RoutedEventArgs e)
        {
            btn_reversal.IsEnabled = false;
            try
            {
                if (WindowModel.ProductUpedSignal)
                {
                    MessageBox.Show("请先将产品降下去");
                    return;
                }

                if (WindowModel.NeedleDownedSignal)
                {
                    MessageBox.Show("请先将针床升上去");
                    return;
                }

                //马达使能
                MainWindowModel.devManager.siemensS1200Helper.WriteBoolData("DB5.1.7", true);

                MainWindowModel.devManager.siemensS1200Helper.WriteBoolData("DB5.2.2", true);
                MainWindowModel.devManager.siemensS1200Helper.WriteBoolData("DB5.2.1", true);

            }
            catch (Exception ex)
            {
                btn_reversal.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btn_reversal.IsEnabled = true;
            }
            

        }
        /// <summary>
        /// 正转到0°
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_foreward_Click(object sender, RoutedEventArgs e)
        {
            btn_foreward.IsEnabled = false;
            try
            {
                if (WindowModel.ProductUpedSignal)
                {
                    MessageBox.Show("请先将产品降下去");
                    return;
                }

                if (WindowModel.NeedleDownedSignal)
                {
                    MessageBox.Show("请先将针床升上去");
                    return;
                }
                //马达使能
                MainWindowModel.devManager.siemensS1200Helper.WriteBoolData("DB5.1.7", true);

                MainWindowModel.devManager.siemensS1200Helper.WriteBoolData("DB5.2.2", true);
                MainWindowModel.devManager.siemensS1200Helper.WriteBoolData("DB5.1.4", true);

            }
            catch (Exception ex)
            {
                btn_foreward.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btn_foreward.IsEnabled = true;
            }
        }
        /// <summary>
        /// 回原点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            btn_reset.IsEnabled = false;
            try
            {
                if (WindowModel.ProductUpedSignal)
                {
                    MessageBox.Show("请先将产品降下去");
                    return;
                }

                if (WindowModel.NeedleDownedSignal)
                {
                    MessageBox.Show("请先将针床升上去");
                    return;
                }
                //马达使能
                MainWindowModel.devManager.siemensS1200Helper.WriteBoolData("DB5.1.7", true);

                MainWindowModel.devManager.siemensS1200Helper.WriteBoolData("DB5.2.2", true);
                MainWindowModel.devManager.siemensS1200Helper.WriteBoolData("DB5.1.1", true);
            }
            catch (Exception ex)
            {
                btn_reset.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btn_reset.IsEnabled = true;
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
                if (!WindowModel.ProductUpedSignal)
                {
                    if (WindowModel.NeedleDownedSignal)
                    {
                        MessageBox.Show("请先将针床升起");
                        return;
                    }

                    //产品上升
                    MainWindowModel.devManager.siemensS1200Helper.WriteBoolData("DB5.2.6", true);
                    WindowModel.ProductDownedSignal = false;
                }
                else
                {
                    //产品下降
                    MainWindowModel.devManager.siemensS1200Helper.WriteBoolData("DB5.2.5", true);
                    WindowModel.ProductDownedSignal = true;
                }
            }
            catch (Exception ex)
            {
                btn_Product.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            finally
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
                if (WindowModel.NeedleUpedSignal)
                {
                    if (WindowModel.ProductUpedSignal)
                    {
                        MessageBox.Show("请先将产品降下去");
                        return;
                    }
                    MainWindowModel.devManager.wL751301Helper.NeedleBedOperation(1);   //配置针床下降
                    WindowModel.NeedleUpedSignal = false;
                }
                else
                {
                    MainWindowModel.devManager.wL751301Helper.NeedleBedOperation(0);
                    WindowModel.NeedleDownedSignal = false;
                }
            }
            catch (Exception ex)
            {
                btn_Needle.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
            finally
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
                float data = float.Parse(tb_SetTemp.Text);
                MainWindowModel.devManager.siemensS1200Helper.WriteFloat("DB5.24.0", data);
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
              
                MainWindowModel.devManager.siemensS1200Helper.WriteBoolData("DB5.1.0",true);
            }
            catch (Exception ex)
                {
                    btn_tempterature_Open.IsEnabled = true;
                    MessageBox.Show(ex.Message);
                }
            btn_tempterature_Close.IsEnabled = true;
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

                MainWindowModel.devManager.siemensS1200Helper.WriteBoolData("DB5.1.0", false);
            }
                catch (Exception ex)
                {
                    btn_tempterature_Close.IsEnabled = true;
                    MessageBox.Show(ex.Message);
                }
            btn_tempterature_Open.IsEnabled = true;
        }
        #endregion

        private void btn_productlocation_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tb_product_location.Text))
            {
                MessageBox.Show("电缸位置不能为空");
                return;
            }
            float data = float.Parse(tb_product_location.Text);
            MainWindowModel.devManager.siemensS1200Helper.WriteFloat("DB5.18.0", data);
        }
    }
}
