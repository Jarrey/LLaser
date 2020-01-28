//********************************************************* 
// LLaser project - ColorSelectView.xaml.cs
// Created at 2013-6-16
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.Windows;
using LLaser.ViewModel;

namespace LLaser.View
{
    /// <summary>
    /// Interaction logic for ColorSelectView.xaml
    /// </summary>
    public partial class ColorSelectView : Window
    {
        public ColorSelectView(ColorSelectViewModel viewModel)
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
