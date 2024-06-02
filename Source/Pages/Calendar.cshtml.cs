namespace progjog.Pages;

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using progjog.Data.Entities;
using progjog.Data;
using progjog.Services;


#warning TODO: remove usage of applicationDBContext, it shouldn't be used directly in controllers (SOC);
public class CalendarModel : PageModel
{
    private readonly ILogger<CalendarModel> _logger;
    private readonly ApplicationDbContext _dbContext;

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
    {
        // var plannedSessions = await _dbContext.TrainingSessions
          //   .Where(ts => ts.DueDate == DateOnly.FromDateTime(DateTime.UtcNow))
            // .ToListAsync();

        /*
        var calendarEvents = new List<CalendarEvent>();

        foreach (var session in plannedSessions)
        {
            calendarEvents.Add(new CalendarEvent
            {
                Id = session.TrainingSessionId,
                Title = session.Title,
                Start = session.DueDate
            });
        }

        PlannedSessions = JsonSerializer.Serialize(calendarEvents);
        */
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
        var trainingSession = await _dbContext.TrainingSessions.FindAsync(trainingSessionId);
        if (trainingSession is null)
        {
            _logger.LogDebug("Training session not found");
            return Page();
        }

        _logger.LogDebug("Trying to delete training session");
        _dbContext.TrainingSessions.Remove(trainingSession);
        await _dbContext.SaveChangesAsync();
        return RedirectToPage();
    }
}


public class CalendarEvent
{
    public Guid Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("start")]
    public DateOnly Start { get; set; }
}

