using GameOfLife.Components;
using NUnit.Framework;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Grid = GameOfLife.Components.Grid;
using Random = Unity.Mathematics.Random;

namespace GameOfLife.Systems
{
    public partial struct SpawnGrid : ISystem
    {
        private EntityQuery _cellQuery;
        private EntityArchetype _cellCreateArchetype;

        public void OnCreate(ref SystemState state)
        {
            var requiredQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Grid, RebuildGrid>()
                .Build(state.EntityManager);
            state.RequireForUpdate(requiredQuery);
            _cellQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Cell>().Build(state.EntityManager);
            _cellCreateArchetype = state.EntityManager.CreateArchetype(
                typeof(Cell),
                typeof(IsAlive)
            );
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var gridEntity = SystemAPI.GetSingletonEntity<Grid>();
            var grid = SystemAPI.GetSingleton<Grid>();

            state.EntityManager.DestroyEntity(_cellQuery);
            state.EntityManager.CreateEntity(_cellCreateArchetype, grid.Dimensions.x * grid.Dimensions.y);

            var rng = new Random(grid.Seed);

            int index = 0;
            foreach (var (cell, isAlive) in SystemAPI.Query<RefRW<Cell>, RefRW<IsAlive>>())
            {
                cell.ValueRW.CellIndex = index;
                isAlive.ValueRW.Value = rng.NextInt(0, 101) >  grid.RandomSpawnThreshold;
                index++;
            }


            state.EntityManager.AddComponent<RunSimulation>(gridEntity);
            state.EntityManager.RemoveComponent<RebuildGrid>(gridEntity);
        }
    }
}