using Microsoft.AspNetCore.Mvc;

namespace TheVilleSkill.Controllers
{
    [Route("PrivacyPolicy")]
    public class PrivacyPolicyController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}