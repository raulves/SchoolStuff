using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.Authors
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        // public IList<Author> Author { get;set; }

        public SelectList BookIdSelectList { get; set; } = default!;

        [BindProperty] public BookAuthor BookAuthor { get; set; } = default!;
        
        public string FirstNameSort { get; set; } = default!;
        public string LastNameSort { get; set; } = default!;
        public string YearOfBirthSort { get; set; } = default!;
        public string CurrentFilter { get; set; } = default!;
        public string CurrentSort { get; set; } = default!;

        public PaginatedList<Author> Author { get; set; } = default!;

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            BookIdSelectList = new SelectList(_context.Books, nameof(Book.BookId), nameof(Book.Title));

            CurrentSort = sortOrder;
            FirstNameSort = String.IsNullOrEmpty(sortOrder) ? "firstname_asc" : "";
            LastNameSort = String.IsNullOrEmpty(sortOrder) ? "lastname_desc" : "";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            
            CurrentFilter = searchString;

            IQueryable<Author> authorsIQ = from a in _context.Authors select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                authorsIQ = authorsIQ.Where(s => s.FirstName.ToLower().Contains(searchString.ToLower()) ||
                                                 s.LastName.ToLower().Contains(searchString.ToLower()));
            }
            
            switch (sortOrder)
            {
                case "firstname_asc":
                    authorsIQ = authorsIQ.OrderBy(s => s.FirstName);
                    break;
                case "lastname_desc":
                    authorsIQ = authorsIQ.OrderByDescending(s => s.LastName);
                    break;
                default:
                    authorsIQ = authorsIQ.OrderBy(s => s.DOB);
                    break;
            }
            
            /*
            var authorQuery = _context.Authors
                .Include(b => b.AuthorBooks)
                .ThenInclude(b => b.Book)
                .AsQueryable();
            */

            int pageSize = 3;
            Author = await PaginatedList<Author>.CreateAsync(authorsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BookAuthors.Add(BookAuthor);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        
        }
    }
}
