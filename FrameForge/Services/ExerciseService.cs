using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;

namespace Services;

public class ExerciseService : IExerciseService
{
    private readonly FrameForgeDbContext _context;

    public ExerciseService(FrameForgeDbContext context)
    {
        _context = context;
    }
    public async Task AddExercise(Exercise exercise)
    {
        if (!(await CheckIfExerciseExists(exercise.GroupId, exercise.ExerciseName)))
        {
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Exercise>?> GetExercises(Guid? groupId)
    {
        var l = await _context.Exercises.Where(x => x.GroupId == groupId).ToListAsync();
        
        return l;
    }

    public async Task<Exercise> GetExercise(Guid exerciseId)
    {
        return await _context.Exercises.FirstOrDefaultAsync(x => x.ExerciseId == exerciseId);
    }

    public Task<Exercise> Receive(Guid exerciseId)
    {
        return _context.Exercises.FirstOrDefaultAsync(x => x.ExerciseId == exerciseId);
    }

    public Task SubmitExercise(ExerciseSubmission exerciseSubmission)
    {
        throw new NotImplementedException();
    }

    private async Task<bool> CheckIfExerciseExists(Guid groupId, string exerciseName)
    {
        return await _context.Exercises.AnyAsync(e => e.GroupId == groupId && e.ExerciseName == exerciseName);
    }
}