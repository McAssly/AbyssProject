using Abyss.Entities;
using Abyss.Levels;
using Abyss.Levels.data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Draw
{
    internal partial class DrawState : SpriteBatch
    {

        /// <summary>
        /// draws the given map
        /// </summary>
        /// <param name="tilemap"></param>
        public void Draw(Map tilemap)
        {
            foreach (Layer layer in tilemap.GetLayers())
                Draw(layer);
        }

        /// <summary>
        /// draws the given tile layer
        /// </summary>
        /// <param name="layer"></param>
        public void Draw(Layer layer)
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
                                new Rectangle((int)layer.GetTiles()[y, x].position.X, (int)layer.GetTiles()[y, x].position.Y, 16, 16),
                                layer.GetTiles()[y, x].rectangle,
                                Color.White
                            );
                    }
                }
            }
        }
    }
}
