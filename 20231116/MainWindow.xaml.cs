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
using _20231116;
using System.Windows.Shapes;

namespace _20231116
{
   
    public partial class MainWindow : Window
    {
        private string[] angles = { "GRAD", "DEG", "RAD" };
        private int anglesInt = 1;

        HashSet<string> allowedButtons = new HashSet<string>
        {
            "C", "DEG", "Ⅽ", "⌫", "=", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
        }; //문제 생길 시 켜져있을 버튼

        HashSet<string> disallowedButtons = new HashSet<string>
        {
            "MC", "MR", "MS", "M∨"
        }; //기존에 이미 꺼져있는 버튼


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
            FormatHelper formatHelper = new FormatHelper();

            if (viewModel == null) return;

            // 숫자 키 입력 처리
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                // 키보드의 상단 숫자 키 입력 처리
                formatHelper.TextValueUpdate((e.Key - Key.D0), viewModel);
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                // 숫자 패드 키 입력 처리
                formatHelper.TextValueUpdate((e.Key - Key.NumPad0), viewModel);
            }
            // ViewModel의 TextValue가 자동으로 업데이트되도록 구현
        } //키 입력

        private void AnglesButton(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            anglesInt = (anglesInt + 1) % angles.Length;

            button.Content = angles[anglesInt];

        } //각도 버튼

        private void ExpressionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                // 수식 길이에 따라 폰트 크기 조정
                textBox.FontSize = CalculateFontSizeForExpression(textBox.Text);
            }
        } //수식 텍스트 사이즈 길이 비례 조정
        private double CalculateFontSizeForExpression(string text)
        {
            if (text.Length < 20) return 15;
            if (text.Length < 40) return 13;
            if (text.Length < 60) return 10;

            return 14;
        } //숫자 텍스트 사이즈 길이 비례 조절
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {

                double newFontSize = CalculateFontSize(textBox.Text);
                textBox.FontSize = newFontSize;
            }
        } //텍스트 사이즈 자동 조절
        private double CalculateFontSize(string text)
        {

            if (text.Length < 10) return 20;
            if (text.Length < 20) return 18;
            if (text.Length < 30) return 16;

            return 12;
        } // 텍스트 사이즈 자동조절

        private void Popup_Closed(object sender, EventArgs e)
        {
            Trigonometry.IsOpen = false;
            function.IsOpen = false;
        } //팝업처리

        private void OpenTriPopupButton_Click(object sender, RoutedEventArgs e)
        {
            Trigonometry.IsOpen = !Trigonometry.IsOpen;
            e.Handled = true;
        } //삼각법
        private void ChangeTrigonometry(object sender, EventArgs e)
        {
            UpdateTrigonometryTexts();
        } //삼각법-2nd, hyp
        private void UpdateTrigonometryTexts()
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
        } //삼각법 - 버튼들 조건 체크 및 텍스트 변경 << 여기에 나중에 수식 관련 추가하면됨 
       
        private void openFunPopupButton_Click(object sender, RoutedEventArgs e)
        {
            function.IsOpen = !function.IsOpen;
            e.Handled = true;
        } //함수

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



        } //넘패드 2nd 체크

        private void NumPad2nd_Unchecked(object sender, RoutedEventArgs e)
        {

            sqr.Content = "x²";
            root.Content = "2√x";
            square.Content = "x^y";
            square10.Content = "10^x";
            log.Content = "log";
            nSquare.Content = "ln";
        } //넘패드 2nd 체크헤제
       
        private void Button_Click_num(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var viewModel = DataContext as ViewModel;
            FormatHelper textUpdates = new FormatHelper();

            textUpdates.TextValueUpdate(int.Parse(button.Content.ToString()), viewModel);
        } //넘패드0 ~ 9
       
        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            FormatHelper formatHelper = new FormatHelper();
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
                        viewModel.TextValue = formatHelper.FormatNumberWithCommas(Convert.ToString(updatedNumber), viewModel);
                    }
                    else
                    {
                        Decimal.TryParse(numericText, out Decimal updatedNumber);
                        viewModel.TextValue = formatHelper.FormatNumberWithCommas(Convert.ToString(updatedNumber), viewModel);
                    }
                }
                else
                {
                    viewModel.TextValue = "0";
                }
            }
        } //del버튼

        private void FourBasicOperationsButton(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var viewModel = DataContext as ViewModel;
            Calculator calculator = new Calculator();

            calculator.FourBasicOperations(button.Content.ToString() ,viewModel);
        } //연산 버튼(사칙연산)

        private void LBraButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            if (viewModel != null)
            {
                viewModel.Expression += " ( ";
            }
        } //오른쪽 괄호

        private void RBraButton_Click(object sender, RoutedEventArgs e)
        {
            FormatHelper formatHelper = new FormatHelper();
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
                var result = formatHelper.EvaluateExpression(viewModel.Expression + viewModel.TextValue, viewModel);
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
                viewModel.TextValue = formatHelper.FormatNumberWithCommas(Convert.ToString(result), viewModel);

            }
        } //왼쪽 괄호

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            FormatHelper formatHelper = new FormatHelper();
            var viewModel = DataContext as ViewModel;
            if (viewModel != null)
            {
                // 현재 수식과 입력된 값을 결합하여 결과를 계산
                var combinedExpression = viewModel.Expression + viewModel.TextValue;
                var result = formatHelper.EvaluateExpression(combinedExpression, viewModel);

                // 결과와 함께 전체 수식을 FullExpression에 저장
                viewModel.FullExpression = combinedExpression + " = " + formatHelper.FormatNumberWithCommas(Convert.ToString(result), viewModel);

                // 결과를 TextValue에 표시
                viewModel.TextValue = formatHelper.FormatNumberWithCommas(Convert.ToString(result), viewModel);

                // 다음 계산을 위해 PreviousResult 초기화
                viewModel.PreviousResult = 0;

                // 현재 수식을 마지막 연산으로 업데이트
                viewModel.Expression = combinedExpression + " = ";

                viewModel._isFinal = true;
            }
        } //= 버튼

        private void ClearButton(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            if (viewModel != null)
            {
                viewModel.Expression = "";
                viewModel.FullExpression = "";
                viewModel.TextValue = "0";
                viewModel._isInt = true;
            }
        } //c버튼

        private void DotButton(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            if (viewModel._isInt == true)
            {
                viewModel.TextValue += ".";
            }
            viewModel._isInt = false;
        } //.버튼

        private void PiButton(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            FormatHelper formatHelper = new FormatHelper();
            viewModel._isInt = false;
            viewModel.TextValue = formatHelper.FormatNumberWithCommas(Convert.ToString(Math.PI), viewModel);
        } //파이 버튼

        private void Naturalis(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            Button button = sender as Button;
            Calculator calculator = new Calculator();
            calculator.Naturalis(Convert.ToString(button.Content), viewModel);
        } //자연로그 버튼

        private void Factorial(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            Calculator calculator = new Calculator();
            ButtonOnOff(calculator.Factorial(viewModel));
        }
        public void ButtonOnOff(bool isOff)
        {
            if(isOff == false)
            {
                foreach (var control in FindVisualChildren<Grid>(this))
                {
                    foreach (var child in control.Children)
                    {
                        if (child is Button button && !allowedButtons.Contains(button.Content.ToString()))
                        {
                            button.IsEnabled = false;
                        }
                    }
                }
            }
            else
            {
                foreach (var control in FindVisualChildren<Grid>(this))
                {
                    foreach (var child in control.Children)
                    {
                        if (child is Button button && !disallowedButtons.Contains(button.Content.ToString()))
                        {
                            button.IsEnabled = true;
                        }
                    }
                }
            }
        } //버튼 활/비활 제어

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        } //Grid찾기
    }

}
