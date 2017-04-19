using System.Collections.Generic;
using Trees.Models.EventFilters;
using Trees.Models.Stateful;

namespace Trees.Models.Events
{
    /// <summary>
    /// Supports an event killing plantings on the table via several possible filter combinations
    /// </summary>
    public abstract class PlantingFilterBaseEvent : BaseEvent
    {

        /// <summary>
        /// Default event constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public PlantingFilterBaseEvent(string name, string description) : base(name, description){ }

        /// <summary>
        /// List of filters that specify what plantings should be killed
        /// </summary>
        public List<IFilter> Filters { get; set; } = new List<IFilter>();

        /// <summary>
        /// Applies all the filters to the table and returns the results for the child event to act on
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<Planting> RunFilters(Table table)
        {
            // flatten all plantings into list for filtering
            List<Planting> plantings = new List<Planting>();
            foreach (Grove grove in table.Groves)
            {
                plantings.AddRange(grove.Plantings);
            }
            // run filters
            Player currentPlayer = table.GetCurrentPlayer();
            foreach (IFilter filter in Filters)
            {
                filter.Filter(currentPlayer, plantings);
            }
            return plantings;
        }

    }
}