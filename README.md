# TurkishWordNet-CS

A WordNet is a graph data structure where the nodes are word senses with their associated lemmas (and collocations in the case of multiword expressions (MWEs)) and edges are semantic relations between the sense pairs. Usually, the multiple senses corresponding to a single lemma are enumerated and are referenced as such. For example, the triple
􏰀

w<sup>5</sup><sub>2</sub>,w<sup>7</sup><sub>3</sub>,r<sub>1</sub>

represents an edge in the WordNet graph and corresponds to a semantic relation r<sub>1</sub> between the second sense of the lemma w<sup>5</sup> and the third sense of the lemma w<sup>7</sup>. The direction of the relation is usually implicit in the ordering of the elements of the triple. For synonymy, the direction is symmetric. For hypernymy, as a convention, the first sense is an hyponym of the second.

The main lexical source for KeNet is the Contemporary Dictionary of Turkish (CDT) (Güncel Türkçe Sözlük) published online and in paper by the Turkish Language Institute (TLI) (Türk Dil Kurumu), a government organization. Among other literary and academic works, the TLI publishes specialized and comprehensive dictionaries. These dictionaries are often taken as an authoritative reference by other dictionaries. The online version of the CDT contains 65,944 lemmas. Although the TLI publishes a separate dictionary of idioms and proverbs, the CDT still contains some MWE entries that have idiomatic senses.

## Data Format

The structure of a sample synset is as follows:

	<SYNSET>
		<ID>TUR10-0038510</ID>
		<LITERAL>anne<SENSE>2</SENSE>
		</LITERAL>
		<POS>n</POS>
		<DEF>...</DEF>
		<EXAMPLE>...</EXAMPLE>
	</SYNSET>

Each entry in the dictionary is enclosed by <SYNSET> and </SYNSET> tags. Synset members are represented as literals and their sense numbers. <ID> shows the unique identifier given to the synset. <POS> and <DEF> tags denote part of speech and definition, respectively. As for the <EXAMPLE> tag, it gives a sample sentence for the synset.

------------------------------------------------

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

	Collection<SynSet> synSetList()

Belirli bir synseti getirmek için

	SynSet getSynSetWithId(String synSetId)

Belirli bir kelimenin tüm anlamlarını (Synsetlerini) getirmek için

	ArrayList<SynSet> getSynSetsWithLiteral(String literal)

## SynSet

Bir synsetin eş anlamlı literallerini bulmak için Synonym elde edilir.

	Synonym getSynonym()
	
Bir synsetin içindeki Relation'ları indeks bazlı elde etmek için

	Relation getRelation(int index)

metodu ile bulunur. Örneğin, bir synsetin içindeki tüm ilişkiler

	for (int i = 0; i < synset.relationSize(); i++){
		relation = synset.getRelation(i);
		...
	}

## Synonym

Synonym'in içindeki literaller indeks bazlı

	Literal getLiteral(int index)

metodu ile bulunur. Örneğin, bir synonym içindeki tüm literaller

	for (int i = 0; i < synonym.literalSize(); i++){
		literal = synonym.getLiteral(i);
		...
	}

## Cite
If you use this resource on your research, please cite the following paper: 

```
@article{ehsani18,
  title={Constructing a WordNet for {T}urkish Using Manual and Automatic Annotation},
  author={R. Ehsani and E. Solak and O.T. Yildiz},
  journal={ACM Transactions on Asian and Low-Resource Language Information Processing (TALLIP)},
  volume={17},
  number={3},
  pages={24},
  year={2018},
  publisher={ACM}
}

@inproceedings{bakay19b,
  title={Integrating {T}urkish {W}ord{N}et {K}e{N}et to {P}rinceton {W}ord{N}et: The Case of One-to-Many Correspondences},
  author={Ozge Bakay and Ozlem Ergelen and Olcay Taner Yildiz},
  booktitle={Innovations in Intelligent Systems and Applications},
  year={2019}
}

@inproceedings{bakay19a,
  title={Problems Caused by Semantic Drift in WordNet SynSet Construction},
  author={Ozge Bakay and Ozlem Ergelen and Olcay Taner Yildiz},
  booktitle={International Conference on Computer Science and Engineering},
  year={2019}
}

@inproceedings{ozcelik19,
  title={User Interface for {T}urkish Word Network {K}e{N}et},
  author={Riza Ozcelik and Selen Parlar and Ozge Bakay and Ozlem Ergelen and Olcay Taner Yildiz},
  booktitle={Signal Processing and Communication Applications Conference},
  year={2019}
}
