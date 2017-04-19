using System.Collections.Generic;
using Trees.Core.Models.Stateful;

namespace Trees.Core.Models.EventFilters
{
    /// <summary>
    /// Specifies a filter on a damage value
    /// </summary>
    class DamageFilter : IFilter
    {
        /// <param name="field">Damage field to filter on</param>
        /// <param name="op">Operator to apply in filter</param>
        /// <param name="value">Value to apply in filter</param>
        public DamageFilter(DamageField field, Conditions.Operators op, int value)
        {
            Field = field; Operator = op; Value = value;
        }
        DamageField Field { get; set; }
        Trees.Core.Models.Conditions.Operators Operator { get; set; }
        int Value { get; set; }
        public void Filter(Player currentPlayer, List<Planting> plantings)
        {
            plantings.RemoveAll(p => !Conditions.Evaluate<int>(p.Tree.Damage.ValueOf(Field), Operator, Value));
        }
    }
}