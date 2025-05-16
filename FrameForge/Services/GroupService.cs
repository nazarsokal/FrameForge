using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;

namespace Services;

public class GroupService : IGroupService
{
    private readonly FrameForgeDbContext _dbContext;

    public GroupService(FrameForgeDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Group>> GetGroupByTeacherId(Guid id) => await _dbContext.Groups.Where(g => g.TeacherId == id).ToListAsync();

    public async Task AddGroup(Group group)
    {
        _dbContext.Entry(group.Teacher).State = EntityState.Unchanged;
        _dbContext.Groups.Add(group);
        await _dbContext.SaveChangesAsync();
    }

    public Task UpdateGroup(Group group)
    {
        throw new NotImplementedException();
    }
}