using System;
using System.Collections.Generic;

namespace LotsToDo.Backend.FileIO.Parser.ParserMethods;
public class ParseMultiAttribute(string tagName, List<IAttributeInfo> keyWordList) : ITagParser
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
        List<string> attributeList = [];
        foreach (IAttributeInfo info in KeyWordList)
        {
            int tagIndex = content.IndexOf(info.AttributeName);
            if (tagIndex != -1)
            {
                int tagEndIndex = tagIndex + info.AttributeName.Length;
                Range tagRange = tagIndex..tagEndIndex;
                attributeList.AddRange(info.ExtractMultiAttribute(content, tagRange, out remainingContent));
            }
        }
        return attributeList;
    }
}
