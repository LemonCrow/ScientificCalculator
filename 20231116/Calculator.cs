using System;
using System.Numerics;

namespace _20231116
{
    public class Calculator
    {
        public void FourBasicOperations(string oper, ViewModel viewModel)
        {
            FormatHelper formatHelper = new FormatHelper();
            if (viewModel != null)
            {
                if (!string.IsNullOrEmpty(viewModel.FullExpression))
                {
                    viewModel.Expression = "";
                    viewModel.FullExpression = "";
                    viewModel._isFinal = false;
                }

                // 이전 결과를 현재 값으로 설정
                if (viewModel._isInt)
                {
                    viewModel.PreviousNumber = BigInteger.Parse(viewModel.TextValue.Replace(",", ""));
                }
                else
                {
                    viewModel.DecimalPreviousNumber = Decimal.Parse(viewModel.TextValue.Replace(",", ""));
                }

                if (viewModel._isInt)
                {
                    if (viewModel.PreviousResult != viewModel.PreviousNumber)
                    {
                        System.Diagnostics.Debug.WriteLine("viewModel.Expression, viewModel.TextValue: " + viewModel.Expression + ", " + viewModel.TextValue);
                        // 현재 수식을 평가하여 결과 저장               
                        var result = formatHelper.EvaluateExpression(viewModel.Expression + viewModel.TextValue, viewModel);
                        viewModel.PreviousResult = BigInteger.Parse(result);

                        // 연산자 추가 및 TextValue 업데이트
                        viewModel.Expression += viewModel.TextValue + oper;
                        viewModel.TextValue = formatHelper.FormatNumberWithCommas(Convert.ToString(result), viewModel);

                    }
                    else
                    {
                        if (viewModel.Expression.Split(' ').Length > 2)
                        {
                            System.Diagnostics.Debug.WriteLine(viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2]);
                            if (viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2] == ")")
                            {
                                viewModel.Expression += oper;
                            }
                        }

                    }
                }
                else
                {
                    if (viewModel.DecimalPreviousResult != viewModel.DecimalPreviousNumber)
                    {
                        // 현재 수식을 평가하여 결과 저장               
                        var result = formatHelper.EvaluateExpression(viewModel.Expression + viewModel.TextValue, viewModel);
                        viewModel.DecimalPreviousResult = Decimal.Parse(result);

                        // 연산자 추가 및 TextValue 업데이트
                        viewModel.Expression += viewModel.TextValue + oper;
                        viewModel.TextValue = formatHelper.FormatNumberWithCommas(Convert.ToString(result), viewModel);
                    }
                    else
                    {
                        if (viewModel.Expression.Split(' ').Length > 2)
                        {
                            System.Diagnostics.Debug.WriteLine(viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2]);
                            if (viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2] == ")")
                            {
                                viewModel.Expression += oper;
                            }
                        }

                    }
                }
            }
        }//사칙연산

        internal void Naturalis(string logText, ViewModel viewModel)
        {
            FormatHelper formatHelper = new FormatHelper();
            if(logText == "e")
            {
                viewModel._isInt = false;
                viewModel.TextValue = formatHelper.FormatNumberWithCommas(Convert.ToString(Math.E), viewModel);
            }
            else
            {
                viewModel._isInt = false;
                var result = formatHelper.FormatNumberWithCommas(Convert.ToString(Math.Log(Convert.ToDouble(viewModel.TextValue))), viewModel);
                viewModel.TextValue = result;
            }
        } //자연로그 e, ln 

    }
}
