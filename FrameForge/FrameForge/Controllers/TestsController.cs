using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace FrameForge.Controllers;
public class TestsController : Controller
{
    TestsServise _testsServise;
    List<Test> _tests = null;
    public TestsController(TestsServise testsServise)
    {
        _testsServise = testsServise;
    }
    // GET
    [Route("[action]")]

    public async Task<IActionResult> StartTesting( string title = "intro")
    {
        _tests = await _testsServise.getAllTests(title);
        List<TestQuestionModel> tests = _testsServise.convertFromDto(_tests);
        return View(tests[0]);
    }
}