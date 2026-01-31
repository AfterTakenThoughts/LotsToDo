using System;
using System.Collections.Generic;
using LotsToDo.Backend.FileIO.Parser.ExtractionMethods;
using LotsToDo.Backend.StringHandlingExtensions;

namespace LotsToDo.Backend.FileIO.Parser.AttributeInfo;

//TODO: add string comparison options.
public readonly struct AttributeByWord : IAttributeInfo
{
    public MatchInfo TagIdentifierMatch { get; }
    public BypassKeyword BypassKeyword { get; }
    public int ExtractWordCount { get; }
    public ParseDirection TextDirection { get; }

    public AttributeByWord(MatchInfo tagIdentifier, BypassKeyword bypassKeyword, int extractWordCount, ParseDirection textDirection)
    {
        TagIdentifierMatch = tagIdentifier;
        BypassKeyword = bypassKeyword;
        ExtractWordCount = extractWordCount;
        TextDirection = textDirection;
    }

    public AttributeByWord(string tagIdentifier, BypassKeyword bypassKeyword, int extractWordCount, ParseDirection textDirection)
    {
        TagIdentifierMatch = new(tagIdentifier);
        BypassKeyword = bypassKeyword;
        ExtractWordCount = extractWordCount;
        TextDirection = textDirection;
    }

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