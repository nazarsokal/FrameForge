using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace ServiceContracts;

public interface IBattleService
{
    Task<BattleRoom> CreateRoom(Student player);
    Task<BattleRoom> JoinRoom(Guid roomId, Student player);
    Task<BattleResult> SubmitAnswer(Guid roomId, Guid playerId, string answer, int questionIndex);
    Task<BattleRoom> GetRoomStatus(Guid roomId);
    Task<List<BattleRoom>> GetAvailableRooms();
    Task EndBattle(Guid roomId);
    Task<BattleRoom> LeaveRoom(Guid roomId);
}

public class BattleResult
{
    public bool IsCorrect { get; set; }
    public int CurrentPlayer1Score { get; set; }
    
    public int CurrentPlayer2Score { get; set; }

    public bool IsBattleComplete { get; set; }
    public Guid? WinnerId { get; set; }
}