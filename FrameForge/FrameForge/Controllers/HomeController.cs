using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace FrameForge.Controllers;

public class HomeController : Controller
{
    private readonly IUserService _userService;
    private readonly IGroupService _groupService;
    private readonly IExerciseService _exerciseService;

    public HomeController(IUserService userService, IGroupService groupService, IExerciseService exerciseService)
    {
        _userService = userService;
        _groupService = groupService;
        _exerciseService = exerciseService;
    }
    [Route("/")]
    public async Task<IActionResult> Index()
    {
        User user = new User();
        var userType = HttpContext.Session.GetString("UserType");
        if (userType != null)
        {
            if (userType == "Student")
            {
                var userJson = HttpContext.Session.GetString("Student");
                user = JsonSerializer.Deserialize<Student>(userJson);
 
                Group? studentGroup = await _groupService.GetGroupByStudentId(user.UserId);
                if (studentGroup != null)
                {
                    List<Exercise>? exercisesList = await _exerciseService.GetExercises(studentGroup.Id);

                    ViewBag.GroupName = studentGroup.GroupName;
                    ViewBag.Exercises = exercisesList;
                }
                else
                {
                    ViewBag.GroupName = "";
                }
            }
            else if(userType == "Teacher")
            {
                var userJson = HttpContext.Session.GetString("Teacher");
                user = JsonSerializer.Deserialize<Teacher>(userJson);
                Dictionary<string, List<Exercise>> exercisesDictionary = new Dictionary<string, List<Exercise>>();
                
                List<Group> groups = await _groupService.GetGroupByTeacherId(user.UserId);

                foreach (var group in groups)
                {
                    exercisesDictionary[group.GroupName] = await _exerciseService.GetExercises(group.Id); 
                }
                
                ViewBag.Groups = groups;
                ViewBag.Exercises = exercisesDictionary;
            }
            return View(user);
        }

        return View();
    }
}