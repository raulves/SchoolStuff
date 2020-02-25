using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Comment
    {
        [Display(Name = "Comment:")]
        public int CommentId { get; set; }
        
        [MaxLength(1024, ErrorMessage = "{0} can have max {1} characters. Stop writing!")]
        [MinLength(10, ErrorMessage = "{0} can have min {1} characters. You can do better!")]
        [Display(Name = "Comment")]
        public string CommentText { get; set; } = default!;

        public int BookId { get; set; }
        public Book? Book { get; set; }
    }
}