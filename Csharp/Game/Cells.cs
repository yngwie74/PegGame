namespace Game
{
    using System;

    public class Cell : ICell
    {
        #region Constants and Fields

        private readonly ReboundDirection reboundDirection;

        #endregion

        #region Constructors and Destructors

        internal Cell(ReboundDirection reboundDirection)
        {
            this.reboundDirection = reboundDirection;
        }

        #endregion

        #region Public Properties

        public bool IsPin
        {
            get { return reboundDirection != ReboundDirection.Down; }
        }

        public float Probability
        {
            get { return 1; }
        }

        #endregion

        #region Public Methods and Operators

        public virtual IRoutable NextStepFrom(Coord p)
        {
            var next = this.CalcNextStep(p);
            return new Route(next, this.Probability);
        }

        #endregion

        #region Methods

        private Coord CalcNextStep(Coord p)
        {
            Coord next;
            switch (this.reboundDirection)
            {
                case ReboundDirection.Down:
                    next = p.Down;
                    break;
                case ReboundDirection.Left:
                    next = p.ToLeft;
                    break;
                case ReboundDirection.Right:
                    next = p.ToRight;
                    break;
                default:
                    throw new ApplicationException("¡Dirección inválida!");
            }
            return next;
        }

        #endregion
    }

    public class Pin : ICell
    {
        #region Constructors and Destructors

        internal Pin() { }

        #endregion

        #region Public Properties

        public bool IsPin
        {
            get { return true; }
        }

        public virtual float Probability
        {
            get
            {
                return 0.5f;
            }
        }

        #endregion

        #region Public Methods and Operators

        public IRoutable NextStepFrom(Coord p)
        {
            var left = new Route(p.ToLeft, this.Probability);
            var right = new Route(p.ToRight, this.Probability);
            return new Fork(left, right);
        }

        #endregion
    }
}