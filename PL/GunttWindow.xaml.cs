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
    /// Interaction logic for GunttWindow.xaml
    /// </summary>
    public partial class GunttWindow : Window
    {
        public BO.EngineerExperience? experience { get; set; } = BO.EngineerExperience.None;

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public IEnumerable<BO.Task> TaskList
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.Task>),
                typeof(GunttWindow), new PropertyMetadata(null));
        public GunttWindow()
        {
            InitializeComponent();
            TaskList = s_bl.task.ReadAll().OrderBy(x => x.ScheduledDate);
        }
    }
}
