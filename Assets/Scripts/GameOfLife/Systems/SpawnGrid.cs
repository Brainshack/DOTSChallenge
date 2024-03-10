using System.Resources;
using GameOfLife.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Grid = GameOfLife.Components.Grid;
using Random = Unity.Mathematics.Random;

namespace GameOfLife.Systems
{
    
    public partial struct SpawnGrid : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Grid>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var grid = SystemAPI.GetSingleton<Grid>();
            var rng = new Random(grid.Seed);
            var gridEntity = SystemAPI.GetSingletonEntity<Grid>();

            var builder = new BlobBuilder(Allocator.Temp);
            ref GridCellRendererLookup cellRendererLookup = ref builder.ConstructRoot<GridCellRendererLookup>();
            var length = grid.Dimensions.x * grid.Dimensions.y;
            BlobBuilderArray<GridCellRendererLookupCell> arrayBuilder = builder.Allocate(
                ref cellRendererLookup.Cells, length
            );
            var width = grid.Dimensions.x;
            var height = grid.Dimensions.y;
            var padding = grid.Padding;
            var cellPrefab = grid.CellPrefab;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var rendererEntity = state.EntityManager.Instantiate(cellPrefab);
                    state.EntityManager.SetComponentData(rendererEntity, new LocalTransform
                    {
                        Position = new float3(x + x * padding, y + y * padding, 0),
                        Rotation = Quaternion.identity,
                        Scale = 1
                    });

                    var cellEntity = state.EntityManager.CreateEntity();
                    state.EntityManager.AddComponentData(cellEntity, new Cell { IsAlive = rng.NextFloat(0f,1f) > grid.RandomSpawnThreshold, CellIndex =y * width + x});
                    
                    var cell = new GridCellRendererLookupCell
                    {
                        RendererEntity = rendererEntity,
                        CellEntity = cellEntity
                    };

                    arrayBuilder[y * width + x] = cell;
                }
            }

            var blobRef = builder.CreateBlobAssetReference<GridCellRendererLookup>(Allocator.Persistent);
            builder.Dispose();
            grid.Cells = blobRef;
            state.EntityManager.SetComponentData(gridEntity, grid);
            state.Enabled = false;
        }
    }
}