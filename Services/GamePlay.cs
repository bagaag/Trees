using System;
using System.Collections.Generic;
using Trees.Models;

namespace Trees.Services
{
    public class GamePlay : IGamePlay
    {
        private readonly IGameData _gameData;
        private Dictionary<Guid,Table> _tables;
        private const int StarterGroveCount = 3;
        private const int TreeHandCount = 5;
        
        public GamePlay(IGameData gameData)
        {
            _gameData = gameData;
            _tables = new Dictionary<Guid, Table>();
        }

        /// <summary>
        /// Creates a new table with provided player list, shuffles decks and returns a Guid reference for game state.
        /// </summary>
        /// <param name="players"></param>
        /// <returns></returns>
        public Guid NewGame(List<Player> players)
        {
            // initialize decks
            List<Land> lands = new List<Land>(_gameData.Lands);
            lands.Shuffle();
            List<Tree> trees = new List<Tree>(_gameData.Trees);
            trees.Shuffle();
            List<Event> events = new List<Event>(_gameData.Events);
            events.Shuffle();
            Guid guid = Guid.NewGuid();

            // setup table
            Table table = new Table(guid, lands, trees, events, players);
            for (var i=0; i<StarterGroveCount; i++) {
                Grove grove = new Grove(table.LandDeck.Pop());
                table.Groves.Add(grove);
            }

            // deal hands
            foreach (Player player in players) 
            {
                for (var i=0; i<TreeHandCount; i++) 
                {
                    player.Hand.Add(table.TreeDeck.Pop());
                }
            }

            _tables.Add(guid, table);
            return guid;
        }

        public Table GetTable(Guid guid) 
        {
            Table table;
            if (_tables.TryGetValue(guid, out table)) 
            {
                return table;
            }
            else {
                return null;
            }
        }

        public void EndGame(Guid guid)
        {
            _tables.Remove(guid);
        }

        public void PlantTree(Grove grove, Player player, Tree tree)
        {
            if (grove.Land.Spaces > grove.Plantings.Count) {
                Planting planting = new Planting(player, tree);
                player.Hand.Remove(tree);
                player.Plantings.Add(planting);
                grove.Plantings.Add(planting);
            }
        }
    }
}