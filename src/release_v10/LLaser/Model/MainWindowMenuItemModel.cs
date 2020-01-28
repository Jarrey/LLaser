//********************************************************* 
// LLaser project - MainWindowMenuItemModel.cs
// Created at 2013-5-14
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using LLaser.Common.Command;
using LLaser.Component.Lang;
using LLaser.View;

namespace LLaser.Model
{
    using LLaser.ViewModel;

    public class MainWindowMenuItemModel : ObservableCollection<MenuItemModel>
    {
        public static readonly string SEPERATOR = "SEPERATOR";

        public MainWindowMenuItemModel()
        {
            #region initialize main window menus

            var fileMenu = new MenuItemModel("MENU_FILE", LangManager.Current["menuFile"]);
            var toolsMenu = new MenuItemModel("MENU_TOOLS", LangManager.Current["menuTools"]);
            var helpMenu = new MenuItemModel("MENU_HELP", LangManager.Current["menuHelp"]);

            // help main menu items
            var helpMenuItem = new MenuItemModel("MENU_HELPITEM", LangManager.Current["menuHelpItem"], LangManager.Current["menuHelpItemGesture"])
            {
                Icon = "pack://application:,,,/LLaser;component/Image/help.png",
                Command = MainWindowViewModel.OpenHelpCommand
            };
            helpMenu.Menus.Add(helpMenuItem);

            var aboutMenu = new MenuItemModel("MENU_ABOUT", LangManager.Current["menuAbout"])
                {
                    Icon = "pack://application:,,,/LLaser;component/LLaser.ico",
                    Command = new RelayCommand(() => new About().ShowDialog())
                };
            helpMenu.Menus.Add(aboutMenu);

            Add(fileMenu);
            Add(toolsMenu);
            Add(helpMenu);

            // file main menu items
            var exitMenu = new MenuItemModel("MENU_EXIT", LangManager.Current["menuExit"], LangManager.Current["menuExitGesture"])
            {
                Icon = "pack://application:,,,/LLaser;component/Image/exit.png",
                Command = new RelayCommand(() => Application.Current.Dispatcher.InvokeShutdown())
            };
            this["MENU_FILE"].Menus.Add(exitMenu);

            // tool main menu items
            var perferenceMenu = new MenuItemModel("MENU_PERFERENCE", LangManager.Current["menuPerference"])
            {
                Icon = "pack://application:,,,/LLaser;component/Image/perference.png",
                Command = MainWindowViewModel.OpenSettingCommand
            };
            this["MENU_TOOLS"].Menus.Add(perferenceMenu);

            #endregion
        }

        public MenuItemModel this[string key]
        {
            get
            {
                return this.FirstOrDefault(m => m.Key == key);
            }
        }
    }
}