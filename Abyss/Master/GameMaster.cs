using Abyss.Draw;
using Abyss.Entities;
using Abyss.Entities.Magic;
using Abyss.Map;
using Abyss.Sprite;
using Abyss.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Effect = Abyss.Entities.Effect;

namespace Abyss.Master
{
    internal class GameMaster
    {
        // declare testing placeholders ---------------------------------------------------------------------------------------------
        public static Dialogue TestDialogue = new Dialogue
            (
                new Text("Hello World.", Globals.DialoguePosition, 0.3f, Globals.DialogueSize), 
                new Text[] 
                {
                    new Text("Hi", Globals.DialoguePosition + Globals.OptionOffset[0], 0.3f, Globals.DialogueSize, true),
                    new Text("Why?", Globals.DialoguePosition + Globals.OptionOffset[1], 0.3f, Globals.DialogueSize, true),
                    new Text("Goodbye.", Globals.DialoguePosition + Globals.OptionOffset[2], 0.3f, Globals.DialogueSize, true)
                }
            );
        // --------------------------------------------------------------------------------------------------------------------------




        // Declare the game managers (UI/Level)
        private Level[] levels;
        private int level_index;
        private Ui current_ui;

        // Declare game entities
        protected internal Player player;

        // declare debug vars
        protected internal double fps;

        // draw data
        protected internal List<Effect> effects;

        // save data
        protected internal GameData save;
        private bool in_tutorial;
        private byte player_gender;

        // declare basic constructor
        public GameMaster() 
        {
            levels = new Level[1] 
            {
                Levels.Eastwoods
            };
            effects = new List<Effect>();
        }






        // SETUP / LOAD SECTION
        /// <summary>
        /// Sets up both the current level and current ui of the game within the game master
        /// </summary>
        /// <param name="ui"></param>
        public void Setup(Ui ui)
        {
            this.current_ui = ui;
        }


        /// <summary>
        /// Loads every level in the game <--- currently a place holder
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="initial_map"></param>
        public void LoadLevels(ContentManager Content, int initial_map = 0) 
        {
            foreach (Level level in levels)
            {
                level.LoadLevel(Content, initial_map);
            }
        }


        /// <summary>
        /// Loads the player entity for the game
        /// </summary>
        /// <param name="texture"></param>
        public void LoadPlayer(SpriteSheet sprite) { player = new Player(sprite); }

        /// <summary>
        /// Loads the current save file
        /// </summary>
        /// <param name="map_index"></param>
        public void LoadSave(int level_index, int map_index, PlayerData player, bool in_tutorial, string gender)
        {
            switch (gender)
            {
                case "boy":
                    this.LoadPlayer(Sprites.PlayerM);  this.player_gender = 0; break;
                case "girl":
                    this.LoadPlayer(Sprites.PlayerF);  this.player_gender = 1; break;
                default:
                    this.LoadPlayer(Sprites.PlayerF);  this.player_gender = 1; break;
            }
            this.level_index = level_index;
            levels[this.level_index].SetCurrent(map_index); 
            this.player.LoadSave(player);
            this.in_tutorial = in_tutorial;
        }

        public void LoadSaveState(GameData data)
        {
            levels[level_index].SetCurrent(data.map_index);
            this.player.LoadSave(data.player);
            this.in_tutorial = data.in_tutorial;
            this.player_gender = data.player_gender;
            if (player_gender != 0 && player_gender != 1)
            {
                player_gender = Data.LoadGender("save.xml");
            }
        }







        // COMMANDS
        /// <summary>
        /// Opens the given UI, sets the current UI to the given
        /// </summary>
        /// <param name="ui"></param>
        public void Open(Ui ui){ this.current_ui = ui; }

        /// <summary>
        /// opens the given dialogue in the dialogue ui window
        /// </summary>
        /// <param name="dialogue"></param>
        public void OpenDialogue(Dialogue dialogue)
        {
            // unhook the input controller
            Game.GameWindow.TextInput -= RegisterInput;

            // open the given dialogue
            UiControllers.Dialogue.SetDialogue(dialogue);
            this.current_ui = UiControllers.Dialogue;
        }

        /// <summary>
        /// If the current UI is subject to close then close it and reset back to the HUD
        /// . Otherwise simply do nothing
        /// </summary>
        public void Close()
        {
            if (this.current_ui.IsClosed())
            {
                if (this.current_ui is UI.Console)
                {
                    Game.GameWindow.TextInput -= RegisterInput;
                }
                this.current_ui.UnClose();
                this.current_ui = UiControllers.HUD;
            }
        }

        /// <summary>
        /// Saves the current save state of the game, currently a placeholder
        /// </summary>
        public void Save()
        {
            this.save = new GameData(
                this.in_tutorial, this.player_gender, this.level_index,
                this.GetMapIndex(),
                (int)this.player.GetPosition().X,
                (int)this.player.GetPosition().Y,
                (int)this.player.GetHealth(), (int)this.player.GetMaxHealth(),
                (int)this.player.GetMana(), (int)this.player.GetMaxMana(),
                this.player.Inventory
                );
            Data.Save("save.xml", this.save);
        }

        /// <summary>
        /// Force closes the current ui of the game state
        /// </summary>
        public void CloseCurrent() { this.current_ui.Close(); }







        // GETTERS / SETTERS
        /// <summary>
        /// Whether or not the game state is running the game or not (Ui is showing the HUD not any other menu)
        /// </summary>
        /// <returns>Whether the current ui is set to the HUD or not</returns>
        public bool IsHud() { return current_ui is Hud; }

        /// <summary>
        /// Grabs the current level the game state is playing in
        /// </summary>
        /// <returns>the game state's current level</returns>
        public Level CurrentLevel() { return levels[level_index]; }

        /// <summary>
        /// Grabs the current ui the game state is running
        /// </summary>
        /// <returns>the game state's current ui state</returns>
        public Ui CurrentUi() { return current_ui; }

        /// <summary>
        /// Grabs the current tile map that the game state is rendering
        /// </summary>
        /// <returns>the game state's current tile map</returns>
        public TileMap GetCurrentTileMap() { return levels[level_index].GetCurrent(); }

        /// <summary>
        /// Grabs which map index the current level is running in
        /// </summary>
        /// <returns>the game state's current map index</returns>
        public int GetMapIndex() { return Array.IndexOf(levels[level_index].GetMaps(), levels[level_index].GetCurrent()); }







        // UPDATE SECTION
        /// <summary>
        /// Updates the current ui of the game state
        /// </summary>
        /// <param name="KB"></param>
        /// <param name="MS"></param>
        public void UpdateUi(KeyboardState KB, MouseState MS)
        {
            this.current_ui.Update(KB, MS);
        }

        /// <summary>
        /// Update's the level whenever the player moves to the next level
        /// </summary>
        public void Update(double delta, KeyboardState KB, MouseState MS)
        {
            // update the player
            player.Update(delta, KB, MS, this);


            // update the game level
            if (player.ExittingSide().HasValue)
            {
                // get the next map the current map links to (at the exitting side)
                int next_map = levels[level_index].GetNextMap(player.ExittingSide().Value);
                int next_map_index = levels[level_index].GetCurrent().GetNext()[next_map];
                // get the new position the player will take
                NullableVector new_position = player.GetNextPosition(player.ExittingSide().Value);

                // then get the player's position based on the new position
                Vector2 new_pos = player.GetPosition(new_position);

                // turn it into the proper tile coord
                Vector tile_coords = MathUtil.CoordsToTileCoords(new_pos);

                // if the new position on the next map is not a collision tile then allow the player to move into the next map
                if (levels[level_index].GetMaps()[next_map_index].GetCollisionLayer().GetTiles()[tile_coords.y, tile_coords.x].NULL)
                {
                    // change to the new map
                    var prev_map = levels[level_index].GetCurrent();
                    levels[level_index].SetCurrent(next_map_index);
                    // wrap the player's position to the new one
                    if (levels[level_index].GetCurrent() != prev_map) // if the map properly changed
                    {
                        // set the new position
                        player.SetPosition(new_pos);
                        // clear particles
                        player.Inventory.grimoires[0].Clear();
                        player.Inventory.grimoires[1].Clear();
                        effects.Clear();
                    }
                }
            }



            // update the game's particles
            // player attacks
            player.Inventory.grimoires[0].Update(delta, this);
            player.Inventory.grimoires[1].Update(delta, this);

            // Update the entities
            // enemies
            levels[level_index].GetEntities().RemoveAll(x => x.GetHealth() <= 0);
            foreach (Entity entity in levels[level_index].GetEntities())
            {
                // enemies take damage
                for (int i = 0; i < 2; i++)
                {
                    Particle damager = player.Inventory.grimoires[i].Hits(entity);
                    if (damager != null)
                    {
                        Effect.BurstEffect(damager.position, this);
                        entity.ReduceHealth(damager.damage);
                    }
                }

                // enemies deal damage
                if (entity.Hits(player) > 0 && entity.attack_cooldown <= 0)
                {
                    player.ReduceHealth(entity.Hits(player));
                    entity.attack_cooldown = entity.attack_cooldown_max;
                }

                if (entity.attack_cooldown > 0) entity.attack_cooldown -= delta;
            }


            // kill the player if they should be dead
            if (player.GetHealth() <= 0)
            {
                if (this.in_tutorial)
                    this.LoadSaveState(SaveState.tutorial);
                else
                    this.LoadSaveState(SaveState.start);
                this.levels[level_index].ResetEntities();
            }

            foreach (Effect effect in effects) effect.Update(delta);
            effects.RemoveAll(fx => fx.IsDead());
        }






        // DRAW SECTION
        /// <summary>
        /// Draw's the current game state
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(DrawBatch sprite_batch)
        {
            // draw the current level map
            sprite_batch.Draw(levels[level_index].GetCurrent());

            // draw the entities
            if (levels[level_index].GetEntities().Count > 0)
                sprite_batch.Draw(levels[level_index].GetEntities());

            // draw the player
            sprite_batch.Draw(player);

            // draw the particles of the game
            sprite_batch.Draw(player.Inventory.grimoires[0]);
            sprite_batch.Draw(player.Inventory.grimoires[1]);

            foreach (Effect fx in effects) sprite_batch.Draw(fx);
        }

        /// <summary>
        /// Draw's the current Ui of the game
        /// </summary>
        /// <param name="sprite_batch"></param>
        public void DrawUi(DrawBatch sprite_batch)
        {
            if (current_ui == null) return;
            sprite_batch.Draw(current_ui, this);
        }






        // STATIC
        /// <summary>
        /// Handle's keyboard input for the game, when using the debug console, or entering in the player name, etc.
        /// </summary>
        /// <param name="KB"></param>
        /// <returns></returns>
        public static bool HandleInput(KeyboardState KB)
        {
            Keys[] keys = KB.GetPressedKeys();

            // Handle control keys
            foreach (Keys key in keys)
            {
                if (Game._prevKeyboardState.IsKeyUp(key)) // ensure repetitions are not caused
                {
                    if (key == Keys.Enter) // close key
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// The text input hook for the game window to register keyboard text input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void RegisterInput(object sender, TextInputEventArgs e)
        {
            Keys? k = e.Key;
            char c = e.Character;
            if (!char.IsControl(c) && c != '`')
                Game._TextInput.Append(c);
            else if (k == Keys.Back && Game._TextInput.Length > 0)
                Game._TextInput.Length--;
        }
    }
}
