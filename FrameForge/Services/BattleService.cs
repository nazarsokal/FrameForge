using Entities;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Microsoft.AspNetCore.SignalR;
using Services.Hubs;

namespace Services;

public class BattleService : IBattleService
{
    private readonly FrameForgeDbContext _context;
    private readonly IHubContext<BattleHub> _hubContext;
    private readonly Random _random;

    public BattleService(
        FrameForgeDbContext context,
        IHubContext<BattleHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
        _random = new Random();
    }

    public string GetRandom20Tests()
    {
        var tests = _context.Tests.ToList();
        if (tests == null)
            throw new ArgumentNullException(nameof(tests));

        if (20 > tests.Count)
            throw new ArgumentException("Count cannot be greater than the number of available units.");

        var RandTests = tests
            .OrderBy(x => _random.Next())
            .Take(20)
            .ToList();
        return JsonSerializer.Serialize(RandTests);
    }

    public async Task<BattleRoom> CreateRoom(Student player)
    {
        var room = new BattleRoom
        {
            roomId = Guid.NewGuid(),
            Player1Id = player.UserId,
            Questions = GetRandom20Tests(),
            Status = BattleStatus.WaitingForPlayer,
            Player1Score = 0,
            Player2Score = 0
        };

        _context.BattleRooms.Add(room);
        await _context.SaveChangesAsync();

        await _hubContext.Clients.All.SendAsync("NewRoomAvailable", room);
        return room;
    }

    public async Task<BattleRoom> JoinRoom(Guid roomId, Student player)
    {
        var room = await _context.BattleRooms.FindAsync(roomId);
        if (room == null || room.Status != BattleStatus.WaitingForPlayer)
            throw new InvalidOperationException("Room is not available");

        room.Player2Id = player.UserId;
        room.Status = BattleStatus.InProgress;
        await _context.SaveChangesAsync();

        return room;
    }

    public async Task<BattleResult> SubmitAnswer(Guid roomId, Guid playerId, string answer, int questionIndex)
    {
        var room = await _context.BattleRooms.FindAsync(roomId);
        if (room == null || room.Status != BattleStatus.InProgress)
            throw new InvalidOperationException("Invalid battle room");

        var questions = JsonSerializer.Deserialize<List<Test>>(room.Questions);
        var currentQuestion = questions[questionIndex];
        bool isCorrect = answer == currentQuestion.Answer;
        if (isCorrect)
        {
            if (playerId == room.Player1Id)
                room.Player1Score++;
            else
                room.Player2Score++;
        }

        bool isBattleComplete = (room.Player1Score + room.Player2Score) >= questions.Count;
        if (isBattleComplete)
        {
            room.Status = BattleStatus.Completed;
            room.WinnerId = room.Player1Score > room.Player2Score ? room.Player1Id : room.Player2Id;
        }

        await _context.SaveChangesAsync();

        var result = new BattleResult
        {
            IsCorrect = isCorrect,
            CurrentPlayer1Score =  room.Player1Score,
            CurrentPlayer2Score =  room.Player2Score,
            IsBattleComplete = isBattleComplete,
            WinnerId = room.WinnerId
        };

        await _hubContext.Clients.Group(roomId.ToString()).SendAsync("AnswerProcessed", result);
        return result;
    }

    public async Task<BattleRoom> GetRoomStatus(Guid roomId)
    {
        return await _context.BattleRooms.FindAsync(roomId);
    }

    public async Task<List<BattleRoom>> GetAvailableRooms()
    {
        return await _context.BattleRooms
            .Where(r => r.Status == BattleStatus.WaitingForPlayer)
            .ToListAsync();
    }

    public async Task<bool> AreRoomsToEnterExsist()
    {
        List<BattleRoom> rooms = await _context.BattleRooms.ToListAsync();
        foreach (BattleRoom room in rooms)
        {
            if (room.Player2Id == null)
            {
                return true;
            }
        }
        return false;
    }
    

    public async Task<BattleRoom> EnterTheRoom(Student curStudent)
    {
        var allRooms = await _context.BattleRooms.ToListAsync();
        if (allRooms!=null)
        {
            foreach (var room in allRooms)
            {
                if (room.Player2Id == null)
                {
                    room.Player2Id = curStudent.UserId;
                    await _context.SaveChangesAsync();                    
                    return room;
                }
                else
                {
                    return null;
                }
            }
        }
        
        return null;
    }

    public async Task EndBattle(Guid roomId)
    {
        var room = await _context.BattleRooms.FirstOrDefaultAsync(r => r.roomId == roomId);

        if (room == null)
            throw new Exception("Room not found");
        room.Status = BattleStatus.Completed;

        // Створюємо запис в історії
        var battleHistory = new BattleHistory
        {
            Player1Id = (Guid)room.Player1Id,
            Player2Id = (Guid)room.Player2Id,
            EndTime = DateTime.UtcNow,
            WinnerId = (Guid)(room.Player1Score > room.Player2Score ? room.Player1Id :
                room.Player2Score > room.Player1Score ? room.Player2Id : null)
        };
        

        await _context.BattleHistory.AddAsync(battleHistory);
        await _context.SaveChangesAsync();
    }
    public async Task<BattleRoom> LeaveRoom(Guid roomId)
    {
        var room = await _context.BattleRooms.FirstOrDefaultAsync(r => r.roomId == roomId);
        if (room != null)
        {
            _context.BattleRooms.Remove(room);
            await _context.SaveChangesAsync();
        }
        return room;
    }


    public async Task<BattleRoom> DeleteRoom(BattleRoom room)
    {
        if (room == null)
        {
            return null;
        }
        _context.BattleRooms.Remove(room);
        return room;
    }
    
}