//********************************************************* 
// LLaser project - CompomentModel.cs
// Created at 2013-4-25
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using LLaser.Common;
using LLaser.Component;

namespace LLaser.Model
{
    public class CompomentModel : ModelBase
    {
        #region constructor

        public CompomentModel()
        {
            Menu = new MainWindowMenuItemModel();
        }

        #endregion

        #region fields

        private FunctionTypes _type;
        private MainWindowMenuItemModel _menu;
        private TabItemModel _tabItem;
        private ToolbarItemModel _toolbar;

        #endregion

        #region view compoments

        public FunctionTypes Type
        {
            get { return _type; }
            set
            {
                SetProperty(ref _type, value);
            }
        }

        public TabItemModel TabItem
        {
            get { return _tabItem; }
            set
            {
                SetProperty(ref _tabItem, value);
            }
        }

        public MainWindowMenuItemModel Menu
        {
            get { return _menu; }
            set
            {
                SetProperty(ref _menu, value);
            }
        }


        public ToolbarItemModel Toolbar
        {
            get { return _toolbar; }
            set
            {
                SetProperty(ref _toolbar, value);
            }
        }

        #endregion

    }
}