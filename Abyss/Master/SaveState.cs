using Abyss.Entities;
using Abyss.Entities.Magic;
using Abyss.Entities.Magic.Grimoires;
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
        public int level_index;
        public int map_index;
        public bool in_tutorial;

        public GameData(bool in_tutorial, int level_index, int map_index, int player_x, int player_y, int current_hp, int max_hp, int current_mana, int max_mana, Inventory inventory)
        {
            player = new PlayerData(new Vector2(player_x, player_y), current_hp, max_hp, current_mana, max_mana);
            player.inventory = inventory;
            this.level_index = level_index;
            this.map_index = map_index;
            this.in_tutorial = in_tutorial;
        }
    }

    internal class SaveState
    {
        public static GameData tutorial = new GameData(
            true, 0,
            0, 8, 8,
            100, 100, 100, 100,
            new Inventory(new Grimoire[2] { new Grimoire(), new Water() })
            );
        public static GameData start = new GameData(
            false, 0,
            1, 8, 12,
            100, 100, 100, 100,
            new Inventory(new Grimoire[2] { new Grimoire(), new Water() })
            );
    }
}
