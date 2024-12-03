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
    [Route("/{culture}/administrateurs/messages/")]
    public class MessagesController : Controller
    {
        private readonly AafebenDbContext _context;

        public MessagesController(AafebenDbContext context)
        {
            _context = context;
        }

        // GET: Messages
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Messages.ToListAsync());
        }
        
        // GET: Messages/Delete/5
        [Route("supprimer/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messageModel = await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageModel == null)
            {
                return NotFound();
            }

            return View(messageModel);
        }

        // POST: Messages/Delete/5
        [HttpPost("supprimer/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var messageModel = await _context.Messages.FindAsync(id);
            if (messageModel != null)
            {
                _context.Messages.Remove(messageModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageModelExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}
