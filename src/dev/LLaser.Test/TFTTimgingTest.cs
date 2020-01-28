//*********************************************************
// LLaser.Test project - TFTTimgingTest.cs
// Created at 2013-5-13
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code.
//
//*********************************************************

using System;
using LLaser.Component;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LLaser.Test
{
    [TestClass]
    public class TFTTimgingTest
    {

        [TestMethod]
        public void TestConstructor()
        {
            var tft = new TFTTiming();
            Assert.IsNotNull(tft);
            Console.WriteLine(tft);
        }

        [TestMethod]
        public void TestTFTTimingCalculation()
        {
            var tft = new TFTTiming();

            #region test period
            tft.DCK = 33.3;
            /* 1 / DCK = 1000.0 / 33.3 = 30.03003003003 */
            Assert.AreEqual(1000.0 / 33.3, tft.Period);
            Console.WriteLine(tft);
            #endregion

            #region test ver_time
            tft.HSyncWidth = 2;
            tft.HBackPorch = 4;
            tft.HValid = 480;
            tft.HFrontPorch = 2;
            /* 1000.0 * 33.3 / (2 + 4 + 480 + 2) = 68.2377049180328 */
            Assert.AreEqual(1000.0 * 33.3 / (2 + 4 + 480 + 2), tft.VerTime);
            Console.WriteLine(tft);
            #endregion

            #region test ver_time
            tft.VSyncWidth = 22;
            tft.VBackPorch = 2;
            tft.VValid = 240;
            tft.VFrontPorch = 2;
            /* 1E+6 * (33.3 / (2 + 4 + 480 + 2)) / (22 + 2 + 240 + 2) = 256.532725255762 */
            Assert.AreEqual(1E+6 * (33.3 / (2 + 4 + 480 + 2)) / (22 + 2 + 240 + 2), tft.FraTime);
            Console.WriteLine(tft);
            #endregion
        }
    }
}
