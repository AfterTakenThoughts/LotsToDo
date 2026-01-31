using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LotsToDo.Backend.StringHandlingExtensions;

namespace LotsToDo.Backend.FileIO.Parser.ParserMethods;
public class ParseSingleAttribute(string tagName, List<AttributeInfo> keyWordList) : ITagParser
{
    public string TagName { get; init; } = tagName;
    public List<AttributeInfo> KeyWordList { get; set; } = keyWordList;

    /// <summary>
    /// Parses the string into a single tag with multiple items.
    /// </summary>
    /// <param name="content">The contents of the string to parse.</param>
    public List<string> ParseAttributes(string content, out string remainingContent)
    {
        remainingContent = content;
        foreach (AttributeInfo info in KeyWordList)
        {
            int tagIndex = content.IndexOf(info.AttributeName);
            if (tagIndex != -1)
            {
                int tagEndIndex = tagIndex + info.AttributeName.Length;
                Range tagRange = tagIndex..tagEndIndex;
                if (info.AttributeFormat != null && info.AttributeFormat.Count != 0)
                {
                    string? attribute = ExtractAttributes(content, info, tagEndIndex, out Range attributeRange);
                    if (attribute != null)
                    {
                        remainingContent = RemoveRange(content, [tagRange, attributeRange]);
                        return [attribute];
                    }
                    else
                    {
                        return [];
                    }
                }
                else
                {
                    int startAttributeIndex = BypassKeyword(content, info, tagEndIndex);
                    string[] segmentWords = content[startAttributeIndex..].Split(' ');
                    if (segmentWords.Length != 0)
                    {
                        Range attributeRange = startAttributeIndex..(startAttributeIndex + segmentWords[0].Length);
                        remainingContent = RemoveRange(content, [tagRange, attributeRange]);
                        return [segmentWords[0]];
                    }
                    else
                    {
                        return [];
                    }
                }
            }
        }
        return [];
    }

    static string RemoveRange(string content, Range[] removeRange)
    {
        for (int i = removeRange.Length - 1; i >= 0 ; i--)
        {
            content = content.Remove(removeRange[i].Start.Value, removeRange[i].End.Value - removeRange[i].Start.Value);
        }
        return content;
    }

    /// <summary>
    /// Reads the attributes and skips any bypass words.
    /// </summary>
    static string? ExtractAttributes(string content, AttributeInfo info, int startStringIndex, out Range attributeRange)
    {
        foreach (string attribute in info.AttributeFormat)
        {
            startStringIndex = BypassKeyword(content, info, startStringIndex);
            Match m = Regex.Match(content[startStringIndex..], attribute);
            if (m.Success)
            {
                startStringIndex += m.Index;
                attributeRange = startStringIndex..(startStringIndex + m.Groups[1].Value.Length);
                return m.Groups[1].Value;
            }
        }
        attributeRange = new();
        return null;
    }
    /// <summary>
    /// Skips any bypass words in <paramref name="info"/> to the relevant index.
    /// </summary>
    /// <remarks>Unless specified otherwise, this method does not remove whitespaces.</remarks>
    ///<param name="segment">The string to trim.</param>
    ///<param name="info">The attribute information for the method to trim the string out of.</param>
    ///<param name="startStringIndex">The index to start removing bypass keywords.</param>
    static int BypassKeyword(string segment, AttributeInfo info, int startStringIndex)
    {
        int bypassIndex = 0;
        while (bypassIndex != -1)
        {
            bypassIndex = BypassSingleKeyword(segment, info, startStringIndex, out _);
            if (bypassIndex != -1)
            {
                startStringIndex = bypassIndex;
            }
        }
        return startStringIndex;
    }

    static int BypassSingleKeyword(string segment, AttributeInfo info, int startStringIndex, out string bypassKeyWord)
    {
        bypassKeyWord = "";
        return info.TextDirection switch
        {
            ParseDirection.ParseLeft => BypassKeywordLeft(segment, info, startStringIndex, out bypassKeyWord),
            ParseDirection.ParseRight => BypassKeywordRight(segment, info, startStringIndex, out bypassKeyWord),
            _ => -1,
        };
    }
    static int BypassKeywordRight(string content, AttributeInfo info, int startStringIndex, out string bypassKeyWord)
    {
        bypassKeyWord = "";
        foreach (string keyWord in info.BypassKeywordList)
        {
            if (CharComparison.StartsWith(content, keyWord, startStringIndex))
            {
                bypassKeyWord = keyWord;
                return startStringIndex + bypassKeyWord.Length;
            }
        }
        return -1;
    }
    //TODO: use charcomparison
    static int BypassKeywordLeft(string content, AttributeInfo info, int startStringIndex, out string bypassKeyWord)
    {
        bypassKeyWord = "";
        foreach (string keyWord in info.BypassKeywordList)
        {
            if (content.EndsWith(keyWord))
            {
                bypassKeyWord = keyWord;
                return startStringIndex - bypassKeyWord.Length;
            }
        }
        return -1;
    }
}
