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

    public BattleHub(IBattleService battleService, IStudentService studentService)
    {
        _battleService = battleService;
        _studentService = studentService;
    }

    public async Task CreateBattleRoom()
    {
        var studentString = Context.GetHttpContext().Session.GetString("Student");
        if (string.IsNullOrEmpty(studentString))
            return;
        var student = JsonSerializer.Deserialize<Student>(studentString);
        var room = await _battleService.CreateRoom(student);
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
        
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("PlayerJoined", room);
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

    // public async Task SubmitAnswer(string roomId, string answer)
    // {
    //     var studentString = Context.GetHttpContext().Session.GetString("Student");
    //     if (string.IsNullOrEmpty(studentString))
    //         return;
    //
    //     var student = JsonSerializer.Deserialize<Student>(studentString);
    //     var result = await _battleService.SubmitAnswer(Guid.Parse(roomId), student.StudentId, answer);
    //     
    //     await Clients.Group(roomId).SendAsync("AnswerSubmitted", new
    //     {
    //         PlayerId = student.StudentId,
    //         IsCorrect = result.IsCorrect,
    //         CurrentScore = result.CurrentScore,
    //         IsBattleComplete = result.IsBattleComplete,
    //         WinnerId = result.WinnerId
    //     });
    // }
    public async Task StartBattle(string roomId)
    {
        
    }

    public async Task GetAvailableRooms()
    {
        var rooms = await _battleService.GetAvailableRooms();
        await Clients.Caller.SendAsync("AvailableRooms", rooms);
    }
}