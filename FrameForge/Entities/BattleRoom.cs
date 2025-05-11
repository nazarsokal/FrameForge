using System.ComponentModel.DataAnnotations;

namespace Entities;

public class BattleRoom
{
    [Key]
    public Guid roomId { get; set; }
    
    public Guid Player1Id { get; set; }
    public Guid? Player2Id { get; set; }
    public string Questions { get; set; }
}