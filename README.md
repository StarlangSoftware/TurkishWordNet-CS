For Developers
============

You can also see [Java](https://github.com/starlangsoftware/TurkishWordNet), [Python](https://github.com/starlangsoftware/TurkishWordNet-Py), or [C++](https://github.com/starlangsoftware/TurkishWordNet-CPP) repository.

Detailed Description
============

+ [WordNet](#wordnet)
+ [SynSet](#synset)
+ [Synonym](#synonym)

## WordNet

To load the WordNet KeNet,

	WordNet a = new WordNet();

To load a particular WordNet,

	WordNet domain = new WordNet("domain_wordnet.xml", new Locale("tr"));

To bring all the synsets,

	List<SynSet> SynSetList()

To bring a particular synset,

	SynSet GetSynSetWithId(string synSetId)

And, to bring all the meanings (Synsets) of a particular word, the following is used.

	List<SynSet> GetSynSetsWithLiteral(string literal)

## SynSet

Synonym is procured in order to find the synonymous literals of a synset.

	Synonym GetSynonym()
	
In order to obtain the Relations inside a synset as index based, the following method is used.

	Relation GetRelation(int index)

For instance, all the relations in a synset can be found with the following method.



	for (int i = 0; i < synset.RelationSize(); i++){
		relation = synset.GetRelation(i);
		...
	}

## Synonym

The literals inside the Synonym can be found as index based with the following method.

	Literal GetLiteral(int index)

For example, all the literals inside a synonym can be found with the following:

	for (int i = 0; i < synonym.LiteralSize(); i++){
		literal = synonym.GetLiteral(i);
		...
	}
