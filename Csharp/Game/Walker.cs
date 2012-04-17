namespace Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Walker
    {
        #region Constants and Fields

        private readonly Board board;

        #endregion

        #region Constructors and Destructors

        public Walker(Board board)
        {
            this.board = board;
        }

        #endregion

        #region Properties

        private IEnumerable<int> EntryCols
        {
            get
            {
                return Enumerable.Range(0, this.board.Width).Where(c => c % 2 != 0);
            }
        }

        #endregion

        #region Public Methods and Operators

        public float CanArrive(Coord from, Coord to)
        {
            return this.RouteHelper(from, to, 1);
        }

        public float[] FindAllRoutesTo(int row, int column)
        {
            return this.GetRoutesTo(new Coord(row, column)).ToArray();
        }

        public Result FindBestRouteTo(int row, int column)
        {
            return this.GetRoutesTo(new Coord(row, column)).Select(Result.Make).Max();
        }

        public IRoutable NextStep(Coord p)
        {
            return this.board[p].NextStepFrom(p);
        }

        #endregion

        #region Methods

        private float FindHighestProbRoute(Coord goal, float initialProb, IRoutable paths)
        {
            return paths.Routes.Max(r => this.RouteHelper(r.NextStep, goal, initialProb * r.Probability));
        }

        private IEnumerable<float> GetRoutesTo(Coord to)
        {
            return this.EntryCols.Select(c => this.CanArrive(new Coord(0, c), to));
        }

        private float RouteHelper(Coord from, Coord to, float initialProb)
        {
            if (from.Equals(to))
            {
                return initialProb;
            }

            if (!this.board.InBoard(from))
            {
                return 0;
            }

            var paths = this.NextStep(from);
            return this.FindHighestProbRoute(to, initialProb, paths);
        }

        #endregion
    }

    public struct Result : IComparable<Result>
    {
        #region Constants and Fields

        public readonly int Column;

        public readonly float Probability;

        #endregion

        #region Constructors and Destructors

        public Result(int column, float probability)
        {
            this.Column = column;
            this.Probability = probability;
        }

        #endregion

        #region Public Methods and Operators

        public static Result Make(float probability, int column)
        {
            return new Result(column, probability);
        }

        public int CompareTo(Result other)
        {
            return this.Probability.CompareTo(other.Probability);
        }

        #endregion
    }
}