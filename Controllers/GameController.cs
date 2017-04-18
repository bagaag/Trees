using Microsoft.AspNetCore.Mvc;
using Trees.Services;
using Trees.Models.Stateful;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

namespace Trees.Controllers
{
    public class GameController : Controller
    {
        private readonly GamePlay _game;
        const string SessionKeyTable = "table";

        public GameController(GamePlay game)
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
            int groveIx;
            string sguid;
            if (Int32.TryParse(HttpContext.Request.Query["grove_ix"], out groveIx))
            {
            sguid = HttpContext.Session.GetString(SessionKeyTable);
                if (sguid != null) 
                {
                    Table table = _game.GetTable(new Guid(sguid));
                    Grove grove = table.Groves[groveIx];
                    _game.PlantTree(table, grove);                            
                }            
            }

            return RedirectToAction("Index");
        }

        public IActionResult ReplaceTree()
        {
            int groveIx;
            int plantingIx;
            string sguid;
            if (Int32.TryParse(HttpContext.Request.Query["grove_ix"], out groveIx))
            {
                if (Int32.TryParse(HttpContext.Request.Query["planting_ix"], out plantingIx))
                {
                sguid = HttpContext.Session.GetString(SessionKeyTable);
                    if (sguid != null) 
                    {
                        Table table = _game.GetTable(new Guid(sguid));
                        Grove grove = table.Groves[groveIx];
                        Planting planting = grove.Plantings[plantingIx];
                        _game.ReplaceTree(table, planting);                            
                    }
                }
            }

            return RedirectToAction("Index");
        }

/* 
        public IActionResult RemoveTree()
        {
            int groveIx;
            int plantingIx;
            string sguid;
            if (Int32.TryParse(HttpContext.Request.Query["grove_ix"], out groveIx))
            {
                if (Int32.TryParse(HttpContext.Request.Query["planting_ix"], out plantingIx))
                {
                sguid = HttpContext.Session.GetString(SessionKeyTable);
                    if (sguid != null) 
                    {
                        Table table = _game.GetTable(new Guid(sguid));
                        Grove grove = table.Groves[groveIx];
                        Planting planting = grove.Plantings[plantingIx];
                        _game.RemoveTree(table, planting);                            
                    }
                }
            }

            return RedirectToAction("Index");
        }
*/
        public IActionResult ProcessEvent()
        {
            Table table = GetTable();
            if (table != null)
            {
                _game.ProcessEvent(table);
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