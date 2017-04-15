using System.Collections.Generic;

namespace Trees.Models.Stateful
{
    public class Player
    {
        public Player(string name) 
        {
            Name = name;
        }
        public string Name { get; set; }
        public Hand Hand { get; set; }
        public List<Planting> Plantings { get; set; } = new List<Planting>();
        public int Score { get; set; } = 0;
    }
}