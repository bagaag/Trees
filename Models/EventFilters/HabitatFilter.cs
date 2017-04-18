using System.Collections.Generic;
using Trees.Models.Stateful;

namespace Trees.Models.EventFilters
{
    /// <summary>
    /// Specifies a filter on a habitat value
    /// </summary>
    class HabitatFilter : IFilter
    {
        /// <param name="field">Habitat field to filter on</param>
        /// <param name="condition">Operator to apply in filter</param>
        /// <param name="value">Value to apply in filter</param>
        public HabitatFilter(HabitatField field, Conditions.Operators op, int value)
        {
            Field = field; Operator = op; Value = value;
        }
        HabitatField Field { get; set; }
        Trees.Models.Conditions.Operators Operator { get; set; }
        int Value { get; set; }
        public void Filter(Player currentPlayer, List<Planting> plantings)
        {
            plantings.RemoveAll(p => !Conditions.Evaluate<int>(p.Tree.Habitat.ValueOf(Field), Operator, Value));
        }
    }

}