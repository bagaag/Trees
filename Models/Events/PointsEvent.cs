using Trees.Models.Stateful;
using Trees.Services;
using Trees.Models.EventFilters;

namespace Trees.Models.Events
{
    /// <summary>
    /// Supports adding or subtracting points
    /// Planting filters are used to indicate whether and how many extra points are granted
    /// </summary>
    public class PointsEvent : PlantingFilterBaseEvent
    {
        /// <summary>
        /// Default event constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public PointsEvent(string name, string description) : base(name, description) { }

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
            int points = 0;
            if (Filters.Count > 0)
            {
                var plantings = RunFilters(table);
                if (!GrantPerPlanting && plantings.Count > 0)
                {
                    points = Points;
                }
                // apply points for plantings returned by filters
                foreach (Planting planting in plantings)
                {
                    planting.Flag = PlantingFlag.Kill;
                    if (GrantPerPlanting)
                    {
                        points += Points;
                    }
                }
            }
            // default to extra planting if no filters are specified
            else
            {
                points += Points;
            }
            // apply to points differential
            if (IsForCurrentPlayer)
            {
                table.GetCurrentPlayer().PointsDifferential += points;
                table.TurnLog.Add(table.GetCurrentPlayer().Name + " receives " + points + " points.");
            }
            //TODO: Allow filtering of trees per player to apply points specific to each competitor
            else
            {
                foreach (Player player in table.Players)
                {
                    player.PointsDifferential += points;
                    table.TurnLog.Add(player.Name + " receives " + points + " points.");
                }
            }
        }

        /// <summary>
        /// How many points (plus or minus) are granted
        /// </summary>
        /// <returns></returns>
        public int Points { get; set; } = 1;

        /// <summary>
        /// If true, player receives points differential for each planting matched by filters. 
        /// If false, player receives points differential if 1 or more plantings are matched. 
        /// </summary>
        /// <returns></returns>
        public bool GrantPerPlanting { get; set; } = false;

        public PointsEvent ForEveryPlantingMatched(bool value = true)
        {
            GrantPerPlanting = value;
            return this;
        }

        public PointsEvent WithPoints(int points = 1)
        {
            Points = points;
            return this;
        }

        public bool IsForCurrentPlayer { get; set; } = true;
        public PointsEvent ForCurrentPlayer(bool current = true)
        {
            IsForCurrentPlayer = current;
            return this;
        }

        /// <summary>
        /// Specifies a Land filter for single land
        /// </summary>
        /// <param name="landName">Land.Name to match on</param>
        /// <param name="matches">Use false to match plantings without this land instead of with this land</param>
        /// <returns></returns>
        public PointsEvent WhereLand(string landName, bool matches = true)
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
        public PointsEvent WhereLand(string[] landNames, bool matches = true)
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
        public PointsEvent WhereHabitat(HabitatField field, Conditions.Operators op, int value)
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
        public PointsEvent WhereHabitat(HabitatField field, int value)
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
        public PointsEvent WhereDamage(DamageField field, Conditions.Operators op, int value)
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
        public PointsEvent WhereDamage(DamageField field, int value)
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
        public PointsEvent WhereHabitatScore(Conditions.Operators op, int value)
        {
            Filters.Add(new HabitatScoreFilter(op, value));
            return this;
        }

        /// <summary>
        /// Specifies a filter on the lowest or highest N planting scores
        /// </summary>
        /// <param name="value">Negative filters on lowest N values, positive filters on highest N values</param>
        /// <returns></returns>
        public PointsEvent WhereHabitatScore(int value)
        {
            Filters.Add(new HabitatScoreSuperlativeFilter(value));
            return this;
        }

        /// <summary>
        /// Specifies a superlative filter on a planting's age
        /// </summary>
        /// <param name="value">Positive number for the oldest N, negative for the youngest N</param>
        /// <returns></returns>
        public PointsEvent WhereAge(int value)
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
        public PointsEvent WhereGenus(string genus, bool matches = true)
        {
            Filters.Add(new GenusFilter(new string[] { genus }, matches));
            return this;
        }
        public PointsEvent WhereGenus(string[] genus, bool matches = true)
        {
            Filters.Add(new GenusFilter(genus, matches));
            return this;
        }
        public PointsEvent WhereDamageSuperlative(DamageField field, int value)
        {
            Filters.Add(new DamageSuperlativeFilter(field, value));
            return this;
        }
        public PointsEvent WhereHabitatScoreSuperlative(int value)
        {
            Filters.Add(new HabitatScoreSuperlativeFilter(value));
            return this;
        }
        public PointsEvent WhereHabitatSuperlative(HabitatField field, int value)
        {
            Filters.Add(new HabitatSuperlativeFilter(field, value));
            return this;
        }
        /// <summary>
        /// Specifies a filter on whether planting belongs to the current player or not
        /// </summary>
        /// <param name="current">true to filter on current player</param>
        /// <returns></returns>
        public PointsEvent WherePlayer(bool current)
        {
            Filters.Add(new PlayerFilter(current));
            return this;
        }

    }
}