using System;
using System.Collections.Generic;
using Trees.Core.Models.Stateful;

namespace Trees.Core.Models.EventFilters
{
    /// <summary>
    /// /// Kills tree based on a match or mismatch of one or more land types
    /// </summary>
    class LandFilter : IFilter
    {
        public LandFilter(string[] landName, bool matches = true)
        {
            LandName = landName;
            Matches = matches;
        }
        string[] LandName { get; set; }
        bool Matches { get; set; }
        public void Filter(Player currentPlayer, List<Planting> plantings)
        {
            if (Matches)
            {
                plantings.RemoveAll(fp => Array.Exists(LandName, name => name != fp.Grove.Land.Name));
            }
            else
            {
                plantings.RemoveAll(fp => Array.Exists(LandName, name => name == fp.Grove.Land.Name));
            }
        }
    }
}

