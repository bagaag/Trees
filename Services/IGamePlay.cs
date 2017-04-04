using System;
using System.Collections.Generic;
using Trees.Models;
using Trees.Models.Stateful;

namespace Trees.Services
{
    public interface IGamePlay
    {
        Guid NewGame(List<Player> players);

        Table GetTable(Guid guid);

        void EndGame(Guid guid);

        void PlantTree(Table table, Grove grove, Player player, Tree tree);

        void CompleteTurn(Table table);
    }
}