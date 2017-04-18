using System;
using System.Collections.Generic;
using Trees.Models.Stateful;

namespace Trees.Models.EventFilters
{
    /// <summary>
    /// Specifies a filter on the higest or lowest Habitat values
    /// </summary>
    class HabitatSuperlativeFilter : IFilter
    {
        /// <param name="field">Habitat field to filter on</param>
        /// <param name="value">Positive number for the highest N, negative for the lowest N</param>
        public HabitatSuperlativeFilter(HabitatField field, int value)
        {
            Field = field; Value = value;
        }
        HabitatField Field { get; set; }
        int Value { get; set; }
        public void Filter(Player currentPlayer, List<Planting> plantings)
        {
            var sortList = (new List<Planting>(plantings));
            // if Value > 0, sort descending (biggest first), else sort ascending (smallest first)
            sortList.Sort((x, y) => Value > 0 ? 
                y.Tree.Habitat.ValueOf(Field).CompareTo(x.Tree.Habitat.ValueOf(Field)) : 
                x.Tree.Habitat.ValueOf(Field).CompareTo(y.Tree.Habitat.ValueOf(Field)));
            // remove plantings that don't fall within the first <Value> items in the list
            sortList.RemoveRange(0, Math.Min(Math.Abs(Value), sortList.Count));
            // remove the remainders from the filter list
            sortList.ForEach(p => plantings.Remove(p));
        }
    }
}