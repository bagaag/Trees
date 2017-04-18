using Trees.Models.Stateful;
using Trees.Services;

namespace Trees.Models.Events
{
    /// <summary>
    /// Supports an event adding extra plantings as part of the current turn. 
    /// Planting filters are used to indicate whether and how many extra plantings are granted.
    /// </summary>
    public class ExtraPlantingEvent : PlantingFilterBaseEvent
    {
        /// <summary>
        /// Default event constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public ExtraPlantingEvent(string name, string description) : base(name, description) { }

        /// <summary>
        /// Runs the event filters on all plantings and removes list of matched plantings that have to die
        /// </summary>
        /// <returns></returns>
        public override void Execute(GamePlay game, Table table)
        {
            // nothing happens here
        }

        public override void Stage(GamePlay game, Table table)
        {
            int extraPlantings = 0;
            if (Filters.Count > 0)
            {
                var plantings = RunFilters(table);
                if (!GrantPerPlanting && plantings.Count > 0)
                {
                    extraPlantings = ExtraPlantings;
                }
                // kill trees returned by filters
                foreach (Planting planting in plantings)
                {
                    planting.Flag = PlantingFlag.Kill;
                    if (GrantPerPlanting)
                    {
                        extraPlantings += ExtraPlantings;
                    }
                }
            }
            // default to extra planting if no filters are specified
            else 
            {
                extraPlantings += ExtraPlantings;
            }
            table.PlantingsRemaining += extraPlantings;
            table.TurnLog.Add(table.GetCurrentPlayer().Name + " granted " + extraPlantings + " extra plantings.");
        }

        /// <summary>
        /// How many extra plantings are granted
        /// </summary>
        /// <returns></returns>
        public int ExtraPlantings { get; set; } = 1;

        /// <summary>
        /// If true, player receives extra planting(s) for each planting matched by filters. 
        /// If false, player receives extra planting(s) if 1 or more plantings are matched. 
        /// </summary>
        /// <returns></returns>
        public bool GrantPerPlanting { get; set; } = false;

        public ExtraPlantingEvent ForEveryPlantingMatched(bool value = true)
        {
            GrantPerPlanting = value;
            return this;
        }

        public ExtraPlantingEvent WithExtraPlantings(int plantings)
        {
            ExtraPlantings = plantings;
            return this;
        }

        /// <summary>
        /// This filter is not supported on this event type; throws NotSupportedException
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public override BaseEvent WherePlayer(bool current) { throw new System.NotSupportedException(); }
    }
}