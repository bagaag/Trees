using System.Collections.Generic;
using Trees.Core.Models.EventFilters;
using Trees.Core.Models.Stateful;
using Trees.Core.Services;

namespace Trees.Core.Models.Events
{
    /// <summary>
    /// Supports an event killing plantings on the table via several possible filter combinations
    /// </summary>
    public class KillEvent : PlantingFilterBaseEvent
    {
        /// <summary>
        /// List of filters that specify what plantings should be killed
        /// </summary>

        /// <summary>
        /// Default event constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public KillEvent(string name, string description) : base(name, description) { }

        /// <summary>
        /// Runs the event filters on all plantings and removes list of matched plantings that have to die
        /// </summary>
        /// <returns></returns>
        public override void Execute(GamePlay game, Table table)
        {
            var kill = RunFilters(table);
            // kill trees returned by filters
            foreach (Planting planting in kill)
            {
                game.RemoveTree(table, planting);
            }
        }

        public override void Stage(GamePlay game, Table table)
        {
            var kill = RunFilters(table);
            // kill trees returned by filters
            foreach (Planting planting in kill)
            {
                planting.Flag = PlantingFlag.Kill;
            }
        }

        /// <summary>
        /// Specifies a Land filter for single land
        /// </summary>
        /// <param name="landName">Land.Name to match on</param>
        /// <param name="matches">Use false to match plantings without this land instead of with this land</param>
        /// <returns></returns>
        public KillEvent WhereLand(string landName, bool matches = true)
        {
            Filters.Add(new LandFilter(new string[] { landName }, matches));
            return this;
        }
        /// <summary>
        /// Specifies a Land filter for multiple lands
        /// </summary>
        /// <param name="landName"></param>
        /// <param name="matches"></param>
        /// <returns></returns>
        public KillEvent WhereLand(string[] landNames, bool matches = true)
        {
            Filters.Add(new LandFilter(landNames, matches));
            return this;
        }

        /// <summary>
        /// Specifies a filter on a habitat value
        /// </summary>
        /// <param name="field">Habitat field to filter on</param>
        /// <param name="condition">Operator to apply in filter</param>
        /// <param name="value">Value to apply in filter</param>
        /// <returns></returns>
        public KillEvent WhereHabitat(HabitatField field, Conditions.Operators op, int value)
        {
            Filters.Add(new HabitatFilter(field, op, value));
            return this;
        }

        /// <summary>
        /// Specifies a filter on the lowest or highest N habitat values
        /// </summary>
        /// <param name="field">Habitat field to filter on</param>
        /// <param name="value">Negative filters on lowest N values, positive filters on highest N values</param>
        /// <returns></returns>
        public KillEvent WhereHabitat(HabitatField field, int value)
        {
            Filters.Add(new HabitatSuperlativeFilter(field, value));
            return this;
        }

        /// <summary>
        /// Specifies a filter on a damage value
        /// </summary>
        /// <param name="field">Damage field to filter on</param>
        /// <param name="op">Operator to apply in filter</param>
        /// <param name="value">Value to apply in filter</param>
        /// <returns></returns>
        public KillEvent WhereDamage(DamageField field, Conditions.Operators op, int value)
        {
            Filters.Add(new DamageFilter(field, op, value));
            return this;
        }

        /// <summary>
        /// Specifies a filter on the lowest or highest N damage values
        /// </summary>
        /// <param name="field">Damage field to filter on</param>
        /// <param name="value">Negative filters on lowest N values, positive filters on highest N values</param>
        /// <returns></returns>
        public KillEvent WhereDamage(DamageField field, int value)
        {
            Filters.Add(new DamageSuperlativeFilter(field, value));
            return this;
        }

        /// <summary>
        /// Specifies a filter on a planting's habitat match score
        /// </summary>
        /// <param name="op">Operator to apply in filter</param>
        /// <param name="value">Value to apply in filter</param>
        /// <returns></returns>
        public KillEvent WhereHabitatScore(Conditions.Operators op, int value)
        {
            Filters.Add(new HabitatScoreFilter(op, value));
            return this;
        }

        /// <summary>
        /// Specifies a filter on the lowest or highest N planting scores
        /// </summary>
        /// <param name="value">Negative filters on lowest N values, positive filters on highest N values</param>
        /// <returns></returns>
        public KillEvent WhereHabitatScore(int value)
        {
            Filters.Add(new HabitatScoreSuperlativeFilter(value));
            return this;
        }

        /// <summary>
        /// Specifies a superlative filter on a planting's age
        /// </summary>
        /// <param name="value">Positive number for the oldest N, negative for the youngest N</param>
        /// <returns></returns>
        public KillEvent WhereAge(int value)
        {
            Filters.Add(new AgeSuperlativeFilter(value));
            return this;
        }

        /// <summary>
        /// Specifies a filter on the Genus of a planting
        /// </summary>
        /// <param name="genus"></param>
        /// <param name="matches">Use False to filter on plantings without this genus</param>
        /// <returns></returns>
        public KillEvent WhereGenus(string genus, bool matches = true)
        {
            Filters.Add(new GenusFilter(new string[] { genus }, matches));
            return this;
        }
        public KillEvent WhereGenus(string[] genus, bool matches = true)
        {
            Filters.Add(new GenusFilter(genus, matches));
            return this;
        }

        /// <summary>
        /// Specifies a filter on whether planting belongs to the current player or not
        /// </summary>
        /// <param name="current">true to filter on current player</param>
        /// <returns></returns>
        public KillEvent WherePlayer(bool current)
        {
            Filters.Add(new PlayerFilter(current));
            return this;
        }
        public KillEvent WhereDamageSuperlative(DamageField field, int value)
        {
            Filters.Add(new DamageSuperlativeFilter(field, value));
            return this;
        }
        public KillEvent WhereHabitatScoreSuperlative(int value)
        {
            Filters.Add(new HabitatScoreSuperlativeFilter(value));
            return this;
        }
        public KillEvent WhereHabitatSuperlative(HabitatField field, int value)
        {
            Filters.Add(new HabitatSuperlativeFilter(field, value));
            return this;
        }
    }
}