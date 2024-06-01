namespace progjog.Pages;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using progjog.Data.Entities;
using progjog.Data;


public class DashboardModel : PageModel
{
    private readonly ILogger<DashboardModel> _logger;
    private readonly ApplicationDbContext _dbContext;

    [BindProperty]
    public TrainingSession? NewTrainingSession { get; set; }

    [BindProperty]
    public List<TrainingSession> TrainingSessions { get; set; } = new();

    [BindProperty]
    public TrainingSession? UpdateTrainingSession { get; set; }


    public DashboardModel(
        ILogger<DashboardModel> logger,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task OnGet()
    {
        TrainingSessions = await _dbContext.TrainingSessions
            .OrderBy(ts => ts.DueDate)
            .ToListAsync();
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

    public async Task<ActionResult> OnPostDeleteAsync(Guid trainingSessionId)
    {
        _logger.LogInformation("Trying to delete training session with id {id}", trainingSessionId);

        if (!ModelState.IsValid)
        {
            _logger.LogInformation("Model is not valid");
            LogModelStateErrors();
            return Page();
        }

        // assign model to a variable
        var trainingSession = await _dbContext.TrainingSessions.FindAsync(trainingSessionId);
        if (trainingSession is null)
        {
            _logger.LogInformation("Training session with id {id} not found", trainingSessionId);
            return Page();
        }
        _dbContext.TrainingSessions.Remove(trainingSession);
        await _dbContext.SaveChangesAsync();

        return RedirectToPage(); 
    }

    public async Task<ActionResult> OnPostUpdateAsync(Guid trainingSessionId)
    {
        _logger.LogInformation("Trying to update training session with id {id}", trainingSessionId);

        if (!ModelState.IsValid)
        {
            _logger.LogInformation("Model is not valid");
            LogModelStateErrors();
            return Page();
        }

        // assign model to a variable
        var trainingSession = await _dbContext.TrainingSessions
            .FirstOrDefaultAsync(ts => ts.TrainingSessionId == trainingSessionId);

        if (trainingSession is null)
        {
            _logger.LogInformation("Training session with id {id} not found", trainingSessionId);
            return Page();
        }

        if (UpdateTrainingSession is null)
        {
            return Page();
        }

        trainingSession.Title = UpdateTrainingSession.Title;
        trainingSession.DueDate = UpdateTrainingSession.DueDate;


        _dbContext.TrainingSessions.Update(trainingSession);
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
