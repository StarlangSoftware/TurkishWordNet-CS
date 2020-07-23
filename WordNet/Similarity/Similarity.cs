namespace WordNet.Similarity
{
    public abstract class Similarity
    {
        protected WordNet wordNet;
        public abstract double ComputeSimilarity(SynSet synSet1, SynSet synSet2);

        public Similarity(WordNet wordNet){
            this.wordNet = wordNet;
        }
        
    }
}