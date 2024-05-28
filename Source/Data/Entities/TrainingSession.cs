namespace progjog.Data.Entities;

public class TrainingSession
{
    public Guid TrainingSessionId { get; set; }
    public string Title { get; set; } = String.Empty;
    public DateOnly DueDate { get; set; }
    public bool Done { get; set; }
}
