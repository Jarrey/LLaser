//********************************************************* 
// LLaser project - PowerTimingView.xaml.cs
// Created at 2013-5-7
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace LLaser.View
{
    using System;
    using System.ComponentModel;
    using System.Windows.Data;

    using LLaser.Component;

    /// <summary>
    ///     Interaction logic for PowerTimingView.xaml
    /// </summary>
    public partial class PowerTimingView : UserControl
    {
        public PowerTimingView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event hanlder to create one new row when user enter editing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgPowerTiming_OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            Signal.IngoreCommitEvent = true;
            (sender as DataGrid).CommitEdit(DataGridEditingUnit.Row, false);
            Signal.IngoreCommitEvent = false;
        }
    }
}