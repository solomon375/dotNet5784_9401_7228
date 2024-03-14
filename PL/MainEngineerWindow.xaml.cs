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
    /// Interaction logic for MainEngineerWindow.xaml
    /// </summary>
    public partial class MainEngineerWindow : Window
    {
        public static int ID = 0;

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public MainEngineerWindow(int id)
        {
            if (id ==0)
            {
                Close();
                new MainWindow();
            }
            if (s_bl.engineer.Read(id) != null)
            {
                InitializeComponent();

                CurrentEngineer = s_bl.engineer.Read(id);
            }
            else
            {
                Close();

                MessageBoxResult result = MessageBox.Show("there isnt engineer with this id", "Massege", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                new MainWindow();
            }
        }

        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(MainEngineerWindow), new PropertyMetadata(null));

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new FullTaskWindow(CurrentEngineer.Task.Id).Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            s_bl.engineer.finishTask(CurrentEngineer);
            Close();
            new MainEngineerWindow(CurrentEngineer.Id).Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new TakeTaskWindow(CurrentEngineer.Id).Show();
            Close();
        }
    }
}
