namespace progjog.Pages;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using progjog.Data.Entities;
using progjog.Data;
using progjog.Models;


#warning TODO: remove usage of applicationDBContext, it shouldn't be used directly in controllers (SOC);
public class CalendarModel : PageModel
{
    private readonly ILogger<CalendarModel> _logger;
    private readonly ApplicationDbContext _dbContext;

    [BindProperty]
    public required TrainingSessionModel TrainingSessionModel { get; set; } 

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
        if (trainingSessionId == Guid.Empty)
        {
            _logger.LogDebug("Training session id is empty");
            return RedirectToPage();
        }

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

    public async Task<ActionResult> OnPostSaveAsync()
    {
#warning TODO: when modelstate is invalid, give feedback to client
        if (!ModelState.IsValid)
        {
            LogModelStateErrors();
            return Page();
        }

        if (TrainingSessionModel is null)
        {
            _logger.LogInformation($"{nameof(TrainingSessionModel)} is null");
            return Page();
        }

        var trainingSession = new TrainingSession
        {
            Title = TrainingSessionModel.Title,
            DueDate = TrainingSessionModel.DueDate
        };

#warning TODO: use a mapper to map the form to the entity
        await _dbContext.TrainingSessions.AddAsync(trainingSession);
        await _dbContext.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<ActionResult> OnPostUpdateAsync(Guid trainingSessionId)
    {
        if (trainingSessionId == Guid.Empty)
        {
            _logger.LogInformation("Training session id is empty");
            return RedirectToPage();
        }

        if (!ModelState.IsValid)
        {
            LogModelStateErrors();
            return RedirectToPage();
        }

        if (TrainingSessionModel is null)
        {
            _logger.LogInformation("TrainingSessionModel is null");
            return RedirectToPage();
        }

        var trainingSession = await _dbContext.TrainingSessions.FindAsync(trainingSessionId);

        if (trainingSession is null)
        {
            _logger.LogInformation("Training session not found");
            return RedirectToPage();
        }

        trainingSession.Title = TrainingSessionModel.Title;
        trainingSession.DueDate = TrainingSessionModel.DueDate;

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

