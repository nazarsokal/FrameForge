using System.ComponentModel.DataAnnotations;

namespace Entities;

public class BattleHistory
{
    [Key]
    public Guid Id { get; set; }
    public Guid Player1Id { get; set; }
    public Guid Player2Id { get; set; }
    public DateTime EndTime {get; set;}

    public Guid WinnerId { get; set; }
    
}