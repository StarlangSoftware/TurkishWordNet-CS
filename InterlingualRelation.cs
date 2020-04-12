namespace WordNet
{
    public class InterlingualRelation : Relation
    {
        private readonly InterlingualDependencyType _dependencyType;

        private static readonly string[] IlrDependency =
        {
            "Hypernym", "Near_antonym", "Holo_member", "Holo_part", "Holo_portion",
            "Usage_domain", "Category_domain", "Be_in_state", "Subevent", "Verb_group",
            "Similar_to", "Also_see", "Causes", "SYNONYM", "None"
        };

        private static readonly InterlingualDependencyType[] InterlingualDependencyTags =
        {
            InterlingualDependencyType.HYPERNYM,
            InterlingualDependencyType.NEAR_ANTONYM, InterlingualDependencyType.HOLO_MEMBER, InterlingualDependencyType
                .HOLO_PART,
            InterlingualDependencyType.HOLO_PORTION, InterlingualDependencyType.USAGE_DOMAIN, InterlingualDependencyType
                .CATEGORY_DOMAIN,
            InterlingualDependencyType.BE_IN_STATE, InterlingualDependencyType.SUBEVENT, InterlingualDependencyType
                .VERB_GROUP,
            InterlingualDependencyType.SIMILAR_TO, InterlingualDependencyType.ALSO_SEE, InterlingualDependencyType
                .CAUSES,
            InterlingualDependencyType.SYNONYM, InterlingualDependencyType.NONE
        };

        /**
         * <summary>Compares specified {@code string} tag with the tags in InterlingualDependencyType {@code Array}, ignoring case
         * considerations.</summary>
         *
         * <param name="tag">string to compare</param>
         * <returns>interlingual dependency type according to specified tag</returns>
         */
        public static InterlingualDependencyType GetInterlingualDependencyTag(string tag)
        {
            for (var j = 0; j < IlrDependency.Length; j++)
            {
                if (tag == IlrDependency[j])
                {
                    return InterlingualDependencyTags[j];
                }
            }

            return InterlingualDependencyType.NONE;
        }

        /**
         * <summary>InterlingualRelation method sets its relation with the specified string name, then gets the InterlingualDependencyType
         * according to specified string dependencyType.</summary>
         *
         * <param name="name">          relation name</param>
         * <param name="dependencyType">interlingual dependency type</param>
         */
        public InterlingualRelation(string name, string dependencyType) : base(name)
        {
            this._dependencyType = GetInterlingualDependencyTag(dependencyType);
        }

        /**
         * <summary>Accessor method to get the private InterlingualDependencyType.</summary>
         *
         * <returns>interlingual dependency type</returns>
         */
        public new InterlingualDependencyType GetType()
        {
            return _dependencyType;
        }

        /**
         * <summary>Method to retrieve interlingual dependency type as {@code string}.</summary>
         *
         * <returns>string interlingual dependency type</returns>
         */
        public string GetTypeAsString()
        {
            return IlrDependency[(int) _dependencyType];
        }

        /**
         * <summary>ToString method to print interlingual dependency type.</summary>
         *
         * <returns>string of relation name</returns>
         */
        public override string ToString()
        {
            return GetTypeAsString() + "->" + name;
        }
    }
}