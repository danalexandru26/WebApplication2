using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Commands
{
    public record ShipOrderCartCommands
    {
        public ShipOrderCartCommands(IReadOnlyCollection<UnvalidatedOrderCartProduct> products)
        {
            InputProducts = products;
        }
        public IReadOnlyCollection<UnvalidatedOrderCartProduct> InputProducts { get; }
    }
}
