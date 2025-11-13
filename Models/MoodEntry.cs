namespace MoodTunes.Models;

public enum MoodCategory
{
    Happy,
    Sad,
    Energetic,
    Calm,
    Angry,
    Romantic,
    Anxious,
    Creative
}

public class MoodEntry
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public MoodCategory Mood { get; set; }
    public int Intensity { get; set; }
    public string? Notes { get; set; }
    public List<string> SuggestedGenres { get; set; } = new();
    public List<string> Tags { get; set; } = new(); // User-defined tags
}
