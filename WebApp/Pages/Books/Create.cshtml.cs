using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Books
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public SelectList LanguageIdSelectList { get; set; } = default!;
        public SelectList PublisherIdSelectList { get; set; } = default!;

        public SelectList AuthorIdSelectList { get; set; } = default!;
        
        [BindProperty]
        [Display(Name = "Publisher name", Prompt = "Please enter name for new publisher...")]
        [MinLength(2)]
        [MaxLength(128)]
        public string? NewPublisherName { get; set; }


        public async Task<IActionResult> OnGet()
        {
            LanguageIdSelectList = new SelectList(await _context.Languages.ToListAsync(), nameof(Language.LanguageId),
                nameof(Language.LanguageName));
            AuthorIdSelectList =
                new SelectList(await _context.Authors.ToListAsync(), nameof(Author.AuthorId), nameof(Author.FirstLastName));
            PublisherIdSelectList = new SelectList(await _context.Publishers.ToListAsync( ), nameof(Publisher.PublisherId),
                nameof(Publisher.PublisherName));
            return Page();
        }

        [BindProperty] public Book Book { get; set; } = default!;

        public BookAuthor BookAuthor { get; set; } = default!;

        /*
        [BindProperty]
        public Language Language { get; set; }
        */

        [BindProperty]
        public IEnumerable<int> BookAuthorIds { get; set; } = default!;
        
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            if (Book.PublisherId == -1 && string.IsNullOrWhiteSpace(NewPublisherName))
            {
                ModelState.AddModelError(nameof(NewPublisherName), "Publisher name is required");
                return Page();
            }

            if (Book.PublisherId == -1 && !string.IsNullOrWhiteSpace(NewPublisherName))
            {
                var publisher = new Publisher() {PublisherName = NewPublisherName.Trim()};
                _context.Publishers.Add(publisher);
                Book.Publisher = publisher;
            }


            _context.Books.Add(Book);
            await _context.SaveChangesAsync();

            foreach (var authorId in BookAuthorIds)
            {
                BookAuthor bookAuthor = new BookAuthor
                {
                    BookId = Book.BookId, 
                    AuthorId = authorId
                };
                _context.BookAuthors.Add(bookAuthor);
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}