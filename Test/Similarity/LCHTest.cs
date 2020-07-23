using NUnit.Framework;
using WordNet.Similarity;

namespace Test.Similarity
{
    public class LchTest
    {
        [Test]
        public void TestComputeSimilarity()
        {
            var turkish = new WordNet.WordNet();
            var lch = new LCH(turkish);
            Assert.AreEqual(2.9957,
                lch.ComputeSimilarity(turkish.GetSynSetWithId("TUR10-0656390"),
                    turkish.GetSynSetWithId("TUR10-0600460")), 0.0001);
            Assert.AreEqual(0.2623,
                lch.ComputeSimilarity(turkish.GetSynSetWithId("TUR10-0066050"),
                    turkish.GetSynSetWithId("TUR10-1198750")), 0.0001);
            Assert.AreEqual(0.5596,
                lch.ComputeSimilarity(turkish.GetSynSetWithId("TUR10-0012910"),
                    turkish.GetSynSetWithId("TUR10-0172740")), 0.0001);
            Assert.AreEqual(0.7673,
                lch.ComputeSimilarity(turkish.GetSynSetWithId("TUR10-0412120"),
                    turkish.GetSynSetWithId("TUR10-0755370")), 0.0001);
            Assert.AreEqual(0.6360,
                lch.ComputeSimilarity(turkish.GetSynSetWithId("TUR10-0195110"),
                    turkish.GetSynSetWithId("TUR10-0822980")), 0.0001);
        }
    }
}