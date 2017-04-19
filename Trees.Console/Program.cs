using System;
using System.Collections.Generic;
using Trees.Core.Services;
using Trees.Core.Models.Stateful;
using System.Text;

namespace Trees.Console
{
    /// <summary>
    /// Console app that plays the game with itself to test various scenarios
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            int turns = 0;
            bool gameOver = false;
            System.Console.WriteLine("Starting...");
            bool fillEmptyGrovesFirst = true;
            int lastLogLine = 0;
            RandomGameData rgd = new RandomGameData();
            GamePlay game = new GamePlay(rgd);
            game.MaxGroveCount = 6;
            List<Player> players = new List<Player>();
            players.Add(new Player("Matt"));
            players.Add(new Player("Ava"));
            Guid guid = game.NewGame(players);
            Table table = game.GetTable(guid);
            while (!gameOver)
            {
                game.ProcessEvent(table);
                while (table.PlantingsRemaining > 0)
                {
                    // strategy: fill empty groves first
                    if (fillEmptyGrovesFirst)
                    {
                        Grove grove = FindEmptyGrove(table);
                        if (grove != null)
                        {
                            game.PlantTree(table, grove);
                        }
                        else
                        {
                            Planting replace = FindReplacementPlanting(table);
                            if (replace != null)
                            {
                                game.ReplaceTree(table, replace);
                            }
                            // end game if no open groves and nothing to replace
                            else
                            {
                                System.Console.WriteLine("Game over! " + PlayerScores(table));
                                game.EndGame(guid);
                                gameOver = true;
                                break;
                            }
                        }
                    }
                }
                if (!gameOver)
                {
                    game.CompleteTurn(table);
                    turns++;
                    System.Console.WriteLine(GameStatus(turns, table));
                    for (int i = lastLogLine; i < table.GameLog.Count; i++)
                    {
                        System.Console.WriteLine(table.GameLog[i]);
                    }
                    lastLogLine = table.GameLog.Count - 1;
                }
            }
        }

        /// <summary>
        /// Returns the open grove with the highest score or null if no groves are open
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        static Grove FindEmptyGrove(Table table)
        {
            Planting target = null;
            foreach (Grove grove in table.Groves)
            {
                if (grove.HasSpace)
                {
                    Planting temp = new Planting(table.GetCurrentPlayer(), grove, table.GetCurrentPlayer().Hand.Peek());
                    if (target == null)
                    {
                        target = temp;
                    }
                    else if (temp.Score > target.Score)
                    {
                        target = temp;
                    }
                }
            }
            if (target == null) return null;
            else return target.Grove;
        }

        /// <summary>
        /// Returns the replacement that will cost the opponent the most points
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        static Planting FindReplacementPlanting(Table table)
        {
            int opponentScore = 0;
            Planting target = null;
            foreach (Grove grove in table.Groves)
            {
                Planting temp = new Planting(table.GetCurrentPlayer(), grove, table.GetCurrentPlayer().Hand.Peek());
                foreach (Planting planting in grove.Plantings)
                {
                    if (planting.Player != table.GetCurrentPlayer())
                    {
                        if (temp.Score > planting.Score)
                        {
                            if (target == null)
                            {
                                opponentScore = planting.Score;
                                target = planting;
                            }
                            else if (planting.Score > opponentScore)
                            {
                                opponentScore = planting.Score;
                                target = planting;
                            }
                        }
                    }
                }
            }
            return target;
        }

        static string PlayerScores(Table table)
        {
            string ret = "";
            foreach (Player player in table.Players)
            {
                ret += player.Name + "=" + player.Score + " ";
            }
            return ret;
        }

        static string GameStatus(int turns, Table table)
        {
            StringBuilder sb = new StringBuilder("T" + turns + ": ");
            for (int g = 0; g < table.Groves.Count; g++)
            {
                Grove grove = table.Groves[g];
                sb.Append("G" + g + "[ ");
                for (int p = 0; p < grove.Plantings.Count; p++)
                {
                    Planting planting = grove.Plantings[p];
                    sb.Append("P" + p + "(" + planting.Score + ") ");
                }
                sb.Append("]");
            }
            return sb.ToString();
        }
    }
}