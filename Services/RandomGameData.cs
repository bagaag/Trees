using System;
using System.Collections.Generic;
using Trees.Models;

namespace Trees.Services
{
    public class RandomGameData : IGameData
    {
        private Random random = new Random();

        public RandomGameData()
        {
            for (int i = 0; i < 52; i++)
            {
                List<string> families = new List<string>()
                    {"Oak","Pine","Cherry","Cedar","Aspen","Maple","Birch","Ash","Poplar","Redwood","Linden","Spruce","Willow"};
                string family = families[random.Next(0,families.Count-1)];
                Tree tree = new Tree(family + " " + (i + 1), family,
                    new Habitat(R(), R(), R(), R()),
                    new Damage(R(), R(), R(), R(), R()));
                Trees.Add(tree);
            }
            List<string> biomes = new List<string>()
                {"Mountain","Field","Forest","City","Swamp","Coast","Pond","Wetlands"};
            for (int i = 0; i < 12; i++)
            {
                Lands.Add(new Land(biomes[random.Next(0, biomes.Count - 1)], random.Next(2,5),
                    new Habitat(R(), R(), R(), R())));
            }
            Events.Add(new Event("Flood", "Any tree with water below 4 perishes in the flood. Mountain habitats are spared."));
            Events.Add(new Event("Acorns!", "If you have an oak tree planted, plan another oak tree if you have one in your hand."));
            Events.Add(new Event("Wart Beetle", "The tree that takes the most insect damage in each land succombs to the wart beetle."));
            Events.Add(new Event("Lightning Storm", "The most recently planted tree is hit by lightening and dies."));
            Events.Add(new Event("Snow!", "A heavy snow fells the tree with the most snow damage."));
            Events.Add(new Event("Blizzard!", "A blizzard brings heavy snow and high winds. Any tree with more than 5 wind and 5 snow damage is lost."));
            Events.Add(new Event("Rain", "A much needed rain falls. Swap your planting with the worst water match with any competing tree."));
            Events.Add(new Event("Monsoon", "After four weeks of rain, the tree with the lowest water habitat cannot survive."));
            Events.Add(new Event("Flood", "A flood devistates the forest. The tree with the lowest water damage in each Swamp, Pond, Coast and Wetlands cannot survive."));
            Events.Add(new Event("Fire!", "A campfire gets out of control and burns the three trees with the highest fire damage to the ground."));
            Events.Add(new Event("Clear Cutting", "A rogue logging operation takes out every tree with a harvesting damage of 7 and higher."));
            Events.Add(new Event("Harvest Time", "The tree with the highest harvesting damage is felled for lumber."));
            Events.Add(new Event("Hunting Season", "The deer population is out of control. The last competing tree planted is eaten."));
            Events.Add(new Event("Beavers", "Your opponents tree with the best overall match in Wetlands is felled by a beaver."));
            Events.Add(new Event("Highrise", "A new highrise claims your oppoents City tree with the worse habitat match."));
            Events.Add(new Event("Global Warming", "The three trees with the lowest temparature damage can't take the heat."));
        }

        private int R()
        {
            return random.Next(1, 10);
        }
        public List<Tree> Trees { get; private set; } = new List<Tree>();
        public List<Event> Events { get; private set; } = new List<Event>();
        public List<Land> Lands { get; private set; } = new List<Land>();
    }
}