using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Language
    {
        [Display(Name = "Language")]
        public int LanguageId { get; set; }
        
        [Display(Name = "Language")]
        public string LanguageName { get; set; } = default!;
        public ICollection<Book>? Books { get; set; }  // it can be null
    }
}