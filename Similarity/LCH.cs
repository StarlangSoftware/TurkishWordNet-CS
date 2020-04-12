namespace WordNet.Similarity
{
    public class LCH : Similarity
    {
        public LCH(WordNet wordNet) : base(wordNet)
        {
        }

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