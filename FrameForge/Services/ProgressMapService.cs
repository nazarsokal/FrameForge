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
        var levelEnrolled = new EnrolledLevels() {LevelsEnrolledKey = Guid.NewGuid(), StudentId = studentEnrolled.UserId, State = States.InProgress.ToString(), LevelTopicName = levelTopicName, MoneyReward = 0.0, StarsReward = 0};
        
        _dbContext.LevelsEnrolled.Add(levelEnrolled);
        _dbContext.SaveChanges();
    }

    public void EnrolOffLevel(Student studentEnrolled, string levelTopicName)
    {
        var levelToEnrollOff = _dbContext.LevelsEnrolled.FirstOrDefault(x => x.StudentId == studentEnrolled.UserId && x.LevelTopicName == levelTopicName);
        if(levelToEnrollOff == null) throw new NullReferenceException();
        
        _dbContext.LevelsEnrolled.Remove(levelToEnrollOff);
        _dbContext.SaveChanges();
    }

    public void CompleteOnLevel(Student studentEnrolled, string levelTopicName, int starsRewarded, double moneyRewarded)
    {
        var levelFromDb = _dbContext.LevelsEnrolled.FirstOrDefault(x => x.StudentId == studentEnrolled.UserId && x.LevelTopicName == levelTopicName);
        if(levelFromDb == null) throw new NullReferenceException();
        
        levelFromDb.MoneyReward = moneyRewarded;
        levelFromDb.StarsReward = starsRewarded;
        levelFromDb.State = States.Completed.ToString();
        _dbContext.SaveChanges();
    }

    public List<EnrolledLevels> GetUsersEnrolledLevelsInProgress(Student studentEnrolled) => _dbContext.LevelsEnrolled.Where(x => x.StudentId == studentEnrolled.UserId && x.State == States.InProgress.ToString()).ToList();
    public List<EnrolledLevels> GetUsersEnrolledLevelsCompleted(Student studentEnrolled) => _dbContext.LevelsEnrolled.Where(x => x.StudentId == studentEnrolled.UserId && x.State == States.Completed.ToString()).ToList();
    public async Task SetNextLevel(Student studentEnrolled, string levelTopicName)
    {
        EnrolledLevels levelEnrolled = new EnrolledLevels() {LevelsEnrolledKey = Guid.NewGuid(), LevelTopicName = levelTopicName, 
            StudentId = studentEnrolled.UserId, State = States.InProgress.ToString(), MoneyReward = 0, StarsReward = 0};
        
        _dbContext.LevelsEnrolled.Add(levelEnrolled);
        await _dbContext.SaveChangesAsync();
    }
}