using Entities;

namespace ServiceContracts;

public interface IBattleService
{
    public Task<BattleRoom> CreateRoom(Student curStudent);
    public Task<bool> IsRoomsToEnterExsist();
    public Task<BattleRoom> EnterTheRoom(Student curStudent);
    public Task<BattleRoom> DeleteRoom(BattleRoom room);




}