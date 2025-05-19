using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;

namespace Services;

public class AlgorithmsService : IAlgorithmsService
{
    private readonly FrameForgeDbContext _frameForgeDbContext;

    public AlgorithmsService(FrameForgeDbContext frameForgeDbContext)
    {
        _frameForgeDbContext = frameForgeDbContext;
    }
    
    public async Task<List<Algorithm>> GetAlgorithms(List<Algorithm> algorithms)
    {
        var algorithmsFromDb = await _frameForgeDbContext.Algorithms.ToListAsync();
        var finalAlgorithmsList = new List<Algorithm>();
        
        foreach (var algorithm in algorithmsFromDb)
        {
            var algToEdit = algorithms.FirstOrDefault(a => a.AlgorithmName == algorithm.AlgorithmName);     
            algToEdit.AlgorithmName = algToEdit?.AlgorithmName;
            algToEdit.Description = algorithm.Description;
            algToEdit.Image = algorithm.Image;
            algToEdit.Id = algorithm.Id;
            
            finalAlgorithmsList.Add(algToEdit);
        }
        
        return finalAlgorithmsList;
    }
}