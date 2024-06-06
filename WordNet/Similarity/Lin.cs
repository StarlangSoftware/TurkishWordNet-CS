using System.Collections.Generic;

namespace WordNet.Similarity
{
    public class Lin : ICSimilarity
    {
        /// <summary>
        /// Class constructor to set the wordnet and the information content hash map.
        /// </summary>
        /// <param name="wordNet">WordNet for which similarity metrics will be calculated.</param>
        /// <param name="informationContents">Information content hash map.</param>
        public Lin(WordNet wordNet, Dictionary<string, double> informationContents) : base(wordNet, informationContents)
        {
        }

        /// <summary>
        /// Computes Lin wordnet similarity metric between two synsets.
        /// </summary>
        /// <param name="synSet1">First synset</param>
        /// <param name="synSet2">Second synset</param>
        /// <returns>Lin wordnet similarity metric between two synsets</returns>
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