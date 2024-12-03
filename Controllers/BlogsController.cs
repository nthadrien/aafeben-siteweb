
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aafeben.Data;
using Aafeben.Models;
using Microsoft.AspNetCore.Authorization;

namespace Aafeben.Controllers
{
    [Authorize]
    [Route("/{culture}/administrateurs/blogs/")]
    public class BlogsController : Controller
    {
        private readonly AafebenDbContext _context;
        private readonly string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images","blogs");

        public BlogsController(AafebenDbContext context)
        {
            _context = context;
        }

        // GET: Blogs
        // [Route("/{culture}/administrateurs/blogs")]
        [HttpGet]
        public async Task<IActionResult> Index(
            string searchString,
            string currentFilter,
            int? pageNumber
        )
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
           
            var Blogs = from s in _context.BlogPosts.OrderByDescending(s=> s.PublishedDate) select s;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                Blogs = Blogs.Where( s => 
                    s.Title.Contains(searchString) 
                    || s.ShortDescription.Contains(searchString)
                );
            }

            ViewData["CurrentFilter"] = "";
            int pageSize = 30;

            return View(await PaginatedList<BlogPostModel>.CreateAsync( Blogs.AsNoTracking(), pageNumber ?? 1, pageSize) );
        }

        // GET: Blogs/Create
        [Route("nouveau")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("nouveau")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPostModel blogPostModel, IFormFile FeaturedImageUrl )
        {
            if (ModelState.IsValid && FeaturedImageUrl != null )
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(FeaturedImageUrl.FileName);
                string filePath = Path.Combine(directoryPath, fileName);

                using ( var memoryStream = new MemoryStream())
                {
                    await FeaturedImageUrl.CopyToAsync(memoryStream);
                    // Upload the file if less than 2 MB
                    if (memoryStream.Length < 2097152) {
                        var stream = new FileStream(filePath, FileMode.Create);
                        await FeaturedImageUrl.CopyToAsync(stream);
                    } else {
                        throw new Exception ("Image trop large.");
                    }

                }
                blogPostModel.FeaturedImageUrl = $"{fileName}";
                _context.Add(blogPostModel);
                await _context.SaveChangesAsync();
                return Redirect("/fr/administrateurs/blogs");
            }
            return View(blogPostModel);
        }

        // GET: Blogs/Edit/5
        [Route("modifier/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPostModel = await _context.BlogPosts.FindAsync(id);
            if (blogPostModel == null)
            {
                return NotFound();
            }
            return View(blogPostModel);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("modifier/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,ShortDescription,PublishedDate,FeaturedImageUrl,Language")] BlogPostModel blogPostModel)
        {
            if (id != blogPostModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogPostModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostModelExists(blogPostModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/fr/administrateurs/blogs");
            }
            return View(blogPostModel);
        }

        // GET: Blogs/Delete/5
        [Route("supprimer/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPostModel = await _context.BlogPosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPostModel == null)
            {
                return NotFound();
            }

            return View(blogPostModel);
        }

        // POST: Blogs/Delete/5
        [HttpPost("supprimer/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPostModel = await _context.BlogPosts.FindAsync(id);
            
            if (blogPostModel != null)
            {
                var filePath = blogPostModel.FeaturedImageUrl;

                if ( filePath != null )
                {
                    var ImgPath = Path.Combine(directoryPath, filePath );
                    if (System.IO.File.Exists(ImgPath))
                    {
                        System.IO.File.Delete(ImgPath);
                    }
                }
                _context.BlogPosts.Remove(blogPostModel);
            }
            await _context.SaveChangesAsync();
            return Redirect("/fr/administrateurs/blogs");
        }

        private bool BlogPostModelExists(int id)
        {
            return _context.BlogPosts.Any(e => e.Id == id);
        }
    }
}
