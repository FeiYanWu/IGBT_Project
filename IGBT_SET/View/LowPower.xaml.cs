using IGBT_SET.ViewModel;
using System.ComponentModel;
using System.Windows.Controls;


namespace IGBT_SET.View
{
    /// <summary>
    /// LowPower.xaml 的交互逻辑
    /// </summary>
    public partial class LowPower : UserControl
    {
        private MainWindowModel windowModel;
        public LowPower()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (windowModel == null)
                    windowModel = MainWindowModel.GetInstance();
            }
        }
    }
}
