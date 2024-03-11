using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace GameOfLife.Components
{
    [MaterialProperty("_Color")]
    public struct CellColor : IComponentData
    {
        public float4 Value;

        public static CellColor Alive()
        {
            return new CellColor { Value = new float4(1, 1, 1, 1) };
        } 
        
        public static CellColor Dead()
        {
            return new CellColor { Value = new float4(0, 0, 0, 1) };
        }
    }
}