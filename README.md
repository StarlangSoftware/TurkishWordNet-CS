For Developers
============

You can also see [Java](https://github.com/starlangsoftware/TurkishWordNet), [Python](https://github.com/starlangsoftware/TurkishWordNet-Py), or [C++](https://github.com/starlangsoftware/TurkishWordNet-CPP) repository.

Detailed Description
============
+ [WordNet](#wordnet)
+ [SynSet](#synset)
+ [Synonym](#synonym)

## WordNet

Türkçe WordNet KeNet'i yüklemek için

	WordNet a = new WordNet();

Belirli bir WordNet'i yüklemek için

	WordNet domain = new WordNet("domain_wordnet.xml", new Locale("tr"));

Tüm synsetleri getirmek için

	List<SynSet> SynSetList()

Belirli bir synseti getirmek için

	SynSet GetSynSetWithId(string synSetId)

Belirli bir kelimenin tüm anlamlarını (Synsetlerini) getirmek için

	List<SynSet> GetSynSetsWithLiteral(string literal)

## SynSet

Bir synsetin eş anlamlı literallerini bulmak için Synonym elde edilir.

	Synonym GetSynonym()
	
Bir synsetin içindeki Relation'ları indeks bazlı elde etmek için

	Relation GetRelation(int index)

metodu ile bulunur. Örneğin, bir synsetin içindeki tüm ilişkiler

	for (int i = 0; i < synset.RelationSize(); i++){
		relation = synset.GetRelation(i);
		...
	}

## Synonym

Synonym'in içindeki literaller indeks bazlı

	Literal GetLiteral(int index)

metodu ile bulunur. Örneğin, bir synonym içindeki tüm literaller

	for (int i = 0; i < synonym.LiteralSize(); i++){
		literal = synonym.GetLiteral(i);
		...
	}

