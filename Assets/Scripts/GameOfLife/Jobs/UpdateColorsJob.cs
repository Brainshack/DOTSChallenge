using GameOfLife.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace GameOfLife.Jobs
{
    [BurstCompile]
    public struct UpdateColorsJob : IJobParallelFor
    {
        [WriteOnly]
        public NativeArray<Color32> colors;

        [ReadOnly]
        public NativeArray<Cell> cells;
        
        [ReadOnly]
        public NativeArray<IsAlive> isALives;
        
        [BurstCompile]
        public void Execute(int index)
        {
            var cell = cells[index];
            var isAlive = isALives[index];
            colors[cell.CellIndex] = isAlive.Value ? Color.white : Color.black;
        }
    }
}