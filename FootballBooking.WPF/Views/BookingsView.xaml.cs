using System.Windows.Controls;
using FootballBooking.WPF.ViewModels;

namespace FootballBooking.WPF.Views;

public partial class BookingsView : UserControl
{
    public BookingsView(BookingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
