using System;

namespace Trees.Models 
{
    public enum HabitatField { Sun, Water, Soil, Temperature }

    public class Habitat 
    {
        public Habitat (int sun, int water, int soil, int temperature) {
            Sun = sun;
            Water = water;
            Soil = soil;
            Temperature = temperature;
        }

        public int ValueOf(HabitatField field) 
        {
            switch (field)
            {
                case HabitatField.Sun:
                    return Sun;
                case HabitatField.Water:
                    return Water;
                case HabitatField.Soil:
                    return Soil;
                case HabitatField.Temperature:
                    return Temperature;
                default:
                    throw new Exception("Invalid HabitatField");
            }
        }
        public int Sun { get; set; }

        public int Water { get; set; }

        public int Soil { get; set; }

        public int Temperature { get; set; }

    }
}