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
    /// Interaction logic for FullTaskWindow.xaml
    /// </summary>
    public partial class FullTaskWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }

        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task),
                typeof(FullTaskWindow), new PropertyMetadata(null));
        public FullTaskWindow(int id)
        {
            InitializeComponent();

            CurrentTask = s_bl.task.Read(id);
        }
    }
}
