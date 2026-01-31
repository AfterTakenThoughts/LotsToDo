using System;
using System.Collections.Generic;
using LotsToDo.Backend.FileIO.Parser.ExtractionMethods;
using LotsToDo.Backend.StringHandlingExtensions;

namespace LotsToDo.Backend.FileIO.Parser.AttributeInfo;

//TODO: add string comparison options.
public readonly struct AttributeByWord(string attributeName, BypassKeyword bypassKeyword, int extractWordCount, ParseDirection textDirection) : IAttributeInfo
{
    public string AttributeName { get; } = attributeName;
    public BypassKeyword BypassKeyword { get; } = bypassKeyword;
    public int ExtractWordCount { get; } = extractWordCount;
    public ParseDirection TextDirection { get; } = textDirection;

    public List<string> ExtractMultiAttribute(string content, Range tagRange, out string remainingContent)
    {
        throw new NotImplementedException();
    }

    public string ExtractSingleAttribute(string content, Range tagRange, out string remainingContent)
    {
        remainingContent = content;
        int startAttributeIndex = BypassKeyword.SkipBypassKeywords(content, TextDirection, tagRange.End.Value);
        string[] segmentWords = content[startAttributeIndex..].Split(' ');
        if (segmentWords.Length != 0)
        {
            Range attributeRange = startAttributeIndex..(startAttributeIndex + segmentWords[0].Length);
            remainingContent = TrimString.RemoveRange(content, [tagRange, attributeRange]);
            return segmentWords[0];
        }
        else
        {
            return "";
        }
    }
}