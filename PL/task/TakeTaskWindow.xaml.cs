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
    /// Interaction logic for TakeTaskWindow.xaml
    /// </summary>
    public partial class TakeTaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public TakeTaskWindow(int ID)
        {
            InitializeComponent();

            CurrentEngineer = s_bl.engineer.Read(ID);

            taskList = s_bl.engineer.ListTaskCanTake(CurrentEngineer);
        }

        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer),
                typeof(TakeTaskWindow), new PropertyMetadata(null));

        public IEnumerable<DO.Task> taskList
        {
            get { return (IEnumerable<DO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("taskList", typeof(IEnumerable<DO.Task>),
                typeof(TakeTaskWindow), new PropertyMetadata(null));

        private void lv_MouseButtonEventArgs(object sender, MouseButtonEventArgs e)
        {
            DO.Task? task = (sender as ListView)?.SelectedItem as DO.Task;
            int id = task.Id;

            s_bl.engineer.takeTask(CurrentEngineer, id);
            //s_bl.task.updateTasksEngineer(id, CurrentEngineer.Id);

            Close();

            new MainEngineerWindow(CurrentEngineer.Id).Show();
        }
    }
}
