using Microsoft.AspNetCore.Mvc;
using Trees.Services;
using Trees.Models;
using Trees.Models.Stateful;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

namespace Trees.Controllers
{
    public class GameController : Controller
    {
        private readonly IGamePlay _game;
        const string SessionKeyTable = "table";

        public GameController(IGamePlay game)
        {
            _game = game;
        }

        /// <summary>
        /// Returns the current table from the session guid, or null
        /// </summary>
        /// <returns></returns>
        Table GetTable() 
        {
            Table table = null;
            string sguid = HttpContext.Session.GetString(SessionKeyTable);
            if (sguid != null) 
            {
                Guid tableGuid = new Guid(sguid);
                table = _game.GetTable(tableGuid);
            }
            return table;
        }

        public IActionResult Index()
        {
            return View(GetTable());
        }

        public IActionResult NewGame()
        {
            string sguid = HttpContext.Session.GetString(SessionKeyTable);
            if (sguid != null) 
            {
                _game.EndGame(new Guid(sguid));
                HttpContext.Session.Remove(SessionKeyTable);
            }
            List<Player> players = new List<Player>() {
                new Player("Matt"),
                new Player("Ava")
            };
            Guid gameId = _game.NewGame(players);
            HttpContext.Session.SetString(SessionKeyTable, gameId.ToString());
            return RedirectToAction("Index");
        }

        public IActionResult PlantTree()
        {
            int playerIx;
            int handIx;
            int groveIx;
            string sguid;
            if (Int32.TryParse(HttpContext.Request.Query["player_ix"], out playerIx))
            {
                if (Int32.TryParse(HttpContext.Request.Query["hand_ix"], out handIx))
                {
                    if (Int32.TryParse(HttpContext.Request.Query["grove_ix"], out groveIx))
                    {
                    sguid = HttpContext.Session.GetString(SessionKeyTable);
                        if (sguid != null) 
                        {
                            Table table = _game.GetTable(new Guid(sguid));
                            Player player = table.Players[playerIx];
                            Tree tree = player.Hand[handIx];
                            Grove grove = table.Groves[groveIx];
                            _game.PlantTree(table, grove, player, tree);                            
                        }            
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult CompleteTurn()
        {
            Table table = GetTable();
            if (table != null) 
            {
                _game.CompleteTurn(table);
            }
            return RedirectToAction("Index");
        }
    }
}