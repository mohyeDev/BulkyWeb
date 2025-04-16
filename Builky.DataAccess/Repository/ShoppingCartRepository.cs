using Builky.DataAccess.Data;
using Builky.DataAccess.Repository.IRepository;
using Builky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Builky.DataAccess.Repository
{
    public class ShoppingCartRepository :  Repository<ShoppingCart>  , IShoppingCartRepository
    {

        private readonly ApplicationDbContext _db;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        
        public void Update(ShoppingCart shoppingCart)
        {
            _db.shoppingCarts.Update(shoppingCart);
        }
    }
}
