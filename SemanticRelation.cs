namespace WordNet
{
    public class SemanticRelation : Relation
    {
        private SemanticRelationType _relationType;
        private readonly int _toIndex;


        private static readonly string[] SemanticDependency =
        {
            "ANTONYM", "HYPERNYM",
            "INSTANCE_HYPERNYM", "HYPONYM", "INSTANCE_HYPONYM", "MEMBER_HOLONYM", "SUBSTANCE_HOLONYM",
            "PART_HOLONYM", "MEMBER_MERONYM", "SUBSTANCE_MERONYM", "PART_MERONYM", "ATTRIBUTE",
            "DERIVATION_RELATED", "DOMAIN_TOPIC", "MEMBER_TOPIC", "DOMAIN_REGION", "MEMBER_REGION",
            "DOMAIN_USAGE", "MEMBER_USAGE", "ENTAILMENT", "CAUSE", "ALSO_SEE",
            "VERB_GROUP", "SIMILAR_TO", "PARTICIPLE_OF_VERB", "NONE"
        };

        private static readonly SemanticRelationType[] SemanticDependencyTags =
        {
            SemanticRelationType.ANTONYM, SemanticRelationType.HYPERNYM,
            SemanticRelationType.INSTANCE_HYPERNYM, SemanticRelationType.HYPONYM, SemanticRelationType.INSTANCE_HYPONYM,
            SemanticRelationType.MEMBER_HOLONYM, SemanticRelationType.SUBSTANCE_HOLONYM,
            SemanticRelationType.PART_HOLONYM, SemanticRelationType.MEMBER_MERONYM,
            SemanticRelationType.SUBSTANCE_MERONYM, SemanticRelationType.PART_MERONYM, SemanticRelationType.ATTRIBUTE,
            SemanticRelationType.DERIVATION_RELATED, SemanticRelationType.DOMAIN_TOPIC,
            SemanticRelationType.MEMBER_TOPIC, SemanticRelationType.DOMAIN_REGION, SemanticRelationType.MEMBER_REGION,
            SemanticRelationType.DOMAIN_USAGE, SemanticRelationType.MEMBER_USAGE, SemanticRelationType.ENTAILMENT,
            SemanticRelationType.CAUSE, SemanticRelationType.ALSO_SEE,
            SemanticRelationType.VERB_GROUP, SemanticRelationType.SIMILAR_TO, SemanticRelationType.PARTICIPLE_OF_VERB,
            SemanticRelationType.NONE
        };

        /**
         * <summary>Accessor to retrieve semantic relation type given a specific semantic dependency tag.</summary>
         *
         * <param name="tag">string semantic dependency tag</param>
         * <returns>semantic relation type</returns>
         */
        public static SemanticRelationType GetSemanticTag(string tag)
        {
            for (var j = 0; j < SemanticDependencyTags.Length; j++)
            {
                if (tag == SemanticDependency[j])
                {
                    return SemanticDependencyTags[j];
                }
            }

            return SemanticRelationType.NONE;
        }

        /**
         * <summary>Returns the reverse of a specific semantic relation type.</summary>
         *
         * <param name="semanticRelationType">semantic relation type to be reversed</param>
         * <returns>reversed version of the semantic relation type</returns>
         */
        public static SemanticRelationType Reverse(SemanticRelationType semanticRelationType)
        {
            switch (semanticRelationType)
            {
                case SemanticRelationType.HYPERNYM:
                    return SemanticRelationType.HYPONYM;
                case SemanticRelationType.HYPONYM:
                    return SemanticRelationType.HYPERNYM;
                case SemanticRelationType.ANTONYM:
                    return SemanticRelationType.ANTONYM;
                case SemanticRelationType.INSTANCE_HYPERNYM:
                    return SemanticRelationType.INSTANCE_HYPONYM;
                case SemanticRelationType.INSTANCE_HYPONYM:
                    return SemanticRelationType.INSTANCE_HYPERNYM;
                case SemanticRelationType.MEMBER_HOLONYM:
                    return SemanticRelationType.MEMBER_MERONYM;
                case SemanticRelationType.MEMBER_MERONYM:
                    return SemanticRelationType.MEMBER_HOLONYM;
                case SemanticRelationType.PART_MERONYM:
                    return SemanticRelationType.PART_HOLONYM;
                case SemanticRelationType.PART_HOLONYM:
                    return SemanticRelationType.PART_MERONYM;
                case SemanticRelationType.SUBSTANCE_MERONYM:
                    return SemanticRelationType.SUBSTANCE_HOLONYM;
                case SemanticRelationType.SUBSTANCE_HOLONYM:
                    return SemanticRelationType.SUBSTANCE_MERONYM;
                case SemanticRelationType.DOMAIN_TOPIC:
                    return SemanticRelationType.MEMBER_TOPIC;
                case SemanticRelationType.MEMBER_TOPIC:
                    return SemanticRelationType.DOMAIN_TOPIC;
                case SemanticRelationType.DOMAIN_REGION:
                    return SemanticRelationType.MEMBER_REGION;
                case SemanticRelationType.MEMBER_REGION:
                    return SemanticRelationType.DOMAIN_REGION;
                case SemanticRelationType.DOMAIN_USAGE:
                    return SemanticRelationType.MEMBER_USAGE;
                case SemanticRelationType.MEMBER_USAGE:
                    return SemanticRelationType.DOMAIN_USAGE;
            }

            return SemanticRelationType.NONE;
        }

        /**
         * <summary>A constructor to initialize relation type and the relation name.</summary>
         *
         * <param name="name">        name of the relation</param>
         * <param name="relationType">string semantic dependency tag</param>
         */
        public SemanticRelation(string name, string relationType) : base(name)
        {
            this._relationType = GetSemanticTag(relationType);
        }

        /**
         * <summary>Another constructor that initializes relation type, relation name, and the index.</summary>
         *
         * <param name="name">        name of the relation</param>
         * <param name="relationType">string semantic dependency tag</param>
         * <param name="toIndex">     index of the relation</param>
         */
        public SemanticRelation(string name, string relationType, int toIndex) : base(name)
        {
            this._relationType = GetSemanticTag(relationType);
            this._toIndex = toIndex;
        }

        /**
         * <summary>Another constructor that initializes relation type and relation name.</summary>
         *
         * <param name="name">        name of the relation</param>
         * <param name="relationType">semantic dependency tag</param>
         */
        public SemanticRelation(string name, SemanticRelationType relationType) : base(name)
        {
            this._relationType = relationType;
        }

        /**
         * <summary>Another constructor that initializes relation type, relation name, and the index.</summary>
         *
         * <param name="name">        name of the relation</param>
         * <param name="relationType">semantic dependency tag</param>
         * <param name="toIndex">     index of the relation</param>
         */
        public SemanticRelation(string name, SemanticRelationType relationType, int toIndex) : base(name)
        {
            this._relationType = relationType;
            this._toIndex = toIndex;
        }

        /**
         * <summary>An overridden equals method to compare two {@code Object}s.</summary>
         *
         * <param name="second">the reference object with which to compare</param>
         * <returns>{@code true} if this object is the same as the obj</returns>
         * argument; {@code false} otherwise.
         */
        public override bool Equals(object second)
        {
            var relation = (SemanticRelation) second;
            return name.Equals(relation.name) && _relationType.Equals(relation._relationType) &&
                   _toIndex == relation._toIndex;
        }

        /**
         * <summary>Returns the index value.</summary>
         *
         * <returns>index value.</returns>
         */
        public int ToIndex()
        {
            return _toIndex;
        }

        /**
         * <summary>Accessor for the semantic relation type.</summary>
         *
         * <returns>semantic relation type</returns>
         */
        public SemanticRelationType GetRelationType()
        {
            return _relationType;
        }

        /**
         * <summary>Mutator for the semantic relation type.</summary>
         *
         * <param name="relationType">semantic relation type.</param>
         */
        public void SetRelationType(SemanticRelationType relationType)
        {
            this._relationType = relationType;
        }

        /**
         * <summary>Accessor method to retrieve the semantic relation type as a string.</summary>
         *
         * <returns>string semantic relation type</returns>
         */
        public string GetTypeAsString()
        {
            if (_relationType != SemanticRelationType.NONE)
            {
                return SemanticDependency[(int) _relationType];
            }

            return null;
        }

        /**
         * <summary>Overridden tostring method to print semantic relation types and names.</summary>
         *
         * <returns>semantic relation types and names</returns>
         */
        public override string ToString()
        {
            return GetTypeAsString() + "->" + name;
        }
    }
}