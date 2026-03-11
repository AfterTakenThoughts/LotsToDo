using System;
using System.Collections.Generic;
using LotsToDo.Backend.FileIO.Parser.AttributeInfo;

namespace LotsToDo.Backend.FileIO.Parser.ParserMethods;
public class ParseSingleAttribute(string tagName, List<IAttributeInfo> keyWordList) : ITagParser
{
    public string TagName { get; init; } = tagName;
    public List<IAttributeInfo> KeyWordList { get; set; } = keyWordList;

    /// <summary>
    /// Parses the string into a single tag with multiple items.
    /// </summary>
    /// <param name="content">The contents of the string to parse.</param>
    public List<string> ParseAttributes(string content, out string remainingContent)
    {
        remainingContent = content;
        foreach (IAttributeInfo info in KeyWordList)
        {
            int tagIndex = content.IndexOf(info.TagIdentifierMatch.MatchString, info.TagIdentifierMatch.Comparer);
            if (tagIndex != -1)
            {
                int tagEndIndex = tagIndex + info.TagIdentifierMatch.MatchString.Length;
                Range tagRange = tagIndex..tagEndIndex;
                return [info.ExtractSingleAttribute(content, tagRange, out remainingContent)];
            }
        }
        return [];
    }
}
