namespace jarp
{
    readonly struct Token
    {
        public readonly string literal;
        public readonly TokenType tokenType;

        public Token(TokenType t, string l) { literal = l; tokenType = t; }
        public Token(TokenType t, char c) { literal = c.ToString(); tokenType = t; }

        public override string ToString() => $"type: {tokenType}\t|\tliteral: {literal}";
    }
}
