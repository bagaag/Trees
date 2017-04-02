using Microsoft.AspNetCore.Mvc;
using Trees.Services;
using Trees.Models;
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

        public IActionResult Index()
        {
            string sguid = HttpContext.Session.GetString(SessionKeyTable);
            if (sguid != null) 
            {
                Guid tableGuid = new Guid(sguid);
                ViewData["Table"] = _game.GetTable(tableGuid);
            }
            return View();
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
    }
}