using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    [AsChoice]
    public static partial class OrderCartState
    {
        public interface IOrderCartState { }
       
        public record EmptyOrderCart : IOrderCartState
        {
            public EmptyOrderCart(IReadOnlyCollection<UnvalidatedOrderCartProduct> products)
            {
                Products = products;
            }

            public IReadOnlyCollection<UnvalidatedOrderCartProduct> Products { get; }
        }

        public record UnvalidatedOrderCart : IOrderCartState
        {
            public UnvalidatedOrderCart(IReadOnlyCollection<UnvalidatedOrderCartProduct> orderCartProducts, string reason)
            {
                OrderCartProducts = orderCartProducts;
                Reason = reason;
            }

            public IReadOnlyCollection<UnvalidatedOrderCartProduct> OrderCartProducts { get; }
            public string Reason { get; }
        }

        public record ValidatedOrderCart : IOrderCartState
        {

            public ValidatedOrderCart(IReadOnlyCollection<ValidatedOrderCartProduct> orderCartProducts)
            {
                OrderCartProducts = orderCartProducts;
            }
            public IReadOnlyCollection<ValidatedOrderCartProduct> OrderCartProducts { get; }
        }

        public record PayedOrderCart : IOrderCartState
        {
            public PayedOrderCart(IReadOnlyCollection<ValidatedOrderCartProduct> orderCartProducts, decimal total)
            {
                OrderCartProducts = orderCartProducts;
                Total = total;
            }

            public IReadOnlyCollection<ValidatedOrderCartProduct> OrderCartProducts { get; }
            public decimal Total { get; }
        }

        
        public record InvoicedOrderCart : IOrderCartState
        {
            public InvoicedOrderCart(bool confirmation)
            {
                ConfirmBilling = confirmation;
            }
            public bool ConfirmBilling { get; }
        }


        public record ShippedOrderCart : IOrderCartState
        {

            public ShippedOrderCart(string statusLivrare)
            {
                CheckStatus = statusLivrare;
            }

            public string CheckStatus { get; }
        }  
    }
}