using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Text.Json;

namespace FrameForge.Controllers
{
    public class TestsController : Controller
    {
        private readonly TestsServise _testsServise;

        public TestsController(TestsServise testsServise)
        {
            _testsServise = testsServise;
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
            
            ViewBag.Counter = 0;
            ViewBag.Count = testModels.Count;
            return View(testModels[0]);
        }
        
        [HttpPost]
        [Route("[action]")]
        public IActionResult NextTest(int counter)
        {
            var testsJson = HttpContext.Session.GetString("Tests");
            var tests = JsonSerializer.Deserialize<List<TestQuestionModel>>(testsJson);
    
            if (tests == null || counter >= tests.Count)
                return RedirectToAction("Index", "Home");

            // Оновлюємо каунтер в сесії
            HttpContext.Session.SetInt32("Counter", counter);
            ViewBag.Counter = counter;
            ViewBag.Count = tests.Count;
            
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
                return View("TestAnsCorrect", currentTest);
            }
            else
            {
                return View("TestAnsIncorrect", currentTest);
            }
        }
    }
}