using Builky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builky.DataAccess.Repository.IRepository
{
    public interface ICompanyReositiory : IRepository<Company>
    {

        void Update(Company company);
    }
}
