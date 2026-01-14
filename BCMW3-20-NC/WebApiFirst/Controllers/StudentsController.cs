using Microsoft.AspNetCore.Mvc;

namespace WebApiFirst.Controllers
{
    public class Student
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }


    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private static List<Student> _students = new()
        {
            new Student { Id = 1, Name = "Alice" },
            new Student { Id = 2, Name = "Bob" },
            new Student { Id = 3, Name = "Charlie" }
        };


        //[HttpGet]
        //public string GetStudent()
        //{
        //    return _students.Select(s => string.Join(", ", s)).ToString();
        //}

        //[HttpGet]
        //public string GetStudent([FromQuery] int id)
        //{
        //    return _students.FirstOrDefault(x => x.Id == id)?.Name ?? "Not Found";
        //}

        //[HttpGet("{id}")]
        //public string GetStudent([FromRoute] int id)
        //{
        //    return _students.FirstOrDefault(x => x.Id == id)?.Name ?? "Not Found";
        //}


        [HttpPost]
        public IActionResult CreateStudent([FromBody] Student student)
        {
            _students.Add(student);
            return NoContent();
        }
    }
}
