namespace progjog.Pages;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using progjog.Data.Entities;
using progjog.Data;
using progjog.Forms;


#warning TODO: remove usage of applicationDBContext, it shouldn't be used directly in controllers (SOC);
public class CalendarModel : PageModel
{
    private readonly ILogger<CalendarModel> _logger;
    private readonly ApplicationDbContext _dbContext;

    [BindProperty]
    public UpdateTrainingSessionForm? UpdateTrainingSessionForm { get; set; }

    [BindProperty]
    public TrainingSession? NewTrainingSession { get; set; }

    public CalendarModel(
            ILogger<CalendarModel> logger,
            ApplicationDbContext dbContext
    )
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public void OnGet()
    { }

    public async Task<ActionResult> OnPostDeleteAsync(Guid trainingSessionId)
    {
        var trainingSession = await _dbContext.TrainingSessions.FindAsync(trainingSessionId);
        if (trainingSession is null)
        {
            _logger.LogDebug("Training session not found");
            return RedirectToPage();
        }

        _logger.LogDebug("Trying to delete training session");
        _dbContext.TrainingSessions.Remove(trainingSession);
        await _dbContext.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<ActionResult> OnPostSaveAsync(Guid trainingSessionId)
    {
        // Add code here
        if (!ModelState.IsValid)
        {
            _logger.LogDebug("Model is not valid");
            return Page();
        }

        if (NewTrainingSession is null)
        {
            _logger.LogDebug("NewTrainingSession is null");
            return Page();
        }

        _logger.LogDebug("Trying to save new training session");
        await _dbContext.TrainingSessions.AddAsync(NewTrainingSession);
        await _dbContext.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<ActionResult> OnPostUpdateTrainingSessionAsync(Guid trainingSessionId)
    {
        if (!ModelState.IsValid)
        {
            LogModelStateErrors();
            return RedirectToPage();
        }

        if (UpdateTrainingSessionForm is null)
        {
            _logger.LogInformation("UpdateTrainingSessionForm is null");
            return RedirectToPage();
        }

        var trainingSession = await _dbContext.TrainingSessions.FindAsync(trainingSessionId);
        if (trainingSession is null)
        {
            _logger.LogInformation("Training session not found");
            return RedirectToPage();
        }

        trainingSession.Title = UpdateTrainingSessionForm.Title;
        trainingSession.DueDate = UpdateTrainingSessionForm.DueDate;

        _logger.LogInformation("Trying to update training session");
        await _dbContext.SaveChangesAsync();
        return RedirectToPage();
    }

    private void LogModelStateErrors()
    {
        foreach (var state in ModelState)
        {
            var key = state.Key;
            var errors = state.Value.Errors;

            foreach (var error in errors)
            {
                _logger.LogError($"Validation error in {key}: {error.ErrorMessage}");
            }
        }
    }
}

