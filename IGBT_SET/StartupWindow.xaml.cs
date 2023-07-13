using IGBT_SET.Common;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace IGBT_SET
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();

            InitData();
        }

        private void InitData()
        {
            List<Product> p2list = XMLHelper.LoadXmlFromFile<List<Product>>("product.xml");
            IEnumerable<string> enumerable = p2list.Select(p=>p.Model);
            productList.DataContext = enumerable.ToArray();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string item = (string)productList.SelectedItem;
            // 关闭当前窗口
            Close();
            // 启动主界面窗口
            MainWindow mainWindow = new MainWindow(item);
            mainWindow.Show();

            
        }
    }
}
