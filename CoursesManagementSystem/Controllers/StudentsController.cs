using CoursesManagementSystem.Models;
using CoursesManagementSystem.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CoursesManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        public IServiceRepositery<Student> Repo;
        public StudentsController(IServiceRepositery<Student> repo)
        {
            Repo = repo;
        }
        [HttpGet, Authorize]
        [ProducesResponseType(200,Type=typeof(IEnumerable<Student>))]
        public async Task<IEnumerable<Student>> GetAllStudents(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await Repo.GetAllAsync();
            }
            else
            {
                return (await Repo.GetAllAsync()).Where(s => s.Name!.Contains(name));
            }
        }
        // GET: api/students/[id]
        [HttpGet("{id}", Name = nameof(GetStudent)), Authorize(Roles = "CourseAdmin")] // named route
        [ProducesResponseType(200, Type = typeof(Student))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStudent(int id)
        {
            Student s = await Repo.GetByIdAsync(id);
            if (s == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(s); // 200 OK with student in body
        }
        // POST: api/students
        // BODY: Student (JSON, XML)
        [Produces("application/json","text/plain")]
        [HttpPost, Authorize(Roles = "CourseAdmin")]
        [ProducesResponseType(201, Type = typeof(Student))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Student s)
        {
            if (s == null)
            {
                return BadRequest(); // 400 Bad request
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad request
            }
            Student added = await Repo.CreateAsync(s);
            return CreatedAtRoute( // 201 Created
            routeName: nameof(GetStudent),
            routeValues: new { id = added.Id },
            value: added);
        }
        // PUT: api/students/[id]
        // BODY: Student (JSON, XML)
        
        [HttpPut("{id}"), Authorize(Roles = "CourseAdmin"+","+"Student")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(
int id, [FromBody] Student s)
        {
            
           
            if (s == null || s.Id != id)
            {
                return BadRequest(); // 400 Bad request
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad request
            }
            var existing = await Repo.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(); // 404 Resource not found
            }
            await Repo.UpadteAsync(id, s);
            return new NoContentResult(); // 204 No content
        }
        // DELETE: api/students/[id]
        [HttpDelete("{id}"), Authorize(Roles = "CourseAdmin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await Repo.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(); // 404 Resource not found
            }
            bool? deleted = await Repo.DeleteAsync(id);
            if (deleted.HasValue && deleted.Value) // short circuit AND
            {
                return new NoContentResult(); // 204 No content
            }
            else
            {
                return BadRequest( // 400 Bad request
                $"student {id} was found but failed to delete.");
            }
        }
    }
}
