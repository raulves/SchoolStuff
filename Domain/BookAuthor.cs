using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class BookAuthor
    {
        public int BookAuthorId { get; set; }

        [Display(Name = "Book:")]
        public int BookId { get; set; }
        public Book? Book { get; set; }

        [Display(Name = "Author:")]
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}