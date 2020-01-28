//********************************************************* 
// LLaser.Common project - FileSizeExtension.cs
// Created at 2013-5-15
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;

namespace LLaser.Common.Core
{
    public static class BinarySizeExtension
    {
        public static string ToBinarySize(this ulong size, int d = 2)
        {
            return ConvertToBinarySize(size, d);
        }

        public static string ToBinarySize(this long size, int d = 2)
        {
            return ConvertToBinarySize((ulong)size, d);
        }

        public static string ToBinarySize(this int size, int d = 2)
        {
            return ConvertToBinarySize((ulong)size, d);
        }

        public static string ToBinarySize(this uint size, int d = 2)
        {
            return ConvertToBinarySize(size, d);
        }

        private static string ConvertToBinarySize(ulong size, int d)
        {
            if (size <= 0) return "0";
            var units = new string[5] { "B", "KB", "MB", "GB", "TB" };
            var digitGroups = (ulong)(Math.Log10(size) / Math.Log10(1024));
            return (size / Math.Pow(1024, digitGroups)).ToString("N" + d) + " " + units[digitGroups];
        }
    }
}
