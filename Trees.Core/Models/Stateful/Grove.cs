using System.Collections.Generic;

namespace Trees.Core.Models.Stateful
{
    public class Grove 
    {
        public Grove(Land land) 
        {
            Land = land;
        }
        public Land Land { get; set; }
        public List<Planting> Plantings { get; set; } = new List<Planting>();
        public int CurrentPlayerPotentialScore { get; set; } = 0;

        public bool HasSpace { get {
            return Land.Spaces > Plantings.Count;
        }}
    }
}