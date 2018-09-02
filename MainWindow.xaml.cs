using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using BonnyUI.ViewModel;

namespace BonnyUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        MainViewModel vm;

        public MainWindow()
        {
            
            InitializeComponent();
            
            vm = (MainViewModel)this.TryFindResource("vm");
            if(vm != null)
            {
                // Commands aus dem Viewmodel initiailisieren

            }

            

            this.Closing += MainWindow_Closing;
            

        }

       

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Console.WriteLine("Programm geschlossen");
            Console.ReadLine();
        }

        
    }
}
