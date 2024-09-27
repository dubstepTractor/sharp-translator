string filePath = "C:\\Users\\timofey.latypov\\source\\repos\\Translator\\Translator.Integration\\code.txt";
LexicalAnalyzer.Initialize(filePath);

while (LexicalAnalyzer.CurrentLexem != Lexems.EOF)
{
    Console.WriteLine("\"" + LexicalAnalyzer.CurrentName + "\" |" + LexicalAnalyzer.CurrentLexem);
    LexicalAnalyzer.ParseNextLexem();
}

Reader.Close();


var nameTable = new NameTable();
LexicalAnalyzer.Initialize(filePath);

while (LexicalAnalyzer.CurrentLexem != Lexems.EOF)
{
    if (LexicalAnalyzer.CurrentLexem == Lexems.Name &&
        nameTable.FindByName(LexicalAnalyzer.CurrentName).Equals(default(Identifier)))
    {
        nameTable.AddIdentifier(LexicalAnalyzer.CurrentName, tCat.Var);
    }
    LexicalAnalyzer.ParseNextLexem();
}

Console.WriteLine(string.Join(",", nameTable.GetIdentifiers().Select(x => x.Name)));