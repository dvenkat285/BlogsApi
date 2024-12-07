using BlogsApi.Data;
using BlogsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogsApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BlogsController(AppDbContext context) 
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blogs>>> GetBlogs()
        {
            return await _context.Blogs.ToListAsync();
        }
        [HttpGet("{id}")]
        public  async Task<ActionResult<Blogs>> GetBlogs(int id)
        {
            var blogs = await _context.Blogs.FindAsync(id);
            if(blogs == null)
            {
                return NotFound();
            }
            return blogs;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlogs(int id, Blogs blogs)
        {
            if (id != blogs.BlogId)
            {
                return BadRequest();
            }
            _context.Entry(blogs).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool BlogsExists(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<Blogs>>PostBlogs(Blogs blogs)
        {
            _context.Blogs.Add(blogs);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetBlogs", new { id = blogs.BlogId }, blogs);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogs(int id)
        {
            var blogs = await _context.Blogs.FindAsync(id);
            if(blogs == null)
            {
                return NotFound();
            }
            _context.Blogs.Remove(blogs);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
