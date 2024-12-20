using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aafeben.Data;
using Aafeben.Models;
using Microsoft.AspNetCore.Authorization;

namespace Aafeben.Controllers
{
    [Authorize]
    [Route("/{culture}/administrateurs/publications/")]
    public class PublicationsController : Controller
    {
        private readonly AafebenDbContext _context;
        private readonly string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "docs","publications");

        public PublicationsController(AafebenDbContext context)
        {
            _context = context;
        }

        // GET: Publications
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
            var publications = from p in _context.Publications select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                publications = publications.Where(s => s.Title.Contains(searchString) ||
                 s.PublishedDate.ToString().Contains(searchString) 
                );
            }

            publications = publications.OrderByDescending(s => s.PublishedDate);
            int pageSize = 30;

            return View(await PaginatedList<PublicationModel>.CreateAsync(publications.AsNoTracking(), pageNumber ?? 1 , pageSize ) );
        }

        // GET: Publications/Create
        [Route("nouvelle")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("nouvelle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,PublishedDate,Language")] PublicationModel publicationModel, IFormFile Content)
        {

            if (ModelState.IsValid && Content != null  )
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string fileName = Guid.NewGuid()  + Path.GetExtension(Content.FileName);
                string filePath = Path.Combine(directoryPath, fileName);

                using ( var memoryStream = new MemoryStream())
                {
                    await Content.CopyToAsync(memoryStream);
                    // Upload the file if less than 2 MB
                    var stream = new FileStream(filePath, FileMode.Create);
                    await Content.CopyToAsync(stream);
                }

                publicationModel.Content = $"{fileName}";

                _context.Add(publicationModel);
                await _context.SaveChangesAsync();
                return Redirect("/fr/administrateurs/publications");
            }
            return View(publicationModel);
        }

        // GET: Publications/Edit/5
        [Route("modifier/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicationModel = await _context.Publications.FindAsync(id);
            if (publicationModel == null)
            {
                return NotFound();
            }
            return View(publicationModel);
        }

        // POST: Publications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("modifier/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,PublishedDate,Language")] PublicationModel publicationModel)
        {
            if (id != publicationModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publicationModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicationModelExists(publicationModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/fr/administrateurs/publications");
            }
            return View(publicationModel);
        }

        // GET: Publications/Delete/5
        [Route("supprimer/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicationModel = await _context.Publications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publicationModel == null)
            {
                return NotFound();
            }

            return View(publicationModel);
        }

        // POST: Publications/Delete/5
        [HttpPost("supprimer/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publicationModel = await _context.Publications.FindAsync(id);
            if (publicationModel != null)
            {
                var filePath = publicationModel.Content;

                if ( filePath != null )
                {
                    var ImgPath = Path.Combine(directoryPath, filePath );
                    if (System.IO.File.Exists(ImgPath))
                    {
                        System.IO.File.Delete(ImgPath);
                    }
                }
                _context.Publications.Remove(publicationModel);
            }

            await _context.SaveChangesAsync();
             return Redirect("/fr/administrateurs/publications");
        }

        private bool PublicationModelExists(int id)
        {
            return _context.Publications.Any(e => e.Id == id);
        }
    }
}
