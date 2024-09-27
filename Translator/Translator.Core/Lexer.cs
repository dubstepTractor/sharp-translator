using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Core
{
    public class Lexer
    {
        private string input;
        private int position;
        private List<string> tokens;

        private static readonly Dictionary<string, string> keywords = new Dictionary<string, string>
    {
        { "Var", "VAR" },
        { "Begin", "BEGIN" },
        { "End", "END" },
        { "Print", "PRINT" },
    };

        public Lexer(string input)
        {
            this.input = input;
            this.position = 0;
            this.tokens = new List<string>();
        }
        public List<string> Tokenize()
        {
            while (position < input.Length)
            {
                char currentChar = input[position];

                // Игнорируем пробелы
                if (char.IsWhiteSpace(currentChar))
                {
                    position++;
                    continue;
                }

                // Обработка идентификаторов и ключевых слов
                if (char.IsLetter(currentChar))
                {
                    string identifier = ReadIdentifier();
                    tokens.Add(keywords.ContainsKey(identifier) ? keywords[identifier] : identifier);
                    continue;
                }

                // Обработка чисел
                if (char.IsDigit(currentChar))
                {
                    string number = ReadNumber();
                    tokens.Add(number);
                    continue;
                }

                // Обработка операторов и других символов
                switch (currentChar)
                {
                    case '=':
                        tokens.Add("=");
                        break;
                    case ';':
                        tokens.Add(";");
                        break;
                    case ',':
                        tokens.Add(",");
                        break;
                    case '(':
                        tokens.Add("(");
                        break;
                    case ')':
                        tokens.Add(")");
                        break;
                    case '+':
                        tokens.Add("+");
                        break;
                    case '-':
                        tokens.Add("-");
                        break;
                    case '*':
                        tokens.Add("*");
                        break;
                    case '/':
                        tokens.Add("/");
                        break;
                    case ':': // Добавляем обработку символа ':'
                        if (position + 1 < input.Length && input[position + 1] == 'I') // Проверяем, есть ли 'I' дальше
                        {
                            // Исправляем ':Integer;'
                            tokens.Add(":Integer");
                            position += 8; // Пропускаем ':Integer;'
                            continue;
                        }
                        else
                        {
                            throw new Exception($"Неизвестный символ: {currentChar}");
                        }
                    default:
                        throw new Exception($"Неизвестный символ: {currentChar}");
                }
                position++;
            }

            return tokens;
        }


        private string ReadIdentifier()
        {
            int start = position;
            while (position < input.Length && (char.IsLetter(input[position]) || char.IsDigit(input[position])))
            {
                position++;
            }
            return input.Substring(start, position - start);
        }

        private string ReadNumber()
        {
            int start = position;
            while (position < input.Length && char.IsDigit(input[position]))
            {
                position++;
            }
            return input.Substring(start, position - start);
        }
    }
}
