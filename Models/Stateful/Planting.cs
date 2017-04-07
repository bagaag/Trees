namespace Trees.Models.Stateful
{
    public class Planting
    {
        public Planting(Player player, Tree tree)
        {
            Player = player;
            Tree = tree;
        }
        public Tree Tree { get; set; }
        public Player Player { get; set; }
        public int Score { get; set; }
        public bool CanBeReplaced { get; set; } = false;
    }
}