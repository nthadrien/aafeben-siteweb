
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aafeben.Data;
using Aafeben.Models;
using Microsoft.AspNetCore.Authorization;

namespace Aafeben.Controllers
{
    [Authorize]
    [Route("/{culture}/administrateurs/vitrine")]
    public class GrenierController : Controller
    {
        private readonly AafebenDbContext _context;
        private readonly string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images","grenier");

        public GrenierController(AafebenDbContext context)
        {
            _context = context;
        }

        // GET: Grenier
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
            var products = from p in _context.Products select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.FrName.Contains(searchString)
                    || s.EnName.Contains(searchString) || s.Address.Contains(searchString)
                );
            }
            
            int pageSize = 30;
            return View(
                await PaginatedList<ProductModel>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1 , pageSize ) 
            );
        }


        // GET: Grenier/Create
        [Route("nouveau")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Grenier/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("nouveau")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FrName,EnName,EnDescription, FrDescription, Contact,Address,Qty")] ProductModel productModel, IFormFile Image )
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
                        await Image.CopyToAsync(memoryStream); // Upload the file if less than 2 MB
                        if (memoryStream.Length <  1676160 ) {
                            var stream = new FileStream(filePath, FileMode.Create);
                            await Image.CopyToAsync(stream);
                            stream.Close();
                        } else {
                            ModelState.AddModelError("Image","L'image doit peser moins de 1,5 Mo, svp.");
                            return View( productModel );
                        }
                    }

                productModel.Image = $"{fileName}";
                _context.Add(productModel);
                await _context.SaveChangesAsync();
                return Redirect("/fr/administrateurs/vitrine/");
            }
            return View(productModel);
        }

        // GET: Grenier/Edit/5
        [Route("modifier/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Products.FindAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }
            return View(productModel);
        }

        // POST: Grenier/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("modifier/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FrName,EnName,Contact,EnDescription,Qty, FrDescription,Address, Image")] ProductModel productModel)
        {
            if (id != productModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductModelExists(productModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/fr/administrateurs/vitrine/");
            }
            return View(productModel);
        }

        // GET: Grenier/Delete/5
        [Route("supprimer/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        // POST: Grenier/Delete/5
        [HttpPost("supprimer/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productModel = await _context.Products.FindAsync(id);
            if (productModel != null)
            {
                var filePath = productModel.Image;

                if ( filePath != null )
                {
                    var ImgPath = Path.Combine(directoryPath, filePath );
                    if (System.IO.File.Exists(ImgPath))
                    {
                        System.IO.File.Delete(ImgPath);
                    }
                }
                _context.Products.Remove(productModel);
            }

            await _context.SaveChangesAsync();
            return Redirect("/fr/administrateurs/vitrine/");
        }

        private bool ProductModelExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
