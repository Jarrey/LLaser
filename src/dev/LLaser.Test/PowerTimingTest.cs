//*********************************************************
// LLaser.Test project - PowerTimingTest.cs
// Created at 2013-5-11
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LLaser.Component;

namespace LLaser.Test
{
    [TestClass]
    public class PowerTimingTest
    {
        private PowerTiming _powerTiming = new PowerTiming();

        /// <summary>
        ///     Test PowerTiming construcutor
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            try
            {
                var pt = new PowerTiming();
                Assert.IsNotNull(pt);
                Assert.AreEqual(0, pt.OnSignals.Count);
                PrintAllSignals();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Test Add signal method
        /// </summary>
        [TestMethod]
        public void TestAddSignal()
        {
            _powerTiming.CleanAll(PowerStatus.On);
            _powerTiming.Add(new Signal(0, ElectricalLevels.High), PowerStatus.On);
            Assert.AreEqual(1, _powerTiming.OnSignals.Count);
            PrintAllSignals();

            _powerTiming.CleanAll(PowerStatus.On);
            _powerTiming.Add(new Signal(10, ElectricalLevels.High), PowerStatus.On);
            Assert.AreEqual(1, _powerTiming.OnSignals.Count);
            PrintAllSignals();

            _powerTiming.CleanAll(PowerStatus.On);
            _powerTiming.Add(new Signal(5, ElectricalLevels.High), PowerStatus.On);
            Assert.AreEqual(1, _powerTiming.OnSignals.Count);
            _powerTiming.Add(new Signal(10, ElectricalLevels.High), PowerStatus.On);
            Assert.AreEqual(2, _powerTiming.OnSignals.Count);
            _powerTiming.Add(new Signal(2, ElectricalLevels.Low), PowerStatus.On);
            Assert.AreEqual(3, _powerTiming.OnSignals.Count);
            _powerTiming.Add(new Signal(6, ElectricalLevels.High), PowerStatus.On);
            Assert.AreEqual(4, _powerTiming.OnSignals.Count);
            _powerTiming.Add(new Signal(23, ElectricalLevels.Low), PowerStatus.On);
            Assert.AreEqual(5, _powerTiming.OnSignals.Count);
            _powerTiming.Add(45, ElectricalLevels.Low, PowerStatus.On);
            Assert.AreEqual(6, _powerTiming.OnSignals.Count);
            _powerTiming.Add(1, ElectricalLevels.Low, PowerStatus.On);
            Assert.AreEqual(7, _powerTiming.OnSignals.Count);
            _powerTiming.Add(0, ElectricalLevels.Low, PowerStatus.On);
            Assert.AreEqual(8, _powerTiming.OnSignals.Count);
            PrintAllSignals();

            #region test add method performence
            _powerTiming.CleanAll(PowerStatus.On);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (uint i = 1; i <= 10000; i++)
            {
                _powerTiming.Add(i, i % 2 == 0 ? ElectricalLevels.Low : ElectricalLevels.High, PowerStatus.On);
                Assert.AreEqual((int)i, _powerTiming.OnSignals.Count);
            }
            sw.Stop();
            Console.WriteLine(@"{0} ms", sw.ElapsedMilliseconds);
            PrintAllSignals();

            _powerTiming.CleanAll(PowerStatus.On);
            sw.Restart();
            for (uint i = 10000; i >= 1; i--)
            {
                _powerTiming.Add(i, i % 2 == 0 ? ElectricalLevels.Low : ElectricalLevels.High, PowerStatus.On);
                Assert.AreEqual((int)(10001 - i), _powerTiming.OnSignals.Count);
            }
            sw.Stop();
            Console.WriteLine(@"{0} ms", sw.ElapsedMilliseconds);
            PrintAllSignals();
            #endregion

            #region test dirty data
            _powerTiming.CleanAll(PowerStatus.On);
            try
            {
                _powerTiming.Add(new Signal(0, ElectricalLevels.Low), PowerStatus.On);
                _powerTiming.Add(new Signal(0, ElectricalLevels.High), PowerStatus.On);
            }
            catch (SignalExistingException ex)
            {
                Assert.AreEqual(0U, ex.Time);
                _powerTiming.CleanAll(PowerStatus.On);
            }

            try
            {
                _powerTiming.Add(new Signal(10, ElectricalLevels.Low), PowerStatus.On);
                _powerTiming.Add(new Signal(10, ElectricalLevels.High), PowerStatus.On);
            }
            catch (SignalExistingException ex)
            {
                Assert.AreEqual(10U, ex.Time);
                _powerTiming.CleanAll(PowerStatus.On);
            }
            #endregion
        }

        /// <summary>
        /// Test Remove signal method
        /// </summary>
        [TestMethod]
        public void TestRemoveSignal()
        {
            _powerTiming.CleanAll(PowerStatus.On);
            _powerTiming.Add(new Signal(5, ElectricalLevels.High), PowerStatus.On);
            Assert.AreEqual(1, _powerTiming.OnSignals.Count);
            _powerTiming.Add(new Signal(10, ElectricalLevels.High), PowerStatus.On);
            Assert.AreEqual(2, _powerTiming.OnSignals.Count);
            _powerTiming.Add(new Signal(2, ElectricalLevels.Low), PowerStatus.On);
            Assert.AreEqual(3, _powerTiming.OnSignals.Count);
            _powerTiming.Add(new Signal(6, ElectricalLevels.High), PowerStatus.On);
            Assert.AreEqual(4, _powerTiming.OnSignals.Count);
            _powerTiming.Add(new Signal(23, ElectricalLevels.Low), PowerStatus.On);
            Assert.AreEqual(5, _powerTiming.OnSignals.Count);
            _powerTiming.Add(45, ElectricalLevels.Low, PowerStatus.On);
            Assert.AreEqual(6, _powerTiming.OnSignals.Count);
            _powerTiming.Add(1, ElectricalLevels.Low, PowerStatus.On);
            Assert.AreEqual(7, _powerTiming.OnSignals.Count);
            _powerTiming.Add(0, ElectricalLevels.Low, PowerStatus.On);
            Assert.AreEqual(8, _powerTiming.OnSignals.Count);
            PrintAllSignals();

            _powerTiming.Remove(5, ElectricalLevels.High, PowerStatus.On);
            Assert.AreEqual(7, _powerTiming.OnSignals.Count);
            _powerTiming.Remove(new Signal(23, ElectricalLevels.Low), PowerStatus.On);
            Assert.AreEqual(6, _powerTiming.OnSignals.Count);
            _powerTiming.Remove(65, ElectricalLevels.High, PowerStatus.On);
            Assert.AreEqual(6, _powerTiming.OnSignals.Count);
            _powerTiming.Remove(0, ElectricalLevels.High, PowerStatus.On);
            Assert.AreEqual(6, _powerTiming.OnSignals.Count);
            PrintAllSignals();

            _powerTiming.ForceRemove(0, PowerStatus.On);
            Assert.AreEqual(5, _powerTiming.OnSignals.Count);
            _powerTiming.ForceRemove(5, PowerStatus.On);
            Assert.AreEqual(5, _powerTiming.OnSignals.Count);
            PrintAllSignals();
        }

        [TestMethod]
        public void TestCleanMethod()
        {
            _powerTiming.CleanAll(PowerStatus.On);
            Assert.AreEqual(0, _powerTiming.OnSignals.Count);
            PrintAllSignals();
        }

        [TestMethod]
        public void TestExistingMethod()
        {
            _powerTiming.CleanAll(PowerStatus.On);
            Assert.IsFalse(_powerTiming.Existing(0, PowerStatus.On));
            Assert.IsFalse(_powerTiming.Existing(10, PowerStatus.On));

            _powerTiming.Add(new Signal(5, ElectricalLevels.High), PowerStatus.On);
            _powerTiming.Add(new Signal(10, ElectricalLevels.High), PowerStatus.On);
            Assert.IsTrue(_powerTiming.Existing(10, PowerStatus.On));
            Assert.IsTrue(_powerTiming.Existing(5, PowerStatus.On));
            Assert.IsFalse(_powerTiming.Existing(0, PowerStatus.On));
            Assert.IsFalse(_powerTiming.Existing(15, PowerStatus.On));
            Assert.IsTrue(_powerTiming.Existing(new Signal(5, ElectricalLevels.Low), PowerStatus.On));
            Assert.IsFalse(_powerTiming.Existing(6, PowerStatus.On));
        }

        private void PrintAllSignals()
        {
            Enumerable.All(_powerTiming.OnSignals, p =>
                {
                    Console.WriteLine(@"time: {0}  level: {1}", p.Time, p.ElectricalLevel);
                    return true;
                });
            Console.WriteLine(@"-----------------------------------");
        }
    }
}
