using System;
using System.Collections.Generic;
using Trees.Services;
using Trees.Models;
using Trees.Models.Stateful;
using Trees.Models.Events;

namespace Trees
{
    /// <summary>
    /// Console app that plays the game with itself to test various scenarios
    /// </summary>
    public class Console
    {
        public static void Main(string[] args)
        {
            RandomGameData rgd = new RandomGameData();
            GamePlay game = new GamePlay(rgd);
            List<Player> players = new List<Player>();
            players.Add(new Player("Matt"));
            players.Add(new Player("Ava"));
            Guid guid = game.NewGame(players);
            Table table = game.GetTable(guid);
            while (true) {
                game.ProcessEvent(table);
                while (table.PlantingsRemaining > 0)
                {
                    
                }
            }
        }
    }
}