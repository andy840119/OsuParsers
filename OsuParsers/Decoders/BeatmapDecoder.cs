﻿using OsuParsers.Beatmaps;
using OsuParsers.Beatmaps.Objects;
using OsuParsers.Beatmaps.Objects.Catch;
using OsuParsers.Beatmaps.Objects.Mania;
using OsuParsers.Beatmaps.Objects.Taiko;
using OsuParsers.Beatmaps.Sections.Events;
using OsuParsers.Enums;
using OsuParsers.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OsuParsers.Decoders
{
    public class BeatmapDecoder
    {
        private Beatmap Beatmap;
        private Sections currentSection = Sections.None;
        private List<string> sbLines = new List<string>();

        public Beatmap Decode(string[] lines)
        {
            Beatmap = new Beatmap();
            currentSection = Sections.Format;
            sbLines.Clear();

            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("//"))
                {
                    if (ParseHelper.GetCurrentSection(line) != Sections.None)
                        currentSection = ParseHelper.GetCurrentSection(line);
                    else
                        ParseLine(line);
                }
            }

            Beatmap.EventsSection.Storyboard = new StoryboardDecoder().Decode(sbLines.ToArray());

            Beatmap.GeneralSection.CirclesCount = Beatmap.HitObjects.Count(c => c is Circle || c is TaikoHit || c is ManiaHit || c is CatchFruit);
            Beatmap.GeneralSection.SlidersCount = Beatmap.HitObjects.Count(c => c is Slider || c is TaikoDrumroll || c is ManiaHold || c is CatchDroplets);
            Beatmap.GeneralSection.SpinnersCount = Beatmap.HitObjects.Count(c => c is Spinner || c is TaikoSpinner || c is CatchSpinner);

            Beatmap.GeneralSection.Length = Beatmap.HitObjects.Last().EndTime / 1000;

            return Beatmap;
        }

        private void ParseLine(string line)
        {
            switch (currentSection)
            {
                case Sections.Format:
                    Beatmap.Version = Convert.ToInt32(line.Split(new string[] { "osu file format v" }, StringSplitOptions.None)[1]);
                    break;
                case Sections.General:
                    ParseGeneral(line);
                    break;
                case Sections.Editor:
                    ParseEditor(line);
                    break;
                case Sections.Metadata:
                    ParseMetadata(line);
                    break;
                case Sections.Difficulty:
                    ParseDifficulty(line);
                    break;
                case Sections.Events:
                    ParseEvents(line);
                    break;
                case Sections.TimingPoints:
                    ParseTimingPoints(line);
                    break;
                case Sections.Colours:
                    ParseColours(line);
                    break;
                case Sections.HitObjects:
                    ParseHitObjects(line);
                    break;
            }
        }

        private void ParseGeneral(string line)
        {
            string[] tokens = line.Split(':');
            switch (tokens[0])
            {
                case "AudioFilename":
                    Beatmap.GeneralSection.AudioFilename = tokens[1].Trim();
                    break;
                case "AudioLeadIn":
                    Beatmap.GeneralSection.AudioLeadIn = Convert.ToInt32(tokens[1].Trim());
                    break;
                case "PreviewTime":
                    Beatmap.GeneralSection.PreviewTime = Convert.ToInt32(tokens[1].Trim());
                    break;
                case "Countdown":
                    Beatmap.GeneralSection.Countdown = ParseHelper.ToBool(tokens[1].Trim());
                    break;
                case "SampleSet":
                    Beatmap.GeneralSection.SampleSet = (SampleSet)Enum.Parse(typeof(SampleSet), tokens[1].Trim());
                    break;
                case "StackLeniency":
                    Beatmap.GeneralSection.StackLeniency = ParseHelper.ToDouble(tokens[1].Trim());
                    break;
                case "Mode":
                    Beatmap.GeneralSection.Mode = (Ruleset)Enum.Parse(typeof(Ruleset), tokens[1].Trim());
                    Beatmap.GeneralSection.ModeId = Convert.ToInt32(tokens[1].Trim());
                    break;
                case "LetterboxInBreaks":
                    Beatmap.GeneralSection.LetterboxInBreaks = ParseHelper.ToBool(tokens[1].Trim());
                    break;
                case "WidescreenStoryboard":
                    Beatmap.GeneralSection.WidescreenStoryboard = ParseHelper.ToBool(tokens[1].Trim());
                    break;
                case "StoryFireInFront":
                    Beatmap.GeneralSection.StoryFireInFront = ParseHelper.ToBool(tokens[1].Trim());
                    break;
                case "SpecialStyle":
                    Beatmap.GeneralSection.SpecialStyle = ParseHelper.ToBool(tokens[1].Trim());
                    break;
                case "EpilepsyWarning":
                    Beatmap.GeneralSection.EpilepsyWarning = ParseHelper.ToBool(tokens[1].Trim());
                    break;
                case "UseSkinSprites":
                    Beatmap.GeneralSection.UseSkinSprites = ParseHelper.ToBool(tokens[1].Trim());
                    break;
            }
        }

        private void ParseEditor(string line)
        {
            string[] tokens = line.Split(':');
            switch (tokens[0])
            {
                case "Bookmarks":
                    Beatmap.EditorSection.Bookmarks = tokens[1].Trim().Split(',').Select(b => Convert.ToInt32(b)).ToArray();
                    break;
                case "DistanceSpacing":
                    Beatmap.EditorSection.DistanceSpacing = ParseHelper.ToDouble(tokens[1].Trim());
                    break;
                case "BeatDivisor":
                    Beatmap.EditorSection.BeatDivisor = Convert.ToInt32(tokens[1].Trim());
                    break;
                case "GridSize":
                    Beatmap.EditorSection.GridSize = Convert.ToInt32(tokens[1].Trim());
                    break;
                case "TimelineZoom":
                    Beatmap.EditorSection.TimelineZoom = ParseHelper.ToFloat(tokens[1].Trim());
                    break;
            }
        }

        private void ParseMetadata(string line)
        {
            string[] tokens = line.Split(':');
            switch (tokens[0])
            {
                case "Title":
                    Beatmap.MetadataSection.Title = tokens[1].Trim();
                    break;
                case "TitleUnicode":
                    Beatmap.MetadataSection.TitleUnicode = tokens[1].Trim();
                    break;
                case "Artist":
                    Beatmap.MetadataSection.Artist = tokens[1].Trim();
                    break;
                case "ArtistUnicode":
                    Beatmap.MetadataSection.ArtistUnicode = tokens[1].Trim();
                    break;
                case "Creator":
                    Beatmap.MetadataSection.Creator = tokens[1].Trim();
                    break;
                case "Version":
                    Beatmap.MetadataSection.Version = tokens[1].Trim();
                    break;
                case "Source":
                    Beatmap.MetadataSection.Source = tokens[1].Trim();
                    break;
                case "Tags":
                    Beatmap.MetadataSection.Tags = tokens[1].Trim().Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    break;
                case "BeatmapID":
                    Beatmap.MetadataSection.BeatmapID = Convert.ToInt32(tokens[1].Trim());
                    break;
                case "BeatmapSetID":
                    Beatmap.MetadataSection.BeatmapSetID = Convert.ToInt32(tokens[1].Trim());
                    break;
            }
        }

        private void ParseDifficulty(string line)
        {
            string[] tokens = line.Split(':');
            switch (tokens[0])
            {
                case "HPDrainRate":
                    Beatmap.DifficultySection.HPDrainRate = ParseHelper.ToFloat(tokens[1].Trim());
                    break;
                case "CircleSize":
                    Beatmap.DifficultySection.CircleSize = ParseHelper.ToFloat(tokens[1].Trim());
                    break;
                case "OverallDifficulty":
                    Beatmap.DifficultySection.OverallDifficulty = ParseHelper.ToFloat(tokens[1].Trim());
                    break;
                case "ApproachRate":
                    Beatmap.DifficultySection.ApproachRate = ParseHelper.ToFloat(tokens[1].Trim());
                    break;
                case "SliderMultiplier":
                    Beatmap.DifficultySection.SliderMultiplier = ParseHelper.ToDouble(tokens[1].Trim());
                    break;
                case "SliderTickRate":
                    Beatmap.DifficultySection.SliderTickRate = ParseHelper.ToDouble(tokens[1].Trim());
                    break;
            }
        }

        private void ParseEvents(string line)
        {
            string[] tokens = line.Split(',');

            EventType eventType = default(EventType);

            if (Enum.TryParse(tokens[0], out EventType e))
                eventType = (EventType)Enum.Parse(typeof(EventType), tokens[0]);
            else if (line.StartsWith(" ") || line.StartsWith("_"))
                eventType = EventType.StoryboardCommand;
            else
                return;

            switch (eventType)
            {
                case EventType.Background:
                    Beatmap.EventsSection.BackgroundImage = tokens[2].Trim('"');
                    break;
                case EventType.Video:
                    Beatmap.EventsSection.Video = tokens[2].Trim('"');
                    Beatmap.EventsSection.VideoOffset = Convert.ToInt32(tokens[1]);
                    break;
                case EventType.Break:
                    Beatmap.EventsSection.Breaks.Add(new BreakEvent(Convert.ToInt32(tokens[1]), Convert.ToInt32(tokens[2])));
                    break;
                case EventType.Sprite:
                case EventType.Animation:
                case EventType.Sample:
                case EventType.StoryboardCommand:
                    sbLines.Add(line);
                    break;
            }
        }

        private void ParseTimingPoints(string line)
        {
            string[] tokens = line.Split(',');

            int offset = (int)ParseHelper.ToFloat(tokens[0]);
            double beatLength = ParseHelper.ToDouble(tokens[1]);
            TimeSignature timeSignature = TimeSignature.SimpleQuadruple;
            SampleSet sampleSet = SampleSet.None;
            int customSampleSet = 0;
            int volume = 100;
            bool inherited = true;
            bool kiaiMode = false;

            if (tokens.Length >= 3)
                timeSignature = (TimeSignature)Convert.ToInt32(tokens[2]);

            if (tokens.Length >= 4)
                sampleSet = (SampleSet)Convert.ToInt32(tokens[3]);

            if (tokens.Length >= 5)
                customSampleSet = Convert.ToInt32(tokens[4]);

            if (tokens.Length >= 6)
                volume = Convert.ToInt32(tokens[5]);

            if (tokens.Length >= 7)
                inherited = ParseHelper.ToBool(tokens[6]);

            if (tokens.Length >= 8)
                kiaiMode = ParseHelper.ToBool(tokens[7]);

            Beatmap.TimingPoints.Add(new TimingPoint
            {
                Offset = offset,
                BeatLength = beatLength,
                TimeSignature = timeSignature,
                SampleSet = sampleSet,
                CustomSampleSet = customSampleSet,
                Volume = volume,
                Inherited = inherited,
                KiaiMode = kiaiMode,
            });
        }

        private void ParseColours(string line)
        {
            string[] tokens = line.Split(':');
            int[] rgb = tokens[1].Trim().Split(',').Select(c => Convert.ToInt32(c)).ToArray();
            Beatmap.Colours.Add(Color.FromArgb(rgb.Length == 4 ? rgb[3] : 255, rgb[0], rgb[1], rgb[2]));
        }
        
        private void ParseHitObjects(string line)
        {
            string[] tokens = line.Split(',');

            Point position = new Point(Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]));

            int startTime = Convert.ToInt32(tokens[2]);

            HitObjectType type = (HitObjectType)int.Parse(tokens[3]);

            int comboOffset = (int)(type & HitObjectType.ComboOffset) >> 4;
            type &= ~HitObjectType.ComboOffset;

            bool isNewCombo = type.HasFlag(HitObjectType.NewCombo);
            type &= ~HitObjectType.NewCombo;

            HitSoundType hitSound = (HitSoundType)Convert.ToInt32(tokens[4]);

            HitObject hitObject = null;

            string[] extrasSplit = tokens.Last().Split(':');
            int extrasOffset = type.HasFlag(HitObjectType.Hold) ? 1 : 0;
            Extras extras = tokens.Last().Contains(":") ? new Extras
            {
                SampleSet = (SampleSet)Convert.ToInt32(extrasSplit[0 + extrasOffset]),
                AdditionSet = (SampleSet)Convert.ToInt32(extrasSplit[1 + extrasOffset]),
                CustomIndex = Convert.ToInt32(extrasSplit[2 + extrasOffset]),
                Volume = Convert.ToInt32(extrasSplit[3 + extrasOffset]),
                SampleFileName = extrasSplit[4 + extrasOffset]
            } : new Extras();

            switch (type)
            {
                case HitObjectType.Circle:
                {
                    if (Beatmap.GeneralSection.Mode == Ruleset.Standard)
                        hitObject = new Circle(position, startTime, startTime, hitSound, extras, isNewCombo, comboOffset);
                    else if (Beatmap.GeneralSection.Mode == Ruleset.Taiko)
                        hitObject = new TaikoHit(position, startTime, startTime, hitSound, extras, isNewCombo, comboOffset);
                    else if (Beatmap.GeneralSection.Mode == Ruleset.Fruits)
                        hitObject = new CatchFruit(position, startTime, startTime, hitSound, extras, isNewCombo, comboOffset);
                    else if (Beatmap.GeneralSection.Mode == Ruleset.Mania)
                        hitObject = new ManiaHit(position, startTime, startTime, hitSound, extras, isNewCombo, comboOffset);
                }
                    break;
                case HitObjectType.Slider:
                {
                    CurveType curveType = ParseHelper.GetCurveType(tokens[5].Split('|')[0][0]);
                    List<Point> sliderPoints = ParseHelper.GetSliderPoints(tokens[5].Split('|'));

                    int repeats = Convert.ToInt32(tokens[6]);
                    double pixelLength = ParseHelper.ToDouble(tokens[7]);

                    int endTime = CalculateEndTime(startTime, repeats, pixelLength);

                    List<HitSoundType> edgeHitSounds = null;
                    if (tokens.Length > 8 && tokens[8].Length > 0)
                    {
                        edgeHitSounds = new List<HitSoundType>();
                        edgeHitSounds = Array.ConvertAll(tokens[8].Split('|'), s => (HitSoundType)Convert.ToInt32(s)).ToList();
                    }

                    List<Tuple<SampleSet, SampleSet>> edgeAdditions = null;
                    if (tokens.Length > 9 && tokens[9].Length > 0)
                    {
                        edgeAdditions = new List<Tuple<SampleSet, SampleSet>>();
                        foreach (var s in tokens[9].Split('|'))
                        {
                            edgeAdditions.Add(new Tuple<SampleSet, SampleSet>((SampleSet)Convert.ToInt32(s.Split(':').First()), (SampleSet)Convert.ToInt32(s.Split(':').Last())));
                        }
                    }

                    if (Beatmap.GeneralSection.Mode == Ruleset.Standard)
                        hitObject = new Slider(position, startTime, endTime, hitSound, curveType, sliderPoints, repeats,
                            pixelLength, edgeHitSounds, edgeAdditions, extras, isNewCombo, comboOffset);
                    else if (Beatmap.GeneralSection.Mode == Ruleset.Taiko)
                        hitObject = new TaikoDrumroll(position, startTime, endTime, hitSound, curveType, sliderPoints,
                            repeats, pixelLength, edgeHitSounds, edgeAdditions, extras, isNewCombo, comboOffset);
                    else if (Beatmap.GeneralSection.Mode == Ruleset.Fruits)
                        hitObject = new CatchDroplets(position, startTime, endTime, hitSound, curveType, sliderPoints,
                            repeats, pixelLength, edgeHitSounds, edgeAdditions, extras, isNewCombo, comboOffset);
                }
                    break;
                case HitObjectType.Spinner:
                {
                    int endTime = Convert.ToInt32(tokens[5].Trim());

                    if (Beatmap.GeneralSection.Mode == Ruleset.Standard)
                        hitObject = new Spinner(position, startTime, endTime, hitSound, extras, isNewCombo, comboOffset);
                    else if (Beatmap.GeneralSection.Mode == Ruleset.Taiko)
                        hitObject = new TaikoSpinner(position, startTime, endTime, hitSound, extras, isNewCombo, comboOffset);
                    else if (Beatmap.GeneralSection.Mode == Ruleset.Fruits)
                        hitObject = new CatchSpinner(position, startTime, endTime, hitSound, extras, isNewCombo, comboOffset);
                }
                    break;
                case HitObjectType.Hold:
                {
                    string[] additions = tokens[5].Split(':');
                    int endTime = Convert.ToInt32(additions[0].Trim());
                    hitObject = new ManiaHold(position, startTime, endTime, hitSound, extras, isNewCombo, comboOffset);
                }
                    break;
            }

            Beatmap.HitObjects.Add(hitObject);
        }

        private int CalculateEndTime(int startTime, int repeats, double pixelLength)
        {
            int duration = (int)(pixelLength / (100.0 * Beatmap.DifficultySection.SliderMultiplier) * BeatLengthAt(startTime));

            return startTime + duration;
        }

        private double BeatLengthAt(int offset)
        {
            if (Beatmap.TimingPoints.Count == 0)
                return 0;

            int timingPoint = 0;
            int samplePoint = 0;

            for (int i = 0; i < Beatmap.TimingPoints.Count; i++)
            {
                if (Beatmap.TimingPoints[i].Offset <= offset)
                {
                    if (Beatmap.TimingPoints[i].Inherited)
                        timingPoint = i;
                    else
                        samplePoint = i;
                }
            }

            double multiplier = 1;

            if (samplePoint > timingPoint && Beatmap.TimingPoints[samplePoint].BeatLength < 0)
                multiplier = MathHelper.CalculateBpmMultiplier(Beatmap.TimingPoints[samplePoint]);

            return Beatmap.TimingPoints[timingPoint].BeatLength * multiplier;
        }

        #region Old slider end time algorithm

        //kept it just in case if the new one will work worse
        private int OldSliderEndTimeAlgorithm(int startTime, int repeats, double pixelLength)
        {
            var timingPoint = GetTimingPointFromOffset(startTime);
            var parentTimingPoint = timingPoint;
            double velocity = 1;

            if (timingPoint.BeatLength < 0)
            {
                velocity = Math.Abs(100 / timingPoint.BeatLength);
                parentTimingPoint = GetParentTimingPoint(timingPoint);
            }

            double pixelsPerBeat = Beatmap.DifficultySection.SliderMultiplier * 100 * velocity;
            double beats = pixelLength * repeats / pixelsPerBeat;
            int duration = (int)Math.Ceiling(beats * parentTimingPoint.BeatLength);

            return startTime + duration;
        }

        private TimingPoint GetTimingPointFromOffset(int offset)
        {
            if (Beatmap.TimingPoints.Count == 0)
                return null;

            if (offset < Beatmap.TimingPoints.First().Offset)
                return Beatmap.TimingPoints.First();

            for (int i = Beatmap.TimingPoints.Count - 1; i >= 0; i--)
            {
                if (Beatmap.TimingPoints[i].Offset <= offset)
                    return Beatmap.TimingPoints[i];
            }

            return null;
        }

        private TimingPoint GetParentTimingPoint(TimingPoint child)
        {
            if (Beatmap.TimingPoints.Count == 0)
                return null;

            for (int i = Beatmap.TimingPoints.IndexOf(child) - 1; i >= 0; i--)
            {
                if (Beatmap.TimingPoints[i].BeatLength > 0)
                    return Beatmap.TimingPoints[i];
            }

            return null;
        }

        #endregion
    }
}
