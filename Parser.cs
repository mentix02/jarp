using System;
using System.Collections;
using System.Collections.Generic;

namespace Jarp
{
    public class Parser
    {
        private Token currToken;
        private Token peekToken;
        private readonly Lexer lexer;

        public object Parse()
        {
            return currToken.tokenType switch
            {
                TokenType.LBRACK => ParseArray(),
                TokenType.LBRACE => ParseObject(),
                TokenType.STRING => currToken.literal,
                TokenType.INT => int.Parse(currToken.literal),
                TokenType.BOOL => currToken.literal == "true" ? true : false,
                _ => throw new Exception($"Unexpected token {currToken.literal} found")
            };
        }

        public Parser(Lexer l)
        {
            lexer = l;

            // Read two tokens to populate currToken & peekToken
            NextToken();
            NextToken();
        }

        private void NextToken()
        {
            currToken = peekToken;
            peekToken = lexer.NextToken();
        }


        // Helper token funcs

        private bool PeekTokenIs(TokenType tokenType) => peekToken.tokenType == tokenType;

        private bool CurrentTokenIs(TokenType tokenType) => currToken.tokenType == tokenType;

        private bool ExpectPeek(TokenType tokenType)
        {
            if (PeekTokenIs(tokenType))
            {
                NextToken();
                return true;
            }
            else return false;
        }

        // Parsing Funcs

        private ArrayList ParseArray()
        {
            var list = new ArrayList();

            // Array ends right after starting.
            if (PeekTokenIs(TokenType.RBRACK))
            {
                NextToken();
                return list;
            }

            // Increment over current LBRACK token and
            // recursively parse objects inside array
            NextToken();
            list.Add(Parse());

            while (PeekTokenIs(TokenType.COMMA))
            {
                // Skip over current COMMA token OR
                // increment over left over token that
                // was parsed in the first object parsing
                NextToken();

                // Place currToken as the first token of
                // the object to be recursively parsed
                // and Add-ed to current array (list)
                NextToken();

                list.Add(Parse());
            }

            if (!ExpectPeek(TokenType.RBRACK))
                throw new Exception($"Expecting token ']' | found '{peekToken.literal}'");

            return list;
        }

        private Dictionary<string, object> ParseObject()
        {
            string key;
            var dict = new Dictionary<string, object>();

            if (PeekTokenIs(TokenType.RBRACE))
            {
                NextToken();
                return dict;
            }

            NextToken();
            if (!CurrentTokenIs(TokenType.STRING))
                throw new Exception($"Expecting string | found {peekToken.literal}");

            key = currToken.literal;

            if (!PeekTokenIs(TokenType.COLON))
                throw new Exception($"Expecting colon | found '{peekToken.literal}'");

            // Skip over COLON token
            NextToken();
            // Place object to be placed as value as currToken
            NextToken();

            // Recurisvely parse current object to be placed as value
            dict[key] = Parse();

            while (PeekTokenIs(TokenType.COMMA))
            {
                NextToken();

                if (!CurrentTokenIs(TokenType.STRING))
                    throw new Exception($"Expecting string | found {peekToken.literal}");

                key = currToken.literal;

                if (!PeekTokenIs(TokenType.COLON))
                    throw new Exception($"Expecting colon | found '{peekToken.literal}'");

                NextToken();
                NextToken();
                dict[key] = ParseArray();
            }

            if (!ExpectPeek(TokenType.RBRACE))
                throw new Exception($"Expecting token '}}' | found '{peekToken.literal}'");

            return dict;
        }

    }
}
