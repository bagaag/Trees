using System;
using System.Collections.Generic;

namespace Trees.Models.Stateful
{
    /// <summary>
    /// Maintains a deck of cards and discard pile. Reshuffles the discard pile back 
    /// into the deck when the deck is empty.
    /// </summary>
    public class Deck<T>
    {
        private static Random rng = new Random();
        private List<T> _deck;
        private List<T> _discard;

        public Deck(List<T> list) 
        {
            _deck = list;
            _discard = new List<T>();
            Shuffle();
        }

        public int DeckCount { get {
            return _deck.Count;
        }}
        public int DiscardCount { get {
            return _discard.Count;
        }}

        public T Draw()
        {
            if (_deck.Count==0) throw new Exception("The deck is empty!");

            T card = _deck[0];
            _deck.RemoveAt(0);
            if (_deck.Count==0) 
            {
                Shuffle();
            }
            return card;
        }

        /// <summary>
        /// Clears discard pile into the deck and shuffles
        /// </summary>
        private void Shuffle()
        {
            _deck.AddRange(_discard);
            _discard = new List<T>();

            int i, n, k;
            for (i=0; i<3; i++) 
            {
                n = _deck.Count;
                while (n > 1)
                {
                    n--;
                    k = rng.Next(n + 1);
                    T value = _deck[k];
                    _deck[k] = _deck[n];
                    _deck[n] = value;
                }
            }
        }

        public List<T> Hand(int count) 
        {
            List<T> hand = new List<T>(count);
            for (int i=0; i<count; i++)
            {
                hand.Add(Draw());
            }
            return hand;
        }

        public void Return(T card) 
        {
            _discard.Add(card);
        }

        public void Return(List<T> cards) 
        {
            _discard.AddRange(cards);
        }
    }
}