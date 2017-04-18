using System.Collections.Generic;
using Trees.Models.Stateful;

namespace Trees.Models.EventFilters
{
    /// <summary>
    /// Kills tree based on a match or mismatch of the tree's genus
    /// </summary>
    class PlayerFilter : IFilter
    {
        public PlayerFilter(bool current)
        {
            Current = current;
        }
        bool Current { get; set; }
        public void Filter(Player currentPlayer, List<Planting> plantings)
        {
            if (Current)
            {
                plantings.RemoveAll(p => p.Player != currentPlayer);
            }
            else
            {
                plantings.RemoveAll(p => p.Player == currentPlayer);
            }
        }
    }
}

