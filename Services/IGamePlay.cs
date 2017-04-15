using System;
using System.Collections.Generic;
using Trees.Models.Stateful;

namespace Trees.Services
{
    public interface IGamePlay
    {
        Guid NewGame(List<Player> players);

        Table GetTable(Guid guid);

        void EndGame(Guid guid);

        void PlantTree(Table table, Grove grove);

        void ReplaceTree(Table table, Grove targetGrove, Planting targetPlanting);

        void RemoveTree(Table table, Grove targetGrove, Planting targetPlanting);
        void CompleteTurn(Table table);
    }
}