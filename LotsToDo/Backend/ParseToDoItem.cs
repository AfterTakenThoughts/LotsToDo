using System;
using System.Collections.Generic;
using System.Text;
using LotsToDo.Backend;

public static class ParseToDoItem
{
    public static ToDoItem ParseProperties(ToDoItem item, string content)
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
            Dictionary<string, List<string>> tags = [];

            string tagName = "";
            List<string> tagContents = [];

            bool insideParenthesis = false;
            StringBuilder word = new();
            for (int i = startStringIndex; i < content.Length; i++)
            {
                switch (content[i])
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
                }
                if (insideParenthesis)
                {
                    if (content[i] != ',')
                    {
                        tagContents.Add(word.ToString().Trim());
                    }
                    else
                    {
                        word.Append(content[i]);
                    }
                }
                else if (Char.IsWhiteSpace(content[i]) == false && content[i] != ':' && content[i] != ',')
                {
                    word.Append(content[i]);
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