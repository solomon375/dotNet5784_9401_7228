using BlApi;
using PL.Engineer;
using PL.Task;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        private readonly IBl _bl = Factory.Get();
        public ManagerWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void btnEngineers_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }
        private void btnTasks_click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().Show();
        }

        private void btnInitialization_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to Initialize?", "Massege", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _bl.InitializeDB();
                MessageBox.Show("Database initialized successfully!", "Initialize Database", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to Reset?", "Massege", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new GunttWindow().Show();
        }
    }
}
