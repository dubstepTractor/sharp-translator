using System.Diagnostics;
using Translator.Core;

const string DosBoxApplicationPath = @"DOSBox\DOSBox.exe";
const string DosBoxProgramData = @"DOSBox\data\";
const string SourceFileName = "code";
const string ProgramFileName = "compile";

// Запуск теста в DOSBox
void RunDosBoxTest(string fileName, string code)
{
    if (!File.Exists(DosBoxApplicationPath))
    {
        throw new FileNotFoundException("Не найден файл: " + DosBoxApplicationPath);
    }

    File.WriteAllText(DosBoxProgramData + fileName + ".asm", code);
    var mountData = @"mount D " + DosBoxProgramData.Remove(DosBoxProgramData.Length - 1);
    var masmData = "MASM.EXE " + fileName + ".asm" + " " + fileName + ".obj" + " " + fileName + ".lst" + " " + fileName + ".crf";
    var linkData = "LINK.EXE " + fileName + ".obj" + "," + fileName + ".exe" + "," + fileName + ".map" + "," + "/NODEFAULTLIB";
    var programLauch = fileName + ".exe";
    var psi = new ProcessStartInfo
    {
        FileName = DosBoxApplicationPath,
        Arguments = $"-c \"{mountData}\" -c D: -c \"{masmData}\" -c \"{linkData}\" -c " + programLauch,
        RedirectStandardOutput = true,
        UseShellExecute = false
    };
    Process.Start(psi);
}

string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
string sourceFilePath = Path.Combine(baseDirectory, SourceFileName + ".txt");
string compiledFilePath = Path.Combine(baseDirectory, ProgramFileName + ".asm");

var syntaxAnalyzer = new SyntaxAnalyzer();
syntaxAnalyzer.Compile(sourceFilePath);

var code = string.Join("\n", CodeGenerator.GetGeneratedCode());
File.WriteAllText(compiledFilePath, code);
Console.WriteLine(code);
Reader.Close();

RunDosBoxTest(ProgramFileName, code);