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
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product product)
        {
            _db.Products.Update(product);
        }
    }
}
