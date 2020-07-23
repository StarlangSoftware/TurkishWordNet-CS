using System.Collections.Generic;

namespace WordNet.Similarity
{
    public class Lin : ICSimilarity
    {
        public Lin(WordNet wordNet, Dictionary<string, double> informationContents) : base(wordNet, informationContents)
        {
        }

        public override double ComputeSimilarity(SynSet synSet1, SynSet synSet2)
        {
            var pathToRootOfSynSet1 = wordNet.FindPathToRoot(synSet1);
            var pathToRootOfSynSet2 = wordNet.FindPathToRoot(synSet2);
            var lcSid = wordNet.FindLCSid(pathToRootOfSynSet1, pathToRootOfSynSet2);
            return (2 * informationContents[lcSid])
                   / (informationContents[synSet1.GetId()] + informationContents[synSet2.GetId()]);
        }
    }
}