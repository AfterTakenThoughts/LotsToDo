using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LotsToDo.Backend.FileIO.Parser.ExtractionMethods;
using LotsToDo.Backend.StringHandlingExtensions;

namespace LotsToDo.Backend.FileIO.Parser.AttributeInfo;

//TODO: add string comparison options.
public readonly struct AttributeByRegex(string attributeName, BypassKeyword bypassKeyword, List<string> attributeFormat, ParseDirection textDirection) : IAttributeInfo
{
    public string AttributeName { get; } = attributeName;
    public BypassKeyword BypassKeyword { get; } = bypassKeyword;
    public List<string> AttributeFormat { get; } = attributeFormat;
    public ParseDirection TextDirection { get; } = textDirection;
    public List<string> ExtractMultiAttribute(string content, Range tagRange, out string remainingContent)
    {
        throw new NotImplementedException();
    }

    public string ExtractSingleAttribute(string content, Range tagRange, out string remainingContent)
    {
        remainingContent = content;
        string? attribute = ExtractAttributes(content, tagRange.End.Value, out Range attributeRange);
        if (attribute != null)
        {
            remainingContent = TrimString.RemoveRange(content, [tagRange, attributeRange]);
            return attribute;
        }
        else
        {
            return "";
        }
    }

    string? ExtractAttributes(string content, int startStringIndex, out Range attributeRange)
    {
        foreach (string attribute in AttributeFormat)
        {
            startStringIndex = BypassKeyword.SkipBypassKeywords(content, TextDirection, startStringIndex);
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
}