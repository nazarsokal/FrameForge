using Entities;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class BattleService
{
    FrameForgeDbContext _context;

    public BattleService(FrameForgeDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<BattleRoom> CreateRoom(Student curStudent)
    {
        var room = new BattleRoom()
        {
            Player1Id = curStudent.StudentId
        };
        _context.BattleRooms.Add(new Entities.BattleRoom());
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task<bool> IfRoomsToEnterExsist()
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
        if (allRooms==null)
        {
            foreach (var room in allRooms)
            {
                if (room.Player2Id != null)
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