using Builky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builky.DataAccess.Repository.IRepository
{
    public  interface ICategoryRespoitory : IRepository<Category>
    {

        void Update(Category category);
        void save();
    }
}
