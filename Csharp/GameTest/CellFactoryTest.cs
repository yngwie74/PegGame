namespace Game.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CellFactoryTest
    {
        [TestMethod]
        public void MakeBlank()
        {
            var cell = CellFactory.MakeBlank();
            Assert.AreSame(cell, CellFactory.Blank);
        }

        [TestMethod]
        public void MakeLeftBorder()
        {
            var cell = CellFactory.MakePin(0, 2);
            Assert.AreSame(cell, CellFactory.LeftBorder);
        }

        [TestMethod]
        public void MakePin()
        {
            var cell = CellFactory.MakePin(1, 2);
            Assert.AreSame(cell, CellFactory.MiddlePin);
        }

        [TestMethod]
        public void MakeRightBorder()
        {
            var cell = CellFactory.MakePin(2, 2);
            Assert.AreSame(cell, CellFactory.RightBorder);
        }
    }
}