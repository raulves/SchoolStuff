using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Uploads
{
    public class Upload : PageModel
    {
        private readonly IWebHostEnvironment _env;

        public Upload(IWebHostEnvironment env)
        {
            _env = env;
        }

        [BindProperty]
        public IFormFile FormFile { get; set; } = default!;
        
        public void OnGet()
        {
            
        }
        
        public async Task<ActionResult> OnPost()
        {
            var file = Path.Combine(_env.WebRootPath, "uploads", FormFile.FileName);


            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await FormFile.CopyToAsync(fileStream);
            }

            return RedirectToPage("./Index");
        }
    }
}