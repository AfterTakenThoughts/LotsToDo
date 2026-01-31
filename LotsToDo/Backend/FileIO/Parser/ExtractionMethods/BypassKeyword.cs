using System;
using System.Collections.Generic;
using LotsToDo.Backend.StringHandlingExtensions;

namespace LotsToDo.Backend.FileIO.Parser.ExtractionMethods;

public class BypassKeyword(List<string> bypassKeywordList)
{
    public List<string> BypassKeywordList { get; } = bypassKeywordList;
    /// <summary>
    /// Skips any bypass words in <paramref name="info"/> to the relevant index.
    /// </summary>
    /// <remarks>Unless specified otherwise, this method does not remove whitespaces.</remarks>
    ///<param name="segment">The string to trim.</param>
    ///<param name="info">The attribute information for the method to trim the string out of.</param>
    ///<param name="startStringIndex">The index to start removing bypass keywords.</param>
    public int SkipBypassKeywords(string segment, ParseDirection direction, int startStringIndex)
    {
        int bypassIndex = 0;
        while (bypassIndex != -1)
        {
            bypassIndex = BypassSingleKeyword(segment, direction, startStringIndex, out _);
            if (bypassIndex != -1)
            {
                startStringIndex = bypassIndex;
            }
        }
        return startStringIndex;
    }

    int BypassSingleKeyword(string segment, ParseDirection direction, int startStringIndex, out string bypassKeyWord)
    {
        bypassKeyWord = "";
        return direction switch
        {
            ParseDirection.ParseLeft => BypassKeywordLeft(segment, startStringIndex, out bypassKeyWord),
            ParseDirection.ParseRight => BypassKeywordRight(segment, startStringIndex, out bypassKeyWord),
            _ => -1,
        };
    }
    int BypassKeywordRight(string content, int startStringIndex, out string bypassKeyWord)
    {
        bypassKeyWord = "";
        foreach (string keyWord in BypassKeywordList)
        {
            if (CharComparisons.StartsWith(content, keyWord, startStringIndex))
            {
                bypassKeyWord = keyWord;
                return startStringIndex + bypassKeyWord.Length;
            }
        }
        return -1;
    }
    int BypassKeywordLeft(string content, int startStringIndex, out string bypassKeyWord)
    {
        bypassKeyWord = "";
        foreach (string keyWord in BypassKeywordList)
        {
            if (CharComparisons.EndsWith(content, keyWord, startStringIndex))
            {
                bypassKeyWord = keyWord;
                return startStringIndex - bypassKeyWord.Length;
            }
        }
        return -1;
    }
}