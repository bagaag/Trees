using System;
using System.Collections.Generic;

namespace Trees.Models.Stateful
{
    public class Table
    {
        public Table(Guid guid, List<Land> lands, List<Tree> trees, List<Event> events, List<Player> players) 
        {            
            LandDeck = new Stack<Land>(lands);
            TreeDeck = new Stack<Tree>(trees);
            EventDeck = new Stack<Event>(events);
            Players = players;
            Id = guid;
        }
        public Guid Id { get; set; }
        public Stack<Land> LandDeck { get; private set; } 
        public Stack<Tree> TreeDeck { get; set; } 
        public Stack<Event> EventDeck { get; set; }
        public List<Player> Players { get; set; } 
        public int CurrentPlayer { get; set; }
        public Player GetCurrentPlayer()
        {
            return this.Players[this.CurrentPlayer];
        }
        public Event CurrentEvent { get; set; }
        public List<Grove> Groves { get; set; } = new List<Grove>();
        public Stack<Tree> DeadTrees { get; set; } = new Stack<Tree>();
        public Stack<Event> PastEvents { get; set; } = new Stack<Event>();
        public List<string> TurnLog { get; set; } = new List<string>();
        public List<string> GameLog { get; set; } = new List<string>();

    }
}