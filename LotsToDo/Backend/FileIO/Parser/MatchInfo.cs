using System;
using System.Collections.Specialized;

namespace LotsToDo.Backend.FileIO.Parser;

public class MatchInfo(string matchString, StringComparison comparer = StringComparison.CurrentCulture)
{
    public string MatchString { get; set; } = matchString;
    public StringComparison Comparer { get; set; } = comparer;
}