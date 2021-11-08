namespace Jarp
{
    public class Jarp
    {

        readonly public Lexer _lexer;
        readonly public Parser _parser;

        public Jarp(string json)
        {
            _lexer = new Lexer(json);
            _parser = new Parser(_lexer);
        }

        public object Parse()
        {
            return _parser.Parse();
        }
    }
}
