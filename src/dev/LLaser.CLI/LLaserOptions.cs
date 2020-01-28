//*********************************************************
// LLaser.CLI project - LLaserOptions.cs
// Created at 2013-7-26
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code.
//
//*********************************************************

namespace LLaser.CLI
{
    using CommandLine;
    using CommandLine.Text;
    using LLaser.Common;
    using LLaser.Common.Core;
    using LLaser.Component;
    using LLaser.Component.Import;
    using LLaser.Component.Lang;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;

    class LLaserOptions
    {
        #region Options

        [Option('i', "input", DefaultValue = "config.xml", HelpText = @"Input config file path.  Example: -i ""D:\BmpConfig.xml""")]
        public string InputConfig { get; set; }

        [Option('o', "out", DefaultValue = "bimap.bmp", HelpText = @"Output file path.  Example: -o ""D:\ColorBar.bmp""")]
        public string OutputBitmap { get; set; }

        [Option('f', "function", DefaultValue = Functions.BMP, HelpText = @"Function type.  Example: -f bmp  # to use BMPMaker to generate the LCD testing BMP file")]
        public Functions Function { get; set; }

        [Option('a', "about", HelpText = @"Show infomation about this application.")]
        public bool About { get; set; }

        #endregion

        #region Methods

        public bool GenerateBitmaps()
        {
            // check input file
            string inputFile = string.Empty;
            if (!Directory.Exists(Path.GetDirectoryName(InputConfig)))
            {
                inputFile = Path.Combine(LLaserCLIParameter.Instance.WorkLocation, InputConfig);
            }
            else
            {
                inputFile = InputConfig;
            }

            if (!File.Exists(inputFile))
            {
                Console.WriteLine(LangManager.Current["lblNotFindConfigFile"], inputFile);
                return false;
            }

            // checek output file
            string outputFile = string.Empty;
            if (!Directory.Exists(Path.GetDirectoryName(OutputBitmap)))
            {
                outputFile = Path.Combine(LLaserCLIParameter.Instance.WorkLocation, OutputBitmap);
            }
            else
            {
                outputFile = OutputBitmap;
            }
            outputFile = GetUniqueFileName(outputFile);


            // load config file
            BmpMaker confgModel = null;
            try
            {
                IImport importer = ImportFactory.GetImport(Path.GetExtension(inputFile));
                using (Stream stream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                {
                    if (importer != null && importer.Import<BmpMaker>(stream))
                    {
                        if (importer.Target != null && importer.Target is BmpMaker)
                        {
                            confgModel = importer.Target as BmpMaker;
                            confgModel.UpdateBitmap();
                        }
                    }
                    else
                    {
                        Console.WriteLine(LangManager.Current["lblImportWrongFileFormatError"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            Console.WriteLine(LangManager.Current["lblOpenBmpConfigSuccessfully"], inputFile);

            // ouput the bitmap file
            try
            {
                using (Stream s = new FileStream(outputFile, FileMode.Create, FileAccess.ReadWrite))
                {

                    // choose encoder for data
                    BitmapEncoder encoder = new BmpBitmapEncoder();
                    // push the rendered bitmap to it
                    encoder.Frames.Add(BitmapFrame.Create(confgModel.Bitmap));
                    // save the data to the stream
                    encoder.Save(s);
                }
                Console.WriteLine(LangManager.Current["lblSaveBmpFileSuccessfully"], outputFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        private string GetUniqueFileName(string filePath)
        {
            int i = 0;
            string fileName = string.Empty;
            while (File.Exists(fileName = Path.Combine(Path.GetDirectoryName(filePath),
                                          Path.GetFileNameWithoutExtension(filePath) +
                                          (i == 0 ? string.Empty : "(" + i.ToString() + ")") +
                                          Path.GetExtension(filePath))))
            {
                i++;
            }
            return fileName;
        }

        #endregion
    }
}
