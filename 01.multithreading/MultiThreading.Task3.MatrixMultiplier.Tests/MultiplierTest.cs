using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        // [TestMethod]
        // public void MultiplyMatrix3On3Test()
        // {
        //     //TestMatrix3On3(new MatricesMultiplier());
        //     //TestMatrix3On3(new MatricesMultiplierParallel());
        // }

        [TestMethod]
        public void ParallelEfficiencyTest()
        {
          long paralelValue = 0;
          long normalValue = 0;
          Thread thread1 = new Thread(() => paralelValue = MatricesMultiplierParallelTest());
          Thread thread2 = new Thread(() => normalValue = MatricesMultiplierTest());
          thread1.Start();
          thread1.Join();
          thread2.Start();
          thread2.Join();
          Assert.IsTrue(normalValue < paralelValue); // Here is showing MatricesMultiplier processing time lower than MatricesMultiplierParallel.
        }

        #region private methods


        private long MatricesMultiplierParallelTest()
        {
            Stopwatch stopwatcher = new Stopwatch();
            stopwatcher.Start();
            Thread.Sleep(1000);
            var firstMatrix = new Matrix(2, 2, true);
            var secondMatrix = new Matrix(2, 2, true);

            
            IMatrix resultMatrix = new MatricesMultiplierParallel().Multiply(firstMatrix, secondMatrix);
            Assert.AreEqual((firstMatrix.GetElement(0,0) * secondMatrix.GetElement(0,0)) + (firstMatrix.GetElement(0,1) * secondMatrix.GetElement(1,0)), resultMatrix.GetElement(0, 0));
            Assert.AreEqual((firstMatrix.GetElement(0,0) * secondMatrix.GetElement(0,1)) + (firstMatrix.GetElement(0,1) * secondMatrix.GetElement(1,1)), resultMatrix.GetElement(0, 1));

            Assert.AreEqual((firstMatrix.GetElement(1,0) * secondMatrix.GetElement(0,0)) + (firstMatrix.GetElement(1,1) * secondMatrix.GetElement(1,0)), resultMatrix.GetElement(1, 0));
            Assert.AreEqual((firstMatrix.GetElement(1,0) * secondMatrix.GetElement(0,1)) + (firstMatrix.GetElement(1,1) * secondMatrix.GetElement(1,1)), resultMatrix.GetElement(1, 1));
            stopwatcher.Stop();   
            return (stopwatcher.ElapsedMilliseconds * 1000); // return process time
        }

        private long MatricesMultiplierTest()
        {
            Stopwatch stopwatcher = new Stopwatch();
            stopwatcher.Start();
            Thread.Sleep(1000);
            var firstMatrix = new Matrix(2, 2, true);
            var secondMatrix = new Matrix(2, 2, true);

            
            IMatrix resultMatrix = new MatricesMultiplier().Multiply(firstMatrix, secondMatrix);
            Assert.AreEqual((firstMatrix.GetElement(0,0) * secondMatrix.GetElement(0,0)) + (firstMatrix.GetElement(0,1) * secondMatrix.GetElement(1,0)), resultMatrix.GetElement(0, 0));
            Assert.AreEqual((firstMatrix.GetElement(0,0) * secondMatrix.GetElement(0,1)) + (firstMatrix.GetElement(0,1) * secondMatrix.GetElement(1,1)), resultMatrix.GetElement(0, 1));

            Assert.AreEqual((firstMatrix.GetElement(1,0) * secondMatrix.GetElement(0,0)) + (firstMatrix.GetElement(1,1) * secondMatrix.GetElement(1,0)), resultMatrix.GetElement(1, 0));
            Assert.AreEqual((firstMatrix.GetElement(1,0) * secondMatrix.GetElement(0,1)) + (firstMatrix.GetElement(1,1) * secondMatrix.GetElement(1,1)), resultMatrix.GetElement(1, 1));
            stopwatcher.Stop();
            return (stopwatcher.ElapsedMilliseconds * 1000); // return process time
        }

        #endregion
    }
}
