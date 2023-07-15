using Abyss.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Abyss.Master
{
    internal struct PlayerData
    {
        public Inventory inventory;
        public Vector2 position;
        public int currentHealth;
        public int maxHealth;
        public int currentMana;
        public int maxMana;

        public PlayerData(Vector2 position, int chp, int mhp, int cmn, int mmn)
        {
            this.inventory = new Inventory();
            this.position = position;
            this.currentHealth = chp;
            this.maxHealth = mhp;
            this.currentMana = cmn;
            this.maxMana = mmn;
        }

        public void LoadInventory(string[] grimoires, int[] cards, int[] regen, int[] instant)
        {
            inventory.grimoires = Inventory.ParseGrimoires(grimoires);
            inventory.cards = Inventory.ParseCards(cards);
            inventory.instantPots = instant;
            inventory.regenPots = regen;
        }
    }

    internal class Data
    {
        /// <summary>
        /// Saves the current game data into the given save file
        /// </summary>
        /// <param name="savefile"></param>
        /// <param name="GM"></param>
        /// <param name="player"></param>
        public static void Save(string savefile, GameMaster GM)
        {
            // Load the XML doc
            XmlDocument save = new XmlDocument();
            save.Load(savefile);

            XmlNode posNode = save.DocumentElement.SelectSingleNode("/player/pos");
            XmlNode hpNode = save.DocumentElement.SelectSingleNode("/player/hp");
            XmlNode manaNode = save.DocumentElement.SelectSingleNode("/player/mana");

            // save the position, hp and mana
            if (posNode != null && hpNode != null && manaNode != null)
            {
                // save the position data
                posNode.InnerText = WriteData(new int[] { GM.GetMapIndex(), (int)GM.player.Position().X / 16, (int)GM.player.Position().Y / 16 }, ',');
                // save the health data
                hpNode.InnerText = WriteData(new int[] { (int)GM.player.Health(), (int)GM.player.MaxHealth() }, '/');
                // save the mana data
                manaNode.InnerText = WriteData(new int[] { (int)GM.player.Mana(), (int)GM.player.MaxMana() }, '/');
                // overite the data
                save.Save(savefile);
            }
        }

        /// <summary>
        /// Loads the given save data into the game
        /// </summary>
        /// <param name="savefile"></param>
        /// <param name="GM"></param>
        /// <param name="player"></param>
        public static void Load(string savefile, GameMaster GM)
        {
            // load the XML document
            XmlDocument save = new XmlDocument();
            save.Load(savefile);

            // Get the player variables ------------------------------
            string positionData = save.DocumentElement.SelectSingleNode("/player/pos").InnerText;
            string healthData = save.DocumentElement.SelectSingleNode("/player/hp").InnerText;
            string manaData = save.DocumentElement.SelectSingleNode("/player/mana").InnerText;

            // INVENTORY DATA ------------------------------
            string grimoireData = save.DocumentElement.SelectSingleNode("/player/inventory/grim").InnerText;
            string cardData = save.DocumentElement.SelectSingleNode("/player/inventory/cards").InnerText;
            string regenData = save.DocumentElement.SelectSingleNode("/player/inventory/regen").InnerText;
            string instantData = save.DocumentElement.SelectSingleNode("/player/inventory/instant").InnerText;

            // parse the player variables
            int[] parsedPosition = ParseData(positionData, ',');
            int[] parsedHealth = ParseData(healthData, '/');
            int[] parsedMana = ParseData(manaData, '/');

            // convert them into usable variables ------------------------------
            PlayerData data = new PlayerData(
                new Vector2(parsedPosition[1] * 16, parsedPosition[2] * 16),
                parsedHealth[0], parsedHealth[1],
                parsedMana[0], parsedMana[1]
                );

            // load the inventory data
            data.LoadInventory(
                grimoireData.Split(','),
                ParseData(cardData, ','),
                ParseData(instantData, ','),
                ParseData(regenData, ',')
                );

            // have the player load this data
            GM.player.LoadSave(data);

            // load the map index
            GM.LoadSave(parsedPosition[0]);
        }

        /// <summary>
        /// Parses the given raw save data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        private static int[] ParseData(string data, char delimeter)
        {
            List<int> dataList = new List<int>();
            foreach (string str in data.Split(delimeter))
            {
                if (int.TryParse(str, out int num))
                { dataList.Add(num); }
            }

            return dataList.ToArray();
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
