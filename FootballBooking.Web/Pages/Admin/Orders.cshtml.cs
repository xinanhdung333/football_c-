using FootballBooking.BLL;
using FootballBooking.DAL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace FootballBooking.Web.Pages.Admin;

public class OrdersModel : PageModel
{
    private readonly CheckoutService _checkoutService;
    private readonly UserRepository _userRepository;

    public OrdersModel(CheckoutService checkoutService, UserRepository userRepository)
    {
        _checkoutService = checkoutService;
        _userRepository = userRepository;
    }

    public User? CurrentUser { get; set; }
    public IEnumerable<OrderListViewModel> Orders { get; set; } = new List<OrderListViewModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        if (!TryLoadCurrentUser())
            return RedirectToPage("/Account/Login");

        await LoadOrdersAsync();
        return Page();
    }

    public async Task<IActionResult> OnGetExportExcelAsync()
    {
        if (!TryLoadCurrentUser())
            return RedirectToPage("/Account/Login");

        await LoadOrdersAsync();

        var csv = new StringBuilder();
        csv.AppendLine("Order Id,Khách hàng,Email,Tổng tiền,Phương thức,Trạng thái,Ngày tạo");

        foreach (var order in Orders)
        {
            csv.AppendLine($"{order.Id},\"{order.UserName}\",\"{order.UserEmail}\",{order.TotalAmount:N0},\"{order.PaymentMethod}\",\"{order.Status}\",{order.CreatedAt:dd/MM/yyyy HH:mm}");
        }

        var bytes = Encoding.UTF8.GetBytes(csv.ToString());
        return File(bytes, "text/csv", "DanhSachDonHang.csv");
    }

    private bool TryLoadCurrentUser()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
            return false;

        CurrentUser = JsonSerializer.Deserialize<User>(userJson);
        if (CurrentUser == null || (CurrentUser.Role != UserRole.Admin && CurrentUser.Role != UserRole.Boss))
            return false;

        return true;
    }

    public async Task<IActionResult> OnPostUpdateStatusAsync(int orderId, OrderStatus status)
    {
        if (!TryLoadCurrentUser())
            return RedirectToPage("/Account/Login");

        await _checkoutService.UpdateOrderStatusAsync(orderId, status);
        await LoadOrdersAsync();
        return Page();
    }

    private async Task LoadOrdersAsync()
    {
        var orders = await _checkoutService.GetAllOrdersAsync();
        var userCache = new Dictionary<int, User>();
        var orderList = new List<OrderListViewModel>();

        foreach (var order in orders)
        {
            if (!userCache.TryGetValue(order.UserId, out var user))
            {
                user = await _userRepository.GetUserByIdAsync(order.UserId);
                if (user == null)
                {
                    continue;
                }
                userCache[order.UserId] = user;
            }

            orderList.Add(new OrderListViewModel
            {
                Id = order.Id,
                UserName = user.Name,
                UserEmail = user.Email,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.PaymentMethod ?? "Chưa xác định",
                Status = order.Status,
                CreatedAt = order.CreatedAt ?? DateTime.MinValue
            });
        }

        Orders = orderList;
    }

    public class OrderListViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
