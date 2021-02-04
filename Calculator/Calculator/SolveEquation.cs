using System;
using System.Collections.Generic;

namespace Calculator
{
    public class SolveEquation
    {
        private readonly ParseEquation equationParser;
        private static readonly Dictionary<string, int> operatorsPriority = new Dictionary<string, int>()
        {
            { "(", 0 },
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 }
        };

        public SolveEquation(ParseEquation equationParser)
        {
            this.equationParser = equationParser;
        }

        /// <summary>
        /// Метод, проверяющий является ли унарным токен, находящийся во входном листе на заданном индексе
        /// </summary>
        /// <param name="tokens">Лист токенов</param>
        /// <param name="index">Индекс проверяемого токена</param>
        /// <returns>true если токен является унарным, false если токен не является унарным</returns>
        private bool IsUnaryOperator(List<IToken> tokens, int index)
        {
            return index == 0 || tokens[index - 1] is OperatorToken && tokens[index - 1].GetStringValue() != ")";
        }

        /// <summary>
        /// Метод, заменяющий унарные операторы на бинарные
        /// </summary>
        /// <param name="tokens">Лист токенов</param>
        /// <exception cref="ArgumentException">Выбрасывается, когда унарный минус стоит в конце строки или перед другим оператором</exception>
        private void RemoveUnaryOperators(List<IToken> tokens)
        {
            for (int i = 0; i < tokens.Count; ++i)
            {
                IToken token = tokens[i];
                if (token.GetStringValue() == "-")
                {
                    if (IsUnaryOperator(tokens, i))
                    {
                        if (i == tokens.Count - 1 || !(tokens[i + 1] is NumberToken))
                            throw new ArgumentException("Invalid unary minus");
                        tokens.Insert(i, new OperatorToken("("));
                        tokens.Insert(i + 1, new NumberToken("0"));
                        tokens.Insert(i + 4, new OperatorToken(")"));
                    }
                }
                else if (token.GetStringValue() == "+")
                {
                    if (IsUnaryOperator(tokens, i))
                    {
                        tokens.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// Метод, преобразующий лист токенов в обратную польскую запись (постфиксную)
        /// </summary>
        /// <param name="tokens">Лист токенов в инфиксном порядке без унарных операторов</param>
        /// <returns>Лист токенов в обратной польской записи (постфиксной)</returns>
        /// <exception cref="ArgumentException">Выбрасывается при неправильном расположении скобок</exception>
        private List<IToken> GetReversedPolishNotation(List<IToken> tokens)
        {
            List<IToken> result = new List<IToken>();
            Stack<IToken> stack = new Stack<IToken>();
            foreach (IToken token in tokens)
            {
                if (token is NumberToken)
                    result.Add(token);
                else
                {
                    if (token.GetStringValue() == "(")
                        stack.Push(token);
                    else if (token.GetStringValue() == ")")
                    {
                        while (stack.Peek().GetStringValue() != "(")
                        {
                            if (stack.Count == 0)
                                throw new ArgumentException("Invalid brackets");
                            result.Add(stack.Pop());
                        }
                        stack.Pop();
                    }
                    else if (operatorsPriority.ContainsKey(token.GetStringValue()))
                    {
                        while (stack.Count != 0)
                        {
                            if (operatorsPriority[stack.Peek().GetStringValue()] < operatorsPriority[token.GetStringValue()])
                                break;
                            result.Add(stack.Pop());
                        }
                        stack.Push(token);
                    }
                }
            }
            while (stack.Count > 0)
            {
                if (stack.Peek().GetStringValue() == "(")
                    throw new ArgumentException("Invalid brackets");
                result.Add(stack.Pop());
            }
            return result;
        }

        /// <summary>
        /// Метод, вычисляющий результат выражения по обратной польской записи (постфиксной)
        /// </summary>
        /// <param name="tokens">Лист токенов в обратной польской записи (постфиксном)</param>
        /// <returns>Результат вычисления выражения</returns>
        /// <exception cref="ArgumentException">Выбрасывается, когда в постфиксной записи остаются неиспользованные операторы
        /// или когда не хватает операндов для применения очередного оператора</exception>
        private float CalculateReversedPolishNotation(List<IToken> tokens)
        {
            Stack<IToken> stack = new Stack<IToken>();
            foreach (IToken token in tokens)
            {
                if (token is NumberToken)
                    stack.Push(token);
                else
                {
                    if (stack.Count < 2)
                        throw new ArgumentException("Invalid equation");
                    IToken a = stack.Pop();
                    IToken b = stack.Pop();
                    float result = CalculateExpression(b.GetFloatValue(), a.GetFloatValue(), token.GetStringValue());
                    stack.Push(new NumberToken(result));
                }
            }
            if (stack.Count != 1)
                throw new ArgumentException("Invalid equation");
            return stack.Peek().GetFloatValue();
        }

        /// <summary>
        /// Метод, вычисляющий результат выражения
        /// </summary>
        /// <param name="a">Первое число в выражении</param>
        /// <param name="b">Второе число в выражении</param>
        /// <param name="sign">Арифметический знак в выражении</param>
        /// <returns>Результат вычисления выражения</returns>
        /// <exception cref="ArgumentException">Выбрасывается, когда встречается неопознанный знак операции</exception>
        private float CalculateExpression(float a, float b, string sign)
        {
            float result;
            switch (sign)
            {
                case "+":
                    {
                        result = a + b;
                        break;
                    }
                case "-":
                    {
                        result = a - b;
                        break;
                    }
                case "*":
                    {
                        result = a * b;
                        break;
                    }
                case "/":
                    {
                        result = a / b;
                        break;
                    }
                default:
                    throw new ArgumentException("Invalid equation");
            }
            return result;
        }

        public float Calculate(string equation)
        {
            List<IToken> tokens = equationParser.Parse(equation);
            RemoveUnaryOperators(tokens);
            List<IToken> reversedPolishNotation = GetReversedPolishNotation(tokens);
            return CalculateReversedPolishNotation(reversedPolishNotation);
        }
    }
}
