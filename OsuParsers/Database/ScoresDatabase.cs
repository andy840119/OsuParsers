using OsuParsers.Database.Objects;
using System;
using System.Collections.Generic;
using OsuParsers.Encoders;

namespace OsuParsers.Database
{
    public class ScoresDatabase
    {
        public int OsuVersion { get; set; }
        public List<Tuple<string, List<Score>>> Scores { get; private set; } = new List<Tuple<string, List<Score>>>();

        public void Write(string path)
        {
            DatabaseEncoder.EncodeScoresDatabase(path, this);
        }
    }
}
