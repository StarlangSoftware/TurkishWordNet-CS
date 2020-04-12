using System.Collections.Generic;
using System.Globalization;

namespace WordNet
{
    public class LiteralWithSenseComparator : IComparer<Literal>
    {
        private readonly CultureInfo _cultureInfo;
        
        /**
         * Another constructor that sets the current culture.
         *
         * <param name="cultureInfo">CultureInfo</param>
         */
        public LiteralWithSenseComparator(CultureInfo cultureInfo)
        {
            this._cultureInfo = cultureInfo;
        }
        
        /**
         * <summary>Compares the source literal to the target literal according to the
         * given culture.  Returns an integer less than,
         * equal to or greater than zero depending on whether the source literal is
         * less than, equal to or greater than the target literal.</summary>
         *
         * <param name="literalA">the source Literal</param>
         * <param name="literalB">the target Literal</param>
         *
         * <returns>Returns an integer value. Value is less than zero if source is less than
         * target, value is zero if source and target are equal, value is greater than zero
         * if source is greater than target</returns>
         */
        public int Compare(Literal literalA, Literal literalB)
        {
            if (string.Compare(literalA.GetName(), literalB.GetName(), _cultureInfo, CompareOptions.OrdinalIgnoreCase) == 0)
            {
                if (literalA.GetSense() < literalB.GetSense())
                {
                    return -1;
                }

                if (literalA.GetSense() > literalB.GetSense())
                {
                    return 1;
                }

                return 0;
            }

            return string.Compare(literalA.GetName(), literalB.GetName(), _cultureInfo, CompareOptions.Ordinal);
        }

    }
}