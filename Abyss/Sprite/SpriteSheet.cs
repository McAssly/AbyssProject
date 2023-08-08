using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Abyss.Master;
using Vector = Abyss.Master.Vector;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Abyss.Sprite
{
    internal class SpriteSheet
    {
        private protected Texture2D sprite_sheet;
        private protected AnimatedSprite[] animations;
        private protected int sprite_index;
        private protected bool playing;
        private protected bool looping;
        private protected int section_seperator;
        private protected int section;
        public int px_width, px_height;
        public int width, height;
        public Vector origin;

        public int shared_frame_limit;
        public double shared_frame_speed;
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
        public SpriteSheet(Texture2D sprite_sheet, int px_width, int px_height, int width, int height, int section_sperator, double frame_speed = 1, int frame_limit = 0, double start_frame = 0)
        {
            this.sprite_sheet = sprite_sheet;
            this.sprite_index = 0;
            this.section = 0;
            this.section_seperator = section_sperator;
            this.playing = false;
            this.looping = false;
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
            GetCurrent().Update(delta);
        }

        public void Update(double delta, Vector2 movement_vec)
        {
            if (this.playing) this.Update(delta);

            int direction = this.Direction(movement_vec);
            if (direction != -1)
            {
                this.sprite_index = direction;
            }
        }

        public void AnimationReset()
        {
            GetCurrent().Reset();
        }

        public void Play()
        {
            this.playing = true;
        }

        public void Stop()
        {
            this.playing = false;
        }

        public void Loop()
        {
            this.looping = true;
        }

        public void UnLoop()
        {
            this.looping = false;
        }

        public bool IsPlaying()
        {
            return this.playing;
        }

        public bool HasEnded()
        {
            if (looping) return false;
            if (GetCurrent().HasEnded())
            {
                GetCurrent().Reset();
                return true;
            }
            return false;
        }


        public int GetSection()
        {
            return section;
        }

        public void SetSection(int section)
        {
            // section indicies: (example)
            // 0 = idle
            // 1 = running
            // 2 = attacking
            this.section = section;
        }


        public int Direction(Vector2 normalized_direction)
        {
            // direction indicies:
            // 0 = left
            // 1 = down
            // 2 = right
            // 3 = up
            if (normalized_direction != Vector2.Zero)
            {
                double angle = Math.Atan2(normalized_direction.Y, normalized_direction.X);
                if (angle < 0) angle += Math.PI * 2;

                angle = Math.Round(angle, 3);

                switch (angle)
                {
                    case 0: case 0.785: case 5.498: return 0;
                    case 3.142: case 2.356: case 3.927: return 2;
                    case 1.571: return 1;
                    case 4.712: return 3;
                }
            }
            return -1;
        }

        public Texture2D GetSheet()
        {
            return sprite_sheet;
        }

        public AnimatedSprite GetCurrent()
        {
            return animations[sprite_index + (section * section_seperator)];
        }

        public SpriteSheet Clone()
        {
            return new SpriteSheet(sprite_sheet, px_width, px_height, width, height, section_seperator, shared_frame_speed, shared_frame_limit, shared_start_frame);
        }
    }
}
