using GameOfLife.Components;
using Unity.Entities;
using UnityEngine;

namespace GameOfLife.Systems
{
    [UpdateBefore(typeof(SpawnGrid))]
    public partial class FindMeshRenderer : SystemBase
    {
        protected override void OnUpdate()
        {
            var mr = GameObject.FindObjectOfType<MeshRenderer>();

            var mrEntity = EntityManager.CreateEntity();
            EntityManager.AddComponentObject(mrEntity, new CellRenderer
            {
                Renderer = mr
            });

            Enabled = false;
        }

        private object FindObjectOfType<T>()
        {
            throw new System.NotImplementedException();
        }
    }
}