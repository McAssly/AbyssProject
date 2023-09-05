using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Abyss.Sprites
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

            this.animations = Sprite.GenerateAnimationMapping(sprite_sheet, px_width, px_height, width, height);

            this.origin = Math0.CenterWithinRectangle(px_width, px_height, width, height);
        }


        public void Update(double delta)
        {
            // we only need to update the current animation we are on so therefore:
            GetCurrent().Update(delta);
        }

        public void Update(double delta, Vector2 target_vec)
        {
            if (this.playing) this.Update(delta);

            this.UpdateDirection(target_vec);
        }


        public void UpdateDirection(Vector2 target_vec)
        {
            if (target_vec == Vector2.Zero) return;
            int direction = this.Direction(target_vec);
            if (direction != -1) this.sprite_index = direction;
        }

        public void AnimationReset()
        {
            GetCurrent().Reset();
        }

        public void Play()
        {
            this.playing = true;
        }

        public void Stop(bool reset = false)
        {
            if (reset) this.AnimationReset();
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
                    case 0: case 0.785: case 5.498: return 1;       // RIGHT
                    case 3.142: case 2.356: case 3.927: return 3;   // LEFT
                    case 1.571: return 0;                           // DOWN
                    case 4.712: return 2;                           // UP
                }
            }
            return -1;
        }

        public Vector2 DirectionToVector()
        {
            switch (sprite_index)
            {
                case 0: return new Vector2(0, 1);
                case 1: return new Vector2(1, 0);
                case 2: return new Vector2(0, -1);
                case 3: return new Vector2(-1, 0);
            }
            return Vector2.Zero;
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
