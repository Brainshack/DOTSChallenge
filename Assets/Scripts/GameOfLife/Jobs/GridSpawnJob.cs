using GameOfLife.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace GameOfLife.Jobs
{
    [BurstCompile]
    public partial struct GridSpawnJob : IJobEntity
    {
        public int seed;

        public void Execute([EntityIndexInQuery]int index, ref Cell cell, ref IsAlive isAlive)
        {
            var rng = new Random((uint) (seed + index * 20));
            cell.CellIndex = index;
            isAlive.Value = rng.NextBool();
        }
    }
}