using GameOfLife.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace GameOfLife.Jobs
{
    [BurstCompile]
    public partial struct SimulateCellJob : IJobEntity
    {
        [ReadOnly] public NativeArray<bool> currentState;
        [ReadOnly] public int2 GridDimensions;
        
        public void Execute(ref Cell cell)
        {
            int aliveNeighbors = 0;
            var maxIndex = (GridDimensions.x * GridDimensions.y) - 1;
            
            var topIndex = cell.CellIndex - GridDimensions.x;
            if (topIndex >= 0 && topIndex < maxIndex && currentState[topIndex])
                aliveNeighbors++;
            
            var topLeftIndex = topIndex - 1;
            if (topLeftIndex >= 0 && topLeftIndex < maxIndex && currentState[topLeftIndex])
                aliveNeighbors++;
            
            var topRightIndex = topIndex + 1;
            if (topRightIndex >= 0 && topRightIndex < maxIndex && currentState[topRightIndex])
                aliveNeighbors++;
            
            var bottomIndex = cell.CellIndex + GridDimensions.x;
            if (bottomIndex >= 0 && bottomIndex < maxIndex && currentState[bottomIndex])
                aliveNeighbors++;
            
            var bottomLeftIndex = bottomIndex - 1;
            if (bottomLeftIndex >= 0 && bottomLeftIndex < maxIndex && currentState[bottomLeftIndex])
                aliveNeighbors++;
            
            var bottomRightIndex = bottomIndex + 1;
            if (bottomRightIndex >= 0 && bottomRightIndex < maxIndex && currentState[bottomRightIndex])
                aliveNeighbors++;
            
            var leftIndex = cell.CellIndex - 1;
            if (leftIndex >= 0 && leftIndex < maxIndex && currentState[leftIndex])
                aliveNeighbors++;
            
            var rightIndex = cell.CellIndex + 1;
            if (rightIndex >= 0 && rightIndex < maxIndex && currentState[rightIndex])
                aliveNeighbors++;

            if (aliveNeighbors <= 1)
            {
                cell.IsAlive = false;
                return;
            }
            // Cell is currently alive
            if (cell.IsAlive)
            {
                if (aliveNeighbors == 2 || aliveNeighbors == 3)
                {
                    cell.IsAlive = true;
                    return;
                }

                if (aliveNeighbors >= 4)
                {
                    cell.IsAlive = false;
                }
            }
            else
            {
                if (aliveNeighbors == 3)
                {
                    cell.IsAlive = true;
                }   
            }
        }
    }
}