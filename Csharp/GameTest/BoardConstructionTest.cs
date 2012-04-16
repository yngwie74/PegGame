namespace Game.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BoardConstructionTest
    {
        public const string BadHeightMsg =
            "El valor del parámetro debe estar entre 3 y 99.\r\nParameter name: height";

        public const string BadWidthMsg =
            "El valor del parámetro debe estar entre 3 y 99.\r\nParameter name: width";

        public const string OddHeightMsg =
            "El valor del parámetro debe ser impar.\r\nParameter name: height";

        public const string OddWidthMsg =
            "El valor del parámetro debe ser impar.\r\nParameter name: width";

        [TestMethod]
        public void BoardsHeightIsTooLong()
        {
            AssertCatchesBadCtorParameters(101, 3, BadHeightMsg);
        }

        [TestMethod]
        public void BoardsHeightIsTooShort()
        {
            AssertCatchesBadCtorParameters(0, 3, BadHeightMsg);
        }

        [TestMethod]
        public void BoardsWidthIsTooNarrow()
        {
            AssertCatchesBadCtorParameters(3, 0, BadWidthMsg);
        }

        [TestMethod]
        public void BoardsWidthIsTooWide()
        {
            AssertCatchesBadCtorParameters(3, 101, BadWidthMsg);
        }

        [TestMethod]
        public void BoardsWidthShouldBeOdd()
        {
            AssertCatchesBadCtorParameters(5, 4, OddWidthMsg);
        }

        [TestMethod]
        public void DefaultPinLayout()
        {
            var board = new Board(3, 3);
            for (var r = 0; r < board.Height; r++)
            {
                for (var c = r % 2; c < board.Width; c += 2)
                {
                    var message = string.Format("Se esperaba Pin en ({0}, {1})", r, c);
                    Assert.IsTrue(board[r, c].IsPin, message);
                }
            }
        }

        [TestMethod]
        public void MakeMaxBoard()
        {
            var board = new Board(99, 99);
            Assert.AreEqual(99, board.Width, "board's width");
            Assert.AreEqual(99, board.Height, "board's height");
        }

        [TestMethod]
        public void MakeMinBoard()
        {
            var board = new Board(3, 3);
            Assert.AreEqual(3, board.Width, "board's width");
            Assert.AreEqual(3, board.Height, "board's height");
        }

        private static void AssertCatchesBadCtorParameters(
            int height, int width, string exceptionMessage)
        {
            TestTools.ExpectException(
                typeof(ArgumentOutOfRangeException), exceptionMessage,
                () =>
                    {
                        new Board(height, width);
                    });
        }
    }
}