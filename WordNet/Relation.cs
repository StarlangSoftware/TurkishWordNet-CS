namespace WordNet
{
    public class Relation
    {
        protected string name;

        /**
         * <summary>A constructor that sets the name of the relation.</summary>
         *
         * <param name="name">string relation name</param>
         */
        public Relation(string name)
        {
            this.name = name;
        }

        /**
         * <summary>Accessor method for the relation name.</summary>
         *
         * <returns>string relation name</returns>
         */
        public string GetName()
        {
            return name;
        }

        /**
         * <summary>Mutator for the relation name.</summary>
         *
         * <param name="name">string relation name</param>
         */
        public void SetName(string name)
        {
            this.name = name;
        }

        /**
         * <summary>An overridden equals method to compare two {@code Object}s.</summary>
         *
         * <param name="second">the reference object with which to compare</param>
         * <returns>{@code true} if this object is the same as the obj
         * argument; {@code false} otherwise.</returns>
         */
        public override bool Equals(object second)
        {
            var relation = (Relation) second;
            return name.Equals(relation.name);
        }
    }
}