using CoursesManagementSystem.Models;
using CoursesManagementSystem.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CoursesManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        public IServiceRepositery<Instructor> Repo;
        public InstructorsController(IServiceRepositery<Instructor> repo)
        {
            Repo = repo;
        }
        [HttpGet, Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Instructor>))]
        public async Task<IEnumerable<Instructor>> GetAllInstructors(string? name)
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
        // GET: api/instructors/[id]
        [HttpGet("{id}", Name = nameof(GetInstructor)),Authorize] // named route
        [ProducesResponseType(200, Type = typeof(Instructor))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetInstructor(int id)
        {
            Instructor i= await Repo.GetByIdAsync(id);
            if (i == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(i); // 200 OK with instructor in body
        }
        // POST: api/instructors
        // BODY: Student (JSON, XML)
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Instructor)), Authorize(Roles = "CourseAdmin")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Instructor i)
        {
            if (i == null)
            {
                return BadRequest(); // 400 Bad request
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad request
            }
            Instructor added = await Repo.CreateAsync(i);
            return CreatedAtRoute( // 201 Created
            routeName: nameof(GetInstructor),
            routeValues: new { id = added.Id },
            value: added);
        }
        // PUT: api/instructors/[id]
        // BODY: Instructor (JSON, XML)
        [HttpPut("{id}"), Authorize(Roles = "CourseAdmin"+","+"Intructor")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(
int id, [FromBody] Instructor i)
        {


            if (i == null || i.Id != id)
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
            await Repo.UpadteAsync(id, i);
            return new NoContentResult(); // 204 No content
        }
        // DELETE: api/instructors/[id]
        [HttpDelete("{id}"),Authorize(Roles = "CourseAdmin")]
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
                $"Instructor {id} was found but failed to delete.");
            }
        }
    }
}
