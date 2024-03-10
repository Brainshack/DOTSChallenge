using Unity.Entities;

namespace GameOfLife.Components
{
    public struct Cell : IComponentData
    {
        public bool IsAlive;
        public int CellIndex;
    }
}