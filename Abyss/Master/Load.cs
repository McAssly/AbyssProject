﻿using Abyss.Globals;
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

            Texture2D base_spell_texture = Content.Load<Texture2D>("spells/spell");
            Texture2D fire_spell_texture = Content.Load<Texture2D>("spells/fire");
            Texture2D water_spell_texture = Content.Load<Texture2D>("spells/water");
            //Texture2D boil_spell_texture = Content.Load<Texture2D>("spells/boil");
            //Texture2D steam_spell_texture = Content.Load<Texture2D>("spells/steam");

            _Sprites.BaseSpell = new AnimatedSprite(base_spell_texture, 20, 20, 71);
            _Sprites.FireSpell = new AnimatedSprite(fire_spell_texture, 19, 19, 71, 5);
            _Sprites.WaterSpell = new AnimatedSprite(water_spell_texture, 25, 25, 61, 9);

            _Sprites.BurstEffect = new AnimatedSprite(burst_texture, 25, 25, 19, 8);


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
