using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain;
using WebApp.DTO;

namespace WebApp.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        // public IList<Book> Book { get;set; } = default!;

        public IList<BookIndexDto> Books { get; set; } = default!;
        
        [BindProperty]
        public Comment Comment { get; set; } = default!;
        public string? Search { get; set; }
        
        public async Task OnGetAsync(string? search, string? toDoActionReset, int? authorId)
        {
            if (toDoActionReset == "Reset")
            {
                Search = "";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(search))
                {
                    Search = search.ToLower().Trim();
                }
            }

            var bookQuery = _context.Books
                .Include(b => b.Language)
                .Include(b => b.Publisher)
                .Include(b => b.BookAuthors)
                .ThenInclude(a => a.Author)
                .Include(b => b.Comments)
                .Select(a => new BookIndexDto()
                {
                    Book = a, 
                    CommentCount =  a.Comments!.Count, // 
                    LastComment = "" // a.Comments.LastOrDefault().CommentText
                })

                .AsQueryable(); // saving it as querable, it's not finished query yet, nothing is executed yet
            
            if (!string.IsNullOrWhiteSpace(Search))
            {
                bookQuery = bookQuery
                    .Where(b =>
                        b.Book.Title.ToLower().Contains(Search) ||
                        b.Book.Summary!.ToLower().Contains(Search) ||
                        b.Book.Publisher!.PublisherName.ToLower().Contains(Search) ||
                        b.Book.BookAuthors.Any(a =>
                            a.Author!.FirstName.ToLower().Contains(Search) ||
                            a.Author.LastName.ToLower().Contains(Search)));
            }

            if (authorId != null)
            {
                bookQuery = bookQuery.Where(b => b.Book.BookAuthors.Any(a => a.AuthorId == authorId));
            }

            bookQuery = bookQuery.OrderBy(b => b.Book.Title);
            Books = await bookQuery.ToListAsync();
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Comments.Add(Comment);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
