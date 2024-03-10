using Unity.Entities;

namespace GameOfLife.Components
{
    public struct GridCellRendererLookupCell
    {
        public Entity RendererEntity;
        public Entity CellEntity;
    }
    
    public struct GridCellRendererLookup
    {
        public BlobArray<GridCellRendererLookupCell> Cells;
    }
}