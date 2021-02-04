using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Calculator
{
    public interface IToken
    {
        string GetStringValue();
        float GetFloatValue();
    }

    public class NumberToken : IToken
    {
        private readonly float value;

        public NumberToken(string value)
        {
            value = value.Replace('.', ',');
            if (!float.TryParse(value, out this.value))
            {
                throw new ArgumentException($"Can not parse {value} as number");
            }
        }

        public NumberToken(float value)
        {
            this.value = value;
        }

        public string GetStringValue()
        {
            return value.ToString();
        }

        public float GetFloatValue()
        {
            return value;
        }
    }

    public class OperatorToken : IToken
    {
        private readonly string value;

        public OperatorToken(string value)
        {
            this.value = value;
        }

        public string GetStringValue()
        {
            return value;
        }

        public float GetFloatValue()
        {
            throw new InvalidOperationException("Operator can not be cast to number");
        }
    }

    public class ParseEquation
    {
        private static readonly List<char> possibleOperators = new List<char> { '+', '-', '*', '/', '(', ')' };

        /// <summary>
        /// Метод, разбирающий исходную строку на токены, содержащие числа и операторы
        /// </summary>
        /// <param name="equation">Исходная строка, содержащая выражение</param>
        /// <returns>Лист токенов</returns>
        /// <exception cref="ArgumentException">Выбрасывается, когда число с плавающей точкой содержит
        /// две точки/запятые или когда строка содержит неизвестный символ</exception>
        public List<IToken> Parse(string equation)
        {
            List<IToken> result = new List<IToken>();
            equation = Regex.Replace(equation, "\\s+", string.Empty);
            bool isFloat = false;
            StringBuilder numberBuilder = new StringBuilder();
            foreach (char c in equation)
            {
                if (char.IsDigit(c) || c == '.' || c == ',')
                {
                    if (c == '.' || c == ',')
                    {
                        if (isFloat)
                            throw new ArgumentException("Can't parse with second dot!");
                        isFloat = true;
                    }

                    numberBuilder.Append(c);
                }
                else if (possibleOperators.Contains(c))
                {
                    if (numberBuilder.Length > 0)
                    {
                        result.Add(new NumberToken(numberBuilder.ToString()));
                        isFloat = false;
                        numberBuilder.Clear();
                    }

                    result.Add(new OperatorToken(c.ToString()));
                }
                else
                {
                    throw new ArgumentException($"Unknown character: {c}");
                }
            }

            if (numberBuilder.Length > 0)
                result.Add(new NumberToken(numberBuilder.ToString()));
            return result;
        }
    }
}
