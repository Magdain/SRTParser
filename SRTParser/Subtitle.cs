using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace SRTParser
{
    class Timestamp
    {
        public int Hours, Minutes, Seconds, Milliseconds;
        private string _value;
        public Timestamp(string value)
        {
            var tokens = value.Split(':');
            if (tokens.Length == 3)
            {
                Hours = int.Parse(tokens[0]);
                Minutes = int.Parse(tokens[1]);

                var sms = tokens[2].Split(',');
                if (sms.Length == 2)
                {
                    Seconds = int.Parse(sms[0]);
                    Milliseconds = int.Parse(sms[1]);
                }
                else
                {
                    throw new Exception("Timestamp expects a comma delimited seconds,milliseconds value.");
                }

                _value = value;
            }
            else
            {
                throw new Exception("Timestamp expects three values delimited by a colon.");
            }
        }
        public Timestamp(int hours, int minutes, int seconds, int milliseconds)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Milliseconds = milliseconds;
            _value = string.Format("{0}:{1}:{2},{3}", Hours, Minutes, Seconds, Milliseconds);
        }

        public override string ToString()
        {
            return _value;
        }
    }

    class Subtitle
    {
        public int Index { get; set; }
        private Timestamp _start;
        public Timestamp Start
        { 
            get
            {
                return _start;
            }
            set
            {
                _start = value;
            }
        }
        public Timestamp _end;
        public Timestamp End
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
                Duration = new Timestamp(End.Hours - Start.Hours, End.Minutes - Start.Minutes, End.Seconds - Start.Seconds, End.Milliseconds - Start.Milliseconds);
                Console.WriteLine(Duration);
            }
        }
        public Timestamp Duration { get; set; }
        public string Content { get; set; }
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
                    if (line == String.Empty) // A blank line.
                    {
                        subtitles.Add(curSub);

                        curSub = new Subtitle();
                        continue;
                    }
                    else if (int.TryParse(line, out index)) // A subtitle number.
                        curSub.Index = index;
                    else if (split.Length > 1) // A --> delimited start and end timestamp.
                    {
                        curSub.Start = new Timestamp(split[0]);
                        curSub.End = new Timestamp(split[1]);
                    }
                    else
                    {
                        curSub.Content += line;
                    }
                }
                stream.Close();
            }

            return subtitles.OrderBy(ts => ts.Index).ToList();
        }
    }
}
