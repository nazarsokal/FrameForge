using Entities;

namespace ServiceContracts;

public interface IGroupService
{
    public Task<List<Group>> GetGroupByTeacherId(Guid id);
    
    public Task AddGroup(Group group);
    
    public Task<Group> GetGroupByStudentId(Guid id);
    
    public Task AssignStudent(Guid groupId, Guid studentId);
    
    public Task UpdateGroup(Group group);
    
    public Task<List<Group>> GetGroups();
}