using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
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
    //숫자담는용
    public class ViewModel : INotifyPropertyChanged
    {
        private string _textValue = "0";

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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
            else if(Trigonometry2nd.IsChecked == false && TrigonometryHyp.IsChecked == false)
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
                var numericText = viewModel.TextValue.Replace(",", "").Replace(" ", ""); // 콤마와 공백 제거

                // BigInteger로 변환
                BigInteger.TryParse(numericText, out BigInteger currentNumber);

                var maxValue = BigInteger.Parse("9999999999999999999999999999999");

                // 최대값 체크
                if (currentNumber <= maxValue)
                {
                    // 숫자 추가
                    var updatedNumber = BigInteger.Parse(numericText + Convert.ToString(num));

                    // 새로운 숫자를 문자열로 변환하고 콤마를 추가
                    viewModel.TextValue = FormatNumberWithCommas(updatedNumber);
                }
            }
        }

        //콤마 추가
        private string FormatNumberWithCommas(BigInteger number)
        {
            var numericText = number.ToString();
            var builder = new StringBuilder();
            int count = 0;
            for (int i = numericText.Length - 1; i >= 0; i--)
            {
                builder.Append(numericText[i]);
                count++;
                if (count % 3 == 0 && i != 0)
                {
                    builder.Append(",");
                }
            }
            return new string(builder.ToString().Reverse().ToArray());
        }



    }
}
