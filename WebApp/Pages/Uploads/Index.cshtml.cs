using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace WebApp.Pages.Uploads
{
    public class Index : PageModel
    {
        private readonly IWebHostEnvironment _env;

        public Index(IWebHostEnvironment env)
        {
            _env = env;
        }

        public List<string> FileNames { get; set; } = new List<string>();

        public ActionResult OnGet()
        {
            string webRootPath = _env.WebRootPath + "/uploads";

            if (System.IO.Directory.Exists(webRootPath))
            {
                string [] fileEntries = Directory.GetFiles(webRootPath);
                foreach (string fullPathFileName in fileEntries)
                {
                    var fileName = Path.GetFileName(fullPathFileName);
                    var fileExtension = Path.GetExtension(fileName).ToLower();
                    if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".gif")
                    {
                        FileNames.Add(fileName);
                    }

                }
            }
            else
            {
                FileNames.Add("Directory not found!");
            }

            return Page();
        }
    }
}