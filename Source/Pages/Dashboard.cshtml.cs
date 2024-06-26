namespace progjog.Pages;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using progjog.Data.Entities;
using progjog.Data;
using progjog.Services;


# warning TODO: remove usage of applicationDBContext, it shouldn't be used directly in controllers (SOC);

public class DashboardModel : PageModel
{
    private readonly ILogger<DashboardModel> _logger;

    private readonly ApplicationDbContext _dbContext;
    private readonly ITrainingSessionService _trainingSessionService;

    [BindProperty]
    public TrainingSession? NewTrainingSession { get; set; }

    [BindProperty]
    public TrainingSession? UpdateTrainingSession { get; set; }

    [BindProperty]
    public List<TrainingSession> TodaysTrainingSessions { get; set; } = new();


    public DashboardModel(
        ILogger<DashboardModel> logger,
        ITrainingSessionService trainingSessionService,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
        _trainingSessionService = trainingSessionService;
    }

    public async Task<ActionResult> OnGetAsync()
    {
        TodaysTrainingSessions = await _trainingSessionService
            .GetTodaysTrainingSessionsForUser(Guid.Empty);

        return Page();
    }

    public async Task<ActionResult> OnPostSaveAsync()
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
