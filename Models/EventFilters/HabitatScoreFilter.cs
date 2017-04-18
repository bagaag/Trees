using System.Collections.Generic;
using Trees.Models.Stateful;

namespace Trees.Models.EventFilters
{
    /// <summary>
    /// Specifies a filter on a planting's habitat match score
    /// </summary>
    class HabitatScoreFilter : IFilter
    {
        /// <param name="op">Operator to apply in filter</param>
        /// <param name="value">Value to apply in filter</param>
        public HabitatScoreFilter(Conditions.Operators op, int value)
        {
            Operator = op; Value = value;
        }
        Trees.Models.Conditions.Operators Operator { get; set; }
        int Value { get; set; }

        public void Filter(Player currentPlayer, List<Planting> plantings)
        {
            plantings.RemoveAll(p => !Conditions.Evaluate<int>(p.Score, Operator, Value));
        }
    }

}