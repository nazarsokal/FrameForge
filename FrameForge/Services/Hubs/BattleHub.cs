using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Http;
using ServiceContracts;

namespace Services.Hubs;

public class BattleHub : Hub
{
    private readonly IBattleService _battleService;
    private readonly IStudentService _studentService;
    private readonly BattleSingleton _battleSingleton;

    public BattleHub(IBattleService battleService, IStudentService studentService, BattleSingleton battleSingleton)
    {
        _battleService = battleService;
        _studentService = studentService;
        _battleSingleton = battleSingleton;
    }

    public async Task CreateBattleRoom()
    {
        var studentString = Context.GetHttpContext().Session.GetString("Student");
        if (string.IsNullOrEmpty(studentString))
            return;
        var student = JsonSerializer.Deserialize<Student>(studentString);
        var room = await _battleService.CreateRoom(student);
        _battleSingleton.Add(student.StudentId);
        await Groups.AddToGroupAsync(Context.ConnectionId, room.roomId.ToString());
        await Clients.Caller.SendAsync("BattleRoomCreated", room);
    }

    public async Task JoinBattleRoom(string roomId)
    {
        var studentString = Context.GetHttpContext().Session.GetString("Student");
        if (string.IsNullOrEmpty(studentString))
            return;

        var student = JsonSerializer.Deserialize<Student>(studentString);
        var room = await _battleService.JoinRoom(Guid.Parse(roomId), student);
        _battleSingleton.Add(student.StudentId);

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("PlayerJoined", room);
    }
    public async Task JoinGroup(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
    }

    public async Task GetStudent(string id)
    {
        try
        {
            if (!Guid.TryParse(id, out Guid guid))
            {
                await Clients.Caller.SendAsync("StudentError", "Неправильний формат Id");
                return;
            }

            var student = await _studentService.GetStudentById(guid);
            if (student != null)
            {
                await Clients.Caller.SendAsync("Student", student);
            }
            else
            {
                await Clients.Caller.SendAsync("StudentError", "Студента не знайдено");
            }
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("StudentError", $"Помилка: {ex.Message}");
        }
    }

    public async Task SubmitAnswer(string roomId, string answer, int questionIndex)
    {
        var studentString = Context.GetHttpContext().Session.GetString("Student");
        if (string.IsNullOrEmpty(studentString))
            return;
    
        var student = JsonSerializer.Deserialize<Student>(studentString);
        var result = await _battleService.SubmitAnswer(Guid.Parse(roomId), student.StudentId, answer, questionIndex - 1);
        
        await Clients.Group(roomId).SendAsync("AnswerSubmitted", new
        {
            PlayerId = student.StudentId,
            PlayerName = student.Username,
            IsCorrect = result.IsCorrect,
            Player1score = result.CurrentPlayer1Score,
            player2score = result.CurrentPlayer2Score,
            IsBattleComplete = result.IsBattleComplete,
            WinnerId = result.WinnerId
        });

        if (result.IsBattleComplete)
        {
            await _battleService.EndBattle(Guid.Parse(roomId));
        }
    }
    
    public async Task SetReady(string roomId)
    {
        var studentString = Context.GetHttpContext().Session.GetString("Student");
        if (string.IsNullOrEmpty(studentString))
            return;

        var student = JsonSerializer.Deserialize<Student>(studentString);
        _battleSingleton.SetReady(student.StudentId);
        var curRoom = await _battleService.GetRoomStatus(Guid.Parse(roomId));
        if (_battleSingleton.IsReady(curRoom.Player1Id) && _battleSingleton.IsReady(curRoom.Player2Id))
        {
            await Clients.Group(roomId).SendAsync("AllAreReady");
        }
        else if (_battleSingleton.IsReady(curRoom.Player1Id) || _battleSingleton.IsReady(curRoom.Player2Id))
        {
            await Clients.OthersInGroup(roomId).SendAsync("OpponentIsReady",
                (_battleSingleton.IsReady(curRoom.Player1Id) == true) ? curRoom.Player1Id : curRoom.Player2Id);
        }
        else
        {
            await Clients.Group(roomId).SendAsync("ErrorNoOneReady");
        }
    }
    public async Task LeaveRoom(string roomId)
    {
        var studentString = Context.GetHttpContext().Session.GetString("Student");
        if (string.IsNullOrEmpty(studentString))
            return;

        var student = JsonSerializer.Deserialize<Student>(studentString);
    
    
        // Видалення кімнати
        var room =  await _battleService.LeaveRoom(Guid.Parse(roomId));
        

        // Повідомлення іншим клієнтам у групі
        if (room != null)
        {
            await Clients.OthersInGroup(roomId).SendAsync("OpponentLeft", room);
        }
        // Видалення користувача з групи
        //await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

        // Видалення зі сінглтона, якщо використовується
        _battleSingleton.Remove(room.Player1Id);
        _battleSingleton.Remove((Guid)room.Player2Id);
    }
    public async Task GetAvailableRooms()
    {
        var rooms = await _battleService.GetAvailableRooms();
        await Clients.Caller.SendAsync("AvailableRooms", rooms);
    }
}