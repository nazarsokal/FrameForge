using Entities;

namespace ServiceContracts;

public interface IAlgorithmsService
{
    public Task<List<Algorithm>> GetAlgorithms(List<Algorithm> algorithms);
}