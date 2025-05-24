using System.ComponentModel.DataAnnotations;

namespace Entities;

public class BattleRoom
{
    [Key]
    public Guid roomId { get; set; }
    
    public Guid Player1Id { get; set; }
    public Guid? Player2Id { get; set; }
    public string Questions { get; set; }
    
    public int Player1Score { get; set; }
    public int Player2Score { get; set; }

    public BattleStatus Status { get; set; }
    public Guid? WinnerId { get; set; }
}

public enum BattleStatus
{
    WaitingForPlayer,
    InProgress,
    Completed
}