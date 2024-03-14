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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        public static int ID = 0;

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EngineerWindow(int Id = 0)
        {
            InitializeComponent();

            if (Id == 0)
            {
                CurrentEngineer = new BO.Engineer();
                ID = CurrentEngineer.Id;
            }
            else
            {
                try
                {
                    CurrentEngineer = s_bl.engineer.Read(Id);
                    ID = CurrentEngineer.Id;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                    Close();
                }
            }
        }

        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));


        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ID == 0)
                {
                    s_bl.engineer.Create(CurrentEngineer);
                    MessageBox.Show("Engineer added successfully!");
                }
                else
                {
                    s_bl.engineer.Update(CurrentEngineer);
                    MessageBox.Show("Engineer updated successfully!");
                }

                Close();

                new EngineerListWindow().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new TakeTaskFromManigeerWindow(CurrentEngineer.Id).Show();
            Close();
        }

        private void TextBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MessageBox.Show("enter engineer cost between 0-150\n\nBeginner cost between 0-50\nAdvancedBeginner cost between 50-70\n" +
                "Intermidate cost between 70-90\n" +
                "Advanced cost between 90-100\nExpert cost between 100-150");
        }
    }
}
