using System.Windows.Controls;
using FootballBooking.WPF.ViewModels;

namespace FootballBooking.WPF.Views;

public partial class CartView : UserControl
{
    public CartView(CartViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
