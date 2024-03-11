using Unity.Entities;

namespace GameOfLife.Components
{
    public struct Cell : IComponentData
    {
        public int CellIndex;
    }
}