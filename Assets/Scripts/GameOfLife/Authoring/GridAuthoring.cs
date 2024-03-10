using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Grid = GameOfLife.Components.Grid;

namespace GameOfLife.Authoring
{
    public class GridAuthoring : MonoBehaviour
    {
        public int2 Dimensions;
        public GameObject CellPrefab;
        public float Padding;
        public uint Seed;
        [Range(0,1)]
        public float RandomThreshold;
        
        public class GridBaker : Baker<GridAuthoring>
        {
            public override void Bake(GridAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Grid { Dimensions = authoring.Dimensions, CellPrefab = GetEntity(authoring.CellPrefab, TransformUsageFlags.None), Padding = authoring.Padding, Seed = authoring.Seed, RandomSpawnThreshold = authoring.RandomThreshold});
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