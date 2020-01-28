//********************************************************* 
// LLaser project - BmpMakerMenuItemModel.cs
// Created at 2013-5-20
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
using LLaser.Model;

namespace LLaser.Model
{
    using LLaser.Component.Lang;
    using LLaser.ViewModel;
    using LLaser.WPF.Controls;

    public class BmpMakerMenuItemModel : MainWindowMenuItemModel
    {
        public BmpMakerMenuItemModel()
        {

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

            #region File Menu

            this["MENU_FILE"].Menus.Insert(0, new MenuItemModel(SEPERATOR, SEPERATOR));

            var saveImgMenu = new MenuItemModel("MENU_SAVEIMG", LangManager.Current["menuSaveImg"],
                                                LangManager.Current["menuSaveImgGesture"])
            {
                Icon = "pack://application:,,,/LLaser;component/Image/image_save.png",
                Command = ImagePreview.SaveCommmand
            };
            this["MENU_FILE"].Menus.Insert(0, saveImgMenu);

            var saveCfgMenu = new MenuItemModel("MENU_SAVECFG", LangManager.Current["menuSaveCfg"],
                                                LangManager.Current["menuSaveCfgGesture"])
            {
                Icon = "pack://application:,,,/LLaser;component/Image/save.png",
                Command = MainWindowViewModel.SaveBMPMakerCommand
            };
            this["MENU_FILE"].Menus.Insert(0, saveCfgMenu);

            var openCfgMenu = new MenuItemModel("MENU_OPENCFG", LangManager.Current["menuOpenCfg"],
                                                LangManager.Current["menuOpenCfgGesture"])
            {
                Icon = "pack://application:,,,/LLaser;component/Image/open_file.png",
                Command = MainWindowViewModel.OpenBMPMakerCommand
            };
            this["MENU_FILE"].Menus.Insert(0, openCfgMenu);

            #endregion
        }
    }
}
