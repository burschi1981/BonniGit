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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BonnyUI.View
{
    /// <summary>
    /// Interaction logic for BonTemplate.xaml
    /// </summary>
    public partial class BonTemplate : UserControl
    {
        public BonTemplate()
        {
            InitializeComponent();
        }



        private void ChangeBackground(object sender, RoutedEventArgs e)
        {
            Background = (Brush)Application.Current.MainWindow.FindResource("backgroundNina");
        }

        private void ChangeBackgroundBack(object sender, RoutedEventArgs e)
        {
            Background = (Brush)Application.Current.MainWindow.FindResource("backgroundMarc");
        }

        
    }
}
