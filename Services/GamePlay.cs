using System;
using System.Collections.Generic;
using Trees.Models;

namespace Trees.Services
{
    public class GamePlay : IGamePlay
    {
        private readonly IGameData _gameData;
        private Dictionary<Guid,Table> _tables;
        
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
            List<Land> lands = new List<Land>(_gameData.Lands);
            lands.Shuffle();
            List<Tree> trees = new List<Tree>(_gameData.Trees);
            trees.Shuffle();
            List<Event> events = new List<Event>(_gameData.Events);
            events.Shuffle();

            Table table = new Table(lands, trees, events, players);

            Guid guid = new Guid();
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
    }
}