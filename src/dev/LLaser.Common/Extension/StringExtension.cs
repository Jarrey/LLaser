//********************************************************* 
// LLaser.Common project - StringExtension.cs
// Created at 2013-5-20
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
    using System.Windows.Media;

namespace LLaser.Common.Extension
{

    public static class StringExtension
    {
        public static byte StringToByte(this String value)
        {
            byte result = 0;
            byte.TryParse(value, out result);
            return result;
        }

        public static int StringToInt(this String value)
        {
            int result = 0;
            int.TryParse(value, out result);
            return result;
        }

        public static uint StringToUInt(this String value)
        {
            uint result = 0;
            uint.TryParse(value, out result);
            return result;
        }

        public static long StringToLong(this String value)
        {
            long result = 0L;
            long.TryParse(value, out result);
            return result;
        }

        public static ulong StringToULong(this String value)
        {
            ulong result = 0L;
            ulong.TryParse(value, out result);
            return result;
        }

        public static double StringToDouble(this String value)
        {
            double result = 0.0;
            double.TryParse(value, out result);
            return result;
        }

        public static float StringToFloat(this String value)
        {
            float result = 0.0f;
            float.TryParse(value, out result);
            return result;
        }

        public static bool StringToBoolean(this string value)
        {
            return (value.ToLower().Trim() == "true" || value.Trim() == "1") ? true : false;
        }

        public static Color StringToColor(this string value)
        {
            int color = Convert.ToInt32(value.Substring(1), 16);
            return Color.FromArgb((byte)((color >> 24) & 0xff), (byte)((color >> 16) & 0xff), (byte)((color >> 8) & 0xff), (byte)(color & 0xff));
        }

        public static Point StringToPoint(this string value)
        {
            string[] point = value.Split(new[] { ',', ' ' });
            return point.Length == 2 ? new Point(StringToDouble(point[0]), StringToDouble(point[1])) : default(Point);
        }

        public static string[] StringToArray(this string value, char split = ' ')
        {
            string[] result;
            result = split == ' ' ? value.Split(new[] { ',', ' ', '|' }) : value.Split(split);
            if (result != null && result.Length != 0)
                return result;
            else
                return null;
        }

        public static DateTime StringToDateTime(this string value, string format = "yyyyMMdd")
        {
            var result = new DateTime(1970, 1, 1, 0, 0, 0);
            string[] dateTimeFormats;
            if (format == "yyyyMMdd")
                dateTimeFormats = new[] { format };
            else
                dateTimeFormats = new[]
                    {
                        format,
                        "yyyyMMdd",
                        "yyyy-MM-dd",
                        "yyyy/MM/dd",
                        "yyyyMMdd HH:mm:ss.fff",
                        "yyyy-MM-dd HH:mm:ss.fff",
                        "yyyy/MM/dd HH:mm:ss.fff"
                    };
            DateTime.TryParseExact(value, dateTimeFormats, new CultureInfo("en-US"), DateTimeStyles.None, out result);
            return result;
        }

        public static Dictionary<string, string> StringToDictionary(this string value)
        {
            var returnvalue = new Dictionary<string, string>();
            string[] parameters = value.Split('|');
            foreach (string p in parameters)
            {
                string[] values = p.Split(':');
                if (values.Length > 1)
                {
                    returnvalue[values[0]] = values[1];
                }
            }
            return returnvalue;
        }
    }
}
