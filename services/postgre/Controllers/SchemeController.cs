using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using postgre.Services;

namespace postgre.Controllers
{
    [Route("api/")]
    [ApiController]
    public class SchemeController : ControllerBase
    {
        private readonly SchemeCreator schemeCreator;
        public SchemeController(SchemeCreator creator)
        {
            schemeCreator= creator;
        }

        [HttpGet("create")]
        public IActionResult Get()
        {
            schemeCreator.CreateScheme();
            return Ok();
        }

        [HttpGet("fill")]
        public IActionResult Fill([FromServices] DataFiller dataFiller)
        {
            dataFiller.FillScheme();
            return Ok();
        }

        [HttpGet("students")]
        public IActionResult GetStudents([FromServices] DataHelper dataHelper)
        {
            var students = dataHelper.GetStudents();
            return Ok(students);
        }
    }
}
