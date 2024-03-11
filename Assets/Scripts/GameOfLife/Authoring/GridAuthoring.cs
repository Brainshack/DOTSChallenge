using System;
using GameOfLife.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Grid = GameOfLife.Components.Grid;

namespace GameOfLife.Authoring
{
    public class GridAuthoring : MonoBehaviour
    {
        public int2 Dimensions;
        public float Padding;
        public uint Seed;
        [Range(1,100)]
        public int RandomThreshold;
        
        public class GridBaker : Baker<GridAuthoring>
        {
            public override void Bake(GridAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Grid { Dimensions = authoring.Dimensions, Padding = authoring.Padding, Seed = authoring.Seed, RandomSpawnThreshold = authoring.RandomThreshold});
                AddComponent<RebuildGrid>(entity);
            }
        }

        private void OnDrawGizmos()
        {
            var width = Dimensions.x + Dimensions.x * Padding;
            var height = Dimensions.y + Dimensions.y * Padding;
            Gizmos.DrawWireCube(new Vector3(width / 2f, height / 2f, 0), new Vector3(width, height, 0));
        }
    }
}