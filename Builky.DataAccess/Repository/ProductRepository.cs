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
            var productFromDb = _db.Products.FirstOrDefault(u => u.Id == product.Id);
            if(productFromDb is not null)
            {
                productFromDb.Title = product.Title;
                productFromDb.ISBN = product.ISBN;
                productFromDb.Price = product.Price;
                productFromDb.Price50 = product.Price50;
                productFromDb.ListPrice = product.ListPrice;
                productFromDb.Price100 = product.Price100;
                productFromDb.Description = product.Description;
                productFromDb.CategoryId = product.CategoryId;
                productFromDb.Author = product.Author;

                if(product.ImageUrl is not null)
                {
                    productFromDb.ImageUrl = product.ImageUrl;
                }
            }
        }
    }
}
