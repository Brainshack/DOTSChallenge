using GameOfLife.Components;
using GameOfLife.Jobs;
using NUnit.Framework;
using Unity.Collections;
using Unity.Mathematics;

namespace Tests
{
    [TestFixture]
    public class SimulateCellJobTest
    {
        [Test]
        public void TestCountActiveNeighbors()
        {
            var cellsArrayNoNeighbor = new int[]
            {
                0,0,0,0,0,
                0,0,0,0,0,
                0,0,1,0,0,
                0,0,0,0,0,
                0,0,0,0,0
            };
            
            var cellsArrayOneNeighbor = new int[]
            {
                0,0,0,0,0,
                0,0,0,0,0,
                0,0,1,1,0,
                0,0,0,0,0,
                0,0,0,0,0
            };
            
            var cellsArrayThreeNeighbor = new int[]
            {
                0,0,0,0,0,
                0,1,0,0,0,
                0,1,1,0,0,
                0,0,0,1,0,
                0,0,0,0,0
            };


            var cell = new Cell
            {
                CellIndex = 4,
            };
            
            var cellsNative = new NativeArray<int>(cellsArrayNoNeighbor, Allocator.Temp);
            var job = new SimulateCellJob
            {
                currentState = cellsNative,
                GridDimensions = new int2(3,3)
            };

            var count = job.CountAliveNeighbors(cell);
            
            Assert.AreEqual(0, count);

            cellsNative.Dispose();
            
            cellsNative = new NativeArray<int>(cellsArrayOneNeighbor, Allocator.Temp);
            job = new SimulateCellJob
            {
                currentState = cellsNative,
                GridDimensions = new int2(3,3)
            };

            count = job.CountAliveNeighbors(cell);
            
            Assert.AreEqual(1, count);
            
            cellsNative.Dispose();
            
            cellsNative = new NativeArray<int>(cellsArrayThreeNeighbor, Allocator.Temp);
            job = new SimulateCellJob
            {
                currentState = cellsNative,
                GridDimensions = new int2(3,3)
            };

            count = job.CountAliveNeighbors(cell);
            
            Assert.AreEqual(3, count);
            
            cellsNative.Dispose();
        }
        
        [Test]
        public void TestCellDiesWithZeroNeighbors()
        {
            var cellsArray = new int[]
            {
                0,0,0,0,0,
                0,0,0,0,0,
                0,0,1,0,0,
                0,0,0,0,0,
                0,0,0,0,0
            };

            var cellsNative = new NativeArray<int>(cellsArray, Allocator.Temp);

            var cell = new Cell
            {
                CellIndex = 4,
            };

            var isAlive = new IsAlive
            {
                Value = true
            };
            var job = new SimulateCellJob
            {
                currentState = cellsNative,
                GridDimensions = new int2(3,3)
            };

            job.Execute(ref cell, ref isAlive);
            
            Assert.False(isAlive.Value);
            
            cellsNative.Dispose();
        }
        
        [Test]
        public void TestCellDiesWithOneNeighbor()
        {
            var cellsArray = new int[]
            {
                0,0,0,0,0,
                0,0,0,0,0,
                0,0,1,1,0,
                0,0,0,0,0,
                0,0,0,0,0
            };

            var cellsNative = new NativeArray<int>(cellsArray, Allocator.Temp);

            var cell = new Cell
            {
                CellIndex = 4,
            };

            var isAlive = new IsAlive
            {
                Value = true
            };
            var job = new SimulateCellJob
            {
                currentState = cellsNative,
                GridDimensions = new int2(3,3)
            };

            job.Execute(ref cell, ref isAlive);
            
            Assert.False(isAlive.Value);
            
            cellsNative.Dispose();
        }
        
        [Test]
        public void TestCellStaysAliveWithTwoNeighbor()
        {
            var cellsArray = new int[]
            {
                0,0,0,0,0,
                0,0,0,0,0,
                0,0,1,1,0,
                0,0,0,1,0,
                0,0,0,0,0
            };

            var cellsNative = new NativeArray<int>(cellsArray, Allocator.Temp);

            var cell = new Cell
            {
                CellIndex = 4,
            };

            var isAlive = new IsAlive
            {
                Value = true
            };
            var job = new SimulateCellJob
            {
                currentState = cellsNative,
                GridDimensions = new int2(3,3)
            };

            job.Execute(ref cell, ref isAlive);
            
            Assert.True(isAlive.Value);
            
            cellsNative.Dispose();
        }
        
        [Test]
        public void TestCellStaysAliveThreeTwoNeighbor()
        {
            var cellsArray = new int[]
            {
                0,0,0,0,0,
                0,1,0,0,0,
                0,0,1,1,0,
                0,0,0,1,0,
                0,0,0,0,0
            };

            var cellsNative = new NativeArray<int>(cellsArray, Allocator.Temp);

            var cell = new Cell
            {
                CellIndex = 4,
            };

            var isAlive = new IsAlive
            {
                Value = true
            };
            var job = new SimulateCellJob
            {
                currentState = cellsNative,
                GridDimensions = new int2(3,3)
            };

            job.Execute(ref cell, ref isAlive);
            
            Assert.True(isAlive.Value);
            
            cellsNative.Dispose();
        }
        
        [Test]
        public void TestCellDiesWithFourTwoNeighbor()
        {
            var cellsArray = new int[]
            {
                0,0,0,0,0,
                0,1,1,0,0,
                0,0,1,1,0,
                0,0,0,1,0,
                0,0,0,0,0
            };

            var cellsNative = new NativeArray<int>(cellsArray, Allocator.Temp);

            var cell = new Cell
            {
                CellIndex = 4,
            };

            var isAlive = new IsAlive
            {
                Value = true
            };
            var job = new SimulateCellJob
            {
                currentState = cellsNative,
                GridDimensions = new int2(3,3)
            };

            job.Execute(ref cell, ref isAlive);
            
            Assert.False(isAlive.Value);
            
            cellsNative.Dispose();
        }
        
        [Test]
        public void TestDeadCellBecomesAliveWithThreeNeighbor()
        {
            var cellsArray = new int[]
            {
                0,0,0,0,0,
                0,1,0,0,0,
                0,0,0,1,0,
                0,0,0,1,0,
                0,0,0,0,0
            };

            var cellsNative = new NativeArray<int>(cellsArray, Allocator.Temp);

            var cell = new Cell
            {
                CellIndex = 4,
            };

            var isAlive = new IsAlive
            {
                Value = false
            };
            var job = new SimulateCellJob
            {
                currentState = cellsNative,
                GridDimensions = new int2(3,3)
            };

            job.Execute(ref cell, ref isAlive);
            
            Assert.True(isAlive.Value);
            
            cellsNative.Dispose();
        }
    }
}