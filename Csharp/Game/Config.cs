namespace Game
{
    using System.Collections.Generic;

    public struct Config
    {
        #region Constants and Fields

        public readonly int GoalColumn;

        public readonly int Height;

        public readonly IList<Coord> MissingPins;

        public readonly int Width;

        #endregion

        #region Constructors and Destructors

        public Config(int height, int width, int goalColumn, IList<Coord> missingPins)
        {
            this.Height = height;
            this.Width = width;
            this.GoalColumn = goalColumn;
            this.MissingPins = missingPins;
        }

        #endregion
    }
}