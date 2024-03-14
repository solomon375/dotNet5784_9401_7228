using BlApi;
using PL.Engineer;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BlApi.IBl _bl = BlApi.Factory.Get();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            CurrentTime = _bl.Now;
        }

        private void btnManager_click(object sender, RoutedEventArgs e)
        {
            new ManagerWindow().Show();
        }

        private void bteEngineer_click(object sender, RoutedEventArgs e)
        {
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("enter your ID", "enter here", "0");
            int number;
            if (int.TryParse(userInput, out number)) { }
            new MainEngineerWindow(number).Show();
        }

        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(DateTime.Now));

        // מתודות לקידום השעון
        private void AdvanceHour_Click(object sender, RoutedEventArgs e)
        {
            _bl.AdvanceTimeByHour();
            CurrentTime = _bl.Now;
            _bl.UpdateTasksStatus();
        }

        private void AdvanceDay_Click(object sender, RoutedEventArgs e)
        {
            _bl.AdvanceTimeByDay();
            CurrentTime = _bl.Now;
            _bl.UpdateTasksStatus();
        }

        private void AdvanceMunth_Click(object sender, RoutedEventArgs e)
        {
            _bl.AdvanceTimeByMonth();
            CurrentTime = _bl.Now;
            _bl.UpdateTasksStatus();
        }

        // מתודה לאתחול השעון
        private void ResetClock_Click(object sender, RoutedEventArgs e)
        {
            _bl.ResetDB();
            CurrentTime = _bl.Now;
            _bl.UpdateTasksStatus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DonationWindow().Show();
        }
    }
}