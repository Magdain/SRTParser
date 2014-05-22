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
        public int Hours, Minutes, Seconds;
        private string _value;
        public Timestamp(string value)
        {
            var tokens = value.Split(':');
            if (tokens.Length == 3)
            {
                Hours = int.Parse(tokens[0]);
                Minutes = int.Parse(tokens[1]);
                Seconds = int.Parse(tokens[2].Replace(",", string.Empty));
                _value = value;
            }
            else
            {
                throw new Exception("Timestamp expects three values delimited by a colon.");
            }
        }
        public override string ToString()
        {
            return _value;
        }
    }

    class Subtitle
    {
        public int Index { get; set; }
        public Timestamp Start { get; set; }
        public Timestamp End { get; set; }
        //public float Duration;
        public string Content { get; set; }

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
                        //curSub.Content.TrimEnd('\r', '\n');
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
                        //curSub.Duration =
                    }
                    else
                    {
                        curSub.Content += line;// +"\n";
                    }
                }
                stream.Close();
            }

            return subtitles.OrderBy(ts => ts.Index).ToList();
        }
    }
}
