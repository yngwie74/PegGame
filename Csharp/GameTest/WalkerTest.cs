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

        [TestMethod]
        public void ForkIfHitsAPin()
        {
            var from = new Coord(1, 1);
            var shouldBe = new Fork(new Route(new Coord(1, 0), 0.5f), new Route(new Coord(1, 2), 0.5f));
            this.AssertNextStep(from, shouldBe);
        }

        [TestMethod]
        public void MoveForwardIfAtABlank()
        {
            var from = new Coord(0, 1);
            var shouldBe = new Route(new Coord(1, 1), 1);
            this.AssertNextStep(from, shouldBe);
        }

        [TestMethod]
        public void MoveLeftIfHitsRightBorder()
        {
            var from = new Coord(2, 2);
            var shouldBe = new Route(new Coord(2, 1), 1);
            this.AssertNextStep(from, shouldBe);
        }

        [TestMethod]
        public void MoveRightIfHitsLeftBorder()
        {
            var from = new Coord(0, 0);
            var shouldBe = new Route(new Coord(0, 1), 1);
            this.AssertNextStep(from, shouldBe);
        }

        #endregion

        #region Methods

        private void AssertNextStep(Coord from, IRoutable shouldBe)
        {
            Assert.AreEqual(shouldBe, this.walker.NextStep(from));
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
        public void StraightTrajectory()
        {
            this.board.RemovePinAt(1, 1);

            Coord from = new Coord(0, 1), to = new Coord(2, 1);
            this.AssertBestProbToArrive(from, to, 1);
        }

        [TestMethod]
        public void SingleHopTrajectory()
        {
            Coord from = new Coord(0, 1), to = new Coord(2, 1);
            this.AssertBestProbToArrive(from, to, 0.5f);
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

        [TestMethod]
        public void FindAllRoutes()
        {
            var expected = new[] { 0.2500f, 0.2500f, 0.0625f, 0.0000f };

            var w = MakeSampleBoard();
            var found = w.FindAllRoutesTo(4, 3);

            CollectionAssert.AreEqual(expected, found);
        }

        [TestMethod]
        public void FindBestRoute()
        {
            var expected = new Result(0, 0.25f);

            var w = MakeSampleBoard();
            var r = w.FindBestRouteTo(4, 3);

            AssertAreEqual(expected, r);
        }

        #endregion

        #region Methods

        private static void AssertAreEqual(Result expected, Result actual)
        {
            Assert.AreEqual(expected.Column, actual.Column, "Columna de entrada");
            Assert.AreEqual(expected.Probability, actual.Probability, "Probabilidad");
        }

        private static Walker MakeSampleBoard()
        {
            return new Walker(
                new Board(5, 9)
                    .RemovePinAt(1, 3)
                    .RemovePinAt(2, 2)
                    .RemovePinAt(3, 5));
        }

        private void AssertBestProbToArrive(Coord from, Coord to, float shouldBe)
        {
            Assert.AreEqual(shouldBe, this.walker.CanArrive(from, to));
        }

        #endregion
    }
}