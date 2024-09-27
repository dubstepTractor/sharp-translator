using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Core
{
    public class Parser
    {
        private List<string> tokens;
        private int currentToken;

        public Parser(List<string> tokens)
        {
            this.tokens = tokens;
            currentToken = 0;
        }

        public Node Parse()
        {
            return Program();
        }

        private Node Program()
        {
            var node = new Node { Type = "PROGRAM" };
            node.Children.Add(VariableDeclaration());
            node.Children.Add(ComputationalDescription());
            node.Children.Add(PrintOperator());
            return node;
        }

        private Node VariableDeclaration()
        {
            if (tokens[currentToken] != "VAR")
                throw new Exception("Ожидалось 'Var'");

            currentToken++; // Пропускаем "Var"
            var node = new Node { Type = "VARIABLE_DECLARATION" };
            node.Children.Add(VariableList());

            if (tokens[currentToken] != ":Integer")
                throw new Exception("Ожидалось ':Integer'");

            currentToken++; // Пропускаем ":Integer;"
            return node;
        }

        private Node VariableList()
        {
            var node = new Node { Type = "VARIABLE_LIST" };
            node.Children.Add(Identifier());

            while (tokens[currentToken] == ",")
            {
                currentToken++; // Пропускаем ","
                node.Children.Add(Identifier());
            }
            return node;
        }

        private Node ComputationalDescription()
        {
            if (tokens[currentToken] != "BEGIN")
                throw new Exception("Ожидалось 'BEGIN'");

            currentToken++; // Пропускаем "Begin"
            var node = new Node { Type = "COMPUTATIONAL_DESCRIPTION" };
            node.Children.Add(Assignments());

            if (tokens[currentToken] != "End")
                throw new Exception("Ожидалось 'End'");

            currentToken++; // Пропускаем "End"
            return node;
        }

        private Node Assignments()
        {
            var node = new Node { Type = "ASSIGNMENTS" };
            node.Children.Add(Assignment());

            while (tokens[currentToken] == "VAR")
            {
                node.Children.Add(Assignment());
            }
            return node;
        }

        private Node Assignment()
        {
            var node = new Node { Type = "ASSIGNMENT" };
            node.Children.Add(Identifier());

            if (tokens[currentToken] != "=")
                throw new Exception("Ожидалось '='");

            currentToken++; // Пропускаем "="
            node.Children.Add(Expression());

            if (tokens[currentToken] != ";")
                throw new Exception("Ожидалось ';'");

            currentToken++; // Пропускаем ";"
            return node;
        }

        private Node Expression()
        {
            var left = Subexpression();
            while (IsBinaryOperator(tokens[currentToken]))
            {
                var node = new Node { Type = "BINARY_EXPRESSION", Value = tokens[currentToken] };
                currentToken++; // Пропускаем бинарный оператор
                node.Children.Add(left);
                node.Children.Add(Subexpression());
                left = node;
            }
            return left;
        }

        private Node Subexpression()
        {
            if (tokens[currentToken] == "(")
            {
                currentToken++; // Пропускаем "("
                var expr = Expression();
                if (tokens[currentToken] != ")")
                    throw new Exception("Ожидалось ')'");

                currentToken++; // Пропускаем ")"
                return expr;
            }
            else if (tokens[currentToken] == "-")
            {
                var node = new Node { Type = "UNARY_EXPRESSION", Value = tokens[currentToken] };
                currentToken++; // Пропускаем "-"
                node.Children.Add(Subexpression());
                return node;
            }
            else if (IsOperand(tokens[currentToken]))
            {
                return Operand();
            }
            throw new Exception("Неожиданный токен");
        }

        private Node Operand()
        {
            if (IsIdentifier(tokens[currentToken]))
            {
                return Identifier();
            }
            else if (IsConstant(tokens[currentToken]))
            {
                return new Node { Type = "CONSTANT", Value = tokens[currentToken++] };
            }
            throw new Exception("Неожиданный токен при разборе операнда");
        }

        private Node Identifier()
        {
            return new Node { Type = "IDENTIFIER", Value = tokens[currentToken++] };
        }

        private Node PrintOperator()
        {
            if (tokens[currentToken] != "PRINT")
                throw new Exception("Ожидалось 'PRINT'");

            currentToken++; // Пропускаем "PRINT"
            return new Node { Type = "PRINT_OPERATOR", Value = tokens[currentToken++] };
        }

        private bool IsBinaryOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/";
        }

        private bool IsIdentifier(string token)
        {
            return char.IsLetter(token[0]); // Простое определение идентификатора
        }

        private bool IsConstant(string token)
        {
            return char.IsDigit(token[0]); // Простое определение константы
        }

        private bool IsOperand(string token)
        {
            return IsIdentifier(token) || IsConstant(token);
        }
    }
}
