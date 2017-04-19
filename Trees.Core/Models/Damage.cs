using System;
namespace Trees.Core.Models 
{
    public enum DamageField {Insect, Snow, Wind, Fire, Harvesting}

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

        public int ValueOf(DamageField field) 
        {
            switch (field)
            {
                case DamageField.Insect:
                    return Insect;
                case DamageField.Snow:
                    return Snow;
                case DamageField.Wind:
                    return Wind;
                case DamageField.Fire:
                    return Fire;
                case DamageField.Harvesting:
                    return Harvesting;
                default:
                    throw new Exception("Invalid DamageField");
            }
        }
        
        public int Insect { get; set; }

        public int Snow { get; set; }

        public int Wind { get; set; }
        
        public int Fire { get; set; }

        public int Harvesting { get; set; }
        
    }
}