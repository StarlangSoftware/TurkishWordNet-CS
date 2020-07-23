using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Dictionary.Dictionary;

namespace WordNet
{
    public class SynSet
    {
        private string _id;
        private Pos _pos;
        private string[] _definition;
        private string _example;
        private readonly Synonym _synonym;
        private readonly List<Relation> _relations;
        private string _note;
        private int _bcs;

        /**
         * <summary>Constructor initialize SynSet ID, synonym and relations list.</summary>
         *
         * <param name="id">Synset ID</param>
         */
        public SynSet(string id)
        {
            this._id = id;
            this._synonym = new Synonym();
            _relations = new List<Relation>();
        }

        /**
         * <summary>Accessor for the SynSet ID.</summary>
         *
         * <returns>SynSet ID</returns>
         */
        public string GetId()
        {
            return _id;
        }

        /**
         * <summary>Mutator method for the SynSet ID.</summary>
         *
         * <param name="id">SynSet ID to be set</param>
         */
        public void SetId(string id)
        {
            this._id = id;
            for (var i = 0; i < _synonym.LiteralSize(); i++)
            {
                _synonym.GetLiteral(i).SetSynSetId(id);
            }
        }

        /**
         * <summary>Mutator method for the definition.</summary>
         *
         * <param name="definition">string definition</param>
         */
        public void SetDefinition(string definition)
        {
            this._definition = definition.Split("|");
        }

        /**
         * <summary>Removes the specified definition from long definition.</summary>
         *
         * <param name="definition">definition to be removed</param>
         */
        public void RemoveDefinition(string definition)
        {
            var longDefinition = GetLongDefinition();
            if (longDefinition.StartsWith(definition + "|"))
            {
                SetDefinition(longDefinition.Replace(definition + "|", ""));
            }
            else
            {
                if (longDefinition.EndsWith("|" + definition))
                {
                    SetDefinition(longDefinition.Replace("|" + definition, ""));
                }
                else
                {
                    if (longDefinition.Contains("|" + definition + "|"))
                    {
                        SetDefinition(longDefinition.Replace("|" + definition, ""));
                    }
                }
            }
        }

        /**
         * <summary>Removes the same definitions from long definition.</summary>
         *
         * <param name="cultureInfo">Locale of the programme that will be used in converting upper/lower cases</param>
         */
        public void RemoveSameDefinitions(CultureInfo cultureInfo)
        {
            var definition = GetLongDefinition();
            var removed = true;
            while (definition != null && removed)
            {
                removed = false;
                for (var j = 0; j < GetSynonym().LiteralSize(); j++)
                {
                    var literal = GetSynonym().GetLiteral(j);
                    var word = literal.GetName().ToLower(cultureInfo);
                    var uppercaseWord = literal.GetName().Substring(0, 1).ToUpper(cultureInfo) +
                                        literal.GetName().Substring(1);
                    if (definition.Contains("|" + word + "|"))
                    {
                        definition = definition.Replace("|" + word + "|", "|");
                        removed = true;
                    }

                    if (definition.Contains("|" + word + "; "))
                    {
                        definition = definition.Replace("|" + word + "; ", "|");
                        removed = true;
                    }

                    if (definition.Contains("|" + uppercaseWord + "|"))
                    {
                        definition = definition.Replace("|" + uppercaseWord + "|", "|");
                        removed = true;
                    }

                    if (definition.Contains("|" + uppercaseWord + "; "))
                    {
                        definition = definition.Replace("|" + uppercaseWord + "; ", "|");
                        removed = true;
                    }

                    if (definition.Contains("; " + word + "|"))
                    {
                        removed = true;
                        definition = definition.Replace("; " + word + "|", "|");
                    }

                    if (definition.Contains("; " + uppercaseWord + "|"))
                    {
                        removed = true;
                        definition = definition.Replace("; " + uppercaseWord + "|", "|");
                    }

                    if (definition.EndsWith("; " + word))
                    {
                        definition = definition.Replace("; " + word, "");
                        removed = true;
                    }

                    if (definition.EndsWith("|" + word))
                    {
                        definition = definition.Replace("|" + word, "");
                        removed = true;
                    }

                    if (definition.StartsWith(word + "|"))
                    {
                        definition = definition.Replace(word + "|", "");
                        removed = true;
                    }

                    if (definition.StartsWith(uppercaseWord + "|"))
                    {
                        definition = definition.Replace(uppercaseWord + "|", "");
                        removed = true;
                    }

                    if (definition.EndsWith("; " + uppercaseWord))
                    {
                        definition = definition.Replace("; " + uppercaseWord, "");
                        removed = true;
                    }

                    if (definition.EndsWith("|" + uppercaseWord))
                    {
                        definition = definition.Replace("|" + uppercaseWord, "");
                        removed = true;
                    }

                    if (definition == word)
                    {
                        definition = "";
                        removed = true;
                    }
                }
            }

            if (!string.IsNullOrEmpty(definition))
            {
                SetDefinition(definition);
            }
            else
            {
                SetDefinition("NO DEFINITION");
            }
        }

        /**
         * <summary>Accessor for the definition.</summary>
         *
         * <returns>definition</returns>
         */
        public string GetDefinition()
        {
            return _definition?[0];
        }

        /**
         * <summary>Returns the first literal's name.</summary>
         *
         * <returns>the first literal's name.</returns>
         */
        public string Representative()
        {
            return GetSynonym().GetLiteral(0).GetName();
        }

        /**
         * <summary>Returns all the definitions in the list.</summary>
         *
         * <returns>all the definitions</returns>
         */
        public string GetLongDefinition()
        {
            if (_definition != null)
            {
                var longDefinition = _definition[0];
                for (var i = 1; i < _definition.Length; i++)
                {
                    longDefinition = longDefinition + "|" + _definition[i];
                }

                return longDefinition;
            }

            return null;
        }

        /**
         * <summary>Sorts definitions list according to their lengths.</summary>
         */
        public void SortDefinitions()
        {
            if (_definition != null)
            {
                for (var i = 0; i < _definition.Length; i++)
                {
                    for (var j = i + 1; j < _definition.Length; j++)
                    {
                        if (_definition[i].Length < _definition[j].Length)
                        {
                            var tmp = _definition[i];
                            _definition[i] = _definition[j];
                            _definition[j] = tmp;
                        }
                    }
                }
            }
        }

        /**
         * <summary>Accessor for the definition at specified index.</summary>
         *
         * <param name="index">definition index to be accessed</param>
         * <returns>definition at specified index</returns>
         */
        public string GetDefinition(int index)
        {
            if (index < _definition.Length && index >= 0)
            {
                return _definition[index];
            }

            return null;
        }

        /**
         * <summary>Returns number of definitions in the list.</summary>
         *
         * <returns>number of definitions in the list.</returns>
         */
        public int NumberOfDefinitions()
        {
            if (_definition != null)
            {
                return _definition.Length;
            }

            return 0;
        }

        /**
         * <summary>Mutator for the example.</summary>
         *
         * <param name="example">string that will be used to set</param>
         */
        public void SetExample(string example)
        {
            this._example = example;
        }

        /**
         * <summary>Accessor for the example.</summary>
         *
         * <returns>string example</returns>
         */
        public string GetExample()
        {
            return _example;
        }

        /**
         * <summary>Mutator for the bcs value which enables the connection with the BalkaNet.</summary>
         *
         * <param name="bcs">bcs value</param>
         */
        public void SetBcs(int bcs)
        {
            if (bcs >= 1 && bcs <= 3)
            {
                this._bcs = bcs;
            }
        }

        /**
         * <summary>Accessor for the bcs value</summary>
         *
         * <returns>bcs value</returns>
         */
        public int GetBcs()
        {
            return _bcs;
        }

        /**
         * <summary>Mutator for the part of speech tags.</summary>
         *
         * <param name="pos">part of speech tag</param>
         */
        public void SetPos(Pos pos)
        {
            this._pos = pos;
        }

        /**
         * <summary>Accessor for the part of speech tag.</summary>
         *
         * <returns>part of speech tag</returns>
         */
        public Pos GetPos()
        {
            return _pos;
        }

        /**
         * <summary>Mutator for the available notes.</summary>
         *
         * <param name="note">string note to be set</param>
         */
        public void SetNote(string note)
        {
            this._note = note;
        }

        /**
         * <summary>Accessor for the available notes.</summary>
         *
         * <returns>string note</returns>
         */
        public string GetNote()
        {
            return _note;
        }

        /**
         * <summary>Appends the specified Relation to the end of relations list.</summary>
         *
         * <param name="relation">element to be appended to the list</param>
         */
        public void AddRelation(Relation relation)
        {
            _relations.Add(relation);
        }

        /**
         * <summary>Removes the first occurrence of the specified element from relations list,
         * if it is present. If the list does not contain the element, it stays unchanged.</summary>
         *
         * <param name="relation">element to be removed from the list, if present</param>
         */
        public void RemoveRelation(Relation relation)
        {
            _relations.Remove(relation);
        }

        /**
         * <summary>Removes the first occurrence of the specified element from relations list according to relation name,
         * if it is present. If the list does not contain the element, it stays unchanged.</summary>
         *
         * <param name="name">element to be removed from the list, if present</param>
         */
        public void RemoveRelation(string name)
        {
            for (var i = 0; i < _relations.Count; i++)
            {
                if (_relations[i].GetName() == name)
                {
                    _relations.RemoveAt(i);
                    break;
                }
            }
        }

        /**
         * <summary>Returns the element at the specified position in relations list.</summary>
         *
         * <param name="index">index of the element to return</param>
         * <returns>the element at the specified position in the list</returns>
         */
        public Relation GetRelation(int index)
        {
            return _relations[index];
        }

        /**
         * <summary>Returns SynSets with the synonym interlingual dependencies.</summary>
         *
         * <param name="secondLanguage">WordNet in other language to find relations</param>
         * <returns>a list of SynSets that has interlingual relations in it</returns>
         */
        public List<SynSet> GetInterlingual(WordNet secondLanguage)
        {
            var result = new List<SynSet>();
            foreach (var t in _relations)
            {
                if (t is InterlingualRelation relation)
                {
                    if (relation.GetType() == InterlingualDependencyType.SYNONYM)
                    {
                        var second = secondLanguage.GetSynSetWithId(relation.GetName());
                        if (second != null)
                        {
                            result.Add(second);
                        }
                    }
                }
            }

            return result;
        }

        /**
         * <summary>Returns interlingual relations with the synonym interlingual dependencies.</summary>
         *
         * <returns>a list of SynSets that has interlingual relations in it</returns>
         */
        public List<string> GetInterlingual()
        {
            var result = new List<string>();
            foreach (var t in _relations)
            {
                if (t is InterlingualRelation relation)
                {
                    if (relation.GetType() == InterlingualDependencyType.SYNONYM)
                    {
                        result.Add(relation.GetName());
                    }
                }
            }

            return result;
        }

        /**
         * <summary>Returns the size of the relations list.</summary>
         *
         * <returns>the size of the relations list</returns>
         */
        public int RelationSize()
        {
            return _relations.Count;
        }

        /**
         * <summary>Adds a specified literal to the synonym.</summary>
         *
         * <param name="literal">literal to be added</param>
         */
        public void AddLiteral(Literal literal)
        {
            _synonym.AddLiteral(literal);
        }

        /**
         * <summary>Accessor for the synonym.</summary>
         *
         * <returns>synonym</returns>
         */
        public Synonym GetSynonym()
        {
            return _synonym;
        }

        /**
         * <summary>Compares literals of synonym and the specified SynSet, returns true if their have same literals.</summary>
         *
         * <param name="synSet">SynSet to compare</param>
         * <returns>true if SynSets have same literals, false otherwise</returns>
         */
        public bool ContainsSameLiteral(SynSet synSet)
        {
            for (var i = 0; i < _synonym.LiteralSize(); i++)
            {
                var literal1 = _synonym.GetLiteral(i).GetName();
                for (var j = 0; j < synSet.GetSynonym().LiteralSize(); j++)
                {
                    var literal2 = synSet.GetSynonym().GetLiteral(j).GetName();
                    if (literal1 == literal2)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /**
         * <summary>Returns true if relations list contains the specified relation.</summary>
         *
         * <param name="relation">element whose presence in the list is to be tested</param>
         * <returns>true if the list contains the specified element</returns>
         */
        public bool ContainsRelation(Relation relation)
        {
            return _relations.Contains(relation);
        }

        /**
         * <summary>Returns true if specified semantic relation type presents in the relations list.</summary>
         *
         * <param name="semanticRelationType">element whose presence in the list is to be tested</param>
         * <returns>true if specified semantic relation type presents in the relations list</returns>
         */
        public bool ContainsRelationType(SemanticRelationType semanticRelationType)
        {
            foreach (var relation in _relations)
            {
                if (relation is SemanticRelation semanticRelation &&
                    semanticRelation.GetRelationType() == semanticRelationType)
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * <summary>Merges synonym and a specified SynSet with their definitions, relations, part of speech tags and examples.</summary>
         *
         * <param name="synSet">SynSet to be merged</param>
         */
        public void MergeSynSet(SynSet synSet)
        {
            for (var i = 0; i < synSet.GetSynonym().LiteralSize(); i++)
            {
                if (!_synonym.Contains(synSet.GetSynonym().GetLiteral(i)))
                {
                    _synonym.AddLiteral(synSet.GetSynonym().GetLiteral(i));
                }
            }

            if (_definition == null && synSet.GetDefinition() != null)
            {
                SetDefinition(synSet.GetDefinition());
            }
            else
            {
                if (_definition != null && synSet.GetDefinition() != null &&
                    GetLongDefinition() != synSet.GetLongDefinition())
                {
                    SetDefinition(GetLongDefinition() + "|" + synSet.GetLongDefinition());
                }
            }

            if (synSet.RelationSize() != 0)
            {
                for (var i = 0; i < synSet.RelationSize(); i++)
                {
                    if (!ContainsRelation(synSet.GetRelation(i)) && synSet.GetRelation(i).GetName() != _id)
                    {
                        AddRelation(synSet.GetRelation(i));
                    }
                }
            }

            if (_example == null && synSet.GetExample() != null)
            {
                _example = synSet.GetExample();
            }
        }

        /**
         * <summary>An overridden equals method to compare two {@code Object}s.</summary>
         *
         * <param name="secondObject">the reference object with which to compare</param>
         * <returns>{@code true} if this object's ID is the same as the obj argument's ID; {@code false} otherwise.</returns>
         */
        public override bool Equals(object secondObject)
        {
            if (!(secondObject is SynSet))
            {
                return false;
            }

            var second = (SynSet) secondObject;
            return _id.Equals(second._id);
        }

        /**
         * <summary>Returns a hash code for the ID.</summary>
         *
         * <returns>a hash code for the ID</returns>
         */
        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        /**
         * <summary>Overridden ToString method to print the first definition or representative.</summary>
         *
         * <returns>print the first definition or representative.</returns>
         */
        public override string ToString()
        {
            if (_definition != null)
            {
                return _definition[0];
            }

            return Representative();
        }

        /**
         * <summary>Method to write SynSets to the specified file in the XML format.</summary>
         *
         * <param name="outfile">BufferedWriter to write XML files</param>
         */
        public void SaveAsXml(StreamWriter outfile)
        {
            outfile.Write("<SYNSET>");
            outfile.Write("<ID>" + _id + "</ID>");
            _synonym.SaveAsXml(outfile);
            switch (_pos)
            {
                case Pos.NOUN:
                    outfile.Write("<POS>n</POS>");
                    break;
                case Pos.ADJECTIVE:
                    outfile.Write("<POS>a</POS>");
                    break;
                case Pos.VERB:
                    outfile.Write("<POS>v</POS>");
                    break;
                case Pos.ADVERB:
                    outfile.Write("<POS>b</POS>");
                    break;
                case Pos.CONJUNCTION:
                    outfile.Write("<POS>c</POS>");
                    break;
                case Pos.PRONOUN:
                    outfile.Write("<POS>r</POS>");
                    break;
                case Pos.INTERJECTION:
                    outfile.Write("<POS>i</POS>");
                    break;
                case Pos.PREPOSITION:
                    outfile.Write("<POS>p</POS>");
                    break;
            }

            foreach (var r in _relations)
            {
                switch (r)
                {
                    case InterlingualRelation interlingualRelation:
                        outfile.Write("<ILR>" + interlingualRelation.GetName() + "<TYPE>" +
                                      interlingualRelation.GetTypeAsString() +
                                      "</TYPE></ILR>");
                        break;
                    case SemanticRelation semanticRelation:
                        outfile.Write("<SR>" + semanticRelation.GetName() + "<TYPE>" +
                                      semanticRelation.GetTypeAsString() +
                                      "</TYPE></SR>");
                        break;
                }
            }

            if (_definition != null)
            {
                if (GetLongDefinition().Length == 0)
                {
                    outfile.Write("<DEF> </DEF>");

                }
                else
                {
                    outfile.Write("<DEF>" + GetLongDefinition() + "</DEF>");
                }
            }

            if (_example != null)
            {
                outfile.Write("<EXAMPLE>" + _example + "</EXAMPLE>");
            }

            outfile.WriteLine("</SYNSET>");
        }
    }
}