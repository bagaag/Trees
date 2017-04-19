using System;

namespace Trees.Core.Models.Stateful
{
    public enum PlantingFlag { None, Kill }
    public class Planting
    {
        public Planting(Player player, Grove grove, Tree tree)
        {
            Player = player;
            Grove = grove;
            Tree = tree;
        }
        public Tree Tree { get; set; }
        public Player Player { get; set; }
        public int Score { get; set; }
        public bool CanBeReplaced { get; set; } = false;
        public DateTime TimePlanted { get; set; } = System.DateTime.Now;
        public int Age { get { return (DateTime.Now - TimePlanted).Seconds; } }
        public Grove Grove { get; set; }
        public PlantingFlag Flag { get; set; } = PlantingFlag.None;
    }
}