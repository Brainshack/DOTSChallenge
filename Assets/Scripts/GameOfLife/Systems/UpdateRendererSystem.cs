using GameOfLife.Components;
using GameOfLife.Jobs;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using Grid = GameOfLife.Components.Grid;

namespace GameOfLife.Systems
{
    [UpdateAfter(typeof(SimulateLife))]
    public partial class UpdateRendererSystem : SystemBase
    {
        private EntityQuery _query;
        private Texture2D _gridTexture;
        private EntityQuery _crQuery;

        protected override void OnCreate()
        {
            RequireForUpdate<UpdateRenderer>();
            _query = new EntityQueryBuilder(Allocator.Temp).WithAll<Cell, IsAlive>().Build(EntityManager);
        }

        protected override void OnUpdate()
        { 
            var rendererEntity = SystemAPI.GetSingletonEntity<UpdateRenderer>();
            var grid = SystemAPI.GetSingleton<Grid>();
            MeshRenderer mr = default;
            
            Entities.ForEach((CellRenderer cellRenderer) =>
            {
                mr = cellRenderer.Renderer;
            }).WithoutBurst().Run();
            
            var cells = _query.ToComponentDataArray<Cell>(Allocator.TempJob);
            var isAlives = _query.ToComponentDataArray<IsAlive>(Allocator.TempJob);

            var _tempTexture = mr.material.GetTexture("_BaseColor");
            if (_tempTexture && _tempTexture.width == grid.Dimensions.x)
            {
                _gridTexture = _tempTexture as Texture2D;
            }
            else
            {
                _gridTexture= new Texture2D(grid.Dimensions.x, grid.Dimensions.y, TextureFormat.RGBA32, 1, true);
            }
            var colors = _gridTexture.GetRawTextureData<Color32>();
            
            var job = new UpdateColorsJob
            {
                colors = colors,
                cells = cells,
                isALives = isAlives
            };
            this.Dependency = job.Schedule(cells.Length, 64);
            this.Dependency.Complete();

            _gridTexture.filterMode = FilterMode.Point;
            _gridTexture.Apply();
            
            var mat = mr.material;
            mat.SetTexture("_BaseColor", _gridTexture);

            colors.Dispose();
            cells.Dispose();
            isAlives.Dispose();
            
            EntityManager.DestroyEntity(rendererEntity);
        }
    }
}