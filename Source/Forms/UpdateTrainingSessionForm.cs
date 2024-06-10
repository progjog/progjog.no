namespace progjog.Forms;

public class UpdateTrainingSessionForm
{
    public required Guid TrainingSessionId { get; set; }
    public required string Title { get; set; }
    public required DateOnly DueDate { get; set; }
}
