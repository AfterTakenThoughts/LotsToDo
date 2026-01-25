using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LotsToDo.Backend.FileIO.Parser.ParserMethods;
public class ParseSingleAttribute(string tagName, List<AttributeInfo> keyWordList) : ITagParser
{
    public string TagName { get; init; } = tagName;
    public List<AttributeInfo> KeyWordList { get; set; } = keyWordList;

    /// <summary>
    /// Parses the string into a single tag with multiple items.
    /// </summary>
    /// <
    public List<string> ParseAttributes(string segment)
    {
        //TODO: change this segment of code such that it reads the tag attribute and then searches (include tag attribute in the segment param)
        foreach (AttributeInfo info in KeyWordList)
        {
            int indexOfTag = segment.IndexOf(info.AttributeName);
            if (indexOfTag != -1)
            {
                string trimmedSegment = TrimBypassKeywords(segment[(indexOfTag + info.AttributeName.Length)..].Trim(), info);
                if (info.AttributeFormat != null && info.AttributeFormat.Count != 0)
                {
                    return ExtractAttributes(trimmedSegment, info);
                }
                else
                {
                    string[] segmentWords = trimmedSegment.Split(' ');
                    return [segmentWords[0]];
                }
            }
        }
        return [];
    }
    static string TrimBypassKeywords(string segment, AttributeInfo info)
    {
        int i = 0;
        while (i < info.BypassKeywordList.Count)
        {
            if (segment.StartsWith(info.BypassKeywordList[i]))
            {
                segment = segment[info.BypassKeywordList[i].Length..].Trim();
                i = 0;
            }
            else
            {
                i++;
            }
        }
        return segment;
    }
    static List<string> ExtractAttributes(string segment, AttributeInfo info)
    {
        foreach (string attribute in info.AttributeFormat)
        {
            Match m = Regex.Match(segment, attribute);
            if (m.Success)
            {
                return [m.Groups[1].Value];
            }
        }
        return [];
    }
}
