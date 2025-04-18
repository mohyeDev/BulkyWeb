using Builky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builky.Models.ViewModels
{
    public class ShoppingCartVM
    {

        public IEnumerable<ShoppingCart> shoppingCartList { get; set; }

        public OrderHeader OrderHeader { get; set; }
    }
}
