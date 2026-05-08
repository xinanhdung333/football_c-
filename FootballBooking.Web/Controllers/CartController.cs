using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FootballBooking.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[IgnoreAntiforgeryToken]
public class CartController : ControllerBase
{
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] CartRequest request)
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
        {
            return BadRequest(new { success = false, message = "Vui lòng đăng nhập trước" });
        }

        var user = JsonSerializer.Deserialize<User>(userJson);
        if (user == null)
        {
            return BadRequest(new { success = false, message = "Phiên đăng nhập không hợp lệ" });
        }

        try
        {
            await _cartService.AddToCartAsync(user.Id, request.ServiceId, request.Quantity);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update([FromBody] CartUpdateRequest request)
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
        {
            return BadRequest(new { success = false, message = "Vui lòng đăng nhập trước" });
        }

        var user = JsonSerializer.Deserialize<User>(userJson);
        if (user == null)
        {
            return BadRequest(new { success = false, message = "Phiên đăng nhập không hợp lệ" });
        }

        try
        {
            await _cartService.UpdateCartItemQuantityAsync(user.Id, request.CartItemId, request.Quantity);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("remove")]
    public async Task<IActionResult> Remove([FromBody] CartRemoveRequest request)
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
        {
            return BadRequest(new { success = false, message = "Vui lòng đăng nhập trước" });
        }

        var user = JsonSerializer.Deserialize<User>(userJson);
        if (user == null)
        {
            return BadRequest(new { success = false, message = "Phiên đăng nhập không hợp lệ" });
        }

        try
        {
            await _cartService.RemoveCartItemAsync(user.Id, request.CartItemId);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    public class CartRequest
    {
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartUpdateRequest
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartRemoveRequest
    {
        public int CartItemId { get; set; }
    }
}
