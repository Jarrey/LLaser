//********************************************************* 
// LLaser project - TFTTimingMenuItemModel.cs
// Created at 2013-4-25
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using LLaser.Component;
using LLaser.Component.Lang;
using LLaser.ViewModel;
using LLaser.WPF.Controls;

namespace LLaser.Model
{
    public class TFTTimingMenuItemModel : MainWindowMenuItemModel
    {
        public TFTTimingMenuItemModel()
        {
            #region initialize TFTTiming panel menus

            var opMenu = new MenuItemModel("MENU_OP", LangManager.Current["menuOP"]);
            Insert(Count > 0 ? 1 : 0, opMenu);
            
            var copyImgMenu = new MenuItemModel("MENU_COPYIMG", LangManager.Current["menuCopyImg"])
            {
                Icon = "pack://application:,,,/LLaser;component/Image/image_copy.png",
                Command = ImagePreview.CopyCommmand
            };
            this["MENU_OP"].Menus.Add(copyImgMenu);

            var fullScreenMenu = new MenuItemModel("MENU_FULLSREEN", LangManager.Current["menuFullScreen"],
                                                   LangManager.Current["menuFullScreenGesture"])
                {
                    Icon = "pack://application:,,,/LLaser;component/Image/fullscreen.png",
                    Command = ImagePreview.FullScreenCommand
                };
            this["MENU_OP"].Menus.Add(fullScreenMenu);

            this["MENU_FILE"].Menus.Insert(0, new MenuItemModel(SEPERATOR, SEPERATOR));

            var saveImgMenu = new MenuItemModel("MENU_SAVEIMG", LangManager.Current["menuSaveImg"],
                                                LangManager.Current["menuSaveImgGesture"])
                {
                    Icon = "pack://application:,,,/LLaser;component/Image/image_save.png",
                    Command = ImagePreview.SaveCommmand
                };
            this["MENU_FILE"].Menus.Insert(0, saveImgMenu);

            var buildCfgMenu = new MenuItemModel("MENU_BUILDCFG", LangManager.Current["menuBuildCfg"],
                                                LangManager.Current["menuBuildCfgGesture"])
            {
                Icon = "pack://application:,,,/LLaser;component/Image/build.png",
                IsEnabled = false
            };
            this["MENU_FILE"].Menus.Insert(0, buildCfgMenu);

            var saveAsCfgMenu = new MenuItemModel("MENU_SAVEASCFG", LangManager.Current["menuSaveAsCfg"],
                                                LangManager.Current["menuSaveAsCfgGesture"])
            {
                Icon = "pack://application:,,,/LLaser;component/Image/save_as.png",
                Command = MainWindowViewModel.SaveAsCommand
            };
            this["MENU_FILE"].Menus.Insert(0, saveAsCfgMenu);

            var saveCfgMenu = new MenuItemModel("MENU_SAVECFG", LangManager.Current["menuSaveCfg"],
                                                LangManager.Current["menuSaveCfgGesture"])
                {
                    Icon = "pack://application:,,,/LLaser;component/Image/save.png",
                    Command = MainWindowViewModel.SaveCommand
                };
            this["MENU_FILE"].Menus.Insert(0, saveCfgMenu);

            var openCfgMenu = new MenuItemModel("MENU_OPENCFG", LangManager.Current["menuOpenCfg"],
                                                LangManager.Current["menuOpenCfgGesture"])
                {
                    Icon = "pack://application:,,,/LLaser;component/Image/open_file.png",
                    Command = MainWindowViewModel.OpenCommand
                };
            this["MENU_FILE"].Menus.Insert(0, openCfgMenu);
            
            #endregion
        }
    }
}