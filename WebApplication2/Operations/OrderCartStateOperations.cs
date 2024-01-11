using WebApplication2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WebApplication2.Models.OrderCartState;

namespace WebApplication2.Operations
{
    public static class OrderCartStateOperations
    {
        public static IOrderCartState ValidateOrderCart(Func<ProductName, bool> checkProductExists, EmptyOrderCart orderCart)
        {
            List<ValidatedOrderCartProduct> validatedProducts = new List<ValidatedOrderCartProduct>();
            bool isValidList = true;
            string invalidReson = string.Empty;
            foreach (var product in orderCart.Products)
            {
                if (!ProductName.TryParseProductName(product.productName, out ProductName productName))
                {
                    invalidReson = $"Invalid product name: {product.productName}";
                    isValidList = false;
                    break;
                }
                if (!ProductPrice.TryParseProductPrice(product.price, out ProductPrice productPrice))
                {
                    invalidReson = $"Invalid product price: {product.price}";
                    isValidList = false;
                    break;
                }
                ValidatedOrderCartProduct validatedOrderCartProduct = new(productName, productPrice);
                validatedProducts.Add(validatedOrderCartProduct);
            }

            if (isValidList)
            {
                Console.WriteLine($"Products successfully validated - ValidateOrderCart");
                return new ValidatedOrderCart(validatedProducts);
            }
            else
            {
                return new UnvalidatedOrderCart(orderCart.Products, invalidReson);
            }
        }

        public static IOrderCartState PayAndOrderProducts(IOrderCartState orderCart) =>
            orderCart.Match(
                whenEmptyOrderCart: emptyOrderCart => emptyOrderCart,
                whenUnvalidatedOrderCart: unvalidatedCardCart => unvalidatedCardCart,
                whenValidatedOrderCart: validatedOrderCart => 
                {
                    decimal total = 0;
                    foreach (var product in validatedOrderCart.OrderCartProducts)
                    {
                        total += product.price.Price;
                    }

                    Console.WriteLine($"Order Price: {total} - PayAndOrderProducts");
                    PayedOrderCart payedShoppingCart = new(validatedOrderCart.OrderCartProducts, total);
                    return payedShoppingCart;
                },

                whenPayedOrderCart: payedOrderCart => payedOrderCart,
                whenInvoicedOrderCart: invoicedOrderCart => invoicedOrderCart,
                whenShippedOrderCart: shippedOrderCart => shippedOrderCart
                );
        public static IOrderCartState BillOrder(IOrderCartState orderCart) =>
            orderCart.Match(
                whenEmptyOrderCart: emptyOrderCart => emptyOrderCart,
                whenUnvalidatedOrderCart: unvalidatedCardCart => unvalidatedCardCart,
                whenValidatedOrderCart: validatedOrderCart => validatedOrderCart,

                whenPayedOrderCart: payedOrderCart =>
                {
                    var total = payedOrderCart.Total;
                    var confirmation = false;
                    if (total > 0)
                    {
                        confirmation = true;
                    }

                    Console.WriteLine($"Order was successfully billed: {confirmation} - BillOrder");
                    InvoicedOrderCart invoicedOrder = new(confirmation);
                    return invoicedOrder;
                },

                whenInvoicedOrderCart: invoicedOrderCart => invoicedOrderCart,
                whenShippedOrderCart: shippedOrderCart => shippedOrderCart
                );

        public static IOrderCartState ShipOrder(IOrderCartState orderCart) =>
            orderCart.Match(
                whenEmptyOrderCart: emptyOrderCart => emptyOrderCart,
                whenUnvalidatedOrderCart: unvalidatedCardCart => unvalidatedCardCart,
                whenValidatedOrderCart: validatedOrderCart => validatedOrderCart,
                whenPayedOrderCart: payedOrderCart => payedOrderCart,

                whenInvoicedOrderCart: invoicedOrderCart => {

                    var isStatusReady = "The Order has failed to be shipped - ShipOrder";
                    var confirmation = invoicedOrderCart.ConfirmBilling;
                    if (confirmation == true)
                    {
                        isStatusReady = "Order successfully shipped - ShipOrder";
                    }

                    ShippedOrderCart shippedOrder = new(isStatusReady);
                    return shippedOrder;
                },

                whenShippedOrderCart: shippedOrderCart => shippedOrderCart
                );
    }
}
