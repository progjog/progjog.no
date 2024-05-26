using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using progjog.Data.Entities;
using progjog.Data;

namespace progjog.Pages;

public class MySiteModel : PageModel
{
    private readonly ILogger<MySiteModel> _logger;
    private readonly ApplicationDbContext _dbContext;

    [BindProperty]
    public TrainingSession? NewTrainingSession { get; set; }

    [BindProperty]
    public List<TrainingSession> TrainingSessions { get; set; } = new();


    public MySiteModel(
        ILogger<MySiteModel> logger,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task OnGet()
    {
        TrainingSessions = await _dbContext.TrainingSessions
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ActionResult> OnPostSaveAsync()
    {

        // Add code here
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (NewTrainingSession is null)
        {
            return Page();
        }

        _logger.LogInformation("Saving new training session");
        await _dbContext.TrainingSessions.AddAsync(NewTrainingSession);
        await _dbContext.SaveChangesAsync();

        return Page();
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

        return Page();
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