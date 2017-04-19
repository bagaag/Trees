using System.Collections.Generic;
using Trees.Core.Models.Stateful;

namespace Trees.Core.Models.EventFilters
{
    /// <summary>
    /// /// Interface for all filter types
    /// </summary>
    public interface IFilter
    {
        void Filter(Player currentPlayer, List<Planting> plantings);
    }

}
