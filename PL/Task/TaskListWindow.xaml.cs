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
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public TaskListWindow()
        {
            InitializeComponent();

            taskList = s_bl?.task.ReadAll()!;
        }
        private void btnAdd_click(object sender, RoutedEventArgs e)
        {
            TaskWindow taskWindow = new TaskWindow(0);
            taskWindow.ShowDialog();

            Close();
        }

        private void lv_MouseButtonEventArgs(object sender, MouseButtonEventArgs e)
        {
            BO.Task? task1 = (sender as ListView)?.SelectedItem as BO.Task;

            BO.Task? task = s_bl?.task.Read(task1.Id)!;
            int id = task1.Id;

            TaskWindow taskWindow = new TaskWindow(id);
            taskWindow.ShowDialog();

            Close();
        }

        public IEnumerable<BO.Task> taskList
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("taskList", typeof(IEnumerable<BO.Task>), typeof(TaskListWindow), new PropertyMetadata(null));


        public BO.Status? status { get; set; } = BO.Status.None;

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTaskList();
        }

        private void UpdateTaskList()
        {
            taskList = (status == BO.Status.None) ?
                s_bl?.task.ReadAll()! : s_bl?.task.ReadAll(item => (BO.Status?)item.status == status)!;
        }
    }
}
