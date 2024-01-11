using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApplication2.Models
{
    public partial class CartItem
    {
        public int CartItemId { get; set; }
        public float? Amount { get; set; }
        public int? CartId { get; set; }
        public float? Price { get; set; }
        public int? ProductId { get; set; }

        [JsonIgnore]
        public virtual Cart? Cart { get; set; }

        [JsonIgnore]
        public virtual Product? Product { get; set; }
    }
}
