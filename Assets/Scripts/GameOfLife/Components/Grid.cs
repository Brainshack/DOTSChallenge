using Unity.Entities;
using Unity.Mathematics;

namespace GameOfLife.Components
{
    public struct Grid : IComponentData
    {
        public int2 Dimensions;
        public Entity CellPrefab;
        public float Padding;
        public BlobAssetReference<GridCellRendererLookup> Cells;
        public uint Seed;
        public float RandomSpawnThreshold;
    }
}