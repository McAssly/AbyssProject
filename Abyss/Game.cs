using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Abyss.Map;
using System.Collections.Generic;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using Abyss.Entities;
using Abyss.UI;
using Abyss.Master;
using System.Text;
using System.Diagnostics;
using Abyss.Draw;
using System;

namespace Abyss
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private DrawBatch sprite_batch;

        private GameMaster game_state;

        public static StringBuilder _TextInput;

        public static MouseState _MouseState;
        public static KeyboardState _prevKeyboardState;

        public static GameWindow GameWindow;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Frame rates
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 72.0);
        }

        protected override void Initialize()
        {
            // Set screen size
            _graphics.PreferredBackBufferWidth = Globals.WindowW;
            _graphics.PreferredBackBufferHeight = Globals.WindowH;
            _graphics.ApplyChanges(); // apply changes to the screen

            Window.AllowUserResizing = false;
            Window.AllowAltF4 = true;
            // window title
            Window.Title = "One of the Title's of All Time";

            GameWindow = Window;

            game_state = new GameMaster();
            _TextInput = new StringBuilder();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            sprite_batch = new DrawBatch(GraphicsDevice);

            // Load the game font
            Globals.Font = Content.Load<SpriteFont>("font");

            // Load sprites
            Globals.TestBox = Content.Load<Texture2D>("testbox");

            Texture2D base_spell_texture = Content.Load<Texture2D>("spells/spell");
            Texture2D fire_spell_texture = Content.Load<Texture2D>("spells/fire");
            Texture2D water_spell_texture = Content.Load<Texture2D>("spells/water");
            //Texture2D boil_spell_texture = Content.Load<Texture2D>("spells/boil");
            //Texture2D steam_spell_texture = Content.Load<Texture2D>("spells/steam");

            Globals.BaseSpell = new AnimatedSprite(base_spell_texture, 20, 20, 71);
            Globals.FireSpell = new AnimatedSprite(fire_spell_texture, 19, 19, 71);
            Globals.WaterSpell = new AnimatedSprite(water_spell_texture, 20, 20, 71);

            // Load the Primary Game / UI
            GameMaster.TestLevel = Levels.Eastwoods;
            GameMaster.LoadLevels(Content, 0);

            game_state.Setup(GameMaster.TestLevel, UiControllers.HUD);

            // load all entities
            game_state.LoadPlayer(Globals.TestBox);
            Data.Load("save.xml", game_state);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState KB = Keyboard.GetState();
            _MouseState = Mouse.GetState();
            double delta = gameTime.ElapsedGameTime.TotalSeconds * Globals.FRAME_SPEED;

            /** Global UI processes
             */
            game_state.Close();

            // open the debug menu
            if (Keyboard.GetState().IsKeyDown(Controls.DebugMenu) && !(game_state.CurrentUi() is UI.Console))
            {
                // Hook the text input function to the game window
                Window.TextInput += GameMaster.RegisterInput;
                game_state.Open(UiControllers._Debug); // open the debug menu
            }

            // update the current ui menu
            game_state.UpdateUi(KB, Mouse.GetState());



            // CONSOLE PROCESS              CONSOLE
            if (game_state.CurrentUi() is UI.Console)
                if (GameMaster.HandleInput(KB))
                {
                    game_state.CloseCurrent();
                    UiControllers._Debug.ProcessCommand(game_state);
                    _TextInput = new StringBuilder(); // reset the text
                }



            /** ALL GAME RELATED CODE       GAME + HUD
             */
            if (game_state.IsHud())
            {
                // Update the game state
                game_state.Update(delta, KB, _MouseState);
            }

            _prevKeyboardState = KB;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            game_state.fps = 1 / gameTime.ElapsedGameTime.TotalSeconds;

            GraphicsDevice.Clear(Globals.Black); // background color and clears after each frame
            sprite_batch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullCounterClockwise,
                null,
                Matrix.Multiply(Matrix.CreateTranslation(Globals.DrawPosition), Matrix.CreateScale((float)Globals.GameScale))
                ) ; // start drawing through the sprite batch

            // Draw the level and its entities
            game_state.Draw(sprite_batch);

            sprite_batch.End(); // end the sprite batch


            // Draw UI

            sprite_batch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullCounterClockwise,
                null,
                Matrix.CreateScale((float)Globals.GameScale));

            
            
            // Draw the UI
            game_state.DrawUi(sprite_batch);

            sprite_batch.End();

            base.Draw(gameTime);
        }
    }
}