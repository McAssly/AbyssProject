using Abyss.Map;
using Abyss.Master;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Entities
{
    internal class Entity
    {
        private protected Rectangle drawObj;
        private protected Texture2D texture;
        private protected int drawSize = Globals.TILE_SIZE; // by default we will use the global tile size
        // offsets for the entity, useful for knowing where each corner and face are located on the entity's draw object
        private protected readonly Vector2[] _offsets;

        // declare max values for the entity
        private protected readonly int MAX_SPEED = 1;
        private protected readonly int MAX_ACCEL = 50;
        private protected readonly int FRICTION = 50;

        // declare the entity's stats
        private protected double speed = 1;
        private protected double max_health;
        private protected double max_mana;

        // current stats
        private protected double health;
        private protected double mana;

        // declare the entity's substats
        private protected double attack;
        private protected double crit_dmg;
        private protected double crit_rate; // between 0 and 1, a percentage value
        private protected double defense;    // physical resistence
        private protected double resistence; // magical resistence

        // declare position variables
        private protected Vector2 movement_vec = Vector2.Zero;
        private protected Vector2 pos;
        private protected Vector2 vel = Vector2.Zero; // starts off not moving

        public Entity(Texture2D texture) 
        { 
            this.texture = texture;
            _offsets = new Vector2[8]
                {
                    Vector2.Zero,
                    new Vector2(drawSize),
                    new Vector2(drawSize, 0),
                    new Vector2(0, drawSize),

                    new Vector2(0, drawSize / 2),
                    new Vector2(drawSize, drawSize / 2),
                    new Vector2(drawSize / 2, 0),
                    new Vector2(drawSize / 2, drawSize)
                };
        }

        /**
         * Moves the entity based on the current movement vector (already normalized)
         * 
         * @param TileMap   the current map level the entity is playing in
         * @param double    the time it took the last frame to load (seconds)
         */
        public void Move(TileMap map, double delta)
        {
            // if the movement vector is not zero then the entity must be trying to move
            if (movement_vec != Vector2.Zero)
                vel = MathUtil.MoveToward(vel, movement_vec * MAX_SPEED * (float)speed, MAX_ACCEL * delta);
            else // otherwise it is not trying to move at all so slow it down to zero
                vel = MathUtil.MoveToward(vel, Vector2.Zero, FRICTION * delta);

            // handle collision, if they are about to collide we must alter our velocity before we move forward
            if (this.WillCollide(map) && vel != Vector2.Zero)
            {
                // get the side we will collide into and based on that alter out velocity
                foreach (Side side in this.CollisionSide(map))
                {
                    /**
                     * If we are colliding with the top or bottom of a tile's surface
                     * then we will make sure they do not move in the y-axis
                     * 
                     * same for left/right on the x-axis
                     */
                    switch (side)
                    {
                        case Side.LEFT:
                        case Side.RIGHT:
                            vel.X = 0; break;
                        case Side.TOP:
                        case Side.BOTTOM:
                            vel.Y = 0; break;
                        default: break;
                    }
                }
                // now that the collision was handled we can now move our entity
                pos += vel;
            }
            else // nothing is standing in our way, ONWARD!
                pos += vel;
        }

        /**
         * Determines if the entity is about to collide with a collision tile
         * 
         * @param TileMap   the current map level
         */
        public bool WillCollide(TileMap map)
        {
            // we must loop through each offset of our entity
            for (int i=0; i<8; i++)
            {
                // we now will grab the tile position of where we are trying to move towards
                Vector2 tilePos = Vector2.Clamp(MathUtil.CoordsToTileCoords(pos + vel + MathUtil.offsets[i]), Vector2.Zero, new Vector2(map.GetWidth() - 1, map.GetHeight() - 1));
                // then we will get the tile that we are trying to move towards, from the collision layer of the map
                Tile targetTile = map.GetCollisionLayer().GetTiles()[(int)tilePos.Y, (int)tilePos.X];
                if (!targetTile.NULL && targetTile.Colliding(pos + vel, new Vector2(Globals.TILE_SIZE))) // if said tile is a collision tile then we must check if we are colliding with that tile
                    return true;
            }
            // if not a single check passed we won't collide so all is well, return false
            return false;
        }

        /**
         * Determines which sides the entity is colliding with
         * 
         * @param TileMap   the current map level of the game
         */
        public List<Side> CollisionSide(TileMap map)
        {
            List<Side> sides = new List<Side>();
            for (int i = 0; i < 8; i++)
            {
                Vector2 tilePos_p = Vector2.Clamp(MathUtil.CoordsToTileCoords(pos + MathUtil.offsets[i]), Vector2.Zero, new Vector2(map.GetWidth() - 1, map.GetWidth() - 1));
                Vector2 tilePos = Vector2.Clamp(MathUtil.CoordsToTileCoords(pos + vel + MathUtil.offsets[i]), Vector2.Zero, new Vector2(map.GetWidth() - 1, map.GetHeight() - 1));
                // Ignore straight diagnal tiles as they are meaningless here, or rather they fuck shit up
                if (!new List<Vector2>() {
                            tilePos_p + new Vector2(1,1),
                            tilePos_p + new Vector2(-1, -1),
                            tilePos_p + new Vector2(-1, 1),
                            tilePos_p + new Vector2(1, -1)
                        }.Contains(tilePos))
                {
                    Tile targetTile = map.GetCollisionLayer().GetTiles()[(int)tilePos.Y, (int)tilePos.X];

                    Side ClosestSide = targetTile.CollisionSide(pos);
                    if (!targetTile.ignores.Contains(ClosestSide))
                        sides.Add(ClosestSide);
                }
            }

            return sides;
        }


        // set the entities position
        public void SetPosition(float? x, float? y)
        {
            if (x != null) pos.X = x.Value;
            if (y != null) pos.Y = y.Value;
        }

        /**
         * Getters/Setters
         */
        public double MaxHealth() { return max_health; }
        public double MaxMana() { return max_mana; }
        public double Health() { return health; }
        public double Mana() { return mana; }
        public Vector2 Position() { return pos; }


        /**
         * Simply updates the draw object's position
         */
        public void UpdateDrawObj()
        {
            pos = Vector2.Clamp(pos, new Vector2(-1), new Vector2(16 * 16 - 16 + 1));
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
        public void Draw(SpriteBatch spriteBatch)
        {
            if (spriteBatch == null) return;
            spriteBatch.Draw(Globals.TESTBOX, drawObj, Color.White);
        }
    }
}
