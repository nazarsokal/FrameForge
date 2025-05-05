using Entities;

namespace ServiceContracts;

public interface IProgressMapService
{
    public void EnrolOnLevel(Student studentEnrolled, string levelTopicName);
    public void EnrolOffLevel(Student studentEnrolled, string levelTopicName);
    public void CompleteOnLevel(Student studentEnrolled, string levelTopicName, int starsAwarded, double moneyAwarded);
    public List<EnrolledLevels> GetUsersEnrolledLevelsInProgress(Student studentEnrolled);
    public List<EnrolledLevels> GetUsersEnrolledLevelsCompleted(Student studentEnrolled);
    public Task SetNextLevel(Student studentEnrolled, string levelTopicName);
}