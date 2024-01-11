using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Utilities;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PSSCController : ControllerBase
    {
        private ProiectPSSCContext _dbcontext;

        public PSSCController(ProiectPSSCContext _dbcontext)
        {
            this._dbcontext = _dbcontext;
        }

        [HttpGet]
        [Route("Cart")]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                List<Cart> listCart = _dbcontext.Carts
                    .Include(c => c.CartItems)
                    .ToList();

                if (listCart != null && listCart.Count > 0)
                {
                    var cartDtoList = listCart.Select(c => new Cart
                    {
                        CartId = c.CartId,
                        Total = c.Total,
                        CartItems = c.CartItems.Select(ci => new CartItem
                        {
                            CartId = ci.CartId,
                            ProductId = ci.ProductId,
                            CartItemId = ci.CartItemId,
                            Amount = ci.Amount,
                            Price = ci.Price,
                        }).ToList()
                    }).ToList();

                    return Ok(cartDtoList);
                }
                else
                {
                    return Ok("Nu exista cosuri de cumparaturi");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
    /*
    [HttpPost]
    [Route("Cart")]
    public async Task<IActionResult> AddCart([FromBody] Cart cartInput)
    {
        try
        {
            if (cartInput == null)
            {
                return BadRequest("Invalid input for the cart.");
            }

            var cart = new Cart
            {
                Total = cartInput.Total,
                CartItems = cartInput.CartItems?.Select(ci => new CartItem
                {
                    Amount = ci.Amount,
                    Price = ci.Price,
                }).ToList()
            };

            _dbcontext.Carts.Add(cart);
            await _dbcontext.SaveChangesAsync();

            return Ok("Cart added successfully");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    */
}
