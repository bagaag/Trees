using System;
using System.Collections.Generic;
using Trees.Models;
using Trees.Models.Stateful;

namespace Trees.Services
{
    public class GamePlay : IGamePlay
    {
        private const int StarterGroveCount = 3;
        private const int TreeHandCount = 5;
        private const int PlantingValue = 10;

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
            // initialize decks
            Deck<Land> lands = _gameData.Lands;
            Deck<Tree> trees = _gameData.Trees;
            Deck<Event> events = _gameData.Events;
            Guid guid = Guid.NewGuid();

            // setup table
            Table table = new Table(guid, lands, trees, events, players);
            for (var i=0; i<StarterGroveCount; i++) {
                Grove grove = new Grove(table.LandDeck.Draw());
                table.Groves.Add(grove);
            }

            // deal hands
            foreach (Player player in players) 
            {
                player.Hand = new Hand(table.TreeDeck);
            }

            // setup first turn
            table.CurrentPlayer = 0;
            UpdateReplacementStatuses(table);
            SetCurrentPlayerPotentialScores(table);
            table.CurrentEvent = table.EventDeck.Draw();
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

        /// <summary>
        /// Moves a tree from player's hand to a Land on the table and updates scores
        /// </summary>
        /// <param name="table">current game table</param>
        /// <param name="grove">grove to plant in</param>
        /// <param name="player">player's hand to take tree from</param>
        /// <param name="tree">tree to plant</param>
        public void PlantTree(Table table, Grove grove)
        {
            var player = table.GetCurrentPlayer();
            var tree = player.Hand.Peek();
            // confirm an open space
            if (grove.Land.Spaces > grove.Plantings.Count) {
                // plant the tree
                tree = player.Hand.Draw();
                Planting planting = new Planting(player, tree);
                player.Plantings.Add(planting);
                grove.Plantings.Add(planting);
                // update table
                ScorePlanting(grove, planting);
                ScorePlayer(player);
                SetCurrentPlayerPotentialScores(table);            
                UpdateReplacementStatuses(table);
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
            table.EventDeck.Return(table.CurrentEvent);
            // take the next event card
            table.CurrentEvent = table.EventDeck.Draw();
            // clear the turn log
            table.GameLog.AddRange(table.TurnLog);
            table.TurnLog.Clear();
            // record the turn event
            LogEventDraw(table.TurnLog, table.GetCurrentPlayer().Name, table.CurrentEvent.Name);
        }

        /// <summary>
        /// Replaces another player's planting with the current player's top tree card. 
        /// Moves the replaced tree to the discard pile stack and updates the replaced 
        /// tree's player's score.
        /// </summary>
        /// <param name="table">Current game table</param>
        /// <param name="targetGrove">Grove containing planting being replaced</param>
        /// <param name="targetPlanting">Planting being replaced</param>
        public void ReplaceTree(Table table, Grove targetGrove, Planting targetPlanting)
        {
            Player currentPlayer = table.GetCurrentPlayer();
            Tree testTree = currentPlayer.Hand.Peek();
            Planting replacementPlanting = new Planting(currentPlayer, testTree);
            // find and replace the planting in the grove
            for (int i = 0; i < targetGrove.Plantings.Count; i++) 
            {
                if (targetGrove.Plantings[i] == targetPlanting)
                {
                    //TODO: Throw exception if this never happens
                    targetGrove.Plantings[i] = replacementPlanting;
                    break;
                }
            }
            // update current player
            currentPlayer.Hand.Draw();
            currentPlayer.Plantings.Add(replacementPlanting);
            // remove replaced planting
            targetPlanting.Player.Plantings.Remove(targetPlanting);
            table.TreeDeck.Return(targetPlanting.Tree);
            // update scores
            ScorePlanting(targetGrove, replacementPlanting);
            ScorePlayer(targetPlanting.Player);
            ScorePlayer(currentPlayer);
            SetCurrentPlayerPotentialScores(table);
            UpdateReplacementStatuses(table);
        }

        /// <summary>
        /// Removes a planted tree, usually due to event enforcement.
        /// Moves the tree to the discard pile and updates the replaced 
        /// tree's player's score.
        /// </summary>
        /// <param name="table">Current game table</param>
        /// <param name="targetGrove">Grove containing planting being replaced</param>
        /// <param name="targetPlanting">Planting being replaced</param>
        public void RemoveTree(Table table, Grove targetGrove, Planting targetPlanting)
        {
            // remove planting
            targetGrove.Plantings.Remove(targetPlanting);
            targetPlanting.Player.Plantings.Remove(targetPlanting);
            table.TreeDeck.Return(targetPlanting.Tree);
            // update scores
            ScorePlayer(targetPlanting.Player);
        }

        // PROTECTED METHODS

        /// <summary>
        /// Updates existing plantings with status indicating whether each can 
        /// be replaced by the current player's top tree card.
        /// </summary>
        /// <param name="tree">current player's tree to test</param>
        /// <param name="table">current game table</param>
        void UpdateReplacementStatuses(Table table) 
        {
            Player currentPlayer = table.GetCurrentPlayer();
            foreach (Grove grove in table.Groves)
            {
                foreach (Planting planting in grove.Plantings) 
                {
                    // can't replace own tree or if there are open plantings available 
                    if (planting.Player == currentPlayer || grove.Plantings.Count < grove.Land.Spaces) 
                    {
                        planting.CanBeReplaced = false;
                    } 
                    else 
                    {
                        // can replace if test tree's score is more than existing tree
                        planting.CanBeReplaced = (grove.CurrentPlayerPotentialScore > planting.Score);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the score of the planting by calculating the sum of habitat value differences between
        /// the land and the tree. Lower score is better.
        /// </summary>
        /// <param name="grove"></param>
        /// <param name="planting"></param>
        void ScorePlanting(Grove grove, Planting planting)
        {
            int score = 0;
            Habitat g = grove.Land.Habitat;
            Habitat p = planting.Tree.Habitat;
            score += Math.Abs(g.Soil - p.Soil);
            score += Math.Abs(g.Sun - p.Sun);
            score += Math.Abs(g.Temperature - p.Temperature);
            score += Math.Abs(g.Water - p.Water);
            planting.Score = PlantingValue - score;
        }

        /// <summary>
        /// Sets the CurrentPlayerPotentialScore attribute on a grove to show 
        /// the score of the current player's top card if planted in that grove 
        /// </summary>
        /// <param name="grove"></param>
        /// <param name="player">presumably the current player</param>
        void SetCurrentPlayerPotentialScores(Table table) 
        {
            Player currentPlayer = table.GetCurrentPlayer();
            foreach (Grove grove in table.Groves) 
            {
                Planting temp = new Planting(currentPlayer, currentPlayer.Hand.Peek());
                ScorePlanting(grove, temp);
                grove.CurrentPlayerPotentialScore = temp.Score;
            }
        }

        /// <summary>
        /// Sums the scores of each planting and assigns to the player's score. Lower score is better.
        /// </summary>
        /// <param name="player"></param>
        void ScorePlayer(Player player) 
        {
            player.Score = 0;
            player.Plantings.ForEach(p => { player.Score += p.Score; });
        }

        void LogEventDraw(List<string> log, string playerName, string eventName) 
        {
            log.Add($"{playerName} drew the \"{eventName}\" event");
        }
    }
}