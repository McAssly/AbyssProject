using Abyss.Globals;
using Abyss.Sprites;
using Abyss.UI;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Abyss.Master
{
    internal class Load
    {
        public static void LoadContent(ContentManager Content, GameState game_state, UiState ui_state)
        {
            // Load the game font
            _Sprites.Font = Content.Load<SpriteFont>("font");

            // Load sprites
            _Sprites.TestBox = new SpriteSheet(Content.Load<Texture2D>("testbox"), 16, 16, 16, 16, 0);

            Texture2D burst_texture = Content.Load<Texture2D>("particles/burst");
            Texture2D explosion_hit_texture = Content.Load<Texture2D>("particles/explosion_hit");
            Texture2D wind_hit_texture = Content.Load<Texture2D>("particles/wind_hit");

            Texture2D base_spell_texture = Content.Load<Texture2D>("spells/spell");
            Texture2D fire_spell_texture = Content.Load<Texture2D>("spells/fire");
            Texture2D fire_burst_spell_texture = Content.Load<Texture2D>("spells/fire_burst");
            Texture2D water_spell_texture = Content.Load<Texture2D>("spells/water");
            Texture2D wind_spell_texture = Content.Load<Texture2D>("spells/wind_slash");
            Texture2D wind_dash_spell_texture = Content.Load<Texture2D>("spells/wind_dash");
            //Texture2D boil_spell_texture = Content.Load<Texture2D>("spells/boil");
            //Texture2D steam_spell_texture = Content.Load<Texture2D>("spells/steam");

            _Sprites.BaseSpell = new AnimatedSprite(base_spell_texture, 30, 30, 40, 4);
            _Sprites.FireSpell = new AnimatedSprite(fire_spell_texture, 50, 50, 60, 5);
            _Sprites.FireBurstSpell = new AnimatedSprite(fire_burst_spell_texture, 50, 50, 60, 5);
            _Sprites.WaterSpell = new AnimatedSprite(water_spell_texture, 25, 25, 62, 9);
            _Sprites.WindSpell = new AnimatedSprite(wind_spell_texture, 50, 50, 20, 10);
            _Sprites.WindDashSpell = new AnimatedSprite(wind_dash_spell_texture, 80, 80, 81, 40);

            _Sprites.BurstEffect = new AnimatedSprite(burst_texture, 25, 25, 20, 8);
            _Sprites.ExplosionEffect = new AnimatedSprite(explosion_hit_texture, 50, 50, 20, 8);
            _Sprites.WindHitEffect = new AnimatedSprite(wind_hit_texture, 50, 50, 42, 8);


            // entity textures
            _Sprites.Player = new SpriteSheet(Content.Load<Texture2D>("entity/player"), 16, 16, 14, 16, 4, -1);

            game_state.InitializePlayer(_Sprites.Player);

            // load the maps
            game_state.LoadLevels(Content);
            _Dungeons.TestDungeon = Content.Load<Texture2D>("tilesets/dungeon0");

            // load save file  < ---- place holder
            Data.Load("save", game_state);

            ui_state.Setup(UiControllers.HUD);
        }
    }
}
