using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Abyss.Map;
using System.Collections.Generic;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Abyss
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Player player = new Player();

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

            // Load sprites
            Globals.TESTBOX = Content.Load<Texture2D>("testbox");

            // Load all maps
            Levels.LoadTileSets(Content);
            MapManager.TestMap = new TileMap(Levels.MP_START0);

            // Load all instances
            MapManager.Manager = new MapManager(MapManager.TestMap);
        }

        protected override void Update(GameTime gameTime)
        {
            double delta = gameTime.ElapsedGameTime.TotalSeconds * Globals.FRAME_SPEED;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            /** Player
             * all player related update processes
             */
            player.CalcInputVector(Keyboard.GetState());
            player.Move(delta);
            player.UpdateDrawObj();
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black); // background color and clears after each frame
            _spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullCounterClockwise,
                null,
                Matrix.CreateScale(Globals.GAME_SCALE)
                ) ; // start drawing through the sprite batch

            // Draw the tile map
            MapManager.Manager.GetCurrent().Draw(_spriteBatch, Vector2.Zero);

            // Draw the Player next
            player.Draw(_spriteBatch);

            _spriteBatch.End(); // end the sprite batch

            base.Draw(gameTime);
        }
    }
}