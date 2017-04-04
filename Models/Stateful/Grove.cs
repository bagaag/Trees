using System.Collections.Generic;

namespace Trees.Models.Stateful
{
    public class Grove 
    {
        public Grove(Land land) 
        {
            Land = land;
        }
        public Land Land { get; set; }
        public List<Planting> Plantings { get; set; } = new List<Planting>();
    }
}