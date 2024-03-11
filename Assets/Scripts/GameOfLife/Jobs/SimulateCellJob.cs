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
        [ReadOnly] public NativeArray<int> currentState;
        [ReadOnly] public int2 GridDimensions;
        [BurstCompile]
        public void Execute(ref Cell cell, ref IsAlive isAlive)
        {
            var aliveNeighbors = CountAliveNeighbors(cell);
            
            // Cell is currently alive
            if (isAlive.Value)
            {
                if (aliveNeighbors <= 1)
                {
                    isAlive.Value = false;
                    return;
                }
                if (aliveNeighbors == 2 || aliveNeighbors == 3)
                {
                    return;
                }

                isAlive.Value = false;
            }
            else
            {
                if (aliveNeighbors == 3)
                {
                    isAlive.Value = true;
                }   
            }
        }

        public int CountAliveNeighbors(Cell cell)
        {
            int aliveNeighbors = 0;

            // Calculate the unpadded x and y values
            int TotalUnpaddedWidth = GridDimensions.x; // Original Width (without padding)
            int unpaddedY = cell.CellIndex / TotalUnpaddedWidth;
            int unpaddedX = cell.CellIndex % TotalUnpaddedWidth;

            // Pad the coordinates
            int paddedX = unpaddedX + 1;
            int paddedY = unpaddedY + 1;

            // Calculate the padded index using the padded x and padded y values
            int TotalWidth = GridDimensions.x + 2; // Original Width + Both Sides Padding
            int index = paddedY * TotalWidth + paddedX;
            
            var topIndex = index - TotalWidth;
            aliveNeighbors += currentState[topIndex];
            
            var topLeftIndex = topIndex - 1;
            aliveNeighbors += currentState[topLeftIndex];
            
            var topRightIndex = topIndex + 1;
            aliveNeighbors += currentState[topRightIndex];
            
            var bottomIndex = index + TotalWidth;
            aliveNeighbors += currentState[bottomIndex];

            var bottomLeftIndex = bottomIndex - 1;
            aliveNeighbors += currentState[bottomLeftIndex];

            var bottomRightIndex = bottomIndex + 1;
            aliveNeighbors += currentState[bottomRightIndex];

            var leftIndex = index - 1;
            aliveNeighbors += currentState[leftIndex];

            var rightIndex = index + 1;
            aliveNeighbors += currentState[rightIndex];
            return aliveNeighbors;
        }
    }
}