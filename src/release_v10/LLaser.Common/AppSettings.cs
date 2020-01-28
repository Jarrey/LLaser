//********************************************************* 
// LLaser.Common project - AppSettings.cs
// Created at 2013-5-20
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using LLaser.Common.Extension;
using System.Windows.Media;

namespace LLaser.Common
{
    public class AppSettings : IAppSettings
    {
        public static void InitializeSettings(IAppSettings settingInstance)
        {
            try
            {
                if (File.Exists(settingInstance.SettingFilePath))
                {
                    var xmlSetting = new XmlDocument();
                    xmlSetting.Load(settingInstance.SettingFilePath);

                    XmlNodeList xmlNodeList = xmlSetting.SelectNodes("/AppSettings/Setting");
                    if (xmlNodeList != null)
                    {
                        foreach (XmlElement element in xmlNodeList)
                        {
                            string keyName = element.GetAttribute("KeyName");
                            if (settingInstance.Settings.ContainsKey(keyName))
                            {
                                string typeName = element.GetAttribute("Type");
                                if (typeName == typeof(int).FullName)
                                    settingInstance.Settings[element.GetAttribute("KeyName")] =
                                        element.GetAttribute("Value").StringToInt();
                                else if (typeName == typeof(double).FullName)
                                    settingInstance.Settings[element.GetAttribute("KeyName")] =
                                        element.GetAttribute("Value").StringToDouble();
                                else if (typeName == typeof(float).FullName)
                                    settingInstance.Settings[element.GetAttribute("KeyName")] =
                                        element.GetAttribute("Value").StringToFloat();
                                else if (typeName == typeof(bool).FullName)
                                    settingInstance.Settings[element.GetAttribute("KeyName")] =
                                        element.GetAttribute("Value").StringToBoolean();
                                else if (typeName == typeof(string).FullName)
                                    settingInstance.Settings[element.GetAttribute("KeyName")] =
                                        element.GetAttribute("Value");
                                else if (typeName == typeof(Color).FullName)
                                    settingInstance.Settings[element.GetAttribute("KeyName")] =
                                        element.GetAttribute("Value").StringToColor();
                            }
                        }
                    }
                }
                SaveSettings(settingInstance);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SaveSettings(IAppSettings settingInstance)
        {
            var xmlSetting = new XmlDocument();
            XmlDocumentFragment xmlDocumentFragment = xmlSetting.CreateDocumentFragment();
            XmlElement rootElement = xmlSetting.CreateElement("AppSettings");
            xmlDocumentFragment.AppendChild(rootElement);

            foreach (KeyValuePair<string, object> setting in settingInstance.Settings)
            {
                rootElement.AppendChild(CreateSettingsElement(xmlSetting, setting.Key, setting.Value));
            }

            xmlSetting.AppendChild(rootElement);
            xmlSetting.Save(settingInstance.SettingFilePath);
        }

        private static XmlElement CreateSettingsElement(XmlDocument xmlDoc, string keyName, object value)
        {
            XmlElement settingElement = xmlDoc.CreateElement("Setting");
            settingElement.SetAttribute("KeyName", keyName);
            settingElement.SetAttribute("Value", value.ToString());
            settingElement.SetAttribute("Type", value.GetType().FullName);
            return settingElement;
        }

        #region For instance

        private static AppSettings _instance;

        #region For instance

        private AppSettings()
        {
            Settings = new ObservableDictionary<string, object>();

            // Create Settings for AppSettings
            Settings[GLOBAL_LANGUAGE] = "zh-cn";
            Settings[GLOBAL_SUPPORT_LANGUAGES] = "简体中文(Simplified Chinese):zh-cn|English:en-us";
            Settings[GLOBAL_SUPPORTMAIL] = "jar.bob@gmail.com";
            Settings[APP_TEXT_SIZE] = 12.0;
            Settings[APP_TEXT_COLOR] = Colors.Red;
            Settings[APP_TEXT_HIGHLIGHT_COLOR] = Colors.Yellow;
            Settings[APP_VALUE_TEXT_COLOR] = Colors.Yellow;
            Settings[APP_LINE_COLOR] = Colors.Red;
            Settings[APP_SIGNAL_LINE_COLOR] = Colors.Yellow;
            Settings[APP_SIGNAL_LINE_HIGHLIGHT_COLOR] = Colors.Red;
        }

        #region Setting fields

        public const string GLOBAL_LANGUAGE = "GLOBAL_LANGUAGE";
        public const string GLOBAL_SUPPORT_LANGUAGES = "GLOBAL_SUPPORT_LANGUAGES";
        public const string GLOBAL_SUPPORTMAIL = "GLOBAL_SUPPORTMAIL";

        public const string APP_TEXT_SIZE = "APP_TEXT_SIZE";
        public const string APP_TEXT_COLOR = "APP_TEXT_COLOR";
        public const string APP_TEXT_HIGHLIGHT_COLOR = "APP_TEXT_HIGHLIGHT_COLOR";
        public const string APP_VALUE_TEXT_COLOR = "APP_VALUE_TEXT_COLOR";
        public const string APP_LINE_COLOR = "APP_LINE_COLOR";
        public const string APP_SIGNAL_LINE_COLOR = "APP_SIGNAL_LINE_COLOR";
        public const string APP_SIGNAL_LINE_HIGHLIGHT_COLOR = "APP_SIGNAL_LINE_HIGHLIGHT_COLOR";

        #endregion

        public static AppSettings Instance
        {
            get { return _instance ?? (_instance = new AppSettings()); }
        }

        #region Indexer
        public object this[string keyName]
        {
            get { return Settings.ContainsKey(keyName) ? Settings[keyName] : null; }
            set { if (Settings.ContainsKey(keyName)) Settings[keyName] = value; }
        }
        #endregion

        public string SettingFilePath
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "llaser.setting"); }
        }

        public ObservableDictionary<string, object> Settings { get; private set; }

        #endregion

        #endregion
    }

    public interface IAppSettings
    {
        string SettingFilePath { get; }
        ObservableDictionary<string, object> Settings { get; }
    }
}