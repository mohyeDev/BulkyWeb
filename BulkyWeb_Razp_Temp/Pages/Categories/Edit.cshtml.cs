using BulkyWeb_Razp_Temp.Data;
using BulkyWeb_Razp_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWeb_Razp_Temp.Pages.Categories
{
    public class EditModel : PageModel
    {

        public ApplicationDbContext _db;
        [BindProperty]
        public Category category { get; set; }
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
        }


        public IActionResult OnEdit()
        {
            _db.Categrories.Update(category);
            _db.SaveChanges();
            return RedirectToPage("Index");

        }
    }
}
