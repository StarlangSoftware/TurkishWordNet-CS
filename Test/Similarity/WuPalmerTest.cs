using NUnit.Framework;
using WordNet.Similarity;

namespace Test.Similarity
{
    public class WuPalmerTest
    {
        [Test]
        public void TestComputeSimilarity()
        {
            var turkish = new WordNet.WordNet();
            var lch = new WuPalmer(turkish);
            Assert.AreEqual(0.9697,
                lch.ComputeSimilarity(turkish.GetSynSetWithId("TUR10-0656390"),
                    turkish.GetSynSetWithId("TUR10-0600460")), 0.0001);
            Assert.AreEqual(0.2857,
                lch.ComputeSimilarity(turkish.GetSynSetWithId("TUR10-0412120"),
                    turkish.GetSynSetWithId("TUR10-0755370")), 0.0001);
            Assert.AreEqual(0.3636,
                lch.ComputeSimilarity(turkish.GetSynSetWithId("TUR10-0195110"),
                    turkish.GetSynSetWithId("TUR10-0822980")), 0.0001);
        }
    }
}