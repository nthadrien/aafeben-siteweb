using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Aafeben.Models;
using Aafeben.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Aafeben.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly AafebenDbContext _context;
    public HomeController(ILogger<HomeController> logger, AafebenDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    // [Route("a-propos", Name = "AboutFr")] // French version
    // [Route("acerca", Name = "AboutEs")]
    // [Route("{culture}/contacts", Name="contacts")]
    [Route("/{culture}/contacts")]
    public IActionResult Contacts ()
    {
        return View();
    }

    [Route("/{culture}/a-propos-de-nous")]
    [Route("/{culture}/about-us")]
    public async Task<IActionResult> AProposDeNous ( )
    {
        var Staff = await _context.Users.ToListAsync();
        var Patners = await _context.Partners.ToListAsync(); 
        ViewBag.Language = CultureInfo.CurrentCulture.ToString();
  
        var model = new AboutCombinedModelDto
        {
            Users = Staff,
            Partners = Patners
        };
        return View(model);
    }

    // Blogs sections
    [Route("/{culture}/blogs")]
    public async Task<IActionResult> Blogs (int? Page, string? searchString)
    {
        var cult = CultureInfo.CurrentCulture.Name.ToString();
        var Blogs = from s in _context.BlogPosts.Where(
            s => s.Language == cult
        ).OrderByDescending(s=> s.PublishedDate) select s;

        if (!String.IsNullOrEmpty(searchString))
        {
            Blogs = Blogs.Where( s => 
                s.Title.Contains(searchString) 
                || s.ShortDescription.Contains(searchString)
            );
        }

        int pageSize = 30;
        return View(await PaginatedList<BlogPostModel>.CreateAsync( Blogs.AsNoTracking(), Page ?? 1, pageSize) );
    }

    [Route("/{culture}/blog/{id}")]
    public async Task<IActionResult> Blog (string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var blogPostModel = await _context.BlogPosts
            .FirstOrDefaultAsync(m => m.Title == id);

        if (blogPostModel == null)
        {
            return NotFound();
        }
        return View(blogPostModel);
    }

    // Publications Section
    [Route("/{culture}/publications")]
    public async Task<IActionResult> Publications (
        int? pageNumber
    )
    {
        var cult = CultureInfo.CurrentCulture.Name;
        var publications = from p in _context.Publications.OrderByDescending(s => s.PublishedDate).Where(
            s => s.Language == cult
        ) select p;

        int pageSize = 30;
        return View(await PaginatedList<PublicationModel>.CreateAsync(publications.AsNoTracking(), pageNumber ?? 1 , pageSize ) );
    }

    [Route("/{culture}/publication/{title}")]
    public async Task<IActionResult> Publication (string? title)
    {
        if (title == null)
        {
            return NotFound();
        }

        var publicationModel = await _context.Publications.FirstOrDefaultAsync(s => s.Title == title);
        if (publicationModel == null)
        {
            return NotFound();
        }
        return View(publicationModel);
    }

    // Projets Section
    [Route("/{culture}/projects")]
    [Route("/{culture}/projets")]
    public async Task<IActionResult> Projets (
        int? pageNumber
    )
    {
        var cult = CultureInfo.CurrentCulture.Name;
        var projets = from p in _context.Projets.OrderByDescending(s => s.EndDate).Where(
            s => s.Language == cult
        ) select p;

        int pageSize = 30;
        return View(await PaginatedList<ProjectModel>.CreateAsync(projets.AsNoTracking(), pageNumber ?? 1 , pageSize ) );
    }

    [Route("/{culture}/project/{title}")]
    [Route("/{culture}/projet/{title}")]
    public async Task<IActionResult> Projet (string? title)
    {
        if (title == null)
        {
            return NotFound();
        }

        var publicationModel = await _context.Projets.FirstOrDefaultAsync(m => m.Title == title);
        if (publicationModel == null)
        {
            return NotFound();
        }  
        return View(publicationModel);
    }


    [Route("/{culture}/opportunities")]
    [Route("/{culture}/opportunités")]
    public async Task<IActionResult> Opportunites()
    {
        var cult = CultureInfo.CurrentCulture.ToString();
        return View(await _context.Opportunities.Where( s => s.Language.ToLower() == cult ).OrderByDescending( s => s.PublishedDate ).ToListAsync());
    }


    // Lee grenier or Granary
    // "/en/granary": "/fr/le-grenier";
    [Route("/{culture}/granary")]
    [Route("/{culture}/le-grenier")]
    public async Task<IActionResult> LeGrenier (
            int? pageNumber
    )
    {
        var products = from p in _context.Products select p;   
        int pageSize = 30;
        return View(
            await PaginatedList<ProductModel>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1 , pageSize ) 
        );
    }

    // Media Tech 
    [Route("/{culture}/médiathèque")]
    [Route("/{culture}/media-library")]
    public async Task<IActionResult> MediaTech (
            int? pageNumber
    )
    {
        var media = from p in _context.Medias select p;   
        int pageSize = 30;
        return View(
            await PaginatedList<MediaModel>.CreateAsync(media.AsNoTracking(), pageNumber ?? 1 , pageSize ) 
        );
    }

    // public IActionResult MediaTech ()
    // {
    //     return View();
    // }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    // authentications 
    [Route("/{culture}/administrateurs")]
    public IActionResult Login ( )
    {
        return View();
    }

    [HttpPost("/{culture}/administrateurs")]
    public async Task<IActionResult> Login( LoginDto loginDto, string? ReturnUrl )
    {
        string culture = CultureInfo.CurrentCulture.ToString();


        if ( loginDto.Username == "admin aafeben" && loginDto.Password == "H05RtP5E0Iq3k1N5xMhXwsGS0jU"  )
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginDto.Username ),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(240),
                IsPersistent = true // Remember the user
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties );
            return Redirect(ReturnUrl ?? $"/{culture}/administrateurs/blogs");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(loginDto);
    }
    
    [HttpPost]    
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index","Home");
    }
}
