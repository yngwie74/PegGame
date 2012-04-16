namespace Game.Test
{
    using System.Collections.Generic;
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConfigReaderTest
    {
        #region Public Methods and Operators

        [TestMethod]
        public void ReadBoardHeightAndWidth()
        {
            var config = ReadConfigFrom("5  6");
            Assert.AreEqual(5, config.Height, "Altura");
            Assert.AreEqual(6, config.Width, "Ancho");
        }

        [TestMethod]
        public void ReadGoalColumn()
        {
            var config = ReadConfigFrom("1  1\n3");
            Assert.AreEqual(3, config.GoalColumn, "Columna meta");
        }

        [TestMethod]
        public void ReadMissingPins()
        {
            var config = ReadConfigFrom("1 1\n3\n3\n1 3\n2 2\n3 5");

            var expected = new[]
                {
                    new Coord(1, 3), new Coord(2, 2), new Coord(3, 5)
                };

            AssertCoordsAsExpected(expected, config.MissingPins);
        }

        #endregion

        #region Methods

        private static void AssertAreCoordsEqual(
            IList<Coord> expected, IList<Coord> actual, int i)
        {
            var message = string.Format("Elemento #{0}", i);
            Assert.AreEqual(expected[i], actual[i], message);
        }

        private static void AssertCoordsAsExpected(IList<Coord> expected, IList<Coord> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count, "# Elementos");
            for (var i = 0; i < expected.Count; i++)
            {
                AssertAreCoordsEqual(expected, actual, i);
            }
        }

        private static Config ReadConfigFrom(string configStr)
        {
            var reader = new StringReader(configStr);
            return new ConfigReader(reader).ReadConfig();
        }

        #endregion
    }
}