//********************************************************* 
// LLaser project - MenuItemModel.cs
// Created at 2013-4-25
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.Collections.ObjectModel;
using System.Windows.Input;
using LLaser.Common;

namespace LLaser.Model
{
    public class MenuItemModel : ModelBase
    {
        #region Constructors

        public MenuItemModel()
        {
            Menus = new ObservableCollection<MenuItemModel>();
        }

        public MenuItemModel(string key, string header, string gesture = "")
            : this()
        {
            Key = key;
            Header = header;
            Gesture = gesture;
        }

        #endregion

        #region Properties and Fields

        private string _header;
        private string _key;
        private string _icon = null;
        private ICommand _command;
        private string _gesture;
        private bool _isEnabled = true;

        private ObservableCollection<MenuItemModel> _menus;

        public string Header
        {
            get { return _header; }
            set { SetProperty(ref _header, value); }
        }

        public ObservableCollection<MenuItemModel> Menus
        {
            get { return _menus; }
            set { SetProperty(ref _menus, value); }
        }

        public string Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        public string Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        public ICommand Command
        {
            get { return _command; }
            set { SetProperty(ref _command, value); }
        }

        public string Gesture
        {
            get { return _gesture; }
            set { SetProperty(ref _gesture, value); }
        }

        #endregion
    }
}