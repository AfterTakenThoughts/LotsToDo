using System.Collections.Generic;

namespace LotsToDo.Backend.StringHandlingExtensions;
public static class CharComparisons
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
    public static bool StartsWith(this string content, string value, int startStringIndex = 0)
    {
        //Escape condition when the length diff between startStringIndex and content end is longer than value.
        if (content.Length - startStringIndex < value.Length)
        {
            return false;
        }
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] != content[i + startStringIndex])
            {
                return false;
            }
        }
        return true;
    }
    public static bool EndsWith(this string content, string value, int startStringIndex = 0)
    {
        //Escape condition when the length diff between startStringIndex and content start is longer than value.
        if (startStringIndex < value.Length)
        {
            return false;
        }
        for (int i = value.Length - 1; i >= 0; i--)
        {
            if (value[i] != content[i + startStringIndex])
            {
                return false;
            }
        }
        return true;
    }
}