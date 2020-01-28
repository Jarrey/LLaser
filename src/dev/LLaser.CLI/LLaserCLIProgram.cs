//*********************************************************
// LLaser.CLI project - LLaserCLIProgram.cs
// Created at 2013-7-26
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code.
//
//*********************************************************

namespace LLaser.CLI
{
    using CommandLine.Text;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommandLine;
    using System.Reflection;
    using System.IO;
    using LLaser.Common;

    class LLaserCLIProgram
    {
        static void Main(string[] args)
        {
            // initialize application settings
            AppSettings.InitializeSettings(AppSettings.Instance);

            // init parameters
            Initializer();

            try
            {
                var result = Parser.Default.ParseArguments<LLaserOptions>(args);
                bool hr = true;

                if (!result.Errors.Any())
                {
                    LLaserOptions option = result.Value;
                    switch (option.Function)
                    {
                        case Functions.BMP:
                            hr &= option.GenerateBitmaps();
                            PrintLine();
                            break;
                    }

                    if (option.About) ShowAbout();
                }

                if (!hr)
                {
                    Console.WriteLine();
                    Console.WriteLine(HelpText.AutoBuild(result));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void Initializer()
        {
            LLaserCLIParameter.Instance.WorkLocation = Environment.CurrentDirectory;
        }

        private static void ShowAbout()
        {
            Console.WriteLine();
        }

        private static void PrintLine()
        {
            Console.WriteLine(new string('=', 40));
        }
    }
}
