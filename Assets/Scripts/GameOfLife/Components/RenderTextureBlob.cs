using Unity.Entities;
using Unity.Mathematics;

namespace GameOfLife.Components
{
    public struct RenderTextureBlob
    {
        public BlobArray<float4> Colors;

    }
}