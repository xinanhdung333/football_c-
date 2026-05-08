# Football Booking System

Hệ thống đặt sân bóng với kiến trúc nhiều lớp (`Models` - `DAL` - `BLL` - `Web/UI`) dùng C# .NET và SQL Server.

## Kiến trúc dự án

- `FootballBooking.Models`: Entity, enum trạng thái
- `FootballBooking.DAL`: Truy cập dữ liệu SQL Server (repository)
- `FootballBooking.BLL`: Logic nghiệp vụ, validation, checkout, analytics
- `FootballBooking.Web`: Razor Pages + REST API cho web/mobile
- `FootballBooking.UI`: WinForms client

## Tính năng đã hoàn thành

### Người dùng
- Đăng ký/đăng nhập
- Đặt sân (kiểm tra trùng lịch)
- Mua dịch vụ kèm theo (giỏ hàng, checkout)
- Theo dõi lịch sử booking và đơn dịch vụ
- Gửi feedback

### Quản trị
- Quản lý người dùng, sân, dịch vụ, booking, feedback
- Quản lý đơn hàng và hóa đơn
- Dashboard thống kê có lọc thời gian
- Xuất báo cáo CSV

### Nâng cao
- REST API public cho danh sách sân/dịch vụ + kiểm tra khả dụng
- REST API analytics cho admin
- Thống kê top sân, top dịch vụ, doanh thu theo tháng
- Checkout transaction-safe (order + order items + payment + tồn kho trong cùng transaction)
- Đồng bộ UI Web theo design system Apple-inspired trong `FootballBooking.Web/AI-gents/design/DESIGN.md`

## Database

Dùng database `football_booking` (SQL Server) với các bảng chính:

- `users`
- `fields`
- `services`
- `bookings`
- `booking_services`
- `cart`, `cart_items`
- `orders`, `order_items`
- `payments`
- `feedbacks`
- `user_spending`

### Lưu ý Unicode tiếng Việt

Nếu bạn gặp dữ liệu hiển thị kiểu `m? dinh`, `l?c chay`, chạy migration sau để chuyển cột text sang `NVARCHAR`:

```sql
database/2026-04-24_unicode_columns.sql
```

Dữ liệu đã bị lỗi trước đó không thể tự phục hồi, cần cập nhật lại bản ghi sau khi migrate.

### Tính năng phản hồi của admin cho feedback

Để bật chức năng admin/boss phản hồi lại feedback của user, chạy thêm migration:

```sql
database/2026-04-24_feedback_reply.sql
```

## REST API mới

### Public
- `GET /api/public/fields?location=&status=`
- `GET /api/public/services?query=&inStockOnly=`
- `GET /api/public/availability?fieldId=&date=&start=&end=`

### Admin (cần session admin/boss)
- `GET /api/admin/analytics/summary?fromDate=&toDate=`
- `GET /api/admin/analytics/monthly?months=`

### Chatbot (dữ liệu JSON)
- `POST /api/chatbot/ask` với body:
```json
{ "question": "Làm sao để đặt sân?" }
```
- Nguồn tri thức: `FootballBooking.Web/App_Data/chatbot_knowledge.json`
- Có thể cập nhật câu trả lời bằng cách sửa JSON, bot tự reload theo thời gian sửa file.

## Cách chạy

### Yêu cầu
- .NET 10 SDK
- SQL Server `SQLEXPRESS`
- Đã tạo DB `football_booking` theo script SQL

### Build
```bash
cd c:\c#football_booking
dotnet build FootballBooking.slnx
```

### Chạy Web
```bash
cd FootballBooking.Web
dotnet run
```

### Chạy WinForms
```bash
cd FootballBooking.UI
dotnet run
```

## Tài khoản mặc định Web

Web tự seed tài khoản admin khi khởi động:

- Email: `admin@footballbooking.local`
- Password: `Admin@123`

## Roadmap tiếp theo

- [ ] Tích hợp thanh toán MoMo thực tế (callback/verify)
- [ ] Thông báo email/SMS
- [ ] Đa ngôn ngữ VN/EN
