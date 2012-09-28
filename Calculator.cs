using System;
using System.Collections.Generic;
using System.Globalization;

namespace SimpleCalculator
{
    public class Calculator
    {
        //TODO add support to typing negative numbers
        private const string STR_0 = "0";

        private const string STR_Dot = ".";

        private bool _decimalFound;
        private CalculatorStatus _status;
        private Stack<Token> _tokens;
        private Action<decimal> _resultCallback;

        public Calculator(Action<decimal> resultCallback)
        {
            _tokens = new Stack<Token>();
            _status = CalculatorStatus.Empty;
            _resultCallback = resultCallback;
        }

        public bool Calculate(out decimal result)
        {
            result = 0.0m;
            switch (_status)
            {
                case CalculatorStatus.Empty:

                    //Nothing to do
                    return false;

                case CalculatorStatus.FirstNumber:

                    //The first number becomes a result
                    result = decimal.Parse(_tokens.Peek().Value, CultureInfo.InvariantCulture.NumberFormat);
                    _tokens.Push(new Token("=", TokenType.Operator));
                    _status = CalculatorStatus.Operator;
                    return false;

                case CalculatorStatus.Operator:

                    //TODO ?
                    return false;

                case CalculatorStatus.SecondNumber:
                    var secondNumber = decimal.Parse(_tokens.Pop().Value, CultureInfo.InvariantCulture.NumberFormat);
                    var @operator = _tokens.Pop().Value;
                    var firstNumber = decimal.Parse(_tokens.Pop().Value, CultureInfo.InvariantCulture.NumberFormat);
                    switch (@operator)
                    {
                        case "=":

                            //The operator "=" is used as a placeholder to prevent editing the result of an operation
                            //Nothing to do
                            break;

                        case "+":
                            result = firstNumber + secondNumber;
                            _tokens.Push(new Token(result.ToString(CultureInfo.InvariantCulture.NumberFormat), TokenType.Number));
                            _tokens.Push(new Token("=", TokenType.Operator));
                            _status = CalculatorStatus.Operator;
                            break;

                        case "-":
                            result = firstNumber - secondNumber;
                            _tokens.Push(new Token(result.ToString(CultureInfo.InvariantCulture.NumberFormat), TokenType.Number));
                            _tokens.Push(new Token("=", TokenType.Operator));
                            _status = CalculatorStatus.Operator;
                            break;

                        case "*":
                            result = firstNumber * secondNumber;
                            _tokens.Push(new Token(result.ToString(CultureInfo.InvariantCulture.NumberFormat), TokenType.Number));
                            _tokens.Push(new Token("=", TokenType.Operator));
                            _status = CalculatorStatus.Operator;
                            break;

                        case "/":
                            result = firstNumber / secondNumber;
                            _tokens.Push(new Token(result.ToString(CultureInfo.InvariantCulture.NumberFormat), TokenType.Number));
                            _tokens.Push(new Token("=", TokenType.Operator));
                            _status = CalculatorStatus.Operator;
                            break;
                        default:

                            //TODO add support for additional operators
                            break;
                    }
                    if (!ReferenceEquals(_resultCallback, null))
                    {
                        _resultCallback.Invoke(result);
                    }
                    return true;

                default:

                    //TODO ?
                    return false;
            }
        }

        public string Clear()
        {
            _tokens.Clear();
            _status = CalculatorStatus.Empty;
            _decimalFound = false;
            return STR_0;
        }

        public string Digit(byte value)
        {
            AddToken(new Token(value.ToString(CultureInfo.InvariantCulture.NumberFormat), TokenType.Number));
            return _tokens.Peek().Value;
        }

        public void Operator(string value)
        {
            AddToken(new Token(value, TokenType.Operator));
        }

        private void AddToken(Token token)
        {
            switch (_status)
            {
                case CalculatorStatus.Empty:
                    switch (token.Type)
                    {
                        case TokenType.Number:
                            _tokens.Push(token);
                            _status = CalculatorStatus.FirstNumber;
                            _decimalFound = false;
                            break;

                        case TokenType.Operator:
                            _tokens.Push(new Token(STR_0, TokenType.Number));
                            _tokens.Push(token);
                            _status = CalculatorStatus.Operator;
                            break;

                        case TokenType.Special:
                            if (token.Value == STR_Dot)
                            {
                                _tokens.Push(new Token(STR_0 + STR_Dot, TokenType.Number));
                                _status = CalculatorStatus.FirstNumber;
                                _decimalFound = true;
                            }

                            //TODO (HOMEWORK) add support for parenthesis
                            break;
                        default:

                            //TODO
                            break;
                    }
                    break;

                case CalculatorStatus.FirstNumber:
                    switch (token.Type)
                    {
                        case TokenType.Number:
                            _tokens.Push(new Token(_tokens.Pop().Value + token.Value, TokenType.Number));
                            break;

                        case TokenType.Operator:
                            _tokens.Push(token);
                            _status = CalculatorStatus.Operator;
                            break;

                        case TokenType.Special:
                            if (token.Value == STR_Dot)
                            {
                                if (!_decimalFound)
                                {
                                    _tokens.Push(new Token(_tokens.Pop().Value + STR_Dot, TokenType.Number));
                                    _status = CalculatorStatus.FirstNumber;
                                    _decimalFound = true;
                                }
                            }

                            //TODO (HOMEWORK) add support for parenthesis
                            break;
                        default:

                            //TODO
                            break;
                    }
                    break;

                case CalculatorStatus.Operator:
                    switch (token.Type)
                    {
                        case TokenType.Number:
                            //The operator "=" means that the input is being recieved after requesting a result
                            //If we find the operator "=" we discard all
                            if (_tokens.Peek().Value == "=")
                            {
                                //discard
                                Clear();
                                AddToken(token);
                            }
                            else
                            {
                                _tokens.Push(token);
                                _status = CalculatorStatus.SecondNumber;
                                _decimalFound = false;
                            }
                            break;

                        case TokenType.Operator:

                            //replace the operator
                            _tokens.Pop();
                            _tokens.Push(token);
                            break;

                        case TokenType.Special:
                            //The operator "=" means that the input is being recieved after requesting a result
                            //If we find the operator "=" we discard all
                            if (_tokens.Peek().Value == "=")
                            {
                                //discard
                                Clear();
                                AddToken(token);
                            }
                            else
                            {
                                if (token.Value == STR_Dot)
                                {
                                    _tokens.Push(new Token(STR_0 + STR_Dot, TokenType.Number));
                                    _decimalFound = true;
                                    _status = CalculatorStatus.SecondNumber;
                                }
                                //TODO (HOMEWORK) add support for parenthesis
                            }
                            break;
                        default:

                            //TODO
                            break;
                        
                    }
                    break;
                case CalculatorStatus.SecondNumber:
                    switch (token.Type)
                    {
                        case TokenType.Number:
                            _tokens.Push(new Token(_tokens.Pop().Value + token.Value, TokenType.Number));
                            break;

                        case TokenType.Operator:

                            //It is like pressing equals and then taking the result as first number, afterwards add the operator
                            Calculate();
                            AddToken(token);
                            break;

                        case TokenType.Special:
                            if (token.Value == STR_Dot)
                            {
                                if (!_decimalFound)
                                {
                                    _tokens.Push(new Token(_tokens.Pop().Value + STR_Dot, TokenType.Number));
                                    _status = CalculatorStatus.FirstNumber;
                                    _decimalFound = true;
                                }
                            }

                            //TODO (HOMEWORK) add support for parenthesis
                            break;
                        default:

                            //TODO
                            break;
                    }
                    break;
                default:

                    //TODO ?
                    break;
            }
        }

        public bool Calculate()
        {
            decimal dummy;
            return Calculate(out dummy);
        }

        public string Special(string value)
        {
            AddToken(new Token(value, TokenType.Special));
            return _tokens.Peek().Value;
        }
    }
}