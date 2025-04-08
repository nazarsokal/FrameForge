using Entities;
using ServiceContracts;
using ServiceContracts.Enums;

namespace Services;

public class ProgressMapService : IProgressMapService
{
    private readonly FrameForgeDbContext _dbContext;

    public ProgressMapService(FrameForgeDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void EnrolOnLevel(Student studentEnrolled, string levelTopicName)
    {
        var levelEnrolled = new EnrolledLevels() {LevelsEnrolledKey = Guid.NewGuid(), StudentId = studentEnrolled.StudentId, State = States.InProgress.ToString(), LevelTopicName = levelTopicName, MoneyReward = 0.0, StarsReward = 0};
        
        _dbContext.LevelsEnrolled.Add(levelEnrolled);
        _dbContext.SaveChanges();
    }

    public void EnrolOffLevel(Student studentEnrolled, string levelTopicName)
    {
        var levelToEnrollOff = _dbContext.LevelsEnrolled.FirstOrDefault(x => x.StudentId == studentEnrolled.StudentId && x.LevelTopicName == levelTopicName);
        if(levelToEnrollOff == null) throw new NullReferenceException();
        
        _dbContext.LevelsEnrolled.Remove(levelToEnrollOff);
        _dbContext.SaveChanges();
    }

    public void CompleteOnLevel(Student studentEnrolled, string levelTopicName)
    {
        throw new NotImplementedException();
    }

    public List<EnrolledLevels> GetUsersEnrolledLevels(Student studentEnrolled) => _dbContext.LevelsEnrolled.Where(x => x.StudentId == studentEnrolled.StudentId).ToList();
}