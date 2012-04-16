namespace Game.Test
{
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BoardTest
    {
        private Board board;

        [TestMethod]
        public void CellsDefaultToBlank()
        {
            Assert.IsFalse(this.board[1, 0].IsPin);
        }

        [TestMethod]
        public void InBoard()
        {
            var testData = this.AddAllValidCells(
                new Dictionary<Coord, bool>
                    {
                        { new Coord(-1, 0), false },
                        { new Coord(-1, -1), false },   { new Coord(-1, 3), false },
                        { new Coord(0, -1), false },    { new Coord(0, 3), false },
                        { new Coord(1, -1), false },    { new Coord(1, 3), false },
                        { new Coord(2, -1), false },    { new Coord(2, 3), false },
                        { new Coord(3, -1), false },    { new Coord(3, 3), false },
                    });

            foreach (var coord in testData.Keys)
            {
                this.AssertCoordInBoard(coord, testData[coord]);
            }
        }

        [TestMethod]
        public void RemovePinAt()
        {
            this.board.SetPinAt(1, 1).RemovePinAt(1, 1);
            Assert.IsFalse(this.board[1, 1].IsPin);
        }

        [TestMethod]
        public void SetPinAtLeftBorder()
        {
            var pos = new Coord(1, 0);
            this.board.SetPinAt(pos);
            Assert.AreSame(this.board[pos], CellFactory.LeftBorder);
        }

        [TestMethod]
        public void SetPinAtRightBorder()
        {
            var pos = new Coord(1, 2);
            this.board.SetPinAt(pos);
            Assert.AreSame(this.board[pos], CellFactory.RightBorder);
        }

        [TestMethod]
        public void SetPinClear()
        {
            var pos = new Coord(2, 1);
            this.board.SetPinAt(pos);
            Assert.AreSame(this.board[pos], CellFactory.MiddlePin);
        }

        [TestInitialize]
        public void SetUp()
        {
            this.board = new Board(3, 3);
        }

        [TestMethod]
        public void ToStringOverride()
        {
            const string expected = "x.x\n.x.\nx.x";
            Assert.AreEqual(expected, this.board.ToString());
        }

        private IDictionary<Coord, bool> AddAllValidCells(IDictionary<Coord, bool> testData)
        {
            for (var r = 0; r < this.board.Height; r++)
            {
                for (var c = 0; c < this.board.Width; c++)
                {
                    testData[new Coord(r, c)] = true;
                }
            }
            return testData;
        }

        private void AssertCoordInBoard(Coord coord, bool expected)
        {
            var msg = string.Format(
                "{0} debería estar {1} del tablero", coord, expected ? "dentro" : "fuera");
            Assert.AreEqual(expected, this.board.InBoard(coord), msg);
        }
    }
}