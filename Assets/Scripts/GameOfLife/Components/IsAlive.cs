using Unity.Entities;

namespace GameOfLife.Components
{
    public struct IsAlive : IComponentData
    {
        public bool Value;
    }
}