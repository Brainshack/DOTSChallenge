using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace GameOfLife.Components
{
    public struct Grid : IComponentData
    {
        public int2 Dimensions;
        public float Padding;
        public uint Seed;
        public uint RandomSpawnThreshold;
    }
}