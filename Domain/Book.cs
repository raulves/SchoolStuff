using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Book
    {
        public int BookId { get; set; }

        [MaxLength(128)]
        [MinLength(1)]
        [Display(Name = "Title", Prompt = "Enter title here...")]
        public string Title { get; set; } = default!;

        [MaxLength(1024)]
        [Display(Name = "Summary:")]
        public string? Summary { get; set; } // it can be empty

        [Display(Name = "Publishing year:")] public int PublishingYear { get; set; }
        
        [Display(Name = "Authored year:")] public int AuthoredYear { get; set; }

        [Display(Name = "Word count:")] public int WordCount { get; set; }

        [Display(Name = "Language:")] public int LanguageId { get; set; }

        public Language? Language { get; set; } // this is null, because in database in Book table there is only LanguageId

        public ICollection<Comment>? Comments { get; set; }

        [Display(Name = "Publisher:")] public int PublisherId { get; set; }

        public Publisher? Publisher
        {
            get;
            set;
        } // from the Post when creating a book, there's only PublisherId referenced, we don't get an object

        public ICollection<BookAuthor>? BookAuthors { get; set; }

        public string BookAuthorsString => GetBookAuthors();


        private string GetBookAuthors()
        {
            var bookAuthors = "";
            if (BookAuthors?.Count > 0)
            {
                foreach (var author in BookAuthors)
                {
                    bookAuthors += author.Author?.FirstLastName + ", ";
                }

                return bookAuthors.Substring(0, bookAuthors.Length - 2);
            }
            else
            {
                return bookAuthors;
            }
        }
    }
}