using System;
using System.Collections.Generic;


namespace PostfixCalculator
{
    class PostfixCalculator
    {
        public static void Main()
        {
            var tests = new string[]
            {
                "( 1 + ( 3 - 4 ) + 5 ) * 6",
                "( ( ( 2 - 3 ) * 4 ) + 1 ) / 3",
                "1 + 2 ^ ( 1 / 2 )",
                "sin 7 ^ 2 + cos 7 ^ 2",
                "( sin 3 / cos 3 ) * ctg 3",
                "sin ( 3 + 0,1415 )",
                "2 * sin 3 * cos 3"
            };
            var answers = new double[]
            {
                30,
                -1,
                1+Math.Sqrt(2),
                1,
                1,
                0,
                Math.Sin(6)
            };
            var i = 0;
            foreach(var e in tests)
            {
                
                Console.WriteLine(e);
                var notation = GetPostfixNotation(e);
                Console.WriteLine(notation);
                Console.WriteLine("Expected {0}, Actual {1}", answers[i], Calculate(notation));
                Console.WriteLine();
                i++;
            }

        }
        public static int GetOperationPriority(string operation)
        {
            switch (operation)
            {
                case "(":
                    return 10;
                case ")":
                    return 10;
                case "+":
                    return 0;
                case "-":
                    return 0;
                case "*":
                    return 1;
                case "/":
                    return 1;
                case "^":
                    return 1;
                case "sin":
                    return 2;
                case "cos":
                    return 2;
                case "tg":
                    return 2;
                case "ctg":
                    return 2;
            }
            throw new Exception("Unknown operator");
        }
        public static string GetPostfixNotation(string str)
        {
            var instructions = str.Split(' ');
            var operationsStack = new Stack<string>();
            string result = "";
            foreach(var e in instructions)
            {
                if (double.TryParse(e, out double number))
                    result = string.Join(' ', result, number.ToString());
                else if (e == ")")
                {
                    while (operationsStack.Peek() != "(")
                        result = string.Join(' ', result, operationsStack.Pop());
                    operationsStack.Pop();
                }
                else
                {
                    if (operationsStack.Count != 0)
                    {
                        var operation = operationsStack.Peek();
                        if (GetOperationPriority(e) <= GetOperationPriority(operation) && (operation != "("))
                        {
                            result = string.Join(' ', result, operationsStack.Pop());
                        }
                    }
                    operationsStack.Push(e);
                }
            }
            while(operationsStack.Count>0)
            {
                result = string.Join(' ', result, operationsStack.Pop());
            }
            return result.Remove(0,1);
        }

        public static double Calculate(string str)
        {
            var instructions = str.Split(' ');
            var binaryOperations = new Dictionary<string, Func<double, double, double>>();
            var unaryOperations = new Dictionary<string, Func<double, double>>();
            binaryOperations.Add("+", (y, x) => x + y);
            binaryOperations.Add("-", (y, x) => x - y);
            binaryOperations.Add("*", (y, x) => x * y);
            binaryOperations.Add("/", (y, x) => x / y);

            binaryOperations.Add("^", (y, x) => Math.Pow(x, y));
            unaryOperations.Add("sin", (x) => Math.Sin(x));
            unaryOperations.Add("cos", (x) => Math.Cos(x));
            unaryOperations.Add("tg", (x) => Math.Tan(x));
            unaryOperations.Add("ctg", (x) => 1 / Math.Tan(x));

            var numbersStack = new Stack<double>();
            foreach (var e in instructions)
            {
                if (double.TryParse(e, out double number))
                    numbersStack.Push(number);
                else if (binaryOperations.ContainsKey(e))
                    numbersStack.Push(binaryOperations[e](numbersStack.Pop(), numbersStack.Pop()));
                else if (unaryOperations.ContainsKey(e))
                    numbersStack.Push(unaryOperations[e](numbersStack.Pop()));
                else
                    throw new ArgumentException();
            }
            return numbersStack.Pop();
        }
    }
}
