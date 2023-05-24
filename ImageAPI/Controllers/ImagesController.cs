using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImageAPI.Models;
using Microsoft.Extensions.Hosting;

namespace ImageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ImageContext _context;
        public static IWebHostEnvironment environment;

        public ImagesController(ImageContext context, IWebHostEnvironment env)
        {
            _context = context;
            environment = env;
        }

        // GET: api/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Image>>> GetImages()
        {
            if (_context.Images == null)
            {
                return NotFound();
            }
            return await _context.Images.ToListAsync();
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Image>> GetImage(string id)
        {
            if (_context.Images == null)
            {
                return NotFound();
            }
            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return image;
        }

        // POST: api/Images
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Image>> PostImage([FromForm] ImageDTO imgInput)
        {
            if (_context.Images == null)
            {
                return Problem("Entity set 'ImageContext.Images'  is null.");
            }
            Random random = new();
            Image img = new()
            {
                ID = random.Next().ToString(),
            };
            try
            {
                if (imgInput.File.Length > 0)
                {
                    if (!Directory.Exists(environment.WebRootPath + "\\Img"))
                    {
                        Directory.CreateDirectory(environment.WebRootPath + "\\Img\\");
                    }
                    string extension = Path.GetExtension(imgInput.File.FileName);
                    string filePath = $"Img/{img.ID}{extension}";
                    img.ImageURI = $"{Request.Scheme}://{Request.Host.Value}/{filePath}";
                    using FileStream fileStream = System.IO.File.Create($"{environment.WebRootPath}/{filePath}");
                    imgInput.File.CopyTo(fileStream);
                    await fileStream.FlushAsync();
                    img.Created = DateTime.Now;
                    _context.Images.Add(img);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ImageExists(img.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetImage), new { id = img.ID }, img);
        }

        // DELETE: api/Images/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(string id)
        {
            if (_context.Images == null)
            {
                return NotFound();
            }
            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImageExists(string id)
        {
            return (_context.Images?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
