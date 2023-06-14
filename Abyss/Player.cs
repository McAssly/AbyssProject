using Abyss.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss
{
    internal class Player
    {
        private Rectangle drawObj = new Rectangle(0, 0, 16, 16); // placeholder

        // declare the maximum values for the player
        private readonly int MAX_SPEED = 2;
        private readonly int MAX_ACCEL = 50;
        private readonly int FRICTION = 50;

        // Declare input variables
        private Vector2 inputVec = Vector2.Zero;

        // Declare the position variables
        private Vector2 pos = Vector2.Zero;
        private Vector2 vel = Vector2.Zero;

        public Player() { }

        /**
         * Determines the input vector of the player and normalizes it to a radius of 1
         * 
         * @param   KeyboardState   the current keyboard state of the game (what keys are being pressed)
         */
        public void CalcInputVector(KeyboardState KB)
        {
            // If a key is being pressed it is = 1, if it is not then it is = 0
            // Therefore if the player is pressing the right key their input.x should be (+), if left then (-), if both then (0)
            inputVec.X = MathUtil.KeyStrength(KB, Controls.Right) - MathUtil.KeyStrength(KB, Controls.Left);
            inputVec.Y = MathUtil.KeyStrength(KB, Controls.Down) - MathUtil.KeyStrength(KB, Controls.Up);

            if (inputVec != Vector2.Zero)
                inputVec.Normalize();
        }


        /** 
         * Moves the player based on the current input vector (already normalized)
         * 
         * @param  double       the time it took for the last frame to load (seconds)
         * @param  Vector2      the input vector of the player
         */
        public void Move(double delta)
        {
            // if the input vector is not zero then the player must be trying to move
            if (inputVec != Vector2.Zero)
                vel = MathUtil.MoveToward(vel, inputVec * MAX_SPEED, MAX_ACCEL * delta);
            else
                vel = MathUtil.MoveToward(vel, Vector2.Zero, FRICTION * delta);

            pos += vel;
        }

        /**
         * Gets the adjacent tiles that are blocked to the player
         * 
         * @param   TileMap     the current tilemap
         */
        public List<Tile> AdjacentTiles(TileMap tileMap)
        {
            // First get the layers that are blocked and not the ones that aren't
            List<MapLayer> blockedLayers = new List<MapLayer>();
            foreach (MapLayer layer in tileMap.GetLayers())
            {
                if (layer.IsBlocked())
                    blockedLayers.Add(layer);
            }
            // if there are no blocked layers in the tile map there are no collidable tiles
            if (blockedLayers.Count <= 0) return null;
            // otherwise continue...
            List<Tile> tiles = new List<Tile>();
            // get the tile position of the player
            Vector2 tPos = MathUtil.CoordsToTileCoords(this.pos);

            foreach (MapLayer layer in blockedLayers)
            {
                // loop through 8 times there should only be about 8 tiles adjacent to the player's tile
                for (int i = 0; i < 8; i++)
                {
                    // this loop will us a parametric equation to grab the adjacent tiles
                    // since i = 4 means we're at 0,0 there is no need for it so skip:
                    if (i == 4) continue;
                    else
                    {
                        int x = (int)Math.Tan((i % 3) * Math.PI / 4 + Math.PI / 4); // repeating pattern of -1, 0, 1
                        int y = (i / 3) + 1; // repeating pattern of 3x-1, 3x0, 3x1
                        tiles.Add(layer.GetTiles()[(int)tPos.X+x, (int)tPos.Y+y]);
                    }
                }
            }

            return tiles;
        }

        /**
         * Simply updates the draw object's position
         */
        public void UpdateDrawObj()
        {
            // if the player's position updated then therefore so does the draw object's
            if (drawObj.X != pos.X)
                drawObj.X = (int)pos.X;

            // the same goes for the y-axis
            if (drawObj.Y != pos.Y)
                drawObj.Y = (int)pos.Y;
        }

        /**
         * Draws the player sprite
         * 
         * @param   SpriteBatch     the sprite batch to draw to
         */
        public void Draw(SpriteBatch spriteBatch) // placeholder
        {
            if (spriteBatch == null) return;
            spriteBatch.Draw(Globals.TESTBOX, drawObj, Color.White);
        }
    }
}
