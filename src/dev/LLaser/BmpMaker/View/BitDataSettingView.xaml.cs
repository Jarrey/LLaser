//********************************************************* 
// LLaser project - BitDataSettingView.xaml.cs
// Created at 2013-6-21
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

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
using LLaser.ViewModel;

namespace LLaser.View
{
    /// <summary>
    /// Interaction logic for BitdataSettingView.xaml
    /// </summary>
    public partial class BitDataSettingView : Window
    {
        public BitDataSettingView(BitDataSettingViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
