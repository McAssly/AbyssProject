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

namespace Abyss.Entities
{
    internal class Player : Entity
    {
        public Player(Texture2D texture) :base(texture)
        {
            // BOTH OF THESE ARE PLACEHOLDERS
            this.drawObj = new Rectangle(0, 0, 16, 16);
            this.pos = new Vector2(5 * 16, 5 * 16);
            this.speed = 2;
        }

        /**
         * Determines the input vector of the player and normalizes it to a radius of 1
         * 
         * @param   KeyboardState   the current keyboard state of the game (what keys are being pressed)
         */
        public void CalcInputVector(KeyboardState KB)
        {
            // If a key is being pressed it is = 1, if it is not then it is = 0
            // Therefore if the player is pressing the right key their input.x should be (+), if left then (-), if both then (0)
            this.movement_vec.X = MathUtil.KeyStrength(KB, Controls.Right) - MathUtil.KeyStrength(KB, Controls.Left);
            this.movement_vec.Y = MathUtil.KeyStrength(KB, Controls.Down) - MathUtil.KeyStrength(KB, Controls.Up);

            if (this.movement_vec != Vector2.Zero)
                this.movement_vec.Normalize();
        }
    }
}
