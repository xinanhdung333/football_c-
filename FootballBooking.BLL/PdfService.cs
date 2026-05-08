using FootballBooking.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using System.Globalization;

namespace FootballBooking.BLL;

public class PdfService
{
    public byte[] GenerateBookingInvoice(Booking booking, Field field, User user)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new PdfWriter(memoryStream);
        using var pdf = new PdfDocument(writer);
        using var document = new Document(pdf);

        // Font setup
        var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

        // Header
        var header = new Paragraph("HÓA ĐƠN ĐẶT SÂN BÓNG")
            .SetFont(boldFont)
            .SetFontSize(20)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(20);
        document.Add(header);

        // Company info
        var companyInfo = new Paragraph("Football Booking System\nĐịa chỉ: 123 Đường ABC, TP.HCM\nHotline: 0123 456 789")
            .SetFont(font)
            .SetFontSize(10)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(20);
        document.Add(companyInfo);

        // Invoice details
        var invoiceTable = new Table(UnitValue.CreatePercentArray(new float[] { 2, 3 })).SetWidth(UnitValue.CreatePercentValue(100));

        // Invoice number and date
        invoiceTable.AddCell(new Cell().Add(new Paragraph("Số hóa đơn:").SetFont(boldFont)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
        invoiceTable.AddCell(new Cell().Add(new Paragraph($"#{booking.Id:D6}").SetFont(font)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

        invoiceTable.AddCell(new Cell().Add(new Paragraph("Ngày xuất:").SetFont(boldFont)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
        invoiceTable.AddCell(new Cell().Add(new Paragraph(DateTime.Now.ToString("dd/MM/yyyy")).SetFont(font)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

        document.Add(invoiceTable);
        document.Add(new Paragraph("\n"));

        // Customer info
        var customerTable = new Table(UnitValue.CreatePercentArray(new float[] { 2, 3 })).SetWidth(UnitValue.CreatePercentValue(100));
        customerTable.AddCell(new Cell().Add(new Paragraph("Khách hàng:").SetFont(boldFont)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
        customerTable.AddCell(new Cell().Add(new Paragraph(user.Name).SetFont(font)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

        customerTable.AddCell(new Cell().Add(new Paragraph("Email:").SetFont(boldFont)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
        customerTable.AddCell(new Cell().Add(new Paragraph(user.Email).SetFont(font)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

        customerTable.AddCell(new Cell().Add(new Paragraph("SĐT:").SetFont(boldFont)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
        customerTable.AddCell(new Cell().Add(new Paragraph(user.Phone).SetFont(font)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

        document.Add(customerTable);
        document.Add(new Paragraph("\n"));

        // Booking details
        var bookingTable = new Table(UnitValue.CreatePercentArray(new float[] { 2, 3 })).SetWidth(UnitValue.CreatePercentValue(100));
        bookingTable.AddCell(new Cell().Add(new Paragraph("Sân bóng:").SetFont(boldFont)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
        bookingTable.AddCell(new Cell().Add(new Paragraph(field.Name).SetFont(font)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

        bookingTable.AddCell(new Cell().Add(new Paragraph("Địa chỉ:").SetFont(boldFont)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
        bookingTable.AddCell(new Cell().Add(new Paragraph(field.Location).SetFont(font)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

        bookingTable.AddCell(new Cell().Add(new Paragraph("Ngày đặt:").SetFont(boldFont)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
        bookingTable.AddCell(new Cell().Add(new Paragraph(booking.BookingDate.ToString("dd/MM/yyyy")).SetFont(font)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

        bookingTable.AddCell(new Cell().Add(new Paragraph("Thời gian:").SetFont(boldFont)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
        bookingTable.AddCell(new Cell().Add(new Paragraph($"{booking.StartTime:hh\\:mm} - {booking.EndTime:hh\\:mm} ({booking.DurationHours}h)").SetFont(font)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

        document.Add(bookingTable);
        document.Add(new Paragraph("\n"));

        // Services table
        var servicesTable = new Table(UnitValue.CreatePercentArray(new float[] { 4, 1, 2, 2 })).SetWidth(UnitValue.CreatePercentValue(100));
        servicesTable.AddHeaderCell(new Cell().Add(new Paragraph("Dịch vụ").SetFont(boldFont)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY));
        servicesTable.AddHeaderCell(new Cell().Add(new Paragraph("SL").SetFont(boldFont)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY));
        servicesTable.AddHeaderCell(new Cell().Add(new Paragraph("Đơn giá").SetFont(boldFont)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY));
        servicesTable.AddHeaderCell(new Cell().Add(new Paragraph("Thành tiền").SetFont(boldFont)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY));

        // Field booking
        servicesTable.AddCell(new Cell().Add(new Paragraph($"Đặt sân {field.Name}").SetFont(font)));
        servicesTable.AddCell(new Cell().Add(new Paragraph("1").SetFont(font)).SetTextAlignment(TextAlignment.CENTER));
        servicesTable.AddCell(new Cell().Add(new Paragraph(field.PricePerHour.ToString("C", new CultureInfo("vi-VN"))).SetFont(font)).SetTextAlignment(TextAlignment.RIGHT));
        servicesTable.AddCell(new Cell().Add(new Paragraph(booking.TotalPrice.ToString("C", new CultureInfo("vi-VN"))).SetFont(font)).SetTextAlignment(TextAlignment.RIGHT));

        document.Add(servicesTable);
        document.Add(new Paragraph("\n"));

        // Total
        var totalTable = new Table(UnitValue.CreatePercentArray(new float[] { 3, 2 })).SetWidth(UnitValue.CreatePercentValue(100));
        totalTable.AddCell(new Cell().Add(new Paragraph("Tổng cộng:").SetFont(boldFont).SetFontSize(14)).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT));
        totalTable.AddCell(new Cell().Add(new Paragraph(booking.TotalPrice.ToString("C", new CultureInfo("vi-VN"))).SetFont(boldFont).SetFontSize(14)).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT));

        document.Add(totalTable);

        // Footer
        document.Add(new Paragraph("\n"));
        var footer = new Paragraph("Cảm ơn quý khách đã sử dụng dịch vụ!\nHẹn gặp lại quý khách lần sau.")
            .SetFont(font)
            .SetFontSize(10)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginTop(30);
        document.Add(footer);

        document.Close();
        return memoryStream.ToArray();
    }

    public byte[] GenerateOrderInvoice(Order order, IEnumerable<OrderItem> orderItems, User user, IEnumerable<Service> services)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new PdfWriter(memoryStream);
        using var pdf = new PdfDocument(writer);
        using var document = new Document(pdf);

        // Font setup
        var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

        // Header
        var header = new Paragraph("HÓA ĐƠN DỊCH VỤ")
            .SetFont(boldFont)
            .SetFontSize(20)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(20);
        document.Add(header);

        // Company info
        var companyInfo = new Paragraph("Football Booking System\nĐịa chỉ: 123 Đường ABC, TP.HCM\nHotline: 0123 456 789")
            .SetFont(font)
            .SetFontSize(10)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(20);
        document.Add(companyInfo);

        // Invoice details
        var invoiceTable = new Table(UnitValue.CreatePercentArray(new float[] { 2, 3 })).SetWidth(UnitValue.CreatePercentValue(100));

        invoiceTable.AddCell(new Cell().Add(new Paragraph("Số hóa đơn:").SetFont(boldFont)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
        invoiceTable.AddCell(new Cell().Add(new Paragraph($"#{order.Id:D6}").SetFont(font)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

        invoiceTable.AddCell(new Cell().Add(new Paragraph("Ngày xuất:").SetFont(boldFont)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
        invoiceTable.AddCell(new Cell().Add(new Paragraph(DateTime.Now.ToString("dd/MM/yyyy")).SetFont(font)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

        document.Add(invoiceTable);
        document.Add(new Paragraph("\n"));

        // Customer info
        var customerTable = new Table(UnitValue.CreatePercentArray(new float[] { 2, 3 })).SetWidth(UnitValue.CreatePercentValue(100));
        customerTable.AddCell(new Cell().Add(new Paragraph("Khách hàng:").SetFont(boldFont)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
        customerTable.AddCell(new Cell().Add(new Paragraph(user.Name).SetFont(font)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

        customerTable.AddCell(new Cell().Add(new Paragraph("Email:").SetFont(boldFont)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
        customerTable.AddCell(new Cell().Add(new Paragraph(user.Email).SetFont(font)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

        customerTable.AddCell(new Cell().Add(new Paragraph("SĐT:").SetFont(boldFont)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
        customerTable.AddCell(new Cell().Add(new Paragraph(user.Phone).SetFont(font)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

        document.Add(customerTable);
        document.Add(new Paragraph("\n"));

        // Services table
        var servicesTable = new Table(UnitValue.CreatePercentArray(new float[] { 4, 1, 2, 2 })).SetWidth(UnitValue.CreatePercentValue(100));
        servicesTable.AddHeaderCell(new Cell().Add(new Paragraph("Dịch vụ").SetFont(boldFont)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY));
        servicesTable.AddHeaderCell(new Cell().Add(new Paragraph("SL").SetFont(boldFont)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY));
        servicesTable.AddHeaderCell(new Cell().Add(new Paragraph("Đơn giá").SetFont(boldFont)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY));
        servicesTable.AddHeaderCell(new Cell().Add(new Paragraph("Thành tiền").SetFont(boldFont)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY));

        foreach (var item in orderItems)
        {
            var service = services.FirstOrDefault(s => s.Id == item.ServiceId);
            if (service != null)
            {
                servicesTable.AddCell(new Cell().Add(new Paragraph(service.Name).SetFont(font)));
                servicesTable.AddCell(new Cell().Add(new Paragraph(item.Quantity.ToString()).SetFont(font)).SetTextAlignment(TextAlignment.CENTER));
                servicesTable.AddCell(new Cell().Add(new Paragraph(item.Price.ToString("C", new CultureInfo("vi-VN"))).SetFont(font)).SetTextAlignment(TextAlignment.RIGHT));
                servicesTable.AddCell(new Cell().Add(new Paragraph((item.Quantity * item.Price).ToString("C", new CultureInfo("vi-VN"))).SetFont(font)).SetTextAlignment(TextAlignment.RIGHT));
            }
        }

        document.Add(servicesTable);
        document.Add(new Paragraph("\n"));

        // Total
        var totalTable = new Table(UnitValue.CreatePercentArray(new float[] { 3, 2 })).SetWidth(UnitValue.CreatePercentValue(100));
        totalTable.AddCell(new Cell().Add(new Paragraph("Tổng cộng:").SetFont(boldFont).SetFontSize(14)).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT));
        totalTable.AddCell(new Cell().Add(new Paragraph(order.TotalAmount.ToString("C", new CultureInfo("vi-VN"))).SetFont(boldFont).SetFontSize(14)).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT));

        document.Add(totalTable);

        // Footer
        document.Add(new Paragraph("\n"));
        var footer = new Paragraph("Cảm ơn quý khách đã sử dụng dịch vụ!\nHẹn gặp lại quý khách lần sau.")
            .SetFont(font)
            .SetFontSize(10)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginTop(30);
        document.Add(footer);

        document.Close();
        return memoryStream.ToArray();
    }
}