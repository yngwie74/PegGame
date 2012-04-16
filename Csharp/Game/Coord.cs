namespace Game
{
    public class Coord
    {
        #region Constants and Fields

        private const int HashSalt = 0x18d;

        public readonly int Column;
        public readonly int Row;

        #endregion

        #region Constructors and Destructors

        public Coord(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }

        #endregion

        #region Public Properties

        public Coord Down
        {
            get
            {
                return new Coord(this.Row + 1, this.Column);
            }
        }

        public Coord ToLeft
        {
            get
            {
                return new Coord(this.Row, this.Column - 1);
            }
        }

        public Coord ToRight
        {
            get
            {
                return new Coord(this.Row, this.Column + 1);
            }
        }

        #endregion

        #region Public Methods and Operators

        public override bool Equals(object obj)
        {
            if (obj is Coord)
            {
                return this.Equals((Coord)obj);
            }
            return false;
        }

        public bool Equals(Coord other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other.Column == this.Column && other.Row == this.Row;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Column * HashSalt) ^ this.Row;
            }
        }

        public override string ToString()
        {
            return string.Format("@({0}, {1})", this.Row, this.Column);
        }

        #endregion
    }
}