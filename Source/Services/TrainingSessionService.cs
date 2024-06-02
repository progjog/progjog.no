namespace progjog.Services;

using System.Collections.Generic;
using progjog.Data;
using progjog.Data.Entities;
using Microsoft.EntityFrameworkCore;

public class TrainingSessionService : ITrainingSessionService
{
    private readonly ApplicationDbContext _dbContext;

    public TrainingSessionService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

#warning TODO: trainingsessions should be filtered by user, but userid is not a field on the training session entity;
    public async Task<List<TrainingSession>> GetTodaysTrainingSessionsForUser(Guid userId)
    {
        return await _dbContext.TrainingSessions
            .Where(ts => ts.DueDate == DateOnly.FromDateTime(DateTime.UtcNow))
            .ToListAsync();
    }

#warning TODO: trainingsessions should be filtered by user, but userid is not a field on the training session entity;
    public async Task<List<TrainingSession>> GetTrainingSessionsBetweenDatesForUser(
            Guid userId, 
            DateOnly startDate, 
            DateOnly endDate
    )
    {
        return await _dbContext.TrainingSessions
            .Where(ts => ts.DueDate >= startDate && ts.DueDate <= endDate)
            .ToListAsync();
    }
}

public interface ITrainingSessionService
{
    Task<List<TrainingSession>> GetTodaysTrainingSessionsForUser(Guid userId);
    Task<List<TrainingSession>> GetTrainingSessionsBetweenDatesForUser(Guid userId, DateOnly startDate, DateOnly endDate);
}
