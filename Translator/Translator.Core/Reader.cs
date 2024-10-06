/// <summary>
/// Статический класс, отвечающий за чтение символов из файла.
/// </summary>
public static class Reader
{
    private static int lineNumber;
    private static int characterPositionInLine;
    private static int currentSymbol;

    private static StreamReader streamReader;

    /// <summary>
    /// Номер текущей строки.
    /// </summary>
    public static int LineNumber => lineNumber;

    /// <summary>
    /// Позиция текущего символа в строке.
    /// </summary>
    public static int CharacterPositionInLine => characterPositionInLine;

    /// <summary>
    /// Текущий читаемый символ.
    /// </summary>
    public static char CurrentSymbol => (char)currentSymbol;

    /// <summary>
    /// Константа, представляющая конец файла.
    /// </summary>
    public const int EndOfFile = 65535;

    /// <summary>
    /// Читает следующий символ из файла и обновляет состояние строки и позиции.
    /// </summary>
    public static void ReadNextSymbol()
    {
        currentSymbol = streamReader.Read();

        if (currentSymbol == EndOfFile)
        {
            return;
        }
        else if (currentSymbol == '\n')
        {
            lineNumber++;
            characterPositionInLine = 0;
        }
        else if (currentSymbol == '\r' || currentSymbol == '\t')
        {
            ReadNextSymbol();
            return;
        }
        else
        {
            characterPositionInLine++;
        }
    }

    /// <summary>
    /// Инициализирует чтение из указанного файла.
    /// </summary>
    /// <param name="filePath">Путь к файлу для чтения.</param>
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

    /// <summary>
    /// Закрывает поток чтения.
    /// </summary>
    public static void Close()
    {
        streamReader?.Close();
        streamReader = null;
    }
}
