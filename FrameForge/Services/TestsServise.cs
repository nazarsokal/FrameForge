using Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Services;

public class TestsServise
{
    FrameForgeDbContext _context;

    public TestsServise(FrameForgeDbContext context)
    {
        _context = context;
    }
    public async Task<List<Test>> getAllTests(string title = null)
    {
        if (title == null)
        {
            return await _context.Tests.ToListAsync();
        }
        else
        {
            return await _context.Tests.Where(t => t.Title == title).ToListAsync();
        }
    }

    public List<TestQuestionModel> convertFromDto(List<Test> tests)
    {
        var result = new List<TestQuestionModel>();

        foreach (var test in tests)
        {
            // Parse AnswerVariants string into a List<string>
            var answerVariants = new List<string>();
            if (!string.IsNullOrEmpty(test.AnswerVariants))
            {
                // Remove leading/trailing brackets and split by "]["
                var variants = JsonSerializer.Deserialize<List<string>>(test.AnswerVariants);   
                answerVariants.AddRange(variants);
            }

            // Create and add the TestQuestionModel
            result.Add(new TestQuestionModel
            {
                TestID = test.TestID,
                Title = test.Title,
                Question = test.Question,
                AnswerVariants = answerVariants,
                Answer = test.Answer
            });
        }

        return result;
    }
    
}