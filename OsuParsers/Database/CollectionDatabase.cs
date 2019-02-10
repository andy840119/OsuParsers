﻿using OsuParsers.Database.Objects;
using System.Collections.Generic;
using OsuParsers.Encoders;

namespace OsuParsers.Database
{
    public class CollectionDatabase
    {
        public int OsuVersion { get; set; }
        public int CollectionCount { get; set; }
        public List<Collection> Collections { get; private set; } = new List<Collection>();

        public void Write(string path)
        {
            DatabaseEncoder.WriteCollectionDatabase(path, this);
        }
    }
}
