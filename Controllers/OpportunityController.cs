using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aafeben.Data;
using Aafeben.Models;
using Microsoft.AspNetCore.Authorization;

namespace Aafeben.Controllers
{
    [Authorize]
    [Route("/{culture}/administrateurs/opportunites")]
    public class OpportunityController : Controller
    {
        private readonly AafebenDbContext _context;

        public OpportunityController(AafebenDbContext context)
        {
            _context = context;
        }

        // GET: Opportunity
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // return View(await _context.Opportunities.ToListAsync());
            return View(await _context.Opportunities.ToListAsync());
        }

        // GET: Opportunity/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunityModel = await _context.Opportunities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (opportunityModel == null)
            {
                return NotFound();
            }

            return View(opportunityModel);
        }

        // GET: Opportunity/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Opportunity/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Job,JobDescription,Language,JobRequirements,PublishedDate")] OpportunityModel opportunityModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(opportunityModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(opportunityModel);
        }

        // GET: Opportunity/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunityModel = await _context.Opportunities.FindAsync(id);
            if (opportunityModel == null)
            {
                return NotFound();
            }
            return View(opportunityModel);
        }

        // POST: Opportunity/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Job,JobDescription,Language,JobRequirements,PublishedDate")] OpportunityModel opportunityModel)
        {
            if (id != opportunityModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(opportunityModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpportunityModelExists(opportunityModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(opportunityModel);
        }

        // GET: Opportunity/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunityModel = await _context.Opportunities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (opportunityModel == null)
            {
                return NotFound();
            }

            return View(opportunityModel);
        }

        // POST: Opportunity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var opportunityModel = await _context.Opportunities.FindAsync(id);
            if (opportunityModel != null)
            {
                _context.Opportunities.Remove(opportunityModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OpportunityModelExists(int id)
        {
            return _context.Opportunities.Any(e => e.Id == id);
        }
    }
}
