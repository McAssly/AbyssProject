
namespace Abyss.Items
{
    internal abstract class Item
    {

        public static Card WhichCard(int id)
        {
            switch (id)
            {
                case 0:
                    return Card.Time;
                case 1:
                    return Card.Water;
                case 2:
                    return Card.Wind;
                case 3:
                    return Card.Earth;
                case 4:
                    return Card.Fire;
                case 5:
                    return Card.Lightning;
                default: return null;
            }
        }
    }
}
