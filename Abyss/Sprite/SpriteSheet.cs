using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Abyss.Master;
using Vector = Abyss.Master.Vector;

namespace Abyss.Sprite
{
    internal class SpriteSheet
    {
        private protected Texture2D sprite_sheet;
        private protected AnimatedSprite[] animations;
        private protected int sprite_index;
        public int px_width, px_height;
        public int width, height;
        public Vector origin;

        private protected int shared_frame_limit;
        private protected double shared_frame_speed;
        private protected double shared_start_frame;

        /// <summary>
        /// For having multiple sprites on a single sheet
        /// </summary>
        /// <param name="sprite_sheet">The sprite sheet to draw from and map to</param>
        /// <param name="px_width">the width of each frame in pixels</param>
        /// <param name="px_height">the height of each frame</param>
        /// <param name="width">the width of the actual sprite (collision)</param>
        /// <param name="height">the height of the actual sprite</param>
        /// <param name="frame_limit">the SHARED frame limit of every animation frame</param>
        /// <param name="frame_speed">the SHARED frame speed of every animation</param>
        /// <param name="start_frame">the SHARED starting frame for every animation</param>
        public SpriteSheet(Texture2D sprite_sheet, int px_width, int px_height, int width, int height, int frame_limit = 0, double frame_speed = 1, double start_frame = 0)
        {
            this.sprite_sheet = sprite_sheet;
            this.sprite_index = 0;
            this.px_width = px_width;
            this.px_height = px_height;
            this.width = width;
            this.height = height;

            shared_frame_limit = frame_limit;
            shared_frame_speed = frame_speed;
            shared_start_frame = start_frame;

            // generate all the animated sprites
            List<AnimatedSprite> animations = new List<AnimatedSprite>();
            for (int y=0; y<sprite_sheet.Height/px_height; y++)
            {
                List<Rectangle> frames = new List<Rectangle>();
                for (int x=0; x<sprite_sheet.Width/px_width; x++)
                {
                    frames.Add(new Rectangle(x * px_width, y * px_height, px_width, px_height));
                }
                animations.Add(new AnimatedSprite(sprite_sheet, frames.ToArray(), px_width, px_height, frame_limit, frame_speed, start_frame));
            }

            this.animations = animations.ToArray();

            this.origin = MathUtil.CenterWithinRectangle(px_width, px_height, width, height);
        }


        public void Update(double delta)
        {
            // we only need to update the current animation we are on so therefore:
            animations[sprite_index].Update(delta);
        }

        public Texture2D GetSheet()
        {
            return sprite_sheet;
        }


        public SpriteSheet Clone()
        {
            return new SpriteSheet(sprite_sheet, px_width, px_height, width, height, shared_frame_limit, shared_frame_speed, shared_start_frame);
        }
    }
}
