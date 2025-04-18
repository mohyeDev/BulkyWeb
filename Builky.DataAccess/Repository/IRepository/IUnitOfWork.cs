using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builky.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRespoitory categoryRespoitory { get; }
        IProductRepository productRepository { get; }

        ICompanyReositiory companyReositiory { get; }

        IShoppingCartRepository ShoppingCartRepository { get; }

        IApplicationUserRepository applicationUserRepository { get; }

        IOrderDetailsRespoitory orderDetailsRespoitory { get; }

        IOrderHeaderRespoitory orderHeaderRespoitory { get; }

        void Save();
    }
}
