using Abyss.Entities.Magic;
using Abyss.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Effect = Abyss.Entities.Effect;
using Abyss.Sprite;

namespace Abyss.Draw
{
    internal partial class DrawBatch : SpriteBatch
    {
        public void Draw(SpriteSheet sheet, Vector2 source, double rotation = 0, float scalar = 1, bool centered = false)
        {

        }


        public void Draw(AnimatedSprite sprite, Vector2 source, double rotation = 0, float scalar = 1, bool centered = false)
        {
            // calculate the new width and height of the draw object based on scalar
            int width = (int)(sprite.width * scalar);
            int height = (int)(sprite.height * scalar);

            // set the origin to zero
            Vector2 origin = Vector2.Zero;
            // if asked to be centered change the origin to the central position
            if (centered)
                origin = new Vector2(width / 2, height / 2);

            // draw the animated sprite's current frame
            this.Draw(
                sprite.GetSpriteMap(),
                new Rectangle((int)source.X, (int)source.Y, width, height),
                sprite.DestinationRectangle(),
                Color.White,
                (float)rotation, origin,
                SpriteEffects.None, 0);
        }


        public void Draw(Effect fx)
        {
            this.Draw(fx.sprite, fx.position, 0, 1, true);
        }
    }
}
