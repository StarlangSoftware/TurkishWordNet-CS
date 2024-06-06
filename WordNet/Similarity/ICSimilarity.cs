using System.Collections.Generic;

namespace WordNet.Similarity
{
    public abstract class ICSimilarity : Similarity
    {
        protected Dictionary<string, double> informationContents;

        /// <summary>
        /// Abstract class constructor to set the wordnet and the information content hash map.
        /// </summary>
        /// <param name="wordNet">WordNet for which similarity metrics will be calculated.</param>
        /// <param name="informationContents">Information content hash map.</param>
        public ICSimilarity(WordNet wordNet, Dictionary<string, double> informationContents) : base(wordNet)
        {
            this.informationContents = informationContents;
        }
    }
}