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
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _db;
        public ICategoryRespoitory categoryRespoitory { get; private set; }

        public IProductRepository productRepository { get; private set; }

        public ICompanyReositiory companyReositiory { get; private set; }


        public IShoppingCartRepository ShoppingCartRepository { get; private set; }

        public IApplicationUserRepository applicationUserRepository { get; private set; }

        public IOrderDetailsRespoitory orderDetailsRespoitory { get; private set; }

        public IOrderHeaderRespoitory orderHeaderRespoitory { get; private set; }


        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            categoryRespoitory = new CategoryRepository(_db);
            productRepository = new ProductRepository(_db);
            companyReositiory = new CompanyReositiory(_db);

            ShoppingCartRepository = new ShoppingCartRepository(_db);

            applicationUserRepository = new ApplicationUserRepostiroy(_db);

            orderDetailsRespoitory = new OrderDetailsRespoitory(_db);

            orderHeaderRespoitory = new OrderHeaderRespoitory(_db);
        }



        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
