using System;
using System.Collections.Generic;

namespace WebApplication2.Models
{
    public partial class Product
    {
        public Product()
        {
            CartItems = new HashSet<CartItem>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public float? Stock { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
