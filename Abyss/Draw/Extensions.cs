using Abyss.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Effect = Abyss.Magic.Effect;

namespace Abyss.Draw
{
    internal partial class DrawState : SpriteBatch
    {
        public DrawState(GraphicsDevice graphicsDevice) : base(graphicsDevice) { }
        public DrawState(GraphicsDevice graphicsDevice, int capacity) : base(graphicsDevice, capacity) { }


        /// <summary>
        /// draws the given sprite sheet
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="source"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="scalar"></param>
        /// <param name="centered"></param>
        public void Draw(SpriteSheet sheet, Vector2 source, Color color, double rotation = 0, float scalar = 1, bool centered = false)
        {
            Draw(sheet.GetCurrent(), source - sheet.origin.To2(), color, rotation, scalar, false);
        }


        /// <summary>
        /// draws the given animated sprite
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="source"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="scalar"></param>
        /// <param name="centered"></param>
        public void Draw(AnimatedSprite sprite, Vector2 source, Color color, double rotation = 0, float scalar = 1, bool centered = false)
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
                color,
                (float)rotation, origin,
                SpriteEffects.None, 0);
        }


        /// <summary>
        /// draws the given effect
        /// </summary>
        /// <param name="fx"></param>
        public void Draw(Effect fx)
        {
            this.Draw(fx.sprite, fx.position, Color.White, 0, 1, true);
        }
    }
}
