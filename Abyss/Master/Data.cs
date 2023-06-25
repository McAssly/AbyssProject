using Abyss.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Abyss.Master
{
    internal class Data
    {
        public static void Load(string savedata, GameMaster GM, Player player)
        {
            // load the XML document
            XmlDocument save = new XmlDocument();
            save.Load(savedata);

            // Get the player variables
            string positionData = save.DocumentElement.SelectSingleNode("/player/pos").InnerText;
            string healthData = save.DocumentElement.SelectSingleNode("/player/hp").InnerText;
            string manaData = save.DocumentElement.SelectSingleNode("/player/mana").InnerText;

            // parse the player variables
            int[] parsedPosition = ParseData(positionData, ',');
            int[] parsedHealth = ParseData(healthData, '/');
            int[] parsedMana = ParseData(manaData, '/');

            // convert them into usable variables
            Vector2 position = new Vector2(parsedPosition[1] * 16, parsedPosition[2] * 16);
            int mapIndex = parsedPosition[0];
            int maxHealth = parsedHealth[1];
            int currentHealth = parsedHealth[0];
            int maxMana = parsedMana[1];
            int currentMana = parsedMana[0];

            // have the player load this data
            player.LoadSave(position, currentHealth, maxHealth, currentMana, maxMana);

            // load the map index
            GM.LoadSave(mapIndex);
        }

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
    }
}
