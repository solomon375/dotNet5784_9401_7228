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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        public static int ID = 0;

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public TaskWindow(int Id = 0)
        {
            InitializeComponent();

            if (Id == 0)
            {
                CurrentTask = new BO.Task();
                ID = CurrentTask.Id;
            }
            else
            {
                try
                {
                    CurrentTask = s_bl.task.Read(Id);
                    ID = CurrentTask.Id;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                    Close();
                }
            }
        }

        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }

        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ID == 0)
                {
                    s_bl.task.Create(CurrentTask);
                    MessageBox.Show("Task added successfully!");
                }
                else
                {
                    s_bl.task.Update(CurrentTask);
                    MessageBox.Show("Task updated successfully!");
                }

                Close();

                new TaskListWindow().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<int> dep = s_bl.task.choiceDependecy(CurrentTask);
            string concatenatedString = string.Join(",", dep);
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("enter a dependent from this list", concatenatedString, "0");
            int number;
            if (int.TryParse(userInput, out number)) { }
            s_bl.task.addDependecy(CurrentTask, number);

            Close();

            new TaskWindow(CurrentTask.Id).Show();
        }
    }
}
