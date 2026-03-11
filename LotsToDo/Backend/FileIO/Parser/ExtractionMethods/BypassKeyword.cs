using System;
using System.Collections.Generic;
using System.Linq;
using LotsToDo.Backend.StringHandlingExtensions;

namespace LotsToDo.Backend.FileIO.Parser.ExtractionMethods;

public class BypassKeyword
{
    public List<MatchInfo> BypassKeywordList { get; }

    public BypassKeyword(List<MatchInfo> bypassKeywordList)
    {
        BypassKeywordList = bypassKeywordList;
    }
    public BypassKeyword(List<string> bypassKeywordList)
    {
        BypassKeywordList = [.. bypassKeywordList.Select(x => new MatchInfo(x))];
    }

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
        foreach (MatchInfo keyWord in BypassKeywordList)
        {
            if (CharComparisons.StartsWith(content, keyWord.MatchString, startStringIndex, StringComparer.FromComparison(keyWord.Comparer)))
            {
                bypassKeyWord = keyWord.MatchString;
                return startStringIndex + bypassKeyWord.Length;
            }
        }
        return -1;
    }
    int BypassKeywordLeft(string content, int startStringIndex, out string bypassKeyWord)
    {
        bypassKeyWord = "";
        foreach (MatchInfo keyWord in BypassKeywordList)
        {
            if (CharComparisons.EndsWith(content, keyWord.MatchString, startStringIndex, StringComparer.FromComparison(keyWord.Comparer)))
            {
                bypassKeyWord = keyWord.MatchString;
                return startStringIndex - bypassKeyWord.Length;
            }
        }
        return -1;
    }
}