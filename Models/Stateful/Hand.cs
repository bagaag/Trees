using System.Collections.Generic;

namespace Trees.Models.Stateful
{
    public class Hand  
    {
        private const int HandCount = 5;
        private List<Tree> _cards;
        private Deck<Tree> _deck;

        public Hand(Deck<Tree> deck) {
            _cards = deck.Hand(HandCount);
            _deck = deck;
        }

        public Tree Draw() 
        {
            Tree tree = _cards[0];
            _cards.RemoveAt(0);
            if (_cards.Count==0) 
            {
                _cards = _deck.Hand(HandCount);
            }
            return tree;
        }

        public Tree Peek()
        {
            return _cards[0];
        }

        public int Count { get { return _cards.Count; }}
    }
}