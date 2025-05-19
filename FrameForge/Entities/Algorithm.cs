using System.ComponentModel.DataAnnotations;

namespace Entities;

public class Algorithm
{
    [Key] public Guid Id { get; set; }
    public required string AlgorithmName { get; set; }
    public byte[]? Image { get; set; }
    public string? Description { get; set; }
    public string? AlgorithmHtml { get; set; }
    public string? AlgorithmCss { get; set; }
    public string? AlgorithmJs { get; set; }
}