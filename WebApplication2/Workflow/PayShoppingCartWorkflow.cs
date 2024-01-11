using WebApplication2.Commands;
using WebApplication2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WebApplication2.Models.ShippedCartEvent;
using static WebApplication2.Models.OrderCartState;
using static WebApplication2.Operations.OrderCartStateOperations;


namespace WebApplication2.Workflow
{
    internal class PayOrderCartWorkflow
    {
        public IShippedCartEvent Execute(ShipOrderCartCommands command, Func<ProductName, bool> checkProductExists)
        {

            EmptyOrderCart emptyOrderCart = new EmptyOrderCart(command.InputProducts);
            IOrderCartState orderCartState = ValidateOrderCart(checkProductExists, emptyOrderCart);
            orderCartState = PayAndOrderProducts(orderCartState);
            orderCartState = BillOrder(orderCartState);
            orderCartState = ShipOrder(orderCartState);

            return orderCartState.Match(
                whenEmptyOrderCart: emptyOrderCart => new IShippedCartFailedEvent("Unexpected result") as IShippedCartEvent,
                whenUnvalidatedOrderCart: unvalidatedOrderCart => new IShippedCartFailedEvent(unvalidatedOrderCart.Reason),
                whenValidatedOrderCart: validatedOrderCart => new IShippedCartFailedEvent("Unexpected result"),
                whenPayedOrderCart: payedOrderCart => new IShippedCartFailedEvent("Unexpected result"),
                whenInvoicedOrderCart: invoicedOrderCart => new IShippedCartFailedEvent("Unexpected result"),
                whenShippedOrderCart: shippedOrderCart => new IShippedCartSuccessfulEvent(shippedOrderCart.CheckStatus)
            );
        }
    }
}
