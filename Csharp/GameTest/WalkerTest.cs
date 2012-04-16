namespace Game.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WalkerTest
    {
        #region Constants and Fields

        private Walker walker;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void ForkIfHitsAPin()
        {
            var from = new Coord(1, 1);
            var expected = new Fork(
                new Route(new Coord(1, 0), 0.5f), new Route(new Coord(1, 2), 0.5f));
            this.AssertNextStepFromShouldBe(from, expected);
        }

        [TestMethod]
        public void MoveForwardIfAtABlank()
        {
            var from = new Coord(0, 1);
            var expected = new Route(new Coord(1, 1), 1);
            this.AssertNextStepFromShouldBe(from, expected);
        }

        [TestMethod]
        public void MoveLeftIfHitsRightBorder()
        {
            var from = new Coord(2, 2);
            var expected = new Route(new Coord(2, 1), 1);
            this.AssertNextStepFromShouldBe(from, expected);
        }

        [TestMethod]
        public void MoveRightIfHitsLeftBorder()
        {
            var from = new Coord(0, 0);
            var expected = new Route(new Coord(0, 1), 1);
            this.AssertNextStepFromShouldBe(from, expected);
        }

        [TestInitialize]
        public void SetUp()
        {
            this.walker = new Walker(new Board(3, 3));
        }

        [TestMethod]
        public void WalkerCtor()
        {
            Assert.IsNotNull(this.walker);
        }

        #endregion

        #region Methods

        private void AssertNextStepFromShouldBe(Coord point, IRoutable route)
        {
            Assert.AreEqual(route, this.walker.NextStep(point));
        }

        #endregion
    }

    [TestClass]
    public class WalkerRoutesTest
    {
        #region Constants and Fields

        private Board board;

        private Walker walker;

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void SetUp()
        {
            this.board = new Board(3, 3);
            this.walker = new Walker(this.board);
        }

        [TestMethod]
        public void SingleHopTrajectory()
        {
            Coord from = new Coord(0, 1), to = new Coord(2, 1);
            this.AssertBestProbToArrive(from, to, 0.5f);
        }

        [TestMethod]
        public void StraightTrajectory()
        {
            this.board.RemovePinAt(1, 1);

            Coord from = new Coord(0, 1), to = new Coord(2, 1);
            this.AssertBestProbToArrive(from, to, 1);
        }

        [TestMethod]
        public void TwoHopTrajectory()
        {
            this.walker = new Walker(new Board(5, 5));

            Coord from = new Coord(0, 3), to = new Coord(2, 1);
            this.AssertBestProbToArrive(from, to, 0.25f);
        }

        [TestMethod]
        public void ImpossibleTrajectory()
        {
            this.walker = new Walker(new Board(5, 9));

            Coord from = new Coord(0, 7), to = new Coord(4, 1);
            this.AssertBestProbToArrive(from, to, 0);
        }

        #endregion

        #region Methods

        private void AssertBestProbToArrive(Coord from, Coord to, float shouldBe)
        {
            var probability = this.walker.CanArrive(from, to);

            Assert.AreEqual(shouldBe, probability);
        }

        #endregion
    }
}