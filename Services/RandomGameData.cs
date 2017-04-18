using System;
using System.Collections.Generic;
using Trees.Models;
using Trees.Models.Stateful;
using Trees.Models.Events;

namespace Trees.Services
{
    public class RandomGameData : IGameData
    {
        private Random random = new Random();

        public RandomGameData()
        {
            List<BaseEvent> _events = new List<BaseEvent>();
            List<Tree> _trees = new List<Tree>();
            List<Land> _lands = new List<Land>();

            for (int i = 0; i < 52; i++)
            {
                List<string> families = new List<string>()
                    {"Oak","Pine","Cherry","Cedar","Aspen","Maple","Birch","Ash","Poplar","Redwood","Linden","Spruce","Willow"};
                string family = families[random.Next(0,families.Count-1)];
                Tree tree = new Tree(family + " " + (i + 1), family,
                    new Habitat(R(), R(), R(), R()),
                    new Damage(R(), R(), R(), R(), R()));
                _trees.Add(tree);
            }
            List<string> biomes = new List<string>()
                {"Mountain","Field","Forest","City","Swamp","Coast","Pond","Wetlands"};
            for (int i = 0; i < 12; i++)
            {
                _lands.Add(new Land(biomes[random.Next(0, biomes.Count - 1)], random.Next(2,5),
                    new Habitat(R(), R(), R(), R())));
            }
            // _events.Add((new KillEvent("Forest Death", "Everything in the Forest Dies.")).WhereLand("Forest"));
            // _events.Add((new KillEvent("Mountain Death", "Everything in the Mountain Dies.")).WhereLand("Mountain"));
            // _events.Add((new KillEvent("Field Death", "Everything in the Field Dies.")).WhereLand("Field"));
            // _events.Add((new KillEvent("City Death", "Everything in the City Dies.")).WhereLand("City"));
            //_events.Add((new KillEvent("Water < 3", "Water < 3")).WhereHabitat(HabitatField.Water, Conditions.Operators.LT, 3));
            _events.Add(new EmptyEvent("Empty", "Empty"));
            _events.Add(new EmptyEvent("Empty", "Empty"));
            _events.Add(new EmptyEvent("Empty", "Empty"));
            _events.Add(new ExtraPlantingEvent("Extra Planting", "Extra Planting"));
            // _events.Add((new KillEvent("Lowest 1 Water", "Lowest 1 Water")).WhereHabitat(HabitatField.Water, -1));
            // _events.Add((new KillEvent("Highest 1 Water", "Highest 1 Water")).WhereHabitat(HabitatField.Water, 1));
            // _events.Add((new KillEvent("Highest 2 Wind", "Highest 2 Wind")).WhereDamage(DamageField.Wind, 2));
            // _events.Add((new KillEvent("Lowest 2 Score", "Lowest 2 Score")).WhereHabitatScore(-2));
            // _events.Add((new KillEvent("Highest 1 Score", "Highest 1 Score")).WhereHabitatScore(1));
            // _events.Add((new KillEvent("Current Player", "Current Player")).WherePlayer(true));
            // _events.Add((new KillEvent("Other Players", "Other Players")).WherePlayer(false));
            
/*
            _events.Add(new Event("Flood", "Any tree with water below 3 perishes in the flood. Mountain habitats are spared."));
            _events.Add(new Event("Wart Beetle", "The tree that takes the most insect damage in each land succombs to the wart beetle."));
            _events.Add(new Event("Lightning Storm", "The most recently planted tree is hit by lightening and dies."));
            _events.Add(new Event("Snow!", "A heavy snow fells the tree with the most snow damage."));
            _events.Add(new Event("Blizzard!", "A blizzard brings heavy snow and high winds. Any tree with more than 2 wind and 2 snow damage is lost."));
            _events.Add(new Event("Monsoon", "After four weeks of rain, the tree with the lowest water habitat cannot survive."));
            _events.Add(new Event("Flood", "A flood devistates the forest. The tree with the lowest water habitat in each Swamp, Pond, Coast and Wetlands cannot survive."));
            _events.Add(new Event("Fire!", "A campfire gets out of control and burns the three trees with the highest fire damage to the ground."));
            _events.Add(new Event("Clear Cutting", "A rogue logging operation takes out every tree with a harvesting damage of 7 and higher."));
            _events.Add(new Event("Harvest Time", "The tree with the highest harvesting damage is felled for lumber."));
            _events.Add(new Event("Hunting Season", "The deer population is out of control. The last competing tree planted is eaten."));
            _events.Add(new Event("Beavers", "Your opponents tree with the best overall match in Wetlands is felled by a beaver."));
            _events.Add(new Event("Highrise", "A new highrise claims your opponents City tree with the worse habitat match."));
            _events.Add(new Event("Global Warming", "The three trees with the lowest temparature habitat can't take the heat."));
            _events.Add(new Event("Acorns!", "If you have an oak tree planted, plant another oak tree if you have one in your hand in addition to your normal turn."));
            _events.Add(new Event("Rain", "A much needed rain falls. Plant an extra tree if your top card has a water habitat of 3 or more."));
*/
            Trees = new Deck<Tree>(_trees);
            Events = new Deck<BaseEvent>(_events);
            Lands = new Deck<Land>(_lands);
            
        }

        private int R()
        {
            return random.Next(1, 4);
        }
        public Deck<Tree> Trees { get; private set; } 
        public Deck<BaseEvent> Events { get; private set; }
        public Deck<Land> Lands { get; private set; }
    }
}