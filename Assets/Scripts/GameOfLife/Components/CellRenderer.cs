using Unity.Entities;
using UnityEngine;

namespace GameOfLife.Components
{
    public class CellRenderer : IComponentData
    {
        public MeshRenderer Renderer;
    }
}