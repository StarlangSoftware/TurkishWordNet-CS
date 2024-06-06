namespace WordNet.Similarity
{
    
    public class WuPalmer : Similarity
    {
        /// <summary>
        /// Class constructor that sets the wordnet and the information content hash map.
        /// </summary>
        /// <param name="wordNet">WordNet for which similarity metrics will be calculated.</param>
        public WuPalmer(WordNet wordNet) : base(wordNet){
        }

        /// <summary>
        /// Computes Wu-Palmer wordnet similarity metric between two synsets.
        /// </summary>
        /// <param name="synSet1">First synset</param>
        /// <param name="synSet2">Second synset</param>
        /// <returns>Wu-Palmer wordnet similarity metric between two synsets</returns>
        public override double ComputeSimilarity(SynSet synSet1, SynSet synSet2) {
            var pathToRootOfSynSet1 = wordNet.FindPathToRoot(synSet1);
            var pathToRootOfSynSet2 = wordNet.FindPathToRoot(synSet2);
            float lcsDepth = wordNet.FindLCSdepth(pathToRootOfSynSet1, pathToRootOfSynSet2);
            return 2 * lcsDepth / (pathToRootOfSynSet1.Count + pathToRootOfSynSet2.Count);
        }
        
    }
}