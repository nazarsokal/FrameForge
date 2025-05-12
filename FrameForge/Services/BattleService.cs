using Entities;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;

namespace Services;

public class BattleService : IBattleService
{
    FrameForgeDbContext _context;
    private Random _random;

    public BattleService(FrameForgeDbContext context)
    {
        _context = context;
        _random = new Random();
    }
    
    public string GetRandom20Tests()
    {
        var tests = _context.Tests.ToList();
        if (tests == null)
            throw new ArgumentNullException(nameof(tests));

        if (20 > tests.Count)
            throw new ArgumentException("Count cannot be greater than the number of available units.");

        var RandTests =  tests
            .OrderBy(x => _random.Next())
            .Take(20)
            .ToList();
        return JsonSerializer.Serialize(RandTests);
    }
    public async Task<BattleRoom> CreateRoom(Student curStudent)
    {
        var room = new BattleRoom()
        {
            Player1Id = curStudent.StudentId,
            Questions = GetRandom20Tests()
        };
        _context.BattleRooms.Add(room);
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task<bool> IsRoomsToEnterExsist()
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
                    room.Player2Id = curStudent.StudentId;
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