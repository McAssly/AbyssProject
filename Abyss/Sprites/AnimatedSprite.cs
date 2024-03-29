﻿using Abyss.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Abyss.Sprites
{
    internal class AnimatedSprite
    {
        private protected Texture2D sprite_map;
        private protected Rectangle[] frames;
        private protected double frame;
        private protected int frame_limit;
        private protected double frame_speed;
        public int width, height;


        /// <summary>
        /// its very important to input the correct px_width and px_height for each frame
        /// </summary>
        /// <param name="loaded_sprite"></param>
        /// <param name="start_frame"></param>
        /// <param name="px_width"></param>
        /// <param name="px_height"></param>
        public AnimatedSprite(Texture2D loaded_sprite, int px_width, int px_height, int frame_limit = 0, double frame_speed = 1, double start_frame = 0)
        {
            sprite_map = loaded_sprite;
            frame = start_frame;

            List<Rectangle> frames = new List<Rectangle>();
            for (int y = 0; y < loaded_sprite.Height / px_height; y++)
                for (int x = 0; x < loaded_sprite.Width / px_width; x++)
                    frames.Add(new Rectangle(x * px_width, y * px_height, px_width, px_height));

            this.frames = frames.ToArray();
            if (frame_limit == 0)
                this.frame_limit = frames.Count - 1;
            else
                this.frame_limit = frame_limit - 1;

            width = px_width;
            height = px_height;
            this.frame_speed = frame_speed;
        }


        /// <summary>
        /// For pre-generated frames
        /// </summary>
        /// <param name="loaded_sprite"></param>
        /// <param name="frames"></param>
        /// <param name="px_width"></param>
        /// <param name="px_height"></param>
        /// <param name="frame_limit"></param>
        /// <param name="frame_speed"></param>
        /// <param name="start_frame"></param>
        public AnimatedSprite(Texture2D loaded_sprite, Rectangle[] frames, int px_width, int px_height, int frame_limit = 0, double frame_speed = 1, double start_frame = 0)
        {
            sprite_map = loaded_sprite;
            frame = start_frame;
            this.frames = frames;
            if (frame_limit == 0) this.frame_limit = this.frames.Length;
            else this.frame_limit = frame_limit;

            width = px_width;
            height = px_height;
            if (frame_speed == -1) this.frame_speed = this.frame_limit / Config.MaxFrameRate;
            else this.frame_speed = frame_speed;
        }


        public bool HasEnded()
        {
            return this.frame >= frame_limit;
        }

        public void Reset()
        {
            this.frame = 0;
        }


        /// <summary>
        /// Update the frame every second
        /// </summary>
        /// <param name="delta"></param>
        public void Update(double delta)
        {
            frame += delta * Variables.ANIMATION_SPEED * frame_speed;
            if (frame > frame_limit) frame = 0;
        }

        public Texture2D GetSpriteMap()
        {
            return sprite_map;
        }

        public int GetLimit()
        {
            return frame_limit;
        }

        public Rectangle DestinationRectangle()
        {
            if (frame >= frame_limit) frame = frame_limit;
            return frames[(int)frame];
        }


        public AnimatedSprite Clone()
        {
            return new AnimatedSprite(sprite_map, width, height, frame_limit, frame_speed, frame);
        }
    }
}
