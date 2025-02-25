using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Task
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasks()
        {
            try
            {
                // Attempt to retrieve all tasks from the database
                var tasks = await _context.Tasks.ToListAsync();

                // If tasks are found, return them with a 200 OK status
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                // If an exception occurs (e.g., database connection issue), return 500 Internal Server Error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Task
        [HttpPost]
        public async Task<ActionResult<Models.Task>> CreateTask(Models.Task task)
        {
            try
            {
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
            }
            catch (DbUpdateException ex)
            {
                // Handle database-related errors 
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other general exceptions 
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Task/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, Models.Task task)
        {
            try
            {
                if (id != task.Id)
                    return BadRequest();

                _context.Entry(task).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflicts 
                return Conflict($"Concurrency error: {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Handle database update-related errors 
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other types of exceptions 
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            }

        // DELETE: api/Task/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                    return NotFound();

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                // Handle database-related errors 
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other exceptions 
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
