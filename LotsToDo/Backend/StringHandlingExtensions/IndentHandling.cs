using System;
using System.Text;

namespace LotsToDo.Backend.StringHandlingExtensions;


public class IndentHandling
{
    public static string GetIndent(string indentLiteral, int indentLength)
    {
        StringBuilder indent = new();
        for (int i = 0; i < indentLength; i++)
        {
            indent.Append(indentLiteral);
        }
        return indent.ToString();
    }
    public static StringBuilder IndentString(StringBuilder content, string indent)
    {
        content.Insert(0, indent);
        return content.Replace(Environment.NewLine, $"{Environment.NewLine}{indent}");
    }
    public static StringBuilder IndentString(StringBuilder content, string indentLiteral, int indentLength)
    {
        string indent = GetIndent(indentLiteral, indentLength);
        return IndentString(content, indent);
    }
    public static string IndentString(string content, string indent)
    {
        return IndentString(new StringBuilder(content), indent).ToString();
    }
    public static string IndentString(string content, string indentLiteral, int indentLength)
    {
        return IndentString(new StringBuilder(content), indentLiteral, indentLength).ToString();
    }
}