using Entities;

namespace ServiceContracts;

public interface IBattleService
{
    Task<BattleRoom> CreateRoom(Student player);
    Task<BattleRoom> JoinRoom(Guid roomId, Student player);
    Task<BattleResult> SubmitAnswer(Guid roomId, Guid playerId, string answer, int questionIndex);
    Task<BattleRoom> GetRoomStatus(Guid roomId);
    Task<List<BattleRoom>> GetAvailableRooms();
}

public class BattleResult
{
    public bool IsCorrect { get; set; }
    public int CurrentPlayer1Score { get; set; }
    
    public int CurrentPlayer2Score { get; set; }

    public bool IsBattleComplete { get; set; }
    public Guid? WinnerId { get; set; }
}