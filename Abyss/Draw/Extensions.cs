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

namespace Abyss.Draw
{
    internal partial class DrawBatch : SpriteBatch
    {
        public void Draw(AnimatedSprite sprite, Vector2 source, double rotation)
        {
            this.Draw(
                sprite.GetSpriteMap(),
                new Rectangle((int)source.X, (int)source.Y, sprite.width, sprite.height),
                sprite.DestinationRectangle(),
                Color.White,
                (float)rotation, new Vector2(sprite.width / 2, sprite.height / 2),
                SpriteEffects.None, 0);
        }
    }
}
