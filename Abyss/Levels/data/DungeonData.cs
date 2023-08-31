namespace Abyss.Levels.data
{
    /// <summary>
    /// stores basic dungeon data
    /// </summary>
    internal struct DungeonData
    {
        internal int width, height, path_limit;
        internal double divergence_rate;
        internal DungeonPieces pieces;

        public DungeonData(int width, int height, int path_limit, double divergence_rate, DungeonPieces pieces)
        {
            this.width = width;
            this.height = height;
            this.path_limit = path_limit;
            this.divergence_rate = divergence_rate;
            this.pieces = pieces;
        }
    }
}
