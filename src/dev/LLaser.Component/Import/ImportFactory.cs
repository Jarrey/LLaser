//********************************************************* 
// LLaser.Component project - ImportFactory.cs
// Created at 2013-5-19
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using LLaser.Common.Core;

namespace LLaser.Component.Import
{
    public class ImportFactory
    {
        public static IImport GetImport(string fileExtension)
        {
            switch (fileExtension.Trim().ToLower())
            {
                case ".cfg":
                case ".xml":
                    return new xmlImport();
                case ".bin":
                    return new binImport();
                case ".csv":
                    return new csvImport();
                case ".xlsx":
                    return new excelImport();
            }
            return null;
        }
    }
}