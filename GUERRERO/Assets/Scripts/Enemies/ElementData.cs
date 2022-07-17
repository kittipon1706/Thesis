using UnityEngine;

namespace Ingame.UI
{
    public class ElementData 
    {
        public string Type;
        public Color Color;
        public int Cost;

        public ElementData(string type, Color color, int cost)
        {
            Type = type;
            Color = color;
            Cost = cost;
        }
    }
}
