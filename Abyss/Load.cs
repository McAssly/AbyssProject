using Abyss.Draw;
using Abyss.Master;
using Abyss.UI;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss
{
    internal class Load
    {
        public static void LoadContent(ContentManager Content, GameMaster game_state)
        {
            // Load the game font
            Globals.Font = Content.Load<SpriteFont>("font");

            // Load sprites
            Globals.TestBox = Content.Load<Texture2D>("testbox");

            Texture2D burst_texture = Content.Load<Texture2D>("particles/burst");

            Texture2D base_spell_texture = Content.Load<Texture2D>("spells/spell");
            Texture2D fire_spell_texture = Content.Load<Texture2D>("spells/fire");
            Texture2D water_spell_texture = Content.Load<Texture2D>("spells/water");
            //Texture2D boil_spell_texture = Content.Load<Texture2D>("spells/boil");
            //Texture2D steam_spell_texture = Content.Load<Texture2D>("spells/steam");

            Globals.BaseSpell = new AnimatedSprite(base_spell_texture, 20, 20, 71);
            Globals.FireSpell = new AnimatedSprite(fire_spell_texture, 19, 19, 71);
            Globals.WaterSpell = new AnimatedSprite(water_spell_texture, 20, 20, 63, 2);

            Globals.BurstEffect = new AnimatedSprite(burst_texture, 25, 25, 19);

            // load the maps
            game_state.LoadLevels(Content);

            // load all entities
            game_state.LoadPlayer(Globals.TestBox);
            Data.Load("save.xml", game_state);

            
            // load the hud
            game_state.Setup(UiControllers.HUD);
        }
    }
}
