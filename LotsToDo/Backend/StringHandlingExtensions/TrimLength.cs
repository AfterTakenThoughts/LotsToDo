using System;
using System.Collections.Generic;
using Avalonia;

namespace LotsToDo.Backend.StringHandlingExtensions;
public static class CharComparison
{
    public static int CharacterCountAtStart(this string content, List<char> chars, int startStringIndex = 0)
    {
        int count = 0;
        for (int i = startStringIndex; i < content.Length; i++)
        {
            if (chars.Contains(content[i]))
            {
                count++;
                continue;
            }
            else
            {
                break;
            }
        }
        return count;
    }
    public static int CharacterCountAtEnd(this string content, List<char> chars, int startStringIndex = -1)
    {
        int count = 0;
        if (startStringIndex == -1)
        {
            startStringIndex = content.Length - 1;
        }
        for (int i = startStringIndex; i >= 0; i--)
        {
            if (chars.Contains(content[i]))
            {
                count++;
                continue;
            }
            else
            {
                break;
            }
        }
        return count;
    }
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
    public static bool StartsWith(this string content, string value, int startStringIndex = 0)
    {
        for (int i = 0; i < content.Length - startStringIndex; i++)
        {
            if (i == value.Length)
            {
                return true;
            }
            else if (value[i] != content[i + startStringIndex])
            {
                return false;
            }
        }
        return false;
    }
}