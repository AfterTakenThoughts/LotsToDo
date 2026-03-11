using System;
using System.Collections.Generic;

namespace LotsToDo.Backend.FileIO.Parser.AttributeInfo;
public interface IAttributeInfo
{
    /// <summary>
    /// The attribute identifier to read from the string contents.
    /// </summary>
    public MatchInfo TagIdentifierMatch { get; }
    /// <summary>
    /// Reads the attributes and skips any bypass words.
    /// </summary>
    public string ExtractSingleAttribute(string content, Range tagRange, out string remainingContent);
    /// <summary>
    /// Reads the attributes and skips any bypass words.
    /// </summary>
    public List<string> ExtractMultiAttribute(string content, Range tagRange, out string remainingContent);
}