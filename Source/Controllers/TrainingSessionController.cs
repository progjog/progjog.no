using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using progjog.Services;
using progjog.Models;

namespace progjog.Controllers;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TrainingSessionController : ControllerBase
{
    private readonly ITrainingSessionService _trainingSessionService;

    public TrainingSessionController(ITrainingSessionService trainingSessionService)
    {
        _trainingSessionService = trainingSessionService;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAsync(DateTime start, DateTime end)
    {
        var startDateonly = DateOnly.FromDateTime(start);
        var endDateOnly = DateOnly.FromDateTime(end);

        var plannedSessions = await _trainingSessionService
            .GetTrainingSessionsBetweenDatesForUser(Guid.Empty, startDateonly, endDateOnly);

        var events = new List<CalendarEvent>();

        foreach (var session in plannedSessions)
        {
            events.Add(new CalendarEvent
            {
                Id = session.TrainingSessionId,
                Title = session.Title,
                Start = session.DueDate
            });
        }

        return Ok(events);
    }

}
