using GameOfLife.Components;
using GameOfLife.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Rendering;
using Grid = GameOfLife.Components.Grid;

namespace GameOfLife.Systems
{
    public partial struct SimulateLife : ISystem
    {
        private EntityQuery _query;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Grid>();
            _query = state.GetEntityQuery(ComponentType.ReadWrite<Cell>());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var grid = SystemAPI.GetSingleton<Grid>();
            var currentState = new NativeArray<bool>(grid.Dimensions.x * grid.Dimensions.y, Allocator.TempJob);
            ref GridCellRendererLookup lookup = ref grid.Cells.Value;
            for (var x = 0; x < grid.Dimensions.x; x++)
            {
                for (var y = 0; y < grid.Dimensions.y; y++)
                {
                    var cellEntity = lookup.Cells[y * grid.Dimensions.x + x].CellEntity;
                    var cell = state.EntityManager.GetComponentData<Cell>(cellEntity);
                    currentState[y * grid.Dimensions.x + x] = cell.IsAlive;
                }
            }
            
            var job = new SimulateCellJob
            {
                currentState = currentState,
                GridDimensions = grid.Dimensions
            };

            state.Dependency = job.ScheduleParallel(_query, state.Dependency);
            state.Dependency.Complete();
            
            var requestEntity = state.EntityManager.CreateEntity();
            state.EntityManager.SetName(requestEntity, "RequestRendererUpdate");
            state.EntityManager.AddComponentData(requestEntity, new RequestRendererUpdate());

            currentState.Dispose();
        }
    }
}