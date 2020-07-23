using System.Collections.Generic;

namespace WordNet.Similarity
{
    public class Resnik : ICSimilarity
    {
        public Resnik(WordNet wordNet, Dictionary<string, double> informationContents) : base(wordNet,
            informationContents)
        {
        }

        public override double ComputeSimilarity(SynSet synSet1, SynSet synSet2)
        {
            var pathToRootOfSynSet1 = wordNet.FindPathToRoot(synSet1);
            var pathToRootOfSynSet2 = wordNet.FindPathToRoot(synSet2);
            var LCSid = wordNet.FindLCSid(pathToRootOfSynSet1, pathToRootOfSynSet2);
            return informationContents[LCSid];
        }
    }
}