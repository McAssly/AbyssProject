using Abyss.Entities;
using Abyss.Map;
using Abyss.Master;
using Abyss.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Abyss.Draw
{
    internal partial class DrawBatch : SpriteBatch
    {
        public DrawBatch(GraphicsDevice graphicsDevice) : base(graphicsDevice) { }
        public DrawBatch(GraphicsDevice graphicsDevice, int capacity) : base(graphicsDevice, capacity) { }

        public void Draw(TileMap tilemap)
        {
            foreach (MapLayer layer in tilemap.GetLayers())
            {
                Draw(layer);
            }
        }

        public void Draw(MapLayer layer)
        {
            // loop through the tile grid
            for (int y = 0; y < layer.GetHeight(); y++)
            {
                for (int x = 0; x < layer.GetWidth(); x++)
                {
                    // if the tile is not null then it can be drawn
                    if (!layer.GetTiles()[y, x].NULL)
                    {
                        // draw the tile
                        Draw
                            (
                                layer.GetTileset(),
                                new Rectangle((int)layer.GetTiles()[y, x].pos.X, (int)layer.GetTiles()[y, x].pos.Y, Globals.TILE_SIZE, Globals.TILE_SIZE),
                                layer.GetTiles()[y, x].rect,
                                Color.White
                            ) ;
                    }
                }
            }
        }
    }
}
