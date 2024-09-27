using System;
using Translator.Core;

public enum Lexems
{
    None, Name, Number, Begin, End, Var, Print, Assign,
    Plus, Minus, Multiplication, Division,
    LeftBracket, RightBracket, Semi, Comma, EOF,
    UnaryOp, BinaryOp, Colon
}

public struct Keyword
{
    public string word;
    public Lexems lex;
}

public static class LexicalAnalyzer
{
    private static Keyword[] keywords;
    private static int keywordsPointer;
    private static Lexems currentLexem;
    private static string currentName;

    private const int MaxIdentifierLength = 50;

    public static void Initialize(string filePath)
    {
        keywords = new Keyword[20];
        keywordsPointer = 0;

        AddKeyword("begin", Lexems.Begin);
        AddKeyword("end", Lexems.End);
        AddKeyword("var", Lexems.Var);
        AddKeyword("print", Lexems.Print);

        Reader.Initialize(filePath);
        currentLexem = Lexems.None;
    }

    private static void AddKeyword(string keyword, Lexems lex)
    {
        Keyword kw = new Keyword { word = keyword, lex = lex };
        keywords[keywordsPointer++] = kw;
    }

    private static Lexems GetKeywordLexem(string keyword)
    {
        for (int i = 0; i < keywordsPointer; i++)
        {
            if (keywords[i].word == keyword)
                return keywords[i].lex;
        }
        return Lexems.Name;
    }

    public static void ParseNextLexem()
    {
        while (char.IsWhiteSpace(Reader.CurrentSymbol))
            Reader.ReadNextSymbol();

        if (Reader.CurrentSymbol == Reader.EndOfFile)
        {
            currentLexem = Lexems.EOF;
            return;
        }

        if (char.IsLetter(Reader.CurrentSymbol))
        {
            ParseIdentifier();
        }
        else if (char.IsDigit(Reader.CurrentSymbol))
        {
            ParseNumber();
        }
        else if (Reader.CurrentSymbol == '=')
        {
            currentName = null;
            Reader.ReadNextSymbol();
            currentLexem = Lexems.Assign;
        }
        else if (Reader.CurrentSymbol == '+')
        {
            currentName = null;
            Reader.ReadNextSymbol();
            currentLexem = Lexems.Plus;
        }
        else if (Reader.CurrentSymbol == '-')
        {
            currentName = null;
            Reader.ReadNextSymbol();
            currentLexem = Lexems.Minus;
        }
        else if (Reader.CurrentSymbol == '*')
        {
            currentName = null;
            Reader.ReadNextSymbol();
            currentLexem = Lexems.Multiplication;
        }
        else if (Reader.CurrentSymbol == '/')
        {
            currentName = null;
            Reader.ReadNextSymbol();
            currentLexem = Lexems.Division;
        }
        else if (Reader.CurrentSymbol == '(')
        {
            currentName = null;
            Reader.ReadNextSymbol();
            currentLexem = Lexems.LeftBracket;
        }
        else if (Reader.CurrentSymbol == ')')
        {
            currentName = null;
            Reader.ReadNextSymbol();
            currentLexem = Lexems.RightBracket;
        }
        else if (Reader.CurrentSymbol == ';')
        {
            currentName = null;
            Reader.ReadNextSymbol();
            currentLexem = Lexems.Semi;
        }
        else if (Reader.CurrentSymbol == ':')
        {
            currentName = null;
            Reader.ReadNextSymbol();
            currentLexem = Lexems.Colon;
        }
        else if (Reader.CurrentSymbol == ',')
        {
            currentName = null;
            Reader.ReadNextSymbol();
            currentLexem = Lexems.Comma;
        }
        else if (Reader.CurrentSymbol == '!')
        {
            currentName = null;
            Reader.ReadNextSymbol();
            currentLexem = Lexems.UnaryOp; // обработка унарного оператора
        }
        else if (Reader.CurrentSymbol == '&' || Reader.CurrentSymbol == '|' || Reader.CurrentSymbol == '^')
        {
            currentName = null;
            currentLexem = Lexems.BinaryOp;
            Reader.ReadNextSymbol();
        }
        else
        {
            throw new Exception($"Ошибка: Недопустимый символ: {Reader.CurrentSymbol}");
        }
    }

    private static void ParseIdentifier()
    {
        string identifier = string.Empty;

        do
        {
            identifier += Reader.CurrentSymbol;
            Reader.ReadNextSymbol();
        }
        while (char.IsLetter(Reader.CurrentSymbol) && identifier.Length < MaxIdentifierLength);

        if (identifier.Length >= MaxIdentifierLength)
        {
            throw new Exception("Ошибка: Длина идентификатора превышает максимальную допустимую.");
        }

        currentName = identifier;
        currentLexem = GetKeywordLexem(identifier);
    }

    private static void ParseNumber()
    {
        string numberString = string.Empty;

        do
        {
            numberString += Reader.CurrentSymbol;
            Reader.ReadNextSymbol();
        }
        while (char.IsDigit(Reader.CurrentSymbol));

        if (!int.TryParse(numberString, out _))
        {
            throw new Exception("Ошибка: Переполнение при разборе числа.");
        }

        currentName = numberString;
        currentLexem = Lexems.Number;
    }

    public static Lexems CurrentLexem => currentLexem;
    public static string? CurrentName => currentName;
}
