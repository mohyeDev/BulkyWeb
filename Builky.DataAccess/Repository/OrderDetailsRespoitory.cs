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
    public class OrderDetailsRespoitory :  Repository<OrderDetail>  , IOrderDetailsRespoitory
    {

        private readonly ApplicationDbContext _db;

        public OrderDetailsRespoitory(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        

        public void Update(OrderDetail orderDetail)
        {
            _db.orderDetails.Update(orderDetail);
        }
    }
}
