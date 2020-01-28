//********************************************************* 
// LLaser.Component project - ExportFactory.cs
// Created at 2013-5-19
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using LLaser.Common.Core;

namespace LLaser.Component.Export
{
    public class ExportFactory
    {
        public static IExport GetExport(string fileExtension)
        {
            switch (fileExtension.Trim().ToLower())
            {
                case ".cfg":
                case ".xml":
                    return new xmlExport();
                case ".bin":
                    return new binExport();
                case ".csv":
                    return new csvExport();
                case ".xlsx":
                    return new excelExport();
            }
            return null;
        }
    }
}