using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Levels.data
{
    internal class DungeonMap
    {
        private static Random rand = new Random();

        internal bool is_entrance;
        internal bool is_end;
        internal byte piece_type;
        internal TileMap piece;
        internal int[] next_maps;
        internal int width, height;

        public DungeonMap(DungeonPieces pieces, int[] next_maps, int index, int limit, int previous_level)
        {
            this.next_maps = next_maps;

            this.is_entrance = index == 0;
            this.is_end = index == limit;

            // set the exit from the entrance to next level index
            if (is_entrance) this.next_maps[2] = -previous_level - 2;

            this.piece_type = CalculatePieceType(previous_level);
            this.piece = RandomPiece(pieces);


            // preset the width and height
            this.width = this.height = 16;
        }


        private byte CalculatePieceType(int previous_level)
        {
            // check which directions have a connection
            bool north = this.next_maps[0] != -1;
            bool east = this.next_maps[1] != -1;
            bool south = this.next_maps[2] != -1;
            bool west = this.next_maps[3] != -1;

            if (north && south && west && east) return 0;   // ALL SIDES OPEN
            if (east && north) return 1;                    // BOTTOM LEFT CORNER
            if (west && north) return 2;                    // BOTTOM RIGHT CORNER
            if (east && south) return 3;                    // TOP LEFT CORNER
            if (west && south) return 4;                    // TOP RIGHT CORNER
            if (east && west && north) return 5;            // BOTTOM IS CLOSED
            if (east && west && south) return 6;            // TOP IS CLOSED
            if (east && west) return 7;                     // LEFT-RIGHT CONNECTORS
            if (south && north) return 8;                   // TOP-BOTTOM CONNECTORS

            throw new Exception("ERROR: unable to determine piece type");
        }


        private TileMap RandomPiece(DungeonPieces pieces)
        {
            TileMap result;
            switch (this.piece_type)
            {
                case 0: result = pieces.opens[rand.Next(pieces.opens.Length)]; break;
                case 1: result = pieces.bl_corners[rand.Next(pieces.bl_corners.Length)]; break;
                case 2: result = pieces.br_corners[rand.Next(pieces.br_corners.Length)]; break;
                case 3: result = pieces.tl_corners[rand.Next(pieces.tl_corners.Length)]; break;
                case 4: result = pieces.tr_corners[rand.Next(pieces.tr_corners.Length)]; break;
                case 5: result = pieces.bottoms[rand.Next(pieces.bottoms.Length)]; break;
                case 6: result = pieces.tops[rand.Next(pieces.tops.Length)]; break;
                case 7: result = pieces.lr_connectors[rand.Next(pieces.lr_connectors.Length)]; break;
                case 8: result = pieces.tb_connectors[rand.Next(pieces.tb_connectors.Length)]; break;
                default:
                    throw new Exception("ERROR: improper piece type obtained when generating dungeon");
            }
            return result;
        }
    }
}
