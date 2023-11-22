using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using _20231116;

namespace _20231116
{
    public class FormatHelper
    {

        internal void TextValueUpdate(int num, ViewModel viewModel)
        {
            if (viewModel != null)
            {
                if (!string.IsNullOrEmpty(viewModel.FullExpression) || viewModel._isText == true)
                {
                    viewModel.Expression = "";
                    viewModel.FullExpression = "";
                    viewModel.TextValue = "0";
                    viewModel._isFinal = false;
                    viewModel._isText = false;
                }

                // 이전 연산 결과가 표시되고 있는지 확인
                if (viewModel.TextValue == FormatNumberWithCommas(Convert.ToString(viewModel.PreviousResult), viewModel) || viewModel.TextValue == FormatNumberWithCommas(Convert.ToString(viewModel.DecimalPreviousResult), viewModel))
                {
                    // 새로운 숫자로 텍스트 업데이트
                    viewModel.TextValue = num.ToString();
                }
                else
                {
                    // 기존 값에 숫자 추가
                    var numericText = viewModel.TextValue.Replace(",", "").Replace(" ", "");
                    if (viewModel._isInt)
                    {
                        var updatedNumber = BigInteger.Parse(numericText + num);
                        viewModel.TextValue = FormatNumberWithCommas(Convert.ToString(updatedNumber), viewModel);
                    }
                    else
                    {
                        var updatedNumber = Decimal.Parse(numericText + num);
                        viewModel.TextValue = FormatNumberWithCommas(Convert.ToString(updatedNumber), viewModel);
                    }
                }
            }
        } //숫자 업데이트
        
        internal string FormatNumberWithCommas(string str, ViewModel viewModel)
        {

            if (viewModel._isInt)
            {
                BigInteger number = BigInteger.Parse(str);
                bool isNegative = number < 0;
                var numericText = number.ToString();
                var builder = new StringBuilder();

                int count = 0;
                int start = isNegative ? 1 : 0; // 음수인 경우 첫 번째 자리 ('-')를 건너뜁니다.

                for (int i = numericText.Length - 1; i >= start; i--)
                {
                    builder.Append(numericText[i]);
                    count++;

                    if (count % 3 == 0 && i != start)
                    {
                        builder.Append(",");
                    }
                }

                string formattedNumber = new string(builder.ToString().Reverse().ToArray());
                return isNegative ? "-" + formattedNumber : formattedNumber; // 음수인 경우 앞에 '-' 추가
            }
            else
            {
                Decimal number = Decimal.Parse(str);
                bool isNegative = number < 0;
                var numericText = number.ToString();
                string[] parts = numericText.Split('.');
                StringBuilder builder = new StringBuilder();

                // 음수 부호 확인 및 정수 부분 처리
                string integerPart = parts[0];

                if (integerPart.StartsWith("-"))
                {
                    isNegative = true;
                    integerPart = integerPart.Substring(1); // 음수 부호 제거
                }

                for (int i = integerPart.Length - 1; i >= 0; i--)
                {
                    builder.Insert(0, integerPart[i]);
                    if ((integerPart.Length - i) % 3 == 0 && i > 0)
                    {
                        builder.Insert(0, ",");
                    }
                }

                // 음수 부호 추가
                if (isNegative)
                {
                    builder.Insert(0, "-");
                }

                // 소수점과 소수 부분 처리
                if (parts.Length > 1)
                {
                    builder.Append(".");
                    builder.Append(parts[1]);
                }

                string formattedNumber = builder.ToString();
                return isNegative ? "-" + formattedNumber : formattedNumber; // 음수인 경우 앞에 '-' 추가
            }
        } //콤마 추가
        internal string EvaluateExpression(string expression, ViewModel viewModel)
        {
            System.Diagnostics.Debug.WriteLine("expression: " + expression);
            var outputQueue = ConvertToPostfix(expression, viewModel);
            return EvaluatePostfix(outputQueue, viewModel);
        } //중위 -> 후위 컨버터 및 필요 항목 추출 평가
        private Queue<string> ConvertToPostfix(string infix, ViewModel viewModel)
        {
            string result = infix.Replace(",", "");
            var outputQueue = new Queue<string>();
            var operatorStack = new Stack<string>();
            var tokens = Regex.Split(result, @"(\s+|\+|\-|\*|\/|\^|\(|\))");

            foreach (var token in tokens)
            {
                if (string.IsNullOrWhiteSpace(token)) continue;

                if (viewModel._isInt)
                {
                    if (BigInteger.TryParse(token, out BigInteger number))
                    {
                        outputQueue.Enqueue(token);
                    }
                    else if (IsOperator(token))
                    {
                        while (operatorStack.Count > 0 && GetPrecedence(operatorStack.Peek()) >= GetPrecedence(token))
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }
                        operatorStack.Push(token);
                    }
                    else if (token == "(")
                    {
                        operatorStack.Push(token);
                    }
                    else if (token == ")")
                    {
                        while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }
                        if (operatorStack.Count > 0)
                            operatorStack.Pop(); // 여는 괄호 제거
                    }
                }
                else
                {
                    if (Decimal.TryParse(token, out Decimal number))
                    {
                        outputQueue.Enqueue(token);
                    }
                    else if (IsOperator(token))
                    {
                        while (operatorStack.Count > 0 && GetPrecedence(operatorStack.Peek()) >= GetPrecedence(token))
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }
                        operatorStack.Push(token);
                    }
                    else if (token == "(")
                    {
                        operatorStack.Push(token);
                    }
                    else if (token == ")")
                    {
                        while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }
                        if (operatorStack.Count > 0)
                            operatorStack.Pop(); // 여는 괄호 제거
                    }
                }

            }

            while (operatorStack.Count > 0)
            {
                outputQueue.Enqueue(operatorStack.Pop());
            }

            return outputQueue;
        } //괄호 제거 및 우선순위 컨버터
        private string EvaluatePostfix(Queue<string> postfix, ViewModel viewModel)
        {
            if (viewModel._isInt)
            {
                var valuesStack = new Stack<BigInteger>();

                while (postfix.Count > 0)
                {

                    var token = postfix.Dequeue();
                    System.Diagnostics.Debug.WriteLine("Processing token: " + token); // 현재 토큰 로그 출력

                    if (BigInteger.TryParse(token, out BigInteger number))
                    {
                        valuesStack.Push(number);
                    }
                    else if (IsOperator(token))
                    {
                        if (valuesStack.Count < 2)
                        {
                            if (token == "-")
                            {
                                BigInteger val4 = valuesStack.Pop();
                                BigInteger val3 = 0;
                                valuesStack.Push(ApplyOperation(val3, val4, token));
                            }
                            else
                            {
                                throw new InvalidOperationException("Not enough values in the stack for the operation: " + token);
                            }
                        }

                        BigInteger val2 = valuesStack.Pop();
                        BigInteger val1 = valuesStack.Pop();
                        valuesStack.Push(ApplyOperation(val1, val2, token));
                    }

                    System.Diagnostics.Debug.WriteLine("Stack status: " + string.Join(", ", valuesStack)); // 스택 상태 로그 출력
                }

                if (valuesStack.Count != 1)
                {
                    throw new InvalidOperationException("Invalid expression - the stack should contain exactly one value.");
                }

                return Convert.ToString(valuesStack.Pop());
            }
            else
            {
                var valuesStack = new Stack<Decimal>();
                System.Diagnostics.Debug.WriteLine("postfix.Count: " + postfix.Count);
                while (postfix.Count > 0)
                {

                    var token = postfix.Dequeue();
                    System.Diagnostics.Debug.WriteLine("Processing token: " + token); // 현재 토큰 로그 출력

                    if (Decimal.TryParse(token, out Decimal number))
                    {
                        valuesStack.Push(number);
                    }
                    else if (IsOperator(token))
                    {
                        if (valuesStack.Count < 2)
                        {
                            if (token == "-")
                            {
                                Decimal val4 = valuesStack.Pop();
                                Decimal val3 = 0;
                                valuesStack.Push(ApplyOperation(val3, val4, token));
                            }
                            else
                            {
                                throw new InvalidOperationException("Not enough values in the stack for the operation: " + token);
                            }
                        }

                        Decimal val2 = valuesStack.Pop();
                        Decimal val1 = valuesStack.Pop();
                        valuesStack.Push(ApplyOperation(val1, val2, token));
                    }

                    System.Diagnostics.Debug.WriteLine("Stack status: " + string.Join(", ", valuesStack)); // 스택 상태 로그 출력
                }

                if (valuesStack.Count != 1)
                {
                    throw new InvalidOperationException("Invalid expression - the stack should contain exactly one value.");
                }

                return Convert.ToString(valuesStack.Pop());
            }

        } //연산 전 최종 정리
        private bool IsOperator(string token)
        {
            return new HashSet<string> { "+", "-", "×", "÷", "^" }.Contains(token);
        } //연산자 찾기
        private int GetPrecedence(string op)
        {
            switch (op)
            {
                case "+":
                case "-":
                    return 1;
                case "×":
                case "÷":
                    return 2;
                case "^":
                    return 3;
                default:
                    return 0;
            }
        } //우선순위
        private BigInteger ApplyOperation(BigInteger val1, BigInteger val2, string operation)
        {
            switch (operation)
            {
                case "+":
                    return val1 + val2;
                case "-":
                    // 첫 번째 피연산자가 없거나 0이면 두 번째 피연산자를 음수로 변환
                    if (val1 == 0)
                        return -val2;
                    else
                        return val1 - val2;
                case "×":
                    return val1 * val2;
                case "÷":
                    if (val2 == 0) throw new DivideByZeroException();
                    return val1 / val2;
                case "^":
                    return BigInteger.Pow(val1, (int)val2);
                default:
                    throw new NotSupportedException($"Unsupported operation: {operation}");
            }
        } //Biginteager 연산
        private Decimal ApplyOperation(Decimal val1, Decimal val2, string operation)
        {
            switch (operation)
            {
                case "+":
                    return val1 + val2;
                case "-":
                    // 첫 번째 피연산자가 없거나 0이면 두 번째 피연산자를 음수로 변환
                    if (val1 == 0)
                        return -val2;
                    else
                        return val1 - val2;
                case "×":
                    return val1 * val2;
                case "÷":
                    if (val2 == 0) throw new DivideByZeroException();
                    return val1 / val2;
                //case "^":
                //  return BigInteger.Pow(val1, (int)val2);
                default:
                    throw new NotSupportedException($"Unsupported operation: {operation}");
            }
        } //Decimal 연산
    }
}
