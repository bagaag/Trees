using System;

namespace Trees.Core.Models
{
    public static class Conditions
    {
        public enum Operators { LT, LTE, E, NE, GTE, GT }

        public static bool Evaluate<T>(T value1, Operators condition, T value2) where T : IComparable
        {
            int comparison = value1.CompareTo(value2);
            if (condition == Operators.LT) return comparison < 0;
            else if (condition == Operators.LTE) return comparison <= 0;
            else if (condition == Operators.E) return comparison == 0;
            else if (condition == Operators.NE) return comparison != 0;
            else if (condition == Operators.GTE) return comparison >= 0;
            else if (condition == Operators.GT) return comparison > 0;
            else throw new Exception("IComparable cannot evaluate superlatives");
        }

    }
}
