using System.Collections.Generic;
using Trees.Core.Models.EventFilters;
using Trees.Core.Models.Stateful;
using Trees.Core.Services;

namespace Trees.Core.Models.Events
{
    /// <summary>
    /// Supports an event killing plantings on the table via several possible filter combinations
    /// </summary>
    public abstract class BaseEvent
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
        public BaseEvent(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Implements any actions to be taken for this event
        /// </summary>
        /// <param name="table"></param>
        public abstract void Execute(GamePlay game, Table table);
        
        /// <summary>
        /// Sets the stage for showing what the event will do, setting flags, etc. Happens on Draw
        /// </summary>
        /// <param name="table"></param>
        public abstract void Stage(GamePlay game, Table table);

    }
}