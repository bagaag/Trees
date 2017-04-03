
namespace Trees.Models 
{
    public class Habitat 
    {
        public Habitat (int sun, int water, int soil, int temperature) {
            Sun = sun;
            Water = water;
            Soil = soil;
            Temperature = temperature;
        }

        public int Sun { get; set; }

        public int Water { get; set; }

        public int Soil { get; set; }

        public int Temperature { get; set; }

    }
}