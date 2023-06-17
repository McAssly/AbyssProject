using Abyss.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
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
        private Vector2 pos = new Vector2(5*16, 5*16);
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
        public void Move(TileMap map, double delta)
        {
            // if the input vector is not zero then the player must be trying to move
            if (inputVec != Vector2.Zero)
            {
                vel = MathUtil.MoveToward(vel, inputVec * MAX_SPEED, MAX_ACCEL * delta);
                // collision
                for (int i=0; i < 4; i++) // loop through each offset (corner)
                {
                    Vector2 tilePos = Vector2.Clamp(MathUtil.CoordsToTileCoords(pos + vel + MathUtil.offsets[i]), Vector2.Zero, new Vector2(Globals.TILE_SIZE * Globals.TILE_SIZE - Globals.TILE_SIZE));
                    Tile targetTile = map.GetCollisionLayer().GetTiles()[(int)tilePos.Y, (int)tilePos.X];

                    // if the tile is a blocked space or a collision tile 
                    if (!targetTile.NULL)
                    {
                        // If the target new position to move to is not passable subtract from the velocity until the target position is moveable
                        while (targetTile.Colliding(pos + vel, new Vector2(Globals.TILE_SIZE)))
                        {
                            // Update the velocity
                            vel = MathUtil.MoveToward(vel, Vector2.Zero, FRICTION * delta);

                            // Update the target positions to test for
                            tilePos = Vector2.Clamp(MathUtil.CoordsToTileCoords(pos + vel + MathUtil.offsets[i]), Vector2.Zero, new Vector2(Globals.TILE_SIZE * Globals.TILE_SIZE - Globals.TILE_SIZE));
                            targetTile = map.GetCollisionLayer().GetTiles()[(int)tilePos.Y, (int)tilePos.X];
                        }
                    }
                }
            }
            else
                vel = MathUtil.MoveToward(vel, Vector2.Zero, FRICTION * delta);

            pos += vel;
        }

        /**
         * Simply updates the draw object's position
         */
        public void UpdateDrawObj()
        {
            pos = Vector2.Clamp(pos, Vector2.Zero, new Vector2(16 * 16 - 16));
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
