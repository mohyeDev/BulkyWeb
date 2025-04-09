using Builky.DataAccess.Data;
using Builky.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builky.DataAccess.Repository
{
    class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _db;
        public ICategoryRespoitory categoryRespoitory { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            categoryRespoitory = new CategoryRepository(_db);
        }



        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
