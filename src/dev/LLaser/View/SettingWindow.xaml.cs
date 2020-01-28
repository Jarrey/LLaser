//********************************************************* 
// LLaser project - SettingWindow.xaml.cs
// Created at 2013-6-25
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.Windows;
using LLaser.Common;
using LLaser.ViewModel;

namespace LLaser.View
{

    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
            this.DataContext = new SettingWindowViewModel(AppSettings.Instance);
        }
    }
}
