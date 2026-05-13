using System.Windows;
using FootballBooking.WPF.ViewModels;

namespace FootballBooking.WPF.Views;

public partial class AdminView : Window
{
    public AdminView(AdminViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
