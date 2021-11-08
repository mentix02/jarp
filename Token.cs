namespace Jarp
{
    public class Token
    {
        public readonly string literal;
        public readonly TokenType tokenType;

        public Token(TokenType t, string l) { literal = l; tokenType = t; }
        public Token(TokenType t, char c) { literal = c.ToString(); tokenType = t; }

        public override string ToString() => $"type: {tokenType}\t|\tliteral: {literal}";

        public static bool operator ==(Token a, Token b) => a.Equals(b);

        public static bool operator !=(Token a, Token b) => !a.Equals(b);

        public override bool Equals(object obj)
        {
            if (obj is not Token token)
                return false;
            else
                return (
                    tokenType == token.tokenType &&
                    literal == token.literal
                );
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }
    }
}
