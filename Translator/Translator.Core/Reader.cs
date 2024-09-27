using System;
using System.IO;

public static class Reader
{
    private static int lineNumber;
    private static int characterPositionInLine;
    private static int currentSymbol;

    private static StreamReader streamReader;

    public static int LineNumber => lineNumber;
    public static int CharacterPositionInLine => characterPositionInLine;
    public static char CurrentSymbol => (char)currentSymbol;

    public const int EndOfFile = 65535;

    public static void ReadNextSymbol()
    {
        currentSymbol = streamReader.Read();

        if (currentSymbol == EndOfFile)
        {
            return; // Достигнут конец файла
        }
        else if (currentSymbol == '\n')
        {
            lineNumber++;
            characterPositionInLine = 0;
        }
        else if (currentSymbol == '\r' || currentSymbol == '\t')
        {
            ReadNextSymbol(); // Пропускаем
            return;
        }
        else
        {
            characterPositionInLine++;
        }
    }

    public static void Initialize(string filePath)
    {
        if (streamReader != null)
        {
            Close();
        }

        if (File.Exists(filePath))
        {
            streamReader = new StreamReader(filePath);
            lineNumber = 1;
            characterPositionInLine = 0;
            ReadNextSymbol();
        }
        else
        {
            throw new FileNotFoundException($"Файл не найден: {filePath}");
        }
    }

    public static void Close()
    {
        streamReader?.Close();
        streamReader = null;
    }
}
