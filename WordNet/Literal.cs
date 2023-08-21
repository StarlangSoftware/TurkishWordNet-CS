using System.Collections.Generic;
using System.IO;

namespace WordNet
{
    public class Literal
    {
        private string _name;
        private int _sense;
        private string _synSetId;
        private string _origin;
        private readonly List<Relation> _relations;
        private int _groupNo;

        /**
         * <summary>A constructor that initializes name, sense, SynSet ID and the relations.</summary>
         *
         * <param name="name">    name of a literal</param>
         * <param name="sense">   index of sense</param>
         * <param name="synSetId">ID of the SynSet</param>
         */
        public Literal(string name, int sense, string synSetId)
        {
            this._name = name;
            this._sense = sense;
            this._synSetId = synSetId;
            _relations = new List<Relation>();
        }

        /**
         * <summary>Overridden equals method returns true if the specified object literal equals to the current literal's name.</summary>
         *
         * <param name="literal">Object literal to compare</param>
         * <returns>true if the specified object literal equals to the current literal's name</returns>
         */
        public override bool Equals(object literal)
        {
            if (literal == null)
                return false;
            if (literal == this)
                return true;
            var secondLiteral = (Literal) literal;
            return _name == secondLiteral.GetName() && _sense == secondLiteral.GetSense();
        }

        /**
         * <summary>Accessor method to return SynSet ID.</summary>
         *
         * <returns>string of SynSet ID</returns>
         */
        public string GetSynSetId()
        {
            return _synSetId;
        }

        /**
         * <summary>Accessor method to return name of the literal.</summary>
         *
         * <returns>name of the literal</returns>
         */
        public string GetName()
        {
            return _name;
        }

        /**
         * <summary>Accessor method to return the index of sense of the literal.</summary>
         *
         * <returns>index of sense of the literal</returns>
         */
        public int GetSense()
        {
            return _sense;
        }

        /**
         * <summary>Accessor method to return the origin of the literal.</summary>
         *
         * <returns>origin of the literal</returns>
         */
        public string GetOrigin()
        {
            return _origin;
        }

        /**
         * <summary>Mutator method to set the origin with specified origin.</summary>
         *
         * <param name="origin">origin of the literal to set</param>
         */
        public void SetOrigin(string origin)
        {
            this._origin = origin;
        }

        /**
         * <summary>Accessor method to return the groupNo of the literal.</summary>
         *
         * <returns>groupNo of the literal</returns>
         */
        public int GetGroupNo()
        {
            return _groupNo;
        }

        /**
         * <summary>Mutator method to set the origin with specified groupNo.</summary>
         *
         * <param name="groupNo">groupNo of the literal to set</param>
         */
        public void SetGroupNo(int groupNo)
        {
            this._groupNo = groupNo;
        }
        
        /**
         * <summary>Mutator method to set the sense index of the literal.</summary>
         *
         * <param name="sense">sense index of the literal to set</param>
         */
        public void SetSense(int sense)
        {
            this._sense = sense;
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
                    semanticRelation.GetRelationType().Equals(semanticRelationType))
                {
                    return true;
                }
            }

            return false;
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
         * <summary>Returns size of relations list.</summary>
         *
         * <returns>the size of the list</returns>
         */
        public int RelationSize()
        {
            return _relations.Count;
        }

        /**
         * <summary>Mutator method to set name of a literal.</summary>
         *
         * <param name="name">name of the literal to set</param>
         */
        public void SetName(string name)
        {
            this._name = name;
        }

        /**
         * <summary>Mutator method to set SynSet ID of a literal.</summary>
         *
         * <param name="synSetId">SynSet ID of the literal to set</param>
         */
        public void SetSynSetId(string synSetId)
        {
            this._synSetId = synSetId;
        }

        /**
         * <summary>Method to write Literals to the specified file in the XML format.</summary>
         *
         * <param name="outfile">BufferedWriter to write XML files</param>
         */
        public void SaveAsXml(StreamWriter outfile)
        {
            if (_name == "&")
            {
                outfile.Write("<LITERAL>&amp;<SENSE>" + _sense + "</SENSE>");
            }
            else
            {
                outfile.Write("<LITERAL>" + _name + "<SENSE>" + _sense + "</SENSE>");
            }

            if (_origin != null)
            {
                outfile.Write("<ORIGIN>" + _origin + "</ORIGIN>");
            }

            foreach (var r in _relations)
            {
                switch (r)
                {
                    case InterlingualRelation interlingualRelation:
                        outfile.Write("<ILR>" + interlingualRelation.GetName() + "<TYPE>" + interlingualRelation.GetTypeAsString() +
                                      "</TYPE></ILR>");
                        break;
                    case SemanticRelation semanticRelation when semanticRelation.ToIndex() == 0:
                        outfile.Write("<SR>" + semanticRelation.GetName() + "<TYPE>" +
                                      semanticRelation.GetTypeAsString() + "</TYPE></SR>");
                        break;
                    case SemanticRelation semanticRelation:
                        outfile.Write("<SR>" + semanticRelation.GetName() + "<TYPE>" +
                                      semanticRelation.GetTypeAsString() + "</TYPE>" + "<TO>" +
                                      semanticRelation.ToIndex() + "</TO>" + "</SR>");
                        break;
                }
            }
            outfile.Write("</LITERAL>");
        }

        /**
         * <summary>Overridden ToString method to print names and sense of literals.</summary>
         *
         * <returns>concatenated names and senses of literals</returns>
         */
        public override string ToString()
        {
            return _name + " " + _sense;
        }
    }
}