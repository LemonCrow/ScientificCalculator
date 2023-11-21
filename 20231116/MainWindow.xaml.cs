using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20231116
{
    //숫자 바인딩
    public class ViewModel : INotifyPropertyChanged
    {
        private string _textValue = "0";
        private string _expression = "";
        public bool _isFinal = false;
        public bool _isInt = true;


        public string TextValue
        {
            get { return _textValue; }
            set
            {
                if (_textValue != value)
                {
                    _textValue = value;
                    OnPropertyChanged(nameof(TextValue));
                }
            }
        }

        public string Expression
        {
            get { return _expression; }
            set
            {
                if (_expression != value)
                {
                    _expression = value;
                    OnPropertyChanged(nameof(Expression));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //BigInteger 관련 처리

        private BigInteger previousNumber;
        public BigInteger PreviousNumber
        {
            get { return previousNumber; }
            set
            {
                if (previousNumber != value)
                {
                    previousNumber = value;
                    OnPropertyChanged(nameof(PreviousNumber));
                }
            }
        }

        //Decimal 관련 처리

        private decimal decimalPreviousNumber;
        public decimal DecimalPreviousNumber
        {
            get { return decimalPreviousNumber; }
            set
            {
                if (decimalPreviousNumber != value)
                {
                    decimalPreviousNumber = value;
                    OnPropertyChanged(nameof(PreviousNumber));
                }
            }
        }

        private string operation = string.Empty;
        public string Operation
        {
            get { return operation; }
            set
            {
                if (operation != value)
                {
                    operation = value;
                    OnPropertyChanged(nameof(Operation));
                }
            }
        }

        //BigInteger 관련 처리

        private BigInteger _previousResult;
        public BigInteger PreviousResult
        {
            get { return _previousResult; }
            set
            {
                if (_previousResult != value)
                {
                    _previousResult = value;
                    OnPropertyChanged(nameof(PreviousResult));
                }
            }
        }

        //Decimal 관련 처리

        private decimal _decimalPreviousResult;
        public decimal DecimalPreviousResult
        {
            get { return _decimalPreviousResult; }
            set
            {
                if (_decimalPreviousResult != value)
                {
                    _decimalPreviousResult = value;
                    OnPropertyChanged(nameof(DecimalPreviousResult));
                }
            }
        }

        private string _fullExpression = "";
        public string FullExpression
        {
            get { return _fullExpression; }
            set
            {
                if (_fullExpression != value)
                {
                    _fullExpression = value;
                    OnPropertyChanged(nameof(FullExpression));
                }
            }
        }

    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] angles = { "GRAD", "DEG", "RAD" };
        private int anglesInt = 1;


        private BigInteger stringNumber = 0;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel();
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);

        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = DataContext as ViewModel;

            if (viewModel == null) return;

            // 숫자 키 입력 처리
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                // 키보드의 상단 숫자 키 입력 처리
                TextValueUpdate((e.Key - Key.D0));
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                // 숫자 패드 키 입력 처리
                TextValueUpdate((e.Key - Key.NumPad0));
            }
            else if (e.Key == Key.Add)
            {
                // 추가 로직: '+' 연산 처리
                // 예시: viewModel.PerformOperation('+');
            }
            // 여기에 다른 연산자 및 기능에 대한 키 처리 로직 추가
            // 예: 빼기, 곱하기, 나누기, 등호, 백스페이스 등

            // ViewModel의 TextValue가 자동으로 업데이트되도록 구현
        }

        private void ExpressionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                // 수식 길이에 따라 폰트 크기 조정
                textBox.FontSize = CalculateFontSizeForExpression(textBox.Text);
            }
        }

        private double CalculateFontSizeForExpression(string text)
        {
            // 예시: 길이에 따라 폰트 크기 조정 로직
            if (text.Length < 20) return 15;
            if (text.Length < 40) return 13;
            if (text.Length < 60) return 10;

            return 14; // 최소 폰트 크기
        }

        //각도
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            anglesInt = (anglesInt + 1) % angles.Length;

            button.Content = angles[anglesInt];

        }

        //텍스트 사이즈 자동 조절
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {

                double newFontSize = CalculateFontSize(textBox.Text);
                textBox.FontSize = newFontSize;
            }
        }

        private double CalculateFontSize(string text)
        {

            if (text.Length < 10) return 20;
            if (text.Length < 20) return 18;
            if (text.Length < 30) return 16;

            return 12;
        }

        //F-E
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        //삼각법
        private void OpenTriPopupButton_Click(object sender, RoutedEventArgs e)
        {
            Trigonometry.IsOpen = !Trigonometry.IsOpen;
            e.Handled = true;
        }

        private void Trigonometry_LostFocus(object sender, RoutedEventArgs e)
        {
            Trigonometry.IsOpen = false;
        }


        //팝업처리
        private void Popup_Closed(object sender, EventArgs e)
        {
            Trigonometry.IsOpen = false;
            function.IsOpen = false;
        }

        //삼각법-2nd
        private void ChangeTrigonometry2nd(object sender, EventArgs e)
        {
            UpdateButtonTexts();
        }

        //삼각법-hyp
        private void ChangeTrigonometryHyp(object sender, EventArgs e)
        {
            UpdateButtonTexts();
        }

        //삼각법 - 버튼들 조건 체크 및 텍스트 변경 << 여기에 나중에 수식 관련 추가하면됨
        private void UpdateButtonTexts()
        {
            //다 on
            if (Trigonometry2nd.IsChecked == true && TrigonometryHyp.IsChecked == true)
            {
                sin.Content = "sinh^-1";
                cos.Content = "cosh^-1";
                tan.Content = "tanh^-1";

                sec.Content = "sech^-1";
                csc.Content = "csch^-1";
                cot.Content = "coth^-1";
            }
            //다 off
            else if (Trigonometry2nd.IsChecked == false && TrigonometryHyp.IsChecked == false)
            {
                sin.Content = "sin";
                cos.Content = "cos";
                tan.Content = "tan";

                sec.Content = "sec";
                csc.Content = "csc";
                cot.Content = "cot";
            }
            //2nd만 on
            else if (Trigonometry2nd.IsChecked == true && TrigonometryHyp.IsChecked == false)
            {
                sin.Content = "sin^-1";
                cos.Content = "cos^-1";
                tan.Content = "tan^-1";

                sec.Content = "sec^-1";
                csc.Content = "csc^-1";
                cot.Content = "cot^-1";
            }
            //hyp만 on
            else
            {
                sin.Content = "sech";
                cos.Content = "cosh";
                tan.Content = "tanh";

                sec.Content = "sech";
                csc.Content = "csch";
                cot.Content = "coth";
            }
        }

        //함수
        private void openFunPopupButton_Click(object sender, RoutedEventArgs e)
        {
            function.IsOpen = !function.IsOpen;
            e.Handled = true;
        }


        //넘패드 2nd 체크
        private void NumPad2nd_Checked(object sender, RoutedEventArgs e)
        {

            sqr.Content = "x³";
            root.Content = "3√x";
            square.Content = "y√x";
            square10.Content = "2^x";
            //"log_y(x)"라고 읽으며, 이는 "x의 y 밑 로그(logarithm)"를 나타냅니다. 로그 함수는 지수 함수의 역함수로, 어떤 수를 다른 수의 거듭제곱으로 나타낼 수 있는 횟수를 계산합니다. 즉, "y의 몇 제곱이 x와 같은가?"를 나타내는 수학적 연산입니다.
            //나중에 수식짤때 참고
            log.Content = "log_y(x)";
            nSquare.Content = "e^x";



        }

        //넘패드 2nd 체크헤제
        private void NumPad2nd_Unchecked(object sender, RoutedEventArgs e)
        {

            sqr.Content = "x²";
            root.Content = "2√x";
            square.Content = "x^y";
            square10.Content = "10^x";
            log.Content = "log";
            nSquare.Content = "ln";
        }

        //넘패드0 ~ 9
        private void Button_Click_num1(object sender, RoutedEventArgs e)
        {
            TextValueUpdate(1);
        }

        private void Button_Click_num2(object sender, RoutedEventArgs e)
        {
            TextValueUpdate(2);
        }

        private void Button_Click_num3(object sender, RoutedEventArgs e)
        {
            TextValueUpdate(3);
        }

        private void Button_Click_num4(object sender, RoutedEventArgs e)
        {
            TextValueUpdate(4);
        }

        private void Button_Click_num5(object sender, RoutedEventArgs e)
        {
            TextValueUpdate(5);
        }
        private void Button_Click_num6(object sender, RoutedEventArgs e)
        {
            TextValueUpdate(6);
        }

        private void Button_Click_num7(object sender, RoutedEventArgs e)
        {
            TextValueUpdate(7);
        }

        private void Button_Click_num8(object sender, RoutedEventArgs e)
        {
            TextValueUpdate(8);
        }

        private void Button_Click_num9(object sender, RoutedEventArgs e)
        {
            TextValueUpdate(9);
        }

        private void Button_Click_num0(object sender, RoutedEventArgs e)
        {
            TextValueUpdate(0);
        }

        private void TextValueUpdate(int num)
        {
            var viewModel = DataContext as ViewModel;
            if (viewModel != null)
            {
                if (!string.IsNullOrEmpty(viewModel.FullExpression))
                {
                    viewModel.Expression = "";
                    viewModel.FullExpression = "";
                    viewModel.TextValue = "0";
                    viewModel._isFinal = false;
                }
                
                // 이전 연산 결과가 표시되고 있는지 확인
                if (viewModel.TextValue == FormatNumberWithCommas(Convert.ToString(viewModel.PreviousResult)) || viewModel.TextValue == FormatNumberWithCommas(Convert.ToString(viewModel.DecimalPreviousResult)))
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
                        viewModel.TextValue = FormatNumberWithCommas(Convert.ToString(updatedNumber));
                    }
                    else
                    {
                        var updatedNumber = Decimal.Parse(numericText + num);
                        viewModel.TextValue = FormatNumberWithCommas(Convert.ToString(updatedNumber));
                    }
                }
            }
        }



        //콤마 추가
        private string FormatNumberWithCommas(string str)
        {

            var viewModel = DataContext as ViewModel;
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
        }



        // 숫자 하나씩 지우기
        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            if (viewModel != null && viewModel.TextValue.Length > 0)
            {
                var numericText = viewModel.TextValue.Replace(",", "").Replace(" ", "");
                if (viewModel._isInt)
                {
                    numericText = numericText.Substring(0, numericText.Length - 1);
                }
                else
                {

                    numericText = numericText.Substring(0, numericText.Length - 1);
                    if (!numericText.Contains('.'))
                    {
                        viewModel._isInt = true;
                    }
                    
                }

                if (!string.IsNullOrEmpty(numericText))
                {
                    if (viewModel._isInt)
                    {
                        BigInteger.TryParse(numericText, out BigInteger updatedNumber);
                        viewModel.TextValue = FormatNumberWithCommas(Convert.ToString(updatedNumber));
                    }
                    else
                    {
                        Decimal.TryParse(numericText, out Decimal updatedNumber);
                        viewModel.TextValue = FormatNumberWithCommas(Convert.ToString(updatedNumber));
                    }
                }
                else
                {
                    viewModel.TextValue = "0";
                }
            }
        }

        // 연산 버튼 클릭 이벤트 (더하기)
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
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
                        // 현재 수식을 평가하여 결과 저장               
                        var result = EvaluateExpression(viewModel.Expression + viewModel.TextValue);
                        viewModel.PreviousResult = BigInteger.Parse(result);

                        // 연산자 추가 및 TextValue 업데이트
                        viewModel.Expression += viewModel.TextValue + " + ";
                        viewModel.TextValue = FormatNumberWithCommas(Convert.ToString(result));
                    }
                    else
                    {
                        if (viewModel.Expression.Split(' ').Length > 2)
                        {
                            System.Diagnostics.Debug.WriteLine(viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2]);
                            if (viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2] == ")")
                            {
                                viewModel.Expression += " + ";
                            }
                        }

                    }
                }
                else
                {
                    if (viewModel.DecimalPreviousResult != viewModel.DecimalPreviousNumber)
                    {
                        // 현재 수식을 평가하여 결과 저장               
                        var result = EvaluateExpression(viewModel.Expression + viewModel.TextValue);
                        viewModel.DecimalPreviousResult = Decimal.Parse(result);

                        // 연산자 추가 및 TextValue 업데이트
                        viewModel.Expression += viewModel.TextValue + " + ";
                        viewModel.TextValue = FormatNumberWithCommas(Convert.ToString(result));
                    }
                    else
                    {
                        if (viewModel.Expression.Split(' ').Length > 2)
                        {
                            System.Diagnostics.Debug.WriteLine(viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2]);
                            if (viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2] == ")")
                            {
                                viewModel.Expression += " + ";
                            }
                        }

                    }
                }

            }
        }



        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
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

                    if (viewModel.PreviousResult != viewModel.PreviousNumber)
                    {
                        // 현재 수식을 평가하여 결과 저장               
                        var result = EvaluateExpression(viewModel.Expression + viewModel.TextValue);
                        if (viewModel._isInt)
                        {
                            viewModel.PreviousResult = BigInteger.Parse(result);
                        }
                        else
                        {
                            viewModel.DecimalPreviousNumber = Decimal.Parse(result);
                        }

                        // 연산자 추가 및 TextValue 업데이트
                        viewModel.Expression += viewModel.TextValue + " - ";
                        viewModel.TextValue = FormatNumberWithCommas(Convert.ToString(result));
                    }
                    else
                    {
                        if (viewModel.Expression.Split(' ').Length > 2)
                        {
                            System.Diagnostics.Debug.WriteLine(viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2]);
                            if (viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2] == ")")
                            {
                                viewModel.Expression += " - ";
                            }
                        }

                    }
                }
                else
                {
                    viewModel.DecimalPreviousNumber = Decimal.Parse(viewModel.TextValue.Replace(",", ""));

                    if (viewModel.DecimalPreviousResult != viewModel.DecimalPreviousNumber)
                    {
                        // 현재 수식을 평가하여 결과 저장               
                        var result = EvaluateExpression(viewModel.Expression + viewModel.TextValue);
                        if (viewModel._isInt)
                        {
                            viewModel.PreviousResult = BigInteger.Parse(result);
                        }
                        else
                        {
                            viewModel.DecimalPreviousResult = Decimal.Parse(result);
                        }

                        // 연산자 추가 및 TextValue 업데이트
                        viewModel.Expression += viewModel.TextValue + " - ";
                        viewModel.TextValue = FormatNumberWithCommas(Convert.ToString(result));
                    }
                    else
                    {
                        if (viewModel.Expression.Split(' ').Length > 2)
                        {
                            System.Diagnostics.Debug.WriteLine(viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2]);
                            if (viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2] == ")")
                            {
                                viewModel.Expression += " - ";
                            }
                        }

                    }
                }

                
            }
        }

        private void MulButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
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


                    if (viewModel.PreviousResult != viewModel.PreviousNumber)
                    {
                        // 현재 수식을 평가하여 결과 저장               
                        var result = EvaluateExpression(viewModel.Expression + viewModel.TextValue);
                        if (viewModel._isInt)
                        {
                            viewModel.PreviousResult = BigInteger.Parse(result);
                        }
                        else
                        {
                            viewModel.DecimalPreviousNumber = Decimal.Parse(result);
                        }

                        // 연산자 추가 및 TextValue 업데이트
                        viewModel.Expression += viewModel.TextValue + " × ";
                        viewModel.TextValue = FormatNumberWithCommas(Convert.ToString(result));
                    }
                    else
                    {
                        if (viewModel.Expression.Split(' ').Length > 2)
                        {
                            System.Diagnostics.Debug.WriteLine(viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2]);
                            if (viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2] == ")")
                            {
                                viewModel.Expression += " × ";
                            }
                        }

                    }
                }
                else
                {
                    viewModel.DecimalPreviousNumber = Decimal.Parse(viewModel.TextValue.Replace(",", ""));


                    if (viewModel.DecimalPreviousResult != viewModel.DecimalPreviousNumber)
                    {
                        // 현재 수식을 평가하여 결과 저장               
                        var result = EvaluateExpression(viewModel.Expression + viewModel.TextValue);
                        viewModel.DecimalPreviousResult = Decimal.Parse(result);

                        // 연산자 추가 및 TextValue 업데이트
                        viewModel.Expression += viewModel.TextValue + " × ";
                        viewModel.TextValue = FormatNumberWithCommas(Convert.ToString(result));
                    }
                    else
                    {
                        if (viewModel.Expression.Split(' ').Length > 2)
                        {
                            System.Diagnostics.Debug.WriteLine(viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2]);
                            if (viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2] == ")")
                            {
                                viewModel.Expression += " × ";
                            }
                        }

                    }
                }
            }
        }

        private void DivButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
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

                    if (viewModel.PreviousResult != viewModel.PreviousNumber)
                    {
                        // 현재 수식을 평가하여 결과 저장               
                        var result = EvaluateExpression(viewModel.Expression + viewModel.TextValue);
                        if (viewModel._isInt)
                        {
                            viewModel.PreviousResult = BigInteger.Parse(result);
                        }
                        else
                        {
                            viewModel.DecimalPreviousNumber = Decimal.Parse(result);
                        }

                        // 연산자 추가 및 TextValue 업데이트
                        viewModel.Expression += viewModel.TextValue + " ÷ ";
                        viewModel.TextValue = FormatNumberWithCommas(Convert.ToString(result));
                    }
                    else
                    {
                        if (viewModel.Expression.Split(' ').Length > 2)
                        {
                            System.Diagnostics.Debug.WriteLine(viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2]);
                            if (viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2] == ")")
                            {
                                viewModel.Expression += " ÷ ";
                            }
                        }

                    }
                }
                else
                {
                    viewModel.DecimalPreviousNumber = Decimal.Parse(viewModel.TextValue.Replace(",", ""));

                    if (viewModel.DecimalPreviousResult != viewModel.DecimalPreviousNumber)
                    {
                        // 현재 수식을 평가하여 결과 저장               
                        var result = EvaluateExpression(viewModel.Expression + viewModel.TextValue);
                        viewModel.DecimalPreviousResult = Decimal.Parse(result);

                        // 연산자 추가 및 TextValue 업데이트
                        viewModel.Expression += viewModel.TextValue + " ÷ ";
                        viewModel.TextValue = FormatNumberWithCommas(Convert.ToString(result));
                    }
                    else
                    {
                        if (viewModel.Expression.Split(' ').Length > 2)
                        {
                            System.Diagnostics.Debug.WriteLine(viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2]);
                            if (viewModel.Expression.Split(' ')[viewModel.Expression.Split(' ').Length - 2] == ")")
                            {
                                viewModel.Expression += " ÷ ";
                            }
                        }

                    }
                }
            }
        }

        private void LBraButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            if (viewModel != null)
            {
                viewModel.Expression += " ( ";
            }
        }

        private void RBraButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            if (viewModel != null)
            {
                // 'FullExpression'이 비어있지 않으면 초기화
                if (!string.IsNullOrEmpty(viewModel.FullExpression))
                {
                    viewModel.Expression = "";
                    viewModel.FullExpression = "";
                    viewModel._isFinal = false;
                }



                // 전체 수식 평가
                var result = EvaluateExpression(viewModel.Expression + viewModel.TextValue);
                if (viewModel._isInt)
                {
                    viewModel.PreviousResult = BigInteger.Parse(result);
                }
                else
                {
                    viewModel.DecimalPreviousResult = Decimal.Parse(result);
                }

                // 현재 텍스트 값과 닫는 괄호 추가
                viewModel.Expression += viewModel.TextValue + " ) ";
                viewModel.TextValue = FormatNumberWithCommas(Convert.ToString(result));

            }
        }


        // '=' 버튼 클릭 이벤트
        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            if (viewModel != null)
            {
                // 현재 수식과 입력된 값을 결합하여 결과를 계산
                var combinedExpression = viewModel.Expression + viewModel.TextValue;
                var result = EvaluateExpression(combinedExpression);

                // 결과와 함께 전체 수식을 FullExpression에 저장
                viewModel.FullExpression = combinedExpression + " = " + FormatNumberWithCommas(Convert.ToString(result));

                // 결과를 TextValue에 표시
                viewModel.TextValue = FormatNumberWithCommas(Convert.ToString(result));

                // 다음 계산을 위해 PreviousResult 초기화
                viewModel.PreviousResult = 0;

                // 현재 수식을 마지막 연산으로 업데이트
                viewModel.Expression = combinedExpression + " = ";

                viewModel._isFinal = true;
            }
        }


        //중위 -> 후위 컨버터 및 필요 항목 추출 평가
        private string EvaluateExpression(string expression)
        {
            System.Diagnostics.Debug.WriteLine("expression: " + expression);
            var outputQueue = ConvertToPostfix(expression);
            return EvaluatePostfix(outputQueue);
        }

        private Queue<string> ConvertToPostfix(string infix)
        {
            string result = infix.Replace(",", "");
            var outputQueue = new Queue<string>();
            var operatorStack = new Stack<string>();
            var viewModel = DataContext as ViewModel;
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
        }



        private string EvaluatePostfix(Queue<string> postfix)
        {
            var viewModel = DataContext as ViewModel;
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

        }



        private bool IsOperator(string token)
        {
            return new HashSet<string> { "+", "-", "×", "÷", "^" }.Contains(token);
        }

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
        }

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
        }

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
        }



        //c버튼
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            if (viewModel != null)
            {
                viewModel.Expression = "";
                viewModel.FullExpression = "";
                viewModel.TextValue = "0";
                viewModel._isInt = true;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            var viewModel = DataContext as ViewModel;
        }

        //.버튼
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            if (viewModel._isInt == true)
            {
                viewModel.TextValue += ".";
            }
            viewModel._isInt = false;
        }
    }
}
