using Abyss.Draw;
using Abyss.Entities;
using Abyss.Globals;
using Abyss.Levels;
using Abyss.Magic;
using Abyss.Sprites;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Effect = Abyss.Magic.Effect;
using Vector = Abyss.Utility.Vector;

namespace Abyss.Master
{
    internal class GameState
    {
        // declare whether the game state should be drawn and active
        private protected bool visible = true;

        // declare the levels and current level of the game state
        private protected Level[] levels;
        private protected int last_map_index = -1;
        private protected Level dungeon;
        private protected int level_index;

        // declare the player for the game state
        protected internal Player player;

        // declare debug variable control
        protected internal double fps;

        // declare the screen fx variables
        protected internal List<Effect> particle_fx;

        // declare the save data for the game
        protected internal GameData autosave;


        /// <summary>
        /// initializes the game state
        /// </summary>
        public GameState()
        {
            levels = new Level[1]
            {
                _Levels.Eastwoods
            };
            particle_fx = new List<Effect>();
        }



        // UPDATE SECTION
        /// <summary>
        /// updates the game state
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="kb"></param>
        /// <param name="ms"></param>
        public void Update(double delta, KeyboardState kb, MouseState ms)
        {
            // update the player
            player.Update(delta, kb, ms, this);

            // if the player leaves a side of the current map update the level's map index
            if (player.ExittingSide().HasValue)
                this.UpdateLevel(player.ExittingSide().Value);


            // update particles
            player.inventory.grimoires[0].Update(delta, this);
            player.inventory.grimoires[1].Update(delta, this);

            foreach (Effect effect in particle_fx) effect.Update(delta);
            particle_fx.RemoveAll(fx => fx.IsDead());


            // update entities
            int number_killed = levels[level_index].GetEnemies().FindAll(e => e.GetHealth() <= 0).Count;
            player.RegenerateMana(number_killed * 5);
            if (player.GetMana() > player.GetMaxMana()) player.SetMana(player.GetMaxMana());
            levels[level_index].GetEnemies().RemoveAll(x => x.GetHealth() <= 0);
            foreach (Enemy enemy in levels[level_index].GetEnemies())
            {
                // update the enemies
                enemy.Update(delta, this);


                // enemies take damage
                for (int i = 0; i < 2; i++)
                {
                    Particle damager = player.inventory.grimoires[i].Hits(enemy);
                    if (damager != null)
                    {
                        Effect.BurstEffect(Math0.ClosestPosition(enemy.GetPosition(), enemy.GetSize(), damager.position), this);
                        enemy.ReduceHealth(damager.damage);
                    }
                }

                if (enemy.attack_cooldown > 0) enemy.attack_cooldown -= delta;
            }

            // kill the player
            if (player.GetHealth() <= 0)
            {
                if (this.autosave.in_tutorial)
                    this.LoadSave(Abyss.Master.Save.tutorial);
                else
                    this.LoadSave(Abyss.Master.Save.start);
                this.levels[level_index].ResetEnemies();
            }
        }

        /// <summary>
        /// based on the given side that the player exits from
        /// </summary>
        /// <param name="side"></param>
        public void UpdateLevel(Side exitting_from)
        {
            // get the next map in the level's linked map list
            int next_map_index = levels[level_index].GetCurrent().GetNext()[(int)exitting_from];

            // get the new position the player will take
            NullableVector new_possible_position = player.GetNextPosition(exitting_from);

            // get the player's position based on the new one
            Vector2 new_position = player.GetPosition(new_possible_position);

            // then turn it into the proper tile coordinates
            Vector tile_coords = Math0.CoordsToTileCoords(new_position);

            // change maps
            if (next_map_index > -1)
            {
                // if the new position on the next map is not a collision tile then allow the player to move onto the next map
                if (levels[level_index].GetMaps()[next_map_index].GetCollisionLayer().GetTiles()[tile_coords.y, tile_coords.x].NULL)
                {
                    // change to the new map
                    var prev_map = levels[level_index].GetCurrent();
                    levels[level_index].SetCurrent(next_map_index);
                    if (levels[level_index].GetCurrent() != prev_map)
                    {
                        player.SetPosition(new_position);
                        player.inventory.grimoires[0].Clear();
                        player.inventory.grimoires[1].Clear();
                        particle_fx.Clear();
                    }
                }
            }
            // change levels, not map
            else if (next_map_index < -1)
            {
                int next_level_index = Level.ConvertToLevelIndex(next_map_index);
                if (last_map_index > -1)
                {
                    int prev_level = level_index;
                    level_index = next_level_index;
                    levels[level_index].SetCurrent(levels[level_index].GetMapConnection(prev_level)); // set to the start of the map
                }
                else
                {
                    level_index = next_level_index;
                    levels[level_index].SetCurrent(last_map_index);
                }
                player.SetPosition(new_position);
                player.inventory.grimoires[0].Clear();
                player.inventory.grimoires[1].Clear();
                particle_fx.Clear();
            }
        }



        // SETUP / LOAD SECTION of the game state
        /// <summary>
        /// loads all the level data into the actual level system
        /// </summary>
        /// <param name="Content"></param>
        public void LoadLevels(ContentManager Content)
        {
            foreach (Level level in levels)
            {
                level.Load(Content);
            }
        }

        /// <summary>
        /// Initializes the player
        /// </summary>
        /// <param name="sprite"></param>
        public void InitializePlayer(SpriteSheet sprite)
        {
            player = new Player(sprite);
        }

        /// <summary>
        /// Takes in save data and loads it into the player and game state
        /// </summary>
        public void LoadSave(GameData save_data)
        {
            this.level_index = save_data.level_index;
            levels[level_index].SetCurrent(save_data.map_index);
            this.player.Load(save_data.player);
            this.autosave = save_data;
        }



        // DATA CONTROL
        /// <summary>
        /// saves the game's autosave
        /// </summary>
        public void Save()
        {
            Data.Save(this.autosave);
        }

        /// <summary>
        /// saves an "autosave" or in this case temporary save data
        /// </summary>
        public void AutoSave()
        {
            this.autosave = new GameData(
                this.autosave.in_tutorial, this.level_index,
                this.levels[level_index].GetIndex(),
                new PlayerData(this.player), this.autosave.name
                );
        }



        // GETTERS / SETTERS
        /// <summary>
        /// determines if the game state is currently active/visible
        /// </summary>
        /// <returns></returns>
        public bool IsVisible() { return this.visible; }

        /// <summary>
        /// Enables/Disables the game state's visibility
        /// </summary>
        /// <param name="visible"></param>
        public void SetVisible(bool visible) {  this.visible = visible; }

        /// <summary>
        /// Returns the current level the game state is on
        /// </summary>
        /// <returns></returns>
        public Level CurrentLevel() { return levels[this.level_index]; }

        /// <summary>
        /// Gets the current draw map for the level
        /// </summary>
        /// <returns></returns>
        public Map CurrentMap() { return levels[this.level_index].GetCurrent(); }

        /// <summary>
        /// Gets the current map index of the level
        /// </summary>
        /// <returns></returns>
        public int GetMapIndex() { return levels[this.level_index].GetIndex(); }

        /// <summary>
        /// returns the current map's collision layer
        /// </summary>
        /// <returns></returns>
        public Layer GetCollisionLayer() { return levels[level_index].GetCurrent().GetCollisionLayer(); }


        // DRAW SECTION
        /// <summary>
        /// Draw's the current game state
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(DrawState sprite_batch)
        {
            // draw the current level map
            sprite_batch.Draw(levels[level_index].GetCurrent());

            // draw the entities
            if (levels[level_index].GetEnemies().Count > 0)
                sprite_batch.Draw(levels[level_index].GetEnemies().ToArray());

            // draw the player
            sprite_batch.Draw(player);

            // draw the particles of the game
            sprite_batch.Draw(player.inventory.grimoires[0]);
            sprite_batch.Draw(player.inventory.grimoires[1]);

            foreach (Effect fx in particle_fx) sprite_batch.Draw(fx);
        }

    }
}
