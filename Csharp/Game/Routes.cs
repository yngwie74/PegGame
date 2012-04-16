namespace Game
{
    using System.Collections.Generic;

    public class Route : IRoute, IRoutable
    {
        #region Constants and Fields

        private readonly Coord next;
        private readonly float probability;

        #endregion

        #region Constructors and Destructors

        public Route(Coord next, float probability)
        {
            this.next = next;
            this.probability = probability;
        }

        #endregion

        #region Public Properties

        public Coord NextStep
        {
            get
            {
                return this.next;
            }
        }

        public float Probability
        {
            get
            {
                return this.probability;
            }
        }

        public IEnumerable<IRoute> Routes
        {
            get
            {
                return new IRoute[] { this };
            }
        }

        #endregion

        #region Public Methods and Operators

        public bool Equals(Route other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other.next, this.next) && other.probability.Equals(this.probability);
        }

        public override bool Equals(object obj)
        {
            if (obj is Route)
            {
                return this.Equals((Route)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            const int hashSalt = 0x18d;

            unchecked
            {
                var nextHash = (this.next.GetHashCode() * hashSalt);
                return nextHash ^ this.probability.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format(
                "{0}({1}, probability={2})", this.GetType().Name, this.next, this.probability);
        }

        #endregion
    }

    public class Fork : IRoutable
    {
        #region Constants and Fields

        private readonly IRoute left;
        private readonly IRoute right;

        #endregion

        #region Constructors and Destructors

        public Fork(IRoute left, IRoute right)
        {
            this.left = left;
            this.right = right;
        }

        #endregion

        #region Public Properties

        public IEnumerable<IRoute> Routes
        {
            get
            {
                return new[] { this.left, this.right };
            }
        }

        #endregion

        #region Public Methods and Operators

        public override bool Equals(object obj)
        {
            if (obj is Fork)
            {
                return this.Equals((Fork)obj);
            }
            return false;
        }

        public bool Equals(Fork other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            return ReferenceEquals(this, other)
                   || (Equals(other.left, this.left) && Equals(other.right, this.right));
        }

        public override int GetHashCode()
        {
            const int hashSalt = 0x18d;

            unchecked
            {
                var ltHash = (this.left != null ? this.left.GetHashCode() : 0);
                var rtHash = (this.right != null ? this.right.GetHashCode() : 0);
                return (ltHash * hashSalt) ^ rtHash;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}({1}, {2})", this.GetType().Name, this.left, this.right);
        }

        #endregion
    }
}