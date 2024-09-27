using System;

namespace Translator.Core
{
    public class CodeGenerator
    {
        private int indentLevel = 0;

        public string Generate(Node node)
        {
            switch (node.Type)
            {
                case "PROGRAM":
                    return GenerateProgram(node);
                default:
                    throw new Exception($"Неизвестный тип узла: {node.Type}");
            }
        }

        private string GenerateProgram(Node node)
        {
            return GenerateVariableDeclaration(node.Children[0]) +
                   GenerateComputationalDescription(node.Children[1]) +
                   GeneratePrintOperator(node.Children[2]);
        }

        private string GenerateVariableDeclaration(Node node)
        {
            var code = "";
            foreach (var child in node.Children)
            {
                code += GenerateVariableList(child) + Environment.NewLine;
            }
            return code;
        }

        private string GenerateVariableList(Node node)
        {
            return $"section .bss" + Environment.NewLine +
                   $"    {node.Children[0].Value} resd 1"; // Для каждой переменной выделяем 1 DWORD
        }

        private string GenerateComputationalDescription(Node node)
        {
            var code = "section .text" + Environment.NewLine + "    global _start" + Environment.NewLine + "_start:" + Environment.NewLine;
            foreach (var assignment in node.Children)
            {
                code += GenerateAssignment(assignment) + Environment.NewLine;
            }
            return code;
        }

        private string GenerateAssignment(Node node)
        {
            var identifier = node.Children[0].Value;
            var expression = GenerateExpression(node.Children[1]);
            return $"{identifier} = {expression}"; // Здесь должно быть преобразование в код ассемблера
        }

        private string GenerateExpression(Node node)
        {
            if (node.Type == "BINARY_EXPRESSION")
            {
                var left = GenerateExpression(node.Children[0]);
                var right = GenerateExpression(node.Children[1]);
                return $"({left} {node.Value} {right})"; // Пример, нужно будет заменить на реальный ассемблерный код
            }
            else if (node.Type == "UNARY_EXPRESSION")
            {
                var subExpr = GenerateExpression(node.Children[0]);
                return $"{node.Value}{subExpr}"; // Пример унарного оператора
            }
            else if (node.Type == "CONSTANT")
            {
                return node.Value; // Число
            }
            else if (node.Type == "IDENTIFIER")
            {
                return node.Value; // Идентификатор
            }

            throw new Exception("Неизвестный тип выражения");
        }

        private string GeneratePrintOperator(Node node)
        {
            return $"    ; Вывод {node.Value}"; // Генерация кода на печать
        }
    }
}

