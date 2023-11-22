using System;
using System.ComponentModel;
using System.Numerics;

namespace _20231116
{
    //숫자 바인딩
    public class ViewModel : INotifyPropertyChanged
    {
        private string _textValue = "0";
        private string _expression = "";
        public bool _isFinal = false;
        public bool _isInt = true;
        public bool _isText = false;

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
        } //입력값 저장

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
        } //수식 저장

        public event PropertyChangedEventHandler? PropertyChanged; // 프로퍼티 동기화

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } //만약 프로퍼티 값이 변경되면 얘가 호출된다네? 이걸로 동기화를 할 수 있음

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
        } //BigInteger 관련 이전 입력 처리

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
        } //Decimal 관련 이전 입력 처리

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
        } //현재 수식 문자열 저장

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
        } //BigInteger 관련 이전 연산 결과 처리

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
        } //Decimal 관련 이전 연산 결과 처리

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
        } //전체 수식 저장

    }
}
