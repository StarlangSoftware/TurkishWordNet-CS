using System.Collections.Generic;

namespace WordNet.Similarity
{
    public abstract class ICSimilarity : Similarity
    {
        protected Dictionary<string, double> informationContents;

        public ICSimilarity(WordNet wordNet, Dictionary<string, double> informationContents) : base(wordNet)
        {
            this.informationContents = informationContents;
        }
    }
}