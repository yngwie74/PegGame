namespace Game
{
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

        #region Public Methods and Operators

        public float CanArrive(Coord from, Coord to)
        {
            return this.RouteHelper(from, to, 1);
        }

        public IRoutable NextStep(Coord p)
        {
            return this.board[p].NextStepFrom(p);
        }

        #endregion

        #region Methods

        private float FindHighestProbRoute(Coord goal, float initialProb, IRoutable paths)
        {
            return paths.Routes.Max(
                r => this.RouteHelper(
                    r.NextStep, goal, initialProb * r.Probability));
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
}