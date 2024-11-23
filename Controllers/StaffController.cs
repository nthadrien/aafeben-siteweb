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
    [Route("/{culture}/administrateurs/staff/")]
    public class StaffController : Controller
    {
        private readonly AafebenDbContext _context;
        readonly string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images","staff");

        public StaffController(AafebenDbContext context,IWebHostEnvironment webHost)
        {
            _context = context;
        }

        // GET: staff
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
           
            var staff = from s in _context.Users select s;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                staff = staff.Where( s => 
                    s.Name.Contains(searchString) || s.FrRole.Contains(searchString)
                    || s.EnRole.Contains(searchString)
                );
            }

            ViewData["CurrentFilter"] = "";
            int pageSize = 30;

            return View(await PaginatedList<UserModel>.CreateAsync( staff.AsNoTracking(), pageNumber ?? 1, pageSize) );
        
        }

        // GET: staff/Create
        [Route("nouveau")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: staff/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("nouveau")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create(string Name, string EnRole, string FrRole,string EnBio, string FrBio,string UserName, string Password, IFormFile Image )
        public async Task<IActionResult> Create(UserModel userModel, IFormFile Image )
        {

            if (ModelState.IsValid && Image != null ) 
            {
                try {
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
                        if (memoryStream.Length < 2098152) {
                            var stream = new FileStream(filePath, FileMode.Create);
                            await Image.CopyToAsync(stream);
                            stream.Close();
                        } else {
                            Console.WriteLine("Errrrorrrr_____________");
                            ModelState.AddModelError("Image","L'image doit peser moins de 2 Mo, svp.");
                            return View( userModel );
                        }
                    }

                    userModel.Image = $"{fileName}";

                    _context.Users.Add(userModel);
                    await _context.SaveChangesAsync();
                    return Redirect("/fr/administrateurs/staff/"); // Redirect as needed
                } catch {
                    return View( userModel );
                }
            } 
            return View( userModel );
        }

        // GET: staff/Edit/5
        [Route("modifier/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.Users.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }
            return View(userModel);
        }

        // POST: staff/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("modifier/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,EnRole,FrRole,Image")] UserModel userModel)
        {
            if (id != userModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserModelExists(userModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                return Redirect("/fr/administrateurs/staff/"); // Redirect as needed
            }
            return View(userModel);
        }

        // GET: staff/Delete/5
        [Route("supprimer/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // POST: staff/Delete/5
        [HttpPost("supprimer/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userModel = await _context.Users.FindAsync(id);

            if (userModel != null)
            {
                var filePath = userModel.Image;

                if ( filePath != null )
                {
                    var ImgPath = Path.Combine(directoryPath, filePath );
                    if (System.IO.File.Exists(ImgPath))
                    {
                        System.IO.File.Delete(ImgPath);
                    }
                }
                _context.Users.Remove(userModel);
            }

            await _context.SaveChangesAsync();
            return Redirect("/fr/administrateurs/staff/"); // Redirect as needed
        }

        private bool UserModelExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
