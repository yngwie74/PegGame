namespace Game
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Board
    {
        #region Constants and Fields

        public readonly int Height;

        public readonly int Width;

        private const int MaxHeight = 99;

        private const int MaxWidth = 99;

        private const int MinHeight = 3;

        private const int MinWidth = 3;

        private readonly IDictionary<Coord, ICell> pins = new Dictionary<Coord, ICell>();

        #endregion

        #region Constructors and Destructors

        public Board(int height, int width)
        {
            this.Width = CheckWidth(width);
            this.Height = CheckHeight(height);
            this.InitializePins();
        }

        #endregion

        #region Public Indexers

        public ICell this[int r, int c]
        {
            get
            {
                return this[new Coord(r, c)];
            }
            private set
            {
                this[new Coord(r, c)] = value;
            }
        }

        public ICell this[Coord coord]
        {
            get
            {
                if (this.pins.ContainsKey(coord))
                {
                    return this.pins[coord];
                }
                return CellFactory.Blank;
            }
            private set
            {
                if (!this.pins.ContainsKey(coord))
                {
                    this.pins[coord] = value;
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        public bool InBoard(Coord coord)
        {
            return InRange(this.Height, coord.Row) && InRange(this.Width, coord.Column);
        }

        public Board RemovePinAt(int r, int c)
        {
            if (this[r, c].IsPin)
            {
                this.pins.Remove(new Coord(r, c));
            }
            return this;
        }

        public Board SetPinAt(Coord coord)
        {
            return this.SetPinAt(coord.Row, coord.Column);
        }

        public Board SetPinAt(int r, int c)
        {
            this[r, c] = CellFactory.MakePin(c, this.Width - 1);
            return this;
        }

        public override string ToString()
        {
            var lines = new List<string>(this.Height);
            for (var r = 0; r < this.Height; r++)
            {
                var line = new StringBuilder(this.Width);
                for (var c = 0; c < this.Width; c++)
                {
                    line.Append(this[r, c].IsPin ? 'x' : '.');
                }
                lines.Add(line.ToString());
            }
            return string.Join("\n", lines);
        }

        #endregion

        #region Methods

        private static int CheckHeight(int height)
        {
            const string desc = "height";
            CheckRange(MinHeight, MaxHeight, height, desc);
            CheckIsOdd(height, desc);
            return height;
        }

        private static void CheckIsOdd(int value, string desc)
        {
            const string message = "El valor del parámetro debe ser impar.";

            if (value % 2 == 0)
            {
                throw new ArgumentOutOfRangeException(desc, message);
            }
        }

        private static void CheckRange(int min, int max, int value, string desc)
        {
            if (value < min || max < value)
            {
                var message = string.Format(
                    "El valor del parámetro debe estar entre {0} y {1}.", min, max);
                throw new ArgumentOutOfRangeException(desc, message);
            }
        }

        private static int CheckWidth(int width)
        {
            const string desc = "width";
            CheckRange(MinWidth, MaxWidth, width, desc);
            CheckIsOdd(width, desc);
            return width;
        }

        private static bool InRange(int max, int val)
        {
            return (0 <= val) && (val < max);
        }

        private void InitializePins()
        {
            for (var r = 0; r < this.Height; r++)
            {
                for (var c = r % 2; c < this.Width; c += 2)
                {
                    this.SetPinAt(r, c);
                }
            }
        }

        #endregion
    }
}