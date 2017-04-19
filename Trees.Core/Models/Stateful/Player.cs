using System.Collections.Generic;

namespace Trees.Core.Models.Stateful
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
        public int PointsDifferential { get; set; } = 0;

        /// <summary>
        /// Sums the scores of each planting and assigns to the player's score.
        /// </summary>
        public void CalculateScore() 
        {
            Score = PointsDifferential;
            Plantings.ForEach(p => { Score += p.Score; });
        }
    }
}