namespace progjog.Models;

using System.Text.Json.Serialization;

public class CalendarEvent
{
    public Guid Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("start")]
    public DateOnly Start { get; set; }
}

