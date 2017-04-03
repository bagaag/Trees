using System;
using System.Collections.Generic;
using Trees.Models;

namespace Trees.Services
{
    public interface IGamePlay
    {
        Guid NewGame(List<Player> players);

        Table GetTable(Guid guid);

        void EndGame(Guid guid);

        void PlantTree(Grove grove, Player player, Tree tree);
    }
}