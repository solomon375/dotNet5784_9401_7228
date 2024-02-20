using BO;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EngineerListWindow()
        {
            InitializeComponent();

            engineerList = s_bl?.engineer.ReadAll()!;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EngineerWindow engineerWindow = new EngineerWindow(0);
            engineerWindow.ShowDialog();

            Close();
        }

        private void lv_MouseButtonEventArgs(object sender, MouseButtonEventArgs e)
        {
            BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
            int id = engineer.Id;

            EngineerWindow engineerWindow = new EngineerWindow(id);
            engineerWindow.ShowDialog();

            Close();
        }

        public IEnumerable<BO.Engineer> engineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("engineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));


        public BO.EngineerExperience? experience { get; set; } = BO.EngineerExperience.None;


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEngineerList();
        }

        private void UpdateEngineerList()
        {
            engineerList = (experience == BO.EngineerExperience.None)?
                s_bl?.engineer.ReadAll()! : s_bl?.engineer.ReadAll(item => (BO.EngineerExperience?)item.level == experience)!;
        }
    }
}
