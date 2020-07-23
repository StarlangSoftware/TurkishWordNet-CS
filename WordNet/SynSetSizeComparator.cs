using System.Collections.Generic;

namespace WordNet
{
    public class SynSetSizeComparator : IComparer<SynSet>
    {
        /**
         * <summary>Compares the source SynSet's literal size to the target SynSet's literal size according to the
         * collation rules for this Collator. Returns an integer less than,
         * equal to or greater than zero depending on whether the source SynSet's literal size is
         * less than, equal to or greater than the target SynSet's literal size.</summary>
         *
         * <param name="synSetA">the source SynSet</param>
         * <param name="synSetB">the target SynSet</param>
         * <returns>Returns an integer value. Value is less than zero if source is less than
         * target, value is zero if source and target are equal, value is greater than zero
         * if source is greater than target.</returns>
         */
        public int Compare(SynSet synSetA, SynSet synSetB)
        {
            return synSetA.GetSynonym().LiteralSize() - synSetB.GetSynonym().LiteralSize();
        }
    }
}