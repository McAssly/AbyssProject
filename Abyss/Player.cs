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
                vel = MathUtil.moveToward(vel, inputVec * MAX_SPEED, MAX_ACCEL * delta);
            else
                vel = MathUtil.moveToward(vel, Vector2.Zero, FRICTION * delta);

            pos += vel;
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
