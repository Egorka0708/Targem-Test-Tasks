using Calculator;
using NUnit.Framework;

namespace CalculatorTests
{
    public class Tests
    {
        private static ParseEquation equationParser = new ParseEquation();
        private SolveEquation solver = new SolveEquation(equationParser);

        [Test]
        public void EasyExpressions()
        {
            Assert.AreEqual(4f, solver.Calculate("2+2"));
            Assert.AreEqual(0f, solver.Calculate("2-2"));
            Assert.AreEqual(1f, solver.Calculate("2/2"));
            Assert.AreEqual(4f, solver.Calculate("2*2"));
        }

        [Test]
        public void HardExpressions()
        {
            Assert.AreEqual(6f, solver.Calculate("2+2*2"));
            Assert.AreEqual(1f, solver.Calculate("2-2/2"));
            Assert.AreEqual(0f, solver.Calculate("2+2-2*2"));
            Assert.AreEqual(0f, solver.Calculate("2+2-2/2*2*2"));
            Assert.AreEqual(-2f, solver.Calculate("2+2/2-2/2*2*2/2-2+(2+2/2-2/2*2*2/2-2)"));
        }

        [Test]
        public void FloatExpressions()
        {
            Assert.AreEqual((2f / 3f), solver.Calculate("2/3"));
            Assert.AreEqual((-2f / 3f), solver.Calculate("-2/3"));
            Assert.AreEqual(1.5f, solver.Calculate("3/2"));
            Assert.AreEqual(-1.5f, solver.Calculate("-3/2"));
            Assert.AreEqual(-1.5f, solver.Calculate("3/-2"));
            Assert.AreEqual(1.5f, solver.Calculate("-3/-2"));
        }

        [Test]
        public void FloatOperands()
        {
            Assert.AreEqual(5f, solver.Calculate("2.5+2,5"));
            Assert.AreEqual(3f, solver.Calculate("4.5*2/3"));
            Assert.AreEqual(2.5f, solver.Calculate("10/2.5-1.5"));
            Assert.AreEqual(-5.5f, solver.Calculate("-10/2.5-1.5"));
        }

        [Test]
        public void ExpressionsWithBrackets()
        {
            Assert.AreEqual(5f, solver.Calculate("2+(2*2+2)/2"));
            Assert.AreEqual(20f, solver.Calculate("(((2+2*2)/2+8)-1)*2"));
            Assert.AreEqual(17f, solver.Calculate("2+(10-2*2)/3+6+(5*2-3)"));
        }
    }
}