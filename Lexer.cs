using System;
using System.Collections;

namespace Jarp
{
    
    public enum TokenType
    {
        INT,
        EOF,
        BOOL,
        COLON,
        COMMA,
        LBRACK,
        RBRACK,
        LBRACE,
        RBRACE,
        STRING,
        ILLEGAL,
    }

    public class Lexer : IEnumerator, IEnumerable
    {

        private int position;
        private byte currByte;
        private int readPosition;
        private readonly string jstring;

        public Lexer(string input)
        {
            jstring = input;
            ReadChar(); // initializes position, currChar, and readPosition
        }

        private void ReadChar()
        {
            if (readPosition >= jstring.Length)
                currByte = 0;
            else
                currByte = (byte)jstring[readPosition];
            position = readPosition++;
        }

        private string ExtractNumber()
        {
            int startPosition = position;

            if (jstring[position] == '-') ReadChar();
            while (char.IsDigit((char)currByte)) ReadChar();
            return jstring[startPosition..position];
        }

        private string ExtractString()
        {
            int startPosition = position + 1; // +1 to exclude double quote
            while (true)
            {
                ReadChar();
                if (((char)currByte) == '"' || currByte == 0)
                    break;
            }
            return jstring[startPosition..position];
        }

        private string ExtractBoolean()
        {
            int startPosition = position;
            while (char.IsLetter((char)currByte)) ReadChar();
            string possibleBool = jstring[startPosition..position];
            if (possibleBool == "true" || possibleBool == "false")
                return possibleBool;
            else
                throw new Exception($"Expected \"true\" OR \"false\" | found \"{possibleBool}\"");
        }

        private void SkipWhitespace()
        {
            while (char.IsWhiteSpace((char)currByte)) ReadChar();
        }

        public Token NextToken()
        {
            Token tok;
            SkipWhitespace();
            char currChar = (char)currByte;

            switch (currChar)
            {
                case ',':
                    tok = new Token(TokenType.COMMA, currChar);
                    break;
                case ':':
                    tok = new Token(TokenType.COLON, currChar);
                    break;
                case '[':
                    tok = new Token(TokenType.LBRACK, currChar);
                    break;
                case ']':
                    tok = new Token(TokenType.RBRACK, currChar);
                    break;
                case '{':
                    tok = new Token(TokenType.LBRACE, currChar);
                    break;
                case '}':
                    tok = new Token(TokenType.RBRACE, currChar);
                    break;
                case 'f':
                    tok = new Token(TokenType.BOOL, ExtractBoolean());
                    break;
                case 't':
                    tok = new Token(TokenType.BOOL, ExtractBoolean());
                    break;
                case '"':
                    tok = new Token(TokenType.STRING, ExtractString());
                    break;
                default:
                    if (currByte == 0)
                    {
                        tok = new Token(TokenType.EOF, "");
                        return tok;
                    } else if (char.IsDigit(currChar) || currChar == '-')
                    {
                        tok = new Token(TokenType.INT, ExtractNumber());
                        return tok;
                    } else
                    {
                        tok = new Token(TokenType.ILLEGAL, currChar);
                        break;
                    }
            }

            ReadChar();
            return tok;
        }

        // Methods for IEnumerator & IEnumerable

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }

        public bool MoveNext()
        {
            return position < jstring.Length;
        }

        public void Reset()
        {
            position = 0;
            readPosition = 0;
        }
        //IEnumerable
        public object Current
        {
            get { return NextToken(); }
        }


    }
}
