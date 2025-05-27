using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;

namespace ServicesTest;

public class GroupServiceTest
{
    private readonly IGroupService _groupService;
    private readonly IRegistrationService _registrationService;
    private readonly IUserService _userService;
    private readonly IAzureStorageService _azureStorageService;
    public IFixture _fixture;
    private FrameForgeDbContext dbContext;

    public GroupServiceTest()
    {
        _fixture = new Fixture();
        var usersMoqData = new List<User>() {};
        var groupsMoqData = new List<Group>() {};
        
        DbContextMock<FrameForgeDbContext> dbContextMock = new DbContextMock<FrameForgeDbContext>(
            new DbContextOptionsBuilder<FrameForgeDbContext>().Options
        );
        
        dbContext = dbContextMock.Object;
        dbContextMock.CreateDbSetMock(temp => temp.Users, usersMoqData);
        dbContextMock.CreateDbSetMock(temp => temp.Groups, groupsMoqData);

        _groupService = new GroupService(dbContext);
        _azureStorageService = new AzureStorageService();
        _registrationService = new RegistrationService(dbContext, azureStorageService: _azureStorageService);
        _userService = new UserService(dbContext);
    }

    [Fact]
    public async Task AssignStudentToAGroup()
    {
        var teacher = new Teacher()
        {
            Username = "Slavko", Email = "Slavko@gmail.com", UserId = Guid.NewGuid(),
            Password = "Slavko123"
        };

        var student = new Student()
        {
            Username = "Nazarchuk", Email = "Nazarchuk@gmail.com", UserId = Guid.NewGuid(), Password = "Nazarchuk123"
        };

        var group = new Group() { Id = Guid.NewGuid(), TeacherId = teacher.UserId, GroupName = "SlavkovaGrupa"};

        // Add data to the context
        dbContext.Users.Add(teacher);
        dbContext.Users.Add(student);
        await _groupService.AddGroup(group);

        // Act
        var groupsByTeacher = await _groupService.GetGroupByTeacherId(teacher.UserId);
        var firstGroup = groupsByTeacher.First();
        await _groupService.AssignStudent(firstGroup.Id, student.UserId);

        // Assert
        var allGroups = await _groupService.GetGroups();
        Assert.Contains(allGroups, g => g.Students.Any(s => s.UserId == student.UserId));
    }

}