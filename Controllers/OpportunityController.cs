using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aafeben.Data;
using Aafeben.Models;
using Microsoft.AspNetCore.Authorization;

namespace Aafeben.Controllers
{
    [Authorize]
    [Route("/{culture}/administrateurs/opportunites/")]
    public class OpportunityController : Controller
    {
        private readonly AafebenDbContext _context;

        public OpportunityController(AafebenDbContext context)
        {
            _context = context;
        }

        // GET: Opportunity
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
            // return View(await _context.Opportunities.ToListAsync());
            var oppos = from op in  _context.Opportunities.OrderByDescending( o => o.PublishedDate) select op;
            if (!String.IsNullOrEmpty(searchString))
            {
                oppos = oppos.Where(
                    s => s.Job.Contains(searchString) || s.JobDescription.Contains(searchString)
                );
            }
            
            int pageSize = 30;
            return View(await PaginatedList<OpportunityModel>.CreateAsync(oppos.AsNoTracking(), pageNumber ?? 1 , pageSize ) );
        }

        // GET: Opportunity/Create
        [Route("nouvelle")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Opportunity/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("nouvelle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Job,JobDescription,Language,JobRequirements,PublishedDate")] OpportunityModel opportunityModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(opportunityModel);
                await _context.SaveChangesAsync();
                return Redirect("/fr/administrateurs/opportunites/");
            }
            return View(opportunityModel);
        }

        // GET: Opportunity/Edit/5
        [Route("modifier/{id}")]
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
        [HttpPost("modifier/{id}")]
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
                return Redirect("/@opportunityModel.Language/administrateurs/opportunites/");
            }
            return View(opportunityModel);
        }

        // GET: Opportunity/Delete/5
        [HttpGet("supprimer/{id}")]
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
        [HttpPost("supprimer/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var opportunityModel = await _context.Opportunities.FindAsync(id);
            if (opportunityModel != null)
            {
                _context.Opportunities.Remove(opportunityModel);
            }

            await _context.SaveChangesAsync();
            return Redirect("/@opportunityModel.Language/administrateurs/opportunites/");
        }

        private bool OpportunityModelExists(int id)
        {
            return _context.Opportunities.Any(e => e.Id == id);
        }
    }
}
