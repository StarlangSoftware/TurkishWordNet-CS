using DataStructure;
using Dictionary.Dictionary;
using NUnit.Framework;

namespace Test
{
    public class WordNetTest
    {
        WordNet.WordNet turkish;

        [SetUp]
        public void Setup()
        {
            turkish = new WordNet.WordNet();
        }

        [Test]
        public void TestSynSetList()
        {
            var literalCount = 0;
            foreach (var synSet in turkish.SynSetList()){
                literalCount += synSet.GetSynonym().LiteralSize();
            }
            Assert.AreEqual(110258, literalCount);
        }

        [Test]
        public void TestWikiPages()
        {
            int count = 0;
            foreach (var synSet in turkish.SynSetList()){
                if (synSet.GetWikiPage() != null){
                    count++;
                }
            }
            Assert.AreEqual(11001, count);
        }

        [Test]
        public void TestLiteralList()
        {
            Assert.AreEqual(82275, turkish.LiteralList().Count);
        }

        [Test]
        public void TestTotalForeignLiterals()
        {
            var count = 0;
            foreach (var synSet in turkish.SynSetList()){
                for (var i = 0; i < synSet.GetSynonym().LiteralSize(); i++){
                    if (synSet.GetSynonym().GetLiteral(i).GetOrigin() != null){
                        count++;
                    }
                }
            }
            Assert.AreEqual(3981, count);
        }

        [Test]
        public void TestTotalGroupedLiterals()
        {
            var count = 0;
            foreach (var synSet in turkish.SynSetList()){
                for (var i = 0; i < synSet.GetSynonym().LiteralSize(); i++){
                    if (synSet.GetSynonym().GetLiteral(i).GetGroupNo() != 0){
                        count++;
                    }
                }
            }
            Assert.AreEqual(5973, count);
        }

        [Test]
        public void TestGroupSize()
        {
            var groups = new CounterHashMap<int>();
            foreach (var synSet in turkish.SynSetList()){
                var literalGroups = synSet.GetSynonym().GetUniqueLiterals();
                foreach (var synonym in literalGroups){
                    if (synonym.GetLiteral(0).GetGroupNo() != 0){
                        groups.Put(synonym.LiteralSize());
                    }
                }
            }
            Assert.AreEqual(0, groups.Count(1));
            Assert.AreEqual(2949, groups.Count(2));
            Assert.AreEqual(21, groups.Count(3));
            Assert.AreEqual(3, groups.Count(4));
        }

        [Test]
        public void TestGetSynSetWithId()
        {
            Assert.NotNull(turkish.GetSynSetWithId("TUR10-0000040"));
            Assert.NotNull(turkish.GetSynSetWithId("TUR10-0648550"));
            Assert.NotNull(turkish.GetSynSetWithId("TUR10-1034170"));
            Assert.NotNull(turkish.GetSynSetWithId("TUR10-1047180"));
            Assert.NotNull(turkish.GetSynSetWithId("TUR10-1196250"));
        }

        [Test]

        public void TestGetSynSetWithLiteral()
        {
            Assert.NotNull(turkish.GetSynSetWithLiteral("sıradaki", 1));
            Assert.NotNull(turkish.GetSynSetWithLiteral("Türkçesi", 2));
            Assert.NotNull(turkish.GetSynSetWithLiteral("tropikal orman", 1));
            Assert.NotNull(turkish.GetSynSetWithLiteral("mesut olmak", 1));
            Assert.NotNull(turkish.GetSynSetWithLiteral("acı badem kurabiyesi", 1));
            Assert.NotNull(turkish.GetSynSetWithLiteral("açık kapı siyaseti", 1));
            Assert.NotNull(turkish.GetSynSetWithLiteral("bir baştan bir başa", 1));
            Assert.NotNull(turkish.GetSynSetWithLiteral("eş zamanlı dil bilimi", 1));
            Assert.NotNull(turkish.GetSynSetWithLiteral("bir iğne bir iplik olmak", 1));
            Assert.NotNull(turkish.GetSynSetWithLiteral("yedi kat yerin dibine geçmek", 2));
            Assert.NotNull(turkish.GetSynSetWithLiteral("kedi gibi dört ayak üzerine düşmek", 1));
            Assert.NotNull(turkish.GetSynSetWithLiteral("bir kulağından girip öbür kulağından çıkmak", 1));
            Assert.NotNull(turkish.GetSynSetWithLiteral("anasından emdiği süt burnundan fitil fitil gelmek", 1));
            Assert.NotNull(turkish.GetSynSetWithLiteral("bir ayak üstünde kırk yalanın belini bükmek", 1));
        }

        [Test]

        public void TestNumberOfSynSetsWithLiteral()
        {
            Assert.AreEqual(1, turkish.NumberOfSynSetsWithLiteral("yolcu etmek"));
            Assert.AreEqual(2, turkish.NumberOfSynSetsWithLiteral("açık pembe"));
            Assert.AreEqual(3, turkish.NumberOfSynSetsWithLiteral("bürokrasi"));
            Assert.AreEqual(4, turkish.NumberOfSynSetsWithLiteral("bordür"));
            Assert.AreEqual(5, turkish.NumberOfSynSetsWithLiteral("duygulanım"));
            Assert.AreEqual(6, turkish.NumberOfSynSetsWithLiteral("sarsıntı"));
            Assert.AreEqual(7, turkish.NumberOfSynSetsWithLiteral("kuvvetli"));
            Assert.AreEqual(8, turkish.NumberOfSynSetsWithLiteral("merkez"));
            Assert.AreEqual(9, turkish.NumberOfSynSetsWithLiteral("yüksek"));
            Assert.AreEqual(10, turkish.NumberOfSynSetsWithLiteral("biçim"));
            Assert.AreEqual(11, turkish.NumberOfSynSetsWithLiteral("yurt"));
            Assert.AreEqual(12, turkish.NumberOfSynSetsWithLiteral("iğne"));
            Assert.AreEqual(13, turkish.NumberOfSynSetsWithLiteral("kol"));
            Assert.AreEqual(14, turkish.NumberOfSynSetsWithLiteral("alem"));
            Assert.AreEqual(15, turkish.NumberOfSynSetsWithLiteral("taban"));
            Assert.AreEqual(16, turkish.NumberOfSynSetsWithLiteral("yer"));
            Assert.AreEqual(17, turkish.NumberOfSynSetsWithLiteral("ağır"));
            Assert.AreEqual(18, turkish.NumberOfSynSetsWithLiteral("iş"));
            Assert.AreEqual(19, turkish.NumberOfSynSetsWithLiteral("dökmek"));
            Assert.AreEqual(20, turkish.NumberOfSynSetsWithLiteral("kaldırmak"));
            Assert.AreEqual(21, turkish.NumberOfSynSetsWithLiteral("girmek"));
            Assert.AreEqual(22, turkish.NumberOfSynSetsWithLiteral("gitmek"));
            Assert.AreEqual(23, turkish.NumberOfSynSetsWithLiteral("vermek"));
            Assert.AreEqual(24, turkish.NumberOfSynSetsWithLiteral("olmak"));
            Assert.AreEqual(25, turkish.NumberOfSynSetsWithLiteral("bırakmak"));
            Assert.AreEqual(26, turkish.NumberOfSynSetsWithLiteral("çıkarmak"));
            Assert.AreEqual(27, turkish.NumberOfSynSetsWithLiteral("kesmek"));
            Assert.AreEqual(28, turkish.NumberOfSynSetsWithLiteral("açmak"));
            Assert.AreEqual(33, turkish.NumberOfSynSetsWithLiteral("düşmek"));
            Assert.AreEqual(38, turkish.NumberOfSynSetsWithLiteral("atmak"));
            Assert.AreEqual(39, turkish.NumberOfSynSetsWithLiteral("geçmek"));
            Assert.AreEqual(44, turkish.NumberOfSynSetsWithLiteral("çekmek"));
            Assert.AreEqual(50, turkish.NumberOfSynSetsWithLiteral("tutmak"));
            Assert.AreEqual(59, turkish.NumberOfSynSetsWithLiteral("çıkmak"));
        }

        [Test]

        public void TestGetSynSetsWithPartOfSpeech()
        {
            Assert.AreEqual(43884, turkish.GetSynSetsWithPartOfSpeech(Pos.NOUN).Count);
            Assert.AreEqual(17772, turkish.GetSynSetsWithPartOfSpeech(Pos.VERB).Count);
            Assert.AreEqual(12410, turkish.GetSynSetsWithPartOfSpeech(Pos.ADJECTIVE).Count);
            Assert.AreEqual(2549, turkish.GetSynSetsWithPartOfSpeech(Pos.ADVERB).Count);
            Assert.AreEqual(1552, turkish.GetSynSetsWithPartOfSpeech(Pos.INTERJECTION).Count);
            Assert.AreEqual(68, turkish.GetSynSetsWithPartOfSpeech(Pos.PRONOUN).Count);
            Assert.AreEqual(61, turkish.GetSynSetsWithPartOfSpeech(Pos.CONJUNCTION).Count);
            Assert.AreEqual(30, turkish.GetSynSetsWithPartOfSpeech(Pos.PREPOSITION).Count);
        }

        [Test]

        public void TestGetLiteralsWithPossibleModifiedLiteral()
        {
            var english = new WordNet.WordNet("english_wordnet_version_31.xml");
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("went").Contains("go"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("going").Contains("go"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("gone").Contains("go"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("was").Contains("be"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("were").Contains("be"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("been").Contains("be"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("had").Contains("have"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("played").Contains("play"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("plays").Contains("play"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("oranges").Contains("orange"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("better").Contains("good"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("better").Contains("well"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("best").Contains("good"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("best").Contains("well"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("worse").Contains("bad"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("worst").Contains("bad"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("uglier").Contains("ugly"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("ugliest").Contains("ugly"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("buses").Contains("bus"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("flies").Contains("fly"));
            Assert.True(english.GetLiteralsWithPossibleModifiedLiteral("leaves").Contains("leaf"));
        }

        [Test]

        public void TestGetInterlingual()
        {
            Assert.AreEqual(1, turkish.GetInterlingual("ENG31-05674544-n").Count);
            Assert.AreEqual(2, turkish.GetInterlingual("ENG31-00220161-r").Count);
            Assert.AreEqual(3, turkish.GetInterlingual("ENG31-02294200-v").Count);
            Assert.AreEqual(4, turkish.GetInterlingual("ENG31-06205574-n").Count);
            Assert.AreEqual(5, turkish.GetInterlingual("ENG31-02687605-v").Count);
            Assert.AreEqual(6, turkish.GetInterlingual("ENG31-01099197-n").Count);
            Assert.AreEqual(7, turkish.GetInterlingual("ENG31-00587299-n").Count);
            Assert.AreEqual(9, turkish.GetInterlingual("ENG31-02214901-v").Count);
            Assert.AreEqual(10, turkish.GetInterlingual("ENG31-02733337-v").Count);
            Assert.AreEqual(19, turkish.GetInterlingual("ENG31-00149403-v").Count);
        }

        [Test]

        public void TestSize()
        {
            Assert.AreEqual(78326, turkish.Size());
        }

        [Test]

        public void TestFindPathToRoot()
        {
            Assert.AreEqual(1, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0814560")).Count);
            Assert.AreEqual(2, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0755370")).Count);
            Assert.AreEqual(3, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0516010")).Count);
            Assert.AreEqual(4, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0012910")).Count);
            Assert.AreEqual(5, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0046370")).Count);
            Assert.AreEqual(6, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0186560")).Count);
            Assert.AreEqual(7, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0172740")).Count);
            Assert.AreEqual(8, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0195110")).Count);
            Assert.AreEqual(9, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0285060")).Count);
            Assert.AreEqual(10, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0066050")).Count);
            Assert.AreEqual(11, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0226380")).Count);
            Assert.AreEqual(12, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0490230")).Count);
            Assert.AreEqual(13, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-1198750")).Count);
            Assert.AreEqual(12, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0412120")).Count);
            Assert.AreEqual(13, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-1116690")).Count);
            Assert.AreEqual(13, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0621870")).Count);
            Assert.AreEqual(14, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0822980")).Count);
            Assert.AreEqual(15, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0178450")).Count);
            Assert.AreEqual(16, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0600460")).Count);
            Assert.AreEqual(17, turkish.FindPathToRoot(turkish.GetSynSetWithId("TUR10-0656390")).Count);
        }
    }
}