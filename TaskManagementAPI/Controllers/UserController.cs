using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.User>>> GetUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                if (users == null || users.Count == 0)
                {
                    // No users found, return 404 Not Found
                    return NotFound();
                }

                // Return the list of users with a 200 OK status
                return Ok(users);
            }
            catch (Exception ex) 
            {
                // If an exception occurs, return 500 Internal Server Error with the exception message
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<Models.Task>> CreateUser(Models.User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
            }
            catch (DbUpdateException ex)
            {
                // Handle database-related errors 
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other types of exceptions 
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Models.User user)
        {
            try
            {
                if (id != user.Id)
                    return BadRequest();

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle database concurrency exceptions, e.g., when another user has updated the record
                return Conflict($"Concurrency error: {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Handle database update-related exceptions 
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions 
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound();

                _context.Users.Remove(user);
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
