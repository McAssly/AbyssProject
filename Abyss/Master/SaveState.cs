using Abyss.Entities;
using Abyss.Entities.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Master
{
    internal struct GameData
    {
        public PlayerData player;
        public int map_index;
        public bool in_tutorial;

        public GameData(bool in_tutorial, int map_index, int player_x, int player_y, int current_hp, int max_hp, int current_mana, int max_mana, Inventory inventory)
        {
            player = new PlayerData(new Vector2(player_x, player_y), current_hp, max_hp, current_mana, max_mana);
            player.inventory = inventory;
            this.map_index = map_index;
            this.in_tutorial = in_tutorial;
        }
    }

    internal class SaveState
    {
        public static GameData tutorial = new GameData(
            true,
            0, 7, 12,
            100, 100, 100, 100,
            new Inventory(new Grimoire[2] { new Grimoire(), new WaterGrimoire() })
            );
        public static GameData start = new GameData(
            false,
            1, 12, 12,
            100, 100, 100, 100,
            new Inventory(new Grimoire[2] { new Grimoire(), new WaterGrimoire() })
            );
    }
}
