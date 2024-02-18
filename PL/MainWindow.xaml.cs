using BlApi;
using PL.Engineer;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IBl _bl = Factory.Get();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnEngineers_Click(object sender, RoutedEventArgs e)
        { 
            new EngineerListWindow().Show(); 
        }
        private void btnInitialization_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to Initialize?", "Massege", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if(result == MessageBoxResult.Yes)
            {
                _bl.InitializeDB();
            }
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to Reset?", "Massege", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                _bl.ResetDB();
                MessageBoxResult result1 = MessageBox.Show("Do you want to Initialize?", "Massege", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result1 == MessageBoxResult.Yes)
                {
                    _bl.InitializeDB();
                }
            }
        }
        
    }
}