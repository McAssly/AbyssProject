using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Sprites
{
    internal class Sprite
    {
        public static Rectangle[] GenerateSpriteMapping(Texture2D raw_sprite_map, int px_width, int px_height, int width, int height)
        {
            List<Rectangle> result = new List<Rectangle>();
            // loop through the each sprite within the sprite map and place them in order within the list
            for (int y = 0; y < raw_sprite_map.Height / px_width; y++)
            {
                for (int x = 0; x < raw_sprite_map.Width / px_height; x++)
                {
                    /**
                     * The rectangle constructor first takes in position (x,y)
                     * Then it takes in the size (w, h)
                     * 
                     * In this case the sprite is positioned @ x * its px_width, y * px_height
                     * And its width and height are the dimensions of the actual sprite
                     */
                    result.Add(new Rectangle(x * px_width, y * px_height, width, height));
                }
            }

            return result.ToArray();
        }


        public static AnimatedSprite[] GenerateAnimationMapping(Texture2D raw_sprite_sheet, int px_width, int px_height, int width, int height)
        {
            List<AnimatedSprite> result = new List<AnimatedSprite>();
            for (int y = 0; y < raw_sprite_sheet.Height / px_height; y++)
            {
                List<Rectangle> frames = new List<Rectangle>();
                for (int x = 0; x < raw_sprite_sheet.Width / px_width; x++)
                {
                    frames.Add(new Rectangle(x * px_width, y * px_height, px_width, px_height));
                }
                result.Add(new AnimatedSprite(raw_sprite_sheet, frames.ToArray(), px_height, px_height));
            }



            return result.ToArray();
        }
    }
}
