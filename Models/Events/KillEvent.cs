using System.Collections.Generic;
using Trees.Models.EventFilters;
using Trees.Models.Stateful;
using Trees.Services;

namespace Trees.Models.Events
{
    /// <summary>
    /// Supports an event killing plantings on the table via several possible filter combinations
    /// </summary>
    public class KillEvent : BaseEvent
    {
        /// <summary>
        /// List of filters that specify what plantings should be killed
        /// </summary>
        List<IFilter> _filters = new List<IFilter>();

        /// <summary>
        /// Default event constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public KillEvent(string name, string description) : base(name, description) { }

        /// <summary>
        /// Runs the event filters on all plantings and removes list of matched plantings that have to die
        /// </summary>
        /// <returns></returns>
        public override void Execute(GamePlay game, Table table)
        {
            var kill = RunFilters(table);
            // kill trees returned by filters
            foreach (Planting planting in kill)
            {
                game.RemoveTree(table, planting);
            }
        }

        public override void Stage(GamePlay game, Table table)
        {
            var kill = RunFilters(table);
            // kill trees returned by filters
            foreach (Planting planting in kill)
            {
                planting.Flag = PlantingFlag.Kill;
            }
        }

    }
}