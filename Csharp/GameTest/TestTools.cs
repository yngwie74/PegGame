namespace Game.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class TestTools
    {
        #region Public Methods and Operators

        /// <summary>
        /// Se usa este método, ya que el atributo <seealso cref="ExpectedExceptionAttribute"/> no compara los mensajes de error generados.
        /// </summary>
        /// <param name="exceptionType">El tipo de excepción esperado.</param>
        /// <param name="exceptionMessage">El mensaje de excepción esperado.</param>
        /// <param name="action">La acción que se espera produsca la excepción.</param>
        public static void ExpectException(Type exceptionType, string exceptionMessage, Action action)
        {
            var message = string.Format("Expected exception of type {0}", exceptionType.Name);
            try
            {
                action.Invoke();
                Assert.Fail(message);
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, exceptionType, "Expected exception type");
                Assert.AreEqual(exceptionMessage, e.Message, "Expected exception message");
            }
        }

        #endregion
    }
}