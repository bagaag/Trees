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
        public List<Tree> Hand { get; set; }  = new List<Tree>();
        public List<Planting> Plantings { get; set; } = new List<Planting>();
    }
}