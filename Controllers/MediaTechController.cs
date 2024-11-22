using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aafeben.Data;
using Aafeben.Models;

namespace Aafeben.Controllers
{

    [Route("/{culture}/administrateurs/media/")]
    public class MediaTechController : Controller
    {
        private readonly AafebenDbContext _context;

        public MediaTechController(AafebenDbContext context)
        {
            _context = context;
        }

        // GET: MediaTech
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
           
            var Medias = from s in _context.Medias.OrderByDescending(s=> s.PublishedDate) select s;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                Medias = Medias.Where( s => 
                    s.EnCaption.Contains(searchString) 
                    || s.FrCaption.Contains(searchString) ||  s.Type.Contains(searchString)
                );
            }

            ViewData["CurrentFilter"] = "";
            int pageSize = 30;

            return View(await PaginatedList<MediaModel>.CreateAsync( Medias.AsNoTracking(), pageNumber ?? 1, pageSize) );
        }

        // GET: MediaTech/Create
        [Route("ajouter")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: MediaTech/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("ajouter")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EnCaption,FrCaption,Type,URI,PublishedDate")] MediaModel mediaModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mediaModel);
                await _context.SaveChangesAsync();
                return Redirect("/fr/administrateurs/media"); 
            }
            return View(mediaModel);
        }

        // GET: MediaTech/Edit/5
        [Route("modifier/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mediaModel = await _context.Medias.FindAsync(id);
            if (mediaModel == null)
            {
                return NotFound();
            }
            return View(mediaModel);
        }

        // POST: MediaTech/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("modifier/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EnCaption,FrCaption,Type,URI,PublishedDate")] MediaModel mediaModel)
        {
            if (id != mediaModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mediaModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MediaModelExists(mediaModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/fr/administrateurs/media"); 
            }
            return View(mediaModel);
        }

        // GET: MediaTech/Delete/5
        [Route("supprimer/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mediaModel = await _context.Medias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mediaModel == null)
            {
                return NotFound();
            }

            return View(mediaModel);
        }

        // POST: MediaTech/Delete/5
        [HttpPost("supprimer/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mediaModel = await _context.Medias.FindAsync(id);
            if (mediaModel != null)
            {
                _context.Medias.Remove(mediaModel);
            }

            await _context.SaveChangesAsync();
            return Redirect("/fr/administrateurs/media"); 
        }

        private bool MediaModelExists(int id)
        {
            return _context.Medias.Any(e => e.Id == id);
        }
    }
}
