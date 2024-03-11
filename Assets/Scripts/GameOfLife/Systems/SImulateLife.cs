using GameOfLife.Components;
using GameOfLife.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Profiling;
using UnityEngine;
using Grid = GameOfLife.Components.Grid;

namespace GameOfLife.Systems
{
    [UpdateAfter(typeof(SpawnGrid))]
    public partial struct SimulateLife : ISystem
    {
        private EntityQuery _query;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            var requiredQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Grid,RunSimulation>().Build(state.EntityManager);
            state.RequireForUpdate(requiredQuery);
            _query = new EntityQueryBuilder(Allocator.Temp).WithAllRW<Cell, IsAlive>().Build(state.EntityManager);
            _prepareQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Cell, IsAlive>().Build(state.EntityManager);
        }

        private EntityQuery _prepareQuery;

        static readonly ProfilerMarker s_PreparePerfMarker = new ProfilerMarker("SimulateLife.Perf");
        private NativeArray<Entity> _prepareEntities;

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
         
            var grid = SystemAPI.GetSingleton<Grid>();
            // Pad x and y dimentions by 1 on each side
            var currentState = new NativeArray<int>((grid.Dimensions.x + 2) * (grid.Dimensions.y + 2), Allocator.TempJob);
            var cells = _prepareQuery.ToComponentDataArray<Cell>(Allocator.TempJob);
            var isAlives = _prepareQuery.ToComponentDataArray<IsAlive>(Allocator.TempJob);
            
            var prepareJob = new PrepareSimulationJob
            {
                cellStatus = currentState,
                cells = cells,
                width = grid.Dimensions.x,
                isAlives = isAlives
            };
            var prepHandle = prepareJob.Schedule(cells.Length, 64);
            
            var job = new SimulateCellJob
            {
                currentState = currentState,
                GridDimensions = grid.Dimensions,
            };

            state.Dependency = job.ScheduleParallel(_query, prepHandle);
            state.Dependency.Complete();

            var e = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponent<UpdateRenderer>(e);
            currentState.Dispose();
            cells.Dispose();
            isAlives.Dispose();
        }
    }
}