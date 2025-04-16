using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Builky.DataAccess.Data;
using Builky.DataAccess.Repository.IRepository;
using Builky.Models.Models;

namespace Builky.DataAccess.Repository
{
    public class ApplicationUserRepostiroy : Repository<ApplicationUser>, IApplicationUserRepository
    {

        private readonly ApplicationDbContext _db;
        public ApplicationUserRepostiroy(ApplicationDbContext db) : base(db)
        {

            _db = db;
        }
    }
}
