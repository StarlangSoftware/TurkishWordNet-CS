using System;
using System.Collections.Generic;
using System.IO;

namespace WordNet
{
    public class Synonym
    {
        private readonly List<Literal> _literals;

        /**
         * <summary>A constructor that creates a new {@link List} literals.</summary>
         */
        public Synonym()
        {
            _literals = new List<Literal>();
        }

        /**
         * <summary>Appends the specified Literal to the end of literals list.</summary>
         *
         * <param name="literal">element to be appended to the list</param>
         */
        public void AddLiteral(Literal literal)
        {
            _literals.Add(literal);
        }

        /**
         * <summary>Moves the specified literal to the first of literals list.</summary>
         *
         * <param name="literal">element to be moved to the first element of the list</param>
         */
        public void MoveFirst(Literal literal)
        {
            if (Contains(literal))
            {
                _literals.Remove(literal);
                _literals.Insert(0, literal);
            }
        }

        
        public List<Synonym> GetUniqueLiterals(){
            var literalGroups = new List<Synonym>();
            var groupNo = -1;
            var synonym = new Synonym();
            foreach (var literal in _literals){
                if (literal.GetGroupNo() != groupNo){
                    if (groupNo != -1){
                        literalGroups.Add(synonym);
                    }
                    groupNo = literal.GetGroupNo();
                    synonym = new Synonym();
                } else {
                    if (groupNo == 0){
                        literalGroups.Add(synonym);
                        synonym = new Synonym();
                    }
                }
                synonym.AddLiteral(literal);
            }
            literalGroups.Add(synonym);
            return literalGroups;
        }

        /**
         * <summary>Returns the element at the specified position in literals list.</summary>
         *
         * <param name="index">index of the element to return</param>
         * <returns>the element at the specified position in the list</returns>
         */
        public Literal GetLiteral(int index)
        {
            return _literals[index];
        }

        /**
         * <summary>Returns the element with the specified name in literals list.</summary>
         *
         * <param name="name">name of the element to return</param>
         * <returns>the element with the specified name in the list</returns>
         */
        public Literal GetLiteral(String name)
        {
            foreach (var literal in _literals)
            {
                if (literal.GetName() == name)
                {
                    return literal;
                }
            }

            return null;
        }

        /**
         * <summary>Returns size of literals list.</summary>
         *
         * <returns>the size of the list</returns>
         */
        public int LiteralSize()
        {
            return _literals.Count;
        }

        /**
         * <summary>Returns true if literals list contains the specified literal.</summary>
         *
         * <param name="literal">element whose presence in the list is to be tested</param>
         * <returns>true if the list contains the specified element</returns>
         */
        public bool Contains(Literal literal)
        {
            return _literals.Contains(literal);
        }

        /**
         * <summary>Returns true if literals list contains the specified String literal.</summary>
         *
         * <param name="literalName">element whose presence in the list is to be tested</param>
         * <returns>true if the list contains the specified element</returns>
         */
        public bool ContainsLiteral(string literalName)
        {
            foreach (var literal in _literals)
            {
                if (literal.GetName() == literalName)
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * <summary>Removes the first occurrence of the specified element from literals list,
         * if it is present. If the list does not contain the element, it stays unchanged.</summary>
         *
         * <param name="toBeRemoved">element to be removed from the list, if present</param>
         */
        public void RemoveLiteral(Literal toBeRemoved)
        {
            _literals.Remove(toBeRemoved);
        }

        /**
         * <summary>Method to write Synonyms to the specified file in the XML format.</summary>
         *
         * <param name="outfile">BufferedWriter to write XML files</param>
         */
        public void SaveAsXml(StreamWriter outfile)
        {
            outfile.Write("<SYNONYM>");
            foreach (var literal in _literals) {
                literal.SaveAsXml(outfile);
            }
            outfile.Write("</SYNONYM>");
        }

        /**
         * <summary>Overridden toString method to print literals.</summary>
         *
         * <returns>concatenated literals</returns>
         */
        public override string ToString()
        {
            var result = "";
            foreach (var literal in _literals) {
                result = result + literal.GetName() + " ";
            }
            return result;
        }
    }
}