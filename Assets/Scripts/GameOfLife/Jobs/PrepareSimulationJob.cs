using GameOfLife.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace GameOfLife.Jobs
{
    [BurstCompile]
    public struct PrepareSimulationJob : IJobParallelFor
    {
        [NativeDisableParallelForRestriction]
        public NativeArray<int> cellStatus;
        [ReadOnly]
        public NativeArray<Cell> cells;
        
        [ReadOnly]
        public NativeArray<IsAlive> isAlives;

        [ReadOnly]
        public int width;
        
        public void Execute(int index)
        {
            // Calculate the unpadded x and y values
            int TotalUnpaddedWidth = width; // Original Width (without padding)
            int unpaddedY = cells[index].CellIndex / TotalUnpaddedWidth;
            int unpaddedX = cells[index].CellIndex % TotalUnpaddedWidth;

            // Pad the coordinates
            int paddedX = unpaddedX + 1;
            int paddedY = unpaddedY + 1;

            // Calculate the padded index using the padded x and padded y values
            int TotalWidth = width + 2; // Original Width + Both Sides Padding
            int paddedIndex = paddedY * TotalWidth + paddedX;            
            var isAlive = isAlives[index];
            cellStatus[paddedIndex] = isAlive.Value ? 1 : 0;
        }
    }
}