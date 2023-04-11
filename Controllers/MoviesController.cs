using Microsoft.AspNetCore.Mvc;
using Pagination.Data;
using Pagination.Models;

namespace Pagination.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieContext _context;
        private const int PageSize = 5;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString, int? page)
        {
            var movies = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Name.Contains(searchString) || m.Industry.Contains(searchString) && searchString.Length >= 3);
            }

            ViewBag.SearchString = searchString;

            var totalCount = movies.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / PageSize);
            var currentPage = page ?? 1;
            var offset = (currentPage - 1) * PageSize;

            movies = movies.Skip(offset).Take(PageSize);

            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;

            return View(movies.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}
