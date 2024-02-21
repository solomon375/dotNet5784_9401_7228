﻿using BlApi;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnManager_click(object sender, RoutedEventArgs e)
        {
            new ManagerWindow().Show();
        }

        private void bteEngineer_click(object sender, RoutedEventArgs e)
        {
            new MainEngineerWindow().Show();
        }
    }
}