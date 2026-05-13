using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace FootballBooking.WPF.Converters;

public class ImageUriConverter : IValueConverter
{
    public string BasePath { get; set; } = "";

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string? fileName = value as string;
        
        // If filename is empty, try to find ANY image in that folder
        if (string.IsNullOrEmpty(fileName))
        {
            try
            {
                string searchDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BasePath);
                if (Directory.Exists(searchDir))
                {
                    var firstFile = Directory.EnumerateFiles(searchDir, "*.*")
                        .FirstOrDefault(f => f.EndsWith(".jpg") || f.EndsWith(".png") || f.EndsWith(".webp"));
                    if (firstFile != null) fileName = Path.GetFileName(firstFile);
                }
            }
            catch { }
        }

        if (string.IsNullOrEmpty(fileName)) return null;

        fileName = fileName.Trim().Replace('/', '\\').TrimStart('\\');

        // Try absolute path first
        string fullPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
        
        if (!File.Exists(fullPath))
        {
            // Try with BasePath
            fullPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BasePath, Path.GetFileName(fileName)));
        }

        if (File.Exists(fullPath))
        {
            try
            {
                return new Uri(fullPath, UriKind.Absolute);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi nạp ảnh: {fullPath} - {ex.Message}");
                return null;
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine($"KHÔNG TÌM THẤY FILE: {fullPath}");
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
