﻿using Microsoft.Xna.Framework;
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

namespace Abyss
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameMaster GM;
        public static StringBuilder _TextInput;
        private Player player;

        public static KeyboardState _prevKeyboardState;

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

            // Load the Primary Game / UI
            GameMaster.TestLevel = Levels.LVL_START;
            GameMaster.LoadLevels(Content, 0, 0);

            GM.Setup(GameMaster.TestLevel, UiControllers.HUD);

            // load all entities
            player = new Player(Globals.TESTBOX);

            GameMaster.test_text = new Text("This here is a message and this is \nwhere its located on the game's \nhud/UI setup", Globals.MessageLocation, Globals.MessageScale);


            // Hook the text input
            Window.TextInput += GameMaster.RegisterInput;
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState KB = Keyboard.GetState();
            double delta = gameTime.ElapsedGameTime.TotalSeconds * Globals.FRAME_SPEED;

            /**
             * process globals
             */
            if (GM.CurrentUi() is Console)
                if (GameMaster.HandleInput(KB))
                    GM.CloseCurrent();


            /** All UI related shit
             */
            GM.Close();

            // open the debug menu
            if (Keyboard.GetState().IsKeyDown(Controls.DebugMenu))
            {
                _TextInput = new StringBuilder(); // reset the text input
                GM.Open(UiControllers.Debug); // open the debug menu
            }

            // update the current ui menu
            GM.UpdateUi(KB, Mouse.GetState());
            


            /** ALL GAME RELATED CODE
             */
            if (GM.IsHud())
            {
                /** Player
                 * all player related update processes
                 */
                player.CalcInputVector(KB);
                player.Move(GM.GetCurrentTileMap(), delta);
                player.UpdateDrawObj();
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

            // Draw the level
            GM.DrawLevel(_spriteBatch);

            // Draw the Player next
            player.Draw(_spriteBatch);

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