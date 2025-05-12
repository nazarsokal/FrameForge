using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ServiceContracts;
using Services;

namespace ServicesTest;

public class RegistrationServiceTest
{
    private readonly IRegistrationService _registrationService;

    public RegistrationServiceTest()
    {
    }

    #region RegisterUser

    [Fact]
    public async Task RegisterUser_RegisterUser_Success()
    {
        var student = new Student() {Username = "student", Password = "password", Email = "student@email.com", StudentId = Guid.NewGuid(), MoneyAmount = 10.5};
        _registrationService.RegisterStudent(student);

        var studentsReceived = await _registrationService.GetStudents();
        User? studentMatched = studentsReceived.FirstOrDefault(s => s.StudentId == student.StudentId);
        
        Assert.Equal(student, studentMatched);
    }
    #endregion
}