using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Author
    {
        public int AuthorId { get; set; }

        [MaxLength(128, ErrorMessage = "{0} can have max {1} characters")] 
        [MinLength(2, ErrorMessage = "{0} can have min {1} characters")] 
        [Display(Name = "Given name", Prompt = "Enter given name...")]

        
        public string FirstName { get; set; } = default!;

        [MaxLength(128, ErrorMessage = "{0} can have max {1} characters")] 
        [MinLength(2, ErrorMessage = "{0} can have min {1} characters")] 
        [Display(Name = "Surname", Prompt = "Enter surname...")]
        public string LastName { get; set; } = default!;

        
        [Display(Name = "Date of birth", Prompt = "Enter surname...")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        public ICollection<BookAuthor>? AuthorBooks { get; set; }


        public string FirstLastName => FirstName.Substring(0, 1) + "." + LastName; // these are not going to database, because they are only getters
        public string LastFirstName => LastName + " " + FirstName;
    }
}