using FootballBooking.BLL;
using FootballBooking.Models;

namespace FootballBooking.UI;

public partial class CartForm : Form
{
    private CartService _cartService = null!;
    private int _userId;

    public CartForm()
    {
        InitializeComponent();
    }

    public CartForm(CartService cartService, int userId) : this()
    {
        _cartService = cartService;
        _userId = userId;
        LoadCart();
    }

    private async void LoadCart()
    {
        try
        {
            var cartItems = await _cartService.GetCartItemsAsync(_userId);
            listViewCart.Items.Clear();
            decimal totalPrice = 0;

            foreach (var item in cartItems)
            {
                var listItem = new ListViewItem(item.Id.ToString());
                listItem.SubItems.Add(item.FieldName ?? item.ServiceName);
                listItem.SubItems.Add(item.Quantity.ToString());
                listItem.SubItems.Add(item.Price.ToString("C"));
                
                decimal itemTotal = item.Price * item.Quantity;
                listItem.SubItems.Add(itemTotal.ToString("C"));
                
                listViewCart.Items.Add(listItem);
                totalPrice += itemTotal;
            }

            lblTotalPrice.Text = $"Tổng cộng: {totalPrice:C}";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi tải giỏ hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnRemoveItem_Click(object sender, EventArgs e)
    {
        if (listViewCart.SelectedItems.Count > 0)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa mục này khỏi giỏ hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var selectedItem = listViewCart.SelectedItems[0];
                int cartItemId = int.Parse(selectedItem.Text);
                
                // TODO: Call remove from cart service
                MessageBox.Show("Mục đã được xóa khỏi giỏ hàng", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCart();
            }
        }
        else
        {
            MessageBox.Show("Vui lòng chọn một mục để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnUpdateQuantity_Click(object sender, EventArgs e)
    {
        if (listViewCart.SelectedItems.Count > 0)
        {
            try
            {
                int newQuantity = (int)numNewQuantity.Value;
                if (newQuantity <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số lượng lớn hơn 0", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedItem = listViewCart.SelectedItems[0];
                int cartItemId = int.Parse(selectedItem.Text);
                
                // TODO: Call update quantity service
                MessageBox.Show("Số lượng đã được cập nhật", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                numNewQuantity.Value = 1;
                LoadCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("Vui lòng chọn một mục để cập nhật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnCheckout_Click(object sender, EventArgs e)
    {
        if (listViewCart.Items.Count == 0)
        {
            MessageBox.Show("Giỏ hàng trống. Vui lòng thêm sản phẩm trước khi thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        MessageBox.Show("Chuyển đến trang thanh toán", "Tiếp tục thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Information);
        // TODO: Open checkout form
    }

    private void BtnClearCart_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("Bạn có chắc chắn muốn xóa tất cả mục khỏi giỏ hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            // TODO: Call clear cart service
            MessageBox.Show("Giỏ hàng đã được xóa", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadCart();
        }
    }

    private void BtnBack_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
