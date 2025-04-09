using BulkyWeb_Razp_Temp.Data;
using BulkyWeb_Razp_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWeb_Razp_Temp.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category category { get; set; }

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }




        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            _db.Categrories.Add(category);
            _db.SaveChanges();
            return RedirectToPage("Index");

        }

      


    }
}
