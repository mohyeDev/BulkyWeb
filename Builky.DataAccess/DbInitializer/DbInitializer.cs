using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Builky.DataAccess.Data;
using Builky.Models.Models;
using Builky.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Builky.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(UserManager<IdentityUser> userManager , RoleManager<IdentityRole> roleManager , ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public void Initialize()
        {



            // Migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0) {

                    _db.Database.Migrate();
                }

                else { }
            }

            catch(Exception ex )
            {

            }

            //Create Roles If They are not Created
            if (!_roleManager.RoleExistsAsync(SD.Role_User_Cust).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Cust)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Comp)).GetAwaiter().GetResult();

                //if Roles are not Create , Thne We Will Create Admin User As Well 


                _userManager.CreateAsync(new ApplicationUser
                {

                    UserName = "Admin",
                    Email = "Admin@Admin.com",
                    Name = "Admin",
                    PhoneNumber = "01122334455",
                    StreetAddress = "Admin",
                    State = "Admin",
                    PostalCode = "23422",
                    City = "Admin"
                }, "Admin123*").GetAwaiter().GetResult();


                ApplicationUser user = _db.applicationUsers.FirstOrDefault(u => u.Email == "Admin@Admin.com");

                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
