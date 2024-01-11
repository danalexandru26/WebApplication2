using System;
using System.Collections.Generic;

namespace WebApplication2.Models
{
    public partial class Cart
    {
        public Cart()
        {
            CartItems = new HashSet<CartItem>();
        }

        public int CartId { get; set; }
        public float? Total { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
