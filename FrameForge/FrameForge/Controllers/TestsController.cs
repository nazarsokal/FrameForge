using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Text.Json;
using ServiceContracts;

namespace FrameForge.Controllers
{
    public class TestsController : Controller
    {
        private readonly TestsServise _testsServise;
        private readonly IStudentService _studentService;

        public TestsController(TestsServise testsServise, IStudentService studentService)
        {
            _testsServise = testsServise;
            _studentService = studentService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> StartTesting(string title = "intro")
        {
            List<Test> tests = await _testsServise.getAllTests(title);
            List<TestQuestionModel> testModels = _testsServise.convertFromDto(tests);

            // Зберігаємо список і лічильник у Session
            HttpContext.Session.SetString("Tests", JsonSerializer.Serialize(testModels));
            HttpContext.Session.SetInt32("Counter", 0);
            HttpContext.Session.SetInt32("Count", testModels.Count);
            HttpContext.Session.SetInt32("Stars", 0);
            HttpContext.Session.SetInt32("Money", 0);
            
            ViewBag.Counter = 0;
            ViewBag.Count = testModels.Count;
            ViewBag.Stars = 0;
            ViewBag.Money = 0;
            return View(testModels[0]);
        }
        
        [HttpPost]
        [Route("[action]")]
        public IActionResult NextTest(int counter)
        {
            var testsJson = HttpContext.Session.GetString("Tests");
            var tests = JsonSerializer.Deserialize<List<TestQuestionModel>>(testsJson);
    
            if (tests == null || counter >= tests.Count)
                return RedirectToAction("FinishTest", "Tests");

            // Оновлюємо каунтер в сесії
            HttpContext.Session.SetInt32("Counter", counter);
            ViewBag.Counter = counter;
            ViewBag.Count = tests.Count;
            ViewBag.Stars = HttpContext.Session.GetInt32("Stars") ?? 0;
            ViewBag.Money = HttpContext.Session.GetInt32("Money") ?? 0;
            
            return View("StartTesting", tests[counter]);
        }
        [HttpPost]
        [Route("[action]")]
        public IActionResult CheckAnswer(int counter, string answer)
        {
            var testsJson = HttpContext.Session.GetString("Tests");
            var tests = JsonSerializer.Deserialize<List<TestQuestionModel>>(testsJson);
            
            var currentTest = tests[counter];
            bool isCorrect = answer == currentTest.Answer;
            
            // Оновлюємо каунтер в сесії
            counter++;
            HttpContext.Session.SetInt32("Counter", counter);
            ViewBag.Counter = counter;
            ViewBag.Count = tests.Count;

            if (isCorrect)
            {
                // Збільшуємо зірочки і монети за правильну відповідь
                int currentStars = HttpContext.Session.GetInt32("Stars") ?? 0;
                int currentMoney = HttpContext.Session.GetInt32("Money") ?? 0;
                
                HttpContext.Session.SetInt32("Stars", currentStars + 1);
                HttpContext.Session.SetInt32("Money", currentMoney + 10);
                
                ViewBag.Stars = currentStars + 1;
                ViewBag.Money = currentMoney + 10;
                return View("TestAnsCorrect", currentTest);
            }
            else
            {
                ViewBag.Stars = HttpContext.Session.GetInt32("Stars") ?? 0;
                ViewBag.Money = HttpContext.Session.GetInt32("Money") ?? 0;
                return View("TestAnsIncorrect", currentTest);
            }
        }
        [HttpGet]
        [HttpPost]
        [Route("[action]")]
        public IActionResult FinishTest()
        {
            ViewBag.Stars = HttpContext.Session.GetInt32("Stars") ?? 0;
            ViewBag.Money = HttpContext.Session.GetInt32("Money") ?? 0;
            string userString = HttpContext.Session.GetString("Student");
            var st = JsonSerializer.Deserialize<Student>(userString);
            st.MoneyAmount += HttpContext.Session.GetInt32("Money") ?? 0;
            st.StarsAmount += HttpContext.Session.GetInt32("Stars") ?? 0;
            var updatedStudent = _studentService.UpdateStudent(st);
            
            // Оновлюємо дані в сесії
            string updatedUserString = JsonSerializer.Serialize(updatedStudent);
            HttpContext.Session.SetString("Student", updatedUserString);
            
            return View("Conclusion", updatedStudent);
        }
    }
}