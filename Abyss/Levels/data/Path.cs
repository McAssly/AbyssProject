using Abyss.Utility;
using Microsoft.Xna.Framework;
using System;

namespace Abyss.Levels.data
{
    internal class Path
    {
        private static Random rand = new Random();

        internal MapNode previous;

        internal Vector2 head;
        internal Vector2 destination;
        internal Vector2 velocity;

        internal Vector range;
        internal int path_id;

        private bool exists;


        public Path(Vector origin, Vector range, int path_index, Dungeon dungeon)
        {
            this.head = origin.To2();
            this.previous = dungeon.map[origin.y, origin.x];
            this.range = range;
            this.SetDestinationWithinRange(dungeon);
            this.path_id = path_index;
            this.velocity = Vector2.Zero;

            exists = true;
        }

        public Path() { this.exists = false; }

        public bool IsWithinRange() { return head.X < range.y && head.X >= range.x; }
        public bool Exists() { return exists; }

        public int GetY() { return (int)head.Y; }

        public void SetDestinationWithinRange(Dungeon dungeon)
        {
            this.destination = new Vector2(rand.Next(range.x, range.y), dungeon.GetMedian());
        }


        /// <summary>
        /// moves the path
        /// </summary>
        /// <param name="dungeon"></param>
        /// <returns></returns>
        internal void MoveBeforeMedian(Dungeon dungeon)
        {
            if (!exists) return;
            this.PlaceNode(dungeon);

            // create a new branch if able to.

            // 1. check for divergence
            if (dungeon.Diverged((int)head.Y))
            {
                int new_path_id = dungeon.GetFirstNullPath();
                if (new_path_id != -1)
                {
                    int mid_x = (range.y - range.x) / 2 + range.x + 1;
                    Vector left_range = new Vector(range.x, mid_x);
                    Vector right_range = new Vector(mid_x, range.y);

                    Vector new_path_range;

                    // 2. determine new range
                    if (rand.NextDouble() < 0.5)  // place new path's zone to the left
                    {
                        this.range = right_range;
                        new_path_range = left_range;
                    }
                    else                          // place the new path's zone to the right
                    {
                        this.range = left_range;
                        new_path_range = right_range;
                    }


                    // 3. make new path
                    dungeon.paths[new_path_id] = new Path(Vector.Convert(this.head), new_path_range, new_path_id, dungeon);
                    this.SetDestinationWithinRange(dungeon);
                }
            }

            // if the path is not within its zone move into the zone before doing anything else
            while (!IsWithinRange())
            {
                // if left of the zone move to the right into it
                if (head.X < range.x)
                {
                    velocity.X = 1;
                }
                else if (head.X >= range.y)
                {
                    velocity.X = -1;
                }

                this.Move(dungeon, true);
            }

            while (this.head != this.destination) Move(dungeon);
        }


        internal void Move(Dungeon dungeon, bool override_velocity = false, bool connect_paths = false)
        {
            this.PlaceNode(dungeon);

            if (!override_velocity)
            {
                // calculate the base velocity towards the destination

                // 1. get the distance between the destination position and the head position
                velocity = this.destination - this.head;
                // 2. normalize it to proper speed
                velocity.Normalize();
                // 3. randomize its angle slightly
                if (head.Y != destination.Y && dungeon.Diverged((int)head.Y, true))
                    velocity = Math0.OffsetDirection(velocity, Math.PI * dungeon.divergence_rate);
                // 4. make velocity 1 directional
                velocity = Math0.FixDirection(velocity);
                velocity = Math0.Clamp(velocity, new Vector2(-1, 1));

                velocity = new Vector((int)Math.Ceiling(velocity.X), (int)Math.Ceiling(velocity.Y)).To2();
            }


            // move the path header position

            // 1. store previous position
            Vector2 previous_position = head;
            head += velocity;

            // clamp position within boundaries
            if (head.X < 0) head.X = 0;
            if (head.X >= dungeon.width) head.X = dungeon.width - 1;
            if (head.Y < 0) head.Y = 0;
            if (head.Y >= dungeon.height) head.Y = dungeon.height - 1;

            // place the next node

            // 1. check the next node: *(the head)
            MapNode? node_at_head = dungeon.map[(int)head.Y, (int)head.X];
            if (node_at_head != null && previous != null && connect_paths)
            {
                node_at_head.Connect(previous);
                this.previous = dungeon.map[(int)previous_position.Y, (int)previous_position.X];
            }
            else
            {
                this.PlaceNode(dungeon);
                this.previous = dungeon.map[(int)previous_position.Y, (int)previous_position.X];
            }
        }


        internal void PlaceNode(Dungeon dungeon)
        {
            if (dungeon.map[(int)head.Y, (int)head.X] == null)
                dungeon.CreateNode(new Vector((int)head.X, (int)head.Y), previous, path_id);
        }
    }
}
