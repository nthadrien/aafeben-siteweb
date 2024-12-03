using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aafeben.Data;
using Aafeben.Models;
using Microsoft.AspNetCore.Authorization;

namespace Aafeben.Controllers
{
    [Authorize]
    [Route("/{culture}/administrateurs/projets/")]
    public class ProjetsController : Controller
    {
        private readonly AafebenDbContext _context;
        private readonly string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images","projets");


        public ProjetsController(AafebenDbContext context)
        {
            _context = context;
        }

        // GET: Projet
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
            var projects = from p in _context.Projets.OrderByDescending(s => s.EndDate) select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                projects = projects.Where(s => s.Title.Contains(searchString)
                    || s.Duration.Contains(searchString) || s.Zone.Contains(searchString) ||
                    s.StartDate.ToString().Contains(searchString)
                );
            }
            
            int pageSize = 30;

            return View(await PaginatedList<ProjectModel>.CreateAsync(projects.AsNoTracking(), pageNumber ?? 1 , pageSize ) );
        }


        // GET: Projet/Create
        [Route("nouveau")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("nouveau")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Language,Zone,Duration,StartDate,EndDate")] ProjectModel projectModel, IFormFile FeaturedImageUrl )
        {
            if (ModelState.IsValid && FeaturedImageUrl != null )
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string fileName = Guid.NewGuid()  + Path.GetExtension(FeaturedImageUrl.FileName);
                string filePath = Path.Combine(directoryPath, fileName);

                using ( var memoryStream = new MemoryStream())
                {
                    await FeaturedImageUrl.CopyToAsync(memoryStream);
                    // Upload the file if less than 2 MB
                    if (memoryStream.Length < 2097152) {
                        var stream = new FileStream(filePath, FileMode.Create);
                        await FeaturedImageUrl.CopyToAsync(stream);
                    } else {
                        throw new Exception ("Image ne doit pas etre plus de 2MB");
                    }
                }

                projectModel.FeaturedImageUrl = $"{fileName}";
                
                _context.Add(projectModel);
                await _context.SaveChangesAsync();
                return Redirect($"/{projectModel.Language}/administrateurs/projets");
            }
            return View(projectModel);
        }

        // GET: Projet/Edit/5
        [Route("modifier/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectModel = await _context.Projets.FindAsync(id);
            if (projectModel == null)
            {
                return NotFound();
            }
            return View(projectModel);
        }

        // POST: Projet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("modifier/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,FeaturedImageUrl,Language,Zone,Duration,StartDate,EndDate")] ProjectModel projectModel)
        {
            if (id != projectModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectModelExists(projectModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect($"/{projectModel.Language}/administrateurs/projets");
            }
            return View(projectModel);
        }

        // GET: Projet/Delete/5
        [Route("supprimer/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectModel = await _context.Projets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectModel == null)
            {
                return NotFound();
            }

            return View(projectModel);
        }

        // POST: Projet/Delete/5
        [HttpPost("supprimer/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectModel = await _context.Projets.FindAsync(id);

            if (projectModel != null)
            {
                var filePath = projectModel.FeaturedImageUrl;

                if ( filePath != null )
                {
                    var ImgPath = Path.Combine(directoryPath, filePath );
                    if (System.IO.File.Exists(ImgPath))
                    {
                        System.IO.File.Delete(ImgPath);
                    }
                }
                _context.Projets.Remove(projectModel);
            }

            await _context.SaveChangesAsync();
            return Redirect("/fr/administrateurs/projets");
        }

        private bool ProjectModelExists(int id)
        {
            return _context.Projets.Any(e => e.Id == id);
        }
    }
}
