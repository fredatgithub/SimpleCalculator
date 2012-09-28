using System.Collections.Generic;
using System.Globalization;

namespace SimpleCalculator
{
    public struct Token
    {
        private TokenType _type;
        private string _value;

        public Token(string value, TokenType type)
        {
            _value = value;
            _type = type;
        }

        public TokenType Type
        {
            get
            {
                return _type;
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }
        }
    }
}
