using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace SRTParser
{
    class Subtitle
    {
        public int Index { get; set; }
        private TimeSpan _start;
        public TimeSpan Start
        { 
            get
            {
                return _start;
            }
            set
            {
                _start = value;

                if (End != null)
                    Duration = End.Subtract(Start).Duration();
            }
        }
        public TimeSpan _end;
        public TimeSpan End
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
                Duration = End.Subtract(Start).Duration();
            }
        }
        public TimeSpan Duration { get; set; }
        private string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                Size = Content.Length;
                // TODO: Change this to a string and count line breaks separately.
            }
        }
        public int Size { get; set; }

        private static Regex _regex = new Regex(@"-->");
        public static List<Subtitle> Parse(string filepath)
        {
            var subtitles = new List<Subtitle>();

            using (var stream = new System.IO.StreamReader(filepath))
            {
                var curSub = new Subtitle();

                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    int index;
                    var split = _regex.Split(line);
                    if (line == String.Empty) // A blank line, signaling that this caption group is done.
                    {
                        // Easiest way (that is, no additional state required) is to make sure there's newlines between manually broken
                        // caption groups is to add newlines at the end of every line of text and then remove the very last line break.
                        curSub.Content = curSub.Content.TrimEnd('\r', '\n');
                        subtitles.Add(curSub);

                        curSub = new Subtitle();
                        continue;
                    }
                    else if (int.TryParse(line, out index)) // subtitle number
                        curSub.Index = index;
                    else if (split.Length > 1) // "start timespan --> end timespan"
                    {
                        curSub.Start = ParseTimeSpan(split[0]);
                        curSub.End = ParseTimeSpan(split[1]);
                    }
                    else // non-blank text
                    {
                        curSub.Content += line + Environment.NewLine;
                    }
                }
                stream.Close();
            }

            return subtitles.OrderBy(ts => ts.Index).ToList();
        }

        private static TimeSpan ParseTimeSpan(string value)
        {
            int h, m, s, ms;

            var tokens = value.Split(':');            
            if (tokens.Length == 3)
            {
                h = int.Parse(tokens[0]);
                m = int.Parse(tokens[1]);

                var sms = tokens[2].Split(',');
                if (sms.Length == 2)
                {
                    s = int.Parse(sms[0]);
                    ms = int.Parse(sms[1]);
                }
                else
                {
                    throw new Exception("Subtitle: Correct format is HH:MM:SS,MS.");
                }
            }
            else
            {
                throw new Exception("Subtitle: Correct format is HH:MM:SS,MS.");
            }

            return new TimeSpan(0, h, m, s, ms);
        }
    }
}
