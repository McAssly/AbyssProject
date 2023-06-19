using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Abyss.Map;
using System.Collections.Generic;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using Abyss.Entities;
using Abyss.UI;

namespace Abyss
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Text test_text;

        private MapManager mapManager;
        private UiManager UI_Manager;
        private Player player;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the game font
            Globals.FONT = Content.Load<SpriteFont>("font");

            // Load sprites
            Globals.TESTBOX = Content.Load<Texture2D>("testbox");

            // Load all maps
            Levels.LoadTileSets(Content);
            MapManager.TestMap = new TileMap(Levels.MP_START0);

            // Load all instances
            mapManager = new MapManager(MapManager.TestMap);
            UI_Manager = new UiManager(UiManager.Debug);
            player = new Player(Globals.TESTBOX);

            test_text = new Text("This here is a message and this is \nwhere its located on the game's \nhud/UI setup", Globals.MessageLocation, Globals.MessageScale);
        }

        protected override void Update(GameTime gameTime)
        {
            double delta = gameTime.ElapsedGameTime.TotalSeconds * Globals.FRAME_SPEED;

            if (Keyboard.GetState().IsKeyDown(Controls.DebugMenu))
                test_text.Update("Now in debug menu..");

            /** Player
             * all player related update processes
             */
            player.CalcInputVector(Keyboard.GetState());
            player.Move(mapManager.GetCurrent(), delta);
            player.UpdateDrawObj();
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(48, 41, 39)); // background color and clears after each frame
            _spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullCounterClockwise,
                null,
                Matrix.Multiply(Matrix.CreateTranslation(new Vector3(0, 7, 0)), Matrix.CreateScale(Globals.game_scale))
                ) ; // start drawing through the sprite batch

            // Draw the tile map
            mapManager.GetCurrent().Draw(_spriteBatch, Vector2.Zero);

            // Draw the Player next
            player.Draw(_spriteBatch);

            // Draw UI
            test_text.Draw(_spriteBatch);

            _spriteBatch.End(); // end the sprite batch

            base.Draw(gameTime);
        }
    }
}