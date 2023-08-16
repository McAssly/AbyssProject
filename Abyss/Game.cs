using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System.Text;
using System.Diagnostics;
using System;
using Abyss.Globals;
using Abyss.Master;
using Abyss.Draw;
using Abyss.UI;
using Abyss.Utility;

namespace Abyss
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;


        // declare the game's draw state (sprite batch clone)
        private DrawState sprite_batch;
        // declare the game's state to control the game system
        private GameState game_state;
        // declare the game's ui state
        private UiState ui_state;


        // declare the text input builder for the text input hook
        public static StringBuilder _TextInput;
        public static Keys _KeyInput;

        // declare input controllers
        public static MouseState _MouseState;
        public static KeyboardState _prevKeyboardState;


        // declare the game window
        public static GameWindow GameWindow;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Frame rate control
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / Config.MaxFrameRate);
        }

        protected override void Initialize()
        {
            // Set screen size
            _graphics.PreferredBackBufferWidth = Variables.WindowW;
            _graphics.PreferredBackBufferHeight = Variables.WindowH;
            _graphics.ApplyChanges(); // apply changes to the screen

            Window.AllowUserResizing = false;
            Window.AllowAltF4 = true;
            // window title
            Window.Title = "Oops she's probably broken";

            GameWindow = Window;

            // initialize game controllers
            game_state = new GameState();
            ui_state = new UiState();
            _TextInput = new StringBuilder();
            _KeyInput = Keys.None;

            UiControllers.Options.Initialize(game_state, ui_state);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            sprite_batch = new DrawState(GraphicsDevice);

            Window.TextInput += InputUtility.RegisterKey;

            // load all necessary game content into the game state and ui state
            Load.LoadContent(Content, game_state, ui_state);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState KB = Keyboard.GetState();
            _MouseState = Mouse.GetState();
            double delta = gameTime.ElapsedGameTime.TotalSeconds * Variables.FRAME_SPEED;

            // update the current ui menu
            ui_state.Update(Window, _graphics, KB, _MouseState, game_state);

            /** ALL GAME RELATED CODE       GAME + HUD
             */
            if (ui_state.IsHud())
            {
                // Update the game state
                game_state.Update(delta, KB, _MouseState);
            }

            Variables.ShiftingTimer += delta;
            if (Variables.ShiftingTimer >= 0.25)
            {
                if (Variables.ShiftingColor == Color.White) Variables.ShiftingColor = Color.Black;
                else Variables.ShiftingColor = Color.White;
                Variables.ShiftingTimer = 0;
            }


            Config.Update(_graphics);

            _prevKeyboardState = KB;
            _KeyInput = Keys.None;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            game_state.fps = 1 / gameTime.ElapsedGameTime.TotalSeconds;

            // if the game state is currently visible then display it
            if (game_state.IsVisible())
            {
                // begin drawing the game
                GraphicsDevice.Clear(Color.Black); // background color and clears after each frame
                sprite_batch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    DepthStencilState.Default,
                    RasterizerState.CullCounterClockwise,
                    null,
                    Matrix.Multiply(Matrix.CreateTranslation(Variables.DrawPosition), Matrix.CreateScale(Variables.GameScale))
                    ); // start drawing through the sprite batch

                // Draw the level and its entities
                game_state.Draw(sprite_batch);

                sprite_batch.End(); // end the sprite batch
            }
            else
            {
                GraphicsDevice.Clear(Color.Black);
            }


            // Draw UI (always drawn)

            sprite_batch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullCounterClockwise,
                null,
                Matrix.CreateScale(Variables.GameScale));

            
            
            // Draw the UI
            ui_state.Draw(sprite_batch, game_state);

            sprite_batch.End();

            base.Draw(gameTime);
        }
    }
}