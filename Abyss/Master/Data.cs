using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Abyss.Master
{
    internal class Data
    {
        /// <summary>
        /// Saves the current game data into the given save file
        /// </summary>
        /// <param name="save_file"></param>
        /// <param name="GM"></param>
        /// <param name="player"></param>
        public static void Save(string save_file, GameData data)
        {
            // Load the XML doc
            XmlDocument save = new XmlDocument();
            save.Load(save_file + ".xml");

            XmlNode pos_node = save.DocumentElement.SelectSingleNode("/player/pos");
            XmlNode hp_node = save.DocumentElement.SelectSingleNode("/player/hp");
            XmlNode mana_node = save.DocumentElement.SelectSingleNode("/player/mana");

            // save the position, hp and mana
            if (pos_node != null && hp_node != null && mana_node != null)
            {
                // save the position data
                pos_node.InnerText = WriteData(new int[] { data.level_index, data.map_index, (int)data.player.position.X / 16, (int)data.player.position.Y / 16 }, ',');
                // save the health data
                hp_node.InnerText = WriteData(new int[] { (int)data.player.current_hp, (int)data.player.max_hp }, '/');
                // save the mana data
                mana_node.InnerText = WriteData(new int[] { (int)data.player.current_mana, (int)data.player.max_mana }, '/');
                // overite the data
                save.Save(save_file);
            }
        }


        /// <summary>
        /// save the game given only save data
        /// </summary>
        /// <param name="save_data"></param>
        public static void Save(GameData save_data)
        {
            Save(save_data.name, save_data);
        }

        /// <summary>
        /// Loads the given save data into the game
        /// </summary>
        /// <param name="save_name"></param>
        /// <param name="game_state"></param>
        /// <param name="player"></param>
        public static void Load(string save_name, GameState game_state)
        {
            // load the XML document
            XmlDocument save = new XmlDocument();
            save.Load(save_name + ".xml");

            // determine if the player is in the tutorial or not
            bool in_tutorial = bool.Parse(save.DocumentElement.SelectSingleNode("/player/tutorial").InnerText);

            // Get the player variables ------------------------------
            string pos_data = save.DocumentElement.SelectSingleNode("/player/pos").InnerText;
            string hp_data = save.DocumentElement.SelectSingleNode("/player/hp").InnerText;
            string mana_data = save.DocumentElement.SelectSingleNode("/player/mana").InnerText;

            // INVENTORY DATA ------------------------------
            string grim_data = save.DocumentElement.SelectSingleNode("/player/inventory/grim").InnerText;
            string card_data = save.DocumentElement.SelectSingleNode("/player/inventory/cards").InnerText;
            string regen_data = save.DocumentElement.SelectSingleNode("/player/inventory/regen").InnerText;
            string instant_data = save.DocumentElement.SelectSingleNode("/player/inventory/instant").InnerText;

            // parse the player variables
            int[] parsed_pos = ParseData(pos_data, ',');
            int[] parsed_hp = ParseData(hp_data, '/');
            int[] parsed_mana = ParseData(mana_data, '/');

            // convert them into usable variables ------------------------------
            PlayerData player_data = new PlayerData(
                new Vector2(parsed_pos[2], parsed_pos[3]),
                parsed_hp[0], parsed_hp[1],
                parsed_mana[0], parsed_mana[1]
                );

            // load the inventory data
            player_data.LoadInventory(
                grim_data.Split(','),
                ParseData(card_data, ','),
                ParseData(instant_data, ','),
                ParseData(regen_data, ',')
                );


            // load the save data
            game_state.LoadSave(new GameData(in_tutorial, parsed_pos[0], parsed_pos[1], player_data, save_name));
        }

        /// <summary>
        /// Parses the given raw save data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        private static int[] ParseData(string data, char delimeter)
        {
            List<int> data_list = new List<int>();
            foreach (string str in data.Split(delimeter))
            {
                if (int.TryParse(str, out int num))
                { data_list.Add(num); }
            }

            return data_list.ToArray();
        }


        /// <summary>
        /// Converts the given data into readable and simplistic save data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        private static string WriteData(int[] data, char delimeter)
        {
            // start a string builder
            StringBuilder sb = new StringBuilder();

            // for each number in the data array add it to the string builder and place a delimeter between each
            foreach (int num in data)
                sb.Append(num.ToString() + delimeter);

            sb.Length -= 1; // remove the extra delimeter at the very end we dont want it

            // return the resulting string
            return sb.ToString();
        }
    }
}
