using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace FrameForge.Controllers;

public class UserPageController : Controller
{
    private Student student;
    
    [HttpGet]
    [Route("[action]")]
    public IActionResult UserPage()
    {
        student = GetStudentFromSession();
        return View(student);
    }
    
    private Student GetStudentFromSession()
    {
        var studentJson = HttpContext.Session.GetString("student");
        if (studentJson != null) student = JsonSerializer.Deserialize<Student>(studentJson);
        if(student == null) throw new NullReferenceException("Student is null");
        
        return student;
    }
}