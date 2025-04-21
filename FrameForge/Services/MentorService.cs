using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;

namespace Services;

public class MentorService : IMentorService
{
    private readonly FrameForgeDbContext _dbContext;

    public MentorService(FrameForgeDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<MentorNpcQuotes>> GetMentorQuotesByLevelNumber(int levelNumber) => await _dbContext.MentorQuotes
        .Where(m => m.LevelNumber == levelNumber)
        .OrderBy(m => m.OrderNumber)
        .ToListAsync();
    

    public List<MentorNpcQuotes> GroupMentorQuotes(List<MentorNpcQuotes> mentorQuotes) => mentorQuotes.OrderBy(n => n.OrderNumber).ToList();
        
    
}