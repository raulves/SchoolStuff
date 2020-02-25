using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Publisher
    {
        public int PublisherId { get; set; }
        
        [Display(Name = "Publisher name")]
        [MaxLength(128)] 
        [MinLength(1)]
        public string PublisherName { get; set; } = default!;
        
        public ICollection<Book>? Books { get; set; } // this is a nullable , kui loome netis Publisher, siis server saab ainult publisheri nime, Books ei saa
    }
}