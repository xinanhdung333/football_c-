/**
 * HƯỚNG DẪN TÍCH HỢP CÁC FORM MỚI VÀO WINFORM
 * 
 * Đã tạo 6 form mới để hoàn thiện WinForm:
 * 1. BookingsForm - Lịch sử đặt sân
 * 2. FeedbackForm - Quản lý phản hồi
 * 3. FieldsForm - Danh sách sân bóng
 * 4. ServicesForm - Danh sách dịch vụ
 * 5. CartForm - Giỏ hàng
 * 6. ChatbotForm - Trợ lý AI Chatbot
 * 
 * MỖI FORM CÓ CẤU TRÚC:
 * - [FormName].cs - Chứa logic chính
 * - [FormName].Designer.cs - Chứa thiết kế UI (có thể edit trong Visual Designer)
 * 
 * CÓ THỂ MỞ DESIGNER BẰNG:
 * - Chuột phải vào file .cs -> View Designer
 * - Hoặc kéo file vào designer view
 * - Edit các control và layout trực tiếp
 * 
 * ═══════════════════════════════════════════════════════════════
 * CÁC BƯỚC TÍCH HỢP VÀO MAINFORM
 * ═══════════════════════════════════════════════════════════════
 * 
 * 1. MỞ MainForm.Designer.cs và thêm các nút menu:
 * 
 *    btnBookings = new Button();
 *    btnFeedback = new Button();
 *    btnFields = new Button();
 *    btnServices = new Button();
 *    btnCart = new Button();
 *    btnChatbot = new Button();
 * 
 * 2. CẤU HÌNH BUTTON VÀ EVENT:
 * 
 *    // Trong MainForm.cs
 *    private BookingService _bookingService;
 *    private FeedbackService _feedbackService;
 *    private FieldManagementService _fieldService;
 *    private ServiceInventoryService _serviceService;
 *    private CartService _cartService;
 * 
 *    private void BtnBookings_Click(object sender, EventArgs e)
 *    {
 *        BookingsForm form = new BookingsForm(_bookingService, CurrentUser.UserId);
 *        form.ShowDialog();
 *    }
 * 
 *    private void BtnFeedback_Click(object sender, EventArgs e)
 *    {
 *        FeedbackForm form = new FeedbackForm(_feedbackService, CurrentUser.UserId);
 *        form.ShowDialog();
 *    }
 * 
 *    private void BtnFields_Click(object sender, EventArgs e)
 *    {
 *        FieldsForm form = new FieldsForm(_fieldService);
 *        form.ShowDialog();
 *    }
 * 
 *    private void BtnServices_Click(object sender, EventArgs e)
 *    {
 *        ServicesForm form = new ServicesForm(_serviceService);
 *        form.ShowDialog();
 *    }
 * 
 *    private void BtnCart_Click(object sender, EventArgs e)
 *    {
 *        CartForm form = new CartForm(_cartService, CurrentUser.UserId);
 *        form.ShowDialog();
 *    }
 * 
 *    private void BtnChatbot_Click(object sender, EventArgs e)
 *    {
 *        ChatbotForm form = new ChatbotForm();
 *        form.ShowDialog();
 *    }
 * 
 * 3. CẤU HÌNH LAYOUT:
 *    - Thêm các button vào menu bar hoặc tạo toolbar
 *    - Căn chỉnh vị trí theo thiết kế
 *    - Set font, size, color phù hợp
 * 
 * ═══════════════════════════════════════════════════════════════
 * TÍNH NĂNG CỦA MỖI FORM
 * ═══════════════════════════════════════════════════════════════
 * 
 * BOOKINGSFORM:
 * - Xem lịch sử đặt sân của người dùng
 * - Hủy đặt sân
 * - Làm mới danh sách
 * - Cột: ID, Tên sân, Ngày đặt, Thời gian, Giá, Trạng thái
 * 
 * FEEDBACKFORM:
 * - Xem phản hồi đã gửi
 * - Thêm phản hồi mới cho booking
 * - Nhập booking ID, chọn đánh giá (1-5 sao), viết bình luận
 * - Xem câu trả lời từ admin
 * - Cột: ID, Booking ID, Đánh giá, Bình luận, Ngày tạo, Trả lời
 * 
 * FIELDSFORM:
 * - Xem danh sách sân bóng
 * - Xem chi tiết sân
 * - Đặt sân
 * - Cột: ID, Tên sân, Địa điểm, Loại, Giá/giờ, Trạng thái
 * 
 * SERVICESFORM:
 * - Xem danh sách dịch vụ
 * - Xem chi tiết dịch vụ
 * - Thêm dịch vụ vào giỏ hàng
 * - Cột: ID, Tên dịch vụ, Mô tả, Giá, Số lượng
 * 
 * CARTFORM:
 * - Xem giỏ hàng
 * - Xóa mục khỏi giỏ
 * - Cập nhật số lượng
 * - Xóa toàn bộ giỏ
 * - Tiến hành thanh toán
 * - Hiển thị tổng giá
 * - Cột: ID, Tên, Số lượng, Đơn giá, Thành tiền
 * 
 * CHATBOTFORM:
 * - Trò chuyện với AI Chatbot
 * - Hỗ trợ tìm kiếm thông tin, hướng dẫn sử dụng
 * - Có các câu trả lời mẫu cho các câu hỏi phổ biến
 * - Lưu lịch sử trò chuyện
 * - Xóa trò chuyện
 * 
 * ═══════════════════════════════════════════════════════════════
 * CHỈNH SỬA GIAO DIỆN VỀ SAU
 * ═══════════════════════════════════════════════════════════════
 * 
 * Để chỉnh sửa giao diện của các form:
 * 1. Mở file .cs (ví dụ: BookingsForm.cs)
 * 2. Chuột phải -> View Designer
 * 3. Kéo thả các control, chỉnh sửa tùy ý
 * 4. Thay đổi thuộc tính (Properties) trong panel bên phải
 * 5. Lưu file (Ctrl+S)
 * 
 * Tất cả các điều chỉnh sẽ tự động cập nhật trong file .Designer.cs
 * 
 * ═══════════════════════════════════════════════════════════════
 * GHI CHÚ QUAN TRỌNG
 * ═══════════════════════════════════════════════════════════════
 * 
 * - Tất cả form sử dụng async/await, cần có TaskScheduler.Current.Post() hoặc BeginInvoke()
 * - Các service cần được inject từ MainForm hoặc Program.cs
 * - Cần kết nối database để service hoạt động
 * - Các button "TODO: Call [service]" cần được implement thêm
 * - CurrentUser.UserId được sử dụng để lấy ID người dùng đang đăng nhập
 */
