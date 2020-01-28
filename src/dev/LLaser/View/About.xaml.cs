//********************************************************* 
// LLaser project - About.xaml.cs
// Created at 2013-5-16
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using LLaser.Properties;

namespace LLaser.View
{
    /// <summary>
    ///     Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            DataContext = new AppInfo();
        }

        #region Event handlers

        private void About_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        #endregion

        /// <summary>
        ///     AppInfo internal class to provide version information
        /// </summary>
        public class AppInfo
        {
            public string Version
            {
                get
                {
                    return string.Format("{0}.{1}", Assembly.GetEntryAssembly().GetName().Version.Major,
                                         Assembly.GetEntryAssembly().GetName().Version.Minor);
                }
            }

            public string BuildVersion
            {
                get { return Assembly.GetEntryAssembly().GetName().Version.ToString(); }
            }

            public string Company
            {
                get
                {
                    var assemblyCompanyAttribute =
                        Assembly.GetEntryAssembly().GetCustomAttribute(typeof(AssemblyCompanyAttribute)) as AssemblyCompanyAttribute;
                    if (assemblyCompanyAttribute != null)
                        return assemblyCompanyAttribute.Company;
                    return string.Empty;
                }
            }

            public string Author { get { return "Speedpro"; } }

            public string License
            {
                get { return Encoding.Unicode.GetString(LLaser.Properties.Resources.License); }
            }

            public string VersionInfo
            {
                get { return LLaser.Properties.Resources.VersionInfo; }
            }

            public FlowDocument ThirdPartyLicenses
            {
                get
                {
                    return Application.LoadComponent(new Uri("/LLaser;component/Docs/ThirdPartyLicenses.xaml", UriKind.Relative)) as FlowDocument;
                }
            }
        }
    }
}