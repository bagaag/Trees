using Trees.Models.Stateful;
using Trees.Services;
using Trees.Models.EventFilters;

namespace Trees.Models.Events
{
    /// <summary>
    /// Supports an event adding extra plantings as part of the current turn. 
    /// Planting filters are used to indicate whether and how many extra plantings are granted.
    /// </summary>
    public class ExtraPlantingEvent : PlantingFilterBaseEvent
    {
        /// <summary>
        /// Default event constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public ExtraPlantingEvent(string name, string description) : base(name, description) { }

        /// <summary>
        /// Runs the event filters on all plantings and removes list of matched plantings that have to die
        /// </summary>
        /// <returns></returns>
        public override void Execute(GamePlay game, Table table)
        {
            // nothing happens here
        }

        public override void Stage(GamePlay game, Table table)
        {
            int extraPlantings = 0;
            if (Filters.Count > 0)
            {
                var plantings = RunFilters(table);
                if (!GrantPerPlanting && plantings.Count > 0)
                {
                    extraPlantings = ExtraPlantings;
                }
                // kill trees returned by filters
                foreach (Planting planting in plantings)
                {
                    planting.Flag = PlantingFlag.Kill;
                    if (GrantPerPlanting)
                    {
                        extraPlantings += ExtraPlantings;
                    }
                }
            }
            // default to extra planting if no filters are specified
            else 
            {
                extraPlantings += ExtraPlantings;
            }
            table.PlantingsRemaining += extraPlantings;
            table.TurnLog.Add(table.GetCurrentPlayer().Name + " granted " + extraPlantings + " extra plantings.");
        }

        /// <summary>
        /// How many extra plantings are granted
        /// </summary>
        /// <returns></returns>
        public int ExtraPlantings { get; set; } = 1;

        /// <summary>
        /// If true, player receives extra planting(s) for each planting matched by filters. 
        /// If false, player receives extra planting(s) if 1 or more plantings are matched. 
        /// </summary>
        /// <returns></returns>
        public bool GrantPerPlanting { get; set; } = false;

        public ExtraPlantingEvent ForEveryPlantingMatched(bool value = true)
        {
            GrantPerPlanting = value;
            return this;
        }

        public ExtraPlantingEvent WithExtraPlantings(int plantings)
        {
            ExtraPlantings = plantings;
            return this;
        }

        /// <summary>
        /// Specifies a Land filter for single land
        /// </summary>
        /// <param name="landName">Land.Name to match on</param>
        /// <param name="matches">Use false to match plantings without this land instead of with this land</param>
        /// <returns></returns>
        public ExtraPlantingEvent WhereLand(string landName, bool matches = true)
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
        public ExtraPlantingEvent WhereLand(string[] landNames, bool matches = true)
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
        public ExtraPlantingEvent WhereHabitat(HabitatField field, Conditions.Operators op, int value)
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
        public ExtraPlantingEvent WhereHabitat(HabitatField field, int value)
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
        public ExtraPlantingEvent WhereDamage(DamageField field, Conditions.Operators op, int value)
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
        public ExtraPlantingEvent WhereDamage(DamageField field, int value)
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
        public ExtraPlantingEvent WhereHabitatScore(Conditions.Operators op, int value)
        {
            Filters.Add(new HabitatScoreFilter(op, value));
            return this;
        }

        /// <summary>
        /// Specifies a filter on the lowest or highest N planting scores
        /// </summary>
        /// <param name="value">Negative filters on lowest N values, positive filters on highest N values</param>
        /// <returns></returns>
        public ExtraPlantingEvent WhereHabitatScore(int value)
        {
            Filters.Add(new HabitatScoreSuperlativeFilter(value));
            return this;
        }

        /// <summary>
        /// Specifies a superlative filter on a planting's age
        /// </summary>
        /// <param name="value">Positive number for the oldest N, negative for the youngest N</param>
        /// <returns></returns>
        public ExtraPlantingEvent WhereAge(int value)
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
        public ExtraPlantingEvent WhereGenus(string genus, bool matches = true)
        {
            Filters.Add(new GenusFilter(new string[] { genus }, matches));
            return this;
        }
        public ExtraPlantingEvent WhereGenus(string[] genus, bool matches = true)
        {
            Filters.Add(new GenusFilter(genus, matches));
            return this;
        }
        public ExtraPlantingEvent WhereDamageSuperlative(DamageField field, int value)
        {
            Filters.Add(new DamageSuperlativeFilter(field, value));
            return this;
        }
        public ExtraPlantingEvent WhereHabitatScoreSuperlative(int value)
        {
            Filters.Add(new HabitatScoreSuperlativeFilter(value));
            return this;
        }
        public ExtraPlantingEvent WhereHabitatSuperlative(HabitatField field, int value)
        {
            Filters.Add(new HabitatSuperlativeFilter(field, value));
            return this;
        }
        /// <summary>
        /// Specifies a filter on whether planting belongs to the current player or not
        /// </summary>
        /// <param name="current">true to filter on current player</param>
        /// <returns></returns>
        public ExtraPlantingEvent WherePlayer(bool current)
        {
            Filters.Add(new PlayerFilter(current));
            return this;
        }
    }
}