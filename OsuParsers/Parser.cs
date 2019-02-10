using OsuParsers.Beatmaps;
using OsuParsers.Database;
using OsuParsers.Decoders;
using OsuParsers.Replays;
using OsuParsers.Storyboards;
using System.IO;
using OsuParsers.Encoders;

namespace OsuParsers
{
    public class Parser
    {
        private static StoryboardDecoder storyboardDecoder = new StoryboardDecoder();
        private static BeatmapDecoder beatmapDecoder = new BeatmapDecoder();
        private static DatabaseDecoder databaseDecoder = new DatabaseDecoder();
        private static ReplayDecoder replayDecoder = new ReplayDecoder();

        private static BeatmapEncoder beatmapEncoder = new BeatmapEncoder();
        private static StoryboardEncoder storyboardEncoder = new StoryboardEncoder();
        private static DatabaseEncoder databaseEncoder = new DatabaseEncoder();
        private static ReplayEncoder replayEncoder = new ReplayEncoder();

        #region Decodes

        /// <summary>
        /// Parses .osu file.
        /// </summary>
        /// <param name="pathToBeatmap">Path to the .osu file.</param>
        /// <returns>A usable beatmap.</returns>
        public static Beatmap ParseBeatmap(string pathToBeatmap) => beatmapDecoder.Decode(File.ReadAllLines(pathToBeatmap));

        /// <summary>
        /// Parses .osb file.
        /// </summary>
        /// <param name="pathToStoryboard">Path to the .osb file.</param>
        /// <returns>A usable storyboard.</returns>
        public static Storyboard ParseStoryboard(string pathToStoryboard) => storyboardDecoder.Decode(File.ReadAllLines(pathToStoryboard));

        /// <summary>
        /// Parses osu!.db file.
        /// </summary>
        /// <param name="pathToOsuDb">Path to the osu!.db file.</param>
        /// <returns>A usable <see cref="OsuDatabase"/>.</returns>
        public static OsuDatabase ParseOsuDatabase(string pathToOsuDb) => databaseDecoder.DecodeOsuDatabase(File.OpenRead(pathToOsuDb));

        /// <summary>
        /// Parses collection.db file.
        /// </summary>
        /// <param name="pathToCollectionDb">Path to the collection.db file.</param>
        /// <returns>A usable <see cref="CollectionDatabase"/>.</returns>
        public static CollectionDatabase ParseCollectionDatabase(string pathToCollectionDb) => databaseDecoder.DecodeCollectionDatabase(File.OpenRead(pathToCollectionDb));

        /// <summary>
        /// Parses scores.db file.
        /// </summary>
        /// <param name="pathToScoresDb">Path to the scores.db file.</param>
        /// <returns>A usable <see cref="ScoresDatabase"/>.</returns>
        public static ScoresDatabase ParseScoresDatabase(string pathToScoresDb) => databaseDecoder.DecodeScoresDatabase(File.OpenRead(pathToScoresDb));

        /// <summary>
        /// Parses presence.db file.
        /// </summary>
        /// <param name="pathToPresenceDb">Path to the presence.db file.</param>
        /// <returns>A usable <see cref="PresenceDatabase"/>.</returns>
        public static PresenceDatabase ParsePresenceDatabase(string pathToPresenceDb) => databaseDecoder.DecodePresenceDatabase(File.OpenRead(pathToPresenceDb));

        /// <summary>
        /// Parses .osr file.
        /// </summary>
        /// <param name="pathToReplay">Path to the .osr file.</param>
        /// <returns>A usable <see cref="Replay"/>.</returns>
        public static Replay ParseReplay(string pathToReplay) => replayDecoder.Decode(File.OpenRead(pathToReplay));

        #endregion

        #region Encodes

        /// <summary>
        /// Save to .osu file.
        /// </summary>
        /// <param name="path">Path to the .osu file.</param>
        /// <param name="beatmap">Beatmap.</param>
        public static void SaveBeatmap(string path, Beatmap beatmap) => File.WriteAllLines(path, beatmapEncoder.Encode(beatmap));

        /// <summary>
        /// Save to .osb file.
        /// </summary>
        /// <param name="path">Path to the .osb file.</param>
        /// <param name="storyboard">Storyboard.</param>
        public static void SaveStoryboard(string path, Storyboard storyboard) => File.WriteAllLines(path, storyboardEncoder.Encode(storyboard));

        /// <summary>
        /// Save to osu!.db file.
        /// </summary>
        /// <param name="path">Path to the osu!.db file.</param>
        /// <param name="osuDatabase">Osu database.</param>
        public static void SaveOsuDatabase(string path, OsuDatabase osuDatabase) => databaseEncoder.EncodeOsuDatabase(path, osuDatabase);

        /// <summary>
        /// Save to collection.db file.
        /// </summary>
        /// <param name="path">Path to the collection.db file.</param>
        /// <param name="collectionDatabase">Collection database.</param>
        public static void SaveCollectionDatabase(string path, CollectionDatabase collectionDatabase) => databaseEncoder.EncodeCollectionDatabase(path, collectionDatabase);

        /// <summary>
        /// Save to scores.db file.
        /// </summary>
        /// <param name="path">Path to the scores.db file.</param>
        /// <param name="scoresDatabase">Scores database.</param>
        public static void SaveScoresDatabase(string path, ScoresDatabase scoresDatabase) => databaseEncoder.EncodeScoresDatabase(path, scoresDatabase);

        /// <summary>
        /// Save to presence.db file.
        /// </summary>
        /// <param name="path">Path to the presence.db file.</param>
        /// <param name="presenceDatabase">Presence database.</param>
        public static void SavePresenceDatabase(string path, PresenceDatabase presenceDatabase) => databaseEncoder.EncodePresenceDatabase(path, presenceDatabase);

        /// <summary>
        /// Save to .osr file.
        /// </summary>
        /// <param name="path">Path to the .osr file.</param>
        /// <param name="replay">Replay.</param>
        public static void SaveReplay(string path, Replay replay) => replayEncoder.Encode(replay, path);

        #endregion
    }
}
