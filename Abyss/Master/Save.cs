using Abyss.Entities;
using Abyss.Magic;
using Abyss.Magic.Grimoires;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public string name;

        public GameData(bool in_tutorial, int level_index, int map_index, int player_x, int player_y, int current_hp, int max_hp, int current_mana, int max_mana, Inventory inventory)
        {
            player = new PlayerData(new Vector2(player_x, player_y), current_hp, max_hp, current_mana, max_mana);
            player.inventory = inventory;
            this.level_index = level_index;
            this.map_index = map_index;
            this.in_tutorial = in_tutorial;
            this.name = "";
        }

        public GameData(bool in_tutorial, int level_index, int map_index, PlayerData player_data, string name)
        {
            this.in_tutorial = in_tutorial;
            this.level_index = level_index;
            this.map_index = map_index;
            this.player = player_data;
            this.name = name;
        }
    }

    internal struct PlayerData
    {
        public Inventory inventory;
        public Vector2 position;
        public int current_hp;
        public int max_hp;
        public int current_mana;
        public int max_mana;

        public PlayerData(Vector2 position, int chp, int mhp, int cmn, int mmn)
        {
            this.inventory = new Inventory();
            this.position = position;
            this.current_hp = chp;
            this.max_hp = mhp;
            this.current_mana = cmn;
            this.max_mana = mmn;
        }

        public PlayerData(Player player)
        {
            this.inventory = player.inventory;
            this.position = player.GetPosition();
            this.current_hp = (int)player.GetHealth();
            this.current_mana = (int)player.GetMana();
            this.max_hp = (int)player.GetMaxHealth();
            this.max_mana = (int)player.GetMaxMana();
        }

        public void LoadInventory(string[] grimoires, int[] cards, int[] regen, int[] instant)
        {
            inventory.grimoires = Inventory.ParseGrimoires(grimoires);
            inventory.cards = Inventory.ParseCards(cards);
            inventory.instant_pots = instant;
            inventory.regen_pots = regen;
        }
    }

    internal class Save
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
