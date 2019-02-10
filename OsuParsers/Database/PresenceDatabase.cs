using OsuParsers.Database.Objects;
using System.Collections.Generic;
using OsuParsers.Encoders;

namespace OsuParsers.Database
{
    public class PresenceDatabase
    {
        public int OsuVersion { get; set; }
        public List<Player> Players { get; private set; } = new List<Player>();

        public void Write(string path)
        {
            DatabaseEncoder.WritePresenceDatabase(path, this);
        }
    }
}
