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
    [Route("/{culture}/administrateurs/partenaires/")]
    public class PartenairesController : Controller
    {
        private readonly AafebenDbContext _context;
        readonly string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images","partenaires");


        public PartenairesController(AafebenDbContext context)
        {
            _context = context;
        }

        // GET: Partenaires
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
            var partenaires = from p in _context.Partners.OrderByDescending( p => p.CreatedAt ) select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                partenaires = partenaires.Where(
                    s => s.Name.Contains(searchString) 
                );
            }
            
            int pageSize = 30;
            return View(await PaginatedList<PartnerModel>.CreateAsync(partenaires.AsNoTracking(), pageNumber ?? 1 , pageSize ) );
        }

        // GET: Partenaires/Create
        [Route("nouveau")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Partenaires/Create
        [HttpPost("nouveau")]
        public async Task<IActionResult> Create(PartnerModel partnerModel, IFormFile Image )
        {
            if (ModelState.IsValid && Image != null ) 
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                string filePath = Path.Combine(directoryPath, fileName);

                using ( var memoryStream = new MemoryStream())
                {
                    await Image.CopyToAsync(memoryStream);
                    // Upload the file if less than 2 MB
                    if (memoryStream.Length < 2097152) {
                        var stream = new FileStream(filePath, FileMode.Create);
                        await Image.CopyToAsync(stream);
                    } else throw new Exception ("Image too big");
                }
                partnerModel.Image =$"{fileName}";
                _context.Partners.Add(partnerModel);
                await _context.SaveChangesAsync();
                return Redirect("/fr/administrateurs/partenaires/"); // Redirect as needed
            } 
            return View( partnerModel );
        }

        // GET: Partenaires/Edit/5
        [Route("modifier/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerModel = await _context.Partners.FindAsync(id);
            if (partnerModel == null)
            {
                return NotFound();
            }
            return View(partnerModel);
        }

        // POST: Partenaires/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("modifier/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UrlLink,Name,Image")] PartnerModel partnerModel)
        {
            Console.WriteLine(partnerModel.Image);
            
            if (id != partnerModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partnerModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartnerModelExists(partnerModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/fr/administrateurs/partenaires/");
            }
            return View(partnerModel);
        }

        // GET: Partenaires/Delete/5
        [Route("supprimer/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerModel = await _context.Partners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partnerModel == null)
            {
                return NotFound();
            }

            return View(partnerModel);
        }

        // POST: Partenaires/Delete/5
        [HttpPost("supprimer/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partnerModel = await _context.Partners.FindAsync(id);
            if (partnerModel != null)
            {
                _context.Partners.Remove(partnerModel);
            }

            await _context.SaveChangesAsync();
            return Redirect("/fr/administrateurs/partenaires/");
        }

        private bool PartnerModelExists(int id)
        {
            return _context.Partners.Any(e => e.Id == id);
        }
    }
}
