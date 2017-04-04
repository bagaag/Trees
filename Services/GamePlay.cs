using System;
using System.Collections.Generic;
using Trees.Models;
using Trees.Models.Stateful;

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

            // setup first turn
            table.CurrentPlayer = 0;
            table.CurrentEvent = table.EventDeck.Pop();
            LogEventDraw(table.TurnLog, table.GetCurrentPlayer().Name, table.CurrentEvent.Name);

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

        public void PlantTree(Table table, Grove grove, Player player, Tree tree)
        {
            // confirm an open space
            if (grove.Land.Spaces > grove.Plantings.Count) {
                // plant the tree
                Planting planting = new Planting(player, tree);
                player.Hand.Remove(tree);
                player.Plantings.Add(planting);
                grove.Plantings.Add(planting);
                // record the turn event
                table.TurnLog.Add($"{player.Name} planted a {tree.Name} in the {grove.Land.Name}");
            }
        }

        public void CompleteTurn(Table table)
        {
            // next player
            table.CurrentPlayer++;
            if (table.CurrentPlayer == table.Players.Count) table.CurrentPlayer = 0;
            // dispose of current event card
            table.PastEvents.Push(table.CurrentEvent);
            // reshuffle events if needed
            if (table.EventDeck.Count == 0) 
            {
                var events = new List<Event>(table.PastEvents.ToArray());
                events.Shuffle();
                table.EventDeck = new Stack<Event>(events);
                table.PastEvents.Clear();
            } 
            // take the next event card
            table.CurrentEvent = table.EventDeck.Pop();
            // clear the turn log
            table.GameLog.AddRange(table.TurnLog);
            table.TurnLog.Clear();
            // record the turn event
            LogEventDraw(table.TurnLog, table.GetCurrentPlayer().Name, table.CurrentEvent.Name);
        }

        void LogEventDraw(List<string> log, string playerName, string eventName) 
        {
            log.Add($"{playerName} drew the \"{eventName}\" event");
        }
    }
}