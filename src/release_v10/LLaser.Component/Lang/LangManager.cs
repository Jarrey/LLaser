//********************************************************* 
// LLaser project - LangManager.cs
// Created at 2013-5-14
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Markup;
using LLaser.Common;

namespace LLaser.Component.Lang
{
    public class LangManager
    {
        private static LangManager _instance;

        private readonly ObservableDictionary<string, string> _res;

        private LangManager(string lang = "zh-cn")
        {
            _res = new ObservableDictionary<string, string>();
            LoadLangs(lang);
        }

        public static LangManager Current
        {
            get
            {
                return _instance ??
                       (_instance = new LangManager((string)AppSettings.Instance[AppSettings.GLOBAL_LANGUAGE]));
            }
        }

        public string this[string key]
        {
            get { return _res.ContainsKey(key) ? _res[key] : string.Empty; }
        }

        private void LoadLangs(string lang)
        {
            _res.Clear();

            // generate strings for globalization
            using (var s = this.GetType().Assembly.GetManifestResourceStream("LLaser.Component.Lang.Language_" + lang + ".xaml"))
            {
                var rs = XamlReader.Load(s) as ResourceDictionary;

                if (rs == null) return;

                var langs = rs["Res_" + lang] as Collection<L>;
                if (langs != null)
                {
                    foreach (L l in langs)
                        _res.Add(l.Key, l.Value);
                }
            }
        }
    }
}