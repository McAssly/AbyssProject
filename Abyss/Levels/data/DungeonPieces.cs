namespace Abyss.Levels.data
{
    internal struct DungeonPieces
    {
        // every different piece type
        internal TileMap[] opens;
        internal TileMap[] bl_corners;
        internal TileMap[] br_corners;
        internal TileMap[] tl_corners;
        internal TileMap[] tr_corners;
        internal TileMap[] bottoms;
        internal TileMap[] tops;
        internal TileMap[] lr_connectors;
        internal TileMap[] tb_connectors;

        public DungeonPieces(TileMap[] opens, TileMap[] bl_corners, TileMap[] br_corners, TileMap[] tl_corners, TileMap[] tr_corners, TileMap[] bottoms, TileMap[] tops, TileMap[] lr_connectors, TileMap[] tb_connectors)
        {
            this.opens = opens;
            this.bl_corners = bl_corners;
            this.br_corners = br_corners;
            this.tl_corners = tl_corners;
            this.tr_corners = tr_corners;
            this.bottoms = bottoms;
            this.tops = tops;
            this.lr_connectors = lr_connectors;
            this.tb_connectors = tb_connectors;
        }
    }
}
