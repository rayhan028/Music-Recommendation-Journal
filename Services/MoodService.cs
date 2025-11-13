using MoodTunes.Models;

namespace MoodTunes.Services;

public class MoodService
{
    private readonly List<MoodEntry> _entries = new();
    private int _nextId = 1;

    private static readonly Dictionary<MoodCategory, List<string>> MoodGenreMap = new()
    {
        { MoodCategory.Happy, new() { "Pop", "Dance", "Reggae", "Funk" } },
        { MoodCategory.Sad, new() { "Blues", "Classical", "Indie Folk", "Ambient" } },
        { MoodCategory.Energetic, new() { "Rock", "Electronic", "Hip Hop", "Metal" } },
        { MoodCategory.Calm, new() { "Jazz", "Lo-fi", "Acoustic", "New Age" } },
        { MoodCategory.Angry, new() { "Punk", "Heavy Metal", "Industrial", "Hard Rock" } },
        { MoodCategory.Romantic, new() { "R&B", "Soul", "Bossa Nova", "Soft Rock" } },
        { MoodCategory.Anxious, new() { "Downtempo", "Chillwave", "Meditation", "Classical Piano" } },
        { MoodCategory.Creative, new() { "Progressive Rock", "Experimental", "Art Pop", "Jazz Fusion" } }
    };

    public List<MoodEntry> GetAllEntries() => _entries.OrderByDescending(e => e.Date).ToList();

    public void AddEntry(MoodEntry entry)
    {
        entry.Id = _nextId++;
        entry.Date = DateTime.Now;

        if (MoodGenreMap.ContainsKey(entry.Mood))
        {
            entry.SuggestedGenres = MoodGenreMap[entry.Mood].Take(3).ToList();
        }

        _entries.Add(entry);
    }

    public void DeleteEntry(int id) => _entries.RemoveAll(e => e.Id == id);

    public List<MoodEntry> FilterByMood(MoodCategory mood) =>
        _entries.Where(e => e.Mood == mood).OrderByDescending(e => e.Date).ToList();

    public List<MoodEntry> FilterByDateRange(DateTime from, DateTime to) =>
        _entries.Where(e => e.Date >= from && e.Date <= to)
                .OrderByDescending(e => e.Date)
                .ToList();

    public List<MoodEntry> SearchNotes(string keyword) =>
        _entries.Where(e => !string.IsNullOrEmpty(e.Notes) && e.Notes.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(e => e.Date)
                .ToList();

    public MoodStats GetStats()
    {
        if (!_entries.Any())
            return new MoodStats();

        var moodFreq = _entries.GroupBy(e => e.Mood)
                               .ToDictionary(g => g.Key.ToString(), g => g.Count());

        var topGenres = _entries.SelectMany(e => e.SuggestedGenres)
                                .GroupBy(g => g)
                                .OrderByDescending(g => g.Count())
                                .Take(5)
                                .Select(g => g.Key)
                                .ToList();

        var intensityTrend = _entries
            .GroupBy(e => e.Date.Date)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => (int)Math.Round(g.Average(e => e.Intensity)));

        return new MoodStats
        {
            MostCommonMood = moodFreq.OrderByDescending(kvp => kvp.Value).First().Key,
            AverageIntensity = _entries.Average(e => e.Intensity),
            MoodFrequency = moodFreq,
            TopGenres = topGenres,
            IntensityTrend = intensityTrend
        };
    }
}
