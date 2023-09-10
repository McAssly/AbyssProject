using Abyss.Entities;

namespace Abyss.Levels.data
{
    /// <summary>
    /// stores map segments for level data
    /// </summary>
    internal struct TileMap
    {
        // Represents the assortment of tiles within different layers (lower the layer the higher load priority)
        public TileLayer[] tile_layers;
        public TileLayer collision_layer;
        // the map to go to based on the direction given, 0 = north, 1 = east, 2 = south, 3 = west
        public int[] index_locations;

        // list of enemies on the map
        public Enemy[] enemies;

        /// <summary>
        /// constructs the map segement with its associated data
        /// </summary>
        /// <param name="tile_layers"></param>
        /// <param name="collision_layer"></param>
        /// <param name="north_index"></param>
        /// <param name="east_index"></param>
        /// <param name="south_index"></param>
        /// <param name="west_index"></param>
        public TileMap(TileLayer[] tile_layers, TileLayer collision_layer, Enemy[] enemies, int west_index, int north_index, int east_index, int south_index)
        {
            this.tile_layers = tile_layers;
            this.collision_layer = collision_layer;
            this.index_locations = new int[4]
            {
                west_index, north_index, east_index, south_index
            };
            this.enemies = enemies;
        }
    }
}
