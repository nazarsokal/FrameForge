using Entities;

namespace ServiceContracts;

public interface IExerciseService
{
    public Task AddExercise(Exercise exercise);
    
    public Task<List<Exercise>> GetExercises(Guid groupId);
    
    public Task<Exercise> GetExercise(Guid exerciseId);
    
    public Task SubmitExercise(ExerciseSubmission exerciseSubmission);
}