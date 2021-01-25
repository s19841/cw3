using System.Linq;
using Cw3.Services;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("students")]
    public class StudentsController : ControllerBase
    {
        private static IDbService<Student> _dbService;

        public StudentsController(IDbService<Student> dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{idStudent}")]
        public IActionResult GetStudent([FromRoute] int idStudent)
        {
            var student = _dbService.GetEntry(idStudent);
            if (student == null) return NotFound($"Student not found ID : {idStudent}!");
            return Ok(student);
        }

        [HttpGet]
        public IActionResult GetStudents([FromQuery] string orderBy)
        {
            var orderByToUse = orderBy ?? "IdStudent";
            var orderedEnumerable = _dbService.GetEntries();
            return orderByToUse.ToLower() switch
            {
                "firstname"   => Ok(orderedEnumerable.OrderBy(student => student.FirstName)),
                "lastname"    => Ok(orderedEnumerable.OrderBy(student => student.LastName)),
                "indexnumber" => Ok(orderedEnumerable.OrderBy(student => student.IndexNumber)),
                _             => Ok(orderedEnumerable.OrderBy(student => student.IdStudent))
            };
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IdStudent = _dbService.NextId();
            _dbService.AddEntry(student);
            return Ok($"Student created : {student}.");
        }

        [HttpPut("{idStudent}")]
        public IActionResult PutStudent([FromRoute] int idStudent, [FromBody] Student newStudent)
        {
            var student = _dbService.GetEntry(idStudent);
            if (student == null) return CreateStudent(newStudent);
            student.FirstName = newStudent.FirstName;
            student.LastName = newStudent.LastName;
            return Ok($"Student updated : {student}.");
        }

        [HttpDelete("{idStudent}")]
        public IActionResult DeleteStudent([FromRoute] int idStudent)
        {
            var student = _dbService.GetEntry(idStudent);
            if (student == null) return NotFound($"Student not found ID : {idStudent}!");
            _dbService.RemoveEntry(student);
            return Ok($"Student deleted : {student}");
        }
    }
}