namespace WordNet.Similarity
{
    public class SimilarityPath : Similarity
    {
        /// <summary>
        /// Class constructor that sets the wordnet and the information content hash map.
        /// </summary>
        /// <param name="wordNet">WordNet for which similarity metrics will be calculated.</param>
        public SimilarityPath(WordNet wordNet): base(wordNet){
        }

        /// <summary>
        /// Computes wordnet similarity metric based on similarity path between two synsets.
        /// </summary>
        /// <param name="synSet1">First synset</param>
        /// <param name="synSet2">Second synset</param>
        /// <returns>Wordnet similarity metric based on similarity path between two synsets.</returns>
        public override double ComputeSimilarity(SynSet synSet1, SynSet synSet2) {
            // Find path to root of both elements. Percolating up until root is necessary since depth is necessary to compute the score.
            var pathToRootOfSynSet1 = wordNet.FindPathToRoot(synSet1);
            var pathToRootOfSynSet2 = wordNet.FindPathToRoot(synSet2);
            // Find path length
            var pathLength = wordNet.FindPathLength(pathToRootOfSynSet1, pathToRootOfSynSet2);
            var maxDepth = System.Math.Max(pathToRootOfSynSet1.Count, pathToRootOfSynSet2.Count);
            return 2 * maxDepth - pathLength;
        }
        
    }
}