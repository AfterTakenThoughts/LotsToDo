using System;

namespace LotsToDo.Backend.StringHandlingExtensions;
public static class TrimString
{
    public static string TrimStart(this string content, out int trimmedLength)
    {
        string trimmedContent = content.TrimStart();
        trimmedLength = Math.Abs(content.Length - trimmedContent.Length);
        return trimmedContent;
    }
    public static string TrimEnd(this string content, out int trimmedLength)
    {
        string trimmedContent = content.TrimEnd();
        trimmedLength = Math.Abs(content.Length - trimmedContent.Length);
        return trimmedContent;
    }
    public static string Trim(this string content, out int trimmedStartLength, out int trimmedEndLength)
    {
        string trimmedContent = content.TrimStart(out trimmedStartLength);
        trimmedContent = trimmedContent.TrimEnd(out trimmedEndLength);
        return trimmedContent;
    }
    public static string RemoveRange(string content, Range[] removeRange)
    {
        for (int i = removeRange.Length - 1; i >= 0 ; i--)
        {
            content = content.Remove(removeRange[i].Start.Value, removeRange[i].End.Value - removeRange[i].Start.Value);
        }
        return content;
    }
}