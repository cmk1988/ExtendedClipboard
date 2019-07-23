﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExtendedClipboard
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel vm;
        public MainWindow()
        {
            vm = new MainViewModel(col);
            DataContext = vm;
            InitializeComponent();
        }

        // Less work than property changed implementation...
        void col(int i)
        {
            a.Foreground = Brushes.Black;
            b.Foreground = Brushes.Black;
            c.Foreground = Brushes.Black;
            d.Foreground = Brushes.Black;
            e.Foreground = Brushes.Black;
            f.Foreground = Brushes.Black;
            switch (i)
            {
                case 0:
                    a.Foreground = Brushes.Red;
                    break;
                case 1:
                    b.Foreground = Brushes.Red;
                    break;
                case 2:
                    c.Foreground = Brushes.Red;
                    break;
                case 3:
                    d.Foreground = Brushes.Red;
                    break;
                case 4:
                    e.Foreground = Brushes.Red;
                    break;
                case 5:
                    f.Foreground = Brushes.Red;
                    break;
            }
        }
    }
}
