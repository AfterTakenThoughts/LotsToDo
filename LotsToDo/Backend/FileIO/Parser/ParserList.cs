// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text.RegularExpressions;
// using LotsToDo.Backend.FileIO.Parser.ParserMethods;

// namespace LotsToDo.Backend.FileIO.Parser;
// public class ParserList
// {
//     public List<ParseSingleAttribute> Parsers { get; set; }
//     public Dictionary<string, List<string>> ParseString(string text)
//     {
//         Dictionary<string, List<string>> tags = [];
//         foreach (ParseSingleAttribute parser in Parsers)
//         {
//             List<string> segment = [];
//             segment = parser.TextDirection switch
//             {
//                 ParseDirection.ParseLeft => [text.Split(parser.KeyWordList)[1..]],
//                 ParseDirection.ParseRight => [text.Split(keyWord)[..^1]],
//                 ParseDirection.ParseBoth => segment.
//             };
//             if (parser.IsParseMultiLine)
//             {
//                 segment =
//                 List<string> attributes = ParseAttributes(parser, text);
//                 tags.Add(parser.TagName, attributes);
//             }qqqqq
//         }
//         return tags;
//     }
// }