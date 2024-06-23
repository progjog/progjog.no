namespace progjog.Models;

using System.ComponentModel.DataAnnotations;

public class TrainingSessionModel
{
    public Guid TrainingSessionId { get; set; } = Guid.Empty;

    [Required]
    public string Title { get; set; } = String.Empty;

    [Required]
    public DateOnly DueDate { get; set; }
}
