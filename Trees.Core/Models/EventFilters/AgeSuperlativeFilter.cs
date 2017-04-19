using System;
using System.Collections.Generic;
using Trees.Core.Models.Stateful;

namespace Trees.Core.Models.EventFilters
{
    /// <summary>
    /// Specifies a filter on a planting's age 
    /// </summary>
    class AgeSuperlativeFilter : IFilter
    {
        /// <param name="value">Positive number for the oldest N, negative for the youngest N</param>
        public AgeSuperlativeFilter(int value)
        {
            Value = value;
        }
        int Value { get; set; }
        public void Filter(Player currentPlayer, List<Planting> plantings)
        {
            var sortList = (new List<Planting>(plantings));
            // if Value > 0, sort descending (biggest first), else sort ascending (smallest first)
            sortList.Sort((x, y) => Value > 0 ? y.Age.CompareTo(x.Age) : x.Age.CompareTo(y.Age));
            // remove plantings that don't fall within the first <Value> items in the list
            sortList.RemoveRange(0, Math.Min(Math.Abs(Value), sortList.Count));
            // remove the remainders from the filter list
            sortList.ForEach(p => plantings.Remove(p));
        }
    }

}