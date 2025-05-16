using Entities;

namespace ServiceContracts;

public interface IGroupService
{
    public Task<List<Group>> GetGroupByTeacherId(Guid id);
    
    public Task AddGroup(Group group);
    
    public Task UpdateGroup(Group group);
}