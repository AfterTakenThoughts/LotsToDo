using System;
using System.Collections.Generic;
using System.Text;
using LotsToDo.Backend;

public static class ParseTaskItem
{
    public static TaskItem ParseProperties(TaskItem item, string content)
    {
        string[] splitLines = content.Split(':');
        if (splitLines[0].TrimEnd().EndsWith("Tags") && splitLines.Length >= 2)
        {
            item.Tags = ParseTags(splitLines[0].Length - 1, content);
        }
        else
        {
            ParseTime(content);
        }
        return item;


        static Dictionary<string, List<string>> ParseTags(int startStringIndex, string content)
        {
            string contentWithoutTag = content[(startStringIndex + 1)..].Trim().TrimStart(':').Trim();
            Dictionary<string, List<string>> tags = [];

            string tagName = "";
            List<string> tagContents = [];

            bool insideParenthesis = false;
            StringBuilder word = new();
            for (int i = 0; i < contentWithoutTag.Length; i++)
            {
                //Ordered so it performs on the next loop (don't include '(')
                if (insideParenthesis)
                {
                    if (contentWithoutTag[i] == ',' || contentWithoutTag[i] == ')')
                    {
                        tagContents.Add(word.ToString().Trim());
                        word.Clear();
                    }
                    else
                    {
                        word.Append(contentWithoutTag[i]);
                    }
                }
                switch (contentWithoutTag[i])
                {
                    case '(':
                        insideParenthesis = true;
                        break;
                    case ')':
                        tags.Add(tagName, tagContents);
                        tagName = "";
                        tagContents = [];
                        insideParenthesis = false;
                        break;
                    case ':':
                        if (insideParenthesis == false)
                        {
                            tagName = word.ToString();
                            tagContents = [];
                            word.Clear();
                        }
                        break;
                    default:
                        if (insideParenthesis == false && Char.IsWhiteSpace(contentWithoutTag[i]) == false && contentWithoutTag[i] != ':' && contentWithoutTag[i] != ',')
                        {
                            word.Append(contentWithoutTag[i]);
                        }
                        break;
                }
            }
            return tags;
        }
        void ParseTime(string line)
        {
            string[] words = line.Split(' ');

            SelectTime selectTime = SelectTime.None;
            StringBuilder? parsedPhrase = null;

            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Contains("Start"))
                {
                    ParseWord(words[i], "Start");
                    selectTime = SelectTime.StartTime;
                }
                else if (words[i].Contains("Due"))
                {
                    ParseWord(words[i], "Due");
                    selectTime = SelectTime.DueDate;
                }
                else if (words[i].Contains("Created"))
                {
                    ParseWord(words[i], "Created");
                    selectTime = SelectTime.CreateTime;
                }
                else
                {
                    parsedPhrase?.Append($" {words[i]}");
                }
            }
            if (parsedPhrase != null)
            {
                ParseDateTime(parsedPhrase, selectTime);
            }


            void ParseWord(string word, string targetWord)
            {
                int startIndex = word.IndexOf(targetWord);
                int endIndex = startIndex + targetWord.Length;
                if (parsedPhrase != null)
                {
                    ParseDateTime(parsedPhrase.Append(word[..startIndex]), selectTime);
                }
                parsedPhrase = new(word[endIndex..]);
            }
            void ParseDateTime(StringBuilder content, SelectTime selectTime)
            {
                if (content == null || content.Length == 0)
                {
                    return;
                }
                string trimmedContent = content.ToString().Trim().TrimStart(':').Trim();
                DateTime time = DateTime.Parse(trimmedContent);
                switch (selectTime)
                {
                    case SelectTime.StartTime:
                        item.StartTime = time;
                        break;
                    case SelectTime.DueDate:
                        item.DueDate = time;
                        break;
                    case SelectTime.CreateTime:
                        item.CreateTime = time;
                        break;
                }
            }
        }
    }
    enum SelectTime
    {
        None,
        StartTime,
        DueDate,
        CreateTime
    }

}