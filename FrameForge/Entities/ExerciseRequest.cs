using System.Text.Json.Serialization;
namespace Entities;

public class ExerciseRequest
{
    [JsonPropertyName("exercise")]
    public Exercise Exercise { get; set; }
    
    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("files")]
    public List<ExerciseFile> Files { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("result")]
    public ExerciseResult Result { get; set; }

    [JsonPropertyName("stdin")]
    public string Stdin { get; set; }

    [JsonPropertyName("_id")]
    public string Id { get; set; }
}

public class ExerciseFile
{
    public string Name { get; set; }
    public string Content { get; set; }
}

public class ExerciseResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
}