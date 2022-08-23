using CoursesManagementSystem.Models;
using CoursesManagementSystem.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoursesManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        public IServiceRepositery<Course> Repo;
        public CoursesController(IServiceRepositery<Course> repo)
        {
            Repo = repo;
        }
        [HttpGet,Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Student>))]
        public async Task<IEnumerable<Course>> GetAllCourses(string? name)
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
        // GET: api/courses/[id]
        [HttpGet("{id}", Name = nameof(GetCourse)), Authorize] // named route
        [ProducesResponseType(200, Type = typeof(Course))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCourse(int id)
        {
            Course s = await Repo.GetByIdAsync(id);
            if (s == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(s); // 200 OK with course in body
        }
        // POST: api/courses
        // BODY: Student (JSON, XML)
        [HttpPost, Authorize(Roles = "CourseAdmin")]
        [ProducesResponseType(201, Type = typeof(Course))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Course c)
        {
            if (c == null)
            {
                return BadRequest(); // 400 Bad request
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad request
            }
            Course added = await Repo.CreateAsync(c);
            return CreatedAtRoute( // 201 Created
            routeName: nameof(GetCourse),
            routeValues: new { id = added.Id },
            value: added);
        }
        // PUT: api/courses/[id]
        // BODY: Course (JSON, XML)
        [HttpPut("{id}"), Authorize(Roles = "CourseAdmin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(
int id, [FromBody] Course c)
        {


            if (c == null || c.Id != id)
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
            await Repo.UpadteAsync(id, c);
            return new NoContentResult(); // 204 No content
        }
        // DELETE: api/courses/[id]
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
                $"Course {id} was found but failed to delete.");
            }
        }
    }
}
