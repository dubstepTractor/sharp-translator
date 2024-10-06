using Translator.Core;

string sourceFilePath = "D:\\Prog\\sharp-translator\\Translator\\Translator.Integration\\code.txt";
string compiledFilePath = "D:\\Prog\\sharp-translator\\Translator\\Translator.Integration\\compile.asm";
var syntaxAnalyzer = new SyntaxAnalyzer();
syntaxAnalyzer.Compile(sourceFilePath);
File.WriteAllText(compiledFilePath, string.Join("\n", CodeGenerator.GetGeneratedCode()));
Console.WriteLine(string.Join("\n", CodeGenerator.GetGeneratedCode()));

Reader.Close();