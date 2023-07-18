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

        private GameMaster GM;

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

            GM = new GameMaster();
            _TextInput = new StringBuilder();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the game font
            Globals.FONT = Content.Load<SpriteFont>("font");

            // Load sprites
            Globals.TESTBOX = Content.Load<Texture2D>("testbox");
            Globals.BaseSpell = Content.Load<Texture2D>("spells/spell");

            // Load the Primary Game / UI
            GameMaster.TestLevel = Levels.EASTWOODS;
            GameMaster.LoadLevels(Content, 0);

            GM.Setup(GameMaster.TestLevel, UiControllers.HUD);

            // load all entities
            GM.LoadPlayer(Globals.TESTBOX);
            Data.Load("save.xml", GM);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState KB = Keyboard.GetState();
            _MouseState = Mouse.GetState();
            double delta = gameTime.ElapsedGameTime.TotalSeconds * Globals.FRAME_SPEED;

            /** Global UI processes
             */
            GM.Close();

            // open the debug menu
            if (Keyboard.GetState().IsKeyDown(Controls.DebugMenu) && !(GM.CurrentUi() is Console))
            {
                // Hook the text input function to the game window
                Window.TextInput += GameMaster.RegisterInput;
                GM.Open(UiControllers._Debug); // open the debug menu
            }

            // update the current ui menu
            GM.UpdateUi(KB, Mouse.GetState());



            // CONSOLE PROCESS              CONSOLE
            if (GM.CurrentUi() is Console)
                if (GameMaster.HandleInput(KB))
                {
                    GM.CloseCurrent();
                    UiControllers._Debug.ProcessCommand(GM);
                    _TextInput = new StringBuilder(); // reset the text
                }



            /** ALL GAME RELATED CODE       GAME + HUD
             */
            if (GM.IsHud())
            {
                // update the player
                GM.player.Update(delta, KB, _MouseState, GM);

                // Update the game state
                GM.Update(delta);
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
                Matrix.Multiply(Matrix.CreateTranslation(Globals.DrawPosition), Matrix.CreateScale((float)Globals.game_scale))
                ) ; // start drawing through the sprite batch

            // Draw the level and its entities
            GM.Draw(_spriteBatch);

            _spriteBatch.End(); // end the sprite batch


            // Draw UI

            _spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullCounterClockwise,
                null,
                Matrix.CreateScale((float)Globals.game_scale));

            
            
            // Draw the UI
            GM.DrawUi(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}