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

namespace Abyss
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

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
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the game font
            Globals.Font = Content.Load<SpriteFont>("font");

            // Load sprites
            Globals.TestBox = Content.Load<Texture2D>("testbox");
            Globals.BaseSpell = Content.Load<Texture2D>("spells/spell");

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
            if (Keyboard.GetState().IsKeyDown(Controls.DebugMenu) && !(game_state.CurrentUi() is Console))
            {
                // Hook the text input function to the game window
                Window.TextInput += GameMaster.RegisterInput;
                game_state.Open(UiControllers._Debug); // open the debug menu
            }

            // update the current ui menu
            game_state.UpdateUi(KB, Mouse.GetState());



            // CONSOLE PROCESS              CONSOLE
            if (game_state.CurrentUi() is Console)
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
                // update the player
                game_state.player.Update(delta, KB, _MouseState, game_state);

                // Update the game state
                game_state.Update(delta);
            }

            _prevKeyboardState = KB;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Globals.Black); // background color and clears after each frame
            _spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullCounterClockwise,
                null,
                Matrix.Multiply(Matrix.CreateTranslation(Globals.DrawPosition), Matrix.CreateScale((float)Globals.GameScale))
                ) ; // start drawing through the sprite batch

            // Draw the level and its entities
            game_state.Draw(_spriteBatch);

            _spriteBatch.End(); // end the sprite batch


            // Draw UI

            _spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullCounterClockwise,
                null,
                Matrix.CreateScale((float)Globals.GameScale));

            
            
            // Draw the UI
            game_state.DrawUi(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}