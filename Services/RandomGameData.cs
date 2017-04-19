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

            _events.Add((new KillEvent(
                "Flood","Any tree with water below 3 perishes in the flood. Mountain habitats are spared."))
                .WhereLand("Mountain", false)
                .WhereHabitat(HabitatField.Water, Conditions.Operators.LT, 3)
            );

            _events.Add((new KillEvent(
                "Wart Beetle", "The tree that takes the most insect damage succombs to the wart beetle."))
                .WhereDamageSuperlative(DamageField.Insect, 1)
            );
            
            _events.Add((new KillEvent(
                "Lightning Storm", "The oldest living tree is hit by lightening and dies."))
                .WhereAge(1)
            );

            _events.Add((new KillEvent(
                "Snow!", "A heavy snow fells the tree with the most snow damage."))
                .WhereDamageSuperlative(DamageField.Snow, 1)
            );

            _events.Add((new KillEvent(
                "Blizzard!", "A blizzard brings heavy snow and high winds. Any tree with more than 2 wind and 2 snow damage is lost."))
                .WhereDamage(DamageField.Wind, Conditions.Operators.GT, 2)
                .WhereDamage(DamageField.Snow, Conditions.Operators.GT, 2)
            );

            _events.Add((new KillEvent(
                "Monsoon", "After four weeks of rain, the tree with the lowest water habitat cannot survive."))
                .WhereHabitatSuperlative(HabitatField.Water, -1)
            );

            _events.Add((new KillEvent(
                "Fire!", "A campfire gets out of control and burns the three trees with the highest fire damage to the ground."))
                .WhereDamageSuperlative(DamageField.Fire, 3)
            );

            _events.Add((new KillEvent(
                "Clear Cutting", "A rogue logging operation takes out every tree with a harvesting damage of 3 and higher."))
                .WhereDamage(DamageField.Harvesting, Conditions.Operators.GTE, 3)
            );

            _events.Add((new KillEvent(
                "Harvest Time", "The tree with the highest harvesting damage is felled for lumber."))
                .WhereDamageSuperlative(DamageField.Harvesting, 1)
            );

            _events.Add((new KillEvent(
                "Hunting Season", "The deer population is out of control. The last competing tree planted is eaten."))
                .WherePlayer(false)
                .WhereAge(-1)
            );

            _events.Add((new KillEvent(
                "Beavers", "Your opponent's tree with the best overall match in Wetlands is felled by a beaver."))
                .WherePlayer(false)
                .WhereLand("Wetlands")
                .WhereHabitatScoreSuperlative(1)
            );

            _events.Add((new KillEvent(
                "Highrise", "A new highrise claims your opponents City tree with the worse habitat match."))
                .WherePlayer(false)
                .WhereLand("City")
                .WhereHabitatScoreSuperlative(-1)
            );

            _events.Add((new KillEvent(         
                "Global Warming", "The three trees with the lowest temparature habitat can't take the heat."))
                .WhereHabitatSuperlative(HabitatField.Temperature, -3)
            );

            //TODO: Add filter on cards in hand
            _events.Add((new ExtraPlantingEvent( 
                "Acorns!", "If you have an oak tree planted, plant another oak tree if you have one in your hand in addition to your normal turn."))
                .WherePlayer(true)
                .WhereGenus("Oak")
            );

            //TODO: Add filter on cards in hand
            _events.Add((new ExtraPlantingEvent( 
                "Rain", "A much needed rain falls. Plant an extra tree if your top card has a water habitat of 3 or more."))
                .WherePlayer(true)
                .WhereHabitat(HabitatField.Water, Conditions.Operators.GTE, 3)
            );

            //TODO: Allow points filter to work for all players at once
            _events.Add((new PointsEvent(
                "Summer","Summer comes and shines on sun loving trees. You gain two points for each planting with sun of 3 or more."))
                .WhereHabitat(HabitatField.Sun, Conditions.Operators.GTE, 3)
                .WithPoints(2)
                .ForEveryPlantingMatched()
            );

            _events.Add((new PointsEvent(
                "Rain","A soft rain falls. You gain a point for each planting with water of 3 or more."))
                .WhereHabitat(HabitatField.Water, Conditions.Operators.GTE, 3)
                .ForEveryPlantingMatched()
            );

            _events.Add((new PointsEvent(
                "Cold Winter","A cold winter costs you three points for each planting with temperature of 3 or more."))
                .WhereHabitat(HabitatField.Temperature, Conditions.Operators.GTE, 3)
                .ForEveryPlantingMatched()
                .WithPoints(3)
            );

            _events.Add((new PointsEvent(
                "Pine Flowering","The pines trees flower! A costly gamble. You gain three points for each pine tree planted."))
                .WhereGenus("Pine")
                .ForEveryPlantingMatched()
                .WithPoints(3)
            );

            _events.Add((new PointsEvent(
                "Pine Flowering","The pines trees flower! A costly gamble. You lose three points for each pine tree planted."))
                .WhereGenus("Pine")
                .ForEveryPlantingMatched()
                .WithPoints(-3)
            );

            _events.Add((new PointsEvent(
                "Acid Rain","Acid rain makes life for your trees that much harder and costs you 10 points."))
                .WithPoints(-10)
            );         

            //TODO: Populate fun fact events
            _events.Add((new EmptyEvent(
                "Empty Event 1","Insert fun tree fact here."))
            );
            _events.Add((new EmptyEvent(
                "Empty Event 2","Insert fun tree fact here."))
            );
            _events.Add((new EmptyEvent(
                "Empty Event 3","Insert fun tree fact here."))
            );
            _events.Add((new EmptyEvent(
                "Empty Event 4","Insert fun tree fact here."))
            );
            _events.Add((new EmptyEvent(
                "Empty Event 5","Insert fun tree fact here."))
            );
            _events.Add((new EmptyEvent(
                "Empty Event 6","Insert fun tree fact here."))
            );
            _events.Add((new EmptyEvent(
                "Empty Event 7","Insert fun tree fact here."))
            );
            _events.Add((new EmptyEvent(
                "Empty Event 8","Insert fun tree fact here."))
            );
            _events.Add((new EmptyEvent(
                "Empty Event 9","Insert fun tree fact here."))
            );
            _events.Add((new EmptyEvent(
                "Empty Event 10","Insert fun tree fact here."))
            );
            _events.Add((new EmptyEvent(
                "Empty Event 11","Insert fun tree fact here."))
            );
            _events.Add((new EmptyEvent(
                "Empty Event 12","Insert fun tree fact here."))
            );

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