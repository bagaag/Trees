using System;
using System.Collections.Generic;
using Trees.Core.Models.Stateful;

namespace Trees.Core.Models.EventFilters
{
    /// <summary>
    /// Kills tree based on a match or mismatch of the tree's genus
    /// </summary>
    class GenusFilter : IFilter
    {
        public GenusFilter(string[] genus, bool matches = true)
        {
            Genus = genus;
            Matches = matches;
        }
        string[] Genus { get; set; }
        bool Matches { get; set; }
        public void Filter(Player currentPlayer, List<Planting> plantings)
        {
            if (Matches)
            {
                plantings.RemoveAll(fp => Array.Exists(Genus, name => name != fp.Tree.Genus));
            }
            else
            {
                plantings.RemoveAll(fp => Array.Exists(Genus, name => name == fp.Tree.Genus));
            }
        }
    }
}

