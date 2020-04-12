namespace WordNet.Similarity
{
    public class WuPalmer : Similarity
    {
        public WuPalmer(WordNet wordNet) : base(wordNet){
        }

        public override double ComputeSimilarity(SynSet synSet1, SynSet synSet2) {
            var pathToRootOfSynSet1 = wordNet.FindPathToRoot(synSet1);
            var pathToRootOfSynSet2 = wordNet.FindPathToRoot(synSet2);
            float lcsDepth = wordNet.FindLCSdepth(pathToRootOfSynSet1, pathToRootOfSynSet2);
            return 2 * lcsDepth / (pathToRootOfSynSet1.Count + pathToRootOfSynSet2.Count);
        }
        
    }
}