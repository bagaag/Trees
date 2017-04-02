namespace Trees.Models 
{
    public class Damage 
    {
        public Damage(int insect, int snow, int wind, int fire, int harvesting) 
        {
            Insect = insect;
            Snow = snow;
            Wind = wind;
            Fire = fire;
            Harvesting = harvesting;
        }

        public int Insect { get; set; }

        public int Snow { get; set; }

        public int Wind { get; set; }
        
        public int Fire { get; set; }

        public int Harvesting { get; set; }
        
    }
}