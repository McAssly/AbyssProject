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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            sprite_batch = new DrawState(GraphicsDevice);

            // load all necessary game content into the game state and ui state
            Load.LoadContent(Content, game_state, ui_state);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState KB = Keyboard.GetState();
            _MouseState = Mouse.GetState();
            double delta = gameTime.ElapsedGameTime.TotalSeconds * Variables.FRAME_SPEED;

            // update the current ui state, close it if it needs to close
            ui_state.Close();

            // open the debug menu
            if (Keyboard.GetState().IsKeyDown(Controls.DebugMenu) && !(ui_state.CurrentUi() is UI.Console))
            {
                // Hook the text input function to the game window
                Window.TextInput += InputUtility.RegisterInput;
                ui_state.Open(UiControllers._Debug); // open the debug menu, but close the previous ui
            }

            // update the current ui menu
            ui_state.Update(KB, Mouse.GetState());



            // CONSOLE PROCESS              CONSOLE     < --- (debug menu)
            if (ui_state.CurrentUi() is UI.Console)
                if (InputUtility.HandleInput(KB))
                {
                    ui_state.CloseCurrent(); // force close the current ui
                    UiControllers._Debug.ProcessCommand(ui_state, game_state, _graphics); // process the given command
                    _TextInput = new StringBuilder(); // reset the text
                }



            /** ALL GAME RELATED CODE       GAME + HUD
             */
            if (ui_state.IsHud())
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

            GraphicsDevice.Clear(Color.Black); // background color and clears after each frame
            sprite_batch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullCounterClockwise,
                null,
                Matrix.Multiply(Matrix.CreateTranslation(Variables.DrawPosition), Matrix.CreateScale(Variables.GameScale))
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
                Matrix.CreateScale(Variables.GameScale));

            
            
            // Draw the UI
            ui_state.Draw(sprite_batch, game_state);

            sprite_batch.End();

            base.Draw(gameTime);
        }
    }
}