using BO;
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

        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }

        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

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

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ID == 0)
                {
                    CurrentTask.Dependencies = new List<TaskInList>();
                    s_bl.task.Create(CurrentTask);
                    MessageBox.Show("Engineer added successfully!");
                }
                else
                {
                    CurrentTask.Id = ID;
                    s_bl.task.Update(CurrentTask);
                    MessageBox.Show("Engineer updated successfully!");
                }

                Close();

                new TaskListWindow().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

    }
}
