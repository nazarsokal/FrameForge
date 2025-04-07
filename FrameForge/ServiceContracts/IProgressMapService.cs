using Entities;

namespace ServiceContracts;

public interface IProgressMapService
{
    public void EnrolOnLevel(Student studentEnrolled, string levelTopicName);
    public void EnrolOffLevel(Student studentEnrolled, string levelTopicName);
    public void CompleteOnLevel(Student studentEnrolled, string levelTopicName);
    public List<LevelsEnrolled> GetUsersEnrolledLevels(Student studentEnrolled);
}