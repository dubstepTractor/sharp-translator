using Translator.Core;

string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
string sourceFilePath = Path.Combine(baseDirectory, "code.txt");
string compiledFilePath = Path.Combine(baseDirectory, "compile.asm");

var syntaxAnalyzer = new SyntaxAnalyzer();
syntaxAnalyzer.Compile(sourceFilePath);
File.WriteAllText(compiledFilePath, string.Join("\n", CodeGenerator.GetGeneratedCode()));
Console.WriteLine(string.Join("\n", CodeGenerator.GetGeneratedCode()));

Reader.Close();