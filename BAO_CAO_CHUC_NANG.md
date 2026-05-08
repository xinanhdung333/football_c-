# Tổng Hợp Chức Năng Project Football Booking System

## 1. Tổng quan hệ thống

Project là hệ thống đặt sân bóng viết bằng C# .NET, sử dụng kiến trúc nhiều lớp:

- `FootballBooking.Models`: định nghĩa entity và enum trạng thái.
- `FootballBooking.DAL`: truy cập dữ liệu SQL Server bằng repository.
- `FootballBooking.BLL`: xử lý nghiệp vụ, kiểm tra dữ liệu, checkout, thống kê.
- `FootballBooking.Web`: giao diện web bằng Razor Pages và REST API.
- `FootballBooking.UI`: ứng dụng WinForms client.
- Database chính: `football_booking` trên SQL Server.

## 2. Chức năng người dùng

- Đăng ký tài khoản mới.
- Đăng nhập, đăng xuất bằng session.
- Xem danh sách sân bóng.
- Xem thông tin sân: tên sân, địa điểm, giá theo giờ, hình ảnh, trạng thái.
- Đặt sân theo ngày, giờ bắt đầu, giờ kết thúc và ghi chú.
- Tự động kiểm tra trùng lịch đặt sân trước khi tạo booking.
- Tự động tính tiền thuê sân dựa trên thời lượng thuê và giá theo giờ.
- Tự động áp dụng giá tăng theo khung giờ nếu sân có cấu hình giờ cao điểm.
- Xem lịch sử đặt sân cá nhân.
- Xem chi tiết đơn đặt sân.
- Xuất hóa đơn PDF cho booking.
- Xem danh sách dịch vụ đi kèm.
- Xem chi tiết dịch vụ.
- Thêm dịch vụ vào giỏ hàng.
- Cập nhật số lượng dịch vụ trong giỏ hàng.
- Xóa dịch vụ khỏi giỏ hàng.
- Tính tổng tiền giỏ hàng.
- Thanh toán dịch vụ trong giỏ hàng.
- Tạo đơn hàng dịch vụ và thông tin thanh toán.
- Xem lịch sử đơn hàng dịch vụ.
- Xem chi tiết đơn hàng.
- Xuất hóa đơn PDF cho đơn dịch vụ.
- Gửi feedback/đánh giá từ 1 đến 5 sao cho booking hoặc dịch vụ.
- Xem phản hồi của admin đối với feedback.
- Xem và chỉnh sửa thông tin cá nhân.

## 3. Chức năng đặt sân

- Hiển thị danh sách sân đang có trong hệ thống.
- Cho phép người dùng chọn sân, ngày đặt, giờ bắt đầu, giờ kết thúc.
- Kiểm tra thời gian thuê hợp lệ, giờ kết thúc phải lớn hơn giờ bắt đầu.
- Kiểm tra sân có bị trùng lịch hay không.
- Nếu không trùng lịch, hệ thống tạo booking với trạng thái mặc định `Pending`.
- Tính tổng tiền booking theo công thức:

```text
Tổng tiền = Số giờ thường x Giá thường + Số giờ tăng giá x Giá theo khung giờ tăng giá
```

- Booking có các trạng thái: `Pending`, `Confirmed`, `InProgress`, `Completed`, `Cancelled`, `Expired`.
- Nếu thời gian đặt sân chỉ nằm một phần trong khung giờ tăng giá, hệ thống tách riêng phần giờ thường và phần giờ tăng giá để tính tiền.

## 4. Chức năng dịch vụ và giỏ hàng

- Hiển thị danh sách dịch vụ như nước uống, thuê đồ, phụ kiện.
- Mỗi dịch vụ có tên, mô tả, giá, số lượng tồn kho, hình ảnh và trạng thái.
- Người dùng có thể thêm dịch vụ vào giỏ hàng.
- Nếu dịch vụ đã tồn tại trong giỏ, hệ thống cộng dồn số lượng.
- Kiểm tra tồn kho trước khi thêm hoặc cập nhật số lượng.
- Cho phép cập nhật số lượng từng sản phẩm.
- Cho phép xóa sản phẩm khỏi giỏ hàng.
- Cho phép xóa toàn bộ giỏ sau khi thanh toán thành công.
- Tính tổng tiền dựa trên số lượng và đơn giá từng dịch vụ.

## 5. Chức năng checkout và thanh toán

- Kiểm tra người dùng đã đăng nhập trước khi thanh toán.
- Kiểm tra giỏ hàng không được rỗng.
- Bắt buộc chọn phương thức thanh toán.
- Tạo order từ các item trong giỏ hàng.
- Tạo payment tương ứng với order.
- Trừ tồn kho dịch vụ sau khi tạo đơn.
- Xử lý order, order item, payment và tồn kho trong cùng một transaction để tránh lỗi dữ liệu.
- Hỗ trợ trạng thái thanh toán: `Pending`, `Success`, `Failed`, `Refunded`.
- Hỗ trợ trạng thái đơn hàng: `Pending`, `Confirmed`, `Processing`, `InProgress`, `Completed`, `Cancelled`.

## 6. Chức năng quản trị

Admin/Boss có các chức năng quản lý sau:

### Quản lý người dùng

- Xem danh sách người dùng.
- Thêm người dùng mới.
- Cập nhật thông tin người dùng.
- Phân quyền người dùng: `User`, `Admin`, `Boss`.
- Xóa người dùng.
- Cập nhật mật khẩu.

### Quản lý sân bóng

- Xem danh sách sân.
- Thêm sân mới.
- Cập nhật tên sân, địa điểm, giá theo giờ, trạng thái.
- Cấu hình khung giờ tăng giá cho từng sân gồm giờ bắt đầu, giờ kết thúc và giá tăng theo giờ.
- Upload ảnh sân bóng.
- Xóa sân.
- Cập nhật giá sân.

### Quản lý dịch vụ

- Xem danh sách dịch vụ.
- Thêm dịch vụ mới.
- Cập nhật tên, mô tả, giá, tồn kho, trạng thái.
- Upload ảnh dịch vụ.
- Xóa dịch vụ.
- Cập nhật số lượng tồn kho.

### Quản lý booking

- Xem toàn bộ lịch đặt sân.
- Xem thông tin khách hàng, sân, ngày giờ đặt, tổng tiền, trạng thái.
- Cập nhật trạng thái booking.
- Xóa booking.
- Xuất danh sách booking ra file CSV.

### Quản lý đơn hàng dịch vụ

- Xem danh sách đơn hàng.
- Xem khách hàng, email, tổng tiền, phương thức thanh toán, trạng thái.
- Cập nhật trạng thái đơn hàng.
- Xuất danh sách đơn hàng ra file CSV.

### Quản lý hóa đơn

- Xem danh sách hóa đơn đặt sân.
- Xuất danh sách hóa đơn booking ra CSV.
- Xuất hóa đơn PDF cho booking và order.

### Quản lý feedback

- Xem danh sách feedback của người dùng.
- Xem rating, nội dung, booking/dịch vụ liên quan.
- Xóa feedback.
- Admin/Boss phản hồi lại feedback của người dùng.

## 7. Dashboard và thống kê

Hệ thống có module thống kê cho admin:

- Tổng số booking trong khoảng thời gian.
- Tổng số order dịch vụ.
- Doanh thu từ đặt sân.
- Doanh thu từ dịch vụ.
- Tổng doanh thu.
- Top 5 sân được đặt nhiều nhất.
- Top 5 dịch vụ bán chạy nhất.
- Thống kê số lượng booking theo trạng thái.
- Thống kê số lượng order theo trạng thái.
- Thống kê doanh thu theo tháng.
- Lọc thống kê theo khoảng ngày.
- Xuất báo cáo thống kê ra CSV.

## 8. REST API

Project có các API phục vụ web/mobile hoặc tích hợp bên ngoài:

### Public API

- `GET /api/public/fields`
  - Lấy danh sách sân.
  - Có thể lọc theo địa điểm và trạng thái.

- `GET /api/public/services`
  - Lấy danh sách dịch vụ.
  - Có thể tìm theo từ khóa.
  - Có thể lọc dịch vụ còn hàng.

- `GET /api/public/availability`
  - Kiểm tra sân có trống trong khung giờ cụ thể hay không.

### Cart API

- `POST /api/cart/add`
  - Thêm dịch vụ vào giỏ hàng.

- `POST /api/cart/update`
  - Cập nhật số lượng item trong giỏ hàng.

- `POST /api/cart/remove`
  - Xóa item khỏi giỏ hàng.

### Admin Analytics API

- `GET /api/admin/analytics/summary`
  - Lấy thống kê tổng quan theo ngày.

- `GET /api/admin/analytics/monthly`
  - Lấy doanh thu theo tháng.

### Chatbot API

- `POST /api/chatbot/ask`
  - Người dùng gửi câu hỏi.
  - Chatbot trả lời dựa trên dữ liệu JSON trong `App_Data/chatbot_knowledge.json`.

## 9. Chatbot hỗ trợ

- Chatbot chỉ cho phép người dùng đã đăng nhập sử dụng.
- Dữ liệu chatbot được lưu trong file JSON.
- Chatbot tự đọc lại file khi dữ liệu thay đổi.
- Cơ chế trả lời dựa trên keyword matching.
- Có câu trả lời mặc định nếu không tìm thấy nội dung phù hợp.
- Có gợi ý câu hỏi liên quan.

## 10. Ứng dụng WinForms

Ngoài web, project còn có client WinForms với các chức năng:

- Đăng nhập.
- Đăng ký tài khoản.
- Xem danh sách sân.
- Đặt sân.
- Xem danh sách dịch vụ.
- Thêm dịch vụ vào giỏ hàng.
- Xem giỏ hàng.
- Thanh toán dịch vụ.
- Đăng xuất.

Giao diện admin WinForms:

- Quản lý sân.
- Cập nhật giá sân.
- Quản lý dịch vụ.
- Cập nhật tồn kho dịch vụ.

## 11. Phân quyền

Hệ thống có 3 vai trò:

- `User`: người dùng thường, có thể đặt sân, mua dịch vụ, gửi feedback, xem lịch sử.
- `Admin`: quản lý hệ thống, người dùng, sân, dịch vụ, booking, order, feedback, thống kê.
- `Boss`: quyền tương tự admin, dùng cho vai trò quản lý cấp cao.

## 12. Các bảng dữ liệu chính

- `users`: lưu thông tin người dùng.
- `fields`: lưu thông tin sân bóng.
- `services`: lưu dịch vụ đi kèm.
- `bookings`: lưu đơn đặt sân.
- `booking_services`: lưu dịch vụ gắn với booking nếu có.
- `cart`: giỏ hàng của người dùng.
- `cart_items`: chi tiết giỏ hàng.
- `orders`: đơn hàng dịch vụ.
- `order_items`: chi tiết đơn hàng.
- `payments`: thông tin thanh toán.
- `feedbacks`: phản hồi/đánh giá.
- `user_spending`: thống kê chi tiêu người dùng.

## 13. Chức năng kỹ thuật nổi bật

- Kiến trúc nhiều lớp rõ ràng: Models, DAL, BLL, Web/UI.
- Sử dụng Repository Pattern để truy cập SQL Server.
- Sử dụng Session để lưu người dùng đăng nhập.
- Tự seed tài khoản admin mặc định khi chạy web.
- Upload và lưu ảnh sân/dịch vụ vào `wwwroot/images`.
- Xuất PDF hóa đơn bằng thư viện iText.
- Xuất báo cáo CSV cho admin.
- Checkout dùng transaction để đảm bảo dữ liệu order, payment và tồn kho đồng bộ.
- Có REST API để mở rộng cho mobile hoặc tích hợp hệ thống khác.

## 14. Tài khoản admin mặc định

Web tự tạo tài khoản admin nếu chưa tồn tại:

```text
Email: admin@footballbooking.local
Password: Admin@123
```

## 15. Hướng phát triển tiếp theo

Một số chức năng có thể phát triển thêm:

- Tích hợp thanh toán MoMo thực tế.
- Thêm callback/verify thanh toán online.
- Gửi thông báo qua email hoặc SMS.
- Hỗ trợ đa ngôn ngữ Việt/Anh.
- Cải thiện bảo mật mật khẩu bằng hash password.
- Thêm phân quyền chi tiết hơn cho Admin và Boss.
