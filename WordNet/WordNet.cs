using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using Dictionary.Dictionary;
using MorphologicalAnalysis;

namespace WordNet
{
    public class WordNet
    {
        private SortedDictionary<string, SynSet> _synSetList;
        private SortedDictionary<string, List<Literal>> _literalList;
        private readonly CultureInfo _locale;
        private readonly Dictionary<string, List<ExceptionalWord>> _exceptionList;
        private Dictionary<string, List<SynSet>> _interlingualList;

        private void LoadWordNet(Stream stream)
        {
            var doc = new XmlDocument();
            doc.Load(stream);
            SynSet currentSynSet = null;
            _interlingualList = new Dictionary<string, List<SynSet>>();
            _synSetList = new SortedDictionary<string, SynSet>();
            _literalList = new SortedDictionary<string, List<Literal>>();
            foreach (XmlNode synSetNode in doc.DocumentElement.ChildNodes)
            {
                foreach (XmlNode partNode in synSetNode.ChildNodes)
                {
                    XmlNode typeNode;
                    XmlNode toNode;
                    switch (partNode.Name)
                    {
                        case "ID":
                            currentSynSet = new SynSet(partNode.InnerText);
                            AddSynSet(currentSynSet);
                            break;
                        case "DEF" when currentSynSet != null:
                            currentSynSet.SetDefinition(partNode.InnerText);
                            break;
                        case "WIKI" when currentSynSet != null:
                            currentSynSet.SetWikiPage(partNode.InnerText);
                            break;
                        case "EXAMPLE" when currentSynSet != null:
                            currentSynSet.SetExample(partNode.InnerText);
                            break;
                        case "BCS" when currentSynSet != null:
                            currentSynSet.SetBcs(int.Parse(partNode.InnerText));
                            break;
                        case "POS" when currentSynSet != null:
                            switch (partNode.InnerText[0])
                            {
                                case 'a':
                                    currentSynSet.SetPos(Pos.ADJECTIVE);
                                    break;
                                case 'v':
                                    currentSynSet.SetPos(Pos.VERB);
                                    break;
                                case 'b':
                                    currentSynSet.SetPos(Pos.ADVERB);
                                    break;
                                case 'n':
                                    currentSynSet.SetPos(Pos.NOUN);
                                    break;
                                case 'i':
                                    currentSynSet.SetPos(Pos.INTERJECTION);
                                    break;
                                case 'c':
                                    currentSynSet.SetPos(Pos.CONJUNCTION);
                                    break;
                                case 'p':
                                    currentSynSet.SetPos(Pos.PREPOSITION);
                                    break;
                                case 'r':
                                    currentSynSet.SetPos(Pos.PRONOUN);
                                    break;
                                default:
                                    currentSynSet.SetPos(Pos.NOUN);
                                    Console.WriteLine("Pos " + partNode.InnerText + " is not defined for SynSet " +
                                                      currentSynSet.GetId());
                                    break;
                            }

                            break;
                        case "SR" when currentSynSet != null:
                        {
                            typeNode = partNode.FirstChild.NextSibling;
                            if (typeNode != null && typeNode.Name == "TYPE")
                            {
                                toNode = typeNode.NextSibling;
                                if (toNode != null && toNode.Name == "TO")
                                {
                                    currentSynSet.AddRelation(new SemanticRelation(partNode.FirstChild.InnerText,
                                        typeNode.InnerText, int.Parse(toNode.InnerText)));
                                }
                                else
                                {
                                    currentSynSet.AddRelation(new SemanticRelation(partNode.FirstChild.InnerText,
                                        typeNode.InnerText));
                                }
                            }
                            else
                            {
                                Console.WriteLine("SR node " + partNode.InnerText + " of synSet " +
                                                  currentSynSet.GetId() + " does not contain type value");
                            }

                            break;
                        }
                        case "ILR" when currentSynSet != null:
                        {
                            typeNode = partNode.FirstChild.NextSibling;
                            if (typeNode != null && typeNode.Name == "TYPE")
                            {
                                var interlingualId = partNode.FirstChild.InnerText;
                                List<SynSet> synSetList;
                                if (_interlingualList.ContainsKey(interlingualId))
                                {
                                    synSetList = _interlingualList[interlingualId];
                                }
                                else
                                {
                                    synSetList = new List<SynSet>();
                                }

                                synSetList.Add(currentSynSet);
                                _interlingualList[interlingualId] = synSetList;
                                currentSynSet.AddRelation(new InterlingualRelation(interlingualId,
                                    typeNode.InnerText));
                            }
                            else
                            {
                                Console.WriteLine("ILR node " + partNode.InnerText + " of synSet " +
                                                  currentSynSet.GetId() + " does not contain type value");
                            }

                            break;
                        }
                        case "SYNONYM" when currentSynSet != null:
                        {
                            foreach (XmlNode literalNode in partNode.ChildNodes)
                            {
                                var senseNode = literalNode.FirstChild.NextSibling;
                                if (senseNode != null)
                                {
                                    var currentLiteral = new Literal(literalNode.FirstChild.InnerText,
                                        int.Parse(senseNode.InnerText),
                                        currentSynSet.GetId());
                                    currentSynSet.AddLiteral(currentLiteral);
                                    AddLiteralToLiteralList(currentLiteral);
                                    var srNode = senseNode.NextSibling;
                                    while (srNode != null)
                                    {
                                        if (srNode.Name == "SR")
                                        {
                                            typeNode = srNode.FirstChild.NextSibling;
                                            if (typeNode != null && typeNode.Name == "TYPE")
                                            {
                                                toNode = typeNode.NextSibling;
                                                if (toNode != null && toNode.Name == "TO")
                                                {
                                                    currentLiteral.AddRelation(
                                                        new SemanticRelation(srNode.FirstChild.InnerText, typeNode.InnerText,
                                                            int.Parse(toNode.InnerText)));
                                                }
                                                else
                                                {
                                                    currentLiteral.AddRelation(
                                                        new SemanticRelation(srNode.FirstChild.InnerText,
                                                            typeNode.InnerText));
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("SR node " + srNode.InnerText + "of literal " +
                                                                  currentLiteral.GetName() + " of synSet " +
                                                                  currentSynSet.GetId() +
                                                                  " does not contain type value");
                                            }
                                        }
                                        else
                                        {
                                            if (srNode.Name == "ORIGIN")
                                            {
                                                currentLiteral.SetOrigin(srNode.FirstChild.InnerText);
                                            }
                                        }

                                        srNode = srNode.NextSibling;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Literal Node " + partNode.InnerText + " of SynSet " +
                                                      currentSynSet.GetId() + " does not include sense node");
                                }
                            }

                            break;
                        }
                        case "SNOTE" when currentSynSet != null:
                            currentSynSet.SetNote(partNode.InnerText);
                            break;
                    }
                }
            }
        }

        /**
         * Method constructs a DOM parser using the dtd/xml schema parser configuration and using this parser it
         * reads exceptions from file and puts to exceptionList HashMap.
         *
         * @param exceptionFileName exception file to be read
         */
        public void ReadExceptionFile(string exceptionFileName)
        {
            var assembly = typeof(WordNet).Assembly;
            var stream = assembly.GetManifestResourceStream("WordNet." + exceptionFileName);
            var doc = new XmlDocument();
            doc.Load(stream);
            foreach (XmlNode wordNode in doc.DocumentElement.ChildNodes)
            {
                var wordName = wordNode.Attributes["name"].Value;
                var rootForm = wordNode.Attributes["root"].Value;
                Pos pos;
                switch (wordNode.Attributes["pos"].Value)
                {
                    case "Adj":
                        pos = Pos.ADJECTIVE;
                        break;
                    case "Adv":
                        pos = Pos.ADVERB;
                        break;
                    case "Noun":
                        pos = Pos.NOUN;
                        break;
                    case "Verb":
                        pos = Pos.VERB;
                        break;
                    default:
                        pos = Pos.NOUN;
                        break;
                }

                List<ExceptionalWord> rootList;
                if (_exceptionList.ContainsKey(wordName))
                {
                    rootList = _exceptionList[wordName];
                }
                else
                {
                    rootList = new List<ExceptionalWord>();
                }
                rootList.Add(new ExceptionalWord(wordName, rootForm, pos));
                _exceptionList[wordName] = rootList;
            }
        }

        /**
         * A constructor that initializes the SynSet list, literal list and schedules the {@code SwingWorker} for execution
         * on a <i>worker</i> thread.
         */
        public WordNet()
        {
            _synSetList = new SortedDictionary<string, SynSet>();
            _literalList = new SortedDictionary<string, List<Literal>>();
            _locale = new CultureInfo("tr");
            var assembly = typeof(WordNet).Assembly;
            var stream = assembly.GetManifestResourceStream("WordNet.turkish_wordnet.xml");
            LoadWordNet(stream);
        }

        /**
         * Another constructor that initializes the SynSet list, literal list, reads exception,
         * and schedules the {@code SwingWorker} according to file with a specified name for execution on a <i>worker</i> thread.
         *
         * @param fileName resource to be read for the WordNet task
         */
        public WordNet(string fileName)
        {
            _synSetList = new SortedDictionary<string, SynSet>();
            _literalList = new SortedDictionary<string, List<Literal>>();
            _locale = new CultureInfo("en");
            _exceptionList = new Dictionary<string, List<ExceptionalWord>>();
            ReadExceptionFile("english_exception.xml");
            var assembly = typeof(WordNet).Assembly;
            var stream = assembly.GetManifestResourceStream("WordNet." + fileName);
            LoadWordNet(stream);
        }

        /**
         * Another constructor that initializes the SynSet list, literal list, reads exception,
         * sets the Locale of the programme with the specified locale, and schedules the {@code SwingWorker} according
         * to file with a specified name for execution on a <i>worker</i> thread.
         *
         * @param fileName resource to be read for the WordNet task
         * @param locale   the locale to be used to set
         */
        public WordNet(string fileName, CultureInfo locale)
        {
            _synSetList = new SortedDictionary<string, SynSet>();
            _literalList = new SortedDictionary<string, List<Literal>>();
            this._locale = locale;
            LoadWordNet(File.Create(fileName));
        }

        /**
         * Another constructor that initializes the SynSet list, literal list, reads exception file with a specified name,
         * sets the Locale of the programme with the specified locale, and schedules the {@code SwingWorker} according
         * to file with a specified name for execution on a <i>worker</i> thread.
         *
         * @param fileName          resource to be read for the WordNet task
         * @param exceptionFileName exception file to be read
         * @param locale            the locale to be used to set
         */
        public WordNet(string fileName, string exceptionFileName, CultureInfo locale) : this(fileName, locale)
        {
            ReadExceptionFile(exceptionFileName);
        }

        /**
         * Adds a specified literal to the literal list.
         *
         * @param literal literal to be.Added
         */
        public void AddLiteralToLiteralList(Literal literal)
        {
            List<Literal> literals;
            if (_literalList.ContainsKey(literal.GetName()))
            {
                literals = _literalList[literal.GetName()];
            }
            else
            {
                literals = new List<Literal>();
            }

            literals.Add(literal);
            _literalList[literal.GetName()] = literals;
        }

        /**
         * Return CultureInfo of the programme.
         *
         * @return CultureInfo of the programme
         */
        public CultureInfo GetLocale()
        {
            return _locale;
        }

        private void UpdateAllRelationsAccordingToNewSynSet(SynSet oldSynSet, SynSet newSynSet)
        {
            foreach (var synSet in SynSetList())
            {
                for (var i = 0; i < synSet.RelationSize(); i++)
                {
                    if (synSet.GetRelation(i) is SemanticRelation)
                    {
                        if (synSet.GetRelation(i).GetName() == oldSynSet.GetId())
                        {
                            if (synSet.GetId() == newSynSet.GetId() || synSet.ContainsRelation(
                                new SemanticRelation(newSynSet.GetId(),
                                    ((SemanticRelation) synSet.GetRelation(i)).GetRelationType())))
                            {
                                synSet.RemoveRelation(synSet.GetRelation(i));
                                i--;
                            }
                            else
                            {
                                synSet.GetRelation(i).SetName(newSynSet.GetId());
                            }
                        }
                    }
                }
            }
        }

        /**
         * Method reads the specified SynSet file, gets the SynSets according to IDs in the file, and merges SynSets.
         *
         * @param synSetFile SynSet file to be read and merged
         */
        public void MergeSynSets(string synSetFile)
        {
            var infile = new StreamReader(synSetFile);
            var line = infile.ReadLine();
            while (line != null)
            {
                var synSetIds = line.Split(" ");
                var mergedOne = GetSynSetWithId(synSetIds[0]);
                if (mergedOne != null)
                {
                    for (var i = 1; i < synSetIds.Length; i++)
                    {
                        var toBeMerged = GetSynSetWithId(synSetIds[i]);
                        if (toBeMerged != null && mergedOne.GetPos().Equals(toBeMerged.GetPos()))
                        {
                            if (!ContainsSameLiteral(mergedOne, toBeMerged))
                            {
                                mergedOne.MergeSynSet(toBeMerged);
                                RemoveSynSet(toBeMerged);
                                UpdateAllRelationsAccordingToNewSynSet(toBeMerged, mergedOne);
                            }
                        }
                    }
                }

                line = infile.ReadLine();
            }

            infile.Close();
        }

        /**
         * Returns the values of the SynSet list.
         *
         * @return values of the SynSet list
         */
        public List<SynSet> SynSetList()
        {
            return new List<SynSet>(_synSetList.Values);
        }

        /**
         * Returns the keys of the literal list.
         *
         * @return keys of the literal list
         */
        public List<string> LiteralList()
        {
            return new List<string>(_literalList.Keys);
        }

        /**
         * Adds specified SynSet to the SynSet list.
         *
         * @param synSet SynSet to be.Added
         */
        public void AddSynSet(SynSet synSet)
        {
            _synSetList[synSet.GetId()] = synSet;
        }

        /**
         * Removes specified SynSet from the SynSet list.
         *
         * @param synSet SynSet to be removed
         */
        public void RemoveSynSet(SynSet synSet)
        {
            _synSetList.Remove(synSet.GetId());
        }

        /**
         * Changes ID of a specified SynSet with the specified new ID.
         *
         * @param synSet SynSet whose ID will be updated
         * @param newId  new ID
         */
        public void ChangeSynSetId(SynSet synSet, string newId)
        {
            _synSetList.Remove(synSet.GetId());
            synSet.SetId(newId);
            _synSetList[newId] = synSet;
        }

        /**
         * Returns SynSet with the specified SynSet ID.
         *
         * @param synSetId ID of the SynSet to be returned
         * @return SynSet with the specified SynSet ID
         */
        public SynSet GetSynSetWithId(string synSetId)
        {
            if (_synSetList.ContainsKey(synSetId))
            {
                return _synSetList[synSetId];
            }

            return null;
        }

        /**
         * Returns SynSet with the specified literal and sense index.
         *
         * @param literal SynSet literal
         * @param sense   SynSet's corresponding sense index
         * @return SynSet with the specified literal and sense index
         */
        public SynSet GetSynSetWithLiteral(string literal, int sense)
        {
            var literals = _literalList[literal];
            if (literals != null)
            {
                foreach (var current in literals)
                {
                    if (current.GetSense() == sense)
                    {
                        return GetSynSetWithId(current.GetSynSetId());
                    }
                }
            }

            return null;
        }

        /**
         * Returns the number of SynSets with a specified literal.
         *
         * @param literal literal to be searched in SynSets
         * @return the number of SynSets with a specified literal
         */
        public int NumberOfSynSetsWithLiteral(string literal)
        {
            if (_literalList.ContainsKey(literal))
            {
                return _literalList[literal].Count;
            }

            return 0;
        }

        /**
         * Returns a list of SynSets with a specified part of speech tag.
         *
         * @param pos part of speech tag to be searched in SynSets
         * @return a list of SynSets with a specified part of speech tag
         */
        public List<SynSet> GetSynSetsWithPartOfSpeech(Pos pos)
        {
            var result = new List<SynSet>();
            foreach (var synSet in _synSetList.Values)
            {
                if (synSet.GetPos().Equals(pos))
                {
                    result.Add(synSet);
                }
            }

            return result;
        }

        /**
         * Returns a list of literals with a specified literal string.
         *
         * @param literal literal string to be searched in literal list
         * @return a list of literals with a specified literal string
         */
        public List<Literal> GetLiteralsWithName(string literal)
        {
            if (_literalList.ContainsKey(literal))
            {
                return _literalList[literal];
            }

            return new List<Literal>();
        }

        /**
         * Finds the SynSet with specified literal string and part of speech tag and.Adds to the given SynSet list.
         *
         * @param result  SynSet list to.Add the specified SynSet
         * @param literal literal string to be searched in literal list
         * @param pos     part of speech tag to be searched in SynSets
         */
        private void AddSynSetsWithLiteralToList(List<SynSet> result, string literal, Pos pos)
        {
            foreach (var current in _literalList[literal])
            {
                var synSet = GetSynSetWithId(current.GetSynSetId());
                if (synSet != null && synSet.GetPos().Equals(pos))
                {
                    result.Add(synSet);
                }
            }
        }

        /**
         * Finds SynSets with specified literal string and.Adds to the newly created SynSet list.
         *
         * @param literal literal string to be searched in literal list
         * @return returns a list of SynSets with specified literal string
         */
        public List<SynSet> GetSynSetsWithLiteral(string literal)
        {
            var result = new List<SynSet>();
            if (_literalList.ContainsKey(literal))
            {
                foreach (var current in _literalList[literal])
                {
                    var synSet = GetSynSetWithId(current.GetSynSetId());
                    if (synSet != null)
                    {
                        result.Add(synSet);
                    }
                }
            }

            return result;
        }

        /**
         * Finds literals with specified literal string and.Adds to the newly created literal string list. Ex: cleanest - clean
         *
         * @param literal literal string to be searched in literal list
         * @return returns a list of literals with specified literal string
         */
        public List<string> GetLiteralsWithPossibleModifiedLiteral(string literal)
        {
            var result = new List<string> {literal};
            var wordWithoutLastOne = literal.Substring(0, literal.Length - 1);
            var wordWithoutLastTwo = literal.Substring(0, literal.Length - 2);

            var wordWithoutLastThree = literal.Substring(0, literal.Length - 3);
            if (_exceptionList.ContainsKey(literal))
            {
                foreach (var exceptionalWord in _exceptionList[literal])
                {
                    result.Add(exceptionalWord.GetRoot());
                } 
            }

            if (literal.EndsWith("s") && _literalList.ContainsKey(wordWithoutLastOne))
            {
                result.Add(wordWithoutLastOne);
            }

            if ((literal.EndsWith("es") || literal.EndsWith("ed") || literal.EndsWith("er")) &&
                _literalList.ContainsKey(
                    wordWithoutLastTwo))
            {
                result.Add(wordWithoutLastTwo);
            }

            if (literal.EndsWith("ed") &&
                _literalList.ContainsKey(wordWithoutLastTwo + literal[literal.Length - 3]))
            {
                result.Add(wordWithoutLastTwo + literal[literal.Length - 3]);
            }

            if ((literal.EndsWith("ed") || literal.EndsWith("er")) &&
                _literalList.ContainsKey(wordWithoutLastTwo + "e"))
            {
                result.Add(wordWithoutLastTwo + "e");
            }

            if ((literal.EndsWith("ing") || literal.EndsWith("est")) && _literalList.ContainsKey(wordWithoutLastThree))
            {
                result.Add(wordWithoutLastThree);
            }

            if (literal.EndsWith("ing") &&
                _literalList.ContainsKey(wordWithoutLastThree + literal[literal.Length - 4]))
            {
                result.Add(wordWithoutLastThree + literal[literal.Length - 4]);
            }

            if ((literal.EndsWith("ing") || literal.EndsWith("est")) &&
                _literalList.ContainsKey(wordWithoutLastThree + "e"))
            {
                result.Add(wordWithoutLastThree + "e");
            }

            if (literal.EndsWith("ies") && _literalList.ContainsKey(wordWithoutLastThree + "y"))
            {
                result.Add(wordWithoutLastThree + "y");
            }

            return result;
        }

        /**
         * Finds SynSets with specified literal string and part of speech tag, then.Adds to the newly created SynSet list. Ex: cleanest - clean
         *
         * @param literal literal string to be searched in literal list
         * @param pos     part of speech tag to be searched in SynSets
         * @return returns a list of SynSets with specified literal string and part of speech tag
         */
        public List<SynSet> GetSynSetsWithPossiblyModifiedLiteral(string literal, Pos pos)
        {
            var result = new List<SynSet>();
            var modifiedLiterals = GetLiteralsWithPossibleModifiedLiteral(literal);
            foreach (var modifiedLiteral in modifiedLiterals)
            {
                if (_literalList.ContainsKey(modifiedLiteral))
                {
                    AddSynSetsWithLiteralToList(result, modifiedLiteral, pos);
                }
            }

            return result;
        }

        /**
         * Adds the reverse relations to the SynSet.
         *
         * @param synSet           SynSet to.Add the reverse relations
         * @param semanticRelation relation whose reverse will be.Added
         */
        public void AddReverseRelation(SynSet synSet, SemanticRelation semanticRelation)
        {
            var otherSynSet = GetSynSetWithId(semanticRelation.GetName());
            if (otherSynSet != null && SemanticRelation.Reverse(semanticRelation.GetRelationType()) !=
                SemanticRelationType.NONE)
            {
                Relation otherRelation =
                    new SemanticRelation(synSet.GetId(), SemanticRelation.Reverse(semanticRelation.GetRelationType()));
                if (!otherSynSet.ContainsRelation(otherRelation))
                {
                    otherSynSet.AddRelation(otherRelation);
                }
            }
        }

        /**
         * Removes the reverse relations from the SynSet.
         *
         * @param synSet           SynSet to remove the reverse relation
         * @param semanticRelation relation whose reverse will be removed
         */
        public void RemoveReverseRelation(SynSet synSet, SemanticRelation semanticRelation)
        {
            var otherSynSet = GetSynSetWithId(semanticRelation.GetName());
            if (otherSynSet != null && SemanticRelation.Reverse(semanticRelation.GetRelationType()) != SemanticRelationType.NONE)
            {
                Relation otherRelation =
                    new SemanticRelation(synSet.GetId(), SemanticRelation.Reverse(semanticRelation.GetRelationType()));
                if (otherSynSet.ContainsRelation(otherRelation))
                {
                    otherSynSet.RemoveRelation(otherRelation);
                }
            }
        }

        /**
         * Loops through the SynSet list and.Adds the possible reverse relations.
         */
        private void EqualizeSemanticRelations()
        {
            foreach (var synSet in _synSetList.Values)
            {
                for (var i = 0; i < synSet.RelationSize(); i++)
                {
                    if (synSet.GetRelation(i) is SemanticRelation)
                    {
                        var relation = (SemanticRelation) synSet.GetRelation(i);
                        AddReverseRelation(synSet, relation);
                    }
                }
            }
        }

        /**
         * Creates a list of literals with a specified word, or possible words corresponding to morphological parse.
         *
         * @param word      literal string
         * @param parse     morphological parse to get possible words
         * @param metaParse metamorphic parse to get possible words
         * @param fsm       finite state machine morphological analyzer to be used at getting possible words
         * @return a list of literal
         */
        public List<Literal> ConstructLiterals(string word, MorphologicalParse parse, MetamorphicParse metaParse,
            FsmMorphologicalAnalyzer fsm)
        {
            var result = new List<Literal>();
            if (parse.Size() > 0)
            {
                if (!parse.IsPunctuation() && !parse.IsCardinal() && !parse.IsReal())
                {
                    var possibleWords = fsm.GetPossibleWords(parse, metaParse);
                    foreach (var possibleWord in possibleWords)
                    {
                        result = result.Union(GetLiteralsWithName(possibleWord)).ToList();
                    }
                }
                else
                {
                    result = result.Union(GetLiteralsWithName(word)).ToList();
                }
            }
            else
            {
                result = result.Union(GetLiteralsWithName(word)).ToList();
            }

            return result;
        }

        /**
         * Creates a list of SynSets with a specified word, or possible words corresponding to morphological parse.
         *
         * @param word      literal string  to get SynSets with
         * @param parse     morphological parse to get SynSets with proper literals
         * @param metaParse metamorphic parse to get possible words
         * @param fsm       finite state machine morphological analyzer to be used at getting possible words
         * @return a list of SynSets
         */
        public List<SynSet> ConstructSynSets(string word, MorphologicalParse parse, MetamorphicParse metaParse,
            FsmMorphologicalAnalyzer fsm)
        {
            var result = new List<SynSet>();
            if (parse.Size() > 0)
            {
                if (parse.IsProperNoun())
                {
                    result.Add(GetSynSetWithLiteral("(özel isim)", 1));
                }

                if (parse.IsTime())
                {
                    result.Add(GetSynSetWithLiteral("(zaman)", 1));
                }

                if (parse.IsDate())
                {
                    result.Add(GetSynSetWithLiteral("(tarih)", 1));
                }

                if (parse.IsHashTag())
                {
                    result.Add(GetSynSetWithLiteral("(hashtag)", 1));
                }

                if (parse.IsEmail())
                {
                    result.Add(GetSynSetWithLiteral("(eposta)", 1));
                }

                if (parse.IsOrdinal())
                {
                    result.Add(GetSynSetWithLiteral("(sayı sıra sıfatı)", 1));
                }

                if (parse.IsPercent())
                {
                    result.Add(GetSynSetWithLiteral("(yüzde)", 1));
                }

                if (parse.IsFraction())
                {
                    result.Add(GetSynSetWithLiteral("(kesir sayı)", 1));
                }

                if (parse.IsRange())
                {
                    result.Add(GetSynSetWithLiteral("(sayı aralığı)", 1));
                }

                if (parse.IsReal())
                {
                    result.Add(GetSynSetWithLiteral("(reel sayı)", 1));
                }

                if (!parse.IsPunctuation() && !parse.IsCardinal() && !parse.IsReal())
                {
                    var possibleWords = fsm.GetPossibleWords(parse, metaParse);
                    foreach (var possibleWord in possibleWords)
                    {
                        var synSets = GetSynSetsWithLiteral(possibleWord);
                        if (synSets.Count > 0)
                        {
                            foreach (var synSet in synSets)
                            {
                                if (parse.GetPos() == "NOUN" || parse.GetPos() == "ADVERB" ||
                                     parse.GetPos() == "VERB" || parse.GetPos() == "ADJ" ||
                                     parse.GetPos() == "CONJ")
                                {
                                    if (synSet.GetPos().Equals(Pos.NOUN))
                                    {
                                        if (parse.GetPos() == "NOUN" || parse.GetRootPos() == "NOUN")
                                        {
                                            result.Add(synSet);
                                        }
                                    }
                                    else
                                    {
                                        if (synSet.GetPos().Equals(Pos.ADVERB))
                                        {
                                            if (parse.GetPos() == "ADVERB" || parse.GetRootPos() == "ADVERB")
                                            {
                                                result.Add(synSet);
                                            }
                                        }
                                        else
                                        {
                                            if (synSet.GetPos().Equals(Pos.VERB))
                                            {
                                                if (parse.GetPos() == "VERB" || parse.GetRootPos() == "VERB")
                                                {
                                                    result.Add(synSet);
                                                }
                                            }
                                            else
                                            {
                                                if (synSet.GetPos().Equals(Pos.ADJECTIVE))
                                                {
                                                    if (parse.GetPos() == "ADJ" ||
                                                        parse.GetRootPos() == "ADJ")
                                                    {
                                                        result.Add(synSet);
                                                    }
                                                }
                                                else
                                                {
                                                    if (synSet.GetPos().Equals(Pos.CONJUNCTION))
                                                    {
                                                        if (parse.GetPos() == "CONJ" ||
                                                            parse.GetRootPos() == "CONJ")
                                                        {
                                                            result.Add(synSet);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        result.Add(synSet);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    result.Add(synSet);
                                }
                            }
                        }
                    }

                    if (result.Count == 0)
                    {
                        foreach (var possibleWord in possibleWords)
                        {
                            var synSets = GetSynSetsWithLiteral(possibleWord);
                            result = result.Union(synSets).ToList();
                        }
                    }
                }
                else
                {
                    result = result.Union(GetSynSetsWithLiteral(word)).ToList();
                }

                if (parse.IsCardinal() && result.Count == 0)
                {
                    result.Add(GetSynSetWithLiteral("(tam sayı)", 1));
                }
            }
            else
            {
                result = result.Union(GetSynSetsWithLiteral(word)).ToList();
            }

            return result;
        }

        /**
         * Returns a list of literals using 3 possible words gathered with the specified morphological parses and metamorphic parses.
         *
         * @param morphologicalParse1 morphological parse to get possible words
         * @param morphologicalParse2 morphological parse to get possible words
         * @param morphologicalParse3 morphological parse to get possible words
         * @param metaParse1          metamorphic parse to get possible words
         * @param metaParse2          metamorphic parse to get possible words
         * @param metaParse3          metamorphic parse to get possible words
         * @param fsm                 finite state machine morphological analyzer to be used at getting possible words
         * @return a list of literals
         */
        public List<Literal> ConstructIdiomLiterals(MorphologicalParse morphologicalParse1, MorphologicalParse
                morphologicalParse2, MorphologicalParse morphologicalParse3, MetamorphicParse metaParse1,
            MetamorphicParse metaParse2,
            MetamorphicParse metaParse3, FsmMorphologicalAnalyzer fsm)
        {
            var result = new List<Literal>();
            var possibleWords1 = fsm.GetPossibleWords(morphologicalParse1, metaParse1);
            var possibleWords2 = fsm.GetPossibleWords(morphologicalParse2, metaParse2);
            var possibleWords3 = fsm.GetPossibleWords(morphologicalParse3, metaParse3);
            foreach (var possibleWord1 in possibleWords1)
            {
                foreach (var possibleWord2 in possibleWords2)
                {
                    foreach (var possibleWord3 in possibleWords3)
                    {
                        result = result
                            .Union(GetLiteralsWithName(possibleWord1 + " " + possibleWord2 + " " + possibleWord3))
                            .ToList();
                    }
                }
            }

            return result;
        }

        /**
         * Returns a list of SynSets using 3 possible words gathered with the specified morphological parses and metamorphic parses.
         *
         * @param morphologicalParse1 morphological parse to get possible words
         * @param morphologicalParse2 morphological parse to get possible words
         * @param morphologicalParse3 morphological parse to get possible words
         * @param metaParse1          metamorphic parse to get possible words
         * @param metaParse2          metamorphic parse to get possible words
         * @param metaParse3          metamorphic parse to get possible words
         * @param fsm                 finite state machine morphological analyzer to be used at getting possible words
         * @return a list of SynSets
         */
        public List<SynSet> ConstructIdiomSynSets(MorphologicalParse morphologicalParse1, MorphologicalParse
                morphologicalParse2, MorphologicalParse morphologicalParse3, MetamorphicParse metaParse1,
            MetamorphicParse metaParse2,
            MetamorphicParse metaParse3, FsmMorphologicalAnalyzer fsm)
        {
            var result = new List<SynSet>();
            var possibleWords1 = fsm.GetPossibleWords(morphologicalParse1, metaParse1);
            var possibleWords2 = fsm.GetPossibleWords(morphologicalParse2, metaParse2);
            var possibleWords3 = fsm.GetPossibleWords(morphologicalParse3, metaParse3);
            foreach (var possibleWord1 in possibleWords1)
            {
                foreach (var possibleWord2 in possibleWords2)
                {
                    foreach (var possibleWord3 in possibleWords3)
                    {
                        if (NumberOfSynSetsWithLiteral(possibleWord1 + " " + possibleWord2 + " " + possibleWord3) > 0)
                        {
                            result = result.Union(
                                    GetSynSetsWithLiteral(possibleWord1 + " " + possibleWord2 + " " + possibleWord3))
                                .ToList();
                        }
                    }
                }
            }

            return result;
        }

        /**
         * Returns a list of literals using 2 possible words gathered with the specified morphological parses and metamorphic parses.
         *
         * @param morphologicalParse1 morphological parse to get possible words
         * @param morphologicalParse2 morphological parse to get possible words
         * @param metaParse1          metamorphic parse to get possible words
         * @param metaParse2          metamorphic parse to get possible words
         * @param fsm                 finite state machine morphological analyzer to be used at getting possible words
         * @return a list of literals
         */
        public List<Literal> ConstructIdiomLiterals(MorphologicalParse morphologicalParse1, MorphologicalParse
            morphologicalParse2, MetamorphicParse metaParse1, MetamorphicParse metaParse2, FsmMorphologicalAnalyzer fsm)
        {
            var result = new List<Literal>();
            var possibleWords1 = fsm.GetPossibleWords(morphologicalParse1, metaParse1);
            var possibleWords2 = fsm.GetPossibleWords(morphologicalParse2, metaParse2);
            foreach (var possibleWord1 in possibleWords1)
            {
                foreach (var possibleWord2 in possibleWords2)
                {
                    result = result.Union(GetLiteralsWithName(possibleWord1 + " " + possibleWord2)).ToList();
                }
            }

            return result;
        }

        /**
         * Returns a list of SynSets using 2 possible words gathered with the specified morphological parses and metamorphic parses.
         *
         * @param morphologicalParse1 morphological parse to get possible words
         * @param morphologicalParse2 morphological parse to get possible words
         * @param metaParse1          metamorphic parse to get possible words
         * @param metaParse2          metamorphic parse to get possible words
         * @param fsm                 finite state machine morphological analyzer to be used at getting possible words
         * @return a list of SynSets
         */
        public List<SynSet> ConstructIdiomSynSets(MorphologicalParse morphologicalParse1, MorphologicalParse
            morphologicalParse2, MetamorphicParse metaParse1, MetamorphicParse metaParse2, FsmMorphologicalAnalyzer fsm)
        {
            var result = new List<SynSet>();
            var possibleWords1 = fsm.GetPossibleWords(morphologicalParse1, metaParse1);
            var possibleWords2 = fsm.GetPossibleWords(morphologicalParse2, metaParse2);
            foreach (var possibleWord1 in possibleWords1)
            {
                foreach (var possibleWord2 in possibleWords2)
                {
                    if (NumberOfSynSetsWithLiteral(possibleWord1 + " " + possibleWord2) > 0)
                    {
                        result = result.Union(GetSynSetsWithLiteral(possibleWord1 + " " + possibleWord2)).ToList();
                    }
                }
            }

            return result;
        }

        /**
         * Sorts definitions of SynSets in SynSet list according to their.Lengths.
         */
        public void SortDefinitions()
        {
            foreach (var synSet in SynSetList())
            {
                synSet.SortDefinitions();
            }
        }

        /**
         * Returns a list of SynSets with the interlingual relations of a specified SynSet ID.
         *
         * @param synSetId SynSet ID to be searched
         * @return a list of SynSets with the interlingual relations of a specified SynSet ID
         */
        public List<SynSet> GetInterlingual(string synSetId)
        {
            if (_interlingualList.ContainsKey(synSetId))
            {
                return _interlingualList[synSetId];
            }

            return new List<SynSet>();
        }

        /**
         * Print the literals with same senses.
         */
        private void SameLiteralSameSenseCheck()
        {
            foreach (var name in _literalList.Keys)
            {
                var literals = _literalList[name];
                for (var i = 0; i < literals.Count; i++)
                {
                    for (var j = i + 1; j < literals.Count; j++)
                    {
                        if (literals[i].GetSense() == literals[j].GetSense() &&
                            literals[i].GetName() == literals[j].GetName())
                        {
                            Console.WriteLine("Literal " + name + " has same senses.");
                        }
                    }
                }
            }
        }

        private bool ContainsSameLiteral(SynSet synSet1, SynSet synSet2)
        {
            for (var i = 0; i < synSet1.GetSynonym().LiteralSize(); i++)
            {
                var literal1 = synSet1.GetSynonym().GetLiteral(i);
                for (var j = i + 1; j < synSet2.GetSynonym().LiteralSize(); j++)
                {
                    var literal2 = synSet2.GetSynonym().GetLiteral(j);
                    if (literal1.GetName() == literal2.GetName())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /**
         * Prints the literals with same SynSets.
         */
        private void SameLiteralSameSynSetCheck()
        {
            foreach (var synSet in SynSetList())
            {
                if (ContainsSameLiteral(synSet, synSet))
                {
                    Console.WriteLine(synSet.GetPos() + "->" + synSet.GetSynonym() + "->" + synSet.GetDefinition());
                }
            }
        }

        /**
         * Prints the SynSets without definitions.
         */
        private void NoDefinitionCheck()
        {
            foreach (var synSet in SynSetList())
            {
                if (synSet.GetDefinition() == null)
                {
                    Console.WriteLine("SynSet " + synSet.GetId() + " has no definition " + synSet.GetSynonym());
                }
            }
        }

        /**
         * Prints SynSets without relation IDs.
         */
        private void SemanticRelationNoIdCheck()
        {
            foreach (var synSet in SynSetList())
            {
                for (var j = 0; j < synSet.RelationSize(); j++)
                {
                    var relation = synSet.GetRelation(j);
                    if (relation is SemanticRelation && GetSynSetWithId(relation.GetName()) == null)
                    {
                        synSet.RemoveRelation(relation);
                        j--;
                        Console.WriteLine("Relation " + relation.GetName() + " of Synset " + synSet.GetId() +
                                          " does not exists " +
                                          synSet.GetSynonym());
                    }
                }
            }
        }

        /**
         * Prints SynSets with same relations.
         */
        private void SameSemanticRelationCheck()
        {
            foreach (var synSet in SynSetList())
            {
                for (var j = 0; j < synSet.RelationSize(); j++)
                {
                    var relation = synSet.GetRelation(j);
                    Relation same = null;
                    for (var k = j + 1; k < synSet.RelationSize(); k++)
                    {
                        if (relation.GetName() == synSet.GetRelation(k).GetName())
                        {
                            Console.WriteLine(relation.GetName() + "--" + synSet.GetRelation(k).GetName() +
                                              " are same relation for synset " + synSet.GetId());
                            same = synSet.GetRelation(k);
                        }
                    }

                    if (same != null)
                    {
                        synSet.RemoveRelation(same);
                        j--;
                    }
                }
            }
        }

        /**
         * Performs check processes.
         */
        public void Check()
        {
            SemanticRelationNoIdCheck();
            EqualizeSemanticRelations();
            SameSemanticRelationCheck();
        }

        /**
         * Method to write SynSets to the specified file in the XML format.
         *
         * @param fileName file name to write XML files
         */
        public void SaveAsXml(string fileName)
        {
            var streamWriter = new StreamWriter(fileName);
            streamWriter.WriteLine("<SYNSETS>");
            foreach (var synSet in _synSetList.Values) {
                synSet.SaveAsXml(streamWriter);
            }
            streamWriter.WriteLine("</SYNSETS>");
            streamWriter.Close();
        }

        /**
         * Returns the size of the SynSet list.
         *
         * @return the size of the SynSet list
         */
        public int Size()
        {
            return _synSetList.Count;
        }

        /**
         * Conduct common operations between similarity metrics.
         *
         * @param pathToRootOfSynSet1 first list of strings
         * @param pathToRootOfSynSet2 second list of strings
         * @return path.Length
         */
        public int FindPathLength(List<string> pathToRootOfSynSet1, List<string> pathToRootOfSynSet2)
        {
            // There might not be a path between nodes, due to missing nodes. Keep track of that as well. Break when the LCS if found.
            for (var i = 0; i < pathToRootOfSynSet1.Count; i++)
            {
                var foundIndex = pathToRootOfSynSet2.IndexOf(pathToRootOfSynSet1[i]);
                if (foundIndex != -1)
                {
                    // Index of two lists - 1 is equal to path.Length. If there is not path, return -1
                    return i + foundIndex - 1;
                }
            }

            return -1;
        }

        /**
         * Returns the depth of path.
         *
         * @param pathToRootOfSynSet1 first list of strings
         * @param pathToRootOfSynSet2 second list of strings
         * @return LCS depth
         */
        public int FindLCSdepth(List<string> pathToRootOfSynSet1, List<string> pathToRootOfSynSet2)
        {
            var temp = FindLCS(pathToRootOfSynSet1, pathToRootOfSynSet2);
            if (temp != null)
            {
                return temp.Item2;
            }

            return -1;
        }

        /**
         * Returns the ID of LCS of path.
         *
         * @param pathToRootOfSynSet1 first list of strings
         * @param pathToRootOfSynSet2 second list of strings
         * @return LCS ID
         */
        public string FindLCSid(List<string> pathToRootOfSynSet1, List<string> pathToRootOfSynSet2)
        {
            var temp = FindLCS(pathToRootOfSynSet1, pathToRootOfSynSet2);
            if (temp != null)
            {
                return temp.Item1;
            }

            return null;
        }

        /**
         * Returns depth and ID of the LCS.
         *
         * @param pathToRootOfSynSet1 first list of strings
         * @param pathToRootOfSynSet2 second list of strings
         * @return depth and ID of the LCS
         */
        private Tuple<string, int> FindLCS(List<string> pathToRootOfSynSet1, List<string>
            pathToRootOfSynSet2)
        {
            for (var i = 0; i < pathToRootOfSynSet1.Count; i++)
            {
                var lcSid = pathToRootOfSynSet1[i];
                if (pathToRootOfSynSet2.Contains(lcSid))
                {
                    return new Tuple<string, int>(lcSid, pathToRootOfSynSet1.Count - i + 1);
                }
            }

            return null;
        }

        /**
         * Finds the path to the root node of a SynSets.
         *
         * @param synSet SynSet whose root path will be found
         * @return list of string corresponding to nodes in the path
         */
        public List<string> FindPathToRoot(SynSet synSet)
        {
            var pathToRoot = new List<string>();
            while (synSet != null)
            {
                if (pathToRoot.Contains(synSet.GetId()))
                {
                    break;
                }

                pathToRoot.Add(synSet.GetId());
                synSet = PercolateUp(synSet);
            }

            return pathToRoot;
        }

        /**
         * Finds the parent of a node. It does not move until the root, instead it goes one level up.
         *
         * @param root SynSet whose parent will be find
         * @return parent SynSet
         */
        public SynSet PercolateUp(SynSet root)
        {
            for (var i = 0; i < root.RelationSize(); i++)
            {
                var r = root.GetRelation(i);
                if (r is SemanticRelation semanticRelation) {
                    if (semanticRelation.GetRelationType().Equals(SemanticRelationType.HYPERNYM) ||
                        semanticRelation
                        .GetRelationType().Equals(SemanticRelationType.INSTANCE_HYPERNYM))
                    {
                        root = GetSynSetWithId(semanticRelation.GetName());
                        // return even if one hypernym is found.
                        return root;
                    }
                }
            }

            return null;
        }
    }
}