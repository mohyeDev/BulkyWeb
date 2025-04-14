using Builky.DataAccess.Data;
using Builky.DataAccess.Repository.IRepository;
using Builky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builky.DataAccess.Repository
{
    public class CompanyReositiory : Repository<Company> , ICompanyReositiory
    {

        private readonly ApplicationDbContext _db;

        public CompanyReositiory(ApplicationDbContext db) : base(db)
        {
            _db = db; 
        }

        public void Update(Company company)
        {
            _db.companies.Update(company);
        }
    }


    
}
