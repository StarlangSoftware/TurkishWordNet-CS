namespace WordNet.Similarity
{
    public class LCH : Similarity
    {
        /// <summary>
        /// Class constructor that sets the wordnet.
        /// </summary>
        /// <param name="wordNet">WordNet for which similarity metrics will be calculated.</param>
        public LCH(WordNet wordNet) : base(wordNet)
        {
        }

        /// <summary>
        /// Computes LCH wordnet similarity metric between two synsets.
        /// </summary>
        /// <param name="synSet1">First synset</param>
        /// <param name="synSet2">Second synset</param>
        /// <returns>LCH wordnet similarity metric between two synsets</returns>
        public override double ComputeSimilarity(SynSet synSet1, SynSet synSet2)
        {
            var pathToRootOfSynSet1 = wordNet.FindPathToRoot(synSet1);
            var pathToRootOfSynSet2 = wordNet.FindPathToRoot(synSet2);
            var pathLength = wordNet.FindPathLength(pathToRootOfSynSet1, pathToRootOfSynSet2);
            float maxDepth = System.Math.Max(pathToRootOfSynSet1.Count, pathToRootOfSynSet2.Count);
            return -System.Math.Log(pathLength / (2 * maxDepth));
        }
    }
}