using Entities;

namespace ServiceContracts;

public interface IMentorService
{
    public Task<List<MentorNpcQuotes>> GetMentorQuotesByLevelNumber(int levelNumber);
    
    public List<MentorNpcQuotes> GroupMentorQuotes(List<MentorNpcQuotes> mentorQuotes);
}